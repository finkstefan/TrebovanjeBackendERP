using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TrebovanjeBackendERP.Entities
{
    [Table("Lokacija")]
    public partial class Lokacija
    {
        [Key]
        public int LokacijaId { get; set; }
        [Required]
        [StringLength(100)]
        public string Grad { get; set; }
        [Required]
        [StringLength(100)]
        public string Drzava { get; set; }
        [Required]
        [StringLength(100)]
        public string Adresa { get; set; }
    }
}
