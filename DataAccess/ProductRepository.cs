using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public class ProductRepository : IProductDL
    {
        public List<Product> LoadAll()
        {
            var products = new List<Product>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM products", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Name = reader["name"].ToString(),
                        Stock = (int)reader["stock"],
                        Price = (int)reader["price"],
                        ExpDay = (int)reader["exp_day"],
                        ExpMonth = (int)reader["exp_month"],
                        ExpYear = (int)reader["exp_year"]
                    });
                }
            }
            return products;
        }

        public void SaveAll(List<Product> products)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                var clear = new MySqlCommand("DELETE FROM products", conn);
                clear.ExecuteNonQuery();
                foreach (var p in products)
                {
                    var cmd = new MySqlCommand(
                        "INSERT INTO products (name,stock,price,exp_day,exp_month,exp_year) " +
                        "VALUES (@n,@s,@p,@d,@m,@y)", conn);
                    cmd.Parameters.AddWithValue("@n", p.Name);
                    cmd.Parameters.AddWithValue("@s", p.Stock);
                    cmd.Parameters.AddWithValue("@p", p.Price);
                    cmd.Parameters.AddWithValue("@d", p.ExpDay);
                    cmd.Parameters.AddWithValue("@m", p.ExpMonth);
                    cmd.Parameters.AddWithValue("@y", p.ExpYear);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}