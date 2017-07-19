using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace MatchPDFreport
{
    class SQLconnection
    {          
        string connectionString;
        string connectionProvider;
        string conName = "con";
        public SQLconnection()
        {
            //GetConnectionString();

        }
        public void GetConnectionString()
        {
            connectionString = ConfigurationManager.ConnectionStrings[conName].ConnectionString;
            connectionProvider = ConfigurationManager.ConnectionStrings[conName].ProviderName;
        }
        public DataTable QueryDataTable(string sql)
        {
            GetConnectionString();
            DataTable dt = new DataTable();
            switch(connectionProvider)
            {
                case "System.Data.SqlClient":
                    using (SqlConnection con= new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                            sqlAdapter.Fill(dt);
                        }
                    }
                    break;
                case "Microsoft.Jet.OLEDB.4.0":
                    using (OleDbConnection con = new OleDbConnection(connectionString))
                    {
                        using (OleDbCommand cmd = new OleDbCommand(sql, con))
                        {
                            OleDbDataAdapter oldebAdapter = new OleDbDataAdapter(cmd);
                            oldebAdapter.Fill(dt);
                        }
                    }
                    break;
            }
            return dt;
        }

    }   
}
