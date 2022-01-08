using System;
using System.Collections.Generic;

#nullable disable

namespace Veterinerapp.Models.DB
{
    public partial class Asi
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Detay { get; set; }
        public int PetId { get; set; }
        public int VeterinerId { get; set; }
        public DateTime? Tarih { get; set; }

        public virtual Pet Pet { get; set; }
        public virtual Veteriner Veteriner { get; set; }
    }
}
