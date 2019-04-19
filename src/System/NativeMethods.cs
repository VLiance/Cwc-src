using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace cwc
{
	public class NativeMethods
	{


   [DllImport("user32.dll")]
     public static extern bool SetWindowText(IntPtr hWnd, string text);


	[DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

    private struct WINDOWPLACEMENT {
        public int length;
        public int flags;
        public int showCmd;
        public System.Drawing.Point ptMinPosition;
        public System.Drawing.Point ptMaxPosition;
        public System.Drawing.Rectangle rcNormalPosition;
    }

		public  static int fGetWindowPlacement(IntPtr _nHandle){
			if (_nHandle != IntPtr.Zero) {
				
				WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
				GetWindowPlacement(_nHandle, ref placement);
				switch (placement.showCmd) {
					case 1:
						//Console.WriteLine("Normal");
						return 1;//Normal
					case 2:
					//	Console.WriteLine("Minimized");
						return 2;//Minimized
					case 3:

						return 3;//Maximized
				}
			}   
			return 0;//Error               
		}








			 public const Int32 STD_INPUT_HANDLE = -10;

            public const Int32 ENABLE_MOUSE_INPUT = 0x0010;
            public const Int32 ENABLE_QUICK_EDIT_MODE = 0x0040;
            public const Int32 ENABLE_EXTENDED_FLAGS = 0x0080;

            public const Int32 KEY_EVENT   = 0x0001;
            public const Int32 MOUSE_EVENT = 0x0002;
            public const Int32 WINDOW_BUFFER_SIZE_EVENT = 0x0004;
            public const Int32 MENU_EVENT  = 0x0008;
            public const Int32 FOCUS_EVENT = 0x0010;






public static Size GetConsoleFontSize(){
		// getting the console out buffer handle
		IntPtr outHandle = CreateFile("CONOUT$", GENERIC_READ | GENERIC_WRITE, 
			FILE_SHARE_READ | FILE_SHARE_WRITE,
			IntPtr.Zero,
			OPEN_EXISTING,
			0,
			IntPtr.Zero);
		int errorCode = Marshal.GetLastWin32Error();
		if (outHandle.ToInt32() == INVALID_HANDLE_VALUE)
		{
			throw new IOException("Unable to open CONOUT$", errorCode);
		}

		ConsoleFontInfo cfi = new ConsoleFontInfo();
		if (!GetCurrentConsoleFont(outHandle, false, cfi))
		{
			throw new InvalidOperationException("Unable to get font information.");
		}

		return new Size(cfi.dwFontSize.X, cfi.dwFontSize.Y);            
	}


[DllImport("kernel32.dll", SetLastError = true)]
private static extern IntPtr CreateFile(
    string lpFileName,
    int dwDesiredAccess,
    int dwShareMode,
    IntPtr lpSecurityAttributes,
    int dwCreationDisposition,
    int dwFlagsAndAttributes,
    IntPtr hTemplateFile);

[DllImport("kernel32.dll", SetLastError = true)]
private static extern bool GetCurrentConsoleFont(
    IntPtr hConsoleOutput,
    bool bMaximumWindow,
    [Out][MarshalAs(UnmanagedType.LPStruct)]ConsoleFontInfo lpConsoleCurrentFont);

[StructLayout(LayoutKind.Sequential)]
internal class ConsoleFontInfo
{
    internal int nFont;
    internal Coord dwFontSize;
}

[StructLayout(LayoutKind.Explicit)]
internal struct Coord
{
    [FieldOffset(0)]
    internal short X;
    [FieldOffset(2)]
    internal short Y;
}

private const int GENERIC_READ = unchecked((int)0x80000000);
private const int GENERIC_WRITE = 0x40000000;
private const int FILE_SHARE_READ = 1;
private const int FILE_SHARE_WRITE = 2;
private const int INVALID_HANDLE_VALUE = -1;
private const int OPEN_EXISTING = 3;





            [DebuggerDisplay("EventType: {EventType}")]
            [StructLayout(LayoutKind.Explicit)]
            public struct INPUT_RECORD {
                [FieldOffset(0)]
                public Int16 EventType;
                [FieldOffset(4)]
                public KEY_EVENT_RECORD KeyEvent;
                [FieldOffset(4)]
                public MOUSE_EVENT_RECORD MouseEvent;
			   [FieldOffset(4)]
                public FOCUS_EVENT_RECORD FocusEvent;
				[FieldOffset(4)]
                public MENU_EVENT_RECORD MenuEvent;
				[FieldOffset(4)]
                public WINDOW_BUFFER_SIZE_RECORD WindowsBufferSizeEvent;
            }

            [DebuggerDisplay("{dwMousePosition.X}, {dwMousePosition.Y}")]
            public struct MOUSE_EVENT_RECORD {
                public COORD dwMousePosition;
                public Int32 dwButtonState;
                public Int32 dwControlKeyState;
                public Int32 dwEventFlags;
            }

            [DebuggerDisplay("{X}, {Y}")]
            public struct COORD {
                public UInt16 X;
                public UInt16 Y;
            }

            [DebuggerDisplay("{Focus}")]
            public struct FOCUS_EVENT_RECORD {
                public bool bSetFocus;
            }
			
			[DebuggerDisplay("{CommandID}")]
            public struct MENU_EVENT_RECORD {
                public UInt32 dwCommandId;
            }

			[DebuggerDisplay("{WindowBufferSize}")]
            public struct WINDOW_BUFFER_SIZE_RECORD {
                public COORD dwSize;
            }

            [DebuggerDisplay("KeyCode: {wVirtualKeyCode}")]
            [StructLayout(LayoutKind.Explicit)]
            public struct KEY_EVENT_RECORD {
                [FieldOffset(0)]
                [MarshalAsAttribute(UnmanagedType.Bool)]
                public Boolean bKeyDown;
                [FieldOffset(4)]
                public UInt16 wRepeatCount;
                [FieldOffset(6)]
                public UInt16 wVirtualKeyCode;
                [FieldOffset(8)]
                public UInt16 wVirtualScanCode;
                [FieldOffset(10)]
                public Char UnicodeChar;
                [FieldOffset(10)]
                public Byte AsciiChar;
                [FieldOffset(12)]
                public Int32 dwControlKeyState;
            };
/*
            public class ConsoleHandle : SafeHandleMinusOneIsInvalid {
                public ConsoleHandle() : base(false) { }

                protected override bool ReleaseHandle() {
                    return true; //releasing console handle is not our business
                }
            }*/


            [DllImport("user32.dll")]
            public static extern bool GetCursorPos(out Point lpPoint);


            [DllImportAttribute("kernel32.dll", SetLastError = true)]
            [return: MarshalAsAttribute(UnmanagedType.Bool)]
            public static extern Boolean GetConsoleMode(IntPtr hConsoleHandle, ref Int32 lpMode);
/*
            [DllImportAttribute("kernel32.dll", SetLastError = true)]
            public static extern ConsoleHandle GetStdHandle(Int32 nStdHandle);
*/
            [DllImportAttribute("kernel32.dll", SetLastError = true)]
            [return: MarshalAsAttribute(UnmanagedType.Bool)]
            public static extern Boolean ReadConsoleInput(IntPtr hConsoleInput, ref INPUT_RECORD lpBuffer, UInt32 nLength, ref UInt32 lpNumberOfEventsRead);

            [DllImportAttribute("kernel32.dll", SetLastError = true)]
            [return: MarshalAsAttribute(UnmanagedType.Bool)]
            public static extern Boolean SetConsoleMode(IntPtr hConsoleHandle, Int32 dwMode);

			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool SetForegroundWindow(IntPtr hWnd);
  
    //    [DllImport("user32")]
    //    private static extern Boolean GetClientRect(IntPtr hWnd, ref Rectangle rect);


			[DllImport("user32.dll")]
			public static extern IntPtr SetParent(IntPtr hWndChild,IntPtr hWndNewParent);


			[DllImport("kernel32.dll", ExactSpelling = true)]
			public static extern IntPtr GetConsoleWindow();

			


    
	}
}
