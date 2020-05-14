﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using Gma.System.MouseKeyHook;
using System.Collections.Generic;

namespace winuake
{
    public partial class frmMain : Form
    {
        private List<Process> listOfProcesses = new List<Process>();
        private List<TabPage> listOfTabPages = new List<TabPage>();

        private bool exiting = false;

        //Minimization Magic
        const int WS_MINIMIZEBOX = 0x20000;
        const int CS_DBLCLKS = 0x8;

        //For Mouse Key Hooks
        private IKeyboardMouseEvents m_GlobalHook;

        //Constants for Padding
        private const int PAD_WIDTH = -16;
        private const int PAD_HEIGHT = -40;

        //Constants for Window Styles
        private const uint MF_BYPOSITION = 0x400;
        private const uint MF_REMOVE = 0x1000;
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;
        public const int WS_CHILD = 0x40000000; //Child Window
        public const int WS_BORDER = 0x00800000; //Window with border
        public const int WS_DLGFRAME = 0x00400000; //Window with double border but no title
        public const int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //Window with a title bar 
        public const int WS_SYSMENU = 0x00080000; //Window menu
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
        private const uint RDW_INVALIDATE = 0x1;
        private const int WmPaint = 0x000F;

        //Constants for Focus
        private const int WM_SETFOCUS = 0x0007;
        private const int WM_KILLFOCUS = 0x0008;

        //Constant for process(es)
        //private Process p = null;
        private String shell = @"C:\Windows\sysnative\wsl.exe";
        private String startDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        //overrides minimize and maximize default behavior
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xf020;
        private const int SC_MAXIMIZE = 0xf030;

        //Keyboard Combination Vars
        //private 

        public frmMain()
        {
            InitializeComponent();
            Subscribe();
        }

        //override to allow dialog to think it has minimize capability, despite it's style.
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;
                return cp;
            }
        }

        private void ExitFunk(object sender, EventArgs e, TabPage tabOutput)
        {
            listOfTabPages.Remove(tabOutput);
            if (tabCtrl.InvokeRequired)
            {
                Thread exitThread = new Thread(delegate ()
                {
                    try
                    {
                        tabCtrl.Invoke(new MethodInvoker(delegate
                        {
                            if (tabCtrl.TabPages.Count > 1)
                            {
                                tabCtrl.TabPages.Remove(tabOutput);
                                focusLastTab();
                            }
                        }));
                    }catch(Exception err)
                    {
                        Console.WriteLine("Exception: " + err.ToString());
                    }
                });
                exitThread.Start();
            }
            else
            {
                if (tabCtrl.TabPages.Count > 1)
                {
                    tabCtrl.TabPages.Remove(tabOutput);
                    focusLastTab();
                }
            }

            if(listOfTabPages.Count < 1)
            {
                if (!exiting)
                {
                    if (tabCtrl.InvokeRequired)
                    {
                        Thread exitThread = new Thread(delegate ()
                        {
                            tabCtrl.Invoke(new MethodInvoker(delegate
                            {
                                tabCtrl.TabPages.Clear();
                                addTab();
                            }));
                        });
                        exitThread.Start();
                    }
                    else
                    {
                        tabCtrl.TabPages.Clear();
                        addTab();
                    }
                }
            }
        }

        private void FixSize()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                Show();
            }
            tabCtrl.Height = this.Bounds.Height-15;
            tabCtrl.Width = this.Bounds.Width;

            for(int i = 0; i < listOfProcesses.Count; i++)
            {
                Process p = listOfProcesses[i];
                MoveWindow(p.MainWindowHandle, 0, 0, tabCtrl.Width + PAD_WIDTH, tabCtrl.Height, true);
                SendMessage(p.MainWindowHandle, WmPaint, IntPtr.Zero, IntPtr.Zero);
            }

            
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Unsubscribe();
            for (int i = 0;i < listOfProcesses.Count; i++)
            {
                Process p = listOfProcesses[i];
                try
                {
                    p.Kill();
                }catch(Exception err)
                {
                    Console.WriteLine("Exception: " + err.ToString());
                }
                
            }
        }

        private void addProcess(TabPage tabOutput)
        {
            Process p = new Process();
            p.StartInfo.FileName = shell;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = startDir;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.EnableRaisingEvents = true;
            p.Exited += new EventHandler((s, e) => ExitFunk(s, e, tabOutput));
            p.Start();

            Thread.Sleep(500);

            //Set Window Style
            StyleWindow(p);

            //Focus on new Tab
            if (tabCtrl.InvokeRequired)
            {
                Thread exitThread = new Thread(delegate ()
                {
                    tabCtrl.Invoke(new MethodInvoker(delegate
                    {
                        tabCtrl.SelectedTab = tabOutput;
                        SetParent(p.MainWindowHandle, tabOutput.Handle);
                    }));
                });
                exitThread.Start();
            }
            else
            {
                tabCtrl.SelectedTab = tabOutput;
                SetParent(p.MainWindowHandle, tabOutput.Handle);
            }

            //Position the Window
            PositionWindow(p, tabOutput);
            SetForegroundWindow(p.MainWindowHandle);

            listOfProcesses.Add(p);
            listOfTabPages.Add(tabOutput);
        }

        private void focusLastTab()
        {
            //Focus on new Tab
            if (tabCtrl.InvokeRequired)
            {
                Thread exitThread = new Thread(delegate ()
                {
                    tabCtrl.Invoke(new MethodInvoker(delegate
                    {
                        tabCtrl.SelectedTab = tabCtrl.TabPages[tabCtrl.TabPages.Count-1];
                        SetParent(listOfProcesses[tabCtrl.TabPages.Count - 1].MainWindowHandle, tabCtrl.TabPages[tabCtrl.TabPages.Count - 1].Handle);
                        PositionWindow(listOfProcesses[tabCtrl.TabPages.Count - 1], tabCtrl.TabPages[tabCtrl.TabPages.Count - 1]);
                        SetForegroundWindow(listOfProcesses[tabCtrl.TabPages.Count - 1].MainWindowHandle);
                    }));
                });
                exitThread.Start();
            }
            else
            {
                tabCtrl.SelectedTab = tabCtrl.TabPages[tabCtrl.TabPages.Count-1];
                SetParent(listOfProcesses[tabCtrl.TabPages.Count - 1].MainWindowHandle, tabCtrl.TabPages[tabCtrl.TabPages.Count - 1].Handle);
                PositionWindow(listOfProcesses[tabCtrl.TabPages.Count - 1], tabCtrl.TabPages[tabCtrl.TabPages.Count - 1]);
                SetForegroundWindow(listOfProcesses[tabCtrl.TabPages.Count - 1].MainWindowHandle);
            }

            //Position the Window
            
            
        }

        private void addTab()
        {
            int tabNum = 0;
            bool foundFree = false;
            while (!foundFree)
            {
                tabNum += 1;
                if (tabNum < tabCtrl.TabCount + 1)
                {
                    bool foundMatch = false;
                    for (int i = 0; i < tabCtrl.TabCount; i++)
                    {
                        TabPage tabPage = tabCtrl.TabPages[i];
                        if (tabPage.Text.ToString().Contains(tabNum.ToString()))
                        {
                            foundMatch = true;
                        }
                    }
                    if (!foundMatch)
                    {
                        foundFree = true;
                    }
                }
                else
                {
                    foundFree = true;
                }
            }
            string tabTitle = "Shell" + (tabNum).ToString();
            TabPage newTab = new TabPage(tabTitle);
            if (tabCtrl.InvokeRequired)
            {
                Thread exitThread = new Thread(delegate ()
                {
                    tabCtrl.Invoke(new MethodInvoker(delegate
                    {
                        tabCtrl.TabPages.Add(newTab);
                        addProcess(newTab);
                    }));
                });
                exitThread.Start();
            }
            else
            {
                tabCtrl.TabPages.Add(newTab);
                addProcess(newTab);
            }
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.FromControl(this).Bounds.Height / 2;
            tabCtrl.Location = new Point(0, 0);
            tabCtrl.Height = this.Bounds.Height;
            tabCtrl.Width = this.Bounds.Width;
            this.FormBorderStyle = FormBorderStyle.None;
            this.CenterToScreen();
            this.Top = 0;
            tabCtrl.TabPages.Clear();
            addTab();
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
                FixSize();
        }

        private void GlobalHookKeyCtrlF1Press()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                Show();
                //PositionWindow(p);
                //tabOutput.Focus();
                //SetForegroundWindow(p.MainWindowHandle);
                notifyIcon.Visible = false;
            }
            else
            {
                //SendMessage(p.MainWindowHandle, WM_KILLFOCUS, IntPtr.Zero, IntPtr.Zero);
                Hide();
                this.WindowState = FormWindowState.Minimized;
                notifyIcon.Visible = true;
            }
        }

        private void GlobalHookKeyCtrlShiftF1Press()
        {
            //MessageBox.Show("Triggered");
            //tabOutput.Focus();
            //SendMessage(p.MainWindowHandle, WM_KILLFOCUS, IntPtr.Zero, IntPtr.Zero);
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void PositionWindow(Process p, TabPage tabOutput)
        {
            SetWindowPos(p.MainWindowHandle, 0, 0, 0, tabOutput.Bounds.Width, tabOutput.Bounds.Height, SWP_NOMOVE | SWP_NOSIZE | SWP_FRAMECHANGED);
            MoveWindow(p.MainWindowHandle, 0, 0, tabOutput.Width, tabOutput.Height, true);
            SendMessage(p.MainWindowHandle, WmPaint, IntPtr.Zero, IntPtr.Zero);
        }

        private void StyleWindow(Process p)
        {
            int style = GetWindowLong(p.MainWindowHandle, GWL_STYLE);
            style = style & ~WS_CAPTION;
            style = style & ~WS_SYSMENU;
            style = style & ~WS_THICKFRAME;
            style = style & ~WS_MINIMIZE;
            style = style & ~WS_MAXIMIZEBOX;
            SetWindowLong(p.MainWindowHandle, GWL_STYLE, style);
            style = GetWindowLong(p.MainWindowHandle, GWL_EXSTYLE);
            SetWindowLong(p.MainWindowHandle, GWL_EXSTYLE, style | WS_EX_DLGMODALFRAME);
        }

        private void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;

            //Detect Ctrl + F12
            m_GlobalHook.OnCombination(new Dictionary<Combination, Action>() {
                { Combination.FromString("Control+Shift+F1"), GlobalHookKeyCtrlShiftF1Press },
            });
            m_GlobalHook.OnCombination(new Dictionary<Combination, Action>() {
                { Combination.FromString("Control+F1"), GlobalHookKeyCtrlF1Press },
            });
        }

        private void Unsubscribe()
        {
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (m.WParam.ToInt32() == SC_MINIMIZE)
                {
                    m.Result = IntPtr.Zero;
                    GlobalHookKeyCtrlF1Press();
                    return;
                }
                else if (m.WParam.ToInt32() == SC_MAXIMIZE)
                {
                    //my code for the maximize event
                    return;
                }
            }
            base.WndProc(ref m);
        }

        //Set Foreground Window
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        //Set Focus to Window
        [DllImport("user32.dll")]
        private static extern IntPtr SetFocus(IntPtr hwnd);

        //Move Window
        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int width, int height, bool repaint);

        //Sets a window to be a child window of another window
        [DllImport("USER32.DLL")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        //Sets window attributes
        [DllImport("USER32.DLL")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //Gets window attributes
        [DllImport("USER32.DLL")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        //Finds a window by class name
        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //Finds a window by it's caption
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        //Redraws a window
        [DllImport("user32.dll")]
        static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

        //Sends a message to a Window
        [DllImport("User32.dll")]
        public static extern Int64 SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            exiting = true;
            Application.Exit();
        }

        private void btnAddTab_Click(object sender, EventArgs e)
        {
            addTab();
        }
    }
}
