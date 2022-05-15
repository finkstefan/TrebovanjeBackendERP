using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TrebovanjeBackendERP.Entities
{
    [Table("Korisnik")]
    public partial class Korisnik
    {
        public Korisnik()
        {
            Proizvods = new HashSet<Proizvod>();
        }

        [Key]
        public int KorisnikId { get; set; }
        [Required]
        [StringLength(100)]
        public string KorisnickoIme { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Lozinka { get; set; }
        [Required]
        [StringLength(50)]
        public string BrojTelefona { get; set; }
        public bool Admin { get; set; }

        [NotMapped]
        public string Role { get; set; }

        [InverseProperty("Korisnik")]
        public virtual Admin AdminNavigation { get; set; }
        [InverseProperty("Korisnik")]
        public virtual Distributer Distributer { get; set; }
        [InverseProperty(nameof(Proizvod.Admin))]
        public virtual ICollection<Proizvod> Proizvods { get; set; }
    }
}
