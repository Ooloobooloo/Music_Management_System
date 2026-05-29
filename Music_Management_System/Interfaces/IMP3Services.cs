using CloudinaryDotNet.Actions;


public interface IMP3Service {
    Task<VideoUploadResult> AddVideoAsync(IFormFile file);
}