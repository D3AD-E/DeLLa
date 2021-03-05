using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeLLaGUI.Core.Obfuscator
{
    class Obfuscator
    {
        //returns path to new DLL

        public static string RenameDll(string path)
        {
            var rand = new Random();
            var size = rand.Next(1,20);

            string newPath = Application.StartupPath + @"\Temp\" + RandomString(size) + ".dll";

            if (!Directory.Exists(Application.StartupPath + @"\Temp\"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Temp\");
            }

            File.Copy(path, newPath);
            return newPath;
        }

        private static string RandomString(int length)
        {
            var rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        public static string ChangeHashSumDll(string path)
        {
            
            string newPath = Application.StartupPath + @"\Temp\" + Path.GetFileName(path);

            if (!Directory.Exists(Application.StartupPath + @"\Temp\"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Temp\");
            }

            if (!File.Exists(newPath))
            {
                File.Copy(path, newPath);
            }

            var rand = new Random();
            var size = rand.Next(5, 1001);
            var bytes = new byte[size];

            rand.NextBytes(bytes);

            using var stream = new FileStream(newPath, FileMode.Append);
            stream.Write(bytes, 0, bytes.Length);
            return newPath;
        }

        public static void ClearTemp()
        {
            if (!File.Exists(Application.StartupPath + @"\Temp"))
                return;
            System.IO.DirectoryInfo di = new DirectoryInfo(Application.StartupPath + @"\Temp");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
