namespace PLINQ
{
    internal class TestTemplates
    {
        /// <summary>
        /// Тестирование с авторскими значениями
        /// </summary>
        public static void StartDefaultTest()
        {
            var test = new TestPLINQ();

            test.StartTest();

            Console.WriteLine($"Общее время выполнения теста: {test.ExecutionTime}");

            foreach (var result in test.Result.GetResultDefaultGrouping())
            {
                Console.WriteLine($"Степень параллелизма: {result.DegreeParallelism}; " +
                $"Элементов в коллекции: {result.RangeLimit}; Задержка: {result.Delay} ms; " +
                $"Время последовательной обработки: {result.ExecutionTimeWithoutParallel}; " +
                $"Время параллельной обработки: {result.ExecutionTimeWithParallel}; " +
                $"Ускорение на: {result.Difference} ms");
            }

            Console.Read();
        }

        /// <summary>
        /// Тест времени выполнения тестирования в зависимости от вложенности AsParallel
        /// </summary>
        public static void StartTestExecutionTimeNestingAsParallel()
        {
            var nestings = Enumerable.Range(0, 3);

            var tests = nestings.AsParallel().Select(nesting => new TestPLINQ().StartTest(nesting));

            //return tests.Sum(_ => _.ExecutionTime);
        }

        /// <summary>
        /// Тестирование с пользовательскими значениями
        /// </summary>
        public static void StartExampleCustomTest()
        {
            var parallelismDegrees = new List<int>() { 2, 5, 512 };

            var rangeLimits = new List<int>() { 5 };

            var delays = new List<int>() { 50, 100 };

            var test = new TestPLINQ(parallelismDegrees, rangeLimits, delays);

            test.StartTest();

            Console.WriteLine($"Общее время выполнения теста: {test.ExecutionTime}");

            foreach (var result in test.Result.GetResultDefaultGrouping())
            {
                Console.WriteLine($"Степень параллелизма: {result.DegreeParallelism}; " +
                $"Элементов в коллекции: {result.RangeLimit}; Задержка: {result.Delay} ms; " +
                $"Время последовательной обработки: {result.ExecutionTimeWithoutParallel}; " +
                $"Время параллельной обработки: {result.ExecutionTimeWithParallel}; " +
                $"Ускорение на: {result.Difference} ms");
            }

            Console.Read();
        }
    }
}
