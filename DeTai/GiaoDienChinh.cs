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
    public partial class GiaoDienChinh : Form
    {
        public GiaoDienChinh()
        {
            InitializeComponent();
        }

        private void btQuanLyPCP_Click(object sender, EventArgs e)
        {
            QuanLyPhongChieuPhim quanLyPhongChieu = new QuanLyPhongChieuPhim();
            this.Hide();
            quanLyPhongChieu.ShowDialog();
            this.Show();
        }

        private void btQuanLyPhim_Click(object sender, EventArgs e)
        {
            QuanLyPhim quanLyPhim = new QuanLyPhim();
            this.Hide();
            quanLyPhim.ShowDialog();
            this.Show();
        }

        private void btQuanLyTV_Click(object sender, EventArgs e)
        {
            QuanLyThanhVien quanLyTV = new QuanLyThanhVien();
            this.Hide();
            quanLyTV.ShowDialog();
            this.Show();
        }

        private void btQuanLyNV_Click(object sender, EventArgs e)
        {
            QuanLyNhanVien quanLyNV = new QuanLyNhanVien();
            this.Hide();
            quanLyNV.ShowDialog();
            this.Show();
        }

        private void btDatVe_Click(object sender, EventArgs e)
        {
            QuanLyVe datVe = new QuanLyVe();
            this.Hide();
            datVe.ShowDialog();
            this.Show();
        }

        private void btQuanLyDV_Click(object sender, EventArgs e)
        {
            QuanLyDichVu quanLyDV = new QuanLyDichVu();
            this.Hide();
            quanLyDV.ShowDialog();
            this.Show();
        }

        // đóng form
        private void GiaoDienChinh_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult thoat = MessageBox.Show("Bạn có chắc chắn thoát", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (thoat == DialogResult.No)
                e.Cancel = true;
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

        private void GiaoDienChinh_Load(object sender, EventArgs e)
        {
            btQuanLyTV.Select();
        }
    }
}
