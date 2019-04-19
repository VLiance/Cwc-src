using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using static cwc.ModuleData;

namespace cwc.Update {


    public class ModuleLink {

       public static Dictionary<string,  List<ModuleLink>> aModule = new Dictionary<string,  List<ModuleLink>>();


       // static List<ModuleLink> aModule_LibRT;
       // static List<ModuleLink> aModule_Other;

        public string sFile = "";
        public string sName = "";
        public  string sArch= "";
        public string sVer= "";
		
        public string sDisplayVer= "";
        public string sCompression= "";
        public string sDl_zip = "";

        public string sDl_zip_betterCompression= "";

        public string sTargetDir = "";
        public string sFullTargetDir = "";




        public ProgressBar pbBar = null;

        public Tag oTag = null;
        public Release oRelease = null;

		public ModuleData oModule;
       
  // public int nIndex = 0;

	   public string sToFile = "";
        public string sDownloadLink = "";


     


       public  ModuleLink(ModuleData _oModule,Tag _oTag ) {	
			oTag = _oTag;
			oModule = _oModule;
			sName = oModule.sName;
			sVer = _oTag.name;

			 sFile = sName;
			
			if( _oTag.userDate != "") {
			//	sDisplayVer = sVer + "(" +  _oTag.userDate + ")";
				sDisplayVer = sVer;
			}else {
				sDisplayVer = sVer;
			}
			//oModule.sRepoURL;
	//		  Debug.fTrace("ModuleLink: " + _oTag.name);
			fAddModuleLink();

			  if (!oModule.aLink.ContainsKey(sVer)){
		     //      Debug.fTrace("NEw : " + sName + ": "  +sVer);
                oModule.aLink.Add( sVer, this);
            }
			
			sCompression =  "zip";
			sDl_zip = _oModule.sUrl_Archive + sVer +  "." + sCompression;

			sDl_zip_betterCompression = _oModule.sUrl_Download + sVer + "/" +  sName + "-" + ModuleData.fGetVersion(sVer) + ".7z";
	//		Debug.fTrace("bt:" + sDl_zip_betterCompression);

			sDownloadLink = sDl_zip; //by default  sDl_zip  / sDl_zip_betterCompression
			sToFile = oModule.sDownloadDir +  sName + "_" +sVer + "." + sCompression;
			
		}


       public  ModuleLink(string _sLine) {

                      //      Debug.fTrace("ModuleLink: " + _sLine);
            try { 
         
               _sLine = _sLine.Trim();
                sFile = _sLine;

               sCompression =  Path.GetExtension(_sLine).Substring(1);
                _sLine = _sLine.Substring(0,_sLine.Length - sCompression.Length-1);

                string[] _aLine = _sLine.Split('_'); //Squential
                 sName =  _aLine[0].Trim(); 

                string _sNext = _aLine[1].Trim(); 
                if(_sNext[0] == 'x') {
                    sArch =  _sNext; 
                    sVer =  _aLine[2].Trim();
                }else {
                    sArch =  ""; 
                    sVer =  _sNext;
                }
               /*
                Debug.fTrace("_sName: " + sName);
                Debug.fTrace("_sArch: " + sArch);
                Debug.fTrace("_sVer: " + sVer);
                Debug.fTrace("_sCompression: " + sCompression);
                */
           


            }catch(Exception Ex) { }

			fAddModuleLink();


        }


		public void fAddModuleLink() {
		      //     Debug.fTrace("_sName: " + sName);
            if (!aModule.ContainsKey(sName)){
		     //      Debug.fTrace("NEw : " + sName);
                aModule.Add( sName, new List<ModuleLink>());
            }
             aModule[sName].Add(this);
           		         //  Debug.fTrace("Add : " + sName);
			
		}

		 public void fUpdateData()	{

			if(oRelease == null) {
				Http.fGetHttp(  oModule.sUrl_TagInfo + sVer, fGetReleaseData);
			}else {
			    if(oModule.oForm != null) {	oModule.oForm.fUpdateReleaseInfo(this); }
			}
			
		}
		
		public void fGetReleaseData(ParamHttp _oData) {
				oRelease = Http.fGetRelease( _oData.sResult);
				if(oModule.oForm != null) {oModule.oForm.fUpdateReleaseInfo(this);}
		}	
		

		


		

		public void fProgress(ParamHttp _oParam) {
		
			if(oModule.oForm != null) {oModule.oForm.fUpdateProgress(_oParam); }
					//		Debug.fTrace("prog!!");

		}
		
		public void fComplete(ParamHttp _oParam) {
            bDl_InProgress = false;
            bDl_Completed = true;

             if(oModule.oForm != null) {   oModule.oForm.fDownloadComplete(); }
				
		//	fExtract();
		}
		

          public  WebClient oDlClient;
      
          public bool bDl_InProgress = false;
          public bool bDl_Completed= false;
        public void fDownload() {
              
            string _sDownloadDir = Path.GetDirectoryName(sToFile);
            if(!Directory.Exists(_sDownloadDir)) {
                Directory.CreateDirectory(_sDownloadDir); //TODO create all subdir ...
            }

		     Debug.fTrace("Download Module: " + sName);
  
              Debug.fTrace("sDownloadLink: " + sDownloadLink);
               Debug.fTrace("_sToFile: " + sToFile);
               Debug.fTrace("_sOutFile: " + oModule.sOutFolder);

			if(Http.RemoteFileExists(sDl_zip_betterCompression)) { //Todo BETTER WAY
				sDownloadLink = sDl_zip_betterCompression;
			}
            bDl_InProgress = true;
			Http.fDownload(sDownloadLink, sToFile, fComplete,fProgress );
		   
	
        }


		public void fExtract(){
  
			    oModule.bExtact_InProgress = true;



                sTargetDir = oModule.sOutFolder;
                sFullTargetDir =  sTargetDir;//**TODO _oTool.sTarget contain all libs version, todo optimise to the current extraction **


                try  {

                  //  string _sVersion = "Error";

                    if (sCompression == "zip" || sCompression == "exe" || sCompression == "7z")
                    {

                         Output.TraceWarning("Extract, Compression : " + sCompression);
                /*
                        //Delete directory
						if(sName != "CwcUpd") {
							if (Directory.Exists(_sFullTargetDir)) {
								try {
									Directory.Delete(_sFullTargetDir, true);
								} catch (Exception ex)  {
									Debug.fTrace("Can't delete folder : " + ex.Message);

									//this.BeginInvoke((MethodInvoker)delegate{
									//	 fServerFail(ex.Message);
									//}); return;
								} 
							}
						}
*/
						
                        if (sCompression == "zip") {

                            oModule.bSubExtract = false;
							oModule.fExtractSevenZip(sToFile, sTargetDir, sFullTargetDir, this);

                        }
                        else if (sCompression == "exe")
                        {
  //                          fExtractExe(zipFileName, targetDir);
                        }
                        else if (sCompression == "7z")
                        {
    //                        fExtractSevenZip(zipFileName, targetDir, _sFullTargetDir, _oModule);
                        }


                        //Delete zip
                     //   File.Delete(zipFileName);


                    }else {
                       // fModuleError(_filename);
                        // Debug.fTrace("Wrong compression format");
                    }

                }  catch {
                    /*
                    fModuleError(_filename);
                    this.BeginInvoke((MethodInvoker)delegate { aStatus[_filename].Text = "Error"; });*/
                    
                }

            
            }




	}
}
