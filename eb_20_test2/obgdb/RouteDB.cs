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
    public class RouteDB
    {
        public Route rou;
        ConnectDB conn;
        public List<Route> lRou;

        public RouteDB(ConnectDB c)
        {
            conn = c;
            initConfig();
        }
        private void initConfig()
        {
            lRou = new List<Route>();
            rou = new Route();
            rou.active = "active";
            rou.remark = "remark";
            rou.rou_id = "rou_id";
            rou.rou_name = "rou_name";

            rou.table = "b_route";
            rou.pkField = "rou_id";
        }
        private void chkNull(Route p)
        {
            int chk = 0;

            p.active = p.active == null ? "" : p.active;
            p.remark = p.remark == null ? "" : p.remark;
            p.rou_name = p.rou_name == null ? "" : p.rou_name;

            //p.item_sub_group_id = int.TryParse(p.item_sub_group_id, out chk) ? chk.ToString() : "0";

        }
        public String insert(Route p, String userId)
        {
            String re = "";
            String sql = "";
            p.active = "1";
            //p.ssdata_id = "";
            int chk = 0;

            chkNull(p);
            sql = "Insert Into " + rou.table + "(" + rou.rou_name + "," + rou.remark + "," + rou.active + " " +
                ") " +
                "Values ('" + p.rou_name.Replace("'", "''") + "','" + p.remark.Replace("'","''") + "','" + p.active + "' " +
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
        public String update(Route p, String userId)
        {
            String re = "";
            String sql = "";
            int chk = 0;

            chkNull(p);
            sql = "Update " + rou.table + " Set " +
                " " + rou.rou_name + " = '" + p.rou_name + "'" +
                "," + rou.remark + " = '" + p.remark.Replace("'", "''") + "'" +

                "Where " + rou.pkField + "='" + p.rou_id + "'"
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
        public String insertRoute(Route p, String userId)
        {
            String re = "";

            if (p.rou_id.Equals(""))
            {
                re = insert(p, "");
            }
            else
            {
                re = update(p, "");
            }

            return re;
        }
        public void getlRou()
        {
            //lDept = new List<Position>();

            lRou.Clear();
            DataTable dt = new DataTable();
            dt = selectAll();
            foreach (DataRow row in dt.Rows)
            {
                Route stf1 = new Route();
                stf1.rou_id = row[rou.rou_id].ToString();
                stf1.rou_name = row[rou.rou_name].ToString();

                lRou.Add(stf1);
            }
        }

        public String getIdByName(String name)
        {
            String id = "";
            foreach (Route stf1 in lRou)
            {
                if (name.Trim().Equals(stf1.rou_name))
                {
                    id = stf1.rou_id;
                    break;
                }
            }
            return id;
        }
        public String VoidRoute(String stfId, String userIdVoid)
        {
            DataTable dt = new DataTable();
            String sql = "Update " + rou.table + " Set " +
                "" + rou.active + "='3'" +
                "," + rou.date_cancel + "=now() " +
                "," + rou.user_cancel + "='" + userIdVoid + "' " +
                "Where " + rou.pkField + "='" + stfId + "'";
            conn.ExecuteNonQuery(sql);

            return "1";
        }
        public DataTable selectAll()
        {
            DataTable dt = new DataTable();
            String sql = "select cop.*  " +
                "From " + rou.table + " cop " +
                " " +
                "Where cop." + rou.active + " ='1' ";
            dt = conn.selectData(sql);

            return dt;
        }
        public DataTable selectByPk(String copId)
        {
            DataTable dt = new DataTable();
            String sql = "select rou.* " +
                "From " + rou.table + " rou " +
                "Where rou." + rou.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            return dt;
        }
        public Route selectByPk1(String copId)
        {
            Route cop1 = new Route();
            DataTable dt = new DataTable();
            String sql = "select rou.* " +
                "From " + rou.table + " rou " +
                "Where rou." + rou.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            cop1 = setRoute(dt);
            return cop1;
        }
        public Route setRoute(DataTable dt)
        {
            Route stf1 = new Route();
            if (dt.Rows.Count > 0)
            {
                stf1.rou_id = dt.Rows[0][rou.rou_id].ToString();
                stf1.rou_name = dt.Rows[0][rou.rou_name].ToString();

            }
            else
            {
                setRoute1(stf1);
            }
            return stf1;
        }
        private Route setRoute1(Route stf1)
        {
            stf1.rou_id = "";
            stf1.rou_name = "";

            return stf1;
        }
        public void setCboRoute(ComboBox c, String selected)
        {
            ComboBoxItem item = new ComboBoxItem();
            //DataTable dt = selectWard();
            c.Items.Clear();
            int i = 0;
            if (lRou.Count <= 0) getlRou();
            item = new ComboBoxItem();
            item.Value = "";
            item.Text = "";
            c.Items.Add(item);
            foreach (Route cus1 in lRou)
            {
                item = new ComboBoxItem();
                item.Value = cus1.rou_id;
                item.Text = cus1.rou_name;
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
