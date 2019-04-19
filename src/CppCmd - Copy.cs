using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwc {
    class CppCmd {
       public string sCmd;
       public bool bLink = false;
       public  Depandance oDepandance;
       public string sOutputFile = "";
       bool bSkip = false;

       public  List<string> aInclude = new List<string>();


       ArgumentManager oParent = null;



       public CppCmd (ArgumentManager _oParent, string _sCmd) {

            oParent = _oParent;
             sCmd  = _sCmd;

            ///Check for existing directory
            string[] _aArg = _sCmd.Split('-'); 
            foreach (string _sArg in _aArg) { if (!FileUtils.IsEmpty(_sArg)) { 
            
                string[] _aCmd = _sArg.Split(' ', '=', '+'); 
                  string _sCmdName = _aCmd[0].Trim();
                
                if(!Program.bNowBuilding) {
                    return;
                }
                  //2 arg cmd analyse
                // if(_aSub.Length >= 2) {
                    switch(_sCmdName) {

                            case "I":
                                      Console.WriteLine("I Detected!!!");
                                fIncludePath(fExtractSpaceVal(_sArg));

                            break;

                            case "o":
                                Console.WriteLine("O Detected!!!");
                                 fOutputCmd(fExtractSpaceVal(_sArg));

                            break;

                
                   }

                    //-wBuild=Sanitize "|"^
                    //By first letter
                    switch(_sCmdName[0]) {

                            case 'w':
                            //Console.WriteLine("O Detected!!!");
                                fCwcCommand(_sCmdName, _sArg);

                        break;
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
                 CppCompiler.fSend2Compiler(sCmd, bLink);
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


         public  void fOutputCmd(string _sPath) {


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

            int _nIndex =  _sValue.IndexOf(' ');
            if(_nIndex != -1) {
                return _sValue.Substring(_nIndex+1);
            }else {
                return "";
            }
        }




    }
}
