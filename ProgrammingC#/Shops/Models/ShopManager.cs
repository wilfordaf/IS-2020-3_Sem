using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Models
{
    public class ShopManager
    {
        private readonly List<Shop> _shops = new List<Shop>();
        private readonly List<string> _registeredProducts = new List<string>();

        public Shop FindShopById(int id)
        {
            return _shops.FirstOrDefault(s => s.Id == id);
        }

        public Shop CreateShop(string name, string address)
        {
            var newShop = new Shop(name, address);
            _shops.Add(newShop);
            return newShop;
        }

        public void RegisterProduct(string name)
        {
            _registeredProducts.Add(name);
        }

        public void AddProductToShop(Shop shop, Product newProduct)
        {
            if (!_registeredProducts.Contains(newProduct.Name))
            {
                throw new ShopException("This product is unregistered");
            }

            shop.AddProduct(newProduct);
        }

        public void AddProductsToShop(Shop shop, List<Product> products)
        {
            foreach (Product product in products)
            {
                AddProductToShop(shop, product);
            }
        }

        public void BuyCheapest(Person person, Product productToBuy)
        {
            Shop bestShop = null;
            int bestPrice = int.MaxValue;

            foreach (Shop shop in _shops)
            {
                try
                {
                    Product product = shop.IsPossibleToBuy(person, productToBuy);
                    int currentPrice = productToBuy.Amount * product.Price;

                    if (currentPrice < bestPrice)
                    {
                        bestPrice = currentPrice;
                        bestShop = shop;
                    }
                }
                catch (ShopException)
                {
                    continue;
                }
            }

            if (bestShop is null)
            {
                throw new ShopException("There is no shop that contains this product");
            }

            bestShop.BuyProduct(person, productToBuy);
        }
    }
}
