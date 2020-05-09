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
            this.pnlOutput = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlOutput
            // 
            this.pnlOutput.Location = new System.Drawing.Point(1, 1);
            this.pnlOutput.Name = "pnlOutput";
            this.pnlOutput.Size = new System.Drawing.Size(1308, 551);
            this.pnlOutput.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1437, 564);
            this.Controls.Add(this.pnlOutput);
            this.Name = "frmMain";
            this.Text = "Winuake";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlOutput;
    }
}

