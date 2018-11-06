using eb_20_test2.object1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eb_20_test2.obgdb
{
    public class Count1DB
    {
        public Count1 cnt;
        ConnectDB conn;
        public List<Count1> lStf;

        public Count1DB(ConnectDB c)
        {
            conn = c;
            initConfig();
        }
        private void initConfig()
        {
            lStf = new List<Count1>();
            cnt = new Count1();
            cnt.active = "active";
            cnt.remark = "remark";
            cnt.count_id = "count_id";
            cnt.count_date = "count_date";
            cnt.staff_name = "staff_name";
            cnt.rou_name = "rou_name";
            cnt.loca_name = "loca_name";
            cnt.place_name = "place_name";
            cnt.c1 = "c1";
            cnt.c2 = "c2";
            cnt.c5 = "c5";
            cnt.c10 = "c10";
            cnt.b20 = "b20";
            cnt.b50 = "b50";
            cnt.b100 = "b100";
            cnt.b500 = "b500";
            cnt.b1000 = "b1000";
            cnt.plus1 = "plus1";
            cnt.minus1 = "minus1";
            cnt.amount = "amount";

            cnt.table = "t_count";
            cnt.pkField = "count_id";
        }
        private void chkNull(Count1 p)
        {
            int chk = 0;

            p.active = p.active == null ? "" : p.active;
            p.remark = p.remark == null ? "" : p.remark;
            p.staff_name = p.staff_name == null ? "" : p.staff_name;
            p.count_date = p.count_date == null ? "" : p.count_date;
            p.rou_name = p.rou_name == null ? "" : p.rou_name;
            p.loca_name = p.loca_name == null ? "" : p.loca_name;
            p.place_name = p.place_name == null ? "" : p.place_name;

            p.c1 = p.c1 == null ? "" : p.c1;
            p.c2 = p.c2 == null ? "" : p.c2;
            p.c5 = p.c5 == null ? "" : p.c5;
            p.c10 = p.c10 == null ? "" : p.c10;
            p.b20 = p.b20 == null ? "" : p.b20;
            p.b50 = p.b50 == null ? "" : p.b50;
            p.b100 = p.b100 == null ? "" : p.b100;
            p.b500 = p.b500 == null ? "" : p.b500;
            p.b1000 = p.b1000 == null ? "" : p.b1000;
            p.plus1 = p.plus1 == null ? "" : p.plus1;
            p.minus1 = p.minus1 == null ? "" : p.minus1;
            p.amount = p.amount == null ? "" : p.amount;

            //p.item_sub_group_id = int.TryParse(p.item_sub_group_id, out chk) ? chk.ToString() : "0";

        }
        public String insert(Count1 p, String userId)
        {
            String re = "";
            String sql = "";
            p.active = "1";
            //p.ssdata_id = "";
            int chk = 0;

            chkNull(p);
            sql = "Insert Into " + cnt.table + "(" + cnt.staff_name + "," + cnt.remark + "," + cnt.active + "," +
                "" + cnt.count_date + "," + cnt.rou_name + "," + cnt.c5 + "," +
                "" + cnt.loca_name + "," + cnt.place_name + "," + cnt.c1 + "," +
                "" + cnt.c2 + "," + cnt.c10 + "," + cnt.b20 + "," +
                "" + cnt.b50 + "," + cnt.b100 + "," + cnt.b500 + "," +
                "" + cnt.b1000 + "," + cnt.plus1 + "," + cnt.minus1 + ", " +
                "" + cnt.amount + " " +
                ") " +
                "Values ('" + p.staff_name.Replace("'", "''") + "','" + p.remark.Replace("'", "''") + "','" + p.active + "'," +
                "'" + "" + p.count_date.Replace("'", "''") + "','" + p.rou_name.Replace("'", "''") + "','" + p.c5 + "'," +
                "'" + p.loca_name.Replace("'", "''") + "','" + p.place_name.Replace("'", "''") + "','" + p.c1 + "'," +
                "'" + p.c2.Replace("'", "''") + "','" + p.c10.Replace("'", "''") + "','" + p.b20 + "'," +
                "'" + p.b50.Replace("'", "''") + "','" + p.b100.Replace("'", "''") + "','" + p.b500 + "'," +
                "'" + p.b1000.Replace("'", "''") + "','" + p.plus1.Replace("'", "''") + "','" + p.minus1 + "', " +
                "'" + p.amount.Replace("'", "''") + "' " +
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
        public String update(Count1 p, String userId)
        {
            String re = "";
            String sql = "";
            int chk = 0;

            chkNull(p);
            sql = "Update " + cnt.table + " Set " +
                " " + cnt.staff_name + " = '" + p.staff_name + "'" +
                "," + cnt.count_date + " = '" + p.count_date.Replace("'", "''") + "'" +
                "," + cnt.rou_name + " = '" + p.rou_name.Replace("'", "''") + "'" +
                "," + cnt.loca_name + " = '" + p.loca_name.Replace("'", "''") + "'" +
                "," + cnt.place_name + " = '" + p.place_name.Replace("'", "''") + "'" +
                "," + cnt.c1 + " = '" + p.c1.Replace("'", "''") + "'" +
                "," + cnt.c2 + " = '" + p.c2.Replace("'", "''") + "'" +
                "," + cnt.c10 + " = '" + p.c10.Replace("'", "''") + "'" +
                "," + cnt.b20 + " = '" + p.b20.Replace("'", "''") + "'" +
                "," + cnt.b50 + " = '" + p.b50.Replace("'", "''") + "'" +
                "," + cnt.b100 + " = '" + p.b100.Replace("'", "''") + "'" +
                "," + cnt.b500 + " = '" + p.b500.Replace("'", "''") + "'" +
                "," + cnt.b1000 + " = '" + p.b1000.Replace("'", "''") + "'" +
                "," + cnt.plus1 + " = '" + p.plus1.Replace("'", "''") + "'" +
                "," + cnt.minus1 + " = '" + p.minus1.Replace("'", "''") + "'" +
                "," + cnt.remark + " = '" + p.remark.Replace("'", "''") + "'" +
                "," + cnt.c5 + " = '" + p.c5.Replace("'", "''") + "'" +
                "," + cnt.amount + " = '" + p.amount.Replace("'", "''") + "'" +
                "Where " + cnt.pkField + "='" + p.count_id + "'"
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
        public String insertCount(Count1 p, String userId)
        {
            String re = "";

            if (p.count_id.Equals(""))
            {
                re = insert(p, "");
            }
            else
            {
                re = update(p, "");
            }

            return re;
        }
        public void getlCnt()
        {
            //lDept = new List<Position>();

            lStf.Clear();
            DataTable dt = new DataTable();
            dt = selectAll();
            foreach (DataRow row in dt.Rows)
            {
                Count1 stf1 = new Count1();
                stf1.count_id = row[cnt.count_id].ToString();
                stf1.count_date = row[cnt.count_date].ToString();

                lStf.Add(stf1);
            }
        }

        public String getIdByName(String name)
        {
            String id = "";
            foreach (Count1 stf1 in lStf)
            {
                if (name.Trim().Equals(stf1.staff_name))
                {
                    id = stf1.count_id;
                    break;
                }
            }
            return id;
        }
        public String VoidCount(String stfId, String userIdVoid)
        {
            DataTable dt = new DataTable();
            String sql = "Update " + cnt.table + " Set " +
                "" + cnt.active + "='3'" +
                "," + cnt.date_cancel + "=now() " +
                "," + cnt.user_cancel + "='" + userIdVoid + "' " +
                "Where " + cnt.pkField + "='" + stfId + "'";
            conn.ExecuteNonQuery(sql);

            return "1";
        }
        public DataTable selectAll()
        {
            DataTable dt = new DataTable();
            String sql = "select cnt.*  " +
                "From " + cnt.table + " cnt " +
                " " +
                "Where cnt." + cnt.active + " ='1' ";
            dt = conn.selectData(sql);

            return dt;
        }
        public DataTable selectByDate(String date)
        {
            DataTable dt = new DataTable();
            String sql = "select cnt.*  " +
                "From " + cnt.table + " cnt " +
                " " +
                "Where cnt." + cnt.active + " ='1' and cnt."+cnt.count_date+"='"+date+"'";
            dt = conn.selectData(sql);

            return dt;
        }
        public DataTable selectByPk(String copId)
        {
            DataTable dt = new DataTable();
            String sql = "select cnt.* " +
                "From " + cnt.table + " cnt " +
                "Where cnt." + cnt.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            return dt;
        }
        public Count1 selectByPk1(String copId)
        {
            Count1 cop1 = new Count1();
            DataTable dt = new DataTable();
            String sql = "select cnt.* " +
                "From " + cnt.table + " cnt " +
                "Where cnt." + cnt.pkField + " ='" + copId + "' ";
            dt = conn.selectData(sql);
            cop1 = setCount(dt);
            return cop1;
        }
        public Count1 setCount(DataTable dt)
        {
            Count1 stf1 = new Count1();
            if (dt.Rows.Count > 0)
            {
                stf1.count_id = dt.Rows[0][cnt.count_id].ToString();
                stf1.staff_name = dt.Rows[0][cnt.staff_name].ToString();
                stf1.active = dt.Rows[0][cnt.active].ToString();
                stf1.remark = dt.Rows[0][cnt.remark].ToString();
                stf1.count_date = dt.Rows[0][cnt.count_date].ToString();
                stf1.c1 = dt.Rows[0][cnt.c1].ToString();
                stf1.rou_name = dt.Rows[0][cnt.rou_name].ToString();
                stf1.loca_name = dt.Rows[0][cnt.loca_name].ToString();
                stf1.place_name = dt.Rows[0][cnt.place_name].ToString();
                stf1.c2 = dt.Rows[0][cnt.c2].ToString();
                stf1.c5 = dt.Rows[0][cnt.c5].ToString();
                stf1.c10 = dt.Rows[0][cnt.c10].ToString();
                stf1.b20 = dt.Rows[0][cnt.b20].ToString();
                stf1.b50 = dt.Rows[0][cnt.b50].ToString();
                stf1.b100 = dt.Rows[0][cnt.b100].ToString();
                stf1.b500 = dt.Rows[0][cnt.b500].ToString();
                stf1.b1000 = dt.Rows[0][cnt.b1000].ToString();
                stf1.plus1 = dt.Rows[0][cnt.plus1].ToString();
                stf1.minus1 = dt.Rows[0][cnt.minus1].ToString();
                stf1.amount = dt.Rows[0][cnt.amount].ToString();
            }
            else
            {
                setCount1(stf1);
            }
            return stf1;
        }
        private Count1 setCount1(Count1 stf1)
        {
            stf1.active = "";
            stf1.remark = "";
            stf1.count_id = "";
            stf1.count_date = "";
            stf1.staff_name = "";
            stf1.rou_name = "";
            stf1.loca_name = "";
            stf1.place_name = "";
            stf1.c1 = "";
            stf1.c2 = "";
            stf1.c5 = "";
            stf1.c10 = "";
            stf1.b20 = "";
            stf1.b50 = "";
            stf1.b100 = "";
            stf1.b500 = "";
            stf1.b1000 = "";
            stf1.plus1 = "";
            stf1.minus1 = "";
            stf1.amount = "";
            return stf1;
        }
        
    }
}
