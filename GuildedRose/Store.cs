using System.Collections.Generic;
using System.Linq;

namespace GuildedRose
{
    public static class Store
    {
        public static IEnumerable<Item> UpdateQuality(IEnumerable<Item> items) 
            => items.Select(item => item.UpdateItem());
    }
}