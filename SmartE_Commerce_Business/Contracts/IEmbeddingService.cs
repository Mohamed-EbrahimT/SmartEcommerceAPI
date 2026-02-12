using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartE_Commerce_Business.Contracts
{
    /// <summary>
    /// Service interface for sending product data to FastAPI for image embeddings
    /// </summary>
    public interface IEmbeddingService
    {
        /// <summary>
        /// Sends product data to FastAPI to generate and store image embeddings
        /// </summary>
        /// <param name="productId">The product ID</param>
        /// <param name="imageUrls">List of Cloudinary image URLs</param>
        /// <returns>True if embeddings were stored successfully</returns>
        Task<bool> StoreProductEmbeddingsAsync(int productId, List<string> imageUrls);
    }
}
