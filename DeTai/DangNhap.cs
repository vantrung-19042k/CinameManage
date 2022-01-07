using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;

namespace DeTai
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }


        private void BtDangNhapEmployee_Click(object sender, EventArgs e)
        {
            LoginEmployee();
        }


        private void BtCustomer_Click_1(object sender, EventArgs e)
        {
            LoginCustomer();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch(keyData)
            {
                case Keys.Enter: LoginCustomer(); return true;
                case Keys.Space : LoginEmployee(); return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        public void LoginCustomer()
        {
            MySqlConnection conn = DBUtils.GetDBConnection();

            if (txtTenDangNhap.Text != "" && txtMatKhau.Text != "")
            {
                try
                {
                    string tk = txtTenDangNhap.Text;
                    string mk = txtMatKhau.Text;

                    String sql = "SELECT * FROM thanhvien " +
                    "where taiKhoan = '" + tk + "'" + "and matKhau=" + "'" + mk + "';";

                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = conn,
                        CommandText = sql
                    };

                    conn.Open();

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows == true)
                    {
                        MessageBox.Show("Đăng nhập thành công", "Tiếp tục", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        while (reader.Read())
                        {
                            DatVe.idKhachHang = reader.GetString(0);
                            this.Hide();
                            FormPhim dsPhim = new FormPhim();
                            dsPhim.ShowDialog();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Thông tin tài khoản hoặc mật khẩu không chính xác", "Tiếp tục", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }                   
                                     
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    //this.Close();
                }
            }
            else
            {
                MessageBox.Show("Username or password is blank");
            }
        }

        public void LoginEmployee()
        {
            MySqlConnection conn = DBUtils.GetDBConnection();

            if (txtTenDangNhap.Text != "" && txtMatKhau.Text != "")
            {
                try
                {

                    string tk = txtTenDangNhap.Text;
                    string mk = txtMatKhau.Text;

                    String sql = "SELECT * FROM nhanvien " +
                    "where taiKhoan = '" + tk + "'" + "and matKhau=" + "'" + mk + "';";

                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = conn,
                        CommandText = sql
                    };

                    conn.Open();

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows == true)
                    {
                        MessageBox.Show("Đăng nhập thành công", "Tiếp tục", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        GiaoDienChinh gdc = new GiaoDienChinh();
                        gdc.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("User name or password incorrect.Try again");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Username or password is blank");
            }
        }
    }
}

