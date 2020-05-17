﻿namespace winuake
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
            this.tabCtrl = new Dotnetrix.Controls.TabControlEX();
            this.tabPageEX1 = new Dotnetrix.Controls.TabPageEX();
            this.tabPageEX2 = new Dotnetrix.Controls.TabPageEX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pctAdd = new System.Windows.Forms.PictureBox();
            this.pctHide = new System.Windows.Forms.PictureBox();
            this.pctToggle = new System.Windows.Forms.PictureBox();
            this.ptcClose = new System.Windows.Forms.PictureBox();
            this.tabCtrl.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctHide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctToggle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptcClose)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Winuake";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // tabCtrl
            // 
            this.tabCtrl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabCtrl.AllowDrop = true;
            this.tabCtrl.AllowTabDrag = true;
            this.tabCtrl.Appearance = Dotnetrix.Controls.TabAppearanceEX.FlatTab;
            this.tabCtrl.Controls.Add(this.tabPageEX1);
            this.tabCtrl.Controls.Add(this.tabPageEX2);
            this.tabCtrl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tabCtrl.FlatBorderColor = System.Drawing.Color.LimeGreen;
            this.tabCtrl.ForeColor = System.Drawing.Color.LimeGreen;
            this.tabCtrl.HotColor = System.Drawing.Color.White;
            this.tabCtrl.HotTrack = true;
            this.tabCtrl.Location = new System.Drawing.Point(-2, 1);
            this.tabCtrl.Name = "tabCtrl";
            this.tabCtrl.SelectedIndex = 0;
            this.tabCtrl.SelectedTabColor = System.Drawing.Color.Black;
            this.tabCtrl.Size = new System.Drawing.Size(1424, 370);
            this.tabCtrl.TabColor = System.Drawing.Color.Black;
            this.tabCtrl.TabIndex = 3;
            this.tabCtrl.UseVisualStyles = false;
            // 
            // tabPageEX1
            // 
            this.tabPageEX1.Location = new System.Drawing.Point(4, 4);
            this.tabPageEX1.Name = "tabPageEX1";
            this.tabPageEX1.Size = new System.Drawing.Size(1416, 341);
            this.tabPageEX1.TabIndex = 0;
            this.tabPageEX1.Text = "tabPageEX1";
            // 
            // tabPageEX2
            // 
            this.tabPageEX2.Location = new System.Drawing.Point(4, 4);
            this.tabPageEX2.Name = "tabPageEX2";
            this.tabPageEX2.Size = new System.Drawing.Size(1416, 341);
            this.tabPageEX2.TabIndex = 1;
            this.tabPageEX2.Text = "tabPageEX2";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LimeGreen;
            this.panel1.Controls.Add(this.pctAdd);
            this.panel1.Controls.Add(this.pctHide);
            this.panel1.Controls.Add(this.pctToggle);
            this.panel1.Controls.Add(this.ptcClose);
            this.panel1.Location = new System.Drawing.Point(-2, 514);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1435, 28);
            this.panel1.TabIndex = 5;
            // 
            // pctAdd
            // 
            this.pctAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pctAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pctAdd.Image = global::winuake.Properties.Resources.add_black_transparent;
            this.pctAdd.Location = new System.Drawing.Point(4, 0);
            this.pctAdd.Name = "pctAdd";
            this.pctAdd.Size = new System.Drawing.Size(27, 25);
            this.pctAdd.TabIndex = 6;
            this.pctAdd.TabStop = false;
            this.pctAdd.Click += new System.EventHandler(this.pctAdd_Click);
            this.pctAdd.MouseEnter += new System.EventHandler(this.pctAdd_MouseEnter);
            this.pctAdd.MouseLeave += new System.EventHandler(this.pctAdd_MouseLeave);
            // 
            // pctHide
            // 
            this.pctHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pctHide.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pctHide.Image = global::winuake.Properties.Resources.hide_black_transparent;
            this.pctHide.Location = new System.Drawing.Point(1338, 0);
            this.pctHide.Name = "pctHide";
            this.pctHide.Size = new System.Drawing.Size(28, 25);
            this.pctHide.TabIndex = 6;
            this.pctHide.TabStop = false;
            this.pctHide.Click += new System.EventHandler(this.pctHide_Click);
            this.pctHide.MouseEnter += new System.EventHandler(this.pctHide_MouseEnter);
            this.pctHide.MouseLeave += new System.EventHandler(this.pctHide_MouseLeave);
            // 
            // pctToggle
            // 
            this.pctToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pctToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pctToggle.Image = global::winuake.Properties.Resources.minimize_black_transparent;
            this.pctToggle.Location = new System.Drawing.Point(1372, 0);
            this.pctToggle.Name = "pctToggle";
            this.pctToggle.Size = new System.Drawing.Size(26, 25);
            this.pctToggle.TabIndex = 6;
            this.pctToggle.TabStop = false;
            this.pctToggle.Click += new System.EventHandler(this.pctToggle_Click);
            this.pctToggle.MouseEnter += new System.EventHandler(this.pctToggle_MouseEnter);
            this.pctToggle.MouseLeave += new System.EventHandler(this.pctToggle_MouseLeave);
            // 
            // ptcClose
            // 
            this.ptcClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ptcClose.BackColor = System.Drawing.Color.Transparent;
            this.ptcClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ptcClose.Image = global::winuake.Properties.Resources.close_black_transparent;
            this.ptcClose.Location = new System.Drawing.Point(1404, 0);
            this.ptcClose.Name = "ptcClose";
            this.ptcClose.Size = new System.Drawing.Size(27, 25);
            this.ptcClose.TabIndex = 4;
            this.ptcClose.TabStop = false;
            this.ptcClose.Click += new System.EventHandler(this.ptcClose_Click);
            this.ptcClose.MouseEnter += new System.EventHandler(this.ptcClose_MouseEnter);
            this.ptcClose.MouseLeave += new System.EventHandler(this.ptcClose_MouseLeave);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InfoText;
            this.ClientSize = new System.Drawing.Size(1429, 538);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabCtrl);
            this.ForeColor = System.Drawing.Color.DarkGreen;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.Opacity = 0.8D;
            this.Text = "Winuake";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.SizeChanged += new System.EventHandler(this.frmMain_SizeChanged);
            this.tabCtrl.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctHide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctToggle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptcClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private Dotnetrix.Controls.TabControlEX tabCtrl;
        private Dotnetrix.Controls.TabPageEX tabPageEX1;
        private Dotnetrix.Controls.TabPageEX tabPageEX2;
        private System.Windows.Forms.PictureBox ptcClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pctToggle;
        private System.Windows.Forms.PictureBox pctHide;
        private System.Windows.Forms.PictureBox pctAdd;
    }
}

