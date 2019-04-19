using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwc.Utilities {

    public class ASSFileUtils {


        private  string sTab = "";
        private  string sFile = "";
        StreamWriter writer;

		public void fIniFile(string _sFile) {
			sFile = _sFile;

            fCreateDirectoryRecursively(Path.GetDirectoryName(_sFile));
			if(File.Exists(_sFile)) { //Stupid c# can't create empty file
				File.Delete(_sFile);
			}

            FileStream fs1 = new FileStream(_sFile, FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(fs1);
        }



		public void fSubTab() {
            sTab = sTab.Substring(0, sTab.Length - 1);
        }
        public void fAddTab()  {
            sTab += '\t';
        }

        public void fAdd(string _sLine){
            writer.WriteLine(sTab + _sLine );
        } 

		 public void fClose()  {   
		   writer.Close();
        }

		public static  void fCreateDirectoryRecursively(string path)
        {
            path.Replace('\\','/');
            string[] pathParts = path.Split('/');

            for (int i = 0; i < pathParts.Length; i++)
            {
                if (i > 0)
                    pathParts[i] = Path.Combine(pathParts[i - 1], pathParts[i]);

                if (!Directory.Exists(pathParts[i]))
                    Directory.CreateDirectory(pathParts[i]);
            }
        }

		public static bool fIsNewer(string _sSrcFile, string _sToFile) {
 
            try{
				
				if(!File.Exists(_sToFile)) {
					return true;
				}

				DateTime dtSrc = File.GetLastWriteTime(_sSrcFile);
                DateTime dtTo = File.GetLastWriteTime(_sToFile);
  
                if (dtSrc > dtTo){
                    return true;
                }
            }
            catch  {
                return true;
            }
         //   Debug.fTrace("--Skip Asm ------" + _sSrcFile);
            return false;
        }


    }
}
