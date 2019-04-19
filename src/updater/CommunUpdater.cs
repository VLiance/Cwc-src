using ICSharpCode.SharpZipLib.Zip;
using PluginCore.Helpers;
using PluginCore.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Simacode.Dialogs
{
    public class CommunUpdater : Form
    {
        public static bool isApp64 = (IntPtr.Size == 8);
        public static bool is64 = isApp64 || InternalCheckIsWow64();
        public static string sOS_bit = "";
        /*
        public Button closeButton;
        public Button btnDownload;
        public TableLayoutPanel tableLayoutPanel1;
        public Label lblFlashVer;
        public Button btnAll;
        public Button btnNone;
        public Label lblOS_Architeture;
        */

        public String URL;
        public String BaseURL;
         public String sDownloadDir;
        public string[] aModule;
        public string[] aPfm;
        public bool bCloseMe = false;
        public int nSel = 0;

        public bool bReadFinished = false;

        public BackgroundWorker worker;
        public BackgroundWorker DlgWorker;



        public Dictionary<string, CheckBox> aChk = new Dictionary<string, CheckBox>();
        public Dictionary<string, Label> aStatus = new Dictionary<string, Label>();
        public Dictionary<string, Label> aCurrVersion = new Dictionary<string, Label>();
        public Dictionary<string, Label> aZipSize = new Dictionary<string, Label>();
        public Dictionary<string, Label> aLastVersion = new Dictionary<string, Label>();
        public Dictionary<string, string> aLastVersionType = new Dictionary<string, string>();
        public Dictionary<string, string> aLastCompression = new Dictionary<string, string>();
        public Dictionary<string, ProgressBar> aPrgBar = new Dictionary<string, ProgressBar>();

        /// <summary>
        /// Startups the update check
        /// </summary>
        public void InitializeUpdating()
        {
			TraceManager.Add("---------------------------Initialize Updating-------------------------");
            this.worker = new BackgroundWorker();
            this.worker.DoWork += new DoWorkEventHandler(this.WorkerDoWork);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.WorkerCompleted);
            this.worker.WorkerSupportsCancellation = true;
            this.worker.RunWorkerAsync();
        }

        /// <summary>
        /// Does the actual work on background
        /// </summary>
        public void WorkerDoWork(Object sender, DoWorkEventArgs e)
        {
            try
            {
                TraceManager.Add("--Update check start.");
                WebRequest request = WebRequest.Create(URL);
                request.Proxy = null; //Important remove slow proxy down
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    fReadHttpInfoModule(line);
                }
                bReadFinished = true;

                this.BeginInvoke((MethodInvoker)delegate
                {
                    fUpdateStatus();
                });


            }
            catch (Exception ex)
            {

                TraceManager.Add("Update check failed." + ex);
                ErrorManager.AddToLog("Update check failed.", ex);
                e.Result = null;
            }
        }

        public void WorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {

        }



        public void fExtractZip(string zipFileName, string targetDir)
        {
            FastZip fastZip = new FastZip();
            string fileFilter = null;
            fastZip.ExtractZip(zipFileName, targetDir, fileFilter);
        }

        public void fExtractExe(string sFileName, string sTargetDir)
        {
            string Arguments = " /auto " + sTargetDir;
            Process p = new Process();
            p.StartInfo.FileName = sFileName;// "cmd.exe";
            p.StartInfo.Arguments = Arguments;// " /auto " + sOutputFilePath;

            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            //make the window Hidden 
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
        }

        public void fExtractSevenZip(string zipFileName, string targetDir)
        {
            TraceManager.Add("Extact : " + zipFileName);
            string zPath = PathHelper.ToolDir + "/7z/7zG.exe";
            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.FileName = zPath;
                pro.Arguments = "x \"" + zipFileName + "\" -o" + targetDir;
                Process x = Process.Start(pro);
                x.WaitForExit();
            }
            catch (System.Exception Ex)
            {

                TraceManager.Add("7z error : " + Ex.Message + " : " + Ex.Data + " : " + PathHelper.ToolDir + "/7z/7zG.exe");
                //DO logic here 
            }
        }
        /*
         *   public void fExtractSevenZip(string zipFileName, string targetDir)
         {
             TraceManager.Add("Extract7z");
             string zPath = PathHelper.ToolDir + "/7z/7zG.exe";
             try
             {
                 ProcessStartInfo pro = new ProcessStartInfo();
                 pro.WindowStyle = ProcessWindowStyle.Hidden;
                 pro.FileName = zPath;
                 pro.Arguments = "x \"" + zipFileName + "\" -o" + targetDir;
                 Process x = Process.Start(pro);
                 x.WaitForExit();
             }
             catch (System.Exception Ex)
             {

                 TraceManager.Add("7z error : " + Ex.Message + " : " + Ex.Data + " : " + PathHelper.ToolDir + "/7z/7zG.exe");
                 //DO logic here 
             }
         }*/

        public void fDownloadModule(string _sModule)
        {
            string _sFile = "";
            this.BeginInvoke((MethodInvoker)delegate
            {
                aChk[_sModule].Enabled = false;
                aStatus[_sModule].Text = "Downloading";
            });

            try
            {

                _sFile = _sModule + "_" + aLastVersion[_sModule].Text + "." + aLastCompression[_sModule];
                WebClient _client = new WebClient();
                _client.Proxy = null; //Important remove slow proxy down
                _client.DownloadProgressChanged += client_DownloadProgressChanged(_sModule);
                _client.DownloadFileCompleted += DownloadFileCompleted(_sModule);
                TraceManager.Add("DownloadFile : " + BaseURL + _sFile);
                _client.DownloadFileAsync(new Uri(BaseURL + _sFile), sDownloadDir + "/" + _sModule + "_" + aLastVersionType[_sModule] + "." + aLastCompression[_sModule]);
            }
            catch (Exception ex)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    fModuleError(_sModule);
                });
                TraceManager.Add("Download check failed:" + _sModule);
                ErrorManager.AddToLog("Download check failed.", ex);
                //   e.Result = null;
            }
        }

        public void fModuleError(string _sModule)
        {
            try
            {
                aChk[_sModule].Enabled = true;
                aStatus[_sModule].Text = "Error";
            }
            catch { }
            checkDownloadCompleted();

        }
        public void checkDownloadCompleted()
        {
            if (isDownloadCompleted())
            {
                //chkModuleCLick(null, null);
                fUpdateDownloadBtn();
  
            }
        }

        public bool isDownloadCompleted()
        {
            foreach (Label _lblDl in aStatus.Values)
            {
                if (fIsBusy(_lblDl.Text))
                {
                    return false;
                }
            }
            return true;
        }
        public bool fIsBusy(string _sStatus)
        {
            if (_sStatus == "Downloading" || _sStatus == "Extracting")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Does the actual work on background
        /// </summary>
        public DownloadProgressChangedEventHandler client_DownloadProgressChanged(string filename)
        {
            Action<object, DownloadProgressChangedEventArgs> action = (sender, e) =>
            {
                try
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        try
                        {
                            ProgressBar _oPgr = aPrgBar[filename];

                            double bytesIn = double.Parse(e.BytesReceived.ToString());
                            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                            double percentage = bytesIn / totalBytes * 100;
                            _oPgr.Value = int.Parse(Math.Truncate(percentage).ToString());

                        }
                        catch { }

                    });

                }
                catch { }

            };
            return new DownloadProgressChangedEventHandler(action);

        }

        public void DlgWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            //  TraceManager.Add("--Download Complete");
            //btnDownload.Text = "Download";
        }


     



        public AsyncCompletedEventHandler DownloadFileCompleted(string _filename)
        {
            Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
            {
                try
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {

                        //     TraceManager.Add("Completed : " + _filename);
                        if (e.Error != null)
                        {
                            fModuleError(_filename);

                            TraceManager.Add("My Error : " + _filename + "_" + aLastVersion[_filename].Text + "." + aLastCompression[_filename] + " : " + e.Error + " : " + e.Error.Message); //Used by other process ...
                            // throw e.Error;
                        }
                        else
                        {
                            if (_filename != "Simacode_IDE")
                            {
                                aStatus[_filename].Text = "Extracting";
                                InitializeExtracting(_filename);
                            }
                            else
                            {
                                fExecuteUpdater();
                            }
                        }
                    });

                }
                catch { }

            };
            return new AsyncCompletedEventHandler(action);
        }

        private void fExecuteUpdater()
        {
            aStatus["Simacode_IDE"].Text = "Pending";

            var res = MessageBox.Show(this, "The Simacode IDE must quit to perform the update, do it now?", "Update?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                Process p = new Process();
                p.StartInfo.FileName = PathHelper.ToolDir + "/Updater_x32/Updater.exe";
                p.Start();

                bCloseMe = true;
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                Process p = new Process();
                p.StartInfo.FileName = PathHelper.ToolDir + "/Updater_x32/Updater.exe";
                p.Start();
            }
        }


        public void InitializeExtracting(string _filename)
        {
	TraceManager.Add("---------------------------Initialize Updating-------------------------");
            //TraceManager.Add("-IniExtract");
            BackgroundWorker _worker = new BackgroundWorker();
            _worker.DoWork += ExtractWorkerDoWork(_filename);
            // this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.WorkerCompleted);
            _worker.WorkerSupportsCancellation = true;
            _worker.RunWorkerAsync();
        }

        DoWorkEventHandler ExtractWorkerDoWork(string _filename)
        {
            Action<object, DoWorkEventArgs> action = (sender, e) =>
            {

                string _sBaseDir = sDownloadDir;
                if (_filename == "Updater")
                {
                    _sBaseDir = PathHelper.ToolDir;
                }

                string _sCompression = aLastCompression[_filename];
                var zipFileName = sDownloadDir  + "/" + _filename + "_" + aLastVersionType[_filename] + "." + _sCompression;
                //   TraceManager.Add("-Zipname : " + zipFileName);

                var targetDir = _sBaseDir + "/";
                string _sDir = targetDir + _filename + "_" + aLastVersionType[_filename];

                try
                {

                    string _sVersion = "Error";

                    if (_sCompression == "zip" || _sCompression == "exe" || _sCompression == "7z")
                    {

                        TraceManager.Add("Extract, Compression : " + _sCompression);
                        //Delete directory

                        if (Directory.Exists(_sDir))
                        {
                            try {
                                Directory.Delete(_sDir, true);
                            } catch (IOException)  {
                                this.BeginInvoke((MethodInvoker)delegate{
                                    fModuleError(_filename);
                                }); return;
                            }  catch (UnauthorizedAccessException) {
                                this.BeginInvoke((MethodInvoker)delegate  {
                                    fModuleError(_filename);
                                }); return;
                            }
                        }

                        if (_sCompression == "zip")
                        {
                            fExtractZip(zipFileName, targetDir);
                        }
                        else if (_sCompression == "exe")
                        {
                            fExtractExe(zipFileName, targetDir);
                        }
                        else if (_sCompression == "7z")
                        {
                            fExtractSevenZip(zipFileName, targetDir);
                        }



                        // Will always overwrite if target filenames already exist


                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            try
                            {
                                _sVersion = aLastVersion[_filename].Text;

                                string _sInfoPath = _sDir + "/info.txt";
                                File.Create(_sInfoPath).Dispose(); ;
                                TextWriter tw = new StreamWriter(_sInfoPath, true);
                                tw.WriteLine("Version:");
                                tw.WriteLine(_sVersion);
                                tw.Close();

                                aStatus[_filename].Text = "Complete";
                                checkDownloadCompleted();
                            }
                            catch
                            {
                                fModuleError(_filename);
                                TraceManager.Add("Error writing info : " + targetDir + _filename + "_" + aLastVersionType[_filename] + "/info.txt");

                            }

                        });

                        //Delete zip
                        File.Delete(zipFileName);

                        if (_filename == "Updater")
                        {
                            //Updater finished, Now download simacode IDE
                            nSel = 0; //Updater first, (Workaroud)
                            fOverInitializeDownload();
                        }

                    }
                    else
                    {
                        fModuleError(_filename);
                        TraceManager.Add("Wrong compression format");
                    }

                }
                catch
                {
                    fModuleError(_filename);
                    this.BeginInvoke((MethodInvoker)delegate { aStatus[_filename].Text = "Error"; });
                }


            };
            return new DoWorkEventHandler(action);

        }

        public virtual void fOverInitializeDownload() //TODO
        {

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CommunUpdater
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "CommunUpdater";
            this.Load += new System.EventHandler(this.CommunUpdater_Load);
            this.ResumeLayout(false);

        }

        private void CommunUpdater_Load(object sender, EventArgs e)
        {

        }
        public void fUpdateStatus()
        {
            int i = 0;
            foreach (Label _lblSel in aStatus.Values) {
                if (fIsBusy(_lblSel.Text))
                {
                    return;
                }

                string _sModule = aModule[i];

                if (_sModule == "Simacode_IDE")
                {
                    aCurrVersion[_sModule].Text = "x32_" + Application.ProductVersion;
                    String _sLastVersion = aLastVersion[_sModule].Text;

                    if (aCurrVersion[_sModule].Text != _sLastVersion)
                    {
                        _lblSel.Text = "Outdated";

                    }
                    else
                    {
                        _lblSel.Text = "Up-to-date";
                        aChk[_sModule].Enabled = false;
                    }
                }
                else
                {

                    string _sDirectory = sDownloadDir + "/" + _sModule;
                    bool _bIs64 = false;

                    if (is64 && Directory.Exists(_sDirectory + "_x64"))
                    {
                        _sDirectory = _sDirectory + "_x64";
                        _bIs64 = true;
                    }
                    else
                    {
                        _sDirectory = _sDirectory + "_x32";
                    }

                    if (_bIs64 || Directory.Exists(_sDirectory))
                    {

                        try
                        {   // Open the text file using a stream reader.


                            StreamReader _reader = new StreamReader(_sDirectory + "/info.txt");

                            String _sLabel = _reader.ReadLine(); // Read version
                            String _sVersion = _reader.ReadLine(); // Read download

                            /*
                            string _sBitType = "x32";
                            if (_bIs64)
                            {
                                _sBitType = "x64";
                            }*/
                            //string _sVersionFull =  _sBitType + "_" + _sVersion;

                            aCurrVersion[_sModule].Text = _sVersion;

                            //_bIs64

                            String _sLastVersion = aLastVersion[_sModule].Text;

                            if (_sLastVersion == "---")
                            {
                                _lblSel.Text = "Present/Wait..";
                            }
                            else if (_sVersion != _sLastVersion)
                            {
                                _lblSel.Text = "Outdated";
                            }
                            else
                            {
                                _lblSel.Text = "Present";
                                aChk[_sModule].Checked = true;
                                aChk[_sModule].Enabled = false;
                            }

                        }
                        catch (Exception e)
                        {
                            fModuleError(_sModule);
                            Console.WriteLine("The file could not be read : " + e.Message);
                        }

                    }
                    else
                    {
                        fModuleError(_sModule);
                        _lblSel.Text = "Missing";
                    }
                }

                if (_sModule == "Updater")
                {
                    aChk["Updater"].Enabled = false;
                     if(aStatus["Updater"].Text == "Missing"){
                     aStatus["Updater"].Text = "---";
                  }
                }

                i++;
            }
            fUpdateDownloadBtn();
        }

        public virtual void fUpdateDownloadBtn(){ }


        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );
        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }


        public void fReadHttpInfoModule(string _sLine)
        {
            string _sZipSize = "";
            string _sModuleSize = "";
            //--- Todo reIni all value ---

            TraceManager.Add("ReadLine : " + _sLine);
            this.BeginInvoke((MethodInvoker)delegate
            {
                try
                {
                    string[] _aArg = _sLine.Split(' ');
                    if (_aArg.Length >= 4)
                    {

                        if (_aArg.Length >= 5)
                        {
                            _sZipSize = _aArg[4] + "mb";
                        }
                        if (_aArg.Length >= 6)
                        {
                            _sModuleSize = _aArg[5];
                        }

                        string _sModule = _aArg[0];
                        string _sBit = _aArg[1];
                        string _sVersion = _aArg[2];
                        string _sCompression = _aArg[3];

                        if (is64)
                        {
                            //////////// OS 64 bits /////////////
                            switch (_sBit)
                            {
                                case "x64":
                                case "x64only":
                                    aLastVersion[_sModule].Text = "x64_" + _sVersion;
                                    aLastVersionType[_sModule] = "x64";
                                    aLastCompression[_sModule] = _sCompression;
                                    aZipSize[_sModule].Text = _sZipSize;
                                    break;

                                case "x32only":
                                    aLastVersion[_sModule].Text = "x32_" + _sVersion;
                                    aLastVersionType[_sModule] = "x32";
                                    aLastCompression[_sModule] = _sCompression;
                                    aZipSize[_sModule].Text = _sZipSize;
                                    break;

                                case "any":
                                    aLastVersion[_sModule].Text =  _sVersion;
                                    aLastVersionType[_sModule] = "";
                                    aLastCompression[_sModule] = _sCompression;
                                    aZipSize[_sModule].Text = _sZipSize;
                                  break;
                            }
                        }
                        else
                        {
                            //////////// OS 32 bits /////////////

                            switch (_sBit)
                            {

                                case "x64only":
                                    aLastVersion[_sModule].Text = "x64_" + _sVersion;
                                    aLastVersionType[_sModule] = "Only on x64";
                                    aLastCompression[_sModule] = _sCompression;
                                    aZipSize[_sModule].Text = _sZipSize;
                                    break;

                                case "x32":
                                case "x32only":
                                    aLastVersion[_sModule].Text = "x32_" + _sVersion;
                                    aLastVersionType[_sModule] = "x32";
                                    aLastCompression[_sModule] = _sCompression;
                                    aZipSize[_sModule].Text = _sZipSize;
                                    break;
                                case "any":
                                    aLastVersion[_sModule].Text = _sVersion;
                                    aLastVersionType[_sModule] = "";
                                    aLastCompression[_sModule] = _sCompression;
                                    aZipSize[_sModule].Text = _sZipSize;
                                    break;
                            }

                        }
                    }
                }
                catch
                {
                    TraceManager.Add("Error:fReadHttpInfoModule ");
                };
            });

        }


    }
}
