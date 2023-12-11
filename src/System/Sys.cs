using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;

namespace cwc {
    class Sys {

        public static string sParentName = "";
	    public static Process oParentProcess = null;
            internal static int nConnectedHandle = 0;
  

            public static void fGetParentProcess(){


            try
            {
                var myId = Process.GetCurrentProcess().Id;
                var query = string.Format("SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {0}", myId);
                var search = new ManagementObjectSearcher("root\\CIMV2", query);
                var results = search.Get().GetEnumerator();
                results.MoveNext();
                var queryObj = results.Current;
                var parentId = (uint)queryObj["ParentProcessId"];
                oParentProcess = Process.GetProcessById((int)parentId);
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
