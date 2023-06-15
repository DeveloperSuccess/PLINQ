using PLINQ;

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
