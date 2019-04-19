using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace cwc.Utilities {
    class RegisterFile {
        
        static string sExeCwc_Name = "Cwc.";



         public static string  fCwcRoot()  {
           return  PathHelper.CwcRootPath().Replace('/', '\\');
        }


        public static string  fCwcExe()  {
                return fCwcRoot() + "cwc.exe";
       }
       public static string  fIconPath(string _sExt)  {
                return fCwcRoot() + "Utils\\icon\\"+ _sExt + ".ico";
       }

           static RegistryKey oSoftwareKey;
           static RegistryKey oClassesKey;


       public static bool fRegisterAllFileType( bool  _bForce = false)  {

            _bForce=true; //temps
            string _sExeIDE = PathHelper.ToolDir +  PathHelper.sExeIDE;
             _sExeIDE = _sExeIDE.Replace('/', '\\');
            string _sExeIDE_Name = PathHelper.sExeIDE_Name;
 

            oSoftwareKey =  Registry.CurrentUser.OpenSubKey("Software",true);
            oClassesKey =  oSoftwareKey.OpenSubKey("Classes",true);

            if(IsAssociated("." + "cwMake")) { //Already set
                return false;
            }

             fRegisterFileType(fCwcExe(),sExeCwc_Name, "cwMake", "Header of Header src file",_bForce);
             fRegisterFileType(_sExeIDE,_sExeIDE_Name, "cwc", "Header of Header src file",_bForce);

             fRegisterFileType(_sExeIDE,_sExeIDE_Name, "cw",  "C~ src file",_bForce);
             fRegisterFileType(_sExeIDE,_sExeIDE_Name, "c",  "C src file",_bForce);
             fRegisterFileType(_sExeIDE,_sExeIDE_Name, "cc",  "C/C++ src file",_bForce);
             fRegisterFileType(_sExeIDE,_sExeIDE_Name,"cpp", "C++ src file",_bForce);
             fRegisterFileType(_sExeIDE,_sExeIDE_Name, "h",  "Header scr file",_bForce);
             fRegisterFileType(_sExeIDE,_sExeIDE_Name, "hh", "Header of Header src file",_bForce);

             //   fRefreshTree(); //Update ICONs


            RefreshWindowsExplorer();
            return true;
        }

        public static bool fRegisterFileType(string _sExePath,string _sNameExe, string _sExt, string _sDesc, bool  _bForce = false) {
            string _sIconPath = fIconPath(_sExt);
  
            if(File.Exists(_sIconPath)) { 
                if (_bForce || !IsAssociated("." + _sExt)) { 
            
                  
                    //string _sExePath = PathHelper.AppDir + "\\FlashDevelop.exe";
                    Associate("." + _sExt, _sNameExe + _sExt,_sDesc,_sIconPath, _sExePath);
                }

                 return true;
            }
            throw(new Exception("icon not exist: " +  _sIconPath));
            return false;
        }


        // Associate file extension with progID, description, icon and application
        public static void Associate(string extension,  string progID, string description, string icon, string application)
        {



           oClassesKey.CreateSubKey(extension).SetValue("", progID);
            if (progID != null && progID.Length > 0)
                using (RegistryKey key = oClassesKey.CreateSubKey(progID))
                {
                    if (description != null)
                        key.SetValue("", description);
                    if (icon != null)
                        key.CreateSubKey("DefaultIcon").SetValue("", ToShortPathName(icon));
                    if (application != null)
                        key.CreateSubKey(@"Shell\Open\Command").SetValue("",  ToShortPathName(application) + " \"%1\" %*");
                }
        }

        // Return true if extension already associated in registry
        public static bool IsAssociated(string extension){
            RegistryKey key = oClassesKey.OpenSubKey(extension, false);
            if (key != null) {

                    string _sVal = (string)key.GetValue("");
                    if(_sVal == null || _sVal.Length == 0) {return false; }
                   return true;
            }
                    //  MessageBox.Show("nullkey " + oClassesKey.ToString() );
             return false;
        }
 
        [DllImport("Kernel32.dll")]
        private static extern uint GetShortPathName(string lpszLongPath,  [Out] StringBuilder lpszShortPath, uint cchBuffer);
 
        // Return short path format of a file name
        private static string ToShortPathName(string longName)
        {
            StringBuilder s = new StringBuilder(1000);
            uint iSize = (uint)s.Capacity;
            uint iRet = GetShortPathName(longName, s, iSize);
            return s.ToString();
        }

   







    /// <summary>
    /// /REFRESH ICONS
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="flags"></param>
    /// <param name="item1"></param>
    /// <param name="item2"></param>
    /// <returns></returns>

    [System.Runtime.InteropServices.DllImport("Shell32.dll")]
    private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

    public static void RefreshWindowsExplorer()
    {
       // MessageBox.Show("RefreshWindowsExplorer");
        // Refresh the desktop
        SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);

        // Refresh any open explorer windows
        // based on http://stackoverflow.com/questions/2488727/refresh-windows-explorer-in-win7
        Guid CLSID_ShellApplication = new Guid("13709620-C279-11CE-A49E-444553540000");
        Type shellApplicationType = Type.GetTypeFromCLSID(CLSID_ShellApplication, true);

        object shellApplication = Activator.CreateInstance(shellApplicationType);
        object windows = shellApplicationType.InvokeMember("Windows", System.Reflection.BindingFlags.InvokeMethod, null, shellApplication, new object[] { });

        Type windowsType = windows.GetType();
        object count = windowsType.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, windows, null);
        for (int i = 0; i < (int)count; i++)
        {
            object item = windowsType.InvokeMember("Item", System.Reflection.BindingFlags.InvokeMethod, null, windows, new object[] { i });
            Type itemType = item.GetType();

            // Only refresh Windows Explorer, without checking for the name this could refresh open IE windows
            string itemName = (string)itemType.InvokeMember("Name", System.Reflection.BindingFlags.GetProperty, null, item, null);
            if (itemName == "Windows Explorer")
            {
                itemType.InvokeMember("Refresh", System.Reflection.BindingFlags.InvokeMethod, null, item, null);
            }
        }
    }







 }



}
