using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TrebovanjeBackendERP.Entities;

namespace TrebovanjeBackendERP.Repositories
{
    public class KategorijaProizvodaRepository : IKategorijaProizvodaRepository
    {




        private readonly TrebovanjeDatabaseContext context;

        private readonly IMapper mapper;

        public KategorijaProizvodaRepository(TrebovanjeDatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }



        public KategorijaProizvodum CreateKategorija(KategorijaProizvodum kategorija)
        {

            var createdEntity = context.Add(kategorija);
            SaveChanges();
            return mapper.Map<KategorijaProizvodum>(createdEntity.Entity);


        }

        public void DeleteKategorija(int kategorijaID)
        {
            var kategorija = GetKategorijaById(kategorijaID);
            context.Remove(kategorija);
            SaveChanges();

        }

        public List<KategorijaProizvodum> GetKategorije()
        {
            return context.KategorijaProizvodums.ToList();



        }

        public KategorijaProizvodum GetKategorijaById(int kategorijaID)
        {
            return context.KategorijaProizvodums.FirstOrDefault(k => k.KategorijaId == kategorijaID);


        }

        public void UpdateKategorija(KategorijaProizvodum kategorija)
        {

        }

       
    }
}