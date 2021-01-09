using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cwc {
    public class CwScrollBar: Control
    {

        public  int nDelta = 0;
        public  bool bDown = false;


        private int @value;

        public int Value
        {
            get { return value; }
            set {
                if (this.value == value)
                    return;
                this.value = value;
              //  Invalidate();
             //   OnScroll();
            }
        }

        private int maximum = 0;
        public int Maximum
        {
            get { return maximum; }
            set { maximum = value; Invalidate(); }
        }

        public int thumbSize = 1000;
        public int ThumbSize
        {
            get { return thumbSize; }
            set { thumbSize = value; Invalidate(); }
        }

        private Color thumbColor = Color.LightGray;
        public Color ThumbColor
        {
            get { return thumbColor; }
            set { thumbColor = value; Invalidate(); }
        }

        private Color borderColor = Color.WhiteSmoke;
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        private ScrollOrientation orientation;
        public ScrollOrientation Orientation
        {
            get { return orientation; }
            set { orientation = value; Invalidate(); }
        }

        public event ScrollEventHandler Scroll;

        public CwScrollBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);


        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
              bDown = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left){

                switch(Orientation) {
                    case ScrollOrientation.VerticalScroll:   nDelta =   e.Y - (int)fGetCenter(Height); break;
                    case ScrollOrientation.HorizontalScroll: nDelta =   e.X - (int)fGetCenter(Width); break;
                }

                bDown = true;
                MouseScroll(e);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                MouseScroll(e);
            base.OnMouseMove(e);
        }

        /*
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                OnScroll(ScrollEventType.EndScroll);
            base.OnMouseUp(e);
        }*/

        private void MouseScroll(MouseEventArgs e)
        {
            if(bDown){
            int v = 0;
            switch(Orientation)
            {
                case ScrollOrientation.VerticalScroll: v = Maximum * (e.Y - nDelta - thumbSize / 2) / (Height - thumbSize) ; break;
                case ScrollOrientation.HorizontalScroll: v = Maximum * (e.X - nDelta - thumbSize / 2) / (Width - thumbSize); break;
            }
            Value = Math.Max(0, Math.Min(Maximum, v));
            }
            Invalidate();
            OnScroll();

        }

        public double fGetCenter(int _nDim) {

            return ( (Value) * (_nDim - thumbSize) / Maximum) + thumbSize/2;
        }


        public virtual void OnScroll(int _nVal)
        {
            if (Scroll != null){
                Value+= _nVal *-1;
                if (Value< 0) {
                    Value = 0;
                }
                 if (Value > Maximum) {
                    Value = Maximum;
                }

                Scroll(this, new ScrollEventArgs(ScrollEventType.ThumbPosition,Value, Orientation));
            }
        }

        public virtual void OnScroll(ScrollEventType type = ScrollEventType.ThumbPosition)
        {
            if (Scroll != null)
                Scroll(this, new ScrollEventArgs(type, Value, Orientation));
        }


        public bool bFirst = true;

        public  double nMinimalSize = 30;
        public  double nToTumbSize = 0;
        public  double nToPos = 0;
        public  double nPos = 0;
        public  double nTumbSize = 0;
        protected override void OnPaint(PaintEventArgs e)
        {
           //    Console.WriteLine("Maximum " +  Maximum);
            if (bFirst) {
                bFirst = false;
                if(Height > Width) {
                  nTumbSize = Height/2.0;
                } else {
                    nTumbSize = Width/2.0;
                }
                nToTumbSize = nTumbSize;
              //  return;
            }

            /*
            if (Maximum <= 0){
                return;
            }*/

            Rectangle thumbRect = Rectangle.Empty;
            switch(Orientation)
            {
                case ScrollOrientation.HorizontalScroll:
                    nToTumbSize  = Width/2.0-Maximum/20;
                    thumbSize = (int) nTumbSize;
                    if (nToTumbSize < nMinimalSize) { //Minimal
                      nToTumbSize = nMinimalSize;
                    }
                
                      thumbRect = new Rectangle( (int)(nPos) * (Width - thumbSize) / Maximum, 2, thumbSize, Height - 4);
                
                    //  thumbRect = new Rectangle(value * (Width - thumbSize) / Maximum, 2, thumbSize, Height - 4);
                    break;
                case ScrollOrientation.VerticalScroll:

                    nToTumbSize  = Height/2.0-Maximum/20;
                    thumbSize = (int) nTumbSize;
                    if (nToTumbSize < nMinimalSize) { //Minimal
                      nToTumbSize = nMinimalSize;
                    }

                  //  nToPos = value;
                 
                   // thumbRect = new Rectangle(2, (value) * (Height - thumbSize) / Maximum, Width - 4, thumbSize);
                    thumbRect = new Rectangle(2, (int)(nPos) * (Height - thumbSize) / Maximum, Width - 4, thumbSize);
                    break;
            }
                // Console.WriteLine("Paint " +  nPos);

            using(var brush = new SolidBrush(Color.FromArgb(60,66,70)))
                e.Graphics.FillRectangle(brush, thumbRect);

            using (var pen = new Pen(Color.DarkGray))
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
        }

        internal void fUpdate() {   
			 try { 
                this.BeginInvoke((MethodInvoker)delegate  {
                      if(Base.bAlive){
                            nPos += (value  - nPos) / 5;

                            nTumbSize  +=  (nToTumbSize - nTumbSize )/ 10.0;
                            if(Math.Abs(nTumbSize  - nToTumbSize) >= 1.0 ||  Math.Abs(nPos  - value) >= 1.0){
                               this.Refresh();
                            }
                        }
                   
                });
			}catch( Exception e) { }
            
        }
    }
}
