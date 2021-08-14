using BusinessLayer.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Structs;
using DataLayer.TableDataGateways;

namespace BusinessLayer.Controllers
{
    public class SpravaArtefaktu
    {
        private static readonly object m_LockObj = new object();
        private static SpravaArtefaktu m_Instance;

        private List<Artefakt> m_Artefakty;
        public List<Artefakt> Artefakty
        {
            get => m_Artefakty;
        }

        private SpravaArtefaktu() { }
        public static SpravaArtefaktu Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new SpravaArtefaktu());
                }
            }
        }

        public bool NactiArtefakty(List<Artefakt> listArtefaktu)
        {
            m_Artefakty = listArtefaktu;
            List<ArtefaktStruct> lstArtefakty = null;
            string errMsg = string.Empty;

            if (ArtefaktGW.Instance.LoadAll(out lstArtefakty, out errMsg))
            {
                if (lstArtefakty != null)
                {
                    foreach (var art in lstArtefakty)
                    {
                        m_Artefakty.Add(new Artefakt(art.aID, art.nazev, art.datumNalezeni, art.stari, art.zemeNalezu, art.jeVypujcen, art.datumNavraceni));
                    }
                }
                return true;
            }
            else
            {
                return false;
                //Zalogujeme někam chybu
                //throw new Exception($"Zamestnanci: NacteniZUloziste \n{errMsg}");
            }

            return true;
        }
    }
}
