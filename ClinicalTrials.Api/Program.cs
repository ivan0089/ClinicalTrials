
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Carter;
using Serilog;
using ClinicalTrials.Api.Extension;
using ClinicalTrials.Application;
using ClinicalTrials.Application.Validations;
using ClinicalTrials.Infrastructure;
using ClinicalTrials.Infrastructure.ClinicalTrialContext;
using ClinicalTrials.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var fileConfigurationSettings = builder.Configuration
    .GetSection("FileConfigurationSettings")
    .Get<FileConfigurationSettings>();

builder.Host.UseSerilog(Log.Logger);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterInstance(fileConfigurationSettings)
                    .As<FileConfigurationSettings>()
                    .SingleInstance();

        containerBuilder.RegisterModule<ApplicationModule>();
        containerBuilder.RegisterModule<InfrastructureModule>();
    });

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClinicalTrialsDbContext>(provider => 
    provider.GetService<DatabaseContext>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.MapCarter();

app.UseHttpsRedirection();

app.Run();

