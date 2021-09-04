using DeLLaGUI.Enums;
using Lunar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static DeLLaGUI.Core.Injector.Support.ProcessContext;

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

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool IsWow64Process(IntPtr processHandle, out bool isWow64Process);

		[DllImport("ntdll.dll")]
		internal static extern int NtCreateThreadEx(out IntPtr threadHandle, AccessType accessMask, IntPtr objectAttributes, IntPtr processHandle, IntPtr startAddress, IntPtr argument, ThreadCreationType flags, nint zeroBits, nint stackSize, nint maximumStackSize, IntPtr attributeList);

		//[DllImport("kernel32.dll", SetLastError = true)]
		//static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

		//// Get context of thread x64, in x64 application
		//[DllImport("kernel32.dll", SetLastError = true)]
		//static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT64 lpContext);

		//[DllImport("kernel32.dll", SetLastError = true)]
		//static extern bool SetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

		//// Get context of thread x64, in x64 application
		//[DllImport("kernel32.dll", SetLastError = true)]
		//static extern bool SetThreadContext(IntPtr hThread, ref CONTEXT64 lpContext);

		//[DllImport("kernel32.dll", SetLastError = true)]
		//static extern uint ResumeThread(IntPtr hThread);

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

			if (process is null || process.HasExited || process.Handle.Equals(0))
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
					break;
				case InjectionType.Manual:
					InjectManualMap(dllPath);
					break;
				case InjectionType.NtCreateThread:
					InjectCreateRemoteThreadNt(dllPath);
					break;
			}
		}

        private void InjectManualMap(string dllPath)
        {
			var flags = MappingFlags.DiscardHeaders;

			var mapper = new LibraryMapper(ProcessMemoryManager.Process, dllPath, flags);

			mapper.MapLibrary();

			ProcessMemoryManager.Process.Dispose();
		}

		private bool IsProcess64(Process process)
        {
			if (!Environment.Is64BitOperatingSystem)
			{
				return false;
			}

			if (!IsWow64Process(process.Handle, out var isWow64Process))
			{
				throw new Exception("64 bit process on 32 bit system?");
			}

			return !isWow64Process;
		}


		private void InjectCreateRemoteThreadNt(string dllPath)
        {
			IntPtr DllBaseAddress = ProcessMemoryManager.AllocateMemory(MAX_PATH);

			ProcessMemoryManager.WriteArray(DllBaseAddress, dllPath.AsSpan());

			IntPtr loadlibAddy = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

			if (loadlibAddy.Equals(0))
			{
				throw new Exception("kernel32.dll was not found");
			}

			int res = NtCreateThreadEx(out var threadHandle, AccessType.SpecificRightsAll | AccessType.StandardRightsAll, IntPtr.Zero, ProcessMemoryManager.Process.Handle, loadlibAddy, DllBaseAddress, ThreadCreationType.HideFromDebugger, 0, 0, 0, IntPtr.Zero);
			
			if(threadHandle.Equals(0)||res<0)
            {
				throw new ApplicationException($"Failed to call the entry point of the DLL");
			}

			//CloseHandle(hThread);
			ProcessMemoryManager.Process.Dispose();
		}

		private void InjectLoadLibraryDependant(string dllPath)
        {
			IntPtr DllBaseAddress = ProcessMemoryManager.AllocateMemory(MAX_PATH);

			ProcessMemoryManager.WriteArray(DllBaseAddress, dllPath.AsSpan());

			IntPtr loadlibAddy = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

			if(loadlibAddy.Equals(0))
            {
				throw new Exception("kernel32.dll was not found");
			}

			IntPtr hThread = CreateRemoteThread(ProcessMemoryManager.Process.Handle, IntPtr.Zero, 0, loadlibAddy, DllBaseAddress, 0, out _);


			if (hThread.Equals(0))
			{
				throw new Exception("Could not create thread");
			}
			
			CloseHandle(hThread);
			ProcessMemoryManager.Process.Dispose();
		}
    }
}
