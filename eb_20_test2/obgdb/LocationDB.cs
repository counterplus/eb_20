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
    public class LocationDB
    {
        public Location lct;
        ConnectDB conn;
        public List<Location> lLct;

        public LocationDB(ConnectDB c)
        {
            conn = c;
            initConfig();
        }
        private void initConfig()
        {
            lLct = new List<Location>();
            lct = new Location();
            lct.active = "active";
            lct.remark = "remark";
            lct.loca_id = "loca_id";
            lct.loca_name = "loca_name";

            lct.table = "b_location";
            lct.pkField = "loca_id";
        }
        private void chkNull(Location p)
        {
            int chk = 0;

            p.active = p.active == null ? "" : p.active;
            p.remark = p.remark == null ? "" : p.remark;
            p.loca_name = p.loca_name == null ? "" : p.loca_name;

            //p.item_sub_group_id = int.TryParse(p.item_sub_group_id, out chk) ? chk.ToString() : "0";

        }
        public String insert(Location p, String userId)
        {
            String re = "";
            String sql = "";
            p.active = "1";
            //p.ssdata_id = "";
            int chk = 0;

            chkNull(p);
            sql = "Insert Into " + lct.table + "(" + lct.loca_name + "," + lct.remark + "," + lct.active + " " +

                ") " +
                "Values ('" + p.loca_name.Replace("'", "''") + "','" + p.remark.Replace("'", "''") + "','" + p.active + "' " +

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
        public String update(Location p, String userId)
        {
            String re = "";
            String sql = "";
            int chk = 0;

            chkNull(p);
            sql = "Update " + lct.table + " Set " +
                " " + lct.loca_name + " = '" + p.loca_name + "'" +
                "," + lct.remark + " = '" + p.remark.Replace("'", "''") + "'" +

                "Where " + lct.pkField + "='" + p.loca_id + "'"
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
        public String insertLocation(Location p, String userId)
        {
            String re = "";

            if (p.loca_id.Equals(""))
            {
                re = insert(p, "");
            }
            else
            {
                re = update(p, "");
            }

            return re;
        }
        public void getlLct()
        {
            //lDept = new List<Position>();

            lLct.Clear();
            DataTable dt = new DataTable();
            dt = selectAll();
            foreach (DataRow row in dt.Rows)
            {
                Location stf1 = new Location();
                stf1.loca_id = row[lct.loca_id].ToString();
                stf1.loca_name = row[lct.loca_name].ToString();

                lLct.Add(stf1);
            }
        }

        public String getIdByName(String name)
        {
            String id = "";
            foreach (Location stf1 in lLct)
            {
                if (name.Trim().Equals(stf1.loca_name))
                {
                    id = stf1.loca_id;
                    break;
                }
            }
            return id;
        }
        public String VoidLocation(String stfId, String userIdVoid)
        {
            DataTable dt = new DataTable();
            String sql = "Update " + lct.table + " Set " +
                "" + lct.active + "='3'" +
                "," + lct.date_cancel + "=now() " +
                "," + lct.user_cancel + "='" + userIdVoid + "' " +
                "Where " + lct.pkField + "='" + stfId + "'";
            conn.ExecuteNonQuery(sql);

            return "1";
        }
        public DataTable selectAll()
        {
            DataTable dt = new DataTable();
            String sql = "select cop.*  " +
                "From " + lct.table + " cop " +
                " " +
                "Where cop." + lct.active + " ='1' ";
            dt = conn.selectData(sql);

            return dt;
        }
        public DataTable selectByPk(String copId)
        {
            DataTable dt = new DataTable();
            String sql = "select lct.* " +
                "From " + lct.table + " lct " +
                "Where lct." + lct.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            return dt;
        }
        public Location selectByPk1(String copId)
        {
            Location cop1 = new Location();
            DataTable dt = new DataTable();
            String sql = "select lct.* " +
                "From " + lct.table + " lct " +
                "Where lct." + lct.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            cop1 = setLocation(dt);
            return cop1;
        }
        public Location setLocation(DataTable dt)
        {
            Location stf1 = new Location();
            if (dt.Rows.Count > 0)
            {
                stf1.loca_id = dt.Rows[0][lct.loca_id].ToString();
                stf1.loca_name = dt.Rows[0][lct.loca_name].ToString();

            }
            else
            {
                setLocation1(stf1);
            }
            return stf1;
        }
        private Location setLocation1(Location stf1)
        {
            stf1.loca_id = "";
            stf1.loca_name = "";

            return stf1;
        }
        public void setCboLocation(ComboBox c, String selected)
        {
            ComboBoxItem item = new ComboBoxItem();
            //DataTable dt = selectWard();
            c.Items.Clear();
            int i = 0;
            if (lLct.Count <= 0) getlLct();
            item = new ComboBoxItem();
            item.Value = "";
            item.Text = "";
            c.Items.Add(item);
            foreach (Location cus1 in lLct)
            {
                item = new ComboBoxItem();
                item.Value = cus1.loca_id;
                item.Text = cus1.loca_name;
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
