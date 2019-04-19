namespace Updater
{
    partial class Form1
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
			this.lblProgress = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lOutput = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lblProgress
			// 
			this.lblProgress.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProgress.Location = new System.Drawing.Point(-1, 46);
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new System.Drawing.Size(781, 23);
			this.lblProgress.TabIndex = 0;
			this.lblProgress.Text = "Update in progress ...";
			this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblProgress.Click += new System.EventHandler(this.label1_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(3, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(777, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "CWC";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label2.Click += new System.EventHandler(this.label2_Click);
			// 
			// lOutput
			// 
			this.lOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lOutput.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lOutput.FormattingEnabled = true;
			this.lOutput.ItemHeight = 18;
			this.lOutput.Location = new System.Drawing.Point(34, 83);
			this.lOutput.Name = "lOutput";
			this.lOutput.Size = new System.Drawing.Size(708, 202);
			this.lOutput.TabIndex = 2;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(779, 311);
			this.Controls.Add(this.lOutput);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblProgress);
			this.Name = "Form1";
			this.Text = "Updating";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lOutput;
    }
}

