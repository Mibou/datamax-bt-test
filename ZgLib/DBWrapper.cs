using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;
using System.Diagnostics;

namespace ZgLib
{
    public class DBWrapper : IDisposable
    {
        private string _datasource;
        private string _password;
        public SqlCeConnection sqlConnection = null;

        /// <summary>
        /// Datasource property i.e. the database file path
        /// </summary>
        public string datasource
        {
            get { return _datasource; }
            set { _datasource = value; }
        }

        /// <summary>
        /// Password property for the database access
        /// </summary>
        public string password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Database wrapper constructor
        /// </summary>
        /// <param name="datasource">Database file path</param>
        /// <param name="password">Database password</param>
        public DBWrapper(string datasource, string password)
        {
            this.datasource = datasource;
            this.password = password;
            this.OpenDB();
        }

        /// <summary>
        /// Open the wrapper's database
        /// </summary>
        /// <returns>Opened connection to the wrapper's database</returns>
        public SqlCeConnection OpenDB()
        {
            try
            {
                Log.Write("Opening connection", true);
                if(this.sqlConnection == null)
                    this.sqlConnection = new SqlCeConnection("Data Source = '" + datasource + "';Password='" + password + "'");

                if (this.sqlConnection.State.ToString() == "Closed")
                    this.sqlConnection.Open();
                Log.Finished();

                return this.sqlConnection;
            }
            catch (SqlCeException expt)
            {
                Log.Write(expt);
                throw expt;
            }
            catch (Exception expt)
            {
                Log.Write(expt);
                throw expt;
            }
        }

        /// <summary>
        /// Dispose the database connection
        /// </summary>
        public void Dispose()
        {
            Log.Write("Disposing connection", true);
            this.sqlConnection.Dispose();
            Log.Finished();
        }

        /// <summary>
        /// Close the database
        /// </summary>
        public void Close()
        {
            Log.Write("Closing connection", true);
            this.sqlConnection.Close();
            Log.Finished();
        }

        /// <summary>
        /// Test the database state
        /// </summary>
        /// <returns>Boolean indicating if the database is opened or not</returns>
        public bool TestDB()
        {
            try
            {
                if (this.sqlConnection.State.ToString() == "Open")
                    return true;
                return false;
            }
            catch (SqlCeException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
