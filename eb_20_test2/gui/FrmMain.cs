using eb_20_test2.control;
using eb_20_test2.object1;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb_20_test2.gui
{
    public partial class FrmMain : Form
    {
        EB20Control ebC;
        Count1 cnt;
        ComPort cpt;
        CommunicationManager comm = new CommunicationManager();
        ComboBox COMB_SERIAL_P;
        //TextBox txtAmt;
        byte iComState = 0;
        byte iRecvAck = 1;
        int colId = 0, colDate = 1, colRou = 2, colLoca = 3, colPlc = 4, colC1 = 5, colC2 = 6, colC5 = 7, colC10 = 8, colB20 = 9, colB50 = 10, colB100 = 11, colB500 = 12, colB1000 = 13, colAmt = 14, colPlus = 15, colMinus = 16, colStf = 17, colRemark=18;
        String[] strKey = {"num0", "num1", "num2", "num3", "num4", "num5", "num6", "num7", "num8", "num9", "upar","downar",
            "cf","bat","level","add","mode","cur","clear","start"};
        enum KEY_NAME
        {
            KEY_NONE = 0, KEY_N0, KEY_N1, KEY_N2, KEY_N3, KEY_N4, KEY_N5, KEY_N6, KEY_N7, KEY_N8, KEY_N9, KEY_UP, KEY_DOWN,
            KEY_CF, KEY_BAT, KEY_LEVEL, KEY_ADD, KEY_MODE, KEY_CUR, KEY_CLEAR, KEY_START
        };
        KEY_NAME iKey = KEY_NAME.KEY_NONE;

        public FrmMain(EB20Control ebc)
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
            COMB_SERIAL_P = new ComboBox();
            //txtAmt = new TextBox();
            ebC.eB20DB = new obgdb.EB20DB(ebC.conn);
            ebC.eB20DB.stfDB.setCboStaff(cboStf, "");
            ebC.eB20DB.lctDB.setCboLocation(cboLoca, "");
            ebC.eB20DB.plcDB.setCboPlace(cboPlc, "");
            ebC.eB20DB.rouDB.setCboRoute(cboRou, "");

            cpt = new ComPort();
            cpt = ebC.eB20DB.cptDB.selectByPk1("1");
            LoadValues();
            setGrd();
            btnInit.Click += BtnInit_Click;
            btnConnect.Click += BtnConnect_Click;
            //BT_REM_SET.Click += BT_REM_SET_Click;
            //rtbDisplay.TextChanged += RtbDisplay_TextChanged;
            //BT_KEY_UP.Click += BT_KEY_UP_Click;
            //BT_KEY_CLEAR.Click += BT_KEY_CLEAR_Click;
            //BT_KEY_DOWN.Click += BT_KEY_DOWN_Click;
            //BT_KEY_START.Click += BT_KEY_START_Click;
            //BT_REQ_INFO.Click += BT_REQ_INFO_Click;
            //BT_CO_SET.Click += BT_CO_SET_Click;
            //BT_KEY_MODE.Click += BT_KEY_MODE_Click;
            //BT_KEY_CUR.Click += BT_KEY_CUR_Click;
            //BT_KEY_LEVEL.Click += BT_KEY_LEVEL_Click;
            //BT_KEY_ADD.Click += BT_KEY_ADD_Click;
            //BT_KEY_CF.Click += BT_KEY_CF_Click;
            //BT_KEY_BAT.Click += BT_KEY_BAT_Click;
            //BT_BAT_SET.Click += BT_BAT_SET_Click;
            //BT_KEY_N1.Click += BT_KEY_N1_Click;
            //BT_KEY_N2.Click += BT_KEY_N2_Click;
            //BT_KEY_N3.Click += BT_KEY_N3_Click;
            //BT_KEY_N4.Click += BT_KEY_N4_Click;
            //BT_KEY_N5.Click += BT_KEY_N5_Click;
            //BT_KEY_N6.Click += BT_KEY_N6_Click;
            //BT_KEY_N7.Click += BT_KEY_N7_Click;
            //BT_KEY_N8.Click += BT_KEY_N8_Click;
            //BT_KEY_N9.Click += BT_KEY_N9_Click;
            //BT_KEY_N0.Click += BT_KEY_N0_Click;
            btnPrn.Click += BtnPrn_Click;
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
            //btnPrn.Click += BtnPrn_Click;
        }

        private void BtnPrn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            String date = "";
            date = DateTime.Now.ToString("yyyy-MM-dd");
            DataTable dt = new DataTable();
            dt = ebC.eB20DB.cntDB.selectByDate(date);
            if (dt.Rows.Count <= 0) return;
            
            BaseFont bfR, bfR1;
            BaseColor clrBlack = new iTextSharp.text.BaseColor(0, 0, 0);
            String myFont = Environment.CurrentDirectory + "\\THSarabun.ttf";
            bfR = BaseFont.CreateFont(myFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            bfR1 = BaseFont.CreateFont(myFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font fntHead = new iTextSharp.text.Font(bfR, 12, iTextSharp.text.Font.NORMAL, clrBlack);
            var logo = iTextSharp.text.Image.GetInstance(Environment.CurrentDirectory + "\\logo.png");
            logo.SetAbsolutePosition(10, PageSize.A4.Height - 90);
            logo.ScaleAbsoluteHeight(70);
            logo.ScaleAbsoluteWidth(70);

            Document doc = new Document(PageSize.A4, 36, 36, 36, 36);

            try
            {
                if (File.Exists(Environment.CurrentDirectory + "\\report.pdf"))
                {
                    File.Delete(Environment.CurrentDirectory + "\\report.pdf");
                }

                FileStream output = new FileStream(Environment.CurrentDirectory + "\\report.pdf", FileMode.Create);
                PdfWriter writer = PdfWriter.GetInstance(doc, output);
                doc.Open();

                doc.Add(logo);

                int i = 0, r = 0, row2 = 0, rowEnd = 24;
                r = dt.Rows.Count;
                int next = r / 24;
                for (int p = 0; p <= next; p++)
                {
                    PdfContentByte canvas = writer.DirectContent;
                    canvas.BeginText();
                    canvas.SetFontAndSize(bfR, 12);
                    canvas.ShowTextAligned(Element.ALIGN_LEFT, "บริษัท เคาเตอร์ พลัส จำกัด", 100, 800, 0);
                    canvas.ShowTextAligned(Element.ALIGN_LEFT, "99/19 ซอยประเสริฐมนูกิจ 29 ถนนประเสริฐมนูกิจ แขวงจรเข้บัว เขตลาดพร้าว กรุงเทพฯ 10230", 100, 780, 0);
                    canvas.EndText();

                    canvas.BeginText();
                    canvas.SetFontAndSize(bfR, 18);
                    canvas.ShowTextAligned(Element.ALIGN_CENTER, "รายงานสรุปการนับเงินที่จำหน่ายจากตู้ ตามวันที่ ", PageSize.A4.Width / 2, 740, 0);
                    canvas.EndText();

                    canvas.BeginText();
                    canvas.SetFontAndSize(bfR, 16);
                    canvas.ShowTextAligned(Element.ALIGN_LEFT, "ลำดับ " , 60, 720, 0);
                    canvas.ShowTextAligned(Element.ALIGN_LEFT, "สถานที่ " , 60, 700, 0);
                    canvas.ShowTextAligned(Element.ALIGN_LEFT, "จำนวนเงินที่นับได้จริง " , 60, 680, 0);
                    canvas.ShowTextAligned(Element.ALIGN_LEFT, "ยอดรวม " , 360, 720, 0);
                    canvas.ShowTextAligned(Element.ALIGN_LEFT, "รวมเงิน " , 360, 700, 0);
                    canvas.ShowTextAligned(Element.ALIGN_LEFT, "เงินเกิน ", 360, 680, 0);
                    canvas.ShowTextAligned(Element.ALIGN_LEFT, "เงินขาด " , 360, 660, 0);
                    canvas.EndText();

                    canvas.SaveState();
                    canvas.SetLineWidth(0.05f);
                    canvas.MoveTo(40, 640);//vertical
                    canvas.LineTo(40, 110);

                    canvas.MoveTo(40, 640);//Hericental
                    canvas.LineTo(560, 640);

                    canvas.MoveTo(560, 640);//vertical
                    canvas.LineTo(560, 110);

                    canvas.MoveTo(40, 610);//Hericental
                    canvas.LineTo(560, 610);

                    canvas.MoveTo(40, 110);//Hericental
                    canvas.LineTo(560, 110);

                    canvas.MoveTo(100, 640);//vertical
                    canvas.LineTo(100, 110);

                    canvas.MoveTo(400, 640);//vertical
                    canvas.LineTo(400, 110);

                    canvas.MoveTo(440, 640);//vertical QTY
                    canvas.LineTo(440, 110);

                    canvas.MoveTo(500, 640);//vertical Price
                    canvas.LineTo(500, 110);

                    //canvas.MoveTo(520, 640);//vertical Amount
                    //canvas.LineTo(520, 110);
                    canvas.Stroke();
                    canvas.RestoreState();

                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                doc.Close();
                System.Threading.Thread.Sleep(1000);

                Process pp = new Process();
                ProcessStartInfo s = new ProcessStartInfo(Environment.CurrentDirectory + "\\report.pdf");
                //s.Arguments = "/c dir *.cs";
                pp.StartInfo = s;

                pp.Start();
            }            
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
        private void BtnConnect_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                if (comm.comPort.IsOpen)
                {

                    comm.comPort.Close();
                    btnConnect.Text = "OPEN";
                    btnConnect.BackColor = Color.LightGray;
                    
                }
                else
                {
                    comm.Parity = cpt.parity;
                    comm.StopBits = cpt.stop_bit;
                    comm.DataBits = cpt.data_bit;
                    comm.BaudRate = cpt.baud_rate;
                    comm.DisplayWindow = rtbDisplay;
                    comm.PortName = COMB_SERIAL_P.Text;
                    comm.txt = TXTB_COM_STATUS;
                    comm.txtRemote = TXTB_COM_STATUS;
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
                    btnConnect.Text = "CLOSE";
                    btnConnect.BackColor = Color.LightGreen;
                    
                    comm.OpenPort();
                    System.Threading.Thread.Sleep(1000);
                    Send_Command();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnInit_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            FrmInit frm = new FrmInit(ebC);
            frm.ShowDialog(this);
        }
        public void Send_Command()
        {
            String strCommand = "";

            strCommand = "rem on a\r";
            comm.RDB_SO_A = true;
            comm.RDB_SO_M = false;
            comm.RDB_SO_OFF = false;
          
            if (strCommand.Length > 0)
            {
                //SERIAL_M.Write(strCommand);
                comm.WriteData(strCommand);
                iRecvAck = 0;                
            }
        }
        private void LoadValues()
        {
            comm.SetPortNameValues(COMB_SERIAL_P);
            //comm.SetParityValues(cboParity);
            //comm.SetStopBitValues(cboStop);

        }
        private void setGrd()
        {
            System.Drawing.Font font = new System.Drawing.Font("Verdana", 12);
            DataTable dt = new DataTable();

            grv1.ColumnCount = 19;
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
            grv1.Columns[colRemark].Width = 80;

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
            grv1.Columns[colRemark].HeaderText = "Remark";

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
                    grv1[colStf, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.staff_name].ToString();
                    grv1[colRemark, i].Value = dt.Rows[i][ebC.eB20DB.cntDB.cnt.remark].ToString();
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
        private void calCoin()
        {
            int c1 = 0, c2 = 0, c5 = 0, c10 = 0, coin=0;
            //int b20 = 0, b50 = 0, b100 = 0, b500 = 0, b1000 = 0, amt = 0, plus1 = 0, minus = 0;
            int.TryParse(txtCoin1.Text, out c1);
            int.TryParse(txtCoin2.Text, out c2);
            int.TryParse(txtCoin5.Text, out c5);
            int.TryParse(txtCoin10.Text, out c10);
            
            c2 *= 2;
            c5 *= 5;
            c10 *= 10;

            coin = c1 + c2 + c5 + c10 ;
            txtCoin.Text = coin.ToString();
            //int.TryParse(txtCoin1.Text, out c1);
        }
        private void calAmt()
        {
            int c1 = 0, c2 = 0, c5 = 0, c10 = 0;
            int b20 = 0, b50 = 0, b100 = 0, b500 = 0, b1000 = 0, amt = 0, plus1 = 0, minus = 0;
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
            calCoin();
            if (e.KeyCode == Keys.Enter)
            {
                txtPlus.Focus();
            }
        }

        private void TxtCoin5_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
            calCoin();
            if (e.KeyCode == Keys.Enter)
            {
                txtCoin10.Focus();
            }
        }

        private void TxtCoin2_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
            calCoin();
            if (e.KeyCode == Keys.Enter)
            {
                txtCoin5.Focus();
            }
        }

        private void TxtCoin1_KeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            calAmt();
            calCoin();
            if (e.KeyCode == Keys.Enter)
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
        private void FrmMain_Load(object sender, EventArgs e)
        {
            COMB_SERIAL_P.SelectedIndex = 0;
        }
    }
}
