using System.Diagnostics;

namespace PLINQ
{
    internal class TestPLINQ
    {
        private readonly IEnumerable<int> _parallelismDegrees = new List<int>();
        private readonly IEnumerable<int> _rangeLimits = new List<int>();
        private readonly IEnumerable<int> _delays = new List<int>();

        public int Nesting { get; private set; }
        public long? ExecutionTime { get; private set; } = null;
        public IEnumerable<IterationResult> Result { get; private set; } = new List<IterationResult>();

        internal TestPLINQ()
        {
            _parallelismDegrees = new List<int>() { 1, 2, 3, 4, 6, 8, 10, 50, 100, 200, 300, 400, 512 };
            _rangeLimits = new List<int>() { 1, 2, 3, 4, 5, 10, 25, 50, 100, 250, 500, 1000 };
            _delays = new List<int>() { 0, 1, 5, 10, 50, 100, 250, 500, 1000, 1500, 2000 };
        }

        internal TestPLINQ(IEnumerable<int> parallelismDegrees,
            IEnumerable<int> rangeLimits, IEnumerable<int> delays)
        {
            // Тут нужны проверки на null и пустую коллекцию
            _parallelismDegrees = parallelismDegrees;
            _rangeLimits = rangeLimits;
            _delays = delays;
        }

        public TestPLINQ StartTest(int nestingAsParallel = 1)
        {
            Nesting = nestingAsParallel;

            var stopwatch = Stopwatch.StartNew();

            if (nestingAsParallel == 0)
            {
                Result = _parallelismDegrees
                .SelectMany(degreeParallelism => _rangeLimits
                .SelectMany(rangeLimit => _delays
                .Select(delay => GetIterationResult(degreeParallelism, delay, rangeLimit))))
                .ToList();
            }

            if (nestingAsParallel == 1)
            {
                Result = _parallelismDegrees.AsParallel()
                .SelectMany(degreeParallelism => _rangeLimits
                .SelectMany(rangeLimit => _delays
                .Select(delay => GetIterationResult(degreeParallelism, delay, rangeLimit))))
                .ToList();
            }

            if (nestingAsParallel == 2)
            {
                Result = _parallelismDegrees.AsParallel()
                .SelectMany(degreeParallelism => _rangeLimits.AsParallel()
                .SelectMany(rangeLimit => _delays
                .Select(delay => GetIterationResult(degreeParallelism, delay, rangeLimit))))
                .ToList();
            }

            if (nestingAsParallel == 3)
            {
                Result = _parallelismDegrees.AsParallel()
                .SelectMany(degreeParallelism => _rangeLimits.AsParallel()
                .SelectMany(rangeLimit => _delays.AsParallel()
                .Select(delay => GetIterationResult(degreeParallelism, delay, rangeLimit))))
                .ToList();
            }

            ExecutionTime = stopwatch.ElapsedMilliseconds;

            return this;
        }

        private IterationResult GetIterationResult(int degreeParallelism, int delay, int rangeLimit)
        {
            var source = Enumerable.Range(1, rangeLimit);

            var executionTimeWithoutParallel = Stopwatch.StartNew();
            source.Where(_ => Delay(delay)).ToList();
            executionTimeWithoutParallel.Stop();

            var executionTimeWithParallel = Stopwatch.StartNew();
            source.AsParallel().WithDegreeOfParallelism(degreeParallelism).Where(_ => Delay(delay)).ToList();
            executionTimeWithParallel.Stop();

            var difference = executionTimeWithoutParallel.ElapsedMilliseconds - executionTimeWithParallel.ElapsedMilliseconds;

            return new IterationResult(degreeParallelism, delay, rangeLimit,                
                executionTimeWithoutParallel.ElapsedMilliseconds,
                executionTimeWithParallel.ElapsedMilliseconds,
                difference);
        }

        private bool Delay(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
            return true;
        }
    }
}
