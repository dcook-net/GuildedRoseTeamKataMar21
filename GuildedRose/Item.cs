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
            i.Quality += 1;
        }
    }
}