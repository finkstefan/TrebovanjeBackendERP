using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TrebovanjeBackendERP.Entities;

namespace TrebovanjeBackendERP.Repositories
{
    public class PorudzbinaRepository : IPorudzbinaRepository
    {




        private readonly TrebovanjeDatabaseContext context;

        private readonly IMapper mapper;

        public PorudzbinaRepository(TrebovanjeDatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }



        public Porudzbina CreatePorudzbina(Porudzbina porudzbina)
        {

           // lokacija.LokacijaId = Guid.NewGuid();
            var createdEntity = context.Add(porudzbina);
            SaveChanges();
            return mapper.Map<Porudzbina>(createdEntity.Entity);


        }

        public void DeletePorudzbina(int porudzbinaID)
        {
            var por = GetPorudzbinaById(porudzbinaID);
            context.Remove(por);
            SaveChanges();

        }

        public List<Porudzbina> GetPorudzbine()
        {
            return context.Porudzbinas.ToList().OrderByDescending(o=>o.Datum).ToList();


        }

        public List<Porudzbina> GetNeisplacenePorudzbine()
        {
            return (from p in context.Porudzbinas where p.Isplacena == false select p).ToList();


        }

        public List<Porudzbina> GetPorudzbineByDistributerId(int distributerId)
        {
            return (from p in context.Porudzbinas where p.DistributerId==distributerId select p).ToList();


        }

        public Porudzbina GetPorudzbinaById(int porudzbinaID)
        {
            return context.Porudzbinas.FirstOrDefault(p => p.PorudzbinaId == porudzbinaID);


        }

        public void UpdatePorudzbina(Porudzbina porudzbina)
        {
            Porudzbina oldPor = GetPorudzbinaById(porudzbina.PorudzbinaId);

            oldPor.DistributerId = porudzbina.DistributerId;
            oldPor.Datum = porudzbina.Datum;
            oldPor.Iznos = porudzbina.Iznos;
            oldPor.Isplacena = porudzbina.Isplacena;

            SaveChanges();
        }

       
    }
}