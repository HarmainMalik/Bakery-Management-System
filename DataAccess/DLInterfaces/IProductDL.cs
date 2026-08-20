using System.Collections.Generic;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public interface IProductDL
    {
        List<Product> LoadAll();
        void SaveAll(List<Product> products);
    }
}