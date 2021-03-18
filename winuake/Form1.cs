using System;
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
        private const int PAD_HEIGHT = -10;
        //Constants for Window Styles
        private const uint MF_BYPOSITION = 0x400;
        private const uint MF_REMOVE = 0x1000;
        public const int WS_SIZEBOX = 0x00040000;
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
        //private String shell = @"C:\Windows\System32\cmd.exe";
        //private String shell = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe";
        private String startDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        //overrides minimize and maximize default behavior
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xf020;
        private const int SC_MAXIMIZE = 0xf030;
        //constants for hiding and showing windows
        private const int SW_SHOW = 5;
        private const int SW_HIDE = 0;

        private bool menuOpen = false;

        private FormWindowState lastState;

        [DllImport("user32.dll")]
        static extern bool DrawMenuBar(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
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
        static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);        

        public frmMain()
        {
            InitializeComponent();
            Subscribe();
        }
        private void addProcess(TabPage tabOutput)
        {
            Process p = new Process();
            p.StartInfo.FileName = shell;
            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            p.StartInfo.WorkingDirectory = startDir;
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
                        //SetParent(p.MainWindowHandle, tabOutput.Handle);
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
        private void btnAddTab_Click(object sender, EventArgs e)
        {
            addTab();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            exiting = true;
            Application.Exit();
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
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
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
                        if (!exiting)
                        {
                            tabCtrl.Invoke(new MethodInvoker(delegate
                            {
                                if (tabCtrl.TabPages.Count > 1)
                                {
                                    tabCtrl.TabPages.Remove(tabOutput);
                                    focusLastTab();
                                }
                            }));
                        }
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
                            try
                            {
                                tabCtrl.Invoke(new MethodInvoker(delegate
                                {
                                    tabCtrl.TabPages.Clear();
                                    addTab();
                                }));
                            } catch (Exception err)
                            {
                                Console.WriteLine("Exception: " + err.ToString());
                            }
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
        private void fixSize()
        {
            for(int i = 0; i < listOfProcesses.Count; i++)
            {
                Process p = listOfProcesses[i];
                MoveWindow(p.MainWindowHandle, 0, 0, tabCtrl.Width, tabCtrl.Height, true);
                PositionWindow(p, tabCtrl.SelectedTab);
                SendMessage(p.MainWindowHandle, WmPaint, IntPtr.Zero, IntPtr.Zero);
            }
        }

        private void focusCurrentTab(object sender, EventArgs e)
        {
            fixSize();
            //MessageBox.Show("Searching for index.");
            //Determine the index in the array of the currently selected tab
            int tabIndex = -1;
            TabPage selectedTab = tabCtrl.SelectedTab;
            for (int i = 0; i < tabCtrl.TabCount; i++)
            {
                TabPage currentTab = tabCtrl.TabPages[i];
                if (currentTab == selectedTab)
                {
                    tabIndex = i;
                }
            }
            //If an index was found focus the new tab
            if (tabIndex > -1 && tabIndex < tabCtrl.TabPages.Count)
            {
                //MessageBox.Show("Index Found: "+ tabIndex.ToString());
                if (tabCtrl.InvokeRequired)
                {
                    Thread exitThread = new Thread(delegate ()
                    {
                        tabCtrl.Invoke(new MethodInvoker(delegate
                        {
                            PositionWindow(listOfProcesses[tabIndex], tabCtrl.TabPages[tabIndex]);
                            SetForegroundWindow(listOfProcesses[tabIndex].MainWindowHandle);
                        }));
                    });
                    exitThread.Start();
                }
                else
                {
                    PositionWindow(listOfProcesses[tabIndex], tabCtrl.TabPages[tabIndex]);
                    SetForegroundWindow(listOfProcesses[tabIndex].MainWindowHandle);
                }
            }
        }

        private void focusLastTab()
        {
            fixSize();
            //Focus on new Tab
            if (tabCtrl.InvokeRequired)
            {
                Thread exitThread = new Thread(delegate ()
                {
                    tabCtrl.Invoke(new MethodInvoker(delegate
                    {
                        tabCtrl.SelectedTab = tabCtrl.TabPages[tabCtrl.TabPages.Count - 1];
                        SetParent(listOfProcesses[tabCtrl.TabPages.Count - 1].MainWindowHandle, tabCtrl.TabPages[tabCtrl.TabPages.Count - 1].Handle);
                        PositionWindow(listOfProcesses[tabCtrl.TabPages.Count - 1], tabCtrl.TabPages[tabCtrl.TabPages.Count - 1]);
                        SetForegroundWindow(listOfProcesses[tabCtrl.TabPages.Count - 1].MainWindowHandle);
                    }));
                });
                exitThread.Start();
            }
            else
            {
                tabCtrl.SelectedTab = tabCtrl.TabPages[tabCtrl.TabPages.Count - 1];
                SetParent(listOfProcesses[tabCtrl.TabPages.Count - 1].MainWindowHandle, tabCtrl.TabPages[tabCtrl.TabPages.Count - 1].Handle);
                PositionWindow(listOfProcesses[tabCtrl.TabPages.Count - 1], tabCtrl.TabPages[tabCtrl.TabPages.Count - 1]);
                SetForegroundWindow(listOfProcesses[tabCtrl.TabPages.Count - 1].MainWindowHandle);
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
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
            mainMenu.Opacity = this.Opacity;
            tabCtrl.Location = new Point(0, 0);
            this.FormBorderStyle = FormBorderStyle.None;
            tabCtrl.TabPages.Clear();
            addTab();
            this.Width = Screen.FromControl(this).Bounds.Width / (int)getScalingFactor();
            this.Height = Screen.FromControl(this).Bounds.Height / 2;
            this.CenterToScreen();
            this.Top = 0;
            resizeForm();
        }
        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
                fixSize();
        }
        private float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);
            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
            return ScreenScalingFactor; // 1.25 = 125%
        }
        private void GlobalHookKeyCtrlF1Press()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                Show();
                resizeForm();
                fixSize();
                this.Focus();
                notifyIcon.Visible = false;
                this.lastState = this.WindowState;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                Hide();
                this.WindowState = FormWindowState.Minimized;
                this.lastState = this.WindowState;
                notifyIcon.Visible = true;
            }
        }
        private void GlobalHookKeyCtrlShiftF1Press()
        {
            //Do stuff here
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                this.lastState = this.WindowState;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                resizeForm();
                this.lastState = this.WindowState;
            }
        }
        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GlobalHookKeyCtrlF1Press();
        }
        private void PositionWindow(Process p, TabPage tabOutput)
        {
            SetWindowPos(p.MainWindowHandle, 0, 0, 0, tabOutput.Bounds.Width, tabOutput.Bounds.Height, SWP_NOMOVE | SWP_NOSIZE | SWP_FRAMECHANGED);
            MoveWindow(p.MainWindowHandle, 0, 0, tabOutput.Width, tabOutput.Height, true);
            SendMessage(p.MainWindowHandle, WmPaint, IntPtr.Zero, IntPtr.Zero);
        }

        private void resizeForm()
        {
            this.Width = Screen.FromControl(this).Bounds.Width / (int)getScalingFactor();
            this.Height = Screen.FromControl(this).Bounds.Height / 2;
            this.CenterToScreen();
            this.Top = 0;
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
        }
        private void StyleWindow(IntPtr intPtr)
        {
            int style = GetWindowLong(intPtr, GWL_STYLE);
            style = style & ~WS_CAPTION;
            style = style & ~WS_SYSMENU;
            style = style & ~WS_THICKFRAME;
            style = style & ~WS_MINIMIZE;
            style = style & ~WS_MAXIMIZEBOX;
            SetWindowLong(intPtr, GWL_STYLE, style);
            style = GetWindowLong(intPtr, GWL_EXSTYLE);
        }
        private void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
            //Detect Ctrl + Shift + F11
            m_GlobalHook.OnCombination(new Dictionary<Combination, Action>() {
                { Combination.FromString("Control+Shift+F11"), GlobalHookKeyCtrlShiftF1Press },
            });
            //Detect F12
            m_GlobalHook.OnCombination(new Dictionary<Combination, Action>() {
                { Combination.FromString("F12"), GlobalHookKeyCtrlF1Press },
            });
        }
        private void Unsubscribe()
        {
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;
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
                    GlobalHookKeyCtrlShiftF1Press();
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void tabCtrl_TabClosing(object sender, TabControlCancelEventArgs e)
        {
            ExitFunk(sender, e,tabCtrl.SelectedTab);
        }

        private void ptcClose_MouseEnter(object sender, EventArgs e)
        {
            ptcClose.Image = Properties.Resources.close_red;
        }

        private void ptcClose_MouseLeave(object sender, EventArgs e)
        {
            ptcClose.Image = Properties.Resources.close_black_transparent;
        }

        private void ptcClose_Click(object sender, EventArgs e)
        {
            this.exiting = true;
            Application.Exit();
        }

        private void pctToggle_MouseEnter(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                pctToggle.Image = Properties.Resources.minimize_black_transparent;
            }
            else
            {
                pctToggle.Image = Properties.Resources.maximize_black_transparent;
            }
        }

        private void pctToggle_MouseLeave(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                pctToggle.Image = Properties.Resources.maximize_black_transparent;
            }
            else
            {
                pctToggle.Image = Properties.Resources.minimize_black_transparent;
            }
        }

        private void pctToggle_Click(object sender, EventArgs e)
        {
            GlobalHookKeyCtrlShiftF1Press();
        }

        private void pctHide_Click(object sender, EventArgs e)
        {
            GlobalHookKeyCtrlF1Press();
        }

        private void pctHide_MouseEnter(object sender, EventArgs e)
        {
            pctHide.Image = Properties.Resources.hide_black_transparent;
        }

        private void pctHide_MouseLeave(object sender, EventArgs e)
        {
            pctHide.Image = Properties.Resources.hide_black_filled_transparent;
        }

        private void pctAdd_MouseEnter(object sender, EventArgs e)
        {
            pctAdd.Image = Properties.Resources.add_black_transparent;
        }

        private void pctAdd_MouseLeave(object sender, EventArgs e)
        {
            pctAdd.Image = Properties.Resources.add_black_filled_transparent;
        }

        private void pctAdd_Click(object sender, EventArgs e)
        {
            addTab();
        }

        private void pctMenu_Click(object sender, EventArgs e)
        {
            mainMenu.Show(pctMenu,new Point(0, pctMenu.Height));
        }

        private void pctMenu_MouseEnter(object sender, EventArgs e)
        {
            pctMenu.Image = Properties.Resources.menu_black_transparent;
        }

        private void pctMenu_MouseLeave(object sender, EventArgs e)
        {
            if (!menuOpen)
            {
                pctMenu.Image = Properties.Resources.menu_black_filled_transparent;
            }
        }

        private void mainMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pctMenu.Image = Properties.Resources.down_black_transparent;
            menuOpen = true;
        }

        private void mainMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            pctMenu.Image = Properties.Resources.menu_black_filled_transparent;
            menuOpen = false;
        }

        private void tabCtrl_Selected(object sender, TabControlEventArgs e)
        {
           
            //fixSize();
            //focusCurrentTab(sender,e);
        }

        private void tabCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            fixSize();
            focusCurrentTab(sender,e);
        }

        private void menuConfigure_Click(object sender, EventArgs e)
        {
            frmSettings settingsForm = new frmSettings();
            settingsForm.ShowDialog();
        }
    }
}
