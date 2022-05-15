using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#nullable disable

namespace TrebovanjeBackendERP.Entities
{
    [Table("Distributer")]
    public partial class Distributer
    {
        public Distributer()
        {
            Porudzbinas = new HashSet<Porudzbina>();
        }

        [Key]
        public int KorisnikId { get; set; }
        [Required]
        [StringLength(100)]
        public string NazivDistributera { get; set; }
        [Required]
        [Column("PIB")]
        [StringLength(100)]
        public string Pib { get; set; }
        public bool NaCrnojListi { get; set; }
        public int LokacijaId { get; set; }

        [ForeignKey(nameof(KorisnikId))]
        [InverseProperty("Distributer")]
        
        public virtual Korisnik Korisnik { get; set; }
        [InverseProperty(nameof(Porudzbina.Distributer))]
        public virtual ICollection<Porudzbina> Porudzbinas { get; set; }
    }
}
