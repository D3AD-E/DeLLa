using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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

        private void ButtonInjectClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DLLPath = openFileDialog.FileName;
                labelCurrentDLL.Text = $"Current DLL: {openFileDialog.SafeFileName}";
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

            DLLPath += ".SAD";

            Log("Started injection...");
            Log($"DLL: {DLLPath}");
            Log($"Process: {process}");

            DLLInjector injector = new();
            await Task.Run(() =>
            {
                try
                {
                    if (rBLoadLibrary.Checked)
                        injector.InjectDll(DLLPath, process, InjectionType.Kernell);
                    else if (rBManualMap.Checked)
                        injector.InjectDll(DLLPath, process, InjectionType.Manual);
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
            });
        }
    }
}
