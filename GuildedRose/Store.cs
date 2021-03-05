using System.Collections.Generic;
using System.Linq;

namespace GuildedRose
{
    public static class ItemTypes
    {
        public const string AgedBrie = "Aged Brie";
        public const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
        public const string Sulfuras = "Sulfuras, Hand of Ragnaros";
    }
    
    public static class Store
    {
        public static void UpdateQuality(IEnumerable<Item> items)
        {
            foreach (var item in items.Where(i => i.Name != ItemTypes.Sulfuras && i.Quality < 50))
            {
                item.UpdateQuality();

                item.SellIn--;
                if (item.SellIn >= 0) continue;
            }
        }
    }
}