
using Microsoft.OpenApi.Models;
using SAW.Web.Api.StartUp;
using SAW.Web.Business;
using SAW.Web.Business.Security;
using SAW.Web.Business.Security.Services;
using SAW.Web.Business.Service;
using SAW.Web.Business.Services;
using SAW.Web.Data;
using SAW.Web.Data.Repository;
using SAW.Web.Entities.Config;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;

//Configure environment variables
ConfigConfiguration(builder);

//Configure Serilog logger
ConfigureLogger(services, builder.Logging);

//Configure services
#region 
services.AddControllers();

ConfigureAppSettings(services);
//Add data access layer
ConfigureDataAccessLayer(services);
//Add business logic service layer
ConfigureBusinessServicesLayer(services);

Authentication.ConfigureServices(services, config);
Cors.ConfigureServices(services);

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SAW.Web.Api", Version = "v1" });
});
#endregion

//Create WebApplication instance and run
#region
var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SAW.Web.Api v1"));
}

if (!env.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
Authentication.Configure(app, env);


Cors.Configure(app, env);
app.MapControllers();

app.Run();
#endregion

void ConfigConfiguration(WebApplicationBuilder builder)
{
    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
    IConfigurationBuilder root = builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);

    //the settings in the env settings will override the appsettings.json values, recursively at the key level.
    // where the key could be nested. this would allow very fine tuned control over the settings
    IConfigurationBuilder appSettings = root.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    string jsonFileName = $"appsettings.{builder.Environment.EnvironmentName}.json";
    IConfigurationBuilder envSettings = appSettings
        .AddJsonFile(jsonFileName, optional: true, reloadOnChange: true);
}

void ConfigureLogger(IServiceCollection services, ILoggingBuilder logger)
{
    var loggerConfiguration = new LoggerConfiguration()
    .ReadFrom.Configuration(config.GetSection("Serilog"))
    .CreateLogger();

    services.AddLogging(cfg => cfg.AddSerilog(loggerConfiguration));
}

void ConfigureAppSettings(IServiceCollection services)
{
    services.Configure<AppSettings>(config.GetSection("AppSettings"));
    services.Configure<SecurityConfig>(config.GetSection("SecurityConfig"));
    services.Configure<JsonWebTokenConfig>(config.GetSection("JsonWebTokenConfig"));
    services.Configure<AzureEmailSettings>(config.GetSection("AzureEmailSettings"));
}

void ConfigureDataAccessLayer(IServiceCollection services)
{
    services.AddSingleton<ITokenDataRepository, TokenDataRepository>();
    services.AddSingleton<IUsersDataRepository, UsersDataRepository>();
    services.AddSingleton<IEmailDataRepository, EmailDataRepository>();
}

void ConfigureBusinessServicesLayer(IServiceCollection services)
{
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddSingleton<IAuthenticationBusinessService<int>, AuthenticationBusinessService>();
    services.AddSingleton<ITokenBusinessService, TokenBusinessService>();
    services.AddSingleton<IUsersBusinessService, UsersBusinessService>();
    services.AddSingleton<IEmailerBusinessService, EmailerBusinessService>();
}

