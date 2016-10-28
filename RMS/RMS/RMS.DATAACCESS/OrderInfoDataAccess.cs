using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace RMS.DATAACCESS
{
    public class OrderInfoDataAccess
    {
        public void addOrderDetails(string query)
        {
            string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
            OracleConnection conn = new OracleConnection(connStr);
            string sql = query;
            OracleCommand cmd = new OracleCommand(sql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();    
        }

        public int getCurrentOrderId()
        {
            string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
            OracleConnection conn = new OracleConnection(connStr);

            string sql = "select order_orderid_seq.currval from dual";
            OracleCommand cmd = new OracleCommand(sql, conn);
            conn.Open();
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int currOrderId = dr.GetInt32(0);
            conn.Close();
            return currOrderId;
        }
    }
}
