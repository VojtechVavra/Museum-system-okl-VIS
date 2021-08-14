using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BO
{
    public class Rezervace
    {
        public int? rID { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public int PocetOsob { get; set; }
        public DateTime Datum { get; set; }

        public Rezervace() { }

        public bool PorovnejEmaily(string emailKPorovnani)
        {
            return Email.Equals(emailKPorovnani);
        }
    }
}
