using System.Diagnostics;

namespace AsParallel
{
    internal class TestPLINQ
    {
        private readonly IEnumerable<int> _parallelismDegrees = new List<int>();
        private readonly IEnumerable<int> _rangeLimits = new List<int>();
        private readonly IEnumerable<int> _delays = new List<int>();

        public IEnumerable<IterationResult> Result { get; private set; } = new List<IterationResult>();

        internal TestPLINQ()
        {
            _parallelismDegrees = new List<int>() { 1, 2, 3, 4, 6, 8, 10, 50, 100, 200, 300, 400, 512 };
            _rangeLimits = new List<int>() { 1, 2, 3, 4, 5, 10, 25, 50, 100, 250, 500, 1000 };
            _delays = new List<int>() { 0, 1, 5, 10, 50, 100, 250, 500, 1000, 1500 };
        }

        internal TestPLINQ(IEnumerable<int> parallelismDegrees,
            IEnumerable<int> rangeLimits, IEnumerable<int> delays)
        {
            // Тут нужны проверки на null и пустую коллекцию
            _parallelismDegrees = parallelismDegrees;
            _rangeLimits = rangeLimits;
            _delays = delays;
        }

        public void StartTest()
        {
            Result = _parallelismDegrees.AsParallel()
                .SelectMany(degreeParallelism => _rangeLimits
                .SelectMany(rangeLimit => _delays
                .Select(delay => GetIterationResult(degreeParallelism, delay, rangeLimit))));
        }

        private IterationResult GetIterationResult(int degreeParallelism, int delay, int rangeLimit)
        {
            var source = Enumerable.Range(1, rangeLimit);

            Stopwatch executionTimeWithoutParallel = Stopwatch.StartNew();
            source.Where(_ => Delay(delay)).ToList();
            executionTimeWithoutParallel.Stop();

            var executionTimeWithParallel = Stopwatch.StartNew();
            source.AsParallel().WithDegreeOfParallelism(degreeParallelism).Where(_ => Delay(delay)).ToList();
            executionTimeWithParallel.Stop();

            var difference = executionTimeWithoutParallel.ElapsedMilliseconds - executionTimeWithParallel.ElapsedMilliseconds;

            return new IterationResult(degreeParallelism, delay, rangeLimit,
                executionTimeWithoutParallel.ElapsedMilliseconds, executionTimeWithParallel.ElapsedMilliseconds,
                difference);
        }

        public IEnumerable<IterationResult> GetResultDefaultGrouping()
        {
            var positiveResults = Result.Where(x => x.Difference > 0);

            var groupingByRangeLimit = positiveResults
                .GroupBy(x => x.RangeLimit)
                .Select(x => x.Select(x => x));

            var groupingByDelay = groupingByRangeLimit
                .Select(x => x.GroupBy(x => x.Delay)
                .Select(x => x.Select(x => x)));

            var result = groupingByDelay
                .Select(g => g.Select(d => d.Where(x => x.Difference == d.Select(f => f.Difference).Max())));

            return result.SelectMany(x => x.Select(x => x.FirstOrDefault()));
        }

        private bool Delay(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
            return true;
        }
    }
}
