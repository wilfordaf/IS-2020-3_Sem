using Banks.Models;
using Banks.Tools;

namespace Banks.Builders
{
    public class CustomerBuilder
    {
        private Customer _customer;

        public CustomerBuilder()
        {
            _customer = new Customer();
        }

        public CustomerBuilder SetName(string firstName, string lastName)
        {
            _customer.FirstName = firstName;
            _customer.LastName = lastName;
            return this;
        }

        public CustomerBuilder SetAddress(string address)
        {
            _customer.Address = address;
            return this;
        }

        public CustomerBuilder SetPassport(string passport)
        {
            _customer.Passport = passport;
            return this;
        }

        public Customer Build()
        {
            if (_customer.HasName)
            {
                return _customer;
            }

            Refresh();
            throw new BanksException("Unable to create customer without name.");
        }

        private void Refresh()
        {
            _customer = null;
        }
    }
}