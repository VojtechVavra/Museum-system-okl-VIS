using DTO.Structs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.TableDataGateways
{
    public class NavstevaGW
    {

        public static NavstevaGW Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new NavstevaGW());
                }
            }
        }

        private static readonly object m_LockObj = new object();
        private static NavstevaGW m_Instance;

        private NavstevaGW()
        {
        }

        public int PocetOsob { get; set; }
        public DateTime Datum { get; set; }
        public bool ma_rezervaci { get; set; }


        public bool Save(NavstevaStruct navsteva, out string errMsg)
        {
            errMsg = string.Empty;
            // string datum = '20200618 10:00:00'
            string sqlInsert =
                "INSERT INTO navsteva(pocet_osob, datum, rezervace_rid) " +
                "VALUES(@pocet, @datum, @rezervace_rid);";

            int pocet = navsteva.PocetOsob;

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
                        sqlCmd.Parameters.AddWithValue("@pocet", navsteva.PocetOsob);
                        sqlCmd.Parameters.AddWithValue("@datum", DateTime.Now);
                        sqlCmd.Parameters.AddWithValue("@rezervace_rid", navsteva.Rezervace ?? (object)DBNull.Value);
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
        } //Save
    }
}
