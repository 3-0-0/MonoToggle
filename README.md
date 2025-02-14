# MonoToggle

MonoToggle is a lightweight Windows system tray application that allows you to quickly toggle between mono and stereo audio output. It modifies the relevant registry setting and restarts the Windows Audio service to apply the changes immediately.

I created this app to be able to quickly change between mono & stereo audio when using earbuds.


## Features

- **Toggle Audio Mode:** Easily switch between mono and stereo audio.
- **System Tray Icon:** Displays a dynamic icon (mono or stereo) based on the current setting.
- **Minimal Resource Usage:** Designed to run quietly in the background with minimal impact on system performance.


## Requirements

- **Operating System:** Windows 11 (should work on other versions of Windows as well, but tested on Windows 11).
- **Administrator Privileges:** **Important!** MonoToggle must be run as administrator because it needs elevated permissions to:
  - Modify the registry setting at `HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio\AccessibilityMonoMixState`
  - Restart the Windows Audio service to apply changes immediately.


## Installation & Usage

1. **Download and Run:**
   - Download the latest release of MonoToggle.
    - [MonoToggle v1.0.0 Release Page](https://github.com/3-0-0/MonoToggle/releases/tag/v1.0.0)
    - [Direct Download: MonoToggle.exe](https://github.com/3-0-0/MonoToggle/releases/download/v1.0.0/MonoToggle.exe)
   - Right-click the executable (`MonoToggle.exe`) and choose **Run as administrator**.
   - The system tray icon will appear. Double-click the icon or use the context menu to toggle between mono and stereo audio.


2. **Auto-Start on Login (Optional):**
   - To have MonoToggle start automatically with Windows, add a shortcut to the Startup folder:
   - Press **Win + R**, Type the following command (including quotes) and press **Enter**:

     explorer "%PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs\Startup"

   - Copy a shortcut of the MonoToggle executable into the opened folder.


3. **Set the Executable to Always Run as Administrator:**
   - Right-click the `MonoToggle.exe` file.
   - Select **Properties**.
   - Go to the **Compatibility** tab.
   - Check **"Run this program as an administrator"**.
   - Click **Apply** and then **OK**.

   **Note:** When auto-starting, Windows may still prompt for UAC confirmation because the app requires administrator privileges. To bypass the UAC prompt, you can create a scheduled task that runs the app with the highest privileges (advanced users).


## License

Open for anyone to download, modify and publish.
