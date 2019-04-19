using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace cwc {
    public partial class CompilerConfig : Form {
        /*
        public static string sCompiler;

        public static string s64_Path = "";
        public static string s64_Cpp = "";
        public static string s64_C = "";
        public static string s64_LinkerS = "";
        public static string s64_LinkerD = "";
        public static string s64_RC = "";
        public static string s64_Website = "";

        public static string s32_Path = "";
        public static string s32_Cpp = "";
        public static string s32_C = "";
        public static string s32_LinkerS = "";
        public static string s32_LinkerD = "";
        public static string s32_RC = "";
        public static string s32_Website = "";
        */

        public static CompilerData oData;

        public CompilerConfig(string _sCompiler ) {
			/*
            if (Data.aCompilerData.ContainsKey(_sCompiler) == false) {
               Data.aCompilerData.Add(_sCompiler, new CompilerData(_sCompiler));
            }
            oData = Data.aCompilerData[_sCompiler];
            oData.fLoadConfig(); //Reload to be sure


            InitializeComponent();
    
        
            fLoadLabel();
            fEnable32();
      */
        }
        
        public void fLoadLabel() {
/*
            label3.Text =  oData.sCompiler;
            tb64_Path.Text =  oData.sPath;
            tb64_Cpp.Text =  oData.sCpp;
            tb64_C.Text =  oData.sC;
            tb64_LinkerS.Text =  oData.sLinkerS;
            tb64_LinkerD.Text =  oData.sLinkerD;
            tb64_RC.Text =  oData.sRC;
            tb64_Website.Text =  oData.sWebsite;
*/
        }
    

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e) {

        }

        private void CompilerConfig_Load(object sender, EventArgs e) {
        
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void chkUse_CheckedChanged(object sender, EventArgs e) {

        }


        public static string fCorrectPath(string _sPath) {
            _sPath = _sPath.Replace('\\', '/');
            if(_sPath[_sPath.Length-1] != '/' ) {
                _sPath = _sPath + "/";
            }
            return _sPath;
        }


        private void btn64_Path_Click(object sender, EventArgs e) {
            ///   fbd.SelectedPath = System.Environment.CurrentDirectory;
            tb64_Path.Text  = fOpenFolderBrowsing(tb64_Path.Text);
            
        }


        public static string fOpenFolderBrowsing(string _sCurrentVal) {
             FolderBrowserDialog fbd = new FolderBrowserDialog();

            if(FileUtils.IsEmpty(_sCurrentVal)) {
                 fbd.SelectedPath =  "C:\\";
            }else {
                _sCurrentVal = Path.GetDirectoryName( _sCurrentVal.Replace('/', '\\'));
				if(_sCurrentVal[_sCurrentVal.Length -1 ] != '\\') {
					_sCurrentVal += "\\";
				}



                fbd.SelectedPath =  _sCurrentVal;

            }

            fbd.Description= "Select the compiler binaries folder";
           fbd.ShowNewFolderButton = false;

             SendKeys.Send ("{TAB}{TAB}{RIGHT}");  // <<-- Workaround
            if(fbd.ShowDialog() == DialogResult.OK) {
                 return  fCorrectPath(fbd.SelectedPath);
            }
            return _sCurrentVal.Replace('\\', '/');
        }





        private void btn64_Cpp_Click(object sender, EventArgs e) {
           // FolderBrowserDialog fbd = new FolderBrowserDialog();
               tb64_Cpp.Text =  fDialogExeFile(tb64_Path.Text, tb64_Cpp.Text);
        }

        public static string fDialogExeFile( string _sRoot, string _sCurrentVal, string _sFilter = "") {

            OpenFileDialog fbd = new OpenFileDialog();
            _sRoot = _sRoot.Replace('\\', '/');
            if(_sRoot[_sRoot.Length - 1] != '/' ) {
                _sRoot += '/';
            }

            fbd.InitialDirectory  =  Path.GetDirectoryName(_sRoot + _sCurrentVal);
			if(_sFilter == ""){
				 fbd.Filter = "Executable (*.exe)|*.exe|All files (*.*)|*.*";
			}else{
				 fbd.Filter = _sFilter;
			}
            if(fbd.ShowDialog() == DialogResult.OK) {
               //return Path.GetFileName( fbd.FileName);
              // Debug.fTrace(_sRoot);
              // Debug.fTrace( fbd.FileName);
               return FileUtils.fMakeRelativePath(_sRoot, fbd.FileName);
            }
            return _sCurrentVal;
        }

        private void btn64_C_Click(object sender, EventArgs e) {
             tb64_C.Text =  fDialogExeFile(tb64_Path.Text, tb64_C.Text );
        }

        private void btn64_LinkS_Click(object sender, EventArgs e) {
             tb64_LinkerS.Text =  fDialogExeFile(tb64_Path.Text,  tb64_LinkerS.Text );
        }

        private void btn64_LinkD_Click(object sender, EventArgs e) {
            tb64_LinkerD.Text =  fDialogExeFile(tb64_Path.Text,  tb64_LinkerD.Text );
        }

        private void btn64_RC_Click(object sender, EventArgs e) {
           tb64_RC.Text =  fDialogExeFile(tb64_Path.Text, tb64_RC.Text);
        }


        private void btnOk_Click(object sender, EventArgs e) {

            fSavecConfig();
            Close();
        }


        public void fSavecConfig() {
/*
            oData.sCompiler =  label3.Text ;
            oData.sPath  = tb64_Path.Text   ;
            oData.sCpp = tb64_Cpp.Text ;
            oData.sC = tb64_C.Text ;
            oData.sLinkerS = tb64_LinkerS.Text ;
            oData.sLinkerD = tb64_LinkerD.Text  ;
            oData.sRC = tb64_RC.Text  ;
            oData.sWebsite = tb64_Website.Text   ;

        

            oData.fSavecConfig();*/
        }




        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }


        private void btnCancel_Click_1(object sender, EventArgs e) {
            Close();
        }

        private void chkUse_CheckedChanged_1(object sender, EventArgs e) {
             fEnable32();
        }

        public void fEnable32() {
            /*
            if( chkUse.Checked) {
                tb32_Path.Enabled = false;
                tb32_Cpp.Enabled = false;
                tb32_C.Enabled = false;
                tb32_LinkerS.Enabled = false;
                tb32_LinkerD.Enabled = false;
                tb32_RC.Enabled = false;
                tb32_Website.Enabled = false;

                 btn32_Path.Enabled = false;
                 btn32_Cpp.Enabled = false;
                 btn32_C.Enabled = false;
                 btn32_LinkS.Enabled = false;
                 btn32_LinkD.Enabled = false;
                 btn32_RC.Enabled = false;


            }else{
                tb32_Path.Enabled = true;
                tb32_Cpp.Enabled = true;
                tb32_C.Enabled = true;
                tb32_LinkerS.Enabled = true;
                tb32_LinkerD.Enabled = true;
                tb32_RC.Enabled = true;
                tb32_Website.Enabled = true;

                 btn32_Path.Enabled = true;
                 btn32_Cpp.Enabled = true;
                 btn32_C.Enabled = true;
                 btn32_LinkS.Enabled = true;
                 btn32_LinkD.Enabled = true;
                 btn32_RC.Enabled = true;
            }*/
        }

        private void tb32_Path_TextChanged(object sender, EventArgs e) {

        }
    }

}
