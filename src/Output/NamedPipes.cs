﻿using System;
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
					try {
					//pipe.removepipe();
					//	pipe.pipe  = new NamedPipeClientStream("localhost", "test", PipeDirection.InOut);
					
						//pipe.pipe.Dispose();
						pipe.pipe.Close();
						pipe.dataToSend = _data;
						Thread.Sleep(1);
						pipe.newpipe();

					}catch(Exception e) {
						Output.TraceError("assole");
					}
				

					//pipe.dataToSend = _data;
					//pipe.Send( _data);
				}
			}

		}

		NamedPipeClientStream pipe = null;

		public string result = "";
		public string name = "";
		public string server = "";
		public string dataToSend = "";
		



		public void newpipe() {
			Thread winThread = new Thread(new ThreadStart(() =>  {  
			try { 
				pipe = new NamedPipeClientStream(server, name, PipeDirection.InOut, PipeOptions.Asynchronous);
				while(true) {
					string _sResult = "";
					//pipe.Connect(3000);
					pipe.Connect();
					Output.TraceActionLite("Pipe " + name + " connected");
					Data.bIWantGoToEnd = true;

					pipe.ReadMode = PipeTransmissionMode.Message;
					do
					{
						if(dataToSend != "") {
							Send(dataToSend);
							dataToSend = "";
						}
						
						ReadMessage(pipe);
							
					} while (pipe.IsConnected);
					Output.TraceErrorLite("Pipe " + name + " disconnected");
				}

			}catch(Exception e) {
				Output.TraceError(e.Message);
				removepipe();
			}
			}));  
			winThread.Start();

		}

		public NamedPipes(string _server="localhost", string _name="cwc_pipe")
        {
		
			name = _name;
			server = _server;

			aPipeList.Add(this);
			LauchTool.bListModified = true;

			newpipe();
			
        }

		public void Send(string _data) {
			if(pipe.IsConnected) {
				Output.TraceAction("[:" +name +"] "  + _data);
				_data += "\n\r";
				//Thread winThread = new Thread(new ThreadStart(() =>  {  
					try {
					//	cts.Cancel();
						byte[] bytes = Encoding.Default.GetBytes(_data);
						pipe.Write(bytes, 0, bytes.Length);

					}catch(Exception e) {Output.TraceError(e.Message);}

				//}));  
				//winThread.Start();
			}else {
				Output.TraceError("Not connected");
			}
		}

		public void removepipe() {
			//Remove this pipe
			aPipeList.Remove(this);
			LauchTool.bListModified = true;

		}

        private async void ReadMessage(PipeStream pipe)
        {

			try { 
				 byte[] buffer = new byte[1024 *8];


				var ms = new MemoryStream();
				//int readBytes = await pipe.ReadAsync(buffer, 0, buffer.Length);
				int readBytes = pipe.Read(buffer, 0, buffer.Length);
				 ms.Write(buffer, 0, readBytes);
				result += Encoding.UTF8.GetString( ms.ToArray());


				if(result != "" && result.IndexOf("\n") != -1) {
					Output.Trace(result);
					result = "";
				}

			}catch(Exception e) {
				Output.TraceError(e.Message);
			}
			Thread.Sleep(1);


        }


	}
}
