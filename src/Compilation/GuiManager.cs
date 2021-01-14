using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cwc {
    class GuiManager {
        	public static void fCreateGUI()  {

				   Empty.bHaveMsgForm = true;


				 Thread winThread = new Thread(new ThreadStart(() =>  {  

					 try { 
                                Application.EnableVisualStyles();
                            //    Application.SetCompatibleTextRenderingDefault(false); //Bug in W10
                               GuiConsole _oConsole =   new GuiConsole();
                                Application.Run(_oConsole);

					 }catch(Exception Ex) {
                         Output.TraceError("Error: " + Ex.Message + " : " +Ex.Source  + " : " +Ex.StackTrace);
					 }
				  }));  

                    
				  winThread.SetApartmentState(ApartmentState.STA);  
				//  winThread.IsBackground = true;  
				  winThread.Start();

               Thread.CurrentThread.Join(1);
                while(   Base.bAlive && Data.oGuiConsole== null) {
                       Thread.CurrentThread.Join(1);
                }

				///////////////////////////
			
        }



    }
}
