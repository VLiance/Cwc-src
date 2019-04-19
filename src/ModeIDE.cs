using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cwc
{
	public class ModeIDE {
	
		public bool oCppAstStarted = false;

		public LauchTool oCppAst =  new LauchTool(); 
		public bool bExtracted = false;
		public bool bFinishExtractSended= false;

		public ModeIDE() {

   
		//	LauchTool oCppAst =  new LauchTool();
			oCppAst.sWorkPath = PathHelper.Module;
			oCppAst.dOut = new LauchTool.dIOut(fCppAstOut);
	//		_oCppAst.dExit = new LauchTool.dIExit(fGitCloneEnd);
  // oCppAst. = false;


			oCppAst.fLauchExe( PathHelper.ToolDir + "cppast/cppast.exe", "");
		}
		
		internal void fFinishExtractArg()
		{



		
//	Debug.fTrace("cwc:A:|LibInfo|ASSS<I:/FlashDev/_MyProject/Simacode/LDK/LinxDemo/_MainDemo/<Lib_Demo/<Lib,Demo,false,,Lib_Demo,Lib_Demo/*Rc,Exe,RcExe/,,false*;GZ<I:/FlashDev/_MyProject/Simacode/LDK/GZE/<_Lib/GZE/Lib_GZ/<Lib,GzCpp,true,LibCppWrapper/Lib_GZ/,Lib_GZ,_Lib/GZE/Lib_GZ/*Lib,GzCw,false,SimaCodeEngine/,Lib_GZ,_Lib/GZE/Lib_GZ/*Rc,RcEngine,Rc/,Rc/RcEngine/,true*;GzLite<I:/FlashDev/_MyProject/Simacode/LDK/GZE/<_Lib/GZE/SubLib_System/Lib_GZ_Lite/<Lib,GzLite,false,SubLib_System/Lite/,Lib_GZ_Lite,_Lib/GZE/SubLib_System/Lib_GZ_Lite/*;GzCpcDos<I:/FlashDev/_MyProject/Simacode/LDK/GZE/<_Lib/GZE/SubLib_System/Lib_GZ_CpcDos/<Lib,GzCpcDos,false,SubLib_System/CpcDos/,Lib_GZ_CpcDos,_Lib/GZE/SubLib_System/Lib_GZ_CpcDos/*;GzWindows<I:/FlashDev/_MyProject/Simacode/LDK/GZE/<_Lib/GZE/SubLib_System/Lib_GZ_Windows/<Lib,GzWindows,false,SubLib_System/Windows/,Lib_GZ_Windows,_Lib/GZE/SubLib_System/Lib_GZ_Windows/*;GzEmsc<I:/FlashDev/_MyProject/Simacode/LDK/GZE/<_Lib/GZE/SubLib_System/Lib_GZ_Emscripten/<Lib,GzEmsc,false,SubLib_System/Web/Emscripten/,Lib_GZ_Emscripten,_Lib/GZE/SubLib_System/Lib_GZ_Emscripten/*;GzOpenGL<I:/FlashDev/_MyProject/Simacode/LDK/GZE/<_Lib/GZE/SubLib_System/Lib_GZ_OpenGL/<Lib,GzOpenGL,false,SubLib_System/OpenGL/,Lib_GZ_OpenGL,_Lib/GZE/SubLib_System/Lib_GZ_OpenGL/*Lib,GzEmscOpenGL,false,SubLib_System/EmscOpenGL/,Lib_GZ_EmscOpenGL,_Lib/GZE/SubLib_System/Lib_GZ_EmscOpenGL/*Lib,GzWinOpenGL,false,SubLib_System/WinOpenGL/,Lib_GZ_WinOpenGL,_Lib/GZE/SubLib_System/Lib_GZ_WinOpenGL/*;|I:/FlashDev/_MyProject/Simacode/LDK/LinxDemo/_out/_MainDemo/|" );
			
			string _sCompilerPaths =  Data.fGetGlobalVar("wToolchain") +" (" +  Data.fGetGlobalVar("wPlatform") + " : " + Data.fGetGlobalVar("wArch")   + ")" + "<<<";
			

            
                foreach(string _sInclude in Data.aAllInclude) {
					string _sPath =  _sInclude.Replace('"', ' ').Trim(); 
					string _sPathTrim = _sPath.TrimEnd('/').TrimEnd('\\');
					string _sName =   Path.GetFileName(_sPathTrim) ;
					if(_sName == "include" || _sName == "inc" ) { //Non explicit name add prec folder
						_sPathTrim = _sPathTrim.Substring(0,_sPathTrim.Length-_sName.Length-1);
						string _sPrecFolder =  Path.GetFileName(_sPathTrim );
						_sName = _sPrecFolder + "/" + _sName;
					}
					if(_sName[0] >=  '0' && _sName[0] <=  '9') { //Non explicit name add prec folder
						_sPathTrim = _sPathTrim.Substring(0,_sPathTrim.Length-_sName.Length-1);
						string _sPrecFolder =  Path.GetFileName(_sPathTrim);
						_sName = _sPrecFolder + "/" + _sName;
					}

					///MessageBox.Show("--IncludePath: " + _sInclude);
					_sCompilerPaths += "Lib,"  +_sName  + ",true," + _sInclude + ",,,*";
			    }
            

			_sCompilerPaths += ";";

                 /// MessageBox.Show("--SEND: " + _sCompilerPaths);

			
			string _sSrcPaths = "Project<<<";/*
			 foreach(KeyValuePair<string, SrcDiry> _oDir in Data.oArg.aPrjDirectory) {
					string _sDir =  _oDir.Value.sFile;
					string _sName = "Root";
					if(_sDir != "") {
						string _sPath =  _sDir.Replace('"', ' ').Trim(); 
						string _sPathTrim = _sPath.TrimEnd('/').TrimEnd('\\');
						 _sName =   Path.GetFileName(_sPathTrim) ;
						if(_sName == "include" || _sName == "inc" ) { //Non explicit name add prec folder
							_sPathTrim = _sPathTrim.Substring(0,_sPathTrim.Length-_sName.Length-1);
							string _sPrecFolder =  Path.GetFileName(_sPathTrim );
							_sName = _sPrecFolder + "/" + _sName;
						}
						if(_sName[0] >=  '0' && _sName[0] <=  '9') { //Non explicit name add prec folder
							_sPathTrim = _sPathTrim.Substring(0,_sPathTrim.Length-_sName.Length-1);
							string _sPrecFolder =  Path.GetFileName(_sPathTrim);
							_sName = _sPrecFolder + "/" + _sName;
						}
					}
					
					_sSrcPaths += "Lib,"  + _sName  + ",true," + _sDir + ",,,*";
			}*/
			_sSrcPaths += ";";

			
			string _sOutPaths = "Output<<<";/* If output files outside current project!?
			 foreach(KeyValuePair<string, SrcDiry> _oDir in Data.oArg.aPrjDirectoryOutput) {
					string _sDir =  _oDir.Value.sFile;
					string _sName = "Output";
					if(_sDir != "") {
						string _sPath =  _sDir.Replace('"', ' ').Trim(); 
						string _sPathTrim = _sPath.TrimEnd('/').TrimEnd('\\');
						 _sName =   Path.GetFileName(_sPathTrim) ;
						if(_sName == "include" || _sName == "inc" ) { //Non explicit name add prec folder
							_sPathTrim = _sPathTrim.Substring(0,_sPathTrim.Length-_sName.Length-1);
							string _sPrecFolder =  Path.GetFileName(_sPathTrim );
							_sName = _sPrecFolder + "/" + _sName;
						}
						if(_sName[0] >=  '0' && _sName[0] <=  '9') { //Non explicit name add prec folder
							_sPathTrim = _sPathTrim.Substring(0,_sPathTrim.Length-_sName.Length-1);
							string _sPrecFolder =  Path.GetFileName(_sPathTrim);
							_sName = _sPrecFolder + "/" + _sName;
						}
					}
					
					_sOutPaths += "Lib,"  + _sName  + ",true," + _sDir + ",,,*";
			}*/
			_sOutPaths += ";";

			bExtracted = true;
			fIsFinished();

			fIdeSend("cwc:A:|FinishExtract|" );

			fIdeSend("cwc:A:|LibInfo|"  + _sCompilerPaths + _sSrcPaths + _sOutPaths  + "|E:/_Project/_MyProject/Cwc/|" );


          //	Debug.fTrace("cwc:A:|LibInfo|ASSS<I:/FlashDev/_MyProject/Simacode/LDK/LinxDemo/_MainDemo/<Lib_Demo/<Lib,Demo,false,,Lib_Demo,Lib_Demo/*Rc,Exe,RcExe/,,false*;GZ<E:/MinGW/<_Lib/GZE/Lib_GZ/<Lib,GzCpp,true,LibCppWrapper/Lib_GZ/,Lib_GZ,_Lib/GZE/Lib_GZ/*Lib,GzCw,false,SimaCodeEngine/,Lib_GZ,_Lib/GZE/Lib_GZ/*Rc,RcEngine,Rc/,Rc/RcEngine/,true*;GzLite<I:/FlashDev/_MyProject/Simacode/LDK/GZE/<_Lib/GZE/SubLib_System/Lib_GZ_Lite/<Lib,GzLite,false,SubLib_System/Lite/,Lib_GZ_Lite,_Lib/GZE/SubLib_System/Lib_GZ_Lite/*;GzCpcDos<E:/MinGW/<_Lib/GZE/SubLib_System/Lib_GZ_CpcDos/<Lib,GzCpcDos,false,SubLib_System/CpcDos/,Lib_GZ_CpcDos,_Lib/GZE/SubLib_System/Lib_GZ_CpcDos/*;GzWindows<E:/MinGW/<_Lib/GZE/SubLib_System/Lib_GZ_Windows/<Lib,GzWindows,false,SubLib_System/Windows/,Lib_GZ_Windows,_Lib/GZE/SubLib_System/Lib_GZ_Windows/*;GzEmsc<I:/FlashDev/_MyProject/Simacode/LDK/GZE/<_Lib/GZE/SubLib_System/Lib_GZ_Emscripten/<Lib,GzEmsc,false,SubLib_System/Web/Emscripten/,Lib_GZ_Emscripten,_Lib/GZE/SubLib_System/Lib_GZ_Emscripten/*;GzOpenGL<I:/FlashDev/_MyProject/Simacode/LDK/GZE/<_Lib/GZE/SubLib_System/Lib_GZ_OpenGL/<Lib,GzOpenGL,false,SubLib_System/OpenGL/,Lib_GZ_OpenGL,_Lib/GZE/SubLib_System/Lib_GZ_OpenGL/*Lib,GzEmscOpenGL,false,SubLib_System/EmscOpenGL/,Lib_GZ_EmscOpenGL,_Lib/GZE/SubLib_System/Lib_GZ_EmscOpenGL/*Lib,GzWinOpenGL,false,SubLib_System/WinOpenGL/,Lib_GZ_WinOpenGL,_Lib/GZE/SubLib_System/Lib_GZ_WinOpenGL/*;|E:/MinGW/|" );
	

//			fIdeSend("cwc:A:|LibInfo|"  + _sCompilerPaths + _sSrcPaths + _sOutPaths  + "|I:/FlashDev/_MyProject/Simacode/LDK/LinxDemo/_out/_MainDemo/|" );
	//		MessageBox.Show("cwc:A:|LibInfo|"  + _sCompilerPaths + _sSrcPaths + _sOutPaths  + "|I:/FlashDev/_MyProject/Simacode/LDK/LinxDemo/_out/_MainDemo/|" );
			
				
			







	//		Debug.fTrace("cwc:A:|ClassInfo|E:\\_Project\\Cwc\\_Cwc_Demos\\Demos\\Base_Example\\01_HelloWorld\\HelloWorld.cpp|OpDebug:E:/_Project/_MyProject/Simacode/CWDK/GZE/SubLib_System/Windows/Sys/OpDebug.lnx:6#GZ.Sys/Debug,3,Sys/;#GZ.Sys/Debug,3,E:/_Project/_MyProject/Simacode/CWDK/GZE/SimaCodeEngine/Sys/Debug.lnx;GzWindows.Sys/OpDebug,6,E:/_Project/_MyProject/Simacode/CWDK/GZE/SubLib_System/Windows/Sys/OpDebug.lnx;GZ.Sys/Debug,3,E:/_Project/_MyProject/Simacode/CWDK/GZE/SimaCodeEngine/Sys/Debug.lnx;GZ.Class,20,E:/_Project/_MyProject/Simacode/CWDK/GZE/SimaCodeEngine/Class.lnx;GZ.ThreadMsg,8,E:/_Project/_MyProject/Simacode/CWDK/GZE/SimaCodeEngine/ThreadMsg.lnx;##OpDebug&76&Void&;fTrace1&115&Void&_sValue:String:115,;fTrace2&122&Void&_sValue:String:122,;fTrace3&129&Void&_sValue:String:129,;fWarning&136&Void&_sValue:String:136,;fError&142&Void&_sValue:String:142,;fPass&147&Void&_sValue:String:147,;fFatal&152&Void&_sValue:String:152,;#" );
			/*
			//wait for _oCppAst
			while(_oCppAst.fProcessIsRunning()) {
				
			}
			*/

			/// Load Cpp ast //// 
			
			
			
/*
			string _sIncludes = "";
				 foreach(string _sInclude in  Data.oArg.aAllInclude) {
                   // Debug.fTrace("---aInclude : " + _sInclude);
					string _sPath =  PathHelper.ExeWorkDir +  _sInclude;

                       //_sIncludes += "Test<" + _sPath    + "<" + "test/" +  "<" + "Lib,Demo,false,,Lib_Demo,Lib_Demo/*Rc,Exe,RcExe/,,false*;";
        


				Debug.fTrace("cwc:A:|LibInfo|"  + _sCompilerPaths  + "|I:/FlashDev/_MyProject/Simacode/LDK/LinxDemo/_out/_MainDemo/|" );
			//	Debug.fTrace("cwc:A:|LibInfo|"   + _sIncludes + "|I:/FlashDev/_MyProject/Simacode/LDK/LinxDemo/_out/_MainDemo/|" );
		                }
*/		


		}

        private void fIdeSend(string _sMsg) {
             Debug.fTrace(_sMsg);
        }

        public void fIsFinished() {
		if(!bFinishExtractSended) {
			if(oCppAstStarted && bExtracted) {
				bFinishExtractSended = true;
				fIdeSend("cwc:A:|FinishExtract|" );
			}
		}
	}

		
	
      public void fCppAstOut(LauchTool _oThis, string _sOut)  {
            if(_sOut == null){return;}
			if(_sOut.Length > 7 && _sOut[0] == 'c' && _sOut[1] == 'w' &&   _sOut[2] == 'c') { //cwcAst:
				if(_sOut == "cwcAst:Rdy") {
					if(!oCppAstStarted) {
						oCppAstStarted = true;
						fIsFinished();
					}
	
                    	fIdeSend("cwcAst:Rdy" );//ToDebug


                //  MessageBox.Show("cwcAst:Rdy ");
					oCppAst.fSend("*run|" + "E:/TestProjectFD/TestCw/src/HelloWorld.cpp|");
					
								//	Debug.fTrace("---------------------------------YEAGH");
				}else {
				//	 MessageBox.Show("FrromAstSend: "+  _sOut);
                  //  Debug.fTrace("cwc:OUT---- " + _sOut);
				    fIdeSend( _sOut );
				}
	//Debug.fTrace(_sOut + " lengt " + _sOut.Length.ToString());
			}else {
				fIdeSend("AA:" + _sOut );//ToDebug
			}
		
			
			//Debug.fTrace(_sOut);
		  // this.BeginInvoke((MethodInvoker)delegate {
					
		//	 });

		}

        public void fCppAstEnd(LauchTool _oLauch)  {
		/*
		   this.BeginInvoke((MethodInvoker)delegate {
					fGitCloneEnd();
			 });*/
		}

			
	}
}
