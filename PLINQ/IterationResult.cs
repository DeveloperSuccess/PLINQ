namespace PLINQ
{
    internal class IterationResult
    {
        internal IterationResult(int degreeParallelism, int delay, int rangeLimit,
             long executionTimeWithoutParallel, long executionTimeWithParallel,
            long difference)
        {
            DegreeParallelism = degreeParallelism;
            Delay = delay;
            RangeLimit = rangeLimit;
            ExecutionTimeWithoutParallel = executionTimeWithoutParallel;
            ExecutionTimeWithParallel = executionTimeWithParallel;
            Difference = difference;
        }

        /// <summary>
        /// Степень параллелизма
        /// </summary>
        public int DegreeParallelism { get; }

        /// <summary>
        /// Задержка
        /// </summary>
        public int Delay { get; }

        /// <summary>
        /// Элементов в коллекции
        /// </summary>
        public int RangeLimit { get; }

        /// <summary>
        /// Время последовательной обработки
        /// </summary>
        public long ExecutionTimeWithoutParallel { get; }

        /// <summary>
        /// Время параллельной обработки
        /// </summary>
        public long ExecutionTimeWithParallel { get; }

        /// <summary>
        /// Разность времени между последовательной и паралельной обработкой
        /// </summary>
        public long Difference { get; }
    }
}
