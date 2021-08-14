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
    public class SpravaVystavy
    {
        private static readonly object m_LockObj = new object();
        private static SpravaVystavy m_Instance;

        private Vystava m_Vystava = new Vystava();

        public Vystava getVystava()
        {
            return m_Vystava;
        }

        private SpravaVystavy() { }
        public static SpravaVystavy Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new SpravaVystavy());
                }
            }
        }

        public List<Artefakt> VratRoztvorenouVystavu()
        {
            return m_Vystava.vratArtefakty();
        }
        public void VymazRoztvorenouVystavu()
        {
            m_Vystava = new Vystava();
        }

        public bool pridatArtefaktDoVystavy(DateTime odData, DateTime doData, Artefakt artefakt)
        {
            if(artefakt.datumNavraceni == null)
            {
                m_Vystava.AddArtefakt(artefakt);
                return true;
            }
            else if(doData < artefakt.datumNavraceni)
            {
                m_Vystava.AddArtefakt(artefakt);
                return true;
            }
            // dotaz na druhe muzeum, zda muze byt artefakt prodlouzen
            else if(DotazNaProdlouzeniVypujceni(artefakt))
            {
                m_Vystava.AddArtefakt(artefakt);
                return true;
            }
            return false;
        }

        // simulace odpovedi od spolupracujiciho muzea
        private bool DotazNaProdlouzeniVypujceni(Artefakt artefakt)
        {
            System.Random rnd = new System.Random();
            return rnd.Next() > (Int32.MaxValue / 2);
        }

        public bool ulozVystavu()
        {
            if(m_Vystava == null)
            {
                return false;
            }

            List<ArtefaktStruct> artefaktStruct = new List<ArtefaktStruct>();
            foreach(Artefakt art in m_Vystava.artefakt)
            {
                artefaktStruct.Add(new ArtefaktStruct(
                    art.aID,
                    art.nazev,
                    art.datumNalezeni,
                    art.stari,
                    art.zemeNalezu,
                    art.jeVypujcen,
                    art.datumNavraceni)
                    );
            }
            VystavaStruct vystavaStruct = new VystavaStruct(m_Vystava.nazev, m_Vystava.odData, m_Vystava.doData, artefaktStruct);

            string errMsg = string.Empty;

            if (!VystavaGW.Instance.Save(vystavaStruct, out errMsg))
            {
                //Zalogujeme někam chybu
                throw new Exception($"Chyba Navsteva: Zápis do uložiště \n{errMsg}");
            }
            return true;
        }



        public List<Vystava> nactiVystavy()
        {
            List<VystavaStruct> vystavaStruct = new List<VystavaStruct>();
            VystavyObj vystavyObj = new VystavyObj();
            string errMsg = string.Empty;

            bool uspech = VystavaGW.Instance.Load(out vystavyObj, out errMsg);
            //bool uspech = VystavaGW.Instance.Load(vystavaStruct, out errMsg);
            if(!uspech)
            {
                return null;
            }

            List<Vystava> vystavy = new List<Vystava>();
            //foreach (VystavaStruct vs in vystavaStruct)
            foreach (VystavaStruct vs in vystavyObj.vystavy.vystavaList)
            {
                List<Artefakt> artefakty = new List<Artefakt>();

                foreach (var vystavaArtefakty in vs.artefakt)
                {
                    artefakty.Add(new Artefakt(vystavaArtefakty.aID, vystavaArtefakty.nazev, vystavaArtefakty.datumNalezeni, vystavaArtefakty.stari, vystavaArtefakty.zemeNalezu, vystavaArtefakty.jeVypujcen, vystavaArtefakty.datumNavraceni));
                }

                Vystava vystava = new Vystava(vs.nazev, vs.odData, vs.doData, artefakty);
                vystavy.Add(vystava);
            }

            return vystavy;
        }

        public void SmazVystavu(Vystava vystavaKeSmazani)
        {
            VystavyObj vystavyObj = new VystavyObj();
            string errMsg = string.Empty;

            bool uspech = VystavaGW.Instance.Load(out vystavyObj, out errMsg);
            //foreach (VystavaStruct vs in vystavyObj.vystavy.vystavaList)
            for(int i = 0; i < vystavyObj.vystavy.vystavaList.Count; i++)
            {
                var vs = vystavyObj.vystavy.vystavaList[i];
                if (vs.nazev == vystavaKeSmazani.nazev && vs.odData == vystavaKeSmazani.odData && vs.doData == vystavaKeSmazani.doData && vs.artefakt.Count == vystavaKeSmazani.artefakt.Count)
                {
                    vystavyObj.vystavy.vystavaList.RemoveAt(i);

                    if (!VystavaGW.Instance.Save(vystavyObj, out errMsg))
                    {
                        //Zalogujeme někam chybu
                        throw new Exception($"Chyba Navsteva: Zápis do uložiště \n{errMsg}");
                    }
                    return;
                }
            }
        }
    }
}
