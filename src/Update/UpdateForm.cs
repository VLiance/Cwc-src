using System;

using System.Drawing;

using System.Windows.Forms;
using System.ComponentModel;


using cwc.Update;
using System.Collections.Generic;
using System.IO;
using cwc.Utilities;
using System.Diagnostics;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using MarkdownSharp;
using static cwc.ModuleData;
using System.Runtime.InteropServices;

namespace cwc
{



    public class UpdateForm: Form
    {

       




        /*
   Dictionary<string, string> aURL = new Dictionary<string, string>() {
        { "LibRT", "updater.simacode.com/CWC/LibRT/Files.php"},
        { "Demo", "updater.simacode.com/CWC/Demo/Files.php"},
    };
   Dictionary<string, string> aDesc = new Dictionary<string, string>() {
        { "LibRT", "LibRT is a bundle of C++ compilers and custom C++ sources to build portable app"},
        { "Demo", "Some demos to test Cwc"},
    };

        */


     //   public static UpdateLibRT updateDialog;
        public static Boolean silentCheck = false;
   //     public static bool bInitilised = false;

            private Form oParent = null; 


        public new Button btnDownload;
        private ComboBox cbListVersion;
        private ProgressBar pbExtract;
        private StatusStrip sbStatus;
        private ToolStripStatusLabel lblStatus;
        private Button exploreButton;
        private TextBox tbModuleDir;
        private Label pathLabel;
        public new Label lblOS_Architeture;
		private Label lblTxtCurr;
		private Label lblCurrVer;
		private Label lblTxtLast;
		private Label lblLastVer;
		private WebBrowser wbReadme;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private WebBrowser wbChanglog;
		private TabPage tabPage3;
		private WebBrowser wbLicence;
		private bool bExecuteAfter;

       public string sModuleName = "";

        public ModuleLink oModuleToUpdate;
		private ProgressBar pbDownload;
		private ToolStripStatusLabel lblDetail;
		private ComboBox cbModule;
		private Label label1;
		public Button btnUpdateAll;
		private Label label2;
		private ProgressBar pbAll;
        private CheckBox cbAutoclose;
        public ModuleData oModule = null;


		public UpdateForm(string _sModuleName,  bool _bExecuteAfter = false)
        {

			bExecuteAfter = _bExecuteAfter;

            sModuleName = _sModuleName;

	//		oModule = aMData[sModuleName];

///this.Owner = Globals.MainForm;
        ////    this.Font = Globals.Settings.DefaultFont;
            this.InitializeComponent();
            this.ApplyLocalizedTexts();

     //       lblTitle.Text = sModuleName;
            
     //       ScaleHelper.AdjustForHighDPI(this);
        }

        #region Windows Form Designer Generated Code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
            this.btnDownload = new System.Windows.Forms.Button();
            this.lblOS_Architeture = new System.Windows.Forms.Label();
            this.cbListVersion = new System.Windows.Forms.ComboBox();
            this.pbExtract = new System.Windows.Forms.ProgressBar();
            this.sbStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDetail = new System.Windows.Forms.ToolStripStatusLabel();
            this.exploreButton = new System.Windows.Forms.Button();
            this.tbModuleDir = new System.Windows.Forms.TextBox();
            this.pathLabel = new System.Windows.Forms.Label();
            this.lblTxtCurr = new System.Windows.Forms.Label();
            this.lblCurrVer = new System.Windows.Forms.Label();
            this.lblTxtLast = new System.Windows.Forms.Label();
            this.lblLastVer = new System.Windows.Forms.Label();
            this.wbReadme = new System.Windows.Forms.WebBrowser();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.wbChanglog = new System.Windows.Forms.WebBrowser();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.wbLicence = new System.Windows.Forms.WebBrowser();
            this.pbDownload = new System.Windows.Forms.ProgressBar();
            this.cbModule = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUpdateAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pbAll = new System.Windows.Forms.ProgressBar();
            this.cbAutoclose = new System.Windows.Forms.CheckBox();
            this.sbStatus.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.SlateGray;
            this.btnDownload.Enabled = false;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.Location = new System.Drawing.Point(385, 99);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(89, 36);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "&Download";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.DownloadButtonClick);
            // 
            // lblOS_Architeture
            // 
            this.lblOS_Architeture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOS_Architeture.Font = new System.Drawing.Font("Candara", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOS_Architeture.Location = new System.Drawing.Point(-23, 4);
            this.lblOS_Architeture.Name = "lblOS_Architeture";
            this.lblOS_Architeture.Size = new System.Drawing.Size(163, 22);
            this.lblOS_Architeture.TabIndex = 20;
            this.lblOS_Architeture.Text = "Current Platform xXX";
            this.lblOS_Architeture.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbListVersion
            // 
            this.cbListVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbListVersion.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbListVersion.FormattingEnabled = true;
            this.cbListVersion.Location = new System.Drawing.Point(201, 111);
            this.cbListVersion.Name = "cbListVersion";
            this.cbListVersion.Size = new System.Drawing.Size(180, 24);
            this.cbListVersion.TabIndex = 44;
            this.cbListVersion.SelectedIndexChanged += new System.EventHandler(this.cbListVersion_SelectedIndexChanged);
            // 
            // pbExtract
            // 
            this.pbExtract.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbExtract.Location = new System.Drawing.Point(0, 671);
            this.pbExtract.Name = "pbExtract";
            this.pbExtract.Size = new System.Drawing.Size(1008, 10);
            this.pbExtract.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbExtract.TabIndex = 48;
            this.pbExtract.Click += new System.EventHandler(this.pbBar_Click);
            // 
            // sbStatus
            // 
            this.sbStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblDetail});
            this.sbStatus.Location = new System.Drawing.Point(0, 680);
            this.sbStatus.Name = "sbStatus";
            this.sbStatus.Size = new System.Drawing.Size(1008, 22);
            this.sbStatus.TabIndex = 50;
            this.sbStatus.Text = "sbText";
            this.sbStatus.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.sbStatus_ItemClicked);
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.LightGray;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(51, 17);
            this.lblStatus.Text = "Pending";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // lblDetail
            // 
            this.lblDetail.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Size = new System.Drawing.Size(25, 17);
            this.lblDetail.Text = "      ";
            // 
            // exploreButton
            // 
            this.exploreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exploreButton.Location = new System.Drawing.Point(949, 59);
            this.exploreButton.Name = "exploreButton";
            this.exploreButton.Size = new System.Drawing.Size(64, 39);
            this.exploreButton.TabIndex = 54;
            this.exploreButton.Text = "Explore";
            this.exploreButton.UseVisualStyleBackColor = true;
            this.exploreButton.Click += new System.EventHandler(this.exploreButton_Click);
            // 
            // tbModuleDir
            // 
            this.tbModuleDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbModuleDir.Enabled = false;
            this.tbModuleDir.Location = new System.Drawing.Point(0, 69);
            this.tbModuleDir.Name = "tbModuleDir";
            this.tbModuleDir.ReadOnly = true;
            this.tbModuleDir.Size = new System.Drawing.Size(956, 20);
            this.tbModuleDir.TabIndex = 53;
            this.tbModuleDir.TextChanged += new System.EventHandler(this.tbModuleDir_TextChanged);
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathLabel.Location = new System.Drawing.Point(12, 59);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(46, 12);
            this.pathLabel.TabIndex = 52;
            this.pathLabel.Text = "Directory:";
            this.pathLabel.Click += new System.EventHandler(this.pathLabel_Click);
            // 
            // lblTxtCurr
            // 
            this.lblTxtCurr.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTxtCurr.Location = new System.Drawing.Point(505, 110);
            this.lblTxtCurr.Name = "lblTxtCurr";
            this.lblTxtCurr.Size = new System.Drawing.Size(89, 25);
            this.lblTxtCurr.TabIndex = 57;
            this.lblTxtCurr.Text = "Current:";
            this.lblTxtCurr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTxtCurr.Click += new System.EventHandler(this.lblTxtCurr_Click);
            // 
            // lblCurrVer
            // 
            this.lblCurrVer.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrVer.Location = new System.Drawing.Point(592, 110);
            this.lblCurrVer.Name = "lblCurrVer";
            this.lblCurrVer.Size = new System.Drawing.Size(325, 25);
            this.lblCurrVer.TabIndex = 56;
            this.lblCurrVer.Text = "Unknow";
            this.lblCurrVer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCurrVer.Click += new System.EventHandler(this.lblCurrVer_Click);
            // 
            // lblTxtLast
            // 
            this.lblTxtLast.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTxtLast.Location = new System.Drawing.Point(508, 91);
            this.lblTxtLast.Name = "lblTxtLast";
            this.lblTxtLast.Size = new System.Drawing.Size(86, 25);
            this.lblTxtLast.TabIndex = 59;
            this.lblTxtLast.Text = "Last:";
            this.lblTxtLast.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTxtLast.Click += new System.EventHandler(this.lblTxtLast_Click);
            // 
            // lblLastVer
            // 
            this.lblLastVer.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastVer.Location = new System.Drawing.Point(592, 91);
            this.lblLastVer.Name = "lblLastVer";
            this.lblLastVer.Size = new System.Drawing.Size(325, 25);
            this.lblLastVer.TabIndex = 58;
            this.lblLastVer.Text = "Unknow";
            this.lblLastVer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLastVer.Click += new System.EventHandler(this.lblGitLastVer_Click);
            // 
            // wbReadme
            // 
            this.wbReadme.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbReadme.Location = new System.Drawing.Point(0, 2);
            this.wbReadme.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbReadme.Name = "wbReadme";
            this.wbReadme.ScriptErrorsSuppressed = true;
            this.wbReadme.Size = new System.Drawing.Size(1009, 545);
            this.wbReadme.TabIndex = 62;
            this.wbReadme.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbReadme_DocumentCompleted);
            this.wbReadme.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.wbInfo_Navigating);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(-4, 114);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1017, 555);
            this.tabControl1.TabIndex = 64;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Black;
            this.tabPage1.Controls.Add(this.wbReadme);
            this.tabPage1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1009, 529);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Readme";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.wbChanglog);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1009, 529);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Changelog";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // wbChanglog
            // 
            this.wbChanglog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbChanglog.Location = new System.Drawing.Point(0, -2);
            this.wbChanglog.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbChanglog.Name = "wbChanglog";
            this.wbChanglog.ScriptErrorsSuppressed = true;
            this.wbChanglog.Size = new System.Drawing.Size(1009, 533);
            this.wbChanglog.TabIndex = 63;
            this.wbChanglog.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbChanglog_DocumentCompleted);
            this.wbChanglog.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.wbInfo_Navigating);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.wbLicence);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1009, 529);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Licence";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // wbLicence
            // 
            this.wbLicence.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbLicence.Location = new System.Drawing.Point(0, -2);
            this.wbLicence.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbLicence.Name = "wbLicence";
            this.wbLicence.ScriptErrorsSuppressed = true;
            this.wbLicence.Size = new System.Drawing.Size(1009, 533);
            this.wbLicence.TabIndex = 63;
            this.wbLicence.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.wbInfo_Navigating);
            // 
            // pbDownload
            // 
            this.pbDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDownload.Location = new System.Drawing.Point(0, 663);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(1008, 10);
            this.pbDownload.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbDownload.TabIndex = 65;
            // 
            // cbModule
            // 
            this.cbModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModule.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbModule.FormattingEnabled = true;
            this.cbModule.Location = new System.Drawing.Point(78, 27);
            this.cbModule.Name = "cbModule";
            this.cbModule.Size = new System.Drawing.Size(180, 24);
            this.cbModule.TabIndex = 66;
            this.cbModule.SelectedIndexChanged += new System.EventHandler(this.cbModule_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 67;
            this.label1.Text = "Modules:";
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.BackColor = System.Drawing.Color.SlateGray;
            this.btnUpdateAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUpdateAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateAll.Location = new System.Drawing.Point(264, 22);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(126, 29);
            this.btnUpdateAll.TabIndex = 68;
            this.btnUpdateAll.Text = "&Update All (1)";
            this.btnUpdateAll.UseVisualStyleBackColor = false;
            this.btnUpdateAll.Visible = false;
            this.btnUpdateAll.Click += new System.EventHandler(this.btnUpdateAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(199, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 12);
            this.label2.TabIndex = 69;
            this.label2.Text = "Version:";
            // 
            // pbAll
            // 
            this.pbAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbAll.Location = new System.Drawing.Point(-1, -4);
            this.pbAll.Name = "pbAll";
            this.pbAll.Size = new System.Drawing.Size(1013, 10);
            this.pbAll.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbAll.TabIndex = 70;
            this.pbAll.Click += new System.EventHandler(this.pbAll_Click);
            // 
            // cbAutoclose
            // 
            this.cbAutoclose.AutoSize = true;
            this.cbAutoclose.Location = new System.Drawing.Point(886, 9);
            this.cbAutoclose.Name = "cbAutoclose";
            this.cbAutoclose.Size = new System.Drawing.Size(122, 17);
            this.cbAutoclose.TabIndex = 72;
            this.cbAutoclose.Text = "Close when installed";
            this.cbAutoclose.UseVisualStyleBackColor = true;
            // 
            // UpdateForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(1008, 702);
            this.Controls.Add(this.cbAutoclose);
            this.Controls.Add(this.lblCurrVer);
            this.Controls.Add(this.lblTxtCurr);
            this.Controls.Add(this.pbAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnUpdateAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbModule);
            this.Controls.Add(this.pbDownload);
            this.Controls.Add(this.exploreButton);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.cbListVersion);
            this.Controls.Add(this.tbModuleDir);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblTxtLast);
            this.Controls.Add(this.lblLastVer);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.sbStatus);
            this.Controls.Add(this.pbExtract);
            this.Controls.Add(this.lblOS_Architeture);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdateForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " Update";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdateLibRT_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogClosed);
            this.Load += new System.EventHandler(this.UpdateDialog_Load);
            this.sbStatus.ResumeLayout(false);
            this.sbStatus.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		internal void fExecuteUpdater(ModuleData _oModule)
		{
       this.BeginInvoke((MethodInvoker)delegate  {
          //  aStatus["Simacode_IDE"].Text = "Pending";

            var res = MessageBox.Show(this, "The Cwc must quit to perform the update, do it now?", "Update?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)  {
				UpdateCwc.fLauchUpdate( _oModule.sCurrFolder, oModule.sLastLocalVersion);
		
	   
      /*
         p.StartInfo.FileName = _oModule.sCurrFolder + "cwc.exe";
				p.StartInfo.Arguments =  + " ";
			    p.Start();*/
		//		Data.fQuit(true);

				//bCloseMe = true;
             /*
   p.StartInfo.FileName = PathHelper.GetExeDirectory() + "/Updater_x32/Updater.exe";
          

               */
            }
			/* else  {
                Process p = new Process();
                p.StartInfo.FileName = PathHelper.WorkDirectory() + "/Updater_x32/Updater.exe";
                p.Start();
            }*/
         });
        }
		

		internal void fModuleFinish(){       this.BeginInvoke((MethodInvoker)delegate  {

			if(bSingleDownloadClick) {
				btnDownload.Enabled = false;
				btnUpdateAll.Enabled = false;
			//bSingleDownloadClick = true;
			}

			lblStatus.Text = "";

			
	            
            if ( cbAutoclose.Checked ) {
                    this.Visible = false;
            }
           // Debug.fTrace("Finish Extract");
			if(oModule.bAutoExplore) {
				exploreButton_Click(null,null);
            } else {
                  Data.sCmd = "StartBuild";
                // Data.StartBuild();
            }
            
		    });
          
            
            }


		#endregion

		#region Methods And Event Handlers

		/// <summary>
		/// Applies the localized texts to the form
		/// </summary> 
		private void ApplyLocalizedTexts()
        {
       //     this.Text = " " + TextHelper.GetString("Title.UpdateDialog");
         //   this.infoLabel.Text = TextHelper.GetString("Info.CheckingUpdates");
          //  this.downloadButton.Text = TextHelper.GetString("Label.DownloadUpdate");
           // this.closeButton.Text = TextHelper.GetString("Label.Close");
        }

		public bool bSingleDownloadClick = false;



        private void DownloadButtonClick(Object sender, EventArgs e) {

			btnDownload.Enabled = false;
			btnUpdateAll.Enabled = false;
			bSingleDownloadClick = true;

			ModuleLink _oLink = oModule.aLink[cbListVersion.Text];

			oModule.fSetLastLocalVersion(cbListVersion.Text,false);

			if(btnDownload.Text == "Update" &&  oModule.fLocalVersionExist(cbListVersion.Text)) { //CWC direct update
					fExecuteUpdater(oModule);
			}else {
					_oLink.fDownload();
			}

	
/*
            List<ModuleLink> _aList = oModule.aLink;
            int _nIndex = cbListVersion.SelectedIndex;

            foreach(ModuleLink _oModule in _aList ) {
                if(_oModule.nIndex  ==_nIndex ) {
                    oModuleToUpdate = _oModule;
                    break;
                }
            }*/
	//		oModuleToUpdate =  oModule.aLink[cbListVersion.Text];
/*
            bInProgress = !bInProgress;
             if(fUpdateUpdateComp()) {
                       InitializeDownload();
            }else {
                 if(oDlClient != null) {
                    oDlClient.CancelAsync();
                }
            }
    
*/
        }



/*
		public void fJsonnParser() {
			dynamic stuff = JsonConvert.DeserializeObject("{ 'Name': 'Jon Smith', 'Address': { 'City': 'New York', 'State': 'NY' }, 'Age': 42 }");

			string name = stuff.Name;
			string address = stuff.Address.City;
		}*/

      

         private bool fUpdateUpdateComp() {
    /*

             if(!oModule.bInProgress) {
                   pbBar.Value = 0;
                btnDownload.Text = "Update";
                lblStatus.Text = "Ready";
               cbListVersion.Enabled = true;
                     return false;
            }else {

      
                pbBar.PerformStep();
                btnDownload.Text = "Cancel";
                lblStatus.Text = "Download In Progress";
                cbListVersion.Enabled = false;
                     return true;
         
            }
*/
       return true;
        }

   
      





        /// <summary>
        /// When the form is closed cancel the update check
        /// </summary>
        private void DialogClosed(Object sender, FormClosedEventArgs e)
        {
/*
			if(oParent == null) {
				 Application.Exit();
			}*/
            /*
            if (this.worker.IsBusy)
            {
                this.worker.CancelAsync();
            }*/
        }

        /// <summary>
        /// Closes the dialog when user clicks buttons
        /// </summary>
        private void CloseButtonClick(Object sender, EventArgs e)
        {
            this.Visible = false;
/*
			if(oParent == null) {
				 Application.Exit();
			}*/
        }

      
        public  void fExtractBegin() {
            this.BeginInvoke((MethodInvoker)delegate  {
                    lblStatus.Text = "Extracting ...";
                    btnDownload.Enabled = false;
              });
        }

		internal void fUpdateVersion()
		{
          


			 this.BeginInvoke((MethodInvoker)delegate  {
				lblCurrVer.Text = oModule.sDisplayVersion;
				lblLastVer.Text = oModule.sLastVersion;

				if(lblCurrVer.Text == lblLastVer.Text) {
					lblCurrVer.ForeColor = Color.GreenYellow;
					lblTxtCurr.ForeColor = Color.GreenYellow;
				}else {
					if(oModule.sLastVersion != "") {
						lblCurrVer.ForeColor = Color.DarkOrange;
						lblTxtCurr.ForeColor = Color.DarkOrange;
					}
					
				}

				if(	lblCurrVer.Text  == "") {
					lblCurrVer.Text = "None";
					lblCurrVer.ForeColor = Color.DarkRed;
					lblTxtCurr.ForeColor = Color.DarkRed;
				}
				if(	lblLastVer.Text  == "") {
					lblLastVer.Text = "None";
					lblCurrVer.ForeColor = Color.DarkRed;
					lblTxtCurr.ForeColor = Color.DarkRed;
				}


			
				if(btnDownload.Text == "Update") {
						if(oModule.sDisplayVersion != oModule.sLastVersion || ModuleData.fGetVersion( cbListVersion.Text) != ModuleData.fGetVersion( oModule.sDisplayVersion) ) {	
							btnDownload.Enabled = true;
						}else {
							btnDownload.Enabled = false;
						}
				}else {


                     btnDownload.Enabled = false;
                    if( oModule.aLink.ContainsKey(cbListVersion.Text)) {
	                    	ModuleLink _oLink = oModule.aLink[cbListVersion.Text];
                       
                         if(  !_oLink.bDl_InProgress){
					        if(!oModule.fLocalVersionExist(cbListVersion.Text)) {
						        if(oModule.aServerVersion.Length > 0) {
							        btnDownload.Enabled = true;

						        }
					        }
                         } 
                     } 
                

				}

				btnUpdateAll.Enabled = false;
                foreach(KeyValuePair<string, ModuleData>  _kLink in aMData) {
					ModuleData _oModule = _kLink.Value;
					if(!_oModule.fLocalVersionExist(_oModule.sLastVersion) && _oModule.aServerVersion.Length > 0) { //->Remomended version
						btnUpdateAll.Enabled = true;
						break;
					}		
				}

			
			//	fSelectModule(aMData[ cbModule.Text] );

	


			 });
		}

		public  void fFinishAll() {
            this.BeginInvoke((MethodInvoker)delegate  {
               cbListVersion.Enabled = true;
              
                 lblStatus.Text = "Done";
     //             oModule.bInProgress = false;
		
	
                fUpdateUpdateComp();
			    fLibRTUpdateStatus();
				
	//			CompilerData.fSetLibRtFolder();

				if(bExecuteAfter) {
	
				//	Data.StartBuild();
					
					Data.sCmd = "StartBuild";
					this.Close();
				}
			
			//	((MainForm)(oParent)).fLoadNextModule();
            });

			
        }



        /// <summary>
        /// Shows the update dialog
        /// </summary>
        public static void Show(Form _oFrom, Boolean silent, string _sModuleName, bool _bExecuteAfter = false )  {
			ModuleData _oModule = ModuleData.fGetModule(_sModuleName,false); //False= lib but Already exist anyway
		

             UpdateForm _oUpdateDialog = _oModule.oForm;

            if (_oUpdateDialog == null)   {

                silentCheck = silent;
               
                _oModule.oForm = new UpdateForm(_sModuleName, _bExecuteAfter);
                _oUpdateDialog = (UpdateForm)_oModule.oForm;


                _oUpdateDialog.FormClosing += new FormClosingEventHandler(_oUpdateDialog.onClosing);
                _oUpdateDialog.Enabled = true;
				
				if(_oFrom != null) {
                //  cbCompiler.StartPosition = FormStartPosition.Manual;
					//_oUpdateDialog.Location = new Point( _oFrom.Left  + _oFrom.Width / 2 - _oUpdateDialog.Width / 2, _oFrom.Top + _oFrom.Height / 2  - _oUpdateDialog.Height / 2);
				//	_oUpdateDialog.Location = new Point( _oFrom.Cons_x  + (_oFrom.Cons_width - _oFrom.Cons_x) / 2 - _oUpdateDialog.Width / 2, _oFrom.Cons_y + ( _oFrom.Cons_height -  _oFrom.Cons_y)/ 2  - _oUpdateDialog.Height / 2);
					_oUpdateDialog.Location = new Point( _oFrom.Location.X , _oFrom.Location.Y);
		        
                    //ScreenOperations.IsWindowOnAnyScreen(_oUpdateDialog, true );

//_oUpdateDialog.
					_oUpdateDialog.oParent = _oFrom;
				   if (!silentCheck) _oUpdateDialog.Show(_oFrom);
				


				}else { 
					
					_oUpdateDialog.StartPosition = FormStartPosition.CenterScreen;
					_oUpdateDialog.bExecuteAfter = _bExecuteAfter;
					Data.oMainUpdateForm = _oUpdateDialog;
					aMData[_sModuleName].oForm = null;
					Data.bWaitGUI = true;
					 Thread winThread = new Thread(new ThreadStart(() =>  {  
							try {
									Application.Run(_oUpdateDialog);
									Data.oMainUpdateForm = null;

							 }catch(Exception Ex) {}
						
						}));  
					winThread.Start();
				}

                _oUpdateDialog.fIni();

            }else{


                _oUpdateDialog.fLibRTUpdateStatus();
                _oUpdateDialog.Visible = true;
            }
        }



[DllImport("user32.dll", CharSet = CharSet.Auto)]
internal static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

public void SetPaused(ProgressBar _oPb){
    // 0x410 = PBM_SETSTATE
    // 0x0003 = PBST_PAUSE
    SendMessage(_oPb.Handle, 0x410, 0x0003, 0);
}

public void SetError(ProgressBar _oPb){
    // 0x0002 = PBST_ERROR
    SendMessage(_oPb.Handle, 0x410, 0x0002, 0);
}

public void SetNormal(ProgressBar _oPb){
    // 0x0001 = PBST_NORMAL
    SendMessage(_oPb.Handle, 0x410, 0x0001, 0);
}

		public double nPbToValueExtact = 0;
		public double nPbToValueDownload = 0;
		public double nPbToValueAll = 0;


		public double nPbValExtact = 0;
		public double nPbValDownload = 0;
		public double nPbValAll = 0;



	internal void fExtractProgress(int _nValue, string _sData){
		string _sFile = "";

		int _nIndex =  _sData.IndexOf('-');
		if(_nIndex != -1) {
			_sFile = _sData.Substring(_nIndex );
		}
	
			
		 this.BeginInvoke((MethodInvoker)delegate  {
				try {

				    nPbToValueExtact = _nValue * 10000;

                    string _sExtName = oModule.sName;
                     if (oModule.bSubExtract) {
                        _sExtName += "/" + oModule.sCurrentExtractFile;
                       _sExtName =  "("  + oModule.nExtracted + "/" + oModule.nTotalExtract + ")" + _sExtName;
                     }
				    lblDetail.Text = "Extracting(" +_sExtName + "): " + _nValue.ToString() + " %  " + _sFile ;
			    //	Debug.fTrace(_nValue);
				}catch(Exception e){}  
		   });
		}





		public void fUpdateProgress(ParamHttp _oParam) {
		//	Debug.fTrace(_oParam.nPercentage);
			   this.BeginInvoke((MethodInvoker)delegate  {
		//			Debug.fTrace(_oParam.nPercentage);

				try {
                       

	//						Debug.fTrace("Progress2");
				/*	
						pbExtract.Minimum = 0;
					pbExtract.Maximum = 100;
					pbExtract.Step = 1;
*/						
					nPbToValueDownload  =  _oParam.nPercentage * 10000.0;

			string _sTotal = "Unknow";
			if(_oParam.nTotalBytes != -1) {
					_sTotal = String.Format("{0:0.00}", _oParam.nTotalBytes/1024/1024 ) + " mb";
            }

			string _sDlStatus = "Downloading( " + oModule.sName + " ): " +String.Format("{0:0.00}", _oParam.nBytes/1024/1024)  +  "/ " +_sTotal;
			
			lblStatus.Text = _sDlStatus + "";
 //_oParam.nPercentage.ToString()  
//Debug.fTrace(nPbToValueDownload);
					
				
				}catch(Exception e){}
	
			  });
		}
	internal void fDownloadComplete()
		{
			 this.BeginInvoke((MethodInvoker)delegate  {
					nPbToValueDownload = 100.0 * 10000;
			   });
		}




		public void fThreadPb() {

			pbExtract.Minimum = 0;
			pbDownload.Minimum = 0;
			pbAll.Minimum = 0;

			pbExtract.Maximum = 1000000;
			pbDownload.Maximum = 1000000;
			pbAll.Maximum = 1000000;

			pbDownload.Value =0;
			pbExtract.Value = 0;
			pbAll.Value = 0;

			pbDownload.Step = 0;
			pbExtract.Step = 0;
			pbAll.Step = 0;


	  		BackgroundWorker bw = new BackgroundWorker();bw.DoWork += new DoWorkEventHandler(delegate(object o, DoWorkEventArgs args) {
			while(Base.bAlive) {
			
				if(Base.bAlive) {
				this.BeginInvoke((MethodInvoker)delegate  {
					if(Base.bAlive && this != null && Visible == true) {
						nPbToValueAll = (nPbToValueDownload + nPbToValueExtact)/2.0;
					
						nPbValDownload += (nPbToValueDownload - nPbValDownload )/15.0;
						nPbValExtact += ((nPbToValueExtact  ) -  nPbValExtact )/15.0;
						nPbValAll += (nPbToValueAll -  nPbValAll )/15.0;

						int _nDlVal =  (int) nPbValDownload;
						if(_nDlVal > pbDownload.Maximum) {_nDlVal =  pbDownload.Maximum;}
						if(_nDlVal < pbDownload.Minimum) {_nDlVal =  pbDownload.Minimum;}

						int _nExtVal =  (int) nPbValExtact;
						if(_nExtVal > pbExtract.Maximum) {_nExtVal =  pbExtract.Maximum;}
						if(_nExtVal < pbExtract.Minimum) {_nExtVal =  pbExtract.Minimum;}

						int _nAllVal =  (int) nPbValAll;
						if(_nAllVal > pbAll.Maximum) {_nAllVal =  pbAll.Maximum;}
						if(_nAllVal < pbAll.Minimum) {_nAllVal =  pbAll.Minimum;}

						 pbDownload.Value =_nDlVal;
						 pbExtract.Value = _nExtVal;
						 pbAll.Value = _nAllVal;
					//	Debug.fTrace("bar:" +  pbAll.Value);
					}
				  });
					}
						Thread.Sleep(16);
				}
			});bw.RunWorkerAsync();
		}










         public  void fServerFail(string _sMsg) {

                   this.BeginInvoke((MethodInvoker)delegate  {
           
     
                        cbListVersion.Items.Clear();
                        cbListVersion.Items.Add("Server Error");
                        cbListVersion.SelectedIndex = 0;
                        lblStatus.Text = _sMsg ;
                       if(lblStatus.Text.Length > 71) {
                              lblStatus.Text =   lblStatus.Text.Substring(0, 71) ;
                       }
                       Debug.fTrace("---------- " +    _sMsg);
                                   
                   });


        }

        private void fIni() {
            fLibRTUpdateStatus();
        }

        #endregion



         public void fLibRTUpdateStatus() {
 

        }

			 public void fModuleLinkSelectionChanged() {
				tbModuleDir.Text = oModule.sOutFolder;
				if( oModule.aLink.ContainsKey(cbListVersion.Text)) {
					ModuleLink _oLink =oModule.aLink[cbListVersion.Text ];
					if(_oLink != null) {
						_oLink.fUpdateData();
					}
				}

			}
         private void fTestBtnDLSameVersion() {
            
			   string _sPath =   oModule.sOutFolder;
/*
				if(cbListVersion.Text ==  sVersion) {
					btnDownload.Enabled = false;
				}else {
						if(cbListVersion.Text != "(Pending)") {
							btnDownload.Enabled = true;
					}
				}

				if(sModuleName == "CwcUpd") {
						tbModuleDir.Text = PathHelper.GetExeDirectory();
				}else{
					   tbModuleDir.Text =  oModule.sOutFolder +  sModuleName + "_" + sOS_bit + "/";
				}

                if(Directory.Exists( tbModuleDir.Text)) {
                    exploreButton.Enabled = true;
                }else {
                    exploreButton.Enabled = false;
                }
*/
			/*
            if(aMData[sModuleName].bVersionInFolder) {
				
                string _sPath =   oModule.sOutFolder;

				
				if(cbListVersion.Text != "(Pending)") {
	
					_sPath +=  sModuleName + "_"  + cbListVersion.Text + "/";


					if(Directory.Exists(  _sPath) ) {
						btnDownload.Enabled = false;
						lblStatus.Text = sModuleName + " " +   cbListVersion.Text +  " already present";
					}else {
						btnDownload.Enabled = true;
						lblStatus.Text = "Ready";
					}
				}else {
					btnDownload.Enabled = false;
					lblStatus.Text = "Pending";
				}

                tbModuleDir.Text = _sPath;

            }else {
                 if(cbListVersion.Text ==  sVersion) {
                    btnDownload.Enabled = false;
                }else {
                      if(cbListVersion.Text != "(Pending)") {
                         btnDownload.Enabled = true;
                    }
                }
				if(sModuleName == "CwcUpd") {
						tbModuleDir.Text = PathHelper.GetExeDirectory();
				}else{
					   tbModuleDir.Text = PathHelper.GetExeDirectory() + oModule.sOutFolder +  sModuleName + "_" + sOS_bit + "/";
				}

                if(Directory.Exists( tbModuleDir.Text)) {
                    exploreButton.Enabled = true;
                }else {
                    exploreButton.Enabled = false;
                }
            }
*/
    


        }

/*
        private void btnPlatform_Click(object sender, EventArgs e){
            try
            {
                Button _btnPlt = (Button)(sender);
             
                switch (_btnPlt.Text)
                {
                    case "Windows":
                        aChk["Clang"].Checked = true;
                        break;

                    case "Web (Emsc/WebGL)":
                        aChk["Emscripten"].Checked = true;
                        aChk["Node"].Checked = true;
                        aChk["Python"].Checked = true;
                        aChk["Java"].Checked = true;
                        break;
                }
            }
            catch { }
        }*/
        /*
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var item in Application.Current.Windows)
            {
                Window window = item as Window;
                if (window.Title == "YourChildWindowTitle")
                {

                    // show some message for user to close childWindows

                    e.Cancel = true;
                    break;

                }
            }
        }*/

        
        void onClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (sender is UpdateForm)
                {
                    if (((UpdateForm)(sender)).Visible)
                    {
                       ((UpdateForm)(sender)).Visible = false;
                          e.Cancel = true;
                        return;
                    }
                }
/*
                if (!isDownloadCompleted())  {
                    var res = MessageBox.Show(this, "A download was in progress, are you sure you want quit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (res != DialogResult.Yes)
                    {
                        e.Cancel = true;
                        return;
                    }
                }*/
            }
            catch { }
        }
        

        
        public void fCleanTemp() {
			string _sTemp =  PathHelper.GetExeDirectory() + "temp/";
			if (!Directory.Exists(_sTemp)){
						Directory.CreateDirectory(_sTemp);
			}else {
				
				string[] filePaths = Directory.GetFiles(_sTemp);
				foreach (string filePath in filePaths) {
					try { 
						File.Delete(filePath);
					}catch(Exception Ex) { 
					
					}
				}
			}
        }




		public  void fGetProjectData(ParamHttp _oParam) {

			if(_oParam.bFail) {
				oModule.sReadme = "No Readme";//Projet unexist?
			}else {
				Http.fGetProject(oModule, _oParam.sResult);
			}
			fUpdateWebBrowser(wbReadme, oModule.sReadme  );

			Http.fGetHttp(  oModule.sUrl_Licence , fGetLicenceData);//Get licence -> after be sure of licence link
	
		}

		public  void fGetLicenceData(ParamHttp _oParam) {

			oModule.sLicence = "Unknow Licence";
			if(_oParam.bFail) {
		//		oModule.sLicence = "Unknow Licence";
			}else {
				Http.fGetLicence(oModule, _oParam.sResult);
			}
			fUpdateWebBrowser(wbLicence, oModule.sLicence  );

		}



		public void fSetModule(string _sName) {

			cbModule.Items.Clear();
/*
			foreach(string _sModule in Data.aModuleList) {
				if(_sModule != _sName) {
					cbModule.Items.Add(_sModule);
				}
			}
*/
			
			cbModule.Items.Add(_sName);


		//	lblChooseVer.Text = sModuleName + " :";
		//	Text += " " + sModuleName; 
		
		}




        private void UpdateDialog_Load(object sender, EventArgs e){

            cbAutoclose.Checked = true;
			
			fSetModule(sModuleName);
			if(sModuleName == "Cwc") {
				btnDownload.Text = "Update";
			}else {
				btnDownload.Text = "Download";
			}



			cbModule.SelectedIndex = 0;

			SetPaused(pbExtract); //Yellow color

				fThreadPb();



			wbReadme.DocumentText = sWebStyle +  "Pending..." + sWebStyleEnd;
			wbChanglog.DocumentText = sWebStyle +  "Pending..." + sWebStyleEnd;
			wbLicence.DocumentText = sWebStyle +  "Pending..." + sWebStyleEnd;

			Http.fGetHttp(  oModule.sUrl_Project , fGetProjectData);//Get readme
			oModule.fReadHttpModuleTags();

			/*
			this.Width = 440;
			int nMove = 440;
			if(aMData[sModuleName].sRepoURL != ""){
				lblTxtCV.Visible = false;
				lbCurrVer.Visible = false;
				lblChooseVer.Visible = false;
				cbListVersion.Visible = false;
				btnDownload.Visible = false;
				lblTxtCurr.Location = new Point(lblTxtCurr.Location.X - nMove,lblTxtCurr.Location.Y);
				lblTxtLast.Location = new Point(lblTxtLast.Location.X - nMove,lblTxtLast.Location.Y);
				btnGitUpdate.Location =  new Point(btnGitUpdate.Location.X - nMove,btnGitUpdate.Location.Y);
				btnReClone.Location =  new Point(btnReClone.Location.X - nMove,btnReClone.Location.Y) ;
				lblGitLastVer.Location =  new Point(lblGitLastVer.Location.X - nMove,lblGitLastVer.Location.Y);
			}
			*/
            /*
			if (!Directory.Exists(PathHelper.Module)){
				Directory.CreateDirectory(PathHelper.Module);
			}*/
/*
			if (!Directory.Exists(PathHelper.Demos)){
				Directory.CreateDirectory(PathHelper.Demos);
			}*/
            
			if (Directory.Exists(PathHelper.Updater)){
				 FileUtils.DeleteDirectory(PathHelper.Updater);
			}


       //      lblDesc.Text = aMData[sModuleName].sDesc;


           //  URL = "updater.simacode.com/CWC/Files.php";

		
       //      URL = aMData[sModuleName].sURL;
      ///       BaseURL  = Path.GetDirectoryName(URL).Replace('\\', '/') + "/";
      //       URL =  "http://";
       //      BaseURL =  "http://";



           //  BaseURL = Path.GetDirectoryName(URL);
          //  Debug.fTrace("BaseURL: " + BaseURL);
            if(is64){
                sOS_bit = "x64";
            }else{
                sOS_bit = "x32";
            }
            lblOS_Architeture.Text = "Current Platform " + sOS_bit;

            cbListVersion.Items.Clear();
            cbListVersion.Items.Add( "(Pending)");


            lblStatus.Text = "Connecting to server...";
            cbListVersion.SelectedIndex = 0;
            btnDownload.Enabled= false;
         //   this.InitializeUpdating();
			fCleanTemp();
        }

        

        public  void fDataLoaded() {
        // Debug.fTrace("-fDataLoaded");
				
				
              this.BeginInvoke((MethodInvoker)delegate  {
                //  Debug.fTrace("---------------- : " + sModuleName);

				int _nIndex = 0;

				string[] _aServerVersion = new string[oModule.aLink.Count];
					
			   cbListVersion.Items.Clear();
				if(oModule.aLink.Count > 0) {
                    foreach(KeyValuePair<string, ModuleLink>  _kLink in oModule.aLink ) {
						ModuleLink _oLink = _kLink.Value;

						_aServerVersion[_nIndex] =  _oLink.sDisplayVer;

                        _nIndex++;
                    }
						
					Array.Sort(_aServerVersion, StringComparer.InvariantCultureIgnoreCase);
					Array.Reverse(_aServerVersion);
					
					oModule.fSetLastVersion( _aServerVersion[0]);
					//oModule.sLastVersion = _aServerVersion[0];
					cbListVersion.Items.AddRange(_aServerVersion);
					oModule.aServerVersion = _aServerVersion;
				}else {
					  cbListVersion.Items.Add("No release");
				}
			         



/*
string[] _aDir = Directory.GetDirectories(sOutFolder);
				Array.Sort(_aDir, StringComparer.InvariantCultureIgnoreCase);
				Array.Reverse(_aDir);

				aLocalVersion = new string[_aDir.Length];
			*/


			//	 cbListVersion.Items

                cbListVersion.SelectedIndex = 0;
	//			lblLastVer.Text = oModule.sLastVersion;
					//oModule.fSetLastVersion();		
								
     //           btnDownload.Enabled = true;
                lblStatus.Text = "Ready";
               
            
				fLibRTUpdateStatus();


              });
        }
/*
         private void chkModuleCLick(object sender, EventArgs e)  {
             fUpdateDownloadBtn();
        
         }*/

            
/*
        private void btnAll_Click(object sender, EventArgs e)
        {
            foreach (CheckBox _chkSel in aChk.Values)
            {
                _chkSel.Checked = true;
            }
            fLibRTUpdateStatus();
        }
*/
/*
        private void btnNone_Click(object sender, EventArgs e){
            foreach (CheckBox _chkSel in aChk.Values)
            {
                if (_chkSel.Enabled == true)
                {
                _chkSel.Checked = false;
                }
            }
            fLibRTUpdateStatus();
        }*/

        private void cbListVersion_SelectedIndexChanged(object sender, EventArgs e) {
      
			fModuleLinkSelectionChanged();
				fUpdateVersion();
      //    fTestBtnDLSameVersion();
        }

        private void button1_Click(object sender, EventArgs e) {

        }

        private void sbStatus_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

        private void lbCurrVer_Click(object sender, EventArgs e) {
      //  cbListVersion.Text = "(Pending)";
        
        }

        private void pbBar_Click(object sender, EventArgs e) {
    
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void pathLabel_Click(object sender, EventArgs e) {

        }

        private void exploreButton_Click(object sender, EventArgs e) {
             try {
                Process.Start("explorer.exe", tbModuleDir.Text.Replace('/','\\'));
            } catch (Exception ex)  {
                DialogHelper.ShowError(ex.ToString());
            }
        }

		private void tbModuleDir_TextChanged(object sender, EventArgs e)
		{

		}

		private void UpdateLibRT_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(oParent == null) {
				 Application.Exit();
			}
		}



		 public  void fGitCloneEnd()  {
			fLibRTUpdateStatus();
			fFinishAll(); //Not sure
			
		}
		 public  void fGitFetchEnd()  {
			fLibRTUpdateStatus();
			fFinishAll(); //Not sure
		}

		public  void fLrtGitCloneOut(string _sOut)  {
			if(_sOut.Length > 0) {
				Debug.fTrace( _sOut);
				int _nPcEnd = _sOut.IndexOf('%', 5);
				if(_nPcEnd > 4) {
					int _nPcStart = _nPcEnd - 2;
					if(_sOut[_nPcStart] != ' ') {_nPcStart--;}
					if(_sOut[_nPcStart] != ' ') {_nPcStart--;}
					//	string _pc 
					string _sPc = _sOut.Substring(_nPcStart,_nPcEnd - _nPcStart );
					 int _nPc;
/*
					if (Int32.TryParse(_sPc, out _nPc)) {
						pbExtract.Value = _nPc;
					//	 pbBar.PerformStep();
					}
			*/
				}
			}


		}


/*
		private void btnGitUpdate_Click(object sender, EventArgs e)
		{
			pbBar.Value = 0;
	//		btnGitUpdate.Enabled = false;
			if(fGetLibRTCurrentGitVersion()) {
				fGitPull();
			}else {
				fGitClone();
			}
		}
*/

/*
		public void fReclone() {
			pbBar.Value = 0;
			//btnReClone.Enabled = false;
		//	btnGitUpdate.Enabled = false;
			fGitClone();
		}
*/

/*
		private void btnReClone_Click(object sender, EventArgs e){
			
			string _sFolderPath  =  PathHelper.GetExeDirectory() +  aMData[sModuleName].sOutFolder +  sModuleName;
			var result = MessageBox.Show("Do you want make a backup?",  "Backup Files?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
			if (result == DialogResult.Yes){
				Directory.Move(_sFolderPath, _sFolderPath + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
				fReclone();
			}

			if (result == DialogResult.No){
					
					var resultDel = MessageBox.Show("All modified files will be lost. Are you sure?",  "Delete File?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (resultDel == DialogResult.Yes){
					//	Debug.fTrace("del: " +  PathHelper.GetExeDirectory() +  aMData[sModuleName].sOutFolder +  sModuleName);
						if(Directory.Exists(_sFolderPath)) {
							//	Debug.fTrace("deleted!!: ");
							FileUtils.DirectoryDeleteAll(_sFolderPath);
							fReclone();
						}
					}
			}
		//	fLibRTUpdateStatus();
		}
*/

		private void lblTxtLast_Click(object sender, EventArgs e)
		{

		}


		public static string sWebStyle =    "<body style=\"background-color:#444444;"
												         +"border: thin solid #333333;"
														 +"\"><center><div id=\"header\" align=\"left\" style=\""
															+"width:985px;"
															+"border: thin solid  #111111;"
															+"padding:6px 20px 6px 20px;margin-top:-10px;background-color:#FFFFFF;"
															+"\">";
															
		public static string sWebStyleEnd =   "</div><center></body>";

		public  void fUpdateReleaseInfo(ModuleLink _oModule) {

			if(_oModule.oRelease != null && _oModule.oRelease.body  != null) {
				fUpdateWebBrowser(wbChanglog,_oModule.oRelease.body );
			}

		}

		public  void fUpdateWebBrowser(WebBrowser _oBrowser, string _sHtml) {
			try {
				
		    	_sHtml = _sHtml.Replace("_blank", "_self").Replace("framename", "_self");
				_oBrowser.DocumentText =sWebStyle +  _sHtml + sWebStyleEnd;


			}catch(Exception e) {};
		}


		private void wbInfo_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			 if (!(e.Url.ToString().Equals("about:blank", StringComparison.InvariantCultureIgnoreCase))){
					string _sUrl =  e.Url.ToString();
					_sUrl = _sUrl.Replace("about:","");
					if(_sUrl[0] == '/') {
						_sUrl = _sUrl.Substring(1);
						_sUrl =  ModuleData.sUrl_Github + _sUrl;
					}
					System.Diagnostics.Process.Start(_sUrl);
					e.Cancel = true;
				}
		}


		private void lblGitLastVer_Click(object sender, EventArgs e)
		{

		}

		private void wbChanglog_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{

		}

		private void wbReadme_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{

		}

		private void lblStatus_Click(object sender, EventArgs e)
		{

		}

		private void lblChooseVer_Click(object sender, EventArgs e)
		{

		}

		private void cbModule_SelectedIndexChanged(object sender, EventArgs e)
		{

			fSelectModule(aMData[ cbModule.Text] );
		}

			

		private void fSelectModule(ModuleData _oModule){
			
			oModule = _oModule;
			oModule.fGetLocalVersions();
			
		}

		private void btnUpdateAll_Click(object sender, EventArgs e){
			
		}

		private void lblCurrVer_Click(object sender, EventArgs e)
		{

		}

		private void lblTxtCurr_Click(object sender, EventArgs e)
		{

		}

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {

        }

        private void pbAll_Click(object sender, EventArgs e) {

        }
    }
}