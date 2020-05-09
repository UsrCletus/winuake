using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace winuake
{
    public partial class frmMain : Form
    {
        //Constants for Padding
        private const int PAD_WIDTH = -16;
        private const int PAD_HEIGHT = -40;

        //Constants for Window Styles
        private const uint MF_BYPOSITION = 0x400;
        private const uint MF_REMOVE = 0x1000;
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;
        public static int WS_CHILD = 0x40000000; //Child Window
        public static int WS_BORDER = 0x00800000; //Window with border
        public static int WS_DLGFRAME = 0x00400000; //Window with double border but no title
        public static int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //Window with a title bar 
        public static int WS_SYSMENU = 0x00080000; //Window menu
        private const int WS_THICKFRAME = 262144;
        private const int WS_MINIMIZE = 536870912;
        private const int WS_MAXIMIZEBOX = 65536;
        private const int WS_EX_DLGMODALFRAME = 0x1;
        private const int SWP_FRAMECHANGED = 0x20;

        //Constants for Window Resize
        private const short SWP_NOMOVE = 0X2;
        private const short SWP_NOSIZE = 1;
        private const short SWP_NOZORDER = 0X4;
        private const int SWP_SHOWWINDOW = 0x0040;

        //Constants for Window Redraw Flags
        public static uint RDW_INVALIDATE = 0x1;
        private const int WmPaint = 0x000F;
        public frmMain()
        {
            InitializeComponent();
        }
        private Process p = null;
        private void frmMain_Load(object sender, EventArgs e)
        {
            pnlOutput.Height = this.Bounds.Height;
            pnlOutput.Width = this.Bounds.Width;
            p = Process.Start("cmd.exe");
            Thread.Sleep(500);
            int style = GetWindowLong(p.MainWindowHandle, GWL_STYLE);
            style = style & ~WS_CAPTION;
            style = style & ~WS_SYSMENU;
            style = style & ~WS_THICKFRAME;
            style = style & ~WS_MINIMIZE;
            style = style & ~WS_MAXIMIZEBOX;
            SetWindowLong(p.MainWindowHandle, GWL_STYLE, style);
            style = GetWindowLong(p.MainWindowHandle, GWL_EXSTYLE);
            SetWindowLong(p.MainWindowHandle, GWL_EXSTYLE, style | WS_EX_DLGMODALFRAME);
            SetWindowPos(p.MainWindowHandle, 0, 0, 0, pnlOutput.Bounds.Width, pnlOutput.Bounds.Height, SWP_NOMOVE | SWP_NOSIZE | SWP_FRAMECHANGED);
            SetParent(p.MainWindowHandle, pnlOutput.Handle);
            MoveWindow(p.MainWindowHandle, 0, 0, pnlOutput.Width + PAD_WIDTH, pnlOutput.Height + PAD_HEIGHT, true);
            SendMessage(p.MainWindowHandle, WmPaint, IntPtr.Zero, IntPtr.Zero);
        }

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private void Resize()
        {
            pnlOutput.Height = this.Height;
            pnlOutput.Width = this.Width;
            MoveWindow(p.MainWindowHandle, 0, 0, pnlOutput.Width + PAD_WIDTH, pnlOutput.Height + PAD_HEIGHT, true);
            SendMessage(p.MainWindowHandle, WmPaint, IntPtr.Zero, IntPtr.Zero);
        }

        //Finds a window by class name
        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //Sets a window to be a child window of another window
        [DllImport("USER32.DLL")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        //Sets window attributes
        [DllImport("USER32.DLL")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //Gets window attributes
        [DllImport("USER32.DLL")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, int hwndInsertAfter, int X, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        static extern IntPtr GetMenu(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll")]
        static extern bool DrawMenuBar(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int width, int height, bool repaint);

        [DllImport("User32.dll")]
        public static extern Int64 SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            if ( p != null)
            {
                Resize();
            }
        }
    }
}
