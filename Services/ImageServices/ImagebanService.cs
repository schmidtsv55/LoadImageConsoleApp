
using System.Net;
using System.Net.Http.Json;

namespace LoadImageConsoleApp;

public class ImagebanService : IImageService
{
    HttpClient _httpClient;
    public ImagebanService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string?> LoadImageAsync(byte[] image, string imageName)
    {
        using (var multipartFormContent = new MultipartFormDataContent())
        {
            Stream stream = new MemoryStream(image);
            HttpContent fileStreamContent = new StreamContent(stream);

            multipartFormContent.Add(fileStreamContent, name: "image", imageName);
            var response = await _httpClient.PostAsync("", multipartFormContent);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadFromJsonAsync<ResponseLoadImage>();
            if (json?.error?.message != null)
            {
                if (json?.error?.message?.Contains("Exceeded the daily limit of uploaded images for your account") == true)
                {
                     throw new StopLoadException(json.error.message);
                }
                throw new Exception(json.error.message);
            }
            return json?.data?.link;
        }
    }
}

public class ResponseLoadImage
{
    public Data? data { get; set; }
    public Error? error { get; set; }
    public class Data
    {
        public string? link { get; set; }
    }
    public class Error
    {
        public string? message { get; set; }
    }
}
