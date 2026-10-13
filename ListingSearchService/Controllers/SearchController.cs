using System;
using Microsoft.AspNetCore.Mvc;
using ListingSearchEngine.Search;

namespace ListingSearchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly SearchService _searchService;
        private readonly Microsoft.Extensions.Logging.ILogger<SearchController> _logger;

        public SearchController(SearchService searchService, Microsoft.Extensions.Logging.ILogger<SearchController> logger)
        {
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Search listings with filters, ranking and pagination.
        /// </summary>
        [HttpGet]
        public ActionResult<ListingSearchModels.Models.PagedResult<ListingSearchModels.Models.ScoredListing>> Get([FromQuery] ListingSearchModels.Models.SearchRequest request)
        {
            if (request is null) return BadRequest(new { error = "request is required" });

            if (request.Page <= 0) return BadRequest(new { error = "page must be >= 1" });
            if (request.PageSize <= 0) return BadRequest(new { error = "pageSize must be >= 1" });
            if (request.MinPrice.HasValue && request.MaxPrice.HasValue && request.MinPrice > request.MaxPrice)
                return BadRequest(new { error = "minPrice cannot be greater than maxPrice" });

            var parameters = new ListingSearchModels.Models.SearchParams
            {
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                MinBedrooms = request.MinBedrooms ?? 0,
                City = string.IsNullOrWhiteSpace(request.City) ? null : request.City,
                Keyword = string.IsNullOrWhiteSpace(request.Keyword) ? null : request.Keyword,
                TargetBudget = request.TargetBudget,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = _searchService.Search(parameters);
            return result;
        }
    }
}
