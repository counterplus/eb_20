﻿using eb_20_test2.control;
using eb_20_test2.gui;
using eb_20_test2.object1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb_20_test2
{
    public partial class Form1 : Form
    {
        EB20Control ebC;
        Count1 cnt;
        CommunicationManager comm = new CommunicationManager();
        byte iComState = 0;
        byte iRecvAck = 1;
        int colId = 0, colDate = 1,colRou=2, colLoca=3, colPlc=4, colC1=5, colC2=6, colC5=7, colC10=8, colB20=9,colB50=10, colB100=11, colB500=12, colB1000=13, colAmt=14, colPlus=15, colMinus=16,colStf=15;
        String[] strKey = {"num0", "num1", "num2", "num3", "num4", "num5", "num6", "num7", "num8", "num9", "upar","downar",
            "cf","bat","level","add","mode","cur","clear","start"};
        enum KEY_NAME
        {
            KEY_NONE = 0, KEY_N0, KEY_N1, KEY_N2, KEY_N3, KEY_N4, KEY_N5, KEY_N6, KEY_N7, KEY_N8, KEY_N9, KEY_UP, KEY_DOWN,
            KEY_CF, KEY_BAT, KEY_LEVEL, KEY_ADD, KEY_MODE, KEY_CUR, KEY_CLEAR, KEY_START
        };
        KEY_NAME iKey = KEY_NAME.KEY_NONE;

        public Form1(EB20Control ebc)
        {
            InitializeComponent();
            ebC = ebc;
            initConfig();

        }
        private void initConfig()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                //ebC.eB20DB = new obgdb.EB20DB(ebC.conn);
                //ebC.eB20DB.stfDB.setCboStaff(cboStf, "");
                //ebC.eB20DB.lctDB.setCboLocation(cboStf, "");
                //ebC.eB20DB.plcDB.setCboPlace(cboStf, "");
                //ebC.eB20DB.rouDB.setCboRoute(cboStf, "");

            }).Start();

            ebC.eB20DB = new obgdb.EB20DB(ebC.conn);
            ebC.eB20DB.stfDB.setCboStaff(cboStf, "");
            ebC.eB20DB.lctDB.setCboLocation(cboLoca, "");
            ebC.eB20DB.plcDB.setCboPlace(cboPlc, "");
            ebC.eB20DB.rouDB.setCboRoute(cboRou, "");

            LoadValues();
            setGrd();
            BT_PORT_OC.Click += BT_PORT_OC_Click;
            BT_REM_SET.Click += BT_REM_SET_Click;
            rtbDisplay.TextChanged += RtbDisplay_TextChanged;
            BT_KEY_UP.Click += BT_KEY_UP_Click;
            BT_KEY_CLEAR.Click += BT_KEY_CLEAR_Click;
            BT_KEY_DOWN.Click += BT_KEY_DOWN_Click;
            BT_KEY_START.Click += BT_KEY_START_Click;
            BT_REQ_INFO.Click += BT_REQ_INFO_Click;
            BT_CO_SET.Click += BT_CO_SET_Click;
            BT_KEY_MODE.Click += BT_KEY_MODE_Click;
            BT_KEY_CUR.Click += BT_KEY_CUR_Click;
            BT_KEY_LEVEL.Click += BT_KEY_LEVEL_Click;
            BT_KEY_ADD.Click += BT_KEY_ADD_Click;
            BT_KEY_CF.Click += BT_KEY_CF_Click;
            BT_KEY_BAT.Click += BT_KEY_BAT_Click;
            BT_BAT_SET.Click += BT_BAT_SET_Click;
            BT_KEY_N1.Click += BT_KEY_N1_Click;
            BT_KEY_N2.Click += BT_KEY_N2_Click;
            BT_KEY_N3.Click += BT_KEY_N3_Click;
            BT_KEY_N4.Click += BT_KEY_N4_Click;
            BT_KEY_N5.Click += BT_KEY_N5_Click;
            BT_KEY_N6.Click += BT_KEY_N6_Click;
            BT_KEY_N7.Click += BT_KEY_N7_Click;
            BT_KEY_N8.Click += BT_KEY_N8_Click;
            BT_KEY_N9.Click += BT_KEY_N9_Click;
            BT_KEY_N0.Click += BT_KEY_N0_Click;
            btnSave.Click += BtnSave_Click;
            lbStf.DoubleClick += LbStf_DoubleClick;
            lbRou.DoubleClick += LbRou_DoubleClick;
            lbPlace.DoubleClick += LbPlace_DoubleClick;
            lbLct.DoubleClick += LbLct_DoubleClick;
            txtCoin1.KeyUp += TxtCoin1_KeyUp;
            txtCoin2.KeyUp += TxtCoin2_KeyUp;
            txtCoin5.KeyUp += TxtCoin5_KeyUp;
            txtCoin10.KeyUp += TxtCoin10_KeyUp;
            txtBank20.TextChanged += TxtBank20_TextChanged;
            txtBank50.TextChanged += TxtBank50_TextChanged;
            txtBank100.TextChanged += TxtBank100_TextChanged;
            txtBank500.TextChanged += TxtBank500_TextChanged;
            txtBank1000.TextChanged += TxtBank1000_TextChanged;
            txtMinus.TextChanged += TxtMinus_TextChanged;
            txtPlus.KeyUp += TxtPlus_KeyUp;
            txtMinus.KeyUp += TxtMinus_KeyUp;
            btnPrn.Click += BtnPrn_Click;
        }

        private void BtnPrn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            PrintDialog printdg = new PrintDialog();
            PrintDocument pd_doc = new PrintDocument();
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            printdg.Document = pd_doc;
            //printdg.ShowDialog(this);
            ppd.Document = pd_doc;
            ppd.ShowDialog(this);
        }

        private void TxtMinus_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.KeyCode == Keys.Enter)
            {
                txtRemark.Focus();
            }
        }

        private void TxtPlus_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
            if (e.KeyCode == Keys.Enter)
            {
                txtMinus.Focus();
            }
        }
        private void TxtMinus_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
        }

        private void setGrd()
        {
            Font font = new Font("Verdana", 12);
            DataTable dt = new DataTable();

            grv1.ColumnCount = 18;
            grv1.Rows.Clear();
            grv1.RowCount = 1;
            grv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grv1.Columns[colId].Width = 40;
            grv1.Columns[colDate].Width = 80;
            grv1.Columns[colRou].Width = 100;
            grv1.Columns[colLoca].Width = 100;
            grv1.Columns[colPlc].Width = 100;
            grv1.Columns[colC1].Width = 40;
            grv1.Columns[colC2].Width = 40;
            grv1.Columns[colC5].Width = 40;
            grv1.Columns[colC10].Width = 40;
            grv1.Columns[colB20].Width = 40;
            grv1.Columns[colB50].Width = 40;
            grv1.Columns[colB100].Width = 40;
            grv1.Columns[colB500].Width = 40;
            grv1.Columns[colB1000].Width = 40;
            grv1.Columns[colStf].Width = 80;
            grv1.Columns[colAmt].Width = 100;
            grv1.Columns[colPlus].Width = 80;
            grv1.Columns[colMinus].Width = 80;

            grv1.Columns[colId].HeaderText = "no";
            grv1.Columns[colDate].HeaderText = "Date";
            grv1.Columns[colRou].HeaderText = "Route";
            grv1.Columns[colLoca].HeaderText = "Location";
            grv1.Columns[colPlc].HeaderText = "Place";
            grv1.Columns[colC1].HeaderText = "C1";
            grv1.Columns[colC2].HeaderText = "C2";
            grv1.Columns[colC5].HeaderText = "C5";
            grv1.Columns[colC10].HeaderText = "C10";
            grv1.Columns[colB20].HeaderText = "B20";
            grv1.Columns[colB50].HeaderText = "B50";
            grv1.Columns[colB100].HeaderText = "B100";
            grv1.Columns[colB500].HeaderText = "B500";
            grv1.Columns[colB1000].HeaderText = "B1000";
            grv1.Columns[colStf].HeaderText = "Staff";
            grv1.Columns[colAmt].HeaderText = "Amount";
            grv1.Columns[colPlus].HeaderText = "Plus";
            grv1.Columns[colMinus].HeaderText = "Minus";

            //dgv1.Columns[colPEWeight].HeaderText = "น้ำหนัก";

            dt = ebC.eB20DB.cntDB.selectAll();

            //dgvPE.Columns[colPEId].HeaderText = "id";
            if (dt.Rows.Count > 0)
            {
                grv1.RowCount = dt.Rows.Count;
                for (int i = 0; i < grv1.RowCount; i++)
                {
                    grv1[colId, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.count_id].ToString();
                    grv1[colDate, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.count_date].ToString();
                    grv1[colRou, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.rou_name].ToString();
                    grv1[colLoca, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.loca_name].ToString();
                    grv1[colPlc, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.place_name].ToString();
                    grv1[colC1, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.c1].ToString();
                    grv1[colC2, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.c2].ToString();
                    grv1[colC5, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.c5].ToString();
                    grv1[colC10, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.c10].ToString();
                    grv1[colB20, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.b20].ToString();
                    grv1[colB50, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.b50].ToString();
                    grv1[colB100, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.b100].ToString();
                    grv1[colB500, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.b500].ToString();
                    grv1[colB1000, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.b1000].ToString();
                    grv1[colAmt, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.amount].ToString();
                    grv1[colMinus, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.plus1].ToString();
                    grv1[colPlus, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.minus1].ToString();
                    if ((i % 2) != 0)
                    {
                        grv1.Rows[i].DefaultCellStyle.BackColor = Color.LightSalmon;
                    }
                }
            }

            grv1.Font = font;
            grv1.Columns[colId].Visible = false;
            grv1.ReadOnly = true;
        }
        private void TxtBank1000_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
        }

        private void TxtBank500_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
        }

        private void TxtBank100_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
        }

        private void TxtBank50_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
        }

        private void TxtBank20_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
        }

        private void calAmt()
        {
            int c1 = 0, c2 = 0, c5 = 0, c10 = 0;
            int b20 = 0, b50 = 0, b100 = 0, b500 = 0, b1000 = 0, amt=0, plus1=0, minus=0;
            int.TryParse(txtCoin1.Text, out c1);
            int.TryParse(txtCoin2.Text, out c2);
            int.TryParse(txtCoin5.Text, out c5);
            int.TryParse(txtCoin10.Text, out c10);
            int.TryParse(txtBank20.Text, out b20);
            int.TryParse(txtBank50.Text, out b50);
            int.TryParse(txtBank100.Text, out b100);
            int.TryParse(txtBank500.Text, out b500);
            int.TryParse(txtBank1000.Text, out b1000);
            int.TryParse(txtPlus.Text, out plus1);
            int.TryParse(txtMinus.Text, out minus);
            c2 *= 2;
            c5 *= 5;
            c10 *= 10;
            b20 *= 20;
            b50 *= 50;
            b100 *= 100;
            b500 *= 500;
            b1000 *= 1000;
            amt = c1 + c2 + c5 + c10 + b20 + b50 + b100 + b500 + b1000 + plus1 - minus;
            txtAmount.Text = amt.ToString();
            //int.TryParse(txtCoin1.Text, out c1);
        }
        private void TxtCoin10_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
            if (e.KeyCode == Keys.Enter)
            {
                txtPlus.Focus();
            }
        }

        private void TxtCoin5_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
            if (e.KeyCode == Keys.Enter)
            {
                txtCoin10.Focus();
            }
        }

        private void TxtCoin2_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
            if (e.KeyCode == Keys.Enter)
            {
                txtCoin5.Focus();
            }
        }

        private void TxtCoin1_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
            if(e.KeyCode== Keys.Enter)
            {
                txtCoin2.Focus();
            }
        }

        private void LbLct_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            FrmLoca frm = new FrmLoca(ebC);
            frm.ShowDialog(this);
        }

        private void LbPlace_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            FrmPlace frm = new FrmPlace(ebC);
            frm.ShowDialog(this);
        }

        private void LbRou_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            FrmRoute frm = new FrmRoute(ebC);
            frm.ShowDialog(this);
        }

        private void LbStf_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            FrmStaff frm = new FrmStaff(ebC);
            frm.ShowDialog(this);

        }
        private void setCount()
        {
            cnt = new Count1();
            cnt.active = "1";
            cnt.remark = "";
            cnt.count_id = "";
            cnt.count_date = DateTime.Now.ToString("yyyy-MM-dd");
            cnt.staff_name = cboStf.Text;
            cnt.rou_name = cboRou.Text;
            cnt.loca_name = cboLoca.Text;
            cnt.place_name = cboPlc.Text;
            cnt.c1 = txtCoin1.Text;
            cnt.c2 = txtCoin2.Text;
            cnt.c5 = txtCoin5.Text;
            cnt.c10 = txtCoin10.Text;
            cnt.b20 = txtBank20.Text;
            cnt.b50 = txtBank50.Text;
            cnt.b100 = txtBank100.Text;
            cnt.b500 = txtBank500.Text;
            cnt.b1000 = txtBank1000.Text;
            cnt.plus1 = txtPlus.Text;
            cnt.minus1 = txtMinus.Text;
            cnt.amount = txtAmount.Text;
            
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //string filename = Path.Combine(cPath, "HHTCtrlp.exe");
            //var proc = System.Diagnostics.Process.Start(filename, cParams);
            //SQLiteConnection m_dbConnection;
            //SQLiteConnection.CreateFile("eb_20.sqlite");
            //m_dbConnection = new SQLiteConnection("Data Source=eb_20.sqlite;Version=3;");
            //m_dbConnection.Open();
            //string sql = "";
            //sql = "create table highscores (name varchar(20), score int)";
            //SQLiteCommand command = new SQLiteCommand(m_dbConnection);
            //command.CommandText = sql;
            //command.ExecuteNonQuery();
            //sql = "insert into highscores (name, score) values ('Me', 9001)";
            //command.CommandText = sql;
            //command.ExecuteNonQuery();
            if (MessageBox.Show("ต้องการ บันทึกข้อมูล", "Save Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                if (cboStf.Text.Equals(""))
                {
                    MessageBox.Show("ไม่พบข้อมูล Staff", "Error");
                    cboStf.Focus();
                    return;
                }
                if (cboRou.Text.Equals(""))
                {
                    MessageBox.Show("ไม่พบข้อมูล Route", "Error");
                    cboRou.Focus();
                    return;
                }
                if (cboLoca.Text.Equals(""))
                {
                    MessageBox.Show("ไม่พบข้อมูล LOcation", "Error");
                    cboLoca.Focus();
                    return;
                }
                if (cboPlc.Text.Equals(""))
                {
                    MessageBox.Show("ไม่พบข้อมูล Place", "Error");
                    cboPlc.Focus();
                    return;
                }
                if (txtCoin1.Text.Equals("") && txtCoin2.Text.Equals("") && txtCoin5.Text.Equals("") && txtCoin10.Text.Equals(""))
                {
                    MessageBox.Show("ไม่พบข้อมูล Coin", "Warning");
                }
                if (txtBank20.Text.Equals("") && txtBank50.Text.Equals("") && txtBank100.Text.Equals("") && txtBank500.Text.Equals("") && txtBank1000.Text.Equals(""))
                {
                    MessageBox.Show("ไม่พบข้อมูล Coin", "Warning");
                    return;
                }
                setCount();
                int chk = 0;
                String re = ebC.eB20DB.cntDB.insertCount(cnt, "");
                if (int.TryParse(re, out chk))
                {

                }
                else
                {
                    //MessageBox.Show("", "Error");
                }
                setGrd();
            }
        }

        private void BT_KEY_N0_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_N0);
        }

        private void BT_KEY_N9_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_N9);
        }

        private void BT_KEY_N8_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_N8);
        }

        private void BT_KEY_N7_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_N7);
        }

        private void BT_KEY_N6_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_N6);
        }

        private void BT_KEY_N5_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_N5);
        }

        private void BT_KEY_N4_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_N4);
        }

        private void BT_KEY_N3_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_N3);
        }

        private void BT_KEY_N2_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_N2);
        }

        private void BT_KEY_N1_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_N1);
        }

        private void BT_BAT_SET_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            iComState = 7;
            //iReSend = 0;
            Send_Command();
        }

        private void BT_KEY_BAT_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_BAT);
        }

        private void BT_KEY_CF_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_CF);
        }

        private void BT_KEY_ADD_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_ADD);
        }

        private void BT_KEY_LEVEL_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_LEVEL);
        }

        private void BT_KEY_CUR_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_CUR);
        }

        private void BT_KEY_MODE_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_MODE);
        }

        private void BT_CO_SET_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            iComState = 6;
            Send_Command();
        }

        private void BT_REQ_INFO_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            iComState = 8;
            Send_Command();
        }

        private void BT_KEY_START_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_START);
        }

        private void BT_KEY_DOWN_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_DOWN);
        }

        private void BT_KEY_CLEAR_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_CLEAR);
        }

        private void BT_KEY_UP_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            BT_KEY_Send(KEY_NAME.KEY_UP);
        }
        private void BT_KEY_Send(KEY_NAME key)
        {
            iComState = 5;
            //iReSend = 0;
            iKey = key;
            Send_Command();
        }
        private void RtbDisplay_TextChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            String sa = "";
            //sa = e.
        }

        private void BT_REM_SET_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //comm.WriteData("rem on a\r");
            iComState = 1;
            Send_Command();
        }

        private void BT_PORT_OC_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                if (comm.comPort.IsOpen)
                {
                    
                    comm.comPort.Close();
                    BT_PORT_OC.Text = "OPEN";
                    BT_PORT_OC.BackColor = Color.LightGray;

                    BT_REM_SET.Enabled = false;

                    BT_CO_SET.Enabled = false;
                    BT_BAT_SET.Enabled = false;
                    BT_REQ_INFO.Enabled = false;
                }
                else
                {
                    comm.Parity = cboParity.Text;
                    comm.StopBits = cboStop.Text;
                    comm.DataBits = cboData.Text;
                    comm.BaudRate = cboBaud.Text;
                    comm.DisplayWindow = rtbDisplay;
                    comm.PortName = COMB_SERIAL_P.Text;
                    comm.txt = TXTB_COM_STATUS;
                    comm.txtRemote = TXTB_REMOTE_S;
                    comm.txtTotal = TXTB_LD_TOTAL;
                    comm.txtTotal = TXTB_LD_TOTAL;
                    comm.txtReject = TXTB_LD_REJECT;
                    comm.txtTime = TXTB_LD_TIME;
                    comm.txtAmt = txtAmt;
                    comm.txtB20 = txtBank20;
                    comm.txtB50 = txtBank50;
                    comm.txtB100 = txtBank100;
                    comm.txtB500 = txtBank500;
                    comm.txtB1000 = txtBank1000;
                    BT_PORT_OC.Text = "CLOSE";
                    BT_PORT_OC.BackColor = Color.LightGreen;

                    BT_REM_SET.Enabled = true;

                    BT_CO_SET.Enabled = true;
                    BT_BAT_SET.Enabled = true;
                    BT_REQ_INFO.Enabled = true;
                    comm.OpenPort();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Send_Command()
        {
            String strCommand = "";
            switch (iComState)
            {
                case 1:
                    if (RDB_SO_A.Checked)
                    {
                        strCommand = "rem on a\r";
                        comm.RDB_SO_A = true;
                        comm.RDB_SO_M = false;
                        comm.RDB_SO_OFF = false;
                        //strCommand = TXTB_LD_TIME.Text + "\r";
                    }
                    else if (RDB_SO_M.Checked)
                    {
                        strCommand = "rem on m\r";
                        comm.RDB_SO_A = false;
                        comm.RDB_SO_M = true;
                        comm.RDB_SO_OFF = false;
                    }
                    else if (RDM_SO_OFF.Checked)
                    {
                        strCommand = "rem off\r";
                        comm.RDB_SO_A = false;
                        comm.RDB_SO_M = false;
                        comm.RDB_SO_OFF = true;
                    }
                    break;
                case 2:
                    strCommand = "rem go\r";
                    break;
                case 3:
                    break;
                case 5:
                    if (iKey != KEY_NAME.KEY_NONE)
                    {
                        strCommand = "rem key " + strKey[(byte)iKey - 1] + "\r";
                    }
                    break;
                case 6:
                    if (RDB_CO_A.Checked)
                    {
                        strCommand = "rem userm autom1\r";
                    }
                    else if (RDB_CO_M.Checked)
                    {
                        strCommand = "rem userm autom0\r";
                    }
                    break;
                case 7:
                    strCommand = "rem userm batch" + TXTB_BATCH.Text + "\r";
                    break;
                case 8:
                    strCommand = "rem info\r";
                    break;
                default:
                    break;
            }
            if (strCommand.Length > 0)
            {
                //SERIAL_M.Write(strCommand);
                comm.WriteData(strCommand);
                iRecvAck = 0;
                //if (iReSend == 0)
                //{
                //    //TM_Resend.Start();
                //}
            }
        }
        
        public void ScanPort()
        {
            //foreach (string str in SerialPort.GetPortNames())
            //{
            //    COMB_SERIAL_P.Items.Add(str);
            //}
        }
        private void LoadValues()
        {
            comm.SetPortNameValues(COMB_SERIAL_P);
            comm.SetParityValues(cboParity);
            comm.SetStopBitValues(cboStop);

        }
        private Boolean appExit()
        {
            //flagExit = true;
            if (MessageBox.Show("ต้องการออกจากโปรแกรม2", "ออกจากโปรแกรม menu", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                //if (ic.video != null && ic.video.IsRunning)
                //{
                //    ic.video.SignalToStop();
                //    ic.video.WaitForStop();
                //    ic.video.Stop();
                //    ic.video = null;
                //}
                Close();
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // ...
            if (keyData == (Keys.Escape))
            {
                appExit();
                //if (MessageBox.Show("ต้องการออกจากโปรแกรม1", "ออกจากโปรแกรม", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                //{
                //    Close();
                //    return true;
                //}
            }
            else
            {
                //keyData
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            COMB_SERIAL_P.SelectedIndex = 0;
            cboBaud.SelectedText = "115200";
            cboParity.SelectedIndex = 0;
            cboStop.SelectedIndex = 1;
            cboData.SelectedIndex = 1;
        }
    }
}
