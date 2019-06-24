using System;
using System.Reactive.Linq;

namespace rxdemo.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var singleValue = Observable.Return(-1);
            var transformedEvenNumbers = Observable.Interval(TimeSpan.FromMilliseconds(300))
                .Where(x => x % 2 == 0)
                .Select(x => $"Transformed value {x}")
                .CombineLatest(singleValue, (x, y) => $"{x}, single value: {y}");

            var s1 = transformedEvenNumbers.Subscribe(value => Console.WriteLine(value));

            Console.ReadKey();

            s1.Dispose();
        }
    }
}
