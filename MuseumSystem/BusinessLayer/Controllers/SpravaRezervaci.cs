using BusinessLayer.BO;
using DataLayer.TableDataGateways;
using DTO.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Controllers
{
    public class SpravaRezervaci
    {
        private static readonly object m_LockObj = new object();
        private static SpravaRezervaci m_Instance;

        private List<Rezervace> m_Rezervace;

        private SpravaRezervaci() { }
        public static SpravaRezervaci Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new SpravaRezervaci());
                }
            }
        }

        public bool NactiRezervace(List<Rezervace> listRezervace)
        {
            //List<Rezervace> rezervace = null;
            m_Rezervace = listRezervace;
            List<RezervaceStruct> lstRezervace = null;
            string errMsg = string.Empty;

            if (RezervaceGW.Instance.LoadAll(out lstRezervace, out errMsg))
            {
                if (lstRezervace != null)
                {
                    foreach (var rez in lstRezervace)
                    {
                        m_Rezervace.Add(new Rezervace()
                        {
                            rID = rez.rID,
                            Jmeno = rez.Jmeno,
                            Prijmeni = rez.Prijmeni,
                            Email = rez.Email,
                            PocetOsob = rez.PocetOsob,
                            Datum = rez.Datum
                        });
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

        public bool VytvoritRezervaci()
        {
            return true;
        }

        public bool UlozRezervaci(Rezervace rezervace)
        {
            string errMsg;
            RezervaceGW rgw = RezervaceGW.Instance;

            RezervaceStruct rs = new RezervaceStruct(null, rezervace.Jmeno, rezervace.Prijmeni, rezervace.Email, rezervace.PocetOsob, rezervace.Datum);

            bool uspech = rgw.Save(rs, out errMsg);

            return uspech ? true : false;
        }
    }
}
