using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using SmartE_Commerce_Business.Contracts;

namespace SmartE_Commerce_Business.Services
{
    /// <summary>
    /// Service for uploading images to Cloudinary cloud storage
    /// </summary>
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true; // Use HTTPS URLs
        }

        /// <summary>
        /// Uploads an image from a local file path to Cloudinary
        /// </summary>
        /// <param name="localFilePath">The local file path of the image to upload</param>
        /// <returns>The Cloudinary URL of the uploaded image, or null if upload fails</returns>
        public async Task<string?> UploadImageAsync(string localFilePath)
        {
            try
            {
                // Check if file exists
                if (!File.Exists(localFilePath))
                {
                    Console.WriteLine($"File not found: {localFilePath}");
                    return null;
                }

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(localFilePath),
                    Folder = "products", // Store all product images in a 'products' folder
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = false
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    Console.WriteLine($"Cloudinary upload error: {uploadResult.Error.Message}");
                    return null;
                }

                return uploadResult.SecureUrl?.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Cloudinary upload: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Uploads multiple images to Cloudinary
        /// </summary>
        /// <param name="localFilePaths">List of local file paths to upload</param>
        /// <returns>List of Cloudinary URLs for successfully uploaded images</returns>
        public async Task<List<string>> UploadImagesAsync(List<string> localFilePaths)
        {
            var uploadedUrls = new List<string>();

            foreach (var filePath in localFilePaths)
            {
                var url = await UploadImageAsync(filePath);
                if (!string.IsNullOrEmpty(url))
                {
                    uploadedUrls.Add(url);
                }
            }

            return uploadedUrls;
        }
    }
}
