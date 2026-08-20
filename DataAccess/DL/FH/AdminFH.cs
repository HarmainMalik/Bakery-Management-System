using System.Collections.Generic;
using System.IO;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public class AdminFH : IAdminDL
    {
        private const string FilePath = "admins.txt";

        public List<Admin> LoadAll()
        {
            var list = new List<Admin>();
            if (!File.Exists(FilePath)) return list;
            foreach (var line in File.ReadAllLines(FilePath))
            {
                var p = line.Split(',');
                if (p.Length < 4) continue;
                list.Add(new Admin
                {
                    Id = p[0],
                    Password = p[1],
                    Approved = p[2] == "1",
                    Active = p[3] == "1"
                });
            }
            return list;
        }

        public void SaveAll(List<Admin> admins)
        {
            var lines = new List<string>();
            foreach (var a in admins)
                lines.Add($"{a.Id},{a.Password},{(a.Approved ? 1 : 0)},{(a.Active ? 1 : 0)}");
            File.WriteAllLines(FilePath, lines);
        }
    }
}