using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Description: Přenosový objekt pro zaměstnance

namespace DTO.Structs
{
    public struct ZamestnanecStruct
    {
        int ID { get; set; }
        string Jmeno { get; set; }
        string Prijmeni { get; set; }
        string Email { get; set; }
        string MobilniCislo { get; set; }
        int TypPrihlaseni { get; set; }    // TypPrihlaseni? Typ { get; set; }

        // struktura pro ulozeni
        public ZamestnanecStruct(int id, string jmeno, string prijmeni, string email, string mobilniCislo, int typPrihlaseni)
        {
            ID = id;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            Email = email;
            MobilniCislo = mobilniCislo;
            TypPrihlaseni = typPrihlaseni;
        }
    }
}
