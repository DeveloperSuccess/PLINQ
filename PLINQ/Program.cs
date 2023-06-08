using AsParallel;

var parallelismDegrees = new List<int>() { 5, 512 };

var rangeLimits = new List<int>() { 10, 50 };

var delays = new List<int>() { 0, 1, 5 };

var test = new TestPLINQ(parallelismDegrees, rangeLimits, delays);

test.StartTest();

foreach (var result in test.GetResultDefaultGrouping())
{
    Console.WriteLine($"Степень параллелизма: {result.DegreeParallelism}; " +
    $"Элементов в коллекции: {result.RangeLimit}; Задержка: {result.Delay} ms; " +
    $"Время последовательной обработки: {result.ExecutionTimeWithoutParallel}; " +
    $"Время параллельной обработки: {result.ExecutionTimeWithParallel}; " +
    $"Ускорение на: {result.Difference} ms");
}

Console.Read();
