using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwc{

    	public class DepandanceData{
           public bool bIsDateRead = false;
            public DateTime dtModified;
            public string sPath = "";
            public bool bIncluded = false;
        }



	public class Depandance : FileRead {

        string sPath = "";
        string sOutputPath= "";
     

          DateTime dtObjFile;
       // public bool bHaveNewerFile = true;
        public bool bHaveNewerFile = false;

           public CppCmd parent;
        public bool bFileIsMoreRecent = false;
        DepandanceData oMoreRecent = null;
        bool bReadAll = false;

        public void fReadDepandance(string _sText, CppCmd _oCmd, bool _bReadAll = false) { //_bReadAll for #Has_dependance
            bReadAll = _bReadAll;
             
            if (_oCmd != null) { //Be sure to Verify all depandant other source files
                 foreach (string _sFile in  _oCmd.aCompileFiles) {

                    if(fChekdependance(_sFile)) {
                        if(!bReadAll){ return;}
                    }
                }
            }



            //Replace("/:",":") (=> dgcpp)
            string[] aReadLine = _sText.Replace('\\','/').Replace("/:",":").Split(new[]{ "/\r\n", "\r\n", "\n"  }, StringSplitOptions.None);
            foreach (string _sALine in aReadLine) {
                    
              //  ": " First line is object reference -> Optimise?
              string   _sSubLine = _sALine.Substring(_sALine.IndexOf(": ")+1).Replace("/ ","*").Trim(); //When we have space Clang add '/'  "/ "," "

             foreach (string _sNewLine  in _sSubLine.Split(' ')) {
                 string  _sLine = _sNewLine.Replace('*', ' ');
                
                 if(_sLine=="" || _sLine[_sLine.Length-1] ==':'){continue;}

                    //Cwc Arg Line
                    if(_sALine.Length > 3 && _sALine[0] == 'C' &&  _sALine[1] == 'w' &&  _sALine[2] == 'c') { ///TODO test before to save time
                            // while(true){Thread.Sleep(1);}	
                            string _sCmd =   _sALine.Substring(4); //_sALine is unmodified : "/ " by " " in include folder

						    string _sOldCmd =  parent.sCompiler + " " +   parent.sConfig_Type + " " + parent.sExeCmdUnique +  " >> " +  parent.sRecompileOnChangeCmd;
					        if(_sOldCmd != _sCmd) { //Command are not the same 
                        	    fHaveNewerFile();
                                return;
                            }
                     }else {

                       if(fChekdependance(_sLine)) {
                              if(!bReadAll){ return;}
                        }

                    }  

                }
            }
        }

        public bool fChekdependance(string _sPath) {
            
    
            if (_sPath.Length >= 3 && _sPath[1]  != ':') { //It's a relative path
                    _sPath = PathHelper.ExeWorkDir + _sPath;
            }
            if(parent.oParent.fAddDepandance(this, _sPath)) { //  Stop if IsNewer??
                    return true;
            }

            return false;
        }


        public void fHaveNewerFile() {
            bHaveNewerFile = true;	
        }



        public Depandance(CppCmd _parent, string _sPath) {
		
            //TEMP DESACTIVATE
            //    bHaveNewerFile = true;
            ////////////////////
	
            parent = _parent;
            if(_sPath.Length > 1) {

            //  string _sToType =  Path.GetExtension(_sPath);
            if(_sPath[_sPath.Length-1] == 'o') {
               sOutputPath = _sPath;
               sPath =  _sPath.Substring(0, _sPath.Length-1) + "d";
          
                 if(File.Exists(sOutputPath) && File.Exists(sPath)) {

                       dtObjFile = File.GetLastWriteTime(sOutputPath);
                        fReadDepandance(File.ReadAllText(sPath),_parent,true);
                        return;

                }
             }
            }
            bHaveNewerFile = true;
        }


         public void fShowFile() {
            if (bFileIsMoreRecent) {
                 Output.Trace("\f67NewerFile: (" + Path.GetFileNameWithoutExtension(sPath)  + "):" +oMoreRecent.sPath   + ": " +oMoreRecent.dtModified +  " : " + dtObjFile);
            } 
        }


         public bool fIsNewer(DepandanceData _oFile) {

            try{
                if(_oFile.sPath == "") { //not found
                    return true;
                }

                if (!_oFile.bIsDateRead) {
                    _oFile.dtModified =  File.GetLastWriteTime(_oFile.sPath);
                    _oFile.bIsDateRead = true;
                }
               if (_oFile.dtModified > dtObjFile){
                        // Debug.fTrace("Newer: " +_oFile.sPath   + ": " +_oFile.dtModified +  " : " + dtObjFile);
                        bFileIsMoreRecent = true;
                         oMoreRecent = _oFile;

                        fHaveNewerFile();
                        return true;
                 }

           }catch  {
                fHaveNewerFile();
                return true;
            }
             return false;


        }
     

        public string fGetCppPath(string _sFile) {
            string _sResult =   _sFile ;
            
            if(File.Exists(_sFile)) {
                return _sResult;
            }

            /*
            //Search in order in all lib search path (-I)
            foreach(string sInclude in  parent.aInclude) {
                _sResult = sInclude + _sFile;

                if (File.Exists(_sResult)) {
                    return _sResult;
                }
            }*/
       //     Debug.fTrace("Not found :"  + _sFile);
            return "";
           
        }


		




	}
}
