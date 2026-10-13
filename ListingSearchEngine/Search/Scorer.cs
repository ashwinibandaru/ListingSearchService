using System;
using System.Collections.Generic;
using System.Linq;
using ListingSearchEngine.Search.Interfaces;
using ListingSearchModels.Models;
using ListingSearchEngine.Config;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace ListingSearchEngine.Search
{
    /// <summary>
    /// Default scorer combining budget proximity and recency.
    /// </summary>
    public sealed class Scorer : IScorer
    {
        private readonly ScoringOptions _options;
        private readonly Microsoft.Extensions.Logging.ILogger<Scorer> _logger;

        public Scorer(Microsoft.Extensions.Options.IOptions<ScoringOptions> options, Microsoft.Extensions.Logging.ILogger<Scorer> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<ScoredListing> Score(IEnumerable<Listing> source, ListingSearchModels.Models.SearchParams parameters)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (parameters is null) throw new ArgumentNullException(nameof(parameters));

            try
            {
                var utcNow = DateTime.UtcNow;

                return source.Select(x =>
                {
                    double budgetScore = 0.5;
                    if (parameters.TargetBudget.HasValue && parameters.TargetBudget.Value > 0)
                    {
                        var diff = Math.Abs((double)(x.Price - parameters.TargetBudget.Value));
                        var denom = Math.Max((double)parameters.TargetBudget.Value, 1.0);
                        var norm = Math.Min(diff / denom, 1.0);
                        budgetScore = 1.0 - norm;
                    }

                    var days = (utcNow - x.ListedDate.ToUniversalTime()).TotalDays;
                    if (days < 0) days = 0;
                    var recencyScore = 1.0 - Math.Min(days / _options.MaxRecencyDays, 1.0);

                    var score = _options.BudgetWeight * budgetScore + _options.RecencyWeight * recencyScore;

                    return new ScoredListing { Listing = x, Score = Math.Round(score, 4) };
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while scoring listings. Parameters: {@Parameters}", parameters);
                throw;
            }
        }
    }
}
