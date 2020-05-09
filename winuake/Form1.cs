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
        //Constants for Window Styles
        public static uint MF_BYPOSITION = 0x400;
        public static uint MF_REMOVE = 0x1000;
        public static int GWL_STYLE = -16;
        public static int WS_CHILD = 0x40000000; //Child Window
        public static int WS_BORDER = 0x00800000; //Window with border
        public static int WS_DLGFRAME = 0x00400000; //Window with double border but no title
        public static int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //Window with a title bar 
        public static int WS_SYSMENU = 0x00080000; //Window menu

        //Constants for Window Redraw Flags
        public static uint RDW_INVALIDATE = 0x1;
        private const int WmPaint = 0x000F;
        public frmMain()
        {
            InitializeComponent();
        }
        private static Process p;
        private static IntPtr windowHandle;
        private void frmMain_Load(object sender, EventArgs e)
        {
            p = Process.Start("cmd.exe");
            //MessageBox.Show(p.MainWindowHandle.ToString());
            windowHandle = p.MainWindowHandle;
            Thread.Sleep(500);
            SetParent(p.MainWindowHandle, pnlOutput.Handle);
            MoveWindow(p.MainWindowHandle, 0, 0, pnlOutput.Width, pnlOutput.Height, true);
            SendMessage(windowHandle, WmPaint, IntPtr.Zero, IntPtr.Zero);
        }

        private void frmMain_ResizeEnd(object sender, EventArgs e)
        {
            pnlOutput.Height = this.Height;
            pnlOutput.Width = this.Width;
            //MoveWindow(windowHandle, 0, 0, pnlOutput.Width, pnlOutput.Height, true);
            //SendMessage(windowHandle, WmPaint, IntPtr.Zero, IntPtr.Zero);
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
    }
}
