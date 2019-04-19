

using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cwc {



    public class SrcDiry {

       public  string sFile = "";
       public  string sCondition = "";
		public SrcDiry(string _sFile, string _sCondition) {
			sFile = _sFile;
			sCondition = _sCondition;
		}
	}



    public class CppCmd {

       public  List<CppCmd> aSubCmd = new List<CppCmd>();

		public CompilerData oCompiler = null;

       public string sCmd = "";

	  public string sRecompileOnChangeCmd = "";

        public bool bHaveCW  = false;
   public bool bHaveSourceC  = false;
   public bool bForceCpp  = false;
       public string sSubCmd;
       public string sBackEndCmd;
       public string sGenBackEndCmd = "";
       public int nLastBackEndIndex = 0;

		 public string sAllDefine = "";

       public bool bToStaticLib = false;
       public bool bToDynamicLib = false;

       public bool bHave_wTo = false;
       public string sFile_wTo = "";
       public string s_wDir = "";
       public string sPrecOutput_wTo = "";


       public bool bIsGit = false;
      public string sGitCmd = "";
      public string sGitURL = "";


       public string sCompiler = "";
       public string sPlatform = "";



    
       public string sManuallySetExecutable = "";

       public string sExecutable = "";
       public string sExecutableName = "";
       public string sExecutableType = "";
        
       public string sArgument = "";
       public string sArgLinkerLib = "";
       public string sExtractedCmd = "";

        

       public string sResidualArg = "";

       public string sExeCmdUnique = "";

		public string sOutDirectory = "";

       public bool bLink = false;
       public bool bBuildWithoutLink = false;
       public bool bHaveKnowSourcesFiles = false;
       public bool bHaveKnowObjectFiles = false;
       public bool bHaveUnknowFiles = false;

       public bool bHaveDirectorySource = false;
       public bool bHaveEntrySource = false;

        public string sCommandToShow = "";

       public  Depandance oDepandance;
       public string sOutputFile = "";
       bool bSkip = false;
       public bool bIsSubCmd = false;

		 string[] aPreArg;




       public  List<string> aInclude = new List<string>();
       public  List<string> aCompileFiles = new List<string>();
       public  List<SrcDiry> aDirectory = new List<SrcDiry>();

		
	//  public  bool bCompileAndLink = false;  
      public ArgumentManager oParent = null;


		
		public CppCmd oParentCmd = null;
		
       public CppCmd (ArgumentManager _oParent, string _sCmd, CppCmd _oParentCmd = null) {
			oParentCmd = _oParentCmd;
            oParent = _oParent;

			if(oParentCmd != null){
				bIsSubCmd = true;
/*
				if(_sCmd == oParentCmd.sCmd){ //If is empty don't execute
					return;
				}*/
			}

			if(_sCmd != "" ){
				_sCmd = _sCmd.Replace('\n',' ' );
				_sCmd = _sCmd.Trim();
				sCmd  = fExtractVar( _sCmd);
		
           //     Debug.fTrace("_sCmd " + _sCmd);
			
				fPreExtract();
			}
		}




		public void fPreExtract() {
		    bool _bCompiling = false;



    //        aPreArg  = _sCmd.Split(new string[] { " -" }, StringSplitOptions.None); 
	//		foreach (string _sArg in aPreArg) { if (!FileUtils.IsEmpty(_sArg)) {

            int _nNextIdex = 0;

 

            string _sFullRArg = " " + sCmd;
         
            _nNextIdex = _sFullRArg.IndexOf(" -");
          
            while(_nNextIdex != -1){

             	 _sFullRArg =_sFullRArg.Substring(_nNextIdex+2).TrimStart();
         
                 _nNextIdex = _sFullRArg.IndexOf(" -");

                string _sRArg = _sFullRArg;
                if(_nNextIdex != -1){
                   _sRArg = _sFullRArg.Substring(0,_nNextIdex).TrimEnd();
                }

              //  Console.WriteLine("_sRArg!"  +  _sRArg );


                if (bRunToArgDontPrextract) {
                    //Save following arg
            //        sRunToArgDontPrextract_Arg += " -" + _sRArg;
             //       continue;
                     sRunToArgDontPrextract_Arg = " -" + _sFullRArg;
                     return;
                }




				//Debug.fTrace("##--------------:" + _sArg );
			//	string _sRArg =_sArg.Trim();
			//	 string[] _aCmd = _sRArg.Split(' ', '=', '+'); 
				//  string _sCmdName = _aCmd[0].Trim();



				// switch(_sCmdName[0]) {
				 switch(_sFullRArg[0]) {
                            case '{':
                                 fPre_SetCwcVar( _sRArg);
                    	    break;
                            case '#':
                             //   fPreCwcCommand( _sRArg,_sFullRArg);
                                fPreCwcCommand( _sRArg,_sFullRArg); //Require full
							break;
                    /*
                          case '#':
                                fPreCwcDirective(_sCmdName, _sRArg);
							break;*/
							case 'v'://		
							//	 fSetVarCmd(_sCmdName, _sRArg);
							break;

                            case 'e':
                          
                                bHaveEntrySource = true;
                                _bCompiling = true;
                                fPreGetCompilationType(_sRArg); 
                        	break;

							case 'c': //Must have a compiler -> use Default backend compiler
								_bCompiling = true;
                                //get compilation type
                                fPreGetCompilationType(_sRArg);
                                
							break;
							case 'i':
							case 'o':
								
								if(Data.bCheckLibRTRequired){
									fwToolchain("VLianceTool/LibRT"); //Default compiler
								}
								fPreOutputCmd(_sFullRArg,_bCompiling);
							break;
						default:
							bCallCompiler = true; //Non Cwc Command /unknow -> call the compiler
						break;
			
                       }
			}
		}

        public string sCompileExtention = "";
        private void fPreGetCompilationType(string sRArg) {
         
            if (sRArg[sRArg.Length-1] == '/') { //It's a folder, it will be processed in subcommands
                return;
            }
            int _nDotIndex = sRArg.LastIndexOf(".");
            if (_nDotIndex>= 0) {
               sCompileExtention = sRArg.Substring(_nDotIndex + 1).ToLower();
            }

            Debug.fTrace("sCompileExtention : " +sCompileExtention );
        

        }

        public bool bCallCompiler = false;
		

		
		public void fExtractLib(){


                  oLauchLib_Arg.fSetVar("wPath",  oLib.sCurrFolder);  		
                  oLauchLib_Arg.fSetVar("wExPath", sLauchLib_wExPath);

      
                  oLauchLib_Arg.aLib = oParent.aLib;

              if(oLib.oLibData.sCmd != "") {

                  //      Console.WriteLine("***********************************aaa " + oLib.oLibData.sCmd );
               	     fNewArgCmdRun( oLib.oLibData.sCmd , false,oLauchLib_Arg,false); //Not run
			
       
  
                   fRunLib();
        //          Console.WriteLine("havec commmand!! " + oLib.oLibData.sCmd);
             
            }
		}

        		
		public void fRunLib(){
            if(bIsRunLib){
        //        bIsRunLib = false;
               ///            Output.TraceWarning("#Run "  + sLauchLib_wExPath );

                oLauchLib_Arg.fCompleteExtractMainArgument(oLib);
                oLauchLib_Arg.fExtract(oLib);
                oLauchLib_Arg.fRun(oLib);
        //         Output.TraceWarning("#End "  + sLauchLib_wExPath );
            }
        }



		public ArgumentManager oLauchLib_Arg = null;
		public string sLauchLib_File = "";
		public string sLauchLib_wExPath = "";
        public bool bIsRunLib = false;

        private void fPreBuildCmd( string _sArg) {

            string _sVar = fExtractVar(_sArg);
         //   Console.WriteLine("Extract! : "  + _sVar);

           fwLib(_sVar);
          //  bIsRunLib = true;
      
            
           //  fExtractArg(_sArg)
                /*
            bIsACmdLib = true;
		    oLib =	Data.fAddRequiredModule(_aArg[0] + "/" + _aArg[1]);
			oParent.fAddLib(oLib); //Todo multiple lib with separator ,
            oLauchLib_Arg =  new ArgumentManager();


            //extract varz
            _sArg
            */
           // sLauchLib_wExPath =  fGetVar("wDir") + _sArg;
            
        }



		public void fPreOutputLib(string _sArg){

			sLauchLib_wExPath =  fGetVar("wDir") + _sArg;
		//	sLauchLib_File = oLib.sCurrFolder + "wLib.cwc";
         //  if(){
             bIsRunLib = true;
         //   }
		

		}
		

		public void fPreOutputCmd(string _sArg, bool _bCompiling = false, bool _bwto = false){
			
		

			bCallCompiler = true;
			if(!_bCompiling){   ////TODO better detection
				_sArg= _sArg.Trim();
				string _sExtention = "";
				int _nIndex = _sArg.IndexOf('.');
				if(_nIndex != -1){
					_sExtention = _sArg.Substring(_nIndex+1);
				}
				_nIndex = _sExtention.IndexOf(' ');
				if(_nIndex != -1){
					_sExtention = _sExtention.Substring(0,_nIndex);
				}

			//	if(!_bwto){
					  //onsole.WriteLine("Link TIME!!! " + _sExtention);
						bLink = true;
				

					switch(_sExtention.ToLower()){

						case "o": //Output to .o will never be a Link time (protection)
						case "obj":
						//	bLink = false;
						break;
					
						case "":
						case "*":
						break;

						case "dll":
							bToDynamicLib = true;
						break;

						case "a":
							bToStaticLib = true;
						break;
					}
				}
			//}
		}



		public void fExtractArg(string _sArg) {
			
		}

public string sDelimiter = "";

    

        public string fAddArgBackEnd( string _sAdd) {
	    	//nLastBackEndIndex = sBackEndCmd.Length;
		    return sDelimiter + _sAdd;
  
            ///Debug.fTrace("sBackEndCmd " +  sDelimiter + _sArg );
	}


          public string fExtractManualSetExecutable(string _sCmd) {
                 /////// Special case if we define exe manually //////
            if (_sCmd.Length > 2 && _sCmd[0] == '(') {
                int _nEndIndex = _sCmd.IndexOf(')');
                if(_nEndIndex > 0){
                 string _sExeName = _sCmd.Substring(1, _nEndIndex-1);
                    sManuallySetExecutable = _sExeName;
                    return _sCmd.Substring(_nEndIndex+1).TrimStart();
                  //  Debug.fTrace("*****Found " + _sExeName);
                }
            }
            return _sCmd;
        }
            /////////////////////



        public string fExtractValidCompilerCommand(string _sCmd) {

   

            _sCmd = " " + _sCmd;//Be sure to remove first " -"
				 string[]  _aArg  = _sCmd.Split(new string[] { " -" }, StringSplitOptions.None); 


            string _sTempCompileCommand = "";
            string _sPrecArg = "";
            string _sBackEndCmd = "";
    

            ///Check for existing directory
              if (!FileUtils.IsEmpty(_aArg[0])) {
                _sPrecArg = _aArg[0].Trim();
            }


             sDelimiter = "";

            string _sCmdTest = _sCmd.TrimStart();
            if(_sCmdTest.Length == 0){ return "";}

            if(_sCmdTest[0] == '-') {
                sDelimiter = " -"; // begin with "-"
            }

            foreach (string _sArg in _aArg) { if (!FileUtils.IsEmpty(_sArg)) { 
              
				string _sRArg = _sArg.Trim();

                string[] _aCmd = _sRArg.Split(' ', '=', '+'); 
                string _sCmdName = _aCmd[0].Trim();
                bool _bSkipCmd = false;
                    /*
                if(!Data.bNowBuilding) {
                    return;
                }*/
                
                 ////////////// C~ command  ///////////
               if (!FileUtils.IsEmpty(_sCmdName)) { 

						switch(_sCmdName[0]) {
                                case 'L': //For CWayv
								case 'I':
									fIncludePath(fExtractOneCharCmd(_sRArg));
								break;

								case 'o':
										_bSkipCmd = fOutputCmd(fExtractOneCharCmd(_sRArg), _sPrecArg);
                                        if(!_bSkipCmd){
                                            _bSkipCmd = true;
                                            _sBackEndCmd += _sTempCompileCommand + fAddArgBackEnd(_sRArg);
                                        }
                                        _sTempCompileCommand = "";
								break;
                        
                                case '#':
                                    fCwcCommand(_sCmdName, _sRArg);
                                    _bSkipCmd = true;
                                    if( _sCmdName == "#To"){
                                        _sBackEndCmd += "#To"; //Keep track of it to remplace later by generated command 
                                    }
                            
                                 break;
                       }

						switch(_sCmdName) {

                                case "git":
                                break;
                       }
                 }

               ////////////// C++ command  ///////////

                 if(!_bSkipCmd) {
					///////////// File modif cmd
					 if (!FileUtils.IsEmpty(_sCmdName)) {
						 switch(_sCmdName[0]) {
								case 'I':
									_sBackEndCmd += fAddArgBackEnd( _sRArg);
						            fIncludePath(fExtractOneCharCmd(_sRArg));
								break;

								case 'L':
								case 'l':
				                   _sBackEndCmd +=  fAddArgBackEnd(_sRArg);
								//	sBackEndLinker += sDelimiter + _sRArg;
								break;
                                
								case 'c':

									if(!bLink){
										_sTempCompileCommand +=  fAddArgBackEnd(_sRArg);	
									}

								break;

								default:
									_sBackEndCmd += 	fAddArgBackEnd(_sRArg);
								break;
						   }
					
					}else {
						_sBackEndCmd += fAddArgBackEnd(_sRArg);
					}

                    sDelimiter = " -";
                  }

                 _sPrecArg = _sRArg;
            }}

            return _sBackEndCmd + _sTempCompileCommand;
        }



        public bool bPrecOutputSkip = true;
string sBackEndLinker = "";
        string sCallerCmd = "";
        string sArgCmd = "";
//public bool bExtacted = false;
		public void fExtract() {
       
            if (bRunToArgDontPrextract) {
                 // case "#Run":
                    fCmdRun(sRunToArgDontPrextract_File);
                return;
                // break;
            }

            string _sNewCmd = fGetConditionalRealCommand();
            if (_sNewCmd != null) {
                sCmd = _sNewCmd;
            }
            if (sCmd == "") {
                return;
            }

/*
if(bExtacted){
Output.TraceError("Already extracted!!!!!!!!!!!!");
}
bExtacted = true;
*/
			sCompiler = fGetVar("wToolchain");
			sPlatform = fGetVar("wPlatform_Name");
			s_wDir = fGetVar("wDir");

	
			oCompiler = Finder.fUseCompiler(sCompiler, sPlatform);
            if (oCompiler != null) { 
			    oCompiler.fSetVar(this);
       
             }
			fSetAllDefine();
           

		//	if(bHave_wTo) {
		///		sExecutable = oCompiler.fGetExecutableAndArgForCmd(this, false, false,  false, false);//Always compile (file list) with wTo
		///	}else{
	
		//	}


		
			string _sCmd = "";
			if(!bIsSubCmd){ 
               
	             if (sManuallySetExecutable == "" && oCompiler != null) { 
				    sExecutable = oCompiler.fGetExecutableAndArgForCmd(this, bLink, false,  bToStaticLib, bToDynamicLib);
                    //  Console.WriteLine("sExecutable " + sExecutable);
                                     
                } else {
                    sExecutable = sManuallySetExecutable;
                      
                }
           
             
                  sExtractedCmd = fExtractManualSetExecutable( fExtractVar( sCmd) );
          
	              sCallerCmd =  fExtractValidCompilerCommand(sExtractedCmd);


		        	_sCmd = " " + sResidualArg +  " " +   sArgument  ;
                  sBackEndCmd =  fExtractValidCompilerCommand(_sCmd) + sCallerCmd + sArgLinkerLib;
                    
      

                   Output.Trace("\f1B| \f16" + sExtractedCmd);

            
             //   Console.WriteLine("sBackEndCmd " + sBackEndCmd);
                /*
			   	  Debug.fTrace("--------CMD--------- " );
			   	  Debug.fTrace("-------------------_sExtractedCmd " + _sExtractedCmd );
			   	  Debug.fTrace("-------------------sCallerCmd " + sCallerCmd );
			   	  Debug.fTrace("sResidualArg " + sResidualArg );
			   	  Debug.fTrace("sArgument " + sArgument );
			   	  Debug.fTrace("_sExtractedCmd " + _sExtractedCmd );
                	   	  Debug.fTrace("--------------------- " );*/
/*
                if (sExecutable == "") {
                    throw new Exception("Receive failed");
                }
                */


			}else{  ///Sub command (Generated)
				//_sCmd = " " + sCmd; //SubCommand herite "sArgument" of parent cmd
			//TODO verify + optimize
                //   sArgument = oParentCmd.sSubArg ;

                 sExecutable = oCompiler.fGetExecutableAndArgForCmd(this, bLink, false,  bToStaticLib, bToDynamicLib);
             
                //Output.TraceWarning("sCmd:" + sCmd);
               // Output.TraceWarning("sArg:" + sArgument);

                _sCmd = " " + sResidualArg   + " " + sCmd +  " " +   sArgument  ;
               // _sCmd = " " + sResidualArg   + " " + sCmd  ;
    
                   sBackEndCmd =  fExtractValidCompilerCommand(_sCmd);
                /*s
				sArgument = oParentCmd.sSubArg ;
				sResidualArg = oParentCmd.sSubResidualArg;
				sExecutable = oParentCmd.sSubExe;
                sBackEndCmd =  fExtractValidCompilerCommand(_sCmd);
                   */

			}
       
            sExecutableName = Path.GetFileNameWithoutExtension(sExecutable);
            sExecutableType = "<" +  oCompiler.oCurrentConfigType.sName + ">";
//_sCmd = _sCmd + "  -  ";
           


          


            /*
			if(!bIsSubCmd){ 
	             if (oCompiler != null) { 
				    sExecutable = oCompiler.fGetExecutableAndArgForCmd(this, bLink, false,  bToStaticLib, bToDynamicLib);
                }
		
			}else{  ///Sub command (Generated)
				sExecutable = oParentCmd.sSubExe; //Not sure ...
			}
            sExecutableName = Path.GetFileNameWithoutExtension(sExecutable);
            Console.WriteLine("sExecutable " + sExecutable);
            */
		

		if(oParentCmd != null ){
			sRecompileOnChangeCmd =  oParentCmd.sBackEndCmd;
		}else{
			sRecompileOnChangeCmd = sBackEndCmd;
		}
//Console.WriteLine("sRecompileOnChangeCmd " + sRecompileOnChangeCmd);

		if(!bHaveMultipleFileSrc &&   !bHaveDirectorySource && sOutputFile != "") {
			oParent.aPrecObjOutput.Add(this);
			oParent.sPrecObjOutput += sOutputFile + " ";
			
		//	Debug.fTrace("--sOutputFile : " + sOutputFile);
		}


        		
	
		//// Dir source To sub commands
		if(bHaveDirectorySource) {
           	string _sOutputFolder = Path.GetDirectoryName(sOutputFile).Replace('\\','/'); //TODO when sOutputFile var is set
	    	_sOutputFolder += "/";
			fCreateCompileByDirectorySubCmd(_sOutputFolder);
		}
        else if(bHaveMultipleFileSrc) {
            string _sOutputFolder = Path.GetDirectoryName(sOutputFile).Replace('\\','/');//TODO when sOutputFile var is set
		    _sOutputFolder += "/";
			fCreateCompileByMultiFile(_sOutputFolder);
		}


		/////////////////// 
		if(bHave_wTo) { //Reset prec output

			sPrecOutput_wTo = oParent.sPrecObjOutput;
			oParent.sPrecObjOutput = "";


			///// Check all file if one is modified
			if(File.Exists(sFile_wTo + sToAnyType)) {
                 //   Console.WriteLine("---SET SKIP : true" );
				bPrecOutputSkip = true;
				foreach(CppCmd _oSubCmd in oParent.aPrecObjOutput ) {
					if(_oSubCmd.bSkip == false) {
						bPrecOutputSkip = false;
						break;
					}
				}
			}
			////////////
			oParent.aPrecObjOutput = new List<CppCmd>();

		}



/*
        if(bLink & bHaveKnowSourcesFiles) {
            bCompileAndLink = true; //When compiling & link at the same time it's bug with the linker ( HelloWorld.cpp -o bin\HelloWorld.exe)
        }*/
          

		



                           
		
			
			
			//sExecutable = CppCompiler.fGetExecutableForCmd(sBackEndCmd, bLink, bCompileAndLink, this, bToStaticLib, bToDynamicLib);
			
			if(sExecutable != "") {
				if(sExecutable.Length == 1 || (sExecutable.Length >= 2 && sExecutable[1] != ':') ){
					sExecutable = oCompiler.oModuleData.sCurrFolder + sExecutable;
                    sExecutableName = Path.GetFileNameWithoutExtension(sExecutable);
				}
				
				if( !File.Exists(sExecutable)){
					Output.TraceError("No executable " +  sExecutable+ " for: " + sCompiler + " ("  + sPlatform + ") " + "please update the xml: " + oCompiler.sFilePath  + " //Cmd[" + sCmd +"]");
					return;
				}

				FileInfo _oFileInfo = new FileInfo(sExecutable);
				sExeCmdUnique = Path.GetFileNameWithoutExtension(sExecutable) + " (" + _oFileInfo.LastWriteTimeUtc +")";

				if(!bLink && !bToStaticLib && !bToDynamicLib ) {

					oDepandance = new Depandance(this, sOutputFile);
				    bSkip = !oDepandance.bHaveNewerFile;
                    
				}

			}else { ////Error
               //   throw new Exception("Receive failed");
                if (oCompiler != null) { 
				    string _sExeType = "Cpp Compiler";

				    if(bHaveSourceC) {
					    _sExeType = "C Compiler";
				    }
				    if(bLink){
					    _sExeType = "Linker";
				    }
				    if(bToStaticLib){
					    _sExeType = "Static Linker";
				    }
				    if(bToDynamicLib){
					    _sExeType = "Dynamic Linker";
				    }
				
				    Output.TraceError("No '"+ _sExeType + "' executable for: " + sCompiler + " ("  + sPlatform + ") " + "please update the xml: " + oCompiler.sFilePath );
                 }
            }

            if (bHave_wTo && bPrecOutputSkip == false ) {
                bSkip = bPrecOutputSkip; //TO test !!! Recompile if we are wTo -> 
            }

			if(oLauchLib_Arg != null){
				fExtractLib();
			}
			

        }

        internal void fAddCompiledObject(string _sObj) {
           sObjList += _sObj + ",";
      
        }

        internal void fAddUpToDateObject(string _sObj) {
            sObjUpToDate += _sObj + ",";
            sObjList += _sObj + ",";
        }

        private string fGetConditionalRealCommand() {


            if(sHasIncluded_Arg != ""){
              return  fCmdHasInclude(sHasIncluded_Arg,sHasIncluded_True,sHasIncluded_False);
            }

            return null;

        }



        private void fSetAllDefine()
		{	
			sAllDefine = "";
			sAllDefine += "-DDsToolchain=\"" +  sCompiler.Replace('/','_') + "\" ";
		//	if(sPlatform == "CpcDos") {
		//		sAllDefine += "-DtPlatform_Windows "; //CpcDos will be Always compatible with windows, define a subplatform var?
		//	}
			if(oCompiler != null){
			//	sAllDefine += "-DDwPlatform=\"" +  oCompiler.sType + "\" ";;//Type
				//sAllDefine += "-DDwPlatform_File=\""  +  oCompiler.sSubName + "\" "; //TODO

                sAllDefine += "-DDsPlatform=\"" +  oCompiler.sType + "\" ";;//Type
				sAllDefine += "-DD_Platform_" +  oCompiler.sType + " ";//Type
				sAllDefine += "-DDsPlatform_File=\""  +  oCompiler.sSubName + "\" "; //TODO
			}
		}


		/// <summary>
		/// /////////////////////////////////////////////////////////////////
		/// </summary>
		public  void  fExecute() {

		


			string _sSendCmd = sBackEndCmd;

			if(aSubCmd.Count > 0) {

		//		CppCompiler.fShowArg(sCmd);
				foreach(CppCmd _oSubCmd in aSubCmd) {

						_oSubCmd.fExecute();
					   if(!Data.bNowBuilding) {
                            return;
                        }
				}
			}

			if(bHave_wTo) {

				

				oParent.sCurr_wTo = sFile_wTo + sToAnyType; //Better way?

     
				if(!bSkip) {
					//	Debug.fTrace("-_wTo " + sPrecOutput_wTo);
					//	Debug.fTrace("-_Out " + sFile_wTo);
					//	Debug.fTrace("sBackEndCmd " + sBackEndCmd);
					 CppCompiler.CheckAllThreadsHaveFinishedWorking(true); //Force sequence
			        string _sFile = sFile_wTo + sToAnyType;
					if(File.Exists(_sFile)) {
						try{File.Delete(_sFile);
                        } catch (Exception _e) {
                            Output.TraceWarning("Cannot update binary file ("  + _sFile + "): "  + _e.Message);
                        }

					}

					string _sTypeArg = "";
					string _sBackEndCmd = "";
					string _sEndWith = "";

					if(bLink) {
						_sTypeArg = " -o ";
						_sBackEndCmd = sBackEndCmd;
					}

					if(bToDynamicLib) {
						_sTypeArg = " -shared -o "; //-Wl,--out-implib,libexample_dll.a ??
						_sBackEndCmd = sBackEndCmd;
						//_sEndWith =  "-Wl,--out-implib," + sFile_wTo + ".a"; //Not necessary
					}

					if(bToStaticLib) {
						_sTypeArg = " -r -s ";
						_sBackEndCmd = "";
					}

					//sGenBackEndCmd = _sTypeArg   + sFile_wTo + sToAnyType + " " +  sPrecOutput_wTo + " "  +_sEndWith + sObjectLinkList  ;
					sGenBackEndCmd = _sTypeArg   + sFile_wTo + sToAnyType + " " +  sObjectLinkList + " " + sBackEndLinker + " " +_sEndWith;
					//_sSendCmd  = sGenBackEndCmd + " "+ _sBackEndCmd;
					


                    int _nIndexWTo = _sBackEndCmd.IndexOf("#To");
                    if (_nIndexWTo != -1) {
                       _sSendCmd = _sBackEndCmd.Replace("#To", sGenBackEndCmd);
                    } else {
                        _sSendCmd  =  _sBackEndCmd + sGenBackEndCmd;
                    }





/*
	Console.WriteLine("_sBackEndCmd " + _sBackEndCmd);
	Console.WriteLine("sToAnyType " + sToAnyType);
	Console.WriteLine("sFile_wTo " + sFile_wTo);
	Console.WriteLine("sToAnyType " + sToAnyType);
	Console.WriteLine("sObjectLinkList " + sObjectLinkList);
	Console.WriteLine("_sEndWith " + _sEndWith);
	Console.WriteLine("sGenBackEndCmd " + sGenBackEndCmd);
	Console.WriteLine("_sSendCmd " + _sSendCmd);
*/
				}


				//Debug.fTrace("sGenBackEndCmd " + _sSendCmd);
				// return;
			}

/*
			if(bIsGit) {
				fExecuteGit();
				return;
			}*/

            if(FileUtils.IsEmpty(_sSendCmd)) {
                return;
            }

             if(bLink) {
                  CppCompiler.CheckAllThreadsHaveFinishedWorking(true); //Force sequence??
            }

           // Debug.fTrace("--------------Skip: " + bSkip.ToString() );
            if(!Data.bNowBuilding) {
                  return;
            }

			if(bCallCompiler ){
				if(( bHaveMultipleFileSrc || bHaveDirectorySource) && !bHave_wTo){ //if not, bug with: -c (wPath)SubLib_System/Lib_GZ_OpenGL/ -o (wExPath)/ -std=c++11  |
					bCallCompiler = false;
				}
			}

            if (sManuallySetExecutable != "" ) {
              // if(!( CppCompiler.nError > 0)) {
                string _sExePath =  fFindManualExecutableLocation(sManuallySetExecutable);

                  //   CppCompiler.fSend2Compiler(_sSendCmd, bLink, false, this);
                    fLauchManualExe(_sExePath, sExtractedCmd);

                    
              //  }
            } else {

  

			    if(bCallCompiler && !bIsACmdLib ){
			    //if(bCallCompiler  ){

				    if( !bSkip ) {
                    
					    if(!(bLink && CppCompiler.nError > 0)) {
						    if(bLink && oCompiler.sLink_Action_cmd != "") {
							    fDoLinkCustomAction();
						    }
                      
					       CppCompiler.fSend2Compiler(_sSendCmd, bLink, false, this);
					    }

				    }else {
					      Output.TraceColored("\f27UpToDate: \f28" + sCmd );
				    }
			    }

            }



            /**  Not good
          	if(oLauchLib_Arg != null){
			//	fRunLib();
			}*/

 
       
            

        }

        private void fLauchManualExe(string sExePath, string sExtractedCmd) {
             LauchTool _oExe =  new LauchTool();

			//_oExe.dExit = new LauchTool.dIExit(fCompressionExit);
        
			_oExe.dOut = fExeOut;
			_oExe.dError = fExeOut;
			_oExe.fLauchExe( sExePath, sExtractedCmd);
        }
        public void fExeOut(LauchTool _oThis,string _sOut) {
            Debug.fPrint(_sOut);
        }



        private string fFindManualExecutableLocation(string sManuallySetExecutable) {

            switch (sManuallySetExecutable) {
                case "CWayv":
                return PathHelper.ToolDir +  "CWayv/CWayv.exe";
            }
            //Shearch all included exe
            return sManuallySetExecutable; //Not found
        }

        public void fFinish() {
           

             if(sObjUpToDate != "") {
              //    Console.WriteLine("--------!!!sObjUpToDate " + sObjUpToDate);
            }

           if(sObjList != "") {
             
                if(sObjListVar != "") {
                    fSetVar(sObjListVar, "[" + sObjList  + "]");
                  //   Console.WriteLine("fSetVar " + sObjListVar +  " " + sObjList);
                }
            }
         
        }
   
        ///////////////////////////////////////////////////

        public  void  fIncludePath(string _sPath) {
        
           oParent.fAddInclude(this, _sPath);

        }



         public  void fAddFile(string _sFile, string _sCondition, bool _b_O_as_SourceFiles = false) {

		 
			//Debug.fTrace("---------------_sFile " + _sFile);
		
			
			////////////////////////// Is a Directory / ////////////////////
			if(_sFile[_sFile.Length-1] == '\\' ||  _sFile[_sFile.Length-1] == '/') { 
				
				bHaveDirectorySource = true; //No error if doesn't exist, just do nothing
				if(Directory.Exists(_sFile) ) {
					
				//	Debug.fTrace("Directory.Exists!! " + _sFile);
					SrcDiry _oNewDir = new SrcDiry(_sFile,_sCondition);

					aDirectory.Add(_oNewDir);



					if(!_b_O_as_SourceFiles) {
						oParent.fAddPrjDirectory(_oNewDir);
					}

					return;
				}else {
						// Output.Trace("\f4C Output directory must have a directory source: " +  sCmd);
					 Output.Trace("\f6EWarning, directory not exist: " + _sFile);
					return;
				} //Todo ... error?
			}
			/////////////////////////////////////////////////////////////////


			aCompileFiles.Add(_sFile);

			if(!_b_O_as_SourceFiles) {
				oParent.fAddPrjDirectory( new SrcDiry(Path.GetDirectoryName(_sFile),_sCondition));
			}

            string _sExt = Path.GetExtension(_sFile).ToLower();
           // Console.WriteLine("_sExt C!!!" + _sExt);
            switch(_sExt) {
                case ".c":
					bHaveSourceC = true; 
					 bHaveKnowSourcesFiles = true; 
				break;

                case ".cw":
               //     Console.WriteLine("Have cw");
                    bHaveCW = true;
                    bHaveKnowSourcesFiles = true;
                break;

                case ".cxx":
                case ".cc":
                case ".cpp":
                case ".c++":
                    bHaveKnowSourcesFiles = true;
                break;

                case ".o":
                case ".obj":

                    bHaveKnowObjectFiles = true;

                break;

                default:
                   bHaveUnknowFiles = true;
                 break;

            }
            ///
        }
  
        public List<string> aMultiFile = new List<string>();
        public bool bHaveMultipleFileSrc = false;

        public string fCompileMultiFiles(string _sCmdFiles) {

            int _nStartBracket = _sCmdFiles.IndexOf('[');
            if (_nStartBracket != -1) {
                _nStartBracket++;
                int _nEndBracket = _sCmdFiles.IndexOf(']',_nStartBracket);
                if (_nEndBracket != -1) {
                    string _sFileList = _sCmdFiles.Substring(_nStartBracket, _nEndBracket - _nStartBracket );
                    string[] _aFile = _sFileList.Split(',');
                    foreach (string __sFile in _aFile) {
                        string _sFile = __sFile.Trim();
                        if(_sFile != ""){
                            bHaveMultipleFileSrc = true;
                            aMultiFile.Add(_sFile);
                         // Console.WriteLine("_sFile " + _sFile );
                        }
                    }
                   return _sCmdFiles.Substring(0,_nStartBracket-1) + _sCmdFiles.Substring(_nEndBracket+1); //remove []
                }
            }
            return _sCmdFiles;
        }



         public  bool fOutputCmd(string _sPath, string _sCmdFiles) {

                if (sCompileExtention == "cw"  || sCompileExtention == "c~") {
                    return false;
                }
                

				bCallCompiler = true;
				bool _bHaveC_Compile = false;
				bool _b_O_as_SourceFiles = false;


				//Just remove -c option (gcc -c compiles source files without linking.)
                 if (_sCmdFiles.Length > 2 && (_sCmdFiles[1] == ' ' || _sCmdFiles[1] == '\t'  || _sCmdFiles[1] == '\n') ) {
                     switch(_sCmdFiles[0]) {
                             case 'e':
                             case 'c':
                                  bBuildWithoutLink = true;
								_bHaveC_Compile = true;
                                  _sCmdFiles = _sCmdFiles.Substring(1); //Always work?

                            break;
							default:
								//_sCmdFiles = "";
							break;
                    }
                }
		

           //	if(_sCmdFiles == "") { //IF we don't specifiy files with "-c", take "-o" as source for "-wTo" param, example "-o obj/  -wTo bin/test.exe"
           	//_sCmdFiles[0] was precarg
			if(!_bHaveC_Compile) { //IF we don't specifiy files with "-c", take "-o" as source for "-wTo" param, example "-o obj/  -wTo bin/test.exe"
				_sCmdFiles = _sPath; //TODO multiple files !!!
				_b_O_as_SourceFiles = true;
			}

    
           _sCmdFiles = fCompileMultiFiles(_sCmdFiles);
 //Console.WriteLine("-_sCmdFiles " + _sCmdFiles);

            string[] _aFile = _sCmdFiles.Split(' ', '\t'); //TODO "" or ''
	
			for(int i = 0; i < _aFile.Length; i++) {

				string _sFile = _aFile[i];

				/////////////////////////////////// Add all folowing condition to file
				string _sCondition = "";
				int _nSubIndex = i + 1;
				while(_nSubIndex < _aFile.Length) {	

					string _sSubItem = _aFile[_nSubIndex ].Trim();
					if(_sSubItem != null && _sSubItem.Length > 1) {
						if( _sSubItem[0] != '!' ) { //Begin with exclue special cond
		
							break;
						}else {
	
							_sCondition += _sSubItem + " ";
						}
					}

					_nSubIndex++;
				}
				i = _nSubIndex - 1;
				//////////////////////////////////////////////////////////////////////

				 if(!FileUtils.IsEmpty(_sFile)) {
                    fAddFile(_sFile, _sCondition, _b_O_as_SourceFiles);
                }
			}

		     sOutputFile = _sPath.Replace('\\','/');

			fOutputTo(sOutputFile,!(bHaveDirectorySource || bHaveMultipleFileSrc)); //bHaveDirectorySource Single accpet no "wTo"?

	        //Debug.fTrace("asss + "  + sGenBackEndCmd);
            /*
			if(bToAnyType){//AAAAAASSS
				sOutputFile = sOutputFile.Replace(".*" , sToAnyType);
				//sBackEndCmd = sBackEndCmd.Replace(".*" , sToAnyType);
				Debug.fTrace("asss + "  + sOutputFile);
			}*/


			//////// Is Output was a directory /////////////
			if( sOutputFile[sOutputFile.Length-1] == '/') {

				oParent.fAddPrjDirectoryOutput( new SrcDiry(sOutputFile,"")); //Todo set it like an output

				if(!bHaveDirectorySource  && !bHaveEntrySource && !bHaveMultipleFileSrc) { 

					 Output.Trace("\f4C Output directory must have a directory source: " +  sCmd);
					return false;

				}else{
					//Bug remove utilil things
				//	sBackEndCmd = sBackEndCmd.Substring(0, nLastBackEndIndex); ///Remove -c source command
				}
				FileUtils.fCreateDirectoryRecursively(sOutputFile);
			

				return true;
			}else {

				oParent.fAddPrjDirectoryOutput( new SrcDiry(Path.GetDirectoryName(sOutputFile),""));
				
				if(bHaveDirectorySource  && !bHaveEntrySource && !bHaveMultipleFileSrc) {
					 Output.Trace("\f4C Source directory must have a directory output: " + sCmd);
					return false;
				}
			

		
			}
			////////////////////////////////////////////////

			return false;
        }

	public bool bToAnyType = false;
	public string sToAnyType = "";

		public void fOutputTo(string _sOutput, bool _bwTo = false) {
			 string _sToType =  Path.GetExtension(_sOutput).ToLower();
			if( _sToType == String.Empty) { _sToType = "";}

            string _sDirectory = Path.GetDirectoryName(_sOutput);
		
			

//onsole.WriteLine("Output dir:! " +_sDirectory);

            FileUtils.fCreateDirectoryRecursively(_sDirectory);
			sOutDirectory = _sDirectory.Replace("\\","/" ) + "/";

			//Console.WriteLine("ToType " +_sToType  + " | " + _sOutput);

            switch(_sToType) {
				 case ".a":
	//				bToStaticLib = true; -> PreOutput
				break;	
				 case ".dll":
	//				bToDynamicLib = true; -> PreOutput
				break;
				
			    case ".html":
                case ".exe":
				case ".*":
                case "": //Linux?
					//Console.WriteLine("!!!!!!!!!!!!!!!!-**-*-*!wOutBin" + _sDirectory);
					oParent.fSetVar("wOutBin", sOutDirectory);
					//fSetVarCmd("wOutBin", "wOutBin"+_sDirectory);
					
				//	Console.WriteLine("!! " + 	Data.fGetGlobalVar("wOutBin"));


					if(_bwTo){
						if(_sToType == ".*") {
							_sToType = "";
						}
						if(_sToType == "") {
							fGetAppTypeFromPlatform();
						}
					}

                  //    bLink = true; -> PreOutput
                     oParent.aLinkCmdList.Add(this);
					
                 break;
				default: //Any!
				
				break;
            }
		}
		
		public void fGetAppTypeFromPlatform() {

			bToAnyType = true;
			switch(fGetVar("wPlatform")){
				case "Windows":
					sToAnyType = ".exe";
				break;
				case "Web_Emsc":
					sToAnyType = ".html";
				break;
			}
		//	Debug.fTrace("***************fGetAppTypeFromPlatform : "   +sToAnyType );
		//	Debug.fTrace("***************fGetAppTypeFromPlatform : "   +sToAnyType );
			//oParent.sCurr_wTo = sFile_wTo + sToAnyType;
			//bHave_wTo = true; 
		}



        /**
        public  void fCompileWithoutLink(string _sPath) {

        //    aCompileFiles.Add(_sPath);
         //   oParent.bHaveCompileWithoutLink = true;
            
        }
        */




        public void fGitCommand(string _sCmd,string _sVal, string _sArg) {

			bIsGit = true;
            switch (_sCmd){
                case "clone":
					sGitCmd = _sCmd;
					sGitURL =  "--progress " +  _sVal;
							 //   fBuildAndCommand(_sArg);s
				//	Debug.fTrace("Clone : " + sGitURL);
                 break;
				case "reset":	
					sGitCmd = _sCmd;	
					sGitURL = sCmd.Substring(sCmd.IndexOf("reset") + 5); //Full command

					//Debug.fTrace("sGitURL " + sGitURL);
				break;
            }
        }



		public void fSetVarCmd(string _sCmd, string _sArg) { 
			if(_sCmd.Length> 1 && _sCmd[1] >= 'A' && _sCmd[1] <= 'Z' ){
				///////////////////////////////Same
				string _sFullValue = "";
				int _nIndex = _sArg.IndexOf('=');
				int _nIndexSpace = _sArg.IndexOf(' ');
				if(_nIndex < _nIndexSpace) {
					_nIndex = _nIndexSpace;
				}
				if(_nIndex != -1) {
					_nIndex++;
					_sFullValue = _sArg.Substring(_nIndex, _sArg.Length -  _nIndex).Trim();
				}
				////////////////////////////////////
				fSetVar(_sCmd, _sFullValue);
			}
		}

       public void fSetVar(string _sName, string _sArg) {
           oParent.fSetVar(_sName, _sArg);
        }



        public string fGetStringVar(string _sVarWithBracket) {

             int  _nIndexStart = _sVarWithBracket.IndexOf('{');
            if (_nIndexStart == -1) {return "";}
            int  _nIndexEnd = _sVarWithBracket.IndexOf('}',_nIndexStart);
            if (_nIndexEnd == -1) {return "";}
            return _sVarWithBracket.Substring(_nIndexStart+1, _nIndexEnd - _nIndexStart-1).Trim();
        }


        public void fPre_SetCwcVar( string _sArg) {


			///////////////////////////////Same
			string _sFullValue = "";
			int _nIndex = _sArg.IndexOf('=');
			int _nIndexSpace = _sArg.IndexOf(' ');
			if(_nIndex < _nIndexSpace) {
				_nIndex = _nIndexSpace;
			}
			if(_nIndex != -1) {
				_nIndex++;
				_sFullValue = _sArg.Substring(_nIndex, _sArg.Length -  _nIndex).Trim();
			}
			////////////////////////////////////
      
			string _sMainValue = "";
		//	string[] _aCmd = _sArg.Split('=', ' ');
			string[] _aCmd = _sArg.Split('=');


            if(_aCmd.Length > 1) {
                _sMainValue = _aCmd[1].Trim();
            } else {
                return; //No '='
            }
            string  _sFullVar = _aCmd[0];
            
           
            
            string _sVar = fGetStringVar(_sFullVar);
            if(_sVar == ""){return;}
                /*
            int  _nIndexStart = _sFullVar.IndexOf('{');
            if (_nIndexStart == -1) {return;}
            int  _nIndexEnd = _sFullVar.IndexOf('}',_nIndexStart);
            if (_nIndexEnd == -1) {return;}
            string _sVar = _sFullVar.Substring(_nIndexStart+1, _nIndexEnd - _nIndexStart-1).Trim();
            */

            string[]   aVar = _sVar.Split('_');

    

            string _sVarArg = fExtractSpaceMultiVals(_sArg);
            fSetVar(_sVar, _sVarArg);

			 // switch (_sVar){
             string _sCmd = aVar[0];
			  switch (_sCmd){


				 case "wCwcUpd":
                    fwCwcUpd(_sVarArg);
                 break;
				case "wCwcVer":
                    fwCwcUpdVer(_sVarArg);
                 break;
				case "wCwcUpdated":
                    fwCwcUpdated(_sVarArg);
                 break;

                case "wToolchain":
					//oParent.fSetVar(_sCmd, _sMainValue);
					fwToolchain( _sVarArg);
                break;

				case "wLib": //TODO multilib, ","
					fwLib(_sVarArg);
				break;
				
				 case "wNoColor":
                    Data.bColor = false;
                 break;

				 case "wNoGUI":
                    Data.bGUI = false;
                 break;

				 case "wBuildAnd":
                    //fBuildAndCommand(_sArg);
					oParent.fSetVar(_sCmd, _sMainValue);
                 break;
            	 case "_sOpt":
                    //fBuildAndCommand(_sArg);
					oParent.fSetVar(_sCmd, _sMainValue);
                 break;
				
				case "wArch":
				case "wArchPC":
					oParent.fSetVar(_sCmd, _sMainValue);
				 break;
				case "wPlatform":

					oParent.fSetVar(_sCmd, _sMainValue);
			//		Debug.fTrace("wPlatform : " + _sMainValue);
			//		Debug.fTrace("wToolchain : " + oParent.fGetVar("wToolchain"));
					oParent.fAddCompiler(oParent.fGetVar("wToolchain"), _sMainValue); ///Force create CompilerData ex: detect Emscriptem, maydo do a list?
				//	oParent.aCompilerList.Add(	Data.fGetCompiler, _sMainValue) );

				 break;

				case "wDir":
			        fSetWorkingDir(oParent, _sVarArg);
                 break;

				case "wMode":
                    fCmdMode(_sVarArg);
                 break;
				case "wCloseOnId":
                    fCmdwCloseOnId(_sVarArg);
                 break;
                  case "wConnectHandle":
			         fCmdwConnectHandle(_sVarArg);
                 break;	
            }
		}

        public static void fSetWorkingDir(ArgumentManager _oArg, string _sFullValue) {
                 
             _sFullValue = _sFullValue.Replace('\\','/');
            if (_sFullValue[_sFullValue.Length-1] != '/' ) {
                _sFullValue = _sFullValue + "/";
            }


            try  {  Directory.SetCurrentDirectory(_sFullValue.Replace('"', ' ') );  } catch (DirectoryNotFoundException e) { //TODO verifie order execution of SetCurrentDirectory
				Output.TraceError("wDir not found: " + _sFullValue);
			}

          _oArg.fSetVar("wDir", _sFullValue);
                       
		    // PathHelper.ExeWorkDir =  _sFullValue;
	       	SysAPI.fSetWorkingDir(_sFullValue);
        }




        string sPreCwcCommand = "";
		public void fPreCwcCommand( string _sArg, string _sFullRArg) {
            sPreCwcCommand = _sArg;


            string _sCmdArg = _sArg;
                    
            string _sCmdTrue = "";
            string _sCmdFalse = "";

            int _nIndex = _sCmdArg.IndexOf( "::"); if (_nIndex != -1) {
                string[] _aCond = _sFullRArg.Split(new string[] { "::" }, StringSplitOptions.None);
                  if (_aCond.Length> 0) {
                        _sCmdArg = _aCond[0]; //Remove "::"
                  }
                  if (_aCond.Length> 1) {
                        _sCmdTrue = _aCond[1];
                  }
            }

            /*
             string[] _aCond = _sFullRArg.Split(new string[] { "::", "!:" }, StringSplitOptions.None);
            if (_aCond.Length> 0) {

                    _sCmdArg = _aCond[0];
        //    Console.WriteLine("_sCmdArg2: " + _sCmdArg);
            }
           if (_aCond.Length> 1) {//TOFO test if its realy "::"
                    _sCmdTrue = _aCond[1];
            }
             if (_aCond.Length> 2) { //TOFO test if its realy "!:"
                    _sCmdFalse = _aCond[2];
            }
      
            */
           
             _sCmdArg = fExtractSpaceMultiVals(_sCmdArg, ' ' );


/*
			///////////////////////////////Same
			string _sFullValue = "";
			int _nIndex = _sArg.IndexOf('=');
			int _nIndexSpace = _sArg.IndexOf(' ');
			if(_nIndex < _nIndexSpace) {
				_nIndex = _nIndexSpace;
			}
			if(_nIndex != -1) {
				_nIndex++;
				_sFullValue = _sArg.Substring(_nIndex, _sArg.Length -  _nIndex).Trim();
			}
			////////////////////////////////////

			string _sMainValue = "";
			string[] _aCmd = _sArg.Split('=', ' ');

            if(_aCmd.Length > 1) {
                _sMainValue = _aCmd[1];
			
            }
            */

			  //switch (_sCmd){
			  switch (sRet_ExtractSpaceMultiValsCmd){


               case "#If_HasIncluded":
                    sHasIncluded_Arg = _sCmdArg;
                    sHasIncluded_True = _sCmdTrue;
                    sHasIncluded_False = _sCmdFalse;
                 //  Debug.fTrace("PreHasIncluded !? " + sHasIncluded_Arg );
                
               break;
				 
             case "#Run":
                 //Do not preextract?
                 sRunToArgDontPrextract_File = _sCmdArg;
                 bRunToArgDontPrextract = true;
               break;
				 
              case "#Lauch":
              //    Debug.fTrace("--!!Lauch " + _sArg);
                fCmdwLauch(_sCmdArg);
               break;
            case "#To":
					bHave_wTo = true;
					
					if(bIsACmdLib){
						fPreOutputLib(_sCmdArg);
					}else{
						fPreOutputCmd(_sCmdArg,false, true);
					}
				break;
             case "#Build":
                 	fPreBuildCmd(_sCmdArg);

             break;

             case "#Obj":
                 	fPrObjCmd(_sCmdArg);
             break;
                //    fSetCompiler(_sArg);
                // break;
					
            }
		}

        public string sObjListVar = "";
        private void fPrObjCmd(string sCmdArg) {
     
            if (sCmdArg[0] != '>') {
                Output.TraceError("Output object list to var require assing '=>{VarName}' : " + sPreCwcCommand);
            }   
           sObjListVar = fGetStringVar(sCmdArg);
               
        }

        private void fCmdwLauch(string _sToLauch) {

            //Create settings file


            bSendCmdToLauch = true;
            sToLauch = _sToLauch;

          //  Setting.fNewSettingsLauch(_sToLauch);
           Data.sToLauch = _sToLauch;//Broken??

           Data.bNonBuildCommand = true;

        }

        public void fCwcCommand(string _sCmd, string _sArg) {

            string _sCond = _sArg;
            string _sCmdTrue = "";
            string _sCmdFalse = "";
             string[] _aCond = _sArg.Split(new string[] { "::", "!:" }, StringSplitOptions.None);
            if (_aCond.Length> 0) {
                    _sCond = _aCond[0];
            }
           if (_aCond.Length> 1) {//TOFO test if its realy "::"
                    _sCmdTrue = _aCond[1];            }
             if (_aCond.Length> 2) { //TOFO test if its realy "!:"
                    _sCmdFalse = _aCond[1];
            }

            
             _sCond = fExtractSpaceMultiVals(_sCond, ' ' );


            switch (_sCmd){

				  case "#To":
                    fwTo(_sCond);
                 break;

				  case "#Copy":
                    fCmdCopy(_sCond);
                 break;

				  case "#If_NotExist":
                    fCmdIfNotExist(_sCond);
                 break;

            
               case "#If_HasIncluded":
                 //  fCmdHasInclude(_sCond, _sCmdTrue, _sCmdFalse);
            /*
                    sHasIncluded = _sCond;
                    
                    Console.WriteLine("****HasIncluded !? " + sHasIncluded );
                    if (oParent.fHasInclude(sHasIncluded)) {
                           Console.WriteLine("YYYYEEESS-INCLUDED !? " + sHasIncluded );
                    } else {
                        
                    }*/
               break;

				/*
				  case "#Run":
                    fCmdRun(fExtractSpaceMultiVals(_sArg, ' ' ));
                 break;
			*/
            }
        }

        private string fCmdHasInclude(string _sCond, string _sCmdTrue, string _sCmdFalse) {



             if (oParent.fHasInclude(_sCond)) {
                
               return _sCmdTrue;
      
            } else {
            
                return _sCmdFalse; //TODO not modie real cmd
            }


        }





        /*
                public void fSetPlatform(string _sValue) {


                    if( Data.bIsCompilerSet ) {
                        return;
                    }

                      string[] _aCmd = _sValue.Split('=', ' ');

                    if(_aCmd.Length > 1) {
                        string _sMainValue = _aCmd[1];
                        fSetCompilerVal(_sMainValue.Trim());

                    }
                }


                public void fSetCompiler(string _sValue) {

                    if( Data.bIsCompilerSet ) {
                        return;
                    }

                     string[] _aCmd = _sValue.Split('=', ' ');

                    if(_aCmd.Length > 1) {
                        string _sMainValue = _aCmd[1];
                        fSetPlatformVal(_sMainValue.Trim());

                    }
                }




              public static void fSetPlatformVal(string sVal) {
                     Data.sPlatform = sVal;
                     Data.bIsPlatformSet = true;
                }

        */





        /*
               public void fBuildAndCommand(string _sValue) {
                     string[] _aCmd = _sValue.Split(' ');
              //  Debug.fTrace("fBuildAndCommand "+ _sValue);
                     if(Data.sBuildAnd == "") {

                        if(_aCmd.Length > 1) {
                             string _sMainValue = _aCmd[1];

                            switch(_sMainValue) {

                               case "Sanitize":
                               case "Nothing":
                                case "Run":
                                    Data.sBuildAnd = _sMainValue;
                                break;
                            }
                        }
                    }
                }
        */


        public void fCleanCorruptObj() {
	    string sExtension = System.IO.Path.GetExtension(sOutputFile);
       // string sPath = sOutputFile.Substring(0, sOutputFile.Length - sExtension.Length) ;
	//	Debug.fTrace("---Clean : " + sPath);
/*
		switch(sExtension) {
				case ".o":

					 DirectoryInfo taskDirectory = new DirectoryInfo(Path.GetDirectoryName(sOutputFile));
					FileInfo[] taskFiles = taskDirectory.GetFiles(sOutputFile + "*");
					foreach(FileInfo _oInfo in taskFiles) {
						Debug.fTrace("Corrupt found: " + _oInfo.FullName);
					}


				break;
		}
*/
		
	//	Debug.fTrace("---sExtension : " + sExtension);


		
	}



     public string fExtractValue(string _sValue) {

            int _nIndex =  _sValue.IndexOf('=');
            if(_nIndex != -1) {
                return _sValue.Substring(_nIndex+1);
            }else {
                return "";
            }
        }


   public string fFindEndSentence(string _sResult) {
            _sResult = _sResult.Trim();
			
			if(_sResult[0] == '\"') {
			//	return _sResult.Substring(0, _sResult.IndexOf("\"", 1)+1); //Keep " ?
				return _sResult.Substring(1, _sResult.IndexOf("\"", 1)-1); 
			} 
			else if(_sResult[0] == '\'') {
				//return _sResult.Substring(0, _sResult.IndexOf("\'", 1)+1); //Keep " ?
				return _sResult.Substring(1, _sResult.IndexOf("\'", 1)-1);

			} 
			else  {
				int _nIndex = 1;
				while(_nIndex < _sResult.Length) {
					char _nChar = _sResult[_nIndex];
					if(_nChar == ' ' || _nChar == '\n' || _nChar == '\t'  ) {
						return _sResult.Substring(0,_nIndex);
					}
					_nIndex ++;
				}
				return _sResult;
			}
	}
		

     public string fExtractOneCharCmd(string _sValue) {
            string _sResult = "";
          
           _sResult = _sValue.Substring(1); 

           return fFindEndSentence(_sResult);
        }


        public string fExtractSpaceVal(string _sValue) {

			int _nIndex = 1;
			while(_nIndex < _sValue.Length) {
				char _nChar = _sValue[_nIndex];
				if(_nChar == ' ' || _nChar == '\n' || _nChar == '\t'  ) {
					break;
				}
				_nIndex ++;
			}
			if(_nIndex == _sValue.Length) {
				return "";
			}
			_sValue = _sValue.Substring(_nIndex);
           return fFindEndSentence(_sValue);
        }


        public string sRet_ExtractSpaceMultiValsCmd;
        public string fExtractSpaceMultiVals(string _sValue, char _cRequiredDelim='=' ) {//_cRequiredDelim??

            int _nIndexSpace =  _sValue.IndexOf(' ');
            int _nIndexEgal =  _sValue.IndexOf('='); 

            int _nIndex  = _nIndexSpace;
            if(_nIndexEgal != -1) {_nIndex = _nIndexEgal;}
            if(_nIndexSpace != -1 && _nIndexSpace < _nIndexEgal) {
                _nIndex = _nIndexSpace;
            }

            string _sResult = "";
            if(_nIndex != -1) {

               _sResult = _sValue.Substring(_nIndex+1).Trim();

                if(_sResult.Length >= 2){
                       //Remove quote!!
                    if (_sResult[0] == '\"') {
                        _sResult = _sResult.Substring(1);
                    }

                    if (_sResult[_sResult.Length-1] == '\"') {
                        _sResult = _sResult.Substring(0, _sResult.Length-1);
                    }
                }
                //////
                   

                 sRet_ExtractSpaceMultiValsCmd = _sValue.Substring(0,_nIndex).Trim();

                return _sResult;

            }


             sRet_ExtractSpaceMultiValsCmd = _sValue.Trim();
           return "";
        }




        internal void fAddCommandLineVerificationToDepedancesFile(CppCmd _oCmd) {
            string sExtension = System.IO.Path.GetExtension(sOutputFile);
            string sPath = sOutputFile.Substring(0, sOutputFile.Length - sExtension.Length) + ".d";
            if(File.Exists(sPath)) {
                //ReadDependandece?

                string _sAddToDepandance= "Cwc:" + _oCmd.sCompiler + " " + _oCmd.sPlatform  + " " + _oCmd.sExeCmdUnique + " >> " +  sRecompileOnChangeCmd;

                fReadDepandances(sPath, _sAddToDepandance);

               // Debug.fTrace("FileExist!!");
              //   File.AppendAllText(sPath, "Cwc:" + _oCmd.sCompiler + " " + _oCmd.sPlatform  + " " + _oCmd.sExeCmdUnique + " >> " +  sRecompileOnChangeCmd); //TODO   Data.sCompiler Test if may change between files
                 
            }
        }






        private void fReadDepandances(string _sPath, string sAddToDepandance) {
                string readText = File.ReadAllText(_sPath);

                 

                oDepandance.fReadDepandance(readText,null,true); //Reread depandance to know witch file depend on it, use with #Has_depandace command
                
                File.AppendAllText(_sPath, sAddToDepandance);
        }

        public void fwTo(string _sArg) {
		bCallCompiler = true;

		bHave_wTo = true;
		sFile_wTo = _sArg.Replace('\\','/');
		if(sFile_wTo.Length>2) {if(sFile_wTo[sFile_wTo.Length-1] == '*' && sFile_wTo[sFile_wTo.Length-2] == '.' ) {
				sFile_wTo = sFile_wTo.Substring(0,sFile_wTo.Length-2);//remove .*
		}}

		oParent.sCurr_wTo = sFile_wTo + sToAnyType;

		fOutputTo(_sArg, true);

	}

	public void fwToolchain( string _sArg) {    
		Data.bCheckLibRTRequired = false;

       string[] _aArg = _sArg.Split(' ', '=', '/', ','); 
		if(_aArg.Length < 2) {
			Output.TraceError("{wToolchain} Require argument: \"sAutor/sName/(sPlatform/sSubCompiler)\"" );
		}else {
			string _sName  = _aArg[0] + "/" + _aArg[1];
			string _sPlatform ="";
			if(_aArg.Length >= 3){
				_sPlatform = _aArg[2];
			} 

			oParent.fSetVar("wToolchain", _sName);
			oParent.fSetVar("wPlatform", _sPlatform);


			Data.fAddRequiredModule(_sName,true);
			oParent.fAddCompiler(_sName, _sPlatform); ///Force create CompilerData ex: detect Emscriptem, maydo do a list?
		}
	}

   ModuleData oLib = null;
	public bool bIsACmdLib = false;

	public void fwLib(string _sArg) {
       string[] _aArg = _sArg.Split(' ', '=', '/', ','); 
		if(_aArg.Length < 2) {
			Output.TraceError("{wLib} Require argument: \"sAutor/sName/(sVersion)\"" );
		}else {
			bIsACmdLib = true;
		    oLib =	Data.fAddRequiredModule(_aArg[0] + "/" + _aArg[1]);
			oParent.fAddLib(oLib); //Todo multiple lib with separator ,
            oLauchLib_Arg =  new ArgumentManager();
		}
	}

	public void fwCwcUpd(string _sArg) {

		Data.bUpdateMode = true;
		Data.sUpdateModeSrc = _sArg.Replace('\\','/');
	}
	public void fwCwcUpdVer(string _sArg) {

		Data.sUpdateVer = _sArg;

	}

	public void fwCwcUpdated(string _sArg) {
		Data.bNothingToBuild = true;
		
		UpdateCwc.fUpdated(_sArg);
	//	Output.TraceGood("Current " + Data.sVersion);
		//Data.sUpdateVer = _sArg;

	}









/*
	public void fwDir(string _sArg) {
		s_wDir = _sArg;
	}*/









		public void fExecuteGit() {

			
             LauchTool _oGit =  new LauchTool();
			 //_o7z.dExit = new LauchTool.dIExit(fCompressionExit);
			_oGit.sWorkPath = PathHelper.ExeWorkDir;

			string _sFolderName = Path.GetFileNameWithoutExtension(sGitURL);


			if(!Directory.Exists(  PathHelper.ExeWorkDir + _sFolderName ) ) {
				
				_oGit.fLauchExe( PathHelper.ToolDir + "git/cmd/git.exe", sGitCmd + " \""+  sGitURL + "\"", "", "", true);
				oParent.aExeWaitingList.Add(_oGit);
			}else {
				 Output.TraceColored("\f2 -- " + _sFolderName + " Exist -- \f28" + sCmd );
			}
		}

       public string sObjectLinkList = "";


		public void fCreateCompileByDirectorySubCmd(string _sOutputFolder) {
	

			//List all files
			foreach(SrcDiry _oDirectory in aDirectory) {
				string[] _aCond = null;
				if(_oDirectory.sCondition != "") {
					_aCond = _oDirectory.sCondition.Split(' ');
				}
				string _sDirectoryFolder = Path.GetDirectoryName(_oDirectory.sFile);
				List<String> _aFiles =	FileUtils.GetAllFiles(_sDirectoryFolder);
				foreach(string _sFile in _aFiles) {
                    fCreateCompileMultiFile(_sFile, _sDirectoryFolder, _sOutputFolder,  _aCond );
				}
			}
		}
        public void fCreateCompileByMultiFile(string _sOutputFolder) {
			//List all files
          //  Debug.fTrace("------------------------: " +  _sOutputFolder);
			foreach(string _sFile in aMultiFile) {
                fCreateCompileMultiFile(_sFile, "", _sOutputFolder, null );
              //         Debug.fTrace("- " +  _sFile);
			}
          //  Debug.fTrace("----------****- " );
		}

        public void fCreateCompileMultiFile(string _sFile, string _sDirectoryFolder,string  _sOutputFolder, string[] _aCond = null) {


                    _sFile = _sFile.Replace('\\', '/');

					bool _bInclude = true;
					//if(_oDirectory.sCondition != "") {
					if(_aCond != null) {
						foreach(string _sCond in _aCond) {if(_sCond != null && _sCond.Length > 1 ) {
							if(_sCond[0] == '!') { //Exclude folder
							//		Debug.fTrace("-Test !!! "  + _sFile.Substring(0,_sCond.Length-1 ) );
								if( _sCond.Substring(1) == _sFile.Substring(0,_sCond.Length-1)) {
								//	Debug.fTrace("-Exclude!!! "  + _sCond.Substring(1) );
									_bInclude = false;
								}
							
							}
						}}

					}

					if(_bInclude) {


					    
							string _sName = Path.GetFileNameWithoutExtension(_sFile);
							string _sExt = Path.GetExtension(_sFile).ToLower();
						
							string _sSubFolder  = Path.GetDirectoryName(_sFile.Substring(_sDirectoryFolder.Length  ));

							if(_sSubFolder != "") {
                                if(_sSubFolder[_sSubFolder.Length-1] != '/'){
							    	_sSubFolder = _sSubFolder +  "/";
                                  }
							}
                            if(_sSubFolder == "/") {
                                _sSubFolder = "";
                             }

		//	//		Debug.fTrace("_sOutputFolder: " +  _sOutputFolder);
			//	Console.WriteLine("_sSubFolder: " +  _sSubFolder);
				//Debug.fTrace("_sFile: " + _sFile);
				//Debug.fTrace("_sSubFolder: " + _sSubFolder);
									
								

							switch(_sExt) {
								case ".o":
								case ".obj":
									sObjectLinkList += _sFile + " ";
								
								break;

								case ".c":
                                case ".cxx":
                                case ".cc":
								case ".cpp":
								case ".c++":
								case ".cw":
								case ".gcpp":
                                    if(_sExt == ".cw") {
									//Console.WriteLine("Found CW");
									}
							        if(_sExt == ".c") {
										bHaveSourceC = true;
									}
									/*
									if(_sExt == ".gcpp") {
										bForceCpp = true;
									}*/
									string _sForceLang = "";
									if(bForceCpp){
										_sForceLang = "-x c++ ";
									}
								
                                     
									if(sSubArg == null){  //Reget correct compiler with file type?
										//Ugly code ... :S 
										string _sRestor_sArgument =	sArgument;
										string _sRestor_sResidualArg =	sResidualArg;
										sSubExe = oCompiler.fGetExecutableAndArgForCmd(this, false, false,  false, false);
										sSubResidualArg = sResidualArg;
										sSubArg = sArgument;
										sResidualArg = _sRestor_sResidualArg;
										sArgument = _sRestor_sArgument;
										/////
									}
										
								//	string _sCmd = " " + sCallerCmd + " " + sSubResidualArg + " " +  sSubArg + " " + _sForceLang + " -c " +_sFile + " -o " + _sOutputFolder + _sSubFolder  + _sName +".o";
									string _sCmd = " " + sCallerCmd + " " + _sForceLang + " -c " +_sFile + " -o " + _sOutputFolder + _sSubFolder  + _sName +".o";
									
                
                /*
									Debug.fTrace("sSubArg: " + sSubArg);
									Debug.fTrace("_sCmd: " + _sCmd);
									Debug.fTrace("sBackENd: " + sBackEndCmd);*/

									CppCmd _oSubCmd =  new CppCmd(oParent, _sCmd, this);
                                     _oSubCmd.sCompileExtention = _sExt.Substring(1); //Remove dot
                                   // _oSubCmd.oCompiler.oCurrentConfigType = _oSubCmd.oCompiler.fGetConfigFileType(_oSubCmd.sCompileExtention );
                                   // oCompiler.oCurrentConfigType = oCompiler.fGetConfigFileType(_oSubCmd.sCompileExtention );
                                    _oSubCmd.fExtract();
									aSubCmd.Add(_oSubCmd);
									sObjectLinkList +=_oSubCmd.sOutputFile + " ";

								break;
							}
					}
        }



		public string sSubArg = null;
		public string sSubFiltredArg = "";
		public string sSubExe = "";
		public string sSubResidualArg = "";
        private bool bSendCmdToLauch;
        private string sToLauch;
        private string sRunToArgDontPrextract_File;
       // private string sHasIncluded = "";
        private bool bRunToArgDontPrextract;
        private string sRunToArgDontPrextract_Arg = "";
        private string sHasIncluded_Arg = "";
        private string sHasIncluded_True = "";
        private string sHasIncluded_False = "";

        internal string sObjList = "";
        internal string sObjUpToDate = "";

        public  string fExtractVar( string _sCmd) {
			return CppCmd.fExtractVar(_sCmd, this);
		}

		public  static string fExtractVar( string _sCmd, CppCmd _oCmd) {
         //   Debug.fTrace("*#: " + _sCmd);


			int _nIndexStart = _sCmd.IndexOf('{');
			while(_nIndexStart != -1) {

				//if(_sCmd.Length > _nIndexStart + 2 && (_sCmd[_nIndexStart+ 1] == 'w' || _sCmd[_nIndexStart+ 1] == 'v') && (_sCmd[_nIndexStart+ 2] >= 'A'  &&  _sCmd[_nIndexStart+ 2] <= 'Z') ) { //Minimal length -> wA | vA -> Second must be upper case letter

					int _nIndexEnd = _sCmd.IndexOf('}', _nIndexStart);
					if(_nIndexEnd != -1) {
                       
                         //Get prev char
                   
                         int _nIndexPrevChar =_nIndexStart-1;
                        while (_nIndexPrevChar > 0 && _sCmd[_nIndexPrevChar] == ' ') {
                            _nIndexPrevChar--;
                        }
                        if(_nIndexPrevChar < 0){_nIndexPrevChar = 0;}

                          int _nIndexPrevPrevChar =_nIndexPrevChar;
                        if (_nIndexPrevPrevChar > 0) {
                            _nIndexPrevPrevChar--;
                        }

                          //Get next char
                         int _nIndexNextChar = _nIndexEnd+1;
                        while (_nIndexNextChar < _sCmd.Length && _sCmd[_nIndexNextChar] == ' ') {
                            _nIndexNextChar++;
                        }
                        if(_nIndexNextChar >= _sCmd.Length) {
                            _nIndexNextChar = _sCmd.Length;
                           _sCmd += " ";
                        }
                       

                      //  Debug.fTrace("PrevCharIs: " + _nIndexPrevChar);
                       // Debug.fTrace("NextCharIs: " + _nIndexNextChar);
                       // Debug.fTrace("PrevCharIs: " + _sCmd[_nIndexPrevChar]);
                      //  Debug.fTrace("NextCharIs: " + _sCmd[_nIndexNextChar]);
                        if(  !((_sCmd[_nIndexPrevChar] == '-' && _sCmd[_nIndexNextChar] == '=')  ||  (_sCmd[_nIndexPrevPrevChar] == '=' && _sCmd[_nIndexPrevChar] == '>') ) ){ //Set a var (do not remplace)
                            

						    string _sOriVar = _sCmd.Substring(_nIndexStart + 1, _nIndexEnd - ( _nIndexStart  + 1));
						    string _sVar = "";
						    if(_oCmd != null){
							    _sVar = _oCmd.fGetVar(_sOriVar);
						    }else{
							    _sVar = Data.fGetGlobalVar(_sOriVar);
						    }

						    if( _sVar != ""  ) {

                                int _nRevIndex=0;
                                if(_sVar[0] == '-' &&  _sCmd[_nIndexPrevChar] == '-') { //Example .../App.*  -{vPreloadRc} //--> Remove extra '-' if already in var
                                    //remove '-'
                                    _nRevIndex = 1;
                                 }
							    _sCmd = _sCmd.Substring(0, _nIndexStart - _nRevIndex) + _sVar +  _sCmd.Substring(_nIndexEnd + 1);
						    }
                            
                             if(  _sVar == "" &&  _sCmd[_nIndexPrevChar] == '-' ) { //Clear it if not exist and have command '-' before, TODO verify if this is intended operation
                                _sCmd = _sCmd.Substring(0, _nIndexStart-1) +  _sCmd.Substring(_nIndexEnd + 1); //Remove '-' if double?
						    }
                            


                           if(_nIndexStart >= _sCmd.Length) {
                                break;
                            }
                         }


					
					}
				//}
				_nIndexStart = _sCmd.IndexOf('{', _nIndexStart+1); 
			}
			
			return _sCmd;		
		}
		
/*
		public  static string fGetVar(ArgumentManager _oManager, string _sVar) {
			return _oManager.fGetVar(_sVar);
		}
*/
		public  string fGetVar(string _sVar) {
			return oParent.fGetVar(_sVar);
		}
/*

		public  string fGetVar(string _sVar) {
			switch(_sVar) {
				case "wArch":
					return "x32";
				break;
				case "wPlatform":
					return oParent.sPlatform;
				break;
				case "wArchPC":
					return "x86";
				break;
				case "wOpt":
					return "Debug";
				break;
			}
			return "";
		}
*/

		public void fCmdCopy(string _sAllArg) {
		//	Debug.fTrace("COPY:! " + _sAllArg);
			string[] _aArg =  _sAllArg.Split(' ');
			if(_aArg.Length >= 2) {
				//FileUtils.
				
				FileUtils.CopyFolderContents(_aArg[0], _aArg[1]); //TODO on run pass only?
			}

			
		//	foreach(string _sArg in _aArg ) {
		//	}
		}

		public void fCmdIfNotExist(string _sAllArg){

	        string[] _aCond =_sAllArg.Split(new string[] { "::" }, StringSplitOptions.None); if(_aCond.Length >= 2) {//Condition
				string _sFiles = _aCond[0];

				string[] _aArg =  _sFiles.Split(' ');
				if(_aArg.Length >= 1) {
					//FileUtils.
					string _sFile = _aArg[0];
//Debug.fTrace("----------_sFile: " +_sFile );
					if(!File.Exists(_sFile)) {

						//			Debug.fTrace("!!!!!!!!SubCmd!!: " + "-wRun " + _aCond[1] );
								
							CppCmd _oSubCmd =  new CppCmd(oParent, "-wRun " + _aCond[1], this);
							_oSubCmd.fExtract();
							aSubCmd.Add(_oSubCmd);
					}else {
						//	Debug.fTrace("Exist: " +_sFile );
					}

			}}   
}


		public ArgumentManager fNewArgCmdRun(string _sAllArg, bool bRun = true,  ArgumentManager _oArg = null, bool bFile = true, string _sSendArg = ""){	


 



            string _sFile = _sAllArg;

			if(_oArg == null){
				 _oArg = new ArgumentManager();
			}
			_oArg.bSubArgMan = true;
            //Data.bDontExecute = true;

            if (bFile) {
                _sAllArg = _sSendArg + "@" + _sAllArg;
            }

          

			_oArg.ExtractMainArgument( ArgProcess.fExpandAll(_sAllArg), !bRun);
             // Output.TraceWarning("#Run " + _oArg.sAllArg );



       //     	_oArg.fCompleteExtractMainArgument(null,false);

         
			if(bRun && !Data.bModuleIsRequired){
               Output.Trace("\f7F#Run " + _sFile + " : \f78" +  _oArg.sAllArg );
                _oArg.fCompleteExtractMainArgument(null,false);
				_oArg.fExtract(null);
				_oArg.fRun(null, false, false);
               Output.Trace("\f7F#End " +_sFile );
			}
      
			return _oArg;
		}



	public void fCmdRun(string _sAllArg){
		


			 string _sExt = Path.GetExtension(_sAllArg).ToLower();
			switch(_sExt) {
				case ".cwc":
						Debug.fTrace("Expand!!!!!: " +  _sAllArg);
					//	Debug.fTrace("Expand!!!!!: " +  Data.fExpand("@" + _sAllArg,0));
						fNewArgCmdRun(_sAllArg,true,null,true,sRunToArgDontPrextract_Arg + " > " );

						/*
						string _sFile = ;
						CppCmd _oSubCmd =  new CppCmd(oParent, _sFile , true);
						_oSubCmd.fExtract();
						aSubCmd.Add(_oSubCmd);*/
					
				break;
			
				case ".bat":
				case ".exe":
				   Debug.fTrace("Found WRun, delocalising ... ");
				    Delocalise.fDelocalise(_sAllArg);
                    
				break;

			}
	}


	



	public void fCmdMode(string _sArg){
		if(_sArg == "IDE") {

			Data.bModeIDE = true;
			Data.oModeIDE = new ModeIDE();
		//	Debug.fTrace("!!!!!!!!!!IDE MODE!!");
		}
	}

	public void fCmdwCloseOnId(string _sID){
		
		 int _nId;
         if (Int32.TryParse(_sID, out _nId)) {
               Data.nCloseOnId = _nId;
         }

		Debug.fTrace("----------------------------CloseOnId! : " + _nId.ToString());
	}
       private void fCmdwConnectHandle(string _sID) {
         Data.bNonBuildCommand = true;
		 int _nId;
         if (Int32.TryParse(_sID, out _nId)) {
               Sys.nConnectedHandle = _nId;
         }

//		Console.WriteLine("----------------------------fCmdwMenuHandle! : " +_sID);
//		Console.WriteLine("----------------------------fCmdwMenuHandle! : " + _nId.ToString());
    }



	public void fDoLinkCustomAction() {
		switch(oCompiler.sLink_Action_cmd) {
				case "s_html":
				
					string _sInputFolder =   Path.GetDirectoryName(oCompiler.sLink_Action_src).Replace("\\", "/") + "/";
					string _sOutFolder =  PathHelper.ExeWorkDir + sOutDirectory;
					//Debug.fTrace("!!!!Do Action!!! : " + _sInputFolder  + "  " +   _sOutFolder);
					FileUtils.CopyFolderContents(_sInputFolder, _sOutFolder,"*.*", "*.s_html");
				break;

		}
		
	}

	





    }
}
