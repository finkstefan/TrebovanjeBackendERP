using TrebovanjeBackendERP.Entities;
using System;
using System.Collections.Generic;

namespace TrebovanjeBackendERP.Repositories
{
    public interface IPorudzbinaRepository
    {
        List<Porudzbina> GetPorudzbine();

        Porudzbina GetPorudzbinaById(int porudzbinaID);

        List<Porudzbina> GetNeisplacenePorudzbine();

        List<Porudzbina> GetPorudzbineByDistributerId(int distributerId);

        Porudzbina CreatePorudzbina(Porudzbina porudzbina);

        

        void UpdatePorudzbina(Porudzbina porudzbina);

        void DeletePorudzbina(int porudzbinaID);

        bool SaveChanges();
    }
}