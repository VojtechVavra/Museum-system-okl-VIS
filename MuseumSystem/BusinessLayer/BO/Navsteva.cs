using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BO
{
    public class Navsteva
    {
        public int PocetOsob { get; private set; }
        public DateTime? Datum { get; private set; }
        //bool maRezervaci { get; set; }
        public int? Rezervace { get; private set; }

        public Navsteva(int pocetOsob, DateTime? datum, int? rezervace)
        {
            this.PocetOsob = pocetOsob;
            this.Datum = datum;
            this.Rezervace = rezervace;
        }
    }
}
