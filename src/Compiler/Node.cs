using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwc.Compiler {

    public  class NdValue  {
         public  string sValue = "";
        public NdValue(string _sVal) {
            sValue = _sVal;
        }
    }


   public  class Node {

        public Node oParent;
        public NdValue oVal;
       public  string sName;

        public Node(Node _oParent, string _sName) {
             oParent =  _oParent;
            sName = _sName;
        }

         // string sVal = null;
          public Dictionary<string, NdValue> aVal = new Dictionary<string, NdValue>();
     


           public Dictionary<string, Node> aNode = new Dictionary<string, Node>();

            public Node fAddNode( Node _oNodeOther) {

               return  fAddNode(_oNodeOther.sName);
            }

          public Node fAddNode( string _sName) {

                if(!aNode.ContainsKey(_sName)){
                    Node _oNode =  new Node(this, _sName);
                    aNode.Add(_sName,_oNode);
                      // _oNode.aVal.Add("", new NdValue("") ); //Not work?
                    return _oNode;
                } else {
                   return aNode[_sName];
                }
            
         }

        internal void fSetValue(string _sValue, string _sType) {

           // if(_sValue.IndexOf("nasm") != -1) {
           //       Debug.fTrace("nasm");
            //}

            if (aVal.ContainsKey(_sType)) {
                NdValue _oVal =  aVal[_sType];

                _oVal.sValue += _sValue + " ";
                oVal = _oVal;
            }else{
               oVal =  new NdValue(_sValue + " ");
               aVal.Add(_sType, oVal);
              
            }

            if(_sType != "") {
                 if (!aVal.ContainsKey("")) {
                     oVal =  new NdValue("");
                     aVal.Add("", oVal );
                }
            }


        }
    }
}
