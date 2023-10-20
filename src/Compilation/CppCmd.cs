

using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

       public  string sExplicite_Name = "";
       public  string sExplicite_App = "";
       public  string sExplicite_Call = "";


       public  List<CppCmd> aSubCmd = new List<CppCmd>();
       public  List<CppCmd> aSubFBCmd = new List<CppCmd>();
       public  List<CppCmd> aSubInputCmd = new List<CppCmd>();
        
        public bool bDontIncludeSubFolder = false;
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
       public string s_pProject = "";
       public string sPrecOutput_wTo = "";


       public bool bIsGit = false;
      public string sGitCmd = "";
      public string sGitURL = "";


       public string sCompiler = "";
       public string sConfig_Type = "";



    
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
       public bool bIsFbCmd = false;

		 string[] aPreArg;




       public  List<string> aInclude = new List<string>();
       public  List<string> aCompileFiles = new List<string>();
       public  List<string> aCompileFilesName = new List<string>();
        
       public  List<SrcDiry> aDirectory = new List<SrcDiry>();

		
	//  public  bool bCompileAndLink = false;  
      public ArgumentManager oParent = null;


		
		public CppCmd oParentCmd = null;
		
       public CppCmd (ArgumentManager _oParent, string _sCmd, CppCmd _oParentCmd = null, bool _bIsFbCmd = false) {
			oParentCmd = _oParentCmd;
            oParent = _oParent;

			if(oParentCmd != null){
                if(!_bIsFbCmd) {
				    bIsSubCmd = true;
                }else{
                    bIsFbCmd = true;
                }
/*
				if(_sCmd == oParentCmd.sCmd){ //If is empty don't execute
					return;
				}*/
			}

			if(_sCmd != "" ){
				_sCmd = _sCmd.Replace('\n',' ' );
				//_sCmd = _sCmd.Replace('\r',' ' );
				sCmd = _sCmd.Trim();
				sCmd  = fExtractVar( sCmd, true); //TODO check if this don't break changes
		
           //     Debug.fTrace("_sCmd " + _sCmd);
			
				fPreExtract();
			}
		}




		public void fPreExtract() {
		    bool _bCompiling = false;



    //        aPreArg  = _sCmd.Split(new string[] { " -" }, StringSplitOptions.None); 
	//		foreach (string _sArg in aPreArg) { if (!FileUtils.IsEmpty(_sArg)) {

          //  int _nNextIdex = 0;

 

            string _sFullRArg = " " + sCmd;
         
      //      _nNextIdex = _sFullRArg.IndexOf(" -");
          
            while(true){


                
                   int _nIdex = _sFullRArg.IndexOf(" -");
                    if(_nIdex != -1){
                        _sFullRArg = _sFullRArg.Substring(_nIdex+1);//+2?
                  }


             	// _sFullRArg =_sFullRArg.Substring(_nNextIdex+2).TrimStart();
                _sFullRArg = _sFullRArg.TrimStart();
                if(_sFullRArg[0] == '-'){
                   _sFullRArg = _sFullRArg.Substring(1).Trim();
                }else {
                    break;
                }

          char _cSwithcLetter = _sFullRArg[0];
          char _cSecondLetter = ' ';
           if(_sFullRArg.Length > 1){
                _cSecondLetter= _sFullRArg[1];
           }



                string _sRArg = _sFullRArg;

                int _nNextIdex = _sFullRArg.IndexOf(" -");
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


                 //////Find delemiter///////////////////////////////
                string _sFullValue = _sFullRArg;
                int _nRemIndexFullArg = 0;
                string _sMainSetValue = "";
                 bool _bAssingOnEmpty = false;
                if(_sFullRArg[0] == '{') {
           	        ///////////////////////////////Same

			        int _nIndex = _sFullRArg.IndexOf('=');

                     if(_nIndex> 0 && _sFullRArg[_nIndex-1] == ':') { //Assign only in not initialised or empty
                        _bAssingOnEmpty = true;
                     }

                    /*
			        int _nIndexSpace = _sFullRArg.IndexOf(' ');
			        if(_nIndex < _nIndexSpace) {
				        _nIndex = _nIndexSpace;
			        }*/
			        if(_nIndex != -1) {
				        _nIndex++;
                        _nRemIndexFullArg = _nIndex;


				        _sFullValue  = _sFullRArg.Substring(_nIndex, _sFullRArg.Length -  _nIndex).Trim();
                        _sMainSetValue = _sFullRArg.Substring(0, _nIndex-1);
                       //  _sFullRArg = " " + _sFullValue;
                         _sFullRArg =  _sFullValue;


                        //_sMainSetValue = _sMainSetValue.Replace("}", "").Trim(); //TODO error if  '}' not found
                        _sMainSetValue = fGetStringVar(_sMainSetValue);
                        //fGetStringVar TODO

			        }
                }

                bool _bHaveOtherSetFlag = false;
               
                ////////// Find Others delemiter //////////////////
                //Find endindex of next setting var
                int _nEndIndex = _sFullRArg.IndexOf('=');
                if(_nEndIndex > 4) {//Min = -{nX}=  (5)
                  

                    if(_sFullRArg[_nEndIndex-1] == '}') { 
                        while(_nEndIndex > 1 && _sFullRArg[_nEndIndex] != '{' ) {
                            _nEndIndex--;
                        }
                        if( _sFullRArg[_nEndIndex-1] == '-') { //TODO retry for next '}' if no '-'
                            _nNextIdex = _nEndIndex + _nRemIndexFullArg - 2; //-2 --> +2 on substr
                            _sFullValue = _sFullRArg.Substring(0, _nEndIndex-1).Trim();
                            _sFullRArg = " " + _sFullRArg.Substring( _nEndIndex-2).Trim();
                            _bHaveOtherSetFlag = true;
                        }
                    }
                }
                //////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////



           



				//Debug.fTrace("##--------------:" + _sArg );
			//	string _sRArg =_sArg.Trim();
			//	 string[] _aCmd = _sRArg.Split(' ', '=', '+'); 
				//  string _sCmdName = _aCmd[0].Trim();



				// switch(_sCmdName[0]) {
				 switch(_cSwithcLetter) {
                            case '{':
                              //   fPre_SetCwcVar( _sRArg);
                              //  int _nEndIndex = fPre_SetCwcVar( _sFullRArg);
                                //int _nEndIndex = 
                                fPre_SetCwcVar(_sMainSetValue, _sFullValue, _bAssingOnEmpty);
                                if(!_bHaveOtherSetFlag) {
                                    return;
                                }

                             //   if(_nEndIndex  != 0) { //Full to be able to set flag in vars, TODO Split other set var (-{vNzIncl}=  ... -{vNzIArg}=..)
                              //      _nNextIdex = _nEndIndex; //Stop, // TODO Split other set var (-{vNzIncl}=  ... -{vNzIArg}=..) or directive ...
                              //   }
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
                                if(_cSecondLetter == 'w') {
                                     fPreGetCompilationType(_sRArg, "cw");
                                }else { 
                                     fPreGetCompilationType(_sRArg);
                                }
							
                                //get compilation type 
                                
							break;
							case 'i':
							case 'o':
								/*
								if(Data.bToolchainDefined){
                                  //  Output.TraceWarning("Use Default Compiler [LibRT]");
									//f_wToolchain("VLianceTool/LibRT"); //Default compiler
									f_wToolchain(""); //Default compiler
								}*/
								fPreOutputCmd(_sFullRArg,_bCompiling);
							break;
						default:
							bCallCompiler = true; //Non Cwc Command /unknow -> call the compiler
						break;
			
                       }




			}
		}

        public string sCompileExtention = "";
        private void fPreGetCompilationType(string sRArg, string _sForce = "") {
         
            if(_sForce != "") {
                sCompileExtention = _sForce;
                return;
            }

            if (sRArg[sRArg.Length-1] == '/') { //It's a folder, it will be processed in subcommands
                return;
            }
            int _nDotIndex = sRArg.LastIndexOf(".");
            if (_nDotIndex>= 0) {
               sCompileExtention = sRArg.Substring(_nDotIndex + 1).ToLower();
            }
        }

        public bool bCallCompiler = false;
		

		bool bLibExtracted = false;
		public void fExtractLib(){
            if(bLibExtracted){
                return;
            }bLibExtracted = true;

             oLaunchLib_Arg.aLib = oParent.aLib;

            fExtractSubLib(oLib);
            foreach(ModuleData _oSubLib in  oLib.aSubLib) {
                //Output.TraceAction("_oSubLib "  + _oSubLib.sAutorName);
                 fExtractSubLib(_oSubLib);
            }
		}


        public void fExtractSubLib(ModuleData _oLib ){
                 // oLaunchLib_Arg.fSetVar("wPath",  oLib.sCurrFolder);  		
                  oLaunchLib_Arg.fSetVar("_pModule",  _oLib.sCurrFolder);  		
                  oLaunchLib_Arg.fSetVar("_pOutput", sLaunchLib__pOutput);

      
                  //oLaunchLib_Arg.aLib = oParent.aLib;


              if(_oLib.oLibData != null  && _oLib.oLibData.sCmd != "") {

                  //      Console.WriteLine("***********************************aaa " + oLib.oLibData.sCmd );
               	     fNewArgCmdRun( _oLib.oLibData.sCmd , false,oLaunchLib_Arg,false); //Not run
                   fRunLib(_oLib);
        //          Console.WriteLine("havec commmand!! " + oLib.oLibData.sCmd);
                }
		}






        		
		public void fRunLib(ModuleData _oLib)
        {
            if(bIsRunLib){
        //        bIsRunLib = false;
                 Output.TraceWarning("#Run "  + oLib.sCurrFolder);

                oLaunchLib_Arg.fCompleteExtractMainArgument(_oLib);
                oLaunchLib_Arg.fExtract(_oLib);
                oLaunchLib_Arg.fRun(_oLib);
                 Output.TraceWarning("#End "  +  oLib.sCurrFolder);
            }
        }



		public ArgumentManager oLaunchLib_Arg = null;
		public string sLaunchLib_File = "";
		public string sLaunchLib__pOutput = "";
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
            oLaunchLib_Arg =  new ArgumentManager();


            //extract varz
            _sArg
            */
           // sLaunchLib_wExPath =  fGetVar("wDir") + _sArg;
            
        }



		public void fPreOutputLib(string _sArg){

			sLaunchLib__pOutput =  fGetVar("_pProject") + _sArg;
		//	sLaunchLib_File = oLib.sCurrFolder + "wLib.cwc";
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
				    oParent.bPreOutput_Link = true;//Use to overide run command

					switch(_sExtention.ToLower()){

						case "o": //Output to .o will never be a Link time (protection)
						case "obj":
						//	bLink = false;
						break;
					
						case "":
						case "*":
						break;

                        case "so":
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



        public string fExtractCwcCmd(string _sCmd) {

            _sCmd = " " + _sCmd;//Be sure to remove first " -"
		     string[]  _aArg  = _sCmd.Split(new string[] { " -" }, StringSplitOptions.None); 


            string _sBackEndCmd = "";

            string _sDelimiter = " -";
            string _sCmdTest = _sCmd.TrimStart();
            if(_sCmdTest.Length == 0){ return "";}
             if(_sCmdTest[0] != '-') {
                _sDelimiter = " "; // begin with "-"
            }

            foreach (string _sArg in _aArg) { if (!FileUtils.IsEmpty(_sArg)) { 
              
				string _sRArg = _sArg.Trim();

                string[] _aCmd = _sRArg.Split(' ', '=', '+'); 
                string _sCmdName = _aCmd[0].Trim();

                 ////////////// C~ command  ///////////
               if (!FileUtils.IsEmpty(_sCmdName)) { 

						switch(_sCmdName[0]) {
                               
                                case '#':
                                    fCwcCommand(_sCmdName, _sRArg);
                                    if( _sCmdName == "#To"){
                                        _sBackEndCmd += "#To"; //Keep track of it to remplace later by generated command 
                                    }
                                 break;
                            default:
                                _sBackEndCmd += _sDelimiter + _sRArg;
                                _sDelimiter = " -";
                             break;

                       }

  
                 }

            }}

            return _sBackEndCmd;
        }




        public string fExtractValidCompilerCommand(string _sCmd) {


            //   _sExe = oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Exe", "Linker_Dynamic"}, oCurrentConfigType.sName);


            /////////////////////////////////////////////////////////////
            ////////// Call explicit application argument ///////////////
            /////////////////////////////////////////////////////////////
            _sCmd = _sCmd.TrimStart();
            if(oCompiler != null) {
                int _indexSpace = _sCmd.IndexOf(' ');
                if(_indexSpace != -1) {
                    string fFirstWord = _sCmd.Substring(0, _indexSpace);
                    string _sNode  =   oCompiler.oGblConfigType.fGetNode(null,new string[]{"Exe", fFirstWord}, "");
                    bool _bFound = false;
                    if (_sNode != "") {
                        _bFound = true;
                        sExplicite_App = _sNode;
                    }else{
                        //Internal explicit program
                        switch(fFirstWord) {
                            case "7z":
                                _bFound = true;
                                sExplicite_App = PathHelper.ToolDir +  "7z/7z.exe";
                            break;
                        }
                    }
                    if(_bFound) { //Found
                        sExplicite_Name = fFirstWord;
                        sExplicite_Call = _sCmd.Substring(_indexSpace + 1).Trim();
                        sExplicite_Call =  fExtractCwcCmd(sExplicite_Call);
                        sExplicite_Call = fTest_SubCmdOrCompileMultiFiles(sExplicite_Call);

                         Debug.fTrace("Found Explicit call: " + fFirstWord + " : " + sExplicite_App + " : " + sExplicite_Call);
                         return "";
                    }




                }
            }

            ///////////////////////////////////////////////////////////////////
            //////////////////// Standard Compiler arg ////////////////////////
            ///////////////////////////////////////////////////////////////////

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
                if (Data.fGetGlobalVar("_sType") == "Bash") {
                     Data.fAddRunToBash(this);
                 }
                 // case "#Run":
                    fCmdRun(sRunToArgDontPrextract_File, sRunToArgDontPrextract_File_Arg);
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
			sCompiler = fGetVar("_wToolchain");
			sConfig_Type = fGetVar("_sConfig_Type");
			s_pProject = fGetVar("_pProject");

	
			oCompiler = Finder.fUseCompiler(sCompiler, sConfig_Type);
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
           
             
                 // sExtractedCmd = fExtractManualSetExecutable( fExtractVar( sCmd) );
                  sExtractedCmd = fExtractManualSetExecutable( fExtractVar( sCmd) );
          
	              sCallerCmd =  fExtractValidCompilerCommand(sExtractedCmd);


		        	_sCmd = " " + sResidualArg +  " " +   sArgument  ;
                  sBackEndCmd =  fExtractValidCompilerCommand(_sCmd) + sCallerCmd + sArgLinkerLib;
                    
      
                    
                  // Output.Trace("\f1B| \f16" + sExtractedCmd);
                 CppCompiler.fShowArg(" " + sExtractedCmd, bIsSubCmd | bIsFbCmd);
            
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

                _sCmd = " " + sResidualArg   + " " + sArgument  +  " " +   sCmd  ;
               // _sCmd = " " + sResidualArg   + " " + sCmd  ;
    
                sBackEndCmd =  fExtractValidCompilerCommand(_sCmd);

                /*
				sArgument = oParentCmd.sSubArg ;
				sResidualArg = oParentCmd.sSubResidualArg;
				sExecutable = oParentCmd.sSubExe;
                sBackEndCmd =  fExtractValidCompilerCommand(_sCmd);
                 */
			}


            if(oCompiler == null) {
              //  if(bHaveSourceC || bHaveMultipleFileSrc || bHaveKnowSourcesFiles) {
                if(bCallCompiler) {
                    if(sCompiler == "" || sCompiler == "Unknow") {
                      Output.TraceError("Please set the variable  {_wToolchain} to a valid compiler:");
                       Output.TraceError("-{_wToolchain}=[Server]/Author/Name/[Version]");
                      Output.TraceError("In example:");
                      Output.TraceError("-{_wToolchain}=VLianceTool/LibRT");
		
                    }else { 
                      Output.TraceError("Compiler not exist: " + sCompiler + " (" + sConfig_Type + ")");
                    }
                    Build.fDisableBuild();
                }
                return;
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


        if(sOutputFile.Length!= 0) {
         
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
					Output.TraceError("No executable " +  sExecutable+ " for: " + sCompiler + " ("  + sConfig_Type + ") " + "please update the xml: " + oCompiler.sFilePath  + " :Cmd[" + sCmd +"]");
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
				
				    Output.TraceError("No '"+ _sExeType + "' executable for: " + sCompiler + " ("  + sConfig_Type + ") " + "please update the xml: " + oCompiler.sFilePath );
                 }
            }

            if (bHave_wTo && bPrecOutputSkip == false ) {
                bSkip = bPrecOutputSkip; //TO test !!! Recompile if we are wTo -> 
            }

			if(oLaunchLib_Arg != null){
				fExtractLib();
			}
			

        }

        internal void fAddToObjDic(string _sKey, string _sObj) {
             if(!aObjList.ContainsKey(_sKey)){
                aObjList.Add(_sKey, "");
            }
            aObjList[_sKey] += _sObj + ",";
        }


        internal void fAddCompiledObject(string _sObj) {
            int _nEndIndex = _sObj.IndexOf(']');
            int _nBegIndex = _sObj.IndexOf('[');
            if(_nEndIndex == -1 || _nBegIndex == -1)
            {
                    Output.TraceError(_sObj);
                return;
            }

            string _sFile = _sObj.Substring(_nEndIndex+1);
         
            string _sFullLib = _sObj.Substring(_nBegIndex + 1 , _nEndIndex - (_nBegIndex + 1));
           string[] _aLib =  _sFullLib.Split(':');

           // Output.TraceWarning(_aLib[0]);

            fAddToObjDic(_aLib[0], _sFile);
            if(_aLib.Length > 1) {
                fAddToObjDic(_aLib[1], _sFile);
            }
      
           sObjList += _sFile + ",";
        }

        internal void fAddUpToDateObject(string _sObj) {
            fAddCompiledObject(_sObj);

            sObjUpToDate += _sObj + ","; //TODO add string file to
           // sObjList += _sObj + ",";
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

                sAllDefine += "-DDsPlatform=\"" +  oCompiler.sPlatformName + "\" ";;//Type
				sAllDefine += "-DD_Platform_" +  oCompiler.sPlatformName + " ";//Type
				sAllDefine += "-DDsPlatform_File=\""  +  oCompiler.sSubName + "\" "; //TODO

                if(Data.fGetGlobalVar("_sOpt") == "Debug" || Data.fGetGlobalVar("_sOpt") == "Profiler") {
                    sAllDefine += "-DD_Debug ";
                }else {
                    sAllDefine += "-DD_Release ";
                }
                sAllDefine += "-DDsOpt=\""  + Data.fGetGlobalVar("_sOpt") + "\" " ;

			}
		}


		/// <summary>
		/// /////////////////////////////////////////////////////////////////
		/// </summary>
		public  string  fExecute() {
            
            if(aSubFBCmd.Count > 0) { //like: adb logcat --pid=[adb shell pidof -s {sPckName}]
                foreach(CppCmd _oSubCmd in aSubFBCmd) {
				    sExplicite_Call += _oSubCmd.fExecute();
					if(!Data.bNowBuilding) {
                        return "";
                    }
				}
            }

            if(sExplicite_Name != ""){
                return  CppCompiler.fSend2App(this, sExplicite_Name, sExplicite_App, sExplicite_Call, bIsFbCmd, aSubInputCmd);
            }

            
            /*
            if(bTestCmd && false) //DISABLED
            {
                Output.TraceWarning("Execute Test CMD");

              // ModuleData _oMyModule =  oCompiler.oModuleData;
                    
                  ModuleData _oMyModule = ModuleData.fGetModule("VLiance/GZE",false);



                    ModuleData _oLib = new ModuleData(_oMyModule, "src/SubLib_3rdparty/", "GzNima");
                   // _oMyModule.aSubLib.Add(_oLib);
                       _oMyModule.fAddSubLib(_oLib);

                    ModuleData _oLibBox2d = new ModuleData(_oMyModule, "src/SubLib_3rdparty/", "GzBox2D");
                //     _oMyModule.aSubLib.Add(_oLibBox2d);
                     _oMyModule.fAddSubLib(_oLibBox2d);


                  Output.TraceAction("Parent LIB: " + _oMyModule.sName);
                    Output.TraceAction("ADD NIMA LIB: " + _oLib.sCurrFolder);
                    Output.TraceAction("ADD Box2D LIB: " + _oLib.sCurrFolder);

                    _oMyModule.fGetSubLibCompilerData(oParent);
                       
            }

            */
		


			string _sSendCmd = sBackEndCmd;

			if(aSubCmd.Count > 0) {

		//		CppCompiler.fShowArg(sCmd);
				foreach(CppCmd _oSubCmd in aSubCmd) {

						_oSubCmd.fExecute();
					   if(!Data.bNowBuilding) {
                            return "";
                        }
				}
			}

			if(bHave_wTo) {

				// oParent.sCurr_wTo =  "";
                if(sFile_wTo.Length >= 1  && sFile_wTo[sFile_wTo.Length-1] != '/') {
                    oParent.sCurr_wTo = sFile_wTo + sToAnyType; //Better way?
                }

				bool bash = Data.fGetGlobalVar("_sType") == "Bash";
				if(!bSkip || bash) {
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

					if(bLink && !bToStaticLib) {
						_sTypeArg = " -o";
						_sBackEndCmd = sBackEndCmd;
					}

					if(bToDynamicLib) {
						//_sTypeArg = " -shared -o "; //-Wl,--out-implib,libexample_dll.a ??
						_sBackEndCmd = sBackEndCmd;
						//_sEndWith =  "-Wl,--out-implib," + sFile_wTo + ".a"; //Not necessary
					}

					if(bToStaticLib) {
                     //   _sTypeArg = " -o"; //TODO not emsc!
						//_sTypeArg = " -r -s ";
						//_sTypeArg = " rcs ";
						//_sBackEndCmd = "";
						_sBackEndCmd = fCleanUnknowCmd( sBackEndCmd);
					}

					//sGenBackEndCmd = _sTypeArg   + sFile_wTo + sToAnyType + " " +  sPrecOutput_wTo + " "  +_sEndWith + sObjectLinkList  ;
					sGenBackEndCmd = _sTypeArg + " "  + sFile_wTo + sToAnyType + " " +  sObjectLinkList + " " + sBackEndLinker + " " +_sEndWith;
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
            
            if(FileUtils.IsEmpty(_sSendCmd)) {
                return "";
            }

            ///Cleanup ////
            _sSendCmd = _sSendCmd.Replace("\\", "/").Replace("//", "/").Replace("//", "/");
            ////////

/*
			if(bIsGit) {
				fExecuteGit();
				return;
			}*/

          

             if(bLink) {
                  CppCompiler.CheckAllThreadsHaveFinishedWorking(true); //Force sequence??
            }

           // Debug.fTrace("--------------Skip: " + bSkip.ToString() );
            if(!Data.bNowBuilding) {
                  return "";
            }

			if(bCallCompiler ){
				if(( bHaveMultipleFileSrc || bHaveDirectorySource) && !bHave_wTo){ //if not, bug with: -c (_pModule)SubLib_System/Lib_GZ_OpenGL/ -o (wExPath)/ -std=c++11  |
					bCallCompiler = false;
				}
			}

            if (sManuallySetExecutable != "" ) {
              // if(!( CppCompiler.nError > 0)) {
                string _sExePath =  fFindManualExecutableLocation(sManuallySetExecutable);

                  //   CppCompiler.fSend2Compiler(_sSendCmd, bLink, false, this);
                    fLaunchManualExe(_sExePath, sExtractedCmd);

                    
              //  }
            } else {


                //// Get obj list /////
            string _sAllFile = "";
            string _sObjectList = "";
            string _sDelim= "";
       
            foreach(string _sFile in aCompileFilesName){
                _sAllFile += _sDelim + _sFile ;
                _sDelim = ", ";
            }
            _sObjectList = _sAllFile;
               
             ///////////////////////
              bool bash = Data.fGetGlobalVar("_sType") == "Bash";
        

			    if(bCallCompiler && !bIsACmdLib  ){
			    //if(bCallCompiler  ){

				    if( !bSkip || bash) {
					    if( !(bLink && CppCompiler.nError > 0) ) {
						    if(bLink && oCompiler != null &&  oCompiler.sLink_Action_cmd != "") {
							    fDoLinkCustomAction();
						    }
                      
					       CppCompiler.fSend2Compiler(_sSendCmd, bLink, false, this,_sObjectList);
					    }

				    }else {
					      Output.TraceColored("\f27UpToDate: \f26[\f26" + _sObjectList + "\f26]\f28" + sCmd );
				    }
			    }

            }



            /**  Not good
          	if(oLaunchLib_Arg != null){
			//	fRunLib();
			}*/

 
       
            return "";

        }

        private string fCleanUnknowCmd(string _sBackEndCmd) { //TODO parametrable
            _sBackEndCmd = " " + _sBackEndCmd.Trim();
            if(_sBackEndCmd.Length <= 1) {return ""; }
            string _sDelemiter = "";
            if(_sBackEndCmd[1] == '-') { //Can be without '-' of begening
                _sDelemiter  = " -";
            }

            string _sResult = "";
           string[] aArray = _sBackEndCmd.Split(new string[] { " -" }, StringSplitOptions.None);
            foreach(string _sLine in aArray) {
                string _sLineTest = _sLine.Trim();
                if(_sLineTest.Length>0) {
                    switch(_sLine[0]) {
                      case 'I': break;
                      case 'L': break;
                        default:
                             _sResult += _sDelemiter + _sLineTest;
                        break;
                     }
                }
                _sDelemiter = " -"; 
            }

          return _sResult;
        }

        private void fLaunchManualExe(string sExePath, string sExtractedCmd) {
             LaunchTool _oExe =  new LaunchTool();

			//_oExe.dExit = new LaunchTool.dIExit(fCompressionExit);
        
			_oExe.dOut = fExeOut;
			_oExe.dError = fExeOut;
			_oExe.fLaunchExe( sExePath, sExtractedCmd);
        }
        public void fExeOut(LaunchTool _oThis,string _sOut) {
            Debug.fPrint(_sOut);
        }



        private string fFindManualExecutableLocation(string sManuallySetExecutable) {

            switch (sManuallySetExecutable) {
                case "CWayv":


                     return PathHelper.ToolDir +  "CWayv/CWayv.exe";

                    string _sWayv = oCompiler.oGblConfigType.fGetNode(null, new string[] { "Exe", "CWayv" }, "");
                   return _sWayv;




            }
            //Shearch all included exe
            return sManuallySetExecutable; //Not found
        }

        public void fFinish() {
           

             if(sObjUpToDate != "") {
              //    Console.WriteLine("--------!!!sObjUpToDate " + sObjUpToDate);
            }


           //  .ContainsKey( _oNewDir.sFile)
           //if(aObjList.Count != 0){
            if(sObjList != "") {
                 //   Output.TraceWarning("List: " + sObjList);
                if(sObjListVar != "") {
                    fSetVar(sObjListVar, "[" + sObjList  + "]");
                    Output.TraceAction("Set: " + sObjListVar + " = "  +   "[" + sObjList  + "]");
                    foreach(var  item  in aObjList){
                        fSetVar(sObjListVar + "_" + item.Key , "[" + item.Value  + "]");
                         Output.TraceAction("Set: " + sObjListVar + "_" + item.Key + " = "  +   "[" + item.Value  + "]");
                    }

                  //    Output.TraceAction("sObjListVarval + "  + sObjListVar);
                   //   Output.TraceAction("sObjListVarSert + "  + sObjList);
                  //   Console.WriteLine("fSetVar " + sObjListVar +  " " + sObjList);
                }

            }
             /*
           if(sObjList != "") {
             Output.TraceWarning("List: " + sObjList);
                if(sObjListVar != "") {
                    fSetVar(sObjListVar, "[" + sObjList  + "]");

                  //    Output.TraceAction("sObjListVarval + "  + sObjListVar);
                   //   Output.TraceAction("sObjListVarSert + "  + sObjList);
                  //   Console.WriteLine("fSetVar " + sObjListVar +  " " + sObjList);
                }
            }
            */



         
        }
   
        ///////////////////////////////////////////////////

        public  void  fIncludePath(string _sPath) {
        
           oParent.fAddInclude(this, _sPath);

        }



         public  void fAddFile(string _sFile, string _sCondition, bool _b_O_as_SourceFiles = false) {
            if(_sFile.Length <= 1){
                return; //not a valid file
            }
		 
			//Debug.fTrace("---------------_sFile " + _sFile);
		
			
			////////////////////////// Is a Directory / ////////////////////
			if(_sFile[_sFile.Length-1] == '\\' ||  _sFile[_sFile.Length-1] == '/') { 
				
                string _sTestFile = _sFile;
                if(_sTestFile[0] == '!' || _sTestFile[0] == '&'){
                    _sTestFile = _sTestFile.Substring(1);
                }

				bHaveDirectorySource = true; //No error if doesn't exist, just do nothing
				if(Directory.Exists(_sTestFile) ) {
					 if(_sFile[0] != '!') {

				    //	Debug.fTrace("Directory.Exists!! " + _sFile);
					    SrcDiry _oNewDir = new SrcDiry(_sTestFile,_sCondition);
					    aDirectory.Add(_oNewDir);


					    if(!_b_O_as_SourceFiles) {
						    oParent.fAddPrjDirectory(_oNewDir);
					    }
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
            string _sDir = Path.GetDirectoryName(_sFile);
            /*
            string _sName = _sFile.Substring(_sDir.Length+1);
            //Add one folder to the name?//
            string _sLastFolder = Path.GetFileName(_sDir)+ "/";
            string _sPrevFolder =  "";
            if(_sDir.Length - _sLastFolder.Length > 0) {
                _sPrevFolder = Path.GetFileName(_sDir.Substring(0, _sDir.Length - _sLastFolder.Length)) + "/";
            }

            ///////////////////////////////
            aCompileFilesName.Add(_sPrevFolder + _sLastFolder  + _sName);
            */
            aCompileFilesName.Add(_sFile);

			if(!_b_O_as_SourceFiles) {
				oParent.fAddPrjDirectory( new SrcDiry(_sDir,_sCondition));
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

                case ".mm":
                    if (Data.fGetGlobalVar("_sType") != "Bash") break;
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

        public string fTest_SubCmdOrCompileMultiFiles(string _sCmdFiles) {

            int _nStartBracket = _sCmdFiles.IndexOf('[');
            if (_nStartBracket != -1) {
                _nStartBracket++;
                int _nEndBracket = _sCmdFiles.IndexOf(']',_nStartBracket);
                if (_nEndBracket != -1) {
                    string _sFileList = _sCmdFiles.Substring(_nStartBracket, _nEndBracket - _nStartBracket );
                    string[] _aFile = _sFileList.Split(',');
                    if(_aFile.Length > 1) {  //It's multiple files
                        foreach (string __sFile in _aFile) {
                            string _sFile = __sFile.Trim();
                            if(_sFile != ""){
                                bHaveMultipleFileSrc = true;
                                aMultiFile.Add(_sFile);
                             // Console.WriteLine("_sFile " + _sFile );
                            }
                        }
                    }else{ //It's a sub command ??

                        _sFileList = _sFileList.Trim();

                        if(_sFileList.Length > 3 && _sFileList[0] == '<' && _sFileList[1] == '<' ){ //Ex: ndk-stack -sym {pObj}build/apk/{pLib_ABI} [<< adb logcat -b crash]
                             _sFileList = _sFileList.Substring(2);
                            CppCmd _oSubCmd =  new CppCmd(oParent, _sFileList, this, true);
                             _oSubCmd.fExtract();
                            aSubInputCmd.Add(_oSubCmd);
                        }else { 
                             CppCmd _oSubCmd =  new CppCmd(oParent, _sFileList, this, true);
                            _oSubCmd.fExtract();
					        aSubFBCmd.Add(_oSubCmd);
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

    
           _sCmdFiles = fTest_SubCmdOrCompileMultiFiles(_sCmdFiles);
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
						if( _sSubItem[0] != '!') { //Begin with exclue special cond
		
							break;
						}else {
	
							_sCondition += _sSubItem + " ";
						}
					}

					_nSubIndex++;
				}
				//i = _nSubIndex - 1;
				//////////////////////////////////////////////////////////////////////
                if(_sFile.Length> 1 && _sFile[0] == '&') { //Disable recursive directories
                     _sFile = _sFile.Substring(1);
                    //_sCondition += "& ";
                      bDontIncludeSubFolder = true;
                }

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
			if(sOutputFile.Length > 0 && sOutputFile[sOutputFile.Length-1] == '/') {

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
                if(sOutputFile.Length > 0) {
				    oParent.fAddPrjDirectoryOutput( new SrcDiry(Path.GetDirectoryName(sOutputFile),""));
                }
				if( bHaveDirectorySource  && !bHaveEntrySource && !bHaveMultipleFileSrc) {
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

            if(_sOutput == "") {
                return;
            }
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
					//Console.WriteLine("!!!!!!!!!!!!!!!!-**-*-*!_pOutput_Bin" + _sDirectory);
					oParent.fSetVar("_pOutput_Bin", sOutDirectory);
					//fSetVarCmd("_pOutput_Bin", "wOutBin"+_sDirectory);
					
				//	Console.WriteLine("!! " + 	Data.fGetGlobalVar("_pOutput_Bin"));


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
		
		public void fGetAppTypeFromPlatform() { ///TODO dynamic

			bToAnyType = true;
             string _sBinExt  = "";
            if(oCompiler != null) {
              _sBinExt  =   oCompiler.oGblConfigType.fGetNode(null,new string[]{"Config", "BinExt"}, "");
              // Output.TraceWarning("_sBinExt " +  _sBinExt );
            }
            if(_sBinExt == "") {
			    switch(fGetVar("_sPlatform")){
                    case "Web_Emsc":
					    sToAnyType = ".html";
				    break;
                    // case "Windows":
                    // case "CpcDos":
                    default:
					    sToAnyType = ".exe";
				    break;
			    }
            }else{
                if(_sBinExt[0] == '.') {
                   sToAnyType = _sBinExt;
                }else {
                   sToAnyType = "." + _sBinExt;
                }
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

       public void fSetVar(string _sName, string _sArg, bool _bAssingOnEmpty = false) {
           oParent.fSetVar(_sName, _sArg, _bAssingOnEmpty);
        }



        public string fGetStringVar(string _sVarWithBracket) {

             int  _nIndexStart = _sVarWithBracket.IndexOf('{');
            if (_nIndexStart == -1) {return "";}
            int  _nIndexEnd = _sVarWithBracket.IndexOf('}',_nIndexStart);
            if (_nIndexEnd == -1) {return "";}
            return _sVarWithBracket.Substring(_nIndexStart+1, _nIndexEnd - _nIndexStart-1).Trim();
        }


        public int fPre_SetCwcVar(string _sVar, string _sArg, bool _bAssingOnEmpty) {
             if(_sVar == ""){return 0;}

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
		//	string[] _aCmd = _sArg.Split('=', ' ');
			string[] _aCmd = _sArg.Split('=');


            if(_aCmd.Length > 1) {
                _sMainValue = _aCmd[1].Trim();
            } else {
                return 0; //No '='
            }
            string  _sFullVar = _aCmd[0];
            
           */
            
          //  string _sVar = fGetStringVar(_sArg);
         //   if(_sVar == ""){return 0;}
                /*
            int  _nIndexStart = _sFullVar.IndexOf('{');
            if (_nIndexStart == -1) {return;}
            int  _nIndexEnd = _sFullVar.IndexOf('}',_nIndexStart);
            if (_nIndexEnd == -1) {return;}
            string _sVar = _sFullVar.Substring(_nIndexStart+1, _nIndexEnd - _nIndexStart-1).Trim();
            */

            bool _reAddUnderscore = false;
            if(_sVar[0] == '_') { //Do no remove first undescore
                _sVar = _sVar.Substring(1);
                _reAddUnderscore = true;
            }
            string[]   aVar = _sVar.Split('_');
            if(_reAddUnderscore) {
                 _sVar = "_" + _sVar;
                aVar[0] = "_" + aVar[0];
            }
    

           string _sVarArg = fExtracVals(_sArg);
           // string _sVarArg = _sArg;
            fSetVar(_sVar, _sVarArg, _bAssingOnEmpty);


            _sVarArg = CppCmd.fExtractVar(_sVarArg, null);
			 // switch (_sVar){
           // string _sCmd = aVar[0];
			  switch (aVar[0]){


				 case "wCwcUpd":
                    fwCwcUpd(_sVarArg);
                 break;
				case "wCwcVer":
                    fwCwcUpdVer(_sVarArg);
                 break;
				case "wCwcUpdated":
                    fwCwcUpdated(_sVarArg);
                 break;

                case "_wToolchain":
					//oParent.fSetVar(_sCmd, _sMainValue);
					f_wToolchain( _sVarArg);
                break;

				case "_wLib": //TODO multilib, ","
					fwLib(_sVarArg, _sVar);
				break;
				
				 case "wNoColor":
                    Data.bColor = false;
                 break;

				 case "wNoGUI":
                    Data.bGUI = false;
                 break;

				 case "wBuildAnd":
                    //fBuildAndCommand(_sArg);
					oParent.fSetVar(_sVar, _sVarArg);
                 break;
            	 case "_sOpt":
                    //fBuildAndCommand(_sArg);
					oParent.fSetVar(_sVar, _sVarArg);
                 break;

                case "_sType":
                    //fBuildAndCommand(_sArg);
					oParent.fSetVar(_sVar, _sVarArg);
                 break;
				
				case "wArch":
				case "wArchPC":
					oParent.fSetVar(_sVar, _sVarArg);
				 break;
				case "_sPlatform":

					oParent.fSetVar(_sVar, _sVarArg);
			//		Debug.fTrace("_sPlatform : " + _sMainValue);
			//		Debug.fTrace("_wToolchain : " + oParent.fGetVar("_wToolchain"));
					oParent.fAddCompiler(oParent.fGetVar("_wToolchain"), _sVarArg); ///Force create CompilerData ex: detect Emscriptem, maydo do a list?
				//	oParent.aCompilerList.Add(	Data.fGetCompiler, _sMainValue) );

				 break;

				case "_pProject":
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


            //_sFullValue?
            /*
            //Find endindex of next setting var
            int _nEndIndex = _sFullValue.IndexOf('=');
            if(_nEndIndex > 4) {//Min = -{nX}=  (5)
                if(_sFullValue[_nEndIndex-1] == '}') { 
                    while(_nEndIndex > 1 && _sFullValue[_nEndIndex] != '{' ) {
                        _nEndIndex--;
                    }
                    if( _sFullValue[_nEndIndex-1] == '-') { //TODO retry for next '}' if no '-'
                        return _nEndIndex +  _sFullVar.Length;
                    }
                }
            }
            */
            return 1; 
		}

        public static void fSetWorkingDir(ArgumentManager _oArg, string _sFullValue) {
                 
             _sFullValue = _sFullValue.Replace('\\','/');
            if (_sFullValue[_sFullValue.Length-1] != '/' ) {
                _sFullValue = _sFullValue + "/";
            }


            try  {  Directory.SetCurrentDirectory(_sFullValue.Replace('"', ' ') );  } catch (DirectoryNotFoundException e) { //TODO verifie order execution of SetCurrentDirectory
				Output.TraceError("{_pProject} not found: " + _sFullValue);
			}

          _oArg.fSetVar("_pProject", _sFullValue);
                       
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
                         //     Output.TraceWarning("_sCmdArg: " + _sCmdArg );
               //  Output.TraceWarning("sRet_ExtractSpaceMultiValsAltArg: " + sRet_ExtractSpaceMultiValsAltArg );

                    
                 sRunToArgDontPrextract_File = _sCmdArg;
                 sRunToArgDontPrextract_File_Arg = CppCmd.fExtractVar( sRet_ExtractSpaceMultiValsAltArg, null);
                 bRunToArgDontPrextract = true;
                  //if(oParent.bPreOutput_Link) {
                    //    oParent.bOverideRunCmd = true;
                  //}
               break;
				 
              case "#Launch":
              //    Debug.fTrace("--!!Launch " + _sArg);
                fCmdwLaunch(_sCmdArg);
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
             case "#Test":
                 	fTestCmd(_sCmdArg);
             break;



             case "#Obj":
                 	fPrObjCmd(_sCmdArg);
             break;
                //    fSetCompiler(_sArg);
                // break;
					
            }
		}

        private void fTestCmd(string _sCmdArg)
        {
           bTestCmd = true;
        }

        public string sObjListVar = "";
        private void fPrObjCmd(string sCmdArg) {
     
            if (sCmdArg[0] != '>') {
                Output.TraceError("Output object list to var require assing '=>{VarName}' : " + sPreCwcCommand);
            }   
           sObjListVar = fGetStringVar(sCmdArg);
        //    Output.TraceAction("sObjListVar + "  + sObjListVar);
               
        }

        private void fCmdwLaunch(string _sToLaunch) {

            //Create settings file


            bSendCmdToLaunch = true;
            sToLaunch = _sToLaunch;

          //  Setting.fNewSettingsLaunch(_sToLaunch);
           Data.sToLaunch = _sToLaunch;//Broken??

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

                  case "#CloseWhen":
             
                     sCloseWhen = sRet_ExtractFullArg;
                             //  Output.TraceWarning("CloseWhen: "+ sCloseWhen);
                  break;

                   case "#RetryForInput":
                     sRetryForInput = _sCond;
                     bRetryForInput = true;
                  break;

				 case "#Copy":
                    fCmdCopy(_sCond + " " + sRet_ExtractSpaceMultiValsAltArg);
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

            if(_sResult.Length == 0) {
                return "";
            } 
			
			if(_sResult[0] == '\"') {
			    int _nIndex = _sResult.IndexOf("\"", 1)-1;
                if(_nIndex >= 0) {
                    return _sResult.Substring(1,_nIndex); 
                }
                  Output.TraceError("Missing end '\"' for " + _sResult);
                 return _sResult.Substring(1); 
				
			} 
			else if(_sResult[0] == '\'') {
		        int _nIndex = _sResult.IndexOf("\'", 1)-1;
                if(_nIndex >= 0) {
                    return _sResult.Substring(1, _nIndex); 
                }
                Output.TraceError("Missing end \"'\" for " + _sResult);
                return _sResult.Substring(1); 
			//	return _sResult.Substring(1, _sResult.IndexOf("\'", 1)-1);

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



             public string fExtracVals(string _sValue, char _cRequiredDelim='=' ) {//_cRequiredDelim??

                string _sResult = _sValue; 
                    //Remove quote!!
                if (_sResult[0] == '\"') {
                    _sResult = _sResult.Substring(1);
                            
                    for(int i =0; i < _sResult.Length; i++){
                        if(_sResult[i] == '%'){
                            i++; //Skip next, % mean next char is a special
                        }else if(_sResult[i] == '\"'){
                            _sResult = _sResult.Substring(0, i);
                        }
                    }
                }
                return _sResult;
        }


        public string sRet_ExtractSpaceMultiValsCmd;
        public string sRet_ExtractSpaceMultiValsAltArg;
        public string sRet_ExtractFullArg;
        public string fExtractSpaceMultiVals(string _sValue, char _cRequiredDelim='=' ) {//_cRequiredDelim??

            sRet_ExtractSpaceMultiValsAltArg = "";
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
             }else {
                _sResult = _sValue.Trim();
            }
            //Output.TraceWarning("Result: " + _sResult);
            sRet_ExtractFullArg = _sResult;

            if(_sResult.Length >= 2){
                    //Remove quote!!
                int _nIndexEnd = -1;
                if (_sResult[0] == '\"') {
                    _sResult = _sResult.Substring(1);
                     _nIndexEnd = _sResult.IndexOf('\"');
                    sRet_ExtractFullArg = _sResult.Substring(0, _nIndexEnd);
                }else{
                     _nIndexEnd =  _sResult.IndexOf(' ');
                }
                if(_nIndexEnd != -1){
                    sRet_ExtractSpaceMultiValsAltArg = _sResult.Substring(_nIndexEnd + 1).Trim();
                    _sResult = _sResult.Substring(0, _nIndexEnd);
                }
                
            }
         

            if(_nIndex != -1) {
                    sRet_ExtractSpaceMultiValsCmd = _sValue.Substring(0,_nIndex).Trim();
            }else {
                    sRet_ExtractSpaceMultiValsCmd= _sValue.Trim();
            }


            return _sResult;
        }




        internal void fAddCommandLineVerificationToDepedancesFile(CppCmd _oCmd) {
            string sExtension = System.IO.Path.GetExtension(sOutputFile);
            string sPath = sOutputFile.Substring(0, sOutputFile.Length - sExtension.Length) + ".d";
            if(File.Exists(sPath)) {
                //ReadDependandece?

                string _sAddToDepandance= "Cwc:" + _oCmd.sCompiler + " " + _oCmd.sConfig_Type  + " " + _oCmd.sExeCmdUnique + " >> " +  sRecompileOnChangeCmd;

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



	public void f_wToolchain( string _sArg) {
        Data.bToolchainDefined = true;

        string[] _aArg = _sArg.Split(' ', '=', '/', ','); 
		if(_aArg.Length < 2) {
			Output.TraceError("{_wToolchain} Require argument: \"[Server]/Author/Name/[Version]\" : " + _sArg );
		}else {

			string _sName  = _aArg[0] + "/" + _aArg[1];

             CompilerData _oCompiler = fAddToolchain(_sName, oParent);
             if (_oCompiler != null) { 
                _oCompiler.fSetVar(this);
            }

         }

      //  return null;
    }

	public static CompilerData fAddToolchain( string _sName, ArgumentManager _oManager, bool _bIsSubCompiler = false) {    
		
			string _sType ="";
			//if(_aArg.Length >= 3){
		//		_sPlatform = _aArg[2];
			//} 

		//	oParent.fSetVar("_wToolchain", _sName);
			//oParent.fSetVar("_sPlatform", _sPlatform);
         //   Output.TraceAction("Set _wToolchain " + _sName );

            int _nStart_Index = _sName.IndexOf('[');
            if(_nStart_Index>0) {
                int  _nEnd_Index = _sName.IndexOf(']');
                _sType = _sName.Substring(_nStart_Index+1, _nEnd_Index - (_nStart_Index)-1 );
                _sName = _sName.Substring(0, _nStart_Index);
            }

			Data.fAddRequiredModule(_sName,true);
		   return	_oManager.fAddCompiler(_sName, _sType, _bIsSubCompiler); ///Force create CompilerData ex: detect Emscriptem, maydo do a list?

   
	}

   ModuleData oLib = null;
	public bool bIsACmdLib = false;

	public void fwLib(string _sArg, string _sVar = "") {
       string[] _aArg = _sArg.Split(' ', '=', '/', ','); 
		if(_aArg.Length < 2) {
			Output.TraceError("{_wLib} Require argument: \"sAutor/sName/(sVersion)\"" );
		}else {
			bIsACmdLib = true;
		    oLib =	Data.fAddRequiredModule(_aArg[0] + "/" + _aArg[1]);
			oParent.fAddLib(oLib); //Todo multiple lib with separator ,
            oLaunchLib_Arg =  new ArgumentManager();
		}
        if(_sVar != "") {
                string _sVarPath = "_p" + _sVar.Substring(2);
                fSetVar(_sVarPath, oLib.sCurrFolder);
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

			
             LaunchTool _oGit =  new LaunchTool();
			 //_o7z.dExit = new LaunchTool.dIExit(fCompressionExit);
			_oGit.sWorkPath = PathHelper.ExeWorkDir;

			string _sFolderName = Path.GetFileNameWithoutExtension(sGitURL);


			if(!Directory.Exists(  PathHelper.ExeWorkDir + _sFolderName ) ) {
				
				_oGit.fLaunchExe( PathHelper.ToolDir + "git/cmd/git.exe", sGitCmd + " \""+  sGitURL + "\"", "", "", true);
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
				List<String> _aFiles =	FileUtils.GetAllFiles(_sDirectoryFolder, !bDontIncludeSubFolder);
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
							if(_sCond[0] == '!') { //Exclude folder or file
                                if(_sCond[_sCond.Length-1] == '/' && _sFile.Length >= (_sCond.Length -1)) {//It's a folder
                                    string _sIsInclude = _sCond.Substring(1); //TODO optimise!
                                    if(_sFile.IndexOf(_sIsInclude) == 0) {
                                         _bInclude = false;
                                    }
                                }else { //It's a file
								    if(_sCond.Length -1 == _sFile.Length && _sCond.IndexOf(_sFile) != -1) { //Exclude file if same
									    _bInclude = false;
								    }
                                }

							    
							}
                            /*
                            if(_sCond[0] == '&') { //Exclude folder
                                //Don't include subfolder
                                bDontIncludeSubFolder = true;
                            }*/


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
                                case ".mm":
								case ".gcpp":
                                    if(_sExt == ".mm"&&Data.fGetGlobalVar("_sType") != "Bash") break;

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
									
                                    string extra_define =" -DD_FILE_NAME=" + _sName + " -DD_FILE_ID=" + _sSubFolder.Replace("\\","") .Replace("/","_") + _sName;

								//	string _sCmd = " " + sCallerCmd + " " + sSubResidualArg + " " +  sSubArg + " " + _sForceLang + " -c " +_sFile + " -o " + _sOutputFolder + _sSubFolder  + _sName +".o";
									string _sCmd = " " + sCallerCmd.Replace("#To", "") + " " + _sForceLang + " -c " +_sFile + " -o " + _sOutputFolder + _sSubFolder  + _sName +".o" + extra_define;
									
                
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
        private bool bSendCmdToLaunch;
        private string sToLaunch;
        private string sRunToArgDontPrextract_File;
        private string sRunToArgDontPrextract_File_Arg;
       // private string sHasIncluded = "";
        public bool bRunToArgDontPrextract;
        public string sRunToArgDontPrextract_Arg = "";
        public string sHasIncluded_Arg = "";
        public string sHasIncluded_True = "";
        public string sHasIncluded_False = "";



         public Dictionary<string, string> aObjList = new Dictionary<string, string>();

        internal string sObjList = "";
        internal string sObjUpToDate = "";
        private bool bTestCmd = false;
        public Process oToInputProcess = null;
        public Process oCurrProcess = null;
        public string sCloseWhen = "";
        public string sRetryForInput = "";
        public bool bRetryForInput = false;
        internal string sLaunchCmdResult;

        public  string fExtractVar( string _sCmd, bool _bWeak = false) {
			return CppCmd.fExtractVar(_sCmd, this, _bWeak);
		}

		public  static string fExtractVar( string _sCmd, CppCmd _oCmd, bool _bWeak = false ) {
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
                        if(  !(
                            (_sCmd[_nIndexPrevChar] == '-' && _sCmd[_nIndexNextChar] == '=')  
                        ||  (_sCmd[_nIndexPrevChar] == '-' && _sCmd[_nIndexNextChar] == ':'    && _sCmd[_nIndexNextChar + 1] == '=')  
                        ||  (_sCmd[_nIndexPrevPrevChar] == '=' && _sCmd[_nIndexPrevChar] == '>') 
                            ) ){ //Set a var (do not remplace)
                            

						    string _sOriVar = _sCmd.Substring(_nIndexStart + 1, _nIndexEnd - ( _nIndexStart  + 1));
						    string _sVar = "";
						    if(_oCmd != null){
							    _sVar = _oCmd.fGetVar(_sOriVar, _bWeak);
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
			
			return _sCmd.Trim();		
		}
		
/*
		public  static string fGetVar(ArgumentManager _oManager, string _sVar) {
			return _oManager.fGetVar(_sVar);
		}
*/
		public  string fGetVar(string _sVar, bool _bWeak = false) {
			return oParent.fGetVar(_sVar,_bWeak);
		}
/*

		public  string fGetVar(string _sVar) {
			switch(_sVar) {
				case "wArch":
					return "x32";
				break;
				case "_sPlatform":
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
           // Output.TraceAction("COPY:! " + _sAllArg);

            string[] _aArg =  _sAllArg.Trim().Split(' ');
            string _sFirst = _aArg[0];
            string _sSecond = "";
            for (int i = 1; i < _aArg.Length; i++)  {
                _sSecond = _aArg[i].Trim();
                if (_sSecond != "")  {
                    break;
                }
            }

            if (_sSecond != "") {
				FileUtils.CopyFolderContents(_sFirst, _sSecond); //TODO on run pass only?
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
            if(_oArg == null){
				 _oArg = new ArgumentManager(oParent);
			}
            return fNewArgCmdRunIt( _sAllArg, bRun, _oArg, bFile, _sSendArg);
        }


		public static ArgumentManager fNewArgCmdRunIt( string _sAllArg, bool bRun = true,  ArgumentManager _oArg = null, bool bFile = true, string _sSendArg = ""){	


 
            string _sFile = _sAllArg;
            /*
			if(_oArg == null){
				 _oArg = new ArgumentManager(oParent);
			}*/
			_oArg.bSubArgMan = true;
            //Data.bDontExecute = true;

            if (bFile) {
                _sAllArg = _sSendArg + "@" + _sAllArg;
            }

          

			_oArg.ExtractMainArgument( ArgProcess.fExpandAll(_sAllArg), !bRun);
             // Output.TraceWarning("#Run " + _oArg.sAllArg );



       //     	_oArg.fCompleteExtractMainArgument(null,false);

			if(bRun && !Data.bModuleIsRequired){
              // Output.Trace("\f7F#Run " + _sFile + " : \f78" +  _oArg.sAllArg );
                _oArg.fCompleteExtractMainArgument(null,false);
				_oArg.fExtract(null);
				_oArg.fRun(null, false, false);
              // Output.Trace("\f7F#End " +_sFile );
			}
      
			return _oArg;
		}



	public void fCmdRun(string _sAllArg, string _sSubArg = ""){
		    
            string _sRealArg = _sAllArg.Replace("#Run", "").Trim();
          //  Output.TraceWarning("aaaa: " + _sAllArg );

			string _sExt = Path.GetExtension(_sAllArg).ToLower();
			switch(_sExt) {
				case ".cwc":
                case ".cwmake":
						Debug.fTrace("Expand!!!!!: " +  _sAllArg);
					//	Debug.fTrace("Expand!!!!!: " +  Data.fExpand("@" + _sAllArg,0));
						fNewArgCmdRun(_sAllArg,true,null,true,sRunToArgDontPrextract_Arg + " > " );

						/*
						string _sFile = ;
						CppCmd _oSubCmd =  new CppCmd(oParent, _sFile , true);
						_oSubCmd.fExtract();
						aSubCmd.Add(_oSubCmd);*/
					
				break;
			
				case ".bat": //old method? not sure
				   Debug.fTrace("Found WRun, delocalising ... ");
				    Delocalise.fDelocalise(_sAllArg);
                    
				break;
                default:
                  
                   Data.oLaunchProject.fLaunchDefaultRun(_sRealArg, _sSubArg);
            
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
