using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwc
{
	public class FileRead{

        private  string sFile = "";

        public string[] aLine = new String[]{};

		public void fIniFile(string _sFile) {
			sFile = _sFile;
            if(File.Exists(_sFile)) {
               aLine = File.ReadAllLines(_sFile);
            }else {
                Debug.fTrace("Not found");
            }

        }







		


	}
}
