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
    public class PlaceDB
    {
        public Place plc;
        ConnectDB conn;
        public List<Place> lPlc;

        public PlaceDB(ConnectDB c)
        {
            conn = c;
            initConfig();
        }
        private void initConfig()
        {
            lPlc = new List<Place>();
            plc = new Place();
            plc.active = "active";
            plc.remark = "remark";
            plc.place_id = "place_id";
            plc.place_name = "place_name";

            plc.table = "b_place";
            plc.pkField = "place_id";
        }
        private void chkNull(Place p)
        {
            int chk = 0;

            p.active = p.active == null ? "" : p.active;
            p.remark = p.remark == null ? "" : p.remark;
            p.place_name = p.place_name == null ? "" : p.place_name;

            //p.item_sub_group_id = int.TryParse(p.item_sub_group_id, out chk) ? chk.ToString() : "0";

        }
        public String insert(Place p, String userId)
        {
            String re = "";
            String sql = "";
            p.active = "1";
            //p.ssdata_id = "";
            int chk = 0;

            chkNull(p);
            sql = "Insert Into " + plc.table + "(" + plc.place_name + "," + plc.remark + "," + plc.active + " " +

                ") " +
                "Values ('" + p.place_name.Replace("'", "''") + "','" + p.remark.Replace("'", "''") + "','" + p.active + "' " +

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
        public String update(Place p, String userId)
        {
            String re = "";
            String sql = "";
            int chk = 0;

            chkNull(p);
            sql = "Update " + plc.table + " Set " +
                " " + plc.place_name + " = '" + p.place_name + "'" +
                "," + plc.remark + " = '" + p.remark.Replace("'", "''") + "'" +

                "Where " + plc.pkField + "='" + p.place_id + "'"
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
        public String insertPlace(Place p, String userId)
        {
            String re = "";

            if (p.place_id.Equals(""))
            {
                re = insert(p, "");
            }
            else
            {
                re = update(p, "");
            }

            return re;
        }
        public void getlPlc()
        {
            //lDept = new List<Position>();

            lPlc.Clear();
            DataTable dt = new DataTable();
            dt = selectAll();
            foreach (DataRow row in dt.Rows)
            {
                Place stf1 = new Place();
                stf1.place_id = row[plc.place_id].ToString();
                stf1.place_name = row[plc.place_name].ToString();

                lPlc.Add(stf1);
            }
        }

        public String getIdByName(String name)
        {
            String id = "";
            foreach (Place stf1 in lPlc)
            {
                if (name.Trim().Equals(stf1.place_name))
                {
                    id = stf1.place_id;
                    break;
                }
            }
            return id;
        }
        public String VoidPlace(String stfId, String userIdVoid)
        {
            DataTable dt = new DataTable();
            String sql = "Update " + plc.table + " Set " +
                "" + plc.active + "='3'" +
                "," + plc.date_cancel + "=now() " +
                "," + plc.user_cancel + "='" + userIdVoid + "' " +
                "Where " + plc.pkField + "='" + stfId + "'";
            conn.ExecuteNonQuery(sql);

            return "1";
        }
        public DataTable selectAll()
        {
            DataTable dt = new DataTable();
            String sql = "select cop.*  " +
                "From " + plc.table + " cop " +
                " " +
                "Where cop." + plc.active + " ='1' ";
            dt = conn.selectData(sql);

            return dt;
        }
        public DataTable selectByPk(String copId)
        {
            DataTable dt = new DataTable();
            String sql = "select plc.* " +
                "From " + plc.table + " plc " +
                "Where plc." + plc.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            return dt;
        }
        public Place selectByPk1(String copId)
        {
            Place cop1 = new Place();
            DataTable dt = new DataTable();
            String sql = "select plc.* " +
                "From " + plc.table + " plc " +
                "Where plc." + plc.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            cop1 = setPlace(dt);
            return cop1;
        }
        public Place setPlace(DataTable dt)
        {
            Place stf1 = new Place();
            if (dt.Rows.Count > 0)
            {
                stf1.place_id = dt.Rows[0][plc.place_id].ToString();
                stf1.place_name = dt.Rows[0][plc.place_name].ToString();

            }
            else
            {
                setPlace1(stf1);
            }
            return stf1;
        }
        private Place setPlace1(Place stf1)
        {
            stf1.place_id = "";
            stf1.place_name = "";

            return stf1;
        }
        public void setCboPlace(ComboBox c, String selected)
        {
            ComboBoxItem item = new ComboBoxItem();
            //DataTable dt = selectWard();
            c.Items.Clear();
            int i = 0;
            if (lPlc.Count <= 0) getlPlc();
            item = new ComboBoxItem();
            item.Value = "";
            item.Text = "";
            c.Items.Add(item);
            foreach (Place cus1 in lPlc)
            {
                item = new ComboBoxItem();
                item.Value = cus1.place_id;
                item.Text = cus1.place_name;
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
