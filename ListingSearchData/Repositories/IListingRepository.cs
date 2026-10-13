using System.Collections.Generic;
using ListingSearchModels.Models;

namespace ListingSearchData.Repositories
{
    /// <summary>
    /// Repository abstraction for loading listings.
    /// </summary>
    public interface IListingRepository
    {
        IEnumerable<Listing> GetAll();
    }
}
