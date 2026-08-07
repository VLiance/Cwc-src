using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace cwc {
    class Sys {

        public static string sParentName = "";
	    public static Process oParentProcess = null;
            internal static int nConnectedHandle = 0;

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_BASIC_INFORMATION {
            public IntPtr Reserved1;
            public IntPtr PebBaseAddress;
            public IntPtr Reserved2_0;
            public IntPtr Reserved2_1;
            public IntPtr UniqueProcessId;
            public IntPtr InheritedFromUniqueProcessId;
        }

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(
            IntPtr processHandle,
            int processInformationClass,
            ref PROCESS_BASIC_INFORMATION processInformation,
            int processInformationLength,
            out int returnLength);

            public static void fGetParentProcess(){

            try
            {
                var myProcess = Process.GetCurrentProcess();
                var pbi = new PROCESS_BASIC_INFORMATION();
                int returnLength;
                int status = NtQueryInformationProcess(
                    myProcess.Handle,
                    0, // ProcessBasicInformation
                    ref pbi,
                    Marshal.SizeOf(typeof(PROCESS_BASIC_INFORMATION)),
                    out returnLength);

                if (status != 0) {
                    Console.WriteLine("Warning: unable to get parent process");
                    return;
                }

                int parentId = (int)pbi.InheritedFromUniqueProcessId.ToInt64();
                oParentProcess = Process.GetProcessById(parentId);
                sParentName = oParentProcess.ProcessName;
            }
            catch (Exception e) {
                Console.WriteLine("Warning: unable to get parent process");
            }

      //    Console.WriteLine("I was started by {0}", oParentProcess.ProcessName);
        // Console.WriteLine("I was started by {0}", oParentProcess.MainModule.ModuleName);

         }



            public static Process PriorProcess()  //IF already open
        // Returns a System.Diagnostics.Process pointing to
        // a pre-existing process with the same name as the
        // current one, if any; or null if the current process
        // is unique.
        {

            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
			
                if ((p.Id != curr.Id) && (p.MainModule.FileName == curr.MainModule.FileName)){
                    return p;
			    }
            }

            return null;

        }
    }
}
