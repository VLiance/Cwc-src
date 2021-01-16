using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwc
{
    using cwc;
    using System.Runtime.InteropServices; // DllImport() 
    using System.Threading;
    using global::System.Threading;

    class Output {


        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(uint nStdHandle);
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int mode);
        const int STD_INPUT_HANDLE = -10;
        const int ENABLE_QUICK_EDIT_MODE = 0x40 | 0x80;


  
       
       // string newS = sb.ToString();
        static bool bBold = false;
        static uint nFinalColor = 0;
        static uint nLetterColor = 7;
        static uint nForegroundColor = 0;

		static bool bCustomEscape = false;
          static bool bCustomEscapeChar2 = false;
          static char cCustomFirst = ' ';



       public static string Trace(string _sText, bool _bDirectTrace = false)  {

         /*
			if(_sText == null || _sText == ""){
				if(_sText == ""){
					Debug.fPrint("");
				}
				return "";
			}
            */
           if(_sText == null) {_sText = ""; };


           // _sText = _sText.Replace('\\', '/'); //Always replace??
         //	 string _sResult = "";

            lock(Debug.oLockOutPut) {
			
               return TraceColored(_sText);
                /*
                if(Data.bConsoleMode) {
                   return TraceColored(_sText);
                }else {
					return TraceWithoutColor(_sText,_bDirectTrace);
                }*/

            }
         //   return _sResult;
        }


		 public static string TraceWithoutColor(string _sText, bool _bDirectTrace = false)  {
				string _sResult = "";

                if(_bDirectTrace) {
                    Debug.fTrace(_sText);
                }else { //Remove Custom color code
						
                    foreach (char c in _sText)  {

                        if(bCustomEscapeChar2) {
                            bCustomEscapeChar2 = false;

                        }else if(bCustomEscape) {
                            cCustomFirst = c;
                            if(c == 's') { //Return 2 default
                            }else {
                                bCustomEscapeChar2 = true;
                            }

                            bCustomEscape = false;

                            }else if (c == '\x0C') {
                                bCustomEscape = true;
                            }else {
							_sResult += c;
                        }
                    }

			    	Debug.fPrint(_sResult );
                }

				return _sResult;
		}


		public static  string sBuffer = "";




        public  static void 	fPrjOut(string _sLetter,  string _sOut){

    if (_sOut == null){
        return;
    }

    string _sPrefix = _sLetter + "> " ;
    if(_sOut.Length > 4) { //ex T[1]:xxxx
        if(_sOut[1] == '[') {
            switch(_sOut[0]) {
                   case 'P':
                       Output.TraceGood(_sPrefix +_sOut);
                   break;
                   case 'E':
                       Output.TraceError(_sPrefix +_sOut);
                   break;
                    case 'W':
                       Output.TraceWarning(_sPrefix +_sOut);
                   break;
                   case 'A':
                       Output.TraceAction(_sPrefix +_sOut);
                   break;
                   case 'T':
                       Output.TraceStd(_sPrefix +_sOut);
                   break;
                   default:
                      	Output.Trace(_sPrefix +_sOut);
                   break;
            }
            return;
                
        }
    }
    Output.Trace(_sPrefix  + _sOut);
    return;
}

        public static void ProcessCmdSendToApp(string _sCmd, string _sAppName){
            string _sAppNameNorm = _sAppName.ToLower();
            foreach(LauchTool _oLauch in LauchTool.aLauchList){
                if( _oLauch.sExeName.ToLower() == _sAppNameNorm) {
                    Output.TraceActionLite("C> Send[" +_sAppName + "]:" +_sCmd);
                    _oLauch.fSend(_sCmd.Trim());
                    return;
                }
            }
             Output.TraceErrorLite("C> Unable to Send:[" +_sAppName + "]" +_sCmd);
        }


        public static List<string> aList = new List<string>();
        //Begin with "Cmd"
         public static void ProcessCmd(string _sOut){
            if(_sOut.Length > 5 && _sOut[3] == '['){
                //Get app name
                int _nStartIndex = 4;
                int _nEndIndex = _sOut.IndexOf(']', _nStartIndex);
                if(_nEndIndex != -1) {
                     string _sAppName = _sOut.Substring(_nStartIndex, _nEndIndex-_nStartIndex);
                     int _nStartCmd = _sOut.IndexOf(':', _nEndIndex);
                     if(_nStartCmd  != -1) {
                       string _sCmd = _sOut.Substring(_nStartCmd+1);
                       ProcessCmdSendToApp(_sCmd, _sAppName);
                        return;
                    }
                }
                 Output.TraceErrorLite("C> Unable to Process:" + _sOut);
            }
        }



        public static void ProcessStdErr( string _sOut){
            if(_sOut.Length > 4 && _sOut[0] == 'C' && _sOut[1] == 'm' && _sOut[2] == 'd' && (_sOut[3] == ':' || _sOut[3] == '('|| _sOut[3] == '[')){
                ProcessCmd(_sOut);

            }else { 

                if(_sOut.IndexOf("warning") != -1) {
                     Output.TraceWarningLite("W> " + _sOut);
                }else{
                     Output.TraceErrorLite("E> " + _sOut);
                }
            }
		}


        public static void TraceStd(string _sText){
			Output.TraceColored( "\f0F" + _sText ); 
		}

		public static void TraceWarningLite(string _sText){

			Output.TraceColored( "\f0E" + _sText ); 
		}

        public static void TraceErrorLite(string _sText)  {
			Output.TraceColored("\f0C" + _sText ); 
		}
        public static void TraceActionLite(string _sText)  {
			Output.TraceColored("\f0B" + _sText ); 
		}

        public static  string sWarningColor = "\fE4";
        public static  string sWarningColorLite = "\f0E";
		public static void TraceWarning(string _sText){

			Output.TraceColored(sWarningColor + _sText ); 
		}

        public static  string sUndefinedColor = "\f4D";
    	public static void TraceUndefined(string _sText){
			Output.TraceColored(sUndefinedColor + _sText ); 
		}
        


        public static  string sErrorColor = "\f4C";
        public static void TraceError(string _sText)  {

			Output.TraceColored(sErrorColor + _sText ); 
		}
		 public static void InternalError(Exception Ex)  {

			Output.TraceColored(sErrorColor + "InternalError: " +Ex.Message + " : " +Ex.Source  + " : " +Ex.StackTrace); 
		}


        public static  string sGoodColorLite = "\f0A";
		public static void TraceGood(string _sText)
		{
				Output.TraceColored("\f2A" + _sText ); 
		}
		public static void TraceAction(string _sText)
		{
				Output.TraceColored("\f3B" + _sText ); 
		}

        public static void TraceAction2(string _sText)
		{
				Output.TraceColored("\f3B" + _sText ); 
		}

         public static void TraceReturn(string _sText)
		{
		    	//Output.TraceColored( "\f08" + _sText ); 
		    	Output.TraceColored( "\f07" + _sText ); 
		}

         public static readonly Object oLockTrace = new Object();

        public static Boolean bFirstTrace = true;
        public static string TraceColored(string _sText)  { 
            if(bFirstTrace){bFirstTrace = false; fTraceThread(); }
            lock(oLockTrace) {
                 aTrace.Add(_sText);
            }
         
            return _sText;
        }

   
        public static List<string> aTrace = new List<string>();


         public static void fTraceThread() {

		    Thread winThread = new Thread(new ThreadStart(() =>  {  
			    while(Base.bAlive) {
                    string _sTrace = "";
                    while(aTrace.Count > 0){
                        lock(oLockTrace) {
                            if (aTrace.Count > 0) {
                              _sTrace = aTrace.First();
                                aTrace.RemoveAt(0);
                            }

                        }
                        if(_sTrace != ""){
                             TraceColoredThread(_sTrace);
                        } else {
                            Debug.fPrint("");
                        }
                    }
				    Thread.CurrentThread.Join(1);
			    }

		    }));  
		    winThread.Start();
        }


        public static string TraceColoredThread(string _sText)  { 
            
			if(!Data.bColor) {
				return TraceWithoutColor(_sText);
			}

              _sText = _sText.Replace('\\', '/'); //Always replace??
            
            string _sResult = "";
            string s = "\0\0\0\0";
            StringBuilder sb = new StringBuilder(s);
            int _nNumIndex = 0;
            bCustomEscape = false;
            bCustomEscapeChar2 = false;
            bBold = false;
			bool _bColorChange = false;

         string _sBuffer = "";

        int _nIndex_Start = 0;
        int _nIndex_End = 0;
             string _sShow = "";
            bool bInEscape = false;

          uint _nColorCode = (int)7;
          uint _nNextColorCode =_nColorCode;
          int _nForeGround = 0;
          int _nNumber = 7;

           foreach (char c in _sText)  {
                if(bCustomEscapeChar2) {
                    bCustomEscapeChar2 = false;
                    //   int _nNumber = 7;
                       _nNumber = 7;

                    if(c >= '0' && c <= '9') {
                         _nNumber = (int)c -'0';
                    }
                     if(c >= 'A' && c <= 'F') {
                         _nNumber = (int)c - 'A' + 10;
                    }
                     if(c >= 'a' && c <= 'f') {
                         _nNumber = (int)c - 'a' + 10;
                    }

                  //  int _nForeGround = 0;
                     _nForeGround = 0;
                    if(cCustomFirst >= '0' && cCustomFirst <= '9') {
                         _nForeGround = (int)cCustomFirst -'0';
                    }
                     if(cCustomFirst >= 'A' && cCustomFirst <= 'F') {
                         _nForeGround = (int)cCustomFirst - 'A' + 10;
                    }
                     if(cCustomFirst >= 'a' && cCustomFirst <= 'f') {
                         _nForeGround = (int)cCustomFirst - 'a' + 10;
                    }
					_bColorChange = true;	
                

                }else if(bCustomEscape) {
                    cCustomFirst = c;
                    if(c == 's') { //Return 2 default
						 _bColorChange = true;
                         _nNumber = 7;
                         _nForeGround = 0;

                    }else {
                        bCustomEscapeChar2 = true;
                    }

                    bCustomEscape = false;
          

                }else if (bInEscape)  {

                    if(c == 'm'  && sb.Length > _nNumIndex){
                        bInEscape = false;
                     
                         sb[_nNumIndex] = '\0';
                        if(_nNumIndex > 0) { 
                          fExecuteCode( sb.ToString()  );
                        }else {
                            bCustomEscape = false;
                           fExecuteCode("0");
                        }
						 _bColorChange = true;
                         _nColorCode =  fExecuteColor();

                          bBold = false;
                        nFinalColor = 0;
                        nLetterColor = 7; //1 default white
                        nForegroundColor = 0;
                        _nNumIndex = 0;
                           bCustomEscape = false;

                    }
                    if(c == ';' && sb.Length > _nNumIndex){
                         sb[_nNumIndex] = '\0';
                        _nNumIndex = 0;
                          fExecuteCode(sb.ToString() );

                    }
                    if(c >= '0' && c <= '9' && sb.Length > _nNumIndex) {
                         sb[_nNumIndex] = c;
                        _nNumIndex++;
                    }


                }else {

                     if (c == '\x0C') {
                           bCustomEscape = true;
                     }else if (c == 0x1B ) {
                        bInEscape = true;
                      
                    } else if (!bInEscape ) {
						
						if(_bColorChange) {	_bColorChange = false;

                            if(_sShow != ""){

							    Debug.fWPrint(_sShow,(int)_nColorCode);
                                _sResult  += _sShow;
                                _sShow = "";
                            }
                           /*
                            if(Data.oGuiConsole != null){
                                SetConsoleTextAttribute(SysAPI.hConsole, (int)( (_nForeGround << 4) | _nNumber) );
                            }*/
                             _nColorCode =  (uint)( (_nForeGround << 4) | _nNumber);
                                    
						}
					    _sShow += c;
                    }
                }
           
            }
  
            if(_sShow != ""){
               
			   Debug.fPrint(_sShow, (int)_nColorCode);
                _sResult  += _sShow;
             }
             
           //  SetConsoleTextAttribute(SysAPI.hConsole, (int)7);
            _nColorCode =  (int)7;

            return _sResult;
        }


        public static void fExecuteCode(string _sTest)  {

            uint _nNumber = 0;
            try { 
             _nNumber =  UInt32.Parse( _sTest);
            }catch(Exception e) {return;};

             if(bCustomEscape) {
                nLetterColor = _nNumber;
                return;
            }

            if(_nNumber == 0)  {
               // _nNumber = 37;
               return;
            }

            if(_nNumber == 1)  {
                bBold = true;
                return;
            }

            //background
            if(_nNumber >= 40 && _nNumber <= 47 ) {
                nForegroundColor = _nNumber - 40;
            }

            if(_nNumber >= 30 && _nNumber <= 37 ) {
                _nNumber -= 30;
                _nNumber = fCorrectColor(_nNumber);
                nLetterColor = _nNumber;
            }
        }

         public static uint fCorrectColor(uint _nColor)  {

            switch (_nColor) {

                case 1:
                    return 4;


                case 3:
                    return 6;


                case 4:
                    return 1;
   

                case 6:
                    return 3;
            }
            return _nColor;
        }


        public static void fPrintBuffer()  {
			Debug.fWPrint(sBuffer);
			sBuffer = "";
		}

        public static uint fExecuteColor()  {
            uint _nFinalNumber = nLetterColor;
            if(bBold) {
                _nFinalNumber += 8;
            }
            _nFinalNumber = _nFinalNumber | (nForegroundColor << 4);
			/*
            if(Data.oGuiConsole != null){
                SetConsoleTextAttribute(SysAPI.hConsole, (int)_nFinalNumber);
            }*/
            return _nFinalNumber;
        }
        
    }
}
