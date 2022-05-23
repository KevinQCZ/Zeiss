using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Reflection;
using ZeissMachineStreamCore.Config;
using ZeissMachineStreamCore.Interfaces;
using ZeissMachineStreamCore.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
var configBuilder = new ConfigurationBuilder();

string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Environment.SetEnvironmentVariable("LOGDIR", environment.IsDevelopment() ? environment.ContentRootPath + "/bin/Debug/net6.0" : environment.ContentRootPath);
Environment.SetEnvironmentVariable("APPNAME", Assembly.GetExecutingAssembly().GetName().Name);
configBuilder.SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
if (!string.IsNullOrEmpty(envName))
    configBuilder.AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Zeiss Machine Stream",
        Description = "v1"
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder
            .WithOrigins(configuration["AllowedHosts"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

//register config
builder.Services.Configure<WebSocketConfiguration>(configuration.GetSection("WebSocket"));
builder.Services.Configure<MongoDBConfiguration>(configuration.GetSection("MongoDB"));

//register interface
builder.Services.AddSingleton(typeof(IMongoClient), MongoSettings.GetMongoClient(configuration.GetSection("MongoDB:ClientSettings").Get<ClientSettingsConfiguration>()));
builder.Services.AddScoped<IMongo, MongoService>();
builder.Services.AddScoped<ISaveData, SaveDataService>();
builder.Services.AddScoped<IGetMachineData, GetMachineDataService>();
//add the service used and consumed by the background task
builder.Services.AddScoped<IWebSocket, WebSocketService>();
builder.Services.AddHostedService<WebSocketHostingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{ 
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zeiss Machine Stream v1");
    });
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Home}/{action=Index}/{id?}");

app.Run();
