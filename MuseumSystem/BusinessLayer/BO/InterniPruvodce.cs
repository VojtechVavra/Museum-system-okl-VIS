using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BO
{
    public class InterniPruvodce : IPruvodce
    {
        public int pID { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public string MobilniCislo { get; set; }
        public bool Dostupnost { get; set; }
        public bool Interni { get; set; }
        public TypPrihlaseni? Typ { get; set; }
    }
}
