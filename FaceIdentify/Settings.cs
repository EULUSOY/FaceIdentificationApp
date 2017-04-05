﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FaceIdentify
{
    public partial class Settings : Form
    {

        bool size = true;
        private Point downPoint;
        GroupBox sGroup;


        public Settings()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void Settings_Load(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.None)
            {
                this.MouseDown += new MouseEventHandler(AppFormBase_MouseDown);
                this.MouseMove += new MouseEventHandler(AppFormBase_MouseMove);
                this.MouseUp += new MouseEventHandler(AppFormBase_MouseUp);
            }

            panel1.Dock = DockStyle.Top;


            label3.Font = new Font(label3.Font, FontStyle.Bold);
            label2.Font = new Font(label2.Font, FontStyle.Bold);


            comboBox1.Items.Add("SQL Server Compact");
            comboBox1.Items.Add("Postgresql");
            comboBox1.Items.Add("Mysql");
            comboBox1.Items.Add("Oracle");
            comboBox1.SelectedIndex = 0;

            trackBar1.Minimum = 1;
            trackBar1.Maximum = 100;

            RadioButton rbGenderTrue = new RadioButton();
            rbGenderTrue.Text = "True";
            groupBox1.Controls.Add(rbGenderTrue);
            rbGenderTrue.Location = new Point(10, 20);
            RadioButton rbGenderFalse = new RadioButton();
            rbGenderFalse.Text = "False";
            groupBox1.Controls.Add(rbGenderFalse);
            rbGenderFalse.Location = new Point(10, 40);


            RadioButton rbExpressionTrue = new RadioButton();
            rbExpressionTrue.Text = "True";
            groupBox2.Controls.Add(rbExpressionTrue);
            rbExpressionTrue.Location = new Point(10, 20);
            RadioButton rbExpressionFalse = new RadioButton();
            rbExpressionFalse.Text = "False";
            groupBox2.Controls.Add(rbExpressionFalse);
            rbExpressionFalse.Location = new Point(10, 40);


            TreeNode node2 = new TreeNode("Source Database");
            TreeNode node3 = new TreeNode("VB.NET");
            TreeNode node4 = new TreeNode("Threshold Value");
            TreeNode node5 = new TreeNode("Detect Gender");
            TreeNode node6 = new TreeNode("Detect Expression");
            TreeNode node7 = new TreeNode("Restore Default Settings");
            TreeNode node8 = new TreeNode("Tracker Memory");
            TreeNode node9 = new TreeNode("Recognition Performance");

            TreeNode[] array = new TreeNode[] { node2, node4, node5, node6 ,node8, node9, node7};
            TreeNode treeNode = new TreeNode("Settings", array);

            treeView1.Nodes.Add(treeNode);

            treeView1.ExpandAll();
            




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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //MessageBox.Show(treeView1.SelectedNode.Text);


            if ("Detect Gender" == treeView1.SelectedNode.Text)
            {
                label3.Text = "Detect Gender";
                groupBox1.Visible = true;
                groupBox2.Visible = false;
                trackBar1.Visible = false;
                label1.Visible = false;
                comboBox1.Visible = false;

            }
            else if ("Detect Expression" == treeView1.SelectedNode.Text)
            {
                label3.Text = "Detect Expression";
                groupBox1.Visible = false;
                groupBox2.Visible = true;
                trackBar1.Visible = false;
                label1.Visible = false;
                comboBox1.Visible = false;


            }
            else if ("Threshold Value" == treeView1.SelectedNode.Text)
            {
                label3.Text = "Threshold Value";
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                trackBar1.Visible = true;
                label1.Visible = true;
                comboBox1.Visible = false;

            }
            else if ("Source Database" == treeView1.SelectedNode.Text)
            {
                label3.Text = "Source Database";
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                trackBar1.Visible = false;
                label1.Visible = false;
                comboBox1.Visible = true;

            }
            else if ("Tracker Memory" == treeView1.SelectedNode.Text)
            {
                label3.Text = "Tracker Memory";
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                trackBar1.Visible = false;
                label1.Visible = false;
                comboBox1.Visible = false;

            }
            else if ("Recognition Performance" == treeView1.SelectedNode.Text)
            {
                label3.Text = "Recognition Performance";
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                trackBar1.Visible = false;
                label1.Visible = false;
                comboBox1.Visible = false;

            }
            else if ("Restore Default Settings" == treeView1.SelectedNode.Text)
            {
                label3.Text = "Restore Default Settings";
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                trackBar1.Visible = false;
                label1.Visible = false;
                comboBox1.Visible = false;

            }




        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = trackBar1.Value.ToString();
        }
    }
}
