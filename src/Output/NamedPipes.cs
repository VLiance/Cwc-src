using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cwc {
	class NamedPipes {

		public static List<NamedPipes> aPipeList = new List<NamedPipes>();

		public static void Send(string _name, string _data) {
			if(_name.Length <= 0) { return; }

			string _realname = _name;
			if(_name[0] == ':') {//namedpipe
				_realname = _name.Substring(1);
			}

			foreach(NamedPipes pipe in aPipeList ) {
				if(pipe.name == _realname) {
					pipe.Send(_data);
				}
			}

		}


		NamedPipeClientStream pipe = null;
		public string name = "";
		public string server = "";

		public NamedPipes(string _server="localhost", string _name="cwc_pipe")
        {
			name = _name;
			server = _server;

			aPipeList.Add(this);
			LauchTool.bListModified = true;

			Thread winThread = new Thread(new ThreadStart(() =>  {  

			try { 
				using (pipe = new NamedPipeClientStream(_server, _name, PipeDirection.InOut))
				{
					string _sResult = "";
					//pipe.Connect(3000);
					pipe.Connect();
					pipe.ReadMode = PipeTransmissionMode.Message;
					do
					{
						var result = ReadMessage(pipe);

						_sResult += Encoding.UTF8.GetString(result);
						if(_sResult.IndexOf("\n") != -1) {
							Output.Trace(_sResult);
							_sResult = "";

							//  byte[] bytes = Encoding.Default.GetBytes("ass");
							// pipe.Write(bytes, 0, bytes.Length);
						}
					} while (true);
				}


			}catch(Exception e) {
				Output.TraceError(e.Message);
				removepipe();
			}
			}));  
			winThread.Start();
        }

		public void Send(string _data) {
			if(pipe.IsConnected) {
				byte[] bytes = Encoding.Default.GetBytes(_data);
				pipe.Write(bytes, 0, bytes.Length);
			}else {
				Output.TraceError("Not connected");
			}
		}

		public void removepipe() {
			//Remove this pipe
			aPipeList.Remove(this);
			LauchTool.bListModified = true;
			/*
			List<NamedPipes> _aTempPipeList = new List<NamedPipes>();
			foreach(NamedPipes pipe in aPipeList)  {
				if(pipe != this) {
					_aTempPipeList.Add(pipe);
				}
			}
			aPipeList = _aTempPipeList;
			*/
		}


        private static byte[] ReadMessage(PipeStream pipe)
        {
			int  readBytes;
            byte[] buffer = new byte[1024 * 4];
            using (var ms = new MemoryStream())
            {
                do
                {
                    readBytes = pipe.Read(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, readBytes);
                }
                while (readBytes > 0 && !pipe.IsMessageComplete);

                return ms.ToArray();
            }
        }


	}
}
