using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartE_Commerce_Business.Contracts
{
    /// <summary>
    /// Service interface for uploading images to Cloudinary
    /// </summary>
    public interface ICloudinaryService
    {
        /// <summary>
        /// Uploads an image from a local file path to Cloudinary
        /// </summary>
        /// <param name="localFilePath">The local file path of the image to upload</param>
        /// <returns>The Cloudinary URL of the uploaded image, or null if upload fails</returns>
        Task<string?> UploadImageAsync(string localFilePath);

        /// <summary>
        /// Uploads multiple images to Cloudinary
        /// </summary>
        /// <param name="localFilePaths">List of local file paths to upload</param>
        /// <returns>List of Cloudinary URLs for successfully uploaded images</returns>
        Task<List<string>> UploadImagesAsync(List<string> localFilePaths);
    }
}
