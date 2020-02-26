
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace GetBrowsers
{

	class Program{


		public static List<String> aBrowser;
        public static List<String> aBrowserVersion;
        public static List<String> aBrowserAndVersion = new List<string>();

		public static  string sDirEmsc =  "(unknow)";
		
		public static  string sPathEmsc = "";
		public static  string sPathPython = "";
		public static  string sPathNode= "";
		public static  string sPathJava= "";
		public static  string sPathSpider= "";
		public static  string sPathCrunch= "";
		public static  string sPathClang = "";

		public static  string sEmscVer = "";
		public static  string sPythonVer = "";
		public static  string sNodeVer = "";
		public static  string sJavaVer = "";
		public static  string sSpiderVer = "";
		public static  string sCrunchVer = "";
		public static  string sClangVer = "";



	
		public static void fGetPathEmsc() {
            /* //No subdir version anymore
			string[] directories = Directory.GetDirectories(sDirEmsc + "emscripten/");
			string lastDirectory = "";

			if (directories.Length > 0){
				Array.Sort(directories);
				lastDirectory = directories[directories.Length - 1];
			}
			sEmscVer =  Path.GetFileName(lastDirectory);
			sPathEmsc = sDirEmsc + "emscripten/" + sEmscVer + "/";
            */ 
			sPathEmsc = sDirEmsc + "upstream/emscripten/" + sEmscVer;

		}

		public static void fGetPython() {
			string[] directories = Directory.GetDirectories(sDirEmsc + "python/");
			string lastDirectory = "";

			if (directories.Length > 0){
				Array.Sort(directories);
				lastDirectory = directories[directories.Length - 1];
			}
			sPythonVer =  Path.GetFileName(lastDirectory);
			sPathPython = sDirEmsc + "python/" + sPythonVer + "/";

		}

	//fEmscExist();
		
		public static bool fEmscExist()	{
			
		//	if(CompilerData.bRequireEmsc && !Directory.Exists(sDirEmsc)) {

/*
				Program.oMsgBox = new Form { TopMost = true,TopLevel = true };
				MessageBox.Show(Program.oMsgBox , "The Emsc module is required to build web application", "Download Emsc?", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);	
				Program.aRequiredModule.Add( "Emscripten");
*/
/*
				Program.bShowLibRT = true;
				Program.sShowModule = "Emscripten";
				Program.bStartWithMessage = true;
				Program.bDontExecute = true;
				if(Program.bInConsole) {
					if(Program.oMainForm != null) {
						UpdateLibRT.Show(Program.oMainForm, false,  "Emscripten");
					}
				}
*/


			//}
						return true;
		}


		public  static bool bAlreadyTested = false;




		static void Main(string[] args){
			string sFullDir = Path.GetDirectoryName(  System.Reflection.Assembly.GetEntryAssembly().Location);
			string sDirName = Path.GetFileName(sFullDir);
			sDirEmsc = sFullDir.Replace('\\','/');

		  string _sLast = sDirEmsc;
		     sDirEmsc = sDirEmsc.Substring(0, sDirEmsc.Length - sDirName.Length);
            /*

            if (!Directory.Exists( sDirEmsc + "emscripten")) {
                sDirEmsc = _sLast;
                  if (!Directory.Exists( sDirEmsc + "emscripten")) {
                    Console.WriteLine("Error Emscripten not found");
                    return;
                }
            }*/


			//Console.WriteLine( sDirEmsc);
			fGetPathEmsc();
			fGetPython();
			fCreateEmscConfig();
		}



        public static void fCreateEmscConfig(bool _bForce = false){
		
		//	sDirEmsc = oCmp.oModuleData.sCurrFolder;

	//		 if (!_bForce && bAlreadyTested) {
    //           return;
	//		}        
          	
//	Program.fDebug("**DIr? " + (sDirEmsc));
			if(!Directory.Exists(sDirEmsc)) {
				 return;
			}
	

			fGetSeletedBrowser();
        }
	

	 public static string fGetSeletedBrowser(){
	//	if(!bBrowserProcess) {
			fAddViewTargetBrowser();
		//}

		while(!bBrowserSet) {
			Thread.Sleep(1);
		}
		
		foreach(string _sBrowser in aBrowserAndVersion){
			Console.Write("\n* "  + _sBrowser  );
		}

	//	if(Program.oMainForm != null) {
	//		return  Program.oMainForm.cbView.Text;
	//	}else {
			return aBrowser[0];
	//	}
	}
	
	 public static bool bBrowserSet = false;
	 public static bool bBrowserProcess = false;
	 public static void fAddViewTargetBrowser(){
           aBrowser = new List<String> {};
           aBrowserVersion = new List<String> {};
			
			bBrowserProcess = true;
            BackgroundWorker bw = new BackgroundWorker();

            //bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                
                 string sCompilerPath  = sPathPython + "python.exe"; // C:\Emscripten\emscripten\1.34.1\em++
                 string _sArg = "\"" + sPathEmsc  + "emrun\" --list_browsers ";


                ProcessStartInfo processStartInfo = new ProcessStartInfo(sCompilerPath, _sArg);
                processStartInfo.WorkingDirectory = Path.GetDirectoryName(sCompilerPath);

               // Console.WriteLine("GetBrowser: " + sCompilerPath + " " +  _sArg);

                if (!File.Exists( sCompilerPath)) {
                    Console.WriteLine("Error: Cannot find python: " + sCompilerPath);
                    bBrowserSet = true;
                    return;
                }

                //  Console.WriteLine(sCompilerPath);
                 // Console.WriteLine(_sArg);



                processStartInfo.UseShellExecute = false;
                processStartInfo.ErrorDialog = false;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.RedirectStandardError = true;
              ///  processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                Process process = new Process();
                process.StartInfo = processStartInfo;
                bool processStarted = process.Start();
            
               // StreamWriter inputWriter = process.StandardInput;
                StreamReader outputReader = process.StandardOutput;
              //  StreamReader errorReader = process.StandardError;

               
                string _sOutput = outputReader.ReadToEnd();
                string[]  _aLines = _sOutput.Split('\n');
      
                foreach (string _sLine in _aLines)   {
                    fGetBrowser(_sLine);
                }
				
                //fSetViewTarget(PluginMain.activeProject);
                process.WaitForExit();
				if(aBrowser.Count == 0) {
				      aBrowser.Add("No Browser found");
				      aBrowserAndVersion.Add("No Browser found");
				}
				
			//	if(Program.oMainForm != null) {
				// Program.oMainForm.fSetView();
			//	}
				bBrowserSet = true;
				
            });
            bw.RunWorkerAsync();
			
        }


       public static void fGetBrowser(string _sLine) {


           if (_sLine.Length > 5 &&_sLine[2] == '-')  {
               string _browserLine = _sLine.Substring(4);

				  aBrowserAndVersion.Add(_browserLine);

               string[] _aBrowser = _browserLine.Split(new string[] { ": " }, StringSplitOptions.None);
               if (_aBrowser.Length >= 2){
                   aBrowser.Add(_aBrowser[0]);
                   aBrowserVersion.Add(_aBrowser[1]);
                 

				//	Console.WriteLine("Browser : " +  _aBrowser[0] );
				//	Console.WriteLine("Version : " +   _aBrowser[1] );
                 //     TraceManager.Add("Browser : " +  _aBrowser[0] );
                //      TraceManager.Add("Version : " + _aBrowser[1]);
               }
           }
       }
	



}

}







/*
 * 
* 
    StreamWriter writer = new StreamWriter(fs1);
    writer.WriteLine("import os");
    writer.WriteLine("SPIDERMONKEY_ENGINE = ''");
    writer.WriteLine("NODE_JS = 'node'");
    writer.WriteLine("LLVM_ROOT='" + sDirEmsc + "clang/e1.35.0_64bit'");
    writer.WriteLine("EMSCRIPTEN_NATIVE_OPTIMIZER='" + sDirEmsc + "clang/e1.35.0_64bit/optimizer.exe'");
    writer.WriteLine("NODE_JS='" + sDirEmsc + "node/4.1.1_64bit/bin/node.exe'");
    writer.WriteLine("NODE_JS='" + sDirEmsc + "node/4.1.1_64bit/bin/node.exe'");
    writer.WriteLine("JAVA = '" + sDirEmsc + "java/7.45_64bit/bin/java.exe'");
    writer.WriteLine("SPIDERMONKEY_ENGINE='" + sDirEmsc + "spidermonkey/37.0.1_64bit/js.exe'");
    writer.WriteLine("EMSCRIPTEN_ROOT='" + sDirEmsc + "emscripten/1.35.0'");
    writer.WriteLine("CRUNCH='" + sDirEmsc + "crunch/1.03/crunch.exe'");
    writer.WriteLine("V8_ENGINE = ''");
    writer.WriteLine("TEMP_DIR = '" + Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine) + "'");
    writer.WriteLine("COMPILER_ENGINE = NODE_JS");
    writer.WriteLine("JS_ENGINES = [NODE_JS]");
    writer.Close();
* 
* 
* */