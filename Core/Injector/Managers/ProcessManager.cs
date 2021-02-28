using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeLLaGUI
{
    static class ProcessManager
    {
        public static List<string> GetAllProcesses()
        {
            var processes = Process.GetProcesses();
            var toret = new List<string>();

            foreach (var process in processes)
            {
                //.Where(p => !string.IsNullOrEmpty(p.ProcessName));
                try
                {
                    if (!string.IsNullOrEmpty(process.ProcessName) && process.Handle != IntPtr.Zero)
                        toret.Add(process.ProcessName.EndsWith(".exe") ? process.ProcessName : process.ProcessName+".exe");
                }
                catch(Exception e)
                {

                }
            }

            return toret;
        }

        public static IEnumerable<string> GetAllVisibleProcesses()
        {
            var processes = Process.GetProcesses().Where(p => p.MainWindowHandle != IntPtr.Zero && !string.IsNullOrEmpty(p.MainWindowTitle));
            foreach (var process in processes)
            {
                yield return process.ProcessName.EndsWith(".exe") ? process.ProcessName : process.ProcessName + ".exe";
            }
        }
    }
}
