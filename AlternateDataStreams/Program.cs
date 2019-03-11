using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AlternateDataStreams
{
    class Program
    {
        // http://www.pinvoke.net/default.aspx/kernel32/CreateFile.html
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFile(
             [MarshalAs(UnmanagedType.LPTStr)] string filename,
             [MarshalAs(UnmanagedType.U4)] FileAccess access,
             [MarshalAs(UnmanagedType.U4)] FileShare share,
             IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
             [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
             [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
             IntPtr templateFile);

        static async Task Main(string[] args)
        {
            //CreateFile("test.txt", FileAccess.Write, FileShare.None, IntPtr.Zero, FileMode.Create, 0, IntPtr.Zero);
            //CreateFile("test.txt:alt", FileAccess.Write, FileShare.None, IntPtr.Zero, FileMode.Create, 0, IntPtr.Zero);

            using (var sw = new StreamWriter(@"test2.txt"))
            {
                await sw.WriteLineAsync("hi");
                await sw.FlushAsync();
            }
            using (var sw = new StreamWriter(@"test2.txt:alt3"))
            {
                await sw.WriteLineAsync("hello");
                await sw.FlushAsync();
            }
            using (var sr = new StreamReader(@"test2.txt"))
            {
                var line = await sr.ReadToEndAsync();
                Console.Write(line);
            }
            using (var sr = new StreamReader(@"test2.txt:alt3"))
            {
                var line = await sr.ReadToEndAsync();
                Console.Write(line);
            }
        }
    }
}
