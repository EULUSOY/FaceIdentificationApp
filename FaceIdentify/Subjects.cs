using FaceIdentify.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Luxand;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.IO;

namespace FaceIdentify
{
    public struct TFaceRecord
    {
        public byte[] Template; //Face Template;
        public FSDK.TFacePosition FacePosition;
        public FSDK.TPoint[] FacialFeatures; //Facial Features;

        public string ImageFileName;
        public string subjectName;

        public FSDK.CImage image;
        public FSDK.CImage faceImage;
    }

    public partial class Subjects : Form
    {
        string imgLoc = "";
        SqlCeCommand cmd;
        SqlCeConnection conn;

        string stringCon = "DataSource=\"Subjects.sdf\"; Password=\"1234\"";




        // readonly SqlCeConnection _connection = new SqlCeConnection(@"Data Source=C:\Users\GURKAN\Desktop\Projects\FaceIdentificationApp-master\FaceIdentify\bin\Debug\test01.sdf");

        bool size = true;
        Image Folder = Resources.Add_File_26;

        public static float FaceDetectionThreshold = 3;
        public static float FARValue = 100;

        public static List<TFaceRecord> FaceList;
        

        public Subjects()
        {
            InitializeComponent();
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                try
                {
                    dlg.Filter = "JPEG (*.jpg)|*.jpg|Windows bitmap (*.bmp)|*.bmp|All files|*.*";
                    dlg.Title = "Select Image";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        imgLoc = dlg.FileName.ToString();
                        pictureBox4.ImageLocation = imgLoc;
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TFaceRecord fr = new TFaceRecord();

            FSDK.SetFaceDetectionParameters(false, true, 384);
            FSDK.SetFaceDetectionThreshold((int)FaceDetectionThreshold);

             fr.ImageFileName = imgLoc;
             fr.FacePosition = new FSDK.TFacePosition();
             fr.FacialFeatures = new FSDK.TPoint[2];
             fr.Template = new byte[FSDK.TemplateSize];

             fr.image = new FSDK.CImage(imgLoc);

             fr.FacePosition = fr.image.DetectFace();
             if (0 == fr.FacePosition.w)
                    if (imgLoc.Length <= 1)
                        MessageBox.Show("No faces found. Try to lower the Minimal Face Quality parameter in the Options dialog box.", "Enrollment error");
                    else
                    { }
             else
             {
                 fr.faceImage = fr.image.CopyRect((int)(fr.FacePosition.xc - Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.yc - Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.xc + Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.yc + Math.Round(fr.FacePosition.w * 0.5)));
                 fr.FacialFeatures = fr.image.DetectEyesInRegion(ref fr.FacePosition);
                 fr.Template = fr.image.GetFaceTemplateInRegion(ref fr.FacePosition); // get template with higher precision

                 FaceList.Add(fr);
             }
                

            Image img = null;
            Image img_face = null;
            MemoryStream strm = new MemoryStream();
            MemoryStream strm_face = new MemoryStream();
            img = fr.image.ToCLRImage();
            img_face = fr.faceImage.ToCLRImage();
            img.Save(strm, System.Drawing.Imaging.ImageFormat.Jpeg);
            img_face.Save(strm_face, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] img_array = new byte[strm.Length];
            byte[] img_face_array = new byte[strm_face.Length];
            strm.Position = 0;
            strm.Read(img_array, 0, img_array.Length);
            strm_face.Position = 0;
            strm_face.Read(img_face_array, 0, img_face_array.Length);

            conn = new SqlCeConnection(stringCon);
            conn.Open();

            var cmd = new SqlCeCommand("insert into FaceList (ImageFileName,SubjectName,FacePositionXc,FacePositionYc,FacePositionW,FacePositionAngle,Eye1X,Eye1Y,Eye2X,Eye2Y,Template,Image,FaceImage) values (@IFName,@SName,@FPXc,@FPYc,@FPW,@FPA,@Eye1X,@Eye1Y,@Eye2X,@Eye2Y,@Template,@Image,@FaceImage)", conn);



            cmd.Parameters.Add(@"IFName", fr.ImageFileName);
            cmd.Parameters.Add(@"SName", textBox1.Text.Trim());
            cmd.Parameters.Add(@"FPXc" , fr.FacePosition.xc);
            cmd.Parameters.Add(@"FPYc", fr.FacePosition.yc);
            cmd.Parameters.Add(@"FPW",  fr.FacePosition.w);
            cmd.Parameters.Add(@"FPA", fr.FacePosition.angle);
            cmd.Parameters.Add(@"Eye1X", fr.FacialFeatures[0].x);
            cmd.Parameters.Add(@"Eye1Y", fr.FacialFeatures[0].y);
            cmd.Parameters.Add(@"Eye2X", fr.FacialFeatures[1].x);
            cmd.Parameters.Add(@"Eye2Y", fr.FacialFeatures[1].y);
            cmd.Parameters.Add(@"Template", fr.Template);
            cmd.Parameters.Add(@"Image",img_array);
            cmd.Parameters.Add(@"FaceImage", img_face_array);


           int x= cmd.ExecuteNonQuery();

            conn.Close();
            conn.Dispose();
            cmd.Dispose();
            MessageBox.Show(x.ToString()+"Image successfully added !!");

          
        }

        private void Subjects_Load(object sender, EventArgs e)
        {
            DBConnection db = new DBConnection();
            db.LoadDB();
            TFaceRecord tr = new TFaceRecord();
                tr = db.dbList[0];
                MessageBox.Show(tr.Template.ToString());
           
           

            FaceList = new List<TFaceRecord>();
            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary("VBsVmYmHr/5JxUlk3q0KHjILz7R3Hb5OEhCQ7KdCg/tPbQqJfAaz8ok/9+iTgDp/KjGjkBi23HeCaUq8KKtKeXXN3xbe+bKfQ8q/3mfG6sad3AGUYDj6E+Qi2pzCWFgb4vqWDB3pLzUw+hnOZ7///CBV63IaB1kh7XF6VCaGtNw="))
            {
                MessageBox.Show("Please run the License Key Wizard (Start - Luxand - FaceSDK - License Key Wizard)", "Error activating FaceSDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            if (FSDK.InitializeLibrary() != FSDK.FSDKE_OK)
                MessageBox.Show("Error initializing FaceSDK!", "Error");

            


            
           

        }
    }    
}