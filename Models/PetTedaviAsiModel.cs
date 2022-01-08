using System.Collections.Generic;
using Veterinerapp.Models.DB;

namespace Veterinerapp.Models
{
    public class PetTedaviAsiModel
    {
        public List<Tedavi> Tedaviler { get; internal set; }
        public List<Asi> Asilar { get; internal set; }
        public Pet SeciliPet { get; internal set; }
    }
}