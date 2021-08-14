using DTO.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.TableDataGateways
{
    public class RezervaceGW
    {
        private RezervaceGW()
        {

        }

        private static readonly object m_LockObj = new object();
        private static RezervaceGW m_Instance;

        public static RezervaceGW Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new RezervaceGW());
                }
            }
        }

        //public bool LoadAll(int id, out PruvodceStruct pruvodce)
        public bool LoadAll(out List<RezervaceStruct> rezervace, out string errMsg)
        {
            //string SQL_SELECT_ARCHEOLOG = "SELECT aID, Dostupnost, Jmeno, Prijmeni, Email, MobilniCislo, Typ FROM archeolog";
            errMsg = string.Empty;

            var sql = "SELECT rID, jmeno, prijmeni, email, pocet_osob, datum_a_cas FROM Rezervace";
            rezervace = new List<RezervaceStruct>();  // zamestnanci new PruvodceStruct();
 
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
                                    RezervaceStruct novaRezervace = new RezervaceStruct();
                                    novaRezervace.rID = result.GetInt32(0);
                                    //novaRezervace.Dostupnost = result.GetBoolean(1);
                                    novaRezervace.Jmeno = result.GetString(1);
                                    novaRezervace.Prijmeni = result.GetString(2);
                                    novaRezervace.Email = result.GetString(3);
                                    novaRezervace.PocetOsob = result.GetInt32(4);
                                    novaRezervace.Datum = result.GetDateTime(5);
                                    

                                    rezervace.Add(novaRezervace);
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

        public bool Save(RezervaceStruct rezervace, out string errMsg)
        {
            errMsg = string.Empty;
            // string datum = '20200618 10:00:00'
            //string sqlInsert =
            //    "INSERT INTO Rezervace(pocet_osob, datum, rezervace_rid) " +
            //    "VALUES(@pocet, @datum, @rezervace_rid);";
            string sqlInsert = "INSERT INTO rezervace (jmeno, prijmeni, email, pocet_osob, datum_a_cas) " +
                 "VALUES(@jmeno, @prijmeni, @email, @pocet_osob, @datum_a_cas)";
            int pocet = rezervace.PocetOsob;
            //Vlozeni rezervace do databaze
            try
            {
                DataConnection.Instance.Connect();
                try
                {
                    DataConnection.Instance.BeginTransaction();

                    SqlCommand sqlCmd = DataConnection.Instance.CreateCommand(sqlInsert);
                    try
                    {
                        //sqlCmd.Parameters.AddWithValue("@id", id);
                        sqlCmd.Parameters.AddWithValue("@jmeno", rezervace.Jmeno);
                        sqlCmd.Parameters.AddWithValue("@prijmeni", rezervace.Prijmeni);
                        sqlCmd.Parameters.AddWithValue("@email", rezervace.Email);
                        sqlCmd.Parameters.AddWithValue("@pocet_osob", rezervace.PocetOsob);
                        sqlCmd.Parameters.AddWithValue("@datum_a_cas", rezervace.Datum);
                        //sqlCmd.Parameters.AddWithValue("@rezervace_rid", navsteva.Rezervace ?? (object)DBNull.Value);
                        try
                        {
                            var result = DataConnection.Instance.ExecuteNonQuery(sqlCmd);
                            //Pokud je návratová hodnota záporná nepovedlo se vložit/upravit
                            if (result < 0)
                            {
                                Console.WriteLine("Nekdeje chyba");
                                //throw new DataException($"Nepovedlo se uložit Uživatele ID:({id})");
                            }
                        }
                        catch (Exception e)
                        {
                            //nastala chyba vykonání INSERT/UPDATE - vrátíme změny v DB
                            DataConnection.Instance.Rollback();
                            errMsg = $"Chyba při ukládání objektů Uživatelů \n{e.Message}";
                            return false;
                        }
                    }
                    finally
                    {
                        sqlCmd.Dispose();
                    }

                    DataConnection.Instance.EndTransaction();
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
        }//Save
    }
}
