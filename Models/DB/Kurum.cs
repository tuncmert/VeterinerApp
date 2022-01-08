using System;
using System.Collections.Generic;

#nullable disable

namespace Veterinerapp.Models.DB
{
    public partial class Kurum
    {
        public Kurum()
        {
            Veteriners = new HashSet<Veteriner>();
        }

        public int KurumId { get; set; }
        public string KurumAd { get; set; }
        public int? KurumTelefon { get; set; }
        public string KurumSehir { get; set; }
        public string KurumIlce { get; set; }
        public byte[] KurumLogo { get; set; }

        public virtual ICollection<Veteriner> Veteriners { get; set; }
    }
}
