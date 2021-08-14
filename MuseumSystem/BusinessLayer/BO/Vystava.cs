using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BO
{
    public class Vystava
    {
        public string nazev { get; private set; }
        public DateTime odData { get; private set; }
        public DateTime doData { get; private set; }
        //public Archeolog archeolog { get; private set; }
        public List<Artefakt> artefakt { get; private set; }

        public int pocetArtefaktu { get => artefakt.Count; }
        public Vystava() {
            artefakt = new List<Artefakt>();
        }
        public Vystava(string nazev, DateTime odData, DateTime doData/*, Archeolog archeolog*/, List<Artefakt> artefakt)
        {
            this.nazev = nazev;
            this.odData = odData;
            this.doData = doData;
            //this.archeolog = archeolog;
            this.artefakt = artefakt;
        }

        public List<Artefakt> vratArtefakty()
        {
            return artefakt;
        }

        public void AddArtefakt(Artefakt novyArtefakt)
        {
            artefakt.Add(novyArtefakt);
        }

        public void nastavJmenoVystavy(string jmenoVystavy)
        {
            nazev = jmenoVystavy;
        }
        public void nastavDatumZacatkuKonce(DateTime odData, DateTime doData)
        {
            this.odData = odData;
            this.doData = doData;
        }
    }
}
