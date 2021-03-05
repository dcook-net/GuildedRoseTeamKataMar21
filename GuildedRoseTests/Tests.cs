using System.Collections.Generic;
using FluentAssertions;
using GuildedRose;
using NUnit.Framework;

namespace GildedRoseTests
{
    public class Tests
    {
        private List<Item> _items = new List<Item>
        {
            new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
            new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
            new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
            new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
            new Item
            {
                Name = "Backstage passes to a TAFKAL80ETC concert",
                SellIn = 15,
                Quality = 20
            },
            new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 }
        };

        [Test]
        public void HappyPath()
        {
            Store.UpdateQuality(_items);
            var updatedItems = new List<Item>
            {
                new Item {Name = "+5 Dexterity Vest", SellIn = 9, Quality = 19},
                new Item {Name = "Aged Brie", SellIn = 1, Quality = 1},
                new Item {Name = "Elixir of the Mongoose", SellIn = 4, Quality = 6},
                new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 14,
                    Quality = 21
                },
                new Item { Name = "Conjured Mana Cake", SellIn = 2, Quality = 5 }
            };

            updatedItems.Should().BeEquivalentTo(_items);
        }
        
        [Test]
        public void SulfurasShouldNeverDegradeInQuality()
        {
            var item =  new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 };
            Store.UpdateQuality(new List<Item> { item });
            Assert.That(item.Quality, Is.EqualTo(80));
            Assert.That(item.SellIn, Is.EqualTo(0));
        }
        
        [TestCase("+5 Dexterity Vest", 10, 20)]
        [TestCase("Elixir of the Mongoose", 5, 7)]
        [TestCase("Conjured Mana Cake", 5, 7)]
        public void ItemsShouldDegradeInQualityAndQuantity(string name, int sellIn, int quality)
        {
            var item =  new Item { Name = "+5 Dexterity Vest", SellIn = sellIn, Quality = quality };
            Store.UpdateQuality(new List<Item> { item });
            Assert.That(item.Quality, Is.EqualTo(quality - 1));
            Assert.That(item.SellIn, Is.EqualTo(sellIn - 1));
        }
        
        [Test]
        public void AgedBrieGoesUpInQualityAsAgeIncreases()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 };
            Store.UpdateQuality(new List<Item> { item });
            Assert.That(item.Quality, Is.EqualTo(1));
            Assert.That(item.SellIn, Is.EqualTo(1));
        }
        
        [Test]
        public void ItemsCannotHaveAQualityOver50()
        {
            var items = new List<Item>
            {
                new Item {Name = "Aged Brie", SellIn = 2, Quality = 50},
                new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 4,
                    Quality = 50
                }
            };
            Store.UpdateQuality(items);
            Assert.That(items[0].Quality, Is.EqualTo(50));
            Assert.That(items[1].Quality, Is.EqualTo(50));
        }
        
        [TestCase(10, 20, 2)]
        [TestCase(9, 20, 2)]
        [TestCase(5, 20, 3)]
        [TestCase(4, 20, 3)]
        [TestCase(1, 20, 3)]
        [TestCase(0, 20, -20)]
        public void BackstagePassMeetsQualityRules(int sellIn, int quality, int increase)
        {
            var item = new Item
            {
                Name = "Backstage passes to a TAFKAL80ETC concert",
                SellIn = sellIn,
                Quality = quality
            };
            Store.UpdateQuality(new List<Item> { item });
            Assert.That(item.Quality, Is.EqualTo(quality + increase));
            Assert.That(item.SellIn, Is.EqualTo(sellIn - 1));
        }
        
        [TestCase("+5 Dexterity Vest", 20)]
        [TestCase("Elixir of the Mongoose", 5)]
        [TestCase("Conjured Mana Cake", 5 )]
        public void OnceSellInDateHasPassedQualityDegradesTwiceAsFast(string name, int quality)
        {
            var item = new Item
            {
                Name = name,
                SellIn = 0,
                Quality = quality
            };
            Store.UpdateQuality(new List<Item> { item });
            Assert.That(item.Quality, Is.EqualTo(quality - 2));
        }
        
        [TestCase("+5 Dexterity Vest")]
        [TestCase("Elixir of the Mongoose")]
        [TestCase("Conjured Mana Cake")]
        public void ItemQualityCanNeverGoBelowZero(string name)
        {
            var item = new Item
            {
                Name = name,
                SellIn = 1,
                Quality = 0
            };
            Store.UpdateQuality(new List<Item> { item });
            Assert.That(item.Quality, Is.EqualTo(0));
        }
    }
}