namespace DeTai
{
    partial class FormPhim
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
            this.flowLayoutPanelDsPhim = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCaChieu = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flowLayoutPanelDsPhim
            // 
            this.flowLayoutPanelDsPhim.Location = new System.Drawing.Point(12, 100);
            this.flowLayoutPanelDsPhim.Name = "flowLayoutPanelDsPhim";
            this.flowLayoutPanelDsPhim.Size = new System.Drawing.Size(855, 528);
            this.flowLayoutPanelDsPhim.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(579, 78);
            this.label1.TabIndex = 1;
            this.label1.Text = "Danh sách phim đang chiếu";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbCaChieu
            // 
            this.cbCaChieu.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCaChieu.FormattingEnabled = true;
            this.cbCaChieu.Items.AddRange(new object[] {
            "7h -> 9h",
            "9h -> 11h",
            "11h -> 13h",
            "13h -> 15h",
            "15h -> 17h",
            "17h -> 19h",
            "19h -> 21h",
            "21h -> 23h"});
            this.cbCaChieu.Location = new System.Drawing.Point(692, 35);
            this.cbCaChieu.Name = "cbCaChieu";
            this.cbCaChieu.Size = new System.Drawing.Size(162, 32);
            this.cbCaChieu.TabIndex = 2;
            this.cbCaChieu.TextChanged += new System.EventHandler(this.cbCaChieu_TextChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(597, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ca chiếu";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormPhim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 640);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCaChieu);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanelDsPhim);
            this.MaximizeBox = false;
            this.Name = "FormPhim";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormPhim";
            this.Load += new System.EventHandler(this.FormPhim_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDsPhim;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbCaChieu;
        private System.Windows.Forms.Label label2;
    }
}