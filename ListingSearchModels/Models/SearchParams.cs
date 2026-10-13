using System;

namespace ListingSearchModels.Models
{
    /// <summary>
    /// Parameters used to filter and page search results.
    /// </summary>
    public sealed class SearchParams
    {
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public int MinBedrooms { get; init; }
        public string? City { get; init; }
        public string? Keyword { get; init; }
        public decimal? TargetBudget { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 5;
    }
}
