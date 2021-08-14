using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BO
{
    public interface IPruvodce : IZamestnanec
    {
        bool Dostupnost { get; set; }
        bool Interni { get; set; }
    }
}
