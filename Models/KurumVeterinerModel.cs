using System.Collections.Generic;
using Veterinerapp.Models.DB;

namespace Veterinerapp.Models
{
    public class KurumVeterinerListesiModel
    {
        public List<Veteriner> TumVeterinerler { get; internal set; }
        public Kurum SeciliKurum { get; internal set; }
    }
}