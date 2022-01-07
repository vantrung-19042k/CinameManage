using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DeTai
{
    public partial class QuanLyThanhVien : Form
    {
        public QuanLyThanhVien()
        {
            InitializeComponent();
        }

        //
        private void QuanLyThanhVien_Load(object sender, EventArgs e)
        {
            txtMaTV.Select();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            string sql = "select * from thanhvien";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem itemMaTV = new ListViewItem();
                    itemMaTV.Text = reader.GetString(0);

                    ListViewItem.ListViewSubItem itemTenNV = new ListViewItem.ListViewSubItem();
                    itemTenNV.Text = reader.GetString(1);

                    ListViewItem.ListViewSubItem itemGioiTinh = new ListViewItem.ListViewSubItem();
                    itemGioiTinh.Text = reader.GetString(2);

                    ListViewItem.ListViewSubItem itemNgaySinh = new ListViewItem.ListViewSubItem();
                    itemNgaySinh.Text = reader.GetString(3);

                    ListViewItem.ListViewSubItem itemSDT = new ListViewItem.ListViewSubItem();
                    itemSDT.Text = reader.GetString(4);

                    ListViewItem.ListViewSubItem itemNgayDangKi = new ListViewItem.ListViewSubItem();
                    itemNgayDangKi.Text = reader.GetString(5);

                    ListViewItem.ListViewSubItem itemLoaiThe = new ListViewItem.ListViewSubItem();
                    itemLoaiThe.Text = reader.GetString(6);

                    ListViewItem.ListViewSubItem itemDiemTich = new ListViewItem.ListViewSubItem();
                    itemDiemTich.Text = reader.GetString(7);

                    ListViewItem.ListViewSubItem itemTaiKhoan = new ListViewItem.ListViewSubItem();
                    itemTaiKhoan.Text = reader.GetString(8);

                    ListViewItem.ListViewSubItem itemMatKhau = new ListViewItem.ListViewSubItem();
                    itemMatKhau.Text = reader.GetString(9);

                    itemMaTV.SubItems.AddRange(new ListViewItem.ListViewSubItem[]
                                { itemTenNV, itemGioiTinh, itemNgaySinh, itemSDT,
                                    itemNgayDangKi, itemLoaiThe, itemDiemTich, itemTaiKhoan, itemMatKhau});

                    lsvThanhVien.Items.Add(itemMaTV);
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

        //
        private void btThem_Click(object sender, EventArgs e)
        {
            if (!(txtMaTV.Text == "" || txtTenTV.Text == "" || cbGioiTinh.Text == "" ||
                                            txtSDT.Text == "" || txtLoaiThe.Text == "" || txtDiemTich.Text == ""))
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();

                string sql_ktMaTV = "select maTV from thanhvien";

                MySqlCommand cmd = new MySqlCommand(sql_ktMaTV, conn);
                try
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (txtMaTV.Text.Equals(reader.GetString(0)))
                        {
                            MessageBox.Show("Mã nhân viên không được trùng");
                            txtMaTV.Text = txtTenTV.Text = cbGioiTinh.Text = txtSDT.Text = txtLoaiThe.Text = txtDiemTich.Text = "";
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
                ListViewItem maTV = new ListViewItem(txtMaTV.Text);

                maTV.SubItems.Add(txtTenTV.Text);
                maTV.SubItems.Add(cbGioiTinh.Text);
                maTV.SubItems.Add(string.Format(dtpNgaySinh.Value.Day +
                                "-" + dtpNgaySinh.Value.Month + "-" + dtpNgaySinh.Value.Year));
                maTV.SubItems.Add(txtSDT.Text);
                maTV.SubItems.Add(string.Format(dtpNgayDangKi.Value.Day +
                                "-" + dtpNgayDangKi.Value.Month + "-" + dtpNgayDangKi.Value.Year));
                maTV.SubItems.Add(txtLoaiThe.Text);
                maTV.SubItems.Add(txtDiemTich.Text);
                maTV.SubItems.Add(txtTaiKhoan.Text);
                maTV.SubItems.Add(txtMatKhau.Text);

                lsvThanhVien.Items.Add(maTV);

                // Dua du lieu xuong data base                
                string sql = "insert into thanhvien(maTV, tenTV, gioiTinh, ngaySinh, SDT, ngayDangKi, loaiThe, diemTich, taiKhoan, matKhau)" +
                                        "value(?maTV, ?tenTV, ?gioiTinh, ?ngaySinh, ?SDT, ?ngayDangKi, ?loaiThe, ?diemTich, ?taiKhoan, ?matKhau)";
                cmd = new MySqlCommand(sql, conn);

                try
                {
                    cmd.Parameters.AddWithValue("?maTV", txtMaTV.Text);
                    cmd.Parameters.AddWithValue("?tenTV", txtTenTV.Text);
                    cmd.Parameters.AddWithValue("?gioiTinh", cbGioiTinh.Text);
                    cmd.Parameters.AddWithValue("?ngaySinh",
                        string.Format(dtpNgaySinh.Value.Day + "-" + dtpNgaySinh.Value.Month + "-" + dtpNgaySinh.Value.Year));
                    cmd.Parameters.AddWithValue("?SDT", txtSDT.Text);
                    cmd.Parameters.AddWithValue("?ngayDangKi",
                        string.Format(dtpNgayDangKi.Value.Day + "-" + dtpNgayDangKi.Value.Month + "-" + dtpNgayDangKi.Value.Year));
                    cmd.Parameters.AddWithValue("?loaiThe", txtLoaiThe.Text);
                    cmd.Parameters.AddWithValue("?diemTich", txtDiemTich.Text);
                    cmd.Parameters.AddWithValue("?taiKhoan", txtTaiKhoan.Text);
                    cmd.Parameters.AddWithValue("?matKhau", txtMatKhau.Text);

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
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btThem.Enabled = true;
            }
            txtMaTV.Text = txtTenTV.Text = cbGioiTinh.Text = txtLoaiThe.Text = txtSDT.Text = txtDiemTich.Text = "";
        }

        //
        private void lsvThanhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lsvThanhVien.SelectedItems)
            {
                txtMaTV.Text = item.SubItems[0].Text;
                txtTenTV.Text = item.SubItems[1].Text;
                cbGioiTinh.Text = item.SubItems[2].Text;
                dtpNgaySinh.Text = item.SubItems[3].Text;
                txtSDT.Text = item.SubItems[4].Text;
                dtpNgayDangKi.Text = item.SubItems[5].Text;
                txtLoaiThe.Text = item.SubItems[6].Text;
                txtDiemTich.Text = item.SubItems[7].Text;
                txtTaiKhoan.Text = item.SubItems[8].Text;
                txtMatKhau.Text = item.SubItems[9].Text;
            }
        }

        //
        private void btSua_Click(object sender, EventArgs e)
        {
            string chon = txtMaTV.Text;
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
                    lsvThanhVien.SelectedItems[0].SubItems[0].Text = txtMaTV.Text;
                    lsvThanhVien.SelectedItems[0].SubItems[1].Text = txtTenTV.Text;
                    lsvThanhVien.SelectedItems[0].SubItems[2].Text = cbGioiTinh.Text;
                    lsvThanhVien.SelectedItems[0].SubItems[3].Text = string.Format(
                        dtpNgaySinh.Value.Day + "-" + dtpNgaySinh.Value.Month + "-" + dtpNgaySinh.Value.Year);
                    lsvThanhVien.SelectedItems[0].SubItems[4].Text = txtSDT.Text;
                    lsvThanhVien.SelectedItems[0].SubItems[5].Text = string.Format(
                        dtpNgayDangKi.Value.Day + "-" + dtpNgayDangKi.Value.Month + "-" + dtpNgayDangKi.Value.Year);
                    lsvThanhVien.SelectedItems[0].SubItems[6].Text = txtLoaiThe.Text;
                    lsvThanhVien.SelectedItems[0].SubItems[7].Text = txtDiemTich.Text;

                    // Sua tren data base
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    string sql = "update thanhvien set tenTV = ?tenTV, gioiTinh = ?gioiTinh, ngaySinh = ?ngaySinh, SDT = ?SDT, " +
                                                "ngayDangKi = ?ngayDangKi, loaiThe = ?loaiThe, diemTich = ?diemTich where maTV = ?maTV";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    try
                    {
                        cmd.Parameters.AddWithValue("?maTV", txtMaTV.Text);
                        cmd.Parameters.AddWithValue("?tenTV", txtTenTV.Text);
                        cmd.Parameters.AddWithValue("?gioiTinh", cbGioiTinh.Text);
                        cmd.Parameters.AddWithValue("?ngaySinh",
                            string.Format(dtpNgaySinh.Value.Day + "-" + dtpNgaySinh.Value.Month + "-" + dtpNgaySinh.Value.Year));
                        cmd.Parameters.AddWithValue("?SDT", txtSDT.Text);
                        cmd.Parameters.AddWithValue("?ngayDangKi",
                            string.Format(dtpNgayDangKi.Value.Day + "-" + dtpNgayDangKi.Value.Month + "-" + dtpNgayDangKi.Value.Year));
                        cmd.Parameters.AddWithValue("?loaiThe", txtLoaiThe.Text);
                        cmd.Parameters.AddWithValue("?diemTich", txtDiemTich.Text);

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
            string chon = txtMaTV.Text;
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
                    string sql = "delete from thanhvien where maTV = ?maTV";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    if (chon.Length == 0)
                    {
                        MessageBox.Show("Chưa chọn dữ liệu xóa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    foreach (ListViewItem item in lsvThanhVien.SelectedItems)
                    {
                        // Xoa du lieu tren data base
                        try
                        {
                            cmd.Parameters.AddWithValue("?maTV", item.Text);
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
                        lsvThanhVien.Items.Remove(item);
                        MessageBox.Show("Đã xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            txtMaTV.Text = txtTenTV.Text = cbGioiTinh.Text = txtLoaiThe.Text = txtSDT.Text = txtDiemTich.Text = "";

        }

        //Kiểm soát dữ liệu đầu vào
        //Không cho nhập SĐT là chữ
        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        //Không cho nhập tên là số
        private void txtTenTV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        //Đóng form
        private void QuanLyThanhVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult thoat = MessageBox.Show("Bạn có chắc chắn thoát", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (thoat == DialogResult.No)
                e.Cancel = true;
        }
    }
}
