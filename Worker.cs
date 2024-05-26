using System.Text;
using LoadImageConsoleApp.Services.AutoDisassembly;
using LoadImageConsoleApp.Services.AutoDisassembly.Models;
using LoadImageConsoleApp.Services.AutoDisassemblyExtension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoadImageConsoleApp;

public class Worker : IHostedService
{
    IImageService _imageService;
    AutoDisassemblyContext _autoDisassemblyContext;
    ILogger<Worker> _logger;
    public Worker(
    IImageService imageService,
    AutoDisassemblyContext autoDisassemblyContext,
    ILogger<Worker> logger
)
    {
        _imageService = imageService;
        _autoDisassemblyContext = autoDisassemblyContext;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("-------------------------------------------------------------------------");
        _logger.LogInformation($"Start Programm {DateTime.Now}");
        try
        {
            await JobAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError($"Execute Error {Environment.NewLine}{ex}");
            throw;
        }
    }

    private async Task JobAsync()
    {
        var hasError = false;
        var sb = new StringBuilder();

        var files = _autoDisassemblyContext.Sys_Files.FromSql(
        $"""
            SELECT TOP 500 *
                          FROM AutoDisassembly.dbo.Sys_Files
                          WHERE FORM_NAME = 'Items'
                          AND FILE_NAME IS NOT NULL
                          AND (CHARINDEX('.jpg', LOWER(FILE_NAME)) > 0 OR CHARINDEX('.png', LOWER(FILE_NAME)) > 0)
                          AND FILE_LINK IS NULL
                          AND FILE_DATA IS NOT NULL
                          ORDER BY CREATE_DATE DESC
        """).ToArray();
        foreach (var file in files)
        {
            try
            {

                var link = await _imageService.LoadImageAsync(file.FILE_DATA, file.FILE_NAME!);
                if (string.IsNullOrWhiteSpace(link))
                {
                    throw new Exception("link is empty");
                }
                file.FILE_LINK = link;
                await _autoDisassemblyContext.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {
                sb.AppendLine(ex.ToString());
                hasError = true;
            }

        }

        if (hasError)
        {
            throw new Exception(sb.ToString());
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogError($"Stop Programm {DateTime.Now}");
        return Task.CompletedTask;
    }
}
