using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Structs
{
    public struct ArtefaktStruct
    {
        public int? aID;
        public string nazev;
        public DateTime datumNalezeni;
        public int stari;
        public string zemeNalezu;
        public bool jeVypujcen;
        public DateTime? datumNavraceni;

        public ArtefaktStruct(int? aID, string nazev, DateTime datumNalezeni, int stari, string zemeNalezu, bool jeVypujcen, DateTime? datumNavraceni)
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
