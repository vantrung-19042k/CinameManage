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
    public partial class QuanLyDichVu : Form
    {
        public QuanLyDichVu()
        {
            InitializeComponent();
        }

        public static string file;

        private void btThem_Click(object sender, EventArgs e)
        {
            if (!(txtMaDV.Text == "" || txtTenDV.Text == "" ||
                txtGiaBan.Text == "" || txtKhuyenMai.Text == "" || pictureDV.Image == null))
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();

                //kiểm tra đầu vào mã dịch vụ, nếu trùng => không cho nhập
                string sql_ktMaDV = "select maDichVu from dichvu";

                MySqlCommand cmd = new MySqlCommand(sql_ktMaDV, conn);
                try
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (txtMaDV.Text.Equals(reader.GetString(0)))
                        {
                            MessageBox.Show("Mã dịch vụ không được trùng");
                            txtMaDV.Text = txtTenDV.Text = txtGiaBan.Text = txtKhuyenMai.Text = "";
                            return;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Dua du lieu vao listview
                ListViewItem maDV = new ListViewItem(txtMaDV.Text);

                maDV.SubItems.Add(txtMaDV.Text);
                maDV.SubItems.Add(txtTenDV.Text);
                maDV.SubItems.Add(txtGiaBan.Text);
                maDV.SubItems.Add(txtKhuyenMai.Text);              

                lvThongTinDV.Items.Add(maDV);

                // Dua du lieu xuong data base                
                string sql = "insert into dichvu(maDichVu, tenDichVu, giaBan, khuyenMai, hinhAnh)" +
                                        "value(?maDichVu, ?tenDichVu, ?giaBan, ?khuyenMai, ?hinhAnh)";
                cmd = new MySqlCommand(sql, conn);

                try
                {
                    cmd.Parameters.AddWithValue("?maDichVu", txtMaDV.Text);
                    cmd.Parameters.AddWithValue("?tenDichVu", txtTenDV.Text);
                    cmd.Parameters.AddWithValue("?giaBan", txtGiaBan.Text);
                    cmd.Parameters.AddWithValue("?khuyenMai", txtKhuyenMai.Text);
                    cmd.Parameters.AddWithValue("?hinhAnh", Convert.ToBase64String(converImgToByte()));

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
            }
            else
            {
                btThem.Enabled = false;
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin và hình ảnh", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btThem.Enabled = true;
            }
            txtMaDV.Text = txtTenDV.Text = txtGiaBan.Text = txtKhuyenMai.Text = "";
            pictureDV.Image = null;
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            string chon = txtMaDV.Text;
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
                    // Sua tren listview
                    lvThongTinDV.SelectedItems[0].SubItems[0].Text = txtMaDV.Text;
                    lvThongTinDV.SelectedItems[0].SubItems[1].Text = txtTenDV.Text;
                    lvThongTinDV.SelectedItems[0].SubItems[2].Text = txtGiaBan.Text;
                    lvThongTinDV.SelectedItems[0].SubItems[3].Text = txtKhuyenMai.Text;                   

                    // Sua tren data base
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    string sql = "update dichvu set tenDichVu = ?tenDichVu, giaBan = ?giaBan, khuyenMai = ?khuyenMai" +
                                " where maDichVu = ?maDichVu";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    try
                    {
                        cmd.Parameters.AddWithValue("?maDichVu", txtMaDV.Text);
                        cmd.Parameters.AddWithValue("?tenDichVu", txtTenDV.Text);                        
                        cmd.Parameters.AddWithValue("?giaBan", txtGiaBan.Text);
                        cmd.Parameters.AddWithValue("?khuyenMai", txtKhuyenMai.Text);

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

        private void lvThongTinDV_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvThongTinDV.SelectedItems)
            {
                txtMaDV.Text = item.SubItems[0].Text;
                txtTenDV.Text = item.SubItems[1].Text;
                txtGiaBan.Text = item.SubItems[2].Text;
                txtKhuyenMai.Text = item.SubItems[3].Text;
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            string chon = txtMaDV.Text;
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
                    string sql = "delete from dichvu where maDichVu = ?maDichVu";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    if (chon.Length == 0)
                    {
                        MessageBox.Show("Chưa chọn dữ liệu xóa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    foreach (ListViewItem item in lvThongTinDV.SelectedItems)
                    {
                        // Xoa du lieu tren data base
                        try
                        {
                            cmd.Parameters.AddWithValue("?maDichVu", item.Text);
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
                        lvThongTinDV.Items.Remove(item);
                        MessageBox.Show("Đã xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            txtMaDV.Text = txtTenDV.Text = txtGiaBan.Text = txtKhuyenMai.Text = "";
        }

        private void QuanLyDichVu_Load(object sender, EventArgs e)
        {
            txtMaDV.Select();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            string sql = "select * from dichvu";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem itemMaDV = new ListViewItem();
                    itemMaDV.Text = reader.GetString(0);

                    ListViewItem.ListViewSubItem itemTenDV = new ListViewItem.ListViewSubItem();
                    itemTenDV.Text = reader.GetString(1);

                    ListViewItem.ListViewSubItem itemGiaBan = new ListViewItem.ListViewSubItem();
                    itemGiaBan.Text = reader.GetString(2);

                    ListViewItem.ListViewSubItem itemKhuyenMai = new ListViewItem.ListViewSubItem();
                    itemKhuyenMai.Text = reader.GetString(3);

                    itemMaDV.SubItems.AddRange(new ListViewItem.ListViewSubItem[]
                                {itemTenDV, itemGiaBan, itemKhuyenMai });

                    lvThongTinDV.Items.Add(itemMaDV);
                }
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

        //tải ảnh lên
        private void openPic_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();
            file = openFile.FileName;
            Image myImage = Image.FromFile(file);
            pictureDV.Image = myImage;
        }
    }
}
