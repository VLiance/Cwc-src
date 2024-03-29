﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace cwc {
    class Build {
        
         public static string sBuildAnd = "";



        public static void StartBuild(bool _bShowInfo = true){
            Data.bIWantGoToEnd = true;
           // fDebug("-------------StartBUILDDDD");
            Data.fClearData();

//bNothingToBuild = true;//asss
			//fSetWorkingDir();
            if(!Data.bNowBuilding) {

                 Interlocked.Exchange(ref CppCompiler.safeInstanceCount,0); //Can be negative value, TODO try to kill thread controlling safe instance?
                
                Data.aAllInclude.Clear();
                 Data.aAll_ArgumentManager.Clear();
                 Data.aAll_ArgumentManager.Add( Data.oCompilerArg);

                 Data.bToolchainDefined = false;
                 Data.bModuleIsRequired = false;
                 Data.bNonBuildCommand = false;
				 Data.aRequiredModule.Clear();
				 Data.aBrowser.Clear();
             
				CppCompiler.nError = 0;
                 Data.oArg = new ArgumentManager();
                 Data.oArg.ExtractMainArgument( Data.sArgExpand,true);
				
				if( Data.bUpdateMode) {
					UpdateCwc.fUpdateFiles( Data.sUpdateModeSrc);
					return;
				}


  //  bNothingToBuild = true;//aasss
            


				if(Data.bNothingToBuild || Data.bNonBuildCommand) {
				//if(bNothingToBuild) {
					Data.bNothingToBuild = false;
                    


                    /*
                    if(!Data.bConsoleMode){
                        GuiManager.fCreateGUI();
                    }*/
                                     
                    if(Data.sToLaunch != ""){
					    Delocalise.fDelocaliseInMainThread(Data.sToLaunch);
                    } else {
                       Msg.fShowIntroMessage();
		
                    }
					return;
				}


                /*
				//  if(bInConsole && oMainForm == null && bGUI) {
				   if( Data.oMainForm == null && Data.bGUI && !Data.bUpdateMode) {
						if(Data.sArgExpand.IndexOf('|') != -1 || Data.sArgExpand.IndexOf('>') != -1) { //Auto GUI mode
							 GuiManager.fCreateGUI();
						}
					}*/
             


					Data.oArg.fCompleteExtractMainArgument();


 

                    
					if(Data.bModeIDE) {
						Data.bDontExecute = true; //TODO only one
					}
                  
                            // Debug.fTrace("-------------BegfLoadModules " );
                    if(!Data.bModuleIsRequired){
                        //  fDebug("------RUNN ");
                       Data.oArg.fExtract();
                       
				       Data.oArg.fRun(null,Data.bDontExecute);
                      

                    } else {
                          //fDebug("--fLoadModules ");
                    //     Empty.fLoadModules();
                    }


                if (Data.oGuiConsole != null) {
                    Data.oGuiConsole.fAddAllUsedDir();
                }
			          //  fDebug("-------------AftfLoadModules");
                    /*
					if(!bDontExecute ) {
						 oArg.fRun();
					}*/
				
                 

					if(Data.bModeIDE) {	
						Data.oModeIDE.fFinishExtractArg();
					}
					

          

					if(Data.bNowBuilding && !Data.bDontExecute)  {
                        
                     // CppCompiler.CheckAllThreadsHaveFinishedWorking(true);

						Build.fDisableBuild();
						if( CppCompiler.nError == 0) {
							Data.oLaunchProject.fBuildFinish();

						}

					}else {

                      
                        
					   Build.fDisableBuild();

					}
                 	Data.bDontExecute = false;
			        

                if(Data.bModuleIsRequired){
                 Empty.fLoadModules();
                }


            }
          
        }

        internal static void fStartLoopTestingIdeLinkedClosing() {
           if(Data.nCloseOnId != 0){
		        Thread winThread4 = new Thread(new ThreadStart(() =>  {  
                    while(Base.bAlive){
				      //  PipeInput.fTestIdeClosed(Data.nCloseOnId); //TODO do it in is thread?
				        Thread.CurrentThread.Join(1000);
                    }
		        }));   winThread4.Start();
            }
        }

        internal static void fBeginBuild() {
            //sArg = " -wCwcUpd C:/aaa ";
            Data.sArgExpand = ArgProcess.fExpandAll( Data.sArg); //TODO Expand in another thread
            Data.sArgExpand = ArgProcess.fTestIfBeginWithAFile( Data.sArgExpand);

            Build.StartBuild();
        }

        public static void fEnableBuild(){
            Data.bNowBuilding = true;
         
                //Debug.fTrace("Enable!!");
            if(Data.oGuiConsole != null) {
         
                Data.oGuiConsole.fEnableBuild();
             //   Data.oGuiForm.StartBuild();
            }
        
        }

        internal static void fMainLoop() {
              
            if(!SysAPI.bIsLoadedFromAnotherCwcInstance){
                if(Base.bAlive){

                  // PipeInput.fConsoleExit(null);//Show >>
              //     Data.oLaunchProject.fConsoleExit(null);//Show >>
         
                    while (Base.bAlive ||  Data.bForceTestNextCmd ) {
                         Data.bForceTestNextCmd = false;

                         switch(Data.sCmd) {

                            case "StopBuild":Data.sCmd = "";
                                              //  Debug.fTrace("****StopBuild!!");
                                 Data.oLaunchProject.fCancel();
                            break;
                            case "StartBuild":Data.sCmd = "";
                                   // Debug.fTrace("****StartBuild!!");
                                 //  Console.WriteLine("start build");
                                Build.StartBuild();
                            break;
                            case "Delocalise":Data.sCmd = "";
                    //    Console.WriteLine("fDelocaliseCmd");
                               Delocalise.fDelocaliseCmd();

                            break;
                         }

                      
                     //   Thread.Sleep(1);
                       //Thread.Sleep(1);
                        Thread.CurrentThread.Join(1);
                        if (Data.bConsoleMode && !Data.bNowBuilding) {
                            Base.bAlive = false;
                        }
                    

                    }
                }
            }
         
        }

        public static void fDisableBuild(){

            if(Data.oLaunchProject != null && Data.oLaunchProject.oCurLaunch != null && Data.oLaunchProject.oCurLaunch.bExeLaunch) {
                return;
            }

            Data.bNowBuilding = false;
         CppCompiler.nTotalTicket = 0;
        CppCompiler.nCurrentTicket = 0;
        CppCompiler.nErrorTicket = -1;
            
            
       //   Debug.fTrace("fDisableBuild");
            if(Data.oGuiConsole != null) {
               Data.oGuiConsole.fDisableBuild();
            }
        }

       public static void StopBuild(){
           
              Data.sCmd = "";
            if(Data.bNowBuilding) {
              
                fDisableBuild();
                 Output.Trace("\f4C--Stop Build--");
                 SysAPI.KillProcessAndChildren( Data.MainProcess.Id );
                  //fClearData();
                 
                 //CppCompiler.safeInstanceCount = 0;
                 
            }
        }



        
         
        public static void EndExecution(){
       
            fDisableBuild();
            Output.Trace("\f4C--End Execution--");
           // KillProcessAndChildren( MainProcess.Id );
                 
            
        }



    }
}
