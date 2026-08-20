using System.Collections.Generic;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public interface ICustomerDL
    {
        List<Customer> LoadAll();
        void SaveAll(List<Customer> customers);
    }
}