namespace Shops.Models
{
    public class Person
    {
        private readonly string _name;

        public Person(string name, int money)
        {
            _name = name;
            Money = money;
        }

        public int Money { get; set; }
    }
}
