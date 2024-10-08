﻿using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static cwc.DBGpClient;

namespace cwc {
    public class ArgumentManager {


         public static List<CompilerData> aAllCompilerList= new List<CompilerData>();

       //public  List<SrcDiry> aPrjDirectory = new List<SrcDiry>(); 

		  public Dictionary<string, SrcDiry> aPrjDirectory = new Dictionary<string, SrcDiry>();
		  public Dictionary<string, SrcDiry> aPrjDirectoryOutput = new Dictionary<string, SrcDiry>();


        public  List<ModuleData> aLib = new List<ModuleData>();

		 public  List<LaunchTool> aExeWaitingList = new List<LaunchTool>();
		public string sPlatform = "Tes";

        public  List<CppSeq> aCppSeq = new List<CppSeq>();
        public  List<CompilerData> aCompilerList= new List<CompilerData>();
        public  List<CompilerData> aLibList= new List<CompilerData>();


        public  List<CppCmd> aLinkCmdList = new List<CppCmd>();

      //  public  List<ModuleData> aRequiredModuleList = new List<ModuleData>();//+ sInludeGZE + sIncludeGzeSubLibs


        public  List<string> aAllInclude = new List<string>();

        public  List<CppCmd> aPrecObjOutput = new List<CppCmd>();
        public  string sPrecObjOutput = "";
        public  string sCurr_wTo = "";


       public  bool bSubArgMan = false;
       public  bool bFinalised = false;
      
		public string sAllArg = "";
        private static Stopwatch wBuildTime;
        ArgumentManager oParent;

        public ArgumentManager(ArgumentManager _oParent = null) {

            Data.aAll_ArgumentManager.Add(this);
            oParent = _oParent;
            if(_oParent != null) {
                foreach(ModuleData _oLib in oParent.aLib ) {
                     aLib.Add(_oLib);
                }
            }

        }


         public  bool IsEmpty(string _sText){
              return String.IsNullOrEmpty(_sText) || _sText.Trim().Length == 0;
        }

         public CppSeq oCurCppSeq;
        //public bool bHaveCompileWithoutLink = false;


		public  CompilerData fAddCompiler(string _sName, string _sType = "", bool _bIsSubCompiler = false){

			
			CompilerData _oCompiler = Finder.fGetCompiler(_sName, _sType, _bIsSubCompiler);
			if(_oCompiler != null){
				aCompilerList.Add(_oCompiler 	); //Default compiler
                bool _bAlreadyExist = false;
                ///////////TODO:  Mabe only a global list was required ?!
                foreach(CompilerData _oData in aAllCompilerList) {
                    if(_oData == _oCompiler) {
                        _bAlreadyExist = true;
                        }
                }
                if(!_bAlreadyExist) {
                     aAllCompilerList.Add(_oCompiler);//TODO don't duplicate
                }
                //////////////////

			}else{
				//Output.TraceError("XXX not exist: " + _sName + "  "  +  _sPlatform  );
				Debug.fTrace("XXX not exist: " + _sName + "  "  +  _sType  );
			}
            return _oCompiler;
		}

		internal void fAddLib(ModuleData _oLib){
            if(_oLib != null) {

                if(_oLib.oLibData != null) {
			      aLibList.Add(_oLib.oLibData);
                }

                foreach (ModuleData _oIsLib in aLib)
                {
                    if (_oLib == _oIsLib)
                    {
                        return;
                    }
                }
                aLib.Add(_oLib);
                

                //To test
                foreach (ModuleData _oSubLib in _oLib.aSubLib){
                    aLib.Add(_oSubLib);
                    if(_oSubLib.oLibData != null) {
			          aLibList.Add(_oSubLib.oLibData);
                    }
                }


            }
		}

      





        public  void ExtractMainArgument(string _sAllArg, bool bPreExtract = false){

			//	sPlatform = Data.sPlatform;
//				fAddCompiler(fGetVar("_wToolchain"),  fGetVar("_sPlatform"));
		

			
				sAllArg = _sAllArg;

     //       Debug.fTrace("_sAllArg: " + _sAllArg);

             _sAllArg =  _sAllArg.Replace("=>","=¦");//Bug if we have =>

              string[] _aSequenceArg = _sAllArg.Split('>'); //Squential
              foreach (string __sSeqArg in _aSequenceArg) { if (!IsEmpty(__sSeqArg)) {

                    string _sSeqArg = __sSeqArg.Replace('¦','>').Trim();//Recover Bug if we have =>
                                      //          Console.WriteLine("_sSeqArg " +_sSeqArg);

					aPrecObjOutput = new List<CppCmd>();
					sPrecObjOutput = "";
					
                    oCurCppSeq = new CppSeq(this, _sSeqArg); //PreExtract
                    aCppSeq.Add(oCurCppSeq);


                   string[] _aArg = _sSeqArg.Split('|'); //Simultanous
                   foreach (string _sArg in _aArg) { if (!IsEmpty(_sArg)) {
                       //     Console.WriteLine("ExtractCommandLine " +_sArg);
                        ExtractCommandLine(_sArg);
                  }}

              }}
			
			

			if(bPreExtract || Data.bUpdateMode || Data.bNothingToBuild) {
				return;
			}

		//	fCompleteExtractMainArgument();

        }
		
		public  void fCompleteExtractMainArgument(ModuleData _oModule = null, bool _bShowInfo = true){
			Build.fEnableBuild();


			///Check missing modules lib 
			//if(Data.aCompilerData.Count == 0){
				

			//}

	//		fLibRtExist();
			//fGZE_Exist();

            if(Data.bModuleIsRequired || Data.bDontExecute) {//Stop if we need more modules
                Build.fDisableBuild();
				return;
			} 
	 
        
		    if(_bShowInfo) {
				if(_oModule == null){
				   Output.Trace("\f2A--Start Build--");
				}else{
					// Output.Trace("\f27 - Build " + _oModule.sAutorName + " - " + _oModule.sCurrFolder + " - " );
					 Output.Trace("\f2B - Build " + _oModule.sAutorName + " - " + _oModule.sCurrFolder + " - " );
				}
              }

			///////////////// Extract Compilers
			fExtractCompiler();

            if(Data.bModuleIsRequired){ //For compiler with "Require" Node //TODO recursive
                if(!Empty.fLoadModules(false)) {
                    Build.fDisableBuild();
				    return;
			    } 

                 Data.bModuleIsRequired = false;
				 Data.aRequiredModule.Clear();

              //  Empty.fLoadModules(false);//Reload after download, or look for bExtracting before fGetLastVersions
                fMergeCompiler();
            }
            fExtractCompiler();
      
         


			foreach(CompilerData _oLib in  aLibList) { //TODO separate subcompiler and extract after!?
					//Console.WriteLine("Extract Lib: " + _oLib.sFullName);
			    Debug.fTrace("Extract Lib: " + _oLib.sFullName);
				_oLib.fExtract(this);

			}


			/////////////////////////////
            //Output.Trace("\f2A-Extracted--");
		/*
			 foreach(CppSeq _oSeq in aCppSeq) {
					sCurr_wTo = "";
					foreach(CppCmd _oCmd in _oSeq.aCppCmd) {
						//	_oCmd.fExtract();
					}
				}
                */
		

              ArgProcess.fFinishExtractArg();


		}




	
		private void fExtractCompiler()	{
					//Debug.fTrace(" Extract Compilers: " );
          CompilerData[] _aCompilerList =  aCompilerList.ToArray(); //Copy beacause collection may be modified in itiration
			foreach(CompilerData _oCompiler in  _aCompilerList) { //TODO separate subcompiler and extract after!?
					//Debug.fTrace("have: " + _oCompiler.sFullName);
				_oCompiler.fExtract(this);
			}
		}
	
        private void fMergeCompiler()	{

		      CompilerData[] _aCompilerList =  aCompilerList.ToArray(); //Copy beacause collection may be modified in itiration
			foreach(CompilerData _oCompiler in  _aCompilerList) { //TODO separate subcompiler and extract after!?
					//Debug.fTrace("have: " + _oCompiler.sFullName);
				_oCompiler.fMerge(this);
			}
		}



        
		private void fDeletOutput() {
           foreach(CppCmd _oCmd in aLinkCmdList ) {
                string _sPath = _oCmd.sOutputFile;
                try {
                 if(File.Exists(_sPath)) {   
                    File.Delete(_sPath);
                }
                } catch(Exception ex) { }
               //  break; //Temp, only one
            }
        }


        /* 
       // foreach (string _sSub in _aSub) { if (!IsEmpty(_sSub)) {  //Get first valid  
       break;
        }}
        */

        public void ExtractCommandLine(string _sCmd){
            CppCmd _oCppCmd = new CppCmd(this, _sCmd);
			
            oCurCppSeq.aCppCmd.Add(_oCppCmd);
        }




        internal void fExtract(ModuleData _oModule = null){
            //Not work, TODO send to compiler Extract cmd (CWAVE then compile)
            /*
             foreach(CppSeq _oSeq in aCppSeq) {
                foreach(CppCmd _oCmd in _oSeq.aCppCmd) {
						sCurr_wTo = "";
						_oCmd.fExtract();
					//	 _oCmd.fExecute();
						
                        if(!Data.bNowBuilding) {
                            return;
                        }
						if( CppCompiler.nError > 0) {
							break;
						}						
                }
            }*/
        }



		internal void fRun(ModuleData _oModule = null, bool _bDontExecute = false, bool _bShowInfo = true, bool _bSilent = false){ 
              bFinalised = false;
             if(!_bDontExecute){
			     aExeWaitingList = new List<LaunchTool>();
			     fDeletOutput();
         

			     wBuildTime = new Stopwatch();
                 wBuildTime.Start();


            //     if(Data.oMainForm != null) {
                     Data.oLaunchProject.fSetOutput(aLinkCmdList,sCurr_wTo);
            //    }

                if(!Data.bNowBuilding) {
                    return;
                }
					
				    //TODO test if we really use libt

//			    Output.Trace("\f9B>> \f97 " + sAllArg);
		    }

             int i = 0;
            foreach(CppSeq _oSeq in aCppSeq) {
                i++;

                if(!_bSilent) {
                    Output.Trace("\f1B> \f13" + _oSeq.sSeq);
                 //   Output.Trace("\f1B> \f13 " + CppCmd.fExtractVar( _oSeq.sSeq,null) ); //Todo preextract var?

                }
				sCurr_wTo = "";

                
                foreach(CppCmd _oCmd in _oSeq.aCppCmd) {
						sCurr_wTo = "";
						_oCmd.fExtract();
					//	 _oCmd.fExecute();
						
                        if(!Data.bNowBuilding) {
                            return;
                        }
						if( CppCompiler.nError > 0) {
							break;
						}			
                        			
                }
                
                if( i == aCppSeq.Count) {//Only last one?
                   fFinalize();
                }

                if(!_bDontExecute){
				     CppCompiler.CheckAllThreadsHaveFinishedWorking(true);
				    foreach(CppCmd _oCmd in _oSeq.aCppCmd) {
						     _oCmd.fExecute();
						
                            if(!Data.bNowBuilding) {
                               fShowInfo(_oModule,!_bSilent);
                                return;
                            }
						    if( CppCompiler.nError > 0) {
							    break;
						    }
                       // Thread.Sleep(1);				
                    }
              
			        //  if(Data.oMainForm != null) { //TODO better way?
		        //			 Data.oLaunchProject.fSetOutput(aLinkCmdList,sCurr_wTo);
			        //	}
	
                    CppCompiler.CheckAllThreadsHaveFinishedWorking(true);
				    fWaitForWaitingList();
               

				   
                   foreach(CppCmd _oCmd in _oSeq.aCppCmd) {
                          _oCmd.fFinish();
                              if(!Data.bNowBuilding) {
                                 fShowInfo(_oModule,!_bSilent);
                                return;
                            }
						    if( CppCompiler.nError > 0) {
							    break;
						    }	
                    }

                    Data.oLaunchProject.fSetOutput(aLinkCmdList,sCurr_wTo);

                    if(!Data.bNowBuilding) {
                        fShowInfo(_oModule,!_bSilent);
                        return;
                    }
				    if( CppCompiler.nError > 0) {
					    break;
				    }	
                }

            }

            if(_bDontExecute){
                fShowInfo(_oModule,!_bSilent);
                return;
            }

	///Finalize compiler commands
	///
 
		
            

		 fShowInfo(_oModule, _bShowInfo);

	}

        public void fFinalize(){ //TODO is finallise is essential?!
            if(oParent == null ) { //Only for master cmMake
              //   bFinalised = true;
                if( CppCompiler.nError == 0) {
				    foreach(CompilerData _oCompiler in  aAllCompilerList) { //TODO separate subcompiler and extract after!?
				
					    _oCompiler.fFinalize();

                          
                        
				    }
                  }
		    }
        }


        public void fShowInfo(ModuleData _oModule, bool _bShowInfo)	{
            if(wBuildTime == null) {
                 Output.Trace("\f1B --- End --- \f13 " );
                return;
             }

             wBuildTime.Stop();
           double nfSec = wBuildTime.ElapsedMilliseconds / 1000.0;
           int _nSeconde = (int)(nfSec); int _nDotSeconde = ((int)(nfSec * 100.0)) - _nSeconde * 100;
     


       //     if(Data.bInConsole && !bSubArgMan) {
		 //   if( !bSubArgMan) {
             if(_bShowInfo){
				if( CppCompiler.nError > 0) {
					string _sS = "s";
					if(CppCompiler.nError == 1) {
						_sS = "";
					}

                    string _sWhat = "\f4C --- End with ";
                    if(_oModule != null){
                        _sWhat = "\f4C --- Finish - " + _oModule.sAutorName +" - with " ;
                    }


				    Output.TraceColored(_sWhat +  CppCompiler.nError.ToString()   + " error" + _sS + " --- " +   _nSeconde + "." + _nDotSeconde + " sec" );

				}else {
					if(_oModule != null){
						//  Output.Trace("\f27 --- Finish - " + _oModule.sAutorName +" - \f27 " +   _nSeconde + "." + _nDotSeconde + " sec" );
						  Output.Trace("\f2B --- Finish - " + _oModule.sAutorName +" - \f2B " +   _nSeconde + "." + _nDotSeconde + " sec" );
					}else{
						 Output.Trace("\f1B --- End --- \f13 " +   _nSeconde + "." + _nDotSeconde + " sec" );
					}
				}
               // Data.WriteBash();
                //  }
            }

           
        }



		public void fCleanAllCorruptObj()	{
		  foreach(CppSeq _oSeq in aCppSeq) {
                foreach(CppCmd _oCmd in _oSeq.aCppCmd) {
                     _oCmd.fCleanCorruptObj();  
                }
            }
		}


		
		public void fWaitForWaitingList()	{
			foreach(LaunchTool _oExe in aExeWaitingList) {
				while(_oExe.bExeLaunch && Base.bAlive) {
					//Thread.Sleep(1);
                    Thread.CurrentThread.Join(1);  
				}
			}
			aExeWaitingList = new List<LaunchTool>();
		}

        
		public static void fSetVar(ArgumentManager _oArg, string _sCmd, string _sMainValue, bool _bAssingOnEmpty = false){
            _oArg.fSetVar(_sCmd,_sMainValue, _bAssingOnEmpty );

        }

      
		public void fSetVar(string _sCmd, string _sMainValue, bool _bAssingOnEmpty = false)
        {
			//special case
			if(_sCmd == "_sConfig_Type"){
				if(_sMainValue == ""){
					_sMainValue = "Default";	
				}
			}
			Data.fSetGlobalVar(_sCmd, _sMainValue, _bAssingOnEmpty);
		}

      
		public string fGetVar(string _sVar, bool _bWeak = false){
            /*
            if(_sVar=="_sPlatform") { //Return last (#Run bugfix)
                if (Data.fGetGlobalVar(_sVar,_bWeak) == "") {
                    return "ass";
                }
            }*/

          //  Console.WriteLine("------fGetVar " + _sVar );
			return Data.fGetGlobalVar(_sVar,_bWeak);
		}

		internal void fAddPrjDirectory(SrcDiry _oNewDir){
			if (!aPrjDirectory.ContainsKey( _oNewDir.sFile)){
				aPrjDirectory.Add(_oNewDir.sFile, _oNewDir);
		    	 //Debug.fTrace("Add src dir !!!! "  + _oNewDir.sFile);
			}
		}
		internal void fAddPrjDirectoryOutput(SrcDiry _oNewDir){
			if (!aPrjDirectoryOutput.ContainsKey( _oNewDir.sFile)){
				aPrjDirectoryOutput.Add(_oNewDir.sFile, _oNewDir);
				 //Debug.fTrace("Add out dir !!!! "  + _oNewDir.sFile);
			}
		}

        internal void fAddInclude(CppCmd _oCppCmd, string _sPath) {
            

           _sPath = PathHelper.fNormalizeFolderAndRel(_sPath);
            /*
            _sPath = _sPath.Replace('\\', '/').Replace('\"', ' ').Trim();
			if(_sPath[0] == '\''){
				_sPath =_sPath.Substring(1);
			}
			if(_sPath[_sPath.Length-1] == '\''){
				_sPath = _sPath.Substring(0,_sPath.Length-1);
			}*/

            _oCppCmd.aInclude.Add(_sPath);
           if(aAllInclude.Contains(_sPath)) {
                return;
            }
            aAllInclude.Add(_sPath);


            if(!Data.aAllInclude.Contains(_sPath)) {
                Data.aAllInclude.Add(_sPath);
            }
       

        }
        /*
        internal void fAddDepandances(string _sText) {
           
            string[] aReadLine = _sText.Replace('\\','/').Split(new[]{ " /" }, StringSplitOptions.None);
            foreach (string _sLine in aReadLine) {

             //   Console.WriteLine("Dep:" + _sLine.Trim());
                    
            }

        }
        */

        static  public Dictionary<string, DepandanceData> aDependance = new Dictionary<string, DepandanceData>();
        internal bool bPreOutput_Link =false;
        internal bool bOverideRunCmd =false;

        internal bool fAddDepandance(Depandance _oFrom, string _sFile) { //Return true if newer file

          //  Console.WriteLine("Check: " +_sFile );
            if(!File.Exists(_sFile)) {
              //  _oFrom.bHaveNewerFile = true;//Not exist, force recompilation
                _oFrom.fHaveNewerFile();//Not exist, force recompilation
                //Console.WriteLine("!File.Exists(_sFile): " +_sFile );
                return true; 
            }
           
             DepandanceData _oDep;

            if (!aDependance.ContainsKey(_sFile)) {
                _oDep = new DepandanceData();
                _oDep.sPath = _sFile;
            //    Console.WriteLine("Add: " + _sFile);
                aDependance.Add(_sFile, _oDep);
            } else {
                _oDep = aDependance[_sFile];
            }

            if (_oFrom.fIsNewer(_oDep)) {
                return true;
            }
      
            return false;
        }

        internal bool fHasInclude(string sHasIncluded) {
           sHasIncluded = sHasIncluded.Replace('\\', '/').Trim();

           if (aDependance.ContainsKey(sHasIncluded)) {
                  return true;
            }
           return false;
        }
    }
}


