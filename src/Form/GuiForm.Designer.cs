namespace cwc {
    partial class GuiForm {
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
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setCwcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cwCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gZEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.libRTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emscriptenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lauchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nothingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sanitizeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stopBuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.afterFileErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workingDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_Build = new System.Windows.Forms.ToolStripMenuItem();
            this.iDEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notePadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.msMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMenu
            // 
            this.msMenu.BackColor = System.Drawing.Color.Black;
            this.msMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.msMenu.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msMenu.GripMargin = new System.Windows.Forms.Padding(2);
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.updateToolStripMenuItem,
            this.lauchToolStripMenuItem,
            this.viewInToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.pathToolStripMenuItem,
            this.ToolStrip_Build,
            this.iDEToolStripMenuItem});
            this.msMenu.Location = new System.Drawing.Point(0, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.msMenu.ShowItemToolTips = true;
            this.msMenu.Size = new System.Drawing.Size(467, 26);
            this.msMenu.TabIndex = 49;
            this.msMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.msMenu_ItemClicked);
            this.msMenu.MouseCaptureChanged += new System.EventHandler(this.msMenu_MouseCaptureChanged);
            this.msMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.msMenu_MouseDown);
            this.msMenu.MouseEnter += new System.EventHandler(this.msMenu_MouseEnter);
            this.msMenu.MouseLeave += new System.EventHandler(this.msMenu_MouseLeave);
            this.msMenu.MouseHover += new System.EventHandler(this.msMenu_MouseHover);
            this.msMenu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.msMenu_MouseMove);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setCwcToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // setCwcToolStripMenuItem
            // 
            this.setCwcToolStripMenuItem.Name = "setCwcToolStripMenuItem";
            this.setCwcToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.setCwcToolStripMenuItem.Text = "Set Cwc";
            this.setCwcToolStripMenuItem.Click += new System.EventHandler(this.setCwcToolStripMenuItem_Click);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.BackColor = System.Drawing.Color.Gray;
            this.updateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.cwCToolStripMenuItem,
            this.demosToolStripMenuItem,
            this.gZEToolStripMenuItem,
            this.toolStripSeparator2,
            this.libRTToolStripMenuItem,
            this.emscriptenToolStripMenuItem});
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.RightToLeftAutoMirrorImage = true;
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(155, 6);
            // 
            // cwCToolStripMenuItem
            // 
            this.cwCToolStripMenuItem.Name = "cwCToolStripMenuItem";
            this.cwCToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.cwCToolStripMenuItem.Text = "Honera/Cwc";
            this.cwCToolStripMenuItem.Click += new System.EventHandler(this.cwCToolStripMenuItem_Click);
            // 
            // demosToolStripMenuItem
            // 
            this.demosToolStripMenuItem.Name = "demosToolStripMenuItem";
            this.demosToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.demosToolStripMenuItem.Text = "Honera/Demos";
            // 
            // gZEToolStripMenuItem
            // 
            this.gZEToolStripMenuItem.Name = "gZEToolStripMenuItem";
            this.gZEToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.gZEToolStripMenuItem.Text = "Honera/GZE";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(155, 6);
            // 
            // libRTToolStripMenuItem
            // 
            this.libRTToolStripMenuItem.Name = "libRTToolStripMenuItem";
            this.libRTToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.libRTToolStripMenuItem.Text = "Honera/LibRT";
            // 
            // emscriptenToolStripMenuItem
            // 
            this.emscriptenToolStripMenuItem.Name = "emscriptenToolStripMenuItem";
            this.emscriptenToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.emscriptenToolStripMenuItem.Text = "Honera/WebRT";
            // 
            // lauchToolStripMenuItem
            // 
            this.lauchToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lauchToolStripMenuItem.Name = "lauchToolStripMenuItem";
            this.lauchToolStripMenuItem.Size = new System.Drawing.Size(54, 22);
            this.lauchToolStripMenuItem.Text = "Lauch";
            this.lauchToolStripMenuItem.Click += new System.EventHandler(this.lauchToolStripMenuItem_Click);
            // 
            // viewInToolStripMenuItem
            // 
            this.viewInToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.viewInToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultToolStripMenuItem});
            this.viewInToolStripMenuItem.Name = "viewInToolStripMenuItem";
            this.viewInToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            this.viewInToolStripMenuItem.Text = "View In";
            this.viewInToolStripMenuItem.Click += new System.EventHandler(this.viewInToolStripMenuItem_Click);
            this.viewInToolStripMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.viewInToolStripMenuItem_MouseDown);
            // 
            // defaultToolStripMenuItem
            // 
            this.defaultToolStripMenuItem.Checked = true;
            this.defaultToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            this.defaultToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.defaultToolStripMenuItem.Text = "(Default)";
            this.defaultToolStripMenuItem.Click += new System.EventHandler(this.defaultToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.BackColor = System.Drawing.Color.Gray;
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildToolStripMenuItem,
            this.stopBuildToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // buildToolStripMenuItem
            // 
            this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nothingToolStripMenuItem1,
            this.runToolStripMenuItem1,
            this.sanitizeToolStripMenuItem1});
            this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            this.buildToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.buildToolStripMenuItem.Text = "Build &&";
            // 
            // nothingToolStripMenuItem1
            // 
            this.nothingToolStripMenuItem1.Name = "nothingToolStripMenuItem1";
            this.nothingToolStripMenuItem1.Size = new System.Drawing.Size(130, 22);
            this.nothingToolStripMenuItem1.Text = "Nothing";
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Checked = true;
            this.runToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            this.runToolStripMenuItem1.Size = new System.Drawing.Size(130, 22);
            this.runToolStripMenuItem1.Text = "Run";
            // 
            // sanitizeToolStripMenuItem1
            // 
            this.sanitizeToolStripMenuItem1.Name = "sanitizeToolStripMenuItem1";
            this.sanitizeToolStripMenuItem1.Size = new System.Drawing.Size(130, 22);
            this.sanitizeToolStripMenuItem1.Text = "Sanitize";
            // 
            // stopBuildToolStripMenuItem
            // 
            this.stopBuildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.afterFileErrorToolStripMenuItem});
            this.stopBuildToolStripMenuItem.Name = "stopBuildToolStripMenuItem";
            this.stopBuildToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.stopBuildToolStripMenuItem.Text = "Stop build";
            // 
            // afterFileErrorToolStripMenuItem
            // 
            this.afterFileErrorToolStripMenuItem.Checked = true;
            this.afterFileErrorToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.afterFileErrorToolStripMenuItem.Name = "afterFileErrorToolStripMenuItem";
            this.afterFileErrorToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.afterFileErrorToolStripMenuItem.Text = "After file error";
            this.afterFileErrorToolStripMenuItem.Click += new System.EventHandler(this.afterFileErrorToolStripMenuItem_Click);
            // 
            // pathToolStripMenuItem
            // 
            this.pathToolStripMenuItem.BackColor = System.Drawing.Color.Gray;
            this.pathToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.workingDirToolStripMenuItem,
            this.outputToolStripMenuItem});
            this.pathToolStripMenuItem.Name = "pathToolStripMenuItem";
            this.pathToolStripMenuItem.Size = new System.Drawing.Size(47, 22);
            this.pathToolStripMenuItem.Text = "Path";
            // 
            // workingDirToolStripMenuItem
            // 
            this.workingDirToolStripMenuItem.Name = "workingDirToolStripMenuItem";
            this.workingDirToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.workingDirToolStripMenuItem.Text = "WorkingDir";
            this.workingDirToolStripMenuItem.Click += new System.EventHandler(this.workingDirToolStripMenuItem_Click);
            // 
            // outputToolStripMenuItem
            // 
            this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            this.outputToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.outputToolStripMenuItem.Text = "Output";
            this.outputToolStripMenuItem.Click += new System.EventHandler(this.outputToolStripMenuItem_Click);
            // 
            // ToolStrip_Build
            // 
            this.ToolStrip_Build.BackColor = System.Drawing.Color.Teal;
            this.ToolStrip_Build.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolStrip_Build.Name = "ToolStrip_Build";
            this.ToolStrip_Build.Size = new System.Drawing.Size(60, 22);
            this.ToolStrip_Build.Text = "BUILD";
            this.ToolStrip_Build.Click += new System.EventHandler(this.ToolStrip_Build_Click);
            // 
            // iDEToolStripMenuItem
            // 
            this.iDEToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(94)))));
            this.iDEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notePadToolStripMenuItem});
            this.iDEToolStripMenuItem.Name = "iDEToolStripMenuItem";
            this.iDEToolStripMenuItem.Size = new System.Drawing.Size(40, 22);
            this.iDEToolStripMenuItem.Text = "IDE";
            this.iDEToolStripMenuItem.Click += new System.EventHandler(this.iDEToolStripMenuItem_Click);
            // 
            // notePadToolStripMenuItem
            // 
            this.notePadToolStripMenuItem.Name = "notePadToolStripMenuItem";
            this.notePadToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.notePadToolStripMenuItem.Text = "Notepad++";
            this.notePadToolStripMenuItem.Click += new System.EventHandler(this.notePadToolStripMenuItem_Click);
            // 
            // GuiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(579, 73);
            this.Controls.Add(this.msMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "GuiForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "GuiForm";
            this.Load += new System.EventHandler(this.GuiForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GuiForm_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GuiForm_KeyUp);
            this.MouseLeave += new System.EventHandler(this.GuiForm_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GuiForm_MouseMove);
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCwcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cwCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gZEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem libRTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem demosToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem emscriptenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workingDirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Build;
        private System.Windows.Forms.ToolStripMenuItem lauchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iDEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notePadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nothingToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sanitizeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem stopBuildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem afterFileErrorToolStripMenuItem;
    }
}