using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ToArrayOrToList
{
    [MemoryDiagnoser]
    public class ToArrayOrToList
    {
        [Benchmark]
        [ArgumentsSource(nameof(SourceEnumerables))]
        public object[] ToArray(Source source) => source.Enumerable.ToArray();

        [Benchmark]
        [ArgumentsSource(nameof(SourceEnumerables))]
        public List<object> ToList(Source source) => source.Enumerable.ToList();

        public IEnumerable<Source> SourceEnumerables()
        {
            var counts = new[] { 10, 1000, 10000 };
            foreach (var count in counts)
            {
                // Scenarios...
                yield return new Source(nameof(Array), count, Array(count));
            }
        }

        public object[] Array(int count) => Enumerable.Range(0, count).Select(_ => new object()).ToArray();
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
