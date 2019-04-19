using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cwc
{
	public partial class SelectForm : Form
	{
		public SelectForm()
		{
			InitializeComponent();
		}
		bool _bLoaded = false;
		private void SelectForm_Load(object sender, EventArgs e)
		{        
				_bLoaded = true;

		}

		private void SelectForm_Click(object sender, EventArgs e)
		{
            string _sFile = PipeInput.sFile;
            if (PipeInput.bRealive) {
                _sFile = PathHelper.ExeWorkDir  + _sFile;
            }
			
			 // MainForm.RunInEditor(PipeInput.sFile);
		//	  MainForm.RunInEditor(PipeInput.sFile ,  " -lcpp" + " -n" + PipeInput.nLine + " -c" +  PipeInput.nColomn);
			  FileUtils.RunInEditor(_sFile,  " -n" + PipeInput.nLine + " -c" +  PipeInput.nColomn);
		//	  FileUtils.RunInEditor(_sFile,  " -lcpp" + " -n" + PipeInput.nLine + " -c" +  PipeInput.nColomn);
          
           // GuiManager.fActivate();
            
          fHide();
		}

		internal void fHide(){
			if(_bLoaded){
			this.BeginInvoke((MethodInvoker)delegate  {	
                if(Visible){
					Visible= false;
					Opacity = 0;
                }
			 });
			}
		}

		internal void fShow(){

				if(_bLoaded){
				  this.BeginInvoke((MethodInvoker)delegate  {	
     
						Visible= true;
						Opacity = 0.5;
				 });
			 }
		}

	
		 private void SelectForm_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e){

            int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
			PipeInput.fMouseWheel(numberOfTextLinesToMove);
        }

        internal void fSetPosition(int x, int y, int width, int height, int clientWidth, int clientHeight) {

                 this.BeginInvoke((MethodInvoker)delegate  {              
                     
                    int _nWinBorder = (width  - (clientWidth + 	vScrollBar.Width ))/2;
                    int _nTitleBarHeight = ( height  - (clientHeight + 	hScrollBar.Height +  _nWinBorder));
				



                   int _nSelectTopX = x+_nWinBorder;
			        int _nSelectTopY = y + _nTitleBarHeight;
			        //int _nMaxWidth = Cons_ClientWidth -  Size.Width -  PipeInput.nStartSelX;
			        int _nMaxWidth = clientWidth -  PipeInput.nStartSelX;
			
			        int nWidthTo = PipeInput.nEndSelX;
			        if(nWidthTo > _nMaxWidth){
				        nWidthTo = _nMaxWidth;
			        }
			        int nWidthStart = PipeInput.nStartSelX;
			        if(nWidthStart < 0){
				        nWidthTo += nWidthStart;
				        nWidthStart = 0;
			        }
                

                //    if(PipeInput.bFoundSel ){
                        // Console.Write("fond \r" +  PipeInput.nStartSelY + "   " + _nSelectTopX  + "            ");
				        Location = new Point(_nSelectTopX + nWidthStart, _nSelectTopY + PipeInput.nStartSelY );
				        Size = new Size(nWidthTo, PipeInput.fontSize.Height );
                        //Console.Write("Size \r" + Cons_ClientWidth+ "   " + Size.Width  + "  " + PipeInput.nStartSelX);
				        fShow();
			  //  }

                 });

        }
    }
}
