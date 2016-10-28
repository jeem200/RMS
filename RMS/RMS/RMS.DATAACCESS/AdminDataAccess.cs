using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace RMS.DATAACCESS
{
    public class AdminDataAccess
    {
        public string getAdminPassword()
        {
            string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
            OracleConnection conn = new OracleConnection(connStr);

            string sql = "select * from ADMINPASSWORD";
            OracleCommand cmdChk = new OracleCommand(sql, conn);
            conn.Open();
            OracleDataReader dr = cmdChk.ExecuteReader();
            dr.Read();
            string password = dr.GetString(0);
            conn.Close();

           return password;
        }

        public int updateAdminPassword(string query)
        {
            string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
            OracleConnection conn = new OracleConnection(connStr);
            string sql = query;
            OracleCommand cmd = new OracleCommand(sql, conn);
            conn.Open();
            int rowCount;
            rowCount = cmd.ExecuteNonQuery();
            conn.Close();
            return rowCount;
        }
    }
}
