using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TrebovanjeBackendERP.Entities
{
    [Table("Porudzbina")]
    public partial class Porudzbina
    {
        public Porudzbina()
        {
            StavkaPorudzbines = new HashSet<StavkaPorudzbine>();
        }

        [Key]
        public int PorudzbinaId { get; set; }
        [Column(TypeName = "date")]
        public DateTime Datum { get; set; }
        public int DistributerId { get; set; }
        public bool Isplacena { get; set; }

        [NotMapped]
        public float Iznos { get; set; }

        [ForeignKey(nameof(DistributerId))]
        [InverseProperty("Porudzbinas")]
        public virtual Distributer Distributer { get; set; }
        [InverseProperty(nameof(StavkaPorudzbine.Porudzbina))]
        public virtual ICollection<StavkaPorudzbine> StavkaPorudzbines { get; set; }
    }
}
