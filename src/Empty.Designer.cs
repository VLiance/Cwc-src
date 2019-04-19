namespace cwc
{
	partial class Empty
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
            this.SuspendLayout();
            // 
            // Empty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 195);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Empty";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Empty";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Empty_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Empty_FormClosed);
            this.Load += new System.EventHandler(this.Empty_Load);
            this.ResizeBegin += new System.EventHandler(this.Empty_ResizeBegin);
            this.LocationChanged += new System.EventHandler(this.Empty_LocationChanged);
            this.RegionChanged += new System.EventHandler(this.Empty_RegionChanged);
            this.Move += new System.EventHandler(this.Empty_Move);
            this.Resize += new System.EventHandler(this.Empty_Resize);
            this.ResumeLayout(false);

		}

        #endregion
    }
}