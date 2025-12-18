using CodeEvaluator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using Microsoft.OpenApi.Models;
using CodeEvaluator.Application.Interfaces.Services;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Code Evaluator API",
        Version = "v1",
        Description = "API for automatic evaluation of programming tasks with Moodle integration."
    });

    // Include XML comments (for controllers and DTOs)
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
   
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddSingleton(new CodeEvaluator.Judge0.Client.Judge0Client());
builder.Services.AddScoped<CodeEvaluator.Application.Interfaces.Services.IJudge0Service, CodeEvaluator.Application.Services.Judge0Service>();

builder.Services.AddHostedService<CodeEvaluator.Application.Services.Judge0PollingService>();

builder.Services.AddScoped<ISubmissionService, CodeEvaluator.Application.Services.SubmissionService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;

    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();