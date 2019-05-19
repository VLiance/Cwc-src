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
using System.Windows.Forms;
using System.IO;
using cwc.Utilities;
using Microsoft.Win32;

namespace cwc
{



//if(Sys.sParentName == "devenv"){
//		sArg = "@_Cwc_Demos/Cwc_Demos-0.0.6/Base_Example/01_HelloFolder/Make.bat";
//	}


//if(Sys.oParentProcess.ProcessName.Length> 1 && Sys.oParentProcess.ProcessName[0] == 'F') {//TODO by flashdev
        //bModeIDE = true;
    //  bGUI =false;
// }

 static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [STAThread]
        static int Main(string[] args)
        {
 
                    
            // redirect console output to parent process;
            // must be before any calls to Console.WriteLine()
            AttachConsole(ATTACH_PARENT_PROCESS);
            /*
            // to demonstrate where the console output is going
            int argCount = args == null ? 0 : args.Length;
            Console.WriteLine("nYou specified {0} arguments:", argCount);
            for (int i = 0; i < argCount; i++)
            {
                Console.WriteLine("  {0}", args[i]);
            }

            //////////////*/

            Sys.fGetParentProcess();
            Console.WriteLine("Systeme mode: " + Sys.sParentName);
                         
            if (Sys.sParentName == "cmd") {
                  Data.bConsoleMode = true;
            }

       /*
            if( !Data.bConsoleMode){

                // launch the WinForms application like normal
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new GuiConsole());
            }
            */

         //   SysAPI.fIsLoadedFromCwc(); //Not work
       //     SysAPI.fSetConsoleTitle("");
            

            Data.fSetDefaultVar(args);
            Data.fCheckUpdate();

                try{   /*
			    int _nBufWidth =  Console.BufferWidth;
			    int _nBufHeight =  Console.BufferHeight;
			    if(_nBufWidth < 250){ //Minimal size
				    _nBufWidth = 250;
			    }
			    if(_nBufHeight < 100){ //Minimal size
				    _nBufHeight = 100;
			    }
			    Console.SetBufferSize(_nBufWidth, _nBufHeight);*/
            } catch (Exception ex){
		    //	Debug.fTrace("!!!!!!!!!!!!!!!!!!!!! " + ex.Message);
             //   Data.bConsoleMode = false;
              //  CompilerData.sHiddenArg_sColoConsole = ""; //Disable colored console
            }
         
            //Console.WriteLine("fCreateConfigMng ");
            Data.fCreateConfigMng();

            if( Data.bConsoleMode) {
                SysAPI.fStartConsole();
            }
                    			
	        if(! Data.bConsoleMode ||   SysAPI.bIsLoadedFromAnotherCwcInstance) {
		        Data.bGUI = false;
                    if(! Data.bModeIDE){
		               // Base.bAlive = false; //not work??
                    }
	        }

                  // Console.WriteLine("7");  
	      //  PipeInput.NewPipeInput();
            Data.fGetMainArg();

            /*
            if( Data.bGUI) {
                 fCheckForRegistringFiles(true);
            }else {
                 fCheckForRegistringFiles();
            }
            */
            fCheckForRegistringFiles();


             if( !Data.bConsoleMode){
                GuiManager.fCreateGUI();
            }


            if ( Data.sArg == "") { //No Argument
                  //        Console.WriteLine(" aa.sArg  :" +  Data.sArg );
                //  Output.Trace("\f0ACwC Version " + nMajorVersion.ToString() + "."  + nMinorVersion.ToString() + "\fs" );

                Output.Trace("\f0AVersion " + Data.sVersion + "\fs \n" );
             
               
                   Msg.fShowIntroMessage();

			    SysAPI.fSetWorkingDir(PathHelper.ExeWorkDir);


                //RegisterFile.fRegisterAllFileType(true);

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
                    PipeInput.fLauchPipeInput();
                }

     
                Build.fStartLoopTestingIdeLinkedClosing();
            }


            Build.fMainLoop();






		    SysAPI.fQuit(); 
            
          //  if(Data.bConsoleMode) {SendKeys.SendWait("{ENTER}"); }
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













}

}
