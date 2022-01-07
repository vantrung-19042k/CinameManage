using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeTai
{
    public partial class SavePicture : Form
    {
        public SavePicture()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {          
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();
            string file = openFile.FileName;
            Image myImage = Image.FromFile(file);
            textBox1.Text = file;
            pbSource.Image = myImage;

        }

        private void btnSaveToDatabase_Click(object sender, EventArgs e)
        {
            //MemoryStream stream = new MemoryStream();
            //pbSource.Image.Save(stream, ImageFormat.Jpeg);

            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();

            string sql = "insert into phimdangchieu(maPhim, tenPhim, hinhAnh) values(?maPhim, ?tenPhim, ?hinhAnh)";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("?maPhim", "1");
            cmd.Parameters.AddWithValue("?tenPhim", "a");
            cmd.Parameters.AddWithValue("?hinhAnh", Convert.ToBase64String(converImgToByte()));

            cmd.ExecuteNonQuery();
            richTextBox1.Text = Convert.ToBase64String(converImgToByte());
        }

        public byte[] converImgToByte()
        {
            FileStream fs;
            fs = new FileStream(textBox1.Text, FileMode.Open, FileAccess.Read);
            byte[] picbyte = new byte[fs.Length];
            fs.Read(picbyte, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            return picbyte;
        }

        public Image ByteToImg(string byteString)
        {
            byte[] imgBytes = Convert.FromBase64String(byteString);
            MemoryStream ms = new MemoryStream(imgBytes, 0, imgBytes.Length);
            ms.Write(imgBytes, 0, imgBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();

            string sql = "select hinhAnh from phimdangchieu";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                string textPic = reader.GetString(0);
                pb2Des.Image = ByteToImg(textPic);
            }
            reader.Close();
            cmd.ExecuteReader();
        }
    }
}
