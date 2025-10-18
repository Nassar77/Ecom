using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Ecom.infrastructure.Reposatries.Service;
public class ImageManagementService : IImageManagementService
{
    private readonly IFileProvider fileProvider;
    public ImageManagementService(IFileProvider fileProvider)
    {
        this.fileProvider = fileProvider;
    }
    public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
    {
        var SaveImageSrc = new List<string>();
        var imageDirectory = Path.Combine("wwwroot", "Images", src);
        if (File.Exists(imageDirectory) is not true)
        {
            Directory.CreateDirectory(imageDirectory);
        }

        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var imageName = file.FileName;
                var imagSrc = $"/Images/{src}/{imageName}";
                var root = Path.Combine(imageDirectory, imageName);
                using (FileStream stream = new FileStream(root, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                SaveImageSrc.Add(imagSrc);
            }
        }
        return SaveImageSrc;
    }

    public void DeleteImageAsync(string src)
    {
        var info = fileProvider.GetFileInfo(src);
        var root = info.PhysicalPath;
        File.Delete(root);
    }
}
