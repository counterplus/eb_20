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
    public partial class FrmStaff : Form
    {
        EB20Control ebC;
        int colId = 0, colName = 1;
        Staff stf;
        public FrmStaff(EB20Control ebc)
        {
            InitializeComponent();
            ebC = ebc;
            initConfig();
        }
        private void initConfig()
        {
            grv1.CellMouseDoubleClick += Grv1_CellMouseDoubleClick;
            btnSave.Click += BtnSave_Click;
            btnNew.Click += BtnNew_Click;
            setGrd();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            txtId.Text = "";
            txtName.Text = "";
        }

        private void setStaff()
        {
            stf = new Staff();
            stf.staff_id = txtId.Text;
            stf.staff_name = txtName.Text;
            stf.active = "1";
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if(MessageBox.Show("ต้องการ บันทึกข้อมูล","Save Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                setStaff();
                int chk = 0;
                String re = ebC.eB20DB.stfDB.insertStaff(stf, "");
                if(int.TryParse(re, out chk))
                {
                    
                }
                else
                {
                    //MessageBox.Show("", "Error");
                }
                setGrd();
            }
        }

        private void Grv1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //throw new NotImplementedException();
            if (grv1[colId, e.RowIndex].Value == null) return;
            if (grv1[colName, e.RowIndex].Value == null) return;
            txtId.Text = grv1[colId, e.RowIndex].Value.ToString();
            txtName.Text = grv1[colName, e.RowIndex].Value.ToString();
        }

        private void setGrd()
        {
            Font font = new Font("Verdana", 12);
            DataTable dt = new DataTable();

            grv1.ColumnCount = 2;
            grv1.Rows.Clear();
            grv1.RowCount = 1;
            grv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grv1.Columns[colId].Width = 40;
            grv1.Columns[colName].Width = 200;
            

            grv1.Columns[colId].HeaderText = "no";
            grv1.Columns[colName].HeaderText = "Name";
            
            //dgv1.Columns[colPEWeight].HeaderText = "น้ำหนัก";
            
            dt = ebC.eB20DB.stfDB.selectAll();

            //dgvPE.Columns[colPEId].HeaderText = "id";
            if (dt.Rows.Count > 0)
            {
                grv1.RowCount = dt.Rows.Count;
                for (int i = 0; i < grv1.RowCount; i++)
                {
                    grv1[colId, i].Value = dt.Rows[i][ebC.eB20DB.stfDB.stf.staff_id].ToString();
                    grv1[colName, i].Value = dt.Rows[i][ebC.eB20DB.stfDB.stf.staff_name].ToString();
                    
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // ...
            if (keyData == (Keys.Escape))
            {
                //appExit();
                //if (MessageBox.Show("ต้องการออกจากโปรแกรม1", "ออกจากโปรแกรม", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                //{
                Close();
                //    return true;
                //}
            }
            else
            {
                //keyData
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void FrmStaff_Load(object sender, EventArgs e)
        {

        }
    }
}
