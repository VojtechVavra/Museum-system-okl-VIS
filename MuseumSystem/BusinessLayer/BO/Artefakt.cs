using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BO
{
    public class Artefakt
    {
        public int? aID { get; private set; }
        public string nazev { get; private set; }
        public DateTime datumNalezeni { get; private set; }
        public int stari { get; private set; }
        public string zemeNalezu { get; private set; }
        public bool jeVypujcen { get; private set; }
        public DateTime? datumNavraceni { get; private set; }

        public Artefakt(int? aID, string nazev, DateTime datumNalezeni, int stari, string zemeNalezu, bool jeVypujcen, DateTime? datumNavraceni)
        {
            this.aID = aID;
            this.nazev = nazev;
            this.datumNalezeni = datumNalezeni;
            this.stari = stari;
            this.zemeNalezu = zemeNalezu;
            this.jeVypujcen = jeVypujcen;
            this.datumNavraceni = datumNavraceni;
        }
    }
}
