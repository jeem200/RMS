using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace RMS.DATAACCESS
{
    public class ItemDataAccess
    {
        public int addItem(string name, string category, double unitPrice)
        {
            string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
            OracleConnection conn = new OracleConnection(connStr);

            string sqlChk = "select count(name) from item where name='"+name+"'";
            OracleCommand cmdChk = new OracleCommand(sqlChk,conn);
            conn.Open();
            OracleDataReader dr = cmdChk.ExecuteReader();
            dr.Read();
            int rowCount = dr.GetInt32(0);
            conn.Close();
            
            string sql = "Insert into ITEM VALUES(item_itemid_seq.NEXTVAL,'" + name + "','" + category + "'," + unitPrice + ")";
            OracleCommand cmd = new OracleCommand(sql, conn);
            conn.Open();
            if (rowCount == 0)
            {
                cmd.ExecuteNonQuery();
            }
            
            conn.Close();
            
            return rowCount;
        }

        public int editItem(string query)
        {
            string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
            OracleConnection conn = new OracleConnection(connStr);
            string sql = query;
            OracleCommand cmd = new OracleCommand(sql, conn);
                conn.Open();
                int rowCount;
                rowCount=cmd.ExecuteNonQuery();
                conn.Close();
                return rowCount;
                
        }


        public double getItemUnitPrice(string name)
        {
            string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
            OracleConnection conn = new OracleConnection(connStr);

            string sql = "SELECT get_price('"+name+"')  FROM dual";
            OracleCommand cmd = new OracleCommand(sql, conn);
            conn.Open();
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            double unitPrice = dr.GetDouble(0);
            conn.Close();
            return unitPrice;
        }

    }
}
