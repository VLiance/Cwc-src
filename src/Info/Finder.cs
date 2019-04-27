using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwc {
    class Finder {


            public static CompilerData fUseCompiler(string _sVar, string sPlatform  = ""){
			Debug.fTrace("fUseCompiler : " +  _sVar + " "  + sPlatform );
		
			
			CompilerData _oCompiler =   fGetCompiler(_sVar, sPlatform);
			
			if(_oCompiler != null){
		    	_oCompiler.fUsed();
            }
			return _oCompiler;

		}

		public static CompilerData  fGetCompiler(string _sVar, string _sPlatform = "") {

			if(_sPlatform == ""){
				_sPlatform = "Default";
			}
			Data.fSetGlobalVar("wPlatform_Name", _sPlatform  );

			if (Data.aCompilerData.ContainsKey(_sVar)){
			//	CompilerData _oCompiler =  aCompilerData[_sVar];
				ModuleData _oCompiler = Data.aCompilerData[_sVar];
				if (_oCompiler.aPlatformData.ContainsKey(_sPlatform)){
					CompilerData _oPlatform = _oCompiler.aPlatformData[_sPlatform];
					return _oPlatform;
				}else{
					//Output.TraceError("Platform not exist: " + _sVar + " (" + _sPlatform + ")");
					Debug.fTrace("Platform not exist: " + _sVar + " (" + _sPlatform + ")");
					
					return null;
				}
			}
			//Output.TraceError("Compiler not exist: " + _sVar + " (" + _sPlatform + ")");
			return null;
		}
    }
}
