using TrebovanjeBackendERP.Entities;
using System;
using System.Collections.Generic;

namespace TrebovanjeBackendERP.Repositories
{
    public interface ILokacijaRepository
    {
        List<Lokacija> GetLokacije();

        Lokacija GetLokacijaById(int lokacijaID);

      

        Lokacija CreateLokacija(Lokacija lokacija);

        void UpdateLokacija(Lokacija lokacija);

        void DeleteLokacija(int lokacijaID);

        bool SaveChanges();
    }
}