using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Structs
{
    public class RezervaceStruct
    {
        public int? rID;
        public string Jmeno;
        public string Prijmeni;
        public string Email;
        public int PocetOsob;
        public DateTime Datum;

        public RezervaceStruct() { }

        public RezervaceStruct(int? rID, string Jmeno, string Prijmeni, string Email, int PocetOsob, DateTime Datum)
        {
            this.rID = rID;
            this.Jmeno = Jmeno;
            this.Prijmeni = Prijmeni;
            this.Email = Email;
            this.PocetOsob = PocetOsob;
            this.Datum = Datum;
        }
    }
}
