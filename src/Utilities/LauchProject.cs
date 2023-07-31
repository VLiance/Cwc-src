using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cwc{

	public class LaunchProject{

	//	public  MainForm oMainForm = null;
			
		// public Launch oCurLaunch = null;
		 public LaunchTool oCurLaunch = null;
		 public Boolean bReceiveOutput = false;


		 public static string sOutput = "";

		        
		public  void fBuildFinish() {
           Debug.fTrace("fBuildFinish: " + Data.fGetGlobalVar("wBuildAnd"));
            //fLaunchDefaultRun();
        }

        public  void fLaunchDefaultRun(string _sPath = "", string _sSubArg = "") { //"" = Last link file


            if(!Data.bModeIDE ) {
			   // switch(Data.sBuildAnd) {
				switch(Data.fGetGlobalVar("wBuildAnd")) {

					case "Run":

							fLaunchExe(_sPath, false, _sSubArg);
					break;
					 case "Sanitize":
                    
							fLaunchExe(_sPath, true, _sSubArg);
					break;
				}
			}
        }



		 public void fLaunchExe(string _sPath = "",  bool _bSanitize = false, string _sSubArg = "") {
            if(_sPath == "") {//Launch default last linked output binary
                 _sPath = PathHelper.ExeWorkDir +   sOutput;

            }else {

                //TODO better fonction to detect absolute / relative --> Cleaup
               if(_sPath.Length > 2 && _sPath[1] == ':'){ //Absolute path
					///
				}else{
					_sPath = PathHelper.ExeWorkDir + _sPath; //Relative path
				}
               /////////////
            }

                              //        Output.TraceAction("Run:" + _sPath);
            if(fLaunch(_sPath, _bSanitize, _sSubArg)) {
                
            }
        }


		 public bool fLaunch(string _sPath, bool _bSanitize = false, string _sSubArg = "") {

            if(_sPath.Length > 1 && _sPath[_sPath.Length-1] =='/') { //Not a file
                Output.TraceError("Path is not a File: " + _sPath);
                return false;
            }
           //   Output.TraceError("Try: " + _sPath);

            bool _bDebug = true;//Temp
            bool _bLaunchDebug = false;
	        if(_bDebug){
                _bLaunchDebug = true;
            }



		//	Debug.fTrace("Launch: " + _sPath);
			
			

   // if (oCurLaunch == null) { //We can reLaunch !? => Run button
                if( File.Exists(  _sPath ) ) {
							
					if(Data.oGuiConsole != null) {
						Data.oGuiConsole.fLaunchPrj();
					}
					
            
                    oCurLaunch = new LaunchTool();

		//oCurLaunch.bWaitEndForOutput = true;
		//	oCurLaunch.bRunInThread = false;

        oCurLaunch.bRedirectOutput = false;


        string _sArg = "";
        string _sExePath = _sPath;
		if(Data.fGetGlobalVar("_sPlatform") == "Web_Emsc") {
		        _bDebug = false; // No GDB
                oCurLaunch.bRedirectOutput = true;
                oCurLaunch.bHidden = true;
                oCurLaunch.UseShellExecute = true;


			//	bWeb = true;
			//	bSanitize = false;
				string	_sBrowser =  Data.fGetViewIn();
				 _sArg = "\"" + Data.fGetGlobalVar("vWebRT_Emsc") + "emrun\" ";
				//_sArg +=  "--serve_after_close ";
				//_sArg +=  "--serve_after_exit ";
			
				_sArg +=  "--browser \"" +  _sBrowser + "\" ";
               	//_sArg +=  "--kill_exit ";
               	//sArg +=  "--kill_start ";
                
				_sArg += "\"" + _sExePath + "\" ";
		        
		
				_sExePath = Data.fGetGlobalVar("vWebRT_Python") + "python.exe";

				//_sWorkPath = _sExePath;
				
			//	public
		}else { 
          
            //Normal
            //	oCurLaunch.bDontKill = true;
            //oCurLaunch.UseShellExecute = false;
        }
        
        _sArg += _sSubArg;
        _sArg = _sArg.Trim();
           //Output.TraceWarning("_sSubArg " + _sSubArg);

        oCurLaunch.dError = new LaunchTool.dIError(fAppError); //Too much error in Emsc?
        oCurLaunch.dOut = new LaunchTool.dIOut(fAppOut);
 
	    oCurLaunch.dExit = new LaunchTool.dIExit(fExit);

           string _sPrintArg = "";
          if(_sArg != "") {
                _sPrintArg = " [" + _sExePath + " " + _sArg + "]";
          }



         if(_bLaunchDebug){
		        Output.TraceAction("Debug: " + _sPath + _sPrintArg);
            } else {
                Output.TraceAction("Run: " + _sPath  +  _sPrintArg);
            }

        if(_bDebug){

            //Better way?
            string _sCompiler = Data.fGetGlobalVar("_wToolchain");
			string _sPlatform = Data.fGetGlobalVar("_sConfig_Type");
		    CompilerData	_oCompiler = Finder.fUseCompiler(_sCompiler, _sPlatform);
            string _sDebugger =  _oCompiler.oGblConfigType.fGetNode(null,new string[]{"Exe", "Debugger"}, "");
            GDB _oGdb  =  new GDB(this, oCurLaunch,_sDebugger, _sExePath, _oCompiler, _sArg); //Create debugger proxy

        } else {
          	oCurLaunch.fLaunchExe( _sExePath, _sArg);
        }
	

		//_oPreload.dExit = new LaunchTool.dIExit(fUrlRequestComplete);
	//	Output.TraceWarning("------------------------");
/*
                    oCurLaunch.oForm = this;
                    oCurLaunch.bSanitize = _bSanitize;
                    oCurLaunch.fLaunchExe(_sPath);
*/

                    return true;
                }else {
				Output.TraceError("Executable not found: " + _sPath);
				}
      //      }
             return false;
        }


public  void 	fExit(LaunchTool _oTool){

    if( Data.oLaunchProject.oCurLaunch != null) {
        Output.TraceError("Exit: " + _oTool.sExeName);
	    Data.oLaunchProject.oCurLaunch = null;
        Build.EndExecution();
    }

}





public  void 	fAppOut(LaunchTool _oTool, string _sOut){
    Output.fPrjOut("O", _sOut );
    bReceiveOutput = true;
}


public  void 	fAppError(LaunchTool _oTool, string _sOut){

         Output.ProcessStdErr(_sOut);
        bReceiveOutput = true;
}





		 public void fLaunchEnd() {
            /*
			if(oMainForm!= null) {
				oMainForm.fLaunchEnd();
			}*/

			oCurLaunch = null; 
		}

		 public void fCancel() {

			 if(oCurLaunch != null) {
                oCurLaunch.fEnd();
                  //   Build.fDisableBuild();
             //   oCurLaunch = null;
             //   Data.EndExecution();


            }else if(Data.bNowBuilding) {
                 Build.StopBuild();
            }else {
                    
             //    fResetList();
               //  Data.StartBuild();
               //  Data.sCmd = "StartBuild";

                //Start Last Launch
                  Delocalise.fDelocaliseInMainThread(Config.sLastRecentPath );

            }
		}


		 internal void fSetOutput(List<CppCmd> _aLinkCmd, string _wTo = "" ) {

			if(_wTo != "") {
				
				sOutput =_wTo;
	//Debug.fTrace("---------  _wTo - " +_wTo );

				//return;
			}else {
				foreach(CppCmd _oCmd in _aLinkCmd ) {
					//if( _oCmd.sOutputFile != "") {
					if( _oCmd.sOutputFile != "" && _oCmd.sOutputFile[_oCmd.sOutputFile.Length-1] != '/') {
						 sOutput= _oCmd.sOutputFile;
					}
				}
			}
            /*
			if(oMainForm!= null) {
				oMainForm.fSetOutput(sOutput);
			}
            */


			
        }
			
		
       //private  LaunchTool oLaunchCmd = null;

      public void fLaunchConsoleCmd(string sCurrentCmd) {

            ///// CURRENT APP Launch
            if (Data.oLaunchProject.oCurLaunch != null) {

                 oCurLaunch.fSend(sCurrentCmd);
                Debug.fPrint("");
                 return;
            }

            ///// CONSOLE Launch
            if (sCurrentCmd.Trim() == "") {
                fConsoleExit(null);
                return;
            }

           	oCurLaunch = new LaunchTool();
      //      _oLaunchUrl.bWaitEndForOutput = true;
            oCurLaunch.bRedirectOutput = false; //Important 
           // _oLaunchUrl.bRunInThread = false;

			oCurLaunch.dOut = new LaunchTool.dIOut(fConsoleOut);
			oCurLaunch.dError = new LaunchTool.dIError(fConsoleOut);
			oCurLaunch.dExit = new LaunchTool.dIExit(fConsoleExit);
       // _oLaunchUrl. = true;
		    
	       Debug.fPrint("");

			oCurLaunch.fLaunchExe("cmd.exe" , "/C " + sCurrentCmd); 
            
      }

   public  void  fConsoleOut(LaunchTool _oTool, string _sOut){

      Debug.fPrint(_sOut );
            bReceiveOutput = true;
          
      //Debug.fPrint(_sOut );
            //Require answer
            /*
            if (_sOut == "(gdb) (gdb)") {
                fConsoleExit(null);
            }*/
    }

	
      public  void  fConsoleExit(LaunchTool _oTool){
    
           if(_oTool != null) {
                   bReceiveOutput = false;
             // Data.oLaunchProject.oCurLaunch = null;
                oCurLaunch = null;
            }
              
           string _sName = "Cmd";
            if (oCurLaunch != null) {
                _sName = oCurLaunch.sExeName;
            }

     	 Debug.fPrint("");
	      Debug.fWPrint("(" + _sName + ")> ");
    }

        internal void fBeginEnterNewCmd() {
          
            if (bReceiveOutput) {
                bReceiveOutput = false;
                fConsoleExit(null);
            }

        }
    }
}
