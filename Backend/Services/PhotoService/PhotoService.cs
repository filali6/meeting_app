using System;
using Backend.DTOs;
using Backend.Helpers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace Backend.Services.PhotoService;

public class PhotoService : IPhotoService
{
    private readonly ICloudinary _cloud;
    public PhotoService(IOptions<CloudinarySettings> cloudOptions){
        Console.WriteLine(" PhotoService instancié via la factory");

        var acc = new Account(cloudOptions.Value.CloudName,cloudOptions.Value.ApiKey,cloudOptions.Value.ApiSecret);
        _cloud=new Cloudinary(acc);
    }



    public async Task<DeletionResult> PhotoDeleteAsync(string publicId)
    {
        return await _cloud.DestroyAsync(new DeletionParams(publicId));
    }

    public async Task<ImageUploadResult> PhotoUploadAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();
        using var stream = file.OpenReadStream();
        if(file.Length>0){
            var param = new ImageUploadParams{
                File = new FileDescription(file.FileName,stream),
                Transformation = new Transformation().Height(500).Width(500)
                .Crop("fill").Gravity(Gravity.Face),
                Folder = "DatingApp"
            };
            
            uploadResult =await _cloud.UploadAsync(param);
        }
        return uploadResult;
    }
}
