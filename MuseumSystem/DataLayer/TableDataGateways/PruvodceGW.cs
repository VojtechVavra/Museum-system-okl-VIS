using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO.Structs;


namespace DataLayer.TableDataGateways
{
    public class PruvodceGW
    {
        private PruvodceGW()
        {

        }

        private static readonly object m_LockObj = new object();
        private static PruvodceGW m_Instance;

        public static PruvodceGW Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new PruvodceGW());
                }
            }
        }


        public bool LoadAllPruvodce(out List<PruvodceStruct> pruvodce, int interni, out string errMsg)
        {
            errMsg = null;
            string SQL_SELECT_VSECHNY_PRUVODCE = "SELECT pid, jmeno, prijmeni, email, mobilni_cislo, dostupnost, interni FROM Pruvodce";
            string SQL_SELECT_INTERNI_EXTERNI_PRUVODCI = "SELECT pid, jmeno, prijmeni, email, mobilni_cislo, dostupnost, interni FROM Pruvodce";
            string SQL_SELECT_DOSTUPNI_INTERNI_PRUVODCI = "SELECT pid, jmeno, prijmeni, email, mobilni_cislo, dostupnost, interni FROM Pruvodce WHERE dostupnost = 1 AND interni = 1";
            string SQL_SELECT_DOSTUPNI_EXTERNI_PRUVODCI = "SELECT pid, jmeno, prijmeni, email, mobilni_cislo, dostupnost, interni FROM Pruvodce WHERE dostupnost = 1 AND interni = 0";

            string sql = interni == 1 ? SQL_SELECT_DOSTUPNI_INTERNI_PRUVODCI : SQL_SELECT_DOSTUPNI_EXTERNI_PRUVODCI;

            pruvodce = new List<PruvodceStruct>();  // zamestnanci new PruvodceStruct();

            //Nalezeni uzivatele podle jeho id v DB
            try
            {
                DataConnection.Instance.Connect();
                try
                {
                    // string sql = "SELECT Jmeno,Prijmeni,DatumNarozeni,ClenemOd,Spolehlivost FROM Uzivatele WHERE (Id=@id)";
                    SqlCommand sqlCmd = DataConnection.Instance.CreateCommand(sql);
                    try
                    {
                        //sqlCmd.Parameters.AddWithValue("@id", id);
                        var result = DataConnection.Instance.Select(sqlCmd);
                        try
                        {
                            if (result.HasRows)
                            {
                                while (result.Read())
                                {
                                    PruvodceStruct novyPruvodce = new PruvodceStruct();
                                    novyPruvodce.pID = result.GetInt32(0);
                                    novyPruvodce.Jmeno = result.GetString(1);
                                    novyPruvodce.Prijmeni = result.GetString(2);
                                    novyPruvodce.Email = result.GetString(3);
                                    novyPruvodce.MobilniCislo = result.GetString(4);
                                    novyPruvodce.Dostupnost = result.GetBoolean(5);
                                    novyPruvodce.Interni = result.GetBoolean(6) ? 1 : 0;

                                    pruvodce.Add(novyPruvodce);
                                }
                            }
                        }
                        finally
                        {
                            result.Close();
                        }
                    }
                    finally
                    {
                        sqlCmd.Dispose();
                    }
                }
                finally
                {
                    DataConnection.Instance.Close();
                }
            }
            catch (Exception e)
            {
                errMsg = $"Chyba při Connection do DB \n{e.Message}";
                return false;
            }

            return true;
        }
    }
}
