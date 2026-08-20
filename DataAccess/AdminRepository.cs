using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public class AdminRepository : IAdminDL
    {
        public List<Admin> LoadAll()
        {
            var admins = new List<Admin>();
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM admins", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    admins.Add(new Admin
                    {
                        Id = reader["id"].ToString(),
                        Password = reader["password"].ToString(),
                        Approved = Convert.ToInt32(reader["approved"]) == 1,
                        Active = Convert.ToInt32(reader["active"]) == 1
                    });
                }
            }
            return admins;
        }

        public void SaveAll(List<Admin> admins)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                new MySqlCommand("DELETE FROM admins", conn).ExecuteNonQuery();
                foreach (var a in admins)
                {
                    var cmd = new MySqlCommand(
                        "INSERT INTO admins (id,password,approved,active) " +
                        "VALUES (@i,@p,@ap,@ac)", conn);
                    cmd.Parameters.AddWithValue("@i", a.Id);
                    cmd.Parameters.AddWithValue("@p", a.Password);
                    cmd.Parameters.AddWithValue("@ap", a.Approved ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ac", a.Active ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}