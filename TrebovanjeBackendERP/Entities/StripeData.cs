using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TrebovanjeBackendERP.Entities
{
    public class StripeData
    {
        public float amount{ get; set; }
        public int id { get; set; }
    }
}
