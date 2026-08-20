using System.Collections.Generic;
using System.IO;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public class SaleFH : ISaleDL
    {
        private const string FilePath = "sales.txt";

        public List<Sale> LoadAll()
        {
            var list = new List<Sale>();
            if (!File.Exists(FilePath)) return list;
            foreach (var line in File.ReadAllLines(FilePath))
            {
                var p = line.Split(',');
                if (p.Length < 3) continue;
                list.Add(new Sale
                {
                    ProductName = p[0],
                    Quantity = int.Parse(p[1]),
                    TotalPrice = int.Parse(p[2])
                });
            }
            return list;
        }

        public void SaveAll(List<Sale> sales)
        {
            var lines = new List<string>();
            foreach (var s in sales)
                lines.Add($"{s.ProductName},{s.Quantity},{s.TotalPrice}");
            File.WriteAllLines(FilePath, lines);
        }
    }
}