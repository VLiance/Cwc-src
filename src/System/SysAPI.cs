using cwc.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cwc {
    class SysAPI {


        public static bool bQuitSavedCfg = false;
        

        internal const int CTRL_C_EVENT = 0;
        internal const int CTRL_BREAK_EVENT = 1;
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
      //  [DllImport("kernel32.dll")]
      //  static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);
        // Delegate type to be used as the Handler Routine for SCCH
        delegate Boolean ConsoleCtrlDelegate(uint CtrlType);



  
 public static void StopProgram(uint pid, uint _event)
{
     // Disable Ctrl-C handling for our program
    SetConsoleCtrlHandler(null, true);

    // It's impossible to be attached to 2 consoles at the same time,
    // so release the current one.
    FreeConsole();

    // This does not require the console window to be visible.
    if (AttachConsole(pid)) {
        GenerateConsoleCtrlEvent(_event, 0);
    }
}

        /*
        public static Process attachedProcess = null;
       public static bool fAttachConsole( Process p){

            if(p == null){
                Output.TraceError("Unable to attach");
                return false;
            }

            if (!AttachConsole((uint)p.Id)) {
                Output.TraceError("AttachConsole fail");
                return false;
            }
             Output.TraceGood("Attached");
            
            attachedProcess = p;
            return true;
       }
       */

       public static bool fSend_CTRL_C( Process p, uint _event = CTRL_C_EVENT){
            StopProgram((uint)p.Id, _event);
            /*
            //Console mnust be attached
           // if(attachedProcess != p) {
                fAttachConsole(p);
           // }
            if(p != null && p == attachedProcess) {
              GenerateConsoleCtrlEvent(CTRL_C_EVENT,0);
            }
            */

            /*
            if (AttachConsole((uint)p.Id)) {
                SetConsoleCtrlHandler(null, true);
                try { 
                    if (!GenerateConsoleCtrlEvent(CTRL_C_EVENT,0))
                        return false;
                //    p.WaitForExit();
                } finally {
          //          FreeConsole();
          //          SetConsoleCtrlHandler(null, false);
                }
                return true;
            }else{ //Already attached?
                GenerateConsoleCtrlEvent(CTRL_C_EVENT,0);
            }
            */
             return false;
        }



        static public void fQuit(bool _bForceQuitConsole = false) {

      //      MessageBox.Show("QUIIIIIIIIIIIIIIII!!!!!!!!!!");
            /*
            if (oLauchProject.oCurLauch != null && oLauchProject.oCurLauch.sExeName == "gdb") {
                oLauchProject.oCurLauch.fSend("set confirm off");
                oLauchProject.oCurLauch.fSend("quit");
            }*/
				//fSaveConsolePosition();

                if(!bQuitSavedCfg){bQuitSavedCfg = true;
				    Data.oConfigMng.SaveConfig(); //save screenn coordinate
                 }

				//	MessageBox.Show("QUIIIIIIIIIIIIIIII!!!!!!!!!! ",  "ass", MessageBoxButtons.OK, MessageBoxIcon.Warning);		
//MessageBox.Show("********QUIIIIIIIIIIIIIIII!!!!!!!!!! ",  "aaa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
	Base.bAlive  = false;
   Data.bModeIDE = false;
     Build.StopBuild();
	Thread.CurrentThread.Join(1);


/*
		  if(oMainForm != null && !oMainForm.IsDisposed) {
				oMainForm.AppQuit();
			}*/

            SysAPI.KillProcessAndChildren(Data.MainProcess.Id);
		//	PipeInput.fTestIdeClosed();
/*
             if(oMainForm != null) {
                oMainForm.AppQuit();
            }*/
            if(_bForceQuitConsole) {
                //   Thread.Sleep(1000); //Last chance for other thread
                   Thread.Sleep(90); //Last chance for other thread
                 Environment.Exit(1);
            }
        }



        public static bool IsWindows7andLower() {
            var version = Environment.OSVersion.Version;
            if (version < new Version(6, 2)){
                   return true;
            }
            return false;
            
        }

        


        public static bool bIsLoadedFromAnotherCwcInstance = false;
      public static bool fIsLoadedFromCwc() {
            /*
          string _sConsoleTitle = Console.Title; //Not work in Win10?

             if (_sConsoleTitle.Length>= 3){
                
                if (_sConsoleTitle == "Cwc") {
                    bIsLoadedFromAnotherCwcInstance = true;
                    return true;
                }
               if (_sConsoleTitle.Length >= 5  && _sConsoleTitle.Substring(0,5) == "Cwc -") {
                    bIsLoadedFromAnotherCwcInstance = true;
                   return true;
                }
            }*/
            return false;
      }



    public static void KillProcessAndChildren(int pid){

        Interlocked.Exchange(ref CppCompiler.safeInstanceCount,0); //Just to be sure we will not be waiting for a killed instance

		KillProcessAndChildrens( pid);
	   try {
			if(Data.oArg != null) {
				Data.oArg.fCleanAllCorruptObj();
			}
		} catch (Exception ex) { }
        
	}

     private static bool ProcessExists(int id) {
      return Process.GetProcesses().Any(x => x.Id == id);
    }

    public static void KillProcessAndChildrens(int pid){

     try{
            if (!ProcessExists(pid)) {
                return;
            }

            Process proc = Process.GetProcessById(pid);

  //  MessageBox.Show("Kil: "+  proc.MainWindowTitle);

            if(proc.Id != Data.nDontKillId) {


        ManagementObjectSearcher searcher = new ManagementObjectSearcher
            ("Select * From Win32_Process Where ParentProcessID=" + pid);
        ManagementObjectCollection moc = searcher.Get();
        foreach (ManagementObject mo in moc)
        {
            KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"]));
        }
                   // MessageBox.Show("Kill: "+  proc.MainWindowTitle);
              //Debug.fTrace("Kil: "+  proc.);
                 Thread.CurrentThread.Join(1);
				if(proc.Id != Data.MainProcess.Id && !proc.HasExited ) {
                        bool _bSkip = false;


                     try { //Notepad
                               string _sFileName = Path.GetFileName(proc.MainModule.FileName);
                                if (_sFileName.IndexOf("notepad") != -1) { //never kill notepad app
                                    _bSkip = true;
                                } 
                     } catch (Exception ex) { }


                      try {
                  //  MessageBox.Show(_sFileName);
                        if(!_bSkip){
                          try{
                             proc.Kill();
                              while( !proc.HasExited) {
                                   Thread.Sleep(1);
                              }              

                        } catch (Exception ex) { }
                        }}catch (Exception ex) { }
				}
//proc.CloseMainWindow();
            }


        }
        catch (Exception ex)
        {
            // Process already exited.
        }
     }




   public static List<Process> ListProcessAndChildrens(int pid){
             List<Process> _aProc = new List<Process>();
     try{
        if (!ProcessExists(pid)) {
         return _aProc;
        }

        Process proc = Process.GetProcessById(pid);

        ManagementObjectSearcher searcher = new ManagementObjectSearcher
        ("Select * From Win32_Process Where ParentProcessID=" + pid);
        ManagementObjectCollection moc = searcher.Get();
        foreach (ManagementObject mo in moc){
            _aProc.AddRange(ListProcessAndChildrens(Convert.ToInt32(mo["ProcessID"])));
        }
        if(proc.Id != Data.MainProcess.Id && !proc.HasExited ) {
             _aProc.Add(proc);
             return _aProc;
        }

    }catch (Exception ex){
 
    }
    return _aProc;
   }



        public static void  fSaveConsolePosition(){
         return;
            if (bIsLoadedFromAnotherCwcInstance || Data.bModeIDE ||  Sys.nConnectedHandle != 0) {
                return;
            }
		    if(Data.bConsoleMode){
				RegistryKey regKey = fGetRegkey();
					if (regKey != null){
						int _nWindowPlacement = NativeMethods.fGetWindowPlacement(GetConsoleWindow());
						if(_nWindowPlacement <= 1){ //Normal
							//Save position & size
							Int32 x; Int32 y; Int32 width; Int32 height; Int32 ClientWidth; Int32 ClientHeight;
							GetWindowPosition(out x, out y, out width, out height, out ClientWidth, out ClientHeight );
						///	ConfigMng.oConfig.vStartPos = new Point(x,y);
					///		ConfigMng.oConfig.vStartSize = new Size(width-x,height-y);
				
							int _nWidth = Console.WindowWidth;
							int _nHeight = Console.WindowHeight;
						
							ushort _nX = (ushort)x;
							ushort _nY = (ushort)y;
							regKey.SetValue("WindowPosition", (_nY << 16) | _nX);
							regKey.SetValue("WindowSize", (_nHeight << 16) | _nWidth);
						
						}
				
						regKey.SetValue("WindowPlacement",_nWindowPlacement);
						
						ushort _nBuffWidth = (ushort)Console.BufferWidth;
						ushort _nBuffHeight = (ushort)Console.BufferHeight;
						regKey.SetValue("ScreenBufferSize",(_nBuffHeight << 16) | _nBuffWidth);
					}

            }
		}

		public static void  fSetConsolePosition(){
		
			if(ConfigMng.bNewConfig){

		//	   fSaveConsolePosition();
		
			}else{
				
	//			SetWindowPosition(ConfigMng.oConfig.vStartPos.X,ConfigMng.oConfig.vStartPos.Y,ConfigMng.oConfig.vStartSize.Width,ConfigMng.oConfig.vStartSize.Height);
				
			}
			//fShowConsole();
                   // Int32 x; Int32 y; Int32 width; Int32 height;
                   // GetWindowPosition(out x, out y, out width, out height);
                  //  SetWindowPosition(x, y, width, height);
					/*
                    // Min Size of windows
                    int nCurrWidth = Console.WindowWidth;
                    int nCurrHeight = Console.WindowHeight;
                    int nToWidth = (Console.LargestWindowWidth - 3)*2/3;
                    int nToHeight = (Console.LargestWindowHeight - 3)/2;
                    if(nToWidth < nCurrWidth) {
                        nToWidth = nCurrWidth;
                    }
                    if(nToHeight < nCurrHeight) {
                        nToHeight = nCurrHeight;
                    }*/

				
			//		 Console.SetWindowSize( nToWidth , nToHeight);
				//	SetWindowPosition(0,0,1000,500);

		}

        internal static void fStartConsole() {
           
              try{ 

                /*
					fSetConsolePosition();
                  
                    ////////// Set console /////////
                    //uint STD_OUTPUT_HANDLE = 0xfffffff5;
                     hConsoleInput = GetStdHandle((uint)STD_INPUT_HANDLE);
                    int mode;
                    GetConsoleMode(hConsole, out mode);
                  //  mode |= ENABLE_QUICK_EDIT_MODE;
					
					mode |= NativeMethods.ENABLE_MOUSE_INPUT;
					mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
					mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

                    SetConsoleMode(hConsoleInput, mode);*/
                    /////////////////////////////////////////
                   hConsole = GetStdHandle((uint)STD_OUTPUT_HANDLE);

                } catch (Exception ex){    }
        }

        public static RegistryKey  fGetRegkey(){
			try{
				string _sCurrenKey = "";
				if(Sys.oParentProcess.ProcessName == "cmd"){
					_sCurrenKey = "%SystemRoot%_system32_cmd.exe";
				}else{
					_sCurrenKey = Application.ExecutablePath.Replace('\\','_');
				}
				string _sKey = @"Console\" + _sCurrenKey;
				RegistryKey _oKey =  Registry.CurrentUser.OpenSubKey(_sKey,true);
				if(_oKey == null){
					Registry.CurrentUser.CreateSubKey(_sKey);
					 _oKey =  Registry.CurrentUser.OpenSubKey(_sKey,true);
				}
				return _oKey;

			}catch(Exception e){};
			
			
			return null;
		}

		
		public static void  fRestoreConsole(){
			RegistryKey regKey = fGetRegkey();
			if (regKey != null){
				try{
					object _oKey = regKey.GetValue("WindowPlacement");
					if(_oKey != null){
						int _nWindowPlacement = (int)_oKey;
						if(_nWindowPlacement == 3){//Maximise
							fMaximise();
						}
					}
				}catch(Exception e){};
			}
		}





        


        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput,int wAttributes);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(uint nStdHandle);

      [DllImport("kernel32.dll")]
       static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

       [DllImport("kernel32.dll")]
       static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

         private static  int STD_INPUT_HANDLE = -10;
         private static  uint ENABLE_QUICK_EDIT_MODE = 0x40 | 0x80;
   

         [DllImport("kernel32")]
        private static extern IntPtr GetConsoleWindow();
 
  
        [DllImport("user32")]
        private static extern Boolean GetClientRect(IntPtr hWnd, ref Rectangle rect);

   

        [DllImport("user32")]
        private static extern Boolean SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, Int32 x, Int32 y, Int32 cx, Int32 cy, Int32 flags);
 
        private static int STD_OUTPUT_HANDLE = -11;
        private static int SWP_NOZORDER = 0x4;
        private static int SWP_NOACTIVATE = 0x10;



        public static int nMajorVersion = 0;
        public static int nMinorVersion = 1;
        public static IntPtr hConsoleInput;
    //    public static NativeMethods.ConsoleHandle hConsoleInput;

        public static IntPtr hConsole;







public  delegate void DMainThread();
   public   static DMainThread MainThread;   // Keeps it from getting garbage collected
   

         // Pinvoke
       public delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
       public static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        public    static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
   


        
 [DllImport("user32.dll")]
public static extern bool GetWindowRect(IntPtr hwnd, ref Rectangle rectangle);

        private static void GetWindowPosition(out Int32 x, out Int32 y, out Int32 width, out Int32 height,  out Int32 ClientWidth, out Int32 ClientHeight)
        {
            Rectangle rect = new Rectangle();
            GetWindowRect(GetConsoleWindow(), ref rect);

            x = rect.Left ;
            y = rect.Top;

            width = rect.Right - rect.Left;
            height = rect.Bottom - rect.Top;

			Rectangle rect2 = new Rectangle();
			GetClientRect(GetConsoleWindow(), ref rect2);
			ClientWidth =  rect2.Width;
			ClientHeight =  rect2.Height;


			 //var consoleWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
          
        }
 

        private static void SetWindowPosition(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            SetWindowPos(GetConsoleWindow(), IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
        }

		static int nLastWidth = 0;
		static int nLastHeight = 0;
        
		static int nLastX = 0;
		static int nLastY = 0;


		public static void fSetMainFormPosition() {

		//	if(Data.oMainForm != null ) {
			if(false ) {
				
				Int32 x; Int32 y; Int32 width; Int32 height; Int32 ClientWidth; Int32 ClientHeight;
				GetWindowPosition(out x, out y, out width, out height, out ClientWidth, out ClientHeight );

				/*
              	if(oMainForm != null) {
			    	oMainForm.fSetPosition(x,y, width,height, ClientWidth, ClientHeight);
                }
                */

				
				bool _bMove = false;
				if(width != nLastWidth ){
					nLastWidth = width;
					_bMove = true;
				}
				if(height != nLastHeight ){
					nLastHeight = height;
					_bMove = true;
				}
  

                if(x != nLastX ){
					nLastX = x;
					_bMove = true;
				}
				if(y != nLastY ){
					nLastY = y;
					_bMove = true;
				}


				if(_bMove){
                    /*
					if(Data.oSelectForm != null){
						Data.oSelectForm.fHide();
					}*/

                    /*
                    if (Data.oGuiForm != null) {
                        Data.oGuiForm.fSetPosition(x,y, width -x,height-y, ClientWidth, ClientHeight);
                    }
                    */
					fSaveConsolePosition();//Todo only when mouse is released?
				}
                /*
                 if (Data.oSelectForm != null) {
                    Data.oSelectForm.fSetPosition(x,y, width -x,height-y, ClientWidth, ClientHeight);
                }*/


			//	Rectangle	oRect = new Rectangle();
			//	GetClientRect(hConsoleInput, ref oRect );
		//	Debug.fTrace("width: " +	oRect.X);
					
			}

		}




        




        


	


    

     static bool ConsoleEventCallback(int eventType) {
		 //  MessageBox.Show("The form is now closing.", "Close Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

           //   MessageBox.Show("The form is now closing.",   "Close Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			Debug.fTrace("Console event: " + eventType);
        if (eventType == 2) {
               //  MessageBox.Show("The form is now closing.",   "Close Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
           // Debug.fTrace("Console window closing, death imminent");
             fQuit(true);
                      return true;
            
        }
        return false;
    }



        
[DllImport("user32.dll")]
static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
const int SW_HIDE = 0;
const int SW_SHOW = 5;
 const int SW_MAXIMIZE = 3;
 const int SW_MINIMIZE = 6;

public static void fMinimize() {
	var handle = GetConsoleWindow();
	ShowWindow(handle, SW_MINIMIZE );
}
public static void fMaximise() {
	var handle = GetConsoleWindow();
	ShowWindow(handle, SW_MAXIMIZE );
}

		public static void fShowConsole() {
	var handle = GetConsoleWindow();
	ShowWindow(handle, SW_SHOW);
}


public static void fHideConsole() {
	var handle = GetConsoleWindow();
	ShowWindow(handle, SW_HIDE );
}



      public static void fSetWorkingDir(string _sDir) {
		//fDebug("fSetWorkingDir  " + _sDir);
		PathHelper.ExeWorkDir = _sDir;
         //   fSetConsoleTitle(_sDir);

        if( Data.oGuiConsole != null) {
             Data.oGuiConsole.fSetWorkingDir(_sDir);
        }

	//	Console.Title =	_sDir;
    /*
		if(Data.oMainForm != null){
			Data.oMainForm.fSetWorkingDir(_sDir);
		}*/
	
	}

     public static void fSetConsoleTitle(string _sTitle) {
        /*
        if (_sTitle == "") {
             Console.Title = "Cwc";
        }else{
             Console.Title = "Cwc - " +	_sTitle;
        }*/
     }





    }
}
