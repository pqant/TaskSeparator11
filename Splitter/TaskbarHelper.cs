using System;
using System.Runtime.InteropServices;

namespace Splitter
{
    /// <summary>
    /// Represents the position of the Windows taskbar on the screen
    /// </summary>
    public enum TaskbarPosition
    {
        /// <summary>
        /// Taskbar is positioned on the left side of the screen
        /// </summary>
        Left = 0,

        /// <summary>
        /// Taskbar is positioned at the top of the screen
        /// </summary>
        Top = 1,

        /// <summary>
        /// Taskbar is positioned on the right side of the screen
        /// </summary>
        Right = 2,

        /// <summary>
        /// Taskbar is positioned at the bottom of the screen (default)
        /// </summary>
        Bottom = 3,

        /// <summary>
        /// Taskbar position could not be determined
        /// </summary>
        Unknown = -1
    }

    /// <summary>
    /// Helper class to detect Windows taskbar position and properties
    /// </summary>
    public static class TaskbarHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct APPBARDATA
        {
            public uint cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public RECT rc;
            public int lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);

        private const uint ABM_GETTASKBARPOS = 5;

        /// <summary>
        /// Gets the current position of the Windows taskbar
        /// </summary>
        /// <returns>TaskbarPosition enum indicating where the taskbar is located</returns>
        public static TaskbarPosition GetTaskbarPosition()
        {
            try
            {
                APPBARDATA data = new APPBARDATA
                {
                    cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA))
                };

                IntPtr result = SHAppBarMessage(ABM_GETTASKBARPOS, ref data);

                if (result == IntPtr.Zero)
                {
                    // If API fails, return Bottom as default
                    return TaskbarPosition.Bottom;
                }

                return (TaskbarPosition)data.uEdge;
            }
            catch
            {
                // In case of error, return Bottom as default
                return TaskbarPosition.Bottom;
            }
        }

        /// <summary>
        /// Checks if the taskbar is in vertical position (Left or Right)
        /// </summary>
        /// <returns>True if taskbar is on left or right side</returns>
        public static bool IsVerticalTaskbar()
        {
            var position = GetTaskbarPosition();
            return position == TaskbarPosition.Left || position == TaskbarPosition.Right;
        }

        /// <summary>
        /// Checks if the taskbar is in horizontal position (Top or Bottom)
        /// </summary>
        /// <returns>True if taskbar is on top or bottom</returns>
        public static bool IsHorizontalTaskbar()
        {
            var position = GetTaskbarPosition();
            return position == TaskbarPosition.Top || position == TaskbarPosition.Bottom;
        }
    }
}
