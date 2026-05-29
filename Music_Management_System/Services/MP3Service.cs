namespace Music_Management_System.Services;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Music_Management_System.Helpers;

public class Mp3Service : IMP3Service  {
    private readonly Cloudinary _cloudinary;

    public Mp3Service(IOptions<CloudinarySettings> config) {
        var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<VideoUploadResult> AddVideoAsync(IFormFile file) {
        var uploadResult = new VideoUploadResult();
        if (file.Length > 0) {
            using var stream = file.OpenReadStream();
            var uploadParams = new VideoUploadParams {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill") // Tự động resize
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        return uploadResult;
    }
}