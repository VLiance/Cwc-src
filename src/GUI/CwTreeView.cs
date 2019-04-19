using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace cwc {
    public class CwTreeView : Raccoom.Windows.Forms.TreeViewFolderBrowser{

         public CwScrollBar oVScroll = null;


        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        public CwTreeView():base() {
       
        }
       public void fSetTheme() {
            SetWindowTheme(this.Handle, "explorer", null);
        }

         public void MouseWheel(MouseEventArgs e){ OnMouseWheel(e);}
         protected override void OnMouseWheel(MouseEventArgs e){
             
            //  Invalidate();
            int _nVal = 0;
            if (e.Delta > 0) {
                _nVal = e.Delta / 3;
                if (_nVal < 1) {
                    _nVal = 1;
                }
            } else{
                 _nVal = e.Delta / 3;
                if (_nVal > -1) {
                    _nVal = -1;
                }
            }

            if(oVScroll != null){
                    oVScroll.OnScroll(_nVal);
            }
        } 
 
    }
}
