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
    public partial class QuanLyNhanVien : Form
    {
        public QuanLyNhanVien()
        {
            InitializeComponent();
        }

        private void btThem_Click(object sender, EventArgs e)
        {            
            if (!(txtMaNV.Text == "" || txtHoTen.Text == "" || txtGioiTinh.Text == "" || 
                txtCMND.Text == "" || txtSDT.Text == "" || txtChucVu.Text == ""))
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();

                //kiểm tra đầu vào mã nhân viên, nếu trùng => không cho nhập
                string sql_ktMaNV = "select maNV from nhanvien";

                MySqlCommand cmd = new MySqlCommand(sql_ktMaNV, conn);
                try
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (txtMaNV.Text.Equals(reader.GetString(0)))
                        {
                            MessageBox.Show("Mã nhân viên không được trùng");
                            txtMaNV.Text = txtHoTen.Text = txtGioiTinh.Text = txtCMND.Text = txtSDT.Text = txtChucVu.Text = "";
                            return;
                        }
                    }
                    reader.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                // Đưa dữ liệu vào listview
                ListViewItem maNV = new ListViewItem(txtMaNV.Text);

                maNV.SubItems.Add(txtHoTen.Text);
                maNV.SubItems.Add(string.Format(dtpNgaySinh.Value.Day +
                                "-" + dtpNgaySinh.Value.Month + "-" + dtpNgaySinh.Value.Year));
                maNV.SubItems.Add(txtGioiTinh.Text);
                maNV.SubItems.Add(txtCMND.Text);
                maNV.SubItems.Add(txtSDT.Text);
                maNV.SubItems.Add(txtChucVu.Text);
                maNV.SubItems.Add(txtTaiKhoan.Text);
                maNV.SubItems.Add(txtMatKhau.Text);

                lsvNhanVien.Items.Add(maNV);
                

                //Đưa dữ liệu xuống database               
                string sql = "insert into nhanvien(maNV, hoTen, ngaySinh, gioiTinh, CMND, SDT, chucVu, taiKhoan, matKhau)" +
                                        "value(?maNV, ?hoTen, ?ngaySinh, ?gioiTinh, ?CMND, ?SDT, ?chucVu, ?taiKhoan, ?matKhau)";
                cmd = new MySqlCommand(sql, conn);

                try
                {
                    cmd.Parameters.AddWithValue("?maNV", txtMaNV.Text);
                    cmd.Parameters.AddWithValue("?hoTen", txtHoTen.Text);
                    cmd.Parameters.AddWithValue("?ngaySinh",
                        string.Format(dtpNgaySinh.Value.Day + "-" + dtpNgaySinh.Value.Month + "-" + dtpNgaySinh.Value.Year));
                    cmd.Parameters.AddWithValue("?gioiTinh", txtGioiTinh.Text);
                    cmd.Parameters.AddWithValue("?CMND", txtCMND.Text);
                    cmd.Parameters.AddWithValue("?SDT", txtSDT.Text);
                    cmd.Parameters.AddWithValue("?chucVu", txtChucVu.Text);
                    cmd.Parameters.AddWithValue("?taiKhoan", txtTaiKhoan.Text);
                    cmd.Parameters.AddWithValue("?matKhau", txtMatKhau.Text);

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
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btThem.Enabled = true;
            }

            txtMaNV.Text = txtHoTen.Text = txtGioiTinh.Text = txtCMND.Text = txtSDT.Text = txtChucVu.Text = "";
        }

        //
        private void lsvNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lsvNhanVien.SelectedItems)
            {
                txtMaNV.Text = item.SubItems[0].Text;
                txtHoTen.Text = item.SubItems[1].Text;
                dtpNgaySinh.Text = item.SubItems[2].Text;
                txtGioiTinh.Text = item.SubItems[3].Text;
                txtCMND.Text = item.SubItems[4].Text;
                txtSDT.Text = item.SubItems[5].Text;
                txtChucVu.Text = item.SubItems[6].Text;
                txtTaiKhoan.Text = item.SubItems[7].Text;
                txtMatKhau.Text = item.SubItems[8].Text;
            }
        }

        //
        private void btSua_Click(object sender, EventArgs e)
        {
            string chon = txtMaNV.Text;
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
                    lsvNhanVien.SelectedItems[0].SubItems[0].Text = txtMaNV.Text;
                    lsvNhanVien.SelectedItems[0].SubItems[1].Text = txtHoTen.Text;
                    lsvNhanVien.SelectedItems[0].SubItems[2].Text = string.Format(
                        dtpNgaySinh.Value.Day + "-" + dtpNgaySinh.Value.Month + "-" + dtpNgaySinh.Value.Year);
                    lsvNhanVien.SelectedItems[0].SubItems[3].Text = txtGioiTinh.Text;
                    lsvNhanVien.SelectedItems[0].SubItems[4].Text = txtCMND.Text;
                    lsvNhanVien.SelectedItems[0].SubItems[5].Text = txtSDT.Text;
                    lsvNhanVien.SelectedItems[0].SubItems[6].Text = txtChucVu.Text;

                    // Sửa ở database
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    string sql = "update nhanvien set hoTen = ?hoTen, ngaySinh = ?ngaySinh, gioiTinh = ?gioiTinh," +
                                " CMND = ?CMND, SDT = ?SDT, chucVu = ?chucVu where maNV = ?maNV";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    try
                    {
                        cmd.Parameters.AddWithValue("?maNV", txtMaNV.Text);
                        cmd.Parameters.AddWithValue("?hoTen", txtHoTen.Text);
                        cmd.Parameters.AddWithValue("?ngaySinh",
                            string.Format(dtpNgaySinh.Value.Day + "-" + dtpNgaySinh.Value.Month + "-" + dtpNgaySinh.Value.Year));
                        cmd.Parameters.AddWithValue("?gioiTinh", txtGioiTinh.Text);
                        cmd.Parameters.AddWithValue("?CMND", txtCMND.Text);
                        cmd.Parameters.AddWithValue("?SDT", txtSDT.Text);
                        cmd.Parameters.AddWithValue("?chucVu", txtChucVu.Text);

                        cmd.ExecuteReader();
                        MessageBox.Show("Sửa thành công");
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
            string chon = txtMaNV.Text;
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
                    string sql = "delete from nhanvien where maNV = ?maNV";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    if (chon.Length == 0)
                    {
                        MessageBox.Show("Chưa chọn dữ liệu xóa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    foreach (ListViewItem item in lsvNhanVien.SelectedItems)
                    {
                        // Xóa dữ liệu trên database
                        try
                        {
                            cmd.Parameters.AddWithValue("?maNV", item.Text);
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

                        // Xóa dữ liệu trên listview
                        lsvNhanVien.Items.Remove(item);
                        MessageBox.Show("Đã xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }         
            txtMaNV.Text = txtHoTen.Text = txtGioiTinh.Text = txtCMND.Text = txtSDT.Text = txtChucVu.Text = "";
        }

        //
        private void QuanLyNhanVien_Load(object sender, EventArgs e)
        {
            txtMaNV.Select();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            string sql = "select * from nhanvien";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    ListViewItem itemMaNV = new ListViewItem();
                    itemMaNV.Text = reader.GetString(0);

                    ListViewItem.ListViewSubItem itemHoTen = new ListViewItem.ListViewSubItem();
                    itemHoTen.Text = reader.GetString(1);

                    ListViewItem.ListViewSubItem itemNgaySinh = new ListViewItem.ListViewSubItem();
                    itemNgaySinh.Text = reader.GetString(2);

                    ListViewItem.ListViewSubItem itemGioiTinh = new ListViewItem.ListViewSubItem();
                    itemGioiTinh.Text = reader.GetString(3);

                    ListViewItem.ListViewSubItem itemCMND = new ListViewItem.ListViewSubItem();
                    itemCMND.Text = reader.GetString(4);

                    ListViewItem.ListViewSubItem itemSDT = new ListViewItem.ListViewSubItem();
                    itemSDT.Text = reader.GetString(5);

                    ListViewItem.ListViewSubItem itemChucVu = new ListViewItem.ListViewSubItem();
                    itemChucVu.Text = reader.GetString(6);

                    ListViewItem.ListViewSubItem itemTaiKhoan = new ListViewItem.ListViewSubItem();
                    itemTaiKhoan.Text = reader.GetString(7);

                    ListViewItem.ListViewSubItem itemMatKhau = new ListViewItem.ListViewSubItem();
                    itemMatKhau.Text = reader.GetString(8);

                    itemMaNV.SubItems.AddRange(new ListViewItem.ListViewSubItem[] 
                                { itemHoTen, itemNgaySinh, itemGioiTinh, itemCMND, itemSDT, itemChucVu, itemTaiKhoan, itemMatKhau });

                    lsvNhanVien.Items.Add(itemMaNV);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {                
                conn.Close();
            }
        }

        //đóng form
        private void QuanLyNhanVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult thoat = MessageBox.Show("Bạn có chắc chắn thoát", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (thoat == DialogResult.No)
                e.Cancel = true;
        }

        //không cho nhập CMND có ký tự chữ
        private void txtCMND_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        //không cho nhập họ tên có ký tự số
        private void txtHoTen_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        //Các sự kiện nhập bàn phím
        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape: this.Close(); return true;
            }

            return base.ProcessDialogKey(keyData);
        }      
    }
}
