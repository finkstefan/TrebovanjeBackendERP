using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TrebovanjeBackendERP.Entities;

namespace TrebovanjeBackendERP.Repositories
{
    public class ProizvodRepository : IProizvodRepository
    {

        private readonly TrebovanjeDatabaseContext context;

        private readonly IMapper mapper;

        public ProizvodRepository(TrebovanjeDatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }



        public Proizvod CreateProizvod(Proizvod proizvod)
        {

           // lokacija.LokacijaId = Guid.NewGuid();
            var createdEntity = context.Add(proizvod);
            SaveChanges();
            return mapper.Map<Proizvod>(createdEntity.Entity);


        }

        public void DeleteProizvod(int proizvodID)
        {
            var pr = GetProizvodById(proizvodID);
            context.Remove(pr);
            SaveChanges();

        }

        public List<Proizvod> GetProizvodi()
        {
            return context.Proizvods.ToList();


        }

        public Proizvod GetProizvodById(int proizvodID)
        {
            return context.Proizvods.FirstOrDefault(p => p.ProizvodId == proizvodID);


        }

        public void UpdateProizvod(Proizvod proizvod)
        {

        }

       
    }
}