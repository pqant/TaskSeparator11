using IWshRuntimeLibrary;
using Splitter;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsShortcutFactory;
using File = System.IO.File;

namespace TaskSplitter11
{
    public partial class MainForm : Form
    {
        public enum ShowWindowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_MAX = 10
        }
        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpszOp,
            string lpszFile,
            string lpszParams,
            string lpszDir,
            ShowWindowCommands FsShowCmd
        );

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            //Setup & config
            var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string appPath = Path.GetDirectoryName(Application.ExecutablePath) ?? string.Empty;
            string shortcutsPath = Path.Join(docsPath, "TaskSeparator11", "Shortcuts");
            Directory.CreateDirectory(shortcutsPath);

            //Ensure files are in the right place
            EnsureSplitterFilesAreAvailable(appPath, shortcutsPath);

            //Make a copy of the Splitter exe
            string splitterExe = Path.Join(appPath, "Splitter.exe");
            string splitterExeLocation = GetNewLinkName(shortcutsPath, "exe");
            File.Copy(splitterExe, splitterExeLocation);

            //Create a shortcut to the Splitter exe
            string linkLocation = GetNewLinkName(shortcutsPath, "lnk");

            // Detect taskbar position and choose appropriate icon
            var taskbarPosition = TaskbarHelper.GetTaskbarPosition();
            bool isVerticalTaskbar = TaskbarHelper.IsVerticalTaskbar();

            // Determine which icon to use based on taskbar position
            string iconFileName = isVerticalTaskbar ? "separator_horizontal.ico" : "separator.ico";
            string iconPath = Path.Join(shortcutsPath, "icons", iconFileName);

            //Create shortcut with appropriate icon
            using var shortcut = new WindowsShortcut
            {
                Path = splitterExeLocation,
                IconLocation = new IconLocation(iconPath, 0),
            };
            shortcut.Save(linkLocation);

            ShellExecute(IntPtr.Zero, "open", linkLocation, "--gui", null, ShowWindowCommands.SW_NORMAL);

            Application.Exit();
        }

        private void EnsureSplitterFilesAreAvailable(string appPath, string shortcutsPath)
        {
            if (File.Exists(Path.Join(shortcutsPath, "Splitter.dll"))) return;

            var dir = new DirectoryInfo(appPath);
            var files = dir.GetFiles("Splitter*");

            foreach (var f in files)
            {
                var source = Path.Join(appPath, f.Name);
                var dest = Path.Join(shortcutsPath, f.Name);

                File.Copy(source, dest, true);
            }

            // Also copy icons folder
            string iconsSourcePath = Path.Join(appPath, "icons");
            string iconsDestPath = Path.Join(shortcutsPath, "icons");

            if (Directory.Exists(iconsSourcePath))
            {
                Directory.CreateDirectory(iconsDestPath);

                var iconFiles = new DirectoryInfo(iconsSourcePath).GetFiles("*.ico");
                foreach (var iconFile in iconFiles)
                {
                    var source = Path.Join(iconsSourcePath, iconFile.Name);
                    var dest = Path.Join(iconsDestPath, iconFile.Name);
                    File.Copy(source, dest, true);
                }
            }
        }

        private string GetNewLinkName(string path, string ext)
        {
            int count = 1;
            char c = ext == "exe" ? '_' : ' ';

            string shortcutLink = Path.Join(path, $"{c}.{ext}");
            do
            {
                shortcutLink = Path.Join(path, $"{new string(c, count)}.{ext}");
                count++;
            } while (File.Exists(shortcutLink));

            return shortcutLink;
        }
    }
}