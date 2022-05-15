using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TrebovanjeBackendERP.Entities;

namespace TrebovanjeBackendERP.Repositories
{
    public class AdminRepository : IAdminRepository
    {




        private readonly TrebovanjeDatabaseContext context;

        private readonly IMapper mapper;

        public AdminRepository(TrebovanjeDatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }



        public Admin CreateAdmin(Admin admin)
        {

            var createdEntity = context.Add(admin);
            SaveChanges();
            return mapper.Map<Admin>(createdEntity.Entity);


        }

        public void DeleteAdmin(int adminID)
        {
            var admin = GetAdminById(adminID);
            context.Remove(admin);
            SaveChanges();

        }

        public List<Admin> GetAdmins()
        {
            return context.Admins.ToList();


        }

        public Admin GetAdminById(int adminID)
        {
            return context.Admins.FirstOrDefault(a => a.KorisnikId == adminID);


        }

        public void UpdateAdmin(Admin admin)
        {

        }

       
    }
}