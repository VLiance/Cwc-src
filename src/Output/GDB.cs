using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwc {


    class GDB : DBGpClient {

      


        public List<Frame> aCurrBacktrace = new List<Frame>();
      

        int nLastID = 0;
        LauchProject oLauchProject;
        LauchTool oProcess;
        Boolean bCmdSend = false;
        string sCurrentCmd = "";
        string sCurrentExp = "";
        string sCmdSended = "";
        string sCmdNameSended = "";
        bool bRunning = false;

        public GDB(LauchProject _oLauchProject, LauchTool _oProcess, string _sGdbPath,string _sExePath,  CompilerData _oCompiler ){
            
            oLauchProject = _oLauchProject;
            oProcess = _oProcess;
             
            oProcess.dOut = new LauchTool.dIOut(fAppOut);
            oProcess.dError = new LauchTool.dIError(fAppError);

            oProcess.bHidden = true;
            oProcess.bRedirectOutput = true; //DBG assole in !bRedirectOutput

            oProcess.sWorkPath = Path.GetDirectoryName(_sExePath);
            //Output.TraceAction("Dir: " +  oProcess.sWorkPath );


            //oProcess.fLauchExe(_sGdbPath, "--args " + _oCompiler.oModuleData.sCurrFolder+ _oCompiler.sExe_Sanitizer + " " + " -exit_code_if_errors 1 -malloc_callstacks -no_soft_kills   -no_soft_kills  -pause_at_exit  -batch -crash_at_unaddressable -crash_at_error " + _sExePath   );
      
            bool _bSanitize = true;
            //bool _bSanitize = false;
            
            if(Data.fIsDataTrue("Options/Debug Type/Sanitizer") ){
                //if(_bSanitize) {
                //oProcess.bExterneLauch = false;
                //  oProcess.bHidden = false;
                //     oProcess.bRedirectOutput = false;
                // oProcess.fLauchExe(_sGdbPath, "--args " + _oCompiler.oModuleData.sCurrFolder+ _oCompiler.sExe_Sanitizer + " " + " -v -exit_code_if_errors 1 -malloc_callstacks -no_soft_kills -batch -pause_at_exit " + _sExePath   );
                // oProcess.fLauchExe( _oCompiler.oModuleData.sCurrFolder+ _oCompiler.sExe_Sanitizer,  " -v -exit_code_if_errors 1 -malloc_callstacks -no_soft_kills -batch -pause_at_exit " + _sExePath   );

                
            string _sSanitizer =  _oCompiler.oGblConfigType.fGetNode(null,new string[]{"Exe", "Sanitizer"}, "");

               // oProcess.fLauchExe( _oCompiler.oModuleData.sCurrFolder+ _oCompiler.sExe_Sanitizer,  " -no_callstack_use_fp   -no_callstack_use_top_fp  -v -exit_code_if_errors 1 -malloc_callstacks  -batch " + _sExePath   ); //-no_soft_kills
                oProcess.fLauchExe( _sSanitizer,  " -no_callstack_use_fp   -no_callstack_use_top_fp  -v -exit_code_if_errors 1 -malloc_callstacks  -batch " + _sExePath   ); //-no_soft_kills
                
                return;

            } else { //   if(Data.fIsDataTrue("Options/Debug Type/Debugger") ){
                oProcess.fLauchExe(_sGdbPath,  "\"" +  _sExePath + "\"" ); //GDB only
            }

            fLoadBreakpoints();


             oProcess.fSend("cd " +  Path.GetDirectoryName(_sExePath) );

            oProcess.fSend("set width 0");
            oProcess.fSend("set filename-display absolute");
            oProcess.fSend("set breakpoint pending on");
         
           // oProcess.fSend("set output-radix 16");//All in hex?


            fSetAllGdbBreakpoint();
            

           //oProcess.fSend("set new-console on");

  
           
           // oProcess.fSend("tbreak main"); 

            //oProcess.fSend("break _start");  //Entry point: 0x4014a0
            //oProcess.fSend("set backtrace past-entry on");
                
                 /*
            oProcess.fSend("info files");  //Entry point: 0x4014a0
            oProcess.fSend("break *0x4014a0"); 
            oProcess.fSend("show backtrace past-entry");
            */
         
            oProcess.fSend("run");  bRunning = true;
          //    oProcess.bRedirectOutput = false;
         //   oProcess.processStartInfo.RedirectStandardInput = false;Ok

            Start();
        }





        public void fSetAllGdbBreakpoint() {
            foreach (Breakpoint _oBkp in aBreakpoint) {
                fBreakpointSet(_oBkp);
            }
        }

         public override void fBreakpointSet(Breakpoint _oBreakpoint) {
             oProcess.fSend("break " + _oBreakpoint.sPath + ":" + _oBreakpoint.nLine);
         }
         public override void fBreakpointRemove(Breakpoint _oBreakpoint) {
             oProcess.fSend("clear " + _oBreakpoint.sPath + ":" + _oBreakpoint.nLine);
         }

      //   public override void fRun(){oProcess.fSend("continue");}

         public override void fRun(){
                if(!bRunning){
                    bRunning = true;
                   fSendCmd("Execute", "continue", false);
                }
            
            }
         public override void fStop(){oProcess.fSend("break");}
         public override void fStepInto(){
           // if(){
              oProcess.fSend("step");
           // }
         }
         public override void fStepOver(){oProcess.fSend("next");}

         public override void fStepOut(){
           Console.WriteLine("StepOUT!"); 
            oProcess.fSend("finish");}

        public override void fIni() {
           // base.fIni();
       //    Console.WriteLine("fIni");
        }


     
        

        public  void 	fAppError(LauchTool _oTool, string _sOut){
            bRunning= false;
            oLauchProject.bReceiveOutput = true;
           // bCmdSend = false;//Proble occur, we can resend cmd
             if (bCmdSend) {
               if( fTestEndOfCommand(_sOut)){ //Or just check resutl?
                    return;
                }else{
                _sOut = sCurrentCmd;
                }
            }
		    Output.Trace("E> " + _sOut);
         }

        public  void 	fAppOut(LauchTool _oTool, string _sOut){
             bRunning= false;
            if(_sOut == null || _sOut == "") {
                return;
            }
           
            oLauchProject.bReceiveOutput = true;
	        string _sColor ="";

            
             string _sLetter ="O";

            if (bCmdSend) {

               if( fTestEndOfCommand(_sOut)){
                  // fTestFrame(_sOut);
                      return;
                }else{
                 _sLetter = "C";
                 _sOut = sCurrentCmd;
                }
            }

        
            if ( _sOut.StartsWith("Breakpoint") ) {
                
                _sColor = Output.sWarningColor;
                  Output.Trace(_sLetter + "> " +_sColor +_sOut);
                 fShowBacktrace();
                return;
            }

            //Thread 1 received signal SIGSEGV, Segmentation fault.
            //Program received signal SIGSEGV, Segmentation fault
            if (   _sOut.IndexOf("received signal",0) != -1         ) {
              
                _sColor = Output.sErrorColor;
                  Output.Trace(_sLetter + "> " +_sColor +_sOut);
                  fShowBacktrace();
                  return;
            }
            
           
            LauchProject.fPrjOut(_sLetter,  _sColor +_sOut);
           // Output.Trace(_sLetter + "> " +_sColor +_sOut);
         }

         public void fShowBacktrace(){
             // fSendCmd("GlobalVar", "info variables"); //Too heavy?
              fSendCmd("backtrace", "bt full");

         }


         public void fSendCmd(string _sName, string _sCmd, bool _bWaitResult = true){
            if(oProcess.bExeLauch){
		    Thread sendCmd = new Thread(new ThreadStart(() =>  {  
	
                 while(bCmdSend == true && Base.bAlive) {
                    Thread.Sleep(1);
                }
                if(oProcess.bExeLauch){
                       Debug.fTrace("S> " + _sName + " : " +_sCmd);
                    //sLastGetExp = "";
                    sCmdSended = _sCmd;
                    sCmdNameSended = _sName;
                    if(_bWaitResult){
                        bCmdSend = true;
                        oProcess.fSend(_sCmd); //Get Call Stack
                        oProcess.fSend("show verbose");//End sequences 
                                                       //  oProcess.fSend("show width");//End sequences 
                    } else {
                      oProcess.fSend(_sCmd); 
                    }
                    sCurrentCmd = "";
                }
              
            

		    }));  
		    sendCmd.Start();
            }

        }

        public bool fTestEndOfCommand( string _sOut){
            if(_sOut == null) {return true; }
            //(gdb) Verbose printing of informational messages is on.
            //(gdb) Verbosity is off.
            //(gdb) Verbos
           // _sOut.StartsWith("(gdb)");???
           //  if(  _sOut.StartsWith("(gdb) Verbos")) {
             if(  _sOut.IndexOf("(gdb) Verbos") != -1) {

                Debug.fTrace("End> " +sCurrentCmd);
                 bCmdSend = false;
                if(fProcessCmd(sCurrentCmd)){
                     sCurrentExp = "";
                     return true;
                }
                sCurrentExp = "";
                return false; //show
            } else {
                if (_sOut.StartsWith("(gdb)")) {
                    _sOut = _sOut.Substring(5);
                }
                _sOut = _sOut.Trim();
                sCurrentCmd += _sOut+ "\n";

                
                if (sCurrentCmd.StartsWith("The program is not being run")) {
                    oProcess.fSend("run");
                    return true;
                }
                /*
                if (fDontShowRecognizedCmd(_sOut)) {
                    return true;
                }*/

               // return false; //Don't show result //TODO Show unreconized result?
                return true; //Don't show result //TODO Show unreconized result?
            }
        //    return false;
        }
        /*
        public  bool fDontShowRecognizedCmd( string _sCmd){
            if (_sCmd.StartsWith("#")) {
                return true;
            }
              return false;
        }*/



        public  bool fProcessCmd( string _sCmd){

            switch (sCmdNameSended) {
                case "backtrace":
                    fCmdBackTrace(_sCmd);
                     fPrintBackTrace();
                return true;
                case "fPropertyGetPrint":
                    fReceive_fPropertyGetPrint(_sCmd);
                return true;
                case "fPropertyGetWhat":
                    fReceive_fPropertyGetWhat(_sCmd);
                 
                 return true;
                 case "Execute":
                      //Debug.fTrace("End> " +sCurrentCmd);
                 return false;


            }

            Debug.fTrace("Unreconized> " + sCmdNameSended + " : " +_sCmd);
            sCmdNameSended = "";
            return false;
        }

    

        public  void 	fCmdBackTrace( string _sCmd){
             aCurrBacktrace = new List<Frame>();

            Frame _oCurrentFrame = null;
            string[] _aLines = _sCmd.Split('\n');
            foreach (string _sLine in _aLines) {

                Frame _oFrame = fTestFrame(_sLine);
                if (_oFrame != null) {_oCurrentFrame = _oFrame; }

                if(_oFrame == null  && _oCurrentFrame != null){
                    fAddLocalVar(_oCurrentFrame, _sLine);
                }
            }
        }


    
         public  Frame 	fTestFrame(string _sCmd){
            int _nIndex = 0;
       //     while (_nIndex < _sCmd.Length) {
             //   if (_sCmd[_nIndex] == '#' && _nIndex < 8 ) { //8 Just to optimize
               if (_sCmd.Length > 0 &&  _sCmd[0] == '#' ) { 
                     _nIndex++;
                    string _sNum = "";
                    while (_nIndex < _sCmd.Length && _sCmd[_nIndex] >= '0' && _sCmd[_nIndex] <= '9' ) {
                        _sNum += _sCmd[_nIndex];
                        _nIndex++;
                    }
                    if (_sNum.Length > 0) {
                        int _nId = Int32.Parse(_sNum);
                       return  fNewFrame(_nId, _sCmd.Substring(_nIndex));
                       // return;
                    }
            } 

            return null;
               // _nIndex++;
          //  }
          //   Output.Trace("O> " +_sOut);
        }


       
      public  Frame fNewFrame(int _nId, string _sOut){
            /*
            if (_nId < nLastID ) { //We are in a new backtrace
                aCurrBacktrace = new List<Frame>();
            }
            nLastID = _nId;
            */
          Frame _oFrame = new Frame();
            _oFrame.nStackLevel = _nId;

            int _nIN_index = _sOut.IndexOf(" in ");
            if (_nIN_index != -1) {
                _oFrame.sAdress = _sOut.Substring(0,_nIN_index).Trim();
                _sOut = _sOut.Substring(_nIN_index + 4);
            }

       
             int _nEndFunc_index = _sOut.IndexOf("(");
            if(_nEndFunc_index == -1){_nEndFunc_index = _sOut.Length;}
            _oFrame.sFuncName = _sOut.Substring(0,_nEndFunc_index).Trim();


           _oFrame.sFuncParam = _sOut.Substring(_nEndFunc_index + 1);
            int _nEndParam_index =   _oFrame.sFuncParam.IndexOf(")");
            if ( _nEndParam_index != -1) {
                _oFrame.sFuncParam = _oFrame.sFuncParam.Substring(0,_nEndParam_index);
            }
            _oFrame.sFuncParam = _oFrame.sFuncParam.Trim();

            int _nAT_Index = _sOut.IndexOf(" at ");
            if ( _nAT_Index != -1) {
                _oFrame.sFile = _sOut.Substring(_nAT_Index + 4);
                //Get Line
                int nLINE_index =  _oFrame.sFile.LastIndexOf(":");
                if (nLINE_index != -1 && ( _oFrame.sFile.Length - nLINE_index) < 7  ) { //7 -> 9999999 max linenum 
                    string _sLineVal = _oFrame.sFile.Substring(nLINE_index + 1).Trim();
                    Int32.TryParse(_sLineVal, out  _oFrame.nLine);
                    _oFrame.sFile = _oFrame.sFile.Substring(0,nLINE_index).Trim();
                }


            } else {
                _oFrame.sFile = "??";
            }



           //Console.WriteLine("    _oFrame.sFuncName" +     _oFrame.sFuncName  + "(" + _oFrame.sAdress + ")(" + _oFrame.sFuncParam  );
            if (_oFrame.sFuncName == "??") {
                _oFrame.sFuncName =  "(" + _oFrame.sAdress + ")";
                _oFrame.bUnknowName = true;
            }

            aCurrBacktrace.Add(_oFrame);
            return  _oFrame;
          //  _oFrame._sFile
        //   Output.Trace("O> " +_sOut);
      }

        public  Boolean fAddLocalVar(Frame _oFrame, string _sCmd){
            if (_sCmd.StartsWith("No")) { //No symbol table info available.  //No locals.
                return false;
            }
            Var _oVar = new Var();

            int _nEgalIndex = _sCmd.IndexOf('=');

            if (_nEgalIndex != -1) {
                _oVar.sName = _sCmd.Substring(0, _nEgalIndex).Trim();          
               // _oVar._sValue =  _sCmd.Substring(_nEgalIndex+1, _sCmd.Length - (_nEgalIndex+1));          
                _oVar.sValue =  _sCmd.Substring(_nEgalIndex+1).Trim();          
            }
            if ( _oVar.sName  == "") {
                return false;
            }
            _oFrame.aVar.Add(_oVar);

            return true;
        }


        private void fPrintBackTrace() {
           
            foreach(Frame _oFrame in aCurrBacktrace) {
               // Console.WriteLine("------ " +     _oFrame.sFuncName  + "(" + _oFrame.sFuncParam  + ")" );
                Output.Trace("\f0C>>----  \f1C" +     _oFrame.sFuncName  + "\f13(" + _oFrame.sFuncParam  + ")\fs");
                string _sLine =  ":" + _oFrame.nLine;//TODO changing color break linking file
               // string _sLine =  "\f05:" + _oFrame.nLine;
                if (_oFrame.sFile == "??") { _sLine = ""; } 
                Output.Trace("    \f05at   \f04" + _oFrame.sFile  +_sLine );
                 foreach(Var _oVar in _oFrame.aVar) {
                     Output.Trace("    \f05--    \f06  " + _oVar.sName + " = " + _oVar.sValue);
                }

                Output.Trace("");

            }
        }

        public override List<Frame> fStackGet() {
            return aCurrBacktrace;
        }

        public string sResultPrintExp = "";
        public override string fPropertyGet(string _sExp) {
            sCurrentExp = _sExp;
       //     if(_sExp != sLastGetExp){ //Little optimisation
              
               
            //      sLastGetExp = _sExp;
            //    } else {
            //       fReceive_fPropertyGet(_sExp);
            //   }
            if (oProcess.bExeLauch) {
                 fSendCmd("fPropertyGetPrint", "print " +  _sExp);
            } else {
                return "Error";
            }

            return "";
        }
              

        private void fReceive_fPropertyGetPrint(string _sResult) {
            _sResult = _sResult.Replace('\n', ' ').Replace('\r', ' ').Replace('<', '[').Replace('>', ']').Trim();
            if (_sResult.Length > 2 && _sResult[0] == '$' ) {
               _sResult = _sResult.Substring( _sResult.IndexOf('=') + 1).Trim();
            }
             sResultPrintExp = _sResult;

             fSendCmd("fPropertyGetWhat", "whatis " +  sCurrentExp);
        }


         private void fReceive_fPropertyGetWhat(string _sResult) {

           _sResult = _sResult.Replace('\n', ' ').Replace('\r', ' ').Replace('<', '[').Replace('>', ']').Trim();

             string _sRemType = "";
            if(_sResult.StartsWith("type =")){
             _sRemType = _sResult.Substring(_sResult.IndexOf('=') + 1).Trim();
            }

            //"&quot;$2 = {void (gzDataRC * const)} 0x403b30 [gzDataRC::fAddInstance()]&quot;"
            if (sResultPrintExp == _sResult || sResultPrintExp.IndexOf(_sRemType) != -1) { //Print contain _sResult
                   fPropertyGetSend( sResultPrintExp );
            }else{
                  fPropertyGetSend(sResultPrintExp  + "\n" + _sResult );
            }
        //  fPropertyGetSend("fAddInstance");
           
        }

    }
}
