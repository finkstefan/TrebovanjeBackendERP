using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;


#nullable disable

namespace TrebovanjeBackendERP.Entities
{
    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        public int KorisnikId { get; set; }
        [Required]
        [StringLength(100)]
        public string Ime { get; set; }
        [Required]
        [StringLength(100)]
        public string Prezime { get; set; }

        [ForeignKey(nameof(KorisnikId))]
       
        [InverseProperty("AdminNavigation")]
       
        public virtual Korisnik Korisnik { get; set; }
    }
}
