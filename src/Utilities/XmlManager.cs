using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace cwc {
    class XmlManager {

        static public void loadNppConfig() {
           // return;
            /*
            XmlDocument doc = new XmlDocument();
         //   doc.Load(PathHelper.GetExeDirectory() +  "npp/config.xml");
    
           //  doc.Load(_sPath);

           string _sXml = File.ReadAllText(_sPath);
            doc.LoadXml(_sXml.Substring(_sXml.IndexOf(Environment.NewLine)));


          XmlNode node = doc.DocumentElement.SelectSingleNode("NotepadPlus");
          string text = node.InnerText;
           Debug.fTrace("--------MY NODE : " + text);
            */

            /*
            foreach(XmlNode node in doc.DocumentElement.ChildNodes){
               string text = node.InnerText; //or loop through its children as well
                        Debug.fTrace("--------MY NODE : " + text);
            }
            */

         string _sPath = PathHelper.ToolDir +  "npp/config.xml";
         string _sXml = File.ReadAllText(_sPath);
            //Remove FileBrowser


           _sXml = fRemoveAllNode(_sXml, "FileBrowser");
   

                   //Remove 
            int   _nNodeIndex = _sXml.IndexOf("</NotepadPlus>");
            if(_nNodeIndex != -1) {
        
                 _sXml = _sXml.Substring(0, _nNodeIndex);
                _sXml += "<FileBrowser>\n";

                    int i = 0;
                    foreach(string _sInclude in  Data.aAllInclude) {
                        i++;
                        bool _bGoodPath = true;
                        
		                if(_sInclude.IndexOf("Toolchain") != -1) { //Don't inlcude toolchain paths
                            _bGoodPath =false;
                        }
                        
                        int j = 0;
                        foreach(string _sOther in  Data.aAllInclude) {
                            j++;
                            if(i != j) {//Not the same
                                if(_sOther.Length <_sInclude.Length && _sInclude.IndexOf(_sOther) != -1) { //Don't inlcude recursive paths
                                    _bGoodPath =false;
                                }
                            }
                        }


                        if(_bGoodPath) {
                              _sXml += " <root foldername=\"" +  _sInclude    + "\" />\n";
                         }
                    }
                

                _sXml += "</FileBrowser>\n";
			    
            }
			_sXml += "</NotepadPlus>";
  
            File.WriteAllText(_sPath, _sXml);

         }

        static string fRemoveAllNode(string _sXml, string _sNode) {
            string _sXmlBef  = _sXml;
            string _sXmlAft  = "";

            int _nNodeIndex = _sXml.IndexOf("<" + _sNode);

              //Remove node
            while(_nNodeIndex != -1) {

               _sXmlBef =  _sXml.Substring(0, _nNodeIndex);

               if(_sXml[_nNodeIndex + _sNode.Length + 2 ] == '/') {
                      _sXmlAft =  _sXml.Substring( _nNodeIndex + _sNode.Length + 4);
                }else { 

                    _nNodeIndex = _sXml.IndexOf("</" + _sNode, _nNodeIndex);
                    if(_nNodeIndex != -1) {
                        _nNodeIndex += _sNode.Length + 3;//3 = sizeof("</>")

                        _sXmlAft = _sXml.Substring(_nNodeIndex, _sXml.Length - _nNodeIndex); 
                    }
                }

                _sXml = _sXmlBef +_sXmlAft;
                 _nNodeIndex = _sXml.IndexOf("<" + _sNode);
            }

        

            return _sXml;
        }


    }
}
