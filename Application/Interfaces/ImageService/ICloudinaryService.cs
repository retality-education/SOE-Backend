using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadAvatarAsync(IFormFile file, Guid userId);
        Task<ImageUploadResult> UploadBookImageAsync(IFormFile file, Guid bookId, Guid userId);
        Task<ImageUploadResult> UploadSiteImageAsync(IFormFile file, string folder);
        Task<DeletionResult> DeleteImageAsync(string publicId);
        string GetImageUrl(string publicId, int? width = null, int? height = null);
    }
}
