using System.Collections.Generic;
using SweetBakery.DataAccess;
using SweetBakery.Models;

namespace SweetBakery.BusinessLogic
{
    public class CustomerService
    {
        public readonly ICustomerDL _repo;        
        public List<Customer> _customers;         

        public CustomerService(ICustomerDL repo)  
        {
            _repo = repo;
            _customers = _repo.LoadAll();        
        }

        public List<Customer> GetAll() => _customers;

        public (bool success, Customer customer, string error) Register(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, null, "Password cannot be empty.");

            int num = _customers.Count + 1;
            string id = num < 10 ? $"cust0{num}" : $"cust{num}";

            var customer = new Customer { Id = id, Password = password, Active = true };
            _customers.Add(customer);
            _repo.SaveAll(_customers);           

            return (true, customer, null);
        }
    }
}