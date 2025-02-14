using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using System.Diagnostics;
using System.ServiceProcess;

namespace MonoToggle
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrayApplicationContext());
        }
    }

    public class TrayApplicationContext : ApplicationContext
    {
        private readonly NotifyIcon trayIcon;
        private const string RegistryPath = @"Software\Microsoft\Multimedia\Audio";
        private const string ValueName = "AccessibilityMonoMixState";

        public TrayApplicationContext()
        {
            // Initialize the tray icon with the current icon based on the registry setting.
            trayIcon = new NotifyIcon
            {
                Icon = GetCurrentIcon(),
                Visible = true,
                Text = "MonoToggle"
            };

            // Set up a context menu with toggle and exit options.
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Toggle Audio", null, ToggleAudio);
            contextMenu.Items.Add("Exit", null, Exit);
            trayIcon.ContextMenuStrip = contextMenu;

            // Also toggle on double-click.
            trayIcon.DoubleClick += ToggleAudio;
        }

        private void ToggleAudio(object sender, EventArgs e)
        {
            ToggleAudioSetting();
            trayIcon.Icon = GetCurrentIcon();
        }

        private void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        /// <summary>
        /// Reads the current audio setting, toggles it, and restarts the audio service.
        /// </summary>
        private void ToggleAudioSetting()
        {
            int currentValue = GetCurrentAudioSetting();
            int newValue = (currentValue == 1) ? 0 : 1; // Toggle: 1 (Mono) -> 0 (Stereo) and vice versa.

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue(ValueName, newValue, RegistryValueKind.DWord);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating registry: " + ex.Message);
            }

            // Restart the Windows Audio service to apply changes immediately.
            RestartAudioService();
        }

        /// <summary>
        /// Gets the current mono/stereo setting from the registry.
        /// </summary>
        private int GetCurrentAudioSetting()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, false))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(ValueName);
                        if (value != null)
                        {
                            return (int)value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading registry: " + ex.Message);
            }
            // If the value is not set, default to stereo (0).
            return 0;
        }

        /// <summary>
        /// Loads and returns the appropriate icon based on the current audio setting.
        /// </summary>
        private Icon GetCurrentIcon()
        {
            // If the registry value is 1, we assume Mono; otherwise, Stereo.
            int currentValue = GetCurrentAudioSetting();

            // Use the embedded icons from the project's resources.
            // Ensure that your Resources.resx file has entries named MonoIcon and StereoIcon.
            return currentValue == 1 ? Properties.Resources.MonoIcon : Properties.Resources.StereoIcon;
        }

        /// <summary>
        /// Restarts the Windows Audio service by running the net commands.
        /// </summary>
        private void RestartAudioService()
        {
            try
            {
                ServiceController sc = new ServiceController("Audiosrv");
                TimeSpan timeout = TimeSpan.FromSeconds(10);

                // Stop the service if it's running.
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }

                // Start the service again.
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error restarting audio service: " + ex.Message);
            }
        }
    }
}
