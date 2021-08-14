using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BusinessLayer.TypPrihlaseniClass;

namespace BusinessLayer.BO
{
    public interface IZamestnanec
    {
        string Jmeno { get; set; }
        string Prijmeni { get; set; }
        string Email { get; set; }
        string MobilniCislo { get; set; }
        TypPrihlaseni? Typ { get; set; }
    }

    public enum TypPrihlaseni
    {
        RECEPCE,
        ARCHEOLOG
    }

}
