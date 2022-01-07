using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeTai
{
    public partial class HoaDon : Form
    {
        public HoaDon()
        {
            InitializeComponent();
        }
        public static string tenPhim;
        public static int giaGhe;
        public static string gheDat;
        public static string thucAn;
        public static decimal soLuongGa;
        public static decimal soLuongBap;
        public static int giaGa;
        public static int giaBap;
        public static string danhSachGhe;
        int tongHoaDon = 0;

        private static List<FormDichVu.DichVu> dsDichVu = new List<FormDichVu.DichVu>();
        public static List<FormDichVu.DichVu> DsDichVu { get => dsDichVu; set => dsDichVu = value; }


        //private void txtHoaDon_TextChanged(object sender, EventArgs e)
        //{
        //    //txtHoaDon.Text = gia.ToString();
        //}

        private void HoaDon_Load(object sender, EventArgs e)
        {
            int tongDichVu = 0;
            foreach (FormDichVu.DichVu dv in dsDichVu)
            {
                tongDichVu += int.Parse(dv.GiaBan) * int.Parse(dv.SoLuong);
            }

            tongHoaDon = giaGhe + tongDichVu;

            listboxHD.Items.Add(string.Format("Tên phim: {0}", tenPhim));
            listboxHD.Items.Add(string.Format("Ghế đã đặt: {0}", danhSachGhe));
            listboxHD.Items.Add("Thông tin dịch vụ đã chọn");

            //if (soLuongGa > 0)

            //    listboxHD.Items.Add(string.Format("Gà Rán x {0}   :{1}  ", soLuongGa, giaGa));
            //if (soLuongBap > 0)
            //    listboxHD.Items.Add(string.Format("Bắp Rang x {0}   :{1}  ", soLuongBap, giaBap));
            //tongHoaDon = (giaGhe + giaBap + giaGa);

            foreach(FormDichVu.DichVu dv in dsDichVu)
            {
                if (!dv.SoLuong.Equals("0"))
                {
                    String data = dv.TenDichVu + "   " +
                                      "   Giá bán: " + dv.GiaBan + ";" +
                                      "   Số lượng: " + dv.SoLuong +
                                      "   Thành tiền: " + ((int.Parse(dv.GiaBan) * int.Parse(dv.SoLuong)).ToString());
                    listboxHD.Items.Add(data);
                }
            }
            

            listboxHD.Items.Add(string.Format("======================================"));
            listboxHD.Items.Add(string.Format("Tổng Hóa Đơn:{0} vnd ", tongHoaDon));
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanh toán thành công");
            this.Close();
        }
    }

}
