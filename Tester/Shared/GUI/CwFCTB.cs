using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cwc {




    public class CwFCTB: FastColoredTextBox {

       public int SetStyleLayerIndex(Style style) {
            int i = GetStyleIndex(style);
            if (i < 0)
                i = AddStyle(style);
            return i;
        }

       public static Color fGetColor(int _nIndex ){
                switch (_nIndex) {
                    case 0: return Color.Transparent; break;
                    case 1: return Color.DarkBlue; break;
                    case 2: return Color.DarkGreen; break;
                    case 3: return Color.DarkMagenta; break;
                    case 4: return Color.DarkRed; break;
                    case 5: return Color.DeepPink; break;
                    case 6: return Color.BurlyWood; break;
                    case 7: return Color.LightGray; break;
                    case 8: return Color.Gray; break;
                    case 9: return Color.DarkViolet; break;
                     case 10: return  Color.Green; break;
                     case 11: return  Color.LightBlue; break;
                     case 12: return  Color.Red; break;
                     case 13: return  Color.Pink; break;
                     case 14: return  Color.Yellow; break;
                   default:  case 15: return  Color.White; break;
                }
      }



       public static Brush fGetBrush(int _nIndex ){
                switch (_nIndex) {
                    case 0: return Brushes.Black; break;
                    case 1: return Brushes.DarkBlue; break;
                    case 2: return Brushes.DarkGreen; break;
                    case 3: return Brushes.DarkMagenta; break;
                    case 4: return Brushes.DarkRed; break;
                    case 5: return Brushes.DeepPink; break;
                    case 6: return Brushes.BurlyWood; break;
                    case 7: return Brushes.LightGray; break;
                    case 8: return Brushes.Gray; break;
                    case 9: return Brushes.DarkViolet; break;
                     case 10: return  Brushes.Green; break;
                     case 11: return  Brushes.LightBlue; break;
                     case 12: return  Brushes.Red; break;
                     case 13: return  Brushes.Pink; break;
                     case 14: return  Brushes.Yellow; break;
                   default:  case 15: return  Brushes.White; break;
                }
      }


 

      //  Range oLastRange = new Range();

        public CwRange AddText(string text, Style style, int _nFore, Style _oBackStyle = null, int  _nBack = 0) {
          //  AppendText(text,style);

            CwRange _oRange;
            if (text == null)
                return null;

            Selection.ColumnSelectionMode = false;

            Place oldStart = Selection.Start;
            Place oldEnd = Selection.End;

            Selection.BeginUpdate();
            TextSource.Manager.BeginAutoUndoCommands();
            try
            {
                if (TextSource.Count > 0)
                    Selection.Start = new Place(TextSource[TextSource.Count - 1].Count, TextSource.Count - 1);
                else
                    Selection.Start = new Place(0, 0);

                //remember last caret position
                Place last = Selection.Start;

                TextSource.Manager.ExecuteCommand(new InsertTextCommand(TextSource, text));


                _oRange =  new CwRange(this, last, Selection.Start);
                _oRange.SetStyles(style, (char)_nFore, _oBackStyle, (char)_nBack);
               // _oRange.SetStyle(GuiConsole.oLinkStyle);


             //   CwRange _oCwRage = new CwRange(this,new Place(),null);
             ///   Range _oRange  = new Range(this);
                /*
                
                if (_oBackStyle != null) {
                    new Range(this, last, Selection.Start).SetStyle(_oBackStyle);
                }
                if (style != null){
                   new Range(this, last, Selection.Start).SetStyle(style);
               //   new CwRange(this, last, Selection.Start).SetStyles(style, (char)_nFore, _oBackStyle, (char)2);
                }
                
                */



                }
            finally
            {
                TextSource.Manager.EndAutoUndoCommands();
                Selection.Start = oldStart;
                Selection.End = oldEnd;
                Selection.EndUpdate();
            }
            //
            Invalidate();
            return _oRange;
        }
    }



      public class CwRange : Range {

            public  CwFCTB oFCTB() {
             return (CwFCTB)tb;
        }
         public CwRange(FastColoredTextBox tb, Place start, Place end) : base(tb,start,end){ }
         public CwRange(FastColoredTextBox tb, int iStartChar, int iStartLine, int iEndChar, int iEndLine):base(tb,iStartChar,iStartLine,iEndChar,iEndLine){ }
       

        public  CwRange fSelectRange(int _nStartCol, int _nEndCol) {
             return fSelectRange( Start.iLine,Start.iLine, Start.iChar + _nStartCol, Start.iChar + _nEndCol);
          //   return fSelectRange( Start.iLine,End.iLine, _nStartCol, _nEndCol);

        }

         public  CwRange fSelectRange(int _nStartLine,int _nEndLine, int _nStartCol, int _nEndCol) {
             Start = new Place(  _nStartCol ,_nStartLine);
             End =  new Place(  _nEndCol ,   _nEndLine);
             return this;
        }


        public void SetStyles(Style _frontStyle, char _nFrontColor, Style _backStyle, char _nBackColor) {
            //set code to chars
            SetStyles( ToStyleIndex(GetOrSetStyleLayerIndex(_frontStyle)), _nFrontColor, ToStyleIndex(GetOrSetStyleLayerIndex(_backStyle)), _nBackColor );
            //
            tb.Invalidate();
        }
         public int GetOrSetStyleLayerIndex(Style style) {
            int i = tb.GetStyleIndex(style);
            if (i < 0)
                i = tb.AddStyle(style);
            return i;
        }
        
        /// <summary>
        /// Appends style to chars of range
        /// </summary>
        public void SetStyles(StyleIndex _oFrontIndex, int _nFrontColor, StyleIndex _oBackIndex, int _nBackColor )
        {
            //set code to chars
            int fromLine = Math.Min(End.iLine, Start.iLine);
            int toLine = Math.Max(End.iLine, Start.iLine);
            int fromChar = FromX;
            int toChar = ToX;
            if (fromLine < 0) return;
            //
            for (int y = fromLine; y <= toLine; y++)
            {
                int fromX = y == fromLine ? fromChar : 0;
                int toX = y == toLine ? Math.Min(toChar - 1, tb[y].Count - 1) : tb[y].Count - 1;
                for (int x = fromX; x <= toX; x++)
                {
                    FastColoredTextBoxNS.Char c = tb[y][x];

                   c.style |= _oFrontIndex | _oBackIndex;
               //     c.style |= _oFrontIndex ;
                    c.nCustomData = (ushort)(_nFrontColor | ( _nBackColor << 4));
                    tb[y][x] = c;
                }
            }
        }


        public static ushort fGetStyleData(Range range) {
            return range.tb.TextSource[range.FromLine][range.Start.iChar].nCustomData;
          //  return range.tb.TextSource[range.FromLine][range.Start.iChar+1].nCustomData;
        }


    }








        /// <summary>
    /// Marker style
    /// Draws background color for text
    /// </summary>
    public class LinkStyle : Style
    {
    //    public Brush BackgroundBrush{get;set;}

        public LinkStyle(CwFCTB _Fctb,  Brush underlineBrush)
        {
             _Fctb.SetStyleLayerIndex(this);


           // this.BackgroundBrush = backgroundBrush;
            IsExportable = true;
        }

        public override void Draw(Graphics gr, Point position, Range range)
        {

        //   ushort _nData =  range.tb.TextSource[range.FromLine][range.Start.iChar].nCustomData;
           ushort _nData = CwRange.fGetStyleData(range);
       //    char _nBackData =  (char)((_nData & 0x00F0) >> 4);

            //draw background
            Brush backgroundBrush = Brushes.Aquamarine;
            if (backgroundBrush != Brushes.Black) // TODO or same as background color
            {
               // Rectangle rect = new Rectangle(position.X, position.Y, (range.End.iChar - range.Start.iChar) * range.tb.CharWidth+1, range.tb.CharHeight);
          //      float _nHeight = (((float)(range.tb.CharHeight)) *0.90f);
                float _nHeight = (int)(((range.tb.CharHeight)) *0.90f);
                if (range.tb.CharHeight < 10.0f) {
                    _nHeight+=0.5f;
                }


              //  Rectangle rect = new Rectangle(position.X, position.Y + _nHeight, (range.End.iChar - range.Start.iChar) * range.tb.CharWidth+1, 1);
             //   Line line = new Line(position.X, position.Y + (int)((double)(range.tb.CharHeight) *0.80), (range.End.iChar - range.Start.iChar) * range.tb.CharWidth+1, 1);
       
                
                Pen pen = new Pen(  Color.DarkGoldenrod);
               // Pen pen = new Pen(  Color.DarkCyan);

                gr.DrawLine(pen, (float)position.X, (float)position.Y+ _nHeight,  (float)position.X +  (range.End.iChar - range.Start.iChar) * range.tb.CharWidth+1, (float)position.Y+ _nHeight);


                
            //    if (rect.Width == 0) 
             //       return;

              // gr.FillRectangle(backgroundBrush, rect);

            }
        }

        public override string GetCSS()
        {
            string result = "";

           // if (BackgroundBrush is SolidBrush)
           // {
           /*
                var s = ExportToHTML.GetColorAsString((BackgroundBrush as SolidBrush).Color);
                if (s != "")
                    result += "background-color:" + s + ";";
                    */
         //   }

            return result;
        }
    }


      /// <summary>
    /// Marker style
    /// Draws background color for text
    /// </summary>
    public class HighlightStyle : Style
    {
        public Brush BackgroundBrush{get;set;}

        public HighlightStyle(CwFCTB _Fctb,  Brush backgroundBrush)
        {
             _Fctb.SetStyleLayerIndex(this);


           this.BackgroundBrush = backgroundBrush;
            IsExportable = true;
        }

        public override void Draw(Graphics gr, Point position, Range range)
        {

        //   ushort _nData =  range.tb.TextSource[range.FromLine][range.Start.iChar].nCustomData;


            //draw background
           // Brush backgroundBrush = CwFCTB.fGetBrush(_nBackData);
            if (BackgroundBrush != Brushes.Black) // TODO or same as background color
            {
                Rectangle rect = new Rectangle(position.X, position.Y, (range.End.iChar - range.Start.iChar) * range.tb.CharWidth+1, range.tb.CharHeight);
                if (rect.Width == 0) 
                    return;

            
               gr.FillRectangle(BackgroundBrush, rect);

            }
        }

        public override string GetCSS()
        {
            string result = "";

           // if (BackgroundBrush is SolidBrush)
           // {
           /*
                var s = ExportToHTML.GetColorAsString((BackgroundBrush as SolidBrush).Color);
                if (s != "")
                    result += "background-color:" + s + ";";
                    */
         //   }

            return result;
        }
    }




      /// <summary>
    /// Marker style
    /// Draws background color for text
    /// </summary>
    public class BackgroundStyle : Style
    {
    //    public Brush BackgroundBrush{get;set;}

        public BackgroundStyle(CwFCTB _Fctb,  Brush backgroundBrush)
        {
             _Fctb.SetStyleLayerIndex(this);


           // this.BackgroundBrush = backgroundBrush;
            IsExportable = true;
        }

        public override void Draw(Graphics gr, Point position, Range range)
        {

        //   ushort _nData =  range.tb.TextSource[range.FromLine][range.Start.iChar].nCustomData;
           ushort _nData = CwRange.fGetStyleData(range);
           char _nBackData =  (char)((_nData & 0x00F0) >> 4);

            //draw background
            Brush backgroundBrush = CwFCTB.fGetBrush(_nBackData);
            if (backgroundBrush != Brushes.Black) // TODO or same as background color
            {
                Rectangle rect = new Rectangle(position.X, position.Y, (range.End.iChar - range.Start.iChar) * range.tb.CharWidth+1, range.tb.CharHeight);
                if (rect.Width == 0) 
                    return;

            
               gr.FillRectangle(backgroundBrush, rect);

            }
        }

        public override string GetCSS()
        {
            string result = "";

           // if (BackgroundBrush is SolidBrush)
           // {
           /*
                var s = ExportToHTML.GetColorAsString((BackgroundBrush as SolidBrush).Color);
                if (s != "")
                    result += "background-color:" + s + ";";
                    */
         //   }

            return result;
        }
    }






    /// <summary>
    /// Style for chars rendering
    /// This renderer can draws chars, with defined fore and back colors
    /// </summary>
    public class ForeStyle : TextStyle
  //  public class ForeStyle : Style
    {
    //    public Brush ForeBrush { get; set; }
   //     public Brush BackgroundBrush { get; set; }
        public FontStyle FontStyle { get; set; }
        //public readonly Font Font;
        public StringFormat stringFormat;

   //   public ForeStyle(FontStyle fontStyle)
        public ForeStyle(CwFCTB _Fctb, FontStyle fontStyle):base(null,null,fontStyle)
        {
           //  Fctb.SetStyleLayerIndex(this);
                 _Fctb.SetStyleLayerIndex(this);

        //    this.ForeBrush = foreBrush;
          //  this.BackgroundBrush = backgroundBrush;
            this.FontStyle = fontStyle;
            stringFormat = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);
        }

        public override void Draw(Graphics gr, Point position, Range range)
        {

           ushort _nData = CwRange.fGetStyleData(range);
           char _nForeData =  (char)((_nData & 0x000F) );



            //draw background
           // if (BackgroundBrush != null)
          //      gr.FillRectangle(BackgroundBrush, position.X, position.Y, (range.End.iChar - range.Start.iChar) * range.tb.CharWidth, range.tb.CharHeight);
            //draw chars
            using(var f = new Font(range.tb.Font, FontStyle))
            {
                Line line = range.tb[range.Start.iLine];
                float dx = range.tb.CharWidth;
                float y = position.Y + range.tb.LineInterval/2;
                float x = position.X - range.tb.CharWidth/3;

               
                Brush  ForeBrush  = CwFCTB.fGetBrush(_nForeData);

                if (ForeBrush == null)
                    ForeBrush = new SolidBrush(range.tb.ForeColor);


              //  ForeBrush = Brushes.Blue;

                if (range.tb.ImeAllowed)
                {           
                    //IME mode
                    for (int i = range.Start.iChar; i < range.End.iChar; i++)
                    {
                        SizeF size = FastColoredTextBox.GetCharSize(f, line[i].c);

                        var gs = gr.Save();
                        float k = size.Width > range.tb.CharWidth + 1 ? range.tb.CharWidth/size.Width : 1;
                        gr.TranslateTransform(x, y + (1 - k)*range.tb.CharHeight/2);
                        gr.ScaleTransform(k, (float) Math.Sqrt(k));
                        gr.DrawString(line[i].c.ToString(), f, ForeBrush, 0, 0, stringFormat);
                        gr.Restore(gs);
                        x += dx;
           
                    }
                }
                else
                {  


                   
              //         gr.DrawString(   range.Text, f, ForeBrush, x, y, stringFormat);


                    TextRenderer.DrawText(gr,range.Text, f, new Point((int) x, (int)y), CwFCTB.fGetColor(_nForeData), Color.Transparent );


                    /*
                    //classic mode 
                    for (int i = range.Start.iChar; i < range.End.iChar; i++) {
                        //draw char
                        gr.DrawString(line[i].c.ToString(), f, ForeBrush, x, y, stringFormat);
                        x += dx;
                    }
                    */


                }
                
            }
        }

        public override string GetCSS()
        {
            string result = "";
            /*
            if (BackgroundBrush is SolidBrush)
            {
                var s =  ExportToHTML.GetColorAsString((BackgroundBrush as SolidBrush).Color);
                if (s != "")
                    result += "background-color:" + s + ";";
            }*/
            /*
            if (ForeBrush is SolidBrush)
            {
                var s = ExportToHTML.GetColorAsString((ForeBrush as SolidBrush).Color);
                if (s != "")
                    result += "color:" + s + ";";
            }
            if ((FontStyle & FontStyle.Bold) != 0)
                result += "font-weight:bold;";
            if ((FontStyle & FontStyle.Italic) != 0)
                result += "font-style:oblique;";
            if ((FontStyle & FontStyle.Strikeout) != 0)
                result += "text-decoration:line-through;";
            if ((FontStyle & FontStyle.Underline) != 0)
                result += "text-decoration:underline;";
                */
            return result;
        }

        public override RTFStyleDescriptor GetRTF()
        {
            var result = new RTFStyleDescriptor();

          //  if (BackgroundBrush is SolidBrush)
           //     result.BackColor = (BackgroundBrush as SolidBrush).Color;
            /*
            if (ForeBrush is SolidBrush)
                result.ForeColor = (ForeBrush as SolidBrush).Color;
            
            if ((FontStyle & FontStyle.Bold) != 0)
                result.AdditionalTags += @"\b";
            if ((FontStyle & FontStyle.Italic) != 0)
                result.AdditionalTags += @"\i";
            if ((FontStyle & FontStyle.Strikeout) != 0)
                result.AdditionalTags += @"\strike";
            if ((FontStyle & FontStyle.Underline) != 0)
                result.AdditionalTags += @"\ul";
                */
            return result;
        }
    }




}
