using Microsoft.Extensions.Options;
using Backend.Helpers;

namespace Backend.Services.PhotoService;

public class CloudinaryPhotoServiceFactory : PhotoServiceFactory
{
    private readonly IOptions<CloudinarySettings> _cloudOptions;

    public CloudinaryPhotoServiceFactory(IOptions<CloudinarySettings> cloudOptions)
    {
       
        _cloudOptions = cloudOptions;
    }

    public override IPhotoService CreatePhotoService()
    {
        Console.WriteLine("CreatePhotoService() appelé");
        Console.WriteLine("Factory method appelée !");
        return new PhotoService(_cloudOptions);
    }
}
