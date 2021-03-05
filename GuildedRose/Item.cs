namespace GuildedRose
{
    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }
    
    public static class ItemExtensions
    {
        public static void IncreaseQuality(this Item i)
        {
            i.Quality += (i.SellIn, i.Name) switch
            {
                (int s, ItemTypes.BackstagePasses) when s < 6 => 3,
                (int s, ItemTypes.BackstagePasses) when s < 11 => 2,
                _ => 1
            };
        }

        public static void UpdateQuality(this Item i)
        {
            var (flag, increase) = (i.SellIn, i.Name) switch
            {
                (int s, ItemTypes.BackstagePasses) when s == 0 => (false, 0),
                (int s, ItemTypes.BackstagePasses) when s < 6 => (true, 3),
                (int s, ItemTypes.BackstagePasses) when s < 11 => (true, 2),
                (int s, ItemTypes.AgedBrie) => (true, 1),
                (int s, ItemTypes.Sulfuras) => (false, 0),
                (int s, string str) when s >= 0 => (false, 1),
                _ => (true, -1)
            };

            
            if (flag)
            {
                i.Quality += increase;
            }
            else if(i.Name != ItemTypes.Sulfuras)
            {
                i.SetQualityToZero();
            }
        }

        public static void DecreaseQuality(this Item i)
        {
            if (i.CanDecreaseInQuality() && i.Quality > 0)
            {
                i.Quality -= 1;
            }
        }
        
        public static void SetQualityToZero(this Item i)
        {
            i.Quality = 0;
        }

        public static bool CanDecreaseInQuality(this Item item)
        {
            return item.Name switch
            {
                ItemTypes.BackstagePasses => false,
                ItemTypes.AgedBrie => false,
                _ => true
            };
        }
        
    }
}