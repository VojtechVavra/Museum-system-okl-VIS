using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.TableDataGateways
{
    public class ZakaznikGW
    {
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public int Rezervace_ID { get; set; }
    }
}
