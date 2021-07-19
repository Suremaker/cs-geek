using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ToArrayOrToList
{
    [MemoryDiagnoser]
    public class ToArrayOrToList
    {
        [Benchmark]
        [ArgumentsSource(nameof(SourceEnumerables))]
        public object[] ToArray(Source source) => source.Enumerable.ToArray();

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(SourceEnumerables))]
        public List<object> ToList(Source source) => source.Enumerable.ToList();

        public IEnumerable<Source> SourceEnumerables()
        {
            var counts = new[] { 10, 1000, 10000 };
            foreach (var count in counts)
            {
                // Scenarios...
                yield return new Source("Array", count, Array(count));
                yield return new Source("Array.Select", count, Array(count).Select(o => o));
                yield return new Source("Array.Where", count, Array(count).Where(o => true));
                yield return new Source("Array.Take", count, Array(count).Take(count / 2));
                yield return new Source("Array.Skip", count, Array(count).Skip(count / 2));
                yield return new Source("Array.Cast", count, Array(count).Cast<object>());
                yield return new Source("Array.OfType", count, Array(count).OfType<object>());
                yield return new Source("Yield", count, Yield(Array(count)));
                yield return new Source("Complex", count, Array(count).Where(o => true).Select(o => o).Skip(2).Take(count / 2));
            }
        }
        public object[] Array(int count) => Enumerable.Range(0, count).Select(_ => new object()).ToArray();

        public IEnumerable<object> Yield(object[] data)
        {
            foreach (var d in data)
                yield return d;
        }
    }

    public class Source
    {
        private readonly string _name;
        private readonly int _count;
        public IEnumerable<object> Enumerable { get; }

        public Source(string name, int count, IEnumerable<object> enumerable)
        {
            Enumerable = enumerable;
            _name = name;
            _count = count;
        }

        public override string ToString() => $"{_name}[{_count}]";
    }

    public class Program
    {
        static void Main(string[] args) => BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}
