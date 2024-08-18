// See https://aka.ms/new-console-template for more information

using LoadImageConsoleApp;
using LoadImageConsoleApp.Services.AutoDisassembly;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(hostConfig =>
    {
    })
.ConfigureServices((host, services) =>
{
    services.AddLogging(x => x.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "Log.txt")));
    var AUTO_DISASSEMBLY_STR = host.Configuration["AUTO_DISASSEMBLY_STR"] ?? "data source=.;initial catalog=AutoDisassembly;integrated security=true;Trusted_Connection=True;TrustServerCertificate=True;";
    var IMAGE_SERVICE = host.Configuration["IMAGE_SERVICE"] ?? "https://api.imageban.ru/v1";
    var IMAGE_SERVICE_BEARER = host.Configuration["IMAGE_SERVICE_BEARER"] ?? "Myd4XxG84yByWx8vDIBBkXRpPXlRkK9FJmP";
    var YANDEX_TOKEN = host.Configuration["YANDEX_TOKEN"] ?? "y0_AgAAAAAVLcBsAADLWwAAAAEOH61TAAA3fUItx4xDbI9_S2piRU6vOdx7fQ";
    var YANDEX_SERVICE = host.Configuration["YANDEX_SERVICE"] ?? "https://cloud-api.yandex.net/";

    services.AddHttpClient<IImageService, YaDiskService>(
        options =>
        {
            options.BaseAddress = new Uri(IMAGE_SERVICE);
            options.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", IMAGE_SERVICE_BEARER);
        }
    );
    services.AddDbContext<AutoDisassemblyContext>(
        options =>
        {
            options.UseSqlServer(AUTO_DISASSEMBLY_STR);
        }
    );
    services.AddHostedService<Worker>();
}).Build();

await host.StartAsync();

await host.StopAsync();




