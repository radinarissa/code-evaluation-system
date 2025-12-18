using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using CodeEvaluator.Infrastructure.Data;
using CodeEvaluator.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using CodeEvaluator.Domain.Entities;

namespace CodeEvaluator.Application.Services{
    

public class Judge0PollingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public Judge0PollingService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var judge0Service = scope.ServiceProvider.GetRequiredService<IJudge0Service>();

            Console.WriteLine("Polling Judge0 for pending results: ");
            var pendingResults = await db.TestResults
                .Where(r => r.Status == "Pending" && r.Judge0Token != null)
                .ToListAsync(stoppingToken);
            Console.WriteLine("Found: " + pendingResults.Count);
            var all = await db.TestResults.ToListAsync(stoppingToken);
            Console.WriteLine("Total test results in DB: " + all.Count);
            

            if (pendingResults.Any())
            {
                var pendingByToken = pendingResults.ToDictionary(r=> r.Judge0Token!);
                var tokens = pendingByToken.Keys.ToList();
                var results = await judge0Service.GetSubbmisionBatchAsync(tokens);
                Console.WriteLine("should be "+pendingResults.Count());
                Console.WriteLine("found pending results: " + results.Count);
                foreach(var res in results)
                {
                    if(res==null) 
                        continue;
                    if(res.Status.Id<3) // In Queue or Processing
                        continue;
                    if(!pendingByToken.TryGetValue(res.Token, out var testResult))
                        continue;

                            testResult.Status = res.Status.Description;
                        if (res.Time != null)
                        {
                             testResult.ExecutionTime = float.Parse(res.Time);
                        }
                            testResult.MemoryUsage = res.Memory;
                            testResult.DiskUsedMb = res.FileSize/1024m;
                            testResult.Output = res.Stdout;
                            testResult.ErrorMessage = res.Stderr;
                            testResult.Judge0Token = res.Token;
                           
                           Console.WriteLine("updated test result for token: " + res.Token);
                           Console.WriteLine("Stdout: " + res.Stdout);


                        // TestResultCs also keeps instances of the TestCase and The Submission?
                }

              
            }
            // Wait before next poll
            await db.SaveChangesAsync(stoppingToken);
            await System.Threading.Tasks.Task.Delay(20000, stoppingToken); // 2 seconds
            }
        }
    }
}