using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb_20_test2.obgdb
{
    public class ConnectDB
    {
        SQLiteConnection conn;
        SQLiteCommand com;
        public int _rowsAffected = 0;

        public ConnectDB()
        {
            conn = new SQLiteConnection("Data Source=eb_20.sqlite;Version=3;");
            com = new SQLiteCommand(conn);
        }
        public DataTable selectData(String sql)
        {
            DataTable toReturn = new DataTable();

            com = new SQLiteCommand();
            com.CommandText = sql;
            com.CommandType = CommandType.Text;
            com.Connection = conn;
            SQLiteDataAdapter adapMainhis = new SQLiteDataAdapter(com);
            try
            {
                conn.Open();
                adapMainhis.Fill(toReturn);
                //return toReturn;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HResult " + ex.HResult + "\n" + "Message" + ex.Message + "\n" + sql, "Error");
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                conn.Close();
                com.Dispose();
            }
            return toReturn;
        }
        public String ExecuteNonQuery(String sql)
        {
            String toReturn = "";
            com = new SQLiteCommand();
            com.CommandText = sql;
            com.CommandType = CommandType.Text;
            com.Connection = conn;
            try
            {
                conn.Open();
                _rowsAffected = com.ExecuteNonQuery();
                //_rowsAffected = (int)com.ExecuteScalar();
                //toReturn = _rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception("ExecuteNonQuery::Error occured.", ex);
                toReturn = ex.Message;
            }
            finally
            {
                //_mainConnection.Close();
                conn.Close();
                com.Dispose();
            }

            return toReturn;
        }
    }
}
