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

        List<Proizvod> GetProizvodsByKategorija(string kategorija);

        List<Proizvod> GetProizvodsByPorudzbina(int porudzbinaId);
        void UpdateProizvod(Proizvod proizvod);

        void DeleteProizvod(int proizvodID);

        bool SaveChanges();
    }
}