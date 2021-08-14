using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Structs
{
    public class NavstevaStruct
    {
        public int? ID { get; set; }
        public int PocetOsob { get; set; }
        public DateTime? Datum { get; set; }
        public int? Rezervace { get; set; }

        // struktura pro ulozeni
        public NavstevaStruct(int? id, int PocetOsob, DateTime? Datum, int? Rezervace)
        {
            this.ID = id;
            this.PocetOsob = PocetOsob;
            this.Datum = Datum;
            this.Rezervace = Rezervace;
        }
    }
}
