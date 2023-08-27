using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CabManagement
{
    public partial class login : UserControl
    {
        dbaccess db=new dbaccess();
        public login()
        {
            InitializeComponent();
        }

        private void modify_btn_qd_Click(object sender, EventArgs e)
        {
            string s = "Select Count(*) From [Login] where [users] = '" + bun_logUID_txb.Text + "'and [pass] = '" + bun_logpass_txb.Text + "'";
            DataTable dt= db.FetchData(s);          
            if (dt.Rows[0][0].ToString() == "1")
            {
                home f1 = new home();
                f1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("iv");
            }
        }

        private void new_btn_qd_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
