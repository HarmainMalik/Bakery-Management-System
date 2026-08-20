using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using Models;
using SweetBakery.DataAccess;
using SweetBakery.Models;

namespace SweetBakery.BusinessLogic
{
    public class ProductService
    {
        public readonly IProductDL _repo;
        public List<Product> _products;

        public ProductService(IProductDL repo)
        {
            _repo = repo;
            _products = _repo.LoadAll();
        }

        public IReadOnlyList<Product> GetAll() => _products.AsReadOnly();

        public List<Product> GetLowStock() =>
            _products.Where(p => p.IsLowStock).ToList();

        public List<Product> GetExpiringTomorrow()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1);
            return _products.Where(p => p.ExpiresOn(tomorrow)).ToList();
        }

        public (bool success, string error) AddProduct(string name, int stock, int price,
            int expDay, int expMonth, int expYear)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Product name cannot be empty.");
            if (stock < 0)
                return (false, "Stock cannot be negative.");
            if (price <= 0)
                return (false, "Price must be greater than zero.");

            try
            {
                var expiry = new DateTime(expYear, expMonth, expDay);
                if (expiry < DateTime.Today)
                    return (false, "Expiry date cannot be in the past.");
            }
            catch
            {
                return (false, "Invalid expiry date.");
            }

            _products.Add(new Product
            {
                Name = name.Trim(),
                Stock = stock,
                Price = price,
                ExpDay = expDay,
                ExpMonth = expMonth,
                ExpYear = expYear
            });

            Save();
            return (true, null);
        }

        public (bool success, string error) DeleteProduct(int index)
        {
            if (index < 0 || index >= _products.Count)
                return (false, "Invalid product selection.");

            _products.RemoveAt(index);
            Save();
            return (true, null);
        }

        public (bool success, string error) UpdateStock(int index, int addQuantity)
        {
            if (index < 0 || index >= _products.Count)
                return (false, "Invalid product selection.");
            if (addQuantity < 0)
                return (false, "Quantity to add cannot be negative.");

            _products[index].Stock += addQuantity;
            Save();
            return (true, null);
        }

        public (bool success, string error) UpdatePrice(int index, int newPrice)
        {
            if (index < 0 || index >= _products.Count)
                return (false, "Invalid product selection.");
            if (newPrice <= 0)
                return (false, "Price must be greater than zero.");

            _products[index].Price = newPrice;
            Save();
            return (true, null);
        }

        public void ReduceStock(int index, int quantity)
        {
            if (index >= 0 && index < _products.Count)
            {
                _products[index].Stock -= quantity;
                Save();
            }
        }

        public void Save() => _repo.SaveAll(_products);
    }
}
