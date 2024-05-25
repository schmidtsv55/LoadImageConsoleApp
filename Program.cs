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
        //services.AddLogging(x => x.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "Log.txt")));
        var AUTO_DISASSEMBLY_STR = host.Configuration["AUTO_DISASSEMBLY_STR"] ?? throw new ArgumentNullException("AUTO_DISASSEMBLY_STR");
        var IMAGE_SERVICE = host.Configuration["IMAGE_SERVICE"] ?? throw new ArgumentNullException("IMAGE_SERVICE");
        var IMAGE_SERVICE_BEARER = host.Configuration["IMAGE_SERVICE_BEARER"] ?? throw new ArgumentNullException("IMAGE_SERVICE_BEARER");
        
        services.AddHttpClient<IImageService, ImagebanService>(
            options =>{
                options.BaseAddress = new Uri(IMAGE_SERVICE);
                options.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", IMAGE_SERVICE_BEARER);
            }
        );
        services.AddDbContext<AutoDisassemblyContext>(
            options => {
                options.UseSqlServer(AUTO_DISASSEMBLY_STR);
            }
        );
        services.AddHostedService<Worker>();
    }).Build();

await host.StartAsync();

await host.StopAsync();



/*var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://api.imageban.ru/v1")
};
httpClient.DefaultRequestHeaders.Authorization =
new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "Myd4XxG84yByWx8vDIBBkXRpPXlRkK9FJmP");


IImageService imageService= new ImagebanService(httpClient);

var file = File.ReadAllBytes("../../../Downloads/3.jpg");
var href = await imageService.LoadImageAsync(file, "3.jpg");
System.Console.WriteLine(href);*/
