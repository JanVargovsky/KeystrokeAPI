using System;
using System.Diagnostics;

namespace ConsoleApplicationTest
{
    enum FirewallChangeState
    {
        Enable,
        Disable
    }

    class Firewall
    {
        public void Change(FirewallChangeState state)
        {
            var enabledValue = state == FirewallChangeState.Enable ? bool.TrueString : bool.FalseString;
            var psi = new ProcessStartInfo("powershell.exe")
            {
                Arguments = $"Set-NetFirewallProfile -Profile Domain,Public,Private -Enabled {enabledValue}",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
            }
        }

        public void Enable()
        {
            Change(FirewallChangeState.Enable);
            Console.WriteLine("Firewall enabled");
        }

        public void Disable()
        {
            Change(FirewallChangeState.Disable);
            Console.WriteLine("Firewall disabled");
        }
    }
}
