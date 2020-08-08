using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace cwc {
    public class DBGpClient {

       public  static List<Breakpoint> aBreakpoint = new List<Breakpoint>();

        public class Breakpoint {
             public int nID = 0;
             public string sType= ""; //line/call/return/exception/conditional/watch

            public string sFunction= "";//Function name for call or return type breakpoints.
            public string sPath= "";
            public string sValue= "";
            public int nLine = 0;

            public bool bEnable = false; //state
            public bool bTemporary = false; //Deleted after its first use. 
            public bool bResolved = false;

            public int nHitCount = 0;
            public int nHitValue = 0;
            public string sHitCondition = ">="; //>= (break if hit_count is greater than or equal to hit_value [default]) / == (break if hit_count is equal to hit_value) / % (break if hit_count is a multiple of hit_value)
            public string sExeption = ""; //Exception name for exception type breakpoints.
            public string sExpression = ""; //The expression used for conditional and watch type breakpoints
        }

         public class Var {
            public string sName= "";
            public string sValue= "";//TODO More Complex way
            public string sType= "";
            public int nChildPropertiesCount = 0;
            public bool bHaveChildren = false;

        }
        public class Frame {
            public int nStackLevel = 0;
            public Boolean bUnknowName = false;
            public string sAdress = "";
            public string sFuncName = "";
            public string sFuncParam = "";
            public string sFile= "";
            public int nLine= 0;
            public  List<Var> aVar = new List<Var>();
        }


private const bool SHOW_MESSAGES = false;

		private readonly int nPort;
		private readonly Object oMdbgProcessLock = new Object();
		private bool bDetaching = false;

		private String _steppingCommand;
		private String _steppingTransId;
		private WaitHandle _stepWait = null;
		private String sMessageBuffer = "";

		private int nMaxChildren = 100;
		private int nMaxData = 3000;
		private int nMaxDepth = 1;

		private Socket oSocket;
		//private MDbgProcess oMdbgProcess = null;


       
        public void fLoadBreakpoints() {
        //    fIniFile("", false);//TODO merge with cmMake settings?

        }
       
        public void fSaveBreakpoints() {


        }
    



        public virtual void fIni() {}
      //  public virtual void Ini() {}
       // public virtual void fBreakpointSet(string _sType, string _sFile, int _nLine, bool _bEnabled, out int __nID, out bool __bEnabled, out bool _bResolved) {__nID=-1;__bEnabled=true;_bResolved = false}
        public virtual void fBreakpointSet(Breakpoint _oBreakpoint) {}
        public virtual void fBreakpointRemove(Breakpoint _oBreakpoint) {}

        public virtual List<Frame> fStackGet() {return null;}

        public virtual string fPropertyGet(string _sExp){return null;}


		public DBGpClient() {
            int port = 9000;
			nPort = port;
		}

		public void Start() {

			new Thread(() => {

			    var ip = IPAddress.Loopback;
			    var ipEndPoint = new IPEndPoint(ip, nPort);


                while(Base.bAlive){
                    try{
                        Thread.Sleep(1000);
                        if(oSocket == null) {
                             oSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        }
			            oSocket.Connect(ipEndPoint);
                        this.Run();
                  
                    }catch (Exception e) {}
                }

			
			}).Start();
		}

		public void Run() {
			try {
                /*
				var engine = new MDbgEngine();
				_mdbgProcess = engine.Attach(_pid, VersionPolicy.GetDefaultAttachVersion(_pid));
				_mdbgProcess.AsyncStop().WaitOne();

				Action<IRuntimeModule> processModule = (IRuntimeModule module) => {
					var managedModule = module as ManagedModule;
					if (managedModule != null && managedModule.SymReader != null) {
						if (!managedModule.CorModule.JITCompilerFlags.HasFlag(CorDebugJITCompilerFlags.CORDEBUG_JIT_DISABLE_OPTIMIZATION)) {
							return;
						}

						managedModule.CorModule.SetJmcStatus(true, new int[0]);
					}
				};

				Action<IRuntime> processRuntime = (IRuntime runtime) => {
					runtime.ModuleLoaded += (Object sender, RuntimeModuleEventArgs args) => {
						processModule(args.Module);
					};
					var managedRuntime = (ManagedRuntime)runtime;

					IList<MDbgModule> modules = null;
					while(true) {
						try {
							modules = _mdbgProcess.Modules.ToList();
							break;
						} catch (InvalidOperationException) {}
					}
					foreach(var module in modules) {
						foreach(var managedModule in managedRuntime.Modules.LookupAll(module.FriendlyName, true)) {
							processModule(managedModule);
						}
					}
				};
				_mdbgProcess.Runtimes.RuntimeAdded += (Object sender, RuntimeLoadEventArgs runTimeArgs) => {
					processRuntime(runTimeArgs.Runtime);
				};
				foreach(var runtime in _mdbgProcess.Runtimes) {
					processRuntime(runtime);
				}
                       
				var sourcePosition = !_mdbgProcess.Threads.HaveActive || !_mdbgProcess.Threads.Active.HaveCurrentFrame ? null : _mdbgProcess.Threads.Active.CurrentSourcePosition;
                */



                fIni();

				//_socket.Send(Encoding.UTF8.GetBytes(this.GenerateOutputMessage(this.InitXml(sourcePosition != null ? sourcePosition.Path : null))));
                 oSocket.Send(Encoding.UTF8.GetBytes(this.GenerateOutputMessage(this.InitXml(""))));
                  
                /*
				Console.CancelKeyPress += delegate {
					Console.Write("Exiting...");
					this.Detach();
					System.Environment.Exit(-1);
				};*/

				var _aSocketBuffer = new byte[4096];
				var _oReceiveToken = oSocket.BeginReceive(_aSocketBuffer, 0, _aSocketBuffer.Length, SocketFlags.None, null, null);
				while(Base.bAlive) {
                    Thread.Sleep(1);
					var _aWaitArray = _stepWait != null ? new WaitHandle[] { _oReceiveToken.AsyncWaitHandle, _stepWait } : new WaitHandle[] { _oReceiveToken.AsyncWaitHandle };
					var _nWaitIndex = System.Threading.WaitHandle.WaitAny(_aWaitArray, 50);

					if (_nWaitIndex == 0) {//_oReceiveToken.AsyncWaitHandle
						var _nReadLength = oSocket.EndReceive(_oReceiveToken);
						if (_nReadLength > 0) {
							sMessageBuffer += Encoding.UTF8.GetString(_aSocketBuffer, 0, _nReadLength);
							this.HandleReadySocket();
						} else if (_nReadLength == 0) {
							oSocket.Close();
							oSocket = null;
							break;
						} else {
							throw new Exception("Receive failed");
						}

						_oReceiveToken = oSocket.BeginReceive(_aSocketBuffer, 0, _aSocketBuffer.Length, SocketFlags.None, null, null);
					} else if (_nWaitIndex == 1) {//_stepWait
						this.HandleBreak();
					}
				}
			} catch (Exception e) {
				try {
					this.Detach();
				} catch (Exception e2) {
			//		Console.Error.WriteLine("DETACH FAILURE:\n"+e2.ToString());
				}
		//		Console.Error.WriteLine(e.ToString());
			}
			if (oSocket != null) {
				oSocket.Close();
			}
		}

		private void HandleReadySocket() {
          
			while(sMessageBuffer.Contains("\0")) {
				var _sMessage = sMessageBuffer.Substring(0, sMessageBuffer.IndexOf('\0'));
        //      MessageBox.Show("message " + message);
        
#pragma warning disable 162
				if (SHOW_MESSAGES) { Console.WriteLine("Message: "+(_sMessage.Length > 1000 ? _sMessage.Substring(0, 1000) : _sMessage)); }
#pragma warning restore 162

				sMessageBuffer = sMessageBuffer.Substring(_sMessage.Length+1);
				var _sParsedMessage = this.ParseInputMessage(_sMessage);

				Func<String,String,String> fGetParamOrDefault = (String key, String defaultVal) => {
					string val;
					_sParsedMessage.Item2.TryGetValue("-"+key, out val);
					val = val ?? defaultVal;
					return val;
				};

				var _sTransId = fGetParamOrDefault("i", "");

				var _sCommand = _sParsedMessage.Item1;

                fReceiveCmd(_sCommand);

				String _sOutputMessage;
        //     MessageBox.Show("command " + command  + ": " + transId + " : " +  message);
				switch(_sCommand) {
					case "status":
						_sOutputMessage = this.ContinuationXml(_sParsedMessage.Item1, _sTransId);
						break;
					case "feature_get": {
							var name = fGetParamOrDefault("n", "");
							_sOutputMessage = this.FeatureGetXml(_sTransId, name);
						}
						break;
					case "feature_set": {
                         //     MessageBox.Show("transId " + transId);
							var name = fGetParamOrDefault("n", "");
							var newValue = fGetParamOrDefault("v", "");
							_sOutputMessage = this.FeatureSetXml(_sTransId, name, newValue);
                           //       MessageBox.Show("outputMessage " + outputMessage);
                            //	outputMessage = this.ErrorXml(parsedMessage.Item1, transId, 4, "Test");
						}
						break;
                	case "breakpoint_list": {
							_sOutputMessage = this.Breakpoint_list(_sTransId);
       
						}
                	break;
                	case "source": {
							_sOutputMessage = this.Source(_sTransId);
   
						}
						break;


					default:
					//	if (_mdbgProcess.IsAlive) {
							switch(_sCommand) {
								case "detach":
									this.Detach();
									_sOutputMessage = this.ContinuationXml(_sParsedMessage.Item1, _sTransId);
									break;
								case "context_names":
									_sOutputMessage = this.ContextNamesXml(_sTransId);
									break;
								case "context_get": {
										var contextId = int.Parse(fGetParamOrDefault("c", "0"));
										var depth = int.Parse(fGetParamOrDefault("d", "0"));
										_sOutputMessage = this.ContextGetXml(_sTransId, contextId, depth);
									}
									break;
								case "property_get": {
										var contextId = int.Parse(fGetParamOrDefault("c", "0"));
										var name = fGetParamOrDefault("n", "");
										var depth = int.Parse(fGetParamOrDefault("d", "0"));
										_sOutputMessage = this.PropertyGetXml(_sTransId, contextId, name, depth);
									}
									break;
								case "run":
								case "step_into":
								case "step_over":
								case "step_out":
								case "break":
									if (_stepWait == null || _sCommand == "break") {
										_steppingCommand = _sCommand;
										_steppingTransId = _sTransId;
										_sOutputMessage = null;
										this.Step();
                                          _sOutputMessage = this.ContinuationXml(_sParsedMessage.Item1, _sTransId);
									} else {
										_sOutputMessage = this.ErrorXml(_sParsedMessage.Item1, _sTransId, 5, "Requested stepping while already stepping");
									}
									break;
								case "stop":
								//	_mdbgProcess.Kill().WaitOne();
									_sOutputMessage = this.ContinuationXml(_sParsedMessage.Item1, _sTransId);
									break;
								case "stack_get": {
										var depthStr = fGetParamOrDefault("c", "");
										//var depth = String.IsNullOrWhiteSpace(depthStr) ? (int?)null : (int?)int.Parse(depthStr);
										var depth = String.IsNullOrEmpty(depthStr) ? (int?)null : (int?)int.Parse(depthStr);
										_sOutputMessage = this.StackGetXml(_sTransId, depth);
									}
									break;
								case "breakpoint_set":
									var type = fGetParamOrDefault("t", "");
									var file = fGetParamOrDefault("f", "");
									var line = int.Parse(fGetParamOrDefault("n", "0"));
									var state = fGetParamOrDefault("s", "");
									_sOutputMessage = this.BreakpointSetXml(_sTransId, type, file, line, state);
									break;
								case "breakpoint_remove":
									var id = int.Parse(fGetParamOrDefault("d", "0"));
									_sOutputMessage = this.BreakpointRemoveXml(_sTransId, id);
									break;
								case "eval":
								case "expr":
								case "exec":
									_sOutputMessage = this.EvalXml(_sParsedMessage.Item1, _sTransId, _sParsedMessage.Item3);
									break;
								default:
                                        
								//	outputMessage = this.ErrorXml(parsedMessage.Item1, transId, 4, "Test");
                                   _sOutputMessage = this.ContinuationXml(_sParsedMessage.Item1, _sTransId);
									break;
							}
                /*
						} else {
                 
							outputMessage = this.ContinuationXml(parsedMessage.Item1, transId);
						}*/
						break;
				}
                fSendMessage(_sOutputMessage);
                /*
				if (_sOutputMessage != null) {
					var _sRealMessage = this.GenerateOutputMessage(_sOutputMessage);
					oSocket.Send(Encoding.UTF8.GetBytes(_sRealMessage));
				}*/
			}
		}

       public virtual void fReceiveCmd(string _sCommand)
        {
            throw new NotImplementedException();
        }

        public void fSendMessage(string _sOutputMessage) {
         //   Console.WriteLine(_sOutputMessage);
            if (_sOutputMessage != null) {
				var _sRealMessage = this.GenerateOutputMessage(_sOutputMessage);
				oSocket.Send(Encoding.UTF8.GetBytes(_sRealMessage));
			}
        }

        private void HandleBreak() {
            /*
			lock(oMdbgProcessLock) {
				var validStop = oMdbgProcess.StopReason is AsyncStopStopReason
				|| oMdbgProcess.StopReason is BreakpointHitStopReason
				|| oMdbgProcess.StopReason is StepCompleteStopReason
				|| oMdbgProcess.StopReason is ProcessExitedStopReason
				|| bDetaching;
				if (validStop) {
					if (oMdbgProcess.StopReason is ProcessExitedStopReason) {
						Console.Write("Attached process ended");
						this.Detach();
					} else {
						Console.WriteLine("Breaking");
					}
					var outputMessage = this.ContinuationXml(_steppingCommand, _steppingTransId);
					var realMessage = this.GenerateOutputMessage(outputMessage);
					oSocket.Send(Encoding.UTF8.GetBytes(realMessage));
					_stepWait = null;
					_steppingCommand = null;
					_steppingTransId = null;
				} else {
					var errorStop = oMdbgProcess.StopReason as ErrorStopReason;
					if (errorStop != null) {
						// HACK - Work around for MDBG bug - it errors if unknown threads exit
						if (errorStop.ExceptionThrown.Message == "The given key was not present in the dictionary."
							&& errorStop.ExceptionThrown.StackTrace.Contains("Microsoft.Samples.Debugging.MdbgEngine.ManagedRuntime.ExitThreadEventHandler")) {
							//Console.WriteLine(String.Format("Continuing - invalid stop: {0}", "MDBG exit thread bug"));
							oMdbgProcess.AsyncStop().WaitOne(); // Force valid stop state
							if (oMdbgProcess.StopReason is AsyncStopStopReason) {
								this.Step();
							} else {
								Console.WriteLine(String.Format("Consumed unexpected stop"));
								HandleBreak();
							}
						} else {
							Console.WriteLine(String.Format("Continuing erred: {0}", errorStop.ExceptionThrown));
							throw errorStop.ExceptionThrown;
						}
					} else  {
						Console.WriteLine(String.Format("Continuing - invalid stop: {0}", oMdbgProcess.StopReason));
						this.Step();
					}
				}
			}*/

		}

		private void Step() {
            //    MessageBox.Show("_steppingCommand " + _steppingCommand);
			lock(oMdbgProcessLock) {
				switch (_steppingCommand) {
					case "break":
                        fStop();
					//	_stepWait = _mdbgProcess.AsyncStop();
						break;
					case "run":
                        fRun();
					//	_stepWait = _mdbgProcess.Go();
						break;
					case "step_into":
                        fStepInto();
					//	_stepWait = StepImpl(_mdbgProcess, StepperType.In, false);
						break;
					case "step_over":
                        fStepOver();
					//	_stepWait = StepImpl(_mdbgProcess, StepperType.Over, false);
						break;
					case "step_out":
                        fStepOut();
					//	_stepWait = StepImpl(_mdbgProcess, StepperType.Out, false);
						break;
					default:
						if (_steppingCommand != null) {
							throw new Exception("Assertion failed: "+_steppingCommand);
						}
						break;
				}
			}
		}
                   
        public virtual void fRun(){}
        public virtual void fStop(){}
        public virtual void fStepInto(){}
        public virtual void fStepOver(){}
        public virtual void fStepOut(){}


		private String EvalXml(String command, String transId, byte[] data) {
			var input = System.Text.Encoding.UTF8.GetString(data);
			var rawArguments = this.ParseEvalMessage(input);

			try {
				//var evalResult = DoEval(rawArguments);
                string _sEvalResult = "";
               int _nSuccess = 1;//1:0

				//var resultStr = evalResult.Item1 ? this.ContextGetPropertyXml(evalResult.Item2, nMaxDepth, input) : String.Empty;
			//	string _sResultStr = _nSuccess == 1 ? this.ContextGetPropertyXml(_sEvalResult, nMaxDepth, input) : String.Empty;;
                 string _sResultStr = "";

				return String.Format(
					"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
					+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"{0}\" transaction_id=\"{1}\" success=\"{2}\">"
					+"	{3}"
					+"</response>",
					command,
					transId,
					_nSuccess,
					_sResultStr
				);
			} catch (Exception e) {
				return this.ErrorXml(command, transId, 206, e.ToString());
			}
		}
       

		private String GenerateOutputMessage(String message) {
			var length = message.Length;
			var result = String.Format("{0}\0{1}\0", length.ToString(), message);

#pragma warning disable 162
			if (SHOW_MESSAGES) { Console.WriteLine("Response: "+(result.Length > 1000 ? result.Substring(0, 1000) : result)); }
#pragma warning restore 162

			return result;
		}

		private String ErrorXml(String command, String transId, int errorCode, String errorMessage) {
			return String.Format(
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"{0}\" transaction_id=\"{1}\">"
				+"	<error code=\"{2}\" apperr=\"{3}\">"
				+"		<message>{4}</message>"
				+"	</error>"
				+"</response>"
				,
				command, transId, errorCode, String.Empty, this.EscapeXml(errorMessage));
		}



        public string fGetBreakpoint(Breakpoint _oBkp) {
                string _sState = "enabled";
                if (!_oBkp.bEnable) {
                    _sState = "disabled";
                }
                string _sResolved = "resolved";
                if (!_oBkp.bResolved) {
                    _sResolved = "unresolved";
                }
                return "<breakpoint"
                + " id=\"" + _oBkp.nID + "\"" 
                + " type=\"" + _oBkp.sType + "\"" 
                + " state=\"" + _sState + "\"" 
                + " resolved=\"" + _sResolved + "\"" 
                + " filename=\"file:///" + _oBkp.sPath + "\"" 
                + " lineno=\"" + _oBkp.nLine + "\"" 
                + " function=\"" + _oBkp.sFunction + "\"" 
                + " exception=\"" + _oBkp.sExeption + "\"" 
                + " hit_value=\"" + _oBkp.nHitValue + "\"" 
                + " hit_condition=\"" + _oBkp.sHitCondition + "\"" 
                + " hit_count=\"" + _oBkp.nHitCount + "\"" 
                //+ " expression=\"eeee\"" 
                +">"
        //        + "<expression>EXPRESSION</expression>"
                +"</breakpoint>";
        }

        
        private string Breakpoint_list(string transId) {
           
            string _sBreakpointList = "";

            foreach (Breakpoint _oBkp in aBreakpoint) {
                _sBreakpointList += fGetBreakpoint(_oBkp);
            }


         	return String.Format(
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"breakpoint_list\" transaction_id=\"{0}\">"
			 	+_sBreakpointList
				+"</response>",
				transId
			);
        }

        private string Source(string transId) {
         	return String.Format(
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"source\" success=\"1\" transaction_id=\"{0}\">"
	            	//...data source code...
				+"</response>",
				transId
			);
        }





		private String InitXml(String path) {
           // path = @"E:\_Project\Cwc\cwc.exe";
   

			return String.Format(
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<init xmlns=\"urn:debugger_protocol_v1\" appid=\"DotNetDbgp\" idekey=\"{0}\" session=\"\" thread=\"\" parent=\"\" language=\"C#\" protocol_version=\"1.0\" fileuri=\"{1}\" />",
				//+"<init xmlns=\"urn:debugger_protocol_v1\" appid=\"DotNetDbgp\" idekey=\"\" session=\"\" thread=\"\" parent=\"\" language=\"C#\" protocol_version=\"1.0\" fileuri=\"{0}\" />",
				//path ?? "dbgp:null"
                "Cwc (" + Setting.sLauchedName + ")",
				"" //No path
			);
		}

		private String ContextNamesXml(String transId) {
			return String.Format(
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"context_names\" transaction_id=\"{0}\">"
				+"	<context name=\"Both\" id=\"0\"/>"
				+"	<context name=\"Local\" id=\"1\"/>"
				+"	<context name=\"Arguments\" id=\"2\"/>"
				+"</response>",
				transId
			);
		}

		private String ContinuationXml(String command, String transId) {
			return String.Format(
				 "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"{0}\" status=\"{2}\" reason=\"ok\" transaction_id=\"{1}\"/>",
			//	command, transId, _mdbgProcess.IsAlive ? _mdbgProcess.IsRunning ? "running" : "break" : "stopped"
				command, transId, "break"
			//	command, transId, "running"
                //TODO "running" : "break" : "stopped"
			);
		}

		private String StackGetXml(String transId, int? depth) {

 	        var framesString = String.Empty;


            List<Frame> _aFrame = fStackGet();
         
			 foreach(Frame _oFrame in _aFrame) {
                if(depth != null &&  _oFrame.nStackLevel != depth){continue;}//Skip other if depth level is specified

                string _sFile =  _oFrame.sFile;
               if(_sFile != "??"){
                 _sFile = "file:///" + EscapeXml(_sFile);
              } else {    _sFile = "";  }


				framesString += String.Format(
					"<stack level=\"{0}\" type=\"{3}\" filename=\"{1}\" lineno=\"{2}\" where=\"{4}\" s=\"\" cmdend=\"\"/>",
					_oFrame.nStackLevel,
					_sFile,
					_oFrame.nLine,
					"file", //"eval" : "file" // Valid values are file or eval.
					EscapeXml(_oFrame.sFuncName)
				);
			}

			return String.Format(
				 "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"stack_get\" transaction_id=\"{0}\">"
				+"	{1}"
				+"</response>",
				transId,
				framesString
			);
		}

        public static int nBreakPointCurrID = 0;
		private String BreakpointSetXml(String transId, String type, String file, int line, String state) {
       
			if (file.StartsWith("file:///")) {
				file = file.Substring(8);
			}
			file = file.Replace('\\', '/');


            bool _bEnable = true;
            if (state == "disabled") {
                _bEnable = false;
            }


            Breakpoint _oBreakpoint = new Breakpoint();
            aBreakpoint.Add(_oBreakpoint); nBreakPointCurrID++;

            _oBreakpoint.nID = nBreakPointCurrID;
            _oBreakpoint.sPath = file;
            _oBreakpoint.nLine = line;
            _oBreakpoint.bEnable = _bEnable;


                fBreakpointSet(_oBreakpoint);

            string _sReturnState = "enabled";
            if (!_oBreakpoint.bEnable) {
                _sReturnState = "disabled";
            }

            string _sResolved = "resolved";
            if (!_oBreakpoint.bResolved) {
                _sResolved = "unresolved";
            }

            Setting.oSettingsLauch.fSaveSetting(); //TODO optimize?

            //BREAKPOINT_ID: Is an arbitrary string that uniquely identifies this breakpoint in the debugger engine.
            //STATE: The initial state of the breakpoint as set by the debugger engine  [optional, defaults to "enabled"] "enabled" / "disabled"
            //RESOLVED ?: Resolved if the debugger engine knows the breakpoint is valid, or unresolved otherwise. This attribute is only present if the debugger engine implements the "resolving" feature. "resolved" / "unresolved"
                      
			//Console.WriteLine(String.Format("Breakpoint: {0}", breakpoint.ToString()));
			return String.Format(
					"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"breakpoint_set\" transaction_id=\"{0}\" state=\"{1}\" resolved=\"{2}\" id=\"{3}\"/>", //  resolved="RESOLVED"? resolved /unresolved 
				//transId, state, breakpoint.Number
				transId, _sReturnState, _sResolved, _oBreakpoint.nID //TEst
			);
       
		}

    

        private String BreakpointRemoveXml(String transId, int id) {
            
            foreach (Breakpoint _oBkp in aBreakpoint) {
                if (_oBkp.nID == id) {
                    fBreakpointRemove(_oBkp); //TODO optimize?
                    aBreakpoint.Remove(_oBkp);
                    break;
                }
            }
           // Console.WriteLine("--- "  + aBreakpoint.Count);
            Setting.oSettingsLauch.fSaveSetting(); //TODO optimize?
            
			return String.Format(
					"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"breakpoint_remove\" transaction_id=\"{0}\" />",
				transId
			);
		}

       // contextId:
       //Use context_names before or in general:
       // <context name="Local" id="0"/>
       //<context name="Global" id="1"/>
       //<context name="Class" id="2"/>

        const int nLOCAL = 0;
        const int nGLOBAL = 1;
        const int nCLASS = 2;

		private String ContextGetXml(String transId, int contextId, int depth) {

            var variablesString = new StringBuilder();
            if(contextId == nLOCAL){

                List<Frame> _aFrame = fStackGet();
         
			    foreach(Frame _oFrame in _aFrame) {
                    //if(_oFrame.nStackLevel != depth){continue;}//Skip other if depth level is specified
                    foreach(Var _oVar in _oFrame.aVar) {
                         variablesString.Append(ContextGetPropertyXml(_oVar, nMaxDepth));
                    }
		        }
            }
            if(contextId == nGLOBAL){
                
            }


            /*
			var variables = new List<MDbgValue>();

			if (_mdbgProcess.Threads.HaveActive) {
				var frame = depth == 0 ? _mdbgProcess.Threads.Active.CurrentFrame : _mdbgProcess.Threads.Active.Frames.ElementAt(depth);
				if (contextId == 0 || contextId == 1) {
					variables.AddRange(frame.GetActiveLocalVariables());
				}
				if (contextId == 0 || contextId == 2) {
					variables.AddRange(frame.GetArguments());
				}
			}

			
			foreach(var var in variables) {
				variablesString.Append(this.ContextGetPropertyXml(var, _maxDepth));
			}*/

          //  string _sVariablesString = "";

			return String.Format(
				 "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"context_get\" context=\"{1}\" transaction_id=\"{0}\">"
				+"{2}"
				+"</response>",
				transId,
				contextId,
				variablesString.ToString()
			);
		}


        string PropertyGetXml_transId = "";
        int PropertyGetXml_contextId = 0;

		private String PropertyGetXml(string transId, int contextId, string name, int depth) {
		
            /*
            var frame = depth == 0 ? _mdbgProcess.Threads.Active.CurrentFrame : _mdbgProcess.Threads.Active.Frames.Cast<MDbgFrame>().ElementAt(depth);
			var var = _mdbgProcess.ResolveVariable(name, frame);
			var variablesString = var != null ? this.ContextGetPropertyXml(var, _maxDepth, name) : String.Empty;
            */
            //Set depth?...

            
            //Only one at time  
            if(PropertyGetXml_transId == ""){ //If not ready skip... TODO array?
                PropertyGetXml_transId = transId;
                PropertyGetXml_contextId = contextId;
               if( fPropertyGet(name)  != "") {
                                 return null;
                   	return String.Format(
				         "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				        +"<response xmlns=\"urn:debugger_protocol_v1\" command=\"property_get\"  transaction_id=\"{0}\">"
				        +" <property  name=\""+ name+ "\" >{1}"
				        +"</property></response>",
				        transId,
				        //_sVariablesString
				        "Diconnect"
				        //variablesString.ToString()
			        );
                }
            }
          
         
           
            //Respond later
            
	
               return null;
		}
        
		public void fPropertyGetSend(string _sValue) {



          fSendMessage(
           String.Format(
				 "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"property_get\"  transaction_id=\"{0}\">"
				+" <property >{1}"
				+"</property></response>",
				PropertyGetXml_transId,
			//EscapeXml("\"" + _sValue + "\"" )
			EscapeXml( _sValue)
				//variablesString.ToString()
			) );
            PropertyGetXml_transId = "";
           // PropertyGetXml_contextId = 0;
        }




		private String FeatureGetXml(string transId, string name) {
			String featureValue;
			bool supported = true;
			switch(name) {
				case "language_supports_thread":
					featureValue = "0";
					break;
				case "language_name":
					featureValue = ".NET";
					break;
				case "language_version":
					featureValue = "NYI";
					break;
				case "encoding":
					featureValue = "UTF-8";
					break;
				case "protocol_version":
					featureValue = "1";
					break;
				case "supports_async":
					featureValue = "1";
					break;
				case "data_encoding":
					featureValue = "base64";
					break;
				case "breakpoint_language":
					featureValue = "";
					break;
				case "breakpoint_types":
					featureValue = "line";
					break;
				case "multiple_session":
					featureValue = "0";
					break;
				case "max_children":
					featureValue = nMaxChildren.ToString();
					break;
				case "max_data":
					featureValue = nMaxData.ToString();
					break;
				case "max_depth":
					featureValue = nMaxDepth.ToString();
					break;
				case "detach":
				case "context_names":
				case "context_get":
				case "property_get":
				case "feature_get":
				case "feature_set":
				case "run":
				case "step_into":
				case "step_over":
				case "step_out":
				case "break":
				case "stop":
				case "stack_get":
				case "breakpoint_set":
				case "breakpoint_remove":
				case "eval":
				case "expr":
				case "exec":
				case "status":
					featureValue = "1";
					break;
				default:
					featureValue = String.Empty;
					supported = false;
					break;
			}

			return String.Format(
				 "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"feature_get\" supported=\"{1}\" transaction_id=\"{0}\">"
				+"{2}"
				+"</response>",
				transId,
				supported?1:0,
				this.EscapeXml(featureValue)
			);
		}

		private String FeatureSetXml(string transId, string name, string newValue) {
			try {
				switch(name) {
					case "max_children":
						nMaxChildren = int.Parse(newValue);
                //   MessageBox.Show("max_children " + _maxChildren);
						break;
					case "max_data":
						nMaxData = int.Parse(newValue);
                  //      MessageBox.Show("_maxData " + _maxData);
						break;
					case "max_depth":
						nMaxDepth = int.Parse(newValue);
                 //   MessageBox.Show("_maxDepth " + _maxDepth);
						break;
					default:
						return this.ErrorXml("feature_set", transId, 3, name+" is an unknown or unsupported feature");
				}
			} catch (FormatException) {
				return this.ErrorXml("feature_set", transId, 3, "["+newValue+"] is invalid for "+name);
			}

			return String.Format(
				 "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
				+"<response xmlns=\"urn:debugger_protocol_v1\" command=\"feature_get\" success=\"{1}\" transaction_id=\"{0}\">"
				//+"{2}"
				+"</response>",
				transId,
				1
			);
		}

		private readonly IList<Type> AUTOMATICLY_STRINGIFY = new List<Type> {
			typeof(System.Decimal),
			typeof(System.DateTime),
		};

       

		private String LimitLength(String val, int maxLength) {
			if (val.Length <= maxLength) {
				return val;
			} else {
				return val.Substring(0, maxLength);
			}
		}

		private Tuple<String,IDictionary<String,String>,byte[]> ParseInputMessage(String message) {
			var arguments = this._ParseInputMessageInner(message, true);

			var parts = arguments.Item2;
			var resultArguments = new Dictionary<String,String>();
			for(var j = 0; j + 1 < parts.Count; j += 2) {
				var key = parts[j];
				var val = parts[j+1];
				resultArguments[key] = val;
			}

			return Tuple.Create(arguments.Item1, (IDictionary<String,String>)resultArguments, arguments.Item3);
		}

		private Tuple<String,IList<String>> ParseEvalMessage(String message) {
			var arguments = this._ParseInputMessageInner(message, false);
			return Tuple.Create(arguments.Item1, arguments.Item2);
		}

		private Tuple<String,IList<String>,byte[]> _ParseInputMessageInner(String message, bool hasBody) {
			var commandSplitter = message.IndexOf(" ");
			if (commandSplitter < 0) commandSplitter = message.Length;
			var command = message.Substring(0, commandSplitter);
			//Console.WriteLine("Command: "+command);

			var inQuotes = false;
			var escape = false;
			var part = String.Empty;
			var parts = new List<String>();
			var i = commandSplitter;
			for(; i < message.Length; i++) {
				var messageChar = message[i];
				if (escape) {
					escape = false;
				} else if (messageChar == '"') {
					inQuotes = !inQuotes;
					continue;
				} else if (messageChar == '\\') {
					escape = true;
					continue;
				} else if (messageChar == ' ' && !inQuotes) {
					if (part.Length != 0) {
						if (part == "--" && hasBody) {
							i++;
							break;
						}
						parts.Add(part);
						//Console.WriteLine("Part: "+part);
						part = String.Empty;
					}
					continue;
				}
				part += messageChar;
				//Console.WriteLine("Part: "+part);
			}

			if (part.Length != 0 && part != "--") {
				parts.Add(part);
				//Console.WriteLine("Part: "+part);
			}

			var bodyStr = message.Substring(i);
			var body = !String.IsNullOrEmpty(bodyStr) ? Convert.FromBase64String(bodyStr) : new byte[0];

			//Console.WriteLine("Body: "+body);

			return Tuple.Create(command, (IList<String>)parts, body);
		}

		private String EscapeXml(String input) {
			return new System.Xml.Linq.XText(input == null ? "<null>" : this.EscapeXmlCharacters(input)).ToString().Replace("\"", "&quot;");
		}

		private String EscapeXmlCharacters(String input) {

            return XmlEncode(input);
                
              // return input; // TODO TODO

			try {
				// throws exception if string contains any invalid characters.
				//return XmlConvert.VerifyXmlChars(input);
	         
			} catch {//Never reach TODO
                /*
				var sb = new StringBuilder();

				foreach (var c in input) {
					if (XmlConvert.IsXmlChar(c)) {
						sb.Append(c);
					} else {
						sb.Append(string.Format("[0x{0:X2}]", (short)c));
					}
				}*/

				//return sb.ToString();
				return "";
			}
		}

		public void Detach() {
			lock(oMdbgProcessLock) {
				bDetaching = true;
                /*
				try {
					if (oMdbgProcess.IsAlive && oMdbgProcess.IsRunning) {
						oMdbgProcess.AsyncStop().WaitOne();
					}
					oMdbgProcess.Breakpoints.DeleteAll();
				} finally {
					oMdbgProcess.Detach().WaitOne();
				}*/

				bDetaching = false;
			}
		}



        private String ContextGetPropertyXml(Var _oVar, int depth) {
			if (depth < 0) {
				return String.Empty;
			}
		    string fullName = _oVar.sName;
			
			var childPropertiesString = new StringBuilder();
            childPropertiesString.Append("valueeeeee");
            /*
			var managedValue = val as ManagedValue;
			if (managedValue.IsArrayType) {
				foreach(var child in managedValue.GetArrayItems().ToList()) {
					if (childPropertiesCount <= nMaxChildren) {
						childPropertiesString.Append(this.ContextGetPropertyXml(child, depth-1, fullName+child.Name));
					}
					childPropertiesCount++;
				}
			}
			var automaticlyStringify = AUTOMATICLY_STRINGIFY.Any(i => String.Equals(i.FullName, managedValue.TypeName));
			if (managedValue.IsComplexType && !automaticlyStringify) {
				foreach(var child in managedValue.GetFields()) {
					if (childPropertiesCount <= nMaxChildren) {
						childPropertiesString.Append(this.ContextGetPropertyXml(child, depth-1, fullName+"."+child.Name));
					}
					childPropertiesCount++;
				}
			}*/

          
			Func<String,String> e = (String i) => this.EscapeXml(i);
              /*
			var myValue = e(val.GetStringValue(0, false));
			if (automaticlyStringify) {
				var managedThread =  oMdbgProcess.Threads.Active.Get<ManagedThread>();
				try {
					oMdbgProcess.TemporaryDefaultManagedRuntime.CorProcess.SetAllThreadsDebugState(CorDebugThreadState.THREAD_SUSPEND, managedThread.CorThread);
					var eval = managedThread.CorThread.CreateEval();
					var myValue2 = DoFunctionEval(GetFunction(managedValue.TypeName+".ToString"), new[] { managedValue.CorValue }, eval, managedThread).Item2;
					myValue = e(myValue2.GetStringValue(0, false));
				} finally {
					managedThread.Runtime.CorProcess.SetAllThreadsDebugState(CorDebugThreadState.THREAD_RUN, managedThread.CorThread);
				}
			}*/

           // string _sChildPropertiesString = "_sChildPropertiesString"; // LimitLength(myValue, nMaxData)
           // string _sValue = "_sValue"; // LimitLength(myValue, nMaxData)

            //children:  true/false whether the property has children this would be true for objects or array's.
			return String.Format(
				"<property name=\"{0}\" fullname=\"{1}\" type=\"{2}\" classname=\"{2}\" constant=\"0\" children=\"{3}\" size=\"{4}\" encoding=\"none\" numchildren=\"{6}\">{5}</property>",
				e(_oVar.sName), e(fullName), e(_oVar.sType), _oVar.bHaveChildren,         childPropertiesString.Length, LimitLength(childPropertiesString.ToString(), nMaxData) , _oVar.nChildPropertiesCount
			//	e(_oVar.sName), e(fullName), e(_oVar.sType), _oVar.bHaveChildren,         childPropertiesString.Length, LimitLength(_sValue, nMaxData),  childPropertiesString.ToString(), _oVar.nChildPropertiesCount
              //e(_oVar.sName), e(fullName), e(_oVar.sType), _oVar.nChildPropertiesCount, myValue.Length+childPropertiesString.Length, LimitLength(myValue, nMaxData), childPropertiesString.ToString()
			);
		}




public class Tuple<T1>  { 
    public Tuple(T1 item1) { 
        Item1 = item1; 
    }   
    public T1 Item1 { get; set; }  
} 

public class Tuple<T1, T2> : Tuple<T1>   { 
    public Tuple(T1 item1, T2 item2) : base(item1) { 
        Item2 = item2; 
    } 
    public T2 Item2 { get; set; }  
} 

public class Tuple<T1, T2, T3> : Tuple<T1, T2>   { 
    public Tuple(T1 item1, T2 item2, T3 item3) : base(item1, item2) { 
        Item3 = item3; 
    } 
    public T3 Item3 { get; set; }  
} 

public static class Tuple   { 
    public static Tuple<T1> Create<T1>(T1 item1)  { 
        return new Tuple<T1>(item1); 
    } 
    public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)  { 
        return new Tuple<T1, T2>(item1, item2); 
    } 
    public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)  { 
        return new Tuple<T1, T2, T3>(item1, item2, item3); 
    }  
}

public static string XmlEncode(string value){ 
    System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings  {
        ConformanceLevel = System.Xml.ConformanceLevel.Fragment
    };
    StringBuilder builder = new StringBuilder();
    using (var writer = System.Xml.XmlWriter.Create(builder, settings)) {
        writer.WriteString(value);
    }
    return builder.ToString();
}

public static string XmlDecode(string xmlEncodedValue){
    System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings {
        ConformanceLevel = System.Xml.ConformanceLevel.Fragment
    };
    using (var stringReader = new System.IO.StringReader(xmlEncodedValue)){
        using (var xmlReader = System.Xml.XmlReader.Create(stringReader, settings)){
            xmlReader.Read();
            return xmlReader.Value;
        }
    }
}


	}



    ////////////////////////////////////
    ////// Reference only /////////////////
    ////////////////////////////////////

     /*
		private static WaitHandle StepImpl(MDbgProcess mdbgProcess, StepperType type, bool nativeStepping) {


			//HACKHACKHACK
			mdbgProcess.GetType().GetMethod("EnsureCanExecute", BindingFlags.NonPublic|BindingFlags.Instance, null, new[] { typeof(String) }, null).Invoke(mdbgProcess, new Object[] { "stepping" });
			try {
				var frameData = mdbgProcess.Threads.Active.BottomFrame.GetPreferedFrameData(((IRuntime)mdbgProcess.Runtimes.NativeRuntime ?? mdbgProcess.Runtimes.ManagedRuntime));
				var stepDesc = frameData.CreateStepperDescriptor(type, nativeStepping);
				var managerStepDesc = stepDesc as ManagedStepperDescriptor;
				if (managerStepDesc != null) managerStepDesc.IsJustMyCode = true;
				stepDesc.Step();
			} catch (Microsoft.Samples.Debugging.MdbgEngine.NoActiveInstanceException) {
				try {
					foreach(var thread in mdbgProcess.Threads) {
						try {
							System.Console.WriteLine(thread.Id);
						} catch { System.Console.WriteLine("None"); }
						try {
								System.Console.WriteLine(thread.BottomFrame.Function.FullName);
						} catch { System.Console.WriteLine("None"); }
					}
				} catch {}
			}
			//HACKHACKHACK
			mdbgProcess.GetType().GetMethod("EnterRunningState", BindingFlags.NonPublic|BindingFlags.Instance, null, new Type[0], null).Invoke(mdbgProcess, new Object[0]);
		
           // return mdbgProcess.StopEvent;
            return null;
		}*/

     
		


     /*
		private CorValue[] ParseEvalArguments(IEnumerable<String> arguments, CorEval eval) {
			return arguments.Select(i => {
				bool boolVal;
				int intVal;
				double doubleVal;
				if (int.TryParse(i, out intVal)) {
					return this.MakeVal(intVal, CorElementType.ELEMENT_TYPE_I4, eval);
				} else if (double.TryParse(i, out doubleVal)) {
					return this.MakeVal(doubleVal, CorElementType.ELEMENT_TYPE_R8, eval);
				} else if (bool.TryParse(i, out boolVal)) {
					return this.MakeVal(boolVal, CorElementType.ELEMENT_TYPE_BOOLEAN, eval);
				} else if (i[0] == '\"' && i[i.Length-1] == '\"') {
					return this.MakeStr(i.Substring(1, i.Length - 2), eval);
				} else if (i[0] == '\'' && i[i.Length-1] == '\'') {
					return this.MakeVal(i[1], CorElementType.ELEMENT_TYPE_CHAR, eval);
				} else if (i[0] == '$') {
					return oMdbgProcess.DebuggerVars[i].CorValue;
				} else {
					var variable = oMdbgProcess.ResolveVariable(i, oMdbgProcess.Threads.Active.BottomFrame);
					//Console.WriteLine(String.Format("Argument: {0}", variable.GetStringValue(0)));
					if (variable != null) {
						return variable.CorValue;
					}
				}

				throw new Exception(String.Format("Could not parse value from: {0}", i));
			})
			.ToArray();
		}*/
        /*
		private CorFunction GetFunction(String name) {
			var function = oMdbgProcess.ResolveFunctionNameFromScope(name);
			//Console.WriteLine(String.Format("Function: {0}", function));
			return function == null ? null : function.CorFunction;
		}*/

		//private CorFunction GetMethod(CorValue thisObj, String name, ManagedThread managedThread) {
		//	var managedValue = new ManagedValue(managedThread.Runtime, thisObj);
		//	ManagedModule managedModule;
		//	var type = _mdbgProcess.ResolveClass(managedValue.TypeName, managedThread.CorThread.AppDomain, out managedModule);
		//	if (type != null && managedModule != null) {
		//		var function =  _mdbgProcess.ResolveFunctionName(managedModule, managedValue.TypeName, name);
		//		return function == null ? null : function.CorFunction;
		//	}
		//	return null;
		//}
        /*
		private Type GetType(CorClass corClass, ManagedThread managedThread, out ManagedModule managedModule) {
			if (corClass != null) {
				managedModule = managedThread.Runtime.Modules.Lookup(corClass.Module);
				return managedModule.Importer.GetType(corClass.Token);
			} else {
				managedModule = null;
				return null;
			}
		}*/
        /*
		private bool IsAssignableFrom(Type targetType, Type sourceType, ManagedModule managedModule) {
			var sourceTypeToken = sourceType.MetadataToken;
			var targetTypeToken = targetType.MetadataToken;
			while(true) {
				if (targetTypeToken == sourceTypeToken) {
					return true;
				}
				var interfaceTokens = managedModule.Importer.EnumInterfaceImpls(sourceTypeToken);
				if(interfaceTokens.Any(i => i == targetTypeToken)) {
					return true;
				}
				String typeName; TypeAttributes typeAttributes; int extends;
				managedModule.Importer.GetTypeDefProps(sourceTypeToken, out typeName, out typeAttributes, out extends);
				if (typeName == "System.Object") { break; }
				sourceTypeToken = extends;
			}
			return false;
		}*/
        /*
		private CorFunction GetMethod(CorValue thisObj, String name, CorValue[] arguments, ManagedThread managedThread) {
			ManagedModule dummy;
			var argumentTypes = arguments.Select(i => this.GetType(i.ExactType.Class, managedThread, out dummy)).ToArray();
			ManagedModule managedModule;
			var corClass = thisObj.ExactType.Class;

			var type = this.GetType(corClass, managedThread, out managedModule);
			while(type != null && managedModule != null) {
				var methods = type.GetMethods().Where(i => i != null && i.Name == name).ToArray();
				foreach(var method in methods) {
					if (method != null && method.Name == name) {
						var parameters = method.GetParameters().ToArray();
						var okay = parameters.Select((i, j) => j).Aggregate(true, (i, j) =>
							i && argumentTypes.Length > j ? IsAssignableFrom(parameters[j].ParameterType, argumentTypes[j], managedModule)
							: parameters[j].IsOptional
						);
						if (okay) {
							var function = managedModule.GetFunction(method.MetadataToken);
							if (function != null) {
								return function.CorFunction;
							}
						}
					}
				}
				String typeName; TypeAttributes typeAttributes; int extends;
				managedModule.Importer.GetTypeDefProps(corClass.Token, out typeName, out typeAttributes, out extends);
				corClass = typeName != "System.Object" ? managedModule.CorModule.GetClassFromToken(extends) : (CorClass)null;
				type = this.GetType(corClass, managedThread, out managedModule);
			}
			return null;
		}*/

        /*
		private Tuple<bool,ManagedValue> DoEval(Tuple<String,IList<String>> rawArguments) {
			var managedThread =  oMdbgProcess.Threads.Active.Get<ManagedThread>();
			try {
				oMdbgProcess.TemporaryDefaultManagedRuntime.CorProcess.SetAllThreadsDebugState(CorDebugThreadState.THREAD_SUSPEND, managedThread.CorThread);
				var eval = managedThread.CorThread.CreateEval();

				var function = this.GetFunction(rawArguments.Item1);
				var arguments = rawArguments.Item2;
				if (function == null) {
					if(arguments.Count() == 0) {
						var result = this.ParseEvalArguments(new[] {rawArguments.Item1}, eval).Single();
						return Tuple.Create(true, new ManagedValue(oMdbgProcess.Threads.Active.Get<ManagedThread>().Runtime, result));
					} else if (arguments.First() == "=") {
						var sourceArguments = arguments.Skip(1);
						var source = this.DoEval(Tuple.Create(sourceArguments.First(), (IList<String>)sourceArguments.Skip(1).ToList())).Item2.CorValue;
						if (rawArguments.Item1.First() == '$') {
							var target = oMdbgProcess.DebuggerVars[rawArguments.Item1];
							target.Value = source;
						} else {
							var target = this.ParseEvalArguments(new[] { rawArguments.Item1 }, eval).First();
							var genericTarget = target as CorGenericValue;
							if (genericTarget != null) {
								genericTarget.SetValue(source.CastToGenericValue().GetValue());
							} else if (target is CorReferenceValue) {
								var refTarget = target as CorReferenceValue;
								refTarget.Value = source.CastToReferenceValue().Value;
							}
						}
						return Tuple.Create(true, new ManagedValue(oMdbgProcess.Threads.Active.Get<ManagedThread>().Runtime, source));
					} else if (arguments.First() == ".") {
						var methodArgs = arguments.Skip(2);
						var thisObj = this.ParseEvalArguments(new[] {rawArguments.Item1}, eval).Single();
						var parsedArguments = this.ParseEvalArguments(methodArgs, eval);
						var method = this.GetMethod(thisObj, arguments.Skip(1).First(), parsedArguments, managedThread);
						if (method != null) {
							var functionArgs = (new List<CorValue> { thisObj });
							functionArgs.AddRange(parsedArguments);
							return this.DoFunctionEval(method, functionArgs.ToArray(), eval, managedThread);
						} else {
							throw new Exception("Could not find method");
						}
					} else {
						throw new Exception("Could not parse eval");
					}
				} else {
					var parsedArguments = this.ParseEvalArguments(arguments, eval);
					return this.DoFunctionEval(function, parsedArguments, eval, managedThread);
				}
			} finally {
				managedThread.Runtime.CorProcess.SetAllThreadsDebugState(CorDebugThreadState.THREAD_RUN, managedThread.CorThread);
			}
		}*/
        /*
		private Tuple<bool,ManagedValue> DoFunctionEval(CorFunction function, CorValue[] arguments, CorEval eval, ManagedThread managedThread) {
			eval.CallFunction(function, arguments);
			while(true) {
				oMdbgProcess.Go().WaitOne();
				if (oMdbgProcess.StopReason is EvalExceptionStopReason || oMdbgProcess.StopReason is ProcessExitedStopReason) {
					var errorStop = oMdbgProcess.StopReason as EvalExceptionStopReason;
					if (errorStop != null) {
						throw new Exception(InternalUtil.PrintCorType(oMdbgProcess, errorStop.Eval.Result.ExactType));
					} else {
						return Tuple.Create(false, (ManagedValue)null);
					}
				}
				if (oMdbgProcess.StopReason is EvalCompleteStopReason) {
					break;
				}
			}
			return Tuple.Create(true, new ManagedValue(managedThread.Runtime, eval.Result));
		}*/
        /*
		private CorValue MakeStr(String val, CorEval eval) {
			eval.NewString(val);
			oMdbgProcess.Go().WaitOne();
			if (!(oMdbgProcess.StopReason is EvalCompleteStopReason)) throw new Exception();

			return eval.Result;
		}
		private CorValue MakeVal(object val, CorElementType type, CorEval eval) {
			var corVal = eval.CreateValue(type, null).CastToGenericValue();
			corVal.SetValue(val);
			return corVal;
		}*/




}
