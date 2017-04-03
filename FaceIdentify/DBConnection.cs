using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;

namespace FaceIdentify
{
    class DBConnection
    {
        SqlCeCommand cmd;
        SqlCeConnection conn;
        string stringCon = "DataSource=\"facedb.sdf\"; Password=\"1234\"";

        public void CreateDB(string conString)
        {
            
            SqlCeEngine en = new SqlCeEngine(conString);

            if (!File.Exists("facedb.sdf"))
            {
                en.CreateDatabase();
            }

        }

        public void CreateTable(string conString)
        {

            using (conn = new SqlCeConnection(stringCon))
            {
                conn.Open();

                using (cmd = new SqlCeCommand(@"create table FaceList(
                     ImageFileName nvarchar primary key,
                     SubjectName nvarchar,
                     FacePositionXc int not null,
                     FacePositionYc int not null,
                     FacePositionW int not null,
                     FacePositionAngle real not null,
                     Eye1X int not null,
                     Eye1Y int not null,
                     Eye2X int not null,
                     Eye2Y int not null,
                     Template varbinary not null,
                     Image varbinary not null,
                     FaceImage varbinary not null

                    )", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

        }

    }
}
