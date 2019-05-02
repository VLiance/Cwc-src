using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwc {
    class Finder {


            public static CompilerData fUseCompiler(string _sVar, string sType  = ""){
			Debug.fTrace("fUseCompiler : " +  _sVar + " "  + sType );
		
			
			CompilerData _oCompiler =   fGetCompiler(_sVar, sType);
			
			if(_oCompiler != null){
		    	_oCompiler.fUsed();
            }
			return _oCompiler;

		}

		public static CompilerData  fGetCompiler(string _sVar, string _sType = "") {

			if(_sType == ""){
				_sType = "Default";
			}
			Data.fSetGlobalVar("_sConfig_Type", _sType  );

			if (Data.aCompilerData.ContainsKey(_sVar)){
			//	CompilerData _oCompiler =  aCompilerData[_sVar];
				ModuleData _oCompiler = Data.aCompilerData[_sVar];
				if (_oCompiler.aPlatformData.ContainsKey(_sType)){
					CompilerData _oPlatform = _oCompiler.aPlatformData[_sType];
					return _oPlatform;
				}else{
					//Output.TraceError("Platform not exist: " + _sVar + " (" + _sPlatform + ")");
					Debug.fTrace("Compiler Type not exist: " + _sVar + " (" + _sType + ")");
					
					return null;
				}
			}
			//Output.TraceError("Compiler not exist: " + _sVar + " (" + _sPlatform + ")");
			return null;
		}
    }
}
