using FastColoredTextBoxNS;
using Raccoom.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using static cwc.DBGpClient;


namespace cwc {
    public class SettingsLaunch : FileUtils {

        List<Breakpoint> aBreakpoint = DBGpClient.aBreakpoint;
      //  List<TreeNodePath> aPrjNode = new  List<TreeNodePath>();
        TreeView oPrjNode; 
        XmlNode oXmlPrjNodeLoaded; 


        public SettingsLaunch(string _sFile) {

            fSetNewFile(_sFile);
        }


        public static bool create_directory(string _base, string _file) {
            try
            {
                string reldir = Path.GetDirectoryName(_file);
                string[] list = reldir.Split('/');
                string dir = _base;
                foreach (string d in list)
                {
                    dir += d;
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
                return true;
            }
            catch (Exception e) {
                Output.TraceError("Cannot create dir for file: " + _file);
                return false; }
        }


        public  string sCurrentFile = "";
        public  static string sFileLaunch = "";

        ////////// MIRROR ////////////
        public  static string sFileMirror = "";
        public  static string sMirror = "";
        public static List<string> aFolderToMirror = new List<string>();
        public static List<string> aFileToMirror = new List<string>();


        public static List<string> aOriTimeFileMirror = new List<string>();




     //  public static Dictionary<string,string> aOriTimeFileMirror = new Dictionary<string,string>();
      // public static Dictionary<string,Int32> aRefTimeFileMirror = new Dictionary<string,Int32>();
       public static Dictionary<string,Int32> aReadTimeFileMirror = new Dictionary<string,Int32>();


        public static bool add_to_dic(Dictionary<string,Int32>  _dic, string _key, Int32 _data) {
            if (!_dic.ContainsKey(_key)) {
                _dic.Add(_key, _data);
                return true;
            } else {
                _dic[_key]= _data;
                return false;
            }
        }



        internal static bool Mirror_SaveModeTime(string _file) {
            ////
            String _sDir = Path.GetDirectoryName(_file);
            String _sName = Path.GetFileNameWithoutExtension(_file);
            string f = _sDir + "/.wdat/" + _sName + ".md";
            ////
            string text = "";
            foreach(string line in aOriTimeFileMirror) {
                text+=line+"\n";
            }
            File.WriteAllText(f, text);
            return true;
        }

        internal static bool Read_FileTime(string _file) {
            aReadTimeFileMirror.Clear();
            ////
            String _sDir = Path.GetDirectoryName(_file);
            String _sName = Path.GetFileNameWithoutExtension(_file);
            string f = _sDir + "/.wdat/" + _sName + ".md";
            ////
            if (!File.Exists(f)) return false;

            string _text = File.ReadAllText(f);

            string[] lines = _text.Split('\n');
            foreach (string line in lines) {
                try {
                    string[] _data = line.Split('|');
                    Int32 modtime=  Int32.Parse( _data[0]);
                    add_to_dic(aReadTimeFileMirror,_data[1], modtime);

                }catch(Exception e){ };
            }
            return true;
        }

        internal static bool Mirror_BuildFileList() {
            Output.Trace("\f3B --- Begin Mirroring --- \f13");
            aFileToMirror.Clear();

            Read_FileTime(sFileMirror);

          //  aRefTimeFileMirror.Clear();
            aOriTimeFileMirror.Clear();
            if(sMirror=="")return false;

            Output.Trace("GetFiles");
            List<string> aFile = new List<string>();
            foreach (string folder in aFolderToMirror) {
                if (Directory.Exists(folder)) {
                    Output.Trace("GetFiles from: " + folder);

                    // Liste des extensions que vous souhaitez inclure
                    var allowedExtensions = new[] { ".c", ".cc", ".cpp", ".h", ".hh", ".hpp",  ".glsl",  ".inc", ".s", ".def", ".jc", ".m", ".mm", ".sh" };

                    // Récupérer tous les fichiers du dossier
                    var allFiles = FileUtils.GetAllFiles(folder, true, "*.*");

                    // Filtrer les fichiers par extension autorisée
                    var filteredFiles = allFiles.Where(file =>  allowedExtensions.Any(ext => Path.GetExtension(file).Equals(ext, StringComparison.OrdinalIgnoreCase)));

                    // Ajouter les fichiers filtrés à la liste
                    aFile.AddRange(filteredFiles);

                    //var test = FileUtils.GetAllFiles(folder, true, "*.c*");

                    // aFile.AddRange(  FileUtils.GetAllFiles(folder, true, "*.c*"));
                    // aFile.AddRange(  FileUtils.GetAllFiles(folder, true, "*.h*"));
                    // aFile.AddRange(  FileUtils.GetAllFiles(folder, true, "*.i*"));
                    // aFile.AddRange(  FileUtils.GetAllFiles(folder, true, "*.s*"));
                    // aFile.AddRange(  FileUtils.GetAllFiles(folder, true, "*.de*"));
                    // aFile.AddRange(  FileUtils.GetAllFiles(folder, true, "*.m*"));
                }

                Thread.Sleep(50);
            }
            Output.Trace("EndGetFiles");

            //Remove hidden files (.git)
            foreach (string file in aFile) {
                string f = file.Replace("\\", "/");
                if (f.IndexOf(".git") == -1) {
                    aFileToMirror.Add(f);
                }
            }

            //Copy if newer to dest
           foreach (string f in aFileToMirror) {

                DateTime dc= System.IO.File.GetLastWriteTime(f);
                Int32 dc_time = (int)dc.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
             
                Int32 lastCopyTime =0;
                if (aReadTimeFileMirror.ContainsKey(f)) {
                    lastCopyTime= aReadTimeFileMirror[f];
                 }
                //DateTime dm= System.IO.File.GetLastWriteTime(fm);//SLOW

                if(dc_time > lastCopyTime) {
                    //Copy
                    string fm = Path.GetFullPath( sMirror + f);
                    create_directory(sMirror, f);
                    System.IO.File.Copy(f, fm, true);
			        Output.Trace("\f3FMCopy: \f37 "  + fm );
                }
              
                //add_to_dic(aRefTimeFileMirror, f, dc_time);
                aOriTimeFileMirror.Add(dc_time+"|"+f);
           }
           Output.Trace("\f3B --- End Mirroring --- \f13");
            //save modification time

            Mirror_SaveModeTime(sFileMirror);

           return true;
        }
       internal static bool Mirror_New(string _file) {
            try {
                sFileMirror = _file;
                aFolderToMirror.Clear();
               if (File.Exists(_file)) {
                    string[] alines  = File.ReadAllText(_file).Split('\n');
                    sMirror = alines[0];
                     for (int i =1;i<alines.Length;i++) {
                        string l = alines[i];
                        if(l.Length==0) continue;
                        aFolderToMirror.Add(alines[i].Trim());
                    }
                    if (aFolderToMirror.Count>0) {
                        string folder= Path.GetFullPath( sMirror + aFolderToMirror[0]);//TODO other folder detection
                        if (!Directory.Exists(folder)) {//TODO other foler detection
                            Output.TraceError("Mirror not found: " + folder);
                            sMirror = "";
                        } else {
                            Output.TraceWarning("Mirror to: " + folder);
                        }
                    }
                   
                }
            }catch { }
            sMirror = sMirror.Trim();
            return true;
        }
        //////////////////////////////

        internal void fSetNewFile(string _sFile) {
            sFileLaunch = _sFile;

            ////////// MIRROR ////////////
            //Mirror_New(_sFile.Replace(".cwMake", ".mirror"));
            /////////////////////////////
        
           String _sDir = Path.GetDirectoryName(_sFile);
           String _sFileName = Path.GetFileNameWithoutExtension(_sFile); //Used?

            _sFileName = "_" + _sFileName + ".wdat";
           // _sFileName = "_.cwcfg";
            

            string _sDirectory = _sDir + "/.wdat/";
            if(!Directory.Exists(_sDirectory)){
                DirectoryInfo di = Directory.CreateDirectory(_sDirectory); 
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden; 
            }

          //  string _sFilePath =  _sDir + "/" + _sFileName;
            string _sFilePath = _sDirectory + _sFileName;

            if( sCurrentFile != _sFilePath ){
                  
                fSaveSetting();      
                sCurrentFile = _sDir  + "/" + _sFileName;
         
                if(fIniFile(_sFilePath, false,false)){
                  
                    fLoadSettings();
                }
            }
            bSettingLoaded = true;
        }



 
        public bool bSettingLoaded = false;
       public void fLoadSettings() {

          //  aPrjNode.Clear();

        //           MessageBox.Show("Load");
             DBGpClient.aBreakpoint.Clear();
            DBGpClient.nBreakPointCurrID = 0;

            XmlDocument _oXml = fReadXmlFile();
            if (_oXml == null || _oXml.DocumentElement == null) {
                return;
            }
            //oReadXml = _oXml;
            foreach (XmlNode _oMasterNode in _oXml.DocumentElement.ChildNodes){ 
                switch (_oMasterNode.Name) {
                    case "BreakpointList":
                        foreach (XmlNode _oNode in _oMasterNode.ChildNodes){ 
                            switch (_oNode.Name) {
                                case "breakpoint":
                                    fExtractBreakpoint(_oNode);
                                break;
                            }
                        }
                    break;

                    case "TreeViewPrj":
                        oXmlPrjNodeLoaded = _oMasterNode;
                    break;

                }
            }
        
          
        }

        private void fExtractBreakpoint(XmlNode _oNode) {
            Breakpoint _oBrk = new Breakpoint();
            aBreakpoint.Add(_oBrk);
             DBGpClient.nBreakPointCurrID++;
            _oBrk.nID =  DBGpClient.nBreakPointCurrID;

            foreach( XmlAttribute _oAtt in _oNode.Attributes) {
                switch ( _oAtt.Name) {
                  //  case "id": //nope?
                  //  break;
                    case "type": _oBrk.sType = _oAtt.Value;
                    break;
                    case "state":  
                        if (_oAtt.Value == "disabled") {
                            _oBrk.bEnable = false;
                        } else {
                            _oBrk.bEnable = true;
                        }
                    break;
                    case "resolved": 
                          if (_oAtt.Value == "unresolved") {
                            _oBrk.bResolved = false;
                        } else {
                            _oBrk.bResolved = true;
                        }
                    break;
                    case "filename": _oBrk.sPath =  _oAtt.Value;
                    break;
                    case "lineno":  Int32.TryParse( _oAtt.Value, out _oBrk.nLine);
                    break;
                    case "function":  _oBrk.sFunction =  _oAtt.Value;
                    break;
                    case "exception":  _oBrk.sExeption =  _oAtt.Value;
                    break;
                    case "expression":  _oBrk.sExpression =  _oAtt.Value;
                    break;
                    case "hit_value":  Int32.TryParse( _oAtt.Value, out _oBrk.nHitValue);
                    break;
                    case "hit_condition": _oBrk.sHitCondition =  _oAtt.Value;
                    break;   
                    case "hit_count":  Int32.TryParse( _oAtt.Value, out _oBrk.nHitCount);
                    break;   
                }
                 //  <expression>EXPRESSION</expression>  </breakpoint>
            }
        }


        public XmlAttribute fCreateAtt( XmlElement _oNode, string _sName, string _sValue) {
              XmlAttribute _oAtt = oSaveXml.CreateAttribute(_sName);
              _oAtt.Value = _sValue;
            _oNode.Attributes.Append(_oAtt);
            return _oAtt;
        }


          XmlDocument oSaveXml = null;
        public void fSaveSetting(bool _bFull = false) {
            if(!bSettingLoaded){return;} //Loaded at less 1 time

            oSaveXml = new XmlDocument();
            XmlNode _oRootNode = oSaveXml.CreateNode(XmlNodeType.Element,"Config", null);
            oSaveXml.AppendChild(_oRootNode);
         


            XmlNode _oXmlNode = oSaveXml.CreateNode(XmlNodeType.Element,"BreakpointList", null);
            _oRootNode.AppendChild(_oXmlNode);
         

            foreach(Breakpoint _oBkp in aBreakpoint){
                XmlElement breakpoint = oSaveXml.CreateElement("breakpoint");
                _oXmlNode.AppendChild(breakpoint);

               string _sState = "enabled";
                if (!_oBkp.bEnable) {
                    _sState = "disabled";
                }

                string _sResolved = "resolved";
                if (!_oBkp.bResolved) {
                    _sResolved = "unresolved";
                }

                fCreateAtt(breakpoint, "type", _oBkp.sType);
                fCreateAtt(breakpoint, "state", _sState);
                fCreateAtt(breakpoint, "resolved", _sResolved);
                fCreateAtt(breakpoint, "filename", _oBkp.sPath);
                fCreateAtt(breakpoint, "lineno",_oBkp.nLine.ToString());
                fCreateAtt(breakpoint, "function", _oBkp.sFunction);
                fCreateAtt(breakpoint, "exception", _oBkp.sExeption);
                fCreateAtt(breakpoint, "expression", _oBkp.sExpression);
                fCreateAtt(breakpoint, "hit_value", _oBkp.nHitValue.ToString());
                fCreateAtt(breakpoint, "hit_condition", _oBkp.sHitCondition);
                fCreateAtt(breakpoint, "hit_count",_oBkp.nHitCount.ToString());

            }

            _oXmlNode = oSaveXml.CreateNode(XmlNodeType.Element,"TreeViewPrj", null);
            _oRootNode.AppendChild(_oXmlNode);
            if(_bFull && bNodeLoaded){
                fWriteSubNodes(_oXmlNode, oPrjNode.Nodes);
            } else {
                if(oXmlPrjNodeLoaded != null){
                    XmlNode importNode = _oXmlNode.OwnerDocument.ImportNode(oXmlPrjNodeLoaded, true);
                    _oXmlNode.AppendChild(importNode);
                }
            }

            fSaveXmlFile(oSaveXml);
        }


       internal void fWriteSubNodes(XmlNode _oXmlNode, TreeNodeCollection _aSubNode) {
            foreach (TreeNodePath _oNode in _aSubNode) {
                if (_oNode.IsExpanded) {

                    XmlElement _oElm = oSaveXml.CreateElement("Node");
                    _oXmlNode.AppendChild(_oElm);
                    fCreateAtt(_oElm, "Name", _oNode.Text);

                 //  aPrjNode.Add(_oNode);
                    fWriteSubNodes(_oElm, _oNode.Nodes);
                }
            }
        }


        /*

        internal void fSetNodes(TreeView _oTree) {
            oPrjNode = _oTree;
            
            aPrjNode.Clear();
            fSetSubNodes("Root", _oTree.Nodes);
        }*/
        /*
        internal void fSetSubNodes(string _sParent, TreeNodeCollection _aSubNode) {
            foreach (TreeNodePath _oNode in _aSubNode) {
                if (_oNode.IsExpanded) {
                    aPrjNode.Add(_oNode);
                    fSetSubNodes(_oNode.Text, _oNode.Nodes);
                }
            }
        }
        */

       internal void fIni(TreeView _oPrjNode) {
            oPrjNode = _oPrjNode;
        }

        public bool bNodeLoaded = false;
       public void fExtractTreeViewPrjData() {

         if(oPrjNode != null){
                if(oXmlPrjNodeLoaded != null){
                    fExtractTreeViewPrjDataNodes( oPrjNode.Nodes, oXmlPrjNodeLoaded.ChildNodes);
                    bNodeLoaded = true;
                }
            }
        }
        public void fExtractTreeViewPrjDataNodes(TreeNodeCollection _aSubNode, XmlNodeList _oNodes) {
                foreach (XmlNode _oNode in _oNodes){ 
                    switch (_oNode.Name) {
                        case "Node":
                            fExtractTreeViewPrjNode(_aSubNode, _oNode);
                        break;
                    }
                }
        }
        private void fExtractTreeViewPrjNode(TreeNodeCollection _aSubNode, XmlNode _oNode) {
            foreach( XmlAttribute _oAtt in _oNode.Attributes) {
                switch ( _oAtt.Name) {
                    case "Name":
                        foreach (TreeNodePath _oTreeNode in _aSubNode) {
                            if (_oTreeNode.Text == _oAtt.Value) {
                                _oTreeNode.Expand();
                                fExtractTreeViewPrjDataNodes(_oTreeNode.Nodes, _oNode.ChildNodes);
                            }
                        }
                    break;
                }
            }
        }

      
    }
}
