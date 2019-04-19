using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace cwc {
    class Debug {


                public static readonly Object oLockOutPut = new Object();
        public static readonly Object oLockError = new Object();

    	public static void  fTrace(string _sTxt) {
    	//public static void  fDebug(string _sTxt) {
			#if tDebug
			Console.WriteLine(_sTxt);
			#endif
		}


       	public static void  fAppOut(string _sTxt) {
			Debug.fPrint( _sTxt);
		} 
	

[DllImport("msvcrt40.dll")]
public static extern int printf(string format, __arglist);
//public static extern int printf(string format);
      public static List<String> aBackupPrint = new List<string>();

         public static void fPrintBackup() {
            foreach(string _sMsg in aBackupPrint) {
                fPrint(_sMsg);
            }
            aBackupPrint.Clear();
        }

      
     public static void fPrint(string _sMsg, int _nColorCode = -2) {
       
            if(Data.oGuiConsole != null){
              Data.oGuiConsole.fTraceColorCode(_sMsg + "\n", _nColorCode);
            } else {
                Console.WriteLine(_sMsg);
            }

         //   Console.WriteLine(_sMsg);
        //    Console.Write(_sMsg + "\n");

       //     if(Data.oLauchProject.oCurLauch != null && Data.oLauchProject.bReceiveOutput){
         //   }
           
            /*
            if(  Data.oSelectForm != null){
               Data.oSelectForm.fHide();
            }       
               
            bLastWasRPrint = false;
          PipeInput.fResetCursorPos(false);
       //    Console.SetCursorPosition(0,0);
        // Console.WriteLine(_sMsg);
          printf("%s\n", __arglist(_sMsg));
            
            if(Data.bConsoleMode){ /////    ///Give space end -> bug when reach buffer
                ///

               //    Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + PipeInput.nNumOfSpaceEnd); //Only eher require?

                int _nLastPos = Console.CursorTop;
                if(Console.CursorTop + PipeInput.nNumOfSpaceEnd < Console.BufferHeight){
                      Console.SetCursorPosition(0,Console.CursorTop + PipeInput.nNumOfSpaceEnd); 
                }

                Console.SetCursorPosition(0,_nLastPos); 

                // Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight ); //Only eher require?

             }
             */



            /*
        if(oGuiForm != null){
            oGuiForm.fFocus(); 
         }*/
    }


          public static void fWPrint(string _sMsg, int _nColorCode = -1) {
                
            if(Data.oGuiConsole != null){
              Data.oGuiConsole.fTraceColorCode(_sMsg, _nColorCode);
            } else {
                Console.Write(_sMsg);
            }
            
        //             bLastWasRPrint = false;
         //   PipeInput.fResetCursorPos();


             // printf("%s", __arglist(_sMsg));
          //      OutputDebugString(_sMsg  );

        }
            public static bool bLastWasRPrint = false;
      

            public static void fRPrint(string _sMsg) {

                   Console.Write("\r" + _sMsg);
            /*
                if (!bLastWasRPrint) {
                     printf("%s\n",__arglist(""));
                }
                bLastWasRPrint = true;
            PipeInput.fResetCursorPos();
           // Console.Write(_sMsg);
              printf("%s\r", __arglist(_sMsg));
              */
        }

    }
}
