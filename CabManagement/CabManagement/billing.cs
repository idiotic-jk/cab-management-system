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
    
    public partial class billing : UserControl
    {
        public int i,cb,b;
        public string s, sp, svd;
        dbaccess db = new dbaccess();
        public SqlCommand cmd = new SqlCommand();
        public SqlDataReader rd = null;
        public billing()
        {
            InitializeComponent();loadallcbx();billdis();
        }
        public void loadallcbx()
        {
             sendfromplace(); sendtoplace(); sendcabno();
        }
        public void allclear()
        {
            bun_bookno.Clear(); bun_custn_tb.Clear(); bun_custadd.Clear(); bun_dis_tb.Clear(); bun_totamt_tb.Clear(); bun_tot_tb.Clear();
            bun_cabno_dd.SelectedIndex= 0; bun_from_dd.SelectedValue = -1; bun_tp_dd.SelectedValue = -1; bun_addcrg_tb.Clear();
        }
        public void billdis()
        {
            s = "select billid,bookid,billdate,billcustn,billcabno from Bill";
            bun_cabill_dgv.DataSource = db.FetchData(s);
        }
        public void loadcbx(ComboBox cx, String s, String vd)
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
        
        public void sendfromplace()
        {
            sp = "select ar_name from area"; svd = "ar_name";
            loadcbx(bun_from_dd, sp, svd);
        }
        public void sendtoplace()
        {
            sp = "select ar_name from area"; svd = "ar_name";
            loadcbx(bun_tp_dd, sp, svd);
        }

        private void bun_cabno_dd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bun_cabno_dd.SelectedIndex != 0)
            {
                SqlCommand cmd = new SqlCommand("select cab_ppkm from cab where cab_reg='" + bun_cabno_dd.SelectedValue + "'", db.con);
                using (rd = db.passread(cmd))
                {

                    if (rd.Read())
                    {
                        
                        bun_rate_tb.Text = (rd.GetValue(0).ToString());
                    }
                    db.con.Close();
                }

            }
            else
                bun_rate_tb.Clear();
        }

        private void bun_dis_tb_TextChanged(object sender, EventArgs e)
        {
            if (bun_dis_tb.Text.Length > 0 && bun_rate_tb.Text.Length > 0)
            {
                bun_tot_tb.Text = Convert.ToString(Convert.ToDecimal(bun_dis_tb.Text.Trim()) * Convert.ToDecimal(bun_rate_tb.Text.Trim()));
                if (bun_addcrg_tb.Text.Length > 0)
                    bun_totamt_tb.Text = Convert.ToString(Convert.ToDecimal(bun_tot_tb.Text.Trim()) + Convert.ToDecimal(bun_addcrg_tb.Text.Trim()));
            }

        }
        private void checkbook()
        {
            string a = "";
            cmd.CommandText = ("Select * From cab_book Where bookid ='" + bun_bookno.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                SqlCommand cmd = new SqlCommand("select * from cab_book where bookid='" + bun_bookno.Text.Trim() + "'", db.con);
                using (rd = db.passread(cmd))
                {
                    if (rd.Read())
                    {
                        
                        bookdate_dtp.Value = Convert.ToDateTime(rd.GetValue(1)).Date;
                        bun_custn_tb.Text = rd.GetValue(2).ToString();
                        bun_custadd.Text = rd.GetValue(3).ToString();
                        bun_from_dd.SelectedValue = rd.GetValue(5).ToString();
                        bun_tp_dd.SelectedValue = rd.GetValue(6).ToString();
                        a = rd.GetValue(7).ToString();
                    }
                    db.con.Close(); bun_cabno_dd.SelectedValue = a;
                }
            }
            else
            {
                MessageBox.Show("Enter valid Booking id");
            }
        }



        private void Save_btn_qd_Click(object sender, EventArgs e)
        {
            if (bun_bookno.Text != "" && bun_custn_tb.Text != "" && bun_custadd.Text != "" && bun_from_dd.SelectedValue != null && bun_tp_dd.SelectedValue != null && bun_cabno_dd.SelectedValue != null && bun_totamt_tb.Text != "")
            { cmd.CommandText = ("Select * From cab_book Where bookid ='" + bun_bookno.Text.Trim() + "'  ");
                if(db.checkexist(cmd) == true)
                {
                    cmd.CommandText = ("Select * From Bill Where bookid ='" + bun_bookno.Text.Trim() + "'  ");
                    if (db.checkexist(cmd) == false)
                    {
                        SqlCommand cmd = new SqlCommand("Insert into Bill values( @a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k)", db.con);
                        cmd.Parameters.AddWithValue("@a", bun_bookno.Text);
                        cmd.Parameters.AddWithValue("@b", bookdate_dtp.Value.Date);
                        cmd.Parameters.AddWithValue("@c", bun_custn_tb.Text);
                        cmd.Parameters.AddWithValue("@d", bun_custadd.Text);
                        cmd.Parameters.AddWithValue("@e", bun_from_dd.SelectedValue);
                        cmd.Parameters.AddWithValue("@f", bun_tp_dd.SelectedValue);
                        cmd.Parameters.AddWithValue("@g", bun_cabno_dd.SelectedValue);
                        cmd.Parameters.AddWithValue("@h", bun_dis_tb.Text);
                        cmd.Parameters.AddWithValue("@i", bun_rate_tb.Text);
                        cmd.Parameters.AddWithValue("@j", bun_addcrg_tb.Text);
                        cmd.Parameters.AddWithValue("@k", bun_totamt_tb.Text);
                        i = db.InsertData(cmd);
                        if (i == 1)
                            MessageBox.Show("Inserted");
                        allclear();
                    }
                    else
                    {
                        MessageBox.Show("ENTER unique qual");
                    }
                }
                else
                {
                    MessageBox.Show("ENTER valid booking no");
                }
                
            }
            else
            {
                MessageBox.Show("ENTER DETAILS");
            }
        }

        private void bun_dis_tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back))
            { e.Handled = true; }
        }

        private void new_btn_qd_Click(object sender, EventArgs e)
        {
            allclear();
        }

        private void bun_cabill_dgv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (bun_cabill_dgv.CurrentRow != null)
            {
                string a = "";
                int cr = bun_cabill_dgv.CurrentRow.Index;
                cb = Convert.ToInt32(bun_cabill_dgv[0, cr].Value);
                if (bun_cabill_dgv.CurrentRow.Cells["billno"].Value != DBNull.Value)
                {
                    SqlCommand cmd = new SqlCommand("select * from Bill where billid='" + bun_cabill_dgv.CurrentRow.Cells["billno"].Value + "'", db.con);
                    using (rd = db.passread(cmd))
                    {
                        
                        if (rd.Read())
                        {
                            b = Convert.ToInt32(rd.GetValue(0));
                            bun_bookno.Text = rd.GetValue(1).ToString();
                            bookdate_dtp.Value = Convert.ToDateTime(rd.GetValue(2)).Date;
                            bun_custn_tb.Text = rd.GetValue(3).ToString();
                            bun_custadd.Text = rd.GetValue(4).ToString();
                            bun_from_dd.SelectedValue = rd.GetValue(5).ToString();
                            bun_tp_dd.SelectedValue = rd.GetValue(6).ToString();
                            a = rd.GetValue(7).ToString();
                            bun_dis_tb.Text = rd.GetValue(8).ToString();
                            bun_addcrg_tb.Text = rd.GetValue(10).ToString();
                            bun_totamt_tb.Text = rd.GetValue(11).ToString();
                            bun_tot_tb.Text = Convert.ToString(Convert.ToDecimal(bun_totamt_tb.Text) - Convert.ToDecimal(bun_addcrg_tb.Text));
                        }
                        db.con.Close(); bun_cabno_dd.SelectedValue = a;
                    }
                }
            }
                
        }

        private void list_btn_qd_Click(object sender, EventArgs e)
        {
            billdis();
        }

        private void modify_btn_qd_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From Bill Where bookid ='" + bun_bookno.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = "update  Bill set bookid=@a ,billdate=@b ,billcustn = @c , billcustadd = @d , billfrpl = @e ,billtopl=@f ,billcabno=@g,billdist=@h,billrate=@i ,billadd=@j ,billtot=@k Where billid = '" + cb + "'  ";
                cmd.Parameters.AddWithValue("@a", bun_bookno.Text);
                cmd.Parameters.AddWithValue("@b", bookdate_dtp.Value.Date);
                cmd.Parameters.AddWithValue("@c", bun_custn_tb.Text);
                cmd.Parameters.AddWithValue("@d", bun_custadd.Text);
                cmd.Parameters.AddWithValue("@e", bun_from_dd.SelectedValue);
                cmd.Parameters.AddWithValue("@f", bun_tp_dd.SelectedValue);
                cmd.Parameters.AddWithValue("@g", bun_cabno_dd.SelectedValue);
                cmd.Parameters.AddWithValue("@h", bun_dis_tb.Text);
                cmd.Parameters.AddWithValue("@i", bun_rate_tb.Text);
                cmd.Parameters.AddWithValue("@j", bun_addcrg_tb.Text);
                cmd.Parameters.AddWithValue("@k", bun_totamt_tb.Text);
                db.ExecuteQuery(cmd); allclear(); MessageBox.Show("ROW Modified");
            }
            else
                MessageBox.Show("ENTER Known book-No");

                                                              
        }

        private void bun_from_dd_Click(object sender, EventArgs e)
        {
            loadallcbx();
        }

        private void bun_print_btn_Click(object sender, EventArgs e)
        {
            if(prtprewdilog.ShowDialog()==DialogResult.OK)
            {
                prtdoc.Print();
            }
        }

        private void prtdoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            Pen p = new Pen(Color.Black, 4);
            Font f1 = new Font("Times New Roman", 40, FontStyle.Bold);

            //e.Graphics.DrawString(b.ToString(), f1, Brushes.Black, 150, 200);
            //e.Graphics.DrawString(bun_bookno.Text, f1, Brushes.Black, 150, 200);
            //e.Graphics.DrawString(bun_bookno.Text, f1, Brushes.Black, 150, 200);
            //e.Graphics.DrawString(bun_bookno.Text, f1, Brushes.Black, 150, 200);
            //e.Graphics.DrawString(bun_bookno.Text, f1, Brushes.Black, 150, 200);
            //e.Graphics.DrawString(bun_bookno.Text, f1, Brushes.Black, 150, 200);

            //e.Graphics.DrawString(bun_bookno.Text, f1, Brushes.Black, 150, 200);
            //e.Graphics.DrawLine(p, 100, 100, 500, 100);
            IList<string> lstString = new List<string>();
            e.Graphics.DrawString("CAB SERVICE BILL", f1, Brushes.Black, 150, 200);
            e.Graphics.DrawString("........................................", f1, Brushes.Black, 150, 220);
            e.Graphics.DrawString("Sales Recipt",new Font("Times New Roman", 25, FontStyle.Bold), Brushes.Black, 150, 300);
            e.Graphics.DrawString("Date : "+DateTime.Now.ToShortDateString(), new Font("Times New Roman", 25, FontStyle.Regular), Brushes.Black, 500, 300);
            e.Graphics.DrawString("Customer name : " + bun_custn_tb.Text, new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, 150, 400);
            e.Graphics.DrawString("From  : " + bun_from_dd.Text , new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, 150, 450);
            e.Graphics.DrawString("TO : " + bun_tp_dd.Text, new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, 150, 500);
            e.Graphics.DrawString("Total Distance : " + bun_dis_tb.Text, new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, 150, 550);
            e.Graphics.DrawString("Charge per km : " + bun_rate_tb.Text, new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, 150, 600);
            e.Graphics.DrawString("Extra charges : " + bun_addcrg_tb.Text, new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, 150, 650);
            e.Graphics.DrawString("-----------------------------------------", new Font("Times New Roman", 20, FontStyle.Regular), Brushes.Black, 150, 700);
            e.Graphics.DrawString("TOTAL PAYMENT : " + bun_tot_tb.Text, new Font("Times New Roman", 30, FontStyle.Regular), Brushes.Black, 150, 800);
        

    }

        private void Delete_btn_qd_Click(object sender, EventArgs e)
        {
            cmd.CommandText = ("Select * From Bill Where bookid ='" + bun_bookno.Text.Trim() + "'  ");
            if (db.checkexist(cmd) == true)
            {
                cmd.CommandText = ("delete from Bill  Where bookid ='" + bun_bookno.Text.Trim() + "'  ");
                db.ExecuteQuery(cmd); allclear(); MessageBox.Show("ROW Deleted");
            }
            else
                MessageBox.Show("ENTER Known book-No");

        }

        public void sendcabno()
        {
            sp = "select cab_reg from cab"; svd = "cab_reg";
            loadcbx(bun_cabno_dd, sp, svd);
        }


        private void bunifuTextBox1_Leave(object sender, EventArgs e)
        {
            checkbook();
        }
    }
}
