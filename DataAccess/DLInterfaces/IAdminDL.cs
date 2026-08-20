using System.Collections.Generic;
using SweetBakery.Models;

namespace SweetBakery.DataAccess
{
    public interface IAdminDL
    {
        List<Admin> LoadAll();
        void SaveAll(List<Admin> admins);
    }
}