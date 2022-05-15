using TrebovanjeBackendERP.Entities;
using System;
using System.Collections.Generic;

namespace TrebovanjeBackendERP.Repositories
{
    public interface IStavkaPorudzbineRepository
    {
        List<StavkaPorudzbine> GetStavkePorudzbine();

        StavkaPorudzbine GetStavkaPorudzbineById(int stavkaPorID);

      

        StavkaPorudzbine CreateStavkaPorudzbine(StavkaPorudzbine sp);

        void UpdateStavkaPorudzbine(StavkaPorudzbine sp);

        void DeleteStavkaPorudzbine(int stavkaPorID);

        bool SaveChanges();
    }
}