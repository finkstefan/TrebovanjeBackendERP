using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TrebovanjeBackendERP.Entities
{
    public partial class KategorijaProizvodum
    {
        public KategorijaProizvodum()
        {
            Proizvods = new HashSet<Proizvod>();
        }

        [Key]
        public int KategorijaId { get; set; }
        [Required]
        [StringLength(100)]
        public string NazivKategorije { get; set; }

        [InverseProperty(nameof(Proizvod.Kategorija))]
        public virtual ICollection<Proizvod> Proizvods { get; set; }
    }
}
