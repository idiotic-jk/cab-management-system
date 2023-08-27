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
    public partial class custdet : UserControl
    {
        public int i;
        public string s;
        dbaccess db = new dbaccess();
        public SqlCommand cmd = new SqlCommand();
        
        public custdet()
        {
            InitializeComponent();
            cmd.Connection = db.con;
        }
        private void custclear()
        {
            bun_custcode_txb.Clear(); bun_custnam_txb.Clear();
            bun_custadd_txb.Clear(); bun_custph_txb.Clear();
            bun_custmail_txb.Clear();
        }

        private void custdis()
        {
            s = "select * from cust";
            bun_addcust_dgv.DataSource = db.FetchData(s);
        }
        private void list_btn_qd_Click(object sender, EventArgs e)
        {
            custdis();
        }

        private void new_btn_qd_Click(object sender, EventArgs e)
        {
            custclear();
        }
        private void bun_addcust_dgv_DoubleClick(object sender, EventArgs e)
        {
            if (bun_addcust_dgv.CurrentRow != null)
            {
                int cr = bun_addcust_dgv.CurrentRow.Index;
                if (bun_addcust_dgv.CurrentRow.Cells["custcode"].Value != DBNull.Value)
                {
                    bun_custcode_txb.Text = Convert.ToString(bun_addcust_dgv[0, cr].Value);
                    bun_custnam_txb.Text = Convert.ToString(bun_addcust_dgv[1, cr].Value);
                    bun_custph_txb.Text = Convert.ToString(bun_addcust_dgv[2, cr].Value);
                    bun_custadd_txb.Text = Convert.ToString(bun_addcust_dgv[3, cr].Value);
                    bun_custmail_txb.Text = Convert.ToString(bun_addcust_dgv[4, cr].Value);

                }
            }
                
        }

        private void Save_btn_qd_Click(object sender, EventArgs e)
        {
            if (bun_custcode_txb.Text != "" && bun_custnam_txb.Text != "" && bun_custadd_txb.Text != "" && bun_custph_txb.Text.Length == 10 && bun_custmail_txb.Text != "")
            {
                cmd.CommandText = ("Select * From cust Where custcode ='" + bun_custcode_txb.Text.Trim() + "'  ");
                if (db.checkexist(cmd) == false)
                {
                    SqlCommand cmd = new SqlCommand("Insert into cust values( @a,@b,@d,@c,@e)", db.con);
                    cmd.Parameters.AddWithValue("@a", bun_custcode_txb.Text);
                    cmd.Parameters.AddWithValue("@b", bun_custnam_txb.Text);
                    cmd.Parameters.AddWithValue("@c", bun_custadd_txb.Text);
                    cmd.Parameters.AddWithValue("@d", bun_custph_txb.Text);
                    cmd.Parameters.AddWithValue("@e", bun_custmail_txb.Text);
                    i = db.InsertData(cmd);
                    if (i == 1)
                        MessageBox.Show("Inserted"); 
                    custclear();
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
            
        }

        private void modify_btn_qd_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From cust Where custcode ='" + bun_custcode_txb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("update  cust set custcode= @a ,custname = @b , cust_phno = @d , custadd = @c ,custmail = @e Where custcode = @a  ");
                cmd.Parameters.AddWithValue("@a", bun_custcode_txb.Text);
                cmd.Parameters.AddWithValue("@b", bun_custnam_txb.Text);
                cmd.Parameters.AddWithValue("@c", bun_custadd_txb.Text);
                cmd.Parameters.AddWithValue("@d", bun_custph_txb.Text);
                cmd.Parameters.AddWithValue("@e", bun_custmail_txb.Text);
                db.ExecuteQuery(cmd); custclear(); MessageBox.Show("ROW UPDATED");
            }
            else
            {
                MessageBox.Show("ENTER Known Customer");
            }
        }

        

        private void Delete_btn_qd_Click(object sender, EventArgs e)
        {

            cmd.CommandText = ("Select * From cust Where custcode ='" + bun_custcode_txb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("delete from  cust  Where custcode ='" + bun_custcode_txb.Text.Trim() + "'  ");
                db.ExecuteQuery(cmd); custclear(); MessageBox.Show("ROW Deleted");
            }
            else
            {
                MessageBox.Show("ENTER Known Customer");
            }
           
        }

        private void bun_custcode_txb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            { e.Handled = true; }
        }

        private void bun_custph_txb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            { e.Handled = true; }
        }

        private void bun_custph_txb_Leave(object sender, EventArgs e)
        {
            if(bun_custph_txb.Text.Length!=10)
            {
                MessageBox.Show("ENTER 10 Digits");
                bun_custph_txb.Focus();
            }
        }
    }
}
