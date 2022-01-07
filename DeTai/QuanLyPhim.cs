using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeTai
{
    public partial class QuanLyPhim : Form
    {
        public QuanLyPhim()
        {
            InitializeComponent();
        }

        //
        private void btThem_Click(object sender, EventArgs e)
        {
            if (!(txtMaPhim.Text.Equals("") || txtTenPhim.Text.Equals("") || cbTheLoai.Text.Equals("")
                || txtThoiLuong.Text.Equals("") || cbCaChieu.Text.Equals("")
                || cbPhongChieu.Text.Equals("") || txtDaoDien.Text.Equals("") || picturePhim.Image == null))
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();

                //kiểm tra đầu vào mã phim, nếu trùng => không cho nhập
                string sql_ktMaPhim = "select maPhim from phim";

                MySqlCommand cmd = new MySqlCommand(sql_ktMaPhim, conn);
                try
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (txtMaPhim.Text.Equals(reader.GetString(0)))
                        {
                            MessageBox.Show("Mã phim không được trùng");
                            txtMaPhim.Text = txtTenPhim.Text = cbTheLoai.Text = txtThoiLuong.Text
                                = cbCaChieu.Text = cbPhongChieu.Text = txtDaoDien.Text = "";
                            return;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //Đưa dữ liệu vào listview
                ListViewItem maPhim = new ListViewItem(txtMaPhim.Text);

                maPhim.SubItems.Add(txtTenPhim.Text);
                maPhim.SubItems.Add(cbTheLoai.Text);
                maPhim.SubItems.Add(txtThoiLuong.Text);
                maPhim.SubItems.Add(cbCaChieu.Text);
                maPhim.SubItems.Add(cbPhongChieu.Text);
                maPhim.SubItems.Add(string.Format(dateNgayChieu.Value.Day + "/"
                                    + dateNgayChieu.Value.Month + "/"
                                    + dateNgayChieu.Value.Year));
                maPhim.SubItems.Add(txtDaoDien.Text);
                

                lvDsPhim.Items.Add(maPhim);

                //Đưa dữ liệu vào database               
                string sql = "insert into phim(maPhim, tenPhim, theLoai, thoiLuong, caChieu, phongChieu, ngayChieu, daoDien, hinhAnh)" +
                                        "value(?maPhim, ?tenPhim, ?theLoai, ?thoiLuong, ?caChieu, ?phongChieu, ?ngayChieu, ?daoDien, ?hinhAnh)";
                cmd = new MySqlCommand(sql, conn);

                try
                {
                    cmd.Parameters.AddWithValue("?maPhim", txtMaPhim.Text);
                    cmd.Parameters.AddWithValue("?tenPhim", txtTenPhim.Text);
                    cmd.Parameters.AddWithValue("?theLoai", cbTheLoai.Text);
                    cmd.Parameters.AddWithValue("?thoiLuong", txtThoiLuong.Text);
                    cmd.Parameters.AddWithValue("?caChieu", cbCaChieu.Text);
                    cmd.Parameters.AddWithValue("?phongChieu", cbPhongChieu.Text);
                    cmd.Parameters.AddWithValue("?ngayChieu", string.Format(dateNgayChieu.Value.Day + "/"
                                                                + dateNgayChieu.Value.Month + "/"
                                                                + dateNgayChieu.Value.Year));
                    cmd.Parameters.AddWithValue("?daoDien", txtDaoDien.Text);
                    cmd.Parameters.AddWithValue("?hinhAnh", Convert.ToBase64String(converImgToByte())); //lưu hình ảnh dưới dạng chuỗi

                    cmd.ExecuteReader();
                    MessageBox.Show("Thêm thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                btThem.Enabled = false;
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin và thêm hình ảnh", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btThem.Enabled = true;
            }
            txtMaPhim.Text = txtTenPhim.Text = cbTheLoai.Text = txtThoiLuong.Text = cbCaChieu.Text = cbPhongChieu.Text = txtDaoDien.Text = "";
        }

        //
        private void QuanLyPhim_Load(object sender, EventArgs e)
        {
            txtMaPhim.Select();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();

            //lấy dánh sách phòng chiếu, gán cho combobox
            string sql_selectPCP = "select tenPhong from phongchieu";

            MySqlCommand cmdSelectPCP = new MySqlCommand(sql_selectPCP, conn);

            try
            {
                List<String> phongChieu = new List<string>();
                MySqlDataReader readerSelectPCP = cmdSelectPCP.ExecuteReader();
                while(readerSelectPCP.Read())
                {
                    phongChieu.Add(readerSelectPCP.GetString(0));
                }
                cbPhongChieu.DataSource = phongChieu;

                readerSelectPCP.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

            /*
             * lấy dữ liệu từ database và hiển thị trên listview
             */
            string sql = "select * from phim";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem itemMaPhim = new ListViewItem();
                    itemMaPhim.Text = reader.GetString(0);

                    ListViewItem.ListViewSubItem itemTenPhim = new ListViewItem.ListViewSubItem();
                    itemTenPhim.Text = reader.GetString(1);

                    ListViewItem.ListViewSubItem itemTheLoai = new ListViewItem.ListViewSubItem();
                    itemTheLoai.Text = reader.GetString(2);

                    ListViewItem.ListViewSubItem itemThoiLuong = new ListViewItem.ListViewSubItem();
                    itemThoiLuong.Text = reader.GetString(3);

                    ListViewItem.ListViewSubItem itemCaChieu = new ListViewItem.ListViewSubItem();
                    itemCaChieu.Text = reader.GetString(4);

                    ListViewItem.ListViewSubItem itemPhongChieu = new ListViewItem.ListViewSubItem();
                    itemPhongChieu.Text = reader.GetString(5);

                    ListViewItem.ListViewSubItem itemNgayChieu = new ListViewItem.ListViewSubItem();
                    itemNgayChieu.Text = reader.GetString(6);

                    ListViewItem.ListViewSubItem itemDaoDien = new ListViewItem.ListViewSubItem();
                    itemDaoDien.Text = reader.GetString(7);

                    itemMaPhim.SubItems.AddRange(new ListViewItem.ListViewSubItem[]
                                { itemTenPhim, itemTheLoai, itemThoiLuong, itemCaChieu, itemPhongChieu, itemNgayChieu, itemDaoDien });

                    lvDsPhim.Items.Add(itemMaPhim);                   
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        //
        private void lvDsPhim_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvDsPhim.SelectedItems)
            {
                txtMaPhim.Text = item.SubItems[0].Text;
                txtTenPhim.Text = item.SubItems[1].Text;
                cbTheLoai.Text = item.SubItems[2].Text;
                txtThoiLuong.Text = item.SubItems[3].Text;
                cbCaChieu.Text = item.SubItems[4].Text;
                cbPhongChieu.Text = item.SubItems[5].Text;
                dateNgayChieu.Text = item.SubItems[6].Text;
                txtDaoDien.Text = item.SubItems[7].Text;
            }
        }

        //
        private void btSua_Click(object sender, EventArgs e)
        {
            string chon = txtMaPhim.Text;
            if (chon.Length == 0)
            {
                MessageBox.Show("Chưa chọn dữ liệu sửa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DialogResult sua = MessageBox.Show("Bạn có chắc chắn sửa thông tin?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (sua == DialogResult.Yes)
                {
                    // Sửa ở listview
                    lvDsPhim.SelectedItems[0].SubItems[0].Text = txtMaPhim.Text;
                    lvDsPhim.SelectedItems[0].SubItems[1].Text = txtTenPhim.Text;
                    lvDsPhim.SelectedItems[0].SubItems[2].Text = cbTheLoai.Text;   
                    lvDsPhim.SelectedItems[0].SubItems[3].Text = txtThoiLuong.Text;
                    lvDsPhim.SelectedItems[0].SubItems[4].Text = cbCaChieu.Text;
                    lvDsPhim.SelectedItems[0].SubItems[5].Text = cbPhongChieu.Text;
                    lvDsPhim.SelectedItems[0].SubItems[6].Text = String.Format(dateNgayChieu.Value.Day +
                                                                 "/" + dateNgayChieu.Value.Month +
                                                                 "/" + dateNgayChieu.Value.Year);
                    lvDsPhim.SelectedItems[0].SubItems[7].Text = txtDaoDien.Text;

                    // Sua ở database
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    string sql = "update phim set maPhim = ?maPhim, tenPhim = ?tenPhim, theLoai = ?theLoai," +
                                " thoiLuong = ?thoiLuong, caChieu = ?caChieu, phongChieu = ?phongChieu, ngayChieu = ?ngayChieu, daoDien = ?daoDien" +
                                " where maPhim = ?maPhim";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    try
                    {
                        cmd.Parameters.AddWithValue("?maPhim", txtMaPhim.Text);
                        cmd.Parameters.AddWithValue("?tenPhim", txtTenPhim.Text);
                        cmd.Parameters.AddWithValue("?theLoai", cbTheLoai.Text);                      
                        cmd.Parameters.AddWithValue("?thoiLuong", txtThoiLuong.Text);
                        cmd.Parameters.AddWithValue("?caChieu", cbCaChieu.Text);
                        cmd.Parameters.AddWithValue("?phongChieu", cbPhongChieu.Text);
                        cmd.Parameters.AddWithValue("?ngayChieu", String.Format(dateNgayChieu.Value.Day +
                                                                 "/" + dateNgayChieu.Value.Month +
                                                                 "/" + dateNgayChieu.Value.Year));
                        cmd.Parameters.AddWithValue("?daoDien", txtDaoDien.Text);

                        cmd.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    MessageBox.Show("Đã sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //
        private void btXoa_Click(object sender, EventArgs e)
        {
            string chon = txtMaPhim.Text;
            if (chon.Length == 0)
            {
                MessageBox.Show("Chưa chọn dữ liệu xóa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DialogResult xoa = MessageBox.Show("Bạn có chắc chắn xóa?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (xoa == DialogResult.Yes)
                {
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    string sql = "delete from phim where maPhim = ?maPhim";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    if (chon.Length == 0)
                    {
                        MessageBox.Show("Chưa chọn dữ liệu xóa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    foreach (ListViewItem item in lvDsPhim.SelectedItems)
                    {
                        // Xoa du lieu tren data base
                        try
                        {
                            cmd.Parameters.AddWithValue("?maPhim", item.Text);
                            cmd.ExecuteReader();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            conn.Close();
                        }

                        // Xoa du lieu tren listview
                        lvDsPhim.Items.Remove(item);
                        MessageBox.Show("Đã xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            txtMaPhim.Text = txtTenPhim.Text = cbTheLoai.Text = txtThoiLuong.Text 
                = cbCaChieu.Text = cbPhongChieu.Text = txtDaoDien.Text = "";
        }

        //tải ảnh lên
        private void openPic_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();
            file = openFile.FileName;
            Image myImage = Image.FromFile(file);
            picturePhim.Image = myImage;
        }

        //chuyển ảnh thành file byte
        public byte[] converImgToByte()
        {
            FileStream fs;
            fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] picbyte = new byte[fs.Length];
            fs.Read(picbyte, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            return picbyte;
        }

        //chuyển chuỗi byte thành hình ảnh
        //public Image ByteToImg(string byteString)
        //{
        //    byte[] imgBytes = Convert.FromBase64String(byteString);
        //    MemoryStream ms = new MemoryStream(imgBytes, 0, imgBytes.Length);
        //    ms.Write(imgBytes, 0, imgBytes.Length);
        //    Image image = Image.FromStream(ms, true);
        //    return image;
        //}

        public static string file;
        //Kiểm tra đầu vào
    }
}
