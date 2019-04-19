using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace cwc
{
     
 public class ComboBoxCustom : ComboBox
    {
        public ComboBoxCustom()
        {
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            if (e.Index < 0) { return; }
            e.DrawBackground();
            ComboBoxItem item = (ComboBoxItem)this.Items[e.Index];
            Brush brush = new SolidBrush(item.ForeColor);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {// brush = Brushes.Yellow;
            }
            e.Graphics.DrawString(item.Text,   this.Font, brush, e.Bounds.X, e.Bounds.Y);
        }
        object selectedValue = null;
        public new Object SelectedValue
        {
            get
            {
                object ret = null;
                if (this.SelectedIndex >= 0) { 
                    ret = ((ComboBoxItem)this.SelectedItem).Value; 
                }
                return ret;
            }
            set { selectedValue = value; }
        }
        string selectedText = "";
        public new String SelectedText
        {
            get
            {
                return ((ComboBoxItem)this.SelectedItem).Text;
            }
            set { selectedText = value; }
        }
    }
    public class ComboBoxItem
    {
        public ComboBoxItem() { }

        public ComboBoxItem(string pText, object pValue)
        {
            text = pText; val = pValue;
        }

        public ComboBoxItem(string pText, object pValue, Color pColor)
        {
            text = pText; val = pValue; foreColor = pColor;
        }

        string text = "";
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        object val;
        public object Value
        {
            get { return val; }
            set { val = value; }
        }

        Color foreColor = Color.Black;
        public Color ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        public override string ToString()
        {
            return text;
        }
    }
    
    partial class MainForm
    {
        private System.Windows.Forms.TextBox tbWorkingDir;
        public System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button exploreButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ColumnHeader versionHeader;
        private System.Windows.Forms.ColumnHeader statusHeader;
        private System.Windows.Forms.ColumnHeader infoHeader;
        private System.Windows.Forms.ColumnHeader lineHeader;
        private System.Windows.Forms.ColumnHeader colHeader;
        private System.Windows.Forms.ColumnHeader nameHeader;
        private System.Windows.Forms.ColumnHeader descHeader;
        private System.Windows.Forms.ColumnHeader typeHeader;

        #region Windows Form Designer Generated Code




        /// <summary>
   //  this.listView = new ListViewDoubleBuffer();  
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbWorkingDir = new System.Windows.Forms.TextBox();
            this.exploreButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.nameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.versionHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbCompiler = new System.Windows.Forms.ComboBox();
            this.cbArchiteture = new System.Windows.Forms.ComboBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.cbPlatform = new System.Windows.Forms.ComboBox();
            this.btnWorkingDir = new System.Windows.Forms.Button();
            this.lblCommand = new System.Windows.Forms.Label();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.btnOutputExplore = new System.Windows.Forms.Button();
            this.btOutputSet = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnSanitize = new System.Windows.Forms.Button();
            this.msMenu = new System.Windows.Forms.MenuStrip();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setCwcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cwCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gZEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.libRTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.emscriptenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildAndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nothingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sanitizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workingDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cbView = new System.Windows.Forms.ComboBox();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.btnExpand = new System.Windows.Forms.Button();
            this.btnLauch = new System.Windows.Forms.Button();
            this.cbBuildType = new System.Windows.Forms.ComboBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.cbCommands = new cwc.ComboBoxCustom();
            this.listView = new cwc.ListViewDoubleBuffer();
            this.infoHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lineHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.descHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip.SuspendLayout();
            this.msMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.Black;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.lblVersion});
            this.statusStrip.Location = new System.Drawing.Point(0, 557);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(959, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip";
            this.statusStrip.Visible = false;
            // 
            // statusLabel
            // 
            this.statusLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(894, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "No items selected.";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Black;
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(50, 17);
            this.lblVersion.Text = "vX.X.X.X";
            // 
            // tbWorkingDir
            // 
            this.tbWorkingDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWorkingDir.Enabled = false;
            this.tbWorkingDir.Location = new System.Drawing.Point(5, 29);
            this.tbWorkingDir.Name = "tbWorkingDir";
            this.tbWorkingDir.ReadOnly = true;
            this.tbWorkingDir.Size = new System.Drawing.Size(955, 20);
            this.tbWorkingDir.TabIndex = 2;
            this.tbWorkingDir.Visible = false;
            this.tbWorkingDir.TextChanged += new System.EventHandler(this.tbWorkingDir_TextChanged);
            // 
            // exploreButton
            // 
            this.exploreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exploreButton.Font = new System.Drawing.Font("Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exploreButton.Location = new System.Drawing.Point(906, 68);
            this.exploreButton.Name = "exploreButton";
            this.exploreButton.Size = new System.Drawing.Size(54, 17);
            this.exploreButton.TabIndex = 3;
            this.exploreButton.Text = "Explore";
            this.exploreButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.exploreButton.UseVisualStyleBackColor = true;
            this.exploreButton.Visible = false;
            this.exploreButton.Click += new System.EventHandler(this.ExploreButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Enabled = false;
            this.cancelButton.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cancelButton.Location = new System.Drawing.Point(890, 29);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(68, 24);
            this.cancelButton.TabIndex = 15;
            this.cancelButton.Text = "Stop";
            this.cancelButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Visible = false;
            this.cancelButton.EnabledChanged += new System.EventHandler(this.cancelButton_EnabledChanged);
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // cbCompiler
            // 
            this.cbCompiler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCompiler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCompiler.Enabled = false;
            this.cbCompiler.FormattingEnabled = true;
            this.cbCompiler.Location = new System.Drawing.Point(586, 445);
            this.cbCompiler.Name = "cbCompiler";
            this.cbCompiler.Size = new System.Drawing.Size(146, 21);
            this.cbCompiler.TabIndex = 18;
            this.cbCompiler.Visible = false;
            this.cbCompiler.SelectedIndexChanged += new System.EventHandler(this.cbCompiler_SelectedIndexChanged);
            // 
            // cbArchiteture
            // 
            this.cbArchiteture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbArchiteture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbArchiteture.Enabled = false;
            this.cbArchiteture.FormattingEnabled = true;
            this.cbArchiteture.Items.AddRange(new object[] {
            " x32",
            " x64"});
            this.cbArchiteture.Location = new System.Drawing.Point(906, 172);
            this.cbArchiteture.Name = "cbArchiteture";
            this.cbArchiteture.Size = new System.Drawing.Size(53, 21);
            this.cbArchiteture.TabIndex = 19;
            this.cbArchiteture.Visible = false;
            this.cbArchiteture.SelectedIndexChanged += new System.EventHandler(this.cbArchiteture_SelectedIndexChanged);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(892, 135);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(33, 23);
            this.btnEdit.TabIndex = 20;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // cbPlatform
            // 
            this.cbPlatform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlatform.Enabled = false;
            this.cbPlatform.FormattingEnabled = true;
            this.cbPlatform.Location = new System.Drawing.Point(810, 234);
            this.cbPlatform.Name = "cbPlatform";
            this.cbPlatform.Size = new System.Drawing.Size(125, 21);
            this.cbPlatform.TabIndex = 21;
            this.cbPlatform.Visible = false;
            this.cbPlatform.SelectedIndexChanged += new System.EventHandler(this.cbPlatform_SelectedIndexChanged);
            // 
            // btnWorkingDir
            // 
            this.btnWorkingDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWorkingDir.Font = new System.Drawing.Font("Consolas", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWorkingDir.Location = new System.Drawing.Point(880, 68);
            this.btnWorkingDir.Name = "btnWorkingDir";
            this.btnWorkingDir.Size = new System.Drawing.Size(27, 17);
            this.btnWorkingDir.TabIndex = 29;
            this.btnWorkingDir.Text = "...";
            this.btnWorkingDir.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnWorkingDir.UseVisualStyleBackColor = true;
            this.btnWorkingDir.Visible = false;
            this.btnWorkingDir.Click += new System.EventHandler(this.btnWorkingDir_Click);
            // 
            // lblCommand
            // 
            this.lblCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.lblCommand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCommand.Location = new System.Drawing.Point(931, 28);
            this.lblCommand.Name = "lblCommand";
            this.lblCommand.Size = new System.Drawing.Size(23, 21);
            this.lblCommand.TabIndex = 34;
            this.lblCommand.Text = "0";
            this.lblCommand.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCommand.Click += new System.EventHandler(this.lblCommand_Click);
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.Enabled = false;
            this.tbOutput.Location = new System.Drawing.Point(0, 526);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Size = new System.Drawing.Size(960, 20);
            this.tbOutput.TabIndex = 37;
            this.tbOutput.Visible = false;
            this.tbOutput.TextChanged += new System.EventHandler(this.tbOutput_TextChanged);
            // 
            // btnOutputExplore
            // 
            this.btnOutputExplore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOutputExplore.Font = new System.Drawing.Font("Consolas", 6.75F);
            this.btnOutputExplore.Location = new System.Drawing.Point(906, 509);
            this.btnOutputExplore.Name = "btnOutputExplore";
            this.btnOutputExplore.Size = new System.Drawing.Size(54, 17);
            this.btnOutputExplore.TabIndex = 38;
            this.btnOutputExplore.Text = "Explore";
            this.btnOutputExplore.UseVisualStyleBackColor = true;
            this.btnOutputExplore.Visible = false;
            this.btnOutputExplore.Click += new System.EventHandler(this.btnOutputExplore_Click);
            // 
            // btOutputSet
            // 
            this.btOutputSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOutputSet.Font = new System.Drawing.Font("Consolas", 6F, System.Drawing.FontStyle.Bold);
            this.btOutputSet.Location = new System.Drawing.Point(880, 509);
            this.btOutputSet.Name = "btOutputSet";
            this.btOutputSet.Size = new System.Drawing.Size(27, 17);
            this.btOutputSet.TabIndex = 39;
            this.btOutputSet.Text = "...";
            this.btOutputSet.UseVisualStyleBackColor = true;
            this.btOutputSet.Visible = false;
            this.btOutputSet.Click += new System.EventHandler(this.btOutputSet_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Enabled = false;
            this.btnPlay.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlay.Location = new System.Drawing.Point(873, 440);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(87, 28);
            this.btnPlay.TabIndex = 40;
            this.btnPlay.Text = "Run";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Visible = false;
            this.btnPlay.EnabledChanged += new System.EventHandler(this.btnPlay_EnabledChanged);
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnSanitize
            // 
            this.btnSanitize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSanitize.Enabled = false;
            this.btnSanitize.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSanitize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.btnSanitize.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSanitize.Location = new System.Drawing.Point(782, 440);
            this.btnSanitize.Name = "btnSanitize";
            this.btnSanitize.Size = new System.Drawing.Size(93, 28);
            this.btnSanitize.TabIndex = 41;
            this.btnSanitize.Text = "Sanitize";
            this.btnSanitize.UseVisualStyleBackColor = true;
            this.btnSanitize.Visible = false;
            this.btnSanitize.EnabledChanged += new System.EventHandler(this.btnSanitize_EnabledChanged);
            this.btnSanitize.Click += new System.EventHandler(this.btnSanitize_Click);
            // 
            // msMenu
            // 
            this.msMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.msMenu.BackColor = System.Drawing.Color.Black;
            this.msMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.msMenu.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msMenu.GripMargin = new System.Windows.Forms.Padding(0, 2, 2, 2);
            this.msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.updateToolStripMenuItem,
            this.viewInToolStripMenuItem,
            this.buildAndToolStripMenuItem,
            this.pathToolStripMenuItem,
            this.aaToolStripMenuItem1});
            this.msMenu.Location = new System.Drawing.Point(493, 0);
            this.msMenu.Name = "msMenu";
            this.msMenu.Size = new System.Drawing.Size(373, 26);
            this.msMenu.TabIndex = 48;
            this.msMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.msMenu_ItemClicked);
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
            this.configToolStripMenuItem.MouseHover += new System.EventHandler(this.configToolStripMenuItem_MouseHover);
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
            this.gZEToolStripMenuItem,
            this.libRTToolStripMenuItem,
            this.demosToolStripMenuItem,
            this.toolStripSeparator2,
            this.emscriptenToolStripMenuItem});
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.RightToLeftAutoMirrorImage = true;
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
            this.updateToolStripMenuItem.Text = "Update";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(141, 6);
            // 
            // cwCToolStripMenuItem
            // 
            this.cwCToolStripMenuItem.Name = "cwCToolStripMenuItem";
            this.cwCToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.cwCToolStripMenuItem.Text = "Cwc";
            this.cwCToolStripMenuItem.Click += new System.EventHandler(this.cwCToolStripMenuItem_Click);
            // 
            // gZEToolStripMenuItem
            // 
            this.gZEToolStripMenuItem.Name = "gZEToolStripMenuItem";
            this.gZEToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.gZEToolStripMenuItem.Text = "GZE";
            this.gZEToolStripMenuItem.Click += new System.EventHandler(this.gZEToolStripMenuItem_Click);
            // 
            // libRTToolStripMenuItem
            // 
            this.libRTToolStripMenuItem.Name = "libRTToolStripMenuItem";
            this.libRTToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.libRTToolStripMenuItem.Text = "LibRT";
            this.libRTToolStripMenuItem.Click += new System.EventHandler(this.libRTToolStripMenuItem_Click);
            // 
            // demosToolStripMenuItem
            // 
            this.demosToolStripMenuItem.Name = "demosToolStripMenuItem";
            this.demosToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.demosToolStripMenuItem.Text = "Demos";
            this.demosToolStripMenuItem.Click += new System.EventHandler(this.demosToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(141, 6);
            // 
            // emscriptenToolStripMenuItem
            // 
            this.emscriptenToolStripMenuItem.Name = "emscriptenToolStripMenuItem";
            this.emscriptenToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.emscriptenToolStripMenuItem.Text = "Emscripten";
            this.emscriptenToolStripMenuItem.Click += new System.EventHandler(this.emscriptenToolStripMenuItem_Click);
            // 
            // viewInToolStripMenuItem
            // 
            this.viewInToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.viewInToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultToolStripMenuItem});
            this.viewInToolStripMenuItem.Name = "viewInToolStripMenuItem";
            this.viewInToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            this.viewInToolStripMenuItem.Text = "View In";
            this.viewInToolStripMenuItem.CheckedChanged += new System.EventHandler(this.viewInToolStripMenuItem_CheckedChanged);
            this.viewInToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.viewInToolStripMenuItem_CheckStateChanged);
            this.viewInToolStripMenuItem.Click += new System.EventHandler(this.viewInToolStripMenuItem_Click);
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
            // buildAndToolStripMenuItem
            // 
            this.buildAndToolStripMenuItem.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.buildAndToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nothingToolStripMenuItem,
            this.runToolStripMenuItem,
            this.sanitizeToolStripMenuItem});
            this.buildAndToolStripMenuItem.Name = "buildAndToolStripMenuItem";
            this.buildAndToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            this.buildAndToolStripMenuItem.Text = "Build &&";
            this.buildAndToolStripMenuItem.Click += new System.EventHandler(this.buildAndToolStripMenuItem_Click);
            // 
            // nothingToolStripMenuItem
            // 
            this.nothingToolStripMenuItem.Name = "nothingToolStripMenuItem";
            this.nothingToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.nothingToolStripMenuItem.Text = "Nothing";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Checked = true;
            this.runToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // sanitizeToolStripMenuItem
            // 
            this.sanitizeToolStripMenuItem.Name = "sanitizeToolStripMenuItem";
            this.sanitizeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.sanitizeToolStripMenuItem.Text = "Sanitize";
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
            // aaToolStripMenuItem1
            // 
            this.aaToolStripMenuItem1.BackColor = System.Drawing.Color.Teal;
            this.aaToolStripMenuItem1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aaToolStripMenuItem1.Name = "aaToolStripMenuItem1";
            this.aaToolStripMenuItem1.Size = new System.Drawing.Size(60, 22);
            this.aaToolStripMenuItem1.Text = "BUILD";
            this.aaToolStripMenuItem1.Click += new System.EventHandler(this.aaToolStripMenuItem1_Click);
            // 
            // cbView
            // 
            this.cbView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbView.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbView.FormattingEnabled = true;
            this.cbView.Items.AddRange(new object[] {
            "(View In)"});
            this.cbView.Location = new System.Drawing.Point(824, 261);
            this.cbView.Name = "cbView";
            this.cbView.Size = new System.Drawing.Size(103, 22);
            this.cbView.TabIndex = 49;
            this.cbView.Visible = false;
            this.cbView.SelectedIndexChanged += new System.EventHandler(this.cbView_SelectedIndexChanged);
            // 
            // hScrollBar
            // 
            this.hScrollBar.Location = new System.Drawing.Point(333, 399);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(80, 17);
            this.hScrollBar.TabIndex = 54;
            this.hScrollBar.Visible = false;
            // 
            // vScrollBar
            // 
            this.vScrollBar.Location = new System.Drawing.Point(396, 319);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 80);
            this.vScrollBar.TabIndex = 55;
            this.vScrollBar.Visible = false;
            // 
            // btnExpand
            // 
            this.btnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpand.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpand.Location = new System.Drawing.Point(783, 211);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(35, 72);
            this.btnExpand.TabIndex = 56;
            this.btnExpand.Text = "<<";
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Visible = false;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // btnLauch
            // 
            this.btnLauch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLauch.Font = new System.Drawing.Font("Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLauch.Location = new System.Drawing.Point(810, 71);
            this.btnLauch.Name = "btnLauch";
            this.btnLauch.Size = new System.Drawing.Size(45, 20);
            this.btnLauch.TabIndex = 57;
            this.btnLauch.Text = "Lauch";
            this.btnLauch.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLauch.UseVisualStyleBackColor = true;
            this.btnLauch.Visible = false;
            this.btnLauch.Click += new System.EventHandler(this.btnLauch_Click);
            // 
            // cbBuildType
            // 
            this.cbBuildType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbBuildType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuildType.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBuildType.FormattingEnabled = true;
            this.cbBuildType.Items.AddRange(new object[] {
            "(Build And)",
            "Nothing",
            "Run",
            "Sanitize"});
            this.cbBuildType.Location = new System.Drawing.Point(860, 319);
            this.cbBuildType.Name = "cbBuildType";
            this.cbBuildType.Size = new System.Drawing.Size(67, 22);
            this.cbBuildType.TabIndex = 58;
            this.cbBuildType.Visible = false;
            this.cbBuildType.SelectedIndexChanged += new System.EventHandler(this.cbBuildType_SelectedIndexChanged_1);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(-1, 557);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(959, 5);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 59;
            this.progressBar.Visible = false;
            this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // cbCommands
            // 
            this.cbCommands.AllowDrop = true;
            this.cbCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCommands.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbCommands.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCommands.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbCommands.DropDownWidth = 100;
            this.cbCommands.FormattingEnabled = true;
            this.cbCommands.Location = new System.Drawing.Point(9, 118);
            this.cbCommands.MaxDropDownItems = 30;
            this.cbCommands.Name = "cbCommands";
            this.cbCommands.Size = new System.Drawing.Size(956, 21);
            this.cbCommands.TabIndex = 30;
            this.cbCommands.Visible = false;
            this.cbCommands.SelectedIndexChanged += new System.EventHandler(this.cbCommands_SelectedIndexChanged);
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.infoHeader,
            this.lineHeader,
            this.colHeader,
            this.descHeader});
            this.listView.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView.Location = new System.Drawing.Point(55, 135);
            this.listView.Name = "listView";
            this.listView.OwnerDraw = true;
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(763, 293);
            this.listView.TabIndex = 4;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.Visible = false;
            this.listView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.ListViewDrawColumnHeader);
            this.listView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListViewDrawItem);
            this.listView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.ListViewDrawSubItem);
            this.listView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListViewItemCheck);
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.DoubleClick += new System.EventHandler(this.ListViewClick);
            this.listView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listView_KeyPress);
            this.listView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listView_KeyUp);
            this.listView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListViewMouseMove);
            // 
            // infoHeader
            // 
            this.infoHeader.Text = " !";
            this.infoHeader.Width = 26;
            // 
            // lineHeader
            // 
            this.lineHeader.Text = " L";
            this.lineHeader.Width = 26;
            // 
            // colHeader
            // 
            this.colHeader.Text = " C";
            this.colHeader.Width = 28;
            // 
            // descHeader
            // 
            this.descHeader.Text = "Description";
            this.descHeader.Width = 2019;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(959, 579);
            this.Controls.Add(this.cbBuildType);
            this.Controls.Add(this.btnLauch);
            this.Controls.Add(this.btnExpand);
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.hScrollBar);
            this.Controls.Add(this.btOutputSet);
            this.Controls.Add(this.cbView);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.btnSanitize);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnOutputExplore);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.exploreButton);
            this.Controls.Add(this.btnWorkingDir);
            this.Controls.Add(this.tbWorkingDir);
            this.Controls.Add(this.cbCommands);
            this.Controls.Add(this.cbPlatform);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.cbArchiteture);
            this.Controls.Add(this.cbCompiler);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.msMenu);
            this.Controls.Add(this.lblCommand);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.statusStrip);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CWC";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MainFormHelpButtonClicked);
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.Click += new System.EventHandler(this.MainForm_Click);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.MainFormHelpRequested);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseWheel);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.msMenu.ResumeLayout(false);
            this.msMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListViewDoubleBuffer listView;
        private ComboBox cbCompiler;
        private ComboBox cbArchiteture;
        private Button btnEdit;
        public ComboBox cbPlatform;
        private Button btnWorkingDir;
        private Label lblCommand;
        private ComboBoxCustom cbCommands;
        private TextBox tbOutput;
        private Button btnOutputExplore;
        private Button btOutputSet;
        public Button btnPlay;
        public Button btnSanitize;
        private ToolStripStatusLabel lblVersion;
		private MenuStrip msMenu;
		private ToolStripMenuItem configToolStripMenuItem;
		private ToolStripMenuItem setCwcToolStripMenuItem;
		private ToolStripMenuItem updateToolStripMenuItem;
		private ToolStripMenuItem cwCToolStripMenuItem;
		private ToolStripMenuItem libRTToolStripMenuItem;
		private ToolStripMenuItem demosToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem emscriptenToolStripMenuItem;
		public ComboBox cbView;
		private ToolStripMenuItem gZEToolStripMenuItem;
		private HScrollBar hScrollBar;
		private VScrollBar vScrollBar;
		private Button btnExpand;
		private ToolStripSeparator toolStripSeparator1;
		private Button btnLauch;
        private ComboBox cbBuildType;
        private ToolStripMenuItem viewInToolStripMenuItem;
        private ToolStripMenuItem defaultToolStripMenuItem;
        private ToolStripMenuItem buildAndToolStripMenuItem;
        private ToolStripMenuItem nothingToolStripMenuItem;
        private ToolStripMenuItem runToolStripMenuItem;
        private ToolStripMenuItem sanitizeToolStripMenuItem;
        private ToolStripMenuItem pathToolStripMenuItem;
        private ToolStripMenuItem workingDirToolStripMenuItem;
        private ToolStripMenuItem outputToolStripMenuItem;
        private ToolStripMenuItem aaToolStripMenuItem1;
        private ProgressBar progressBar;
    }





/* ASSSSSSSSSSSSSSSSSSS mouse lag
	public static class MouseHook
    {
        public static event EventHandler MouseAction = delegate { };

        public static void Start()
        {
            _hookID = SetHook(_proc);


        }
        public static void stop()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                  GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
          int nCode, IntPtr wParam, IntPtr lParam) {


            if (nCode >= 0 && MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseAction(null, new EventArgs());
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
          LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
          IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


    }*/

}
