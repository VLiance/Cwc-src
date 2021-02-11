using cwc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices; // DllImport() 
using System.Windows.Forms;
using System.Management;
using System.IO;
using cwc.Utilities;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace cwc {

 static class Program {
    [DllImport("kernel32.dll")]
    static extern bool AttachConsole(int dwProcessId);
    private const int ATTACH_PARENT_PROCESS = -1;


            internal const int STD_OUTPUT_HANDLE = -11;
    internal const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern SafeFileHandle GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool SetConsoleMode(SafeFileHandle hConsoleHandle, uint mode);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool GetConsoleMode(SafeFileHandle handle, out uint mode);


    [STAThread]
    static int Main(string[] args){

		//try {        
            // redirect console output to parent process;
            // must be before any calls to Console.WriteLine()
            AttachConsole(ATTACH_PARENT_PROCESS);
            Debug.fTrace("--- CWC ---");
            

        Sys.fGetParentProcess();
        Debug.fTrace("Systeme mode: " + Sys.sParentName);
                
        /*         
        if (Sys.sParentName == "cmd" || Sys.sParentName ==  "powershell") {
            Data.bConsoleMode = true;
        }
        */
        Data.bConsoleMode = true;
        if (Sys.sParentName == "explorer" || Sys.sParentName == "devenv") {
            Data.bConsoleMode = false;
        }

        Data.fSetDefaultVar(args);
        Data.fCheckUpdate();

        Data.fCreateConfigMng();

        if( Data.bConsoleMode) {
            SysAPI.fStartConsole();
        }
                    			
        if(! Data.bConsoleMode || SysAPI.bIsLoadedFromAnotherCwcInstance) {
        Data.bGUI = false;
            if(! Data.bModeIDE){
		        // Base.bAlive = false; //not work??
            }
        }

        if( !Data.bConsoleMode){
            GuiManager.fCreateGUI();
        }
         Data.fGetMainArg();

        if(!ArgList.bSelfUpdate) { 
            fCheckForRegistringFiles();
         }

        if ( Data.sArg == "") { //No Argument
          
            if(!ArgList.bReceiveMSG) {
                Output.Trace("\f0AVersion " + Data.sVersion + "\fs \n" );
                Msg.fShowIntroMessage();
            }

            SysAPI.fSetWorkingDir(PathHelper.ExeWorkDir);
        }else {
            Build.fBeginBuild();
        }
        
        CppCompiler.CheckAllThreadsHaveFinishedWorking(true);
       // Build.fDisableBuild();
            
        if( Data.oGuiConsole != null){
            if (!Data.oGuiConsole.fCheckForDemos()){
                Program. fCheckForRegistringFiles(true); //Register if first use
            }
        }

        if(!Data.bConsoleMode) {
            if(Data.bModeIDE) {
	            Base.bAlive = true;
               // PipeInput.fLauchPipeInput();
            }
            Build.fStartLoopTestingIdeLinkedClosing();
        }
        Build.fMainLoop();

         //if(Data.bConsoleMode) {SendKeys.SendWait("{ENTER}"); }
        //return 0;
			try {  
		} catch (Exception e) {
			if( Data.oGuiConsole != null) {
				Output.InternalError(e);
				while(true) {
					Thread.Sleep(1);
				}
			}else {
				Output.InternalError(e);
			}
		}
		return 0;
    }

    public static bool fCheckForRegistringFiles(bool _bForce = false) {
        try { 
            return  RegisterFile.fRegisterAllFileType(_bForce); 
        }catch(Exception Ex) {
            Output.TraceWarning("Warning: " + Ex.Message + " : " +Ex.Source  + " : " +Ex.StackTrace);
            return false;
        }
    }

}}
