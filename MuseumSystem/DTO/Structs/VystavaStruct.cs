using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Structs
{
    public struct VystavaStruct
    {
        public string nazev { get; set; }
        public DateTime odData { get; set; }
        public DateTime doData { get; set; }
        //private Archeolog archeolog;
        public List<ArtefaktStruct> artefakt { get; set; }

        public VystavaStruct(string nazev, DateTime odData, DateTime doData, List<ArtefaktStruct> artefakt)
        {
            this.nazev = nazev;
            this.odData = odData;
            this.doData = doData;
            //this.archeolog = archeolog;
            this.artefakt = artefakt;
        }
    }

    public class VystavyObj
    {
        public VystavaList vystavy { get; set; }
    }

    public class VystavaList
    {
        public List<VystavaStruct> vystavaList { get; set; }
    }
}
