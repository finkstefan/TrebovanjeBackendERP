using TrebovanjeBackendERP.Entities;
using System;
using System.Collections.Generic;

namespace TrebovanjeBackendERP.Repositories
{
    public interface IAdminRepository
    {
        List<Admin> GetAdmins();

        Admin GetAdminById(int adminID);

      

        Admin CreateAdmin(Admin admin);

        void UpdateAdmin(Admin admin);

        void DeleteAdmin(int adminID);

        bool SaveChanges();
    }
}