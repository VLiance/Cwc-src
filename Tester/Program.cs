using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Tester{


public  class App 
{
    private static Mutex SingleMutex;
    public static uint MessageId;

    public App()
    {
        IntPtr Result;
        IntPtr SendOk;
        Win32.COPYDATASTRUCT CopyData;
        string[] Args;
        IntPtr CopyDataMem;
        bool AllowMultipleInstances = false;

        Args = Environment.GetCommandLineArgs();
        if(Args.Length < 1) {
            Args = new string[1];
            Args[0] = "";
        }

         var assembly = typeof(Program).Assembly;
         var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute),true)[0];
         var id = attribute.Value;

        // TODO: Replace {00000000-0000-0000-0000-000000000000} with your application's GUID
       // MessageId   = Win32.RegisterWindowMessage("{00000000-0000-0000-0000-000000000000}");
        MessageId   = Win32.RegisterWindowMessage(id);

        SingleMutex = new Mutex(false, "LiteWayvApp");

        if ((AllowMultipleInstances) || (!AllowMultipleInstances && SingleMutex.WaitOne(1, true)))
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CWide());
            //new Main();
        }
       // else if (Args.Length > 1)
        else {

            Console.WriteLine(Application.ExecutablePath);
            Process _oSame =  cwc.Msg.fFindExistantExe(Application.ExecutablePath);
            if(_oSame != null) {
              cwc.Msg.fSendMsg("NewInst|" + string.Join("|", Args).Trim() + "|", _oSame.MainWindowHandle); 
            }else {
            //     Console.WriteLine("Not FoUND!!!  "  + Process.GetCurrentProcess().Id);
             }

        }
    }
}



    static class Program{

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            new App();
      
        }
    }


   
    public class Win32
    {
        public const uint WM_COPYDATA = 0x004A;

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int    cbData;
            public IntPtr lpData;
        }

        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL             = 0x0000,
            SMTO_BLOCK              = 0x0001,
            SMTO_ABORTIFHUNG        = 0x0002,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
        }

        [DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageTimeout(
            IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam,
            SendMessageTimeoutFlags fuFlags, uint uTimeout, out IntPtr lpdwResult);
    }
}
