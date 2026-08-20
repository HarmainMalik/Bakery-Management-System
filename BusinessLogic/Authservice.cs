using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using Models;
using SweetBakery.Models;

namespace SweetBakery.BusinessLogic
{
    // All login validation lives here. Forms just call these methods.
    public class AuthService
    {
        public readonly Owner _owner = new Owner();

        public bool ValidateOwner(int id, string password)
        {
            return id == _owner.Id && password == _owner.Password;
        }

        // Returns the matched admin, or null on failure.
        public Admin ValidateAdmin(string id, string password, List<Admin> allAdmins)
        {
            return allAdmins.FirstOrDefault(a =>
                a.Id == id &&
                a.Password == password &&
                a.Approved &&
                a.Active);
        }

        // Returns the matched customer, or null on failure.
        public Customer ValidateCustomer(string id, string password, List<Customer> allCustomers)
        {
            return allCustomers.FirstOrDefault(c =>
                c.Id == id &&
                c.Password == password &&
                c.Active);
        }
    }
}