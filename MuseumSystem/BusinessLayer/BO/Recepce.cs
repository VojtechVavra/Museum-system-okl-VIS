using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BO
{
    public class Recepce : IZamestnanec
    {
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public string MobilniCislo { get; set; }
        public TypPrihlaseni? Typ { get; set; }
        Rezervace[] rezervace;

        /*public void ZaevidovatNavstevuSRezervaci()
        {
        }

        public void ZaevidovatNavstevuBezRezervace()
        {   
        }

        public void VytvoritRezervaci()
        {
        }*/
    }
}
