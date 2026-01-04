using CodeEvaluator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using System.Text.Json.Serialization;
using CodeEvaluator.Application.Interfaces.Services;
using Microsoft.OpenApi.Models;
using CodeEvaluator.Application.Interfaces.Services;

using CodeEvaluator.Application.Services;
using CodeEvaluator.Judge0.Client;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Configure CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000", "http://localhost:5500", "http://127.0.0.1:5500")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllersWithViews().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
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


builder.Services.AddHostedService<CodeEvaluator.Application.Services.Judge0PollingService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddHttpClient();

// Judge0 base URL
var judge0BaseUrl =
    Environment.GetEnvironmentVariable("JUDGE0_BASEURL")
    ?? builder.Configuration["Judge0:BaseUrl"];

if (string.IsNullOrWhiteSpace(judge0BaseUrl))
{
    throw new Exception("Judge0 BaseUrl not configured. Set Judge0:BaseUrl or env var JUDGE0_BASEURL.");
}

Judge0Config.SetBaseUrl(judge0BaseUrl);
Console.WriteLine($"Judge0 BaseUrl = {judge0BaseUrl}");

builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IJudge0Service, Judge0Service>();
builder.Services.AddScoped<Judge0Client>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpClient<IMoodleAuthService, MoodleAuthService>();

builder.Services.AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
    });



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // For API attribute routing
app.MapDefaultControllerRoute(); // For MVC conventional routing

app.Run();