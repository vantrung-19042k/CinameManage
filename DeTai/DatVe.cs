using MySql.Data.MySqlClient;
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
    public partial class DatVe : Form
    {
        public DatVe()
        {
            InitializeComponent();
        }

        //tạo danh sách dịch vụ kiểu list để nhận dữ liệu
        private static List<FormDichVu.DichVu> danhSachDV = new List<FormDichVu.DichVu>();
        public static List<FormDichVu.DichVu> DanhSachDV { get => danhSachDV; set => danhSachDV = value; }      

        //đặt vé xem phim
        private void BtDatVe_Click_1(object sender, EventArgs e)
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();

            int giaGhe = 0;

            string luuTenGhe = "";
            string luuIdGhe =  "";

            List<String> dsIdGhe = new List<string>();

            foreach (Control cc in gbxGhe.Controls)
            {
                if (((CheckBox)cc).Checked)
                {
                    giaGhe += 50000;
                    //Ghe ghe = new Ghe();
                    luuTenGhe += cc.Name + " ";
                    //dsIdGhe.Add((ReturnIdGhe(cc).ToString()));
                    luuIdGhe += (ReturnIdGhe(cc).ToString());
                    //dsGhe.Add(ghe);
                
                    try
                    {
                        string sql = "insert into datghe(idGhe, idPhim, idKhachHang) " +
                            "values(?idGhe, ?idPhim, ?idKhachHang)";

                        MySqlCommand cmd = new MySqlCommand(sql, conn);

                        cmd.Parameters.AddWithValue("?idGhe", ReturnIdGhe(cc).ToString());
                        cmd.Parameters.AddWithValue("?idPhim", idPhim);
                        cmd.Parameters.AddWithValue("?idKhachHang", idKhachHang);

                        cmd.ExecuteNonQuery();
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {

                    }
                }              
            }
            
            if((string.IsNullOrWhiteSpace(luuTenGhe) == true))
            {
                MessageBox.Show("Bạn chưa đặt vé nào!!!", "Tiếp tục", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lbxDetail.Items.Clear();
                lbxDetail.Items.Add(string.Format("Tên phim: {0}", tenPhim));
            }
            else
            {             
                HoaDon.danhSachGhe = luuTenGhe;
                HoaDon.giaGhe = giaGhe;
                lbxDetail.Items.Add(string.Format("Danh sách ghế đã đặt: {0}", luuTenGhe));
                lbxDetail.Items.Add(string.Format("Danh sách id: {0}", luuIdGhe));

                this.Hide();
                HoaDon hd = new HoaDon();      
                hd.ShowDialog();
            }
        }

        private void Ve_Load(object sender, EventArgs e)
        {         
            lbxDetail.Items.Add(string.Format("Tên phim: {0}", tenPhim));

            List<string> gheDaDat = new List<string>();

            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();

            string sql = "select tenGhe from datghe, ghe, phim" +
                         " where datghe.idGhe = ghe.idGhe and datghe.idPhim = phim.maPhim and datghe.idPhim = ?datghe.idPhim";

            MySqlCommand cmd = new MySqlCommand(sql, conn);                 

            try
            {
                cmd.Parameters.AddWithValue("?datghe.idPhim", idPhim);
                MySqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    gheDaDat.Add(reader.GetString(0));
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
                
            //kiểm tra ghế đã được đặt hay chưa
            //nếu ghế đã được đặt thì đổi màu đỏ và disable chekedbox
            foreach (Control cc in gbxGhe.Controls)
            {
                if (gheDaDat.IndexOf(cc.Name) != -1)
                {
                    cc.BackColor = Color.Red;
                    cc.Enabled = false;
                }
            }
        }
      
        private void A1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;         

            if (cb.Checked)
            {
                cb.BackColor = Color.Red;
                lbxDetail.Items.Add(string.Format("Bạn vừa đặt ghế: " + cb.Name));
            }
            else
            {
                cb.BackColor = Color.Silver;
                lbxDetail.Items.Add(string.Format("Bạn vừa hủy đặt ghế: " + cb.Name));
            }            
        }

        private void CbThucAn_CheckedChanged(object sender, EventArgs e)
        {
            lbxDetail.Items.Clear(); //reset items trong listbox
            
            FormDichVu dichVu = new FormDichVu();
            dichVu.ShowDialog();

            lbxDetail.Items.Add(string.Format("Tên phim: {0}", tenPhim));
            lbxDetail.Items.Add("Danh sách dịch vụ đã chọn");

            //if (!string.IsNullOrEmpty(dataPhim))
            //{
            //    lbxDetail.Items.Add(dataPhim);
            //}

            //if (!string.IsNullOrEmpty(dataBap))
            //{
            //    lbxDetail.Items.Add(dataBap);
            //}

            foreach(FormDichVu.DichVu dv in DanhSachDV)
            {
                int soSanh = 0;
                if (!(soSanh == int.Parse(dv.SoLuong)))
                {
                    String data = dv.TenDichVu + "   " +
                                  "   Giá bán: " + dv.GiaBan + ";" +
                                  "   Số lượng: " + dv.SoLuong +
                                  "   Thành tiền: " + ((int.Parse(dv.GiaBan) * int.Parse(dv.SoLuong)).ToString());  
                    lbxDetail.Items.Add(data);
                }
            }
        }

        public int ReturnIdGhe(Control cc)
        {
            int idGhe = 0;
            CheckBox a = ((CheckBox)cc);        

            if (a.Name == "A1")
                idGhe = 1;
            if (a.Name == "A2")
                idGhe = 2;
            if (a.Name == "A3")
                idGhe = 3;
            if (a.Name == "A4")
                idGhe = 4;

            if (a.Name == "B1")
                idGhe = 5;
            if (a.Name == "B2")
                idGhe = 6;
            if (a.Name == "B3")
                idGhe = 7;
            if (a.Name == "B4")
                idGhe = 8;

            if (a.Name == "C1")
                idGhe = 9;
            if (a.Name == "C2")
                idGhe = 10;
            if (a.Name == "C3")
                idGhe = 11;
            if (a.Name == "C4")
                idGhe = 12;

            if (a.Name == "D1")
                idGhe = 13;
            if (a.Name == "D2")
                idGhe = 14;
            if (a.Name == "D3")
                idGhe = 15;
            if (a.Name == "D4")
                idGhe = 16;

            return idGhe;
        }

        public static string tenPhim = string.Empty;
        public static double tongHoaDon = 0;
        public static string idPhim = string.Empty;
        public static string idKhachHang = string.Empty;     
    } 
}
