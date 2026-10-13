using System;

namespace ListingSearchModels.Models
{
    /// <summary>
    /// Represents a property listing.
    /// </summary>
    public sealed class Listing
    {
        public string Id { get; init; } = string.Empty;
        public string Source { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Bedrooms { get; init; }
        public double Bathrooms { get; init; }
        public int Sqft { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public DateTime ListedDate { get; init; }
        public string Status { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }
}
