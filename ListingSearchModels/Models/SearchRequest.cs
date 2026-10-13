namespace ListingSearchModels.Models
{
    /// <summary>
    /// Represents incoming search request parameters bound from query string.
    /// </summary>
    public sealed class SearchRequest
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinBedrooms { get; set; }
        public string? City { get; set; }
        public string? Keyword { get; set; }
        public decimal? TargetBudget { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
