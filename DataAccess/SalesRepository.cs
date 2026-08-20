using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public class SalesRepository : ISaleDL
    {
        public List<Sale> LoadAll()
        {
            var sales = new List<Sale>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM sales", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sales.Add(new Sale
                    {
                        ProductName = reader["product_name"].ToString(),
                        Quantity = (int)reader["quantity"],
                        TotalPrice = (int)reader["total_price"]
                    });
                }
            }
            return sales;
        }

        public void SaveAll(List<Sale> sales)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                new MySqlCommand("DELETE FROM sales", conn).ExecuteNonQuery();
                foreach (var s in sales)
                {
                    var cmd = new MySqlCommand(
                        "INSERT INTO sales (product_name,quantity,total_price) " +
                        "VALUES (@n,@q,@t)", conn);
                    cmd.Parameters.AddWithValue("@n", s.ProductName);
                    cmd.Parameters.AddWithValue("@q", s.Quantity);
                    cmd.Parameters.AddWithValue("@t", s.TotalPrice);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}