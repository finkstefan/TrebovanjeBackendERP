using TrebovanjeBackendERP.Entities;
using System;
using System.Collections.Generic;

namespace TrebovanjeBackendERP.Repositories
{
    public interface IKorisnikRepository
    {
        List<Korisnik> GetKorisnici();

        Korisnik GetKorisnikById(int korisnikID);

        Korisnik GetKorisnikByUsernameAndPassword(string username, string password);

        Korisnik CreateKorisnik(Korisnik korisnik);

        void UpdateKorisnik(Korisnik korisnik);

        void DeleteKorisnik(int korisnikID);

        bool SaveChanges();
    }
}