using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwc {
    class ArgProcess {
        
         public static string fTestIfBeginWithAFile(string sArgExpand) {
          //  sArgExpand.Split();
          //  string[]  aPreArg = sArgExpand.Split(new string[] { " -", "|", ">" }, StringSplitOptions.None);
          int _nIndex = 0;
            while (_nIndex <  sArgExpand.Length) {
         
                  if (sArgExpand[_nIndex] == '.'  ) {
                    //Check if is a know file
                    if(  sArgExpand.Length > _nIndex){
                        string _sExt = sArgExpand.Substring(_nIndex+1);
                        _sExt = _sExt.Substring( _sExt.IndexOf(' ')+1);
                        _sExt = _sExt.Substring( _sExt.IndexOf('\n')+1);
                        _sExt = _sExt.Substring( _sExt.IndexOf('\t')+1);
                        switch (_sExt) {
                             case "cwc":
                             case "exe":
                             case "bat":
                            case "cwMake":
                                   return "-#Launch " + sArgExpand;
                             break;
                        }
                    }

                }
                    
                 if (sArgExpand[_nIndex] == '\"'  ) {
                    //First char is quote, we found a file

                    return "-#Launch " + sArgExpand;
                }
                if (sArgExpand[_nIndex] == '-' || sArgExpand[_nIndex] == '|' || sArgExpand[_nIndex] == '>' ) {
                     return sArgExpand;
                   // if(_nIndex>0 && !((sArgExpand[_nIndex-1] == ' ' || sArgExpand[_nIndex-1] == '\t' )) ){ continue; }

                }
                 _nIndex++;
            }

          return sArgExpand;
        }



       public static int fGetEndFile(string _sFullArg, int _nStartIndex ) {
            while(_nStartIndex < _sFullArg.Length) {

                //if(_sFullArg[_nStartIndex] <= 32 ) {
                if(_sFullArg[_nStartIndex] < 32 || _sFullArg[_nStartIndex] == '\"') { //Keep space?
                    break;
                }
           
                _nStartIndex++;
            }
            return _nStartIndex;
        }

        public static string fExpandAll(string _sFullArg ) {
			string _sResult = _sFullArg;
			int _nIndex = _sResult.IndexOf('@');
			while(_nIndex != -1 ) {
                char _cPrecIndex = ' ';
                if(_nIndex>1) {
                    _cPrecIndex = _sResult[_nIndex-1];
                }

                if(_cPrecIndex == ' ' || _cPrecIndex == '\t' || _cPrecIndex == '\n'  || _cPrecIndex == '\r'  || _cPrecIndex == '-') {
				    _sResult = fExpand(_sResult, _nIndex );
				     _nIndex = _sResult.IndexOf('@');
                }else {
                     _nIndex = _sResult.IndexOf('@', _nIndex+1);
                }
               
			}	

              _sResult = fRemoveComment(_sResult);

			return _sResult.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ');
		}

        private static string fRemoveComment(string _sText) {
            string _sResult = "";
            bool bInsideQuote = false;
            bool bInsideComment = false;
            int _nIndex = 0;
             int _nStartComment = 0;
             int _nLastGoodIndex = 0;
            while (_nIndex < _sText.Length) {
                if (_sText[_nIndex] == '\"') {
                    bInsideQuote = !bInsideQuote;
                   // continue;
                }
                if (!bInsideQuote) {

                    if (bInsideComment) { //Finish
                        if (_sText[_nIndex] == '\n') { //Comment
                             bInsideComment = false;
                         //   _sText = _sText.Substring(_nIndex);
                                
                             _nLastGoodIndex = _nIndex+1;

                        //     _nIndex=_nLastGoodIndex;
                                
                        }
                    }
                    if (_sText[_nIndex] == ';') { //Comment
                        _nStartComment = _nIndex;
                        bInsideComment = true;
                        _sResult += _sText.Substring(_nLastGoodIndex, _nIndex-_nLastGoodIndex);

                        //continue;
                    }

                }
                _nIndex++;
            }
            //_nIndex--;
            if(_nLastGoodIndex<_nIndex){
             _sResult += _sText.Substring(_nLastGoodIndex, _nIndex-_nLastGoodIndex);
            }
            return _sResult;
        }

        
        public static string fExpand(string _sFullArg , int _nIndex) {
//	Debug.fTrace("---fExpand!!!!! " + _sFullArg);
            //List<string> _sList = new List<string> ();

            string _sResult = "";
             int  _nSartIndex = 0;
             int  _nEndIndex = 0;
            int  _nIndexOfA = _nIndex;
			///////////////////////////////// Begin with @ 
            while(_nIndexOfA != -1 ) {
                _sResult += _sFullArg.Substring(_nSartIndex, _nIndexOfA - _nSartIndex);
                _nEndIndex = fGetEndFile(_sFullArg, _nIndexOfA+1);
       

                 string _sFile = _sFullArg.Substring(_nIndexOfA + 1, _nEndIndex - (_nIndexOfA + 1));

				//Debug.fTrace("-****************************!!!fExpand  " + _sFile);
				if(_sFile == "wDeloc") { //Special deloc command to quit	
					//Debug.fTrace("!!!---DELOCATISE!!!!!");
					_sFullArg = _sFullArg.Replace("@wDeloc", ""); //remove all wDeloc
					Console.Error.WriteLine("wOut|" + _sFullArg); //Use special stream to resend args
					SysAPI.fQuit(true);
				}


                 string _sPath;
				if(_sFile.Length > 2 && _sFile[1] == ':'){ //Absolute path
					_sPath = _sFile;
				}else{
					_sPath = PathHelper.ExeWorkDir + _sFile; //Relative path
				}

                 string _sText = "";
				
				 string _sExt = Path.GetExtension(_sPath).ToLower();


				
                try {
					switch(_sExt) {
						case ".bat":
						case ".exe":
							   _sText = Delocalise.fDelocalise(_sPath);
						break;
						default:
						       _sText = File.ReadAllText(_sPath);
       
						break;

					}
      


                }catch(Exception Ex) {
            
					 Output.TraceError("Error Expand File: " + Ex.Message + " : "+  _sPath);
				};

                _sResult += _sText;
             _nSartIndex = _nEndIndex;

             //   _nSartIndex = _nEndIndex;
                _nIndexOfA =  _sFullArg.IndexOf('@', _nEndIndex);
            }

              _sResult += _sFullArg.Substring(_nEndIndex, _sFullArg.Length - _nEndIndex );



            /*
           string[] _aFiles =   _sFullArg.Split('@');
            foreach(string _sFileArg in _aFiles) {

                int _nIndex = _sFileArg.IndexOf(' ');
                if(_nIndex == -1) {
                    _nIndex = _sFileArg.IndexOf('\t');  //TODO make "OR" combinaison
                }
                if(_nIndex == -1) {
                    _nIndex = _sFileArg.IndexOf('\n');
                }
                if(_nIndex == -1) {
                    _nIndex = _sFileArg.IndexOf('|');
                }
                if(_nIndex == -1) {
                    _nIndex = _sFileArg.IndexOf('>');
                }
                 if(_nIndex == -1) {
                    _nIndex = _sFileArg.Length;
                }
                
                 string _sFile = _sFileArg.Substring(0, _nIndex);
                
                 string _sPath = PathHelper.GetCurrentDirectory() + _sFile;
                 string _sText = "";
                try {
                    _sText = File.ReadAllText(_sPath);
                }catch(Exception Ex) {};

                 string _sArg =  _sFileArg.Substring(_nIndex);
                _sResult += _sText + _sArg;
           

            }     */

         //   return _sResult.Replace('\n', ' ');
          //  return _sResult.Replace('\n', ' ').Replace('\r', ' ');
            return _sResult;
        }



        
        public static void fFinishExtractArg() {
            /*
              if(Data.oMainForm != null) {
               Data.oMainForm.fFinishExtractArg();
            }*/
/*
Debug.fTrace("fFinishExtractArg!!!!!!!!!!!!!!!!!!!!!!!!-*-*--**--*-*-**-");
			if(bModeIDE) {
				
				Data.oModeIDE.fFinishExtractArg();
			}
   */           //Delete output to be sure to rebuild it
 

        }





    }
}
