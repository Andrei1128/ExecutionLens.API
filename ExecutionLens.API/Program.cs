using ExecutionLens.API.DOMAIN.Utilities;
using ExecutionLens.API.PERSISTENCE.Contracts;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostMortem.Persistance.Repositories;

var builder = WebApplication.CreateBuilder();

builder.Services.AddFastEndpoints()
                .SwaggerDocument();

builder.Services
  .AddOptions<ElasticSettings>()
  .Bind(builder.Configuration.GetSection(ElasticSettings.Key));

string[] corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>()!;

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<ILogRepository, LogRepository>();

var app = builder.Build();

app.UseFastEndpoints()
   .UseSwaggerGen();

app.UseCors();

app.Run();