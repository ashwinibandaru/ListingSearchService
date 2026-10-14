using System;
using System.Collections.Generic;
using System.Linq;
using ListingSearchModels.Models;
using ListingSearchEngine.Search.Interfaces;
using Microsoft.Extensions.Logging;

namespace ListingSearchEngine.Search
{
    /// <summary>
    /// Default implementation of <see cref="IFilterService"/> that applies simple predicate filters.
    /// </summary>
    public sealed class FilterService : IFilterService
    {
        private readonly Microsoft.Extensions.Logging.ILogger<FilterService> _logger;

        public FilterService(Microsoft.Extensions.Logging.ILogger<FilterService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Listing> ApplyFilters(IEnumerable<Listing> source, SearchParams parameters)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (parameters is null) throw new ArgumentNullException(nameof(parameters));

            try
            {
                var q = source;

                if (parameters.MinPrice.HasValue)
                {
                    q = q.Where(x => x.Price >= parameters.MinPrice.Value);
                }

                if (parameters.MaxPrice.HasValue)
                {
                    q = q.Where(x => x.Price <= parameters.MaxPrice.Value);
                }

                if (parameters.MinBedrooms > 0)
                {
                    q = q.Where(x => x.Bedrooms >= parameters.MinBedrooms);
                }

                if (!string.IsNullOrWhiteSpace(parameters.City))
                {
                    q = q.Where(x => !string.IsNullOrWhiteSpace(x.City) && x.City.IndexOf(parameters.City, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                if (!string.IsNullOrWhiteSpace(parameters.Keyword))
                {
                    q = q.Where(x => !string.IsNullOrWhiteSpace(x.Description) && x.Description.IndexOf(parameters.Keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                if (parameters.TargetBudget.HasValue)
                {
                    q = q.Where(x => x.Price >= parameters.TargetBudget.Value);
                }

                return q;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying filters. Parameters: {@Parameters}", parameters);
                throw;
            }
        }
    }
}
