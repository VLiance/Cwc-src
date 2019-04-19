using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace cwc {
    class Msg {
        public const int WM_COPYDATA = 0x004A;
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);


        public struct R_COPYDATASTRUCT {
            public Int32 dwData;
            public Int32 cbData;
            public IntPtr lpData;
        }



       public static Process fFindExistantExe(string _sFullPath){
            Process _oThisProc = Process.GetCurrentProcess();
            string _sName = Path.GetFileNameWithoutExtension(_sFullPath);
            List<Process> aList = new    List<Process>();
            aList.AddRange(Process.GetProcessesByName(_sName));
            aList.AddRange(Process.GetProcessesByName(_sName + ".vshost"));

            foreach (Process _oProc in  aList ) {
                if(_oThisProc.Id != _oProc.Id) {
                    if (_oProc.MainModule.FileName.IndexOf( Path.GetDirectoryName( _sFullPath) ) != -1) {
                       return _oProc;
                    }
                }
            }
            return null;
        }

        /*
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parentWindow, IntPtr previousChildWindow, string windowClass, string windowTitle);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr window, out int process);
        
        public static IntPtr[] GetProcessWindows(int process) {
            IntPtr[] apRet = (new IntPtr[256]);
            int iCount = 0;
            IntPtr pLast = IntPtr.Zero;
            do {
                pLast = FindWindowEx(IntPtr.Zero, pLast, null, null);
                int iProcess_;
                GetWindowThreadProcessId(pLast, out iProcess_);
                if(iProcess_ == process) apRet[iCount++] = pLast;
            } while(pLast != IntPtr.Zero);
            System.Array.Resize(ref apRet, iCount);
            return apRet;
        }
        */




        //In: protected override void WndProc(ref Message m) (: Form)
        public static string fReceiveMsg(ref Message m) {
            switch (m.Msg)  {
                // program receives WM_COPYDATA Message from target app
                case WM_COPYDATA:
                    Console.WriteLine("WM_COPYDATA!!");
						 Object thisLock = new Object();
						 lock (thisLock) {

                                R_COPYDATASTRUCT cds = new R_COPYDATASTRUCT();
                                cds = (R_COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam,
                                typeof(R_COPYDATASTRUCT));

                                if (cds.cbData > 2) //2 to remove carriage return
                                {
                                    byte[] data = new byte[cds.cbData  ];
                                    Marshal.Copy(cds.lpData, data, 0, cds.cbData );

                                    Encoding unicodeStr = Encoding.UTF8; //Not UTF8 ?? Seem work
                                    string _sReceivedText = new string(unicodeStr.GetChars(data ));
                                    _sReceivedText = _sReceivedText.Substring(0,_sReceivedText.Length-2);

                                    return _sReceivedText;
							    }
						m.Result = (IntPtr)1;
						}
                    
                    break;

                default:
                    break;
            }
            return "";
        }


         public static void fSendMsg(string msg, IntPtr _nToHandle) {
		
             int result = 0;
            string _sMsg = (string)msg;
			//  Debug.fTrace("Static thread procedure. Data='{0}'", data);
			byte[] sarr = System.Text.Encoding.Default.GetBytes(_sMsg + "\0");
			int len = sarr.Length;
			COPYDATASTRUCT cds;
			cds.dwData = (IntPtr)0;
			cds.lpData = _sMsg;
			cds.cbData = len + 1;

            //  Debug.fTrace("Send " + (int)_nToHandle + "  Msg " + _sMsg);
			result = SendMessage((int)_nToHandle, WM_COPYDATA, 0, ref cds);
        }
    }
}
