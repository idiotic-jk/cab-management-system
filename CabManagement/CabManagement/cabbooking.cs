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
    public partial class cabbooking : UserControl
    {
        public int i;
        public string s,sp,svd;
        dbaccess db = new dbaccess();
        public SqlCommand cmd = new SqlCommand();
        public SqlDataReader rd = null;

        public cabbooking()
        {
            InitializeComponent();loadallcbx();
        }
        public void cabbookclear()
        {
            bun_bookno_tb.Clear(); bun_custname_dd.SelectedValue = -1; bun_custadd_tb.Clear(); bun_frompl_dd.SelectedValue = -1; bun_topl_dd.SelectedValue = -1; bun_cabno_dd.SelectedValue = -1; bun_cabdet_tb.Clear();
        }
        public void cabdis()
        {
            s = "select bookid,bookdate,bookcustn,bookcabno from cab_book";
            bun_cabook_dgv.DataSource = db.FetchData(s);
        }
        public void loadallcbx()
        {
            sendcustname(); sendfromplace();sendtoplace();sendcabno();
        }
        public void loadcbx(ComboBox cx,String s,String vd)
        {
            try
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(s, db.con);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                DataRow topItem = dtbl.NewRow();
                cx.ValueMember = vd;
                cx.DisplayMember = vd;
                topItem[0] = null;

                dtbl.Rows.InsertAt(topItem, 0);
                cx.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                // write exception info to log or anything else
                MessageBox.Show(ex.Message, "Error occured!");
            }
        }
        public void sendcustname()
        {
            sp = "select custname from cust";svd = "custname";
            loadcbx(bun_custname_dd,sp,svd);
        }
        public void sendfromplace()
        {
            sp = "select ar_name from area"; svd = "ar_name";
            loadcbx(bun_frompl_dd, sp, svd);
        }
        public void sendtoplace()
        {
            sp = "select ar_name from area"; svd = "ar_name";
            loadcbx(bun_topl_dd, sp, svd);
        }
        public void sendcabno()
        {
            sp = "select cab_reg from cab"; svd = "cab_reg";
            loadcbx(bun_cabno_dd, sp, svd);
        }

        private void bun_custname_dd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(bun_custname_dd.SelectedIndex!=0)
            {
                SqlCommand cmd = new SqlCommand("select custadd from cust where custname='" + bun_custname_dd.SelectedValue + "'", db.con);
                using (rd = db.passread(cmd))
                {
                    
                    if (rd.Read())
                    {                        
                        bun_custadd_tb.Text = (rd.GetValue(0).ToString());
                    }
                    db.con.Close();
                }
            }            
        }

        private void bun_cabno_dd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bun_cabno_dd.SelectedIndex != 0)
            {
                SqlCommand cmd = new SqlCommand("select cab_desc from cab where cab_reg='" + bun_cabno_dd.SelectedValue + "'", db.con);
                using ( rd = db.passread(cmd))
                {
                    
                    if (rd.Read())
                    {
                        bun_cabdet_tb.Text = (rd.GetValue(0).ToString());
                    }
                    db.con.Close();
                }
            }

        }

        private void list_btn_qd_Click(object sender, EventArgs e)
        {
            cabdis();
        }

        private void modify_btn_qd_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From cab_book Where bookid ='" + bun_bookno_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("update  cab_book set bookdate=@b ,bookcustn = @c , bookcustadd = @d , booktime = @e ,bookfrpl=@f ,booktopl=@g,bookcabno=@h,bookcabdetails=@i Where bookid = @a  ");
                cmd.Parameters.AddWithValue("@a", bun_bookno_tb.Text);
                cmd.Parameters.AddWithValue("@b", bookdate_dtp.Value.Date);
                cmd.Parameters.AddWithValue("@c", bun_custname_dd.SelectedValue);
                cmd.Parameters.AddWithValue("@d", bun_custadd_tb.Text);
                cmd.Parameters.AddWithValue("@e", put_dtp.Value);
                cmd.Parameters.AddWithValue("@f", bun_frompl_dd.SelectedValue);
                cmd.Parameters.AddWithValue("@g", bun_topl_dd.SelectedValue);
                cmd.Parameters.AddWithValue("@h", bun_cabno_dd.SelectedValue);
                cmd.Parameters.AddWithValue("@i", bun_cabdet_tb.Text);
                db.ExecuteQuery(cmd); cabbookclear(); MessageBox.Show("ROW UPDATED");
            }
            else
                MessageBox.Show("ENTER Known Booking ID");

        }

        private void bun_cabook_dgv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (bun_cabook_dgv.CurrentRow != null)
            {
                string a = "", b = "";
                int cr = bun_cabook_dgv.CurrentRow.Index;
                if (bun_cabook_dgv.CurrentRow.Cells["book_no"].Value != DBNull.Value)
                {
                    SqlCommand cmd = new SqlCommand("select * from cab_book where bookid='" + bun_cabook_dgv.CurrentRow.Cells["book_no"].Value + "'", db.con);
                    using (rd = db.passread(cmd))
                    {
                        
                        if (rd.Read())
                        {
                            bun_bookno_tb.Text = rd.GetValue(0).ToString();
                            bookdate_dtp.Value = Convert.ToDateTime(rd.GetValue(1)).Date;
                            put_dtp.Value = DateTime.Parse(rd.GetValue(4).ToString());
                            bun_frompl_dd.SelectedValue = rd.GetValue(5).ToString();
                            bun_topl_dd.SelectedValue = rd.GetValue(6).ToString();
                            a = rd.GetValue(7).ToString();
                            b = rd.GetValue(2).ToString();
                        }
                        db.con.Close(); bun_cabno_dd.SelectedValue = a; bun_custname_dd.SelectedValue = b;
                    }
                }
            }
                
        }

        private void Delete_btn_qd_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From cab_book Where bookid ='" + bun_bookno_tb.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("delete from  cab_book  Where bookid ='" + bun_bookno_tb.Text.Trim() + "'  ");
                db.ExecuteQuery(cmd); cabbookclear(); MessageBox.Show("ROW Deleted");
            }
            else
                MessageBox.Show("ENTER Known Booking ID");
            
        }

        private void bun_custname_dd_Click(object sender, EventArgs e)
        {
            sendcustname(); 
        }

        private void bun_frompl_dd_Click(object sender, EventArgs e)
        {
            sendfromplace(); 
        }

        private void bun_topl_dd_Click(object sender, EventArgs e)
        {
            sendtoplace();
        }

        private void bun_cabno_dd_Click(object sender, EventArgs e)
        {
            sendcabno();
        }

        private void new_btn_qd_Click(object sender, EventArgs e)
        {
            cabbookclear();
        }

        private void Save_btn_qd_Click(object sender, EventArgs e)
        {
            if (bun_bookno_tb.Text != ""  && bun_custname_dd.SelectedValue != null && bun_custadd_tb.Text != ""  && bun_frompl_dd.SelectedValue != null && bun_topl_dd.SelectedValue != null && bun_cabno_dd.SelectedValue != null && bun_cabdet_tb.Text != "")
            {
                cmd.CommandText = ("Select * From cab_book Where bookid ='" + bun_bookno_tb.Text.Trim() + "'  ");
                if (db.checkexist(cmd) == false)
                {
                    SqlCommand cmd = new SqlCommand("Insert into cab_book values( @a,@b,@c,@d,@e,@f,@g,@h,@i)", db.con);
                    cmd.Parameters.AddWithValue("@a", bun_bookno_tb.Text);
                    cmd.Parameters.AddWithValue("@b", bookdate_dtp.Value.Date);
                    cmd.Parameters.AddWithValue("@c", bun_custname_dd.SelectedValue);
                    cmd.Parameters.AddWithValue("@d", bun_custadd_tb.Text);
                    cmd.Parameters.AddWithValue("@e", put_dtp.Value);
                    cmd.Parameters.AddWithValue("@f", bun_frompl_dd.SelectedValue);
                    cmd.Parameters.AddWithValue("@g", bun_topl_dd.SelectedValue);
                    cmd.Parameters.AddWithValue("@h", bun_cabno_dd.SelectedValue);
                    cmd.Parameters.AddWithValue("@i", bun_cabdet_tb.Text);
                    
                    i = db.InsertData(cmd);
                    if (i == 1)
                        MessageBox.Show("Inserted");
                    cabbookclear();
                }
                else
                   MessageBox.Show("ENTER unique Booking Id");                
            }
            else            
                MessageBox.Show("ENTER DETAILS");            
        }
    }
}
