namespace Backend.Services.PhotoService;

public abstract class PhotoServiceFactory
{
    public abstract IPhotoService CreatePhotoService();
}
