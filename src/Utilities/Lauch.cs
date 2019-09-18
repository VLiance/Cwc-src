using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;

namespace cwc {
    public class Lauch {

        public LauchProject oForm = null;
        public string sBrowser = "";
        public string sPlatform = "";
        public string sViewTarget = "";
        public bool bExportCpp = false;
        public bool bExeLauch = false;
        public bool bStopAll = false;
        public bool bHasError = false;
        public bool bSanitize = false;
        public string sExePath = "";
        public string sWorkPath = "";

        public bool bWeb = false;

       public  Process ExeProcess = null;

        public Lauch() {

        }

        public void fLauchExe(string _sExePath, bool _bSkipLinkTime  = true) {
		//	Debug.fTrace("fLauchExe! " + _sExePath);
            if (sViewTarget != "Build Only" && !bExportCpp){
				string _sWorkPath = _sExePath;
                string _sArg = "";


				if(Data.fGetGlobalVar("_sPlatform") == "Web_Emsc") {
		
					bWeb = true;
					bSanitize = false;
					sBrowser =  Data.fGetViewIn();

					_sArg = "\"" + Data.fGetGlobalVar("vWebRT_Emsc") + "emrun\" ";
					//_sArg +=  "--serve_after_close ";
					_sArg +=  " --kill_exit ";
					//_sArg +=  " --kill_start ";

                   
					_sArg +=  "--browser \"" +  sBrowser + "\" ";
					_sArg += "\"" + _sExePath + "\" ";
		
					_sExePath = Data.fGetGlobalVar("vWebRT_Python") + "python.exe";

					_sWorkPath = _sExePath;
				
				//	public
				}

                /*
                if (sPlatform == "Web_Emsc")   {
                    _sArg = PathHelper.ModulesDir + "/Emscripten_x64/emscripten/1.35.0/emrun ";
                    //_sArg += "--browser \"C:/Program Files (x86)/Mozilla Firefox/firefox.exe\" ";
                   // _sArg += "--browser \"" + sView + "\" ";
                    _sArg += "--browser " + sBrowser + " ";
                    //   _sArg += "--kill_start ";
                    //   _sArg += "--kill_exit ";
                    _sArg += _sExePath + " ";
                    _sExePath = PathHelper.ModulesDir + "/Emscripten_x64/python/2.7.5.3_64bit/python.exe";
                }
*/
                bExeLauch = true;

                 CppCompiler.CheckAllThreadsHaveFinishedWorking(true);

                ////////////////////////

                BackgroundWorker bw = new BackgroundWorker();

                bw.DoWork += new DoWorkEventHandler(
                delegate(object o, DoWorkEventArgs args) {

					
                    if(bSanitize) {
						bSanitize = false;
						Output.TraceWarning("Sanitize feature not enabled for now, please wait for next update");
					}                    



                    if(bSanitize) {
                        //_sArg = " -batch -brief " +_sArg + _sExePath; //-results_to_stderr 
                    //    _sArg = "  -batch -brief " +_sArg + _sExePath; //-results_to_stderr 
						   _sArg = "  -batch -brief -drmemory " + PathHelper.ToolDir  + "/drMemory/ -dr " + PathHelper.ToolDir  + "/drMemory/dynamorio/ " +_sArg + _sExePath + " -lib_blacklist * "; //-results_to_stderr 
                        _sExePath = "Utils\\drMemory\\bin\\drmemory.exe";//PathHelper.GetExeDirectory() ;
                    }

                    sExePath = _sExePath;
                    sWorkPath = _sWorkPath;

                    ProcessStartInfo processStartInfo = new ProcessStartInfo(_sExePath, _sArg);
                    processStartInfo.UseShellExecute = false;
//Debug.fTrace("process.StartInfo.FileName: " + processStartInfo.FileName + "  "  + processStartInfo.Arguments );


                    ExeProcess = new Process();
                //    ExeProcess.EnableRaisingEvents = true;
                 //   ExeProcess.Exited += new EventHandler(myProcess_Exited);

                     if(bSanitize) {

                 //       processStartInfo.CreateNoWindow = true; ///WORK!!!!!
                        processStartInfo.UseShellExecute = false;
                 //    processStartInfo.RedirectStandardOutput = true;  ///WORK!!!!!
                        processStartInfo.RedirectStandardError = true;


                        
                          ExeProcess.OutputDataReceived += (sender, e) => {
                                if (e.Data != null)  {
                                    fSanitizeAppOutput(e.Data);
                                }
                            };

                        ExeProcess.ErrorDataReceived += (sender, e) => {
                            if (e.Data != null)  {
                                fSanitizeOutput(e.Data);
                            }
                        };
                    }


                   


                    ExeProcess.StartInfo = processStartInfo;
                    processStartInfo.WorkingDirectory = Path.GetDirectoryName(_sWorkPath); //_sExePath PathHelper.ModulesDir + "/Emscripten_x64/python/2.7.5.3_64bit/"; //TODO
		//			Debug.fTrace("WorkingDirectory!: " + processStartInfo.WorkingDirectory );



                    bool processStarted = false;

                    if (bStopAll) {
                        bExeLauch = false;
                        return;
                    }

                    try {
                        if (bHasError){
                            return;
                        }
                     

                        processStarted = ExeProcess.Start();

                           if(bSanitize) {
                            // ExeProcess.BeginOutputReadLine();  ///WORK!!!!!
                             ExeProcess.BeginErrorReadLine();
                        }

                        /*
                        Thread.Sleep(500);
                            List<Process>  children = GetChildProcesses(ExeProcess);
                          foreach(Process _procChild in children) {
                            _procChild.OutputDataReceived += (sender, e) => {
                                if (e.Data != null)  {
                                    fSanitizeAppOutput(e.Data);
                                }
                            };
                             _procChild.StartInfo.UseShellExecute = true;
                              _procChild.StartInfo.RedirectStandardOutput = true;
                             _procChild.BeginOutputReadLine();
                        }*/
                             



             //       Output.TraceColored(process.StandardOutput.ReadToEnd());
           //        Output.TraceColored(process.StandardError.ReadToEnd());


                        /*
                         TraceManager.Add(ExeProcess.StandardOutput.ReadToEnd());
                        TraceManager.Add(ExeProcess.StandardError.ReadToEnd());*/
                   
                        //ExeProcess.WaitForExit();
/*
                        if (bWeb)  {
                            string[] _aTest = sBrowser.Split('_');
                            string _sProcessName = (((sBrowser.Split('_'))[0]).Split(' '))[0]; //Correct firefox_nighty

                            int _nTimeOut = 0;
                            while (Process.GetProcessesByName(_sProcessName).Length <= 0 && _nTimeOut < 5000) //5 second if not found  {
                                  Thread.Sleep(1);
                                  _nTimeOut++;
                                    if (bStopAll) {
                                        break;
                                    }
                            }

                            while (!ExeProcess.HasExited && Process.GetProcessesByName(_sProcessName).Length > 0)
                            { 
                                Thread.Sleep(1);
                                if (bStopAll) {
                                    break;
                                }
                            }

                            if (!ExeProcess.HasExited) {
                                try
                                {
                                    ExeProcess.CloseMainWindow();
                                }
                                catch { };
                               // ExeProcess.Kill();
                               // ExeProcess.Close();
                            }

                        }else{*/
                            ////////////////////////////// !!!!! Normal !!!!! //////////////////////////////

                            while (!ExeProcess.HasExited)   {
                                Thread.Sleep(1);
                                if (bStopAll) {
                                    break;
                                }
                            }
							if(!bStopAll) {
							    Build.EndExecution();
							}
                            ////////////////////////////////////////////////////////////////////////////////////////
                      //  }
                        if(oForm != null) {
                            oForm.fLauchEnd();
                        }
                        

                    }
                    catch (Exception ex)
                    {
                       // TraceManager.Add("Error : " + ex.Message + " " + ex.Data + " " + ex.GetType().Name);
                    }


                    bExeLauch = false;

                });
                bw.RunWorkerAsync();

            } else{

                //TraceManager.Add("Success \"" + sViewTarget  + "\" project");

            }
        }

        internal void fEnd() {

			bStopAll = true;
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
            }

            if(!bSanitize) 
                try{
                    /*
                     if (ExeProcess.MainWindowHandle == IntPtr.Zero) {

                       // ExeProcess.CloseMainWindow();
                           ExeProcess.Kill();
                           ExeProcess.Close();
                     }else {
                     */
                    
                            if(ExeProcess.CloseMainWindow()) { //Todo another process  // SEND WM_CLOSE 
                                ExeProcess.WaitForExit(1000); //if hang
                            }
                            if(!ExeProcess.HasExited) {
                                ExeProcess.Kill();
                            }
                            //ExeProcess.Close();
                     // }
               }catch(Exception Ex) { }

               while(!ExeProcess.HasExited  && Base.bAlive) {
                 Thread.Sleep(1);
                 }
                

           }
        


          public static   List<Process>  GetChildProcesses(Process process) {

                List<Process> children = new List<Process>();
                ManagementObjectSearcher mos = new ManagementObjectSearcher(String.Format("Select * From Win32_Process Where ParentProcessID={0}", process.Id));
                foreach (ManagementObject mo in mos.Get())  {
                    children.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
                }

                return children;
            }



      

         public static void fSanitizeAppOutput(string _sOut) {

             lock(Debug.oLockOutPut) {
             //    Console.Write(_sOut + "\r\n");
                 Debug.fAppOut(_sOut );

            }

        }


        
        public static void fSanitizeOutput(string _sOut) {

            //    try{Context.oSingleton.oPluginMain.pluginUI.BeginInvoke((MethodInvoker)delegate { try {

             if( Data.oMainForm != null) {
                     Data.oMainForm.fAddSanitized(_sOut);
                }
            //I:/ FlashDev / _MyProject / Simacode / LDK / LinxDemo / _out / _MainDemo / Windows / Debug / obj / Lib_Demo / Screen / DemoText.o:(.rdata + 0x60): undefined reference to `Lib_GZ::Gfx::cDispacher::fClearChild(Lib_GZ::Gfx::cRoot *)'

            //I:/ FlashDev / _MyProject / Simacode / LDK / LinxDemo / _out / _MainDemo / _cpp / _Lib / GZE / Lib_GZ / Gfx / Dispacher.cpp:43:5: error: expected ';' after expression
                  //  Console.Write(_sOut + "\r");
            lock(Debug.oLockOutPut) {
              Debug.fTrace(_sOut);
            }

        }
          // Handle Exited event and display process information.
    private void myProcess_Exited(object sender, System.EventArgs e) {

     //   eventHandled = true;
    //    Debug.fTrace("Exit time:    {0}\r\n" +  "Exit code:    {1}\r\nElapsed time: {2}", ExeProcess.ExitTime, ExeProcess.ExitCode);
    }





    }
}
