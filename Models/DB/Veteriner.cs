using System;
using System.Collections.Generic;

#nullable disable

namespace Veterinerapp.Models.DB
{
    public partial class Veteriner
    {
        public Veteriner()
        {
            Asis = new HashSet<Asi>();
            Pets = new HashSet<Pet>();
            Tedavis = new HashSet<Tedavi>();
        }

        public int VeterinerId { get; set; }
        public string VeterinerAd { get; set; }
        public string VeterinerSoyad { get; set; }
        public string VeterinerTelefon { get; set; }
        public bool VeterinerAktifMi { get; set; }
        public string VeterinerKullaniciAdi { get; set; }
        public string VeterinerSifre { get; set; }
        public int KurumId { get; set; }
        public byte[] VeterinerImage { get; set; }

        public virtual Kurum Kurum { get; set; }
        public virtual ICollection<Asi> Asis { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<Tedavi> Tedavis { get; set; }
    }
}
