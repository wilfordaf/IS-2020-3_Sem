using System.Collections.Generic;
using NUnit.Framework;
using Shops.Models;
using Shops.Tools;

namespace Shops.Tests
{
    public class ShopManagerTests
    {
        private ShopManager _shopManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
        }

        [Test]
        public void RegisterProducts_DeliveryProductsToShop_BuyProduct()
        {
            Shop shop = _shopManager.CreateShop("Magnit", "Mebel'naya st 49");

            _shopManager.RegisterProduct("tomato");
            _shopManager.RegisterProduct("onion");
            _shopManager.RegisterProduct("cucumber");

            var onion = new Product("onion", 220, 6);
            var addOnion = new Product("onion", 6);

            _shopManager.AddProductToShop(shop, onion);
            _shopManager.AddProductToShop(shop, addOnion);

            var onionToBuy = new Product("onion", 7);

            var person = new Person("Sergey", 20000);

            shop.BuyProduct(person, onionToBuy);
            Assert.AreEqual(20000 - onion.Price * onionToBuy.Amount, person.Money);
        }

        [Test]
        public void DeliverUnregisteredInsuffientAmountInsufficientFunds_ThrowException()
        {
            Shop shop = _shopManager.CreateShop("Magnit", "Mebel'naya st 49");

            var tomato = new Product("tomato", 250, 5);
            
            Assert.Catch<ShopException>(() =>
            {
                _shopManager.AddProductToShop(shop, tomato);
            });

            _shopManager.RegisterProduct("tomato");
            _shopManager.AddProductToShop(shop, tomato);

            var person = new Person("Sergey", 2000);
            var tomatoToBuy = new Product("tomato", 10);

            Assert.Catch<ShopException>(() =>
            {
                shop.BuyProduct(person, tomatoToBuy);
            });

            var tomatoToBuySome = new Product("tomato", 3);
            person.Money = 200;

            Assert.Catch<ShopException>(() =>
            {
                shop.BuyProduct(person, tomatoToBuySome);
            });
        }

        [Test]
        public void FindProduct_ChangePrice()
        {
            Shop shop = _shopManager.CreateShop("Magnit", "Mebel'naya st 49");
            _shopManager.RegisterProduct("tomato");

            var tomato = new Product("tomato", 250, 5);
            _shopManager.AddProductToShop(shop, tomato);
            
            shop.ChangePrice("tomato", 2500);

            var person = new Person("Sergey", 5000);
            var tomatoToBuy = new Product("tomato", 2);
            shop.BuyProduct(person, tomatoToBuy);

            Assert.AreEqual(0, person.Money);
        }

        [Test]
        public void BuyCheapest()
        {
            _shopManager.RegisterProduct("tomato");
            Shop shop1 = _shopManager.CreateShop("Magnit", "Mebel'naya st 49");
            Shop shop2 = _shopManager.CreateShop("Magnit", "Mebel'naya st 4949");
            Shop shop3 = _shopManager.CreateShop("Magnit", "Mebel'naya st 494949");

            var tomato1 = new Product("tomato", 250, 5);
            var tomato2 = new Product("tomato", 220, 1);
            var tomato3 = new Product("tomato", 235, 3);

            shop1.AddProduct(tomato1);
            shop2.AddProduct(tomato2);
            shop3.AddProduct(tomato3);

            var person = new Person("Sergey", 1000);
            var tomatoToBuy = new Product("tomato", 3);

            _shopManager.BuyCheapest(person, tomatoToBuy);
            Assert.AreEqual(1000 - tomato3.Price * tomatoToBuy.Amount, person.Money);
        }

        [Test]
        public void BuyCheapestNoProduct_ThrowException()
        {
            _shopManager.RegisterProduct("tomato");
            Shop shop1 = _shopManager.CreateShop("Magnit", "Mebel'naya st 49");
            Shop shop2 = _shopManager.CreateShop("Magnit", "Mebel'naya st 4949");
            Shop shop3 = _shopManager.CreateShop("Magnit", "Mebel'naya st 494949");

            var tomato1 = new Product("tomato", 250, 5);
            var tomato2 = new Product("tomato", 220, 1);
            var tomato3 = new Product("tomato", 235, 3);

            shop1.AddProduct(tomato1);
            shop2.AddProduct(tomato2);
            shop3.AddProduct(tomato3);

            var person = new Person("Sergey", 1000);
            var lettuceToBuy = new Product("lettuce", 3);

            Assert.Catch<ShopException>(() =>
            {
                _shopManager.BuyCheapest(person, lettuceToBuy);
            });
        }

        [Test]
        public void BuyProducts()
        {
            Shop shop = _shopManager.CreateShop("Magnit", "Mebel'naya st 49");

            _shopManager.RegisterProduct("tomato");
            _shopManager.RegisterProduct("cucumber");

            var tomato = new Product("tomato", 250, 5);
            var cucumber = new Product("cucumber", 200, 8);
            var productList = new List<Product> { tomato, cucumber };

            _shopManager.AddProductsToShop(shop, productList);

            var tomatoToBuy = new Product("tomato", 2);
            var cucumberToBuy = new Product("cucumber", 2);
            var productToBuyList = new List<Product> { tomatoToBuy, cucumberToBuy };

            var person = new Person("Sergey", 20000);

            shop.BuyProducts(person, productToBuyList);
            Assert.AreEqual(20000 - tomato.Price * tomatoToBuy.Amount - cucumber.Price * cucumberToBuy.Amount, person.Money);
            Assert.AreEqual(5 - tomatoToBuy.Amount, shop.FindProduct("tomato").Amount);
            Assert.AreEqual(8 - cucumberToBuy.Amount, shop.FindProduct("cucumber").Amount);
        }
    }
}
