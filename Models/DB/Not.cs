using System;
using System.Collections.Generic;

#nullable disable

namespace Veterinerapp.Models.DB
{
    public partial class Not
    {
        public int Id { get; set; }
        public int SahipId { get; set; }
        public int PetId { get; set; }
        public string Aciklama { get; set; }

        public virtual Pet Pet { get; set; }
        public virtual PetSahip Sahip { get; set; }
    }
}
