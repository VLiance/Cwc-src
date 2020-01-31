using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwc {
    class Finder {


       public static CompilerData fUseCompiler(string _sVar, string sType  = ""){
			//Debug.fTrace("fUseCompiler : " +  _sVar + " "  + sType );
		
			
			CompilerData _oCompiler =   fGetCompiler(_sVar, sType);
			
			if(_oCompiler != null){
		    	_oCompiler.fUsed();
            }
			return _oCompiler;

		}

		public static CompilerData  fGetCompiler(string _sVar, string _sType = "", bool _bIsSubCompiler = false) {

			if(_sType == ""){
				_sType = "Default";
			}
           
         //  Output.TraceWarning("fGetCompiler _sVar " +_sVar);

            //Extract Square subtype ex:LibRT[Clang] if not already done
            if(_sVar.Length > 3 && _sVar[_sVar.Length-1] == ']') {
                int _nIndex = _sVar.IndexOf('[');
                if(_nIndex>0) {
                    _sType = _sVar.Substring(_nIndex+1, _sVar.Length-1 - (_nIndex+1));
                    _sVar =  _sVar.Substring(0, _nIndex);
                }
            }
            if(!_bIsSubCompiler) {
		    	Data.fSetGlobalVar("_sConfig_Type", _sType  );
            }
            
			if (Data.aCompilerData.ContainsKey(_sVar)){
			//	CompilerData _oCompiler =  aCompilerData[_sVar];
				ModuleData _oCompiler = Data.aCompilerData[_sVar];
           //     Output.TraceWarning("_oCompiler " + _oCompiler.sAutorName + " : "  + _sType + " : " +_oCompiler.aPlatformData.Count );
				if (_oCompiler.aPlatformData.ContainsKey(_sType)){
					CompilerData _oPlatform = _oCompiler.aPlatformData[_sType];
					return _oPlatform;
				}else{
					//Output.TraceError("Platform not exist: " + _sVar + " (" + _sPlatform + ")");
					Debug.fTrace("Compiler Type not exist: " + _sVar + " (" + _sType + ")");
					
					return null;
				}
			}
         //    Output.TraceWarning("Compiler not found " +_sVar);
			//Output.TraceError("Compiler not exist: " + _sVar + " (" + _sPlatform + ")");
			return null;
		}
    }
}
