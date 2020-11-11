

namespace QueueMQ.Contingency
{
    using System;
    using System.Data;
    using Microsoft.Data.Sqlite;
    using System.Text;

    internal class DataAccessSqlite_ADO
    {
        private const string _DBURL = "<DB_URL>";
        private const string _CONNECTIONSTRING = "Data Source=\"" + _DBURL + "\";Version=3;New=True;Compress=True;";
        private string cConnectionString;
        private int mDefaultCommandTimeout = 100;
        private static DataAccessSqlite_ADO _Instance;
        private static readonly object ob = new object();
        public static DataAccessSqlite_ADO Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (ob)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new DataAccessSqlite_ADO();
                        }
                    }
                }
                return _Instance;
            }
        }


        public DataAccessSqlite_ADO()
        {
        }
        private string Base64Decode(string Base64String)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(Base64String));
        }
        private bool CreateTable(SqliteConnection conn)
        {
            try
            {
                SqliteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = Base64Decode(RecursosQueueManager.Query);
                sqlite_cmd.ExecuteNonQuery();
                return true;
            }
#pragma warning disable CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            catch (Exception Ex)
            {
#pragma warning restore CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
                //throw Ex;
                return false;
            }
        }

        private bool ExecuteNonQuery(string Command)
        {
            try
            {
                SqliteConnection conn = openConnection();
                SqliteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = Command;
                sqlite_cmd.ExecuteNonQuery();
                return true;
            }
#pragma warning disable CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            catch (Exception Ex)
#pragma warning restore CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            {
                //throw Ex;
                return false;
            }
        }

        private bool ExecuteQuery(string Command)
        {
            try
            {
                SqliteConnection conn = openConnection();
                SqliteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = Command;
                SqliteDataReader r = sqlite_cmd.ExecuteReader();
                while (r.Read())
                {
                    string FileNames = (string)r["FileName"];

                    //ImportedFiles.Add(FileNames);
                }
                return true;
            }
#pragma warning disable CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            catch (Exception Ex)
#pragma warning restore CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            {
                //throw Ex;
                return false;
            }
        }
        private bool TableExist(SqliteConnection conn)
        {
            try
            {
                SqliteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = Base64Decode(RecursosQueueManager.Validate);
                sqlite_cmd.ExecuteNonQuery();
                return true;
            }
#pragma warning disable CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            catch (Exception Ex)
#pragma warning restore CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            {
                //throw Ex;
                return false;
            }
        }
        public void CreateConnection(string connectionString)
        {
            this.cConnectionString = _CONNECTIONSTRING.Replace(_DBURL, connectionString);
        }
        //public void executeNonQuery(string) { }
        public SqliteConnection openConnection()
        {
            try
            {
                SqliteConnection dbSQLiteConnection = new SqliteConnection(cConnectionString);
                dbSQLiteConnection.Open();
                return dbSQLiteConnection;
            }
#pragma warning disable CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            catch (Exception Ex)
#pragma warning restore CS0168 // La variable 'Ex' se ha declarado pero nunca se usa
            {
                //throw Ex;
                return null;
            }
        }


        public string closeConnection(SqliteConnection dbSQLiteConnection = null)
        {
            string message = String.Empty;
            try
            {
                if (dbSQLiteConnection != null)
                {
                    switch (dbSQLiteConnection.State)
                    {
                        case ConnectionState.Open:
                        case ConnectionState.Fetching:
                        case ConnectionState.Executing:
                        case ConnectionState.Broken:
                            dbSQLiteConnection.Close();
                            dbSQLiteConnection.Dispose();
                            dbSQLiteConnection = null;
                            break;
                        case ConnectionState.Closed:
                            dbSQLiteConnection.Dispose();
                            dbSQLiteConnection = null;
                            break;
                        default:
                            dbSQLiteConnection = null;
                            break;
                    }
                    return message;
                }
                message = "";
                return message;
            }
            catch (Exception Ex)
            {
                message = Ex.Message;
                //throw Ex;
                return message;
            }

        }
        void Dispose()
        {
            throw new NotImplementedException();
        }

        public int DefaultCommandTimeout
        {
            get { return mDefaultCommandTimeout; }
            set
            {
                if (value < 0)
                    mDefaultCommandTimeout = 0;
                else
                    mDefaultCommandTimeout = value;
            }
        }
    }
}