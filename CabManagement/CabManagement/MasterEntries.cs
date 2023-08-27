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
    public partial class MasterEntries : UserControl
    {
        public int i;
        public string s;
        dbaccess db = new dbaccess();
        public SqlCommand cmd = new SqlCommand();
        
        public MasterEntries()
        {
            InitializeComponent();
            cmd.Connection = db.con;
        }
        private void qclear()
        {
            bun_qualdet_tb.Clear();bun_qualname_tb.Clear();
        }
        private void dclear()
        {
            bun_addesign_tb.Clear();bun_addesigd_tb.Clear();
        }
        private void aclear()
        {
            bun_addarn_tb.Clear();bun_addard_tb.Clear();
        }
        private void bun_qualname_tb_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void new_btn_qd_Click(object sender, EventArgs e)
        {
            qclear();
        }
        private void insqual()
        {
            if (bun_qualdet_tb.Text != "" && bun_qualname_tb.Text != "")
            {
                cmd.CommandText = ("Select * From qual Where qname ='" + bun_qualname_tb.Text.Trim() + "'  ");
                if (db.checkexist(cmd) == false)
                {
                    SqlCommand cmd = new SqlCommand("Insert into qual values( @a,'" + bun_qualdet_tb.Text + "')", db.con);
                    cmd.Parameters.AddWithValue("@a", bun_qualname_tb.Text);
                    i = db.InsertData(cmd);
                    if (i == 1)
                        MessageBox.Show("Inserted");
                    qclear();
                }
                else
                {
                    MessageBox.Show("ENTER unique Qualification");
                }
            }
            else
            {
                MessageBox.Show("ENTER ALL DETAILS");
            }
        }
        private void upqual()
        {
            cmd.CommandText = ("update  qual set qname='"+ bun_qualname_tb.Text + "',Descr='"+ bun_qualdet_tb.Text + "' Where qname ='" + bun_qualname_tb.Text.Trim() + "'  ");
            db.ExecuteQuery(cmd); MessageBox.Show("ROW UPDATED");
        }
        private void Save_btn_qd_Click(object sender, EventArgs e)
        {
            insqual(); 
        }
        private void Delete_btn_qd_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From qual Where qname ='" + bun_qualname_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("delete from  qual  Where qname ='" + bun_qualname_tb.Text.Trim() + "'  ");
                db.ExecuteQuery(cmd); qclear(); MessageBox.Show("ROW DELETED");
            }
            else
            {
                MessageBox.Show("ENTER Known Qualification");
            }
           
        }
        private void list_btn_qd_Click(object sender, EventArgs e)
        {
            s = "select * from qual";            
            bun_addqual_dgv.DataSource=db.FetchData(s);

        }
        private void bun_addqual_dgv_DoubleClick(object sender, EventArgs e)
        {
            if (bun_addqual_dgv.CurrentRow != null)
            {
                
                int cr = bun_addqual_dgv.CurrentRow.Index;
                if (bun_addqual_dgv.CurrentRow.Cells["add_qqname"].Value != DBNull.Value)
                {
                    bun_qualname_tb.Text = Convert.ToString(bun_addqual_dgv[0, cr].Value);
                    bun_qualdet_tb.Text = Convert.ToString(bun_addqual_dgv[1, cr].Value);
                }
            }
                
        }
        private void modify_btn_qd_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From qual Where qname ='" + bun_qualname_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                upqual(); qclear();
            }
            else
            {
                MessageBox.Show("ENTER Known Qualification");
            }
            
        }
    //------------------------------------------------------------------------------------------------------------------------------

        private void addesig_dgv()
        {
            s = "select * from desig";
            bun_addesig_dgv.DataSource = db.FetchData(s);
        }
        private void new_btn_DD_Click(object sender, EventArgs e)
        {
            dclear();
        }
        private void save_btn_DD_Click(object sender, EventArgs e)
        {
            if (bun_addesign_tb.Text != "" && bun_addesigd_tb.Text != "")
            {
                cmd.CommandText = ("Select * From desig Where dname ='" + bun_addesign_tb.Text.Trim() + "'  ");
                if (db.checkexist(cmd) == false)
                {
                    SqlCommand cmd = new SqlCommand("Insert into desig values( @a,'" + bun_addesigd_tb.Text + "')", db.con);
                    cmd.Parameters.AddWithValue("@a", bun_addesign_tb.Text);
                    i = db.InsertData(cmd);
                    if (i == 1)
                        MessageBox.Show("Inserted");
                    dclear();
                }
                else
                {
                    MessageBox.Show("ENTER unique Designation");
                }
            }
            else
            {
                MessageBox.Show("ENTER ALL DETAILS");
            }
        }

        private void list_btn_DD_Click(object sender, EventArgs e)
        {
            addesig_dgv();
        }

        private void bun_addesig_dgv_DoubleClick(object sender, EventArgs e)
        {
            if (bun_addesig_dgv.CurrentRow != null)
            {
                int cr = bun_addesig_dgv.CurrentRow.Index;
                if (bun_addesig_dgv.CurrentRow.Cells["add_dname"].Value != DBNull.Value)
                {
                    bun_addesign_tb.Text = Convert.ToString(bun_addesig_dgv[0, cr].Value);
                    bun_addesigd_tb.Text = Convert.ToString(bun_addesig_dgv[1, cr].Value);
                }

            }

        }
        private void modify_btn_DD_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From desig Where dname ='" + bun_addesign_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("update  desig set dname='" + bun_addesign_tb.Text + "',Descr='" + bun_addesigd_tb.Text + "' Where dname ='" + bun_addesign_tb.Text.Trim() + "'  ");
                db.ExecuteQuery(cmd); MessageBox.Show("ROW Modified"); dclear();
            }
            else
            {
                MessageBox.Show("ENTER Known Designation");
            }
        }
        private void delete_btn_DD3_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From desig Where dname ='" + bun_addesign_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("delete from  desig  Where dname ='" + bun_addesign_tb.Text.Trim() + "'  ");
                db.ExecuteQuery(cmd); dclear(); MessageBox.Show("ROW DELETED");
            }
               
            else
            {
                MessageBox.Show("ENTER unique Designation");
            }
        }
   //------------------------------------------------------------------------------------------------------------------------------
        private void new_btn_area_Click(object sender, EventArgs e)
        {
            aclear();
        }
        private void addarea_dgv()
        {
            s = "select * from area";
            bun_addar_dgv.DataSource = db.FetchData(s);
        }
        private void list_btn_area_Click(object sender, EventArgs e)
        {
            addarea_dgv();
        }
        private void bun_addar_dgv_DoubleClick(object sender, EventArgs e)
        {
            if (bun_addar_dgv.CurrentRow != null)
            {
                int cr = bun_addar_dgv.CurrentRow.Index;
                if (bun_addar_dgv.CurrentRow.Cells["add_arnam"].Value != DBNull.Value)
                {
                    bun_addarn_tb.Text = Convert.ToString(bun_addar_dgv[0, cr].Value);
                    bun_addard_tb.Text = Convert.ToString(bun_addar_dgv[1, cr].Value);
                }
            }
                
        }

        private void save_btn_area_Click(object sender, EventArgs e)
        {
            if (bun_addarn_tb.Text != "" && bun_addard_tb.Text != "")
            {
                cmd.CommandText = ("Select * From area Where ar_name ='" + bun_addarn_tb.Text.Trim() + "'  ");
                if (db.checkexist(cmd) == false)
                {
                    SqlCommand cmd = new SqlCommand("Insert into area values( @a,'" + bun_addard_tb.Text + "')", db.con);
                    cmd.Parameters.AddWithValue("@a", bun_addarn_tb.Text);
                    i = db.InsertData(cmd);
                    if (i == 1)
                        MessageBox.Show("Inserted");
                    aclear();
                }
                else
                {
                    MessageBox.Show("ENTER unique ADDRESS");
                }
            }
            else
            {
                MessageBox.Show("ENTER ALL DETAILS");
            }
            aclear();
        }

        private void modify_btn_area_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From area Where ar_name ='" + bun_addarn_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("update  area set ar_name='" + bun_addarn_tb.Text + "',ar_det='" + bun_addard_tb.Text + "' Where ar_name ='" + bun_addarn_tb.Text.Trim() + "'  ");
                db.ExecuteQuery(cmd); aclear(); MessageBox.Show("ROW Modified");
            }
            else
            {
                MessageBox.Show("Enter Known ADDRESS");
            }

        }
        private void delete_btn_area_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From area Where ar_name ='" + bun_addarn_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("delete from  area  Where ar_name ='" + bun_addarn_tb.Text.Trim() + "'  ");
                db.ExecuteQuery(cmd);
                aclear(); MessageBox.Show("ROW DELETED");
            }
            else
            {
                MessageBox.Show("Enter Known ADDRESS");
            }
        }

        private void bunifuButton210_Click(object sender, EventArgs e)
        {
            QD_Panel.BringToFront ();
        }

        private void bunifuButton29_Click(object sender, EventArgs e)
        {
            ED_Panel.BringToFront ();
        }

        private void bunifuButton28_Click(object sender, EventArgs e)
        {
           area_panel.BringToFront ();
        }

       
    }
}
