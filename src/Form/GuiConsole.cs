using cwc.Properties;
using cwc.Utilities;
using FastColoredTextBoxNS;
using Raccoom.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cwc {


    public partial class GuiConsole : Form {


       // public static   Dictionary<string,bool> aOption = new Dictionary<string, bool>();
       // public static   Dictionary<string,string> aOption =  Data.aVarGlobal;
   //     public static   Dictionary<string,string> aOption =  Data.aOption;
        public static   Dictionary<string,string> aOption =  Data.aOption;
        //ublic static   Dictionary<string,string> aOption = new Dictionary<string, string>();
 
      
        
   
        public void fUncheckAll(ToolStripMenuItem _oParent) {
            
             foreach ( Object _oItem in _oParent.DropDownItems) {
                if( _oItem is ToolStripMenuItem ) {
                    ToolStripMenuItem _oItemTool = (ToolStripMenuItem)_oItem;
                     _oItemTool.Checked = false;
                      aOption[ _oItemTool.Name] = Data.sFALSE;
                }
            }

        }


        public void fCheckMenu(object sender, EventArgs e) {
            string _sParentName = "";
          //  bool _bChecked = ((ToolStripMenuItem)(sender)).Checked;

              ToolStripItem _oParent = ((ToolStripMenuItem)(sender)).OwnerItem;
            if (_oParent != null) {
                _sParentName = _oParent.Text + "/";
                fUncheckAll((ToolStripMenuItem)_oParent);
            }


             ((ToolStripMenuItem)(sender)).Checked = true;
            // ((ToolStripMenuItem)(sender)).Checked = !_bChecked;
            //aOption[_sParentName + ((ToolStripMenuItem)(sender)).Text] = (bool) ((ToolStripMenuItem)(sender)).Checked;


            string _sFullName = ((ToolStripMenuItem)(sender)).Name;
            string _sOptName = Path.GetDirectoryName( _sFullName ).Replace('\\','/'); //Ugly but work

             aOption[ ((ToolStripMenuItem)(sender)).Name] =  Data.fGetStrBool(  (bool) (((ToolStripMenuItem)(sender)).Checked) );


         //    aOption[_sOptName] =  ((ToolStripMenuItem)(sender)).Text;

            Data.fSetGlobalVar("_s" + _sOptName,  ((ToolStripMenuItem)(sender)).Text );



            Output.TraceAction("Set[" + _sOptName + "]:" + ((ToolStripMenuItem)(sender)).Text);
           // Output.TraceAction("Set[" +  ((ToolStripMenuItem)(sender)).Name + "]:true");
           
        }

        public void fLoadData() {
               this.BeginInvoke((MethodInvoker)delegate {
                       fLoadMenuStrip("Options/", optionsToolStripMenuItem);
                       if(! fLoadMenuStrip("IDE/", iDEToolStripMenuItem)) {
                           notePadToolStripMenuItem.Checked = true;
                           //  fCheckMenu(notePadToolStripMenuItem, null);
                            Data.fSetGlobalVar("IDE/Notepad++", Data.sTRUE);
                        }

               });
        }

       public bool fLoadMenuStrip(string _sParent, ToolStripMenuItem _oMenu) {
            bool _bFound = false;
             foreach ( ToolStripItem _oItem in _oMenu.DropDownItems) {
                 if( !(_oItem is ToolStripSeparator) && _oItem.Text != "Open" ) {
                   ToolStripMenuItem _oMenuItem = (ToolStripMenuItem)_oItem;

                   if( _oMenuItem.DropDownItems.Count == 0) {
                       _oMenuItem.Click += fCheckMenu;
                        _oMenuItem.Name = (_sParent +_oMenuItem.Text).Replace("&&", "&");
                      //  _oItem.Checked = true;
                     // Console.WriteLine("_oItem.Name " +_oItem.Name);
                        // aOption[ _oItem.Name] = Data.fGetStrBool( (bool)_oItem.Checked );
                      _oMenuItem.Checked =  Data.fIsDataTrue(  _oMenuItem.Name);
                        if(_oMenuItem.Checked) {
                            _bFound = true;
                        }
                    } else {
                      // Console.WriteLine("asss " +_oMenu.Text);
                        fLoadMenuStrip(_sParent + _oItem.Text + "/", _oMenuItem);
                    }
                }
            }
             return _bFound;
        }


        Assembly assembly = Assembly.GetExecutingAssembly();
       // Assembly assembly = null;

        public static TextStyle eInfoStyle  = new TextStyle(Brushes.Black, null, FontStyle.Regular);
        public static TextStyle eWarningStyle = new TextStyle(Brushes.BurlyWood, null, FontStyle.Regular);
        public static TextStyle eErrorStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);

         public  int nTreePrjScrollBar_IniX = 0;
         public  int nTreePrjScrollBar_IniY = 0;



        public GuiConsole() {
                    //   Console.WriteLine("GuiConsole");

                ExtendMargins(0, 35, 0, 0, false, true);
             

            SetStyle(ControlStyles.ResizeRedraw, true);
                         //  Console.WriteLine("InitializeComponent");
            InitializeComponent();
            DoubleBuffered = true;
            nTreePrjScrollBar_IniX =  treeViewPrj.Location.X;
            nTreePrjScrollBar_IniY =  treeViewPrj.Location.Y;
                     //      Console.WriteLine("Populate");
          
        //    Populate(true); //Slow to load
              //    Console.WriteLine("Finish");
      

        }
        
         private ITreeStrategyDataProvider _dataProvider;
        TreeStrategyFolderBrowserProvider FolderBrowser;
   /// <summary>
        /// Plumb everything up and fill the tree view
        /// </summary>        
        
        public static bool _bHaveBeenPopulated = false;
        void Populate(bool providerTypeChanged)
        {
            if(!_bHaveBeenPopulated) {
                _bHaveBeenPopulated = true;
          

                Raccoom.Windows.Forms.TreeStrategyShell32Provider shell32Provider = new Raccoom.Windows.Forms.TreeStrategyShell32Provider();
                shell32Provider.EnableContextMenu = true;
                shell32Provider.ShowAllShellObjects = true;
                // collection.Add(shell32Provider);
                //  treeViewPrj.DataSource = shell32Provider;

                FolderBrowser = new Raccoom.Windows.Forms.TreeStrategyFolderBrowserProvider();
                FolderBrowser.fIni();
                FolderBrowser.ShowFiles = true;
                treeViewPrj.DataSource = FolderBrowser;
                treeViewPrj.CheckBoxBehaviorMode = CheckBoxBehaviorMode.None;
                // return;
                // FolderBrowser.sCustomRootDir = @"E:\_Project";
                FolderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
   
                treeViewPrj.Populate(true);

           }

        }

        public void fSetTreeFolder( string _sDir ) {
            if(FolderBrowser == null) {return; }
        //    FolderBrowser.sCustomRootDir =  @"E:\_Project";
            FolderBrowser.sCustomRootDir = _sDir;
            treeViewPrj.Populate(true);
        }
        


        public void fAddTreeFolder( TreeNodePath _oParentNode, string _sDir ) {
             if(FolderBrowser == null) {return; }
        //    FolderBrowser.sCustomRootDir =  @"E:\_Project";
          //  treeViewPrj.BeginUpdate();
            FolderBrowser.sCustomRootDir = _sDir;
          //  FolderBrowser.oParentNode = null;
            FolderBrowser.oParentNode = _oParentNode;
            treeViewPrj.Populate(false);
          //  treeViewPrj.EndUpdate();


        }
        




       public  CwFCTB Fctb;
  

        private void fctb_Load(object sender, EventArgs e) {
                    Data.oGuiConsole = this;
            Fctb = fctbConsole;
            fctbConsole.Visible =true;
              Icon = new Icon(assembly.GetManifestResourceStream("cwc.Resources.cwc.ico"));
      //         fTrace( " Error\r\n", eErrorStyle); 


             Fctb.ContextMenuStrip = cmMain;


            fCreateStdStyle();
        }

                    
     
         public static ForeStyle oForeStyle ;
       public static BackgroundStyle oBackStyle ;//(null,  Brushes.DarkBlue, FontStyle.Regular);
       public static LinkStyle oLinkStyle ;//(null,  Brushes.DarkBlue, FontStyle.Regular);

       public static HighlightStyle oHightLightStyle ;//(null,  Brushes.DarkBlue, FontStyle.Regular);
        
        TextStyle[] aStdStyle = new TextStyle[16];
        private void fCreateStdStyle() {
                    
            oBackStyle  = new BackgroundStyle(Fctb, Brushes.DarkBlue);
              oLinkStyle  = new LinkStyle(Fctb, Brushes.Blue);

         //    if (selectionColor.A == 255)
            //        selectionColor = Color.FromArgb(60, selectionColor);
            //    SelectionStyle = new SelectionStyle(new SolidBrush( Color.FromArgb(60, selectionColor)));

         
        
               oForeStyle  = new ForeStyle(Fctb, FontStyle.Bold);
                   oHightLightStyle  = new HighlightStyle(Fctb,new SolidBrush( Color.FromArgb(60, Color.Blue)) );




     //    CwFCTB.GGetOrSetStyleLayerIndex();
            
    //    public static ForeStyle oForeStyle  = new ForeStyle( FontStyle.Regular);
      // public static BackgroundStyle oBackStyle  = new BackgroundStyle(Brushes.DarkBlue);//(null,  Brushes.DarkBlue, FontStyle.Regular);


            /*

            aStdStyle[0] = fNewStdStyle(Brushes.Black);
            aStdStyle[1] = fNewStdStyle(Brushes.DarkBlue);
            aStdStyle[2] = fNewStdStyle(Brushes.DarkGreen);
            aStdStyle[3] = fNewStdStyle(Brushes.DarkMagenta);
            aStdStyle[4] = fNewStdStyle(Brushes.DarkRed);
            aStdStyle[5] = fNewStdStyle(Brushes.DeepPink);
            aStdStyle[6] = fNewStdStyle(Brushes.BurlyWood);
           // aStdStyle[6] = fNewStdStyle(Brushes.DarkGoldenrod);
            
            aStdStyle[7] = fNewStdStyle(Brushes.LightGray);
            aStdStyle[8] = fNewStdStyle(Brushes.Gray);
            aStdStyle[9] = fNewStdStyle(Brushes.DarkViolet);
            aStdStyle[10] = fNewStdStyle(Brushes.Green);
            aStdStyle[11] = fNewStdStyle(Brushes.LightBlue);
            aStdStyle[12] = fNewStdStyle(Brushes.Red);
            aStdStyle[13] = fNewStdStyle(Brushes.Pink);
            aStdStyle[14] = fNewStdStyle(Brushes.Yellow);
            aStdStyle[15] = fNewStdStyle(Brushes.White);*/
               
        }


       // public string sSelectedViewIn =
        internal void fAddViewIn(List<string> _aBrowser) {
            this.BeginInvoke((MethodInvoker)delegate {
                 try { 

                     viewInToolStripMenuItem.DropDownItems.Clear();
                    foreach(string _sBrowser in _aBrowser){

                       ToolStripMenuItem _oNew = new ToolStripMenuItem(_sBrowser);
                        _oNew.Tag = _sBrowser;

                          _oNew.Text = _sBrowser;
                          _oNew.Name = "ViewIn/" + _sBrowser;
                         viewInToolStripMenuItem.DropDownItems.Add(_oNew);
                        //  _oNew.Checked =  Data.fIsDataTrue(  _oNew.Name);
                        if( Data.fIsDataTrue(  _oNew.Name)) {
                            fCheckMenu(_oNew, null);
                        }

                      //   fCheckMenu(notePadToolStripMenuItem, null);



                        if(_oNew.Checked ) {

                        }

                        _oNew.Click += fCheckMenu;
                    }

                }catch( Exception e) { Console.WriteLine(e.Message);};
            });
        }

        private TextStyle fNewStdStyle(Brush _oFore) {
            TextStyle _oStyle = new TextStyle(_oFore,  null, FontStyle.Bold);
           // _oStyle.FontStyle = new FontStyle();
            return _oStyle;
        }
        
       internal void fSetWorkingDir(string _sDir) {
             this.BeginInvoke((MethodInvoker)delegate {
                 cbTitle.Text =  _sDir;
                // fSetTreeFolder(_sDir.Substring(0,_sDir.Length-1));
                 fSetTreeFolder(_sDir);
             });
        }


           
         TreeNodePath oUnknow = new TreeNodePath("Unknow",true);
         //TreeNodePath oBaseNode  = new TreeNodePath("Unknow",true)
       
         List<TreeNodePath> aBaseNode = new List<TreeNodePath>();

        
         TreeNodePath oNodeToolchain = new TreeNodePath( "Toolchain", true);
         TreeNodePath oNodeLib = new TreeNodePath( "Lib", true);
         TreeNodePath oNodeProject = new TreeNodePath( "Project", true);
         TreeNodePath oNodeOther = new TreeNodePath( "Other", true);

        internal void fSetIconName(TreeNodePath _oNode, string _sName) {
              _oNode.ImageKey =_sName;
              _oNode.SelectedImageKey = _sName;
        }


       internal void fAddAllUsedDir() {
            try { 
             this.BeginInvoke((MethodInvoker)delegate {


            
                  FolderBrowser.fClear();
                  FolderBrowser.fAddRootNode(oNodeProject);
                  FolderBrowser.fAddRootNode(oNodeLib);
      
                  FolderBrowser.fAddRootNode(oNodeToolchain);
              
                  FolderBrowser.fAddRootNode(oNodeOther);
                 
                  // FolderBrowser.fSetIcon(oNodeToolchain,   Resources.cwc, "aa");

                 fSetIconName(oNodeProject,"ProjectRoot" );
                 fSetIconName(oNodeToolchain,"ToolChainRoot" );
                 fSetIconName(oNodeLib,"LibRoot" );
                 fSetIconName(oNodeOther,"OtherRoot" );
                


                 foreach(ModuleData _oCompiler in  Data.aCompilerData.Values) {
                     string _sPath = _oCompiler.sCurrFolder;
                     if(_sPath.Length < 2){continue;}//To be safe
                     string _sName = _oCompiler.sCurrFolder;
                     if (_sName[_sName.Length-1] == '/') {
                         _sName = Path.GetFileName(_sName.Substring(0,_sName.Length-1));
                     }
                     TreeNodePath _oPath = new TreeNodePath( _sName, true);
                     _oPath.Path = PathHelper.fNormalizeFolder(_oCompiler.sCurrFolder);
                     _oPath.ToolTipText = _oPath.Path;

                     aBaseNode.Add(_oPath);
                    // FolderBrowser.fAddRootNode(_oPath);

                     if(_oCompiler.bIsCompiler){
                         oNodeToolchain.Nodes.Add(_oPath);
                          fSetIconName(_oPath,"ToolChain" );

                         if(_oPath.Parent == oNodeToolchain) {
                         //    Console.WriteLine("aaaaaaaaaaaaaaa  oNodeToolchainoNodeToolchain " +  _oPath.Path);
                         }
                     } else {
                         oNodeLib.Nodes.Add(_oPath);
                        fSetIconName(_oPath,"Lib" );
                     }
                   // _oPath. = oNodeLib;

                  //    fAddTreeFolder(_oParentNode, _sInclude);
                     //Console.WriteLine("_oCompiler.sCurrFolder" +    _oCompiler.sCurrFolder);
                 }
                
                 aBaseNode.Add(oNodeProject);
                 oNodeProject.Path = PathHelper.ExeWorkDir;
                 oNodeProject.ToolTipText = oNodeProject.Path;
                
                TreeNodePath _oParentNode  = oNodeOther;
               foreach(string _sInclude in  Data.aAllInclude) {
                    _oParentNode  = oNodeOther;
                     foreach (TreeNodePath _oNode in aBaseNode) {
                         //    Console.WriteLine("_oNode.Path" +   _oNode.Text);
                         
                         if (_sInclude.IndexOf(_oNode.Path ) != -1) {
                             _oParentNode = _oNode;
                         }
                     }

                    fAddTreeFolder(_oParentNode, _sInclude);
                  //   Console.WriteLine(" ---_sInclude " + _sInclude);
                }
                

                Setting.oSettingsLauch.fExtractTreeViewPrjData();

                treeViewPrj_AfterExpand(treeViewPrj,null);//Update size



             });
            }catch(Exception e) { }
        }

        private static  TextStyle oStyle ;
       internal void fTraceColorCode(string _sMsg, int _nColorCode) {
          


               this.BeginInvoke((MethodInvoker)delegate {

         FontStyle _oFont = FontStyle.Regular;

            Brush _oFore = Brushes.Red;
            Brush _oBack = null;


            int _nBack = _nColorCode >> 4;
            int _nFore = _nColorCode & 0xF;
           
  //  fTrace(" : " +_nColorCode + " : ", aStdStyle[6]);
            if (_nColorCode < 0) {
                  _nFore = 6;
            }

            /*
            if (_nBack != 0) {
                switch (_nFore) {
                    case 0: _oFore = Brushes.Black; break;
                    case 1: _oFore = Brushes.DarkBlue; break;
                    case 2: _oFore = Brushes.DarkGreen; break;
                    case 3: _oFore = Brushes.DarkMagenta; break;
                    case 4: _oFore = Brushes.DarkRed; break;
                    case 5: _oFore = Brushes.DeepPink; break;
                    case 6: _oFore = Brushes.DarkGoldenrod; break;
                    case 7: _oFore = Brushes.LightGray; break;
                    case 8: _oFore = Brushes.Gray; break;
                    case 9: _oFore = Brushes.DarkViolet; break;
                     case 10: _oFore = Brushes.Green; break;
                     case 11: _oFore = Brushes.LightBlue; break;
                     case 12: _oFore = Brushes.Red; break;
                     case 13: _oFore = Brushes.Pink; break;
                     case 14: _oFore = Brushes.Yellow; break;
                   default:  case 15: _oFore = Brushes.White; break;
                }
            }*/
            

                 //  fctb.ClearStylesBuffer();      
        // TextStyle _oStyle = new TextStyle(_oFore,  Brushes.Yellow, FontStyle.Regular);
          //  TextStyle _oStyle =new TextStyle(Brushes.Black, null, FontStyle.Regular);

            
         //       fTrace(_sMsg, _oStyle);
            
       //     return;

          
       //    fTrace(_sMsg, _oStyle);
                      fTrace(_sMsg, _nFore, _nBack);
                    //  fTrace(_sMsg, aStdStyle[_nFore]);
                      sFormCmd = "GoEnd";

                        });

        }



      //MarkerStyle oMark  = new MarkerStyle(Brushes.DarkBlue);//(null,  Brushes.DarkBlue, FontStyle.Regular);





        

        WavyLineStyle oWave  = new WavyLineStyle(255, Color.DarkCyan);//(null,  Brushes.DarkBlue, FontStyle.Regular);



           public static  int nLine = -1;
           public static  int nColomn = -1;
           public static  int nStartSelX = 0;
	       public static  int nEndSelX = 0;
           public static  bool bRelative = false;

        private static string fTestSelection(string _sRead){
             nLine = -1;
             nColomn = -1;
             bRelative = false;
            bool bFoundSel = false;

       
          	bool _bAcceptSpace = false;

            string _sFile = "";
       

              //Absolute path
			int _nStartIndex = _sRead.IndexOf(":/");
          	if(_nStartIndex == -1 ){
                 _nStartIndex = _sRead.IndexOf(":\\");
                	//	Console.Write( _nStartIndex  + "            \r");
            }

            //Relative path
          if(_nStartIndex == -1 ){ 
                 _nStartIndex = _sRead.IndexOf("/");
                 if(_nStartIndex == -1 ){
                      _nStartIndex = _sRead.IndexOf("\\");
                 }
                 //Get Prev folder
                 if(_nStartIndex >= 0 ){
                   
                     _nStartIndex = _sRead.LastIndexOf(" ",_nStartIndex) + 2;
                     //Console.Write(_nStartIndex + "aa  \r");
                     char _sFirtChar = _sRead[_nStartIndex-1];
                     if(_sFirtChar == '[' || _sFirtChar == '{' || _sFirtChar == '(') { //Drmemory bug with link between bracket //--> [link]
                        _nStartIndex++;
                    }
                    bRelative = true;
                 }
          }


			if(_nStartIndex >= 1 ){
				if(_nStartIndex >= 2 && ( _sRead[_nStartIndex-2] == '\"' || _sRead[_nStartIndex-2] == '\'') ){
					_bAcceptSpace = true;
				}
				if((_sRead[_nStartIndex - 1] >= 'A' && _sRead[_nStartIndex - 1] <= 'Z') || bRelative){
					//nStartSelX = ((_nStartIndex -1  - (ushort)ConsoleReader.nScrollConsoleX)) * fontSize.Width;
                    nStartSelX = _nStartIndex -1;
                    
					int _nEndIndex = _nStartIndex + 1;
					while(_nEndIndex < _sRead.Length){
						if(_sRead[_nEndIndex] <  32  || _sRead[_nEndIndex] == '"' || _sRead[_nEndIndex] == '\''  || _sRead[_nEndIndex] == '<'  || _sRead[_nEndIndex] == '>' || _sRead[_nEndIndex] == ':'  || _sRead[_nEndIndex] == '*'  || _sRead[_nEndIndex] == '?'  || _sRead[_nEndIndex] == '|'){ //Space or special cher
							break;
						}
						if(!_bAcceptSpace && _sRead[_nEndIndex] ==  32){//Accept space??
							break;
						}

						_nEndIndex++;
					}
					
					if(_nEndIndex > _nStartIndex){
						bFoundSel = true;
						_sFile = _sRead.Substring(_nStartIndex-1,_nEndIndex - (_nStartIndex-1) );
						//get line
						if(_sRead.Length >_nEndIndex && _sRead[_nEndIndex] == ':'){
							//string _sNumber = _sRead.Substring(_nEndIndex+1);
							string _sLine = "";
							_nEndIndex++;
							while(_nEndIndex < _sRead.Length &&  _sRead[_nEndIndex] >= '0' && _sRead[_nEndIndex] <= '9' ){
								_sLine += _sRead[_nEndIndex];
								_nEndIndex++;
							}
						//nLine = 0;
							Int32.TryParse(_sLine, out nLine);
	
							//get Colomn
							if(_sRead.Length >_nEndIndex+1 && _sRead[_nEndIndex] == ':'){
								_nEndIndex++;
								//string _sNumber = _sRead.Substring(_nEndIndex+1);
								string _sColomn = "";
								while(_nEndIndex < _sRead.Length && _sRead[_nEndIndex] >= '0' && _sRead[_nEndIndex] <= '9' ){
									_sColomn += _sRead[_nEndIndex];
									_nEndIndex++;
								}
								//nColomn = 0;
								Int32.TryParse(_sColomn, out nColomn);
			
							}
							if(_sRead.Length >_nEndIndex && _sRead[_nEndIndex] == ':'){
								_nEndIndex++;
							}
					//		_nEndIndex += _nEndIndex - _nEndIndex;			
						}
						//nEndSelX = (_nEndIndex   - (ushort)ConsoleReader.nScrollConsoleX) * fontSize.Width - nStartSelX;
						nEndSelX = _nEndIndex;
                           //    Console.Write(nEndSelX  + "            \r");
					}
		

				}
			}

             _sFile = _sFile.Trim();       
            if (bRelative) {
                _sFile = PathHelper.ExeWorkDir  + _sFile;
            }

            _sFile = _sFile.Replace('\\', '/');
            return _sFile.Replace("//", "/");
          
			//oSelectForm.Location(0,0);
			//Console.Write( sFile  + "            \r");
		}





















        public static List<int> aLineToDelete = new List<int>();


     //   public void fTrace(string text, Style style) {
        public void fTrace(string text, int _nFore, int _nBack) {
              
            if(text.Length == 0) {
                return;
            }
            
            if(text[0] == '\r' || text.IndexOf('\b') >= 0 ) {
         //   if(text.IndexOf('\r') >= 0 ) {
          //  if(text.IndexOf('\r') > 0) {
                Data.oGuiConsole.Fctb.RemoveLines(aLineToDelete);
                aLineToDelete.Clear();
                aLineToDelete.Add( Data.oGuiConsole.Fctb.LinesCount-1);
               if(text[0] == '\r') {
                    text = text.Substring(1);
                }

            }else {
                aLineToDelete.Clear();
            }

            //  new Range(fctb,  new Place(0,0),  new Place(5,0)).SetStyle(oBackStyle);


            //   fctb.Font = new Font()
             bIgnoreNextSelectionChange = true;

            //some stuffs for best performance
            fctbConsole.BeginUpdate();
            fctbConsole.Selection.BeginUpdate();
            //remember user selection
            var userSelection = fctbConsole.Selection.Clone();
            //add text with predefined style
     //       fctb.TextSource.CurrentTB = fctb;

        

            //fctb.Selection.St
      //       new Range(fctb,  new Place(0,0),  new Place(5,0)).SetStyle(oWave);


          //  Place _oLast = fctb.Selection.Start;
         //   Place _oLast = new Place(0,0);;

            CwRange _oRange=  fctbConsole.AddText(text, oForeStyle,_nFore, oBackStyle, _nBack);
        
            string _sFile = fTestSelection(text);

            if(_sFile != ""){
               // _oRange.fSelectRange( nStartSelX, nEndSelX - nStartSelX);
                _oRange.fSelectRange( nStartSelX,nEndSelX);
                _oRange.SetStyle(oLinkStyle);
            }

      //      fctb.Selection.SetStyle(oLinkStyle);
      //      new Range(fctb,  userSelection.Start,   fctb.Selection.End).SetStyle(oBackStyle);


   




            //restore user selection
         
         //  fctb.HighlightingRangeType

            if (!userSelection.IsEmpty || userSelection.Start.iLine < fctbConsole.LinesCount - 2)
            {
         //       fctb.Selection.Start = userSelection.Start;
           //     fctb.Selection.End = userSelection.End;
            }
            else{
            //    fctb.GoEnd();//scroll to end of the text  --> SLOW!
            }

          //  fctb.AutoScrollPosition

                //
            fctbConsole.Selection.EndUpdate();
            fctbConsole.EndUpdate();


            //             new Range(fctb,  new Place(0,0),  new Place(5,0)).SetStyle(oWave);


          
            
        }

        private void btGotToEnd_Click(object sender, EventArgs e) {
             fctbConsole.GoEnd();
        }

        
        private void GuiConsole_FormClosing(object sender, FormClosingEventArgs e) {
            //Save setting
            ConfigMng.oConfig.bMaximize = (WindowState ==  FormWindowState.Maximized);
            ConfigMng.oConfig.bTreePrjOpen = csPrj.IsCollapsed;
            Setting.oSettingsLauch.fSaveSetting(true);

            Base.bAlive = false;
            FastColoredTextBox.bAlive = false;
        }


        private void GuiConsole_FormClosed(object sender, FormClosedEventArgs e) {
            
                 /*
             ConfigMng.oConfig.vStartPos = Location;   
             ConfigMng.oConfig.vStartSize = Size;
             */


           // Base.bAlive = false;
         //   Build.StopBuild();
            Thread winThread = new Thread(new ThreadStart(() =>  { 
                SysAPI.fQuit();
            }));   winThread.Start();
        }

        public void fLoadRecent() {
           this.BeginInvoke((MethodInvoker)delegate  {
                
               List<ToolStripItem> aToRem = new List<ToolStripItem>();

                ///// Clear ///
                foreach(ToolStripItem _oItem in lauchToolStripMenuItem.DropDownItems){
                    //Console.WriteLine("aa" + _oItem.Tag);
                    if( !(_oItem is ToolStripSeparator) ) {
                        if(_oItem.Tag != null){
                           aToRem.Add(_oItem);
                        }
                     }
                 }
                foreach(ToolStripItem _oItem in aToRem){
                   lauchToolStripMenuItem.DropDownItems.Remove(_oItem);
                }
                //////

               try { 
                foreach(string _sRecent in ConfigMng.oConfig.aRecent){
                     ToolStripMenuItem _oNew = new ToolStripMenuItem(_sRecent);
                     _oNew.Tag = _sRecent;

                    string _sName = _sRecent;
                    string _sPath= _sRecent;
                   int _nLastIndex = _sName.LastIndexOf("/");
                    if (_nLastIndex != -1) {
                         _sPath =  _sName.Substring(0,_nLastIndex);
                        _sName = _sName.Substring(_nLastIndex);
                    }
                    _nLastIndex = _sPath.LastIndexOf("/");
                     if (_nLastIndex != -1) {
                        _sName = _sPath.Substring(_nLastIndex+1) + _sName;
                    }

                      _oNew.Text = _sName;
                     lauchToolStripMenuItem.DropDownItems.Add(_oNew);

                    _oNew.Click += fRecentClick;
                }
               }catch( Exception e) { Console.WriteLine(e.Message);};


           });
        }

        private void fRecentClick(object sender, EventArgs e) {

             Delocalise.fDelocaliseInMainThread( (string) ((ToolStripMenuItem) sender).Tag );
        }

       private void fViewInClick(object sender, EventArgs e) {
            /*
            ToolStripMenuItem _oMenu = (ToolStripMenuItem) sender;

            fUncheckAll(_oMenu);
            Output.Trace("Click: "  +  (string) (_oMenu).Tag);

            
             ((ToolStripMenuItem)(sender)).Checked = true;*/
            // Delocalise.fDelocaliseInMainThread( (string) ((ToolStripMenuItem) sender).Tag );
        }

          public static string fDialogExeFile( string _sRoot, string _sCurrentVal, string _sFilter = "") {

            OpenFileDialog fbd = new OpenFileDialog();
            _sRoot = _sRoot.Replace('\\', '/');
            if(_sRoot[_sRoot.Length - 1] != '/' ) {
                _sRoot += '/';
            }

            fbd.InitialDirectory  =  Path.GetDirectoryName(_sRoot + _sCurrentVal);
			if(_sFilter == ""){
				 fbd.Filter = "Executable (*.exe)|*.exe|All files (*.*)|*.*";
			}else{
				 fbd.Filter = _sFilter;
			}
            if(fbd.ShowDialog() == DialogResult.OK) {
               //return Path.GetFileName( fbd.FileName);
              // Debug.fTrace(_sRoot);
              // Debug.fTrace( fbd.FileName);
               return FileUtils.fMakeRelativePath(_sRoot, fbd.FileName);
            }
            return _sCurrentVal;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e) {
              string _sResult = fDialogExeFile( PathHelper.ExeWorkDir, "",  "Commands (*.cwMake)|*.cwMake;|Executable (*.exe)|*.exe|All files (*.*)|*.*");
            if(_sResult.Length > 2 &&  _sResult[1] != ':') {//If is not absolute (relative)
              _sResult = PathHelper.ExeWorkDir + _sResult;
            }

            if (_sResult.Length > 0){
           
                Delocalise.fDelocaliseInMainThread( _sResult);
                //CwcRootPath()
                /*
				string _sText =  Data.fDelocalise(PathHelper.ExeWorkDir + _sResult);
				Data.sArgExpand  = Data.fExpandAll(_sText);
				Data.StartBuild();*/
			//	Debug.fTrace("!!!---- " + _sText);
			}
        }


       int nInitialPosLeft = 0;
         int nInitialPosTop = 0;
        private void GuiConsole_Load(object sender, EventArgs e) {
             csPrj.ToggleState();//Clesed by default
     
            	//		SetWindowPosition(ConfigMng.oConfig.vStartPos.X,ConfigMng.oConfig.vStartPos.Y,ConfigMng.oConfig.vStartSize.Width,ConfigMng.oConfig.vStartSize.Height);

                fLoadRecent();
            
            
                Setting.oSettingsLauch.fIni( treeViewPrj);  

            if(!ConfigMng.bLoadFailed){
                if(ConfigMng.oConfig.vStartPos != null ){
                    Location = ConfigMng.oConfig.vStartPos;
                }
                 if(ConfigMng.oConfig.vStartSize != null ){
                     Size = ConfigMng.oConfig.vStartSize;
                }

                if( ConfigMng.oConfig.bTreePrjOpen){
                //   csPrj.ToggleState();
                    if( !csPrj.IsCollapsed) {
                         csPrj.ToggleState();
                    }
                }else {
                     if( csPrj.IsCollapsed) {
                         csPrj.ToggleState();
                    }
                }
            } else {
                 ConfigMng.oConfig.vStartSize = Size;
                 ConfigMng.oConfig.vStartPos = Location;
            }
          

                   
            if(bWin10_Style) {
              msMenu.Left  -= 50;
           }

            nInitialPosLeft = Width - msMenu.Left;
            nInitialPosTop = msMenu.Top;
            bCreated = true;

            
                fCmdManager();


          //      vTreePrjScrollBar.bFirst = true;
          //  vTreePrjScrollBar.fUpdate();


              fAutoSizeTree(treeViewPrj, pnTtreeViewPrj, true);
              fSetTreeViewScrollBarMaximum(treeViewPrj);
          //  vTreePrjScrollBar.OnScroll();
          //  vTreePrjScrollBar.Refresh();
     
         //   hTreePrjScrollBar.Refresh();
            pnTreeCenter.oTreeView = treeViewPrj;
            treeViewPrj.oVScroll = vTreePrjScrollBar;
            treeViewPrj.fSetTheme();

            if( ConfigMng.oConfig.bMaximize){
                WindowState = FormWindowState.Maximized;
                fMaximizedState();
            }

            fLoadData();
              
 
        }


    public bool fCheckForDemos() {

           
        
       // string _sDemoDir =PathHelper.GetExeDirectory() + "_Cwc_Demos/";
        string _sDemoDir =PathHelper.GetExeDirectory() + "Lib/VLiance/Demos/";
        string _sLibDir =PathHelper.GetExeDirectory() + "Lib/";

	    if(!Directory.Exists(_sLibDir)) {
                /*
                this.BeginInvoke((MethodInvoker)delegate {

                    var res = MessageBox.Show(this, "You don't have any demos, download?", "Demos Update?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
				    if (res == DialogResult.Yes){
					   // UpdateForm.Show(this, false,  "VLiance/Demos");
                          Output.Trace("");
                         Data.fAddRequiredModule( "VLiance/Demos");

                        // Data.sCmd = "StartBuild";
                        
                         Thread winThread = new Thread(new ThreadStart(() =>  {  
                               Empty.fLoadModules();
                          }));  
		                 winThread.Start();
                         

				    }else {
					    Directory.CreateDirectory(_sLibDir);
				    }


                });
                */
                Directory.CreateDirectory(_sLibDir);

                return false;

            } else  {
                return true;
            }
    }








        public static string sFormCmd = "";
        private void fCmdManager() {
                Thread winThread3 = new Thread(new ThreadStart(() =>  {  
			    while(Base.bAlive) {
             
                        vMyScrollBar.fUpdate(); 
                        hMyScrollBar.fUpdate(); 
                        hTreePrjScrollBar.fUpdate(); 
                        vTreePrjScrollBar.fUpdate(); 
                

                        if(sFormCmd != ""){
                             this.BeginInvoke((MethodInvoker)delegate {
			                     if(sFormCmd == "GoEnd"){
                                     if(Data.bIWantGoToEnd){
                                         bIgnoreNextSelectionChange = true;
                                        fctbConsole.GoEnd();
                                     }
                                }
                                sFormCmd = "";
                            });
                        }
                   
				    Thread.CurrentThread.Join(16);
			     }
		    }));  
		    winThread3.Start();
 
        }



        private void textBox1_TextChanged(object sender, EventArgs e) {

        }
        private void fctb_ScrollbarsUpdated(object sender, EventArgs e) {
               AdjustScrollbars();
        }
        private void AdjustScrollbars() {
            AdjustScrollbar(vMyScrollBar, fctbConsole.VerticalScroll.Maximum, fctbConsole.VerticalScroll.Value, fctbConsole.ClientSize.Height);
            AdjustScrollbar(hMyScrollBar, fctbConsole.HorizontalScroll.Maximum, fctbConsole.HorizontalScroll.Value, fctbConsole.ClientSize.Width);

          //  AdjustScrollbar(vScrollBar, fctb.VerticalScroll.Maximum, fctb.VerticalScroll.Value, fctb.ClientSize.Height);
          //  AdjustScrollbar(hScrollBar, fctb.HorizontalScroll.Maximum, fctb.HorizontalScroll.Value, fctb.ClientSize.Width);
        }
        private void AdjustScrollbar(CwScrollBar scrollBar, int max, int value, int clientSize){
            scrollBar.Maximum = max;
            if ( scrollBar.Maximum <= 1) {
                scrollBar.Maximum = 1;
            }

          //  scrollBar.Visible = max > 0;
     
            scrollBar.Value = Math.Min(scrollBar.Maximum, value);
        }



        private void hMyScrollBar_Click(object sender, EventArgs e) {

        }

        public static int nLastScrollValue= 0;
     //   public static int nLastMaxScrollValue= 0;
        private void vMyScrollBar_Scroll(object sender, ScrollEventArgs e) {
    
             fctbConsole.OnScroll(e, e.Type != ScrollEventType.ThumbTrack && e.Type != ScrollEventType.ThumbPosition);

            if (vMyScrollBar.Value < nLastScrollValue) {
                Data.bIWantGoToEnd = false;
            }
            nLastScrollValue =   vMyScrollBar.Value;

            if (vMyScrollBar.Value >= vMyScrollBar.Maximum - 10) {
                  Data.bIWantGoToEnd = true;
            }
        }

        private void hMyScrollBar_Scroll(object sender, ScrollEventArgs e) {
             fctbConsole.OnScroll(e, e.Type != ScrollEventType.ThumbTrack && e.Type != ScrollEventType.ThumbPosition);
        }

        private void label1_Click(object sender, EventArgs e) {

        }



        
    
        #region Extend Frame
        #region Constants
        // windowpos
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOREDRAW = 0x0008;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int SWP_FRAMECHANGED = 0x0020;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int SWP_HIDEWINDOW = 0x0080;
        private const int SWP_NOCOPYBITS = 0x0100;
        private const int SWP_NOOWNERZORDER = 0x0200;
        private const int SWP_NOSENDCHANGING = 0x0400;
        // redraw
        private const int RDW_INVALIDATE = 0x0001;
        private const int RDW_INTERNALPAINT = 0x0002;
        private const int RDW_ERASE = 0x0004;
        private const int RDW_VALIDATE = 0x0008;
        private const int RDW_NOINTERNALPAINT = 0x0010;
        private const int RDW_NOERASE = 0x0020;
        private const int RDW_NOCHILDREN = 0x0040;
        private const int RDW_ALLCHILDREN = 0x0080;
        private const int RDW_UPDATENOW = 0x0100;
        private const int RDW_ERASENOW = 0x0200;
        private const int RDW_FRAME = 0x0400;
        private const int RDW_NOFRAME = 0x0800;
        // frame
        private const int FRAME_WIDTH = 8;
        private const int CAPTION_HEIGHT = 30;
        private const int FRAME_SMWIDTH = 4;
        private const int CAPTION_SMHEIGHT = 24;
        // misc
        private const int WM_SYSCOMMAND = 0x112;
        private const int SC_RESTORE = 0xF120;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SM_SWAPBUTTON = 23;
        private const int WM_GETTITLEBARINFOEX = 0x033F;
        private const int VK_LBUTTON = 0x1;
        private const int VK_RBUTTON = 0x2;
        private const int KEY_PRESSED = 0x1000;
        private const int BLACK_BRUSH = 4;
        // proc
        private const int WM_CREATE = 0x0001;
        private const int WM_NCCALCSIZE = 0x83;
        private const int WM_NCHITTEST = 0x84;
        private const int WM_SIZE = 0x5;
        private const int WM_PAINT = 0xF;
        private const int WM_TIMER = 0x113;
        private const int WM_ACTIVATE = 0x6;
        private const int WM_NCMOUSEMOVE = 0xA0;
        private const int WM_NCMOUSEHOVER = 0x02A0;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int WM_NCLBUTTONUP = 0xA2;
        private const int WM_NCLBUTTONDBLCLK = 0xA3;
        private const int WM_NCRBUTTONDOWN = 0xA4;
        private const int WM_NCRBUTTONUP = 0xA5;
        private const int WM_NCRBUTTONDBLCLK = 0xA6;
        private const int WM_DWMCOMPOSITIONCHANGED = 0x031E;
        private const int WVR_ALIGNTOP = 0x0010;
        private const int WVR_ALIGNLEFT = 0x0020;
        private const int WVR_ALIGNBOTTOM = 0x0040;
        private const int WVR_ALIGNRIGHT = 0x0080;
        private const int WVR_HREDRAW = 0x0100;
        private const int WVR_VREDRAW = 0x0200;
        private const int WVR_REDRAW = (WVR_HREDRAW | WVR_VREDRAW);
        private const int WVR_VALIDRECTS = 0x400;
        private static IntPtr MSG_HANDLED = new IntPtr(0);
        #endregion

        #region Enums
        private enum HIT_CONSTANTS : int
        {
            HTERROR = -2,
            HTTRANSPARENT = -1,
            HTNOWHERE = 0,
            HTCLIENT = 1,
            HTCAPTION = 2,
            HTSYSMENU = 3,
            HTGROWBOX = 4,
            HTMENU = 5,
            HTHSCROLL = 6,
            HTVSCROLL = 7,
            HTMINBUTTON = 8,
            HTMAXBUTTON = 9,
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17,
            HTBORDER = 18,
            HTOBJECT = 19,
            HTCLOSE = 20,
            HTHELP = 21
        }

        private enum PART_TYPE : int
        {
            WP_MINBUTTON = 15,
            WP_MAXBUTTON = 17,
            WP_CLOSEBUTTON = 18,
            WP_RESTOREBUTTON = 21
        }
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            internal int X;
            internal int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SIZE
        {
            public int cx;
            public int cy;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            internal RECT(int X, int Y, int Width, int Height)
            {
                this.Left = X;
                this.Top = Y;
                this.Right = Width;
                this.Bottom = Height;
            }
            internal int Left;
            internal int Top;
            internal int Right;
            internal int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PAINTSTRUCT
        {
            internal IntPtr hdc;
            internal int fErase;
            internal RECT rcPaint;
            internal int fRestore;
            internal int fIncUpdate;
            internal int Reserved1;
            internal int Reserved2;
            internal int Reserved3;
            internal int Reserved4;
            internal int Reserved5;
            internal int Reserved6;
            internal int Reserved7;
            internal int Reserved8;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
            public MARGINS(int Left, int Right, int Top, int Bottom)
            {
                this.cxLeftWidth = Left;
                this.cxRightWidth = Right;
                this.cyTopHeight = Top;
                this.cyBottomHeight = Bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NCCALCSIZE_PARAMS
        {
            internal RECT rect0, rect1, rect2;
            internal IntPtr lppos;
        }
        #endregion

        #region API
        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hdc, ref MARGINS marInset);

        [DllImport("dwmapi.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DwmDefWindowProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, ref IntPtr result);

        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int x, int y, int cx, int cy, uint flags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PtInRect([In] ref RECT lprc, Point pt);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetClientRect(IntPtr hWnd, ref RECT r);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateSolidBrush(int crColor);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        private static extern int FillRect(IntPtr hDC, [In] ref RECT lprc, IntPtr hbr);

        [DllImport("gdi32.dll")]
        private static extern IntPtr GetStockObject(int fnObject);

        [DllImport("user32.dll")]
        private static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool InflateRect(ref RECT lprc, int dx, int dy);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OffsetRect(ref RECT lprc, int dx, int dy);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);
        #endregion

        #region Fields
        private bool _bPaintWindow = false;
        private bool _bDrawCaption = false;
        private bool _bIsCompatible = false;
        private bool _bIsAero = false;
        private bool _bPainting = false;
        private bool _bExtendIntoFrame = false;
        private int _iCaptionHeight = CAPTION_HEIGHT;
        private int _iFrameHeight = FRAME_WIDTH;
        private int _iFrameWidth = FRAME_WIDTH;
        private int _iFrameOffset = 100;
        private int _iStoreHeight = 0;
        private RECT _tClientRect = new RECT();
        private MARGINS _tMargins = new MARGINS();
        private RECT[] _tButtonSize = new RECT[3];
        #endregion

        #region Properties
        private int CaptionHeight
        {
            get { return _iCaptionHeight; }
        }

        private int FrameWidth
        {
            get { return _iFrameWidth; }
        }

        private int FrameHeight
        {
            get { return _iFrameHeight; }
        }
        #endregion



     #region Methods
        public static bool bWin10_Style = false;
        private void ExtendMargins(int left, int top, int right, int bottom, bool drawcaption, bool intoframe)
        {

            if ( !SysAPI.IsWindows7andLower()) {
                bWin10_Style = true;
            }



            // any negative value causes whole window client to extend
            if (left < 0 || top < 0 || right < 0 || bottom < 0)
            {
                _bPaintWindow = true;
                _tMargins.cyTopHeight = -1;
            }
            // only caption can be extended
            else if (intoframe)
            {
                _tMargins.cxLeftWidth = 0;
                _tMargins.cyTopHeight = top;
                _tMargins.cxRightWidth = 0;
                _tMargins.cyBottomHeight = 0;
            }
            // normal extender
            else
            {
                _tMargins.cxLeftWidth = left;
                _tMargins.cyTopHeight = top;
                _tMargins.cxRightWidth = right;
                _tMargins.cyBottomHeight = bottom;
            }
            _bExtendIntoFrame = intoframe;
            _bDrawCaption = drawcaption;
            _bIsCompatible = IsCompatableOS();
            _bIsAero = IsAero();
        }

        private void GetFrameSize()
        {
            if (this.MinimizeBox)
                _iFrameOffset = 100;
            else
                _iFrameOffset = 40;
            switch (this.FormBorderStyle)
            {
                case FormBorderStyle.Sizable:
                    {
                        _iCaptionHeight = CAPTION_HEIGHT;
                        _iFrameHeight = FRAME_WIDTH;
                        _iFrameWidth = FRAME_WIDTH;
                        break;
                    }
                case FormBorderStyle.Fixed3D:
                    {
                        _iCaptionHeight = 27;
                        _iFrameHeight = 4;
                        _iFrameWidth = 4;
                        break;
                    }
                case FormBorderStyle.FixedDialog:
                    {
                        _iCaptionHeight = 25;
                        _iFrameHeight = 2;
                        _iFrameWidth = 2;
                        break;
                    }
                case FormBorderStyle.FixedSingle:
                    {
                        _iCaptionHeight = 25;
                        _iFrameHeight = 2;
                        _iFrameWidth = 2;
                        break;
                    }
                case FormBorderStyle.FixedToolWindow:
                    {
                        _iFrameOffset = 20;
                        _iCaptionHeight = 21;
                        _iFrameHeight = 2;
                        _iFrameWidth = 2;
                        break;
                    }
                case FormBorderStyle.SizableToolWindow:
                    {
                        _iFrameOffset = 20;
                        _iCaptionHeight = 26;
                        _iFrameHeight = 4;
                        _iFrameWidth = 4;
                        break;
                    }
                default:
                    {
                        _iCaptionHeight = CAPTION_HEIGHT;
                        _iFrameHeight = FRAME_WIDTH;
                        _iFrameWidth = FRAME_WIDTH;
                        break;
                    }
            }
        }

        private HIT_CONSTANTS HitTest()
        {
            RECT windowRect = new RECT();
            Point cursorPoint = new Point();
            RECT posRect;
            GetCursorPos(ref cursorPoint);
            GetWindowRect(this.Handle, ref windowRect);
            cursorPoint.X -= windowRect.Left;
            cursorPoint.Y -= windowRect.Top;
            int width = windowRect.Right - windowRect.Left;
            int height = windowRect.Bottom - windowRect.Top;

            posRect = new RECT(0, 0, FrameWidth, FrameHeight);
            if (PtInRect(ref posRect, cursorPoint))
                return HIT_CONSTANTS.HTTOPLEFT;

            posRect = new RECT(width - FrameWidth, 0, width, FrameHeight);
            if (PtInRect(ref posRect, cursorPoint))
                return HIT_CONSTANTS.HTTOPRIGHT;

            posRect = new RECT(FrameWidth, 0, width - (FrameWidth * 2) - _iFrameOffset, FrameHeight);
            if (PtInRect(ref posRect, cursorPoint))
                return HIT_CONSTANTS.HTTOP;

            posRect = new RECT(FrameWidth, FrameHeight, width - ((FrameWidth * 2) + _iFrameOffset), _tMargins.cyTopHeight);
            if (PtInRect(ref posRect, cursorPoint))
                return HIT_CONSTANTS.HTCAPTION;

            posRect = new RECT(0, FrameHeight, FrameWidth, height - FrameHeight);
            if (PtInRect(ref posRect, cursorPoint))
                return HIT_CONSTANTS.HTLEFT;

            posRect = new RECT(0, height - FrameHeight, FrameWidth, height);
            if (PtInRect(ref posRect, cursorPoint))
                return HIT_CONSTANTS.HTBOTTOMLEFT;

            posRect = new RECT(FrameWidth, height - FrameHeight, width - FrameWidth, height);
            if (PtInRect(ref posRect, cursorPoint))
                return HIT_CONSTANTS.HTBOTTOM;

            posRect = new RECT(width - FrameWidth, height - FrameHeight, width, height);
            if (PtInRect(ref posRect, cursorPoint))
                return HIT_CONSTANTS.HTBOTTOMRIGHT;

            posRect = new RECT(width - FrameWidth, FrameHeight, width, height - FrameHeight);
            if (PtInRect(ref posRect, cursorPoint))
                return HIT_CONSTANTS.HTRIGHT;

            return HIT_CONSTANTS.HTCLIENT;
        }

        public bool IsAero()
        {
            int enabled = 0;
            DwmIsCompositionEnabled(ref enabled);
            return (enabled == 1);
        }

        public bool IsCompatableOS()
        {
            return (Environment.OSVersion.Version.Major >= 6);
        }

        private void FrameChanged()
        {
            RECT rcClient = new RECT();
            GetWindowRect(this.Handle, ref rcClient);
            // force a calc size message
            SetWindowPos(this.Handle,
                         IntPtr.Zero,
                         rcClient.Left, rcClient.Top,
                         rcClient.Right - rcClient.Left, rcClient.Bottom - rcClient.Top,
                         SWP_FRAMECHANGED);
        }

        private void InvalidateWindow()
        {
            RedrawWindow(this.Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_UPDATENOW | RDW_INVALIDATE | RDW_ERASE);
        }

        private void PaintThis(IntPtr hdc, RECT rc)
        {
            RECT clientRect = new RECT();
            GetClientRect(this.Handle, ref clientRect);
            if (_bExtendIntoFrame)
            {
                clientRect.Left = _tClientRect.Left - _tMargins.cxLeftWidth;
                clientRect.Top = _tMargins.cyTopHeight;
                clientRect.Right -= _tMargins.cxRightWidth;
                clientRect.Bottom -= _tMargins.cyBottomHeight;
            }
            else if (!_bPaintWindow)
            {
                clientRect.Left = _tMargins.cxLeftWidth;
                clientRect.Top = _tMargins.cyTopHeight;
                clientRect.Right -= _tMargins.cxRightWidth;
                clientRect.Bottom -= _tMargins.cyBottomHeight;
            }
            if (!_bPaintWindow)
            {
                int clr;
                IntPtr hb;
                using (ClippingRegion cp = new ClippingRegion(hdc, clientRect, rc))
                {
                    if (IsAero())
                    {
                        FillRect(hdc, ref rc, GetStockObject(BLACK_BRUSH));
                    }
                    else
                    {
                        clr = ColorTranslator.ToWin32(Color.FromArgb(0xC2, 0xD9, 0xF7));
                        hb = CreateSolidBrush(clr);
                        FillRect(hdc, ref clientRect, hb);
                        DeleteObject(hb);
                    }
                }
                clr = ColorTranslator.ToWin32(this.BackColor);
                hb = CreateSolidBrush(clr);
                FillRect(hdc, ref clientRect, hb);
                DeleteObject(hb);
            }
            else
            {
                FillRect(hdc, ref rc, GetStockObject(BLACK_BRUSH));
            }
            if (_bExtendIntoFrame && _bDrawCaption)
            {
                Rectangle captionBounds = new Rectangle(4, 4, rc.Right, CaptionHeight);
                using (Graphics g = Graphics.FromHdc(hdc))
                {
                    using (Font fc = new Font("_typewriter", 12, FontStyle.Bold))
                    {
                        SizeF sz = g.MeasureString(this.Text, fc);
                        /*
                        int offset = (rc.Right - (int)sz.Width) / 2;
                        if (offset < 2 * FrameWidth)
                            offset = 2 * FrameWidth;
                        captionBounds.X = offset;*/
                        captionBounds.X = 20;
                        captionBounds.Y = 20;
                        using (StringFormat sf = new StringFormat())
                        {
                            sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                            sf.FormatFlags = StringFormatFlags.NoWrap;
                            sf.Alignment = StringAlignment.Near;
                            sf.LineAlignment = StringAlignment.Near;
                            using (GraphicsPath path = new GraphicsPath())
                            {
                                g.SmoothingMode = SmoothingMode.HighQuality;
                                path.AddString(this.Text, fc.FontFamily, (int)fc.Style, fc.Size, captionBounds, sf);
                                g.FillPath(Brushes.WhiteSmoke, path);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region WndProc
        protected override void WndProc(ref Message m)
        {
            if (_bIsCompatible)
                CustomProc(ref m);
            else
                base.WndProc(ref m);
        }

        protected void CustomProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PAINT:
                    {
                        PAINTSTRUCT ps = new PAINTSTRUCT();
                        if (!_bPainting)
                        {
                            _bPainting = true;
                            BeginPaint(m.HWnd, ref ps);
                            PaintThis(ps.hdc, ps.rcPaint);
                            EndPaint(m.HWnd, ref ps);
                            _bPainting = false;
                            base.WndProc(ref m);
                        }
                        else
                        {
                            base.WndProc(ref m);
                        }
                        break;
                    }
                case WM_CREATE:
                    {
                        GetFrameSize();
                        FrameChanged();
                        m.Result = MSG_HANDLED;
                        base.WndProc(ref m);
                        break;
                    }
                case WM_NCCALCSIZE:
                    {

                
                        if (m.WParam != IntPtr.Zero && m.Result == IntPtr.Zero){
                            if (_bExtendIntoFrame){
                                if(bWin10_Style) {
                                    NCCALCSIZE_PARAMS nc = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));
                                    nc.rect0.Right -= 6;
                                    nc.rect1 = nc.rect0;
                                    Marshal.StructureToPtr(nc, m.LParam, false);
                                    m.Result = (IntPtr)WVR_VALIDRECTS;
                                }else {
                                    NCCALCSIZE_PARAMS nc = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));
                                    nc.rect0.Top -= (_tMargins.cyTopHeight > CaptionHeight ? CaptionHeight : _tMargins.cyTopHeight);
                                    nc.rect1 = nc.rect0;
                                    Marshal.StructureToPtr(nc, m.LParam, false);
                                    m.Result = (IntPtr)WVR_VALIDRECTS;
                                   base.WndProc(ref m);
                                }
                            }
                        }else {
                            base.WndProc(ref m);
                        }
                        break;
                
                /*
                        if (m.WParam != IntPtr.Zero && m.Result == IntPtr.Zero)
                        {
                            if (_bExtendIntoFrame)
                            {
                                NCCALCSIZE_PARAMS nc = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));
                                nc.rect0.Top -= (_tMargins.cyTopHeight > CaptionHeight ? CaptionHeight : _tMargins.cyTopHeight);
                                nc.rect1 = nc.rect0;
                                Marshal.StructureToPtr(nc, m.LParam, false);
                                m.Result = (IntPtr)WVR_VALIDRECTS;
                            }
                            base.WndProc(ref m);
                        }
                        else
                        {
                            base.WndProc(ref m);
                        }
                        break;
                        */




                    }
                case WM_SYSCOMMAND:
                    {
                        UInt32 param;
                        if (IntPtr.Size == 4)
                            param = (UInt32)(m.WParam.ToInt32());
                        else
                            param = (UInt32)(m.WParam.ToInt64());
                        if ((param & 0xFFF0) == SC_RESTORE)
                        {
                            this.Height = _iStoreHeight;
                        }
                        else if (this.WindowState == FormWindowState.Normal)
                        {
                            _iStoreHeight = this.Height;
                        }
                        base.WndProc(ref m);
                        break;
                    }
                case WM_NCHITTEST:
                    {
                        if (m.Result == (IntPtr)HIT_CONSTANTS.HTNOWHERE)
                        {
                            IntPtr res = IntPtr.Zero;
                            if (DwmDefWindowProc(m.HWnd, (uint)m.Msg, m.WParam, m.LParam, ref res))
                                m.Result = res;
                            else
                                m.Result = (IntPtr)HitTest();
                        }
                        else
                            base.WndProc(ref m);
                        break;
                    }
                case WM_DWMCOMPOSITIONCHANGED:
                case WM_ACTIVATE:
                    {
                        DwmExtendFrameIntoClientArea(this.Handle, ref _tMargins);
                        m.Result = MSG_HANDLED;
                        base.WndProc(ref m);
                        break;
                    }
                default:
                    {
                        base.WndProc(ref m);
                        break;
                    }
            }
        }
        #endregion

        #region Clipping Region
        /// <summary>Clip rectangles or rounded rectangles</summary>
        internal class ClippingRegion : IDisposable
        {
            #region Enum
            private enum CombineRgnStyles : int
            {
                RGN_AND = 1,
                RGN_OR = 2,
                RGN_XOR = 3,
                RGN_DIFF = 4,
                RGN_COPY = 5,
                RGN_MIN = RGN_AND,
                RGN_MAX = RGN_COPY
            }
            #endregion

            #region API
            [DllImport("gdi32.dll")]
            private static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

            [DllImport("gdi32.dll")]
            private static extern int GetClipRgn(IntPtr hdc, [In, Out]IntPtr hrgn);

            [DllImport("gdi32.dll")]
            private static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

            [DllImport("gdi32.dll")]
            private static extern IntPtr CreateEllipticRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

            [DllImport("gdi32.dll")]
            private static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

            [DllImport("gdi32.dll")]
            private static extern int CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, CombineRgnStyles fnCombineMode);

            [DllImport("gdi32.dll")]
            private static extern int ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool DeleteObject(IntPtr hObject);
            #endregion

            #region Fields
            private IntPtr _hClipRegion;
            private IntPtr _hDc;
            #endregion

            #region Methods
            public ClippingRegion(IntPtr hdc, Rectangle cliprect, Rectangle canvasrect)
            {
                CreateRectangleClip(hdc, cliprect, canvasrect);
            }

            public ClippingRegion(IntPtr hdc, RECT cliprect, RECT canvasrect)
            {
                CreateRectangleClip(hdc, cliprect, canvasrect);
            }

            public ClippingRegion(IntPtr hdc, Rectangle cliprect, Rectangle canvasrect, uint radius)
            {
                CreateRoundedRectangleClip(hdc, cliprect, canvasrect, radius);
            }

            public ClippingRegion(IntPtr hdc, RECT cliprect, RECT canvasrect, uint radius)
            {
                CreateRoundedRectangleClip(hdc, cliprect, canvasrect, radius);
            }

            public void CreateRectangleClip(IntPtr hdc, Rectangle cliprect, Rectangle canvasrect)
            {
                _hDc = hdc;
                IntPtr clip = CreateRectRgn(cliprect.Left, cliprect.Top, cliprect.Right, cliprect.Bottom);
                IntPtr canvas = CreateRectRgn(canvasrect.Left, canvasrect.Top, canvasrect.Right, canvasrect.Bottom);
                _hClipRegion = CreateRectRgn(canvasrect.Left, canvasrect.Top, canvasrect.Right, canvasrect.Bottom);
                CombineRgn(_hClipRegion, canvas, clip, CombineRgnStyles.RGN_DIFF);
                SelectClipRgn(_hDc, _hClipRegion);
                DeleteObject(clip);
                DeleteObject(canvas);
            }

            public void CreateRectangleClip(IntPtr hdc, RECT cliprect, RECT canvasrect)
            {
                _hDc = hdc;
                IntPtr clip = CreateRectRgn(cliprect.Left, cliprect.Top, cliprect.Right, cliprect.Bottom);
                IntPtr canvas = CreateRectRgn(canvasrect.Left, canvasrect.Top, canvasrect.Right, canvasrect.Bottom);
                _hClipRegion = CreateRectRgn(canvasrect.Left, canvasrect.Top, canvasrect.Right, canvasrect.Bottom);
                CombineRgn(_hClipRegion, canvas, clip, CombineRgnStyles.RGN_DIFF);
                SelectClipRgn(_hDc, _hClipRegion);
                DeleteObject(clip);
                DeleteObject(canvas);
            }

            public void CreateRoundedRectangleClip(IntPtr hdc, Rectangle cliprect, Rectangle canvasrect, uint radius)
            {
                int r = (int)radius;
                _hDc = hdc;
                // create rounded regions
                IntPtr clip = CreateRoundRectRgn(cliprect.Left, cliprect.Top, cliprect.Right, cliprect.Bottom, r, r);
                IntPtr canvas = CreateRectRgn(canvasrect.Left, canvasrect.Top, canvasrect.Right, canvasrect.Bottom);
                _hClipRegion = CreateRoundRectRgn(canvasrect.Left, canvasrect.Top, canvasrect.Right, canvasrect.Bottom, r, r);
                CombineRgn(_hClipRegion, canvas, clip, CombineRgnStyles.RGN_DIFF);
                // add it in
                SelectClipRgn(_hDc, _hClipRegion);
                DeleteObject(clip);
                DeleteObject(canvas);
            }

            public void CreateRoundedRectangleClip(IntPtr hdc, RECT cliprect, RECT canvasrect, uint radius)
            {
                int r = (int)radius;
                _hDc = hdc;
                // create rounded regions
                IntPtr clip = CreateRoundRectRgn(cliprect.Left, cliprect.Top, cliprect.Right, cliprect.Bottom, r, r);
                IntPtr canvas = CreateRectRgn(canvasrect.Left, canvasrect.Top, canvasrect.Right, canvasrect.Bottom);
                _hClipRegion = CreateRoundRectRgn(canvasrect.Left, canvasrect.Top, canvasrect.Right, canvasrect.Bottom, r, r);
                CombineRgn(_hClipRegion, canvas, clip, CombineRgnStyles.RGN_DIFF);
                // add it in
                SelectClipRgn(_hDc, _hClipRegion);
                DeleteObject(clip);
                DeleteObject(canvas);
            }

            public void Release()
            {
                if (_hClipRegion != IntPtr.Zero)
                {
                    // remove region
                    SelectClipRgn(_hDc, IntPtr.Zero);
                    // delete region
                    DeleteObject(_hClipRegion);
                }
            }

            public void Dispose()
            {
                Release();
            }
            #endregion
        }



        #endregion

        #endregion

        private void label2_Click(object sender, EventArgs e) {

        }



        private void vMyScrollBar_Click(object sender, EventArgs e) {

        }

        private void hMyScrollBar_Click_1(object sender, EventArgs e) {

        }

        private void setCwcToolStripMenuItem_Click(object sender, EventArgs e) {
            fShowInstallForm();
			setCwcToolStripMenuItem.Enabled = false;
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
		            //  oInstallForm.Location = new Point( Cons_x  + (Cons_width-Cons_x) / 2 - oInstallForm.Width / 2,  Cons_y  + (Cons_height-Cons_y) / 2 - hScrollBar.Height  - oInstallForm.Height / 2);
		              oInstallForm.Location = new Point( Location.X,Location.Y);
		             oInstallForm.Show(this);
			
            });
          }
            private void fInstallFormClosing(Object sender, FormClosingEventArgs e){
			setCwcToolStripMenuItem.Enabled = true;
        }

        private void lauchToolStripMenuItem_Click(object sender, EventArgs e) {
           
        }

        private void workingDirToolStripMenuItem_Click(object sender, EventArgs e) {
             try {
                Process.Start("explorer.exe", PathHelper.ExeWorkDir.Replace('/', '\\'));
            }   catch (Exception ex) {
                DialogHelper.ShowError(ex.ToString());
             }
        }

        private void outputToolStripMenuItem_Click(object sender, EventArgs e) {
            /*
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
            }*/
        }

        private void ToolStrip_Build_Click(object sender, EventArgs e) {
                 Data.oLauchProject.fCancel();
        }

        private void notePadToolStripMenuItem_Click(object sender, EventArgs e) {
           //  FileUtils.fLauchIDE("",  "" );
        }

        private void pathToolStripMenuItem_Click(object sender, EventArgs e) {

        }
         internal void fEnableBuild() {
               try {  this.BeginInvoke((MethodInvoker)delegate  {
                 Populate(true);
         //        ToolStrip_Build.Text = "STOP ";
                ToolStrip_Build.Image = Resources.Menu0007;
                ToolStrip_Build.BackColor = Color.DarkRed;
           });}catch(Exception e) { };
        }

        internal void fDisableBuild() {
            if(Base.bAlive) {
                try { this.BeginInvoke((MethodInvoker)delegate  {
                     //   ToolStrip_Build.Text = "BUILD";
                       ToolStrip_Build.Image = Resources.Menu0007;
                       ToolStrip_Build.BackColor = Color.Teal;
                  });
                }catch(Exception e) { };
            }
        }

        internal void fLauchPrj() {
             try {  this.BeginInvoke((MethodInvoker)delegate  {
            // ToolStrip_Build.Text = "END  ";
            ToolStrip_Build.Image = Resources.Menu0006;
            ToolStrip_Build.BackColor = Color.YellowGreen;
            }); }catch(Exception e) { };
        }

        public bool bCreated = false;


         public void fMaximizedState(){
              // Maximized!
                msMenu.Top = nInitialPosTop + 9;
                msMenu.Left =   Width - nInitialPosLeft - 10;
        }

        public bool NeedRestore = false;
        FormWindowState LastWindowState = FormWindowState.Minimized;
        public bool fIsMaximizeChange() {
          

              // When window state changes
                if (WindowState != LastWindowState) {
                    LastWindowState = WindowState;

                    if (WindowState == FormWindowState.Maximized) {
                         fMaximizedState();
           
                    }
                    if (WindowState == FormWindowState.Normal) {
                        // Restored!
                  
                            msMenu.Top = nInitialPosTop;
                            msMenu.Left = Width - nInitialPosLeft;
         //    Console.WriteLine("Restored " +    ConfigMng.oConfig.vStartPos);
         
                    Location = new Point(-999999,-999999);//Why??
                    Location = ConfigMng.oConfig.vStartPos; //Restore previous loc

                            return false;
                    }
                    return true;
                }
                  return false;
   
        }




        private void GuiConsole_Move(object sender, EventArgs e) {
            if(bCreated){
          //      ConfigMng.oConfig.vStartPos = Location;
            }


        }

        public static bool bIgnoreNextSelectionChange = false;
        private void fctb_SelectionChanged(object sender, EventArgs e) {
            if(!bIgnoreNextSelectionChange){
                 Data.bIWantGoToEnd = false;
            }
        //    bIgnoreNextSelectionChange = false;
     
        }

        private void vMyScrollBar_ControlAdded(object sender, ControlEventArgs e) {
                      
       
        }



        public void fClearLastLinkStyle() {
            
                if (oLastLinkStyle != null) {
                   oLastLinkStyle.ClearStyle(fctbConsole.GetStyleIndexMask( new Style[] { oHightLightStyle } ) );
                }
        }

        public Range oLastLinkStyle = null;
        private void fctb_MouseMove(object sender, MouseEventArgs e) {

              var p = fctbConsole.PointToPlace(e.Location);
      
             fClearLastLinkStyle();
            if (CharIsHyperlink(p)){
                fctbConsole.Cursor = Cursors.Hand;
                Range _oLink = fctbConsole.GetRange(p, p).GetFragment(oLinkStyle,false);
                
                 _oLink.SetStyle(oHightLightStyle);
                 oLastLinkStyle = _oLink;

            } else{
                fctbConsole.Cursor = Cursors.IBeam;
            }
            

        }
         bool CharIsHyperlink(Place place)
        {
            
            var mask = fctbConsole.GetStyleIndexMask( new Style[] { oLinkStyle });
            if (place.iChar < fctbConsole.GetLineLength(place.iLine))
                if ((fctbConsole[place].style & mask) != 0)
                    return true;

            return false;
        }

        private void fctb_MouseClick(object sender, MouseEventArgs e) {
            var p = fctbConsole.PointToPlace(e.Location);
            if (CharIsHyperlink(p))
            {
               // string _sLink = fctb.GetRange(p, p).GetFragment(oLinkStyle,false).Text;
                Range _oLink = fctbConsole.GetRange(p, p).GetFragment(oLinkStyle,false);

                string _sLink = _oLink.Text;

                string _sFile = fTestSelection(_sLink);
            

                if(_sLink.Length > 3 &&  _sLink[0] == 'L' &&  _sLink[1] == ':'  &&  _sLink[2] == '['){

                     Data.fAddRequiredModule( "VLiance/Demos"); //TODO others
                     // Data.sCmd = "StartBuild";
                           Thread winThread = new Thread(new ThreadStart(() =>  {  
                            Empty.fLoadModules();
                          }));  
		                 winThread.Start();

                }else { 
                    /*
                    //It's relative path?
                    if( !(_sLink.Length > 1 && _sLink[1] == ':') ) {

                        _sFile =  PathHelper.ExeWorkDir + _sFile;
                    }
                    */

                    FileUtils.RunInEditor(_sFile,  " -n" + nLine + " -c" +  nColomn);
                                  
                }
             //   _oLink.SetStyle(oHightLightStyle);

                    



            //   Console.WriteLine();
                /*
            string _sFile = PipeInput.sFile;
            if (PipeInput.bRealive) {
                _sFile = PathHelper.ExeWorkDir  + _sFile;
            }
			
			 // MainForm.RunInEditor(PipeInput.sFile);
		//	  MainForm.RunInEditor(PipeInput.sFile ,  " -lcpp" + " -n" + PipeInput.nLine + " -c" +  PipeInput.nColomn);
			  FileUtils.RunInEditor(_sFile,  " -n" + PipeInput.nLine + " -c" +  PipeInput.nColomn);
	*/

             //   var url = fctb.GetRange(p, p).GetFragment(@"[\S]").Text;
              //  Process.Start(url);
            }
        }



        /*
         FastColoredTextBox CurrentTB
        {
            get {
                if (tsFiles.SelectedItem == null)
                    return null;
                return (tsFiles.SelectedItem.Controls[0] as FastColoredTextBox);
            }

            set
            {
                tsFiles.SelectedItem = (value.Parent as FATabStripItem);
                value.Focus();
            }
        }*/

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
           //  CurrentTB.Copy();
           Fctb.Copy();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e) {

        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            Fctb.Selection.SelectAll();
        }

        private void GuiConsole_LocationChanged(object sender, EventArgs e) {
            
        }

        static bool bMouseDown = false;
        private void GuiConsole_MouseDown(object sender, MouseEventArgs e) {
        //    bMouseDown =true;
          //  Console.WriteLine("Down");
        }

        private void GuiConsole_MouseUp(object sender, MouseEventArgs e) {
            // bMouseDown =false;
             //   Console.WriteLine("Up");
        }

        private void GuiConsole_DragEnter(object sender, DragEventArgs e) {
            
          //  Console.WriteLine("asss");

        }

        


        private void GuiConsole_ResizeBegin(object sender, EventArgs e) {
      
           if(bCreated){

                ConfigMng.oConfig.vStartPos = Location;

            }
        }

       private void GuiConsole_Resize(object sender, EventArgs e) {

            if(bCreated){
         
                if (!fIsMaximizeChange()) {
                   ConfigMng.oConfig.vStartSize = Size;
                
                }

            }
     
            fUpdateScrollBar();
        }

        private void GuiConsole_ResizeEnd(object sender, EventArgs e) {
           fUpdateScrollBar();

           if(bCreated){
               if (WindowState != FormWindowState.Maximized) {
                     ConfigMng.oConfig.vStartPos = Location;
                }
            }
        }

        public void fUpdateScrollBar() {

            fSetTreeViewScrollBarMaximum(treeViewPrj);
            vTreePrjScrollBar.fUpdate();
            hTreePrjScrollBar.fUpdate();

            hMyScrollBar.fUpdate();
            vMyScrollBar.fUpdate();
        }



        private void treeViewFolderBrowser1_SelectedDirectoriesChanged(object sender, Raccoom.Windows.Forms.SelectedDirectoriesChangedEventArgs e) {

        }

        private void panel1_Paint(object sender, PaintEventArgs e) {

        }


        private void vTreePrjScrollBar_Scroll(object sender, ScrollEventArgs e) {

           treeViewPrj.Location = new Point(  treeViewPrj.Location.X, nTreePrjScrollBar_IniY + e.NewValue *-1);
        }
        private void hTreePrjScrollBar_Scroll(object sender, ScrollEventArgs e) {
 
           treeViewPrj.Location = new Point( nTreePrjScrollBar_IniX + e.NewValue *-1,  treeViewPrj.Location.Y);
        }



        private const int nGrowSpeed = 10;
        private const int nMinSize= 50;

        private const int GWL_STYLE = -16;
        private const int WS_VSCROLL = 0x00200000;
        private const int WS_HSCROLL = 0x00100000;
        [DllImport("user32.dll", ExactSpelling = false, CharSet = CharSet.Auto)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        public static void fAutoSizeTree(TreeView _oTree, Panel _oBorder, bool _bExpand = false) {

              int style = GetWindowLong( _oTree.Handle, GWL_STYLE);
                if(!_bExpand){
                    while ( _oTree.Width > nMinSize && (style & WS_HSCROLL) == 0) {
                        _oBorder.Width-=nGrowSpeed;
                        _oTree.Width-=nGrowSpeed;
                        style = GetWindowLong( _oTree.Handle, GWL_STYLE);
                    }
                    while ( _oTree.Height > nMinSize &&  (style & WS_VSCROLL) == 0) {
                       
                         _oBorder.Height-=nGrowSpeed;
                        _oTree.Height-=nGrowSpeed;
                        style = GetWindowLong( _oTree.Handle, GWL_STYLE);
                    }
                }
               while ((style & WS_HSCROLL) != 0){
                     _oTree.Width+=nGrowSpeed;
                    style = GetWindowLong( _oTree.Handle, GWL_STYLE);
                }
               while ((style & WS_VSCROLL) != 0) {
                    _oTree.Height+=nGrowSpeed;
                    style = GetWindowLong( _oTree.Handle, GWL_STYLE);
                }
            
            //Hide ScrollBar
             _oTree.Height+=50;
             _oTree.Width+=50;
             _oBorder.Width  =_oTree.Width-50;
            _oBorder.Height  =_oTree.Height-50;
        }

        //Autosize
        private void treeViewPrj_AfterExpand(object sender, TreeViewEventArgs e) {
              fAutoSizeTree((TreeView) sender, pnTtreeViewPrj, true);
              fSetTreeViewScrollBarMaximum((TreeView) sender);
         //   Setting.oSettingsLauch.fSetNodes((TreeView) sender);
        }

        private void treeViewPrj_AfterCollapse(object sender, TreeViewEventArgs e) {
                fAutoSizeTree((TreeView) sender,pnTtreeViewPrj);
                fSetTreeViewScrollBarMaximum((TreeView) sender);
          //       Setting.oSettingsLauch.fSetNodes((TreeView) sender);
            //pnTreeCenter.Refresh();
           // pnTreeCenter.AutoScrollPosition = new Point( pnTreeCenter.Location.X + 10, pnTreeCenter.Location.Y);
           // pnTreeCenter.AutoScrollPosition = new Point( pnTreeCenter.Location.X + 10, pnTreeCenter.Location.Y);
         
        }

        public void fSetTreeViewScrollBarMaximum(TreeView _oTree) {
            hTreePrjScrollBar.Maximum = _oTree.Width - hTreePrjScrollBar.Width; 
            vTreePrjScrollBar.Maximum = _oTree.Height- vTreePrjScrollBar.Height + 50;
           if (vTreePrjScrollBar.Maximum < 1) {
               vTreePrjScrollBar.Maximum = 1;
            }
            if (hTreePrjScrollBar.Maximum < 1) {
               hTreePrjScrollBar.Maximum = 1;
            }
           //Console.WriteLine("hTreePrjScrollBar " +  hTreePrjScrollBar.Maximum );
           // Console.WriteLine("vTreePrjScrollBar " +  vTreePrjScrollBar.Maximum );
        }

        private void pnTreeCenter_Paint(object sender, PaintEventArgs e) {

        }

        private void vTreePrjScrollBar_Click(object sender, EventArgs e) {

        }

        private void collapsibleSplitter2_SplitterMoved(object sender, SplitterEventArgs e) {
            fUpdateScrollBar();
        }

        private void treeViewPrj_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
          
            if (e.Node is TreeNodePath) {
                if (!Directory.Exists(((TreeNodePath)e.Node).Path)) { //It's a file
                    FileUtils.RunInEditor( ((TreeNodePath)e.Node).Path,  "");
                }
            }
        }

        private void viewInToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void nothingToolStripMenuItem1_Click(object sender, EventArgs e) {
            //fCheckMenu(sender,e);
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e) {
           // fCheckMenu(sender,e);
        }

        private void sanitizeToolStripMenuItem1_Click(object sender, EventArgs e) {
           //  fCheckMenu(sender,e);
        }

        private void afterFileErrorToolStripMenuItem_Click(object sender, EventArgs e) {
           //  fCheckMenu(sender,e);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void iDEToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
             FileUtils.fLauchIDE( PathHelper.ToolDir +  "npp/notepad++.exe", "",  "" );
        }

        private void gDBToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
              Data.oLauchProject.fLauchDefaultRun("");
        }
    }





    #region MyScrollBar
    /*
    public class MyScrollBar: Control
    {

        public  int nDelta = 0;
         

        private int @value;

        public int Value
        {
            get { return value; }
            set {
                if (this.value == value)
                    return;
                this.value = value;
                Invalidate();
                OnScroll();
            }
        }

        private int maximum = 0;
        public int Maximum
        {
            get { return maximum; }
            set { maximum = value; Invalidate(); }
        }

        public int thumbSize = 1000;
        public int ThumbSize
        {
            get { return thumbSize; }
            set { thumbSize = value; Invalidate(); }
        }

        private Color thumbColor = Color.Gray;
        public Color ThumbColor
        {
            get { return thumbColor; }
            set { thumbColor = value; Invalidate(); }
        }

        private Color borderColor = Color.Silver;
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        private ScrollOrientation orientation;
        public ScrollOrientation Orientation
        {
            get { return orientation; }
            set { orientation = value; Invalidate(); }
        }

        public event ScrollEventHandler Scroll;

        public MyScrollBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);


        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                MouseScroll(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                MouseScroll(e);
            base.OnMouseMove(e);
        }


        private void MouseScroll(MouseEventArgs e)
        {
            if(GuiConsole.bDown){
            int v = 0;
            switch(Orientation)
            {
                case ScrollOrientation.VerticalScroll: v = Maximum * (e.Y - nDelta - thumbSize / 2) / (Height - thumbSize) ; break;
                case ScrollOrientation.HorizontalScroll: v = Maximum * (e.X - nDelta - thumbSize / 2) / (Width - thumbSize); break;
            }
            Value = Math.Max(0, Math.Min(Maximum, v));
            }

        }

        public double fGetCenter(int _nDim) {

            return ( (Value) * (_nDim - thumbSize) / Maximum) + thumbSize/2;
        }


        public virtual void OnScroll(int _nVal)
        {
            if (Scroll != null){
                Value+= _nVal *-1;
                if (Value< 0) {
                    Value = 0;
                }
                 if (Value > Maximum) {
                    Value = Maximum;
                }
                Scroll(this, new ScrollEventArgs(ScrollEventType.ThumbPosition,Value, Orientation));
            }
        }

        public virtual void OnScroll(ScrollEventType type = ScrollEventType.ThumbPosition)
        {
            if (Scroll != null)
                Scroll(this, new ScrollEventArgs(type, Value, Orientation));
        }


        public bool bFirst = true;

        public  double nMinimalSize = 30;
        public  double nToTumbSize = 0;
        public  double nToPos = 0;
        public  double nPos = 0;
        public  double nTumbSize = 0;
        protected override void OnPaint(PaintEventArgs e)
        {
            //    Console.WriteLine("Maximum " +  Maximum);
            if (bFirst) {
                bFirst = false;
                if(Height > Width) {
                  nTumbSize = Height/2.0;
                } else {
                    nTumbSize = Width/2.0;
                }
                nToTumbSize = nTumbSize;
              //  return;
            }

           
            Rectangle thumbRect = Rectangle.Empty;
            switch(Orientation)
            {
                case ScrollOrientation.HorizontalScroll:
                    nToTumbSize  = Width/2.0-Maximum/20;
                    thumbSize = (int) nTumbSize;
                    if (nToTumbSize < nMinimalSize) { //Minimal
                      nToTumbSize = nMinimalSize;
                    }
                
                      thumbRect = new Rectangle( (int)(nPos) * (Width - thumbSize) / Maximum, 2, thumbSize, Height - 4);
                
                    //  thumbRect = new Rectangle(value * (Width - thumbSize) / Maximum, 2, thumbSize, Height - 4);
                    break;
                case ScrollOrientation.VerticalScroll:

                    nToTumbSize  = Height/2.0-Maximum/20;
                    thumbSize = (int) nTumbSize;
                    if (nToTumbSize < nMinimalSize) { //Minimal
                      nToTumbSize = nMinimalSize;
                    }

                  //  nToPos = value;
                 
                   // thumbRect = new Rectangle(2, (value) * (Height - thumbSize) / Maximum, Width - 4, thumbSize);
                    thumbRect = new Rectangle(2, (int)(nPos) * (Height - thumbSize) / Maximum, Width - 4, thumbSize);
                    break;
            }

            using(var brush = new SolidBrush(thumbColor))
                e.Graphics.FillRectangle(brush, thumbRect);

            using (var pen = new Pen(borderColor))
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
        }

        internal void fUpdate() {   
            this.BeginInvoke((MethodInvoker)delegate  {
                      if(Base.bAlive){
                    nPos += (value  - nPos) / 5;


                    nTumbSize  +=  (nToTumbSize - nTumbSize )/ 10.0;
                    if(Math.Abs(nTumbSize  - nToTumbSize) >= 1.0 ||  Math.Abs(nPos  - value) >= 1.0){
                       this.Refresh();
                    }
              }
            });
        }
    }
    */
    #endregion



}
