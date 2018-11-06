using eb_20_test2.object1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb_20_test2.obgdb
{
    public class ComPortDB
    {
        public ComPort cpt;
        ConnectDB conn;
        public List<ComPort> lCpt;

        public ComPortDB(ConnectDB c)
        {
            conn = c;
            initConfig();
        }
        private void initConfig()
        {
            lCpt = new List<ComPort>();
            cpt = new ComPort();
            cpt.parity = "parity";
            cpt.stop_bit = "stop_bit";
            cpt.id = "id";
            cpt.baud_rate = "baud_rate";
            cpt.data_bit = "data_bit";

            cpt.table = "b_comport";
            cpt.pkField = "id";
        }
        private void chkNull(ComPort p)
        {
            int chk = 0;

            p.parity = p.parity == null ? "" : p.parity;
            p.stop_bit = p.stop_bit == null ? "" : p.stop_bit;
            p.baud_rate = p.baud_rate == null ? "" : p.baud_rate;
            p.id = p.id == null ? "" : p.id;
            p.data_bit = p.data_bit == null ? "" : p.data_bit;

            //p.item_sub_group_id = int.TryParse(p.item_sub_group_id, out chk) ? chk.ToString() : "0";

        }
        public String insert(ComPort p, String userId)
        {
            String re = "";
            String sql = "";
            p.parity = "1";
            //p.ssdata_id = "";
            int chk = 0;

            chkNull(p);
            sql = "Insert Into " + cpt.table + "(" + cpt.baud_rate + "," + cpt.stop_bit + "," + cpt.parity + "," +
                "" + cpt.data_bit + " " +
                ") " +
                "Values ('" + p.baud_rate.Replace("'", "''") + "','" + p.stop_bit.Replace("'", "''") + "','" + p.parity + "'," +
                "'" + p.data_bit + "' " +
                ")";
            try
            {
                re = conn.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                sql = ex.Message + " " + ex.InnerException;
            }

            return re;
        }
        public String update(ComPort p, String userId)
        {
            String re = "";
            String sql = "";
            int chk = 0;

            chkNull(p);
            sql = "Update " + cpt.table + " Set " +
                " " + cpt.baud_rate + " = '" + p.baud_rate.Replace("'", "''") + "'" +
                "," + cpt.stop_bit + " = '" + p.stop_bit.Replace("'", "''") + "'" +
                "," + cpt.parity + " = '" + p.parity.Replace("'", "''") + "'" +
                "," + cpt.data_bit + " = '" + p.data_bit.Replace("'", "''") + "'" +

                "Where " + cpt.pkField + "='" + p.id + "'"
                ;

            try
            {
                re = conn.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                sql = ex.Message + " " + ex.InnerException;
            }

            return re;
        }
        public String insertComPort(ComPort p, String userId)
        {
            String re = "";

            if (p.id.Equals(""))
            {
                re = insert(p, "");
            }
            else
            {
                re = update(p, "");
            }

            return re;
        }
        public void getlComPort()
        {
            //lDept = new List<Position>();

            lCpt.Clear();
            DataTable dt = new DataTable();
            dt = selectAll();
            foreach (DataRow row in dt.Rows)
            {
                ComPort stf1 = new ComPort();
                stf1.id = row[cpt.id].ToString();
                stf1.baud_rate = row[cpt.baud_rate].ToString();

                lCpt.Add(stf1);
            }
        }

        public String getIdByName(String name)
        {
            String id = "";
            foreach (ComPort stf1 in lCpt)
            {
                if (name.Trim().Equals(stf1.baud_rate))
                {
                    id = stf1.id;
                    break;
                }
            }
            return id;
        }
        public String VoidComPort(String stfId, String userIdVoid)
        {
            DataTable dt = new DataTable();
            String sql = "Update " + cpt.table + " Set " +
                "" + cpt.parity + "='3'" +
                "," + cpt.date_cancel + "=now() " +
                "," + cpt.user_cancel + "='" + userIdVoid + "' " +
                "Where " + cpt.pkField + "='" + stfId + "'";
            conn.ExecuteNonQuery(sql);

            return "1";
        }
        public DataTable selectAll()
        {
            DataTable dt = new DataTable();
            String sql = "select cop.*  " +
                "From " + cpt.table + " cop " +
                " " +
                "Where cop." + cpt.parity + " ='1' ";
            dt = conn.selectData(sql);

            return dt;
        }
        public DataTable selectByPk(String copId)
        {
            DataTable dt = new DataTable();
            String sql = "select cpt.* " +
                "From " + cpt.table + " cpt " +
                "Where cpt." + cpt.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            return dt;
        }
        public ComPort selectByPk1(String copId)
        {
            ComPort cop1 = new ComPort();
            DataTable dt = new DataTable();
            String sql = "select cpt.* " +
                "From " + cpt.table + " cpt " +
                "Where cpt." + cpt.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            cop1 = setComPort(dt);
            return cop1;
        }
        public ComPort setComPort(DataTable dt)
        {
            ComPort stf1 = new ComPort();
            if (dt.Rows.Count > 0)
            {
                stf1.id = dt.Rows[0][cpt.id].ToString();
                stf1.baud_rate = dt.Rows[0][cpt.baud_rate].ToString();
                stf1.parity = dt.Rows[0][cpt.parity].ToString();
                stf1.stop_bit = dt.Rows[0][cpt.stop_bit].ToString();
                stf1.data_bit = dt.Rows[0][cpt.data_bit].ToString();
            }
            else
            {
                setComPort1(stf1);
            }
            return stf1;
        }
        private ComPort setComPort1(ComPort stf1)
        {
            stf1.id = "";
            stf1.baud_rate = "";
            stf1.parity = "";
            stf1.stop_bit = "";
            stf1.data_bit = "";

            return stf1;
        }
        public void setCboRoute(ComboBox c, String selected)
        {
            ComboBoxItem item = new ComboBoxItem();
            //DataTable dt = selectWard();
            c.Items.Clear();
            int i = 0;
            if (lCpt.Count <= 0) getlComPort();
            item = new ComboBoxItem();
            item.Value = "";
            item.Text = "";
            c.Items.Add(item);
            foreach (ComPort cus1 in lCpt)
            {
                item = new ComboBoxItem();
                item.Value = cus1.id;
                item.Text = cus1.baud_rate;
                c.Items.Add(item);
                if (item.Value.Equals(selected))
                {
                    //c.SelectedItem = item.Value;
                    c.SelectedText = item.Text;
                    c.SelectedIndex = i + 1;
                }
                i++;
            }
        }
    }
}
