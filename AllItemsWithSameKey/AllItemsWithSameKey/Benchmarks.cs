using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace AllItemsWithSameKey
{
    [MemoryDiagnoser]
    public abstract class BenchmarkBase
    {
        private readonly MultipleParentKeyDetector _detector = new MultipleParentKeyDetector();
        private readonly IEnumerable<Item> _data;

        protected BenchmarkBase()
        {
            _data = GetData().ToArray();
        }

        protected abstract IEnumerable<Item> GetData();

        [Benchmark(Baseline = true)]
        public bool Foreach() => _detector.Foreach(_data);

        [Benchmark]
        public bool GroupBy_Count() => _detector.GroupBy_Count(_data);

        [Benchmark]
        public bool Select_Distinct_Count() => _detector.Select_Distinct_Count(_data);

        [Benchmark]
        public bool Select_Distinct_Skip_Any() => _detector.Select_Distinct_Skip_Any(_data);
    }

    public class AllTheSame : BenchmarkBase
    {
        protected override IEnumerable<Item> GetData() => Enumerable.Range(0, 1000).Select(_ => new Item { ParentKey = "key1" });
    }

    public class FirstDifferent : BenchmarkBase
    {
        protected override IEnumerable<Item> GetData() => Enumerable.Range(0, 999).Select(_ => new Item { ParentKey = "key1" }).Prepend(new Item { ParentKey = "key2" });
    }

    public class LastDifferent : BenchmarkBase
    {
        protected override IEnumerable<Item> GetData() => Enumerable.Range(0, 999).Select(_ => new Item { ParentKey = "key1" }).Append(new Item { ParentKey = "key2" });
    }

    public class MiddleDifferent : BenchmarkBase
    {
        protected override IEnumerable<Item> GetData() => Enumerable.Range(0, 500).Select(_ => new Item { ParentKey = "key1" })
            .Append(new Item { ParentKey = "key2" })
            .Concat(Enumerable.Range(0, 499).Select(_ => new Item { ParentKey = "key1" }));
    }

    public class Item
    {
        public string ParentKey { get; init; }
    }
}
