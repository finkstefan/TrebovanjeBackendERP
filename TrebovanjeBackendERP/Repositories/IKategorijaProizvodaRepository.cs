using TrebovanjeBackendERP.Entities;
using System;
using System.Collections.Generic;

namespace TrebovanjeBackendERP.Repositories
{
    public interface IKategorijaProizvodaRepository
    {
        List<KategorijaProizvodum> GetKategorije();

        KategorijaProizvodum GetKategorijaById(int kategorijaID);

      

        KategorijaProizvodum CreateKategorija(KategorijaProizvodum kategorija);

        void UpdateKategorija(KategorijaProizvodum kategorija);

        void DeleteKategorija(int kategorijaID);

        bool SaveChanges();
    }
}