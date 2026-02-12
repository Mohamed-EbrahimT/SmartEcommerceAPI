using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartE_Commerce_Business.Contracts;

namespace SmartE_Commerce_Business.Services
{
    /// <summary>
    /// Service for sending product data to FastAPI for CLIP embedding generation
    /// </summary>
    public class EmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmbeddingService> _logger;

        public EmbeddingService(HttpClient httpClient, ILogger<EmbeddingService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Sends product data to FastAPI to generate and store image embeddings
        /// </summary>
        public async Task<bool> StoreProductEmbeddingsAsync(int productId, List<string> imageUrls)
        {
            if (imageUrls == null || !imageUrls.Any())
            {
                _logger.LogWarning("No images provided for product {ProductId}", productId);
                return false;
            }

            try
            {
                var request = new
                {
                    product_id = productId,
                    images = imageUrls
                };

                _logger.LogInformation("Sending embeddings request for product {ProductId} with {ImageCount} images", 
                    productId, imageUrls.Count);

                var response = await _httpClient.PostAsJsonAsync("/api/products/embeddings", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Successfully stored embeddings for product {ProductId}: {Result}", 
                        productId, result);
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to store embeddings for product {ProductId}. Status: {Status}, Error: {Error}", 
                        productId, response.StatusCode, error);
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error while storing embeddings for product {ProductId}. Is FastAPI running?", productId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while storing embeddings for product {ProductId}", productId);
                return false;
            }
        }
    }
}
