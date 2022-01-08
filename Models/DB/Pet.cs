using System;
using System.Collections.Generic;

#nullable disable

namespace Veterinerapp.Models.DB
{
    public partial class Pet
    {
        public Pet()
        {
            Asis = new HashSet<Asi>();
            Nots = new HashSet<Not>();
            Tedavis = new HashSet<Tedavi>();
        }

        public int Id { get; set; }
        public string Ad { get; set; }
        public string Yas { get; set; }
        public string Cins { get; set; }
        public string Tur { get; set; }
        public string Kilo { get; set; }
        public byte[] Foto { get; set; }
        public int? VeterinerId { get; set; }
        public int PetSahipId { get; set; }
        public string PetTc { get; set; }
        public string Cinsiyet { get; set; }
        public DateTime? DogumTarihi { get; set; }

        public virtual PetSahip PetSahip { get; set; }
        public virtual Veteriner Veteriner { get; set; }
        public virtual ICollection<Asi> Asis { get; set; }
        public virtual ICollection<Not> Nots { get; set; }
        public virtual ICollection<Tedavi> Tedavis { get; set; }
    }
}
