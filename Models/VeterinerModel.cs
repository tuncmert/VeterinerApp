using System.Collections.Generic;
using Veterinerapp.Models.DB;

namespace Veterinerapp.Models
{
    public class VeterinerModel
    {
        public Veteriner SeciliVeteriner { get; set; }
        public List<PetSahip> TumSahipler { get; set; }
    }
}