using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TrebovanjeBackendERP.Entities;

namespace TrebovanjeBackendERP.Repositories
{
    public class LokacijaRepository : ILokacijaRepository
    {




        private readonly TrebovanjeDatabaseContext context;

        private readonly IMapper mapper;

        public LokacijaRepository(TrebovanjeDatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }



        public Lokacija CreateLokacija(Lokacija lokacija)
        {

           // lokacija.LokacijaId = Guid.NewGuid();
            var createdEntity = context.Add(lokacija);
            SaveChanges();
            return mapper.Map<Lokacija>(createdEntity.Entity);


        }

        public void DeleteLokacija(int lokacijaID)
        {
            var lok = GetLokacijaById(lokacijaID);
            context.Remove(lok);
            SaveChanges();

        }

        public List<Lokacija> GetLokacije()
        {
            return context.Lokacijas.ToList();


        }

        public Lokacija GetLokacijaById(int lokacijaID)
        {
            return context.Lokacijas.FirstOrDefault(l => l.LokacijaId == lokacijaID);


        }

        public void UpdateLokacija(Lokacija lokacija)
        {

        }

       
    }
}