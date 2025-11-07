namespace Splitter
{
    public partial class FormSplitter : Form
    {
        private TaskbarPosition _taskbarPosition;

        public FormSplitter()
        {
            InitializeComponent();

            // Detect taskbar position
            _taskbarPosition = TaskbarHelper.GetTaskbarPosition();
            System.Diagnostics.Debug.WriteLine($">>> Detected Taskbar Position: {_taskbarPosition}");
            System.Diagnostics.Debug.WriteLine($">>> Is Vertical Taskbar: {TaskbarHelper.IsVerticalTaskbar()}");

            // Apply appropriate icon and dimensions based on taskbar position
            ConfigureForTaskbarPosition();

            // Apply drop shadow
            (new Core.DropShadow()).ApplyShadows(this);
        }

        /// <summary>
        /// Configures form icon, size and position based on taskbar location
        /// </summary>
        private void ConfigureForTaskbarPosition()
        {
            System.Diagnostics.Debug.WriteLine(">>> ConfigureForTaskbarPosition called");

            if (TaskbarHelper.IsVerticalTaskbar())
            {
                System.Diagnostics.Debug.WriteLine(">>> Configuring for VERTICAL taskbar (Left/Right)");

                // Use horizontal icon for vertical taskbar
                LoadHorizontalIcon();

                // Adjust form dimensions for vertical taskbar
                // Width should be smaller, height should be larger
                
                //this.Width = 8;  // Thin horizontal separator
                //this.Height = 100; // Longer bar

                System.Diagnostics.Debug.WriteLine($">>> Form dimensions set to: Width={this.Width}, Height={this.Height}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(">>> Configuring for HORIZONTAL taskbar (Top/Bottom)");

                // Use vertical icon for horizontal taskbar (default)
                LoadVerticalIcon();

                // Keep existing dimensions for horizontal taskbar
                this.Width = 100;
                this.Height = 8;

                System.Diagnostics.Debug.WriteLine($">>> Form dimensions set to: Width={this.Width}, Height={this.Height}");
            }
        }

        /// <summary>
        /// Loads the horizontal separator icon (for vertical taskbar)
        /// </summary>
        private void LoadHorizontalIcon()
        {
            try
            {
                string iconPath = System.IO.Path.Combine(
                    Application.StartupPath,
                    "icons",
                    "separator_horizontal.ico"
                );

                System.Diagnostics.Debug.WriteLine($">>> Loading horizontal icon from: {iconPath}");
                System.Diagnostics.Debug.WriteLine($">>> File exists: {System.IO.File.Exists(iconPath)}");

                if (System.IO.File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                    System.Diagnostics.Debug.WriteLine(">>> Horizontal icon loaded successfully ✓");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(">>> ERROR: Horizontal icon file not found!");
                }
            }
            catch (Exception ex)
            {
                // Log error but don't crash the application
                System.Diagnostics.Debug.WriteLine($">>> ERROR loading horizontal icon: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the vertical separator icon (for horizontal taskbar)
        /// </summary>
        private void LoadVerticalIcon()
        {
            try
            {
                string iconPath = System.IO.Path.Combine(
                    Application.StartupPath,
                    "icons",
                    "separator.ico"
                );

                System.Diagnostics.Debug.WriteLine($">>> Loading vertical icon from: {iconPath}");
                System.Diagnostics.Debug.WriteLine($">>> File exists: {System.IO.File.Exists(iconPath)}");

                if (System.IO.File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                    System.Diagnostics.Debug.WriteLine(">>> Vertical icon loaded successfully ✓");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(">>> ERROR: Vertical icon file not found!");
                }
            }
            catch (Exception ex)
            {
                // Log error but don't crash the application
                System.Diagnostics.Debug.WriteLine($">>> ERROR loading vertical icon: {ex.Message}");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}