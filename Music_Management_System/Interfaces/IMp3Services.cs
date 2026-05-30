namespace Music_Management_System.Interfaces;

using CloudinaryDotNet.Actions;

public interface IMp3Service {
    Task<RawUploadResult> AddAudioAsync(IFormFile file);
}