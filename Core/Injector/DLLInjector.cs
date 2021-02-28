using Lunar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeLLaGUI
{
    class DLLInjector
    {
		public const int MAX_PATH = 260;

		//private PExecutable PExecutable;

		private MemoryManager ProcessMemoryManager;

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool CloseHandle(IntPtr hHandle);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string moduleName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll")]
		private static extern IntPtr CreateRemoteThread(IntPtr hProcess,
		   IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress,
		   IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

		public void InjectDll(string dllPath, string processName, InjectionType injectionType)
        {
			if (string.IsNullOrWhiteSpace(dllPath) || !File.Exists(dllPath))
			{
				throw new ArgumentException("The provided file path did not point to a valid file");
			}

			Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName));

			if (processes.Length == 0)
			{
				throw new ArgumentException("The provided process could not be found");
			}

			Process process = processes[0];

			if (process is null || process.HasExited || process.Handle == IntPtr.Zero)
			{
				throw new ArgumentException("The provided process is not currently running");
			}

			if(process == Process.GetCurrentProcess())
            {
				throw new ArgumentException("Cannot inject DLL to injector");
			}

			ProcessMemoryManager = new MemoryManager(process);

			switch (injectionType)
			{
				case InjectionType.Kernell:
					InjectLoadLibraryDependant(dllPath);
					return;
				case InjectionType.Manual:
					InjectManualMap(dllPath);
					return;
			}
		}

        private void InjectManualMap(string dllPath)
        {
			var flags = MappingFlags.DiscardHeaders;

			var mapper = new LibraryMapper(ProcessMemoryManager.Process, dllPath, flags);

			mapper.MapLibrary();
		}

        private void InjectLoadLibraryDependant(string dllPath)
        {
			IntPtr DllBaseAddress = ProcessMemoryManager.AllocateMemory(MAX_PATH);

			if (DllBaseAddress.Equals(0))
			{
				throw new ArgumentException("Could not allocate memory");
			}

			ProcessMemoryManager.WriteArray(DllBaseAddress, dllPath.AsSpan());

			IntPtr loadlibAddy = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

			IntPtr hThread = CreateRemoteThread(ProcessMemoryManager.Process.Handle, IntPtr.Zero, 0, loadlibAddy, DllBaseAddress, 0, out _);


			if (hThread.Equals(0))
			{
				throw new ArgumentException("Could not create thread");
			}
			//?
			CloseHandle(hThread);
			ProcessMemoryManager.Process.Dispose();
		}
    }
}
