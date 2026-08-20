using System;
using System.Collections.Generic;
using System.Collections.Generic;
using DataAccess;
using Models;
using SweetBakery.DataAccess;
using SweetBakery.Models;

namespace SweetBakery.BusinessLogic
{
    public class SalesService
    {
        public readonly ISaleDL _repo;
        public List<Sale> _sales;

        public SalesService(ISaleDL repo)
        {
            _repo = repo;
            _sales = _repo.LoadAll();
        }

        public IReadOnlyList<Sale> GetAll() => _sales.AsReadOnly();

        public int TotalRevenue()
        {
            int total = 0;
            foreach (var s in _sales) total += s.TotalPrice;
            return total;
        }

        public void RecordSales(List<CartItem> items, ProductService productService, List<Product> allProducts)
        {
            foreach (var item in items)
            {
                _sales.Add(new Sale
                {
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    TotalPrice = item.LineTotal
                });

                int idx = allProducts.IndexOf(item.Product);
                if (idx >= 0)
                    productService.ReduceStock(idx, item.Quantity);
            }

            Save();
        }

        public void Save() => _repo.SaveAll(_sales);
    }
}