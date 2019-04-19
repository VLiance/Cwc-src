using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
     
   
		public static string sOutpuFile = "";
		public static string sResendArg = "";
		public static string sWorkDir = "";

	  [STAThread]
       static void Main(string[] args)  {

		//	args = new string[1] { "E:\\_Project\\Modules\\Cwc\\temp\\CwcUpd_x32_0.0.0.2.7z" };

			 if (args.Length != 0) {

					sOutpuFile = args[0];
					sWorkDir = args[1];
					args = args.Skip(2).ToArray(); //shift
				   sResendArg = String.Join(" ", args);


						Application.EnableVisualStyles();
						Application.SetCompatibleTextRenderingDefault(false);
						Application.Run(new Form1());
			}else {

			}
        }
    }
}
