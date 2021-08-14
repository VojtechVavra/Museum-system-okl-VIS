using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Configuration.Assemblies;

namespace DataLayer
{
    public class DataConnection
    {
        private SqlConnection m_Connection;
        private SqlTransaction m_SqlTransaction;
        private static readonly Object m_LockObj = new object();
        private static DataConnection m_Instance = null;

        public SqlTransaction SqlTransaction
        {
            get => m_SqlTransaction;
            set => m_SqlTransaction = value;
        }

        public SqlConnection Connection
        {
            get => m_Connection;
            set => m_Connection = value;
        }

        public static DataConnection Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    return m_Instance ?? (m_Instance = new DataConnection());
                }
            }
        }


        private DataConnection()
        {
            m_Connection = new SqlConnection();
        }

        /// <summary>
        /// Connect
        /// </summary>
        public bool Connect()
        {
            bool ret = true;
            if (Connection.State != ConnectionState.Open)
            {
                // connection string is stored in file App.config or Web.config
                //ret = Connect(ConfigurationManager.AppSettings["ConnectionStringMsSql"]);
                ret = Connect(ConfigurationManager.ConnectionStrings["ConnectionStringMsSql"].ConnectionString);
            }
            return ret;
        }

        public bool Connect(string connString)
        {
            if (Connection.State != ConnectionState.Open)
            {
                // DESKTOP-PFN07OO\SQLEXPRESS (DESKTOP-PFN07OO\Vooja)
                //string cnstr = "Server=DESKTOP-PFN07OO\\SQLEXPRESS;DataBase=vis_muzeum;Integrated Security = SSPI";
                //string cnstr = "Server=DESKTOP-PFN07OO\\SQLEXPRESS;DataBase=vis_muzeum;Integrated Security = SSPI";
                //Connection.ConnectionString = cnstr;
                Connection.ConnectionString = connString;
                Connection.Open();
            }
            return true;
        }

        /// <summary>
        /// Close
        /// </summary>
        public void Close()
        {
            Connection.Close();
        }

        /// <summary>
        /// Begin a transaction.
        /// </summary>
        public void BeginTransaction()
        {
            SqlTransaction = Connection.BeginTransaction(IsolationLevel.Serializable);
        }

        /// <summary>
        /// End a transaction.
        /// </summary>
        public void EndTransaction()
        {
            SqlTransaction.Commit();
            Close();
        }

        /// <summary>
        /// If a transaction is failed call it.
        /// </summary>
        public void Rollback()
        {
            SqlTransaction.Rollback();
        }

        /// <summary>
        /// Insert a record encapulated in the command.
        /// </summary>
        public int ExecuteNonQuery(SqlCommand command)
        {
            int rowNumber = 0;
            try
            {
                rowNumber = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            return rowNumber;
        }

        /// <summary>
        /// Create command
        /// </summary>
        public SqlCommand CreateCommand(string strCommand)
        {
            SqlCommand command = new SqlCommand(strCommand, Connection);

            if (SqlTransaction != null)
            {
                command.Transaction = SqlTransaction;
            }
            return command;
        }

        /// <summary>
        /// Select encapulated in the command.
        /// </summary>
        public SqlDataReader Select(SqlCommand command)
        {
            SqlDataReader sqlReader = command.ExecuteReader();
            return sqlReader;
        }

    }//class
}//namespace