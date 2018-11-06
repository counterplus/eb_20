using eb_20_test2.control;
using eb_20_test2.object1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb_20_test2.gui
{
    public partial class FrmInit : Form
    {
        EB20Control ebC;
        ComPort cpt;

        CommunicationManager comm = new CommunicationManager();

        public FrmInit(EB20Control ebc)
        {
            InitializeComponent();
            ebC = ebc;
            initConfig();
        }
        private void initConfig()
        {
            cpt = new ComPort();
            comm.SetPortNameValues(COMB_SERIAL_P);
            comm.SetParityValues(cboParity);
            comm.SetStopBitValues(cboStop);
            
            setControl();

            btnSave.Click += BtnSave_Click;
        }
        private void setComPort()
        {
            cpt.parity = cboParity.Text;
            cpt.stop_bit = cboStop.Text;
            cpt.id = "1";
            cpt.baud_rate = cboBaud.Text;
            cpt.data_bit = cboData.Text;
        }
        private void setControl()
        {
            cpt = ebC.eB20DB.cptDB.selectByPk1("1");
            cboBaud.Text = cpt.baud_rate;
            cboParity.Text = cpt.parity;
            cboStop.Text = cpt.stop_bit;
            cboData.Text = cpt.data_bit;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (MessageBox.Show("ต้องการ บันทึกข้อมูล", "Save Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                if (cboBaud.Text.Equals(""))
                {
                    MessageBox.Show("ไม่พบข้อมูล Baud Rate", "Error");
                    cboBaud.Focus();
                    return;
                }
                if (cboParity.Text.Equals(""))
                {
                    MessageBox.Show("ไม่พบข้อมูล Parity", "Error");
                    cboParity.Focus();
                    return;
                }
                if (cboStop.Text.Equals(""))
                {
                    MessageBox.Show("ไม่พบข้อมูล Stop Bit", "Error");
                    cboStop.Focus();
                    return;
                }
                if (cboData.Text.Equals(""))
                {
                    MessageBox.Show("ไม่พบข้อมูล Data Bit", "Error");
                    cboData.Focus();
                    return;
                }
                
                setComPort();
                int chk = 0;
                String re = ebC.eB20DB.cptDB.insertComPort(cpt, "");
                if (int.TryParse(re, out chk))
                {

                }
                else
                {
                    //MessageBox.Show("", "Error");
                }
                
            }
        }

        private void FrmInit_Load(object sender, EventArgs e)
        {
            COMB_SERIAL_P.SelectedIndex = 0;
        }
    }
}
