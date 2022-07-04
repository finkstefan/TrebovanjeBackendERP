using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TrebovanjeBackendERP.Entities
{

    public class PorudzbinaCreate
    {
       
       
        public int PorudzbinaId { get; set; }
       
        public DateTime Datum { get; set; }
        public string DistributerEmail { get; set; }
        public bool Isplacena { get; set; }

        [NotMapped]
        public float Iznos { get; set; }

   
    }
}
