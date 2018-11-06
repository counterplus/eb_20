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
    public class StaffDB
    {
        public Staff stf;
        ConnectDB conn;
        public List<Staff> lStf;

        public StaffDB(ConnectDB c)
        {
            conn = c;
            initConfig();
        }
        private void initConfig()
        {
            lStf = new List<Staff>();
            stf = new Staff();
            stf.active = "active";
            stf.remark = "remark";
            stf.staff_id = "staff_id";
            stf.staff_name = "staff_name";

            stf.table = "b_staff";
            stf.pkField = "staff_id";
        }
        private void chkNull(Staff p)
        {
            int chk = 0;

            p.active = p.active == null ? "" : p.active;
            p.remark = p.remark == null ? "" : p.remark;
            p.staff_name = p.staff_name == null ? "" : p.staff_name;           

            //p.item_sub_group_id = int.TryParse(p.item_sub_group_id, out chk) ? chk.ToString() : "0";
            
        }
        public String insert(Staff p, String userId)
        {
            String re = "";
            String sql = "";
            p.active = "1";
            //p.ssdata_id = "";
            int chk = 0;

            chkNull(p);
            sql = "Insert Into " + stf.table + "(" + stf.staff_name + "," + stf.remark + "," + stf.active + " " +

                ") " +
                "Values ('" + p.staff_name.Replace("'", "''") + "','" + p.remark.Replace("'", "''") + "','" + p.active + "' " +

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
        public String update(Staff p, String userId)
        {
            String re = "";
            String sql = "";
            int chk = 0;

            chkNull(p);
            sql = "Update " + stf.table + " Set " +
                " " + stf.staff_name + " = '" + p.staff_name + "'" +
                "," + stf.remark + " = '" + p.remark.Replace("'", "''") + "'" +
                
                "Where " + stf.pkField + "='" + p.staff_id + "'"
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
        public String insertStaff(Staff p, String userId)
        {
            String re = "";

            if (p.staff_id.Equals(""))
            {
                re = insert(p, "");
            }
            else
            {
                re = update(p, "");
            }

            return re;
        }
        public void getlStf()
        {
            //lDept = new List<Position>();

            lStf.Clear();
            DataTable dt = new DataTable();
            dt = selectAll();
            foreach (DataRow row in dt.Rows)
            {
                Staff stf1 = new Staff();
                stf1.staff_id = row[stf.staff_id].ToString();
                stf1.staff_name = row[stf.staff_name].ToString();
                
                lStf.Add(stf1);
            }
        }
        
        public String getIdByName(String name)
        {
            String id = "";
            foreach (Staff stf1 in lStf)
            {
                if (name.Trim().Equals(stf1.staff_name))
                {
                    id = stf1.staff_id;
                    break;
                }
            }
            return id;
        }
        public String VoidStaff(String stfId, String userIdVoid)
        {
            DataTable dt = new DataTable();
            String sql = "Update " + stf.table + " Set " +
                "" + stf.active + "='3'" +
                "," + stf.date_cancel + "=now() " +
                "," + stf.user_cancel + "='" + userIdVoid + "' " +
                "Where " + stf.pkField + "='" + stfId + "'";
            conn.ExecuteNonQuery(sql);

            return "1";
        }
        public DataTable selectAll()
        {
            DataTable dt = new DataTable();
            String sql = "select stf.*  " +
                "From " + stf.table + " stf " +
                " " +
                "Where stf." + stf.active + " ='1' ";
            dt = conn.selectData(sql);

            return dt;
        }
        public DataTable selectByPk(String copId)
        {
            DataTable dt = new DataTable();
            String sql = "select stf.* " +
                "From " + stf.table + " stf " +
                "Where stf." + stf.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            return dt;
        }
        public Staff selectByPk1(String copId)
        {
            Staff cop1 = new Staff();
            DataTable dt = new DataTable();
            String sql = "select stf.* " +
                "From " + stf.table + " stf " +                
                "Where stf." + stf.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            cop1 = setStaff(dt);
            return cop1;
        }
        public Staff setStaff(DataTable dt)
        {
            Staff stf1 = new Staff();
            if (dt.Rows.Count > 0)
            {
                stf1.staff_id = dt.Rows[0][stf.staff_id].ToString();
                stf1.staff_name = dt.Rows[0][stf.staff_name].ToString();
                
            }
            else
            {
                setStaff1(stf1);
            }
            return stf1;
        }
        private Staff setStaff1(Staff stf1)
        {
            stf1.staff_id = "";
            stf1.staff_name = "";
            
            return stf1;
        }
        public void setCboStaff(ComboBox c, String selected)
        {
            ComboBoxItem item = new ComboBoxItem();
            //DataTable dt = selectWard();
            c.Items.Clear();
            int i = 0;
            if (lStf.Count <= 0) getlStf();
            item = new ComboBoxItem();
            item.Value = "";
            item.Text = "";
            c.Items.Add(item);
            foreach (Staff cus1 in lStf)
            {
                item = new ComboBoxItem();
                item.Value = cus1.staff_id;
                item.Text = cus1.staff_name ;
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
