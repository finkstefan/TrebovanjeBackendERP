using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TrebovanjeBackendERP.Entities;

namespace TrebovanjeBackendERP.Repositories
{
    public class StavkaPorudzbineRepository : IStavkaPorudzbineRepository
    {




        private readonly TrebovanjeDatabaseContext context;

        private readonly IMapper mapper;

        public StavkaPorudzbineRepository(TrebovanjeDatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }



        public StavkaPorudzbine CreateStavkaPorudzbine(StavkaPorudzbine sp)
        {

            // lokacija.LokacijaId = Guid.NewGuid();
            var createdEntity = context.Add(sp);
            SaveChanges();
            return mapper.Map<StavkaPorudzbine>(createdEntity.Entity);


        }

        public void DeleteStavkaPorudzbine(int stavkaPorID)
        {
            var sp = GetStavkaPorudzbineById(stavkaPorID);
            context.Remove(sp);
            SaveChanges();

        }

        public List<StavkaPorudzbine> GetStavkePorudzbine()
        {
            return context.StavkaPorudzbines.ToList();


        }

        public StavkaPorudzbine GetStavkaPorudzbineById(int stavkaPorID)
        {
            return context.StavkaPorudzbines.FirstOrDefault(sp => sp.StavkaPorudzbineId == stavkaPorID);


        }

        public void UpdateStavkaPorudzbine(StavkaPorudzbine sp)
        {

        }


    }
}