using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cwc {
    class GuiManager {
        	public static void fCreateGUI()  {

               // Console.WriteLine("fCreateGUI");
            /*
            if (SysAPI.bIsLoadedFromAnotherCwcInstance || Data.bModeIDE) { //Not GUI
                return;
            }*/


			//if(Data.oMsgForm == null) {

				   Empty.bHaveMsgForm = true;


				 Thread winThread = new Thread(new ThreadStart(() =>  {  

					 try { 
                                Application.EnableVisualStyles();
                            //    Application.SetCompatibleTextRenderingDefault(false); //Bug in W10
                               GuiConsole _oConsole =   new GuiConsole();
                                Application.Run(_oConsole);

						

					 }catch(Exception Ex) {
                         Console.WriteLine("Error: " + Ex.Message + " : " +Ex.Source  + " : " +Ex.StackTrace);
					 }
				  }));  

                    
				  winThread.SetApartmentState(ApartmentState.STA);  
				//  winThread.IsBackground = true;  
				  winThread.Start();

               Thread.CurrentThread.Join(1);
                while(   Base.bAlive && Data.oGuiConsole== null) {
                       Thread.CurrentThread.Join(1);
                }

                /*
                while(Data.oMsgForm == null) {
                   Thread.Sleep(1);
                }

                while( Data.oGuiForm== null) {
                   Thread.Sleep(1);
                }*/
               
               // fDebug("finish");
          //      fActivate();
                /*
                while(Data.oMsgForm == null) { //Null protection
					Thread.Sleep(1);
				}*/

                /*
				while(!oMainForm.bPositionSet) { //Wait initialise
					Thread.Sleep(1);
				}*/
			
	
	
				///////////////////////////
			
           




          //  }

        }



/*
        internal static void fActivate() {
           NativeMethods.SetForegroundWindow(NativeMethods.GetConsoleWindow()); //Activate console, Todo, after npp loaded
        }*/


    }
}
