namespace cwc {
    partial class Install {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblTitle = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbAnnoy = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnUnset = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.a = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbPaths = new System.Windows.Forms.ComboBox();
            this.btnAsscociate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(73, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(459, 43);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Cwc is not in your environment, would you set it? (Recommended)";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Current Environnement Paths List:";
            // 
            // cbAnnoy
            // 
            this.cbAnnoy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAnnoy.AutoSize = true;
            this.cbAnnoy.Location = new System.Drawing.Point(525, 263);
            this.cbAnnoy.Name = "cbAnnoy";
            this.cbAnnoy.Size = new System.Drawing.Size(83, 17);
            this.cbAnnoy.TabIndex = 30;
            this.cbAnnoy.Text = "Don\'t annoy";
            this.cbAnnoy.UseVisualStyleBackColor = true;
            this.cbAnnoy.Visible = false;
            this.cbAnnoy.CheckedChanged += new System.EventHandler(this.cbAnnoy_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(237, 254);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(127, 31);
            this.btnOk.TabIndex = 31;
            this.btnOk.Text = "Keep Cwc not set";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnSet
            // 
            this.btnSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSet.Location = new System.Drawing.Point(214, 219);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(69, 22);
            this.btnSet.TabIndex = 32;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnUnset
            // 
            this.btnUnset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnset.Location = new System.Drawing.Point(310, 219);
            this.btnUnset.Name = "btnUnset";
            this.btnUnset.Size = new System.Drawing.Size(72, 22);
            this.btnUnset.TabIndex = 33;
            this.btnUnset.Text = "Unset";
            this.btnUnset.UseVisualStyleBackColor = true;
            this.btnUnset.Click += new System.EventHandler(this.btnUnset_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Current Location :";
            // 
            // a
            // 
            this.a.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.a.Enabled = false;
            this.a.Location = new System.Drawing.Point(22, 106);
            this.a.Name = "a";
            this.a.Size = new System.Drawing.Size(585, 20);
            this.a.TabIndex = 34;
            this.a.TextChanged += new System.EventHandler(this.tbLocation_TextChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(39, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(550, 31);
            this.label3.TabIndex = 36;
            this.label3.Text = "When Cwc is set in your environement, you can use the \"cwc\" command from anywhere" +
    ", without specifing is absolute or relative path";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // cbPaths
            // 
            this.cbPaths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPaths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaths.FormattingEnabled = true;
            this.cbPaths.Location = new System.Drawing.Point(22, 184);
            this.cbPaths.Name = "cbPaths";
            this.cbPaths.Size = new System.Drawing.Size(584, 21);
            this.cbPaths.TabIndex = 37;
            this.cbPaths.SelectedIndexChanged += new System.EventHandler(this.cbPaths_SelectedIndexChanged);
            // 
            // btnAsscociate
            // 
            this.btnAsscociate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAsscociate.Location = new System.Drawing.Point(448, 131);
            this.btnAsscociate.Name = "btnAsscociate";
            this.btnAsscociate.Size = new System.Drawing.Size(160, 31);
            this.btnAsscociate.TabIndex = 38;
            this.btnAsscociate.Text = "Associate Files Extention";
            this.btnAsscociate.UseVisualStyleBackColor = true;
            this.btnAsscociate.Click += new System.EventHandler(this.btnAsscociate_Click);
            // 
            // Install
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 290);
            this.Controls.Add(this.btnAsscociate);
            this.Controls.Add(this.cbPaths);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.a);
            this.Controls.Add(this.btnUnset);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cbAnnoy);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblTitle);
            this.Name = "Install";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.Install_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbAnnoy;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnUnset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox a;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbPaths;
        private System.Windows.Forms.Button btnAsscociate;
    }
}