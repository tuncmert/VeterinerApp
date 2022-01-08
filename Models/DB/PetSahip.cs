using System;
using System.Collections.Generic;

#nullable disable

namespace Veterinerapp.Models.DB
{
    public partial class PetSahip
    {
        public PetSahip()
        {
            Nots = new HashSet<Not>();
            Pets = new HashSet<Pet>();
        }

        public int Id { get; set; }
        public string SahipAd { get; set; }
        public string SahipSoyad { get; set; }
        public string SahipTelefon { get; set; }
        public string SahipSifre { get; set; }
        public string KullaniciAdi { get; set; }

        public virtual ICollection<Not> Nots { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
    }
}
