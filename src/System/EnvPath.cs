using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwc
{
    class SystemPathModifier{
        #region Pre-run Checks
        enum Action
        {
            NO_ACTION = 0,
            ADD_PATH = 1,
            REMOVE_PATH = 2
        }

        // DisplayUsage() displays instructions on the command line
        // of this program.
        public static void DisplayUsage()
        {
            Debug.fTrace("Usage : SystemPathModifier [/a or /r] [Path].");
        }

        // CheckArguments() interprets the command line of the program
        // and sets values for the variables of this program.
         static bool CheckArguments(string[] args, out Action action, out string strSpecifiedPath)
        {
            // Assign receiver to default value.
            action = Action.NO_ACTION;
            strSpecifiedPath = "";

            // Ensure that there are at least 2 arguments.
            if (args.Length < 2)
            {
                DisplayUsage();
                return false;
            }

            if (args[0] == "/a")
            {
                action = Action.ADD_PATH;
                strSpecifiedPath = args[1];
                return true;
            }
            else if (args[0] == "/r")
            {
                action = Action.REMOVE_PATH;
                strSpecifiedPath = args[1];
                return true;
            }
            else
            {
               // Debug.fTrace("Invalid flag : {0:S}.", args[0]);
                DisplayUsage();
                return false;
            }
        }
        #endregion


        static List<string> sUnavalaible = null;

        static string[] sCurList = null;

        public  static void fReadPath() {
            sCurList = null;
            GetCurrentSystemPaths();
        }

        #region System Path-related Helper functions.
        // GetCurrentSystemPaths() obtains the system path from the registry setting :
        // HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Environment
        // After obtaining this registry setting (which is a string), this function 
        // splits up the string into its constutuent parts (separated by a ';') and
        // stores each sub-string into a string array.
        public  static string[] GetCurrentSystemPaths(string _sCustomSet = "") {
              
            if(_sCustomSet == "" && sCurList != null) {
                 return sCurList;
            }

            string strPath;
            if(_sCustomSet == "") {
                strPath = System.Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
            }else {
                strPath = _sCustomSet;
            }

            string[] split = strPath.Split(new Char[] { ';' });

            List<string> _sResult = new List<string>();

            foreach(string _sCur in split) {
                if(!FileUtils.IsEmpty(_sCur) && _sCur.Length > 2) {
                    string _sVal = _sCur.Replace('\\','/').Trim();
                    if(_sVal[_sVal.Length-1] != '/') {
                        _sVal += "/";
                    }
                  _sResult.Add(_sVal);
                }
            }
             string[] _sResultFinal = _sResult.ToArray();

        //    Array.Sort(_sResultFinal);
            sCurList = _sResultFinal;
            return _sResultFinal;
        }





        public  static int fIsSet(string _strCurPath) {
            /*
             _strCurPath= _strCurPath.Replace('\\','/');
            if(_strCurPath[_strCurPath.Length-1] != '/') {
                _strCurPath += "/";
            }*/

            string[] strPaths = GetCurrentSystemPaths();
            sUnavalaible = new List<string>();

            
            //Search for other installation
            int _nFound = 0;
            for(int i = 0; i <  strPaths.Length; i++) {
                string _sCur = strPaths[i];


                 if(_sCur[ _sCur.Length-1] == ')') {
                     try { 
                       _sCur = _sCur.Substring(0,  _sCur.IndexOf('(') - 2); //sure to finish with \
                    }catch(Exception Ex) { }
                  //  _sCur = _sCur.Substring(0, _sCur.Length - sCwcSet.Length); //sure to finish with \
                }

                // Debug.fTrace("------ " + _sCur +  "cwc.exe");


                if(File.Exists( _sCur + "cwc.exe")) {
                     if(_nFound == 0) {   _nFound = i; }  //Found only the first one
                   
                   // if( sCurList[i][ sCurList[i].Length -1 ] != ')') {
                         sCurList[i]  =  sCurList[i] + sCwcSet;
                  //  }
                    //Debug.fTrace("Other Installation : " + _sCur);
                }else {

                      if (Path.GetFileName(_sCur.Substring(0,_sCur.Length-1)) == "Cwc") {
                           sCurList[i]  =  sCurList[i] + sCwcOld;
                    }else  if(!Directory.Exists(_sCur)) {
                        sCurList[i]  =  sCurList[i] + sCwcInvalid;
                    }
                    ////////////////////////
                }

            }

            return _nFound;
        }



        // AddPath() adds a new path to the system path.
        public  static bool AddPath(string strNewPath)
        {

	try {
            string[] strPaths = GetCurrentSystemPaths();

            if (fIsSet(strNewPath) == 0)  {

	
                // Path not found. We now add it in.
                int iSize = strPaths.Length;
                // We increment the strPaths array by one more element.
                Array.Resize(ref strPaths, iSize + 1);
                // Append strNewPath into the strPaths array.
                strPaths[iSize] = strNewPath;

                // We construct the system path string by concatenating
                // all elements from the strPaths array, separated by a ';'.
                string strNewSystemPath = ConcatStringArray(ref strPaths);

                // Replace the current system path with strNewSystemPath.
                 GetCurrentSystemPaths(strNewSystemPath); //Reset Path 

//Debug.fTrace("--strNewSystemPath: " + strNewSystemPath );
                new Thread(delegate() {
					try {
                 System.Environment.SetEnvironmentVariable("Path", strNewSystemPath, EnvironmentVariableTarget.Machine);
					}catch(Exception ex) {
							Debug.fTrace(ex.Message);
					};
                }).Start();
		


               return true;
             
            }
	}catch(Exception ex) {
									Debug.fTrace(ex.Message);
	};

            return false;
        }


         public  static bool fRemoveAllCwc(string strRemovalPath) {
            strRemovalPath= strRemovalPath.Replace('\\','/');
            if(strRemovalPath[strRemovalPath.Length-1] != '/') {
                strRemovalPath += "/";
            }
            string[] strPaths = GetCurrentSystemPaths();

            string strRet = "";
            foreach(string _sPath in strPaths) {

                if(_sPath[_sPath.Length-1 ] != ')') {
                    strRet += fReFormatString(_sPath);
                }
            }

              // Replace the current system path with strNewSystemPath.
                GetCurrentSystemPaths(strRet); //Reset Path


                new Thread(delegate() {
                 System.Environment.SetEnvironmentVariable("Path", strRet, EnvironmentVariableTarget.Machine);
                }).Start();

            return true;
        }


        public  static bool RemovePath(string strRemovalPath) {
            strRemovalPath= strRemovalPath.Replace('\\','/');
            if(strRemovalPath[strRemovalPath.Length-1] != '/') {
                strRemovalPath += "/";
            }

            string[] strPaths = GetCurrentSystemPaths();

            if (fIsSet(strRemovalPath) != 0)  {
                // We construct the new system path string by concatenating
                // all elements from the strPaths array, separated by a ';'.
                string strNewSystemPath = ConcatStringArray(ref strPaths, strRemovalPath);

                // Replace the current system path with strNewSystemPath.
                GetCurrentSystemPaths(strNewSystemPath); //Reset Path


                new Thread(delegate() {
                 System.Environment.SetEnvironmentVariable("Path", strNewSystemPath, EnvironmentVariableTarget.Machine);
                }).Start();
                  return true;

            }
              return false;
        }
      


       static string fReFormatString(string _sPath ) {
              string _result = _sPath.Replace('/', '\\');
            if(_result[ _result.Length-1] == ')') {
                try { 
                   _result = _result.Substring(0,  _result.IndexOf('(') - 2); //sure to finish with \
                }catch(Exception Ex) { }
            }
            if(_result[ _result.Length-1] == '\\') {
               _result = _result.Substring(0, _result.Length - 1); //sure to finish with \
            }
            if (_result != "") {
                _result += ";";
            }

            return _result;
        }


           static string sCwcSet = " (Cwc Set)";
           static string sCwcInvalid = " (Directory not Exist)";
           static string sCwcOld = " (Old Cwc, directory not Exist)";

        // This overload of ConcatStringArray() concatenates all strings from
        // the input strArray but will ensure that strExclude is excluded. 
        // Each string separated by a ';'.
         public static string ConcatStringArray(ref string[] strArray, string strExclude = "")
        {
            string strRet = "";

            foreach (string str in strArray)  {

                if ( strExclude == "" || String.Compare(str, strExclude, true) != 0 ){
                   
                    strRet += fReFormatString(str);
                }
            }
            return strRet;
        }




        // The PathMatcherPredicate class serves as the functor
        // for the Predicate delegate of the Array.Find() method.
        class PathMatcherPredicate
        {
            public PathMatcherPredicate(string strSpecifiedPath)
            {
                m_strSpecifiedPath = strSpecifiedPath;
            }

            public bool MatchPath(string strTestPath)
            {
                if (String.Compare(strTestPath, m_strSpecifiedPath, true) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            private string m_strSpecifiedPath;
        }

        #endregion
      
    }
}