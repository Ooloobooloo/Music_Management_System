using Music_Management_System.Interfaces;

namespace Music_Management_System.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Music_Management_System.Helpers;

public class Mp3Service : IMp3Service {
    private readonly Cloudinary _cloudinary;

    public Mp3Service(IOptions<CloudinarySettings> config) {
        var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<RawUploadResult> AddAudioAsync(IFormFile file) {
        var uploadResult = new RawUploadResult();
        if (file.Length > 0) {
            using var stream = file.OpenReadStream();
            var uploadParams = new RawUploadParams {
                File = new FileDescription(file.FileName, stream),
                Type = "upload"  // Use Type instead of ResourceType
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        return uploadResult;
    }
}