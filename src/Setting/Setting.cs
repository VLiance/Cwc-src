using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwc {
    class Setting {

          public static SettingsLauch oSettingsLauch = new SettingsLauch(PathHelper.ExeWorkDir + "Default");
         public static string sLauchedName = "";
        internal static void fNewSettingsLauch(string _sFile) {
            sLauchedName = Path.GetFileNameWithoutExtension( _sFile);//Broken??
            oSettingsLauch.fSetNewFile(_sFile);
           
        }

    }
}
