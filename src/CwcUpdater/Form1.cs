using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Updater
{
    public partial class Form1 : Form
    {
        private string sCwcPath;
        private bool bHasError = false ;

        public Form1()
        {
            InitializeComponent();

            sCwcPath = (new System.IO.FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)).Directory + "\\..\\";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                Thread workerThread = new Thread(DoWork);
                workerThread.Start();
            });
        }


        private void DoWork(){
           // fWaitInstance("cwc.vshost");
            fWaitInstance("cwc");
            fExtractSevenZip(Program.sOutpuFile, sCwcPath + "/");
        }


        private void fWaitInstance(string _sInstance){
            Process[] _aProc = System.Diagnostics.Process.GetProcessesByName(_sInstance);
			for(int i = 0; i < _aProc.Length; i++) {
				if(_aProc[i].ProcessName == _sInstance){
				
					 Process proc = _aProc[i];
					this.BeginInvoke((MethodInvoker)delegate
					{
						lblProgress.Text = "Waiting for CWC instance closing: " + proc.Id.ToString() ;
					});
					while (!proc.HasExited)
					{
						Thread.Sleep(16);
					}
				}
			}
           
        } 



        public void fExtractSevenZip(string zipFileName, string targetDir)
        {
		Console.WriteLine("  zipFileName: " +    zipFileName);
			zipFileName = Path.GetFullPath(zipFileName);
			targetDir = Path.GetFullPath(targetDir);

            //TraceManager.Add("Extract7z");
            string zPath = Path.GetFullPath( sCwcPath + "Updater/7z.exe");
		//	Console.WriteLine("zPath: " + zPath) + "7z.exe");
            try  {

                ProcessStartInfo pro = new ProcessStartInfo();


                pro.UseShellExecute = false;
                pro.RedirectStandardOutput = true;
               pro.RedirectStandardError = true;

             //   pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.WindowStyle = ProcessWindowStyle.Minimized;
                pro.FileName = zPath;
                pro.Arguments = "x \"" + zipFileName + "\" -o\"" + targetDir  + "\" -x!Updater\\*  -aoa -ssw";

    //    pro.Arguments = "x \"" + zipFileName + "\" -o" + targetDir  + " -x*  -aoa -ssw"; //To disable extract

			//	Console.WriteLine("   pro.Arguments: " +    pro.Arguments);
                Process process = new Process();
                process.StartInfo = pro;

				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

             //   Process x = Process.Start(pro);             
                process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
         
                process.Start();
       
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
               // System.Windows.Forms.Application.Exit();
               // Application.Exit();
    
               this.BeginInvoke((MethodInvoker)delegate  {



//bHasError = true;

                if (!bHasError) {


						string _sFileName = Path.GetFileNameWithoutExtension(zipFileName);
					string[] _aFileName =_sFileName.Split('_');
					string _sVersion = _aFileName[_aFileName.Length - 1];
						
                    lblProgress.Text = "Done";
        //            File.Delete(zipFileName);

                    Process _process = new Process();
                    _process.StartInfo.FileName = targetDir + "/cwc.exe";
					
					 _process.StartInfo.Arguments = "Updated " + _sVersion + " " + Program.sWorkDir + " " + Program.sResendArg;		

                    _process.Start();

                    this.Close();
                }
               
             });
               
            }
            catch (System.Exception Ex)
            {

        this.BeginInvoke((MethodInvoker)delegate  {
                lblProgress.Text = "Error: "+ Ex.Message + " " + Ex.Data;
             //   Debug.WriteLine("7z error : " + Ex.Message + " : " + Ex.Data);
               // TraceManager.Add("7z error : " + Ex.Message + " : " + Ex.Data + " : " + PathHelper.ToolDir + "/7z/7zG.exe");

            });
                //DO logic here 
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

         void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine) {
             this.BeginInvoke((MethodInvoker)delegate
             {
                 string _sOuput = outLine.Data;
                 if (_sOuput != null && _sOuput.Length > 1)
                 {
					if(_sOuput[ _sOuput.Length-1] == '\n') {
						_sOuput = _sOuput.Substring(0, _sOuput.Length-1);
					}

                     lOutput.Items.Add(_sOuput);
                   //  Console.WriteLine(outLine.Data);

                     if (_sOuput.Length > 6 && _sOuput.Substring(0, 6) == "ERROR:") {
                     //    lOutput.Items.Add(outLine.Data);
                         bHasError = true;
                         lblProgress.Text = "Error";
                     }
                 }
             });
        }

         private void Form1_FormClosed(object sender, FormClosedEventArgs e)
         {
             ///System.Windows.Forms.Application.Exit();
             System.Environment.Exit(0);
         }

		private void label2_Click(object sender, EventArgs e)
		{

		}
	}
}
