using System.Collections.Generic;
using System.IO;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public class CustomerFH : ICustomerDL
    {
        private const string FilePath = "customers.txt";

        public List<Customer> LoadAll()
        {
            var list = new List<Customer>();
            if (!File.Exists(FilePath)) return list;
            foreach (var line in File.ReadAllLines(FilePath))
            {
                var p = line.Split(',');
                if (p.Length < 3) continue;
                list.Add(new Customer { Id = p[0], Password = p[1], Active = p[2] == "1" });
            }
            return list;
        }

        public void SaveAll(List<Customer> customers)
        {
            var lines = new List<string>();
            foreach (var c in customers)
                lines.Add($"{c.Id},{c.Password},{(c.Active ? 1 : 0)}");
            File.WriteAllLines(FilePath, lines);
        }
    }
}