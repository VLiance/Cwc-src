using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cwc
{

public class ConsoleReader
{

		  public static int nScrollConsoleX = 0;
		  public static int nScrollConsoleY = 0;

		  public static int nLastScrollY = 0;
		  public static int nLastScrollX = 0;

		public  const int STD_OUTPUT_HANDLE = -11;
		public static IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);

  public static void fGetScroll(){

	
	 // Getting the console output device handle

		// Getting console screen buffer info
		CONSOLE_SCREEN_BUFFER_INFO info;
		GetConsoleScreenBufferInfo(handle, out info);

        //    Console.Write(info.dwCursorPosition.X + "\r");

			nScrollConsoleY = info.srWindow.Top;
			nScrollConsoleX = info.srWindow.Left;

			if(nScrollConsoleY != nLastScrollY){
				nLastScrollY = nScrollConsoleY ;
				if(MainForm.oSelectForm != null){
					PipeInput.bFoundSel = false;
					MainForm.oSelectForm.fHide();
				//	Console.Write(nScrollConsoleY + "\r");
				}
			}
			if(nScrollConsoleX != nLastScrollX){
				nLastScrollX = nScrollConsoleX ;
				if(MainForm.oSelectForm != null){
					PipeInput.bFoundSel = false;
					MainForm.oSelectForm.fHide();
				//	Console.Write(nScrollConsoleY + "\r");
				}
			}

			
	}

  //  public static IEnumerable<string> ReadFromBuffer(short x, short y, short width, short height)
    public static IEnumerable<string> ReadFromBuffer(short x, short y, short width, short height)
    {
        IntPtr buffer = Marshal.AllocHGlobal(width * height * Marshal.SizeOf(typeof(CHAR_INFO)));
        if (buffer == null)
            throw new OutOfMemoryException();

        try
        {
            COORD coord = new COORD();
            SMALL_RECT rc = new SMALL_RECT();
            rc.Left = x;
            rc.Top = y;
            rc.Right = (short)(x + width - 1);
            rc.Bottom = (short)(y + height - 1);

            COORD size = new COORD();
            size.X = width;
            size.Y = height;

            const int STD_OUTPUT_HANDLE = -11;
            if (!ReadConsoleOutput(GetStdHandle(STD_OUTPUT_HANDLE), buffer, size, coord, ref rc))
            {
                // 'Not enough storage is available to process this command' may be raised for buffer size > 64K (see ReadConsoleOutput doc.)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
			
fGetScroll();
/*
    // Getting the console output device handle
    IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);
    // Getting console screen buffer info
    CONSOLE_SCREEN_BUFFER_INFO info;
    GetConsoleScreenBufferInfo(handle, out info);



			nScrollConsoleY = info.srWindow.Top;
			nScrollConsoleX = info.srWindow.Left;
	//		Console.Write((info.srWindow.Top).ToString() +" : " +  rc.Right.ToString()   + " \r " );
*/
            IntPtr ptr = buffer;
            for (int h = 0; h < height; h++)
            {
                StringBuilder sb = new StringBuilder();
                for (int w = 0; w < width; w++)
                {
                    CHAR_INFO ci = (CHAR_INFO)Marshal.PtrToStructure(ptr, typeof(CHAR_INFO));
                    char[] chars = Console.OutputEncoding.GetChars(ci.charData);
                    sb.Append(chars[0]);
                    ptr = new IntPtr(ptr.ToInt64() + Marshal.SizeOf(typeof(CHAR_INFO)));
                }
                yield return sb.ToString();
            }
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct CHAR_INFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] charData;
        public short attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct COORD
    {
        public short X;
        public short Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct CONSOLE_SCREEN_BUFFER_INFO
    {
        public COORD dwSize;
        public COORD dwCursorPosition;
        public short wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadConsoleOutput(IntPtr hConsoleOutput, IntPtr lpBuffer, COORD dwBufferSize, COORD dwBufferCoord, ref SMALL_RECT lpReadRegion);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);



	[DllImport("Kernel32.dll")]
	private static extern int GetConsoleScreenBufferInfo(IntPtr hConsoleOutput,out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);


}




	public class PipeInput	{


		public  static Size fontSize;



//auto close if IDE close or id set in wCloseOnId
	public static void  fTestIdeClosed(int _nId) {
         //   Console.WriteLine("Test IDE CLosing");
			if(Data.nCloseOnId != 0){
					try{
						Process proc = Process.GetProcessById(Data.nCloseOnId);
					}
					catch(Exception){
                           Base.bAlive = false;

						//Data.fQuit(true);
						SysAPI.fQuit(true);
					}
			}
	}

        public static ushort nMousePosX = 0;
        public static  ushort nMousePosY = 0;


	public static void  NewPipeInput() {
         
            


			Console.SetIn(new StreamReader(Console.OpenStandardInput(8192))); // This will allow input >256 chars
			

          //Slow  
		Thread winThreadSelection = new Thread(new ThreadStart(() =>  {  
	       if (SysAPI.bIsLoadedFromAnotherCwcInstance) {  return;}
			while(Base.bAlive ) {
                //   var record = new NativeMethods.INPUT_RECORD();
				//uint recordLen = 0;
	            //    NativeMethods.ReadConsoleInput(Data.hConsoleInput, ref record, 1, ref recordLen);
               	fTestSelection(nMousePosX, nMousePosY);
                
				//Thread.Sleep(32); //30fps
                Thread.CurrentThread.Join(32);
            }
            }));  
           	winThreadSelection.Start();
         
            //Slow  
		Thread winThread3 = new Thread(new ThreadStart(() =>  {  
	
			while(Base.bAlive) {
	
			    SysAPI.fSetMainFormPosition();
				ConsoleReader.fGetScroll();

    
				Thread.Sleep(16);
			}
		//		Console.WriteLine("end "  );

		}));  
		winThread3.Start();





  
		if(Data.bGUI) {


				fontSize =  NativeMethods.GetConsoleFontSize();

			Thread winThread2 = new Thread(new ThreadStart(() =>  {  

				var record = new NativeMethods.INPUT_RECORD();
				uint recordLen = 0;

        

	            string _sCurrentCmd = "";
				while(Base.bAlive) {
					
				
				
					
				
					
					if ((NativeMethods.ReadConsoleInput(SysAPI.hConsoleInput, ref record, 1, ref recordLen))) {
		
			//	Debug.fTrace(" event " +record.EventType );

					 switch (record.EventType) {
						case NativeMethods.MOUSE_EVENT: {
														//	Debug.fTrace(string.Format("    dwEventFlags ....: 0x{0:X4}  ", record.MouseEvent.dwEventFlags));
							switch( record.MouseEvent.dwEventFlags) {
		
								case 0:
								break;
								case 4: //MouseWheel
						
									//If the high word of the dwButtonState member contains a positive value, the wheel was rotated forward, away from the user. Otherwise, the wheel was rotated backward, toward the user.
									//use the high order word to determine how far to scroll the window
									//divide the value by 5 and negate it to get the number of text lines to scroll.
									int _nWheelValue = record.MouseEvent.dwButtonState >> 16;
                                 
                                    int _nVal = (_nWheelValue / 120);
                                    if (_nVal == 0){
                                        if (_nWheelValue > 0){
                                            _nVal = 1;
                                                    
                                        } else {
                                             _nVal = -1;
                                        }
                                    }
                           
									fMouseWheel( _nVal * SystemInformation.MouseWheelScrollLines);
               
								break;

							}


						
                            nMousePosX =  record.MouseEvent.dwMousePosition.X;
                            nMousePosY =  record.MouseEvent.dwMousePosition.Y;
                            
					
							//string.Format("    X ...............:   {0,4:0}  ", record.MouseEvent.dwMousePosition.X)    +
							//Console.Write( _sRead + " \r ");
							//Debug.fTrace(string.Format("    Y ...............:   {0,4:0}  ", record.MouseEvent.dwMousePosition.Y));
							


								//Debug.fTrace("Mouse event");
								/*
								Debug.fTrace(string.Format("    X ...............:   {0,4:0}  ", record.MouseEvent.dwMousePosition.X));
								Debug.fTrace(string.Format("    Y ...............:   {0,4:0}  ", record.MouseEvent.dwMousePosition.Y));
							
								Debug.fTrace(string.Format("    dwButtonState ...: 0x{0:X4}  ", record.MouseEvent.dwButtonState));
								Debug.fTrace(string.Format("    dwControlKeyState: 0x{0:X4}  ", record.MouseEvent.dwControlKeyState));
								Debug.fTrace(string.Format("    dwEventFlags ....: 0x{0:X4}  ", record.MouseEvent.dwEventFlags));
								 */
						 } break;

						case NativeMethods.FOCUS_EVENT: {
							
				
							//	if(record.FocusEvent.bSetFocus) {
									if( Data.oMainForm != null) {
										 Data.oMainForm.fBringToFront(record.FocusEvent.bSetFocus);
									}
								//}

						} break;

						case NativeMethods.WINDOW_BUFFER_SIZE_EVENT: {
							 Debug.fTrace("Buffer event!!! " + record.WindowsBufferSizeEvent.dwSize );	
						} break;

						case NativeMethods.MENU_EVENT: {
							 Debug.fTrace("MENU_EVENT event!!! "  + record.MenuEvent.dwCommandId );	
						
						} break;

						case NativeMethods.KEY_EVENT: {
			
								if( record.KeyEvent.bKeyDown) {
							

									switch( (int)record.KeyEvent.UnicodeChar) {

                                            
                                
                                        case 0x1B://Escape
                                      Build.StopBuild();

                                     break;

                           

										case 0x0D:
										            
                                               Data.oLauchProject.fLauchConsoleCmd(_sCurrentCmd);
                                               _sCurrentCmd = "";

                                             
                                           //     Console.SetWindowPosition(0, Console.);

                                                break;
                                      case 0x08:
                                          if(_sCurrentCmd.Length > 0){
                                              _sCurrentCmd = _sCurrentCmd.Substring(0,_sCurrentCmd.Length-1);
                                              //Data.fWPrint("\r>> " + _sCurrentCmd + "  ");
                                               Console.SetCursorPosition(Console.CursorLeft - 1,Console.CursorTop); 
                                                Console.Write(" ");
                                                 Console.SetCursorPosition(Console.CursorLeft - 1,Console.CursorTop); 
                                            }
                                      break;

										default:

											if(record.KeyEvent.UnicodeChar != 0) {
                                                fResetCursorPos();
                                               
                                                Data.oLauchProject.fBeginEnterNewCmd();
                                                 
												Console.Write(	record.KeyEvent.UnicodeChar);
                                                _sCurrentCmd += record.KeyEvent.UnicodeChar;
											}
										break;
									}
///	Debug.fTrace(	(int)record.KeyEvent.UnicodeChar);
										///	Debug.fTrace(string.Format("    dwControlKeyState: 0x{0:X4}  ", record.KeyEvent.dwControlKeyState));
								}
                                

	/*	
								Debug.fTrace("Key event  ");
								Debug.fTrace(string.Format("    bKeyDown  .......:  {0,5}  ", record.KeyEvent.bKeyDown));
								Debug.fTrace(string.Format("    wRepeatCount ....:   {0,4:0}  ", record.KeyEvent.wRepeatCount));
								Debug.fTrace(string.Format("    wVirtualKeyCode .:   {0,4:0}  ", record.KeyEvent.wVirtualKeyCode));
								Debug.fTrace(string.Format("    uChar ...........:      {0}  ", record.KeyEvent.UnicodeChar));
								Debug.fTrace(string.Format("    dwControlKeyState: 0x{0:X4}  ", record.KeyEvent.dwControlKeyState));
	    */
							//	    if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape) {
                             //         Data.StopBuild();
                                 
                             //   }
                                if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.F1) {
                                       Data.sCmd = "StartBuild";
                                   
                                }
                                   if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape) {
                                       SysAPI.fQuit(true);
                                   
                                }
							} break;
						}
				
					}
 
					//Thread.Sleep(1); no need? slow down
				}

		}));  
		winThread2.Start();

	}else {

		}
	}

    public static void fLauchPipeInput() {

            if(!Data.bGUI) {
				//////// No GUI -> Process full pipe input //////////////
				Thread winThread = new Thread(new ThreadStart(() =>  {  
				PipeInput _oPipeIn = new PipeInput();
                 
                    
                //  MessageBox.Show("PipeInput: "+  Base.bAlive);

				//MessageBox.Show("Data read was ",  "start", MessageBoxButtons.OK, MessageBoxIcon.Warning);		
				while(Base.bAlive) {
				//	Debug.fTrace("-----NewPipeInput");
		
						Thread.CurrentThread.Join(1);
						Console.SetIn(new StreamReader(Console.OpenStandardInput(8192))); // This will allow input >256 chars
						while (Console.In.Peek() != -1){

							string input = Console.In.ReadLine();
							//	MessageBox.Show("Data read was ",  input, MessageBoxButtons.OK, MessageBoxIcon.Warning);		
				//					Data.oMsgForm.BeginInvoke((MethodInvoker)delegate  {
									_oPipeIn.fNewInput(input);
					//				});

						}

					}
				}));  
				winThread.Start();
            }
        }

   
          public static int nCursorPosX = 0;
          public static int nCursorPosY = 0;



        public static int nWheelVal = 0;
		public static void fMouseWheel(int _nLine){
				if(MainForm.oSelectForm != null){
					MainForm.oSelectForm.fHide();
				}

				ConsoleReader.fGetScroll();
				nWheelVal = ConsoleReader.nScrollConsoleY;
				nWheelVal -= _nLine;

				if(nWheelVal < 0) {
					nWheelVal = 0;
				}
            

                if (nWheelVal > Console.BufferHeight- Console.WindowHeight) {
                     nWheelVal = Console.BufferHeight- Console.WindowHeight;
               }
 //   Console.WriteLine(Console.BufferHeight + "  "  + Console.WindowHeight + "         " +nWheelVal  + "        \r" );

            try{

                if( Console.CursorVisible == true){
                   nCursorPosX = Console.CursorLeft;
                   nCursorPosY = Console.CursorTop;
                }

                int _nTop = nWheelVal;
                int _nBottom = nWheelVal +  Console.WindowHeight;

                if(_nLine > 0){
                    bIsToReset = true;
                     Console.SetCursorPosition(0,_nTop );
                      Console.CursorVisible = false;
                } else {
                        bIsToReset = true;
                         Console.SetCursorPosition(0,_nBottom);
                         Console.CursorVisible = false;
                }

                if(nCursorPosY >= _nTop &&  nCursorPosY < _nBottom ){ //cursor inside
                    fResetCursorPos();
                }

               // Console.SetWindowPosition(0, nWheelVal);  //bug

            } catch (Exception e) {
                Debug.fTrace("Wheel err " + e.Message + " " + e.Data );
            }
		}


           public static int nNumOfSpaceEnd = 2;

        public static bool bIsToReset = false;
        public static void fResetCursorPos(bool _bForce = false) {
            if(Data.bConsoleMode){
                if(bIsToReset || _bForce){
                    bIsToReset = false;
                    //try


                    
				//    Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + nNumOfSpaceEnd); //Slow

                    if(nCursorPosY + PipeInput.nNumOfSpaceEnd < Console.BufferHeight){
                      Console.SetCursorPosition(nCursorPosX,nCursorPosY+nNumOfSpaceEnd); //Give space end
                    }
                    Console.SetCursorPosition(nCursorPosX,nCursorPosY); //Show current pos
                    Console.CursorVisible = true;

                  //  Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight ); //Reset buff
                }
            }
            
        }


		public static bool bFoundSel = false;
	public static string sFile = "";
	public static int nLine = 0;
	public static int nColomn = 0;
	public static int nStartSelX = 0;
	public static int nEndSelX = 0;
	public static int  nStartSelY = 0;
	public static ushort  nLastX = 0;
	public static ushort  nLastY = 0;
    public static bool bRealive = false;

		private static void fTestSelection(ushort _x, ushort _y){

   
            if (nLastX != _x ||  nLastY != _y  ) {
                 if (nLastX != _x) {
                    nLastX = _x;
                }
                 if (nLastY != _y) {
                    nLastY = _y;
                }
            } else {
                return;
            }
             

           // Console.WriteLine("(short)Console.BufferWidth" + (short)Console.BufferWidth);
           int _nMaxWidth =  Console.BufferWidth;
         /*
            if (_nMaxWidth > 1000) {
                _nMaxWidth = 1000;
            }
            */
 
			string _sRead = "";
            //read to end
            int _nCurrPos = 0;
            int _nMaxReadBloc = 20;

            while(_nCurrPos < _nMaxWidth - _nMaxReadBloc){
                string _sPart = "";
		       foreach (string line in ConsoleReader.ReadFromBuffer((short)_nCurrPos, (short)_y, (short)_nMaxReadBloc, 1)) {
			       _sPart += line.Replace('\n',' ').Replace('\r',' ');
		       }
                if (_sPart.TrimStart() == "") {
                    break;
                }
                _sRead += _sPart;
               _nCurrPos += _nMaxReadBloc;
            }


			nStartSelY = (_y - (ushort)ConsoleReader.nScrollConsoleY) * fontSize.Height ;
			
			bFoundSel = false;
			nStartSelX = 0;
			nEndSelX = 0;
		
    
			//Find selectable file
			bool _bAcceptSpace = false;
		     bRealive = false;

              //Absolute path
			int _nStartIndex = _sRead.IndexOf(":/");
          	if(_nStartIndex == -1 ){
                 _nStartIndex = _sRead.IndexOf(":\\");
                	//	Console.Write( _nStartIndex  + "            \r");
            }

            //Relative path
          if(_nStartIndex == -1 ){ 
                 _nStartIndex = _sRead.IndexOf("/");
                 if(_nStartIndex == -1 ){
                      _nStartIndex = _sRead.IndexOf("\\");
                 }
                 //Get Prev folder
                 if(_nStartIndex >= 0 ){
                   
                     _nStartIndex = _sRead.LastIndexOf(" ",_nStartIndex) + 2;
                     //Console.Write(_nStartIndex + "aa  \r");
                    bRealive = true;
                 }
          }


			if(_nStartIndex >= 1 ){
				if(_nStartIndex >= 2 && ( _sRead[_nStartIndex-2] == '\"' || _sRead[_nStartIndex-2] == '\'') ){
					_bAcceptSpace = true;
				}
				if((_sRead[_nStartIndex - 1] >= 'A' && _sRead[_nStartIndex - 1] <= 'Z') || bRealive){
					nStartSelX = ((_nStartIndex -1  - (ushort)ConsoleReader.nScrollConsoleX)) * fontSize.Width;
                    
					int _nEndIndex = _nStartIndex + 1;
					while(_nEndIndex < _sRead.Length){
						if(_sRead[_nEndIndex] <  32  || _sRead[_nEndIndex] == '"' || _sRead[_nEndIndex] == '\''  || _sRead[_nEndIndex] == '<'  || _sRead[_nEndIndex] == '>' || _sRead[_nEndIndex] == ':'  || _sRead[_nEndIndex] == '*'  || _sRead[_nEndIndex] == '?'  || _sRead[_nEndIndex] == '|'){ //Space or special cher
							break;
						}
						if(!_bAcceptSpace && _sRead[_nEndIndex] ==  32){//Accept space??
							break;
						}

						_nEndIndex++;
					}
					
					if(_nEndIndex > _nStartIndex){
						bFoundSel = true;
						sFile = _sRead.Substring(_nStartIndex-1,_nEndIndex - (_nStartIndex-1) );
						//get line
						if(_sRead.Length >_nEndIndex && _sRead[_nEndIndex] == ':'){
							//string _sNumber = _sRead.Substring(_nEndIndex+1);
							string _sLine = "";
							_nEndIndex++;
							while(_nEndIndex < _sRead.Length &&  _sRead[_nEndIndex] >= '0' && _sRead[_nEndIndex] <= '9' ){
								_sLine += _sRead[_nEndIndex];
								_nEndIndex++;
							}
						//nLine = 0;
							Int32.TryParse(_sLine, out nLine);
	
							//get Colomn
							if(_sRead.Length >_nEndIndex+1 && _sRead[_nEndIndex] == ':'){
								_nEndIndex++;
								//string _sNumber = _sRead.Substring(_nEndIndex+1);
								string _sColomn = "";
								while(_nEndIndex < _sRead.Length && _sRead[_nEndIndex] >= '0' && _sRead[_nEndIndex] <= '9' ){
									_sColomn += _sRead[_nEndIndex];
									_nEndIndex++;
								}
								//nColomn = 0;
								Int32.TryParse(_sColomn, out nColomn);
			
							}
							if(_sRead.Length >_nEndIndex && _sRead[_nEndIndex] == ':'){
								_nEndIndex++;
							}
					//		_nEndIndex += _nEndIndex - _nEndIndex;			
						}
						nEndSelX = (_nEndIndex   - (ushort)ConsoleReader.nScrollConsoleX) * fontSize.Width - nStartSelX;
                           //    Console.Write(nEndSelX  + "            \r");
					}
		

				}
			}
			
			//oSelectForm.Location(0,0);
			//Console.Write( sFile  + "            \r");
		}

		public PipeInput() {}

	public void fNewInput(string _sMsg) {



       Debug.fTrace("fNewInput " + _sMsg);

				//MessageBox.Show("AstSend: "+  _sMsg);
		if(_sMsg.Length > 4 && _sMsg[0] == 'c' && _sMsg[1] == 'w'  && _sMsg[2] == 'c') {
	
			//MessageBox.Show("Yeah ",  _sMsg, MessageBoxButtons.OK, MessageBoxIcon.Warning);		
			if(Data.oModeIDE.oCppAstStarted) {
				string _sReal = _sMsg.Substring(4);
				string[] aMsg = _sMsg.Split('|');
				switch(aMsg[0]) {


						/*
						case "GetClassInfo" :
							
						break;
						*/

					default: 
             //           Debug.fTrace("*-Send ast " +"*" + _sReal );
						Data.oModeIDE.oCppAst.fSend("*" + _sReal);
              //      MessageBox.Show("AstSend: "+  _sMsg);
			
					break;
				}
				
				
				



			}
		}


	


	//	MessageBox.Show("Data read was ",  _sMsg, MessageBoxButtons.OK, MessageBoxIcon.Warning);		
		
		if(_sMsg == "wQuit") {
	//		MessageBox.Show("QUIIIIIIIIIIIIIIII!!!!!!!!!! ",  _sMsg, MessageBoxButtons.OK, MessageBoxIcon.Warning);		
		//	Debug.fTrace("QUITTTT!!!!!!!!! " + _sMsg);
			SysAPI.fQuit(true);
		}

		
	}


       


}
}
