using AsParallel;

var parallelismDegrees = new List<int>() { 512 };

var rangeLimits = new List<int>() { 50 };

var delays = new List<int>() { 0, 1, 5 };

var test = new TestPLINQ(parallelismDegrees, rangeLimits, delays);

test.StartTest();

foreach (var result in test.Result)
{
    Console.WriteLine($"Степень параллелизма: {result.DegreeParallelism}; " +
    $"Задержка: {result.Delay} ms;  Элементов в коллекции: {result.RangeLimit}; " +
    $"Время последовательной обработки: {result.ExecutionTimeWithoutParallel}; " +
    $"Время параллельной обработки: {result.ExecutionTimeWithParallel}; " +
    $"Ускорение на: {result.Difference} ms");
}

Console.Read();
