using cwc.Update;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static cwc.Win32;

namespace cwc
{





	public partial class Empty : Form
	{



        
		public static  Empty oMsgForm = null;

		public static  SelectForm oSelectForm = null;



		public static  bool bLoaded = false;
       public  static MainForm oMainForm = null;
       public  static IntPtr  nMainHwnd;
       public  static bool  bHaveMsgForm =false;


		public Empty()
		{
		
			InitializeComponent();
			this.Visible = false;
		}


        
		private void Empty_Load(object sender, EventArgs e){
		     Data.oMsgForm = this;
			fCreateGUI();
            bLoaded = true;
       //     fLoadModules();
		}



       public  static bool bFromParent = false;

        public  void fSetToParentApp(Form _oForm) {
            if (bFromParent) {
                if (Sys.nConnectedHandle != 0) {
               //       nMainHwnd = NativeMethods.SetParent( _oForm.Handle, (IntPtr)Data.nConnectedHandle);
                  //Console.WriteLine("fSendMsg " + _oForm.Handle.ToString() + " to " +   (IntPtr)Data.nConnectedHandle);

                   
 
                 //     NativeMethods.SetWindowText((IntPtr)Data.nConnectedHandle, Handle.ToString()); //Sendback our Handle  work!
                    fSendMsg("C:wMsgHandle:" + Handle.ToString() + ";wMenuHandle:" +  _oForm.Handle);



                //   MessageBox.Show("Set to " + Data.nMenuHandle);

                } else {
                     nMainHwnd = NativeMethods.SetParent( Handle, Sys.oParentProcess.MainWindowHandle);
                    
                }
                 // nMainHwnd = NativeMethods.SetParent( _oForm.Handle, Data.oParentProcess.MainWindowHandle);
                
            }
        }





        public void fCreateGUI() {

            Debug.fTrace("fCreateGUI");
            this.BeginInvoke((MethodInvoker)delegate  {
               

				this.Hide();
            
       


                /*

                string _sStartBy = Sys.oParentProcess.ProcessName.ToLower().Trim();
				//IntPtr _hwnd;
				if(_sStartBy == "explorer" || _sStartBy == "devenv"){
                    nMainHwnd = NativeMethods.SetParent( Handle, Process.GetCurrentProcess().MainWindowHandle);
				}else{ //"Cmd" "Flashdev"
                    bFromParent = true;
              
				}

				if(nMainHwnd == null) {
					Debug.fTrace("Unable to set child");
				}
				SysAPI.fRestoreConsole();
                
				oSelectForm = new SelectForm();
				Data.oSelectForm = oSelectForm; 
                  oSelectForm.Show(this);
                  */
                /*
				 oMainForm = new MainForm(Data.CmdArgs,oSelectForm,this);
				Application.ApplicationExit +=new EventHandler(oMainForm.OnApplicationExit);
				AppDomain.CurrentDomain.DomainUnload +=new EventHandler(oMainForm.OnApplicationExit);
                */
                



                //GuiConsole _oGUI = new GuiConsole();
                //_oGUI.Show(this);


           //     Data.oGuiForm = _oGUI;

            //    fSetToParentApp(_oGUI);

                /*
                while(Data.oGuiForm == null) { //Null protection
					Thread.Sleep(1);
				}*/

		 });

        }




        /*
        private void OnPaint(object sender, PaintEventArgs e)
{
            
    this.Invalidate();
    this.Update();
    //this.DrawFrame(e.Graphics);
}
*/
        /*
        private void RedrawFrame(){
    var r = new Rectangle(
        0, 0, this.Width, this.Height);

    this.Invalidate(r);
    this.Update();
}
*/





		private void Empty_FormClosing(object sender, FormClosingEventArgs e)
		{
//MessageBox.Show("********QUIIIIIIIIIIIIIIII!!!!!!!!!! ",  "aaa", MessageBoxButtons.OK, MessageBoxIcon.Warning);		
		//	Data.fQuit();
		}

		private void Empty_FormClosed(object sender, FormClosedEventArgs e)
		{

			//Data.fQuit(true);		
		}



private const int WM_CLOSE = 0x0010;
private const int WM_QUIT = 0x0012 ;
private const int WM_QUERYENDSESSION = 0X0011 ;
private const int WM_DESTROY = 0x0002  ;


        private void Empty_Move(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {

        }

        private void Empty_Resize(object sender, EventArgs e) {
                 
        }

        private void Empty_LocationChanged(object sender, EventArgs e) {

        }

        private void Empty_RegionChanged(object sender, EventArgs e) {

        }

        private void Empty_ResizeBegin(object sender, EventArgs e) {

        }



        //			 PluginMain.SafeInvoke(delegate {  });
        public  static void SafeInvoke( Action updater, bool forceSynchronous = true){
			if ( Data.oMsgForm == null){
				throw new ArgumentNullException("uiElement");
        
			}
			if ( Data.oMsgForm.InvokeRequired){
				if (forceSynchronous){
					 Data.oMsgForm.Invoke((Action)delegate { SafeInvoke( updater, forceSynchronous); });
				}else{
					 Data.oMsgForm.BeginInvoke((Action)delegate { SafeInvoke( updater, forceSynchronous); });
				}
			}else{    
				if ( Data.oMsgForm.IsDisposed){
					throw new ObjectDisposedException("Control is already disposed.");
				}
				updater();
			}
		}






       public struct R_COPYDATASTRUCT {
		   public Int32 dwData;
            public Int32 cbData;
            public IntPtr lpData;
        }



      	public Queue myQ = new Queue();

          protected override void WndProc(ref Message m) {

             // MessageBox.Show("!!!!!!!!!! ",  "aaa", MessageBoxButtons.OK, MessageBoxIcon.Warning);	
                 // if (m.Msg == WM_CLOSE) {
                 //  MessageBox.Show("********QUIIIIIIIIIIIIIIII!!!!!!!!!! ",  "aaa", MessageBoxButtons.OK, MessageBoxIcon.Warning);		
                 // }

                //if(bLoaded) {
                //if(m.Msg != 28 && m.Msg != 10 && m.Msg != 31  ) {
	                //  MessageBox.Show("********QUIIIIIIIIIIIIIIII!!!!!!!!!! ", m.Msg.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);	
                /*
                 Debug.fTrace("********QUIIIIIIIII  " + m.Msg.ToString() + " ");	

	                switch(m.Msg ) {
				                case 130:
				                case WM_DESTROY:
		                case WM_QUIT:
		                case WM_QUERYENDSESSION:
			                  MessageBox.Show("!!!!!!!!!! ",  "aaa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		                break;

                //	}
                //}
                */

  

            switch (m.Msg)  {
                // program receives WM_COPYDATA Message from target app
                case WM_COPYDATA:
                    if (m.Msg == WM_COPYDATA) {
						 Object thisLock = new Object();
						 lock (thisLock) {

					//	Debug.fTrace("WM_COPYDATA ");
						// get the data
						R_COPYDATASTRUCT cds = new R_COPYDATASTRUCT();
						cds = (R_COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam,
						typeof(R_COPYDATASTRUCT));

								if (cds.cbData > 2) //2 to remove carriage return
								{
									byte[] data = new byte[cds.cbData  ];
									Marshal.Copy(cds.lpData, data, 0, cds.cbData );
								//	data[data.Length - 1 ] = 0;
								//	try{this.BeginInvoke((MethodInvoker)delegate { try {

								Encoding unicodeStr = Encoding.UTF8; //Not UTF8 ?? Seem work
								string _sReceivedText = new string(unicodeStr.GetChars(data ));
								_sReceivedText = _sReceivedText.Substring(0,_sReceivedText.Length-2);
								//	TraceManager.Add("****************" + _sReceivedText);
							
                        /*
								 myQ.Enqueue(_sReceivedText );
								 Queue mySyncdQ = Queue.Synchronized( myQ );
								 Empty.SafeInvoke(delegate {  //Not sure
								string _sReceived = (string)mySyncdQ.Dequeue();
                                Empty.fRecewiveMsg(_sReceived);
					 */
                                fRecewiveMsg(_sReceivedText);

											//	Output.SelectionColor = Color.Red;
												//  Output.Text += _sReceivedText;
									
											//		Output.AppendText("\r\n");

									//}); 
							}
						m.Result = (IntPtr)1;
						}
                    }
                    break;

                default:
                    break;
            }
            base.WndProc(ref m);
        }



      public  void fRecewiveMsg(string _sMsg) {

                Debug.fTrace(_sMsg);

            if(_sMsg.Length <= 2){return;} //Invalid
           // try {
            if (_sMsg[1] == ':'){

                switch (_sMsg[0])
                {

                    case 'P'://Print Good
                        Output.Trace(_sMsg.Substring(2));
                      break;


                    case 'G':
                        Output.TraceGood(_sMsg);
                        break;

                    case 'E':
                           
                        break;


                   case 'C'://Print Good
                        fPerformCommand(_sMsg.Substring(2));
                     break;

                    case 'A':

                        break;
                    default:
                        break;

                }
            }

        }





          // public static int n

         public static void fSendMsg(string msg, int _nToHandle = 0) {
			if(_nToHandle == 0){
                _nToHandle = (int)Sys.nConnectedHandle;
                 if(_nToHandle == 0){
                        return;
                  }
            }
           

             int result = 0;
            string _sMsg = (string)msg;
			//  Debug.fTrace("Static thread procedure. Data='{0}'", data);
			byte[] sarr = System.Text.Encoding.Default.GetBytes(_sMsg + "\0");
			int len = sarr.Length;
			COPYDATASTRUCT cds;
			cds.dwData = (IntPtr)0;
			cds.lpData = _sMsg;
			cds.cbData = len + 1;

               Debug.fTrace("Send " + (int)_nToHandle + "  Msg " + _sMsg);
			result = Win32.SendMessage((int)_nToHandle, WM_COPYDATA, 0, ref cds);

        }


        



           private void fPerformCommand(string _sCommandes) {
              string[]  _aCommandes =   _sCommandes.Split(';');
            foreach(string _sCommande in _aCommandes) {
                string[]  _aPart =   _sCommande.Split(':');
                if(_aPart.Length >= 1 ) {
                     switch(_aPart[0]){
                        case "StartBuild":
                           Data.sCmd ="StartBuild";
                        break;
                        case "StopBuild":
                            Data.oLauchProject.fCancel(); //Thread?
                        break;
                        

  
                    }

                }

            }

           
        }






        	
		public string[] aModuleToLoad;
		public static int  nLoadModuleIndex =  0 ;
		public static bool  fLoadModules(bool _bAutoStart = true) {

            bool _bFound = true;
                if(Data.oGuiConsole != null) {
                  Data.bIWantGoToEnd = true;
                   GuiConsole.sFormCmd = "GoEnd";
                     //fctbConsole.GoEnd();
                }
                
                nLoadModuleIndex = 0;
			    foreach(string _sModule in Data.aRequiredModule) {
                    ModuleData _oModule = ModuleData.fFindModule(_sModule);
			        _oModule.fGetLocalVersions();

                    //  Http.fGetHttp(  _oModule.sUrl_Project , fGetProjectData);//Get readme
			        _oModule.fReadHttpModuleTags();
                }

                //Wait to finish
                while(ModuleData.nRequestTag > 0) {
                    Thread.CurrentThread.Join(1);
                }

                List<ModuleLink> _aLink = new List<ModuleLink>();

         
                foreach(string _sModule in Data.aRequiredModule) {
                    ModuleData _oModule = ModuleData.fFindModule(_sModule);
                    if( _oModule.aLinkList.Count > 0) {
                        foreach(string _sKeyLink  in _oModule.aLinkList) {
                          // Output.TraceWarning( "Recommended version:");
                            Output.TraceAction( "Recommended version:" + _oModule.sName + " : " + _sKeyLink );
                            _aLink.Add(_oModule.aLink[_sKeyLink]);
                            break;
                        }
                    }else {
                         Output.TraceError( "Not found:" + _sModule  );
                        _bFound = false;
                        
                    }

                }


                if(_aLink.Count > 0) {
                  //   Output.TraceWarning( "Download? (yes / no)");
                    Output.TraceWarning( "Starting Download ... (press 'n' to cancel)");
                    foreach(ModuleLink _oLink in _aLink) {
                          _oLink.fDownload();
                          while(_oLink.bDl_InProgress) {Thread.CurrentThread.Join(1); }
                          _oLink.fExtract();
                          while(_oLink.oModule.bExtact_InProgress) {Thread.CurrentThread.Join(1); }
                    }
                       Output.Trace("");
                    Output.TraceGood( "---------------- All Required Module Completed ------------------");
                    foreach(ModuleLink _oLink in _aLink) {
                           Output.TraceAction(_oLink.oModule.sCurrFolder);
                    }

            
                    Output.TraceGood( "-----------------------------------------------------------------");
                    if(_bAutoStart) {
                        Data.sCmd = "StartBuild";
                    }

                }

                return _bFound;

               /*
                foreach(ModuleLink _oLink in _aLink) {
                      _oLink.fExtract();
                }
               */
                
			    //  Data.oMsgForm.fLoadNextModule();

		}



        public void fLoadNextModule() {
    
			 this.BeginInvoke((MethodInvoker)delegate  {
			 if(bLoaded){
        
			        if(nLoadModuleIndex < aModuleToLoad.Length) {

				        string _sModule = aModuleToLoad[nLoadModuleIndex];
			            Debug.fTrace("fLoadModules: " + _sModule);


				        Data.oMsgBox = new Form { TopMost = true,TopLevel = true };
					        var res =  MessageBox.Show(Data.oMsgBox , _sModule + " module is required. \nDownload?", "Download " + _sModule + "?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);	
				        //MessageBox.Show(Data.oMsgBox , "The "  +  + " module is required to build C++", "Download LibRT?", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);	
				
				        if (res == DialogResult.Yes){
					        UpdateForm.Show(this, false,_sModule, true );
				        }
				        if (res == DialogResult.Cancel){
					        nLoadModuleIndex =  aModuleToLoad.Length; //Cancel All
				        }
			            
			        }else {	
				        //Reset?
				        nLoadModuleIndex = 0;
				        aModuleToLoad = new string[0];
				        Data.aRequiredModule = new List<string>();
			        }
			        nLoadModuleIndex++;
                 }
           });
		}









    }
}
