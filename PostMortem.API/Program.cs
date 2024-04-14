using PostMortem.Domain;
using PostMortem.Application;
using PostMortem.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddOptions<AppSettings>()
  .Bind(builder.Configuration.GetSection(AppSettings.Key));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors",
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:4200")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});

builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("cors");


app.UseAuthorization();

app.MapControllers();

app.Run();
