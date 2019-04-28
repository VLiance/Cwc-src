using System;
using System.IO;
using System.Net;
using System.Data;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using cwc.Utilities;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace cwc
{

    public partial class MainForm : Form{
/*
/// Start invisible !!///
 private bool mShowAllowed  =false;
protected override void SetVisibleCore(bool value) {
 if (!mShowAllowed) value = false;
 base.SetVisibleCore(value);
}
/// //////////////////
*/
/*
 protected override void OnLoad(EventArgs e)
    {
        Visible = false; // Hide form window.
        ShowInTaskbar = false; // Remove from taskbar.
        Opacity = 0;

        base.OnLoad(e);
    }
*/


		public bool bLoaded = false;
        private String curFile;
        private String tempFile;
        private String localeId;
        private Boolean isLoading;
        private DepEntry curEntry;
        private String entriesFile;
        private WebClient webClient;
        private DepEntries depEntries;
        private DepEntries instEntries;
        private BackgroundWorker bgWorker;
        private Dictionary<String, String> entryStates;
        private Dictionary<String, ListViewGroup> appGroups;

		internal void fLauchPrj()	{this.BeginInvoke((MethodInvoker)delegate  {

			    btnPlay.Enabled = false;
                btnSanitize.Enabled = false;
                tbOutput.Enabled = false;
                btOutputSet.Enabled = false;
                 CancelButtonEndIcon();

		 });}

		private Queue<DepEntry> downloadQueue;
        private Queue<String> fileQueue;
        private LocaleData localeData;
        private Boolean localeOverride;
        private String[] notifyPaths;
        private Boolean haveUpdates;
        private Boolean checkOnly;

        /**
        * Static link label margin constant
        */
        public static Int32 LINK_MARGIN = 4;

        /**
        * Static constant for exposed config groups (separated with ,)
        */
        public static String EXPOSED_GROUPS = "FD5";

        /**
        * Static type and state constants
        */ 
        public static String TYPE_LINK = "Link";
        public static String TYPE_EXECUTABLE = "Executable";
        public static String TYPE_ARCHIVE = "Archive";
        public static String STATE_INSTALLED = "Installed";
        public static String STATE_UPDATE = "Updated";
        public static String STATE_NEW = "New";


      public Empty oEmptyForm =  null;
	public static  SelectForm oSelectForm = null;

        public MainForm(String[] args,SelectForm _oSelectForm, Empty _oEmpty)
        {
            oEmptyForm = _oEmpty;
		//	Data.oMainForm = this;
			oSelectForm=  _oSelectForm;

         //   this.CheckArgs(args);
            this.isLoading = false;
            this.haveUpdates = false;
            this.InitializeSettings();
            this.InitializeLocalization();
            this.InitializeComponent();
            this.InitializeGraphics();
            this.InitializeContextMenu();
            this.InitializeFormScaling();
            this.ApplyLocalizationStrings();
            this.Font = SystemFonts.MenuFont;



            fResetList();
            fInitialiseCmd(Data.sArg);
			
			Data.oLauchProject.oMainForm = this;

        }






         public  void fInitialiseCmd(string _sAllArg){
        //    cbCommands.Items.Add(new ComboBoxItem("México","0",Color.Green));
         //   cbCommands.Items.Add(new ComboBoxItem("México","0",Color.Green));


          //   cbCommands.Items.Add(new ComboBoxItem(">> " + _sAllArg,  cbCommands.Items.Count.ToString(), Color.DarkSlateGray));
             cbCommands.Items.Add(new ComboBoxItem(">> " + _sAllArg,  cbCommands.Items.Count.ToString(), Color.DarkGreen));


              string[] _aSequenceArg = _sAllArg.Split('>'); //Squential
              foreach (string _sSeqArg in _aSequenceArg) { if (!FileUtils.IsEmpty(_sSeqArg)) {

                    // Output.Trace("\f1BCwc Cmd:\f13 " + _sSeqArg);
                   // cbCommands.Items.Add(_sSeqArg);
                   cbCommands.Items.Add(new ComboBoxItem("> " +_sSeqArg,  cbCommands.Items.Count.ToString()  ,Color.DarkRed));

                   string[] _aArg = _sSeqArg.Split('|'); //Simultanous
                   foreach (string _sArg in _aArg) { if (!FileUtils.IsEmpty(_sArg)) {
                             //cbCommands.Items.Add(" -- " + _sArg);
                       cbCommands.Items.Add(new ComboBoxItem("|  " + _sArg, cbCommands.Items.Count.ToString() ,Color.DarkBlue));
                  }}
              }}

         //     cbCommands.SelectedItem = "1";

              cbCommands.Text = _sAllArg.Trim();
             cbCommands.SelectedIndex = 0;
      
        }



       public void fSetOutput( string _wOut = "" ) {this.BeginInvoke((MethodInvoker)delegate  {

			if(_wOut != "") {
               //    tbOutput.Text = _wOut;
               outputToolStripMenuItem.Text = "Output: ( " + _wOut + " )";

            }


		});}



public struct TEst
{
    public bool val;

}

public int WM_NCACTIVATE = 0x86;
public int WM_ACTIVATE = 0x06;
public int  WM_SETFOCUS = 0x07;

        #region Instancing


        protected override void WndProc(ref Message m)  {

			if(m.Msg != 49978 && m.Msg != 70) {
	//	Debug.fTrace(m.Msg);
			}


			if(m.Msg == WM_NCACTIVATE) { // || m.Msg ==  WM_ACTIVATE || m.Msg ==  WM_SETFOCUS
				//int size = Marshal.SizeOf(Int32);

				fActivated((int)m.WParam );

			}


            if (m.Msg == Win32.WM_SHOWME) this.RestoreWindow();

			
				const int WM_NCHITTEST = 0x0084;
				const int HTCLIENT = 0x01;

				if (m.Msg == WM_NCHITTEST)
				{
					m.Result = new IntPtr(HTCLIENT);
					return;
				}

            base.WndProc(ref m);
        }

        string sViewIn_LastItemSelected = "";
        string sViewIn_ItemSelected = "";
        private void viewIn_Item_Click(object sender, EventArgs e)
        {
            
            foreach (ToolStripMenuItem _oTS in viewInToolStripMenuItem.DropDownItems)
            {
                _oTS.Checked = false;
            }
            ToolStripMenuItem _oItem = (ToolStripMenuItem)(sender);
            _oItem.Checked = true;
            sViewIn_LastItemSelected = _oItem.Text;
            sViewIn_ItemSelected = _oItem.Text;
            Data.sCurrViewIn = sViewIn_ItemSelected;
        }


        internal void fUpdateBrowser()	{
			this.BeginInvoke((MethodInvoker)delegate
            {
                sViewIn_ItemSelected = "";
                viewInToolStripMenuItem.DropDownItems.Clear();
                // viewInToolStripMenuItem.DropDownItems.Add("(View In)");
                foreach (string _sBrowser in Data.aBrowser)
                {
                    ToolStripMenuItem _oItem = (ToolStripMenuItem)viewInToolStripMenuItem.DropDownItems.Add(_sBrowser);
                    _oItem.Click += new System.EventHandler(this.viewIn_Item_Click);
                }

                if (sViewIn_LastItemSelected != "") {
                    foreach (ToolStripMenuItem _oTS in viewInToolStripMenuItem.DropDownItems){
                        if (_oTS.Text == sViewIn_LastItemSelected)    {
                            _oTS.Checked = true;
                            sViewIn_ItemSelected = _oTS.Text;
                            break;
                        }
                    }
                } else  {
                    //Selectfirst
                    foreach (ToolStripMenuItem _oTS in viewInToolStripMenuItem.DropDownItems)  {
                        _oTS.Checked = true;
                        sViewIn_ItemSelected = _oTS.Text;
                        break;
                    }
                }
                sViewIn_LastItemSelected =  sViewIn_ItemSelected;
                Data.sCurrViewIn = sViewIn_ItemSelected;
                /*

                int _nIndex = cbView.SelectedIndex;
				cbView.Items.Clear();

				cbView.Items.AddRange(Data.aBrowser.ToArray());
				if(_nIndex < 0){
					_nIndex = 0;
				}
				if(_nIndex > cbView.Items.Count){
					_nIndex = cbView.Items.Count;
				}
				 cbView.SelectedIndex = _nIndex;*/
            });
		}


		/*
		protected override void WndProc(ref Message message)
		{
			const int WM_NCHITTEST = 0x0084;
			const int HTCLIENT = 0x01;

			if (message.Msg == WM_NCHITTEST)
			{
				message.Result = new IntPtr(HTCLIENT);
				return;
			}

			base.WndProc(ref message);
		}
		*/
		#endregion




		/// <summary>
		/// Restore the window of the first instance
		/// </summary>
		private void RestoreWindow() {this.BeginInvoke((MethodInvoker)delegate  {

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            Boolean top = this.TopMost;
            this.TopMost = true;
            this.TopMost = top;
        });}



        #region Initialization

        /// <summary>
        /// Processes command line args.
        /// </summary>
        private void CheckArgs(String[] args)
        {
            this.checkOnly = false;
            this.localeId = "en_US";
            this.localeOverride = false;
            foreach (String arg in args)
            {
                // Handle minimized mode
                if (arg.Trim() == "-minimized")
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.checkOnly = true;
                }
                // Handle locale id values
                if (arg.Trim().Contains("-locale="))
                {
                    this.localeId = arg.Trim().Substring("-locale=".Length);
                    this.localeOverride = true;
                }
            }
        }

        /// <summary>
        /// Initialize the scaling of the form.
        /// </summary>
        private void InitializeFormScaling()
        {
            if (this.GetScale() > 1)
            {
                this.descHeader.Width = this.ScaleValue(319);
                this.nameHeader.Width = this.ScaleValue(160);
                this.versionHeader.Width = this.ScaleValue(90);
                this.statusHeader.Width = this.ScaleValue(70);
                this.typeHeader.Width = this.ScaleValue(75);
                this.infoHeader.Width = this.ScaleValue(30);
                this.lineHeader.Width = this.ScaleValue(30);
                this.colHeader.Width = this.ScaleValue(30);
                Int32 width = Convert.ToInt32(this.Width * 1.06);
                this.Size = new Size(width, this.Height);
            }
        }

         Assembly assembly = Assembly.GetExecutingAssembly();

        /// <summary>
        /// Initializes the graphics of the app.
        /// </summary>
        private void InitializeGraphics()
        {

           Icon = new Icon(assembly.GetManifestResourceStream("cwc.Resources.cwc.ico"));
            
        }


         public void CancelButtonEndIcon(){
            cancelButton.Text = "End";
            ImageList imageList = new ImageList();

            aaToolStripMenuItem1.Text = "END  ";
            aaToolStripMenuItem1.BackColor = Color.YellowGreen;

            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageSize = new Size(this.ScaleValue(24), this.ScaleValue(24));
            imageList.Images.Add(Image.FromStream(assembly.GetManifestResourceStream("cwc.Resources.Cancel.png")));
       
            this.cancelButton.ImageList = imageList;
            this.cancelButton.ImageIndex = 0;
        }


        public void CancelButtonStopIcon(){
            cancelButton.Text = "Stop";

            aaToolStripMenuItem1.Text = "STOP ";
            aaToolStripMenuItem1.BackColor = Color.DarkRed;

            ImageList imageList = new ImageList();
           
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageSize = new Size(this.ScaleValue(24), this.ScaleValue(24));
            imageList.Images.Add(Image.FromStream(assembly.GetManifestResourceStream("cwc.Resources.Cancel.png")));
       
            this.cancelButton.ImageList = imageList;
            this.cancelButton.ImageIndex = 0;
        }
        public void CancelButtonBuildIcon() {
             cancelButton.Text = "Build";

            aaToolStripMenuItem1.Text = "BUILD";
            aaToolStripMenuItem1.BackColor = Color.Teal;

            ImageList imageList = new ImageList();

            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageSize = new Size(this.ScaleValue(24), this.ScaleValue(24));
            imageList.Images.Add(Image.FromStream(assembly.GetManifestResourceStream("cwc.Resources.From.png")));
            this.Icon = new Icon(assembly.GetManifestResourceStream("cwc.Resources.cwc.ico"));
            this.cancelButton.ImageList = imageList;
            this.cancelButton.ImageIndex = 0;
        }

        /// <summary>
        /// Initializes the web client used for item downloads.
        /// </summary>
        private void InitializeWebClient()
        {
            this.webClient = new WebClient();

//webClient.Proxy = null;
webClient.Proxy=GlobalProxySelection.GetEmptyWebProxy();

            this.webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.DownloadProgressChanged);
           // this.webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.DownloadFileCompleted);
        }

        /// <summary>
        /// Initializes the localization of the app.
        /// </summary>
        private void InitializeLocalization()
        {
            this.localeData = new LocaleData();
            String localeDir = Path.Combine(PathHelper.GetExeDirectory(), "Locales");
            String localeFile = Path.Combine(localeDir, this.localeId + ".xml");
            if (File.Exists(localeFile))
            {
                this.localeData = ObjectSerializer.Deserialize(localeFile, this.localeData) as LocaleData;
            }
        }

        /// <summary>
        /// Applies the localization string to controls.
        /// </summary>
        private void ApplyLocalizationStrings()
        {
            this.Text = this.localeData.MainFormTitle;
            this.exploreButton.Text = this.localeData.ExploreLabel;
            this.nameHeader.Text = this.localeData.NameHeader;
            this.descHeader.Text = this.localeData.DescHeader;
            this.statusHeader.Text = this.localeData.StatusHeader;
            this.versionHeader.Text = this.localeData.VersionHeader;
            this.typeHeader.Text = this.localeData.TypeHeader;
            /*
            this.allLinkLabel.Text = this.localeData.LinkAll;
            this.newLinkLabel.Text = this.localeData.LinkNew;
            this.noneLinkLabel.Text = this.localeData.LinkNone;
            this.instLinkLabel.Text = this.localeData.LinkInstalled;
            this.updateLinkLabel.Text = this.localeData.LinkUpdates;
            this.statusLabel.Text = this.localeData.NoItemsSelected;
           // this.pathLabel.Text = this.localeData.InstallPathLabel;
            this.selectLabel.Text = this.localeData.SelectLabel;
      //      this.installButton.Text = String.Format(this.localeData.InstallSelectedLabel, "0");
       //     this.deleteButton.Text = String.Format(this.localeData.DeleteSelectedLabel, "0");*/
        }

        /// <summary>
        /// Initializes the settings of the app.
        /// </summary>
        private void InitializeSettings()
        {


            try{

					/*
					Settings settings = new Settings();
					String file = Path.Combine(PathHelper.GetExeDirectory(), "Config.xml");
					#if SIMACODE
					// Use the customized config file if present next to normal config file...
					if (File.Exists(file.Replace(".xml", ".local.xml"))) file = file.Replace(".xml", ".local.xml");
					#endif
					if (File.Exists(file))
					{
						settings = ObjectSerializer.Deserialize(file, settings) as Settings;
					   // PathHelper.APPS_DIR = ArgProcessor.ProcessArguments(settings.Archive);
						PathHelper.APPS_DIR = PathHelper.WorkDirectory();
					 PathHelper.WORK_DIR = PathHelper.WorkDirectory();
						PathHelper.CONFIG_ADR = ArgProcessor.ProcessArguments(settings.Config);
						PathHelper.HELP_ADR = ArgProcessor.ProcessArguments(settings.Help);
						PathHelper.LOG_DIR = ArgProcessor.ProcessArguments(settings.Logs);
						if (!this.localeOverride) this.localeId = settings.Locale;
						this.notifyPaths = settings.Paths;
					}
					if (!Directory.Exists(PathHelper.LOG_DIR))
					{
						Directory.CreateDirectory(PathHelper.LOG_DIR);
					}
					if (!Directory.Exists(PathHelper.APPS_DIR))
					{
						Directory.CreateDirectory(PathHelper.APPS_DIR);
					}
					*/


            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }



        internal void fFinishExtractArg() {this.BeginInvoke((MethodInvoker)delegate  {
	
				cbBuildType.Text  =  Data.fGetGlobalVar("wBuildAnd"); 
				cbCompiler.Text = Data.fGetGlobalVar("_wToolchain");
				cbPlatform.Text = Data.fGetGlobalVar("_sConfig_Type");
				cbArchiteture.Text  = " " + Data.fGetGlobalVar( "wArch");
        });}



        /// <summary>
        /// Initializes the list view context menu.
        /// </summary>
        private void InitializeContextMenu()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add(this.localeData.ShowInfoLabel, null, new EventHandler(this.OnViewInfoClick));
            cms.Items.Add(this.localeData.ToggleCheckedLabel, null, new EventHandler(this.OnCheckToggleClick));
            this.listView.ContextMenuStrip = cms;
        }

        internal void fEnableBuild() {this.BeginInvoke((MethodInvoker)delegate  {
             CancelButtonStopIcon();
            cbArchiteture.Enabled = false;
            cbPlatform.Enabled = false;
            cbCompiler.Enabled = false;
            tbOutput.Enabled = false;
            btOutputSet.Enabled = false;

        });}



			

[DllImport("user32.dll")]
private static extern int GetSystemMetrics(int nIndex);
private const int SM_CXEDGE = 45;
private const int SM_CYEDGE = 46;

private const int SM_CXBORDER = 5;
private const int SM_CYBORDER = 6;


		public int nHeight = 0;
		public int nWindowsBorderSize = 10;
		public int nLastState = 0;

		
	/*
        internal void fSetPosition(int x, int y,int _width, int _height,int _ClientWidth, int _ClientHeight) {
			
			
			
            try {
			  this.BeginInvoke((MethodInvoker)delegate  {
						
					int _nWinBorder = ( (_width - x) - (_ClientWidth + 	vScrollBar.Width ))/2;
					int _nTitleBarHeight = ( (_height - y) - (_ClientHeight + 	hScrollBar.Height +  _nWinBorder));
				

				//	Debug.fTrace("BorderSize: "  + SystemParameters.BorderWidth );
				//	Debug.fTrace("BorderMultiplierFactor: "  +SystemInformation.);
					int _nPosX = _width - Width - _nWinBorder - vScrollBar.Width;
					int _nPosY =  y + _nTitleBarHeight;

					Location = new Point(_nPosX, _nPosY);
		//Location = new Point(_width - Width - _nWinBorder - vScrollBar.Width, _height - Height - _nWinBorder - hScrollBar.Height);
						
	//	Location = new Point(x  - Width - nWindowsBorderSize, y + 30);
					
					int _nNewHeight = _height - y;
			//		BringToFront();
	

					int _nNewState = x * y * _width * _height;
					if(nLastState != _nNewState) {
						nLastState = _nNewState;
						fBringToFront();
					}


					if(nHeight != _nNewHeight) {
					
						Size = new Size(Width, (_height - _nWinBorder - hScrollBar.Height) - _nPosY );

				
						nHeight = _nNewHeight;
					}

				
				
			   });
            }catch(Exception ex) { }
        }
*/


	public int Cons_x = 0;
	public int Cons_y = 0;
	public int Cons_width = 0;
	public int Cons_height = 0;
	public int Cons_ClientWidth = 0;
	public int Cons_ClientHeight = 0;

		internal void fSetWorkingDir(string _sDir){this.BeginInvoke((MethodInvoker)delegate  {	
				 this.tbWorkingDir.Text =_sDir;
            workingDirToolStripMenuItem.Text = "WorkingDir: ( " +  _sDir + " )";

            });
		}
		

		public bool bPositionSet = false;

		internal void fSetPosition(int x, int y,int _width, int _height,int _ClientWidth, int _ClientHeight) {
	
            try {

			  this.BeginInvoke((MethodInvoker)delegate  {	
				if(Base.bAlive && this != null ) {
					try {
						Cons_x = x;
						Cons_y = y;
						Cons_width = _width;
						Cons_height = _height;
						Cons_ClientWidth = _ClientWidth;
						Cons_ClientHeight = _ClientHeight;
						fSetSize();
						bPositionSet = true;
					}catch(Exception ex) { }
				}  
			});
            }catch(Exception ex) { }
        }





	public int nFormWidth = 100;
	public int nLastWidth = 0;
		public int nLastExpandSize = 0;
		public double nOpacity = 0;
public static double nTimeShow = 2;
		//public double nOpacityTo = 0.97 + nTimeShow;
		public double nOpacityTo = 1.2 + nTimeShow;

        public int nTopOffset = 25 + 20;

         public int nWidowsHeight = 25;



		public void fSetSize() {
            //oEmptyForm.evend;

            if (Empty.bFromParent) {

                 Opacity = 1.0; 
                Visible = true; 
                Width = 10000;
                Height = 10000;
                	Location = new Point(0,0);
                	Size = new Size(10000, 10000);
            //    Console.WriteLine("ok");
                    return;

            }




         
         //   Application.DoEvents();

 
	
		
//Debug.fTrace("x: " + Cons_x + " y:" + Cons_y + " _width:" + Cons_width + " _height: "+Cons_height + "  _ClientWidth "+Cons_ClientWidth + "  Cons_ClientHeight:" +Cons_ClientHeight );
			
					int _nWinBorder = ( (Cons_width - Cons_x) - (Cons_ClientWidth + 	vScrollBar.Width ))/2;
					int _nTitleBarHeight = ( (Cons_height - Cons_y) - (Cons_ClientHeight + 	hScrollBar.Height +  _nWinBorder));
				


					int _nPosX = Cons_width - nFormWidth - _nWinBorder - vScrollBar.Width;

					int _nPosY =  Cons_y + _nTitleBarHeight - nTopOffset;


					
					int _nNewHeight = Cons_height - Cons_y + nTopOffset;


					int _nNewState = ( Math.Abs( Cons_x) + 1)  * ( Math.Abs(Cons_y)+1)  * ( ( Cons_width + 1) * (Cons_height + 1));
            /*
					if(bExpand) {

						nExpandSize = Cons_ClientWidth/2;
					}else {
						nExpandSize =0;
					}*/
            nExpandSize = Cons_ClientWidth - nFormWidth;



            //Debug.fTrace("_nPosX:	" + _nPosX);

            if (nLastState != _nNewState) {
							//Debug.fTrace("nLastState " + nLastState);
							//Debug.fTrace("_nNewState " + _nNewState);
						nLastState = _nNewState;
					
					
						Location = new Point(_nPosX - nExpandSize, _nPosY);
                         
                         msMenu.Focus();//Remove menu bug

						if(nHeight != _nNewHeight || nLastExpandSize != nExpandSize) {
							
							//int _nWidth = nFormWidth +  nExpandSize;
					//		nOpacity = 0;
							
                            int _nWinHeight = (Cons_height - _nWinBorder - hScrollBar.Height) - _nPosY ;
                            _nWinHeight = nWidowsHeight; ////FixedSize///////////////// TODO
							
							Size = new Size(nFormWidth +  nExpandSize, nWidowsHeight);
							nHeight = _nNewHeight;
							nLastExpandSize = nExpandSize;
						}
			//			fPutWindowsToFront();
					
					//	Opacity = 0.93;

					}


					int nCursorYpos =  (Cursor.Position.Y -(Cons_y + _nTitleBarHeight));
					
					//Click on tile bar
					if(!bFocus || nCursorYpos > 0) {nClick = 0;} 
					if(nClick > 0) {
						nClick--;
					//	Debug.fTrace("nClick");
						fPutWindowsToFront();
						fActivateConsole();
					
					}
					nOpacity += (nOpacityTo - nOpacity)/9.3;
					if(nOpacity < nTimeShow) {
	//					Visible = false;
						Opacity = 0;
					}else {	
		//				Visible = true;
						if( nOpacity - nTimeShow > 1) {
								Opacity = 1;
						}else {
							Opacity = nOpacity - nTimeShow;
						}
					}
			
			///////
			int _nSelectTopX = Cons_x+_nWinBorder;
			int _nSelectTopY = _nPosY;
			//int _nMaxWidth = Cons_ClientWidth -  Size.Width -  PipeInput.nStartSelX;
			int _nMaxWidth = Cons_ClientWidth -  PipeInput.nStartSelX;
			
			int nWidthTo = PipeInput.nEndSelX;
                 
			if(nWidthTo > _nMaxWidth){
				nWidthTo = _nMaxWidth;
			}
			int nWidthStart = PipeInput.nStartSelX;
			if(nWidthStart < 0){
				nWidthTo += nWidthStart;
				nWidthStart = 0;
			}
			if(PipeInput.bFoundSel && oSelectForm != null){
               // Console.Write("fond \r" +  PipeInput.nStartSelY + "   " + _nSelectTopX  + "            ");
				oSelectForm.Location = new Point(_nSelectTopX + nWidthStart, _nSelectTopY + PipeInput.nStartSelY + nTopOffset);
				oSelectForm.Size = new Size(nWidthTo, PipeInput.fontSize.Height );
                //Console.Write("Size \r" + Cons_ClientWidth+ "   " + Size.Width  + "  " + PipeInput.nStartSelX);
				oSelectForm.fShow();
			}
            	//oSelectForm.fShow();
          
           
		}

		public void fActivateConsole() {
//	Debug.fTrace("fActivateConsole " );
//			NativeMethods.SetForegroundWindow(NativeMethods.GetConsoleWindow());

		}
			

		public void fPutWindowsToFront() {
//	Debug.fTrace("BringToFront " );
//				BringToFront();



			this.BeginInvoke((MethodInvoker)delegate  {
		//	TopMost = true;
			//TopMost = false;
			});
		}
		public void fUnPutWindows() {
			this.BeginInvoke((MethodInvoker)delegate  {
		//		TopMost = false;
			});
		}




		public bool bFocus = false;
		public bool bFormFocus = false;
		 public void fBringToFront(bool _bFocus = true) {

			if(!_bFocus) {
				nClick  = 0;
				fUnPutWindows();
			}
	//		Debug.fTrace("_bFocus " + _bFocus);
	// Console.WriteLine();
			nLastState = 0;
			bFocus = _bFocus;
		
			//nLastState = 0;
            try {
		//		 Debug.fTrace("Focus !!!" +bFocus);
				if(_bFocus) {
					this.BeginInvoke((MethodInvoker)delegate  {
					//	if(bFormFocus != bFocus ) {
						//	bFormFocus = bFocus;
						//	if(bFocus) {
							if(!_bFocus) {
								nClick  = 0;
							}else {
							
							//	fActivateConsole();
								fPutWindowsToFront();
								fActivateConsole();
					//Thread.Sleep(1);
				//fPutWindowsToFront();
				//	 Debug.fTrace("------GUI Front");
							 }
						//	}
						//}

			
					});
				}
            }catch(Exception ex) { }
        }

		public bool bFinish = false;

		public int bActivated = 0;
		public void fActivated(int _bResult) {

			if(bActivated != _bResult) {
				bActivated = _bResult;
				if(_bResult > 0 && !bFocus) {
			//			fActivateConsole();
					
				}
			}

		}






        internal void fDisableBuild() {
            try {

          this.BeginInvoke((MethodInvoker)delegate  {

                    CancelButtonBuildIcon();
                    cbArchiteture.Enabled = true;
                    cbPlatform.Enabled = true;
                    cbCompiler.Enabled = true;
                    //tbOutput.Enabled = true;
                   btOutputSet.Enabled = true;

           });
            }catch(Exception ex) { }


        }

        #endregion

        #region Key Handling

        /// <summary>
        /// Closes the application when pressing Escape.
        /// </summary>
        protected override Boolean ProcessCmdKey(ref Message msg, Keys k)
        {
            if (k == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, k);
        }

        #endregion

        #region Event Handlers
        /*
        /// <summary>
        /// Handles the mouse wheel on hover
        /// </summary>
        public Boolean PreFilterMessage(ref Message m)
        {
            if (!Win32.IsRunningOnMono && m.Msg == 0x20a) // WM_MOUSEWHEEL
            {
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                IntPtr hWnd = Win32.WindowFromPoint(pos);
                if (hWnd != IntPtr.Zero)
                {
                    if (Control.FromHandle(hWnd) != null)
                    {
                        Win32.SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                        return true;
                    }
                    else if (this.listView != null && hWnd == this.listView.Handle)
                    {
                        Win32.SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                        return true;
                    }
                }
            }
            return false;
        }

            */
		public void fSetView() {
			try {	this.BeginInvoke((MethodInvoker)delegate  {	try {
				cbView.Items.Clear();
			//	cbView = new ComboBox();
				switch( Data.fGetGlobalVar("_wToolchain") ) {
					case "Honera/WebRT":
				//		cbView.Items.AddRange(Emscripten.aBrowser.ToArray());
					break;

					default:
					//	cbView.Items.Add("Default");
					break;
					
				}
				//cbView.SelectedIndex = 0;

          }catch(Exception ex) { }  }); }catch(Exception ex) { }
	
		}

		
		public string[] aModuleToLoad;
		public int  _nLoadModuleIndex =  0 ;
		public void fLoadModules() {

			aModuleToLoad = Data.aRequiredModule.ToArray();
			fLoadNextModule();
		}

		public void fLoadNextModule() {
			 this.BeginInvoke((MethodInvoker)delegate  {
			
			if(_nLoadModuleIndex < aModuleToLoad.Length) {
				string _sModule = aModuleToLoad[_nLoadModuleIndex];
			//Debug.fTrace("fLoadModules: " + _sModule);

				Data.oMsgBox = new Form { TopMost = true,TopLevel = true };
					var res =  MessageBox.Show(Data.oMsgBox , _sModule + " module is required. \nDownload?", "Download " + _sModule + "?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);	
				//MessageBox.Show(Data.oMsgBox , "The "  +  + " module is required to build C++", "Download LibRT?", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);	
				
				if (res == DialogResult.Yes){
					UpdateForm.Show(this, false,_sModule, true );
				}
				if (res == DialogResult.Cancel){
					_nLoadModuleIndex =  aModuleToLoad.Length; //Cancel All
				}
			
			}else {	
				//Reset?
				_nLoadModuleIndex = 0;
				aModuleToLoad = new string[0];
				Data.aRequiredModule = new List<string>();
			}
			_nLoadModuleIndex++;
    });
		}


/*
protected override void OnPaintBackground(PaintEventArgs e)
{
//empty implementation
}

*/


        /// <summary>
        /// On MainForm show, initializes the UI and the props.
        /// </summary>
        private void MainFormLoad(Object sender, EventArgs e) {


      



              //  NativeMethods.SetParent(  oEmptyForm.Handle,Handle);

            //	Http.fDownload("https://github.com/Honera/LibRT/archive/v0.0.3.zip", "test", null,null );


            //IntPtr _hwnd = NativeMethods.SetParent(Handle, Data.hConsoleInput);
            //IntPtr _hwnd = NativeMethods.SetParent( Data.hConsoleInput,Handle);
            //IntPtr _hwnd = NativeMethods.SetParent( Handle, (IntPtr)0);
            //IntPtr _hwnd = NativeMethods.SetParent(  Process.GetCurrentProcess().MainWindowHandle, Handle);
            /*
            IntPtr _hwnd = NativeMethods.SetParent( Handle, Process.GetCurrentProcess().MainWindowHandle);
            if(_hwnd == null) {
            Debug.fTrace("NULLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
            }else {
            Debug.fTrace("OKAY "  + _hwnd);
            }*/

            btnSanitize_EnabledChanged(null, null);
			btnPlay_EnabledChanged(null, null);
			cancelButton_EnabledChanged(null, null);

			cancelButton.Enabled =true;


			fActivateConsole();



//btnSanitize.Enabled = true;
//btnSanitize.Enabled = true;

//btnPlay.FlatStyle = FlatStyle.System;
//cancelButton.FlatStyle = FlatStyle.System;
//.FlatStyle = FlatStyle.System;

/*
	btnSanitize.Font = new Font("Consolas", 8,FontStyle.Regular);
*/
//	btnSanitize.Font = new Font(btnSanitize.Font, );
//btnSanitize.O


	//btnSanitize.Enabled =false;

	//	Data.oAss.fEnableBuild();
/*
		  MouseHook.Start();
            MouseHook.MouseAction += new EventHandler(Event);
			bFocus = true;
*/
	       	SysAPI.fSetMainFormPosition();
			fSetSize();

				bLoaded = true;

			if(Data.bNowBuilding) {
                fEnableBuild();
            }else {
                fDisableBuild();
            }


			fLoadModules();

/*
			if(Data.bShowLibRT) {
				Data.bShowLibRT = false;
				 UpdateLibRT.Show(this, false, Data.sShowModule );
			}*/

             lblVersion.Text  = "Version: "  + Data.sVersion;

         //   this.InitializeWebClient();
                  fResetList();

           this.TryDeleteOldTempFiles();
        //  this.downloadQueue = new Queue<DepEntry>();
       //      this.instEntries = new DepEntries();
        //     List<String> entryFiles = new List<String>();

           
                
          //   AddEntry();        
                      //  this.LoadInstalledEntries();

            cbPlatform.Items.Add("Windows");
            cbPlatform.Items.Add("Linux");
            cbPlatform.Items.Add("Android");
            cbPlatform.Items.Add("OSX");
            cbPlatform.Items.Add("iOS");
            cbPlatform.Items.Add("Web_Emsc");
            cbPlatform.Items.Add("CpcDos");

/*
            foreach (KeyValuePair<string, CompilerData> _sKey  in  Data.aCompilerData) {
               cbCompiler.Items.Add(_sKey.Key);
            }*/
		   foreach (KeyValuePair<string, ModuleData>  _oCompileKey   in  Data.aCompilerData) {
				ModuleData _oCompiler = _oCompileKey.Value;
               cbCompiler.Items.Add(_oCompiler.sAutorName);
            }            

            cbPlatform.SelectedIndex = 0;
           // cbCompiler.SelectedIndex = 0;
            cbArchiteture.SelectedIndex = 0;
         //   cbBuildType.SelectedIndex = 1;

            if(Build.sBuildAnd == "") {
				  cbBuildType.Text = "Build & Run";
                  Build.sBuildAnd = "Run";
            }
        
      //     cbCompiler.Text = Data.fGetGlobalVar("_wToolchain");
	//	   cbPlatform.Text = Data.fGetGlobalVar("_sPlatform");
	
			fFinishExtractArg();
			fSetWorkingDir(PathHelper.ExeWorkDir);


                         //Empty.fSetToParentApp(this);
        }



	 public bool fCheckForDemos() {


        string _sDemoDir = ModuleData.aMData["Honera/Demos"].sOutFolder;////BUUUUUUUUUUUUUUUUUUUUUUUUUGG on exit, thread!!?
		//	Console.WriteLine(_sDemoDir);

			if(!Directory.Exists(_sDemoDir)) {
                this.BeginInvoke((MethodInvoker)delegate {
                    var res = MessageBox.Show(this, "You have no demos, do you want update it now?", "Demos Update?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
				    if (res == DialogResult.Yes){
					    UpdateForm.Show(this, false,  "Honera/Demos");
				    }else {
					    Directory.CreateDirectory(_sDemoDir);
				    }
                });

                return false;

            } else  {
                return true;
            }
        }



        /// <summary>
        /// Opens the help when pressing help button or F1.
        /// </summary>
        private void MainFormHelpRequested(Object sender, HelpEventArgs e)
        {
            try 
            { 
                Process.Start(PathHelper.HELP_ADR); 
            }
            catch (Exception ex) 
            { 
                DialogHelper.ShowError(ex.ToString()); 
            }
        }

       

        /// <summary>
        /// Opens the help when pressing help button or F1.
        /// </summary>
        private void MainFormHelpButtonClicked(Object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.MainFormHelpRequested(null, null);
        }

        /// <summary>
        /// Save notification files to the notify paths.
        /// </summary>
        private void NotifyPaths()
        {
            try
            {
                if (this.notifyPaths == null) return;
                foreach (String nPath in this.notifyPaths)
                {
                    try
                    {
                        String path = Path.GetFullPath(ArgProcessor.ProcessArguments(nPath));
                        if (Directory.Exists(path))
                        {
                            String amFile = Path.Combine(path, ".cwc");
                            File.WriteAllText(amFile, "");
                        }
                    }
                    catch { /* NO ERRORS */ }
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// Open info file or url when clicked.
        /// </summary>
        private void OnViewInfoClick(Object sender, EventArgs e)
        {
            if (this.listView.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView.SelectedItems[0];
                if (item != null)
                {
                    DepEntry entry = item.Tag as DepEntry;
                    if (entry != null && !String.IsNullOrEmpty(entry.Info))
                    {
                        RunInEditor(entry.Info);
                    }
                }
            }
        }

        /// <summary>
        /// Toggles the check state of the item.
        /// </summary>
        private void OnCheckToggleClick(Object sender, EventArgs e)
        {
            if (this.listView.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView.SelectedItems[0];
                if (item != null) item.Checked = !item.Checked;
            }
        }

        /// <summary>
        /// Cancels the item download process.
        /// </summary>
        private void CancelButtonClick(Object sender, EventArgs e)  {
          Data.oLauchProject.fCancel();
        }



        /// <summary>
        /// Browses the archive with windows explorer.
        /// </summary>
        private void ExploreButtonClick(Object sender, EventArgs e)
        {
            try {
                Process.Start("explorer.exe", PathHelper.ExeWorkDir.Replace('/','\\'));
            } catch (Exception ex)  {
                DialogHelper.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// On All link click, selects the all items.
        /// </summary>
        private void AllLinkLabelLinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.isLoading) return;
            this.listView.BeginUpdate();
            foreach (ListViewItem item in this.listView.Items)
            {
                item.Checked = true;
            }
            this.listView.EndUpdate();
        }

        /// <summary>
        /// On None link click, deselects all items.
        /// </summary>
        private void NoneLinkLabelLinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.isLoading) return;
            this.listView.BeginUpdate();
            foreach (ListViewItem item in this.listView.Items)
            {
                item.Checked = false;
            }
            this.listView.EndUpdate();
        }

        /// <summary>
        /// On New link click, selects all new items.
        /// </summary>
        private void NewLinkLabelLinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (this.isLoading) return;
                this.listView.BeginUpdate();
                foreach (ListViewItem item in this.listView.Items)
                {
                    DepEntry entry = item.Tag as DepEntry;
                    String state = this.entryStates[entry.Id];
                    if (state == STATE_NEW) item.Checked = true;
                    else item.Checked = false;
                }
                this.listView.EndUpdate();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// On Installed link click, selects all installed items.
        /// </summary>
        private void InstLinkLabelLinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (this.isLoading) return;
                this.listView.BeginUpdate();
                foreach (ListViewItem item in this.listView.Items)
                {
                    DepEntry entry = item.Tag as DepEntry;
                    String state = this.entryStates[entry.Id];
                    if (state == STATE_INSTALLED) item.Checked = true;
                    else item.Checked = false;
                }
                this.listView.EndUpdate();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// On Updates link click, selects all updatable items.
        /// </summary>
        private void UpdatesLinkLabelLinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (this.isLoading) return;
                this.listView.BeginUpdate();
                foreach (ListViewItem item in this.listView.Items)
                {
                    DepEntry entry = item.Tag as DepEntry;
                    String state = this.entryStates[entry.Id];
                    if (state == STATE_UPDATE) item.Checked = true;
                    else item.Checked = false;
                }
                this.listView.EndUpdate();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// On bundle link click, selects all bundled items.
        /// </summary>
        private void BundleLinkLabelLinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (this.isLoading) return;
                this.listView.BeginUpdate();
                Boolean is64bit = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE", EnvironmentVariableTarget.Machine) != "x86";
                foreach (ListViewItem item in this.listView.Items)
                {
                    DepEntry entry = item.Tag as DepEntry;
                    if (Array.IndexOf(entry.Bundles, e.Link.LinkData.ToString()) != -1)
                    {
                        if (!entry.Name.Contains("(x86)") && !entry.Name.Contains("(x64)")) item.Checked = true;
                        else
                        {
                            if (!is64bit && entry.Name.Contains("(x86)")) item.Checked = true;
                            else if (is64bit && entry.Name.Contains("(x64)")) item.Checked = true;
                            else item.Checked = false;
                        }
                    }
                    else item.Checked = false;
                }
                this.listView.EndUpdate();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// Disables the item checking when downloading.
        /// </summary>
        private void ListViewItemCheck(Object sender, ItemCheckEventArgs e)
        {
            if (this.isLoading) e.NewValue = e.CurrentValue;
        }




       

        /// <summary>
        /// Handles the clicking of the info item.
        /// </summary>
        private void ListViewClick(Object sender, EventArgs e)
        {
            Point point = this.listView.PointToClient(Control.MousePosition);
            ListViewHitTestInfo hitTest = this.listView.HitTest(point);
            Int32 columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
         //   if (columnIndex == 2)
         //   {

                DepEntry entry = hitTest.Item.Tag as DepEntry;
                RunInEditor(entry.RefFile,  " -lcpp" + " -n" + entry.nLine + " -c" + entry.nColumn);


          //  }
        }

        /// <summary>
        /// Change cursor when hovering info sub item.
        /// </summary>
        private void ListViewMouseMove(Object sender, MouseEventArgs e)
        {
            Point point = this.listView.PointToClient(Control.MousePosition);
            ListViewHitTestInfo hitTest = this.listView.HitTest(point);
            if (hitTest.Item != null)
            {
                Int32 columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
                if (columnIndex == 2) this.Cursor = Cursors.Hand;
                else this.Cursor = Cursors.Default;
            }
        }


        /// <summary>
        /// Handles the drawing of the info image.
        /// </summary>
        private Image InfoImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("cwc.Resources.Information.png"));
        private Image ErrorImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("cwc.Resources.Error.png"));
        private Image MsgImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("cwc.Resources.Msg.png"));
        private Image WarningImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("cwc.Resources.Warning.png"));
        private Image FromImage = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("cwc.Resources.From.png"));
        private Image BrokenLink = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("cwc.Resources.BrokenLink.png"));
        private void ListViewDrawSubItem(Object sender, DrawListViewSubItemEventArgs e)   {
            
            if (e.Header == this.infoHeader)  {
                if (!e.Item.Selected && (e.ItemState & ListViewItemStates.Selected) == 0)  {
                    e.DrawBackground();

                }
                else if (e.Item.Selected) {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                }

                Int32 posOffsetX = (e.Bounds.Width - e.Bounds.Height) / 2;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                Image _oSelectImg = MsgImage;
                switch (e.Item.Text.ToLower()) {
                    case  "msg" :
                          _oSelectImg = MsgImage;
                    break;
                    case  "undefined" :
                          _oSelectImg = BrokenLink;
                    break;
                    case  "error" :
                          _oSelectImg = ErrorImage;
                    break;
                    case  "warning" :
                          _oSelectImg = WarningImage;
                    break;
                    case  "from" :
                          _oSelectImg = FromImage;
                    break;
                    case  "cursor" :
                          _oSelectImg = MsgImage;
                    break;
                }

                e.Graphics.DrawImage(_oSelectImg, new Rectangle(e.Bounds.X + posOffsetX, e.Bounds.Y + 1, e.Bounds.Height - 2, e.Bounds.Height - 2));
            }
            else e.DrawDefault = true;
        }


        private void ListViewDrawItem(Object sender, DrawListViewItemEventArgs e)
        {
            if ((e.State & ListViewItemStates.Selected) != 0)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
            }
            else e.DrawDefault = true; 


        }
        private void ListViewDrawColumnHeader(Object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Completes the minimized update process.
        /// </summary>
        private void CompleteMinimizedProcess()
        {
            if (this.checkOnly)
            {
                if (this.haveUpdates)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
                else Application.Exit();
            }
        }

    


        /// <summary>
        /// Populates the list view with current entries.
        /// </summary>
        private void PopulateListView()
        {
            try
            {
                this.listView.BeginUpdate();
                this.tbWorkingDir.Text  = PathHelper.ExeWorkDir;
                foreach (DepEntry entry in this.depEntries)
                {
                    ListViewItem item = new ListViewItem(entry.Name);
              
                    item.Tag = entry; /* Store for later */
                    item.SubItems.Add(entry.Version);
                    item.SubItems.Add(entry.Info);
                    item.SubItems.Add(entry.Desc);
                    item.SubItems.Add(this.GetLocaleState(STATE_NEW));
                    item.SubItems.Add(this.GetLocaleType(entry.Type));
                    this.listView.Items.Add(item);
                    this.AddToGroup(item);
                }
                if (this.appGroups.Count > 1) this.listView.ShowGroups = true;
                else this.listView.ShowGroups = false;
                this.UpdateEntryStates();
                this.UpdateLinkPositions();
                this.GenerateBundleLinks();
                this.listView.EndUpdate();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// Update the link label positions for example if the font is different size.
        /// </summary>
        private void UpdateLinkPositions()
        {
            /*
            this.allLinkLabel.Location = new Point(this.selectLabel.Bounds.Right + LINK_MARGIN, this.allLinkLabel.Location.Y);
            this.noneLinkLabel.Location = new Point(this.allLinkLabel.Bounds.Right + LINK_MARGIN, this.allLinkLabel.Location.Y);
            this.newLinkLabel.Location = new Point(this.noneLinkLabel.Bounds.Right + LINK_MARGIN, this.allLinkLabel.Location.Y);
            this.instLinkLabel.Location = new Point(this.newLinkLabel.Bounds.Right + LINK_MARGIN, this.allLinkLabel.Location.Y);
            this.updateLinkLabel.Location = new Point(this.instLinkLabel.Bounds.Right + LINK_MARGIN, this.allLinkLabel.Location.Y);
            */
        }

        /// <summary>
        /// Generates the bundle selection links.
        /// </summary>
        private void GenerateBundleLinks()
        {
            /*
            LinkLabel prevLink = this.updateLinkLabel;
            List<String> bundleLinks = new List<String>();
            foreach (DepEntry entry in this.depEntries)
            {
                foreach (String bundle in entry.Bundles)
                {
                    if (!bundleLinks.Contains(bundle))
                    {
                        LinkLabel linkLabel = new LinkLabel();
                        linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                        linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.BundleLinkLabelLinkClicked);
                        linkLabel.Location = new Point(prevLink.Bounds.Right + LINK_MARGIN, this.allLinkLabel.Location.Y);
                        linkLabel.Links[0].LinkData = bundle;
                        linkLabel.LinkColor = Color.Green;
                        linkLabel.AutoSize = true;
                        linkLabel.Text = bundle;
                        bundleLinks.Add(bundle);
                        this.Controls.Add(linkLabel);
                        prevLink = linkLabel;
                    }
                }
            }
            */
        }

        /// <summary>
        /// Adds the entry into a new or existing group.
        /// </summary>
        private void AddToGroup(ListViewItem item)
        {
            try
            {
                DepEntry entry = item.Tag as DepEntry;
                if (this.appGroups.ContainsKey(entry.Group))
                {
                    ListViewGroup lvg = this.appGroups[entry.Group];
                    item.Group = lvg;
                }
                else
                {
                    ListViewGroup lvg = new ListViewGroup(entry.Group);
                    this.appGroups[entry.Group] = lvg;
                    this.listView.Groups.Add(lvg);
                    item.Group = lvg;
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// Creates a temporary file with the given extension.
        /// </summary>
        private String GetTempFileName(String file, Boolean unique)
        {
            try
            {
                Int32 counter = 0;
                String tempDir = Path.GetTempPath();
                String fileName = Path.GetFileName(file);
                String tempFile = Path.Combine(tempDir, "cwc_" + fileName);
                if (!unique) return tempFile;
                while (File.Exists(tempFile))
                {
                    counter++;
                    tempFile = Path.Combine(tempDir, "cwc_" + counter + "_" + fileName);
                }
                return tempFile;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Tries to delete old temp files.
        /// </summary>
        private void TryDeleteOldTempFiles()
        {
            String path = Path.GetTempPath();
            String[] oldFiles = Directory.GetFiles(path, "cwc_*.*");
            foreach (String file in oldFiles)
            {
                try { File.Delete(file); }
                catch { /* NO ERRORS */ }
            }
        }

        /// <summary>
        /// Try to delete old entry directory
        /// </summary>
        private void TryDeleteEntryDir(DepEntry entry)
        {
            String folder = Path.Combine(PathHelper.WORK_DIR, entry.Id);
            // Sometimes we might get "dir not empty" error, try 10 times...
            for (Int32 attempts = 0; attempts < 10; attempts++)
            {
                try
                {
                    if (Directory.Exists(folder)) Directory.Delete(folder, true);
                    return;
                }
                catch (IOException) { Thread.Sleep(50); }
            }
            throw new Exception(this.localeData.DeleteDirError + folder);
        }

     [System.Runtime.InteropServices.DllImport("User32.dll")]
      private static extern bool SetForegroundWindow(IntPtr handle);
      [System.Runtime.InteropServices.DllImport("User32.dll")]
      private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
      [System.Runtime.InteropServices.DllImport("User32.dll")]
      private static extern bool IsIconic(IntPtr handle);
        /// <summary>
        /// Runs an executable process.
        /// </summary>
        /// 





     ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      /// 

                 ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      /// 

     ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      ///      /// 





          public static   Process nppProcess = null;
          public static Process firstProcess = null;


        public void AppQuit() { this.BeginInvoke((MethodInvoker)delegate  {	
		        try {		Close(); }catch(Exception ex) { }

		/*
				if(firstProcess != null && !firstProcess.HasExited) { //If already exist
        
					firstProcess.CloseMainWindow();
               
				}*/
				
		

		  //  MessageBox.Show("The form is now closing.", "Close Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
/*
            if(firstProcess != null && !firstProcess.HasExited) { //If already exist
                try {
                firstProcess.CloseMainWindow();
                }catch(Exception ex) { }
            }
*/
       		
			}); }
     


        public static void fFindExistantNotepad(){
             Process[] localByName = Process.GetProcessesByName("notepad++");
            foreach (Process _oProc in  localByName ) {
               Debug.fPrint(_oProc.MainWindowTitle);
               Debug.fPrint(_oProc.MainModule.FileName);
            }
               Debug.fPrint("----");
                 Process[] localByName2 = Process.GetProcesses();
            foreach (Process _oProc in  localByName2 ) {
               Debug.fPrint(_oProc.MainWindowTitle);
               Debug.fPrint(_oProc.MainModule.FileName);
            }
        }


        public static void RunInEditor(String file, string Arg = ""){

		//	this.BeginInvoke((MethodInvoker)delegate  {
           Debug.fTrace(" Lauch Notpad ");
            try  {
                if(File.Exists(file)) { 

                    //Find already opened notepad
                    fFindExistantNotepad();



                    if(firstProcess != null && !firstProcess.HasExited) { //If already exist
                        IntPtr handle = firstProcess.MainWindowHandle;
                   
                      if (IsIconic(handle)) {
                            ShowWindow(handle, 9);
                        }
                       SetForegroundWindow(handle);
                    }
                    nppProcess = new Process();

                    string _sMultiInstance  ="";
                    if(firstProcess == null || firstProcess.HasExited) {
						firstProcess = nppProcess;
						_sMultiInstance =  "-multiInst "; //Other notepad access will not affect this one
					
						try { 
							XmlManager.loadNppConfig();
						} catch (Exception ex) { }
                    }
					 

                    nppProcess.StartInfo.FileName = PathHelper.ToolDir +  "npp/notepad++.exe";

                    nppProcess.StartInfo.Arguments = _sMultiInstance + Arg  + " " + file; //Restore quotes
                    nppProcess.Start();

                }else{

					if( Directory.Exists(file)) { 
				       Process.Start("explorer.exe", file.Replace('/','\\'));
						
					}

				}
                        

            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
      //  });
	}


public void OnApplicationExit(object sender, EventArgs e) {

    // When the application is exiting, write the application data to the
    // user file and close it.
	//	MessageBox.Show("EXIT : "  );

    try {
        // Ignore any errors that might occur while closing the file handle.
     
    } catch {}

}


        
        protected override void OnClosed(EventArgs e) {
		    //MessageBox.Show("The form is now closing.", "Close Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            Base.bAlive = false;
            /*
            MessageBox.Show("The form is now closing.", 
                "Close Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);*/
            base.OnClosed(e);


        }



        /// <summary>
        /// Gets the locale string for state
        /// </summary>
        private String GetLocaleState(String state)
        {
            if (state == STATE_INSTALLED) return this.localeData.StateInstalled;
            else if (state == STATE_UPDATE) return this.localeData.StateUpdate;
            else return this.localeData.StateNew;
        }

        /// <summary>
        /// Gets the locale string for type
        /// </summary>
        private String GetLocaleType(String type)
        {
            if (type == TYPE_LINK) return this.localeData.TypeLink;
            else if (type == TYPE_EXECUTABLE) return this.localeData.TypeExecutable;
            else return this.localeData.TypeArchive;
        }

        /// <summary>
        /// Checks if entry is an executable.
        /// </summary>
        private Boolean IsExecutable(DepEntry entry)
        {
            return entry.Type == TYPE_EXECUTABLE;
        }

        /// <summary>
        /// Checks if entry is an executable.
        /// </summary>
        private Boolean IsLink(DepEntry entry)
        {
            return entry.Type == TYPE_LINK;
        }

        #endregion

        #region Entry Management

        /// <summary>
        /// Downloads the entry config file.
        /// </summary>
        private void LoadEntriesFile()
        {
            try
            {
                if (PathHelper.CONFIG_ADR.StartsWith("http"))
                {



                   WebClient client = new WebClient();
client.Proxy = GlobalProxySelection.GetEmptyWebProxy();
//webClient.Proxy = null;


                    this.entriesFile = Path.GetTempFileName();
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.EntriesDownloadCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.DownloadProgressChanged);
                    client.DownloadFileAsync(new Uri(PathHelper.CONFIG_ADR), this.entriesFile);
                    this.statusLabel.Text = this.localeData.DownloadingItemList;
                }
                else
                {
                    this.entriesFile = PathHelper.CONFIG_ADR;
                    Object data = ObjectSerializer.Deserialize(this.entriesFile, this.depEntries, MainForm.EXPOSED_GROUPS);
                    this.statusLabel.Text = this.localeData.ItemListOpened;
                    this.depEntries = data as DepEntries;
                    this.PopulateListView();
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
            finally
            {
                this.CompleteMinimizedProcess();
            }
        }

        /// <summary>
        /// When entry config is loaded, populates the list view.
        /// </summary>
        private void EntriesDownloadCompleted(Object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                Boolean fileExists = File.Exists(this.entriesFile);
                Boolean fileIsValid = File.ReadAllText(this.entriesFile).Length > 0;
                if (e.Error == null && fileExists && fileIsValid)
                {
                    this.statusLabel.Text = this.localeData.DownloadedItemList;
                    Object data = ObjectSerializer.Deserialize(this.entriesFile, this.depEntries, MainForm.EXPOSED_GROUPS);
                    this.depEntries = data as DepEntries;
                    this.PopulateListView();
                }
                else this.statusLabel.Text = this.localeData.ItemListDownloadFailed;
                TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.NoProgress);
                this.progressBar.Value = 0;
            }
            catch (Exception ex)
            { 
                DialogHelper.ShowError(ex.ToString());
            }
            finally
            {
                this.CompleteMinimizedProcess();
                try { File.Delete(this.entriesFile); }
                catch { /* NO ERRORS*/ }
            }
        }

        /// <summary>
        /// Adds the currently selected entries to download queue.
        /// </summary>
        private void AddEntriesToQueue()
        {
            try
            {
                this.downloadQueue.Clear();
                foreach (ListViewItem item in this.listView.CheckedItems)
                {
                    DepEntry entry = item.Tag as DepEntry;
                    String state = this.entryStates[entry.Id];
                    if (state == STATE_NEW || state == STATE_UPDATE)
                    {
                        this.downloadQueue.Enqueue(entry);
                    }
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }

   

        /// <summary>
        /// Updates the progress bar for individual downloads.
        /// </summary>
        private void DownloadProgressChanged(Object sender, DownloadProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
            TaskbarProgress.SetValue(this.Handle, e.ProgressPercentage, 100);
        }

      

   

    
      

        /// <summary>
        /// Saves the entry info into a xml file.
        /// </summary>
        private void SaveEntryInfo(String path, DepEntry entry)
        {
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                ObjectSerializer.Serialize(Path.Combine(path, entry.Version + ".xml"), entry);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }












    ///    /// ////////////////////   /// ////////////////////   /// ////////////////////   /// ////////////////////
        ///    /// ////////////////////   /// ////////////////////   /// ////////////////////   /// ////////////////////
    ///    /// ////////////////////   /// ////////////////////   /// ////////////////////   /// ////////////////////

         public static int nLastIndex = -1;
         public static string sFound  = "";
         public static string sExtract  = "";
         public void fIniExtractDelemiter(string _sString, int _nLastIndex) {
            sExtract = _sString;
            nLastIndex = _nLastIndex;
        }
        public bool fExtractNextDelemiter() {
             //Get Type 
             sFound = "";
            bool _bResult = false;
            nLastIndex++;
            int _nNextIndex = sExtract.IndexOf(":", nLastIndex );

			/////If is not a path delimiter/////
			if(_nNextIndex + 1 < sExtract.Length) {
				if(sExtract[_nNextIndex + 1 ] == '/' || sExtract[_nNextIndex + 1 ] == '\\' ) {
					 _nNextIndex = sExtract.IndexOf(":", _nNextIndex + 1 );
				}
			}
			////////////////
         
            if(_nNextIndex != -1) {
                 sFound =  sExtract.Substring(nLastIndex,   _nNextIndex - nLastIndex).Trim();
                _bResult = true;
            }
            if(_nNextIndex != -1) {
                nLastIndex = _nNextIndex;
            }
            return _bResult;
        }


        static public string _sLastFile = "";
        static public uint _nLastLine = 0;
        static public uint _nLastCol = 0;


         public  List<DepEntry > aFrom  = new List<DepEntry>();
  

          public void fResetList() {
            depEntries = new DepEntries();
            entryStates = new Dictionary<String, String>();
            appGroups = new Dictionary<String, ListViewGroup>();
                listView.BeginUpdate();
              listView.Items.Clear();
              listView.Update(); // In case there is databinding
            listView.Refresh(); // Redraw items



         //   listView = new ListViewDoubleBuffer();  
         
        //    listView.Clear();

           

       //    listView.Items.Clear();
      //      listView.Update();

            aFrom.Clear();


        
            listView.EndUpdate();


        }

        public void StartBuild() {this.BeginInvoke((MethodInvoker)delegate  {
            fResetList();

       
            /*
           listView.Clear();
            appGroups.Clear(); */
        });}


        int nRetWordIndex = 0;
        public string fGetNextWord(string _sSrc, int _nStartIndex) {
             if(_nStartIndex >= 0 && _nStartIndex < _sSrc.Length) {
                _sSrc = _sSrc.Substring(_nStartIndex, _sSrc.Length - _nStartIndex);
                _sSrc = _sSrc.Trim();
                int _nIndexSpace = _sSrc.IndexOf(' ');
                nRetWordIndex = _nIndexSpace;
                if(_nIndexSpace != -1) {
                    return _sSrc.Substring(0, _nIndexSpace);
                }
            }
            return "";
        }


        string sCurrentCommand = "";

       public void fAddSanitized( string _sItem) {
       
            string _sFile = "";

           // Debug.fTrace("---- asss : "  + _sItem + " : " + _sItem.Length);
            if(_sItem.Length == 9 && _sItem.Substring(0,8) == "~~Dr.M~~") {
                      sCurrentCommand += " " + _sItem;
            }else {

                string _sFirstWord = fGetNextWord(_sItem,0);
                switch(_sFirstWord)  {
                    case "~~Dr.M~~":
                        if(nRetWordIndex != -1) {
                              string _sSecondWord = fGetNextWord(_sItem,nRetWordIndex);
                                   if(nRetWordIndex != -1) {
                                          
                                                 switch(_sSecondWord)  {
                                                       case "Error":
                                                           if(nRetWordIndex < 0) { nRetWordIndex = 0; }
                                                           int _nNext =  _sItem.IndexOf(':', nRetWordIndex);
                                                           if(_nNext < 0) {_nNext = 0;}
                                                                sCurrentCommand = _sSecondWord + " " + _sItem.Substring(_nNext,  _sItem.Length - _nNext);
                                                           return;
                                  
                                                        default:
                                                            sCurrentCommand += _sItem + "|";
                                                         return;
                                                   }
  
                                    }
                    } 
                 
                    break;
                    default:
                        return;
                }
                return;
            }


            ////////////// Analyse command


            string[] _aList = sCurrentCommand.Split('|');
            if(_aList.Length > 0) {
                  listView.BeginUpdate();

                string _sMainCommand = _aList[0];

             //   Debug.fTrace("aaaaaaaaa Cur : " + sCurrentCommand);

                string[] _aCmd = sCurrentCommand.Split(new Char [] {'[' , '+' });
                if(_aCmd.Length > 1) {
                    string _sLine = "";
                    uint _nLine = 0;
                     string _sCmd = _aCmd[0].Trim();
                    string _sMainFile = _aCmd[1];

                    

                    int _nNumSep = _sMainFile.IndexOf(':', 2);

                    try{
                    _sLine = _sMainFile.Substring(_nNumSep,_sMainFile.Length - _nNumSep);
                    _sLine = _sLine.Substring(1, _sLine.IndexOf(']')-1);
                    UInt32.TryParse(_sLine, out _nLine);
                    _sMainFile = _sMainFile.Substring(0, _nNumSep);
                    }catch(Exception Ex) { }
                        

                          DepEntry _oEntry =  new DepEntry();
                    _oEntry.Info = fGetNextWord( _sCmd,0);

                    _sCmd = _sCmd.Replace("~~Dr.M~~ # 0 ","        (") + ")  [" + _sMainFile + "]";

                    _oEntry.Name = "";
                    _oEntry.Desc = _sCmd;
                       _oEntry.nLine = _nLine;
                       _oEntry.nColumn = 0;
                    _oEntry.Group = _sMainFile;
                    _oEntry.RefFile = _sMainFile;

                    depEntries.Add(_oEntry); fAddEntry(_oEntry, false);

                    //Callstack
                  for(int i = 1; i < _aList.Length; i++) {
                          try{

                         DepEntry  _oSubEntry =  new DepEntry();
                            _nLine = 0;

                         _oSubEntry.Info = "from";

                           string _sCallStack = _aList[i];
                            string _sWord = fGetNextWord(_sCallStack,0);
                            if(_sWord ==  "~~Dr.M~~") {
                               _sCallStack  = _sCallStack.Substring(9);
                            }
                            _sWord = fGetNextWord(_sCallStack,0);
                            if(_sWord ==  "Note:") {
                               _oSubEntry.Info = "Msg";
                               _sCallStack  = _sCallStack.Substring(5);
                            }

                       
                            string _sCsCmd = _sCallStack;
                            string _sCsFile = _sMainFile;
                            string[] _aCallStack = _sCallStack.Split('[');
                            if(_aCallStack.Length > 1) {
                                _sCsCmd = _aCallStack[0];
                                _sCsFile = _aCallStack[1];
                            }

    
                          _nNumSep = _sCsFile.IndexOf(':', 2);

                          try{
                            _sLine = _sCsFile.Substring(_nNumSep);
                            _sLine = _sLine.Substring(1, _sLine.IndexOf(']')-1);
                            UInt32.TryParse(_sLine, out _nLine);
                            _sCsFile = _sCsFile.Substring(0, _nNumSep);
                           }catch(Exception Ex) { }
                       
                          //_sItem.in Error #1
                 
                        _oSubEntry.Name = "";
                        _oSubEntry.Desc = _sCallStack;
                      //  _oSubEntry.Desc = _sCsCmd;
                        _oSubEntry.Group = _sMainFile;
                        _oSubEntry.RefFile = _sCsFile;
                 //           Debug.fTrace("aaaaaaaaaaaasdsdsa--: " + _sCsFile);
                          _oSubEntry.nLine = _nLine;
                        _oSubEntry.nColumn = 0;


                       depEntries.Add(_oSubEntry); fAddEntry(_oSubEntry, false);

                         }catch(Exception Ex) { }
                    }

                }

                
                     listView.EndUpdate();
            }
        }




        public void fAddItem( string _sItem) {

            try { 

			string _sDesc = "";
           int _nFromIndex = _sItem.IndexOf("included from ");//In file included from
           if(_nFromIndex != -1) {
                _nFromIndex += 5; //(" from ")
            }
/*
else {
				_nFromIndex += 4; //Remove file disk location ex: E:/ssssxxx if output begin with that
			}*/
            

            string _sFile = _sLastFile;
            uint _nLine = 0;
            uint _nColumn = 0;
            string sType = "Msg";


			
            fIniExtractDelemiter(_sItem, _nFromIndex);
			

  //Debug.fTrace("ITEM------- : " + _sItem + " Form " + _nFromIndex);

            if(fExtractNextDelemiter()) { //Get file
                _sFile = sFound;
                _sLastFile = sFound;
                _nLastLine = 0;
                _nLastCol = 0;

                  // Debug.fTrace("FILEFOUND------- : " + sFound);


				
                if(fExtractNextDelemiter()) { ///Get line
                    UInt32.TryParse(sFound, out _nLine);
                     _nLastLine = _nLine;

                               // Debug.fTrace("LINEFOUND -------- : " + sFound);

                    if(fExtractNextDelemiter()) { //Get Column

                        if(UInt32.TryParse(sFound, out _nColumn)) {
                            _nLastCol = _nColumn;
                            if(fExtractNextDelemiter()) { //Get Type
                                 sType = sFound;
                            }
                        }else {
                
                            sType = sFound;
                        }
                    }else { //No colom like undefined ref, get only next word
                         string _sWord = fGetNextWord(_sItem, nLastIndex);
                        // Debug.fTrace("--------- : " + _sWord);
                          sType = _sWord;
                    }
					
        
						_sDesc = _sItem.Substring( _sItem.IndexOf(":",nLastIndex) +1);
  
					

                }
            }
           
            if(_sFile[0] == '.' && _sFile[1] != '.') {
                _sFile = _sFile.Substring(1);
            }
             if(_sFile[0] == '/') {
                _sFile = _sFile.Substring(1);
            }

            DepEntry _oEntry =  new DepEntry();

            int _nCursorIndex = _sItem.IndexOf("^");//In file included from
            if(_nCursorIndex != -1){
                  sType = "cursor";
              }


             if(_nFromIndex != -1) {
                 sType = "from";
                 aFrom.Add(_oEntry);

            }else { //Restore from list
               foreach( DepEntry _oFrom in aFrom) {
                      _oFrom.Group = _sFile;
                    depEntries.Add(_oFrom);
                    fAddEntry(_oFrom);
                }
               aFrom.Clear();
            }
           


       //     depEntries.Clear();

			if(_nLine == 0) {
				_sDesc = _sItem;
			}else {
				//_sDesc ="(" + _sFile.Substring( _sFile.LastIndexOf("/") + 1) + ") " + _sDesc; //Include file name
			}


            _oEntry.Name = "";
            _oEntry.Desc = _sDesc;
            _oEntry.Group = _sFile;
            _oEntry.RefFile = _sFile;


           //   _oEntry.Type = "Executable";
            _oEntry.Info = sType;
            _oEntry.Version = "6.0.5";
            _oEntry.nLine = _nLine;
            _oEntry.nColumn = _nColumn;
          

            if(_nFromIndex == -1) {
                 depEntries.Add(_oEntry);
                 fAddEntry(_oEntry);
                     //     Console.Write("----1166661" + _sItem);
          }

            }catch(Exception ex) { }



        }





        
        public void fAddEntry( DepEntry _oEntry, bool _bAutoUpdate = true) {this.BeginInvoke((MethodInvoker)delegate  {


               ///////////////////////////////////////
     
            ListViewItem item = new ListViewItem(_oEntry.Info);
            item.Tag = _oEntry; /* Store for later */

            if(_oEntry.nLine != 0) {
                 item.SubItems.Add( _oEntry.nLine.ToString());
            }else{
              item.SubItems.Add("");
               _oEntry.nLine = _nLastLine;
            }
            if(_oEntry.nColumn != 0) {
                  item.SubItems.Add( _oEntry.nColumn.ToString());
            }else{
                  item.SubItems.Add("");
                 _oEntry.nColumn = _nLastCol;
            }


             item.SubItems.Add(_oEntry.Desc);

        //    item.SubItems[3].ForeColor = Color.Red;
           // item.Text = "aaaaaaaaa";

           //   item.SubItems.Add(_oEntry.Name);

            
       /*
            item.SubItems.Add(_oEntry.Version);


            item.SubItems.Add(GetLocaleState(STATE_NEW));
            item.SubItems.Add(GetLocaleType(_oEntry.Type));
           
            */




             


            fStyleItem(item);









            item.UseItemStyleForSubItems = false;


            if(_bAutoUpdate) {
            listView.BeginUpdate();
            }

           

            listView.Items.Add(item);
            AddToGroup(item);


            if(_bAutoUpdate) {
            listView.EndUpdate();
            }
        });}


    
         public void fStyleItem(  ListViewItem item ) {
              switch (item.SubItems[0].Text.ToLower()) {
                    case  "msg" :

                    break;

                    case  "undefined" :
                          item.SubItems[3].ForeColor = Color.DarkMagenta;
                    break;

                    case  "error" :
                          item.SubItems[3].ForeColor = Color.DarkRed;
                    break;
                    case  "warning" :
         
                            item.SubItems[3].ForeColor = Color.DarkGoldenrod;
                    break;
                    case  "from" :
                        
                         item.SubItems[3].ForeColor = Color.DarkGray;
                    break;
                    case  "cursor" :

                          item.SubItems[3].ForeColor = Color.Green;
                           //e.Item.SubItems[3].Font = new Font( e.Item.SubItems[3].Font.FontFamily,  e.Item.SubItems[3].Font.Size, FontStyle.Bold); //Super slow
                    break;
                }

        }





    ///    /// ////////////////////   /// ////////////////////   /// ////////////////////   /// ////////////////////
        /// /////////////////////////////////////////
       ///    /// ////////////////////   /// ////////////////////   /// ////////////////////   /// ////////////////////

            /*
        public void AddEntry() {

          //  Thread.Sleep(100);
             DepEntry Entry1 =  new DepEntry();
            Entry1.Name = "Assssss";
            Entry1.Info = "Asadasdsds";
            Entry1.Group = "File/lalal/test.cpp";
            Entry1.Type = "Executable";

            Entry1.Version = "6.0.5";

             this.depEntries.Add(Entry1);


                         DepEntry Entry2 =  new DepEntry();
            Entry2.Name = "Ass2";
            Entry2.Info = "Asadasdsds2";
            Entry2.Group = "File/lalal/test2.cpp";
            Entry2.Type = "Executable";
            Entry2.Id = "6";
            Entry2.Version = "6.0.5";
                         this.depEntries.Add(Entry2);

    





          //   this.instEntries.Add(Entry);

            PopulateListView();
         //       this.CompleteMinimizedProcess();
        //    this.UpdateEntryStates();
       //         this.UpdateButtonLabels();
      
        }

        */


        /// <summary>
        /// Reads the xml entry files from the archive.
        /// </summary>
        private void LoadInstalledEntries()
        {

            /*
            try
            {
      


                
                String[] entryDirs = Directory.GetDirectories(PathHelper.WORK_DIR);

                foreach (String dir in entryDirs)
                {
					if(dir != "updater") {

						foreach ( string subFile in Directory.GetFiles( dir, "*.xml")){

							string _sFileName = Path.GetFileName(subFile);
							if(_sFileName[0] >= '0' && _sFileName[0] <= '9' ) { //Must be a version number
								
									entryFiles.Add( subFile );
							}
						}
						
						//entryFiles.AddRange(Directory.GetFiles(dir, "*.xml"));
					}
                }

                foreach (String file in entryFiles)
                {
                    Object data = ObjectSerializer.Deserialize(file, new DepEntry());
                    this.instEntries.Add(data as DepEntry);
                }

            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }*/
        }

        /// <summary>
        /// Updates the entry states of the all items.
        /// </summary>
        private void UpdateEntryStates()
        {
            try
            {
                this.listView.BeginUpdate();

                foreach (ListViewItem item in this.listView.Items)
                {
                    DepEntry dep = item.Tag as DepEntry;
                    item.UseItemStyleForSubItems = false;

                    /*
                    item.SubItems[4].ForeColor = SystemColors.ControlText;
                    item.SubItems[4].Text = this.GetLocaleState(STATE_NEW);
                    this.entryStates[dep.Id] = STATE_NEW;
                    */
                    /*
                    foreach (DepEntry inst in this.instEntries)
                    {
                       // if (dep.Id == inst.Id)
                       // {
                            Color color = Color.Green;
                            String state = STATE_INSTALLED;
                            String text = this.GetLocaleState(STATE_INSTALLED);
                            if (this.CustomCompare(dep, inst) > 0 || (dep.Version == inst.Version && dep.Build != inst.Build))
                            {
                                this.haveUpdates = true;
                                text = this.GetLocaleState(STATE_UPDATE);
                                state = STATE_UPDATE;
                                color = Color.Orange;
                            }
                            this.entryStates[inst.Id] = state;
                            item.SubItems[4].ForeColor = color;
                            item.SubItems[4].Text = text;
                            // If we get an exact match, we don't need to compare more...
                            if (dep.Version == inst.Version && dep.Build == inst.Build)
                            {
                                break;
                            }
                       // }
                    }*/
                    

                }
                this.listView.EndUpdate();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }






        /// <summary>
        /// Compare version numbers
        /// </summary>
        private Int32 CustomCompare(DepEntry dep, DepEntry inst)
        {
            try
            {
                String[] v1 = dep.Version.Replace("+", ".").Split('.');
                String[] v2 = inst.Version.Replace("+", ".").Split('.');
                for (Int32 i = 0; i < v1.Length; i++)
                {
                    try
                    {
                        Int32 t1 = Convert.ToInt32(v1[i]);
                        Int32 t2 = Convert.ToInt32(v2[i]);
                        Int32 comp = t1.CompareTo(t2);
                        if (comp > 0) return 1;
                        else if (comp < 0) return -1;
                    }
                    catch {};
                }
                return 0;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Verifies the checksum (MD5 in hex) of the file.
        /// </summary>
        private Boolean VerifyFile(String checksum, String file)
        {
            try 
            {
                using (MD5 md5 = MD5.Create())
                {
                    using (Stream stream = File.OpenRead(file))
                    {
                        String hex = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                        return hex.ToLower() == checksum.ToLower();
                    }
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Executes the next setup command from queue.
        /// </summary>
        private void RunEntrySetup(String path, DepEntry entry)
        {
            try
            {
                if (!String.IsNullOrEmpty(entry.Cmd))
                {
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    String data = ArgProcessor.ProcessArguments(entry.Cmd);
                    for (Int32 i = 0; i < entry.Urls.Length; i++)
                    {
                        String url = entry.Urls[i];
                        if (entry.Temps.ContainsKey(url))
                        {
                            String index = i.ToString();
                            String temp = entry.Temps[url];
                            data = data.Replace("$URL{" + index + "}", url);
                            data = data.Replace("$TMP{" + index + "}", temp);
                        }
                    }
                    String cmd = Path.Combine(path, entry.Version + ".cmd");
                    File.WriteAllText(cmd, data);
                    Process process = new Process();
                    process.StartInfo.FileName = cmd;
                    process.EnableRaisingEvents = true;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(cmd);
                    process.Exited += delegate(Object sender, EventArgs e)
                    {
                        try { File.Delete(cmd); }
                        catch { /* NO ERRORS */ };
                    };
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }

        #endregion

        #region Scaling Helpers

        /// <summary>
        /// Current scale of the form.
        /// </summary>
        private Double curScale = Double.MinValue;

        /// <summary>
        /// Resizes based on display scale.
        /// </summary>
        public Int32 ScaleValue(Int32 value)
        {
            return (Int32)(value * GetScale());
        }

        /// <summary>
        /// Gets the current display scale.
        /// </summary>
        public Double GetScale()
        {
            if (curScale != Double.MinValue) return curScale;
            using (Graphics g = Graphics.FromHwnd(this.Handle))
            {
                curScale = g.DpiX / 96f;
            }
            return curScale;
        }

        #endregion

        private void listView_SelectedIndexChanged(object sender, EventArgs e) {

        }



        private void pathLabel_Click(object sender, EventArgs e) {

        }

        private void listView_KeyPress(object sender, KeyPressEventArgs e) {
            /*
             if (sender != listView) return;

            if (e.Control && e.KeyCode == Keys.C)
                CopySelectedValuesToClipboard();*/
        }

        private void CopySelectedValuesToClipboard()  {
               // var builder = new StringBuilder();
            //   this.BeginInvoke((MethodInvoker)delegate  {

               string _sCopy = "";
                foreach (ListViewItem item in listView.SelectedItems) {
                      _sCopy += item.SubItems[3].Text + "\n";
                  //  builder.AppendLine(item.SubItems[3].Text);
               }

               // Clipboard.SetText(builder.ToString());
               clipboardSetText(_sCopy.Substring(0, _sCopy.Length-1));
             //  clipboardSetText(_sCopy);
            //     });

        }

    protected void clipboardSetText(string inTextToCopy)
    {
        var clipboardThread = new Thread(() => clipBoardThreadWorker(inTextToCopy));
        clipboardThread.SetApartmentState(ApartmentState.STA);
        clipboardThread.IsBackground = false;
        clipboardThread.Start();
    }
    private void clipBoardThreadWorker(string inTextToCopy)
    {
       Clipboard.SetText(inTextToCopy);
    }


        private void listView_KeyUp(object sender, KeyEventArgs e) {
                if (sender != listView) return;

                if (e.Control && e.KeyCode == Keys.C) {
                    CopySelectedValuesToClipboard();
            }
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void lbArchiteture_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void btnEdit_Click(object sender, EventArgs e) {
            btnEdit.Enabled = false;
            CompilerConfig oConfigForm = new CompilerConfig(  cbCompiler.SelectedItem.ToString()  );
         //   oConfigForm.StartPosition = FormStartPosition.CenterParent;
                 oConfigForm.FormClosing += fCompilerConfigClosing;
            cbCompiler.Enabled = false;
           // oConfigForm.ShowDialog(this);

          //  cbCompiler.StartPosition = FormStartPosition.Manual;
       //    oConfigForm.Location = new Point( Left  + Width / 2 - oConfigForm.Width / 2, Top + Height / 2  - oConfigForm.Height / 2);
           oConfigForm.Location = new Point( Cons_x  + (Cons_width-Cons_x) / 2 - oConfigForm.Width / 2,  Cons_y  + (Cons_height-Cons_y) / 2 - hScrollBar.Height - oConfigForm.Height / 2);
			

			
            oConfigForm.Show();


        }

         private void fCompilerConfigClosing(Object sender, FormClosingEventArgs e){
            btnEdit.Enabled = true;
            if(Data.bNowBuilding == false){
                cbCompiler.Enabled = true;
            }

        }

        private void btnWorkingDir_Click(object sender, EventArgs e) {

             tbWorkingDir.Text = CompilerConfig.fOpenFolderBrowsing( tbWorkingDir.Text);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
     
         

        }


        public static int nCbLastIndex = -1;

        private void cbCommands_SelectedIndexChanged(object sender, EventArgs e) {
            if(cbCommands.Focused) {
                 SendKeys.Send ("{RIGHT}");  // <<-- Workaround
            }
          lblCommand.Text = cbCommands.SelectedIndex.ToString();


           /// Debug.fTrace("SelectedIndex: " + cbCommands.SelectedIndex.ToString() +  " :  " + nCbLastIndex);

       //     btnSend.Focus();
         //   cbCommands.Focus();

            /*
            if( cbCommands.SelectedItem   != null && cbCommands.SelectedIndex != nCbLastIndex) {

            
                string _sItem = cbCommands.SelectedText;
             //   
                // cbCommands.SelectedIndex = -1;
             //  cbCommands.Text = _sItem;
             //cbCommands.SelectionLength = 0;
               // cbCommands.SelectionStart = 0;
                //cbCommands.SelectionLength = 0;
                  nCbLastIndex = cbCommands.SelectedIndex;
                  
                cbCommands.SelectedItem = null;
             //  cbCommands.Text = _sItem;
               cbCommands.Items[0] = "AAa";
     
            }*/

        }

        private void btOutputSet_Click(object sender, EventArgs e) {
             tbOutput.Text = CompilerConfig.fDialogExeFile(PathHelper.ExeWorkDir , tbOutput.Text);
        }


        private void tbOutput_TextChanged(object sender, EventArgs e) {
            if(File.Exists(PathHelper.ExeWorkDir +   tbOutput.Text) ) {
                btnPlay.Enabled = true;
                btnSanitize.Enabled = true;
            }else {
                btnPlay.Enabled = false;
                btnSanitize.Enabled = false;
            }
        }

        private void btnOutputExplore_Click(object sender, EventArgs e) {
              try {
				if(tbOutput.Text.Length > 0) {
					string _sPath;
					if(tbOutput.Text.Length > 1 && tbOutput.Text[1] == ':') {
						_sPath = tbOutput.Text;
					}else { 
						_sPath = PathHelper.ExeWorkDir  + tbOutput.Text ;
					}
					 _sPath =  Path.GetDirectoryName(_sPath.Replace('/', '\\').Trim());
					//_sPath = _sPath;
					Debug.fTrace( _sPath );
					Process.Start("explorer.exe", _sPath );
				}
            }    catch (Exception ex)  {
              // DialogHelper.ShowError(ex.ToString());
            }
        }

        private void tbWorkingDir_TextChanged(object sender, EventArgs e) {

        }

    

        public void fLauchEnd() {
           this.BeginInvoke((MethodInvoker)delegate  {
			
               btnSanitize.Enabled = true;
               btnPlay.Enabled = true;
      //         tbOutput.Enabled = true;
               btOutputSet.Enabled = true;
              // cancelButton.Text = "End";
               CancelButtonBuildIcon();

           });
        }


       

        private void btnPlay_Click(object sender, EventArgs e) {
            
            Data.oLauchProject.fLauchExe();
        }


       


        private void btnSanitize_Click(object sender, EventArgs e) {
             Data.oLauchProject.fLauchExe(true);
        }


        private void fInstallFormClosing(Object sender, FormClosingEventArgs e){
          //  btnInstall.Enabled = true;
			setCwcToolStripMenuItem.Enabled = true;
        }


        private void btnInstall_Click(object sender, EventArgs e) {
            
        }


        public void fShowInstallForm(){
	 this.BeginInvoke((MethodInvoker)delegate  {
             Install oInstallForm = new Install();
         //  oConfigForm.StartPosition = FormStartPosition.CenterParent;
            oInstallForm.FormClosing += fInstallFormClosing;


	
       //     btnInstall.Enabled = false;
           // oConfigForm.ShowDialog(this);

          //  cbCompiler.StartPosition = FormStartPosition.Manual;
         //  oInstallForm.Location = new Point( Left  + Width / 2 - oInstallForm.Width / 2, Top + Height / 2  - oInstallForm.Height / 2);
		  oInstallForm.Location = new Point( Cons_x  + (Cons_width-Cons_x) / 2 - oInstallForm.Width / 2,  Cons_y  + (Cons_height-Cons_y) / 2 - hScrollBar.Height  - oInstallForm.Height / 2);
			
		//	Console.WriteLine("Cons_x " + Cons_x);
		
		 oInstallForm.Show(this);
			
});
          
          
        }

        private void MainForm_Shown(object sender, EventArgs e) {
           
			
        }

        private void rbBuildSanitize_CheckedChanged(object sender, EventArgs e) {
			
        }

        private void rbBuildRun_CheckedChanged(object sender, EventArgs e) {

        }

        private void rbBuildOnly_CheckedChanged(object sender, EventArgs e) {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            SysAPI.fQuit(true);
        }



       private void cbPlatform_SelectedIndexChanged(object sender, EventArgs e) {
			 if(cbPlatform.Enabled == true) {
				Data.fSetGlobalVar("_sPlatform", cbPlatform.Text);
			}
			fSetView();
        }

        private void cbArchiteture_SelectedIndexChanged(object sender, EventArgs e) {
			 if(cbArchiteture.Enabled == true) {
				Data.fSetGlobalVar("wArch", cbArchiteture.Text.Trim());
			}
        }


        static bool _bFirstcbBuildType = true;

        private void cbBuildType_SelectedIndexChanged(object sender, EventArgs e) {

            /*
            if(_bFirstcbBuildType) {
                _bFirstcbBuildType = false;
                if(Data.sBuildAnd != "") {
                    return;
                }
            }*/
/*
            switch(cbBuildType.Text) {
                case "Build & Run":  
                        Data.sBuildAnd = "Run";
                break;
                 case "Build & Sanitize":
                       Data.sBuildAnd = "Sanitize";
                break;
                  case "Build Only":  
                   Data.sBuildAnd = "Nothing";
                 break;

                case "Nothing":  
                   Data.sBuildAnd = "Nothing";
                 break;

                default:
					   Data.sBuildAnd = "Run";

                break;
            }*/

			Data.fSetGlobalVar("wBuildAnd", cbBuildType.Text);


             //  Debug.fTrace("Write :  " +  Data.sBuildAnd   );
       
        }
         public static bool bcbCompilerFirst = false;
        private void cbCompiler_SelectedIndexChanged(object sender, EventArgs e) {

			if(cbCompiler.Enabled == true) {
				Data.fSetGlobalVar("_wToolchain", cbCompiler.Text);
			}
        }

        private void label4_Click(object sender, EventArgs e) {

        }

        private void btnUpdateLibRT_Click(object sender, EventArgs e) {
        
        }

        private void btnUpdateDemo_Click(object sender, EventArgs e) {
      
        }

		private void cwCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			 UpdateForm.Show(this, false,  "Honera/Cwc");
		}

		private void libRTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			 UpdateForm.Show(this, false,  "Honera/LibRT");
		}

		private void demosToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateForm.Show(this, false,  "Honera/Demos");
		}

		private void setCwcToolStripMenuItem_Click(object sender, EventArgs e)
		{
			  fShowInstallForm();
			setCwcToolStripMenuItem.Enabled = false;
		}

		private void libRTDKToolStripMenuItem_Click(object sender, EventArgs e)
		{
			 UpdateForm.Show(this, false,  "LibRTDK");
		}

		private void fullToolStripMenuItem_Click(object sender, EventArgs e)
		{
			 UpdateForm.Show(this, false, "Honera/LibRT");
		}

		private void emscriptenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			 UpdateForm.Show(this, false,  "Honera/Emscripten");
		}

		private void cbView_SelectedIndexChanged(object sender, EventArgs e)
		{
			Data.sCurrViewIn = cbView.Text;
		}

		private void gZEToolStripMenuItem_Click(object sender, EventArgs e)
		{
			 UpdateForm.Show(this, false,  "Honera/GZE");
		}

		private void msMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{

		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			AppQuit();
		}

		private void lblCommand_Click(object sender, EventArgs e)
		{

		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void MainForm_Activated(object sender, EventArgs e)
		{

		}

		private void label8_Click(object sender, EventArgs e)
		{

		}




		public	bool bExpand = false;
		public int nExpandSize = 0;
		private void btnExpand_Click(object sender, EventArgs e)
		{	

			Opacity = 0;
			nOpacity = 0;
			bExpand = !bExpand;
			if(!bExpand) {
				btnExpand.Text = "<<";
			}else {
				btnExpand.Text = ">>";
			}
			nLastState = 0;
			fSetSize();
		}

		private void MainForm_ResizeBegin(object sender, EventArgs e)
		{
		
		}





		//	protected override void OnGotFocus(EventArgs e){
		//	Debug.fTrace("---------OnGotFocus!");
		//	NativeMethods.SetForegroundWindow(Data.hConsole);
		//		base.OnGotFocus(e);
		//	}


		public int nClick = 0; 
		private void Event(object sender, EventArgs e) {
			nClick =10;
			//Thread.Sleep(1006);
			//nLastState = 0;
		//	BringToFront(); Debug.fTrace("Left mouse click!"); 
		}

		private void btnSanitize_EnabledChanged(object sender, EventArgs e)
		{
		 if(btnSanitize.Enabled == false) {
				btnSanitize.FlatStyle = FlatStyle.System;
		   }else {
				btnSanitize.FlatStyle = FlatStyle.Standard;
		   }
		}

		private void btnPlay_EnabledChanged(object sender, EventArgs e)
		{
			if(btnPlay.Enabled == false) {
				btnPlay.FlatStyle = FlatStyle.System;
		   }else {
				btnPlay.FlatStyle = FlatStyle.Standard;
		   }
		}

		private void cancelButton_EnabledChanged(object sender, EventArgs e)
		{
			if(cancelButton.Enabled == false) {
				cancelButton.FlatStyle = FlatStyle.System;
		   }else {
				cancelButton.FlatStyle = FlatStyle.Standard;
		   }
		}

		private void MainForm_Click(object sender, EventArgs e)
		{
			fActivateConsole();
		}

		private void allToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Data.aModuleList.Clear();
			Data.aModuleList.Add("Cwc");
			Data.aModuleList.Add("GZE");
			Data.aModuleList.Add("Cwc_Demos");

			UpdateForm.Show(this, false,  "Cwc");
		}

		private void btnLauch_Click(object sender, EventArgs e){
			string _sResult = CompilerConfig.fDialogExeFile( PathHelper.ExeWorkDir, "",  "Commands (*.cwc,*.bat)|*.cwc;*.bat|Executable (*.exe)|*.exe|All files (*.*)|*.*");
			if(_sResult.Length > 0){
                Delocalise.fDelocaliseInMainThread(PathHelper.ExeWorkDir + _sResult);

                /*
				string _sText =  Data.fDelocalise(PathHelper.ExeWorkDir + _sResult);
				  Data.sArgExpand  = Data.fExpandAll(_sText);*/
			//	Data.StartBuild();
			//	Debug.fTrace("!!!---- " + _sText);
			}
		}

	
		 private void MainForm_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e){

            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
			PipeInput.fMouseWheel(numberOfTextLinesToMove);
        }

        private void cbBuildType_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void viewInToolStripMenuItem_Click(object sender, EventArgs e)
        {
          //  viewInToolStripMenuItem.DropDownItems.Clear();
          //  viewInToolStripMenuItem.DropDownItems.Remove();
          //  viewInToolStripMenuItem.DropDownItems.Add("First Menu Item");
        }

        private void viewInToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
      
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void viewInToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var res = MessageBox.Show(this, "aaaaaaa?", "Demos Update?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);


        }

        private void directoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

   

        private void workingDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", PathHelper.ExeWorkDir.Replace('/', '\\'));
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.ToString());
            }
        }

        private void outputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbOutput.Text.Length > 0)
                {
                    string _sPath;
                    if (tbOutput.Text.Length > 1 && tbOutput.Text[1] == ':')
                    {
                        _sPath = tbOutput.Text;
                    }
                    else
                    {
                        _sPath = PathHelper.ExeWorkDir + tbOutput.Text;
                    }
                    _sPath = Path.GetDirectoryName(_sPath.Replace('/', '\\').Trim());
                    //_sPath = _sPath;
                    Debug.fTrace(_sPath);
                    Process.Start("explorer.exe", _sPath);
                }
            }
            catch (Exception ex)
            {
                // DialogHelper.ShowError(ex.ToString());
            }
        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private void aaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Data.oLauchProject.fCancel();
        }

        private void msMenu_MouseHover(object sender, EventArgs e)
        {
            //Focus();
        }

        private void configToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
        //    Focus();
        }

        private void msMenu_MouseMove(object sender, MouseEventArgs e)
        {
            Focus();
        }

        private void buildAndToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void MainForm_Resize(object sender, EventArgs e) {
           //fBringToFront(true);
           // Console.WriteLine("test");
        //     NativeMethods.SetParent( Handle, Data.oParentProcess.MainWindowHandle);


		// NativeMethods.SetParent( Handle, Data.oParentProcess.MainWindowHandle);
            //   Focus();
		//NativeMethods.SetParent( Handle,Data.hConsole);
	//	NativeMethods.SetParent( Handle,Data.hConsoleInput);
    	



        }

        private void MainForm_Move(object sender, EventArgs e) {
        //    Focus();
            	// NativeMethods.SetParent( Handle, Data.oParentProcess.MainWindowHandle);
        }
    }







    #region Data Items

    public class BgArg
    {
        public String File = "";
        public String Path = "";

        public BgArg(String file, String path)
        {
            this.File = file;
            this.Path = path;
        }
    }

    [Serializable]
    [XmlType("Entry")]
    public class DepEntry
    {
        public String Id = "";
        public String Name = "";
        public String Desc = "";
        public String Group = "";
        public String Version = "";
        public uint nLine = 0;
        public uint nColumn = 0;
        public String RefFile = "";
        public String Checksum = "";
        public String Build = "";
        public String Type = "";
        public String Info = "";
        public String Cmd = "";

        [XmlArrayItem("Url")]
        public String[] Urls = new String[0];

        [XmlArrayItem("Bundle")]
        public String[] Bundles = new String[0];

        [XmlIgnore]
        public Dictionary<String, String> Temps;

        public DepEntry()
        {
            this.Type = MainForm.TYPE_ARCHIVE;
            this.Temps = new Dictionary<String, String>();
        }
        public DepEntry(String id, String name, String desc, String group, String version, String build, String type, String info, String cmd, String[] urls, String[] bundles, String checksum)
        {
            this.Id = id;
            this.Name = name;
            this.Desc = desc;
            this.Group = group;
            this.Build = build;
            this.Version = version;
            this.Bundles = bundles;
            this.Checksum = checksum;
            this.Temps = new Dictionary<String, String>();
            if (!String.IsNullOrEmpty(type)) this.Type = type;
            else this.Type = MainForm.TYPE_ARCHIVE;
            this.Info = info;
            this.Urls = urls;
            this.Cmd = cmd;
        }
    }

    [Serializable]
    [XmlRoot("Entries")]
    public class DepEntries : List<DepEntry>
    {
        public DepEntries() {}
    }

    [Serializable]
    public class Settings
    {
        public String Help = "";
        public String Logs = "";
        public String Config = "";
        public String Archive = "";
        public String Locale = "en_US";

        [XmlArrayItem("Path")]
        public String[] Paths = new String[0];

        public Settings() {}
        public Settings(String config, String archive, String[] paths, String locale, String help, String logs)
        {
            this.Logs = logs;
            this.Paths = paths;
            this.Config = config;
            this.Archive = archive;
            this.Locale = locale;
            this.Help = help;
        }

    }

    [Serializable]
    [XmlRoot("Locale")]
    public class LocaleData
    {
        public LocaleData() {}
        public String NameHeader = "Name";
        public String VersionHeader = "Version";
        public String DescHeader = "Description";
        public String StatusHeader = "Status";
        public String TypeHeader = "Type";
        public String StateNew = "New";
        public String StateUpdate = "Update";
        public String StateInstalled = "Installed";
        public String ExtractingFile = "Extracting: ";
        public String DownloadingFile = "Downloading: ";
        public String DownloadingItemList = "Downloading item list...";
        public String NoItemsSelected = "No items selected.";
        public String ItemListOpened = "Item list read from file.";
        public String DownloadedItemList = "Item list downloaded.";
        public String AllItemsCompleted = "All selected items completed.";
        public String ItemListDownloadCancelled = "Item list download cancelled.";
        public String ItemListDownloadFailed = "Item list could not be downloaded.";
        public String DeleteSelectedConfirm = "Are you sure to delete all versions of the selected items?";
        public String ContinueWithNextItem = "Trying to continue with the next item.";
        public String ChecksumVerifyError = "The specified checksum did not match the file: ";
        public String DownloadingError = "Error while downloading file: ";
        public String ExtractingError = "Error while extracting file: ";
        public String DeleteDirError = "Error while deleting directory: ";
        public String MainFormTitle = "cwc";
        public String ConfirmTitle = "Confirm";
        public String LinkAll = "All";
        public String LinkNone = "None";
        public String LinkInstalled = "Installed";
        public String LinkUpdates = "Updates";
        public String LinkNew = "New";
        public String SelectLabel = "Select:";
        public String ExploreLabel = "Explore...";
       // public String InstallPathLabel = "Install path:";
        public String DeleteSelectedLabel = "Delete {0} items.";
        public String InstallSelectedLabel = "Install {0} items.";
        public String ToggleCheckedLabel = "Toggle Checked";
        public String ShowInfoLabel = "Show Info...";
        public String TypeExecutable = "Executable";
        public String TypeArchive = "Archive";
        public String TypeLink = "Link";
    }

    #endregion

    
   
 class ListViewDoubleBuffer : System.Windows.Forms.ListView
{
    public ListViewDoubleBuffer()
    {
        //Activate double buffering
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

        //Enable the OnNotifyMessage event so we get a chance to filter out 
        // Windows messages before they get to the form's WndProc
        this.SetStyle(ControlStyles.EnableNotifyMessage, true);
    }

    protected override void OnNotifyMessage(Message m)
    {
        //Filter out the WM_ERASEBKGND message
        if(m.Msg != 0x14)
        {
            base.OnNotifyMessage(m);
        }
    }

        private void InitializeComponent() {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }





public static class ScreenOperations
{
    public static bool IsWindowOnAnyScreen(Form Window, bool AutoAdjustWindow)
    {
        var Screen = System.Windows.Forms.Screen.FromHandle(Window.Handle);
 int WindowSizeX = Window.Width;
int WindowSizeY =  Window.Height;


        bool LeftSideTest = false, TopSideTest = false, BottomSideTest = false, RightSideTest = false;

        if (Window.Left >= Screen.WorkingArea.Left)
            LeftSideTest = true;

        if (Window.Top >= Screen.WorkingArea.Top)
            TopSideTest = true;

        if ((Window.Top + WindowSizeY) <= Screen.WorkingArea.Bottom)
            BottomSideTest = true;

        if ((Window.Left + WindowSizeX) <= Screen.WorkingArea.Right)
            RightSideTest = true;

        if (LeftSideTest && TopSideTest && BottomSideTest && RightSideTest)
            return true;
        else
        {
            if (AutoAdjustWindow)
            {
                if (!LeftSideTest)
                    Window.Left = Window.Left - (Window.Left - Screen.WorkingArea.Left);

                if (!TopSideTest)
                    Window.Top = Window.Top - (Window.Top - Screen.WorkingArea.Top);

                if (!BottomSideTest)
                    Window.Top = Window.Top - ((Window.Top + WindowSizeY) - Screen.WorkingArea.Bottom);

                if (!RightSideTest)
                    Window.Left = Window.Left - ((Window.Left + WindowSizeX) - Screen.WorkingArea.Right);
            }
        }

        return false;
    }
}


}














