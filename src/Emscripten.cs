using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cwc
{
	class Emscripten{

		public static List<String> aBrowser;
        public static List<String> aBrowserVersion;

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
			string[] directories = Directory.GetDirectories(sDirEmsc + "emscripten/");
			string lastDirectory = "";

			if (directories.Length > 0){
				Array.Sort(directories);
				lastDirectory = directories[directories.Length - 1];
			}
			sEmscVer =  Path.GetFileName(lastDirectory);
			sPathEmsc = sDirEmsc + "emscripten/" + sEmscVer + "/";
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
		public static void fGetNode() {
			string[] directories = Directory.GetDirectories(sDirEmsc + "node/");
			string lastDirectory = "";

			if (directories.Length > 0){
				Array.Sort(directories);
				lastDirectory = directories[directories.Length - 1];
			}
			sNodeVer =  Path.GetFileName(lastDirectory);
			sPathNode = sDirEmsc + "node/" + sNodeVer + "/";
		//	Debug.fTrace("sPathNode: " + sPathNode);
		}
		public static void fGetJava() {
			string[] directories = Directory.GetDirectories(sDirEmsc + "java/");
			string lastDirectory = "";

			if (directories.Length > 0){
				Array.Sort(directories);
				lastDirectory = directories[directories.Length - 1];
			}
			sJavaVer =  Path.GetFileName(lastDirectory);
			sPathJava = sDirEmsc + "java/" + sJavaVer + "/";
		//	Debug.fTrace("java: " + sPathJava);
		}
		public static void fGetSpider() {
			string[] directories = Directory.GetDirectories(sDirEmsc + "spidermonkey/");
			string lastDirectory = "";

			if (directories.Length > 0){
				Array.Sort(directories);
				lastDirectory = directories[directories.Length - 1];
			}
			sSpiderVer =  Path.GetFileName(lastDirectory);
			sPathSpider = sDirEmsc + "spidermonkey/" + sSpiderVer + "/";
		//	Debug.fTrace("spidermonkey: " + sPathSpider);
		}
		public static void fGetCrunch() {
			string[] directories = Directory.GetDirectories(sDirEmsc + "crunch/");
			string lastDirectory = "";

			if (directories.Length > 0){
				Array.Sort(directories);
				lastDirectory = directories[directories.Length - 1];
			}
			sCrunchVer =  Path.GetFileName(lastDirectory);
			sPathCrunch = sDirEmsc + "crunch/" + sCrunchVer + "/";
		//	Debug.fTrace("crunch: " + sPathCrunch);
		}
		public static void fGetClang() {
			string[] directories = Directory.GetDirectories(sDirEmsc + "clang/");
			string lastDirectory = "";

			if (directories.Length > 0){
				Array.Sort(directories);
				lastDirectory = directories[directories.Length - 1];
			}
			sClangVer =  Path.GetFileName(lastDirectory);
			sPathClang = sDirEmsc + "clang/" + sClangVer + "/";
			//Debug.fTrace("clang: " + sPathClang);
		}

	//fEmscExist();
		
		public static bool fEmscExist()	{
			
			if(CompilerData.bRequireEmsc && !Directory.Exists(sDirEmsc)) {
/*
				Data.oMsgBox = new Form { TopMost = true,TopLevel = true };
				MessageBox.Show(Data.oMsgBox , "The Emsc module is required to build web application", "Download Emsc?", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);	
				Data.aRequiredModule.Add( "Emscripten");
*/
/*
				Data.bShowLibRT = true;
				Data.sShowModule = "Emscripten";
				Data.bStartWithMessage = true;
				Data.bDontExecute = true;
				if(Data.bInConsole) {
					if(Data.oMainForm != null) {
						UpdateLibRT.Show(Data.oMainForm, false,  "Emscripten");
					}
				}
*/


			}
			
			return true;
		}


		public  static bool bAlreadyTested = false;
		public  static  CompilerData oCmp = null;

        public static void fCreateEmscConfig(bool _bForce = false){
		
			sDirEmsc = oCmp.oModuleData.sCurrFolder;

			 if (!_bForce && bAlreadyTested) {
               return;
			}        
          	
	//rogram.fDebug("**DIr? " + (sDirEmsc));
			if(!Directory.Exists(sDirEmsc)) {
				 return;
			}
	
	//rogram.fDebug("*******************Create EMSC confg!!");

			bAlreadyTested = true;

		
			//////////////////////////
			fGetPathEmsc();
			fGetPython();
			fGetNode();
			fGetJava();
			fGetSpider();
			fGetCrunch();
			fGetClang();

			/////////////////////////
            string _sPath = sDirEmsc + "Emsc.cfg";
	//File.Delete(_sPath); Force recompile std
			if (File.Exists(_sPath)) {
				string[] lines =  File.ReadAllLines(_sPath);
				if (lines[1] != "LLVM_ROOT='" + sPathClang + "'") {
					File.Delete(_sPath);
				}
			}

//Debug.fTrace(" Path.GetTempPath() " + Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine) );

         //   if (!File.Exists(_sPath)) { //!TODO check if simacode or emscriptem change folder!
              if ( !File.Exists(_sPath)) { //!Temp regen if first time
                    try {
                        FileStream fs1 = new FileStream(_sPath, FileMode.OpenOrCreate, FileAccess.Write);

                        StreamWriter writer = new StreamWriter(fs1);
                        writer.WriteLine("import os");
                        writer.WriteLine("LLVM_ROOT='" + sPathClang + "'");
                        writer.WriteLine("EMSCRIPTEN_NATIVE_OPTIMIZER='" + sPathClang + "optimizer.exe'");
                        writer.WriteLine("NODE_JS='" + sPathNode + "bin/node.exe'");
                        writer.WriteLine("JAVA = '" + sPathJava + "bin/java.exe'");
                        writer.WriteLine("SPIDERMONKEY_ENGINE='" + sPathSpider + "js.exe'");
                        writer.WriteLine("EMSCRIPTEN_ROOT='" + sPathEmsc + "'");
                        writer.WriteLine("CRUNCH='" + sPathCrunch + "crunch.exe'");
                        writer.WriteLine("V8_ENGINE = ''");
                        writer.WriteLine("TEMP_DIR = '" + Environment.GetEnvironmentVariable("TEMP", EnvironmentVariableTarget.Machine) + "'");
                        writer.WriteLine("COMPILER_ENGINE = NODE_JS");
                        writer.WriteLine("JS_ENGINES = [NODE_JS]");
                        writer.Close();
                    }
                    catch { }
            }

			fAddViewTargetBrowser();
        }
	

	 public static string fGetSeletedBrowser(){
		if(!bBrowserProcess) {
			fAddViewTargetBrowser();
		}

		while(!bBrowserSet && Base.bAlive) {
			Thread.Sleep(1);
		}
		
		if(Data.oMainForm != null) {
			return  Data.oMainForm.cbView.Text;
		}else {
			return aBrowser[0];
		}
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
				}
				
				if(Data.oMainForm != null) {
				 Data.oMainForm.fSetView();
				}
				bBrowserSet = true;
				
            });
            bw.RunWorkerAsync();
			
        }


       public static void fGetBrowser(string _sLine) {


           if (_sLine.Length > 5 &&_sLine[2] == '-')  {
               string _browserLine = _sLine.Substring(4);
               string[] _aBrowser = _browserLine.Split(new string[] { ": " }, StringSplitOptions.None);
               if (_aBrowser.Length >= 2){
                   aBrowser.Add(_aBrowser[0]);
                   aBrowserVersion.Add(_aBrowser[1]);
				//	Debug.fTrace("Browser : " +  _aBrowser[0] );
				//	Debug.fTrace("Version : " +   _aBrowser[1] );
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