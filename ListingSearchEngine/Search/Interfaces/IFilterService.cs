using System.Collections.Generic;
using Listing = ListingSearchModels.Models.Listing;
using SearchParams = ListingSearchModels.Models.SearchParams;

namespace ListingSearchEngine.Search.Interfaces
{
    /// <summary>
    /// Applies filter criteria to a sequence of listings.
    /// </summary>
    public interface IFilterService
    {
        IEnumerable<Listing> ApplyFilters(IEnumerable<Listing> source, SearchParams parameters);
    }
}
