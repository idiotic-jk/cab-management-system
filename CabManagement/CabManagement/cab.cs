using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CabManagement
{
    public partial class cab : UserControl
    {
        public int i;
        public string s;
        dbaccess db = new dbaccess();
        public SqlCommand cmd = new SqlCommand();
        public cab()
        {
            InitializeComponent();
            cmd.Connection = db.con;
        }
        
        public void cabclear()
        {
            bun_addcabdet_tb.Clear();bun_addcabnam_tb.Clear();
            bun_addcabno_tb.Clear();bun_addcabppkm_tb.Clear();
        }   
        public void cabdis()
        {
            s = "select * from cab";
            bun_addcab_dgv.DataSource = db.FetchData(s);
        }

        private void bun_addcab_dgv_DoubleClick(object sender, EventArgs e)
        {
            if (bun_addcab_dgv.CurrentRow != null)
            {
                int cr = bun_addcab_dgv.CurrentRow.Index;
                if (bun_addcab_dgv.CurrentRow.Cells["add_cabno"].Value != DBNull.Value)
                {
                    bun_addcabno_tb.Text = Convert.ToString(bun_addcab_dgv[0, cr].Value);
                    bun_addcabnam_tb.Text = Convert.ToString(bun_addcab_dgv[1, cr].Value);
                    bun_addcabdet_tb.Text = Convert.ToString(bun_addcab_dgv[2, cr].Value);
                    bun_addcabppkm_tb.Text = Convert.ToString(bun_addcab_dgv[3, cr].Value);

                }
            }
               
        }

        private void new_btn_qd_Click(object sender, EventArgs e)
        {
            cabclear();
        }

        private void list_btn_qd_Click(object sender, EventArgs e)
        {
            cabdis();
        }

        private void Save_btn_qd_Click(object sender, EventArgs e)
        {
            if (bun_addcabno_tb.Text != "" && bun_addcabnam_tb.Text != "" && bun_addcabdet_tb.Text != "" && bun_addcabppkm_tb.Text != "")
            {
                cmd.CommandText = ("Select * From cab Where cab_reg ='" + bun_addcabno_tb.Text.Trim() + "'  ");
                if (db.checkexist(cmd) == false)
                {
                    SqlCommand cmd = new SqlCommand("Insert into cab values( @a,@b,@c,@d)", db.con);
                    cmd.Parameters.AddWithValue("@a", bun_addcabno_tb.Text);
                    cmd.Parameters.AddWithValue("@b", bun_addcabnam_tb.Text);
                    cmd.Parameters.AddWithValue("@c", bun_addcabdet_tb.Text);
                    cmd.Parameters.AddWithValue("@d", bun_addcabppkm_tb.Text);
                    i = db.InsertData(cmd);
                    if (i == 1)
                        MessageBox.Show("Inserted");
                    cabclear();
                }
                else
                {
                    MessageBox.Show("ENTER unique qual");
                }
            }
            else
            {
                MessageBox.Show("ENTER DETAILS");
            }
            cabclear();
        }

        private void modify_btn_qd_Click(object sender, EventArgs e)
        {

            cmd.CommandText = ("Select * From cab Where cab_reg ='" + bun_addcabno_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("update  cab set cab_reg= @a ,cab_model = @b , cab_desc = @c , cab_ppkm = @d Where cab_reg = @a  ");
                cmd.Parameters.AddWithValue("@a", bun_addcabno_tb.Text);
                cmd.Parameters.AddWithValue("@b", bun_addcabnam_tb.Text);
                cmd.Parameters.AddWithValue("@c", bun_addcabdet_tb.Text);
                cmd.Parameters.AddWithValue("@d", bun_addcabppkm_tb.Text);
                db.ExecuteQuery(cmd); cabclear(); MessageBox.Show("ROW Modified");
            }
            else
                MessageBox.Show("ENTER Known Cab-No");

        }

        private void Delete_btn_qd_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From cab Where cab_reg ='" + bun_addcabno_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("delete from  cab  Where cab_reg ='" + bun_addcabno_tb.Text.Trim() + "'  ");
                db.ExecuteQuery(cmd); cabclear(); MessageBox.Show("ROW Deleted");
            }
            else
                MessageBox.Show("ENTER Known Cab-No");

        }

        private void bun_addcabppkm_tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            { e.Handled = true; }
            TextBox txtDecimal = sender as TextBox;
            if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
            {
                e.Handled = true;
            }
        }
    }
}
