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
             ConfigMng.oConfig.fAddRecent(_sPath);

            string _sExtention = Path.GetExtension(_sPath).Trim();
          //  Debug.fTrace("GetExtension: " + Path.GetExtension(_sPath).ToLower());
            switch (_sExtention.ToLower()) {

                case ".cwmake":
                case ".cwc":
                     // Debug.fTrace("!!!");
                return fDelocaliseFile(_sPath, _sExtention);

                case ".exe":
                case ".bat":
                return fDelocaliseExe(_sPath);
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
                if(_sExtention.ToLower() == ".cwmake") {//Change default dir
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

	public static string fDelocaliseExe(string _sPath){
        
//fLauchConsoleCmd("");

   

		LauchTool _oSubCmd = new LauchTool();
		_oSubCmd.dExit  = new LauchTool.dIExit(fDelocaliseEnd);
		
        _oSubCmd.bRedirectOutput = false;
		//_oSubCmd.bReturnBoth= true;
		_oSubCmd.bReturnError = true;
		//_oSubCmd.bRunInThread = false;
		_oSubCmd.bWaitEndForOutput = true;

		string sResult = _oSubCmd.fLauchExe(_sPath, " @wDeloc ");

         _oSubCmd.fSend("Cwc:Lauch by " + Data.MainProcess.Handle); //Remove "pause" bug ?

		while(_oSubCmd.bExeLauch){
			//Thread.Sleep(1);
            Thread.CurrentThread.Join(1);
		}
		sResult = _oSubCmd.sError;


		int _nBegin = sResult.IndexOf("wOut");
		if(_nBegin != -1){
			sResult = sResult.Substring(_nBegin);
		}
		
	//	fDebug("!!RESULT!: " + sResult);
		//fDebug("-----------------------------: ");

		return sResult;
	}


    }
}
