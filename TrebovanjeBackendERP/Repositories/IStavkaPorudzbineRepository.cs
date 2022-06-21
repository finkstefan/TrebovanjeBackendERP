using TrebovanjeBackendERP.Entities;
using System;
using System.Collections.Generic;

namespace TrebovanjeBackendERP.Repositories
{
    public interface IStavkaPorudzbineRepository
    {
        List<StavkaPorudzbine> GetStavkePorudzbine();

        StavkaPorudzbine GetStavkaPorudzbineById(int stavkaPorID);

        List<int> GetProizvodiIzPorudzbine(int porudzbinaId);

        List<StavkaPorudzbine> GetStavkeByPorudzbinaId(int porudzbinaId);

        StavkaPorudzbine CreateStavkaPorudzbine(StavkaPorudzbine sp);

        void UpdateStavkaPorudzbine(StavkaPorudzbine sp);

        void DeleteStavkaPorudzbine(int stavkaPorID);

        bool SaveChanges();
    }
}