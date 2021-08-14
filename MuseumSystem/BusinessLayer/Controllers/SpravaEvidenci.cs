using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.BO;
using DataLayer.TableDataGateways;
using DTO.Structs;

namespace BusinessLayer.Controllers
{
    public class SpravaEvidenci
    {

        private static readonly object m_LockObj = new object();
        private static SpravaEvidenci m_Instance;

        private List<Navsteva> m_Navsteva;
        public List<Navsteva> Navsteva
        {
            get => m_Navsteva;
        }

        private SpravaEvidenci() { }
        public static SpravaEvidenci Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new SpravaEvidenci());
                }
            }
        }

        public int CelkovyPocetNavstev => m_Navsteva.Count;

        public bool ZapisDoUloziste(Navsteva _navsteva)
        {
            string errMsg = string.Empty;
            
            NavstevaStruct navsteva = new NavstevaStruct(null, _navsteva.PocetOsob, null, _navsteva.Rezervace);
            if (!NavstevaGW.Instance.Save(navsteva, out errMsg))
            {
                //Zalogujeme někam chybu
                throw new Exception($"Chyba Navsteva: Zápis do uložiště \n{errMsg}");
            }
            return true;
        }//ZapisDoUloziste

        public bool EvidenceNavstevyBezRezervaci(string pocet)
        {
            int pocetOsob;
            bool povedloSe = int.TryParse(pocet, out pocetOsob);

            if (povedloSe)
            {
                Navsteva novaNavsteva = new Navsteva(pocetOsob, null, null);
                return ZapisDoUloziste(novaNavsteva);
            }
            else
            {
                return false;
            }
        }

        public bool EvidenceNavstevySRezervaci(Rezervace rezervace, string emailKOtestovani, string pocet)
        {
            int pocetOsob;
            bool povedloSe = int.TryParse(pocet, out pocetOsob);

            if(povedloSe)
            {
                if(rezervace.PorovnejEmaily(emailKOtestovani))
                {
                    Navsteva novaNavsteva = new Navsteva(pocetOsob, null, rezervace.rID);
                    return ZapisDoUloziste(novaNavsteva);
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
