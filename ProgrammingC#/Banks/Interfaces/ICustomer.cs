using System;
using System.Collections.Generic;

namespace Banks.Interfaces
{
    public interface ICustomer
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string Address { get; set; }

        string Passport { get; set; }

        List<Guid> AccountIds { get; set; }

        public List<string> Notifications { get; }

        bool HasName { get; }

        bool IsVerified { get; }

        void Update(string message);

        public void ClearNotifications();
    }
}