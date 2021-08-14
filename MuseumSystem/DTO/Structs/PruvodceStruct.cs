using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Structs
{
    public struct PruvodceStruct
    {
        public int pID { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public string MobilniCislo { get; set; }
        public bool Dostupnost { get; set; }
        public int Interni { get; set; }

        public PruvodceStruct(int pid, string jmeno, string prijmeni, string email, string mobilniCislo, bool dostupnost, int interni)
        {
            pID = pid;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            Email = email;
            MobilniCislo = mobilniCislo;
            Dostupnost = dostupnost;
            Interni = interni;
        }

    }
}
