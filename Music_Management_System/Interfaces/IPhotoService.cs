using CloudinaryDotNet.Actions;


public interface IPhotoService {
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
}