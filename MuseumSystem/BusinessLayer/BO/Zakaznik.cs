using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BO
{
    public class Zakaznik
    {
        string Jmeno { get; set; }
        string Prijmeni { get; set; }
        string Email { get; set; }
        Rezervace[] rezervace;
    }
}
