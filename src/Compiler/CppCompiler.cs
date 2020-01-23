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

        private static volatile bool bLinkerFinish = false;
        private static volatile bool bHasError = false;
        private volatile static bool bStopAll = false;
        public volatile static uint nError = 0;


    //    public  static string sTEst = "aaaaaaaaaa";


/*
        public  static string sLibRT_Dir = PathHelper.GetExeDirectory() + "Modules/LibRT_x64/";
        public  static string sLibRTDK_Dir = PathHelper.GetExeDirectory() + "Modules/LibRTDK/";

        //  private  static string sSubCompiler = "GCC";   
        private static string sCompilerPathClang = sLibRT_Dir + "bin/clang++.exe";
        private static string sCompilerPathGCC = sLibRT_Dir + "bin/g++.exe";

        private static string sSLibPathClang = sLibRT_Dir + "bin/clang++.exe";
        private static string sSLibPathGCC = sLibRT_Dir + "bin/ar.exe";

        //    private  static string sSubCompilerLink = "Clang";  private static string sLinkerPath = sLibRT_Dir + "/bin/clang++.exe";
        //private  static string sSubCompilerLink = "GCC";  private static string sLinkerPath = sLibRT_Dir + "/bin/g++.exe";

        private static string sLinkerPathGCC = sLibRT_Dir + "bin/g++.exe";
        private static string sLinkerPathClang = sLibRT_Dir + "bin/clang.exe";


        private  static string sArch = "-m32 "; 
        //private  static string sArch = "-m64 "; 


       private  static string sCompilerClangOption = " -target i686-pc-mingw32 "; 
     //  private  static string sCompilerClangOption = " "; 

       //private  static string sCompiler = "Gcc";

      
       //private static string sCompilerPath = sLibRT_Dir + "/bin/g++.exe";


     //   private static string sLinkerPath = sLibRT_Dir + "/bin/ld.exe";
       // private static string sLinkerPath = sLibRT_Dir + "/bin/g++.exe";
      //  private static string sLinkerPath = sLibRT_Dir + "/bin/gcc.exe";
       
        
        //bInConsole
      //  public static string sHiddenArg_sColoConsole = " -fdiagnostics-print-source-range-info -fcolor-diagnostics -fansi-escape-codes ";
        public static string sHiddenArg_sColoConsole = "-fcolor-diagnostics -fansi-escape-codes ";
    //    public static string sHiddenArg_sColoConsole = " ";
       
        private static string sHiddenArg_GCC = "-MD  -Werror=return-type -fno-rtti ";
        private static string sHiddenArg_Clang = "-MD  -Werror=return-type -fno-exceptions -fno-rtti ";

     //   private static string sHiddenArg_All = " -MD -Werror=return-type ";

         private static string sClangHideWarning = "-Qunused-arguments -Wno-unused-value -Wno-deprecated-register -Wno-ignored-attributes -Qunused-arguments -Wno-expansion-to-defined -Wno-ignored-pragmas ";

        //private static string sHiddenArg_All = " -Qunused-arguments ";

            
      
        /// ///////////////////////////////////  INCLUDE //////////////////////

         private static string sInlude64_base = "-I " + sLibRT_Dir + "lib/gcc/i686-w64-mingw32/5.4.0/include/ ";
        private static string sInlude64        = "-I " + sLibRT_Dir + "i686-w64-mingw32/include/ ";

      //  private static string sInlude64_2      = "-I " + sLibRT_Dir + "/x86_64-w64-mingw32/include/ ";
     //  private static string sInlude64_base_2  =   "-I " + sLibRT_Dir + "/lib/gcc/x86_64-w64-mingw32/5.1.0/include/ ";

        private static string sInlude      = "-I " + sLibRT_Dir + "include/ ";
     //   private static string sInludeLib      = "-I " + sLibRT_Dir + "/Lib/ ";
        private static string sInludeLib      = " ";


        private static string sMinGwLib64  =  " -L" + sLibRT_Dir + "i686-w64-mingw32/lib64/ ";


     private static string sInludeClang  =  " -I" + sLibRT_Dir + "clang_inc/ ";

     private static string sClangStdInc = sInlude64_base + sInlude64 + sInlude + sInludeLib;

/////////////////////////////////////////////////////////////////
      
     //   private static string sHiddenArg_Platform = " -target i686-pc-mingw32 -lgdi32 -lopengl32 ";
    //    private static string sHiddenArg_Platform = " -target i686-pc-mingw32  -std=c++11 " + sInlude64_base + sInlude64  + " -I "+ sLibRT_Dir + "/lib/include/ -m32  -lgdi32 -lopengl32  ";
       // private static string sHiddenArg_Platform = sArch + " -std=c++11 -g -static  " + sMinGwLib64 + sInludeClang   + sInlude64_base + sInlude64_base_2 + sInlude64 + sInlude64_2 + sInlude  + sInludeGZE   + " -I "+ sLibRT_Dir + "/lib/include/  -lgdi32 -lopengl32  ";
        private static string sHiddenArg_Platform = sArch + "  -static-libgcc -static-libstdc++  -g  -lgdi32 -lopengl32  ";

     //   private static string sLinkArg_Platform = " -s -m i386pe -Bdynamic -o \"bin/MainDemo.exe\" -s \"" + sLibRT_Dir + "/lib/crt2.o\" \"-L" + PathHelper.GetExeDirectory()  + "LibRT/lib\" MainDemo_BuildAll.o -lstdc++ -lgcc_s -lgcc -lmoldname -lmingwex -lmingw32 -lmsvcrt -ladvapi32 -lshell32 -luser32 -lkernel32 -lgdi32 -lopengl32 ";
     //   private static string sLinkArg_Platform = " -m i386pe -Bdynamic   \"" + sLibRT_Dir + "/lib/crt2.o\" \"-L" + PathHelper.GetExeDirectory()  + "LibRT/lib\" -lstdc++ -lgcc_s -lgcc -lmoldname -lmingwex -lmingw32 -lmsvcrt -ladvapi32 -lshell32 -luser32 -lkernel32 ";
        private static string sLinkArg_Platform = sArch + "  -static-libgcc -static-libstdc++  -g -fno-rtti  -lgdi32 -lopengl32 ";
*/


     //  public static List<int> ExecutionIdList = new List<int>();



      //public static   Process  oCurrentProcess;

      //  static int unsafeInstanceCount = 0;
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
                Thread.Sleep(1);
            }
            Thread.Sleep(1);
        }

         public static readonly Object oLockTicket = new Object();
         public static volatile uint nTotalTicket = 0;
         public static volatile uint nCurrentTicket = 0;
         public static volatile int nErrorTicket = -1;
        // public static volatile CppCmd oErrorCmd = null;



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
						//if(bFound && Data.oMainForm != null) {	 Data.oMainForm.fAddItem(_sOut);	return;	}
					
					}
				}

				//with default colored
                 string _sResult = Output.Trace(_sOut, true);
              // Debug.fPrint(_sOut); string _sResult =  "";
                if( Data.oMainForm != null) {
                     Data.oMainForm.fAddItem(_sResult);
                }

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


     
        public static void fSend2Compiler(string _sArg, bool _bLinkTime = false, bool _bCompileAndLink = false, CppCmd _oCmd = null) {
           
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

          //  	string _sFinalArg = _sArg.Replace("\\\"", "\"") + " -lgdi32 " ; 
         //      string _sFinalArg = _sArg.Replace("\\\"", "\"") + " -lcurl " ; 
               string _sFinalArg = _sArg.Replace("\\\"", "\"");
                
              //  Output.TraceColored("\f1F (" +   _oCmd.sExecutableName  + ") "  +_sFinalArg);
                _oCmd.sCommandToShow = "\f1F (" +   _oCmd.sExecutableName  + _oCmd.sExecutableType + _sInfo  + ") "  +_sFinalArg;



			   //Debug.fTrace("------------" + _sArg );
			
             //  Debug.fTrace("--Okay--");
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
           // Debug.fTrace("Inc " + safeInstanceCount);

            BackgroundWorker worker = new BackgroundWorker();
            ///	m_WorkersWithData.Add(worker);
            //bw.WorkerReportsProgress = true;
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
						
			

				        if(_bLinkTime){
					        //_sFinalArg =_sFinalArg.Replace("-c ", "");//remove -c command -> bug
				        }
				
                           process.StartInfo.Arguments =_sFinalArg ;
						

                         process.StartInfo.CreateNoWindow = true;
                         process.StartInfo.UseShellExecute = false;
                         process.StartInfo.RedirectStandardOutput = true;
                         process.StartInfo.RedirectStandardError = true;

	                    process.StartInfo.WorkingDirectory =_oCmd.s_pProject; 
	

                   //     List<string> _aOutput = new List<string>() ;//Temp
                  
             
                         try  {

                          //   if (!_bLinkTime) {

                                 process.OutputDataReceived += (sender, e) => {
                                     if (e.Data != null)  {
                                         fCompilerError(e.Data, _sArg, _nMyTicket,false, _oCmd);
                                     }
                                 };
                                 process.ErrorDataReceived += (sender, e) => {
                                     if (e.Data != null)  {
                                         fCompilerError(e.Data, _sArg, _nMyTicket, true, _oCmd);
                                     }
                                 };
                                 /*
                             }  else {

                                 process.OutputDataReceived += (sender, e) =>
                                 {
                                     if (e.Data != null) {
                                         fLinkerError(e.Data);
                                     }
                                 };
                                 process.ErrorDataReceived += (sender, e) =>
                                 {
                                     if (e.Data != null){
                                         fLinkerError(e.Data);
                                     }
                                 };
                             }*/
                            Console.WriteLine("Start " +	 process.StartInfo.FileName );
                              Console.WriteLine("arg " +	     process.StartInfo.Arguments  );
                            ///Debug.fTrace("Start " +	 process.StartInfo.FileName );
                           ///  Debug.fTrace("arg " +	     process.StartInfo.Arguments  );

                             process.Start();
                             process.BeginOutputReadLine();
                             process.BeginErrorReadLine();
                             
             //       Output.TraceColored(process.StandardOutput.ReadToEnd());
            //        Output.TraceColored(process.StandardError.ReadToEnd());
                             process.WaitForExit();

                             ///////////////////////////////////////
                             ///Wait for displaying in order
                              ////////////////////////////////////////*
                               while(Base.bAlive && Data.bNowBuilding ) {
                                    Thread.Sleep(1);
                                    lock(oLockTicket) {
                                  
                                        if(nCurrentTicket == _nMyTicket) {
                                        // Console.WriteLine("*** Process  " + nCurrentTicket + " " + _oCmd == null );



                                       
                                            fShowSendedCmd(_oCmd);

                                           fShowProcOutput(_oCmd);
                                         /*
                                        //    if(_aOutput != null) { //null when it was first
                                           if(aOutput != null && aOutput.Count > 0) {
                                           //     _oCmd.oDepandance.fShowFile();
                                                fShowArg(_oCmd.sCommandToShow, _oCmd.bIsSubCmd);
                                                _oCmd.sCommandToShow = "";
                                                foreach(string _sOut in aOutput) {
                                                      fShowResult(_sOut,_oCmd);
                                 
                                                }
                                                aOutput.Clear();

                                            }
                                            */
                                           // if (nErrorTicket == nCurrentTicket &&  (nError > 0 && GuiForm.fIsChecked("afterFileErrorToolStripMenuItem")) ) {
                                            if (nErrorTicket == nCurrentTicket &&  (nError > 0 ) ) {
                                                Console.WriteLine(":: " +  GuiForm.fIsChecked("afterFileErrorToolStripMenuItem")); //TODO TODOTODO
                                         //   if (nErrorTicket == nCurrentTicket &&  (nError > 0 ) ) {
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



                         } finally   {

                             //          outputWaitHandle.WaitOne(timeout);
                             //      errorWaitHandle.WaitOne(timeout);
                             //    nTotal--;
                         }

                       


                         while (!process.HasExited)
                         {
                             Thread.Sleep(1);

                            if(!Data.bNowBuilding) {
                                 break;
                             }
                         }
                        /*
                         lock(oLockTicket) {
                            nCurrentTicket ++;
                         }*/


                          Interlocked.Decrement(ref safeInstanceCount); //safeInstanceCount never decremented if after  fAddCommandLineVerificationToDepedancesFile?? on link time : exception?

                         if(_oCmd != null && !_bLinkTime) { 
                              _oCmd.fAddCommandLineVerificationToDepedancesFile(_oCmd); //TODO is infinite loop?
                         }


                  
                          // Debug.fTrace("Dec " + safeInstanceCount);
                     }
                 }

             });

            worker.RunWorkerAsync();
        }

		private static string fGetPlatformArgumentComp(string _sPlatform)
		{
			switch(_sPlatform) {
				//			case "CpcDos":
			//	return   "-Wl,--export-all-symbols ";
					
			}
			return "";
		}

		private static string fGetPlatformArgumentLink(string _sPlatform)
		{
			switch(_sPlatform) {
				case "CpcDos":
				return   "-Wl,--export-all-symbols ";


					
			}
			return "";
		}

/*
		private static string fFilterLinkArg(string _sFullArg, bool _bCompileAndLink = false) {

            if(_bCompileAndLink) { //No need to filter when we use standard compiler that do it, only when we use ld direcly
                return _sFullArg;
            }

              string[] separatingChars = {" -" };
           string[] _aArg =  _sFullArg.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries );
            string _sOutput = ""; 
            string _sLib = ""; 


            foreach(string _sArg in _aArg) {
				if(_sArg.Length > 1 && _sArg[1] == ' ' ) {

                switch  (_sArg[0]) {
                    case 'f':
                        //_sResult += " -" + _sArg;
                    break;

                   case 'l':
                       _sLib += " -" + _sArg;
                    break;

                    case 'o': //Output
	//Debug.fTrace("--Ouput " + _sArg);
	//	Debug.fTrace("-*********ASSSSAAAAAAAAAAAAAAAAAAASAAASSSSSSSSSSSSSSSSSSSS " + _sArg);
                       _sOutput += " -" + _sArg;

                    break;
                }

				}
				

            }
            return _sOutput + _sLib;
        }*/

        private static readonly Object oLock = new Object();
        private static readonly Object oLockFirst = new Object();
           private static volatile bool bFirstAvalaible  = true;


        
        public static bool fManageMessages( CppCmd _oCmd, string _sMsg) {
            //CWift messages
    //

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
                                              // _oCmd.sObjCompiled += _aMsg[1] + ",";
                                            return true;
                                
                                        case "C~:UpToDate":
                                        case "CwUpToDate":
                                        case "CwUp2Date":
                            
                                             // Output.TraceColored("\f27CwUpToDate: \f28" + _sMessage.Substring(_sMessage.IndexOf(':')+1) );
                                              Output.TraceColored("\f27C~:Up2Date|\f28" +_aMsg[1] );
                                              // _oCmd.sObjUpToDate += _aMsg[1] + ",";
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

                    //            ModuleData _oLib = new ModuleData(_oMyModule, "src/SubLib_3rdparty/", "GzNima");
                   ModuleData _oLib = new ModuleData(_oMyModule, _sPath, _sName);
                  _oMyModule.fAddSubLib(_oLib);
                 _oMyModule.fGetSubLibCompilerData( Data.oArg );//Data.oArg  not sure



                Output.TraceAction("Add SubLib: " + _sName);

               //  Output.TraceError("_sParentModule!! " + _sParentModule);
              //   Output.TraceError("_sPath!! " + _sPath);
               //  Output.TraceError("_sName!! " + _sName);

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
              //  Console.WriteLine(" aOutput.Count "  +  aOutput.Count);
                if(aOutput != null && aOutput.Count > 0) {
                    //   _oCmd.oDepandance.fShowFile();
                    foreach(OutToken _oOut in aOutput) { //Show all precedant commands
                     //    Console.WriteLine(" _oOut.sOut "  + _oOut.sOut);
                        if(_oCmd == _oOut.oFrom && _oOut.bShowed == false){
                              _oOut.bShowed = true;
                             fShowProcOutputString(_oOut);
                        }
                         
                    }
                    //   _aOutput = null;
                //    aOutput.Clear();
                }

            //    _oCmd.oDepandance.fShowFile();
           
        }



        public static void fCompilerError(string _sResult,string _sArg, uint _nMyTicket, bool _bStdError = false,  CppCmd _oCmd = null) {

            //Direct show if current
            lock (oLockTicket) {
                
              //  if(!(Base.bAlive && Data.bNowBuilding)) {
                if(!(Base.bAlive && Data.bNowBuilding)) {
                    return;
                }

                if(nCurrentTicket == _nMyTicket) { //Direct show to not wait ending compilation
                    fShowProcOutput(_oCmd);
                }
                 

              // if(aOutput != null && nError == 0) {
          //  bool _bInError = false;
			    if(_bStdError) {

				    if( fFindValidKeyWord(_sResult, "error") != -1 ) { //Generiquer error check!?
					    nError++;
                    //    _bInError = true;
                        
                        if(_nMyTicket < nErrorTicket  || nErrorTicket ==-1){
                            nErrorTicket = (int) _nMyTicket;
                        }
                        /*
                        if (nCurrentTicket > nErrorTicket) { //Just to be safe, why required (okay reset ErrorTicket)?
                            Console.WriteLine("_nMyTicket " + _nMyTicket);
                            Console.WriteLine("nCurrentTicket " + nCurrentTicket);
                            Console.WriteLine("nErrorTicket " + nErrorTicket);
                            nErrorTicket = nCurrentTicket;
                        }*/


				    }
				
				    /*
				    if(!_oCmd.bToStaticLib) { ///Temp bug with ar.exe with false positive
					    nError++;
				    }*/

			    }
               
                if(aOutput != null ) {

                      OutToken _oOut = new OutToken();
                    _oOut.oFrom = _oCmd;
                    _oOut.sOut = _sResult;
           
                     if(_sResult.IndexOf("undefined reference to") != -1) {
                        _oOut.eType = OutType.Undefined;
                    }
                     
                     if( fFindValidKeyWord(_sResult, "error") != -1  ) {
                        _oOut.eType = OutType.Error;
                    }
                     if(  fFindValidKeyWord(_sResult, "warning") != -1) {
                        _oOut.eType = OutType.Warning;
                    }


                //    _oOut.bInError = _bInError;
                    aOutput.Add(_oOut);
                }

/*
            int nErrorIndex = -1;
            if(nErrorIndex == -1) {nErrorIndex = _sResult.IndexOf("error:"); }
            if(nErrorIndex == -1) {nErrorIndex = _sResult.IndexOf("undefined:"); }
            if(nErrorIndex != -1) {
                //bHasError = true; //waring?
                nError++;
            }
*/

           //  lock (oLock){

            //    if(_aOutput != null) {
             

              }
        
          //  }
            //	try{Context.oSingleton.oPluginMain.pluginUI.BeginInvoke((MethodInvoker)delegate { try {
          //     _aOutput.Add(_sResult);


            /*
            lock (oLock){
                Output.Trace( _sResult, true);
            }*/


            //    }catch{}});}catch {}
        
        }





        public static void fLinkerError(string _sLinkError)
        {
            //    try{Context.oSingleton.oPluginMain.pluginUI.BeginInvoke((MethodInvoker)delegate { try {

            if (_sLinkError.Length > 7 && _sLinkError.Substring(0, 7) == "warning") {
                //It only a warning
            }else {
                bHasError = true;
            }

            //I:/ FlashDev / _MyProject / Simacode / LDK / LinxDemo / _out / _MainDemo / Windows / Debug / obj / Lib_Demo / Screen / DemoText.o:(.rdata + 0x60): undefined reference to `Lib_GZ::Gfx::cDispacher::fClearChild(Lib_GZ::Gfx::cRoot *)'

            //I:/ FlashDev / _MyProject / Simacode / LDK / LinxDemo / _out / _MainDemo / _cpp / _Lib / GZE / Lib_GZ / Gfx / Dispacher.cpp:43:5: error: expected ';' after expression

            Output.Trace(_sLinkError, true);


        }


        public static int fFindValidKeyWord(string _sLine, string _sKey) {
            if(_sLine.Length <= 1) {return -1; }
            int _nIndex = 0;
           int _nNext = 0;
            while (_nIndex != -1) {
               
                _nIndex =  _sLine.IndexOf(_sKey, _nIndex+1);

                if(_nIndex != -1) {
                    _nNext = _nIndex + _sKey.Length;
                    if(_nNext < _sLine.Length &&  _sLine[_nNext]  != '.') { //like error.o -`>  not valid
                        return _nIndex;
                    }
                }

            }

    
            return _nIndex;
        }

 



    }
}
