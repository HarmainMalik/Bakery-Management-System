using System.Collections.Generic;
using System.Linq;
using DataAccess;
using Models;
using SweetBakery.DataAccess;
using SweetBakery.Models;


namespace SweetBakery.BusinessLogic
{
    public class AdminService
    {
        public readonly IAdminDL _repo;
        public List<Admin> _admins;

        public AdminService(IAdminDL repo)
        {
            _repo = repo;
            _admins = _repo.LoadAll();
        }

       
        public IReadOnlyList<Admin> GetApproved() =>
            _admins.Where(a => a.Approved && a.Active).ToList().AsReadOnly();

        
        public IReadOnlyList<Admin> GetPending() =>
            _admins.Where(a => !a.Approved).ToList().AsReadOnly();

        public List<Admin> GetAll() => _admins;

        
        public (bool success, string newId, string error) RegisterRequest(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, null, "Password cannot be empty.");

           
            int num = _admins.Count + 1;
            string newId = num < 10 ? $"admin0{num}" : $"admin{num}";

            _admins.Add(new Admin
            {
                Id = newId,
                Password = password,
                Approved = false,
                Active = false
            });

            Save();
            return (true, newId, null);
        }

        public (bool success, string error) ApproveAdmin(string adminId)
        {
            var admin = _admins.FirstOrDefault(a => a.Id == adminId && !a.Approved);
            if (admin == null) return (false, "Admin request not found.");

            admin.Approved = true;
            admin.Active = true;
            Save();
            return (true, null);
        }

        public (bool success, string error) DenyAdmin(string adminId)
        {
            var admin = _admins.FirstOrDefault(a => a.Id == adminId && !a.Approved);
            if (admin == null) return (false, "Admin request not found.");

            _admins.Remove(admin);
            Save();
            return (true, null);
        }

        public void Save() => _repo.SaveAll(_admins);
    }
}