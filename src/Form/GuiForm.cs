using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cwc {
    public partial class GuiForm : Form {

        GuiConsole oEmptyForm = null;
        bool bMustBePositioned = false;



        public GuiForm(GuiConsole _oParent) {
            oEmptyForm = _oParent;
            InitializeComponent();

        }

        private void GuiForm_Load(object sender, EventArgs e) {
   
            NativeMethods.SetParent( Handle,oEmptyForm.Handle);

           Location  = new Point(0,-10);
            Size = new Size(msMenu.Width,msMenu.Height);
          
      
           if( Sys.nConnectedHandle != 0){ //Resolve
                foreach (ToolStripMenuItem _oTS in msMenu.Items) {
                    _oTS.MouseUp += ToolStripMenuItem_Click;
                }
              //  Visible = false;
                // Opacity = 1;
               // fHide();
               //  Opacity = 1; //Bug if not set to 1 when setparent

            } else {
                bMustBePositioned = true;
                msMenu.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            }



            foreach (Object _oTS_o in updateToolStripMenuItem.DropDownItems) {
                if(_oTS_o is ToolStripMenuItem){
                    ToolStripMenuItem _oTS = (ToolStripMenuItem)_oTS_o;

                    _oTS.Click += ToolStrip_Update_Click;
                }
            }


           	
			if (Data.sArg == "" ) {
                //  if (!oMainForm.fCheckForDemos()) {
                if (!fCheckForDemos()) {
                    if (SystemPathModifier.fIsSet("") == 0)
                    {
                        fShowInstallForm();
                    }
                }
                 
            }
         
            fLoadData();
        }

   

        int nBorderRight = 130;
        int nBorderLeft = 10;
        int nBorderTop = 6;

        int nLastX = 0;
        int nLastY = 0;
        int nLast_Width =0;
        int nLast_Height =0;


        internal void fSetPosition(int x, int y, int width, int height, int clientWidth, int clientHeight) {
            
            // Console.WriteLine(oEmptyForm.Location.X + " " + oEmptyForm.Location.Y );
            if(bMustBePositioned){
                     this.BeginInvoke((MethodInvoker)delegate  {                         
         //            msMenu.Focus();
                          if (x != nLastX || y != nLastY ||  width != nLast_Width || height != nLast_Height) {
                            nLastX = x;nLastY=y;nLast_Width=width;nLast_Height=height;
                      
                             fSetSizeAndPos(x+nBorderLeft,  y+nBorderTop, width-nBorderRight-nBorderLeft,msMenu.Height);
                         
                             Opacity = 1;
                
                        }












                    });
            }





        }



        internal void fSetSizeAndPos(int _nLeft, int _nTop,int _nWidth, int _nHeight) {
            int _nMaxSize = msMenu.Width;
            int _nRightPos = _nLeft + _nWidth;
            int _nLeftPos = _nRightPos - _nMaxSize;
            if (_nLeftPos < _nLeft) {
                 _nMaxSize = _nMaxSize - (_nLeft - _nLeftPos);
                _nLeftPos = _nLeft;
            }

            Size = new Size(_nMaxSize,_nHeight);
            Location = new Point(_nLeftPos,_nTop);
        }



          private void ToolStripMenuItem_Click(object sender, EventArgs e) {

                   ToolStripMenuItem _oTS  =  ((ToolStripMenuItem)(sender));
                  if(! _oTS.IsOnDropDown){//not work??
                         this.BeginInvoke((MethodInvoker)delegate  {	
                            _oTS.ShowDropDown();
                      });
                  } else {
                      _oTS.HideDropDown();
                  }

        }

          private void ToolStrip_Update_Click(object sender, EventArgs e) {

                ToolStripMenuItem _oTS  =  ((ToolStripMenuItem)(sender));
               // Console.WriteLine("Click name : " + _oTS.Text);
            	 UpdateForm.Show(this, false, _oTS.Text );
        }





        private void configToolStripMenuItem_Click(object sender, EventArgs e) {
            

        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e) {
            
        }

        private void viewInToolStripMenuItem_Click(object sender, EventArgs e) {
           
        }

        private void buildAndToolStripMenuItem_Click(object sender, EventArgs e) {
              
        }

        private void GuiForm_MouseLeave(object sender, EventArgs e) {

        }

        private void GuiForm_MouseMove(object sender, MouseEventArgs e) {
        }

        private void viewInToolStripMenuItem_MouseDown(object sender, MouseEventArgs e) {

        }

        private void msMenu_MouseLeave(object sender, EventArgs e) {
             bOver = false;
           Debug.fPrintBackup();
        }

        private void msMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            
        }

          internal void fHide() {
              this.BeginInvoke((MethodInvoker)delegate  {           
    
                       Size = new Size(0,0);
                });
        }
        internal void fShow() {
              this.BeginInvoke((MethodInvoker)delegate  {           
  
                    Size = new Size(msMenu.Width,msMenu.Height);
                });
        }

        private void lauchToolStripMenuItem_Click(object sender, EventArgs e) {
            string _sResult = CompilerConfig.fDialogExeFile( PathHelper.ExeWorkDir, "",  "Commands (*.cwMake,*.bat)|*.cwMake;*.bat|Executable (*.exe)|*.exe|All files (*.*)|*.*");
			if(_sResult.Length > 0){
                Delocalise.fDelocaliseInMainThread(PathHelper.ExeWorkDir + _sResult);
                /*
				string _sText =  Data.fDelocalise(PathHelper.ExeWorkDir + _sResult);
				Data.sArgExpand  = Data.fExpandAll(_sText);
				Data.StartBuild();*/
			//	Debug.fTrace("!!!---- " + _sText);
			}
        }



        public bool fCheckForDemos() {

            string _sDemoDir = ModuleData.aMData["Honera/Demos"].sOutFolder;////BUUUUUUUUUUUUUUUUUUUUUUUUUGG on exit, thread!!?
		    //	Console.WriteLine(_sDemoDir);

			    if(!Directory.Exists(_sDemoDir)) {


                   RegisterFile.fRegisterAllFileType(true); //

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
			
		            //	Console.WriteLine("Cons_x " + Cons_x);
		
		             oInstallForm.Show(this);
			
            });
          
        }
          private void fInstallFormClosing(Object sender, FormClosingEventArgs e){
          //  btnInstall.Enabled = true;
			setCwcToolStripMenuItem.Enabled = true;
        }

        private void cwCToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void msMenu_MouseMove(object sender, MouseEventArgs e) {
              msMenu.Focus();
        }

        private void msMenu_MouseDown(object sender, MouseEventArgs e) {
    
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

        internal void fEnableBuild() {
             this.BeginInvoke((MethodInvoker)delegate  {
                 ToolStrip_Build.Text = "STOP ";
                ToolStrip_Build.BackColor = Color.DarkRed;
           });

        }

        internal void fDisableBuild() {
            this.BeginInvoke((MethodInvoker)delegate  {
                    ToolStrip_Build.Text = "BUILD";
                     ToolStrip_Build.BackColor = Color.Teal;
              });
        }

        internal void StartBuild() {
              this.BeginInvoke((MethodInvoker)delegate  {

              });
        }
      internal void fLauchPrj() {
                this.BeginInvoke((MethodInvoker)delegate  {
                ToolStrip_Build.Text = "END  ";
                ToolStrip_Build.BackColor = Color.YellowGreen;
            });
        }

        private void iDEToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void notePadToolStripMenuItem_Click(object sender, EventArgs e) {
            	//  FileUtils.fLauchNotepad("",  " -lcpp" );
            	//  FileUtils.fLauchIDE("",  "" );
        }

        private void setCwcToolStripMenuItem_Click(object sender, EventArgs e) {
            			  fShowInstallForm();
			setCwcToolStripMenuItem.Enabled = false;
        }

        internal void fFocus() {
              this.BeginInvoke((MethodInvoker)delegate  {
    
                
            });
     
        }

        private void msMenu_MouseCaptureChanged(object sender, EventArgs e) {

        }

        private void msMenu_MouseHover(object sender, EventArgs e) {
                msMenu.Focus();
        }


        
        private void aaaToolStripMenuItem_Click(object sender, EventArgs e) {
          //Data.aAllInclude=null;
//Data.aAllInclude.Add("aa");


        }

        private void GuiForm_KeyPress(object sender, KeyPressEventArgs e) {
            if(e.KeyChar == 0x1D) {
                   Build.StopBuild();
            }
        }

        private void GuiForm_KeyUp(object sender, KeyEventArgs e) {

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void afterFileErrorToolStripMenuItem_Click(object sender, EventArgs e) {
            fCheckMenu(sender,e);
        }

        public void fCheckMenu(object sender, EventArgs e) {
              ((ToolStripMenuItem)(sender)).Checked = !((ToolStripMenuItem)(sender)).Checked;

            string _sParentName = "";
              ToolStrip _oParent = ((ToolStripMenuItem)(sender)).GetCurrentParent();
            if (_oParent != null) {
                _sParentName = _oParent.Name + "/";
            }

             aOption[_sParentName + ((ToolStripMenuItem)(sender)).Name] = (bool) ((ToolStripMenuItem)(sender)).Checked;
        }

        public static   List<ToolStripMenuItem> aDataList = new  List<ToolStripMenuItem>();
        public static   Dictionary<string,bool> aOption = new Dictionary<string, bool>();

        public void fLoadData() {
           fAddMenuItem(  afterFileErrorToolStripMenuItem);

        }

        public void fAddMenuItem(ToolStripMenuItem _oItem ) {
      //      Console.WriteLine("AAAAAAAAAAADDDD! : "   +_oItem.Name);
             aDataList.Add( _oItem);
             aOption[_oItem.Name] = (bool)_oItem.Checked;
        }

        public static bool fIsChecked(string _sName) {
            if (aOption.ContainsKey(_sName)) {
               return aOption[_sName];
            }
            return false;
        }


        public static bool bOver = false;
        private void msMenu_MouseEnter(object sender, EventArgs e) {
            bOver = true;
        }
    }
}
