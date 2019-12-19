
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

using System.Net;

using System.Windows.Forms;

using System.Runtime.InteropServices;
using cwc.Utilities;
using cwc.Update;

namespace cwc
{


    public class ModuleData {


        public  Dictionary<string, CompilerData> aPlatformData = new Dictionary<string, CompilerData>();

		 public static ModuleData fGetModule(string _sName, bool _bIsCompiler){
			if (aMData.ContainsKey(_sName) == false) {
				aMData.Add(_sName, new ModuleData(_sName,_bIsCompiler));
			}
			return aMData[_sName];
		}

       public static ModuleData fFindModule(string _sName){
			if (aMData.ContainsKey(_sName) == false) {
				return null;
			}
			return aMData[_sName];
		}



		public static string sUrl_Github = "https://github.com/";
		public static string sUrl_Github_Api = "https://api.github.com/";
		public static string sUrl_Github_Api_Repo =  sUrl_Github_Api + "repos/";
		public static string sUrl_Github_RawContent =  "https://raw.githubusercontent.com/";

		
	

	public static string sCwc_Module_Folder = "(cwc)Modules/";



//https://github.com/d3/d3/tags
        public static Dictionary<string, ModuleData> aMData = new Dictionary<string, ModuleData>() {

				// {"Honera/LibRT", new ModuleData("Honera/LibRT",false) },
			//	 {"Honera/Emscripten", new ModuleData("Honera/Emscripten",false) },

				// {"Honera/GZE", new ModuleData("Honera/GZE" ) },
				 {"Honera/Demos", new ModuleData("Honera/Cwc_Demos") },
				 {"Honera/Cwc", new ModuleData("Honera/Cwc" ) },


			//	{"LibRT", new ModuleData("NazaraEngine", "DigitalPulseSoftware", sCwc_Module_Folder) },
				// {"LibRT", new ModuleData("d3", "d3") },
				// {"LibRT", new ModuleData("witchblast", "Cirrus-Minor") },

			//	 {"GZE", new ModuleData("witchblast", "Cirrus-Minor", _sCwc_Module_Folder) },
		
			
			/*
			 { "GZE", new ModuleData{sURL="www.cwc-repo.com/file/cwc/GZE/Files.php", sDesc="GZE, a true portable engine", sOutFolder="Modules/", sRepoURL = "https://github.com/Maeiky/GZE.git" }},
			//  { "LibRT", new ModuleData{sURL="www.cwc-repo.com/file/cwc/LibRT_x64/Files.php", sDesc="LibRT is a bundle of C++ compilers and custom C++ sources to build portable app", sOutFolder="Modules/" }},
			  { "LibRT", new ModuleData{sName = "LibRT", sURL="www.cwc-repo.com/file/cwc/LibRT_x64/Files.php", sDesc="LibRT is a bundle of C++ compilers and custom C++ sources to build portable app", sOutFolder="Modules/" }},
           

			   { "LibRTDK", new ModuleData{sURL="www.cwc-repo.com/file/cwc/LibRTDK/Files.php", sDesc="LibRTDK is a SDK C++ sources to build portable app", sOutFolder="Modules/", sRepoURL = "https://github.com/Maeiky/LibRTDK.git" }},
			//  { "Demo", new ModuleData{sURL="www.cwc-repo.com/file/cwc/Demos/Files.php",  sDesc="Some demos to test Cwc", sOutFolder="_Demos/",bVersionInFolder = true}},
			  { "Demos", new ModuleData{sURL="www.cwc-repo.com/file/cwc/Demos/Files.php",  sDesc="Some demos to test Cwc", sOutFolder="_Cwc_Demos/",bVersionInFolder = true, sRepoURL = "https://github.com/Maeiky/Cwc_Demos.git"}},
          
		      { "CwcUpd", new ModuleData{sURL="www.cwc-repo.com/file/cwc/Update_x32/Files.php",  sDesc="Update Cwc", sOutFolder=""}},
			 { "Emscripten", new ModuleData{sURL="www.cwc-repo.com/file/cwc/Emscripten_x64/Files.php", sDesc="Export your application on the Web", sOutFolder="Modules/" }},
			 */
			
        };




            public bool bExtact_InProgress = false;

      public Dictionary<string,  int> aLocalVersionDic = new Dictionary<string, int>();
     public string [] aLocalVersion = new string[0];
     public int nTotLocalVersion = 0;
		
       public Dictionary<string,  ModuleLink> aLink = new Dictionary<string, ModuleLink>();
       public List<string> aLinkList = new List<string>();

	 public  string sProject =  "";

		public string[] aServerVersion = new string[0];

      public string sCurrFolder = "";
      public string sReadme = "";
      public string sLicence= "";
    //  public string sLincenceLinkFound= "";
     //   public string sRecommendedVer = "v3.5.5";
        public string sRecommendedVer = "";

        public string sURL;
        public UpdateForm  oForm;//?

    //   public CUpdater oBaseForm;
		
		//public CompilerData oCompilerData = null;
		public List<CompilerData> aCompilerData = new List<CompilerData>();
		//public List<CompilerData> aLibData = new List<CompilerData>();
		public CompilerData oLibData = null;
		
		public  string sLastVersion = "";
        public string sDesc = "";
        public string sOutFolder = "";
        public bool bVersionInFolder = false;

		public string sName = ""; 
		public string sPrefixFolder = ""; 
		public string sRepoURL = ""; 

		//Github
	    public bool bGithubProj = true;
        public string sAutor = "";
        public string sBranch = "master";

        public string sReadMeFile = "README.md";
        public string sReadMeFileAlt = "readme.txt";

		public string sURL_Repo = "";
		public string sUrl_ReadMe = "";
		public string sUrl_ReadMeAlt = "";
		public string sUrl_Branch = "";
		public string sUrl_Tags = "";
		public string sUrl_Tags_API = "";
		public string sUrl_TagInfo = "";
		public string sUrl_Download = "";
		public string sUrl_Project = "";
		public string sUrl_Blob = "";
		public string sUrl_Licence = "";
		public string sUrl_Archive = "";
		
		public Tags aTags; 
		public bool bIsCompiler  = false; 
		public bool bLib  = true; 
		public bool bAutoExplore  = false; 
		
		public string sAutorName;



        public bool bSubLib = false;
        public string sSubPath = "";
        ModuleData oParentLib = null;
        public List<ModuleData> aSubLib = new List<ModuleData>();


        public ModuleData(ModuleData _oParentLib, string _sSubPath = "", string _sName = "")  {
            oParentLib = _oParentLib;
            bSubLib = true;
            bIsCompiler = false;

            sName = _sName;
            sAutorName = _oParentLib.sAutorName + "/" +  sName;
            sAutor = _oParentLib.sAutor;

            sOutFolder = _oParentLib.sOutFolder;
            sPrefixFolder = _oParentLib.sPrefixFolder;

            sSubPath = _sSubPath;
         //   sCurrFolder = _oParentLib.sCurrFolder + _sPath;
         //  fGetCompilerList();
        }
        






        public  ModuleData( string _sAutorName, bool _bIsCompiler = false) {
			sAutorName = _sAutorName;
			 string[] _aValue = _sAutorName.Split('/'); //Squential
			
			sAutor =  _aValue[0];
			sName = _aValue[1];
			bIsCompiler = _bIsCompiler;


			string _sSpecialPrefix = "";
			string _sMainFolder = "Lib/";
			if(bIsCompiler) {
				_sMainFolder = "Toolchain/";
			}
			_sMainFolder += sAutor + "/";


			if(sAutor == "Honera") { //Special lib
				switch(sName) {
					case "Cwc" :
						_sMainFolder = "";
						_sSpecialPrefix = "Upd_";
					break;
					case "Cwc_Demos" :
						_sMainFolder = "";
						_sSpecialPrefix = "_";
						bAutoExplore = true;
					break;
				}
			}
			_sMainFolder += _sSpecialPrefix +  sName + "/";
			
			sOutFolder = PathHelper.GetExeDirectory() + _sMainFolder; //Todo configurable base path
			sPrefixFolder = sName + "-";

			fUpdateGithubUrls();


            if (sName == "GZE")
            {

                ModuleData _oLib = new ModuleData(this, "src/SubLib_3rdparty/", "GzNima");
                aSubLib.Add(_oLib);


               
                Output.TraceAction("ADD NIMA LIB: " + _oLib.sCurrFolder);
                //Testlib
                //  ModuleData _oLib = new ModuleData(true, "");
                // oParent.fAddLib(_oLib); 
            }


        }



		// --- Github Url example:
		//Git           "https://github.com/Maeiky/LibRT.git"
		//tarball_url	"https://api.github.com/repos/Maeiky/LibRT/tarball/0.0.0"
		//zipball_url	"https://api.github.com/repos/Maeiky/LibRT/zipball/0.0.0"

		//Download      "https://github.com/Maeiky/LibRT/releases/download/0.0.0/"
		//BetterComp    "https://github.com/Maeiky/LibRT/releases/download/0.0.0/LibRT_x64_0.0.0.7z"

		//Tags			"https://api.github.com/repos/Maeiky/LibRT/tags"
		//Tag			"https://api.github.com/repos/Maeiky/LibRT/releases/tags/0.0.0" 

		//Branch		"https://raw.githubusercontent.com/Maeiky/LibRT/master/"
		//Readme		"https://raw.githubusercontent.com/Maeiky/LibRT/master/README.md"
			//https://github.com/Cirrus-Minor/witchblast/blob/master/readme.txt
			//https://raw.githubusercontent.com/Cirrus-Minor/witchblast/master/readme.txt
					// ----- 
			//https://github.com/DigitalPulseSoftware/NazaraEngine/releases/tags/
			//https://github.com/Cirrus-Minor/witchblast
			//https://github.com/DigitalPulseSoftware/NazaraEngine/blob/master/LICENSE
			//https://github.com/Maeiky/LibRT/archive/0.0.2.zip




		public void fGetCompilerList(){
			//fGetCompilerData
			//Debug.fTrace("fGetCompilerList : " + 	sCurrFolder + "wType/");
			aCompilerData = new List<CompilerData>();
			aPlatformData = new Dictionary<string, CompilerData>();


            string _sFoundPath = PathHelper.fFindFolder(sCurrFolder, "cwc", 5);
            if(_sFoundPath == "") { //Default
                _sFoundPath = sCurrFolder;
            }
           // Output.TraceAction("FOund?: " + _sFoundPath);
            String[] _aFiles = Directory.GetFiles(_sFoundPath, "*.cwcfg");
            if(_aFiles.Length == 0) {
                Output.TraceError("Cannot find '*.cwcfg' file in " + sCurrFolder );
                Data.bDontExecute = true;
				Build.StopBuild();
                return;
            }
            foreach ( string _sCwcfg_File in _aFiles){
              //     Output.TraceGood("Found " +  _sCwcfg_File);
                CompilerData _oCompilerData = new CompilerData(this, _sCwcfg_File );
                if(bIsCompiler){//It's a Compiler
			        aCompilerData.Add(_oCompilerData);
				    //Output.TraceGood("Add:  " + _oCompilerData.sSubName);
				    aPlatformData.Add(_oCompilerData.sSubName,_oCompilerData );

                }else {//It's a lib
                     oLibData = _oCompilerData;
                }


            }

            /*
			
			string _sCompilerPlatform = sCurrFolder+ "wType/";
			if(!Directory.Exists(_sCompilerPlatform)) {//backward compatibility
                    _sCompilerPlatform = sCurrFolder+ "_sPlatform/";
            }
			if(Directory.Exists(_sCompilerPlatform)) {

				string[] _aFile = Directory.GetFiles( _sCompilerPlatform,"*.xml");
				foreach (string _sFile in  _aFile){
				//	Debug.fTrace("found:  " + _sFile);
					CompilerData _oCompilerData = new CompilerData(this, _sFile );
					aCompilerData.Add(_oCompilerData);
							//Debug.fTrace("Add:  " + _oCompilerData.sSubName);
					aPlatformData.Add(_oCompilerData.sSubName,_oCompilerData );
				}
				if(_aFile.Length == 0){
					Output.TraceError("Compiler " +sAutorName +  " 'wType' forlder require xml data");
					Data.bDontExecute = true;
					Build.StopBuild();
				}
			//Debug.fTrace("---------------------:  " );
			}else{
				Output.TraceError("Compiler " + sAutorName +  " require '_sPlatform' forlder with xml data");
				Data.bDontExecute = true;
				Build.StopBuild();
			}
			*/
			
		}


        public bool bCompilerDataProcessed = false;
		public void fGetCompilerData(){
            if( bExtracting || bCompilerDataProcessed)
            {
                return;
            }
            bCompilerDataProcessed = true;


            aCompilerData = new List<CompilerData>(); //Reset
			if(sCurrFolder != ""){

                fGetCompilerList();

                foreach ( ModuleData _oModule in aSubLib) {
                    _oModule.sCurrFolder = sCurrFolder + _oModule.sSubPath + "/Lib_" +_oModule.sName + "/";
                   // if (Directory.Exists() ) {
                        _oModule.fGetCompilerList();
                    // }
                   // oParent.fAddLib(_oModule.oLibData);
                    //o
                }






                /*
				if(bIsCompiler){
					fGetCompilerList();
				}else{ //Is a lib

                    string _sFoundPath = PathHelper.fFindFolder(sCurrFolder, "cwc");
                    if(_sFoundPath == "") { //Default
                        _sFoundPath = sCurrFolder;
                    }
                    
                    String[] _aFiles = Directory.GetFiles(_sFoundPath, "*.cwcfg");
                    if(_aFiles.Length == 0) {
                        Output.TraceError("Cannot find '*.cwcfg' file in " + sCurrFolder );
                        return;
                    }

                    foreach ( string _sCwcfg_File in _aFiles){
                        //   Output.TraceGood("Found " +  _sCwcfg_File);
                        CompilerData _oCompilerData = new CompilerData(this, _sCwcfg_File );
						oLibData = _oCompilerData;
                    }
				}*/
			}
			
			/*
			if(	oCompilerData == null){
				oCompilerData = new CompilerData(sAutorName);
			}*/

		}



		public void fUpdateGithubUrls() {
			sProject  = sAutor+"/"  +sName+"/";
			sUrl_Project =   sUrl_Github  +sProject;
			sUrl_Branch =  sUrl_Github_RawContent +sProject +sBranch+"/";
			sUrl_ReadMe =  sUrl_Branch  +sReadMeFile;
			sUrl_ReadMeAlt =  sUrl_Branch  +sReadMeFileAlt;
			sURL_Repo =   sUrl_Github +sProject +sName+".git";

	        sUrl_Tags_API =   sUrl_Github_Api_Repo  +sProject + "tags?page=1&per_page=1000";
			sUrl_Tags =   sUrl_Github  +sProject + "tags";
			sUrl_Blob = sUrl_Project + "blob/" + sBranch + "/"; 
			sUrl_Licence  = sUrl_Blob + "LICENSE";
			sUrl_TagInfo =   sUrl_Github  +sProject + "releases/tag/";

			sUrl_Archive = sUrl_Project + "archive/";
			sUrl_Download =  sUrl_Github +sProject + "releases/download/";
		}
 



		public bool bDevMode = false;

        public static bool isApp64 = (IntPtr.Size == 8);
        public static bool is64 = isApp64 || InternalCheckIsWow64();
        public static string sOS_bit = "";


        public string sLabel = "";
        public string sVersion = "Unknow";
        public string sGitVersion = "Unknow";


        public String URL;
        public String BaseURL;
         public String sDownloadDir = PathHelper.GetExeDirectory() + "temp/";
        public string[] aModule;
        public string[] aPfm;
        public bool bCloseMe = false;
        public int nSel = 0;

        public bool bReadFinished = false;

   


        public Dictionary<string, CheckBox> aChk = new Dictionary<string, CheckBox>();
        public Dictionary<string, Label> aStatus = new Dictionary<string, Label>();
        public Dictionary<string, Label> aCurrVersion = new Dictionary<string, Label>();
        public Dictionary<string, Label> aZipSize = new Dictionary<string, Label>();
        public Dictionary<string, Label> aLastVersion = new Dictionary<string, Label>();
        public Dictionary<string, string> aLastVersionType = new Dictionary<string, string>();
        public Dictionary<string, string> aLastCompression = new Dictionary<string, string>();
        public Dictionary<string, ProgressBar> aPrgBar = new Dictionary<string, ProgressBar>();


        
      public static int nRequestTag = 0;

        public void fReadHttpModuleTags()  {
	        nRequestTag++;
			aTags  = new Tags() ;
			Http.fGetAllTag(aTags,  sUrl_Tags_API, fReadHttpModuleTagsFinish, sUrl_Tags, sRecommendedVer);
        }


    private static readonly Object oLock = new Object();
        public void fReadHttpModuleTagsFinish(ParamHttp _oParam)  {
            	//	Debug.fTrace("---------Get-------" + aTags.Count);
			//Tags _aTags = (Tags)_oData.oContainer;
	         lock (oLock){

			    foreach(Tag _oTag in aTags) {
				    new ModuleLink(this, _oTag);
			    }
                aLinkList = new List<string>(this.aLink.Keys);
                aLinkList.Sort();
                aLinkList.Reverse();

                
               //	Debug.fTrace("---------FInish-------" + aLink.Count);
        	    if(oForm != null) {  oForm.fDataLoaded(); }
                nRequestTag--;
            }


		}


        /// <summary>
        /// Startups the update check
        /// </summary>
        public void InitializeUpdating()  {
        }



        public virtual void fServerFail(string _sMsg) {
        }

      


        public void fExtractZip(string zipFileName, string targetDir)
        {/*
            FastZip fastZip = new FastZip();
            string fileFilter = null;
            fastZip.ExtractZip(zipFileName, targetDir, fileFilter);*/
        }

        public void fExtractExe(string sFileName, string sTargetDir){

            string Arguments = " /auto " + sTargetDir;
            Process p = new Process();
            p.StartInfo.FileName = sFileName;// "cmd.exe";
            p.StartInfo.Arguments = Arguments;// " /auto " + sOutputFilePath;

            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            //make the window Hidden 
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
        }

        



        public  void fCompressionExit(LauchTool _oTool)  {

		    		   //Output.TraceWarning("--!!!fCompressionExit!-----: " );
            	bExtracting = false;
		        fGetLastVersions();
                		//   Output.TraceWarning("-#1: " );
			
            	if(oForm != null) {
				    oForm.fExtractProgress(100, "");
                }

                // Debug.fTrace("----------------------fCompressionExit : " );

              try {  
                    //Debug.fTrace("-Trydelete : " +  _oTool.sSourceFile);
                    if(File.Exists(_oTool.sSourceFile)) {
                      File.Delete(_oTool.sSourceFile);
                      // Debug.fTrace("-Deleted : " +  _oTool.sSourceFile);
                    }


                  }catch(Exception Ex) {
					Output.TraceError("--Error: " + Ex.Message );
            }
      
         
            if( !fCheckForAutoExtractFiles(_oTool) ){

                 bExtact_InProgress = false;

                // Output.TraceGood("----- Complete All Sub Extraction ------ " );
                 bSubExtract = false;

			   	if(oForm != null) {   oForm.fModuleFinish(); }
    
            } else {
                 Output.TraceGood("----- Complete " + _oTool.sSourceFile );
            }
            

			if(sName == "Cwc") {
				//Cwc update now!
			    if(oForm != null) {oForm.fExecuteUpdater(this); }
			}

        }

        public bool bSubExtract = false;
        public int nTotalExtract = 0;
        public int nExtracted = 0;
        public  string[] aFileAutoExtract;
         public string sCurrentExtractFile = "";
        public bool fCheckForAutoExtractFiles(LauchTool _oTool) {
 
             if(_oTool.oCustom == null){ //Not for sub extract items (Only master extact get auto sub extract)
                //Master!

                 //   Debug.fTrace("Target: " + _oTool.sTarget );
                    
                    aFileAutoExtract = Directory.GetFiles(_oTool.sTarget, "*.-ex", SearchOption.AllDirectories); //**TODO _oTool.sTarget contain all libs version, todo optimise to the current extraction **
                     nTotalExtract = aFileAutoExtract.Length;
                     nExtracted = 0;
              
                   Output.TraceWarning("SubFile to extract: " + nTotalExtract + "");

                 //   foreach (string _sFile in _aFileAutoExtract) {
             
             
                  //  }
            } else {
                nExtracted++;
              Output.TraceWarning("----- Complete " + _oTool.sSourceFile  + " (" +  nExtracted + "/" + nTotalExtract + ")");
            }
            if (nExtracted != nTotalExtract) {
                  bSubExtract = true;
                  string _sFile = aFileAutoExtract[nExtracted];
                  string _sFileDirectory =  Path.GetDirectoryName(_sFile);
                sCurrentExtractFile = Path.GetFileName(_sFile);
                  fExtractSevenZip( _sFile, _sFileDirectory, _sFileDirectory, _oTool.oModule, true);
            }


         //  bool  bLastOne = false;
            if (nExtracted == nTotalExtract) {
               
                return false;
            }

            return true;
        }

       


        public void fExtractSevenZip(string zipFileName, string targetDir, string _sFullTargetDir, ModuleLink _oLink = null, object _oCustom = null  )
        {

            //Debug.fPrint("-Extact : " + zipFileName);
        //    Output.TraceWarning("-Extact : " + zipFileName);

             LauchTool _o7z =  new LauchTool();
            _o7z.oModule = _oLink;
             _o7z.oCustom = _oCustom;
			_o7z.dExit = new LauchTool.dIExit(fCompressionExit);
             _o7z.bHidden = true;
			_o7z.dOut = fExtractOut;
			_o7z.dError = fExtractOut; 
			_o7z.fLauchExe( PathHelper.ToolDir + "7z/7z.exe",  "x \"" + zipFileName + "\"  -bsp2 -y -o\"" + targetDir + "\"", zipFileName , _sFullTargetDir,true);

        }

		public bool bExtracting = false;
		public string sLastLocalVersion = "";
		public string sDisplayVersion = "";

		public void fExtractOut(LauchTool _oThis,string _sMsg) {

			//Debug.fTrace(_sOut);
			if(!FileUtils.IsEmpty(_sMsg)){
               string[] _aResult = _sMsg.Split('\n');
                bool _bError = false;
                bool _bGood = false;
                if (_sMsg.IndexOf("Can not")  >= 0 ||  _sMsg.IndexOf("Can't")  >= 0 || _sMsg.IndexOf("Unexpected")  >= 0 ||  _sMsg.IndexOf("ERROR")  >= 0 || _sMsg.IndexOf("Error ")  >= 0  || _sMsg.IndexOf("Error:")  >= 0) {
                       _bError = true;
                      //  Output.TraceError( "--- [" + _oThis.sArg + "] ---"   );
                }else if(_sMsg.IndexOf("Everything is Ok")  >= 0) {
                    _bGood = true;
                }

                foreach(string _sOutput in _aResult) {
                     string _sOut  = _sOutput.Trim();
                        if(_sOut == "") {continue; }

				    Debug.fRPrint("Extract: " + _sOut + "                                                                                             ");

                    if (_bGood) {
                         Output.TraceGood( _sOut );
                    } else if (_bError) {
                  
                         Output.TraceError( _sOut );
                          
                    }else {
				        Output.TraceWarning("\rExtract[" + _oThis.oModule.sName + "]: " + _sOut );
                    }

				    if(_sOut.IndexOf("Extracting") != -1) {
					    bExtracting = true;
				    }
				    else if(bExtracting) {
					    string [] _aExtInfo = _sOut.Split('%');
					    if(_aExtInfo.Length >= 2) {
						    string _sNumber = _aExtInfo[0].Trim();
						    int _nValue = 0;
						    //Debug.fTrace("TryParce: " + _sNumber);
						    if (Int32.TryParse(_sNumber, out _nValue)) {
						        if(oForm != null) {oForm.fExtractProgress(_nValue, _aExtInfo[1]); }
						    }

				
					    }
				    }
                }
           


			}
		}






     

        public void fModuleError(ModuleLink _oModule)
        {
 //           Debug.fTrace("Error: " + _sFile);
            try
            {
              //  aChk[_sModule].Enabled = true;
             //   aStatus[_sModule].Text = "Error";
            }
            catch { }
            checkDownloadCompleted();

        }
        public void checkDownloadCompleted()
        {
            if (isDownloadCompleted())
            {
                //chkModuleCLick(null, null);
                fUpdateDownloadBtn();
  
            }
        }

        public bool isDownloadCompleted()
        {
            foreach (Label _lblDl in aStatus.Values)
            {
                if (fIsBusy(_lblDl.Text))
                {
                    return false;
                }
            }
            return true;
        }
        public bool fIsBusy(string _sStatus)
        {
            if (_sStatus == "Downloading" || _sStatus == "Extracting")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
      
   


        

        public virtual void fOverInitializeDownload() {
        }

        private void CommunUpdater_Load(object sender, EventArgs e)
        {

        }



        public void fUpdateStatus()  {

            try{

            string _sBaserPath = PathHelper.GetExeDirectory() + sOutFolder;


            if( bVersionInFolder) {//TODO

                    string _sPath = PathHelper.GetExeDirectory() + sOutFolder;
                    string[] directories = Directory.GetDirectories(_sPath);
                   // string lastDirectory = string.Empty;

                    if (directories.Length > 0)  {
                        Array.Sort(directories);
                      //  lastDirectory = directories[directories.Length - 1];
                        foreach(string _sDir in  directories) {
                            int _nIndex = _sDir.IndexOf(sName);
                            if(_nIndex != -1) {
                                sVersion = _sDir.Substring(_sPath.Length  + sName.Length + 1 );
                            }
                        }
                    }

            }else { 
            
					string _sPath ;
                     if(sName == "CwcUpd") {
						sVersion = Data.sVersion;
					//	_sPath =_sBaserPath + "info.txt";

					}else {

						_sPath =_sBaserPath + sName + "_" + sOS_bit + "/info.txt";
					

						if(!File.Exists(_sPath)) {
								_sPath =_sBaserPath   + sName + "_x32" + "/info.txt";
						}
						if(!File.Exists(_sPath)) {
							_sPath = _sBaserPath   + sName  + "/info.txt";
						}
							//Debug.fTrace("-------------_sPath : " + _sPath);
            
         
						if(File.Exists(_sPath)) {
										//  Debug.fTrace("status path : "  +_sPath);
							StreamReader _reader = new StreamReader(_sPath);
							string sLineVersion =  _reader.ReadLine(); // Read version
								_reader.Close();
								string[] aCurrVersion = sLineVersion.Split(':');
								sLabel = aCurrVersion[0].Trim();
								sVersion = aCurrVersion[1].Trim();
								/*
							sLabel =  _reader.ReadLine(); // Read version
							sVersion = _reader.ReadLine(); // Read download*/
						}
					}
            

                }

          }catch(Exception ex) { }

        }

        public virtual void fUpdateDownloadBtn(){ }


        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );
        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }


        public void fReadHttpInfoModule(string _sLine)  {
					
            if(!FileUtils.IsEmpty(_sLine)) {
                _sLine = _sLine.Trim();
                if(_sLine.Length > 1 && _sLine[0] == '|') {
                    new ModuleLink(_sLine.Substring(1));
                }
            }
        }



	   internal string fGetLastVersions(){
			fGetLocalVersions();

			return sLastLocalVersion;
		}


		internal void fGetLocalVersions()
		{
			aLocalVersionDic = new Dictionary<string, int>();
			aLocalVersion = new string[0];
			nTotLocalVersion = 0;
	
			if(Directory.Exists(sOutFolder)) {
				string[] _aDir = Directory.GetDirectories(sOutFolder);
				Array.Sort(_aDir, StringComparer.InvariantCultureIgnoreCase);
				Array.Reverse(_aDir);

				aLocalVersion = new string[_aDir.Length];
			
				int _nIndex = 0;
				foreach(string _sVal in _aDir){
					string _sDir = Path.GetFileName(_sVal);
					if(_sDir.Length > (sName.Length +  1) &&  _sDir.IndexOf( sName ) == 0) {
					
						aLocalVersion[_nIndex] = _sDir.Substring(sName.Length +  1);
						aLocalVersionDic[aLocalVersion[_nIndex]] = _nIndex;

						_nIndex++;
				
				//		break;
					}
				}
				nTotLocalVersion = _nIndex;
			}

			if(nTotLocalVersion > 0) {
				fSetLastLocalVersion(aLocalVersion[0]);
			}else {
				fSetLastLocalVersion("");
			}
		
		
		}



		public static string fGetVersion(string _sVer){
			if(_sVer.Length > 2 && _sVer[0] == 'v'  && _sVer[1] >= '0' &&  _sVer[1] <= '9') {
				return _sVer.Substring(1);
			}else {
				return _sVer;
			}
		}

		internal void fSetLastVersion(string _sVer){
			sLastVersion = fGetVersion(_sVer);
            if(oForm != null) {
			    oForm.fUpdateVersion();
            }
		}
		internal void fSetLastLocalVersion(string _sVer, bool bCheck =true){
			
			sLastLocalVersion = fGetVersion(_sVer);
			sDisplayVersion = sLastLocalVersion;

			if(_sVer != "") {
				sCurrFolder = sOutFolder + sPrefixFolder + sLastLocalVersion + "/";
			}else {
				sCurrFolder = "";
			}
/*
			if(sName == "LibRT"){
				CompilerData.fUpdateLibRTPath(sCurrFolder);
			}
			if(sName == "GZE"){
				Debug.fTrace(sCurrFolder);
				CompilerData.fUpdateGZEpath(sCurrFolder);
			}
*/
			if(sName == "Cwc") { ///Special case for Cwc
				sDisplayVersion = Data.sVersion;
			}

			if(oForm != null) {
				oForm.fUpdateVersion();
			}
	
            if(bCheck){
			fGetCompilerData();
            }

		}

		internal bool fLocalVersionExist(string text)
		{	

			string _sVersion = fGetVersion(text);
			//Debug.fTrace("Search " + _sVersion);;

			if(aLocalVersionDic.ContainsKey(_sVersion)) {
				return true;
			}else {
				return false;
			}

	

		}
	}
    
}
