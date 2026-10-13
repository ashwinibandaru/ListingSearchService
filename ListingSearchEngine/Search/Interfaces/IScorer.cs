using System.Collections.Generic;


namespace ListingSearchEngine.Search.Interfaces
{
    /// <summary>
    /// Scores listings according to a relevance function.
    /// </summary>
    public interface IScorer
    {
        IEnumerable<ListingSearchModels.Models.ScoredListing> Score(IEnumerable<ListingSearchModels.Models.Listing> source, ListingSearchModels.Models.SearchParams parameters);
    }
}
