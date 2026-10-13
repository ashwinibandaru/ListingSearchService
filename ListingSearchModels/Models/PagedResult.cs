using System;
using System.Collections.Generic;

namespace ListingSearchModels.Models
{
    /// <summary>
    /// A simple paged result container.
    /// </summary>
    public sealed class PagedResult<T>
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int Total { get; init; }
        public IEnumerable<T> Items { get; init; } = Array.Empty<T>();
    }
}
