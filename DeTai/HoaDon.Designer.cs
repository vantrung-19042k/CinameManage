namespace DeTai
{
    partial class HoaDon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbHoaDon = new System.Windows.Forms.Label();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.listboxHD = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbHoaDon
            // 
            this.lbHoaDon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHoaDon.Location = new System.Drawing.Point(70, 9);
            this.lbHoaDon.Name = "lbHoaDon";
            this.lbHoaDon.Size = new System.Drawing.Size(181, 36);
            this.lbHoaDon.TabIndex = 1;
            this.lbHoaDon.Text = "Thông Tin Hóa Đơn";
            this.lbHoaDon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.Location = new System.Drawing.Point(128, 415);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(75, 23);
            this.btnThanhToan.TabIndex = 2;
            this.btnThanhToan.Text = "Thanh Toán";
            this.btnThanhToan.UseVisualStyleBackColor = true;
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // listboxHD
            // 
            this.listboxHD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listboxHD.FormattingEnabled = true;
            this.listboxHD.ItemHeight = 16;
            this.listboxHD.Location = new System.Drawing.Point(29, 65);
            this.listboxHD.Name = "listboxHD";
            this.listboxHD.Size = new System.Drawing.Size(278, 276);
            this.listboxHD.TabIndex = 3;
            // 
            // HoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 450);
            this.Controls.Add(this.listboxHD);
            this.Controls.Add(this.btnThanhToan);
            this.Controls.Add(this.lbHoaDon);
            this.Name = "HoaDon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HoaDon";
            this.Load += new System.EventHandler(this.HoaDon_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbHoaDon;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.ListBox listboxHD;
    }
}