using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NLog.Fluent;

namespace TA.NexDome.Server
    {
    /// <summary>
    ///     Class WindowUtils.
    /// </summary>
    internal static class WindowUtils
        {
        /// <summary>
        ///     Determines whether the specified <paramref name="form" /> is fully visible given the current monitor
        ///     configuration. Method inspired by @Andrija https://stackoverflow.com/a/987090/98516
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns><c>true</c> if the <paramref name="form" /> is fully on-screen; otherwise, <c>false</c> .</returns>
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
        ///     Ensures that the specified <paramref name="form" /> is fully visible. If not, it will be moved to the
        ///     primary monitor
        /// </summary>
        /// <param name="form">The form.</param>
        public static void EnsureVisible(this Form form)
            {
            if (!IsOnScreen(form))
                {
                var workingArea = Screen.PrimaryScreen.WorkingArea;
                form.Location = workingArea.Location;
                }
            }

        /// <summary>
        ///     Navigates to web destination for a control. The target URL is contained in the Tag property of the
        ///     control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void NavigateToWebDestination(object sender)
            {
            Cursor.Current = Cursors.AppStarting;
            var control = sender as Control;
            try
                {
                if (control == null)
                    {
                    Log.Warn()
                        .Message("Navigate to web destination failed: argument is not a control")
                        .Write();
                    return;
                    }
                string url = control.Tag.ToString();
                if (!url.StartsWith("http"))
                    {
                    Log.Warn()
                        .Message("Navigate to web destination failed. Control {control} did not contain a valid URL",
                            control.Name)
                        .Write();
                    return;
                    }
                Log.Debug()
                    .Message("Control {control} launching web destination: {url}", control.Name, url)
                    .Write();
                Process.Start(url);
                }
            catch (Exception)
                {
                Log.Warn()
                    .Message("Unable to navigate to web destination for control {control}", control?.Name ?? "unnamed")
                    .Write();
                }
            finally
                {
                Cursor.Current = Cursors.Default;
                }
            }

        #region Native methods
        private const int MONITOR_DEFAULTTONULL = 0;
        private const int MONITOR_DEFAULTTOPRIMARY = 1;
        private const int MONITOR_DEFAULTTONEAREST = 2;

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
        #endregion Native methods
        }
    }