using CloudinaryDotNet; 
using CloudinaryDotNet.Actions; 
using Microsoft.Extensions.Options; 
using Backend.Helpers;

namespace Backend.Services.PhotoService;

public class CloudinaryAdapter : IPhotoService { private readonly Cloudinary _cloudinary;
 

public CloudinaryAdapter(IOptions<CloudinarySettings> settings)
{
    var acc = new Account(
        settings.Value.CloudName,
        settings.Value.ApiKey,
        settings.Value.ApiSecret
    );
    _cloudinary = new Cloudinary(acc);
}

public async Task<ImageUploadResult> PhotoUploadAsync(IFormFile file)
{
    Console.WriteLine("📤 Appel à PhotoUploadAsync (CloudinaryAdapter)");
    if (file.Length <= 0) return null;
    await using var stream = file.OpenReadStream();

    var uploadParams = new ImageUploadParams
    {
        File = new FileDescription(file.FileName, stream),
        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
        Folder = "DatingApp"
    };

    return await _cloudinary.UploadAsync(uploadParams);
    Console.WriteLine("✅ Upload effectué via Cloudinary");
    
}

public async Task<DeletionResult> PhotoDeleteAsync(string publicId)
{
    var deleteParams = new DeletionParams(publicId);
    return await _cloudinary.DestroyAsync(deleteParams);
}
}