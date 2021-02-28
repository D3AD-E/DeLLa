
namespace DeLLaGUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rBLoadLibrary = new System.Windows.Forms.RadioButton();
            this.rBManualMap = new System.Windows.Forms.RadioButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ButtonInject = new System.Windows.Forms.Button();
            this.rTBLog = new System.Windows.Forms.RichTextBox();
            this.cBProcesses = new System.Windows.Forms.ComboBox();
            this.ButtonChooseDll = new System.Windows.Forms.Button();
            this.labelCurrentDLL = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cBShowVisibleProcesses = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rBLoadLibrary
            // 
            this.rBLoadLibrary.AutoSize = true;
            this.rBLoadLibrary.Location = new System.Drawing.Point(269, 43);
            this.rBLoadLibrary.Name = "rBLoadLibrary";
            this.rBLoadLibrary.Size = new System.Drawing.Size(87, 19);
            this.rBLoadLibrary.TabIndex = 0;
            this.rBLoadLibrary.TabStop = true;
            this.rBLoadLibrary.Text = "LoadLibrary";
            this.rBLoadLibrary.UseVisualStyleBackColor = true;
            // 
            // rBManualMap
            // 
            this.rBManualMap.AutoSize = true;
            this.rBManualMap.Location = new System.Drawing.Point(269, 68);
            this.rBManualMap.Name = "rBManualMap";
            this.rBManualMap.Size = new System.Drawing.Size(92, 19);
            this.rBManualMap.TabIndex = 1;
            this.rBManualMap.TabStop = true;
            this.rBManualMap.Text = "Manual Map";
            this.rBManualMap.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // ButtonInject
            // 
            this.ButtonInject.Location = new System.Drawing.Point(115, 267);
            this.ButtonInject.Name = "ButtonInject";
            this.ButtonInject.Size = new System.Drawing.Size(152, 40);
            this.ButtonInject.TabIndex = 2;
            this.ButtonInject.Text = "Inject";
            this.ButtonInject.UseVisualStyleBackColor = true;
            this.ButtonInject.Click += new System.EventHandler(this.ButtonInject_Click);
            // 
            // rTBLog
            // 
            this.rTBLog.Location = new System.Drawing.Point(11, 163);
            this.rTBLog.Name = "rTBLog";
            this.rTBLog.ReadOnly = true;
            this.rTBLog.ShortcutsEnabled = false;
            this.rTBLog.Size = new System.Drawing.Size(355, 98);
            this.rTBLog.TabIndex = 3;
            this.rTBLog.Text = "";
            // 
            // cBProcesses
            // 
            this.cBProcesses.FormattingEnabled = true;
            this.cBProcesses.Location = new System.Drawing.Point(11, 134);
            this.cBProcesses.Name = "cBProcesses";
            this.cBProcesses.Size = new System.Drawing.Size(355, 23);
            this.cBProcesses.TabIndex = 4;
            this.cBProcesses.DropDown += new System.EventHandler(this.cBProcesses_DropDown);
            // 
            // ButtonChooseDll
            // 
            this.ButtonChooseDll.Location = new System.Drawing.Point(10, 22);
            this.ButtonChooseDll.Name = "ButtonChooseDll";
            this.ButtonChooseDll.Size = new System.Drawing.Size(100, 30);
            this.ButtonChooseDll.TabIndex = 5;
            this.ButtonChooseDll.Text = "Choose DLL";
            this.ButtonChooseDll.UseVisualStyleBackColor = true;
            this.ButtonChooseDll.Click += new System.EventHandler(this.ButtonInjectClick);
            // 
            // labelCurrentDLL
            // 
            this.labelCurrentDLL.AutoSize = true;
            this.labelCurrentDLL.Location = new System.Drawing.Point(10, 62);
            this.labelCurrentDLL.Name = "labelCurrentDLL";
            this.labelCurrentDLL.Size = new System.Drawing.Size(103, 15);
            this.labelCurrentDLL.TabIndex = 6;
            this.labelCurrentDLL.Text = "Current Dll: NONE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(269, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Injection type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Choose process";
            // 
            // cBShowVisibleProcesses
            // 
            this.cBShowVisibleProcesses.AutoSize = true;
            this.cBShowVisibleProcesses.Checked = true;
            this.cBShowVisibleProcesses.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cBShowVisibleProcesses.Location = new System.Drawing.Point(12, 94);
            this.cBShowVisibleProcesses.Name = "cBShowVisibleProcesses";
            this.cBShowVisibleProcesses.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cBShowVisibleProcesses.Size = new System.Drawing.Size(211, 19);
            this.cBShowVisibleProcesses.TabIndex = 9;
            this.cBShowVisibleProcesses.Text = "Show only processes with windows";
            this.cBShowVisibleProcesses.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 313);
            this.Controls.Add(this.cBShowVisibleProcesses);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelCurrentDLL);
            this.Controls.Add(this.ButtonChooseDll);
            this.Controls.Add(this.cBProcesses);
            this.Controls.Add(this.rTBLog);
            this.Controls.Add(this.ButtonInject);
            this.Controls.Add(this.rBManualMap);
            this.Controls.Add(this.rBLoadLibrary);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "DeLLa";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rBLoadLibrary;
        private System.Windows.Forms.RadioButton rBManualMap;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button ButtonInject;
        private System.Windows.Forms.RichTextBox rTBLog;
        private System.Windows.Forms.ComboBox cBProcesses;
        private System.Windows.Forms.Button ButtonChooseDll;
        private System.Windows.Forms.Label labelCurrentDLL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cBShowVisibleProcesses;
    }
}

