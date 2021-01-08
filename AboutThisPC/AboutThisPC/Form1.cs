using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AboutThisPC.Properties;
using Microsoft.Win32;
using System.Management;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace AboutThisPC
{
    public partial class Form1 : Form
    {
        public ToolStripItem closeButton;
        public ToolStripItem minimiseButton;
        public ToolStripItem breakSpace;
        public ToolStripItem overviewMenu;
        public ToolStripItem displayMenu;
        public ToolStripItem storageMenu;

        public Button sysReport = new Button();
        public Button softwareUpdate = new Button();

        public PictureBox displayImg = new PictureBox();
        public Label displaySize = new Label();
        public Label displayAdapter = new Label();
        public Button displaySettings = new Button();

        public Chart chart = new Chart();

        public PrivateFontCollection pfc = new PrivateFontCollection();

        public string OSNum = null;
        public string openMenu = "Overview";

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form1()
        {
            InitializeComponent();
            versionImage.InitialImage = Resources.circle;
            AddFont(Resources.SFProDisplay);
            var version = FriendlyName().Replace("Microsoft ", "");
            versionTitle.Text = version;
            OSNum = version.Replace("Windows ", "").Replace(" Home", "").Replace(" Pro", "").Replace(" Pro for Workstations", "").Replace(" Education", "").Replace(" Pro Education", "").Replace(" Enterprise", "").Replace(" ", "");
            var closeImage = new PictureBox() { BackColor = Color.Transparent, Image = Resources.close };
            var minimiseImage = new PictureBox() { BackColor = Color.Transparent, Image = Resources.minimize };
            windowControls.Renderer = new ButtonRenderer();
            closeButton = windowControls.Items.Add(closeImage.Image);
            minimiseButton = windowControls.Items.Add(minimiseImage.Image);
            breakSpace = windowControls.Items.Add(" ");
            overviewMenu = windowControls.Items.Add("Overview");
            displayMenu = windowControls.Items.Add("Displays");
            storageMenu = windowControls.Items.Add("Storage");
            overviewMenu.TextAlign = ContentAlignment.MiddleCenter;
            displayMenu.TextAlign = ContentAlignment.MiddleCenter;
            storageMenu.TextAlign = ContentAlignment.MiddleCenter;
            overviewMenu.BackgroundImage = Resources.selected;
            displayMenu.BackgroundImage = Resources.unselected;
            storageMenu.BackgroundImage = Resources.unselected;
            overviewMenu.AutoToolTip = false;
            displayMenu.AutoToolTip = false;
            storageMenu.AutoToolTip = false;
            for (int i = 0; i < Convert.ToInt32(windowControls.Width / 10); i++)
            {
                breakSpace.Text += " ";
            }
            breakSpace.AutoToolTip = false;
            breakSpace.MouseDown += BreakSpace_MouseDown;
            overviewMenu.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
            displayMenu.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
            storageMenu.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
            verNum.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
            verNum.Location = new Point(versionTitle.Location.X + 3, versionTitle.Location.Y + versionTitle.Height - 3);
            verNum.Text = "Version " + Environment.OSVersion.Version + " (" + Environment.OSVersion.Platform + ")";
            model.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
            model.Location = new Point(verNum.Location.X, verNum.Location.Y + verNum.Height * 2 - 3);
            model.Text = modelName();
            processorName.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
            processorName.Location = new Point(model.Location.X, model.Location.Y + model.Height);
            var cpu = processor().Replace("Intel(R) Core(TM)", "Intel Core");
            processorName.Text = "Processor: " + cpu;
            memory.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
            memory.Location = new Point(processorName.Location.X, processorName.Location.Y + processorName.Height);
            memory.Text = ram();
            sdLabel.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
            sdLabel.Location = new Point(memory.Location.X, memory.Location.Y + memory.Height);
            sdLabel.Text = "Startup Disk: " + startupDisk();
            graphics.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
            graphics.Location = new Point(sdLabel.Location.X, sdLabel.Location.Y + sdLabel.Height);
            graphics.Text = "Graphics: " + gpu();
            serial.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
            serial.Location = new Point(graphics.Location.X, graphics.Location.Y + graphics.Height);
            serial.Text = "Serial Number: " + serialnum();
            sysReport = new Button() { Text = "System Report...", Font = new Font(pfc.Families[0], 9, FontStyle.Regular), AutoSize = true, BackColor = Color.Transparent };
            softwareUpdate = new Button() { Text = "Software Update...", Font = new Font(pfc.Families[0], 9, FontStyle.Regular), AutoSize = true, BackColor = Color.Transparent };
            sysReport.Location = new Point(serial.Location.X - (softwareUpdate.Width - sysReport.Width), serial.Location.Y + serial.Height * 2);
            softwareUpdate.Location = new Point(serial.Location.X + (softwareUpdate.Width + sysReport.Width), serial.Location.Y + serial.Height * 2);
            sysReport.FlatStyle = FlatStyle.System;
            softwareUpdate.FlatStyle = FlatStyle.System;
            sysReport.BackgroundImage = Resources.unselected;
            sysReport.BackgroundImageLayout = ImageLayout.Stretch;
            softwareUpdate.BackgroundImage = Resources.unselected;
            softwareUpdate.BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(sysReport);
            Controls.Add(softwareUpdate);
            displayImg = new PictureBox() { Image = Resources.Apple_Thunderbolt_Display, BackColor = Color.Transparent, SizeMode = PictureBoxSizeMode.StretchImage, Visible = false, Size = new Size(Resources.Apple_Thunderbolt_Display.Width / 3, Resources.Apple_Thunderbolt_Display.Height / 3) };
            displaySize = new Label() { Text = resolution(), Font = new Font(pfc.Families[0], 11, FontStyle.Regular), BackColor = Color.Transparent, AutoSize = true, Visible = false };
            displayAdapter = new Label() { Text = graphics.Text.Replace("Graphics: ", ""), Font = new Font(pfc.Families[0], 11, FontStyle.Regular), BackColor = Color.Transparent, AutoSize = true, Visible = false };
            displaySettings = new Button() { Text = "Displays Preferences", Font = new Font(pfc.Families[0], 11), BackColor = Color.Transparent, AutoSize = true, Visible = false };
            displaySettings.Click += DisplaySettings_Click;
            displaySettings.MouseEnter += DisplaySettings_MouseEnter;
            displaySettings.MouseLeave += DisplaySettings_MouseLeave;
            var chartArea = new ChartArea("Drives");
            chart.ChartAreas.Add(chartArea);
            chart.Dock = DockStyle.Bottom;
            chart.Visible = false;
            diskUsage();
            Controls.Add(chart);
            Controls.Add(displayImg);
            Controls.Add(displaySize);
            Controls.Add(displayAdapter);
            Controls.Add(displaySettings);
            sysReport.Click += SysReport_Click;
            sysReport.MouseEnter += SysReport_MouseEnter;
            sysReport.MouseLeave += SysReport_MouseLeave;
            softwareUpdate.Click += SoftwareUpdate_Click;
            softwareUpdate.MouseEnter += SoftwareUpdate_MouseEnter;
            softwareUpdate.MouseLeave += SoftwareUpdate_MouseLeave;
            closeButton.MouseEnter += CloseButton_MouseEnter;
            closeButton.MouseLeave += CloseButton_MouseLeave;
            closeButton.Click += CloseButton_Click;
            minimiseButton.MouseEnter += MinimiseButton_MouseEnter;
            minimiseButton.MouseLeave += MinimiseButton_MouseLeave;
            minimiseButton.Click += MinimiseButton_Click;
            overviewMenu.MouseEnter += OverviewMenu_MouseEnter;
            overviewMenu.MouseLeave += OverviewMenu_MouseLeave;
            overviewMenu.Click += OverviewMenu_Click;
            displayMenu.MouseEnter += DisplayMenu_MouseEnter;
            displayMenu.MouseLeave += DisplayMenu_MouseLeave;
            displayMenu.Click += DisplayMenu_Click;
            storageMenu.MouseEnter += StorageMenu_MouseEnter;
            storageMenu.MouseLeave += StorageMenu_MouseLeave;
            storageMenu.Click += StorageMenu_Click;
            if (OSNum == "10")
            {
                versionImage.Image = Resources.win10;
            }
            else if (OSNum == "8" || OSNum == "8.1")
            {
                versionImage.Image = Resources.win8;
            }
            else if (OSNum == "7")
            {
                versionImage.Image = Resources.win7;
            }
            versionImage.Size = new Size(versionImage.Image.Width / 2, versionImage.Image.Height / 2);
        }

        private void DisplaySettings_MouseLeave(object sender, EventArgs e)
        {
            displaySettings.BackgroundImage = Resources.unselected;
        }

        private void DisplaySettings_MouseEnter(object sender, EventArgs e)
        {
            displaySettings.BackgroundImage = Resources.hover;
        }

        private void DisplaySettings_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Windows\System32\control.exe", "desk.cpl");
        }

        private void BreakSpace_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void SoftwareUpdate_MouseLeave(object sender, EventArgs e)
        {
            softwareUpdate.BackgroundImage = Resources.unselected;
        }

        private void SoftwareUpdate_MouseEnter(object sender, EventArgs e)
        {
            softwareUpdate.BackgroundImage = Resources.hover;
        }

        private void SysReport_MouseLeave(object sender, EventArgs e)
        {
            sysReport.BackgroundImage = Resources.unselected;
        }

        private void SysReport_MouseEnter(object sender, EventArgs e)
        {
            sysReport.BackgroundImage = Resources.hover;
        }

        private void SoftwareUpdate_Click(object sender, EventArgs e)
        {
            if (OSNum == "10" || OSNum == "8.1" || OSNum == "8")
            {
                Process.Start("ms-settings:windowsupdate-action");
            }
            else
            {
                Process.Start(@"C:\Windows\system32\wuauclt.exe /detectnow");
            }
        }

        private void SysReport_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Windows\System32\msinfo32.exe");
        }

        private void StorageMenu_Click(object sender, EventArgs e)
        {
            menuOpen("Storage");
        }

        private void StorageMenu_MouseLeave(object sender, EventArgs e)
        {
            if (openMenu == "Storage")
            {
                storageMenu.BackgroundImage = Resources.selected;
            }
            else
            {
                storageMenu.BackgroundImage = Resources.unselected;
            }
        }

        private void StorageMenu_MouseEnter(object sender, EventArgs e)
        {
            storageMenu.BackgroundImage = Resources.hover;
        }

        private void DisplayMenu_Click(object sender, EventArgs e)
        {
            menuOpen("Displays");
        }

        private void DisplayMenu_MouseLeave(object sender, EventArgs e)
        {
            if (openMenu == "Displays")
            {
                displayMenu.BackgroundImage = Resources.selected;
            }
            else
            {
                displayMenu.BackgroundImage = Resources.unselected;
            }
        }

        private void DisplayMenu_MouseEnter(object sender, EventArgs e)
        {
            displayMenu.BackgroundImage = Resources.hover;
        }

        private void OverviewMenu_Click(object sender, EventArgs e)
        {
            menuOpen("Overview");
        }

        private void OverviewMenu_MouseLeave(object sender, EventArgs e)
        {
            if (openMenu == "Overview")
            {
                overviewMenu.BackgroundImage = Resources.selected;
            }
            else
            {
                overviewMenu.BackgroundImage = Resources.unselected;
            }
        }

        private void OverviewMenu_MouseEnter(object sender, EventArgs e)
        {
            overviewMenu.BackgroundImage = Resources.hover;
        }

        public void menuOpen(string menu)
        {
            Control[] overviewItems = { verNum, versionImage, versionTitle, model, processorName, memory, sdLabel, graphics, serial, sysReport, softwareUpdate };
            Control[] displayItems = { displayImg, displaySize, displayAdapter, displaySettings };
            Control[] storageItems = { chart };

            if (menu == "Overview")
            {
                if (openMenu != "Overview")
                {
                    foreach (var item in overviewItems)
                    {
                        item.Visible = true;
                    }
                    foreach (var item in displayItems)
                    {
                        item.Visible = false;
                    }
                    foreach (var item in storageItems)
                    {
                        item.Visible = false;
                    }
                    openMenu = "Overview";
                }
                overviewMenu.BackgroundImage = Resources.selected;
                displayMenu.BackgroundImage = Resources.unselected;
                storageMenu.BackgroundImage = Resources.unselected;
            }
            else if (menu == "Displays")
            {
                if (openMenu != "Displays")
                {
                    foreach (var item in overviewItems)
                    {
                        item.Visible = false;
                    }
                    foreach (var item in displayItems)
                    {
                        item.Visible = true;
                    }
                    foreach (var item in storageItems)
                    {
                        item.Visible = false;
                    }
                    openMenu = "Displays";
                }
                overviewMenu.BackgroundImage = Resources.unselected;
                displayMenu.BackgroundImage = Resources.selected;
                storageMenu.BackgroundImage = Resources.unselected;
            }
            else if (menu == "Storage")
            {
                if (openMenu != "Storage")
                {
                    foreach (var item in overviewItems)
                    {
                        item.Visible = false;
                    }
                    foreach (var item in displayItems)
                    {
                        item.Visible = false;
                    }
                    foreach (var item in storageItems)
                    {
                        item.Visible = true;
                    }
                    openMenu = "Storage";
                }
                overviewMenu.BackgroundImage = Resources.unselected;
                displayMenu.BackgroundImage = Resources.unselected;
                storageMenu.BackgroundImage = Resources.selected;
            }
        }

        private void MinimiseButton_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = ActiveForm.Size.Width + ActiveForm.Size.Height; i != 0; i--)
                {
                    ActiveForm.Size = new Size(ActiveForm.Size.Width - 1, ActiveForm.Size.Height - 1);
                }
                ActiveForm.WindowState = FormWindowState.Minimized;
            }
            catch (NullReferenceException) { }
        }

        private void MinimiseButton_MouseLeave(object sender, EventArgs e)
        {
            minimiseButton.Image = Resources.minimize;
        }

        private void MinimiseButton_MouseEnter(object sender, EventArgs e)
        {
            minimiseButton.Image = Resources.minimize_hover;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.Image = Resources.close;
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.Image = Resources.close_hover;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            try
            {
                ActiveForm.Size = new Size(650, 400);
            }
            catch (NullReferenceException) { }
        }

        private void windowControls_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        private static uint cFonts = 0;
        private void AddFont(byte[] fontdata)
        {
            IntPtr dataPointer;
            dataPointer = Marshal.AllocCoTaskMem(fontdata.Length);
            Marshal.Copy(fontdata, 0, dataPointer, (int)fontdata.Length);
            AddFontMemResourceEx(dataPointer, (uint)fontdata.Length, IntPtr.Zero, ref cFonts);
            cFonts += 1;
            pfc.AddMemoryFont(dataPointer, (int)fontdata.Length);
        }

        public string HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return (string)rk.GetValue(key);
            }
            catch { return ""; }
        }

        public string FriendlyName()
        {
            unsafe
            {
                string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
                string CSDVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
                if (ProductName != "")
                {
                    return (ProductName.StartsWith("Microsoft") ? "" : "Microsoft ") + ProductName +
                                (CSDVersion != "" ? " " + CSDVersion : "");
                }
                return "";
            }
        }

        public string modelName()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Model")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }

        public string processor()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_Processor").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Name")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }

        public string ram()
        {
            var totalNum = Convert.ToInt32((from x in new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemoryArray").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("MaxCapacity")).FirstOrDefault());
            var total = string.Join("", Convert.ToInt32(totalNum / 1024 / 1000) + " GB");
            var speed = string.Join("", (from x in new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory").Get().Cast<ManagementObject>()
                         select x.GetPropertyValue("Speed")).FirstOrDefault() + " MHz");
            var typeNum = Convert.ToInt32((from x in new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("FormFactor")).FirstOrDefault());
            var type = "";
            switch (typeNum)
            {
                case 0: type = "Unknown"; break;
                case 1: type = "Other"; break;
                case 2: type = "SIP"; break;
                case 3: type = "DIP"; break;
                case 4: type = "ZIP"; break;
                case 5: type = "SOJ"; break;
                case 6: type = "Proprietary"; break;
                case 7: type = "SIMM"; break;
                case 8: type = "DIMM"; break;
                case 9: type = "TSOP"; break;
                case 10: type = "PGA"; break;
                case 11: type = "RIMM"; break;
                case 12: type = "SODIMM"; break;
                case 13: type = "SRIMM"; break;
                case 14: type = "SMD"; break;
                case 15: type = "SSMP"; break;
                case 16: type = "QFP"; break;
                case 17: type = "TQFP"; break;
                case 18: type = "SOIC"; break;
                case 19: type = "LCC"; break;
                case 20: type = "PLCC"; break;
                case 21: type = "BGA"; break;
                case 22: type = "FPBGA"; break;
                case 23: type = "LGA"; break;
            }
            return "Memory: " + total + " " + speed + " " + type;
        }

        public string startupDisk()
        {
            return GetDriveLabel(Environment.GetEnvironmentVariable("SystemDrive")) + " (" + Environment.GetEnvironmentVariable("SystemDrive") + ")";
        }

        public string gpu()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Name")).FirstOrDefault();
            var ramBytes = Convert.ToDouble((from x in new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get().Cast<ManagementObject>()
                       select x.GetPropertyValue("AdapterRam")).FirstOrDefault());
            var ramGB = string.Join("", Convert.ToDouble(ramBytes / 1024 / 1000 / 1000));
            ramGB = ramGB.Remove(2, ramGB.Length - 4);
            var ram = string.Join("", ramGB + " GB");
            return string.Join("", name + " " + ram);
        }

        public string serialnum()
        {
            var num = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_BIOS").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("SerialNumber")).FirstOrDefault();
            return num != null ? num.ToString() : "Unknown";
        }

        public string resolution()
        {
            var X = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get().Cast<ManagementObject>()
                       select x.GetPropertyValue("CurrentHorizontalResolution")).FirstOrDefault();
            var Y = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get().Cast<ManagementObject>()
                       select x.GetPropertyValue("CurrentVerticalResolution")).FirstOrDefault();
            var inch = (SystemInformation.PrimaryMonitorSize.Width ^ 2 + SystemInformation.PrimaryMonitorSize.Height ^ 2) / 100;
            return inch + "-inch (" + X + " x " + Y + ")";
        }

        public void diskUsage()
        {
            var drive = DriveInfo.GetDrives()[0];
                var total = new Series(drive.Name + " - Total");
                var used = new Series(drive.Name + " - Used");
                try
                {
                    var diskName = drive.VolumeLabel + " (" + drive.Name + ")";
                    var diskUsed = Convert.ToInt32((drive.TotalSize - drive.AvailableFreeSpace) / 1073741824);
                    var diskFree = Convert.ToInt32(drive.AvailableFreeSpace / 1073741824);
                    var diskTotal = Convert.ToInt32(drive.TotalSize / 1073741824);
                    total.AxisLabel = diskName;
                    total.ChartType = SeriesChartType.StackedBar;
                    total.Points.Add(new DataPoint(0, diskTotal));
                    total.Points[0].Label = string.Join("", "Disk Size: " + diskTotal + " GB");
                    used.ChartType = SeriesChartType.StackedBar;
                    used.Points.Add(new DataPoint(0, diskUsed));
                    used.Points[0].Label = string.Join("", "Used Space: " + diskUsed + " GB");
                    total.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
                    used.Font = new Font(pfc.Families[0], 11, FontStyle.Regular);
                    chart.DataManipulator.InsertEmptyPoints(1, IntervalType.Number, total);
                    chart.DataManipulator.InsertEmptyPoints(1, IntervalType.Number, used);
                    chart.Series.Add(total);
                    chart.Series.Add(used);
                }
                catch (Exception) { }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            versionImage.Location = new Point(Convert.ToInt32(ActiveForm.Width / 8), Convert.ToInt32(Convert.ToDouble(ActiveForm.Height / 3.5)));
            displayImg.Location = new Point(Convert.ToInt32(ActiveForm.Width * 0.4), Convert.ToInt32(ActiveForm.Height * 0.2));
            displaySize.Location = new Point(displayImg.Location.X - displayImg.Width / 3, Convert.ToInt32(displayImg.Location.Y + displayImg.Height * 1.2));
            displayAdapter.Location = new Point(displaySize.Location.X, Convert.ToInt32(displaySize.Location.Y + displaySize.Height * 1.2));
            displaySettings.Location = new Point(ActiveForm.Width - displaySettings.Width - 25, ActiveForm.Height - displaySettings.Height - 25);
            chart.Size = new Size(ActiveForm.Size.Width, ActiveForm.Height - windowControls.Height);
        }

        public const string SHELL = "shell32.dll";

        [DllImport(SHELL, CharSet = CharSet.Unicode)]
        public static extern uint SHParseDisplayName(string pszName, IntPtr zero, [Out] out IntPtr ppidl, uint sfgaoIn, [Out] out uint psfgaoOut);

        [DllImport(SHELL, CharSet = CharSet.Unicode)]
        public static extern uint SHGetNameFromIDList(IntPtr pidl, SIGDN sigdnName, [Out] out String ppszName);

        public enum SIGDN : uint
        {
            NORMALDISPLAY = 0x00000000,
            PARENTRELATIVEPARSING = 0x80018001,
            DESKTOPABSOLUTEPARSING = 0x80028000,
            PARENTRELATIVEEDITING = 0x80031001,
            DESKTOPABSOLUTEEDITING = 0x8004c000,
            FILESYSPATH = 0x80058000,
            URL = 0x80068000,
            PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
            PARENTRELATIVE = 0x80080001
        }

        public string GetDriveLabel(string driveNameAsLetterColonBackslash)
        {
            IntPtr pidl;
            uint dummy;
            string name;
            if (SHParseDisplayName(driveNameAsLetterColonBackslash, IntPtr.Zero, out pidl, 0, out dummy) == 0
                && SHGetNameFromIDList(pidl, SIGDN.PARENTRELATIVEEDITING, out name) == 0
                && name != null)
            {
                return name;
            }
            return null;
        }
    }

    internal class ButtonRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.BackgroundImage != null)
            {
                e.Graphics.DrawImageUnscaledAndClipped(e.Item.BackgroundImage, e.Item.ContentRectangle);
            }
            else
            {
                return;
            }
        }
    }
}
