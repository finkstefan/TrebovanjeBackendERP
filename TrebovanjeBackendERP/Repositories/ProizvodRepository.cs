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

        private IStavkaPorudzbineRepository stavkaPorRepository;


        public ProizvodRepository(TrebovanjeDatabaseContext context, IMapper mapper,IStavkaPorudzbineRepository stavkaPorRepository )
        {
            this.context = context;
            this.mapper = mapper;
            this.stavkaPorRepository = stavkaPorRepository;
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

        public List<Proizvod> GetProizvodsByCena(int cena)
        {
            return (from p in context.Proizvods where p.Cena <= cena select p).ToList();


        }

        

        public List<Proizvod> GetProizvodsByKategorija(string kategorija)
        {
            return (from p in context.Proizvods where p.Kategorija.NazivKategorije == kategorija select p).ToList();


        }
        
        public List<Proizvod> GetProizvodsByPorudzbina(int porudzbinaId)
        {
            List<int> proizvodiIDs = stavkaPorRepository.GetProizvodiIzPorudzbine(porudzbinaId);

            List<Proizvod> proizvodi = new List<Proizvod>();

            foreach (int prId in proizvodiIDs)
            {
                proizvodi.Add((from p in context.Proizvods where p.ProizvodId == prId select p).FirstOrDefault());
            }

            return proizvodi;
        }
        

        public void UpdateProizvod(Proizvod proizvod)
        {

        }

       
    }
}