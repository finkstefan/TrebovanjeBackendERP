using TrebovanjeBackendERP.Entities;
using System;
using System.Collections.Generic;

namespace TrebovanjeBackendERP.Repositories
{
    public interface IProizvodRepository
    {
        List<Proizvod> GetProizvodi();

        Proizvod GetProizvodById(int proizvodID);



        Proizvod CreateProizvod(Proizvod proizvod);

        List<Proizvod> GetProizvodsByCena(int cena);

        List<Proizvod> GetProizvodsByKategorija(int kategorija);

        List<Proizvod> GetProizvodsByPorudzbina(int porudzbinaId);

        List<Proizvod> GetProizvodsByNaziv(string naziv);

        List<Proizvod> GetProizvodsByNazivAndKategorijaSorted(string naziv,int? kategorija,int asc);

        int GetDostupnaKolicina(int proizvodId);
        void UpdateProizvod(Proizvod proizvod);

        void DeleteProizvod(int proizvodID);

        bool SaveChanges();
    }
}