using System;
using System.Collections.Generic;
using Banks.Interfaces;

namespace Banks.Models
{
    public class Customer : ICustomer
    {
        internal Customer() { }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Passport { get; set; }

        public List<string> Notifications { get; } = new ();

        public List<Guid> AccountIds { get; set; } = new ();

        public bool HasName => !(string.IsNullOrEmpty(FirstName) ||
                                 string.IsNullOrEmpty(LastName));

        public bool IsVerified => !(string.IsNullOrEmpty(Address) ||
                                    string.IsNullOrEmpty(Passport));

        public void Update(string message)
        {
            Notifications.Add(message);
        }

        public void ClearNotifications()
        {
            Notifications.Clear();
        }
    }
}