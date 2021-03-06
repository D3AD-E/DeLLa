﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeLLaGUI
{
    class MemoryManager
    {
        public readonly Process Process;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
        IntPtr hProcess, IntPtr lpBaseAddress, in byte lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
                            uint dwSize, AllocationType flAllocationType, MemoryProtectionType flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualFreeEx(IntPtr processHandle, IntPtr address, nint size, int freeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(IntPtr processHandle, IntPtr address, out byte bytes, nint size, out nint bytesRead);

        public MemoryManager(Process process)
        {
            Process = process;
        }

        public void WriteArray<T>(IntPtr sectionAddr, ReadOnlySpan<T> array) where T : unmanaged
        {
            var arrayBytes = MemoryMarshal.AsBytes(array);

            bool result = WriteProcessMemory(Process.Handle, sectionAddr, arrayBytes[0], arrayBytes.Length, out IntPtr bytesRead);

            if (!result || bytesRead.Equals(0))
            {
                throw new ArgumentException("Could not write/read memory");
            }
        }

        public void WriteArray<T>(IntPtr sectionAddr, Span<T> array) where T : unmanaged
        {
            var arrayBytes = MemoryMarshal.AsBytes(array);

            bool result = WriteProcessMemory(Process.Handle, sectionAddr, arrayBytes[0], arrayBytes.Length, out IntPtr bytesRead);

            if (!result || bytesRead.Equals(0))
            {
                throw new ArgumentException("Could not write/read memory");
            }
        }

        public IntPtr AllocateMemory(uint size, bool executable = false)
        {
            var protectionType = executable ? MemoryProtectionType.ExecuteReadWrite : MemoryProtectionType.ReadWrite;

            var address = VirtualAllocEx(Process.Handle, IntPtr.Zero, size, AllocationType.Commit | AllocationType.Reserve, protectionType);

            if (address == IntPtr.Zero)
            {
                throw new ArgumentException("Could not allocate memory");
            }

            return address;
        }
        public T ReadStructure<T>(IntPtr address) where T : unmanaged
        {
            Span<byte> structureBytes = stackalloc byte[Unsafe.SizeOf<T>()];

            if (!ReadProcessMemory(Process.Handle, address, out structureBytes[0], structureBytes.Length, out _))
            {
                throw new Win32Exception();
            }

            return MemoryMarshal.Read<T>(structureBytes);
        }
        public void FreeMemory(IntPtr address)
        {
            if (!VirtualFreeEx(Process.Handle, address, 0, 0x8000))
            {
                throw new Win32Exception();
            }
        }

        public IntPtr GetEntryAdress(string dllName)
        {
            ProcessModule myProcessModule;
            // Get all the modules associated with 'myProcess'.
            ProcessModuleCollection myProcessModuleCollection = Process.Modules;

            // Display the 'EntryPointAddress' of each of the modules.
            for (int i = 0; i < myProcessModuleCollection.Count; i++)
            {
                myProcessModule = myProcessModuleCollection[i];
                if(dllName == myProcessModule.ModuleName)
                    return myProcessModule.EntryPointAddress;
            }

            throw new Exception("Module name was not foind in process");
        }
        
    }
}
