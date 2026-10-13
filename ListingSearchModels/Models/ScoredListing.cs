using System;

namespace ListingSearchModels.Models
{
    /// <summary>
    /// A listing together with a computed relevance score.
    /// </summary>
    public sealed class ScoredListing
    {
        public Listing Listing { get; init; } = new Listing();
        public double Score { get; init; }
    }
}
