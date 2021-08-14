using DTO.Structs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.TableDataGateways
{
    public class ArtefaktGW
    {
        private ArtefaktGW() { }

        private static readonly object m_LockObj = new object();
        private static ArtefaktGW m_Instance;

        public static ArtefaktGW Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new ArtefaktGW());
                }
            }
        }

        public bool LoadAll(out List<ArtefaktStruct> artefakt, out string errMsg)
        {
            errMsg = string.Empty;

            var sql = "SELECT aID, nazev, datum_nalezeni, stari, zeme_nalezu, je_vypujcen, datum_navraceni FROM Artefakt";
            artefakt = new List<ArtefaktStruct>();  // zamestnanci new PruvodceStruct();

            try
            {
                DataConnection.Instance.Connect();
                try
                {
                    SqlCommand sqlCmd = DataConnection.Instance.CreateCommand(sql);
                    try
                    {
                        var result = DataConnection.Instance.Select(sqlCmd);
                        try
                        {
                            if (result.HasRows)
                            {
                                while (result.Read())
                                {
                                    ArtefaktStruct novyArtefakt = new ArtefaktStruct();
                                    novyArtefakt.aID = result.GetInt32(0);
                                    novyArtefakt.nazev = result.GetString(1);
                                    novyArtefakt.datumNalezeni = result.GetDateTime(2);
                                    novyArtefakt.stari = result.GetInt32(3);
                                    novyArtefakt.zemeNalezu = result.GetString(4);
                                    novyArtefakt.jeVypujcen = result.GetBoolean(5);

                                    if (!result.IsDBNull(6))
                                    {
                                        novyArtefakt.datumNavraceni = result.GetDateTime(6);
                                    } else {
                                        novyArtefakt.datumNavraceni = null;
                                    }

                                    artefakt.Add(novyArtefakt);
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
