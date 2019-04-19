using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwc {
    public class CppSeq {
       public string sSeq;

       ArgumentManager oParent = null;

        public  List<CppCmd> aCppCmd = new List<CppCmd>();

       public CppSeq(ArgumentManager _oParent, string _sSeq) {

            oParent = _oParent;
             sSeq  = _sSeq;

         
        }
    }
}
