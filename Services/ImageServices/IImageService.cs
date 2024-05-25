namespace LoadImageConsoleApp;

public interface IImageService
{
    Task<string?> LoadImageAsync(byte[] image, string imageName);
}
