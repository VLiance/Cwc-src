using cwc;
using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwc
{
    class CppCompiler   {

        private static volatile bool bHasError = false;
        public volatile static uint nError = 0;
        public static int safeInstanceCount = 0;
        static public int SafeInstanceCount{
            get { return safeInstanceCount; }
        }

		static public int nNumOfCore =  Environment.ProcessorCount;
		
        public static void CheckAllThreadsHaveFinishedWorking(bool _bAll = false){
			if(nNumOfCore < 2 || nNumOfCore > 555555) { //TODO better way
				nNumOfCore = 2;	
			}

            //Wait for process finish
            int nNbSimulThread = nNumOfCore - 1; //Nb core -1
            if (_bAll)  {
                nNbSimulThread = 1;
            }

            while (!(SafeInstanceCount < nNbSimulThread)) {
                Thread.CurrentThread.Join(1);
            }
            Thread.CurrentThread.Join(1);
        }

        public static readonly Object oLockTicket = new Object();
        public static volatile uint nTotalTicket = 0;
        public static volatile uint nCurrentTicket = 0;
        public static volatile int nErrorTicket = -1;

        public static void fShowArg(string _sArg, bool _bSubCmd = false) {
            if (_sArg == "") {//Already shown
                return;
            }
            if(Data.bNowBuilding) {
	            if(_bSubCmd) {

			            Output.Trace("\f1B:\f18" + _sArg);
	            }else {

			            Output.Trace("\f1B|\f16" + _sArg);
	            }
            }
        }

        public static void fShowResult(OutToken _oOut  ) {
            string _sOut = _oOut.sOut;
            CppCmd _oCmd = _oOut.oFrom;

             if(Data.bNowBuilding) {
              
                if(fManageMessages(_oCmd,_sOut)){return;}

                if (_oOut.eType == OutType.Undefined) {
                    Output.TraceUndefined(_sOut);
                    return;
                }
                 if (_oOut.eType == OutType.Warning) {
                    Output.TraceWarningLite(_sOut);
                    return;
                }
                 if (_oOut.eType == OutType.Error) {
                    Output.TraceErrorLite(_sOut);
                    return;
                }
               
				//Add color manually
				if(_sOut.Length > 8) {
					if(_sOut[7] == ':') { //no color by default
						bool bFound = false;
						string _sCmd = _sOut.Substring(0,7).ToLower();
						switch(_sCmd) {
							case "warning": 
							bFound = true;
							 Output.TraceWarning(_sOut);
							break;
						}
					}
				}

                 string _sResult = Output.Trace(_sOut, true);
                /*
                if( Data.oMainForm != null) {
                     Data.oMainForm.fAddItem(_sResult);
                }*/
            }
        }

		public enum OutType {None, Undefined, Error, Warning};

        public class OutToken {
            public string sOut = "";
            public  CppCmd oFrom = null;
            internal bool bShowed = false;
            internal bool bInError;
            internal OutType eType = OutType.None;
        }

        public static  List<OutToken> aOutput  = new List<OutToken>() ;

        public static void fSend2Compiler(string _sArg, bool _bLinkTime = false, bool _bCompileAndLink = false, CppCmd _oCmd = null, string _sAllFile = "") {
           

			//string _sPlatform = _oCmd.oParent.sPlatform;
			string _sPlatform = _oCmd.oParent.fGetVar("_sPlatform");

			if(_oCmd.oCompiler == null) {	
				//Debug.fTrace("Unknow Compiler");
				Output.TraceError("Unknow Compiler");
				return;
			}
                
                string _sInfo = "";
            if (_oCmd != null && _bLinkTime) {
                if(_oCmd.bToStaticLib) {
                     _sInfo = " [Static_Link]";
                }else if(_oCmd.bToDynamicLib) {
                     _sInfo = " [Dynamic_Link]";
                }else { 
                     _sInfo = " [Link]";
                }
            }

            string _sObjectList = "";
            if(_sAllFile != "") {
                _sObjectList =  "[\f1B" +_sAllFile + "\f1F]";
            }

            string _sFinalArg = _sArg.Replace("\\\"", "\"");

            _sFinalArg = _sFinalArg.Replace("%%", "¶"); //A way to keep %
            _sFinalArg = _sFinalArg.Replace('%', '\\'); //Backslash special char % transform, we must have a ways to send backslash to compiler
            _sFinalArg = _sFinalArg.Replace('¶', '%');  //A way to keep %

            _oCmd.sCommandToShow = "\f1F (" +   _oCmd.sExecutableName  + _oCmd.sExecutableType + _sInfo  + ")" +  _sObjectList + "  " +_sFinalArg;


             if(Data.fGetGlobalVar("_sType") == "Bash") {

                /// Create OUT directory ///
                string reldir = Path.GetDirectoryName(_oCmd.sOutDirectory).Replace("\\","/");
                string[] list = reldir.Split('/');
                //string dir = _base;
                string dir = "";
                foreach (string d in list) {
                    dir+=d+"/";
                    //Data.fAddToBash(_oCmd,"mkdir " + dir+ "\n");
                    Data.sBash += "mkdir " + dir+ "\n";
                }
                //////////////
                 Data.fAddToBash(_oCmd, _oCmd.sExecutableName + " " + _sFinalArg);
                 fShowSendedCmd(_oCmd);
                 return;
             }


            uint _nMyTicket = 0;
            lock(oLockTicket) {
                _nMyTicket = nTotalTicket;
                nTotalTicket++;
            }

            if(bHasError || !Data.bNowBuilding) {
                return;
            }

            CheckAllThreadsHaveFinishedWorking();
            if(bHasError || !Data.bNowBuilding) {
                return;
            }
            Interlocked.Increment(ref safeInstanceCount);
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {

                if(bHasError || !Data.bNowBuilding) {
                     Interlocked.Decrement(ref safeInstanceCount);
                       // Debug.fTrace("Dec " + safeInstanceCount);
                     return;
                 }

                 StringBuilder output = new StringBuilder();
                 StringBuilder error = new StringBuilder();

                 using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                 using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                 {
                     using (Process process = new Process()) {

						process.StartInfo.FileName = _oCmd.sExecutable;
                        process.StartInfo.Arguments =_sFinalArg ;

                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;

                        process.StartInfo.WorkingDirectory =_oCmd.s_pProject; 

                        try  {

                            if(process.StartInfo.FileName.Length +  process.StartInfo.Arguments.Length  >= 32700){ // Command line size limitation, real 32768 
                                //Todo verify for .wdat folder?
                                process.StartInfo.Arguments = "@.wdat/arg.wdat";
                                File.WriteAllText(@".wdat/arg.wdat", _sFinalArg);
                            }

                            process.OutputDataReceived += (sender, e) => { if (e.Data != null) {
                                fCompilerError(e.Data, _sArg, _nMyTicket,false, _oCmd);
                            }};
                            process.ErrorDataReceived +=  (sender, e) => { if (e.Data != null) {
                                fCompilerError(e.Data, _sArg, _nMyTicket, true, _oCmd);
                            }};
                                
                            process.Start();
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();
                            process.WaitForExit();

                            ///////////////////////////////////////
                            ///Wait for displaying in order
                            //////////////////////////////////////
                            while(Base.bAlive && Data.bNowBuilding ) {
                                Thread.CurrentThread.Join(1);
                                lock(oLockTicket) {
                                  
                                    if(nCurrentTicket == _nMyTicket) {

                                        fShowSendedCmd(_oCmd);
                                        fShowProcOutput(_oCmd);

                                        if (nErrorTicket == nCurrentTicket &&  (nError > 0 ) ) { //&& GuiForm.fIsChecked("afterFileErrorToolStripMenuItem") //TODO
                                            fShowProcOutput(_oCmd);
                                            Build.StopBuild(); //Dont display other file errors
                                            break;
                                            }


                                            nCurrentTicket ++;
                                        break;
                                    }
                                }
                            }
                            ////////////////////////////////////
                         }catch(Exception e){
                            Output.TraceError(e.Message);
                            string _sSended = process.StartInfo.FileName + " " + process.StartInfo.Arguments;
                            Output.TraceAction(_sSended);
                            if(_sSended.Length >= 32768){
                                  Output.TraceWarning("You may have exceeded the maximum command line size of 32768 characters: " + _sSended.Length + " chars"  );     
                            }
                            Interlocked.Decrement(ref safeInstanceCount);
                            return;
                        }

                        

                         while (!process.HasExited) {
                            Thread.CurrentThread.Join(1);
                            if(!Data.bNowBuilding) {
                                 break;
                             }
                         }

                         Interlocked.Decrement(ref safeInstanceCount); //safeInstanceCount never decremented if after  fAddCommandLineVerificationToDepedancesFile?? on link time : exception?
                         if(_oCmd != null && !_bLinkTime) { 
                              _oCmd.fAddCommandLineVerificationToDepedancesFile(_oCmd); //TODO is infinite loop?
                         }
                     }
                 }
             });
            worker.RunWorkerAsync();
        }

    

        private static readonly Object oLock = new Object();
        private static readonly Object oLockFirst = new Object();

        public static bool fManageMessages( CppCmd _oCmd, string _sMsg) {

            if( _sMsg.IndexOf("Unhandled Exception") >= 0) {
                Output.TraceError(_sMsg);
                nError++;
                 return true;
            }

            if (_sMsg.Length >= 3 && _sMsg[1] == '|' && ( _sMsg[2] != '/'  ||  _sMsg[2] != '\\') ) {
                string _sMessage = _sMsg.Substring(2).Trim();
                switch (_sMsg[0]) {
                     case 'W':
                        Output.TraceWarning(_sMessage);
                      return true;

                    case 'I':
                        Output.TraceAction(_sMessage);
                        return true;

                    case 'E':
                        Output.TraceError(_sMessage);
                        nError++;
                      return true;
  
                     case 'A':
                      
                             String[] _aMsg = _sMessage.Split('|');
                            if(_aMsg.Length>= 2) {
                                    switch (_aMsg[0]) {
                                  
                                     case "[C~:Lib]":
                                        Output.TraceAction( _sMessage);
                                        fAssistCwLib(_sMessage);

                                     return true;

                                        case "C~>C++":
                                        case "C~2Cpp":
                                        case "Cw2Cpp":
                                             Output.TraceAction(_sMessage);
                                            _oCmd.fAddCompiledObject(_aMsg[1]  + ".cpp"); //Know as .cpp object
                                            return true;
                                
                                        case "C~:UpToDate":
                                        case "CwUpToDate":
                                        case "CwUp2Date":
                                              Output.TraceColored("\f27C~:Up2Date|\f28" +_aMsg[1] );
                                             _oCmd.fAddUpToDateObject(_aMsg[1] + ".cpp");
                                         return true;
                                    }
                              }
                       break;
                }
            }
             return false;
        }

        private static void fAssistCwLib(string _sMessage) {
             string[] _aMessage = _sMessage.Split('|');
            int _nIndex = 0;
            string _sParentModule = "";
            string _sPath = "";
            string _sName = "";

            foreach(string _sMsg in _aMessage) {_nIndex++;
                if(_sMsg.Length>3) {
                    if(_sMsg[0] == '(') {
                        _sParentModule = _sMsg.Substring(1, _sMsg.LastIndexOf(')')   -1);
                        _sName = _sMsg.Substring(_sParentModule.Length + 2);

                    }
                    if(_nIndex != 1 && _sMsg[0] == '[') {
                        _sPath  = _sMsg.Substring(1, _sMsg.Length-1 -1);
                    }
                }
            }
            
            //FOUND a SUBLIB
            if(_sName != "" && _sParentModule != "" && _sPath != "") {

              ModuleData _oMyModule = ModuleData.fGetModule(_sParentModule,false);
              if(_oMyModule != null) {

                    ModuleData _oLib = new ModuleData(_oMyModule, _sPath, _sName);
                    _oMyModule.fAddSubLib(_oLib);
                    _oMyModule.fGetSubLibCompilerData( Data.oArg );//Data.oArg  not sure

                    Output.TraceAction("Add SubLib: " + _sName);
              }
            }
        }

        public static void fShowSendedCmd(CppCmd _oFrom ) {
               fShowArg(_oFrom.sCommandToShow,_oFrom.bIsSubCmd);
             _oFrom.sCommandToShow = "";
        }

       public static void fShowProcOutputString(OutToken _oOut ) {
             fShowSendedCmd(_oOut.oFrom);
             fShowResult(_oOut );
        }

        public static void fShowProcOutput(CppCmd _oCmd ) {
            if(aOutput != null && aOutput.Count > 0) {
                //_oCmd.oDepandance.fShowFile();
                foreach(OutToken _oOut in aOutput) { //Show all precedant commands
                    if(_oCmd == _oOut.oFrom && _oOut.bShowed == false){
                            _oOut.bShowed = true;
                            fShowProcOutputString(_oOut);
                    }
                }
            }
        }

        public static string sProcOutputRetrun  = "";
        public static void fCompilerError(string _sResult,string _sArg, uint _nMyTicket, bool _bStdError = false,  CppCmd _oCmd = null) {

            //Direct show if current
            lock (oLockTicket) {
                
                _oCmd.sLaunchCmdResult += _sResult + "\n";

                if(_oCmd.sCloseWhen != ""){
                    if(_sResult.IndexOf(_oCmd.sCloseWhen) != -1) {
                        if(_oCmd.oCurrProcess != null && !_oCmd.oCurrProcess.HasExited) {
                                _oCmd.oCurrProcess.Kill();
                        }
                    }
                }

                if(_oCmd != null && _oCmd.oToInputProcess != null){
                     _oCmd.oToInputProcess.StandardInput.WriteLine(_sResult );
                  //   _oCmd.oToInputProcess.StandardInput.Write(_sResult + "\n");
                    _oCmd.oToInputProcess.Refresh();
                    Console.WriteLine("R:" + _sResult);
                    return;
                }


                sProcOutputRetrun += " " + _sResult;
                if(!(Base.bAlive && Data.bNowBuilding)) {
                    return;
                }

                if(nCurrentTicket == _nMyTicket) { //Direct show to not wait ending compilation
                    fShowProcOutput(_oCmd);
                }
                 
			    if(_bStdError) {

				    if( fFindValidKeyWord(_sResult, "error") != -1 ) { //Generiquer error check!?
					    nError++;
                        
                        if(_nMyTicket < nErrorTicket  || nErrorTicket ==-1){
                            nErrorTicket = (int) _nMyTicket;
                        }

				    }
			    }
               
                if(aOutput != null ) {

                      OutToken _oOut = new OutToken();
                    _oOut.oFrom = _oCmd;
                    _oOut.sOut = _sResult;
           
                     if(_sResult.IndexOf("undefined reference to") != -1) {
                        _oOut.eType = OutType.Undefined;
                        nError++;
                        if (_nMyTicket < nErrorTicket || nErrorTicket == -1) {
                            nErrorTicket = (int)_nMyTicket;
                        }
                    }
                     
                     if( fFindValidKeyWord(_sResult, "error") != -1  ) {
                        _oOut.eType = OutType.Error;
                        nError++;
                        if (_nMyTicket < nErrorTicket || nErrorTicket == -1) {
                            nErrorTicket = (int)_nMyTicket;
                        }
                    }
                     if(  fFindValidKeyWord(_sResult, "warning") != -1) {
                        _oOut.eType = OutType.Warning;
                    }
                    aOutput.Add(_oOut);
                }

                if(nCurrentTicket == _nMyTicket) { //Direct show to not wait ending compilation
                    fShowProcOutput(_oCmd);
                }
           }
        }

        public static int fFindValidKeyWord(string _sLine, string _sKey) {
            if(_sLine.Length <= 1) {return -1; }
            int _nIndex = 0;
           int _nNext = 0;
             char prev;
            while (_nIndex != -1) {
               
                _nIndex =  _sLine.IndexOf(_sKey, _nIndex);

                if(_nIndex != -1) {
                    _nNext = _nIndex + _sKey.Length;
                    if(_nNext < _sLine.Length){
                        int _nPrev = _nIndex-1;
                        if (_nPrev<0){ _nPrev = 0;prev = ' ';}else{prev =  _sLine[_nPrev];}
                        char next =  _sLine[_nNext];
                        if( next  != '.' && !(next >= 'a' && next <= 'z')  && !(prev >= 'a' && prev <= 'z')  && prev != '_'  && prev != '/' && prev != '\\'  && prev != '.') { //like error.o -`>  not valid
                       // if(_nNext < _sLine.Length &&  next  != '.' &&  (next  <= 45  || next == ':') &&  !(  (prev  >= 'a' && prev  <= 'z')  ||  (prev  >= 'A' && prev  <= 'Z')  ) ) { //like error.o -`>  not valid
                            return _nIndex;
                        }
                     }
                    _nIndex++;
                }
            }

             //  return _nIndex;
            return -1;
        }

        
        public static string fExtracQuote_sArg = "";
         public static string fExtracQuote(string _sValue, bool _bSplitWithSpace = true) {//_cRequiredDelim??
                if(_sValue.Length < 1){return "";}

                fExtracQuote_sArg = "";
                int _nIndexBegin = 0;
                int _nIndexEnd = -1;
                string _sResult = _sValue; 
                    //Remove quote!!
                if (_sResult[0] == '\"' ) { _nIndexBegin = 1;
                    _nIndexEnd = _sResult.IndexOf('\"',1);
                }else if (_sResult[0] == '\'' ) { _nIndexBegin = 1;
                    _nIndexEnd = _sResult.IndexOf('\'',1);
                }else if(_bSplitWithSpace) {
                    _nIndexEnd = _sResult.IndexOf(' ');
                }
                if(_nIndexEnd != -1) {
                     fExtracQuote_sArg = _sResult.Substring( _nIndexEnd + 1).Trim();
                    _sResult = _sResult.Substring(_nIndexBegin, _nIndexEnd + (-_nIndexBegin)).Trim();
                }
                return _sResult;
        }

        internal static void  fLaunchInputApp(Process _oToInputProcess, List<CppCmd> aSubInputCmd) {
            foreach(CppCmd _oCmd in aSubInputCmd) {
                _oCmd.oToInputProcess = _oToInputProcess;
                 Output.TraceWarning("Launch Input App: " + _oCmd.sExplicite_App + ":" +  _oCmd.sExplicite_Call);
                _oCmd.fExecute();
            }
        }

        internal static string fSend2App(CppCmd _oCmd, string _sExplicite_Name, string _sExplicite_App, string _sExplicite_Call, bool _bWaitToFinish = false, List<CppCmd> aSubInputCmd = null) {
            
           bool _bFinished = false;
            if(_bWaitToFinish){
                sProcOutputRetrun = "";
            }

            string _sExe   = fExtracQuote(_sExplicite_App);
            string _sArg = fExtracQuote_sArg + " " + _sExplicite_Call;
           
            //TODO merge with fSend2Compiler
            Output.TraceAction(_sExplicite_Name + " => "  + _sExplicite_Call);

            uint _nMyTicket = 0;
            lock(oLockTicket) {
                _nMyTicket = nTotalTicket;
                nTotalTicket++;
            }

            if(bHasError || !Data.bNowBuilding) {
                return "";
            }

            CheckAllThreadsHaveFinishedWorking();
            if(bHasError || !Data.bNowBuilding) {
                return "";
            }
            Interlocked.Increment(ref safeInstanceCount);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(
             delegate (object o, DoWorkEventArgs args){
                if(bHasError || !Data.bNowBuilding) {
                     Interlocked.Decrement(ref safeInstanceCount);
                     return;
                 }

                 StringBuilder output = new StringBuilder();
                 StringBuilder error = new StringBuilder();

                 using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                 using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false)) {
                 using (Process process = new Process()) {
                      
                        _oCmd.oCurrProcess = process;
                        _oCmd.sLaunchCmdResult = "";

                        process.StartInfo.FileName =  _sExe;
                        process.StartInfo.Arguments = _sArg;

                        process.StartInfo.CreateNoWindow = true;
                        //process.StartInfo.CreateNoWindow = false;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;

                        if(aSubInputCmd != null) {
                            process.StartInfo.RedirectStandardInput = true;
                        }

	                    process.StartInfo.WorkingDirectory =_oCmd.s_pProject; 

                        try  {
                            process.OutputDataReceived += (sender, e) => {
                                if (e.Data != null)  {
                                    fCompilerError(e.Data, _sExplicite_Name + " : " + _sExplicite_Call, _nMyTicket,false, _oCmd);
                                }
                            };
                            process.ErrorDataReceived += (sender, e) => {
                                if (e.Data != null)  {
                                    fCompilerError(e.Data, _sExplicite_Name + " : " + _sExplicite_Call, _nMyTicket, true, _oCmd);
                                }
                            };
                               
                            Console.WriteLine("Start " +	 process.StartInfo.FileName );
                            Console.WriteLine("arg " +	     process.StartInfo.Arguments  );
 
                            process.Start();
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();
                            fLaunchInputApp(process, aSubInputCmd);

                            process.WaitForExit();
                            ///////////////////////////////////////
                            ///Wait for displaying in order
                            ////////////////////////////////////////*
                            while(Base.bAlive && Data.bNowBuilding ) {
                                Thread.CurrentThread.Join(1);
                                lock(oLockTicket) {
                                  
                                    if(nCurrentTicket == _nMyTicket) {
                                    // Console.WriteLine("*** Process  " + nCurrentTicket + " " + _oCmd == null );
                                        fShowSendedCmd(_oCmd);
                                        fShowProcOutput(_oCmd);
                                        if (nErrorTicket == nCurrentTicket &&  (nError > 0 ) ) {
                                            //Console.WriteLine(":: " +  GuiForm.fIsChecked("afterFileErrorToolStripMenuItem")); //TODO TODOTODO
                                            fShowProcOutput(_oCmd);
                                            Build.StopBuild(); //Dont display other file errors
                                            break;
                                                
                                            }
                                            nCurrentTicket ++;
                                        break;
                                    }
                                }
                            }
                        ///////////////////////////////////////////////
                        }catch(Exception e) {
                        Output.TraceError("Error with " + process.StartInfo.FileName + "[" + process.StartInfo.Arguments + "]:" + e.Message);
                        } finally {}

                        try {  while (!process.HasExited){
                             Thread.CurrentThread.Join(1);
                            if(!Data.bNowBuilding) {
                                 break;
                             }
                         } }catch(Exception e) {}

                         _bFinished = true;
                          Interlocked.Decrement(ref safeInstanceCount); //safeInstanceCount never decremented if after  fAddCommandLineVerificationToDepedancesFile?? on link time : exception?

                     }
                 
                    }
                 });
                worker.RunWorkerAsync();

                 if( _oCmd.bRetryForInput){
                    while(!_bFinished){
                      Thread.CurrentThread.Join(1);
                    }
                    if(_oCmd.sLaunchCmdResult == "") {
                           //Recursive call
                         //Thread.Sleep(1); //Minimal wait
                         return fSend2App(_oCmd,_sExplicite_Name,_sExplicite_App,_sExplicite_Call,_bWaitToFinish,aSubInputCmd);
                    }
                    return _oCmd.sLaunchCmdResult ;
                 }

                if(_bWaitToFinish){
                    while(!_bFinished){
                       Thread.CurrentThread.Join(1);
                    }
                    return sProcOutputRetrun.Trim();
                 }

                return "";
            }
    }
}
