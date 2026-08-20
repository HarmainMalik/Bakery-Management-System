using System.Collections.Generic;
using System.IO;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public class ProductFH : IProductDL
    {
        private const string FilePath = "products.txt";

        public List<Product> LoadAll()
        {
            var list = new List<Product>();
            if (!File.Exists(FilePath)) return list;
            foreach (var line in File.ReadAllLines(FilePath))
            {
                var p = line.Split(',');
                if (p.Length < 6) continue;
                list.Add(new Product
                {
                    Name = p[0],
                    Stock = int.Parse(p[1]),
                    Price = int.Parse(p[2]),
                    ExpDay = int.Parse(p[3]),
                    ExpMonth = int.Parse(p[4]),
                    ExpYear = int.Parse(p[5])
                });
            }
            return list;
        }

        public void SaveAll(List<Product> products)
        {
            var lines = new List<string>();
            foreach (var p in products)
                lines.Add($"{p.Name},{p.Stock},{p.Price},{p.ExpDay},{p.ExpMonth},{p.ExpYear}");
            File.WriteAllLines(FilePath, lines);
        }
    }
}