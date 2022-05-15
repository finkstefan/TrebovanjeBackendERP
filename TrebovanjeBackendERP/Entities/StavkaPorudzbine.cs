using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TrebovanjeBackendERP.Entities
{
    [Table("StavkaPorudzbine")]
    public partial class StavkaPorudzbine
    {
        [Key]
        public int StavkaPorudzbineId { get; set; }
        public int ProizvodId { get; set; }
        public int PorudzbinaId { get; set; }
        public int Kolicina { get; set; }

        [ForeignKey(nameof(PorudzbinaId))]
        [InverseProperty("StavkaPorudzbines")]
        public virtual Porudzbina Porudzbina { get; set; }
        [ForeignKey(nameof(ProizvodId))]
        [InverseProperty("StavkaPorudzbines")]
        public virtual Proizvod Proizvod { get; set; }
    }
}
