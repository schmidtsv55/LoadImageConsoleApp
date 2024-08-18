using System;
using System.Net.Http.Json;

namespace LoadImageConsoleApp;

public class YaDiskService:IImageService
{
    HttpClient _httpClient;
    const string _catalog = "AutoDisassembly";
    public YaDiskService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> GetLoadUrlAsync(string imageName)
    {
        var relativePath = $"/v1/disk/resources/upload?path={_catalog}/{imageName}";
        using var response = await _httpClient.GetAsync(relativePath);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        return body!["href"]?.ToString() ?? throw new ArgumentNullException("href");

    }
    public async Task<string> GetUrlImageRetryAsync(string imageName)
    {
        for (int i = 0; i < 320; i++)
        {
            Thread.Sleep(60);
            System.Console.WriteLine(i);
            var url = await GetUrlImageAsync(imageName);
            if (url == null)
            {

            }
            else
            {
                return url;
            }
        }
        throw new ArgumentException("ORIGINAL");
    }
    public async Task<string?> GetUrlImageAsync(string imageName)
    {
        var relativePath = $"/v1/disk/resources?path={_catalog}/{imageName}";
        using var response = await _httpClient.GetAsync(relativePath);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<FileData>();
        return body?.sizes?.FirstOrDefault(x => x.name == "ORIGINAL")?.url;
    }
    public async Task<string?> LoadImageAsync(byte[] image, string imageName)
    {
        var urlLoad = await GetLoadUrlAsync(imageName);
        using Stream stream = new MemoryStream(image);
        using StreamContent content = new StreamContent(stream);
        using var response = await _httpClient.PutAsync(urlLoad, content);
        response.EnsureSuccessStatusCode();
        var url = await GetUrlImageRetryAsync(imageName);
        return url;
    }
    class FileData
    {
        public Size[] sizes { get; set; }
        public class Size
        {
            public string? url { get; set; }
            public string? name { get; set; }
        }
    }
}
