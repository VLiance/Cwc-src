using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwc {
    class Setting {

          public static SettingsLaunch oSettingsLaunch = new SettingsLaunch(PathHelper.ExeWorkDir + "Default");
         public static string sLaunchedName = "";
        internal static void fNewSettingsLaunch(string _sFile) {
            sLaunchedName = Path.GetFileNameWithoutExtension( _sFile);//Broken??
            oSettingsLaunch.fSetNewFile(_sFile);
           
        }

    }
}
