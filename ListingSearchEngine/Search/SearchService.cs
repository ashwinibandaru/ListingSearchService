using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ListingSearchModels.Models;
using Microsoft.AspNetCore.Hosting;

// ReSharper disable once CheckNamespace
namespace ListingSearchEngine.Search
{
    public sealed class SearchService
    {
        private readonly ListingSearchData.Repositories.IListingRepository _repository;
        private readonly Interfaces.IFilterService _filterService;
        private readonly Interfaces.IScorer _scorer;
        private readonly Microsoft.Extensions.Logging.ILogger<SearchService> _logger;

        public SearchService(ListingSearchData.Repositories.IListingRepository repository, Interfaces.IFilterService filterService, Interfaces.IScorer scorer, Microsoft.Extensions.Logging.ILogger<SearchService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _filterService = filterService ?? throw new ArgumentNullException(nameof(filterService));
            _scorer = scorer ?? throw new ArgumentNullException(nameof(scorer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Execute search: load, filter, score, sort and page.
        /// </summary>
        public ListingSearchModels.Models.PagedResult<ListingSearchModels.Models.ScoredListing> Search(ListingSearchModels.Models.SearchParams parameters)
        {
            if (parameters is null) throw new ArgumentNullException(nameof(parameters));

            var all = _repository.GetAll();
            // Use model types from models project
            var filtered = _filterService.ApplyFilters(all, parameters);
            var scored = _scorer.Score(filtered, parameters);

            var ordered = scored.OrderByDescending(s => s.Score).ThenByDescending(s => s.Listing.ListedDate);

            var total = ordered.Count();
            var items = ordered.Skip((parameters.Page - 1) * parameters.PageSize).Take(parameters.PageSize).ToList();

            return new ListingSearchModels.Models.PagedResult<ListingSearchModels.Models.ScoredListing>
            {
                Page = parameters.Page,
                PageSize = parameters.PageSize,
                Total = total,
                Items = items
            };
        }
    }
}
