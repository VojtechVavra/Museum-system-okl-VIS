using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Controllers;
using BusinessLayer.BO;

namespace PresentationLayer
{
    /* 
     * 1. layer - Presentation Layer/ UI Layer
     * This is the top-most layer of the application where the user performs their activity.
     * Basically the user's input validation and rule processing is done in this layer.
     * https://www.c-sharpcorner.com/UploadFile/dacca2/understand-3-tier-architecture-in-C-Sharp-net/
     */

    public class Class1
    {

        /*public void PrintPruvodce()
        {
            SpravaZamestnancu sz = SpravaZamestnancu.Instance;
            Collection<IPruvodce> collPruvodce = sz.selectPruvodce();

            ExterniPruvodce ep = (ExterniPruvodce)collPruvodce[0];

            Console.WriteLine($"jmeno: {ep.Jmeno}, prijmeni: {ep.Prijmeni}");
        }*/

        /*public void UlozNavstevu(int pocet, int? rezervace)
        {
            SpravaEvidenci se = SpravaEvidenci.Instance;
            Navsteva novaNavsteva = new Navsteva(pocet, null, rezervace);

            se.ZapisDoUloziste(novaNavsteva);
        }*/

        /*public bool NactiRezervace(List<Rezervace> rez)
        {
            SpravaRezervaci sr = SpravaRezervaci.Instance;
            List<Rezervace> rezervace = null;

            bool uspech = sr.NactiRezervace(rezervace);

            if(uspech)
            {
                rez = rezervace;
                return true;
            }

            Console.WriteLine("Neuspesny Select nad databazi!");

            rez = null;
            return false;
        }*/

    }
}
