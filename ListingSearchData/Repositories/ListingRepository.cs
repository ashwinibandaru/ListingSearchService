using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ListingSearchModels.Models;
using Microsoft.Extensions.Logging;

namespace ListingSearchData.Repositories
{
    /// <summary>
    /// File-backed implementation that reads sample_listings.json from content root.
    /// </summary>
    public sealed class ListingRepository : IListingRepository
    {
        private readonly List<Listing> _listings = new();
        private readonly Microsoft.Extensions.Logging.ILogger<ListingRepository> _logger;

        public ListingRepository(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env, Microsoft.Extensions.Logging.ILogger<ListingRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (env is null) throw new ArgumentNullException(nameof(env));

            var path = Path.Combine(env.ContentRootPath, "sample_listings.json");
            if (!File.Exists(path))
            {
                _logger.LogWarning("sample_listings.json not found at {Path}", path);
                return;
            }

            var json = File.ReadAllText(path);
            try
            {
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var arr = JsonSerializer.Deserialize<List<Listing>>(json, opts);
                if (arr is not null)
                {
                    _listings.AddRange(arr);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse sample_listings.json at {Path}", path);
                // keep empty on parse errors for this sample
            }
        }

        public IEnumerable<Listing> GetAll() => _listings;
    }
}
