using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Data;
using System.Timers;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using PluginCore.Localization;
using PluginCore.Managers;
using PluginCore.Helpers;
using System.Security.Permissions;
using Microsoft.Win32;
using PluginCore;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows;

namespace Simacode.Dialogs
{
    public class UpdateLDK: CommunUpdater
    {
        public static UpdateLDK updateDialog;
        public static Boolean silentCheck = false;
        public static bool bInitilised = false;

        private Label label1;

        public new Button closeButton;
        public new Button btnDownload;
        public new TableLayoutPanel tableLayoutPanel1;
        public new Label lblFlashVer;
        public new Button btnAll;
        public new Button btnNone;
        public new Label lblOS_Architeture;

        public UpdateLDK()
        {
            this.Owner = Globals.MainForm;
            this.Font = Globals.Settings.DefaultFont;
            this.InitializeComponent();
            this.ApplyLocalizedTexts();
            
            ScaleHelper.AdjustForHighDPI(this);
        }

        #region Windows Form Designer Generated Code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDownload = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFlashVer = new System.Windows.Forms.Label();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.lblOS_Architeture = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.SlateGray;
            this.btnDownload.Enabled = false;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDownload.Location = new System.Drawing.Point(385, 250);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(116, 35);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "&Download";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.DownloadButtonClick);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.LightSlateGray;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.closeButton.Location = new System.Drawing.Point(533, 250);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(84, 35);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.LightSlateGray;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(78, 113);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(117, 57);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // lblFlashVer
            // 
            this.lblFlashVer.Location = new System.Drawing.Point(928, 282);
            this.lblFlashVer.Name = "lblFlashVer";
            this.lblFlashVer.Size = new System.Drawing.Size(77, 13);
            this.lblFlashVer.TabIndex = 9;
            this.lblFlashVer.Text = "FlashVer";
            this.lblFlashVer.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // btnAll
            // 
            this.btnAll.BackColor = System.Drawing.Color.LightSlateGray;
            this.btnAll.Location = new System.Drawing.Point(96, 88);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(75, 22);
            this.btnAll.TabIndex = 14;
            this.btnAll.Text = "All";
            this.btnAll.UseVisualStyleBackColor = false;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnNone
            // 
            this.btnNone.BackColor = System.Drawing.Color.LightSlateGray;
            this.btnNone.Location = new System.Drawing.Point(177, 88);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(75, 22);
            this.btnNone.TabIndex = 15;
            this.btnNone.Text = "None";
            this.btnNone.UseVisualStyleBackColor = false;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // lblOS_Architeture
            // 
            this.lblOS_Architeture.Font = new System.Drawing.Font("Candara", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOS_Architeture.Location = new System.Drawing.Point(841, 1);
            this.lblOS_Architeture.Name = "lblOS_Architeture";
            this.lblOS_Architeture.Size = new System.Drawing.Size(163, 22);
            this.lblOS_Architeture.TabIndex = 20;
            this.lblOS_Architeture.Text = "Current Platform xXX";
            this.lblOS_Architeture.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(984, 22);
            this.label1.TabIndex = 21;
            this.label1.Text = "Update the Linx Development Kit";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpdateLDK
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(1008, 295);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblOS_Architeture);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.lblFlashVer);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.btnDownload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "UpdateLDK";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Update Modules";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogClosed);
            this.Load += new System.EventHandler(this.UpdateDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Methods And Event Handlers

        /// <summary>
        /// Applies the localized texts to the form
        /// </summary> 
        private void ApplyLocalizedTexts()
        {
            this.Text = " " + TextHelper.GetString("Title.UpdateDialog");
         //   this.infoLabel.Text = TextHelper.GetString("Info.CheckingUpdates");
          //  this.downloadButton.Text = TextHelper.GetString("Label.DownloadUpdate");
           // this.closeButton.Text = TextHelper.GetString("Label.Close");
        }

        /// <summary>
        /// Downloads the new simacode release
        /// </summary>
        private void DownloadButtonClick(Object sender, EventArgs e)
        {

            btnDownload.Text = "Download In Process";
            btnDownload.Enabled = false;

            InitializeDownload();
        }

   
        private void InitializeDownload()
        {
	TraceManager.Add("---------------------------Initialize ldk -------------------------");
            this.DlgWorker = new BackgroundWorker();
            this.DlgWorker.DoWork += new DoWorkEventHandler(this.DlgWorkerDoWork);
            this.DlgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.DlgWorkerCompleted);
            this.DlgWorker.WorkerSupportsCancellation = true;
            this.DlgWorker.RunWorkerAsync();
        }


        private void DlgWorkerDoWork(Object sender, DoWorkEventArgs e)
        {
            string _sFile = "";
         

                int i = 0;
                foreach (CheckBox _chkSel in aChk.Values)
                {
                    if (_chkSel.Checked == true && _chkSel.Enabled == true)
                    {

                        fDownloadModule(aModule[i]);
                        
                    }
                    i++;
                }
        }






        /// <summary>
        /// When the form is closed cancel the update check
        /// </summary>
        private void DialogClosed(Object sender, FormClosedEventArgs e)
        {
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
        }

      




        /// <summary>
        /// Shows the update dialog
        /// </summary>
        public static void Show(Boolean silent)
        {
            if (!bInitilised)
            {
                silentCheck = silent;
                updateDialog = new UpdateLDK();


               updateDialog.FormClosing += new FormClosingEventHandler(updateDialog.onClosing);

                ///  if (!silentCheck) updateDialog.ShowDialog();
                ///  
                updateDialog.StartPosition = FormStartPosition.CenterScreen;
                if (!silentCheck) updateDialog.Show(PluginBase.MainForm);

                bInitilised = true;
            }
            else
            {
                updateDialog.fUpdateStatus();
                updateDialog.Visible = true;
            }

          
        }

        #endregion




        private void btnPlatform_Click(object sender, EventArgs e){
            try
            {
                Button _btnPlt = (Button)(sender);
                TraceManager.Add(_btnPlt.Text);
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
        }
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
                if (sender is UpdateLDK)
                {
                    if (((UpdateLDK)(sender)).Visible)
                    {
                       ((UpdateLDK)(sender)).Visible = false;
                          e.Cancel = true;
                        return;
                    }
                }
                if (!isDownloadCompleted())
                {
                    var res = MessageBox.Show(this, "A download was in progress, are you sure you want quit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (res != DialogResult.Yes)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch { }
        }


        public override void fUpdateDownloadBtn()
        {
    
                bool bHaveDl = false;
                foreach (CheckBox _chkSel in aChk.Values)
                {
                    if (_chkSel.Checked && _chkSel.Enabled)
                    {
                        bHaveDl = true;
                    }

                }
                if (bHaveDl && bReadFinished)
                {
                    btnDownload.Enabled = true;
                    btnDownload.Text = "Download";
                }
                else
                {
                    btnDownload.Enabled = false;
                }

        }

        private void UpdateDialog_Load(object sender, EventArgs e){
             URL = "http://updater.simacode.com/LDK/LDK.txt";
             BaseURL = "http://updater.simacode.com/LDK/";

             sDownloadDir = PathHelper.LDKdir;
             aModule = new string[] { "Compiler", "GZE" };
             aPfm = new string[] { "Windows", "MainLib" };

            btnDownload.Enabled = false;

            if(is64){
                sOS_bit = "x64";
            }else{
                sOS_bit = "x32";
            }
            lblOS_Architeture.Text = "Current Platform " + sOS_bit;


             int _nSize = 15;
             Font _Font = new Font("Trebuchet", _nSize,  GraphicsUnit.Pixel);
             Font _FontInfo = new Font("Trebuchet", _nSize + 1, FontStyle.Bold, GraphicsUnit.Pixel);

            Label _lblEmpty = new Label();
            Label _lbl1Module = new Label();
            Label _lbl1Version = new Label();
            Label _lbl1LastVersion = new Label();
            Label _lbl1Status = new Label();
            Label _lblProgress = new Label();
            Label _lbl1ZipSize= new Label();
            Label _lbl1Pfm = new Label();

            _lbl1Version.Font = _FontInfo;
            _lbl1LastVersion.Font = _FontInfo;
            _lbl1Module.Font = _FontInfo;
            _lbl1Status.Font = _FontInfo;
            _lblProgress.Font = _FontInfo;
            _lbl1ZipSize.Font = _FontInfo;
            _lbl1Pfm.Font = _FontInfo;
            
            _lbl1Module.Text = "Module";
            _lbl1Version.Text = "Current";
            _lbl1LastVersion.Text = "Last";
            _lbl1Status.Text = "Status";
            _lblProgress.Text = "%";
            _lblEmpty.Text = "";
            _lbl1ZipSize.Text = "Size";
            _lbl1Pfm.Text = "Platform";

             _lblEmpty.Width = 10;
             _lblEmpty.Height = 10;

             _lbl1Status.Width = 110;
             _lbl1Version.Width = 100;
             //_lbl1Version.Width = 100;
             _lbl1LastVersion.Width = 80;
             _lbl1ZipSize.Width = 70;
             _lbl1Pfm.Width = 150;


            tableLayoutPanel1.Controls.Add(_lblEmpty, 0, 0);
            tableLayoutPanel1.Controls.Add(_lbl1Pfm, 1, 0);
            tableLayoutPanel1.Controls.Add(_lbl1Module, 2, 0);
          

            tableLayoutPanel1.Controls.Add(_lbl1Version, 3, 0);
            tableLayoutPanel1.Controls.Add(_lbl1LastVersion, 4, 0);

            tableLayoutPanel1.Controls.Add(_lblProgress, 5, 0);
            tableLayoutPanel1.Controls.Add(_lbl1ZipSize, 6, 0);
            tableLayoutPanel1.Controls.Add(_lbl1Status, 7, 0);


            for (var i = 0; i < aModule.Length; i++)
            {
                 ProgressBar _prgBar = new ProgressBar();
                _prgBar.Width = 100;
           
                 Label _lblModule = new Label();
                 Label _lblStatus = new Label();
                 Label _lblVersion = new Label();
                 Label _lblLastVersion = new Label();
                 Label _lblZipSize = new Label();
                 Label _lblPfm = new Label();
                 CheckBox _chkText = new CheckBox();
                 _chkText.Click += chkModuleCLick;

                 _chkText.Width = 15;

                 _prgBar.Height = _nSize;
                 _chkText.Height = _nSize;
                 _lblModule.Height = _nSize + 5;
                 _lblStatus.Height = _nSize + 5;
                 _lblZipSize.Height = _nSize;
                 _lblPfm.Height = _nSize + 5;

                 _lblModule.Width = 100;
                 _lblStatus.Width = 110;
                 _lblVersion.Width = 100;
                 _lblLastVersion.Width = 100;
                 _lblZipSize.Width = 70;
                 _lblPfm.Width = 150;

                 _chkText.Font = _Font;
                 _lblModule.Font = _Font;
                 _lblStatus.Font = _Font;
                 _lblVersion.Font = _Font;
                 _lblLastVersion.Font = _Font;
                 _lblZipSize.Font = _Font;
                 _lblPfm.Font = _Font;

              //  _lblStatus.TextAlign = Alib

                 _lblModule.Text = aModule[i];
                 _lblVersion.Text = "---";
                 _lblLastVersion.Text = "---";
                 _lblStatus.Text = "---";
                 _lblZipSize.Text = "---";
                 _lblPfm.Text = aPfm[i];

                 aChk[aModule[i]] = _chkText;
                 aStatus[aModule[i]] = _lblStatus;
                 aCurrVersion[aModule[i]] = _lblVersion;
                 aZipSize[aModule[i]] = _lblZipSize;
                aLastVersion[aModule[i]] = _lblLastVersion;
                aPrgBar[aModule[i]] = _prgBar;

                 tableLayoutPanel1.Controls.Add(_chkText, 0, i+1);
                 tableLayoutPanel1.Controls.Add(_lblPfm, 1, i + 1);
                 tableLayoutPanel1.Controls.Add(_lblModule, 2, i + 1);
                 tableLayoutPanel1.Controls.Add(_lblVersion, 3, i + 1);
                 tableLayoutPanel1.Controls.Add(_lblLastVersion, 4, i + 1);
                 tableLayoutPanel1.Controls.Add(_prgBar, 5, i + 1);
                 tableLayoutPanel1.Controls.Add(_lblZipSize, 6, i + 1);
                 tableLayoutPanel1.Controls.Add(_lblStatus, 7, i + 1);

            }
     
           // TraceManager.Add("Flash vers : " + GetFlashPlayerVersionString());
            lblFlashVer.Text = GetFlashPlayerVersionString();

            fUpdateStatus();

            this.InitializeUpdating();
        }


         private void chkModuleCLick(object sender, EventArgs e)  {
             fUpdateDownloadBtn();
        
         }



        private string GetFlashPlayerVersionString()
        {
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Macromedia\FlashPlayer");
            if (regKey != null)
            {
                string flashVersion = Convert.ToString(regKey.GetValue("CurrentVersion"));
                return flashVersion;
            }
            return string.Empty;
        }


        private void btnAll_Click(object sender, EventArgs e)
        {
            foreach (CheckBox _chkSel in aChk.Values)
            {
                _chkSel.Checked = true;
            }
            fUpdateStatus();
        }


        private void btnNone_Click(object sender, EventArgs e){
            foreach (CheckBox _chkSel in aChk.Values)
            {
                if (_chkSel.Enabled == true)
                {
                _chkSel.Checked = false;
                }
            }
            fUpdateStatus();
        }




    }


}