namespace PLINQ
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<IterationResult> GetResultDefaultGrouping(this IEnumerable<IterationResult> source)
        {
            var positiveResults = source.Where(x => x.Difference > 0);

            var groupingByRangeLimit = positiveResults
                .GroupBy(x => x.RangeLimit)
                .Select(x => x.Select(x => x));

            var groupingByDelay = groupingByRangeLimit
                .Select(x => x.GroupBy(x => x.Delay)
                .Select(x => x.Select(x => x)));

            var result = groupingByDelay
                .Select(g => g.Select(d => d.Where(x => x.Difference == d.Select(f => f.Difference).Max())));

            return result.SelectMany(x => x.Select(x => x.FirstOrDefault()))
                .OrderBy(x => x.RangeLimit)
                .ThenBy(x => x.Delay);
        }
    }
}
