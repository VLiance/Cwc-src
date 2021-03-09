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

		public NamedPipes(string _server="localhost", string _name="psexecsvc")
        {
			Thread winThread = new Thread(new ThreadStart(() =>  {  

			try { 
				using (var pipe = new NamedPipeClientStream(_server, _name, PipeDirection.InOut))
				{
					string _sResult = "";
					pipe.Connect(1000);
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
			}
			}));  
			winThread.Start();
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
