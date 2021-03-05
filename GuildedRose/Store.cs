using System.Collections.Generic;
using System.Linq;

namespace GuildedRose
{
    public class Store
    {
        private class ItemTypes
        {
            public const string AgedBrie = "Aged Brie";
            public const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
            public const string Sulfuras = "Sulfuras, Hand of Ragnaros";
        }
        
        public static void UpdateQuality(IList<Item> items)
        {
            foreach (var item in items.Where(i => i.Name != ItemTypes.Sulfuras && i.Quality < 50))
            {
                if (item.Name != ItemTypes.AgedBrie && item.Name != ItemTypes.BackstagePasses)
                {
                    if (item.Quality > 0)
                    {
                        item.Quality = item.Quality - 1;
                    }
                }
                else
                {
                    item.IncreaseQuality();

                    if (item.Name == ItemTypes.BackstagePasses)
                    {
                        if (item.SellIn < 11)
                        {
                            item.IncreaseQuality();
                        }

                        if (item.SellIn < 6)
                        {
                            item.IncreaseQuality();
                        }
                    }
                }

                item.SellIn = item.SellIn - 1;

                if (item.SellIn < 0)
                {
                    if (item.Name != ItemTypes.AgedBrie)
                    {
                        if (item.Name != ItemTypes.BackstagePasses)
                        {
                            if (item.Quality > 0)
                            {
                                if (item.Name != ItemTypes.Sulfuras)
                                {
                                    item.Quality = item.Quality - 1;
                                }
                            }
                        }
                        else
                        {
                            item.Quality = item.Quality - item.Quality;
                        }
                    }
                    else
                    {
                        item.IncreaseQuality();
                    }
                }
            }
        }
    }
}