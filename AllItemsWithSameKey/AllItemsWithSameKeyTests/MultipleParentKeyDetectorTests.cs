using System;
using System.Collections.Generic;
using AllItemsWithSameKey;
using Xunit;

namespace AllItemsWithSameKeyTests
{
    public class MultipleParentKeyDetectorTests
    {
        [Theory]
        [MemberData(nameof(ProvideMethods))]
        public void ReturnsFalseForAllItemsWithTheSameKey(Func<IEnumerable<Item>, bool> doesAllHaveTheSameKeyFn)
        {
            var items = new[] { new Item { ParentKey = "key1" }, new Item { ParentKey = "key1" } };
            Assert.False(doesAllHaveTheSameKeyFn(items));
        }

        [Theory]
        [MemberData(nameof(ProvideMethods))]
        public void ReturnsFalseForEmptyCollection(Func<IEnumerable<Item>, bool> doesAllHaveTheSameKeyFn)
        {
            Assert.False(doesAllHaveTheSameKeyFn(Array.Empty<Item>()));
        }

        [Theory]
        [MemberData(nameof(ProvideMethods))]
        public void ReturnsTrueForItemsWithDifferentKeys(Func<IEnumerable<Item>, bool> doesAllHaveTheSameKeyFn)
        {
            var items = new[] { new Item { ParentKey = "key1" }, new Item { ParentKey = "key2" } };
            Assert.True(doesAllHaveTheSameKeyFn(items));
        }

        public static IEnumerable<object[]> ProvideMethods()
        {
            var validator = new MultipleParentKeyDetector();
            var methods = new Func<IEnumerable<Item>, bool>[]
            {
                validator.Select_Distinct_Count,
                validator.Select_Distinct_Skip_Any,
                validator.Foreach,
                validator.GroupBy_Count
            };

            foreach (var method in methods)
                yield return new object[] { method };
        }
    }
}
