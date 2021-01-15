using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwc {
    class Delocalise {

        

	public static void 	fDelocaliseEnd(LauchTool _oTool){
		Debug.fTrace("fDelocaliseEnd--------");
	}


 public static string sDelocaliseCmd = "";
    public static void  fDelocaliseInMainThread(string _sPath){
       
			_sPath = CppCompiler.fExtracQuote(_sPath, false).Trim();

           Data.sCmd = "Delocalise";
            Delocalise.sDelocaliseCmd = _sPath;
            Setting.fNewSettingsLauch(_sPath);

    }
    
   
	public static void fDelocaliseCmd(){

            //Reset all -> Already clear in Data.fClearData()
            /*
            Data.aVarGlobal.Clear();
             Data.fSetDefaultVar();
            if(Data.oGuiConsole != null) {
                Data.oGuiConsole.fLoadData();
            }*/

           Output.TraceWarning("Lauch " + sDelocaliseCmd);
            string _sText =  Delocalise.fDelocalise(sDelocaliseCmd);
			Data.sArgExpand  = ArgProcess.fExpandAll(Data.oArg , _sText);
	       Data.sCmd = "StartBuild";
            Data.bForceTestNextCmd = true;
      }



        
	public static string fDelocalise(string _sPath){
            _sPath = _sPath.Replace('\"',' ');//REmove quote
           // Debug.fTrace("Deloc: " + _sPath);
            if (!File.Exists(_sPath)) {
                 Output.TraceError("File not found: " + _sPath);
            }
            

            string _sExtention = Path.GetExtension(_sPath).Trim();
          //  Debug.fTrace("GetExtension: " + Path.GetExtension(_sPath).ToLower());
            switch (_sExtention.ToLower()) {

                case ".cwclean":
                case ".cwmake":
                case ".cwc":
					 ConfigMng.oConfig.fAddRecent(_sPath); //cwclean?
                     // Debug.fTrace("!!!");
                return fDelocaliseFile(_sPath, _sExtention);

                case ".exe":
					Output.TraceAction("Run: " + _sPath );
					 return fDelocaliseExe(_sPath, "");
                case ".bat":
					 Output.TraceAction("Run: " + _sPath );
                return fDelocaliseExe("cmd.exe", "/C "+ _sPath);
                default:
                    Output.TraceError("Unrecognised extention: " +_sExtention);
                    break;
            }
            return "";

      }


        
	public static string fDelocaliseFile(string _sPath, string _sExtention){
            string _sResult = "";
                 //  Debug.fTrace("fDelocaliseFile: " + _sPath);
            try{
                 _sResult = File.ReadAllText(_sPath);
                if(_sExtention.ToLower() == ".cwmake" || _sExtention.ToLower() == ".cwclean") {//Change default dir
                    string _sDirectory = Path.GetDirectoryName( Path.GetFullPath(_sPath));
                     // Debug.fTrace("wDir = " + _sDirectory);
                    	//fSetGlobalVar("wDir", _sDirectory );

                     Data.oArg = new ArgumentManager(null, _sDirectory); //Reset ARG Sure?
     
                   CppCmd.fSetWorkingDir(Data.oArg, _sDirectory);
                 

                  //  ArgumentManager.fSetVar(oArg, );
                  //   .o
			       // fSetWorkingDir(_sFullValue);
                }

            }catch(Exception e){}
              //   Debug.fTrace("Result: " + _sResult);
            return _sResult;
      }

	public static string fDelocaliseExe(string _sExe, string _sArg){

		LauchTool _oLauch = new LauchTool();
            _oLauch.dOut =  Data.oLauchProject.fAppOut;
            _oLauch.dError =  Data.oLauchProject.fAppError;
			_oLauch.bHidden = true;
            _oLauch.UseShellExecute = false;
            _oLauch.bRedirectOutput = true;
            _oLauch.bReturnBoth = true;
			_oLauch.bWaitEndForOutput = true;
			_oLauch.fLauchExe(_sExe, _sArg);
            
		while(_oLauch.bExeLauch){
            Thread.CurrentThread.Join(1);
		}

		string sResult = _oLauch.sResult;

		return sResult;
	}


    }
}
