namespace datamax_bt_test
{
    partial class Form_BtTest
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
            this.btn_Connect = new System.Windows.Forms.Button();
            this.txt_HwAddr = new System.Windows.Forms.TextBox();
            this.lbl_HwAddr = new System.Windows.Forms.Label();
            this.lbl_Title = new System.Windows.Forms.Label();
            this.txt_Log = new System.Windows.Forms.TextBox();
            this.pnl_Main = new System.Windows.Forms.Panel();
            this.pnl_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(111, 51);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(124, 43);
            this.btn_Connect.TabIndex = 1;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // txt_HwAddr
            // 
            this.txt_HwAddr.Location = new System.Drawing.Point(5, 71);
            this.txt_HwAddr.Name = "txt_HwAddr";
            this.txt_HwAddr.Size = new System.Drawing.Size(100, 23);
            this.txt_HwAddr.TabIndex = 2;
            this.txt_HwAddr.GotFocus += new System.EventHandler(this.txt_HwAddr_GotFocus);
            // 
            // lbl_HwAddr
            // 
            this.lbl_HwAddr.Location = new System.Drawing.Point(5, 51);
            this.lbl_HwAddr.Name = "lbl_HwAddr";
            this.lbl_HwAddr.Size = new System.Drawing.Size(100, 15);
            this.lbl_HwAddr.Text = "Mac address:";
            // 
            // lbl_Title
            // 
            this.lbl_Title.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular);
            this.lbl_Title.Location = new System.Drawing.Point(22, 7);
            this.lbl_Title.Name = "lbl_Title";
            this.lbl_Title.Size = new System.Drawing.Size(197, 31);
            this.lbl_Title.Text = "Datamax BT Test";
            // 
            // txt_Log
            // 
            this.txt_Log.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular);
            this.txt_Log.Location = new System.Drawing.Point(5, 100);
            this.txt_Log.Multiline = true;
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.ReadOnly = true;
            this.txt_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Log.Size = new System.Drawing.Size(230, 210);
            this.txt_Log.TabIndex = 4;
            // 
            // pnl_Main
            // 
            this.pnl_Main.BackColor = System.Drawing.Color.White;
            this.pnl_Main.Controls.Add(this.lbl_Title);
            this.pnl_Main.Controls.Add(this.lbl_HwAddr);
            this.pnl_Main.Controls.Add(this.txt_HwAddr);
            this.pnl_Main.Controls.Add(this.btn_Connect);
            this.pnl_Main.Controls.Add(this.txt_Log);
            this.pnl_Main.Location = new System.Drawing.Point(0, 0);
            this.pnl_Main.Name = "pnl_Main";
            this.pnl_Main.Size = new System.Drawing.Size(240, 320);
            // 
            // Form_BtTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.pnl_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "Form_BtTest";
            this.Text = "Datamax BT Test";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_BtTest_KeyDown);
            this.pnl_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.TextBox txt_HwAddr;
        private System.Windows.Forms.Label lbl_HwAddr;
        private System.Windows.Forms.Label lbl_Title;
        private System.Windows.Forms.TextBox txt_Log;
        private System.Windows.Forms.Panel pnl_Main;
    }
}

