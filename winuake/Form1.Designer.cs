namespace winuake
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddTab = new System.Windows.Forms.Button();
            this.tabShell1 = new System.Windows.Forms.TabPage();
            this.tabCtrl = new System.Windows.Forms.TabControl();
            this.tabCtrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Winuake";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(1386, 619);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(20, 21);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddTab
            // 
            this.btnAddTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTab.BackColor = System.Drawing.SystemColors.Desktop;
            this.btnAddTab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTab.Location = new System.Drawing.Point(1360, 619);
            this.btnAddTab.Name = "btnAddTab";
            this.btnAddTab.Size = new System.Drawing.Size(20, 21);
            this.btnAddTab.TabIndex = 2;
            this.btnAddTab.Text = "+";
            this.btnAddTab.UseVisualStyleBackColor = false;
            this.btnAddTab.Click += new System.EventHandler(this.btnAddTab_Click);
            // 
            // tabShell1
            // 
            this.tabShell1.BackColor = System.Drawing.Color.Transparent;
            this.tabShell1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tabShell1.Location = new System.Drawing.Point(4, 4);
            this.tabShell1.Name = "tabShell1";
            this.tabShell1.Padding = new System.Windows.Forms.Padding(3);
            this.tabShell1.Size = new System.Drawing.Size(1089, 414);
            this.tabShell1.TabIndex = 0;
            this.tabShell1.Text = "Shell 1";
            // 
            // tabCtrl
            // 
            this.tabCtrl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabCtrl.AllowDrop = true;
            this.tabCtrl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabCtrl.Controls.Add(this.tabShell1);
            this.tabCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCtrl.Location = new System.Drawing.Point(97, 47);
            this.tabCtrl.Name = "tabCtrl";
            this.tabCtrl.SelectedIndex = 0;
            this.tabCtrl.Size = new System.Drawing.Size(1097, 440);
            this.tabCtrl.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InfoText;
            this.ClientSize = new System.Drawing.Size(1418, 652);
            this.Controls.Add(this.btnAddTab);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabCtrl);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.Text = "Winuake";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
            this.tabCtrl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddTab;
        private System.Windows.Forms.TabPage tabShell1;
        private System.Windows.Forms.TabControl tabCtrl;
    }
}

