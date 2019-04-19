using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using static cwc.Config;

namespace cwc
{

	[Serializable()]
	public class Config : FileUtils
	{

		public class oStringPair{
           public  oStringPair(string _sKey, string _sValue) {
                sKey = _sKey;
                sValue = _sValue;
            }
            public string sKey = "";
            public string sValue= "";
        };


		public Point vStartPos;
		public Size vStartSize;
		public Point vTest;
        public oStringPair[] aOption;

        public bool bTreePrjOpen = false;
        public bool bMaximize ;

    	public List<string> aRecent;

        internal void fAddRecent(string _sPath) {
           _sPath = PathHelper.fNormalizePath(_sPath);
            int _nCount = 0;
            foreach (string _sRecent in aRecent) {
                if (_sPath == _sRecent) {
                    aRecent.RemoveAt(_nCount);
                    break;
                }
                _nCount++;
            }

        
            aRecent.Insert(0,_sPath);
         
            if (aRecent.Count>20) {//Max
                aRecent.RemoveAt(aRecent.Count - 1);
            }

            if (Data.oGuiConsole != null && Data.oGuiConsole.bCreated) {
                Data.oGuiConsole.fLoadRecent();
            }

        }

        /*
                public Point StartPos
                {
                    get { return vStartPos; }
                    set { vStartPos = value; }
                }

                public Size StartSize
                {
                    get { return vStartSize; }
                    set { vStartSize = value; }
                }*/
    }


	public class ConfigMng
	{
		public static Config oConfig = new Config();
		public static bool bNewConfig = false;
		public static bool bLoadFailed = false;
		private string m_sConfigFileName =   PathHelper.GetExeDirectory() + "cwc.xml";
	

		public Config Config
		{
			get { return oConfig; }
			set { oConfig = value; }
		}

		// Load configuration file
		public void LoadConfig(){

			//Console.WriteLine("Load Config!! "+ m_sConfigFileName);
			try{
				if (File.Exists(m_sConfigFileName)){
			
					StreamReader srReader = File.OpenText(m_sConfigFileName);
					Type tType = oConfig.GetType();
                    //	XmlSerializer xsSerializer = XmlSerializer.FromTypes(new[]{tType})[0];
					XmlSerializer xsSerializer = new XmlSerializer(tType);
					object oData = xsSerializer.Deserialize(srReader);
					oConfig = (Config)oData;

					srReader.Close();
       
					return;
				}
				
			}catch(Exception e){
                bLoadFailed = true;
               };

            if (oConfig.vStartSize.Height == 0 && oConfig.vStartSize.Width == 0) {
                  bLoadFailed = true;
            }

			bNewConfig = true;
            //oConfig.vStartPos = new Point();
            // oConfig.vStartSize = new Size();
            if (oConfig.aRecent == null) {
                oConfig.aRecent = new List<string>();
            }

            if(oConfig.aOption == null){
                oConfig.aOption  = new oStringPair[0];
            }
            foreach (oStringPair _oVal in oConfig.aOption) {
                Data.aOption[_oVal.sKey] = _oVal.sValue;
            }

		}

		// Save configuration file
		public void SaveConfig()
		{


            oConfig.aOption = new oStringPair[Data.aOption.Count];
            int i =0;
            foreach (KeyValuePair<string, string> _oKey in Data.aOption) {
                oConfig.aOption[i] = new oStringPair(_oKey.Key, _oKey.Value);
                i++;
            }

			try{
             //  oConfig.aOption =  ;
             

				StreamWriter swWriter = File.CreateText(m_sConfigFileName);
				Type tType = oConfig.GetType();
				if (tType.IsSerializable)
				{
                 //   XmlSerializer xsSerializer = XmlSerializer.FromTypes(new[]{tType})[0];
					XmlSerializer xsSerializer = new XmlSerializer(tType);
					xsSerializer.Serialize(swWriter, oConfig);
					swWriter.Close();
                     
				}
			}catch(Exception e){};
		}
	}
}
