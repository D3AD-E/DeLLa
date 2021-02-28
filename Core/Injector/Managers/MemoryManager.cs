using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        
    }
}
