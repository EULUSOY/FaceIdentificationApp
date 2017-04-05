using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using Luxand;
using System.Drawing;
using System.Windows.Forms;

namespace FaceIdentify
{
   class DBConnection
    {
        public System.Collections.Generic.List<TFaceRecord>dbList= new List<TFaceRecord>();
       
        private SqlCeCommand cmd;
        private SqlCeConnection conn;
        string stringCon = "DataSource=\"FaceDB.sdf\"; Password=\"1234\"";

        public void CreateDB()
        {
            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary("VBsVmYmHr/5JxUlk3q0KHjILz7R3Hb5OEhCQ7KdCg/tPbQqJfAaz8ok/9+iTgDp/KjGjkBi23HeCaUq8KKtKeXXN3xbe+bKfQ8q/3mfG6sad3AGUYDj6E+Qi2pzCWFgb4vqWDB3pLzUw+hnOZ7///CBV63IaB1kh7XF6VCaGtNw="))
            {
                MessageBox.Show("Please run the License Key Wizard (Start - Luxand - FaceSDK - License Key Wizard)", "Error activating FaceSDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (FSDK.InitializeLibrary() != FSDK.FSDKE_OK)
                MessageBox.Show("Error initializing FaceSDK!", "Error");

            SqlCeEngine en = new SqlCeEngine(stringCon);

            if (!File.Exists("FaceDB.sdf"))
            {
                en.CreateDatabase();
                CreateTable();
                CreateSettingsTable();
            }

        }

        public void CreateTable()
        {

            using (conn = new SqlCeConnection(stringCon))
            {
                conn.Open();

                using (cmd = new SqlCeCommand(@"create table FaceList(
                     ImageFileName nvarchar(256) primary key,
                     SubjectName nvarchar(256) not null,
                     FacePositionXc int not null,
                     FacePositionYc int not null,
                     FacePositionW int not null, 
                     FacePositionAngle real not null,
                     Eye1X int not null,
                     Eye1Y int not null,
                     Eye2X int not null,
                     Eye2Y int not null,
                     Template image not null,
                     Image image not null,
                     FaceImage image not null
                    )", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }








        public void CreateSettingsTable()
        {

            using (conn = new SqlCeConnection(stringCon))
            {
                conn.Open();

                using (cmd = new SqlCeCommand(@"create table Settings(
                     ThresholdValue int not null
                    )", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }





        public void LoadDB()
{
    
          

    using (conn = new SqlCeConnection(stringCon))
    {
        conn.Open();
        using (cmd = new SqlCeCommand(@"SELECT * FROM FaceList",conn))
        {
            SqlCeDataReader re = cmd.ExecuteReader();
            while(re.Read())
            {
                TFaceRecord fr = new TFaceRecord();
                fr.ImageFileName = re.GetString(0);
                fr.subjectName = re.GetString(1);

                fr.FacePosition = new FSDK.TFacePosition();
                fr.FacePosition.xc = re.GetInt32(2);
                fr.FacePosition.yc = re.GetInt32(3);
                fr.FacePosition.w = re.GetInt32(4);
                fr.FacePosition.angle = re.GetFloat(5);

                fr.FacialFeatures = new FSDK.TPoint[2];
                fr.FacialFeatures[0] = new FSDK.TPoint();
                fr.FacialFeatures[0].x = re.GetInt32(6);
                fr.FacialFeatures[0].y = re.GetInt32(7);

                fr.FacialFeatures[1] = new FSDK.TPoint();
                fr.FacialFeatures[1].x = re.GetInt32(8);
                fr.FacialFeatures[1].y = re.GetInt32(9);

                fr.Template = new byte[FSDK.TemplateSize];
                re.GetBytes(10, 0, fr.Template, 0, FSDK.TemplateSize);

                Image img = Image.FromStream(new System.IO.MemoryStream(re.GetSqlBinary(11).Value));
                Image img_face = Image.FromStream(new System.IO.MemoryStream(re.GetSqlBinary(12).Value));
                fr.image = new FSDK.CImage(img);
                fr.faceImage = new FSDK.CImage(img_face);

                

                dbList.Add(fr);

                img.Dispose();
                img_face.Dispose();

            }
        }
                conn.Close();
    }
 }
    }
}
