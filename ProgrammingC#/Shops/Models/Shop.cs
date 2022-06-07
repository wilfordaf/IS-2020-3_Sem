using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Models
{
    public class Shop
    {
        private readonly List<Product> _products = new List<Product>();
        private readonly string _name;
        private readonly string _address;

        public Shop(string name, string address)
        {
            _name = name;
            _address = address;
            Id = (name + address).GetHashCode();
        }

        public int Id { get; }

        public Product FindProduct(string productName)
        {
            return _products.FirstOrDefault(p => p.Name == productName);
        }

        public void AddProduct(Product newProduct)
        {
            if (newProduct is null)
            {
                return;
            }

            Product product = FindProduct(newProduct.Name);
            if (product is not null)
            {
                if (product.Amount == 0)
                {
                    product.Price = newProduct.Price;
                }

                product.Amount += newProduct.Amount;
                return;
            }

            _products.Add(newProduct);
        }

        public Product IsPossibleToBuy(Person person, Product product)
        {
            Product productToBuy = FindProduct(product.Name);

            if (productToBuy is null)
            {
                throw new ShopException("There is no such product in the shop");
            }

            if (productToBuy.Amount < product.Amount)
            {
                throw new ShopException("Insufficient quantity of product");
            }

            int productToBuyPrice = product.Amount * productToBuy.Price;

            if (productToBuyPrice > person.Money)
            {
                throw new ShopException("Insufficient funds");
            }

            return productToBuy;
        }

        public void BuyProduct(Person person, Product product)
        {
            Product productToBuy = IsPossibleToBuy(person, product);

            productToBuy.Amount -= product.Amount;

            int productToBuyPrice = product.Amount * productToBuy.Price;
            person.Money -= productToBuyPrice;
        }

        public void BuyProducts(Person person, List<Product> products)
        {
            if (products is null)
            {
                return;
            }

            int totalPrice = 0;
            products.ForEach(p =>
            {
                Product productToBuy = IsPossibleToBuy(person, p);
                totalPrice += productToBuy.Price * p.Amount;
            });

            if (totalPrice > person.Money)
            {
                throw new ShopException("Insufficient funds");
            }

            products.ForEach(p => BuyProduct(person, p));
        }

        public void ChangePrice(string productName, int newPrice)
        {
            Product productChanged = FindProduct(productName);
            if (productChanged is null)
            {
                throw new ShopException("Product with this name is not found");
            }

            productChanged.Price = newPrice;
        }
    }
}
