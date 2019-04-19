using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using System.Runtime.Serialization.Json;
using System.Net;
using System.ComponentModel;
using cwc.Utilities;
using cwc.Update;

namespace cwc
{

	internal class DataAssetAttribute : Attribute
	{
	}

	internal class CollectionDataContractAttribute : Attribute
	{
	}
	internal class DataContractAttribute : Attribute
	{
	}

	internal class DataMemberAttribute : Attribute
	{
	}

	[CollectionDataContract]
	public class Releases : List<Release>
	{}
	[CollectionDataContract]
	public class Tags : List<Tag>
	{}

	[CollectionDataAsset]
	public class aAssetlist : List<Asset>
	{}

	internal class CollectionDataAssetAttribute : Attribute
	{
	}

	internal class DataReleaseAttribute : Attribute
	{}

	internal class DataTagAttribute : Attribute
	{
	}

	internal class DataCommitAttribute : Attribute
	{
	}




	[DataCommit]
	public class Commit
	{
			public Commit() { }
			
			[DataMember]
			public string sha { get; set; }

			[DataMember]
			public string url { get; set; }


	}




//https://github.com/aktau/github-release/tags
//https://github.com/aktau/github-release/tags?after=v0.5.2
//https://github.com/d3/d3/releases
//https://github.com/d3/d3/tags
//https://api.github.com/repos/d3/d3/tags
//https://api.github.com/repos/d3/d3/tags?page=1&per_page=100
//https://api.github.com/repos/d3/d3/tags?page=2&per_page=100


	[DataTag]
	public class Tag
	{
			public Tag() { }

			public string date= "";
			public string userDate = "";
			public string link= "";

			
			[DataMember]
			public string name { get; set; }

			[DataMember]
			public int id{ get; set; }

			[DataMember]
			public string zipball_url { get; set; }

			public string tarball_url { get; set; }

			[DataMember]
			public Commit commit { get; set; }
	}


	[DataRelease]
	public class Release
	{
			public Release() { }


					public string download = "";

			[DataMember]
			public int id{ get; set; }

			[DataMember]
			public string url { get; set; }

			[DataMember]
			public string body { get; set; }

			[DataMember]
			public string tag_name { get; set; }

			[DataMember]
			public string target_commitish { get; set; }

			[DataMember]
			public string name { get; set; }

			[DataMember]
			public aAssetlist assets { get; set; }

			
	}



	[DataAsset]
	public class Asset
	{
			public Asset() { }

			[DataMember]
			public string url { get; set; }

			[DataMember]
			public int id{ get; set; }

			[DataMember]
			public string name { get; set; }

			[DataMember]
			public string label { get; set; }

			[DataMember]
			public string content_type { get; set; }

			[DataMember]
			public uint download_count { get; set; }

			[DataMember]
			public uint size { get; set; }
	}




	public  class ParamHttp{
	
		public bool bDownloadFile = false;
		public string sURL;
		public string sToFile;
		public string sResult;
		public string sInfo;
		public WebHeaderCollection oHeader;
		public dHttpComplete dComplete;
		public dHttpComplete dCustom;
		public dHttpComplete dProgress;
		public bool bFail = false;
		public string sFailMsg = "";
		public Object oContainer;
		public  Object oObj;
		public string sAltURL = "";
		public string sCustom = "";
		public int nCustom = 0;
		public int BytesReceived = 0;
		public int TotalBytesToReceive = 0;
		public double nPercentage = 0;
		public double nAprox = 0;
		public double nTotalBytes = 0;
		public double nBytes = 0;
		public double nLastBytes = 0;


		//String file
		public ParamHttp(string _sURL, dHttpComplete _dComplete, Object _oContainer, Object _oObj, dHttpComplete _dCustom, string _sAltURL = "",  string _sCustom= "", int _nCustom = 0) {
			sURL = _sURL;
			dComplete = _dComplete;
			oContainer = _oContainer;
			oObj = _oObj;
			dCustom = _dCustom;
			sAltURL = _sAltURL;
			sCustom = _sCustom;
			nCustom = _nCustom;
		}

		//Download file
		public ParamHttp(string _sURL, string _sToFile, dHttpComplete _dComplete,  dHttpComplete _dProgress, Object _oContainer, Object _oObj, dHttpComplete _dCustom, string _sAltURL = "",  string _sCustom= "", int _nCustom = 0) {
			bDownloadFile = true;
			sURL = _sURL;
			sToFile = _sToFile;
			dComplete = _dComplete;
			oContainer = _oContainer;
			oObj = _oObj;
			dCustom = _dCustom;
			sAltURL = _sAltURL;
			sCustom = _sCustom;
			nCustom = _nCustom;
			dProgress = _dProgress;
		}
		
		 public void fComplete(object sender, AsyncCompletedEventArgs e){
				dComplete(this);
		}


	}

	public  delegate void dHttpComplete(ParamHttp _sOut);


	public  class Http{
		static T GetNestedException<T>(Exception ex) where T : Exception{
			if (ex == null) { return null; }

			var tEx = ex as T;
			if (tEx != null) { return tEx; }

			return GetNestedException<T>(ex.InnerException);
		}
		
		//TODO USE WGET
		public static bool RemoteFileExists(string url){

		   bool result = true;
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create( url );
		   webRequest.Timeout = 1000; // miliseconds
		//   webRequest.Method = "HEAD";
		   webRequest.UserAgent = "Cwc - app"  ;
		   webRequest.Accept = "*/*";
		    HttpWebResponse response = null;
			try {
				response = (HttpWebResponse)webRequest.GetResponse();
				result = true;
			} catch (WebException webException) {
				//Debug.fTrace( webException.Message);
				result = false;
			}
			if (response != null) {
				response.Close();
			}
			Debug.fTrace("Is Remote file exist:" + url + " " +result );
		
			return result;
		}

		


		/// <summary>
		/// /////////// HTTP Request ////////////////////////
		/// </summary>
		 public static void fGetHttp(string _sURL, dHttpComplete _dComplete, Object _oContainer = null , Object _oObj = null, dHttpComplete _dCustom = null, string _sAltURL = "", string _sCustom = "", int _nCustom = 0)	{
				ParamHttp _oParam = new ParamHttp(_sURL, _dComplete,  _oContainer,  _oObj, _dCustom, _sAltURL, _sCustom, _nCustom);
				fGetHttp(_oParam);
		}
	 public static void fDownload(string _sURL, string _sToFile, dHttpComplete _dComplete,  dHttpComplete _dProgress, Object _oContainer = null , Object _oObj = null, dHttpComplete _dCustom = null, string _sAltURL = "", string _sCustom = "", int _nCustom = 0)	{
				ParamHttp _oParam = new ParamHttp(_sURL,_sToFile, _dComplete, _dProgress,  _oContainer,  _oObj, _dCustom, _sAltURL, _sCustom, _nCustom);
				fGetHttp(_oParam);
		}
		/// </summary>
		 public static void fGetHttp(string _sURL, ParamHttp _oParam)	{
				_oParam.sURL = _sURL;
				fGetHttp(_oParam);
		}


		 public static void fGetHttp(ParamHttp _oParam)	{
//if(_oParam.sURL[8] == 'a'){


		Console.WriteLine("------------------------fGetHttp : " +  _oParam.sURL);
		

	//if(!_oParam.bDownloadFile) {
	if(true) {

				LauchTool _oLauchUrl = new LauchTool();
				_oLauchUrl.dOut = new LauchTool.dIOut(fHttpOut);
				_oLauchUrl.dError = new LauchTool.dIError(fHttpOutInfo);

                _oLauchUrl.bHidden = true;
				_oLauchUrl.dExit = new LauchTool.dIExit(fUrlRequestComplete);
				_oLauchUrl.oCustom = (Object)_oParam;

		//_oLauchUrl.bRunInThread = false;
		//_oLauchUrl.bWaitEndForOutput = true;
					_oParam.sResult  = "";
	
				//	_oLauchUrl.fLauchExe( PathHelper.ToolDir + "curl/curl.exe", "-i  -k -L  -A \"Cwc - app\" \"" + _oParam.sURL + "\"");
					//_oLauchUrl.fLauchExe( PathHelper.ToolDir + "curl/curl.exe", "--anyauth -f -i  -k -L  -A \"Cwc - app\" \"" + _oParam.sURL + "\"");
					//_oLauchUrl.fLauchExe( PathHelper.ToolDir + "wget/wget.exe", "--no-check-certificate -S -O - \"" + _oParam.sURL + "\"");
				if(!_oParam.bDownloadFile) {
					_oLauchUrl.fLauchExe( PathHelper.ToolDir + "wget/wget.exe", "--no-check-certificate -S -O - \"" + _oParam.sURL + "\"");
				}else{
                  //    _oLauchUrl.dOut += new LauchTool.dIOut(fHttpConsole);
			      //  _oLauchUrl.dError += new LauchTool.dIError(fHttpConsole);

                    Console.WriteLine("------------------Download File !! ");
					_oLauchUrl.fLauchExe( PathHelper.ToolDir + "wget/wget.exe", "--no-check-certificate -O \"" +  _oParam.sToFile +  "\" \"" + _oParam.sURL + "\"");
				}
			//	

}else{
				
					BackgroundWorker oWorkHttp;
					oWorkHttp= new BackgroundWorker();
					oWorkHttp.WorkerSupportsCancellation = true;
					oWorkHttp.DoWork += new DoWorkEventHandler(fHttpRequestTagRelease);
					oWorkHttp.RunWorkerCompleted += new RunWorkerCompletedEventHandler(fHttpRequestComplete);
					oWorkHttp.RunWorkerAsync(_oParam );
				}
//}
		}


		public static void 	fHttpOutInfo(LauchTool _oTool, string _sOut){
	
			ParamHttp _oParam = (ParamHttp)_oTool.oCustom;
			//_oParam.sResult = _sOut;
			_oParam.sInfo += _sOut + "\n";
				//_oParam
			//Debug.fTrace("E: " + _sOut);

 //"Length:"
			if(_oParam.bDownloadFile) {
                int _nLimit = -1;
				if(_sOut.Length > 7 && _sOut[0] == 'L' && _sOut[6] == ':'){
					string _sLength = _sOut.Substring(8);
					//Debug.fTrace("_sLength: " + _sLength);
					string _sSubLength = _sLength;
					int _nEnd = _sSubLength.IndexOf(' ');
					if(_nEnd != -1){
						_sSubLength = _sSubLength.Substring(0, _nEnd);
					}
			
					int.TryParse( _sSubLength, out _nLimit);
				//	Debug.fTrace("--Total!! " + _nLimit);
				}
                _oParam.nTotalBytes = _nLimit;
	//	Debug.fTrace("--_sOut[_sOut.Length - 1 ]!! " + _sOut[_sOut.Length - 1 ]);
				if(_sOut[_sOut.Length - 1 ] == 's'){
					int _nEnd = _sOut.IndexOf('K');
					if(_nEnd != -1){
						string _sBytes = _sOut.Substring(0,_nEnd);
						int _nByte = -1;
						int.TryParse( _sBytes, out _nByte);
						_oParam.nBytes = _nByte*1000;
						//	Debug.fTrace("--nBytes!! " + _oParam.nBytes);
						fUrlProgress(_oParam);
					}
				}

			    // if(Data.oGuiConsole != null){ Data.oGuiConsole.Fctb.RemoveLines(aLineToDelete); aLineToDelete.Clear();}
                _sOut = _sOut.Trim();
                if(_sOut != "") {
                    if(_sOut.IndexOf("Cannot") >= 0 || _sOut.IndexOf("Bad") >= 0 ) {
                         Output.TraceError("\rDownload: " + _sOut);
                    }else {
                        Output.TraceAction("\rDownload: " + _sOut);
                    }
                }
                    
                // if(Data.oGuiConsole != null){ aLineToDelete.Add( Data.oGuiConsole.Fctb.LinesCount-1); }

				Debug.fRPrint("Download: " + _sOut + "                                                                                                 ");
			}
		////	_oParam.oHeader = _response.Headers;
		}


		public static void 	fHttpOut(LauchTool _oTool, string _sOut){
	
			ParamHttp _oParam = (ParamHttp)_oTool.oCustom;
			_oParam.sResult += _sOut;
		//	Debug.fTrace(": " + _sOut);

		//	_oParam.oHeader = _response.Headers;

			

		}
		public static void 	fUrlRequestComplete(LauchTool _oTool){
		
			
			ParamHttp _oParam =   (ParamHttp)_oTool.oCustom;
	//Console.WriteLine("------------------ Finish :\n"  +_oParam.sURL );

	//Console.WriteLine( _oParam.sResult );
	//Console.WriteLine("" );
	//Console.WriteLine("------------------" );
			if(_oParam.bDownloadFile) {


				if(_oParam.nTotalBytes > 0){
					_oParam.nBytes = _oParam.nTotalBytes;
                   
				}else{
					_oParam.nBytes *= 1.5; //Boost gain
				}
				fUrlProgress(_oParam); //100%
                Output.TraceGood("Downloaded: " + _oParam.sToFile);
			}
       
			///if(!_oParam.bDownloadFile) {
				_oParam.dComplete(_oParam);
			//}
		}







/*
	public static  void fProgress(Object sender, RunWorkerCompletedEventArgs e) {
			ParamHttp _oParam =   (ParamHttp)e.Result;
			_oParam.dComplete(_oParam);
		}*/



 public static void fUrlProgress(ParamHttp _oParam)  {

					try{
						
						//	double bytesIn = e.BytesReceived;
							//d//ouble totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
						//	double totalBytes = e.TotalBytesToReceive;
						//	double totalBytes = -1;

							double _nApMax = 104857600;
				
						//	_oParam.nBytes = bytesIn;
						//	_oParam.nTotalBytes= totalBytes;
						
						
							if(_oParam.nTotalBytes <= 0) { //Aproximative value
								double _nRatio =_oParam.nBytes/100000000.0;
								_oParam.nAprox +=  (  _oParam.nBytes - _oParam.nLastBytes) / ((_nRatio * _nRatio )  + 1  ) ;
								if( _oParam.nAprox  > _nApMax) {
									_oParam.nAprox  = 	_nApMax;	
								}
								_oParam.nPercentage = _oParam.nAprox * 97.0 / _nApMax; //Aprox Max was 90%
							
							} else {
								
								_oParam.nPercentage = _oParam.nBytes / _oParam.nTotalBytes * 100;
								
							}
							_oParam.nLastBytes = _oParam.nBytes;


			//		Debug.fTrace("pc" + _oParam.nPercentage + " bytesIn  " + bytesIn + " totalBytes " + totalBytes);

							_oParam.dProgress(_oParam);

					} catch (Exception ex)    {
							_oParam.bFail = true;
							_oParam.sFailMsg = ex.Message;
						}
	
	
		}





		  public static DownloadProgressChangedEventHandler fProgress(ParamHttp _oParam)  {
					Action<object, DownloadProgressChangedEventArgs> action = (sender, e) =>   {
					try{
						
							double bytesIn = e.BytesReceived;
							//d//ouble totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
							double totalBytes = e.TotalBytesToReceive;
						//	double totalBytes = -1;

							double _nApMax = 104857600;
				
							_oParam.nBytes = bytesIn;
							_oParam.nTotalBytes= totalBytes;
						
						
							if(totalBytes == -1) { //Aproximative value
								double _nRatio =_oParam.nBytes/100000000.0;
								_oParam.nAprox +=  (  _oParam.nBytes - _oParam.nLastBytes) / ((_nRatio * _nRatio )  + 1  ) ;
								if( _oParam.nAprox  > _nApMax) {
									_oParam.nAprox  = 	_nApMax;	
								}
								_oParam.nPercentage = _oParam.nAprox * 97.0 / _nApMax; //Aprox Max was 90%
							
							} else {
								
								_oParam.nPercentage = bytesIn / totalBytes * 100;
								
							}
							_oParam.nLastBytes = _oParam.nBytes;


			//		Debug.fTrace("pc" + _oParam.nPercentage + " bytesIn  " + bytesIn + " totalBytes " + totalBytes);

							_oParam.dProgress(_oParam);

					} catch (Exception ex)    {
							_oParam.bFail = true;
							_oParam.sFailMsg = ex.Message;
						}
	
				};
			 return new DownloadProgressChangedEventHandler(action);
		}


		public static void fHttpRequestTagRelease(Object sender, DoWorkEventArgs e)    {
			ParamHttp _oParam =   (ParamHttp)e.Argument;
			 e.Result = _oParam;

	//System.Net.ServicePointManager.ServerCertificateValidationCallback +=   (se, cert, chain, sslerror) => {  return true;};
	//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;



		if(_oParam.bDownloadFile) {
			  try {
                WebClient _oClient = new WebClient();
                _oClient.Proxy = null; //Important remove slow proxy
				
                _oClient.DownloadProgressChanged += fProgress(_oParam);
           		 _oClient.DownloadFileCompleted +=  _oParam.fComplete;
			
                _oClient.DownloadFileAsync(new Uri(_oParam.sURL),  _oParam.sToFile);
             //   _oClient.DownloadFile(new Uri(_oParam.sURL), _oParam.sToFile);
			
            }
            catch (Exception ex)    {
				_oParam.bFail = true;
				_oParam.sFailMsg = ex.Message;
            }
			return;
		}


		try {

			//onsole.WriteLine( _oParam.sURL);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( _oParam.sURL );
			request.UserAgent = "Cwc - app"  ;
			 request.Proxy = null; //Important
			request.Accept= "*/*";

//request.KeepAlive = false;

			WebResponse _response = null;
			Debug.fTrace("----GET URL ------ " +  _oParam.sURL);

			try {
				 _response = request.GetResponse();

			}catch (Exception ex){
				//Debug.fTrace("----asss ------ " + _response.Headers);
						//rogram.fDebug("----exr ------ " + ex.Message);
			
					var wex = GetNestedException<WebException>(ex);
						//rogram.fDebug("----wex ------ " + wex.Message);

					// If there is no nested WebException, re-throw the exception.
					if (wex == null) { throw; }

					// Get the response object.
					 _response = wex.Response as HttpWebResponse;
					_oParam.bFail = true;

					// If it's not an HTTP response or is not error 403, re-throw.
			   	   if (_response == null ) {// || _response.StatusCode != HttpStatusCode.Forbidden
						throw;
					}
				}

			Stream _stream = _response.GetResponseStream();
			StreamReader _reader = new StreamReader(_stream);
			_oParam.sResult = _reader.ReadToEnd();
			_oParam.oHeader = _response.Headers;
			//			Debug.fTrace("----_oParam.oHeader ------ " + _oParam.oHeader);

		//	Debug.fTrace("Header:" +  _response.Headers );

		   }  catch (Exception ex)  {
				_oParam.bFail = true;
				_oParam.sFailMsg = ex.Message;
            }

		  

		}
	
		public static  void fHttpRequestComplete(Object sender, RunWorkerCompletedEventArgs e) {

			ParamHttp _oParam =   (ParamHttp)e.Result;
			if(!_oParam.bDownloadFile) {
				_oParam.dComplete(_oParam);
			}
		}
	

/// <summary>
/// //////////////////////////////////////////////////////////////////////
/// </summary>





/*
 * 
* 
https://raw.githubusercontent.com/Maeiky/LibRT/master/README.md
 https://api.github.com/repos/Maeiky/LibRT/releases/tags/0.0.2 
https://api.github.com/repos/Maeiky/LibRT/tags

* 
"https://github.com/Maeiky/LibRT/releases/download/0.0.2/LibRT_x64_0.0.2.7z"
tarball_url	"https://api.github.com/repos/Maeiky/LibRT/tarball/0.0.2"
zipball_url	"https://api.github.com/repos/Maeiky/LibRT/zipball/0.0.2"
*/

//https://api.github.com/repos/DigitalPulseSoftware/NazaraEngine/tags
///"https://api.github.com/repos/Maeiky/LibRT/tarball/0.0.2"

//https://api.github.com/repos/DigitalPulseSoftware/NazaraEngine/releases
		//https://api.github.com/re…eiky/LibRT/tarball/0.0.2

//https://api.github.com/repos/DigitalPulseSoftware/NazaraEngine/tags



		public static int nLastIndex = 0;
		public static string fBetween(string _sData,  string _sBegin, string _sEnd, int _nStartIndex = 0, bool _bIncludeBegin = false, bool _bIncludeEnd = false) {
			string _sResult = "";
			if(_sData == null || _sData == ""){
				return "";
			}
			if(_nStartIndex < 0) {_nStartIndex = 0;}
			int _nIndexB = _nStartIndex;
			if(_sBegin != "") {
				 _nIndexB = _sData.IndexOf(_sBegin,_nStartIndex);
			}
			if(_nIndexB >= 0) {
				if(!_bIncludeBegin) {
					_nIndexB += _sBegin.Length;
				}

				nLastIndex = _sData.IndexOf(_sEnd,_nIndexB);if(nLastIndex > 0) {
					if(_bIncludeEnd) {	
						nLastIndex += _sEnd.Length;
					}
					_sResult = _sData.Substring(_nIndexB, nLastIndex - _nIndexB);
				}
			}

			return _sResult;
		}
		


		public static void fGetProject(ModuleData _oData, string _sData) {


			//string _sButton =  fBetween(_sData, "<div class=\"container repohead", "<h1 class=\"public" , 0,true,false);
			string _sButton =  fBetween(_sData, "<ul class=\"pagehead-actions\">", "</ul>",0 ,true,false);
		
			if(_sButton != ""){
				_sButton = "<div style=\"left:600px;top:-8px;position:absolute; \">"  + _sButton + "</div>";
				_sButton = _sButton.Replace("<li>","<li style=\"float:left;padding:5px;list-style-type:none;\">");
			}


			string _sBody = "";
			string _sEnd = "";
			string _sHead =  fBetween(_sData, "<head>", "</head>" , 0,true,true);

			_sHead += "<div class='markdown-body'>";
			_sEnd += "</div>";

			_sBody=  fBetween(_sData, "<div id=\"readme\"", "</div>" ,nLastIndex,true, true).Trim();
			int _nIndex = 	_sBody.IndexOf("</h3>"); //Remove readme title
			if(_nIndex != -1) {
					_sBody= _sBody.Substring(_nIndex);
			}
			_oData.sReadme = _sButton + _sHead   + _sBody +  _sEnd ;
//Debug.fTrace("-----------!!!!");
			//Find Licence link
			int _nIndexLaw = _sData.IndexOf("octicon-law");
			if(_nIndexLaw != -1) {
				string _sDataLink = _sData.Substring(0, _nIndexLaw);
				int _nIndexLawLink = _sDataLink.LastIndexOf("href=\"");
				if(_nIndexLawLink != -1) {
					string _sLink = fBetween(_sDataLink, "", "\">", _nIndexLawLink + 6);
					if(_sLink != "") {
						_oData.sUrl_Licence = _sLink;
						//Debug.fTrace("LinkFound! " + _oData.sUrl_Licence );
					}
				}
			}
			//_oData.sReadme = _sData;



		}


		public static void fGetLicence(ModuleData _oData, string _sData) {
			string _sBody = "";
			string _sEnd = "";
			string _sHead =  fBetween(_sData, "<head>", "</head>" , 0,true,true);
			if(_sHead == ""){
				return; //no licences
			}			

			string _sLicenceBox = "</br>" + fBetween(_sData, "<div class=\"Box mb-3 clearfix\">", "<div class=\"commit-tease\"" , nLastIndex,true,false);

			int _nRemovePart = _sLicenceBox.IndexOf("<p class=\"text-gray "); //Remove advice
	//	Debug.fTrace("_nRemovePart " +_nRemovePart );
			if(_nRemovePart > 0) {	
		//		Debug.fTrace("_nRemovePart " +_nRemovePart );
				_sLicenceBox =  _sLicenceBox.Substring(0, _nRemovePart) + "<p></p></div>";
			}
//int _nRemovePart = _sLicenceBox.IndexOf("This is not legal advice"); //Remove advice
			//Debug.fTrace(_sLicenceBox);
		//	_sHead += "<div class='markdown-body'>";
		//	_sEnd += "</div>";

			_sBody=  fBetween(_sData, "class=\"blob-wrapper data type-text\">", "</div>" ,nLastIndex,false, false).Trim();
		
			_oData.sLicence = _sHead + _sLicenceBox + _sBody + _sEnd;

	//		Debug.fTrace(_sBody);
		}


		public static Release fGetRelease(string _sData) {



			string _sTitle =  fBetween(_sData, "<div class=\"release-header\">", "</div>" ,0,true,true);
	

			///////// Metod 1  /////////
			string _sEnd = "";
			string _sHead =  fBetween(_sData, "<head>", "</head>" , 0,true,true);



			_sHead += "<div class='markdown-body'>";
			_sEnd += "</div>";


				Release _oRelease = new Release();

				_oRelease.body = _sHead + _sTitle + "<h1></h1><div>" + fBetween(_sData, "\"markdown-body\">", "</div>" ,nLastIndex,false, true).Trim();
				_oRelease.download = fBetween(_sData, "", "</div>" , nLastIndex).Trim();
				_oRelease.body += _oRelease.download ;




				_oRelease.body += _sEnd;

				//Console.Write(_oRelease.body);
				
			//	_oRelease.body = _sData;
				
				return  _oRelease;


		}



/*
		public static void fGetRelease(string _sData) {
			
			
				Tags _aReleases = JSON.From<Tags>( _sData);
				foreach(Tag _oRelease in _aReleases) {
					Debug.fTrace("----------------- " );
					Debug.fTrace("JSON name: " + _oRelease.name);
					Commit _Commit = 
					Debug.fTrace("JSON tag_name: " + _oRelease.tag_name);
					
				
				}
		}*/


	//	public static Tags aTags = new Tags() ;d

		public static void fGetAllTag(Tags _oContainer, string _sURL, dHttpComplete _dFinish, string _sAltURL, string _sRecommendedVer){
		//	Debug.fTrace("getAll Tags");
			Http.fGetHttp( _sURL, fGetTagData, _oContainer,null, _dFinish, _sAltURL, _sRecommendedVer);
		}
		
		public static void fGetTagData(ParamHttp _oData) {
			

			if(_oData.nCustom == 0) {
				fGetTagDataAPI(_oData);
			}else {
				Debug.fTrace("---!!!***fGetTagDataALT !! " + _oData.nCustom  );
				fGetTagDataALT(_oData);
			}

		}



		public static void fGetTagDataAPI(ParamHttp _oData) {
	
			//	Debug.fTrace("sInfo" + _oData.sInfo);
				 string _sLimit = "";
				if(_oData.oHeader != null) {   //C# http method
					 _sLimit = _oData.oHeader["X-RateLimit-Remaining"];
				}else{
					_sLimit = fBetween(_oData.sInfo, "X-RateLimit-Remaining:" , "\n");
				}
			
				if(_sLimit != ""){
					int _nLimit = -1;
					int.TryParse( _sLimit, out _nLimit);
					if(_nLimit == 0) {	
						Debug.fTrace("No API access remain, try alt method... for " + _oData.sURL );
						if(_oData.sCustom != "") {
                          Debug.fTrace("Try the alternative method with recommend ver :" + _oData.sAltURL + "?after=" + _oData.sCustom );
							_oData.nCustom = 1; //Try the alternative method with recommend ver
							Http.fGetHttp( _oData.sAltURL + "?after=" + _oData.sCustom, _oData);
						}else {
                           Debug.fTrace("Try the alternative method & Just get last version: " +  _oData.sAltURL );
							_oData.nCustom = 4; //Try the alternative method & Just get last version
							Http.fGetHttp( _oData.sAltURL, _oData); 
						}
						return;
					}
				}
					
				
				/*
				if(_oData.bFail) {
					Debug.fTrace("Fail: " + _oData.sFailMsg + " " + _oData.sURL );
					if(_oData.oHeader != null) {
						int _nLimit = -1;
						int.TryParse( _oData.oHeader["X-RateLimit-Remaining"], out _nLimit);
						if(_nLimit == 0) {	
							Debug.fTrace("No API access remain, try alt method..." );
						}
					}
					if(_oData.sCustom != "") {
						_oData.nCustom = 1; //Try the alternative method with recommend ver
						Http.fGetHttp( _oData.sAltURL + "?after=" + _oData.sCustom, _oData);
					}else {
						_oData.nCustom = 4; //Try the alternative method & Just get last version
						Http.fGetHttp( _oData.sAltURL, _oData); 
					}

					return;
				}*/


				Tags _aTags = (Tags)_oData.oContainer;
				Tags _aNewTags;

			//	Debug.fTrace("fGetTagData " + _oData.sResult);

				
				 //_aNewTags = fExtractTags_API(_oData.sResult) ;
				 _aNewTags = fExtractTags_API( fBetween( _oData.sResult, "[", "]",0,true,true )  ) ;
			
			
				if(_aNewTags != null){
					
					_aTags.AddRange(_aNewTags);
				}else{
					Debug.fTrace("No new TAG " );
				}
//Console.WriteLine("assssssssssss");

			
	//			Debug.fTrace("--Count: " + _aNewTags.Count.ToString());
	//			Debug.fTrace("--last: " + _aNewTags[_aNewTags.Count-1].name);

			//	string _sNext = oPa oHeader
				string _sAllLink =  "";
				if(_oData.oHeader != null) {   //C# http method
					 _sAllLink = _oData.oHeader["Link"];
				}else{
					_sAllLink = fBetween(_oData.sInfo, "Link:" , "\n");
				}

		//		string _sModified = _oData.oHeader["Last-Modified"];
			//	int _nRemain = _oData.oHeader["X-RateLimit-Limit"];

				string _sNextLink = "";
				if(!FileUtils.IsEmpty(_sAllLink)) {
					string[] _aLink =  _sAllLink.Split(',');
					foreach (string _sLink in _aLink) {	
						//Debug.fTrace("Link: " + _sLink);
						string[] _aLinkData =  _sLink.Split(';');
						if(_aLinkData[1].Trim() == "rel=\"next\"") {
							_sNextLink = fBetween(_aLinkData[0], "<", ">");
							if(_sNextLink == "") {//If fail
								_sNextLink =  _aLinkData[0].Trim();
							}
						}	
					}
				}
				
		
				if(_sNextLink != "") {
				 	Http.fGetHttp( _sNextLink, _oData);
				}else {
					_oData.dCustom(_oData); //Finish();
				}

				
				/*
				//?after=v4.4.3
				if(_aNewTags.Count == 10) { ///Have more -> get other
					string _sNewURL = _oData.sAltURL + "?after=" +  _aNewTags[_aNewTags.Count-1].name;
					Debug.fTrace("_sNewURL" + _sNewURL);
				 	JSON.fGetHttp( _sNewURL, _oData);
				}else {
					_oData.dCustom(_oData); //Finish();
				}*/
				///_oData.dCustom(_oData); //Finish();
		}	
		
		public static void fGetTagDataALT(ParamHttp _oData) {
			Tags _aTags = (Tags)_oData.oContainer;
			Tags _aNewTags;

			_aNewTags = fExtractTags_STD(_oData.sResult);

			switch(_oData.nCustom ) {
				case 1: { //1 = Ver after recommended version -> then get recommended
					string _sPreviousLink = fBetween(_oData.sResult, "\"pagination\"><a href=\"", "\"" );
					_oData.nCustom = 2;
					_aTags.AddRange(_aNewTags);

					//Debug.fTrace("!_sPreviousLink!! " + _sPreviousLink);
					Http.fGetHttp(_sPreviousLink, _oData);
				}break;

				case 2: {//2 = Ver with recommended version -> then get most recent
				
					_oData.nCustom = 3;
					//_aTags.InsertRange(0,_aNewTags);

					string _sFistTag = _aTags[0].name;
					int i =0;
					foreach(Tag _oTag in _aNewTags) {
						if(_oTag.name != _sFistTag) {
							_aTags.Insert(i, _oTag );
							i++;
						}else {
							break;
						}
					}

					//Debug.fTrace("---Test1: " + _oData.sAltURL );
					//Debug.fTrace("---Test2: " + _oData.sURL );
					if( _oData.sURL !=  _oData.sAltURL ) {
						Http.fGetHttp(_oData.sAltURL, _oData); //Get most recent ver
					}else{
						//Already got, stop here

						_oData.dCustom(_oData); //Finish();
					}
					
				}break;

				case 3: {//3 = Get most recent date add but don't double it
					//_aTags.Insert();
	
					string _sFistTag = _aTags[0].name;
					int i =0;
					foreach(Tag _oTag in _aNewTags) {
						if(_oTag.name != _sFistTag) {
							_aTags.Insert(i, _oTag );
							i++;
						}else {
							break;
						}
					}
					_oData.dCustom(_oData); //Finish();
	
				}break;



				default:
					_aTags.AddRange(_aNewTags);
					_oData.dCustom(_oData); //Finish();
				break;

			}			
		}

/*
		public static Tags fExtractTags(string _sData) {
		}*/


		public static Tags fExtractTags_API(string _sData) {
			//	Debug.fTrace("-------_sData----  "  + _sData );

			if(_sData == ""){
				return null;
			}

			Tags _aTags = Http.From<Tags>( _sData);
				/*
				foreach(Tag _oTag in _aTags) {

					Debug.fTrace("----------------- " );
					Debug.fTrace("Tag name: " + _oTag.name);
					Debug.fTrace("zipball_url: " + _oTag.zipball_url);
					Commit _oCommit = _oTag.commit;
					Debug.fTrace("_oCommit_Sha: " + _oCommit.sha);
					Debug.fTrace("_oCommit_url: " + _oCommit.url);
				}*/
			return _aTags;
		}


	/// /////
		public static Tags fExtractTags_STD(string _sData) {
			Tags _aTags = new Tags() ;

                   Debug.fTrace(_sData);
            /*
			string _sSearchTag = "=\"date\">";
				string _sSearchTagEnd = "</a>";
                */
                string _sSearchTag = "archive/";
				string _sSearchTagEnd = "\"";
  
				int _nCurrIndex = _sData.IndexOf( _sSearchTag , 0);

				while(_nCurrIndex >= 0) {
					_nCurrIndex += _sSearchTag.Length;

					int _nStartIndex = _nCurrIndex;
                    
					_nCurrIndex = _sData.IndexOf( _sSearchTagEnd , _nStartIndex);

					if(_nCurrIndex < 0) {	break;}
                     
                    string _sFound = _sData.Substring(_nStartIndex, _nCurrIndex - _nStartIndex);
                
                     int _nExtIndex=_sFound.IndexOf(".zip"); //Not sure
                     if (_nExtIndex> 0) { ///
                        _sFound = _sFound.Substring(0,_nExtIndex).Trim(); //Not sure
					 //   Tag _oTag = fExtractTag_STD_(_sFound);

			            Tag _oTag = new Tag();
			            _oTag.name = _sFound;
			           // _oTag.date = _sDate;
			           // _oTag.userDate = _sUserDate;
			            //_oTag.link = _sLink;  //TODO

					    _aTags.Add(_oTag);
					    Debug.fTrace("New Tag: " + _oTag.name);
			
                     }  
					_nCurrIndex = _sData.IndexOf( _sSearchTag , _nCurrIndex);
				};

	
			
			return _aTags;
		}

		public static Tag fExtractTag_STD_(string _sData  ) {


			string _sSearchLink = "href=\"";
			string _sSearchLinkEnd = "\">";
			string _sLink = "";

			//Get link
			 int _nIndexLink = _sData.IndexOf(_sSearchLink);if(_nIndexLink > 0) {_nIndexLink += _sSearchLink.Length;
				int _nIndexLinkEnd = _sData.IndexOf(_sSearchLinkEnd,_nIndexLink);if(_nIndexLink > 0) {
					if(_sData[_nIndexLink] == '/') {
						_nIndexLink++;
					}
					_sLink = _sData.Substring(_nIndexLink, _nIndexLinkEnd - _nIndexLink);
				}
			}
			
			//Get Version
			string _sVersionName = "";
			if(_sLink != "") {
					_sVersionName = Path.GetFileName(_sLink);
			}
			
			//Get Date
			string _sSearchDate = "datetime=\"";
			string _sSearchDateEnd = "\">";
			string _sDate= "";
			int _nIndexDateEnd = 0;
			 int _nIndexDate = _sData.IndexOf(_sSearchDate);if(_nIndexDate > 0) {_nIndexDate += _sSearchDate.Length;
				 _nIndexDateEnd = _sData.IndexOf(_sSearchDateEnd,_nIndexDate);if(_nIndexDate > 0) {
					_sDate = _sData.Substring(_nIndexDate, _nIndexDateEnd - _nIndexDate);
				}
			}
			
			//Get user date
			string _sUserDate= "";
			if(_nIndexDateEnd > 0) {
				_nIndexDateEnd += _sSearchDateEnd.Length;
				string _sSearchUserDateEnd = "</";
				int _nIndexUserDateEnd = _sData.IndexOf(_sSearchUserDateEnd,_nIndexDateEnd);if(_nIndexUserDateEnd > 0) {
					_sUserDate =  _sData.Substring(_nIndexDateEnd, _nIndexUserDateEnd - _nIndexDateEnd);
				}
			}
			

			Tag _oTag = new Tag();
			_oTag.name = _sVersionName;
			_oTag.date = _sDate;
			_oTag.userDate = _sUserDate;
			_oTag.link = _sLink;

			return _oTag;
			/*
			Debug.fTrace("--Tag:-- " +_sData);
			Debug.fTrace("--_sLink:-- " +_sLink);
			Debug.fTrace("--_sVersion:-- " +_sVersionName);
			Debug.fTrace("--_sDate:-- " +_sDate);
			Debug.fTrace("--_sUserDate:-- " +_sUserDate);
			
			Debug.fTrace("------- ");*/
			
		}


/*
		
		 public static void fTest() {
							* /// Sample code using the above helper methods
			/// to serialize and deserialize the Release object
 
			Release myPerson = new Release("Chris", "Pietschmann");
 
			// Serialize
			string json = JSON.Serialize<Release>(myPerson);
 
			// Deserialize
			myPerson = JSON.Deserialize<Release>(json);
		}


		*/
		 public static string To<T>(T obj)
			{
				string retVal = null;
				System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
				using (MemoryStream ms = new MemoryStream())
				{
				 serializer.WriteObject(ms, obj);
				 retVal = Encoding.Default.GetString(ms.ToArray());
				}

				return retVal;
			}

			public static T From<T>(string json)
			{
				T obj = Activator.CreateInstance<T>();
				using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
				{
				 System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
				 obj = (T)serializer.ReadObject(ms);
				}

				return obj;
			}



/*
 * 
* /// Sample code using the above helper methods
/// to serialize and deserialize the Release object
 
Release myPerson = new Release("Chris", "Pietschmann");
 
// Serialize
string json = JSONHelper.Serialize<Release>(myPerson);
 
// Deserialize
myPerson = JSONHelper.Deserialize<Release>(json);
*/










	}
}
