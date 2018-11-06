using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eb_20_test2.obgdb
{
    public class EB20DB
    {
        public ConnectDB conn;
        public StaffDB stfDB;
        public RouteDB rouDB;
        public PlaceDB plcDB;
        public LocationDB lctDB;
        public Count1DB cntDB;
        public ComPortDB cptDB;

        public EB20DB(ConnectDB c) {
            conn = c;
            initConfig();
            
        }
        private void initConfig()
        {
            stfDB = new StaffDB(conn);
            rouDB = new RouteDB(conn);
            plcDB = new PlaceDB(conn);
            lctDB = new LocationDB(conn);
            cntDB = new Count1DB(conn);
            cptDB = new ComPortDB(conn);
        }
    }
}
