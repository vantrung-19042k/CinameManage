using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeTai
{
    public partial class FormPhim : Form
    {
        public FormPhim()
        {
            InitializeComponent();
        }

        //string path;
        //string pathMangPhim;
        //Random rand = new Random();
        //string[] mangAnh;

        //public void AddNewPhim()
        //{
        //    int so;
        //    PictureBox pictureBox1;
        //    so = rand.Next(1, 8);
        //    path = Application.StartupPath + @"\ImagePhim\" + so.ToString() + ".jpg";
        //    //1. tạo mới đối tượng
        //    pictureBox1 = new PictureBox();

        //    //2. thiết lập thuộc tính
        //    pictureBox1.Image = Image.FromFile(path);
        //    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        //    pictureBox1.BorderStyle = BorderStyle.FixedSingle;
        //    pictureBox1.Width = 80;
        //    pictureBox1.Height = 100;
        //    pictureBox1.Location = new Point(0, 0);

        //    //3.Add pic vao penal
        //    this.Controls.Add(pictureBox1);


        //}


        public void AddPbPhim()
        {
            PictureBox pbPhim;
            Panel panelContainPhim;
            Label lbTenPhim;

            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();

            string sql = "select maPhim, tenPhim, caChieu, hinhAnh from phim";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            MySqlDataReader reader = cmd.ExecuteReader();

            string ca = cbCaChieu.Text;

            if (!string.IsNullOrWhiteSpace(cbCaChieu.Text))
            {
                while (reader.Read())
                {
                    if (reader.GetString(2).Equals(ca))
                    {
                        // tạo đối tượng pictureBox và cài đặt thuộc tính
                        pbPhim = new PictureBox();
                        string hinhAnh = reader.GetString(3);
                        pbPhim.Image = ByteToImg(hinhAnh);
                        pbPhim.SizeMode = PictureBoxSizeMode.StretchImage;
                        pbPhim.BorderStyle = BorderStyle.FixedSingle;
                        pbPhim.Width = 200;
                        pbPhim.Height = 220;
                        pbPhim.Dock = DockStyle.Top;

                        //tạo đối tượng lable và cài đặt thuộc tính
                        lbTenPhim = new Label();
                        lbTenPhim.BorderStyle = BorderStyle.FixedSingle;
                        lbTenPhim.Width = 200;
                        lbTenPhim.Height = 30;
                        lbTenPhim.Text = reader.GetString(1);
                        lbTenPhim.TextAlign = ContentAlignment.MiddleCenter;
                        lbTenPhim.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                        lbTenPhim.Dock = DockStyle.Bottom;


                        //tạo đối tượng panel và cài đặt thuộc tính
                        panelContainPhim = new Panel();
                        panelContainPhim.BorderStyle = BorderStyle.FixedSingle;
                        panelContainPhim.Width = 200;
                        panelContainPhim.Height = 250;


                        panelContainPhim.Controls.Add(lbTenPhim);
                        panelContainPhim.Controls.Add(pbPhim);
                        flowLayoutPanelDsPhim.Controls.Add(panelContainPhim);

                        lbTenPhim.Click += LbTenPhim_Click;
                    }
                }
            }
            else
            {
                AddLable();
            }
        }

        private void LbTenPhim_Click(object sender, EventArgs e)
        {          
            string getTenPhim = ((Label)sender).Text;

            try
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();

                string sql = "select maPhim, tenPhim from phim" +
                    " where tenPhim = ?tenPhim";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("?tenPhim", getTenPhim);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DatVe.idPhim = reader.GetString(0);
                    DatVe.tenPhim = HoaDon.tenPhim = reader.GetString(1);
                    break;
                }

                this.Hide();
                FormVe.Ticket.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Close();
            }
        }

        //private void PbPhim_Click(object sender, EventArgs e)
        //{
        //    /*PictureBox pic = (PictureBox)sender*/;

        //    //chuyển ảnh về dạng chuỗi string
        //    string getTenPhim = Convert.ToBase64String(ImageToByte(((PictureBox)sender).Image));

        //    txtTen.Text = getTenPhim;
        //    string tenPhim = "";
        //    string idPhim = "";

        //    try
        //    {
        //        MySqlConnection conn = DBUtils.GetDBConnection();
        //        conn.Open();

        //        string sql = "select maPhim, tenPhim, hinhAnh from phim" +
        //            " where hinhAnh = ?hinhAnh";
        //        MySqlCommand cmd = new MySqlCommand(sql, conn);
        //        cmd.Parameters.AddWithValue("?hinhAnh", getTenPhim);

        //        MySqlDataReader reader = cmd.ExecuteReader();
        //        while(reader.Read())
        //        {
        //            idPhim = reader.GetString(0);
        //            tenPhim = reader.GetString(1);                
        //        }
        //        //this.Hide();

        //        txtTenPhim.Text = tenPhim;
        //        txtMaPhim.Text = idPhim;

        //        DatVe.tenPhim = tenPhim;
        //        DatVe.idPhim = idPhim;


        //        //FormVe.Ticket.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        //this.Close();
        //    }         
        //}

        private void FormPhim_Load(object sender, EventArgs e)
        {
            AddPbPhim();
        }

        //thêm lable thông báo
        public void AddLable()
        {
            Label nullPhim = new Label();
            nullPhim.BorderStyle = BorderStyle.Fixed3D;
            nullPhim.Width = 853;
            nullPhim.Height = 100;
            nullPhim.Text = "Chọn ca chiếu để hiển thị phim";
            nullPhim.TextAlign = ContentAlignment.MiddleCenter;
            nullPhim.Font = new Font("Microsoft Sans Serif", 40, FontStyle.Regular);
            flowLayoutPanelDsPhim.Controls.Add(nullPhim);
        }

        public void AddLable2()
        {
            Label nullPhim = new Label();
            nullPhim.BorderStyle = BorderStyle.Fixed3D;
            nullPhim.Width = 853;
            nullPhim.Height = 100;
            nullPhim.Text = "Không có phim ở ca này";
            nullPhim.TextAlign = ContentAlignment.MiddleCenter;
            nullPhim.Font = new Font("Microsoft Sans Serif", 40, FontStyle.Regular);
            flowLayoutPanelDsPhim.Controls.Add(nullPhim);
        }

        //chuyển chuỗi byte thành hình ảnh
        public Image ByteToImg(string byteString)
        {
            byte[] imgBytes = Convert.FromBase64String(byteString);
            MemoryStream ms = new MemoryStream(imgBytes, 0, imgBytes.Length);
            ms.Write(imgBytes, 0, imgBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }      

        //chuyển ảnh thành chuỗi byte
        public byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        //lọc các phim đang có trong ca chiếu được chọn
        private void cbCaChieu_TextChanged(object sender, EventArgs e)
        {
            flowLayoutPanelDsPhim.Controls.Clear();
            AddPbPhim();
            if (flowLayoutPanelDsPhim.Controls.Count == 0)
                AddLable2();
        }

        
    }
}
