using cwc.Compiler;
using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace cwc
{


	public class CompilerData {


		public  bool bIf_Compiler = false;
		public  CompilerData oCurr_If_Platform = null;

		public  bool bInside_Conditinal_Compiler = false;
		public  bool bInside_Conditinal_Platform = false;


		public static bool bRequireEmsc = false;



        	public string sLink_Action_src = "";
		    public string sLink_Action_cmd = "";


            public string sCmd = "";
            public string sExe_Debugger = "";
            public string sExe_Sanitizer = "";



		public  bool bExtracted = false;
		public  bool bPreExtracted = false;

        public string sSubCompiler = "";

       public Dictionary<string, CompilerData> aSubCompiler = new Dictionary<string, CompilerData>();

       public Dictionary<string, CompilerData> aPlatformCompilo = new Dictionary<string, CompilerData>();

       public Dictionary<string, ConfigType> aConfigType = new Dictionary<string, ConfigType>();


		public List<String> aExe_Preload = new List<string>();
		public List<String> aExe_ViewIn = new List<string>();
		public List<String> aRequireTC = new List<string>();
      




		public  CppCmd oCmdH = null;


        public bool bLoaded = false;
        public CompilerData oParent ;

        public ConfigType oCurrentConfigType;
        public ConfigType oGblConfigType;

		public ModuleData oModuleData;

		public string sPlatformName = "";

		public string sFilePath;
		public string sSubName = "";
		//public string sSubName = "";

		public string sFullName;
		public string sConditionalName = "";

      //  public CompilerData(string _sCompiler, CompilerData _oParent = null) {
        public CompilerData(ModuleData _oModuleData, string _sFilePath, string _sConditionalName = "") {
				          
      

                oModuleData = _oModuleData;
				sConditionalName  = _sConditionalName;
				if(_sFilePath != ""){
					sFilePath =  _sFilePath;
					sFullName = Path.GetFileName(_sFilePath);
				//	sFullName = sFullName.Substring(10); //Remove _wToolchain_
				//	Debug.fTrace("_sFullName " +  sFullName);
					sSubName = sFullName;
					int _nIndex = sSubName.IndexOf("."); //.cwfg file name -> LibRT.Default.cwfg = Default
					if(_nIndex != -1){
						sSubName =  sSubName.Substring(_nIndex + 1);//10 = _wToolchain_
					}
					 _nIndex = sSubName.LastIndexOf(".");///Remove .xml
					if(_nIndex != -1){
						sSubName =  sSubName.Substring(0,_nIndex);
					}
					Debug.fTrace("Compilo: " +  sSubName);
				}
				
		
                oGblConfigType = new ConfigType(this, "",null);
                aConfigType.Add("" ,oGblConfigType );

				/*
				if(oParent != null) {
	
					sSubCompiler = _sCompiler;
					sCompiler = _oParent.sCompiler;
					//	fSetValueToParent();
					//fSetSubCompilerType();
				}else {
				   sCompiler = _sCompiler;
					fAddSubCompilo();
					//fSetCompilerType();
				}*/

        }

		public void fExtract(ArgumentManager _oArg = null) {

			if(!bExtracted) {
				bExtracted = true;	
		
				fLoadConfig();
				fPerformSpecialCase(_oArg);
	
				/*
				if(oParent != null) {
					fSetValueToParent();
					fSetSubCompilerType();
				}else {
					fSetCompilerType();
				}*/
			}
		}

		void fPerformSpecialCase(ArgumentManager _oParamArg ){
			///Load all prelaod exe
			///
//Debug.fTrace("aaaaaasss : " + oModuleData.sAutorName);
					//		Output.TraceGood(aExe_Preload.Count.ToString());

            

 	        foreach(string _sRequire  in aRequireTC ){
				//Output.TraceGood(_sPath);
				fRequireModule(_oParamArg, _sRequire);
			}
          

			foreach(string _sPath  in aExe_Preload ){
				//Output.TraceGood(_sPath);
				fPreloadExe(_sPath);

			}
			foreach(string _sPath  in aExe_ViewIn ){
				//Output.TraceGood(_sPath);
				fViewIn(_sPath);
				
			}


           if(oModuleData.bIsCompiler && sCmd != "") {

               ArgumentManager  _oArg = new ArgumentManager(Data.oArg);
              // CppCmd.fNewArgCmdRunIt(sCmd,false,_oArg,false);
                _oArg.ExtractMainArgument( ArgProcess.fExpandAll(sCmd), true);
                   _oArg.fCompleteExtractMainArgument(null,false);
				_oArg.fExtract(null);
				_oArg.fRun(null, true, false, true); //Warning if "_bDontExecute = true" , bug with GZE or other lib?

               //  _oArg = new ArgumentManager(oParent);
               /*
                ArgumentManager  _oArg =  new ArgumentManager(Data.oArg);
			
                _oArg.fCompleteExtractMainArgument(null,false);
				_oArg.fExtract(null);
				_oArg.fRun(null, false, false);*/
                  //      Console.WriteLine("***********************************aaa " + oLib.oLibData.sCmd );
            //  fNewArgCmdRun( sCmd , false,oLauchLib_Arg,false); //Not run
              // fRunLib();
        //          Console.WriteLine("havec commmand!! " + oLib.oLibData.sCmd);
            }

            
/*
			switch(oModuleData.sAutorName){
				case "Honera/WebRT":
					Emscripten.oCmp= this;
//	Debug.fTrace("aaaaaasss : " + oModuleData.sAutorName);
					Emscripten.fCreateEmscConfig(true);
				break;
				
			}*/
		}

        private void fRequireModule(ArgumentManager _oParamArg , string _sPath) {

             CppCmd.fAddToolchain(_sPath, _oParamArg);
        }

        public void fMerge() { 
            foreach(string _sModule in aRequireTC) {

               // CompilerData _oCompiler =    Finder.fGetCompiler(_sModule);


           }
        }







        internal void fFinalize(){
			if(oCurrentConfigType != null && oCurrentConfigType.sFinally_CopyFolder!=""){
				string _sVar =  CppCmd.fExtractVar(oCurrentConfigType.sFinally_CopyFolder, null);
				
	
				string _sExclude = "";
				string[] _aInputEx = _sVar.Split(new string[] { "!>" }, StringSplitOptions.None); 
				if(_aInputEx.Length >= 2){
					_sVar = _aInputEx[0].Trim();
					_sExclude = _aInputEx[1].Trim();
				}


				string[] _aInput = _sVar.Split(new string[] { "->" }, StringSplitOptions.None); 
				if(_aInput.Length >= 2){
					string _sInputFolder = Path.GetDirectoryName( _aInput[0].Trim())+ "/";
					string _sOutFolder =  Path.GetDirectoryName(_aInput[1].Trim()) + "/";
					FileUtils.CopyFolderContents(_sInputFolder, _sOutFolder,"*.*", _sExclude);

					
					
				}

				//string _sInputFolder =   Path.GetDirectoryName(oCompiler.sLink_Action_src).Replace("\\", "/") + "/";
				//string _sOutFolder =  PathHelper.ExeWorkDir + sOutDirectory;
			}
			
		}

   

        void fPreloadExe(string _sPath){
		Output.TraceWarning("PreloadExe: " + _sPath);
		LauchTool _oPreload = new LauchTool();
		_oPreload.bWaitEndForOutput = true;

			_oPreload.bRunInThread = false;
            _oPreload.bHidden = true;
		_oPreload.dOut = new LauchTool.dIOut(fOut);
		_oPreload.dError = new LauchTool.dIError(fOut);
		_oPreload.fLauchExe(oModuleData.sCurrFolder +  _sPath,"");
		//_oPreload.dExit = new LauchTool.dIExit(fUrlRequestComplete);
		Output.TraceWarning("------------------------");

	}




	public static void 	fOut(LauchTool _oTool, string _sOut){
	
		CppCmd _oCmd = new CppCmd(Data.oCompilerArg, _sOut );
		_oCmd.fPreExtract();//TODO double call?
		Output.Trace( "\f46" + _sOut);
	}


	void fViewIn(string _sPath){
		Output.TraceWarning("ViewIn: " + _sPath);
		LauchTool _oViewIn = new LauchTool();
		_oViewIn.bWaitEndForOutput = true;

		_oViewIn.bRunInThread = false;
         _oViewIn.bHidden = true;
		_oViewIn.dOut = new LauchTool.dIOut(fOutViewIn);
		_oViewIn.dError = new LauchTool.dIError(fOutViewIn);
		_oViewIn.fLauchExe(oModuleData.sCurrFolder +  _sPath,"");
		//_oPreload.dExit = new LauchTool.dIExit(fUrlRequestComplete);
		Output.TraceWarning("------------------------");
	}

	public static void 	fOutViewIn(LauchTool _oTool, string _sOut){
	
		CppCmd _oCmd = new CppCmd(Data.oCompilerArg, _sOut );
		Output.Trace( "\f47" + _sOut);

		string[]  _aLines = _sOut.Split('*');
        foreach (string _sLine in _aLines)   {
			if(!String.IsNullOrEmpty( _sLine)){
				fFoundBrowser(_sLine.Replace('\n',' ').Trim());
			}
        }
	}

	public static void 	fFoundBrowser(string _sLine){

        string[] _aBrowser = _sLine.Split(new string[] { ": " }, StringSplitOptions.None);
        if (_aBrowser.Length >= 2){
			Data.fAddBrowser(_aBrowser[0],_aBrowser[1]);
			
		}
	}

/*
		private void fSetValueToParent(){
				sPath =  oParent.sPath;
				sCpp =  oParent.sCpp;
				sC =  oParent.sC;
				sLinker =  oParent.sLinker;
				sLinkerS =  oParent.sLinkerS;
				sLinkerD =  oParent.sLinkerD;
				sRC =  oParent.sRC;
				sWebsite =  oParent.sWebsite;

				sArgCpp =  oParent.sArgCpp;
				sArgC =  oParent.sArgC;
				sArgLinkerS =  oParent.sArgLinkerS;
				sArgLinkerD =  oParent.sArgLinkerD;
				sArgRC =  oParent.sArgRC;

				sHArgCpp =  oParent.sHArgCpp;
				sHArgC =  oParent.sHArgC;
				sHArgLinker =  oParent.sHArgLinker;
				sHArgLinkerS =  oParent.sHArgLinkerS;
				sHArgLinkerD =  oParent.sHArgLinkerD;
				sHArgRC =  oParent.sHArgRC;
		}*/


          public string sNodeCurrentType = "";
		 public Dictionary<string,  Dictionary<string,string>  > aNode = new Dictionary<string,  Dictionary<string,string>  >();
		public void fLoadConfig() {
            aNode = new Dictionary<string,  Dictionary<string,string> >();
           // string _sPath =  PathHelper.GetExeDirectory() +  "Config/Compiler/" + sCompiler + ".xml";
            //rogram.fDebug("Load Config: " + sFilePath);
            //rogram.fDebug("---------------------------------------Load Config: " + sFilePath);

			//Reset value



            /*
			sCompiler = "";
			sCompilerLinker = "";
			sCpp = "";
			sCWayv = "";

			sC = "";
			sLinker = "";
			sLink_Static = "";
			sLink_Dynamic = "";
			sRC = "";*/

            string _sCurrentNode = "";
            bool _bRootNode = true;

            XmlTextReader reader = null;
            XmlTextReader lastNode = null;
            if(File.Exists(sFilePath)) { 

            try {
						
                        reader = new XmlTextReader(sFilePath);
                        reader.WhitespaceHandling = WhitespaceHandling.None;
                        while (reader.Read())  {
                            
							CompilerData _oCurrentRead = this;
							if(oCurr_If_Platform != null){
								_oCurrentRead = oCurr_If_Platform;
							}
                     

                            if(!_bRootNode){

                                switch (reader.NodeType)
                                {
                                    case XmlNodeType.Element:
                                           _sCurrentNode =  reader.Name;
									    _oCurrentRead.fAddNode(reader, _sCurrentNode);
							
                          //  lastNode = reader;
									
								    //   Debug.fTrace("---------_sCurrentNode " + _sCurrentNode);
                                        break;

                                    case XmlNodeType.Text: //Display the text in each element.
                                   
                                        _oCurrentRead.fLoadNode(reader, aNode, _sCurrentNode, reader.Value);
                                        sNodeCurrentType = "";
                                        break;

                                    case XmlNodeType.EndElement:
									    _oCurrentRead.fRemoveNode( reader.Name);
									

									    //	Debug.fTrace("end " +   reader.Name);
										
                                        _sCurrentNode = "";
                                        break;
                                }

                            } else {
                                _bRootNode = false;
                            }



                        }

                }
                finally {
                    if (reader != null) {
                        reader.Close();
                    }
                }
              }

				fFinishLoadConfig();
               bLoaded = true;
          }       
       
		public void fFinishLoadConfig(){
			
			if(oCurrentConfigType.sExe_Cpp == ""){
				oCurrentConfigType.sExe_Cpp = oCurrentConfigType.sExe_Compiler;
			}
			if(oCurrentConfigType.sExe_C == ""){
				oCurrentConfigType.sExe_C = oCurrentConfigType.sExe_Cpp;
			}
			
		}



		string sNode_Arch = "";
		bool bNode_Exe = false;
		bool bNode_Config = false;
		bool bNode_CfgType = false;

		 public void fAddNode(XmlTextReader _oNode, string _sName){

            //Load Config
            if(!bNode_CfgType){ //New config set in parent node -> don't reset it
               oCurrentConfigType = oGblConfigType;
            }

            Node _oCurTypeNode =  oGblConfigType.fAddNode(_sName);
          //  _oCurTypeNode.fSetValue("",""); //default node

           for (int nIdx = 0; nIdx < _oNode.AttributeCount; nIdx++){
		    	_oNode.MoveToAttribute( nIdx );
		        switch(_oNode.Name.ToLower()){
			        case "type":
                      //      Console.WriteLine("-******************_oNode.Name.ToLower() : " +_oNode.Name.ToLower());
				     //   string _sTypeName = _oNode.Value;
                          sNodeCurrentType =  _oNode.Value;
                       // if (!aConfigType.ContainsKey(sNodeCurrentType)) {
                        //    oCurrentConfigType = aConfigType[sNodeCurrentType];
                                       //   oCurrentConfigType.fCopyNodes(oGblConfigType.oCurrNode);
                      //  }else {
                            
                            if (!aConfigType.ContainsKey(sNodeCurrentType)) {
                                aConfigType.Add(sNodeCurrentType, new ConfigType(this,sNodeCurrentType, oGblConfigType));
                            }
                        //    oCurrentConfigType = aConfigType[sNodeCurrentType];

                              //  Output.TraceError("Type not found: " + _sName);
                                Console.WriteLine("Type not found: " + _sName);

                       // }
                         oCurrentConfigType = aConfigType[sNodeCurrentType];

			        break;
			    }
	    	}



			if (!aNode.ContainsKey(  _sName )){ //add last "branch" node
				aNode.Add(_sName, new Dictionary<string,string>() );
				switch(_sName){
					case "x16":
					case "x32":
					case "x64":
					case "x128":
					case "x256":
						if(sNode_Arch == ""){
							sNode_Arch = _sName;
						}else{
							Output.TraceError("_sPlatform config error: " + "Can't put arch type inside another arch" + " in " + sFilePath );
						}
					break;
				
				    case "Config":
						bNode_Config = true;
					break;

                    case "Type":
						bNode_CfgType = true;

                         for (int nIdx = 0; nIdx < _oNode.AttributeCount; nIdx++){
							  _oNode.MoveToAttribute( nIdx );
							 switch(_oNode.Name.ToLower()){
								case "name":
									string _sTypeName = _oNode.Value;
                                    if (!aConfigType.ContainsKey(_sTypeName)) {
                                        aConfigType.Add(_sTypeName, new ConfigType(this,_sTypeName, oGblConfigType));
                                    }
                                     oCurrentConfigType = aConfigType[_sTypeName];
								break;
							 }
						  }
                        
					break;


					case "Exe":
						bNode_Exe = true;
					break;


				
					case "If_Compiler":
						bInside_Conditinal_Compiler = true;
						 for (int nIdx = 0; nIdx < _oNode.AttributeCount; nIdx++){
							  _oNode.MoveToAttribute( nIdx );
							 switch(_oNode.Name.ToLower()){
								case "name":
									string _sAutorName = _oNode.Value;
									if(_sAutorName == oParent.oModuleData.sAutorName){
										bIf_Compiler = true;
									}
								break;
							 }
						  }
					break;

					case "If_Platform":
						bInside_Conditinal_Platform = true;
						 for (int nIdx = 0; nIdx < _oNode.AttributeCount; nIdx++){
							  _oNode.MoveToAttribute( nIdx );
							 switch(_oNode.Name.ToLower()){
								case "name":
                               	case "type":
									string _sType= _oNode.Value;
									//Debug.fTrace("!!!!!!!!!!!!!!!!!!!!!!!!!FoundType!!!! :  " + Data.fGetGlobalVar("_sPlatform") );
									
									oCurr_If_Platform =	fGetConditionalPlatformCompilo(_sType);
									/*
									if(_sType ==  Data.fGetGlobalVar("_sPlatform") ){
										bIf_Platform = true;
									}*/
									
								break;
							 }
						  }
					break;
						
					case "Platform":
						//Debug.fTrace("Plat node found: ");
						 for (int nIdx = 0; nIdx < _oNode.AttributeCount; nIdx++){
							  _oNode.MoveToAttribute( nIdx );
							 switch(_oNode.Name.ToLower()){
								case "type": //To remove
								case "name":
									sPlatformName = _oNode.Value;

								break;
							 }
						  }
					break;
				}
			}
		}

		internal void fSetVar(CppCmd cppCmd)
		{
			cppCmd.oParent.fSetVar("_wToolchain", oModuleData.sAutorName);
			cppCmd.oParent.fSetVar("_wToolchain_Dir", oModuleData.sCurrFolder);
			cppCmd.oParent.fSetVar("_sPlatform", sPlatformName);

         //   Output.TraceAction("_wToolchain " +  oModuleData.sAutorName);
         //   Output.TraceAction("_sPlatform " + sPlatformName);

			//cppCmd.oParent.fSetVar("wPlatform_Type", sType);
			cppCmd.oParent.fSetVar("_sConfig_Type", sSubName); //.cwfg file name -> LibRT.Default.cwfg = Default
//Debug.fTrace("SetVar: " + sSubName);
//Debug.fTrace("SetVar: " + sType);
		}

		public void fRemoveNode(string _sName){
			if (aNode.ContainsKey( _sName )){
				switch(_sName){
					case "x16":
					case "x32":
					case "x64":
					case "x128":
					case "x256":
						sNode_Arch = "";
					break;
					case "Exe":
						bNode_Exe = false;
					break;
                    case "Config":
						bNode_Config = false;
					break;
                    case "Type":
						bNode_CfgType = false;
					break;
					case "If_Compiler":
						bIf_Compiler = false;
						bInside_Conditinal_Compiler = false;
					break;
					case "If_Platform":
						oCurr_If_Platform = null;
						bInside_Conditinal_Platform = false;
					break;
			
					
				}

                oGblConfigType.fRemoveNode(_sName);
				aNode.Remove( _sName);
			}
		}


		public string sCurrentArch = "x32";

         public void fLoadNode(XmlTextReader _oNode, Dictionary<string,  Dictionary<string,string>  > _aNode, string _sCurrent, string _sValue) {
			/*
            for (int nIdx = 0; nIdx < _oNode.AttributeCount; nIdx++){
		    	_oNode.MoveToAttribute( nIdx );
                Console.WriteLine("-******************_oNode.Name.ToLower() : " +_oNode.Name.ToLower());
		        switch(_oNode.Name.ToLower()){
			        case "type":
				        string _sTypeName = _oNode.Value;

                        
			        break;
			    }
	    	}*/
            
    
            
            _sValue = fSpecialExtartVar( _sValue);



            oGblConfigType.fSetValue(_sValue, sNodeCurrentType);
         //   oCurrentConfigType.fSetValue(_sValue, sNodeCurrentType);




			if (sNode_Arch == "" || sNode_Arch == sCurrentArch ){

                if(bNode_Config) {
                    switch(_sCurrent){
                       case "Require":
                            aRequireTC.Add(_sValue);
                        break;
                    }

                }else if(bNode_Exe){
					switch(_sCurrent){

						case "C":
							oCurrentConfigType.sExe_C = _sValue;
						break;
						case "Cpp":
							oCurrentConfigType.sExe_Cpp = _sValue;
						break;
						case "Compiler":
							oCurrentConfigType.sExe_Compiler = _sValue;
						break;
						case "Linker":
							oCurrentConfigType.sExe_Linker = _sValue;
						break;
						case "Linker_Static":
							oCurrentConfigType.sExe_Link_Static = _sValue;
						break;
						case "Linker_Dynamic":
							oCurrentConfigType.sExe_Link_Dynamic = _sValue;
						break;
                    	case "Debugger":
							sExe_Debugger = _sValue;
						break;
                        case "Sanitizer":
							sExe_Sanitizer = _sValue;
						break;
                    
						case "Resource":
							oCurrentConfigType.sExe_RC = _sValue;
						break;
						case "Preload":
							aExe_Preload.Add(_sValue);
						break;
						case "ViewIn":
							aExe_ViewIn.Add(_sValue);
						break;

              



					}

                } else if( bNode_CfgType){

                    switch(_sCurrent){
						case "Extention":
                         ///   oCurrentConfigType.sExtention = _sValue;
                            oCurrentConfigType.fSetExtentions(_sValue);
							//sC += _sValue + " ";
						break;
                    }

				}else{
					switch(_sCurrent){


						case "C":
							oCurrentConfigType.sC += _sValue + " ";
						break;
						case "Cpp":
							oCurrentConfigType.sCpp += _sValue + " ";
						break;
                        case "CWayv":
                        case "CWave":
	                    case "CWift":
							oCurrentConfigType.sCWayv += _sValue + " ";
						break;
						case "Compiler":
							oCurrentConfigType.sCompiler += _sValue + " "; 
               
						break;
						case "CompilerLinker":
							oCurrentConfigType.sCompilerLinker += _sValue + " ";
						break;

						case "Finally_CopyFolder":
							oCurrentConfigType.sFinally_CopyFolder += _sValue + " ";
						break;

						case "Linker":
							oCurrentConfigType.sLinker += _sValue + " ";
						break;
						case "Linker_Static":
							oCurrentConfigType.sLink_Static += _sValue + " ";
						break;
						case "Linker_Dynamic":
							oCurrentConfigType.sLink_Dynamic += _sValue + " ";
						break;
						case "Resource":
							oCurrentConfigType.sRC += _sValue + " ";
						break;

                    	case "Debug":
							oCurrentConfigType.sDebug += _sValue + " ";
						break;

                    	case "cmd":
						    sCmd += _sValue + " > ";
					    break;

					}

				}
			}
        }

		public string fSpecialExtartVar(string _sValue){
			//_sValue = _sValue.Replace("{wPath}", oModuleData.sCurrFolder);
			_sValue = _sValue.Replace("{_pModule}", oModuleData.sCurrFolder);
			/*
			if(oModuleData.bIsCompiler){
				_sValue = _sValue.Replace("(_wToolchain_Dir)", oModuleData.sCurrFolder);
			}else{//IsLib
				_sValue = _sValue.Replace("(wLib_Dir)", oModuleData.sCurrFolder);
			}*/

			return _sValue;
		}
/*
		public void fSavecConfig() {

        }*/


        /*
		public void fAddSubCompilo() {

			switch(sCompiler) {
				 case "_LibRT_clang":
					fAddSubCompiler("Web_Emsc"); ///Only if found, when get?
				break;
				

			}
        }
        */

/*
		public void fSetCompilerType() {
			Debug.fTrace("fSetCompilerType: " +sCompiler );
			switch(sCompiler) {
				 case "_LibRT_clang":
					fSetCompilerClang(sClangAbiGCC );
					fSetLinkerGCC();
					//fAddSubCompiler("Web_Emsc"); ///Only if found, when get?
					
				break;
                case "_LibRT_mingw":
						fSeCompilerGCC();

				break;
                case "_LibRT_msvc":
						fSetCompilerClangMsvc(sClangAbiMscv);
						//fSetLinkerGCC();
						fSetLinkerClang();
 
				break;
			}
			
			//Debug.fTrace("New Compiler : " + sCompiler);
		}



		public void fSetSubCompilerType() {

			Debug.fTrace("fSetSubCompilerType: " + sSubCompiler);

			switch(sSubCompiler) {
				 case "Web_Emsc":
					bRequireEmsc = true;
					
					//If module not exist
					if(!Directory.Exists(Emscripten.sDirEmsc)) {
						Data.fAddRequiredModule("Honera/Emscripten");
						bExtracted = false;
						 return;
					}

					Emscripten.fCreateEmscConfig(true);

					fSetCompilerEmsc();
					//Debug.fTrace("Web_Emsc! FOUND");
				break;
			}
			//Debug.fTrace("New Compiler : " + sCompiler);
		}


*/
/*
        public  static string sLibRT_Dir = "";

        private static string sPathExeClang= "";
        private static string sPathExeGCC = "";
        private static string sPathExeC = "";
        private static string sPathExeAR = "";

		private static string sLinkerPathGCC = "";
        private static string sLinkerPathClang = "";

       private static string sInlude64_base = "";
       private static string sInlude64   = "";

       private static string sInlude     = "";
	   private static string sInludeClang  = "";
	  //private static string sInludeLib      = "-I " + sLibRT_Dir + "/Lib/ ";
    //   private static string sInludeLib      = " ";
    */

     //  private static string sStdInc = sInlude64_base + sInlude64 + sInlude;

        /*
		public static void fUpdateLibRTPath(string _sBasePath) {

		
			 sLibRT_Dir = _sBasePath;

			sPathExeClang = sLibRT_Dir + "bin/clang++.exe";
			sPathExeGCC = sLibRT_Dir + "bin/g++.exe";
			sPathExeC = sLibRT_Dir + "bin/gcc.exe";
			sPathExeAR = sLibRT_Dir + "bin/ar.exe";

			sLinkerPathGCC = sLibRT_Dir + "bin/g++.exe";
			sLinkerPathClang = sLibRT_Dir + "bin/clang.exe";
		
		
			sInlude64_base = "-I\"" + sLibRT_Dir + "lib/gcc/i686-w64-mingw32/5.4.0/include/\" ";
			sInlude64        = "-I\"" + sLibRT_Dir + "i686-w64-mingw32/include/\" ";

			sInlude      = "-I\"" + sLibRT_Dir + "include/\" ";
			sInludeClang  =  " -I\"" + sLibRT_Dir + "clang_inc/\" ";

			///fSetGZEpath(); //Depend on LibRT folder

			//TODO RESET ALL COMPILER PATHS
			
				//bExtracted = false;	->?
		}*/
         /*
		public static string sInludeGZE ="";
        public static string sIncludeGzeSubLibs = "";


        public  static string sGZE_Dir = "";
       
		public static void fUpdateGZEpath(string _sBasePath) {
			sGZE_Dir = _sBasePath;

				sInludeGZE = " -I\"" + sGZE_Dir + "\"";

				sIncludeGzeSubLibs = sInludeGZE.Substring(0,sInludeGZE.Length-1) + "SubLib_System/\" ";
				sInludeGZE =  sInludeGZE + " ";
			//	return sInludeGZE + " "; //And is sublibs
		}
    */

	

        //private  static string sSubCompilerLink = "Clang";  private static string sLinkerPath = sLibRT_Dir + "/bin/clang++.exe";
        //private  static string sSubCompilerLink = "GCC";  private static string sLinkerPath = sLibRT_Dir + "/bin/g++.exe";



     private  static string sClangAbiGCC = " -target i686-pc-mingw32 "; 
     private  static string sClangAbiMscv = " "; //Msvc; 


   /// ///////////////////////////////////  INCLUDE //////////////////////
   /// 
/// ////////////////// //// Always included  //////////////////////////
/// 


      //  private static string sInludeGZE = fSetLibRtFolder();

///  ////////////////// ////////////////// ////////////////////////////////////

    //   private static string sMinGwLib64  =  " -L" + sLibRT_Dir + "i686-w64-mingw32/lib64/ ";



/////////////////////////////////////////////////////////////////

		//bInConsole
		//  public static string sHiddenArg_sColoConsole = " -fdiagnostics-print-source-range-info -fcolor-diagnostics -fansi-escape-codes ";
		public static string sHiddenArg_sColoConsole = "-fcolor-diagnostics -fansi-escape-codes ";
/*
		private static string sHiddenArg_GCC = "-MD  -Werror=return-type ";
		private static string sHiddenArg_Clang = "-MD  -Werror=return-type -fno-rtti ";

		private static string sHiddenArg_ClangNoExeption = "-fno-exceptions ";


		private static string sCreateDFile = "-MD  -Werror=return-type "; //Werror=return-type alwasy on, remove it?
		private static string sNoRtti =  "-fno-rtti "; //Werror=return-type alwasy on, remove it?
		


		//   private static string sHiddenArg_All = " -MD -Werror=return-type ";
		private static string sClangHideWarning = "-Qunused-arguments -Wno-unused-value -Wno-deprecated-register -Wno-ignored-attributes -Qunused-arguments -Wno-expansion-to-defined -Wno-ignored-pragmas ";
		//private static string sHiddenArg_All = " -Qunused-arguments ";

   //   private static string sHiddenArg_Platform = sArch + "  -static-libgcc -static-libstdc++  -g  -lgdi32 -lopengl32  ";
      private static string sStaticStd =  "-static-libgcc -static-libstdc++ "; //Temp add gdi32
      private static string sDebugInfo =  "-g -fstandalone-debug "; //Temp add gdi32


		private static string sDefaultLib =  "-lgdi32 -lopengl32 ";

     // private static string sStaticStd =  "-static-libgcc -static-libstdc++  "; //Normal 


	   private  static string sCppVer = "-std=c++11 "; 
	   private  static string sArch = "-m32 "; 


        //private  static string sArch = "-m64 "; 
*/
		 /*

        public string sHArgCpp = "";
        public string sHArgC = "";
        public string sHArgLinkerS = "";
        public string sHArgLinkerD = "";
        public string sHArgRC = "";
 
 *//*
		public void fSetCompilerClangMsvc(String _sAbi) {
			if(!File.Exists(sPathExeClang)) {
		//		Output.TraceError(sCompiler + " exe not found " + sPathExeClang);
			}

			sCpp = sPathExeClang;
			sHArgCpp = sInludeClang + sStdInc + _sAbi   +  sHiddenArg_sColoConsole  + sClangHideWarning + sHiddenArg_Clang  + sArch + sCppVer; //sArch ToDO
			
			////////////////////
			 sHArgLinkerS  = "";
			sHArgC = sHArgLinker = sHArgLinkerD = sHArgRC = sHArgCpp; //Set all to clang by default
			sC = sLinker = sLinkerD = sLinkerS = sCpp; //Set all to clang by default
			sHArgC = "-x c " +  sHArgC; //Add C lang

			sLinkFinalAddLib = sDefaultLib;

		}*/


        /*
		public void fSetCompilerClang(String _sAbi) {  //TODO test exe path
			if(!File.Exists(sPathExeClang)) {
	//			Output.TraceError(sCompiler + " exe not found " + sPathExeClang);
			}
			

			sCpp = sPathExeClang;
			sHArgCpp = sInludeClang + sStdInc + _sAbi   +  sHiddenArg_sColoConsole  + sClangHideWarning   + sHiddenArg_Clang + sHiddenArg_ClangNoExeption + sStaticStd + sArch + sDebugInfo; //sArch ToDO //sDebugInfo TEMP!!!!
			
			////////////////////
			 sHArgLinkerS  = "";
			sHArgC = sHArgLinker = sHArgLinkerD = sHArgRC = sHArgCpp; //Set all to clang by default
			sC = sLinker = sLinkerD = sLinkerS = sCpp; //Set all to clang by default
			sHArgC = "-x c " +  sHArgC; //Add C lang


		}*/

        /*
		public void fSeCompilerGCC() {
	
			if(!File.Exists(sPathExeClang)) {
	//			Output.TraceError(sCompiler + " exe not found " + sPathExeGCC);
			}

			sCpp = sPathExeGCC;
			sHArgCpp = sHiddenArg_GCC + sStaticStd + sArch;  //sArch ToDO
			sLinkerS = sPathExeAR;
			
			////////////////////
			sHArgC = sHArgCpp; 
			sHArgCpp +=	sNoRtti; // no Rtti not work in C

			sC = sPathExeC;
			
			

			fSetLinkerGCC();
		}*/

	/*
		public void fSetLinkerGCC() {

			if(!File.Exists(sPathExeClang)) {
		//		Output.TraceError(sCompiler + " exe not found " + sPathExeGCC);
			}

			sLinker = sPathExeGCC;
			sLinkerS = sPathExeAR;
			
			sHArgLinker =  sStaticStd;
			sHArgLinkerD = sHArgLinker;
			sHArgLinkerS = "";

			sLinkerD  = sLinker;

			sLinkFinalAddLib = sDefaultLib;
		}*/

        /*
		public void fSetLinkerClang() {

			sLinker = sPathExeClang;
			sLinkerS = sPathExeAR;
			
//			sHArgLinker =  sStaticStd;
	sHArgLinker =  "-static ";

			sHArgLinkerD = sHArgLinker;
			sHArgLinkerS = "";

			sLinkerD  = sLinker;
			sLinkFinalAddLib = sDefaultLib;
		}*/


		public CompilerData fGetConditionalPlatformCompilo(string _sPlatform, bool _bNullIfNotFound = false){

			//If we have a platform specific compiler
			if (aPlatformCompilo.ContainsKey(_sPlatform)){
				return aPlatformCompilo[_sPlatform];
			}
			CompilerData _oCompilo = null;
			if(!_bNullIfNotFound){
				_oCompilo = new CompilerData(oModuleData,"",_sPlatform);
				aPlatformCompilo.Add(_sPlatform,_oCompilo);
			}
			return _oCompilo;
		}


             /*
		public CompilerData fGetSubCompiler(string _sPlatform){

			//If we have a platform specific compiler
			if (aSubCompiler.ContainsKey(_sPlatform)){
				CompilerData _oCompilo = aSubCompiler[_sPlatform];
				_oCompilo.fPreExtract();
				return _oCompilo;
			}
			fPreExtract();
			return this;
		}
   
		private void fPreExtract()
		{	
			if(!bPreExtracted) {
				bPreExtracted = true;
				
			}
		}*/
/*
		public void fAddSubCompiler(string _sPlatform) {	

		//	   aSubCompiler.Add(_sPlatform, new CompilerData(_sPlatform, this));
		}
        */

/// //////////////////////////////////
/*
		 private static string sPathExePython;
		 private static string sEmscArgClangExe;
		 private static string sEmscInclude;
		 private static string sHtml_Shell = "--shell-file ";
		 private static string sBind = "--bind ";
		 private static string sMemFile = "--memory-init-file 0 ";
		 private static string sCache = "--cache ";
	*/

		//cache
        /*
		private static void fGetEmscPaths() {
			sPathExePython =  Emscripten.sPathPython + "python.exe"; //TODO Autodetect
			sEmscArgClangExe =  "\"" + Emscripten.sPathEmsc + "em++\" " + "--em-config \"" +  Emscripten.sDirEmsc + "Emsc.cfg\" " + sCache + "\"" + Emscripten.sDirEmsc + "_cache/\" "; //TODO Autodetect
			sEmscInclude =   "-I \"" +  Emscripten.sPathEmsc + "system/include/emscripten\" "; //TODO Autodetect
		//	Debug.fTrace("include: " + sEmscInclude);
		}
        */
/// //////////////////////////////////		
			/*
             sCompilerPath = PathHelper.ModulesDir + "/Emscripten_x64/python/2.7.5.3_64bit/python.exe"; // C:\Emscripten\emscripten\1.34.1\em++
                sFirstArg = PathHelper.ModulesDir + "/Emscripten_x64/emscripten/1.35.0/em++ " + "--em-config " + PathHelper.ModulesDir + "/Emscripten_x64/Emsc.cfg ";
                sFirstArg += "-I" + PathHelper.ModulesDir + "/Emscripten_x64/emscripten/1.35.0system/include/emscripten ";
                sFirstArg += "--bind ";
             //   sFirstArg += "--memory-init-file 1 ";
                sFirstArg += "--memory-init-file 0 ";
                sOutBranch = "Web/" + sDebugModeFolder;
                sExtention = ".html";
                sPlatformLinkerFlag = "-s EXPORTED_FUNCTIONS=\"['_main', '_int_sqrt', '_lerp']\" --emrun ";
			*/

  
		 public string fGetArgs(CppCmd _oCmd, string _sAllDefine = "", bool _bLinkTime = false,  bool _bSLib = false, bool _bDLib = false, bool _bHaveSourceC = false){
			string _sArg = "";
  
            
        //    ConfigType _oConfig = oGblConfigType;
            ConfigType _oConfig = oCurrentConfigType;
             _oConfig =  fGetConfigFileType(_oCmd.sCompileExtention, _oCmd.oCompiler );
           
          //      fGetConfigFileType


          
                if (_oCmd.sCompileExtention == "cw" || _oCmd.sCompileExtention == "c~") {
                   //   Console.WriteLine("sCWift: " + sCWift);
                    _sArg += _oConfig.sCWayv;
              //     _sArg += oGblConfigType.fGetNode(new string[]{"Platform","arguments", "CWayv"}, oCurrentConfigType.sName).sValue;

                    return _sArg; //Rest is for C++
                }


              if(!_bSLib && !_bDLib) {
                   string _sOptLevel =   Data.fGetGlobalVar("_sOpt").ToUpper();
                    switch(_sOptLevel) {
                         case "DEBUG":
                             _sArg += oGblConfigType.fGetNode(oConfigTypeCompiler, new string[]{"Arguments", "Debug"}, _oConfig.sName)+ " ";
                            // _sArg += "-g ";

                         break;

                         case "OS":
                         case "O3":
                         case "O2":
                           //  _sArg += "-O2 " + oGblConfigType.sName + "| ";
                             _sArg += oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Arguments", _sOptLevel}, _oConfig.sName)+ " ";
                         break;
            
                        default:
                        break;
                  }
              }
                 

          
    //string _sTest =    oGblConfigType.fGetNode(new string[]{"Lib","Arguments", "Compiler"}, oCurrentConfigType.sName).sValue;  
            /*
            Console.WriteLine("!!!!!!!!!!! " + _sTest);
            if (_sTest != "") {
                throw new Exception(_sTest);
            }*/


		//		_sArg += oCurrentConfigType.sCompilerLinker;
                 if(!_bSLib && !_bDLib) {
                    _sArg += oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Arguments", "CompilerLinker"}, _oConfig.sName) + " ";
                }
				if(_bLinkTime){

               
				//	_sArg += oCurrentConfigType.sLinker;
                
					if(_bSLib) {
					//	_sArg += oCurrentConfigType.sLink_Static;
                        _sArg += oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Arguments", "Linker_Static"}, _oConfig.sName) + " ";
					}
					else if(_bDLib) {
					//	_sArg += oCurrentConfigType.sLink_Dynamic;
                      _sArg += oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Arguments", "Linker_Dynamic"}, _oConfig.sName) + " ";
					}
                    
                   if(!_bSLib) {

                       _oCmd.sArgLinkerLib += oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Arguments", "Linker_Lib"}, _oConfig.sName) + " ";
                       _sArg += oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Arguments", "Linker"}, _oConfig.sName) + " ";

                    }
					
				}else{

                   _sArg += oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Arguments", "Compiler"}, _oConfig.sName) + " ";
                    /*
					if(_bHaveSourceC) {
						_sArg += oCurrentConfigType.sC;
					}else {
						_sArg += oCurrentConfigType.sCpp;
					}
					_sArg += oCurrentConfigType.sCompiler;
                    */

					_sArg += _sAllDefine;
				}

		    CompilerData _oCond_Plat =	fGetConditionalPlatformCompilo(_oCmd.fGetVar("_sPlatform"),true);
			if(_oCond_Plat != null){
				_sArg += _oCond_Plat.fGetArgs(_oCmd,_sAllDefine,_bLinkTime,_bSLib,_bDLib,_bHaveSourceC);
				//Debug.fTrace("--------------------------------------------------P!!!!!!!!!!LAQFORM : " + _oCond_Plat.sConditionalName);
				//Debug.fTrace("ARG: " +_oCond_Plat.fGetArgs(_oCmd,_sAllDefine,_bLinkTime,_bSLib,_bDLib,_bHaveSourceC));
			}

			return _sArg;
			
		}




	
        ConfigType oConfigTypeCompiler = null;
        public ConfigType fGetConfigFileType(string _sExtention, CompilerData _oCompiler = null) {
            oConfigTypeCompiler = null;

            //Select config from lib to get config from Toolchain (oModuleData.bIsCompiler = is a lib) and set it to oConfigTypeCompiler instead
            if( !oModuleData.bIsCompiler &&  _sExtention != "" && _oCompiler != null) { //Select config from compiler instead
                oConfigTypeCompiler =  _oCompiler.fGetConfigFileType(_sExtention);
                //Get same name config from compiler (get specified extention)
                if(oConfigTypeCompiler != null) {
                    foreach (KeyValuePair<string, ConfigType> _oKeyConfig in aConfigType) {
                         ConfigType _oConfig = _oKeyConfig.Value;
                         if( _oConfig.sName == oConfigTypeCompiler.sName) {
                            return _oConfig;
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, ConfigType> _oKeyConfig in aConfigType) {
                ConfigType _oConfig = _oKeyConfig.Value;
                foreach(string _sExt in _oConfig.aExtention) {
                    if (_sExt == _sExtention) {
                        return _oConfig;
                    }
                }
            }
          
             return oGblConfigType;
        }

        public string fGetExecutableAndArgForCmd(CppCmd _oCmd,  bool _bLinkTime = false, bool _bCompileAndLink = false,  bool _bSLib = false, bool _bDLib = false) {

               oCurrentConfigType = fGetConfigFileType(_oCmd.sCompileExtention, _oCmd.oCompiler );


         //   Console.WriteLine("**********fGetExecutableAndArgForCmd  " +  oCurrentConfigType.sName + " " + oCurrentConfigType.sExe_Compiler);
            /*
            if (_oCmd.sCompileExtention == "cw"  || _oCmd.sCompileExtention == "c~") {
            
            }*/
              
            
                string _sExe = "";

				_oCmd.sArgument =  fGetArgs(_oCmd, _oCmd.sAllDefine, _bLinkTime, _bSLib, _bDLib, _oCmd.bHaveSourceC);

             
            Console.WriteLine(_oCmd.sCmd);
				////// Libs ////
				string _sLibArg = "";
				foreach(ModuleData _oLib in _oCmd.oParent.aLib){
					CompilerData _oData = _oLib.oLibData;
                    if(_oData != null) {
					    _sLibArg += _oData.fGetArgs(_oCmd, "", _bLinkTime, _bSLib, _bDLib,false ) + " ";//_oCmd.oParentCmd.bHaveSourceC ?
                    }   
                }

				///////////////
             
				_oCmd.sArgument  = _oCmd.fExtractVar(_oCmd.sArgument + " " + _sLibArg ); //Extract wVal
		
				//////////////////////
				///// GET EXE /////////////
                 //  Console.WriteLine("Compile type: " + _oCmd.sE);
                if (_oCmd.sCompileExtention == "cw"  || _oCmd.sCompileExtention == "c~") {
                   // Console.WriteLine("HAVE CW!");
                  //  return PathHelper.ToolDir +  "CWift/CWift.exe";
                 //   return PathHelper.ToolDir +  "CWave/CWave.exe";
                    return PathHelper.ToolDir +  "CWayv/CWayv.exe";
                }

				if(_bSLib) {
					if( oCurrentConfigType.sExe_Link_Static != ""){
					//	_sExe = oCurrentConfigType.sExe_Link_Static;
                       _sExe = oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Exe", "Linker_Static"}, oCurrentConfigType.sName);

					}else{
						//_sExe = oCurrentConfigType.sExe_Linker;
                       _sExe = oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Exe", "Linker"}, oCurrentConfigType.sName);

					}
				}
				else if(_bDLib){
					if( oCurrentConfigType.sExe_Link_Static != ""){
					//	_sExe = oCurrentConfigType.sExe_Link_Dynamic;
                      _sExe = oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Exe", "Linker_Dynamic"}, oCurrentConfigType.sName);

					}else{
					//	_sExe = oCurrentConfigType.sExe_Linker;
                       _sExe = oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Exe", "Linker"}, oCurrentConfigType.sName);

					}
				}
                else if(_bLinkTime) {

				//	_sExe = oCurrentConfigType.sExe_Linker;

                     _sExe = oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Exe", "Linker"}, oCurrentConfigType.sName);

                }else {

                    

					//if(_oCmd.bHaveSourceC) {
				//	if(_oCmd.sCompileExtention == "c") {
                //
					//	_sExe = oCurrentConfigType.sExe_C;   Console.WriteLine("HAVE C");

             
				//	}else {
					//	_sExe = oCurrentConfigType.sExe_Cpp;       Console.WriteLine("HAVE C++");
						//_sExe =  oCurrentConfigType.sExe_Compiler;     


			//			_sExe =  oCurrentConfigType.fGet_Exe_Compiler();   
                        
                         _sExe = oGblConfigType.fGetNode(oConfigTypeCompiler,new string[]{"Exe", "Compiler"}, oCurrentConfigType.sName);
                     //   Console.WriteLine("!!!!***** _sExe !!!! " + _sExe);
				//	}
                }
        /*    

	Console.WriteLine("_sExeA " + oCurrentConfigType.sName);
	Console.WriteLine("_sExeA " +  _oCmd.sCompileExtention);
	Console.WriteLine("sSubName " +  sSubName);
	Console.WriteLine("sFullName " +  sFullName);
	Console.WriteLine("sFullName " +  sFilePath);
	Console.WriteLine("_sExeb " + _sExe);*/

			_sExe =  _oCmd.fExtractVar(_sExe); //Extract wVal
			string _sSubExe = _sExe.Trim();
			int _nIndexArg = -1;
			if(_sSubExe.Length > 3){
				_nIndexArg = _sSubExe.IndexOf(":",3); //Remove Arg
			}
			if(_nIndexArg != -1){
				_sSubExe = _sExe.Substring(0,_nIndexArg);
			}
			 _nIndexArg = _sSubExe.IndexOf(" -"); //Remove Arg
			if(_nIndexArg != -1){
				_sSubExe = _sExe.Substring(0,_nIndexArg);
			}

			_oCmd.sResidualArg = "";
			int _nLastDot = _sSubExe.LastIndexOf('.');
			if(_nLastDot != -1){
				_nLastDot = _sExe.IndexOf(' ',_nLastDot);
				if(_nLastDot != -1){
					_oCmd.sResidualArg =  _sExe.Substring(_nLastDot+1) + " ";
					_sExe =  _sExe.Substring(0,_nLastDot);
				}
			}

          
			//Console.WriteLine("sResidualArg " + _oCmd.sResidualArg);
			//Console.WriteLine("_sExe " + _sExe);
			return _sExe.Replace("\'","" ).Replace("\"","" ).Trim();;
		}





			/*
		public void fSetCompilerEmsc() {  
		
			fGetEmscPaths();
			
			sCpp = sPathExePython;
			sHArgCpp = sEmscArgClangExe + sEmscInclude  + sCreateDFile + sHiddenArg_sColoConsole + "--emrun "  + sArch + sCppVer; //sArch sCppVer TODO
			
			////////////////////
			sHArgC = sHArgLinkerS =  sHArgLinker = sHArgLinkerD = sHArgRC = sHArgCpp; //Set all to clang by default
			sC = sLinker = sLinkerD = sLinkerS = sCpp; //Set all to clang by default
			
			
			//sHtml_Shell_src = Emscripten.sPathEmsc + "src/shell_minimal.html";
			sLink_Action_src = sGZE_Dir + "Emsc/Shell/Base/Console.s_html";
			if(!File.Exists(sLink_Action_src)) {
				sLink_Action_src =  Emscripten.sPathEmsc + "src/shell_minimal.html";
			}

			sLink_Action_cmd = Path.GetExtension(sLink_Action_src) ;
			if(sLink_Action_cmd.Length > 0) {
				sLink_Action_cmd = sLink_Action_cmd.Substring(1);
			}else {
				sLink_Action_cmd = "";
			}
			
			
			//Debug.fTrace("E!xte " + Path.GetDirectoryName(sHtml_Shell_src)  );
			//Debug.fTrace("sHtml_Shell_src_ext " +  sHtml_Shell_src_ext );
			
			sHArgLinker += sHtml_Shell + "\"" + sLink_Action_src   + "\" " + sBind + sMemFile;

		}*/


		internal void fUsed()	{
				
				//rogram.fDebug("-------Compiler USED!?: " + sSubName );
				//rogram.fDebug("-------: " + sExe_Cpp );
				
				
				/*
				if(oCmdH == null) {
					oCmdH =  new CppCmd(Data.oCompilerArg, sHArgCpp );

					oCmdH.fPreExtract();
					Debug.fTrace("----------arg: "  + sHArgCpp);
						Debug.fTrace("-------Extract: CmdH " );
					oCmdH.fExtract();
				}*/

		}
	}
}
