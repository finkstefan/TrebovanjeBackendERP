using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TrebovanjeBackendERP.Entities;

namespace TrebovanjeBackendERP.Repositories
{
    public class DistributerRepository : IDistributerRepository
    {




        private readonly TrebovanjeDatabaseContext context;

        private readonly IMapper mapper;

        public DistributerRepository(TrebovanjeDatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }



        public Distributer CreateDistributer(Distributer distributer)
        {

           // lokacija.LokacijaId = Guid.NewGuid();
            var createdEntity = context.Add(distributer);
            SaveChanges();
            return mapper.Map<Distributer>(createdEntity.Entity);


        }

        public void DeleteDistributer(int distributerID)
        {
            var dis = GetDistributerById(distributerID);
            context.Remove(dis);
            SaveChanges();

        }

        public List<Distributer> GetDistributers()
        {
            return context.Distributers.ToList();


        }

        public Distributer GetDistributerById(int distributerID)
        {
            return context.Distributers.FirstOrDefault(d => d.KorisnikId == distributerID);


        }

        public void UpdateDistributer(Distributer distributer)
        {

        }

       
    }
}