using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cwc {
     public class DoubleBufferedPanel : Panel {       
        
        


     public DoubleBufferedPanel(): base() { DoubleBuffered = true; }

    // public CwScrollBar oVScroll = null;
     public CwTreeView oTreeView = null;
        
     protected override void OnMouseWheel(MouseEventArgs e){
        if(oTreeView != null){ oTreeView.MouseWheel(e);}
    }

    [DefaultValue(true)]
    public new bool DoubleBuffered
    {
        get
        {
            return base.DoubleBuffered;
        }
        set
        {
            base.DoubleBuffered = value;
        }


    }
}
}
