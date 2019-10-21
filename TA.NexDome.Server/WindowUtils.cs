using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TA.NexDome.Server
    {
    /// <summary>
    /// Class WindowUtils.
    /// </summary>
    internal static class WindowUtils
        {
        #region Native methods
        const int MONITOR_DEFAULTTONULL = 0;
        const int MONITOR_DEFAULTTOPRIMARY = 1;
        const int MONITOR_DEFAULTTONEAREST = 2;
        [DllImport("user32.dll")] static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
        #endregion Native methods

        /// <summary>
        /// Determines whether the specified form is fully visible given the current monitor configuration.
        /// Method inspired by @Andrija https://stackoverflow.com/a/987090/98516
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns><c>true</c> if the form is fully on-screen; otherwise, <c>false</c>.</returns>
        public static bool IsOnScreen(Form form)
            {
            var screens = Screen.AllScreens;
            foreach (var screen in screens)
                {
                var formRectangle = new Rectangle(form.Left, form.Top,
                    form.Width, form.Height);

                if (screen.WorkingArea.Contains(formRectangle)) return true;
                }
            return false;
            }

        /// <summary>
        /// Ensures that the specified form is fully visible. If not, it will be moved to the primary monitor
        /// </summary>
        /// <param name="form">The form.</param>
        public static void EnsureVisible(Form form)
            {
            if (!IsOnScreen(form))
                {
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                form.Location = workingArea.Location;
                }
            }
        }
    }