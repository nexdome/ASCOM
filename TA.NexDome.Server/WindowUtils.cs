// This file is part of the TA.NexDome.AscomServer project
// 
// Copyright © 2015-2020 Tigra Astronomy, all rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so. The Software comes with no warranty of any kind.
// You make use of the Software entirely at your own risk and assume all liability arising from your use thereof.
// 
// File: WindowUtils.cs  Last modified: 2020-07-21@22:26 by Tim Long

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ninject;
using TA.Utils.Core.Diagnostics;

namespace TA.NexDome.Server
    {
    /// <summary>Class WindowUtils.</summary>
    internal static class WindowUtils
        {
        private static ILog Log => CompositionRoot.Kernel.Get<ILog>();

        /// <summary>
        ///     Determines whether the specified <paramref name="form" /> is fully visible given the current
        ///     monitor configuration. Method inspired by @Andrija https://stackoverflow.com/a/987090/98516
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
        ///     Ensures that the specified <paramref name="form" /> is fully visible. If not, it will be moved
        ///     to the primary monitor
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
        ///     Navigates to web destination for a control. The target URL is contained in the Tag property of
        ///     the control.
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