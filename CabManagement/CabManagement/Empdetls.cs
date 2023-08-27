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
    public partial class Empdetls : UserControl
    {
        public int i;
        public string s;
        dbaccess db = new dbaccess();
        public SqlCommand cmd = new SqlCommand();
        public SqlDataReader rd = null;
        public Empdetls()
        {
            InitializeComponent();
            cmd.Connection = db.con;
            loaddesig();loadqual();
        }
        private void empclear()
        {
            bun_emp_code_tb.Clear(); bun_emp_name_tb.Clear();
            bun_emp_add_tb.Clear(); bun_emp_ph_tb.Clear();
            bun_emp_mail_tb.Clear(); bun_emp_sal_tb.Clear();
        }
        private void custdis()
        {
            s = "select empcode,empname,emp_phno,empdesig from emp";
            bun_empdet_dgv.DataSource = db.FetchData(s);
        }
        private void loadqual()
        {
            try
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter("select qname from qual", db.con);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                bun_qual_dd.DisplayMember = "qname";
                bun_qual_dd.ValueMember = "qname";

                DataRow topItem = dtbl.NewRow();
                topItem[0] = null;

                dtbl.Rows.InsertAt(topItem, 0);
                bun_qual_dd.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                // write exception info to log or anything else
                MessageBox.Show(ex.Message, "Error occured!");
            }

        }
        private void loaddesig()
        {


            try
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter("select dname  from desig", db.con);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                DataRow topItem = dtbl.NewRow();
                bun_prof_dd.ValueMember = "dname";
                bun_prof_dd.DisplayMember = "dname";
                topItem[0] = null;

                dtbl.Rows.InsertAt(topItem, 0);
                bun_prof_dd.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                // write exception info to log or anything else
                MessageBox.Show(ex.Message, "Error occured!");
            }
        }

        private void new_btn_qd_Click(object sender, EventArgs e)
        {
            empclear();
        } 
        
        private void bun_empdet_dgv_DoubleClick(object sender, EventArgs e)
        {
           if( bun_empdet_dgv.CurrentRow != null)
            {
                if (bun_empdet_dgv.CurrentRow.Cells["empcode"].Value != DBNull.Value)
                {
                    int cr = bun_empdet_dgv.CurrentRow.Index;
                    SqlCommand cmd = new SqlCommand("select * from emp where empcode='" + bun_empdet_dgv.CurrentRow.Cells["empcode"].Value + "'", db.con);
                    using (rd = db.passread(cmd))
                    {
                        if (rd.Read())
                        {
                            bun_emp_code_tb.Text = rd.GetValue(0).ToString();
                            bun_emp_name_tb.Text = rd.GetValue(1).ToString();
                            bun_emp_add_tb.Text = rd.GetValue(3).ToString();
                            bun_emp_ph_tb.Text = rd.GetValue(2).ToString();
                            bun_emp_mail_tb.Text = rd.GetValue(4).ToString();
                            bun_prof_dd.SelectedValue = rd.GetValue(5).ToString();
                            bun_qual_dd.SelectedValue = rd.GetValue(6).ToString();
                            bun_emp_doj_dtp.Value = DateTime.Parse(rd.GetValue(7).ToString());
                            bun_emp_sal_tb.Text = rd.GetValue(8).ToString();
                        }
                        db.con.Close();
                    }
                }
           }      
           
        }
        
        private void Save_btn_qd_Click(object sender, EventArgs e)
        {
            if (bun_emp_code_tb.Text != "" && bun_emp_name_tb.Text != "" && bun_emp_add_tb.Text != "" && bun_emp_ph_tb.Text.Length == 10 && bun_emp_mail_tb.Text != "" && bun_qual_dd.SelectedValue != null && bun_prof_dd.SelectedValue != null && bun_emp_doj_dtp.Value != null && bun_emp_sal_tb.Text != "")
            {
                cmd.CommandText = ("Select * From emp Where empcode ='" + bun_emp_code_tb.Text.Trim() + "'  ");
                if (db.checkexist(cmd) == false)
                {
                    SqlCommand cmd = new SqlCommand("Insert into emp values( @a,@b,@c,@d,@e,@f,@g,@h,@i)", db.con);
                    cmd.Parameters.AddWithValue("@a", bun_emp_code_tb.Text);
                    cmd.Parameters.AddWithValue("@b", bun_emp_name_tb.Text);
                    cmd.Parameters.AddWithValue("@c", bun_emp_ph_tb.Text);
                    cmd.Parameters.AddWithValue("@d", bun_emp_add_tb.Text); 
                    cmd.Parameters.AddWithValue("@e", bun_emp_mail_tb.Text);
                    cmd.Parameters.AddWithValue("@f", bun_prof_dd.SelectedValue);
                    cmd.Parameters.AddWithValue("@g", bun_qual_dd.SelectedValue);
                    cmd.Parameters.AddWithValue("@h", bun_emp_doj_dtp.Value.Date);
                    cmd.Parameters.AddWithValue("@i", bun_emp_sal_tb.Text);

                    i = db.InsertData(cmd);
                    if (i == 1)
                        MessageBox.Show("Inserted"); 
                    empclear();
                }
                else
                {
                    MessageBox.Show("ENTER unique ID");
                }
            }
            else
            {
                MessageBox.Show("ENTER DETAILS");
            }
            
        }

        private void modify_btn_qd_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From emp Where empcode ='" + bun_emp_code_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                if (bun_emp_code_tb.Text != "" && bun_emp_name_tb.Text != "" && bun_emp_add_tb.Text != "" && bun_emp_ph_tb.Text.Length == 10 && bun_emp_mail_tb.Text != "" && bun_qual_dd.SelectedValue != null && bun_prof_dd.SelectedValue != null && bun_emp_doj_dtp.Value != null && bun_emp_sal_tb.Text != "")
                {
                    cmd.CommandText = ("update  emp set empcode= @a ,empname = @b , emp_phno = @c , empadd = @d ,empmail = @e, empdesig=@f,empqual=@g,empdoj=@h,empsal=@i Where empcode = @a  ");
                    cmd.Parameters.AddWithValue("@a", bun_emp_code_tb.Text);
                    cmd.Parameters.AddWithValue("@b", bun_emp_name_tb.Text);
                    cmd.Parameters.AddWithValue("@c", bun_emp_add_tb.Text);
                    cmd.Parameters.AddWithValue("@d", bun_emp_ph_tb.Text);
                    cmd.Parameters.AddWithValue("@e", bun_emp_mail_tb.Text);
                    cmd.Parameters.AddWithValue("@f", bun_qual_dd.SelectedValue);
                    cmd.Parameters.AddWithValue("@g", bun_prof_dd.SelectedValue);
                    cmd.Parameters.AddWithValue("@h", bun_emp_doj_dtp.Value);
                    cmd.Parameters.AddWithValue("@i", bun_emp_sal_tb.Text);

                }
                empclear(); MessageBox.Show("ROW UPDATED");
            }
            else
            {
                MessageBox.Show("ENTER Known Customer");
            }

               

        }



        private void Delete_btn_qd_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From emp Where empcode ='" + bun_emp_code_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("delete from  emp  Where empcode ='" + bun_emp_code_tb.Text.Trim() + "'  ");
                db.ExecuteQuery(cmd); empclear(); MessageBox.Show("ROW Deleted");
            }
            else
            {
                MessageBox.Show("ENTER Known Customer");
            }


           
        }

        private void list_btn_qd_Click(object sender, EventArgs e)
        {
            custdis();
        }

        private void bun_emp_code_tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            { e.Handled = true; }
        }

        private void bun_emp_ph_tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            { e.Handled = true; }
        }

        private void bun_emp_sal_tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            { e.Handled = true; }
            TextBox txtDecimal = sender as TextBox;
            if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void bun_qual_dd_Click(object sender, EventArgs e)
        {
            loadqual();
        }

        private void bun_prof_dd_Click(object sender, EventArgs e)
        {
            loaddesig();
        }

        private void bun_emp_ph_tb_Leave(object sender, EventArgs e)
        {
            if (bun_emp_ph_tb.Text.Length != 10)
            {
                MessageBox.Show("ENTER 10 Digits");
                bun_emp_ph_tb.Focus();
            }
        }
    }
}
