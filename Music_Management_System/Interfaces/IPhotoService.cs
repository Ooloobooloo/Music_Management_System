namespace Music_Management_System.Interfaces;

using CloudinaryDotNet.Actions;

public interface IPhotoService {
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
}