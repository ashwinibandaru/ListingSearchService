namespace ListingSearchEngine.Config
{
    /// <summary>
    /// Configuration options for scoring.
    /// </summary>
    public sealed class ScoringOptions
    {
        public double BudgetWeight { get; set; } = 0.6;
        public double RecencyWeight { get; set; } = 0.4;
        public double MaxRecencyDays { get; set; } = 365.0;
    }
}
