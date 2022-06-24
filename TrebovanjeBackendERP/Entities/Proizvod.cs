using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TrebovanjeBackendERP.Entities
{
    [Table("Proizvod")]
    public partial class Proizvod
    {
        public Proizvod()
        {
            StavkaPorudzbines = new HashSet<StavkaPorudzbine>();
        }

        [Key]
        public int ProizvodId { get; set; }

        public string Naziv { get; set; }
        
        [NotMapped]
        public string KategorijaNaziv { get; set; }
        public int KategorijaId { get; set; }
        public float Cena { get; set; }
        public bool Dostupan { get; set; }

        public int DostupnaKolicina { get; set; }
        public int AdminId { get; set; }

        [ForeignKey(nameof(AdminId))]
        [InverseProperty(nameof(Korisnik.Proizvods))]
        public virtual Korisnik Admin { get; set; }
        [ForeignKey(nameof(KategorijaId))]
        [InverseProperty(nameof(KategorijaProizvodum.Proizvods))]
        public virtual KategorijaProizvodum Kategorija { get; set; }
        [InverseProperty(nameof(StavkaPorudzbine.Proizvod))]
        public virtual ICollection<StavkaPorudzbine> StavkaPorudzbines { get; set; }
    }
}
