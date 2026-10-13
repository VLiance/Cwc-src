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
    [DllImport("kernel32.dll")]
    static extern IntPtr GetStdHandle(int nStdHandle);
    [DllImport("kernel32.dll")]
    static extern uint GetFileType(IntPtr hFile);

    static bool fRedirected(int handle) {
        uint type = GetFileType(GetStdHandle(handle));
        return type == 1 || type == 3; // Disk or pipe, including automated callers.
    }

    static bool fConsoleMode(bool redirected, string parent, bool debugging) {
        parent = parent.ToLowerInvariant();
        if(debugging || parent == "code" || parent == "devenv") return false;
        return redirected || parent == "cmd" || parent == "powershell" || parent == "pwsh";
    }

    static int fOptions(ref string command) {
        bool run = false, noRun = false, cli = false, gui = false;
        while(command.StartsWith("--", StringComparison.Ordinal)) {
            int end = command.IndexOfAny(new char[] {' ', '\t'});
            string option = end < 0 ? command : command.Substring(0, end);
            switch(option) {
            case "--cli": cli = true; break;
            case "--gui": gui = true; break;
            case "--run": run = true; break;
            case "--no-run": noRun = true; break;
            case "--help":
                Console.WriteLine("Usage: cwc [--cli | --gui] [--run | --no-run] file.cwMake");
                Console.WriteLine("CLI builds without running by default; --run enables project execution.");
                return 0;
            default:
                Console.Error.WriteLine("Unknown option: " + option);
                return 2;
            }
            command = end < 0 ? "" : command.Substring(end).TrimStart();
        }
        if(cli && gui) {
            Console.Error.WriteLine("--cli and --gui cannot be combined.");
            return 2;
        }
        if(cli || gui) Data.bConsoleMode = cli;
        if(run && noRun) {
            Console.Error.WriteLine("--run and --no-run cannot be combined.");
            return 2;
        }
        Data.bNoRun = noRun || (Data.bConsoleMode && !run);
        return -1;
    }

    [STAThread]
    static int Main(string[] args){
 
                    
        // redirect console output to parent process;
        // must be before any calls to Console.WriteLine()
        bool redirected = fRedirected(-11) || fRedirected(-12);
        if(!redirected) AttachConsole(ATTACH_PARENT_PROCESS);
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        Console.SetError(new StreamWriter(Console.OpenStandardError()) { AutoFlush = true });
        Sys.fGetParentProcess();
        Data.bConsoleMode = fConsoleMode(redirected, Sys.sParentName, Debugger.IsAttached);
        Data.fGetMainArg();
        int optionResult = fOptions(ref Data.sArg);
        if(optionResult >= 0) return optionResult;
        if(Data.bConsoleMode) Data.bGUI = false;

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

        if(!Data.bConsoleMode) fCheckForRegistringFiles();

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
               // PipeInput.fLaunchPipeInput();
            }
            Build.fStartLoopTestingIdeLinkedClosing();
        }
        Build.fMainLoop();

         //if(Data.bConsoleMode) {SendKeys.SendWait("{ENTER}"); }
        return CppCompiler.nError == 0 ? 0 : 1;
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
