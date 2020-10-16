using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using cwc.Update;
using System.Management;
using System.Windows.Forms;

namespace cwc {
    public class LauchTool {

     ///    public MainForm oForm = null; //Delegate?

        public bool bReturnError = false;
        public bool bReturnBoth = false;

        public bool bExeLauch = false;
        public bool bExeLauched = false;
     //   public bool bExited = false;
        public bool bStopAll = false;
        public bool bHasError = false;

        public string sExePath = "";
        public string sExeName = "";
        public string sArg = "";
        public string sWorkPath = "";
        public bool bOutput  = true;
        public string sSourceFile  = "";
        public string sTarget  = "";

        public bool bExterneLauch = false;
        public bool bRedirectOutput = true;

public string sResult ="";
		public	string sError ="";

      public   delegate void dIExit(LauchTool _oTool);
      public   delegate void dIOut(LauchTool _oTool, string _sOut);
      public   delegate void dIError(LauchTool _oTool, string _sOut);

       public   dIExit dExit = null; 
       public   dIOut dOut = null; 
       public   dIError dError = null; 


		public bool bDontKill;
		public bool UseShellExecute = false;
		 public  Process ExeProcess = null;
        public ModuleLink oModule = null;

        public Object oCustom = null;

        public bool bRunInThread = true;
        public ProcessStartInfo processStartInfo = null;
		
        public bool bWaitEndForOutput  = false;


        public string fLauchExe(string _sExePath, string _sArg, string _sSourceFile = "", string _sTarget= "", bool _bDontKill = false) {
			  sTarget =  _sTarget;
                sSourceFile = _sSourceFile;
				sArg = _sArg;
				bDontKill = _bDontKill;

             //   string _sArg = "";
                bExeLauch = true;
	
                    sExePath = _sExePath;
					if(sWorkPath == "") {
						sWorkPath = _sExePath;
					}
                    sExeName =Path.GetFileName( Path.GetDirectoryName(sExePath));

			if(bRunInThread) {
						BackgroundWorker bw = new BackgroundWorker();

						bw.DoWork += new DoWorkEventHandler(
						delegate(object o, DoWorkEventArgs args) {
							fLauch();
						});
						bw.RunWorkerAsync();
			}else {
				return fLauch();
			}
			return "";
				
        }

		
       private  string fLauch() {
				
				string _sResult ="";
				string _sError ="";
            /*
				if(!File.Exists(sExePath)){
					Output.TraceError("Unable to lauch: " + sExePath);
				}*/
				
                     processStartInfo = new ProcessStartInfo( Path.GetFullPath( sExePath), sArg);
                    processStartInfo.UseShellExecute = UseShellExecute;

//bWaitEndForOutput = true;


//Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

                   ExeProcess = new Process();
               //    ExeProcess.EnableRaisingEvents = true;
               //    ExeProcess.Exited += new EventHandler(fExited);
                   

                     if(bOutput) {
         
						if(!bExterneLauch){
								//processStartInfo.UseShellExecute = false;
								processStartInfo.UseShellExecute = false;
                    processStartInfo.CreateNoWindow = bHidden;
                   // processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						//		processStartInfo.CreateNoWindow = true;
							processStartInfo.RedirectStandardOutput = bRedirectOutput;
							processStartInfo.RedirectStandardError = true;
							processStartInfo.RedirectStandardInput = true;
						}

/*
						if(!bWaitEndForOutput) {
							ExeProcess.OutputDataReceived += (sender, e) => {
									if (e.Data != null)  {
										fAppOutput(this, e.Data);
									}
								};
							ExeProcess.ErrorDataReceived += (sender, e) => {
								if (e.Data != null)  {
									fAppError(this, e.Data);
								}
							};
						}*/
                    }

                    ExeProcess.StartInfo = processStartInfo;
					 processStartInfo.WorkingDirectory = Path.GetDirectoryName(sWorkPath); 

                    bool processStarted = false;

                    if (bStopAll) {
                		bExeLauched = true;
                        bExeLauch = false;
                        return "";
                    }

                    try {
                        if (bHasError){
                           return "";
                        }

                Debug.fTrace("--------Lauch: " +   sExePath + "  " + processStartInfo.Arguments  );

                if(!File.Exists( sExePath)) {
                    Output.TraceError("No executable file found to lauch: \"" +  sExePath + "\" " + processStartInfo.Arguments);
                     return "";
                }


			//Debug.fTrace("Arguments: " +   processStartInfo.Arguments );
 			//	Debug.fTrace("WorkingDirectory***: " + processStartInfo.WorkingDirectory );


                try{
                        processStarted = ExeProcess.Start();
                } catch (Exception e) {
                    Output.TraceError("Unable to lauch: " + sExePath + " ["  + sWorkPath + "] : " + e.Message);
                }
        

                         if(bDontKill) {
                            Data.nDontKillId = ExeProcess.Id;
                        }
						bExeLauched = true;

		
			if(!bExterneLauch && !bWaitEndForOutput) {
				ProcessOutputHandler outputHandler = new ProcessOutputHandler(this);
                   
				if(bRedirectOutput){
					Thread stdOutReader = new Thread(new ThreadStart(outputHandler.ReadStdOut));
					stdOutReader.Start();
				}

				Thread stdErrReader = new Thread(new ThreadStart(outputHandler.ReadStdErr));
				stdErrReader.Start();
//stdOutReader.Priority = ThreadPriority.AboveNormal;
				ExeProcess.WaitForExit(); //Stop here
			}


					//	if(bWaitEndForOutput) {
							if(bRedirectOutput){
								_sResult +=  ExeProcess.StandardOutput.ReadToEnd();
							}
							_sError +=  ExeProcess.StandardError.ReadToEnd();
						
							if(bWaitEndForOutput) { ExeProcess.WaitForExit();}
							
							if(bRedirectOutput && dOut != null && _sResult != ""){
								dOut(this, _sResult);
							}
							if(dError != null  && _sError != ""){
								dError(this, _sError);
							}
/*
						}else {
							 ExeProcess.WaitForExit();
							_sResult +=  ExeProcess.StandardOutput.ReadToEnd();
							_sError +=  ExeProcess.StandardError.ReadToEnd();
							if(dOut != null && _sResult != ""){
								dOut(this, _sResult);
							}
							if(dError != null  && _sError != ""){
								dError(this, _sError);
							}*/

							

						/*
							while(!ExeProcess.HasExited){
								// if (!ExeProcess.StandardOutput.EndOfStream){
									string _sOut = ExeProcess.StandardOutput.ReadLine();
									if(dOut != null && _sOut != ""){
										dOut(this, _sOut);
									}
								//}
								//if (!ExeProcess.StandardOutput.EndOfStream){
									string _sErr = ExeProcess.StandardError.ReadLine();
									if(dError != null  && _sErr != ""){
										dError(this, _sErr);
									}
								//}
								Thread.Sleep(1);
							}
							_sResult +=  ExeProcess.StandardOutput.ReadToEnd();
							_sError +=  ExeProcess.StandardError.ReadToEnd();
						
							 ExeProcess.WaitForExit();
							if(dOut != null && _sResult != ""){
								dOut(this, _sResult);
							}
							if(dError != null  && _sError != ""){
								dError(this, _sError);
							}*/
							
							/*
							 if(bOutput) {
								 ExeProcess.BeginOutputReadLine();///SLOWWW
								 ExeProcess.BeginErrorReadLine();
							}*/
							
					//	}

	
					     ExeProcess.WaitForExit(); //important for geting last output!	
						
						sResult = _sResult;
						sError = _sError;

                	     while(!ExeProcess.HasExited  && Base.bAlive) {
                                Thread.CurrentThread.Join(1);
                          }
                        if(dExit != null) {
                             dExit(this);
                        }
                        if(bDontKill && ExeProcess.Id == Data.nDontKillId) {
                            Data.nDontKillId = 0;
                        }
						
					

                        /*
                        if(oForm != null) {
                            oForm.fLauchEnd();
                        }*/

                    }  catch (Exception ex){
							Debug.fTrace(ex.Message);
					 }

                    bExeLauch = false;


                      //           Debug.fTrace("--------------------------- 7z finish -------------------------");
	
			if(bReturnBoth){
				return _sResult + "\n" + _sError;
			}else{
				if(!bReturnError){
					return _sResult;
				}else{
					return _sError;
				}
			}
		}









public bool bSanitize = false;
        public bool bHidden = false;

        internal void fEnd() {
		  //  Output.Trace("\f18--Try to Close--");
			bStopAll = true;

            if(dExit != null){ dExit(this);};
            SysAPI.KillProcessAndChildren( Data.MainProcess.Id ); //TODO more gentle with -- SEND WM_CLOSE -- ?


            while(!ExeProcess.HasExited  && Base.bAlive) {
                Thread.CurrentThread.Join(1);
                }
     

            return;


///////////////////////////////////////////////

             if(bSanitize) {
                List<Process>  children = GetChildProcesses(ExeProcess);
                foreach(Process _procChild in children) {
                  // Debug.fTrace("---------------ID: " + _procChild.Id.ToString());
             
                  //      Debug.fTrace("sExePath: " +sExePath);

                    
                    /* //Try to nurge, (not the besst way)
                     ProcessStartInfo processStartInfo = new ProcessStartInfo(sExePath, " -nudge " + _procChild.Id.ToString());
                    processStartInfo.UseShellExecute = false;

                    ExeProcess.StartInfo = processStartInfo;
                    processStartInfo.WorkingDirectory = Path.GetDirectoryName(sWorkPath); //_sExePath PathHelper.ModulesDir + "/Emscripten_x64/python/2.7.5.3_64bit/"; //TODO
                    ExeProcess.Start();
                    */

                    try{
                         if(_procChild.CloseMainWindow()) { //Todo another process  // SEND WM_CLOSE 
							//	Debug.fTrace("");
							 Output.Trace("\f4C-- SEND WM_CLOSE --");
                             //   _procChild.WaitForExit(1000); //if hang
                          }else {
							 if(!_procChild.HasExited) {
								 Output.Trace("\f4C-- KILL --");
								_procChild.Kill();
                                if(dExit != null){ dExit(this);};
							}
						}
                       // _procChild.Close();
                     }catch(Exception Ex) { }

           
                   // .CloseMainWindow();


                 //   _procChild.WaitForExit(1000); //if hang


                 //      _procChild.WaitForExit(1000); //if hang
                 //    _procChild.Kill();
                   //  _procChild.CloseMainWindow();
                  //   _procChild.Close();
                   //  ExeProcess.Clos();
                  //  _procChild.Kill();
                }

            }else { 

           List<Process>  children = GetChildProcesses(ExeProcess);
                foreach(Process _procChild in children) { //TODO recursive of child process?

                try{

                    //Output.TraceError("Try to kill");
                    
                    /*
                     if (ExeProcess.MainWindowHandle == IntPtr.Zero) {

                       // ExeProcess.CloseMainWindow();
                           ExeProcess.Kill();
                           ExeProcess.Close();
                     }else {
                     */
                    if(!_procChild.HasExited) {
                           //  Output.TraceError("not HasExited");

                            try{if(_procChild.CloseMainWindow()) { //Todo another process  // SEND WM_CLOSE 
                                _procChild.WaitForExit(1000); //if hang
                            } }catch(Exception Ex) { }


                            if(!_procChild.HasExited) {
                               // Output.TraceError("alwaus not HasExited");
                               //  Output.Trace("\f4C-- KILL --");
                                _procChild.Kill();
                                if(dExit != null){ dExit(this);};


                            }
                    }



                            //ExeProcess.Close();
                     // }
               }catch(Exception Ex) { }

            

             }
            }

                while(!ExeProcess.HasExited  && Base.bAlive) {
                 Thread.CurrentThread.Join(1);
                 }
               //             Output.Trace("\f18-Finish-");
           }
        


          public static   List<Process>  GetChildProcesses(Process process) {

                List<Process> children = new List<Process>();
                ManagementObjectSearcher mos = new ManagementObjectSearcher(String.Format("Select * From Win32_Process Where ParentProcessID={0}", process.Id));
                foreach (ManagementObject mo in mos.Get())  {
                    children.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
                }

                return children;
            }











        public static void fAppOutput(LauchTool _oThis,string _sOut) {

      //       lock(Data.oLockOutPut) {
				
				     if(_oThis.dOut != null) {
                             _oThis.dOut(_oThis, _sOut);
                    }else {
							  /*
							if(_sOut.Length > 3 && _sOut[3] == '%') {
							  Console.Write( "\r\r" + _sOut.Substring(4,_sOut.Length-4) );
							}else { */
							lock(Debug.oLockOutPut) {//???
							 Debug.fTrace(_sOut );
								}
						   // }
					}
             
           // }
        }

		 public static void fAppError(LauchTool _oThis,string _sOut) {
				// lock(Data.oLockError) {
						 if(_oThis.dError != null) {
								 _oThis.dError(_oThis, _sOut);
						}else {
								  /*
								if(_sOut.Length > 3 && _sOut[3] == '%') {
								  Console.Write( "\r\r" + _sOut.Substring(4,_sOut.Length-4) );
								}else { */
								Debug.fTrace(_sOut );
							   // }
						}
             
				//}
			}




      private void fExited(object sender, System.EventArgs e) {
			
         //  lock(Data.oLockOutPut) {
               // Debug.fTrace(" -- Finish --" );
             //   Debug.fTrace("Exit time:    {0}\r\n" +  "Exit code:    {1}\r\nElapsed time: {2}", ExeProcess.ExitTime, ExeProcess.ExitCode);
         // }
	   }


		   public  void fSend(string _sMsg) {
                while (bExeLauch && !bExeLauched) { //Wait for starting
                    Thread.CurrentThread.Join(1);
                }
                if(bExeLauch){
			     ExeProcess.StandardInput.WriteLine(_sMsg ); ///bug
                }
			   // ExeProcess.StandardInput.WriteLine(_sMsg + "\n"); ///!!GDB dont like "\n" 
			}
/*
		public bool fProcessIsRunning(){
			  bool isRunning;
			  try {
				isRunning = !ExeProcess.HasExited && ExeProcess.Threads.Count > 0;
			  }
			  catch(SystemException sEx)
			  {
				isRunning = false;
			  }
			  
			  return isRunning;
		  } */






    }




public class ProcessOutputHandler
{
    public Process proc { get; set; }
    public string StdOut { get; set; }
    public string StdErr { get; set; }

		public LauchTool oTool;

    /// <summary>  
    /// The constructor requires a reference to the process that will be read.  
    /// The process should have .RedirectStandardOutput and .RedirectStandardError set to true.  
    /// </summary>  
    /// <param name="process">The process that will have its output read by this class.</param>  
    public ProcessOutputHandler(LauchTool _oTool )
    {
		oTool = _oTool;
        proc = _oTool.ExeProcess;

        StdErr = "";
        StdOut = "";
   //     Debug.Assert(proc.StartInfo.RedirectStandardError, "RedirectStandardError must be true to use ProcessOutputHandler.");
  //      Debug.Assert(proc.StartInfo.RedirectStandardOutput, "RedirectStandardOut must be true to use ProcessOutputHandler.");
    }

 
    public void ReadStdErr() {
   
        string _sLine;
		if(oTool.dError != null){
                try { 
			while (!proc.HasExited){
				_sLine = proc.StandardError.ReadLine();
				if (_sLine != ""){
						oTool.dError(oTool, _sLine);
				}else{
					Thread.CurrentThread.Join(1);
				}
			}
                } catch(Exception e) {       Output.TraceError("Error: " + e.Message);}
		}
    }
    public void ReadStdOut() {
    
        string _sLine;
		if(oTool.dOut != null){
                           try { 
			while (!proc.HasExited){

				_sLine = proc.StandardOutput.ReadLine();
		
				if (_sLine != ""){
						oTool.dOut(oTool, _sLine);
				}else{
					Thread.Sleep(1);
				}
			}
              } catch(Exception e) {       Output.TraceError("Error: " + e.Message);}
		}
    }

/*
    public void ReadStdOut() {
        string line;
		  while (!proc.HasExited){
			Thread.Sleep(1);

				line = proc.StandardOutput.ReadLine();
			if (line != ""){
            StdOut += line;
 			Console.WriteLine("OO:" + line);
        }}
    }*/

}




















}
