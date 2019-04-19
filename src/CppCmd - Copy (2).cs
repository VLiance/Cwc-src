using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwc {
    class CppCmd {
       public string sCmd;
       public bool bLink = false;
       public bool bBuildWithoutLink = false;
       public bool bHaveKnowSourcesFiles = false;
       public bool bHaveKnowObjectFiles = false;
       public bool bHaveUnknowFiles = false;


       public  Depandance oDepandance;
       public string sOutputFile = "";
       bool bSkip = false;

       public  List<string> aInclude = new List<string>();
       public  List<string> aCompileFiles = new List<string>();


       ArgumentManager oParent = null;



       public CppCmd (ArgumentManager _oParent, string _sCmd) {

            oParent = _oParent;
             sCmd  = _sCmd;
            string _sPrecArg = "";

            ///Check for existing directory
            string[] _aArg = _sCmd.Split('-'); 
            foreach (string _sArg in _aArg) { if (!FileUtils.IsEmpty(_sArg)) { 
                
                    _sPrecArg = _sArg;
                string[] _aCmd = _sArg.Split(' ', '=', '+'); 
                  string _sCmdName = _aCmd[0].Trim();
            
                
                if(!Program.bNowBuilding) {
                    return;
                }

               if (!FileUtils.IsEmpty(_sCmdName)) {


                      //2 arg cmd analyse
                    // if(_aSub.Length >= 2) {
                        switch(_sCmdName) {

                                case "I":
                                    fIncludePath(fExtractSpaceVal(_sArg));

                                break;

                                case "o":
                                     fOutputCmd(fExtractSpaceVal(_sArg), _sPrecArg);
                                break;
                       }

                        //-wBuild=Sanitize "|"^
                        //By first letter
                        switch(_sCmdName[0]) {

                                case 'w':
                                    fCwcCommand(_sCmdName, _sArg);

                            break;
                       }


                 }
                // }
            }}
              
  
            if(!bLink) {

                   oDepandance = new Depandance(this, sOutputFile);
                    bSkip = !oDepandance.bHaveNewerFile;
            }

        }


        public  void  fExecute() {
             if(bLink) {
                  CppCompiler.CheckAllThreadsHaveFinishedWorking(true); //Force sequence??
                  Console.WriteLine("---linkCmd");

            }else { //Compile -> check depandency

                   oDepandance = new Depandance(this, sOutputFile);
                    bSkip = !oDepandance.bHaveNewerFile;
            }

           // Console.WriteLine("--------------Skip: " + bSkip.ToString() );

            if(!Program.bNowBuilding) {
                  return;
            }

            if(!bSkip) {
                CppCompiler.fSend2Compiler(sCmd, false);

                bool _bCompileAndLink = false;
                if(bLink & bHaveKnowSourcesFiles) {
                    _bCompileAndLink = true; //When compiling & link at the same time it's bug with the linker ( HelloWorld.cpp -o bin\HelloWorld.exe)
                }
                CppCompiler.fSend2Compiler(sCmd, bLink, _bCompileAndLink);

            }else {
                  Output.TraceColored("\f27UpToDate: \f28" + sCmd );
            }

        }




        public  void  fIncludePath(string _sPath) {
            Console.WriteLine("Inlcude:  " +_sPath);
            _sPath = _sPath.Replace('\\', '/').Trim();
            aInclude.Add(_sPath);
            oParent.aAllInclude.Add(_sPath);
        }



         public  void fAddFile(string _sFile) {

            aCompileFiles.Add(_sFile);
            //
            string _sExt = Path.GetExtension(_sFile).ToLower();
            switch(_sExt) {
                case ".c":
                case ".cpp":
                case ".c++":
                case ".cw":
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
  

         public  void fOutputCmd(string _sPath, string _sCmdFiles) {
            /*
            ////// IS compile without build
             string[] _aCmd = _sCmdFiles.Split(' '); 
              if(_aCmd.Length > 1) {
                 string _sCmdName = _aCmd[0].Trim();
                 if (!FileUtils.IsEmpty(_sCmdName)) {
                     switch(_sCmdName) {
                             case "c":
                                  bBuildWithoutLink = true;
                            break;
                    }
                }
                _sCmdFiles = _aCmd[1];
            }


              /// extarct all files
             string[] _aFile = _sCmdFiles.Split(' '); 
            foreach(string _sFile in _aFile) {
                if(!FileUtils.IsEmpty(_sFile)) {
                    fAddFile(_sFile);
                }
            }
            */


            sOutputFile = _sPath;
            string _sToType =  Path.GetExtension(_sPath);
            string _sDirectory = Path.GetDirectoryName(_sPath);

            FileUtils.fCreateDirectoryRecursively(_sDirectory);

            switch(_sToType) {
             
                case ".exe":
                case "":
                      bLink = true;
                     oParent.aLinkCmdList.Add(this);
                 break;
            }
        }


        /**
        public  void fCompileWithoutLink(string _sPath) {

        //    aCompileFiles.Add(_sPath);
         //   oParent.bHaveCompileWithoutLink = true;
            
        }
        */



        public void fCwcCommand(string _sCmd, string _sArg) {
            switch (_sCmd){
                case "wBuild":


                 break;

            }
        }


          public void fBuildCommand(string _sValue) {

        }

     public string fExtractValue(string _sValue) {

            int _nIndex =  _sValue.IndexOf('=');
            if(_nIndex != -1) {
                return _sValue.Substring(_nIndex+1);
            }else {
                return "";
            }
        }


        public string fExtractSpaceVal(string _sValue) {
            string _sResult = "";
            int _nIndex =  _sValue.IndexOf(' ');
            if(_nIndex != -1) {
                _sResult = _sValue.Substring(_nIndex+1).Trim();
                string[] aResult = _sResult.Split(' ');
                if(aResult.Length > 0) {
                    _sResult = aResult[0];
                }

            }
         //   Console.WriteLine("aaaa---------- " + _sResult);
           return _sResult;
            
        }




    }
}
