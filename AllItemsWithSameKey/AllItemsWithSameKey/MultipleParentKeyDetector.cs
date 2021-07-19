using System.Collections.Generic;
using System.Linq;

namespace AllItemsWithSameKey
{
    public class MultipleParentKeyDetector
    {
        public bool GroupBy_Count(IEnumerable<Item> data) => data.GroupBy(x => x.ParentKey).Count() > 1;

        public bool Select_Distinct_Count(IEnumerable<Item> data) => data.Select(x => x.ParentKey).Distinct().Count() > 1;

        public bool Select_Distinct_Skip_Any(IEnumerable<Item> data) => data.Select(x => x.ParentKey).Distinct().Skip(1).Any();

        public bool Foreach(IEnumerable<Item> data)
        {
            var first = true;
            string firstKey = null;
            foreach (var item in data)
            {
                if (firstKey != item.ParentKey && !first)
                    return true;
                if (first)
                {
                    firstKey = item.ParentKey;
                    first = false;
                }
            }
            return false;
        }
    }
}