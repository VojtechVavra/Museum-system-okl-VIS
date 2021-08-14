using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BO
{
    public class Archeolog : IZamestnanec
    {
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public string MobilniCislo { get; set; }
        public TypPrihlaseni? Typ { get; set; }
        //Vystava[] vystava;

        public void VytvoritVystavu()
        {

        }

        
    }
}
