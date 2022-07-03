using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TrebovanjeBackendERP.Entities;

namespace TrebovanjeBackendERP.Repositories
{
    public class KorisnikRepository : IKorisnikRepository
    {




        private readonly TrebovanjeDatabaseContext context;

        private readonly IMapper mapper;

        public KorisnikRepository(TrebovanjeDatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }



        public Korisnik CreateKorisnik(Korisnik korisnik)
        {

           // lokacija.LokacijaId = Guid.NewGuid();
            var createdEntity = context.Add(korisnik);
            SaveChanges();
            return mapper.Map<Korisnik>(createdEntity.Entity);


        }

        public void DeleteKorisnik(int korisnikID)
        {
            var kor = GetKorisnikById(korisnikID);
            context.Remove(kor);
            SaveChanges();

        }

        public List<Korisnik> GetKorisnici()
        {
            return context.Korisniks.ToList();


        }

        public Korisnik GetKorisnikById(int korisnikID)
        {
            return context.Korisniks.FirstOrDefault(k => k.KorisnikId == korisnikID);


        }

        public int GetKorisnikIdByEmail(string email)
        {
            return (from k in context.Korisniks where k.Email == email select k.KorisnikId).FirstOrDefault();
        }

        public Korisnik GetKorisnikByUsernameAndPassword(string username,string password)
        {
            return context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == username && k.Lozinka==password);


        }

        public void UpdateKorisnik(Korisnik korisnik)
        {

        }

       
    }
}