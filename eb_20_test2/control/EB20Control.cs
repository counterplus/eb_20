using eb_20_test2.obgdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb_20_test2.control
{
    public class EB20Control
    {
        public EB20DB eB20DB;
        public ConnectDB conn;
        public String appName = "";
        public int B20 = 20, B50 = 50, B100 = 100, B500 = 500, B1000 = 1000;

        public EB20Control()
        {
            appName = System.AppDomain.CurrentDomain.FriendlyName;
            appName = appName.ToLower().Replace(".exe", "");

            conn = new ConnectDB();
            //eB20DB = new EB20DB(conn);
        }
        public void setCombo(ComboBox c, String data)
        {
            if (c.Items.Count == 0) return;
            c.SelectedIndex = c.SelectedItem == null ? 0 : c.SelectedIndex;
            //foreach (Items item in c.Items)
            //{
            //    if (item.Value.Equals(data))
            //    {
            //        c.SelectedItem = item;
            //        break;
            //    }
            //}
        }
    }
}
