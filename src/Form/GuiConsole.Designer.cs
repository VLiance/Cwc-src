namespace cwc {
    partial class GuiConsole {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GuiConsole));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.lauchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setCwcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nothingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sanitizeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stopBuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.afterFileErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sanitizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_Build = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tODOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iDEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.liteWayvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notePadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.cmMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnAll = new System.Windows.Forms.Panel();
            this.pnCenter = new System.Windows.Forms.Panel();
            this.fctbConsole = new cwc.CwFCTB();
            this.panel4 = new System.Windows.Forms.Panel();
            this.hMyScrollBar = new cwc.CwScrollBar();
            this.pnRight = new System.Windows.Forms.Panel();
            this.vMyScrollBar = new cwc.CwScrollBar();
            this.csPrj = new NJFLib.Controls.CollapsibleSplitter();
            this.pnTreeView = new System.Windows.Forms.Panel();
            this.pnTreeCenter = new cwc.DoubleBufferedPanel();
            this.pnTtreeViewPrj = new cwc.DoubleBufferedPanel();
            this.treeViewPrj = new cwc.CwTreeView();
            this.pnTreeButtom = new System.Windows.Forms.Panel();
            this.hTreePrjScrollBar = new cwc.CwScrollBar();
            this.pnTreeRight = new System.Windows.Forms.Panel();
            this.vTreePrjScrollBar = new cwc.CwScrollBar();
            this.msMenu.SuspendLayout();
            this.cmMain.SuspendLayout();
            this.pnAll.SuspendLayout();
            this.pnCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctbConsole)).BeginInit();
            this.panel4.SuspendLayout();
            this.pnRight.SuspendLayout();
            this.pnTreeView.SuspendLayout();
            this.pnTreeCenter.SuspendLayout();
            this.pnTtreeViewPrj.SuspendLayout();
            this.pnTreeButtom.SuspendLayout();
            this.pnTreeRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.MenuText;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(62, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(846, 23);
            this.textBox1.TabIndex = 8;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // msMenu
            // 
            this.msMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.msMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(45)))));
            this.msMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.msMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.msMenu.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msMenu.GripMargin = new System.Windows.Forms.Padding(1);
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lauchToolStripMenuItem,
            this.configToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.viewInToolStripMenuItem,
            this.ToolStrip_Build,
            this.updateToolStripMenuItem,
            this.iDEToolStripMenuItem,
            this.runToolStripMenuItem});
            this.msMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.msMenu.Location = new System.Drawing.Point(569, 2);
            this.msMenu.Name = "msMenu";
            this.msMenu.Padding = new System.Windows.Forms.Padding(1);
            this.msMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.msMenu.ShowItemToolTips = true;
            this.msMenu.Size = new System.Drawing.Size(476, 20);
            this.msMenu.TabIndex = 50;
            // 
            // lauchToolStripMenuItem
            // 
            this.lauchToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lauchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripSeparator3});
            this.lauchToolStripMenuItem.Image = global::cwc.Properties.Resources.Menu0003;
            this.lauchToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.lauchToolStripMenuItem.Name = "lauchToolStripMenuItem";
            this.lauchToolStripMenuItem.Size = new System.Drawing.Size(62, 18);
            this.lauchToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.lauchToolStripMenuItem.Click += new System.EventHandler(this.lauchToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(99, 6);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.configToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setCwcToolStripMenuItem});
            this.configToolStripMenuItem.ForeColor = System.Drawing.Color.DarkRed;
            this.configToolStripMenuItem.Image = global::cwc.Properties.Resources.Menu0001;
            this.configToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(62, 18);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // setCwcToolStripMenuItem
            // 
            this.setCwcToolStripMenuItem.Name = "setCwcToolStripMenuItem";
            this.setCwcToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.setCwcToolStripMenuItem.Text = "Set Cwc";
            this.setCwcToolStripMenuItem.Click += new System.EventHandler(this.setCwcToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.BackColor = System.Drawing.Color.Gray;
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildToolStripMenuItem,
            this.stopBuildToolStripMenuItem,
            this.debugTypeToolStripMenuItem});
            this.optionsToolStripMenuItem.Image = global::cwc.Properties.Resources.Menu0005;
            this.optionsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(62, 18);
            this.optionsToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
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
            this.nothingToolStripMenuItem1.Click += new System.EventHandler(this.nothingToolStripMenuItem1_Click);
            // 
            // runToolStripMenuItem1
            // 
            this.runToolStripMenuItem1.Checked = true;
            this.runToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.runToolStripMenuItem1.Name = "runToolStripMenuItem1";
            this.runToolStripMenuItem1.Size = new System.Drawing.Size(130, 22);
            this.runToolStripMenuItem1.Text = "Run";
            this.runToolStripMenuItem1.Click += new System.EventHandler(this.runToolStripMenuItem1_Click);
            // 
            // sanitizeToolStripMenuItem1
            // 
            this.sanitizeToolStripMenuItem1.Name = "sanitizeToolStripMenuItem1";
            this.sanitizeToolStripMenuItem1.Size = new System.Drawing.Size(130, 22);
            this.sanitizeToolStripMenuItem1.Text = "Sanitize";
            this.sanitizeToolStripMenuItem1.Click += new System.EventHandler(this.sanitizeToolStripMenuItem1_Click);
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
            // debugTypeToolStripMenuItem
            // 
            this.debugTypeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.gDBToolStripMenuItem,
            this.sanitizerToolStripMenuItem});
            this.debugTypeToolStripMenuItem.Name = "debugTypeToolStripMenuItem";
            this.debugTypeToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.debugTypeToolStripMenuItem.Text = "Debug Type";
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.noneToolStripMenuItem.Text = "None";
            // 
            // gDBToolStripMenuItem
            // 
            this.gDBToolStripMenuItem.Name = "gDBToolStripMenuItem";
            this.gDBToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.gDBToolStripMenuItem.Text = "Debugger";
            this.gDBToolStripMenuItem.Click += new System.EventHandler(this.gDBToolStripMenuItem_Click);
            // 
            // sanitizerToolStripMenuItem
            // 
            this.sanitizerToolStripMenuItem.Name = "sanitizerToolStripMenuItem";
            this.sanitizerToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.sanitizerToolStripMenuItem.Text = "Sanitizer";
            // 
            // viewInToolStripMenuItem
            // 
            this.viewInToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.viewInToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultToolStripMenuItem});
            this.viewInToolStripMenuItem.Image = global::cwc.Properties.Resources.Menu0004;
            this.viewInToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.viewInToolStripMenuItem.Name = "viewInToolStripMenuItem";
            this.viewInToolStripMenuItem.Size = new System.Drawing.Size(62, 18);
            this.viewInToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.viewInToolStripMenuItem.Click += new System.EventHandler(this.viewInToolStripMenuItem_Click);
            // 
            // defaultToolStripMenuItem
            // 
            this.defaultToolStripMenuItem.Checked = true;
            this.defaultToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            this.defaultToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.defaultToolStripMenuItem.Text = "(Default)";
            // 
            // ToolStrip_Build
            // 
            this.ToolStrip_Build.BackColor = System.Drawing.Color.Teal;
            this.ToolStrip_Build.Font = new System.Drawing.Font("Consolas", 11F);
            this.ToolStrip_Build.Image = global::cwc.Properties.Resources.Menu0007;
            this.ToolStrip_Build.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStrip_Build.Name = "ToolStrip_Build";
            this.ToolStrip_Build.Size = new System.Drawing.Size(62, 18);
            this.ToolStrip_Build.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.ToolStrip_Build.Click += new System.EventHandler(this.ToolStrip_Build_Click);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.BackColor = System.Drawing.Color.Gray;
            this.updateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripSeparator2,
            this.tODOToolStripMenuItem});
            this.updateToolStripMenuItem.Image = global::cwc.Properties.Resources.Menu0002;
            this.updateToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.RightToLeftAutoMirrorImage = true;
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(62, 18);
            this.updateToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(99, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(99, 6);
            // 
            // tODOToolStripMenuItem
            // 
            this.tODOToolStripMenuItem.Name = "tODOToolStripMenuItem";
            this.tODOToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.tODOToolStripMenuItem.Text = "TODO";
            // 
            // iDEToolStripMenuItem
            // 
            this.iDEToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(94)))));
            this.iDEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator4,
            this.liteWayvToolStripMenuItem,
            this.notePadToolStripMenuItem});
            this.iDEToolStripMenuItem.Image = global::cwc.Properties.Resources.Menu0008;
            this.iDEToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.iDEToolStripMenuItem.Name = "iDEToolStripMenuItem";
            this.iDEToolStripMenuItem.Size = new System.Drawing.Size(62, 18);
            this.iDEToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.iDEToolStripMenuItem.Click += new System.EventHandler(this.iDEToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(134, 6);
            // 
            // liteWayvToolStripMenuItem
            // 
            this.liteWayvToolStripMenuItem.Name = "liteWayvToolStripMenuItem";
            this.liteWayvToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.liteWayvToolStripMenuItem.Text = "LiteWayv";
            // 
            // notePadToolStripMenuItem
            // 
            this.notePadToolStripMenuItem.Checked = true;
            this.notePadToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.notePadToolStripMenuItem.Name = "notePadToolStripMenuItem";
            this.notePadToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.notePadToolStripMenuItem.Text = "Notepad++";
            this.notePadToolStripMenuItem.Click += new System.EventHandler(this.notePadToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 18);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // cbTitle
            // 
            this.cbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTitle.BackColor = System.Drawing.SystemColors.MenuText;
            this.cbTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cbTitle.Enabled = false;
            this.cbTitle.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTitle.ForeColor = System.Drawing.SystemColors.Window;
            this.cbTitle.Location = new System.Drawing.Point(24, 20);
            this.cbTitle.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.cbTitle.Name = "cbTitle";
            this.cbTitle.Size = new System.Drawing.Size(1045, 13);
            this.cbTitle.TabIndex = 52;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(909, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 23);
            this.label2.TabIndex = 53;
            this.label2.Text = " ↴ ";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.BackColor = System.Drawing.SystemColors.MenuText;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox2.Location = new System.Drawing.Point(0, 2);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(61, 23);
            this.textBox2.TabIndex = 54;
            this.textBox2.Text = "Cmd >>";
            // 
            // cmMain
            // 
            this.cmMain.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cmMain.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmMain.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.toolStripMenuItem5,
            this.findToolStripMenuItem});
            this.cmMain.Name = "cmMain";
            this.cmMain.ShowImageMargin = false;
            this.cmMain.Size = new System.Drawing.Size(136, 138);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.cutToolStripMenuItem.Image = global::cwc.Properties.Resources.Cut;
            this.cutToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Visible = false;
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(150)))));
            this.copyToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(150)))));
            this.pasteToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pasteToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Visible = false;
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(150)))));
            this.selectAllToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectAllToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.selectAllToolStripMenuItem.Text = "Select all";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(150)))));
            this.replaceToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.replaceToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.replaceToolStripMenuItem.Text = "Replace";
            this.replaceToolStripMenuItem.Visible = false;
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.AutoSize = false;
            this.toolStripMenuItem5.BackColor = System.Drawing.Color.Black;
            this.toolStripMenuItem5.Enabled = false;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(160, 2);
            this.toolStripMenuItem5.Text = "------------";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(80)))), ((int)(((byte)(150)))));
            this.findToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.findToolStripMenuItem.Text = "Find";
            // 
            // pnAll
            // 
            this.pnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnAll.Controls.Add(this.pnCenter);
            this.pnAll.Controls.Add(this.panel4);
            this.pnAll.Controls.Add(this.pnRight);
            this.pnAll.Controls.Add(this.csPrj);
            this.pnAll.Controls.Add(this.pnTreeView);
            this.pnAll.Location = new System.Drawing.Point(1, 35);
            this.pnAll.Name = "pnAll";
            this.pnAll.Size = new System.Drawing.Size(1161, 610);
            this.pnAll.TabIndex = 61;
            this.pnAll.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // pnCenter
            // 
            this.pnCenter.Controls.Add(this.fctbConsole);
            this.pnCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnCenter.Location = new System.Drawing.Point(0, 0);
            this.pnCenter.Name = "pnCenter";
            this.pnCenter.Size = new System.Drawing.Size(950, 564);
            this.pnCenter.TabIndex = 14;
            // 
            // fctbConsole
            // 
            this.fctbConsole.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctbConsole.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.fctbConsole.AutoSize = true;
            this.fctbConsole.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.fctbConsole.BackBrush = null;
            this.fctbConsole.BackColor = System.Drawing.Color.Black;
            this.fctbConsole.BookmarkColor = System.Drawing.Color.Aqua;
            this.fctbConsole.CaretColor = System.Drawing.Color.White;
            this.fctbConsole.CausesValidation = false;
            this.fctbConsole.CharHeight = 15;
            this.fctbConsole.CharWidth = 7;
            this.fctbConsole.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctbConsole.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctbConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fctbConsole.FoldingIndicatorColor = System.Drawing.Color.Gold;
            this.fctbConsole.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.fctbConsole.ForeColor = System.Drawing.Color.White;
            this.fctbConsole.IndentBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(35)))));
            this.fctbConsole.IsReplaceMode = false;
            this.fctbConsole.LineNumberColor = System.Drawing.Color.Cyan;
            this.fctbConsole.Location = new System.Drawing.Point(0, 0);
            this.fctbConsole.Name = "fctbConsole";
            this.fctbConsole.Paddings = new System.Windows.Forms.Padding(0);
            this.fctbConsole.ReadOnly = true;
            this.fctbConsole.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.fctbConsole.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctbConsole.ServiceColors")));
            this.fctbConsole.ServiceLinesColor = System.Drawing.Color.DimGray;
            this.fctbConsole.ShowScrollBars = false;
            this.fctbConsole.Size = new System.Drawing.Size(950, 564);
            this.fctbConsole.TabIndex = 6;
            this.fctbConsole.Zoom = 100;
            this.fctbConsole.SelectionChanged += new System.EventHandler(this.fctb_SelectionChanged);
            this.fctbConsole.ScrollbarsUpdated += new System.EventHandler(this.fctb_ScrollbarsUpdated);
            this.fctbConsole.Load += new System.EventHandler(this.fctb_Load);
            this.fctbConsole.MouseClick += new System.Windows.Forms.MouseEventHandler(this.fctb_MouseClick);
            this.fctbConsole.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fctb_MouseMove);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textBox1);
            this.panel4.Controls.Add(this.textBox2);
            this.panel4.Controls.Add(this.hMyScrollBar);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 564);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(950, 46);
            this.panel4.TabIndex = 13;
            // 
            // hMyScrollBar
            // 
            this.hMyScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hMyScrollBar.BackColor = System.Drawing.Color.Black;
            this.hMyScrollBar.BorderColor = System.Drawing.Color.Silver;
            this.hMyScrollBar.Location = new System.Drawing.Point(0, 27);
            this.hMyScrollBar.Maximum = 100;
            this.hMyScrollBar.Name = "hMyScrollBar";
            this.hMyScrollBar.Orientation = System.Windows.Forms.ScrollOrientation.HorizontalScroll;
            this.hMyScrollBar.Size = new System.Drawing.Size(956, 19);
            this.hMyScrollBar.TabIndex = 9;
            this.hMyScrollBar.Text = "myScrollBar1";
            this.hMyScrollBar.ThumbColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.hMyScrollBar.ThumbSize = 480;
            this.hMyScrollBar.Value = 0;
            this.hMyScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hMyScrollBar_Scroll);
            // 
            // pnRight
            // 
            this.pnRight.Controls.Add(this.vMyScrollBar);
            this.pnRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnRight.Location = new System.Drawing.Point(950, 0);
            this.pnRight.Name = "pnRight";
            this.pnRight.Size = new System.Drawing.Size(19, 610);
            this.pnRight.TabIndex = 12;
            // 
            // vMyScrollBar
            // 
            this.vMyScrollBar.BackColor = System.Drawing.Color.Black;
            this.vMyScrollBar.BorderColor = System.Drawing.Color.Silver;
            this.vMyScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vMyScrollBar.Location = new System.Drawing.Point(0, 0);
            this.vMyScrollBar.Maximum = 100;
            this.vMyScrollBar.Name = "vMyScrollBar";
            this.vMyScrollBar.Orientation = System.Windows.Forms.ScrollOrientation.VerticalScroll;
            this.vMyScrollBar.Size = new System.Drawing.Size(19, 610);
            this.vMyScrollBar.TabIndex = 10;
            this.vMyScrollBar.Text = "myScrollBar1";
            this.vMyScrollBar.ThumbColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.vMyScrollBar.ThumbSize = 305;
            this.vMyScrollBar.Value = 0;
            this.vMyScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vMyScrollBar_Scroll);
            this.vMyScrollBar.Click += new System.EventHandler(this.vMyScrollBar_Click);
            this.vMyScrollBar.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.vMyScrollBar_ControlAdded);
            // 
            // csPrj
            // 
            this.csPrj.AnimationDelay = 20;
            this.csPrj.AnimationStep = 20;
            this.csPrj.BackColor = System.Drawing.Color.Black;
            this.csPrj.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
            this.csPrj.ControlToHide = this.pnTreeView;
            this.csPrj.Dock = System.Windows.Forms.DockStyle.Right;
            this.csPrj.ExpandParentForm = false;
            this.csPrj.Location = new System.Drawing.Point(969, 0);
            this.csPrj.Name = "collapsibleSplitter2";
            this.csPrj.TabIndex = 1;
            this.csPrj.TabStop = false;
            this.csPrj.UseAnimations = false;
            this.csPrj.VisualStyle = NJFLib.Controls.VisualStyles.Mozilla;
            this.csPrj.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.collapsibleSplitter2_SplitterMoved);
            // 
            // pnTreeView
            // 
            this.pnTreeView.Controls.Add(this.pnTreeCenter);
            this.pnTreeView.Controls.Add(this.pnTreeButtom);
            this.pnTreeView.Controls.Add(this.pnTreeRight);
            this.pnTreeView.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnTreeView.Location = new System.Drawing.Point(977, 0);
            this.pnTreeView.Name = "pnTreeView";
            this.pnTreeView.Size = new System.Drawing.Size(184, 610);
            this.pnTreeView.TabIndex = 0;
            // 
            // pnTreeCenter
            // 
            this.pnTreeCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnTreeCenter.Controls.Add(this.pnTtreeViewPrj);
            this.pnTreeCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnTreeCenter.DoubleBuffered = false;
            this.pnTreeCenter.Location = new System.Drawing.Point(0, 0);
            this.pnTreeCenter.Name = "pnTreeCenter";
            this.pnTreeCenter.Size = new System.Drawing.Size(165, 591);
            this.pnTreeCenter.TabIndex = 61;
            this.pnTreeCenter.Paint += new System.Windows.Forms.PaintEventHandler(this.pnTreeCenter_Paint);
            // 
            // pnTtreeViewPrj
            // 
            this.pnTtreeViewPrj.Controls.Add(this.treeViewPrj);
            this.pnTtreeViewPrj.Location = new System.Drawing.Point(1, 2);
            this.pnTtreeViewPrj.Name = "pnTtreeViewPrj";
            this.pnTtreeViewPrj.Size = new System.Drawing.Size(137, 233);
            this.pnTtreeViewPrj.TabIndex = 56;
            // 
            // treeViewPrj
            // 
            this.treeViewPrj.BackColor = System.Drawing.Color.Black;
            this.treeViewPrj.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewPrj.CheckBoxBehaviorMode = Raccoom.Windows.Forms.CheckBoxBehaviorMode.None;
            this.treeViewPrj.DataSource = null;
            this.treeViewPrj.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewPrj.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.treeViewPrj.HideSelection = false;
            this.treeViewPrj.Location = new System.Drawing.Point(0, 0);
            this.treeViewPrj.Name = "treeViewFolderBrowser1";
            this.treeViewPrj.PathSeparator = "/";
            this.treeViewPrj.ShowNodeToolTips = true;
            this.treeViewPrj.Size = new System.Drawing.Size(126, 206);
            this.treeViewPrj.TabIndex = 55;
            this.treeViewPrj.SelectedDirectoriesChanged += new System.EventHandler<Raccoom.Windows.Forms.SelectedDirectoriesChangedEventArgs>(this.treeViewFolderBrowser1_SelectedDirectoriesChanged);
            this.treeViewPrj.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeViewPrj_AfterCollapse);
            this.treeViewPrj.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeViewPrj_AfterExpand);
            this.treeViewPrj.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewPrj_NodeMouseDoubleClick);
            // 
            // pnTreeButtom
            // 
            this.pnTreeButtom.Controls.Add(this.hTreePrjScrollBar);
            this.pnTreeButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnTreeButtom.Location = new System.Drawing.Point(0, 591);
            this.pnTreeButtom.Name = "pnTreeButtom";
            this.pnTreeButtom.Size = new System.Drawing.Size(165, 19);
            this.pnTreeButtom.TabIndex = 60;
            // 
            // hTreePrjScrollBar
            // 
            this.hTreePrjScrollBar.BackColor = System.Drawing.Color.Black;
            this.hTreePrjScrollBar.BorderColor = System.Drawing.Color.Silver;
            this.hTreePrjScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hTreePrjScrollBar.Location = new System.Drawing.Point(0, 0);
            this.hTreePrjScrollBar.Maximum = 100;
            this.hTreePrjScrollBar.Name = "hTreePrjScrollBar";
            this.hTreePrjScrollBar.Orientation = System.Windows.Forms.ScrollOrientation.HorizontalScroll;
            this.hTreePrjScrollBar.Size = new System.Drawing.Size(165, 19);
            this.hTreePrjScrollBar.TabIndex = 56;
            this.hTreePrjScrollBar.Text = "myScrollBar1";
            this.hTreePrjScrollBar.ThumbColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.hTreePrjScrollBar.ThumbSize = 82;
            this.hTreePrjScrollBar.Value = 0;
            this.hTreePrjScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hTreePrjScrollBar_Scroll);
            // 
            // pnTreeRight
            // 
            this.pnTreeRight.Controls.Add(this.vTreePrjScrollBar);
            this.pnTreeRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnTreeRight.Location = new System.Drawing.Point(165, 0);
            this.pnTreeRight.Name = "pnTreeRight";
            this.pnTreeRight.Size = new System.Drawing.Size(19, 610);
            this.pnTreeRight.TabIndex = 59;
            // 
            // vTreePrjScrollBar
            // 
            this.vTreePrjScrollBar.BackColor = System.Drawing.Color.Black;
            this.vTreePrjScrollBar.BorderColor = System.Drawing.Color.Silver;
            this.vTreePrjScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vTreePrjScrollBar.Location = new System.Drawing.Point(0, 0);
            this.vTreePrjScrollBar.Maximum = 100;
            this.vTreePrjScrollBar.Name = "vTreePrjScrollBar";
            this.vTreePrjScrollBar.Orientation = System.Windows.Forms.ScrollOrientation.VerticalScroll;
            this.vTreePrjScrollBar.Size = new System.Drawing.Size(19, 610);
            this.vTreePrjScrollBar.TabIndex = 57;
            this.vTreePrjScrollBar.Text = "myScrollBar2";
            this.vTreePrjScrollBar.ThumbColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.vTreePrjScrollBar.ThumbSize = 305;
            this.vTreePrjScrollBar.Value = 0;
            this.vTreePrjScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vTreePrjScrollBar_Scroll);
            this.vTreePrjScrollBar.Click += new System.EventHandler(this.vTreePrjScrollBar_Click);
            // 
            // GuiConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1162, 646);
            this.Controls.Add(this.msMenu);
            this.Controls.Add(this.cbTitle);
            this.Controls.Add(this.pnAll);
            this.Name = "GuiConsole";
            this.Text = "GuiConsole";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GuiConsole_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GuiConsole_FormClosed);
            this.Load += new System.EventHandler(this.GuiConsole_Load);
            this.ResizeBegin += new System.EventHandler(this.GuiConsole_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.GuiConsole_ResizeEnd);
            this.LocationChanged += new System.EventHandler(this.GuiConsole_LocationChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GuiConsole_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GuiConsole_MouseUp);
            this.Move += new System.EventHandler(this.GuiConsole_Move);
            this.Resize += new System.EventHandler(this.GuiConsole_Resize);
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.cmMain.ResumeLayout(false);
            this.pnAll.ResumeLayout(false);
            this.pnCenter.ResumeLayout(false);
            this.pnCenter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctbConsole)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnRight.ResumeLayout(false);
            this.pnTreeView.ResumeLayout(false);
            this.pnTreeCenter.ResumeLayout(false);
            this.pnTtreeViewPrj.ResumeLayout(false);
            this.pnTreeButtom.ResumeLayout(false);
            this.pnTreeRight.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CwFCTB fctbConsole = null;
        private System.Windows.Forms.TextBox textBox1;
        private CwScrollBar hMyScrollBar;
        private CwScrollBar vMyScrollBar;
        private System.Windows.Forms.MenuStrip msMenu;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCwcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem lauchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nothingToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sanitizeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem stopBuildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem afterFileErrorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Build;
        private System.Windows.Forms.ToolStripMenuItem iDEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notePadToolStripMenuItem;
        private System.Windows.Forms.TextBox cbTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        protected System.Windows.Forms.ContextMenuStrip cmMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private cwc.CwTreeView treeViewPrj;
        private System.Windows.Forms.Panel pnAll;
        private NJFLib.Controls.CollapsibleSplitter csPrj;
        private System.Windows.Forms.Panel pnTreeView;
        private System.Windows.Forms.Panel pnRight;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pnCenter;
        private CwScrollBar vTreePrjScrollBar;
        private CwScrollBar hTreePrjScrollBar;
        private DoubleBufferedPanel pnTreeCenter;
        private System.Windows.Forms.Panel pnTreeButtom;
        private System.Windows.Forms.Panel pnTreeRight;
        private DoubleBufferedPanel pnTtreeViewPrj;
        private System.Windows.Forms.ToolStripMenuItem liteWayvToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tODOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem debugTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sanitizerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
    }
}