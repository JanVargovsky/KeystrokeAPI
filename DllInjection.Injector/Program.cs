using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using static DllInjection.Injector.WinAPI;

namespace DllInjection.Injector
{
    class Program
    {
        static void Inject(string dllPath, Process targetProcess)
        {
            const ProcessAccessFlags processAccessFlags = ProcessAccessFlags.CreateThread | ProcessAccessFlags.QueryInformation |
                ProcessAccessFlags.VirtualMemoryOperation | ProcessAccessFlags.VirtualMemoryWrite | ProcessAccessFlags.VirtualMemoryRead;
            var processHandle = OpenProcess(processAccessFlags, false, targetProcess.Id);
            Debug.Assert(processHandle != IntPtr.Zero);
            var kernel32Handle = GetModuleHandle("kernel32.dll");
            Debug.Assert(kernel32Handle != IntPtr.Zero);
            var loadLibraryAAddress = GetProcAddress(kernel32Handle, "LoadLibraryA");
            Debug.Assert(loadLibraryAAddress != IntPtr.Zero);
            var dllPathBuffer = Encoding.Default.GetBytes(dllPath);
            var dllPathSize = (uint)dllPathBuffer.Length;
            var dllPathAddress = VirtualAllocEx(processHandle, IntPtr.Zero, dllPathSize,
                AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ReadWrite);
            var writeProcessMemoryResult = WriteProcessMemory(processHandle, dllPathAddress, dllPathBuffer, dllPathSize, out var bytesWritten);
            Debug.Assert(writeProcessMemoryResult);
            Debug.Assert((uint)bytesWritten == dllPathSize);
            var createRemoteThreadResult = CreateRemoteThread(processHandle, IntPtr.Zero, 0, loadLibraryAAddress, dllPathAddress, 0, out var threadId);
            Debug.Assert(createRemoteThreadResult != IntPtr.Zero);
            Debug.Assert(threadId != IntPtr.Zero);
        }

        static void Main()
        {
            Console.Write("Enter PID:");
            var pid = int.Parse(Console.ReadLine());
            var process = Process.GetProcessById(pid);
            Console.WriteLine($"Attach to the {process.ProcessName}? [y/n]");
            if (Console.ReadLine() != "y") return;
            const string configuration = "Debug";
            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../../../..", configuration, "DllInjection.InjectedNativeDll.dll"));
            Debug.Assert(File.Exists(path));
            Inject(path, process);
            Console.WriteLine("Injected");
        }
    }
}
