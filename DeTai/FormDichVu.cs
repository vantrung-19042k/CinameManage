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
    public partial class FormDichVu : Form
    {
        public FormDichVu()
        {
            InitializeComponent();
        }

        private static List<DichVu> dsDichVu = new List<DichVu>();
        public static List<DichVu> DsDichVu { get => dsDichVu; set => dsDichVu = value; }


        public class DichVu
        {
            private string tenDichVu;
            private string giaBan;
            private string soLuong;

            public string TenDichVu { get => tenDichVu; set => tenDichVu = value; }
            public string GiaBan { get => giaBan; set => giaBan = value; }
            public string SoLuong { get => soLuong; set => soLuong = value; }
        }


        public void AddNewDichVu()
        {
            PictureBox pbDichVu;
            Panel pnContainDV;
            Label lbTenDichVu;
            NumericUpDown nrudSoLuong;

            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            //string sql = "select maDichVu, tenDichVu, giaBan, khuyenMai, hinhAnh from dichVu";
            string sql = "select * from dichVu";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // tạo đối tượng pictureBox và cài đặt thuộc tính
                pbDichVu = new PictureBox();
                string hinhAnh = reader.GetString(4);
                pbDichVu.Image = ByteToImg(hinhAnh);
                pbDichVu.SizeMode = PictureBoxSizeMode.StretchImage;
                pbDichVu.BorderStyle = BorderStyle.FixedSingle;
                pbDichVu.Width = 150;
                pbDichVu.Height = 130;
                pbDichVu.Dock = DockStyle.Bottom;

                //tạo đối tượng lable và cài đặt thuộc tính
                lbTenDichVu = new Label();
                lbTenDichVu.BorderStyle = BorderStyle.Fixed3D;
                lbTenDichVu.Width = 100;
                lbTenDichVu.Height = 30;
                //lbTenDichVu.Text = reader.GetString(1) + " " + "("+ reader.GetString(2) +"k/1) ";
                lbTenDichVu.Text = reader.GetString(1);
                lbTenDichVu.TextAlign = ContentAlignment.MiddleLeft;
                lbTenDichVu.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);

                //Tạo đối tượng numbericUpDown
                nrudSoLuong = new NumericUpDown();
                nrudSoLuong.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Regular);
                nrudSoLuong.Width = 50;
                nrudSoLuong.Dock = DockStyle.Right;
                nrudSoLuong.TextAlign = HorizontalAlignment.Center;


                //tạo đối tượng panel và cài đặt thuộc tính
                pnContainDV = new Panel();
                pnContainDV.BorderStyle = BorderStyle.FixedSingle;
                pnContainDV.Width = 150;
                pnContainDV.Height = 170;

                //thêm các button vào flpDsDichVu 
                pnContainDV.Controls.Add(nrudSoLuong);
                pnContainDV.Controls.Add(lbTenDichVu);
                pnContainDV.Controls.Add(pbDichVu);
                flpDsDichVu.Controls.Add(pnContainDV);
            }
        }

        private void FormDichVu_Load(object sender, EventArgs e)
        {
            AddNewDichVu();
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

        private static string soLuongThuc;

        private void btnChon_Click(object sender, EventArgs e)
        {          
            foreach (Panel pn in flpDsDichVu.Controls)
            {
                foreach(Control p in pn.Controls)
                {
                    DichVu dv = new DichVu();

                    if(p.GetType() == typeof(NumericUpDown))
                    {
                        soLuongThuc = ((NumericUpDown)p).Value.ToString();
                        //MessageBox.Show("Số lượng" + ((NumericUpDown)p).Value.ToString());
                    }

                    if(p.GetType() == typeof(Label))
                    {
                        MySqlConnection conn = DBUtils.GetDBConnection();
                        conn.Open();

                        string sql = "select tenDichVu, giaBan from dichvu";

                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.GetString(0).Equals(p.Text))
                            {
                                dv.TenDichVu = reader.GetString(0);
                                dv.GiaBan = reader.GetString(1);
                                dv.SoLuong = soLuongThuc;
                                DsDichVu.Add(dv);
                                //MessageBox.Show("Tên sản phẩm" + reader.GetString(0));
                                //MessageBox.Show("Giá bán" + reader.GetString(1));
                            }
                        }
                    }                  
                }
            }

            //foreach(DichVu dv in dsDichVu)
            //{
            //    MessageBox.Show("Tên dịch vụ: " + dv.TenDichVu);
            //    MessageBox.Show("Giá bán: " + dv.GiaBan);
            //    MessageBox.Show("Số lượng: " + dv.SoLuong);
            //}

            DatVe.DanhSachDV = HoaDon.DsDichVu = dsDichVu;
            this.Hide();
            FormVe.Ticket.Show();
            this.Close();
        }
    }
}
