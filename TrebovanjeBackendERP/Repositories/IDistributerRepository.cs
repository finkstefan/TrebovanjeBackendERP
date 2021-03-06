using TrebovanjeBackendERP.Entities;
using System;
using System.Collections.Generic;

namespace TrebovanjeBackendERP.Repositories
{
    public interface IDistributerRepository
    {
        List<Distributer> GetDistributers();

        Distributer GetDistributerById(int distributerID);

        Distributer GetDistributerByNaziv(string naziv);

        Distributer GetDistributerByPib(string pib);

       
        Distributer CreateDistributer(Distributer distributer);

        void UpdateDistributer(Distributer distributer);

        void DeleteDistributer(int distributerID);

        bool SaveChanges();
    }
}