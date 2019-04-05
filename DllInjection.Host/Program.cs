using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DllInjection.Host
{
    class Program
    {
        static async Task Main()
        {
            var process = Process.GetCurrentProcess();
            Console.WriteLine($"Process id = {process.Id}");
            ulong i = 0;
            while (true)
            {
                Console.WriteLine($"t={i++}");
                await Task.Delay(TimeSpan.FromSeconds(0.5));
            }
        }
    }
}
