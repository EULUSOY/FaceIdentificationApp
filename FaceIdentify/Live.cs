using FaceIdentify.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FaceIdentify
{
    public partial class Live : Form
    {
        bool size = true;
        Image Folder = Resources.Add_File_26;

        private string folderName;
        private string selected;
        private string fileName;
        private Point downPoint;

        public Live()
        {
           InitializeComponent();
            pictureBox7.Image = Folder;
        }

        private void Live_Load(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.None)
            {
                this.MouseDown += new MouseEventHandler(AppFormBase_MouseDown);
                this.MouseMove += new MouseEventHandler(AppFormBase_MouseMove);
                this.MouseUp += new MouseEventHandler(AppFormBase_MouseUp);
            }

            panel1.Dock = DockStyle.Top;
        }

        private void AppFormBase_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = new Point(e.X, e.Y);
        }
        void AppFormBase_MouseMove(object sender, MouseEventArgs e)
        {
            if (downPoint == Point.Empty)
            {
                return;
            }
            Point location = new Point(
                this.Left + e.X - downPoint.X,
                this.Top + e.Y - downPoint.Y);
            this.Location = location;
        }

        private void AppFormBase_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = Point.Empty;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (size == true)
            {
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                size = false;
            }
            else if (size == false)
            {
                this.WindowState = FormWindowState.Normal;
                size = true;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            OpenDialog dl = new OpenDialog();   
            dl.ShowDialog();

            folderName = dl.folderName;
            fileName = dl.fileName;
            selected = dl.selectedTab;

            if(folderName != null && selected.ToString() =="Video" )
            {
                MessageBox.Show("Video folder tara");
            }
            else if(fileName !=null && selected.ToString() == "Video")
            {
                MessageBox.Show("Video tara");
            }
            else if(folderName != null && selected.ToString() == "Image")
            {
                MessageBox.Show("Image folder Tara");
            }
            else if(fileName != null && selected.ToString() == "Image")
            {
                MessageBox.Show("Image Tara");
            }
            //pictureBox7.Visible = false;
        }
    }
}
