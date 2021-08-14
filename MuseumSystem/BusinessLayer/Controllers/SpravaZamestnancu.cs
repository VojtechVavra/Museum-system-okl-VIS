using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using DataLayer.TableDataGateways;
using DTO.Structs;
using BusinessLayer.BO;

namespace BusinessLayer.Controllers
{
    public class SpravaZamestnancu
    {
        private static readonly object m_LockObj = new object();
        private static SpravaZamestnancu m_Instance;

        private SpravaZamestnancu() { }
        public static SpravaZamestnancu Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new SpravaZamestnancu());
                }
            }
        }

        private List<IPruvodce> m_Pruvodce;

        public List<IZamestnanec> m_Zamestnanci;
        public List<IZamestnanec> Zamestnanci
        {
            get => m_Zamestnanci;
        }

        public List<IPruvodce> selectPruvodce(int interniPruvodce) {
            m_Pruvodce = null;
            List<IPruvodce> dd = new List<IPruvodce>();
            string errMsg;
            int interni = 1;
            
            List<PruvodceStruct> lstPruv = null;
            List<ExterniPruvodce> lstExtPruv = null;
            List<InterniPruvodce> lstIntPruv = null;
            List<IPruvodce> lstIPruv = null;

            if (PruvodceGW.Instance.LoadAllPruvodce(out lstPruv, interniPruvodce, out errMsg))
            {
                if (lstPruv != null)
                {
                    //lstIPruv = interniPruvodce == 1 ? (List<IPruvodce>)(new List<InterniPruvodce>()) : (List<IPruvodce>)(new List<ExterniPruvodce>());
                    if(interniPruvodce == 1)
                    {
                        lstIPruv = new List<IPruvodce>();
                    }
                    else
                    {
                        lstIPruv = new List<IPruvodce>();
                    }
                    foreach (var pr in lstPruv)
                    {
                        if (interniPruvodce == 1)
                        {
                            lstIPruv.Add(new InterniPruvodce()
                            {
                                pID = pr.pID,
                                Jmeno = pr.Jmeno,
                                Prijmeni = pr.Prijmeni,
                                Email = pr.Email,
                                MobilniCislo = pr.MobilniCislo,
                                Dostupnost = pr.Dostupnost,
                                Interni = pr.Interni == 1 ? true : false,
                                Typ = null
                            });
                        }
                        else // externi pruvodce
                        {
                            lstIPruv.Add(new ExterniPruvodce()
                            {
                                pID = pr.pID,
                                Jmeno = pr.Jmeno,
                                Prijmeni = pr.Prijmeni,
                                Email = pr.Email,
                                MobilniCislo = pr.MobilniCislo,
                                Dostupnost = pr.Dostupnost,
                                Interni = pr.Interni == 1 ? true : false,
                                Typ = null
                            });
                        }
                    }
                    return lstIPruv;
                    //return true;
                }
                else
                {
                    // nenalezen zadny dostupny pruvodce
                    return null;
                    //Zalogujeme někam chybu
                    //throw new Exception($"Zamestnanci: NacteniZUloziste \n{errMsg}");
                }

            }
            return null;
        }

        //public List<IPruvodce> Zamestnanci
        public List<IPruvodce> Pruvodce
        {
            get => m_Pruvodce;
        }
    }
}


