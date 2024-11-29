using System;
using Backend.DTOs;
using CloudinaryDotNet.Actions;

namespace Backend.Services.PhotoService;

public interface IPhotoService
{
    public Task<ImageUploadResult> PhotoUploadAsync(IFormFile file);
    public Task<DeletionResult> PhotoDeleteAsync(string publicId);
    public Task<PhotoDTO> AddPhotoAsync(IFormFile file,string username);

}
