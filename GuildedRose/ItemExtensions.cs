using static GuildedRose.ItemTypes;

namespace GuildedRose
{
    public static class ItemExtensions
    {
        public static Item UpdateItem(this Item i) =>
            new Item
            {
                Name = i.Name,
                SellIn = CalculateNewSellin(i),
                Quality = CalculateNewQuality(i)
            };

        private static int CalculateNewSellin(this Item i) =>
            i.Name switch
            {
                Sulfuras => i.SellIn,
                _ => i.SellIn - 1
            };

        private static int CalculateNewQuality(this Item i)
        {
            var newQuality = (i.SellIn, i.Name) switch
            {
                ({ } s, BackstagePasses) when s == 0 => 0,
                ({ } s, BackstagePasses) when s < 6 => i.Quality + 3,
                ({ } s, BackstagePasses) when s <= 10 => i.Quality + 2,
                ({ } s, BackstagePasses) when s > 10 => i.Quality + 1,
                (_, AgedBrie) => i.Quality + 1,
                ({ } s, _) when s > 0 => i.Quality - 1,
                ({ } s, _) when s <= 0 => i.Quality - 2
            };

            return i.Name == Sulfuras 
                ? i.Quality 
                : newQuality switch
                            {
                                { } q when q <= 0 => 0,
                                { } q when q >= 50 => 50,
                                _ => newQuality
                            };
        }
    }
}