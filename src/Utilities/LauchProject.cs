using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cwc{

	public class LauchProject{

	//	public  MainForm oMainForm = null;
			
		// public Lauch oCurLauch = null;
		 public LauchTool oCurLauch = null;
		 public Boolean bReceiveOutput = false;


		 public string sOutput = "";
		 public string sOutputExecutable = "";

		        
		public  void fBuildFinish() {
           Debug.fTrace("fBuildFinish: " + Data.fGetGlobalVar("wBuildAnd"));
            //fLauchDefaultRun();
        }

        public  void fLauchDefaultRun(string _sPath = "", string _sSubArg = "") { //"" = Last link file


            if(!Data.bModeIDE ) {
			   // switch(Data.sBuildAnd) {
				switch(Data.fGetGlobalVar("wBuildAnd")) {

					case "Run":

							fLauchExe(_sPath, false, _sSubArg);
					break;
					 case "Sanitize":
                    
							fLauchExe(_sPath, true, _sSubArg);
					break;
				}
			}
        }



		 public void fLauchExe(string _sPath = "",  bool _bSanitize = false, string _sSubArg = "") {
            if(_sPath == "") {//Lauch default last linked output binary
                 _sPath = PathHelper.ExeWorkDir +   sOutputExecutable;

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
            if(fLauch(_sPath, _bSanitize, _sSubArg)) {
                
            }
        }


		 public bool fLauch(string _sPath, bool _bSanitize = false, string _sSubArg = "") {

            if(_sPath.Length > 1 && _sPath[_sPath.Length-1] =='/') { //Not a file
                Output.TraceError("Path is not a File: " + _sPath);
                return false;
            }
           //   Output.TraceError("Try: " + _sPath);

            bool _bDebug = true;//Temp
            bool _bLauchDebug = false;
	        if(_bDebug){
                _bLauchDebug = true;
            }



		//	Debug.fTrace("Lauch: " + _sPath);
			
			

   // if (oCurLauch == null) { //We can relauch !? => Run button
                if( File.Exists(  _sPath ) ) {
							
					if(Data.oGuiConsole != null) {
						Data.oGuiConsole.fLauchPrj();
					}
					
            
                    oCurLauch = new LauchTool();

		//oCurLauch.bWaitEndForOutput = true;
		//	oCurLauch.bRunInThread = false;

        oCurLauch.bRedirectOutput = false;


        string _sArg = "";
        string _sExePath = _sPath;
		if(Data.fGetGlobalVar("_sPlatform") == "Web_Emsc") {
		        _bDebug = false; // No GDB
                oCurLauch.bRedirectOutput = true;
                oCurLauch.bHidden = true;
                oCurLauch.UseShellExecute = true;


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
            //	oCurLauch.bDontKill = true;
            //oCurLauch.UseShellExecute = false;
        }
        
        _sArg += _sSubArg;
        _sArg = _sArg.Trim();
           //Output.TraceWarning("_sSubArg " + _sSubArg);

        oCurLauch.dError = new LauchTool.dIError(fAppError); //Too much error in Emsc?
        oCurLauch.dOut = new LauchTool.dIOut(fAppOut);
 
	    oCurLauch.dExit = new LauchTool.dIExit(fExit);

           string _sPrintArg = "";
          if(_sArg != "") {
                _sPrintArg = " [" + _sExePath + " " + _sArg + "]";
          }



         if(_bLauchDebug){
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
            GDB _oGdb  =  new GDB(this, oCurLauch,_sDebugger, _sExePath, _oCompiler, _sArg); //Create debugger proxy

        } else {
          	oCurLauch.fLauchExe( _sExePath, _sArg);
        }
	

		//_oPreload.dExit = new LauchTool.dIExit(fUrlRequestComplete);
	//	Output.TraceWarning("------------------------");
/*
                    oCurLauch.oForm = this;
                    oCurLauch.bSanitize = _bSanitize;
                    oCurLauch.fLauchExe(_sPath);
*/

                    return true;
                }else {
				Output.TraceError("Executable not found: " + _sPath);
				}
      //      }
             return false;
        }


public  void 	fExit(LauchTool _oTool){

    if( Data.oLauchProject.oCurLauch != null) {
        Output.TraceError("Exit: " + _oTool.sExeName);
	    Data.oLauchProject.oCurLauch = null;
        Build.EndExecution();
    }

}





public  void 	fAppOut(LauchTool _oTool, string _sOut){
    Output.fPrjOut("O", _sOut );
    bReceiveOutput = true;
}


public  void 	fAppError(LauchTool _oTool, string _sOut){

         Output.ProcessStdErr(_sOut);
        bReceiveOutput = true;
}





		 public void fLauchEnd() {
            /*
			if(oMainForm!= null) {
				oMainForm.fLauchEnd();
			}*/

			oCurLauch = null; 
		}

		 public void fCancel() {

			 if(oCurLauch != null) {
                oCurLauch.fEnd();
                  //   Build.fDisableBuild();
             //   oCurLauch = null;
             //   Data.EndExecution();


            }else if(Data.bNowBuilding) {
                 Build.StopBuild();
            }else {
                    
             //    fResetList();
               //  Data.StartBuild();
               //  Data.sCmd = "StartBuild";

                //Start Last lauch
				if(Config.sLastRecentPath != "") {
                  Delocalise.fDelocaliseInMainThread(Config.sLastRecentPath );
				}else {
					Output.TraceWarning("Nothing to build...");

				}

            }
		}


		 internal void fSetOutput(List<CppCmd> _aLinkCmd, string _wTo = "" ) {

			if(_wTo != "") {
				
				sOutput =_wTo;
                try {
					 string _sExt = Path.GetExtension(_wTo).ToLower();
					switch(_sExt) {
						case ".bat":
						case ".exe":
								sOutputExecutable = _wTo;
						break;
					}
                }catch(Exception Ex) {};

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
			
		
       //private  LauchTool oLauchCmd = null;

      public void fLauchConsoleCmd(string sCurrentCmd) {

            ///// CURRENT APP LAUCH
            if (Data.oLauchProject.oCurLauch != null) {

                 oCurLauch.fSend(sCurrentCmd);
                Debug.fPrint("");
                 return;
            }

            ///// CONSOLE LAUCH
            if (sCurrentCmd.Trim() == "") {
                fConsoleExit(null);
                return;
            }

           	oCurLauch = new LauchTool();
      //      _oLauchUrl.bWaitEndForOutput = true;
            oCurLauch.bRedirectOutput = false; //Important 
           // _oLauchUrl.bRunInThread = false;

			oCurLauch.dOut = new LauchTool.dIOut(fConsoleOut);
			oCurLauch.dError = new LauchTool.dIError(fConsoleOut);
			oCurLauch.dExit = new LauchTool.dIExit(fConsoleExit);
       // _oLauchUrl. = true;
		    
	       Debug.fPrint("");

			oCurLauch.fLauchExe("cmd.exe" , "/C " + sCurrentCmd); 
            
      }

   public  void  fConsoleOut(LauchTool _oTool, string _sOut){

      Debug.fPrint(_sOut );
            bReceiveOutput = true;
          
      //Debug.fPrint(_sOut );
            //Require answer
            /*
            if (_sOut == "(gdb) (gdb)") {
                fConsoleExit(null);
            }*/
    }

	
      public  void  fConsoleExit(LauchTool _oTool){
    
           if(_oTool != null) {
                   bReceiveOutput = false;
             // Data.oLauchProject.oCurLauch = null;
                oCurLauch = null;
            }
              
           string _sName = "Cmd";
            if (oCurLauch != null) {
                _sName = oCurLauch.sExeName;
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
