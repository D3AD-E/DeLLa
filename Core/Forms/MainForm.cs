using DeLLaGUI.Core.Obfuscator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeLLaGUI
{
    public partial class MainForm : Form
    {
        string DLLPath;
        public MainForm()
        {
            InitializeComponent();
        }

        private void SetupDll(string path)
        {
            if (Path.GetExtension(path) == ".dll")
            {
                DLLPath = path;
                labelCurrentDLL.Text = $"Current DLL: {Path.GetFileName(path)}";
            }
            else
            {
                MessageBox.Show("Selected file is not .DLL", "Invalid file extention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ButtonChooseDLLClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SetupDll(openFileDialog.FileName);
            }
        }

        private void cBProcesses_DropDown(object sender, EventArgs e)
        {
            cBProcesses.Items.Clear();
            if(cBShowVisibleProcesses.Checked)
            {
                foreach (var process in ProcessManager.GetAllVisibleProcesses())
                {
                    cBProcesses.Items.Add(process);
                }
            }
            else
            {
                foreach (var process in ProcessManager.GetAllProcesses())
                {
                    cBProcesses.Items.Add(process);
                }
            }

            if (cBProcesses.Items.Count > 0)
                Log("Successfully got processes");
            else
                Log("Got 0 processes, try running as admin");
        }

        private void Log(string message)
        {
            string timestamp = DateTime.UtcNow.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
            rTBLog.AppendText($"[{timestamp}] {message}\n");
        }

        private async void ButtonInject_ClickAsync(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DLLPath))
            {
                MessageBox.Show("No DLL selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(cBProcesses.SelectedItem.ToString()))
            {
                MessageBox.Show("No process selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var process = cBProcesses.SelectedItem.ToString();

            Log("Started injection...");
            Log($"DLL: {DLLPath}");
            Log($"Process: {process}");

            Obfuscator.ClearTemp();

            if (cBRndName.Checked)
            {
                Log("Randomizing DLL name ...");
                DLLPath = Obfuscator.RenameDll(DLLPath);
            }
            if(cBRndHash.Checked)
            {
                Log("Randomizing DLL HashSum...");
                DLLPath = Obfuscator.ChangeHashSumDll(DLLPath);
            }

            DLLInjector injector = new();
            await Task.Run(() =>
            {
                try
                {
                    if (rBLoadLibrary.Checked)
                        injector.InjectDll(DLLPath, process, InjectionType.Kernell);
                    else if (rBManualMap.Checked)
                        injector.InjectDll(DLLPath, process, InjectionType.Manual);
                    else if (rBNtCreateProc.Checked)
                        injector.InjectDll(DLLPath, process, InjectionType.NtCreateThread);
                    else
                    {
                        throw new ArgumentException("Undefined injection type");
                    }
                    MessageBox.Show("Injected successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error while injecting DLL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Obfuscator.ClearTemp();
                }
            });
        }

        private void rTBLog_TextChanged(object sender, EventArgs e)
        {
            rTBLog.SelectionStart = rTBLog.Text.Length;
            rTBLog.ScrollToCaret();
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (fileList == null || fileList.Length == 0)
                return;
            SetupDll(fileList[0]);
        }
    }
}
