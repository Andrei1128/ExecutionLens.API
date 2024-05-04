using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExecutionLens.Application;
using ExecutionLens.Domain.Utilities;

var builder = WebApplication.CreateBuilder();

builder.Services.AddFastEndpoints()
                .SwaggerDocument();

builder.Services.AddOptions<ElasticSettings>()
                .Bind(builder.Configuration.GetSection(ElasticSettings.Key));

builder.Services.AddOptions<QuerySettings>()
                .Bind(builder.Configuration.GetSection(QuerySettings.Key));

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

builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseFastEndpoints()
   .UseSwaggerGen();

app.UseCors();

app.Run();