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
    public partial class Reports : UserControl
    {
        String s;
        dbaccess db = new dbaccess();
        public SqlCommand cmd = new SqlCommand();
        public Reports()
        {
            InitializeComponent();
        }

        private void bunifuButton29_Click(object sender, EventArgs e)
        {
            emppanel1.BringToFront ();
            
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            custpanel2.BringToFront ();
        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
            cabpanel3.BringToFront ();
        }

        private void bunifuButton23_Click(object sender, EventArgs e)
        {
            bookingpanel4.BringToFront ();
        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {
            Billingpanel5.BringToFront ();
        }

        private void bunifuButton26_Click(object sender, EventArgs e)
        {
            s = "select * from cust";
            custdgv.DataSource = db.FetchData(s);
        }

        private void bun_empdet_but_Click(object sender, EventArgs e)
        {
            s = "select * from emp";
            empdgv.DataSource = db.FetchData(s);
        }

        private void bun_billdet_but_Click(object sender, EventArgs e)
        {
            s = "select * from Bill";
            billing_dgv.DataSource = db.FetchData(s);
        }

        private void bun_book_but_Click(object sender, EventArgs e)
        {
            s = "select * from cab_book";
            book_dgv.DataSource = db.FetchData(s);
        }

        private void bun_cabd_but_Click(object sender, EventArgs e)
        {
            s = "select * from cab";
            cabdgv.DataSource = db.FetchData(s);
        }
    }
}
