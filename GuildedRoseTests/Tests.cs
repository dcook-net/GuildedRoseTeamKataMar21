using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GuildedRose;
using NUnit.Framework;

namespace GildedRoseTests
{
    public class Tests
    {
        [Test]
        public void HappyPath()
        {
            var originalItems = new List<Item>
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
                new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
            };

            var expectedItems = new List<Item>
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
                new Item {Name = "Conjured Mana Cake", SellIn = 2, Quality = 5}
            };

            var updatedItems = Store.UpdateQuality(originalItems);

            updatedItems.Should().BeEquivalentTo(expectedItems);
        }

        [Test]
        public void SulfurasShouldNeverDegradeInQuality()
        {
            var item =  new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 };
            
            var updatedItem = Store.UpdateQuality(new List<Item> { item }).First();
            
            Assert.That(updatedItem.Quality, Is.EqualTo(80));
            Assert.That(updatedItem.SellIn, Is.EqualTo(0));
        }
        
        [TestCase("+5 Dexterity Vest", 10, 20)]
        [TestCase("Elixir of the Mongoose", 5, 7)]
        [TestCase("Conjured Mana Cake", 5, 7)]
        public void ItemsShouldDegradeInQualityAndQuantity(string name, int sellIn, int quality)
        {
            var item =  new Item { Name = "+5 Dexterity Vest", SellIn = sellIn, Quality = quality };
            
            var updatedItem = Store.UpdateQuality(new List<Item> { item }).First();
            
            Assert.That(updatedItem.Quality, Is.EqualTo(quality - 1));
            Assert.That(updatedItem.SellIn, Is.EqualTo(sellIn - 1));
        }
        
        [Test]
        public void AgedBrieGoesUpInQualityAsAgeIncreases()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 };
           
            var updatedItem = Store.UpdateQuality(new List<Item> { item }).First();
            
            Assert.That(updatedItem.Quality, Is.EqualTo(1));
            Assert.That(updatedItem.SellIn, Is.EqualTo(1));
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
            
            var updatedItems = Store.UpdateQuality(items);

            updatedItems.Should().OnlyContain(x => x.Quality == 50);
            
            // collection.Should().OnlyContain(x => x < 10);    
            // Assert.That(updatedItems[0].Quality, Is.EqualTo(50));
            // Assert.That(updatedItems[1].Quality, Is.EqualTo(50));
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
            
            var updatedItem = Store.UpdateQuality(new List<Item> { item }).First();

            var expected = quality + increase;
            
            Assert.That(updatedItem.Quality, Is.EqualTo(expected));
            Assert.That(updatedItem.SellIn, Is.EqualTo(sellIn - 1));
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
            
            var updatedItem = Store.UpdateQuality(new List<Item> { item }).First();
            
            Assert.That(updatedItem.Quality, Is.EqualTo(quality - 2));
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
            
            var updatedItem = Store.UpdateQuality(new List<Item> { item }).First();
            
            Assert.That(updatedItem.Quality, Is.EqualTo(0));
        }
    }
}