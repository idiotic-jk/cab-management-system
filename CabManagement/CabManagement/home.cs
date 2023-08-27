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
    public partial class home : Form
    {
        
        public home()
        {
            InitializeComponent();
         
    }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Application.Exit ();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            masterEntries1.BringToFront ();
        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {
            empdetls1.BringToFront();
        }

        private void bunifuButton23_Click(object sender, EventArgs e)
        {
            custdet1.BringToFront();
        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {
            cab1.BringToFront();
        }

        private void bunifuButton25_Click(object sender, EventArgs e)
        {
            cabbooking1.BringToFront();
        }

        private void bunifuButton26_Click(object sender, EventArgs e)
        {
            billing1.BringToFront();
        }

        private void bunifuButton27_Click(object sender, EventArgs e)
        {
            reports1.BringToFront();
        }

        private void bunifuButton29_Click(object sender, EventArgs e)
        {
            blank1.BringToFront();
        }

        private void blank1_Load(object sender, EventArgs e)
        {

        }
        private bool mouseDown;
        private Point lastLocation;

        private void home_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void home_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void home_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        private void home_Load(object sender, EventArgs e)
        {

        }
    }
}
