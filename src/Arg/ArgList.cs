using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwc {

    class ArgStruct {
        public delegate void dfunc(string str);
        public dfunc func;
        public string arg_short = "";
        public string arg_long = "";
        public string description = "";
         
        public ArgStruct(string _short, string _long, dfunc _func, string _description) {
            arg_short = _short;
            arg_long = _long;
            arg_long = _long;
            func = _func;
            description = _description;
        }

    }

    class ArgList {
        public static ArgStruct[] aList = new ArgStruct[] {
            new ArgStruct("-v", "--version",    getVersion  , "Get current version"),
            new ArgStruct("-h", "--help",       getHelp     , "Get command list"),



        };


        public static string alignTo(string _in, int _to) {
            for(int i = _in.Length; i < _to; i++) {
                _in+=" ";
            }
            return _in;
        }
        
       public static void ProcessArg(string _arg) {
            Output.TraceAction( _arg );

            foreach(ArgStruct _o in ArgList.aList) {
                if( _o.arg_short == _arg || _o.arg_long == _arg) {
                    _o.func(_arg);
                    return;
                }
            }
             Output.TraceWarningLite( "Unknow command" );
        }


       public static void getVersion(string _param) {
          //   Output.TraceAction("getVersion!");

        }

       public static void getHelp(string _param) {

            foreach(ArgStruct _o in ArgList.aList) {
                string _line = "    ";
                _line =   alignTo(_line  + _o.arg_short,        8);
                _line =   alignTo(_line  + _o.arg_long ,       25); 
                _line += "(" + _o.description + ")";

                Output.TraceReturn(_line);


            }
        }
    }
}
