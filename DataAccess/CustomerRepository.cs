using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    
    public class CustomerRepository : ICustomerDL
    {
       
        public List<Customer> LoadAll()
        {
            var customers = new List<Customer>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM customers", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        Id = reader["id"].ToString(),
                        Password = reader["password"].ToString(),
                        Active = Convert.ToInt32(reader["active"]) == 1
                    });
                }
            }
            return customers;
        }

        public void SaveAll(List<Customer> customers)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                new MySqlCommand("DELETE FROM customers", conn).ExecuteNonQuery();
                foreach (var c in customers)
                {
                    var cmd = new MySqlCommand(
                        "INSERT INTO customers (id,password,active) " +
                        "VALUES (@i,@p,@a)", conn);
                    cmd.Parameters.AddWithValue("@i", c.Id);
                    cmd.Parameters.AddWithValue("@p", c.Password);
                    cmd.Parameters.AddWithValue("@a", c.Active ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}