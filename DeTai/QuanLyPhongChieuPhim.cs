using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeTai
{
    public partial class QuanLyPhongChieuPhim : Form
    {
        public QuanLyPhongChieuPhim()
        {
            InitializeComponent();
        }

        //
        private void btThem_Click(object sender, EventArgs e)
        {
            if (!(txtMaPhong.Text.Equals("") || txtTenPhong.Text.Equals("") || cbLoaiPhong.Text.Equals("") ||
                txtSoCho.Text.Equals("") || txtTinhTrang.Text.Equals("") || txtDienTich.Text.Equals("")))
            {
                MySqlConnection conn = DBUtils.GetDBConnection();
                conn.Open();

                string sql_ktMaPhong = "select maPhong from phongchieu";

                MySqlCommand cmd = new MySqlCommand(sql_ktMaPhong, conn);

                try
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (txtMaPhong.Text.Equals(reader.GetString(0)))
                        {
                            MessageBox.Show("Mã phòng không được trùng");
                            txtMaPhong.Text = txtTenPhong.Text = cbLoaiPhong.Text = txtSoCho.Text = txtTinhTrang.Text = txtDienTich.Text = "";
                            return;
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //đưa dữ liệu vào listview
                ListViewItem maPhong = new ListViewItem(txtMaPhong.Text);

                maPhong.SubItems.Add(txtTenPhong.Text);
                maPhong.SubItems.Add(cbLoaiPhong.Text);
                maPhong.SubItems.Add(txtSoCho.Text);
                maPhong.SubItems.Add(txtTinhTrang.Text);
                maPhong.SubItems.Add(txtDienTich.Text);

                lvPhongChieu.Items.Add(maPhong);

                //đưa dữ liệu xuống database
                string sql = "insert into phongchieu(maPhong, tenPhong, loaiPhong, soCho, tinhTrang, dienTich) " +
                    "value(?maPhong, ?tenPhong, ?loaiPhong, ?soCho, ?tinhTrang, ?dienTich)";
                cmd = new MySqlCommand(sql, conn);

                try
                {
                    cmd.Parameters.AddWithValue("?maPhong", txtMaPhong.Text);
                    cmd.Parameters.AddWithValue("?tenPhong", txtTenPhong.Text);
                    cmd.Parameters.AddWithValue("?loaiPhong", cbLoaiPhong.Text);
                    cmd.Parameters.AddWithValue("?soCho", txtSoCho.Text);
                    cmd.Parameters.AddWithValue("?tinhTrang", txtTinhTrang.Text);
                    cmd.Parameters.AddWithValue("?dienTich", txtDienTich.Text);

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
            txtMaPhong.Text = txtTenPhong.Text = cbLoaiPhong.Text = txtSoCho.Text = txtTinhTrang.Text = txtDienTich.Text = "";
        }

        //
        private void lvPhongChieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(ListViewItem item in lvPhongChieu.SelectedItems)
            {
                txtMaPhong.Text = item.SubItems[0].Text;
                txtTenPhong.Text = item.SubItems[1].Text;
                cbLoaiPhong.Text = item.SubItems[2].Text;
                txtSoCho.Text = item.SubItems[3].Text;
                txtTinhTrang.Text = item.SubItems[4].Text;
                txtDienTich.Text = item.SubItems[5].Text;
            }
        }

        //
        private void btSua_Click(object sender, EventArgs e)
        {
            string chon = txtMaPhong.Text;
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
                    //sửa trên listview
                    lvPhongChieu.SelectedItems[0].SubItems[1].Text = txtTenPhong.Text;
                    lvPhongChieu.SelectedItems[0].SubItems[2].Text = cbLoaiPhong.Text;
                    lvPhongChieu.SelectedItems[0].SubItems[3].Text = txtSoCho.Text;
                    lvPhongChieu.SelectedItems[0].SubItems[4].Text = txtTinhTrang.Text;
                    lvPhongChieu.SelectedItems[0].SubItems[5].Text = txtDienTich.Text;

                    //sua tren database
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    string sql = "update phongchieu set tenPhong=?tenPhong, loaiPhong=?loaiPhong, soCho=?soCho," +
                                " tinhTrang=?tinhTrang, dienTich=?dienTich" +
                                " where maPhong = ?maPhong";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    try
                    {
                        cmd.Parameters.AddWithValue("?tenPhong", txtTenPhong.Text);
                        cmd.Parameters.AddWithValue("?loaiPhong", cbLoaiPhong.Text);
                        cmd.Parameters.AddWithValue("?soCho", txtSoCho.Text);
                        cmd.Parameters.AddWithValue("?tinhTrang", txtTinhTrang.Text);
                        cmd.Parameters.AddWithValue("?dienTich", txtDienTich.Text);
                        cmd.Parameters.AddWithValue("?maPhong", txtMaPhong.Text);

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
                    txtMaPhong.Text = txtTenPhong.Text = cbLoaiPhong.Text = txtSoCho.Text = txtTinhTrang.Text = txtDienTich.Text = "";
                }
            }
        }

        //
        private void QuanLyPhongChieuPhim_Load(object sender, EventArgs e)
        {
            txtMaPhong.Select();
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            string sql = "select * from phongchieu";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem itemMaPhong = new ListViewItem();
                    itemMaPhong.Text = reader.GetString(0);

                    ListViewItem.ListViewSubItem itemSubTenPhong = new ListViewItem.ListViewSubItem();
                    itemSubTenPhong.Text = reader.GetString(1);

                    ListViewItem.ListViewSubItem itemSubLoaiPhong = new ListViewItem.ListViewSubItem();
                    itemSubLoaiPhong.Text = reader.GetString(2);

                    ListViewItem.ListViewSubItem itemSubSoCho = new ListViewItem.ListViewSubItem();
                    itemSubSoCho.Text = reader.GetString(3);

                    ListViewItem.ListViewSubItem itemSubTinhTrang = new ListViewItem.ListViewSubItem();
                    itemSubTinhTrang.Text = reader.GetString(4);

                    ListViewItem.ListViewSubItem itemSubDienTich = new ListViewItem.ListViewSubItem();
                    itemSubDienTich.Text = reader.GetString(5);

                    itemMaPhong.SubItems.AddRange(new ListViewItem.ListViewSubItem[]
                                { itemSubTenPhong, itemSubLoaiPhong, itemSubSoCho, itemSubTinhTrang, itemSubDienTich});

                    lvPhongChieu.Items.Add(itemMaPhong);
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
        private void btXoa_Click(object sender, EventArgs e)
        {
            string chon = txtMaPhong.Text;
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
                    string sql = "delete from phongchieu where maPhong = ?maPhong";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    if (chon.Length == 0)
                    {
                        MessageBox.Show("Chưa chọn dữ liệu xóa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    foreach (ListViewItem item in lvPhongChieu.SelectedItems)
                    {
                        // Xóa dữ liệu ở database
                        try
                        {
                            cmd.Parameters.AddWithValue("?maPhong", item.Text);
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

                        // Xoá dữ liệu ở listview
                        lvPhongChieu.Items.Remove(item);
                        MessageBox.Show("Đã xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            txtMaPhong.Text = txtTenPhong.Text = cbLoaiPhong.Text = txtSoCho.Text = txtTinhTrang.Text = txtDienTich.Text = "";
        }

        //Đóng form
        private void QuanLyPhongChieuPhim_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult thoat = MessageBox.Show("Bạn có chắc chắn thoát", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (thoat == DialogResult.No)
                e.Cancel = true;
        }

        //Kiểm tra đầu vào

    }
}
