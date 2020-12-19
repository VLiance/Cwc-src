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

namespace cwc {

 static class Program {
    [DllImport("kernel32.dll")]
    static extern bool AttachConsole(int dwProcessId);
    private const int ATTACH_PARENT_PROCESS = -1;

    [STAThread]
    static int Main(string[] args){
 
                    
        // redirect console output to parent process;
        // must be before any calls to Console.WriteLine()
        AttachConsole(ATTACH_PARENT_PROCESS);

        Sys.fGetParentProcess();
        Console.WriteLine("Systeme mode: " + Sys.sParentName);
                         
        if (Sys.sParentName == "cmd") {
            Data.bConsoleMode = true;
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

        Data.fGetMainArg();
        fCheckForRegistringFiles();

        if( !Data.bConsoleMode){
            GuiManager.fCreateGUI();
        }


        if ( Data.sArg == "") { //No Argument
          
            Output.Trace("\f0AVersion " + Data.sVersion + "\fs \n" );
            Msg.fShowIntroMessage();
            SysAPI.fSetWorkingDir(PathHelper.ExeWorkDir);
        }else {
            Console.WriteLine("fBeginBuild ");
            Build.fBeginBuild();
        }
                 
        CppCompiler.CheckAllThreadsHaveFinishedWorking(true);
        Build.fDisableBuild();
            
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
        return 0;
    }

    public static bool fCheckForRegistringFiles(bool _bForce = false) {
        try { 
            return  RegisterFile.fRegisterAllFileType(_bForce); 
        }catch(Exception Ex) {
            Console.WriteLine("Error: " + Ex.Message + " : " +Ex.Source  + " : " +Ex.StackTrace);
            return false;
        }
    }

}}
