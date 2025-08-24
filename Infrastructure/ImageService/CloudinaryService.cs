using Application.Interfaces.CloudinaryService;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ImageService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinaryOptions _settings;

        public CloudinaryService(IOptions<CloudinaryOptions> settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            _settings = settings.Value;

            var account = new Account(
                _settings.CloudName,
                _settings.ApiKey,
                _settings.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadAvatarAsync(IFormFile file, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(file);
            return await UploadImageAsync(file, $"avatars/{userId}").ConfigureAwait(false);
        }

        public async Task<ImageUploadResult> UploadBookImageAsync(IFormFile file, Guid bookId, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(file);
            return await UploadImageAsync(file, $"books/{userId}/{bookId}").ConfigureAwait(false);
        }

        public async Task<ImageUploadResult> UploadSiteImageAsync(IFormFile file, string folder)
        {
            ArgumentNullException.ThrowIfNull(file);
            return await UploadImageAsync(file, $"site/{folder}").ConfigureAwait(false);
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams).ConfigureAwait(false);
        }

        public string GetImageUrl(string publicId, int? width = null, int? height = null, bool isBlurred = false, int blurStrength = 1000)
        {
            var transformation = new Transformation();
            
            if (isBlurred)
            {
                transformation = transformation
                    .Effect($"blur:{blurStrength}")
                    .Quality("auto");

            }

            if (width.HasValue || height.HasValue)
            {
                transformation = transformation
                    .Width(width)
                    .Height(height)
                    .Crop("fill")
                    .Gravity("face")
                    .Quality("auto");
            }

            return _cloudinary.Api.UrlImgUp
                .Transform(transformation)
                .BuildUrl(publicId);
        }

        private async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folderPath)
        {
            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderPath,
                Transformation = new Transformation()
                    .Width(800)
                    .Height(800)
                    .Crop("limit")
                    .Quality("auto"),
                Overwrite = false
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }
    }
}
