namespace Shops.Models
{
    public class Product
    {
        public Product(string name, int price, int amount)
        {
            Name = name;
            Price = price;
            Amount = amount;
        }

        public Product(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }

        public string Name { get; }

        public int Price { get; set; }

        public int Amount { get; set; }
    }
}
