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

        void UpdateProizvod(Proizvod proizvod);

        void DeleteProizvod(int proizvodID);

        bool SaveChanges();
    }
}