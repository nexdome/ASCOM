namespace TA.NexDome.Server
    {
    partial class FirmwareUpdate
        {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
            {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirmwareUpdate));
            this.ConsoleOutput = new ConsoleControl.ConsoleControl();
            this.FirmwareImageName = new System.Windows.Forms.ComboBox();
            this.InputGroup = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ComPortName = new System.Windows.Forms.ComboBox();
            this.UpdateCommand = new System.Windows.Forms.Button();
            this.ConsoleGroup = new System.Windows.Forms.GroupBox();
            this.UpdateProcessError = new System.Windows.Forms.ErrorProvider(this.components);
            this.ShowAllComPorts = new System.Windows.Forms.CheckBox();
            this.BalloonHelp = new System.Windows.Forms.ToolTip(this.components);
            this.VerboseLogging = new System.Windows.Forms.CheckBox();
            this.InputGroup.SuspendLayout();
            this.ConsoleGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateProcessError)).BeginInit();
            this.SuspendLayout();
            // 
            // ConsoleOutput
            // 
            this.ConsoleOutput.AutoScroll = true;
            this.ConsoleOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConsoleOutput.IsInputEnabled = true;
            this.ConsoleOutput.Location = new System.Drawing.Point(3, 16);
            this.ConsoleOutput.Name = "ConsoleOutput";
            this.ConsoleOutput.SendKeyboardCommandsToProcess = false;
            this.ConsoleOutput.ShowDiagnostics = true;
            this.ConsoleOutput.Size = new System.Drawing.Size(694, 427);
            this.ConsoleOutput.TabIndex = 0;
            this.BalloonHelp.SetToolTip(this.ConsoleOutput, "Displays log output from the flash programmer");
            // 
            // FirmwareImageName
            // 
            this.FirmwareImageName.FormattingEnabled = true;
            this.FirmwareImageName.Location = new System.Drawing.Point(99, 19);
            this.FirmwareImageName.Name = "FirmwareImageName";
            this.FirmwareImageName.Size = new System.Drawing.Size(229, 21);
            this.FirmwareImageName.TabIndex = 1;
            this.BalloonHelp.SetToolTip(this.FirmwareImageName, resources.GetString("FirmwareImageName.ToolTip"));
            this.FirmwareImageName.SelectedIndexChanged += new System.EventHandler(this.FirmwareImageName_SelectedIndexChanged);
            // 
            // InputGroup
            // 
            this.InputGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputGroup.Controls.Add(this.VerboseLogging);
            this.InputGroup.Controls.Add(this.label1);
            this.InputGroup.Controls.Add(this.ShowAllComPorts);
            this.InputGroup.Controls.Add(this.ComPortName);
            this.InputGroup.Controls.Add(this.label2);
            this.InputGroup.Controls.Add(this.FirmwareImageName);
            this.InputGroup.Controls.Add(this.UpdateCommand);
            this.InputGroup.Location = new System.Drawing.Point(13, 13);
            this.InputGroup.Name = "InputGroup";
            this.InputGroup.Size = new System.Drawing.Size(699, 84);
            this.InputGroup.TabIndex = 2;
            this.InputGroup.TabStop = false;
            this.InputGroup.Text = "Firmware Selections";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Firmware Image";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(334, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "COM Port";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ComPortName
            // 
            this.ComPortName.FormattingEnabled = true;
            this.ComPortName.Location = new System.Drawing.Point(399, 19);
            this.ComPortName.Name = "ComPortName";
            this.ComPortName.Size = new System.Drawing.Size(190, 21);
            this.ComPortName.TabIndex = 4;
            this.BalloonHelp.SetToolTip(this.ComPortName, "Select the COM port to upload firmware to.");
            this.ComPortName.SelectedIndexChanged += new System.EventHandler(this.ComPortName_SelectedIndexChanged);
            // 
            // UpdateCommand
            // 
            this.UpdateProcessError.SetIconPadding(this.UpdateCommand, 8);
            this.UpdateCommand.Location = new System.Drawing.Point(595, 19);
            this.UpdateCommand.Name = "UpdateCommand";
            this.UpdateCommand.Size = new System.Drawing.Size(79, 23);
            this.UpdateCommand.TabIndex = 5;
            this.UpdateCommand.Text = "Update";
            this.BalloonHelp.SetToolTip(this.UpdateCommand, "Start the update process");
            this.UpdateCommand.UseVisualStyleBackColor = true;
            // 
            // ConsoleGroup
            // 
            this.ConsoleGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsoleGroup.Controls.Add(this.ConsoleOutput);
            this.ConsoleGroup.Location = new System.Drawing.Point(12, 103);
            this.ConsoleGroup.Name = "ConsoleGroup";
            this.ConsoleGroup.Size = new System.Drawing.Size(700, 446);
            this.ConsoleGroup.TabIndex = 3;
            this.ConsoleGroup.TabStop = false;
            this.ConsoleGroup.Text = "Flash Programmer Output";
            // 
            // UpdateProcessError
            // 
            this.UpdateProcessError.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.UpdateProcessError.ContainerControl = this;
            // 
            // ShowAllComPorts
            // 
            this.ShowAllComPorts.AutoSize = true;
            this.ShowAllComPorts.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ShowAllComPorts.Location = new System.Drawing.Point(399, 46);
            this.ShowAllComPorts.Name = "ShowAllComPorts";
            this.ShowAllComPorts.Size = new System.Drawing.Size(118, 17);
            this.ShowAllComPorts.TabIndex = 6;
            this.ShowAllComPorts.Text = "Show all COM Ports";
            this.ShowAllComPorts.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.BalloonHelp.SetToolTip(this.ShowAllComPorts, resources.GetString("ShowAllComPorts.ToolTip"));
            this.ShowAllComPorts.UseVisualStyleBackColor = true;
            this.ShowAllComPorts.CheckedChanged += new System.EventHandler(this.ShowAllComPorts_CheckedChanged);
            // 
            // BalloonHelp
            // 
            this.BalloonHelp.AutomaticDelay = 0;
            this.BalloonHelp.AutoPopDelay = 0;
            this.BalloonHelp.InitialDelay = 300;
            this.BalloonHelp.ReshowDelay = 250;
            this.BalloonHelp.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.BalloonHelp.ToolTipTitle = "Help with Controls";
            // 
            // VerboseLogging
            // 
            this.VerboseLogging.AutoSize = true;
            this.VerboseLogging.Checked = global::TA.NexDome.Server.Properties.Settings.Default.FirmwareUploadVerboseOutput;
            this.VerboseLogging.Location = new System.Drawing.Point(99, 47);
            this.VerboseLogging.Name = "VerboseLogging";
            this.VerboseLogging.Size = new System.Drawing.Size(106, 17);
            this.VerboseLogging.TabIndex = 7;
            this.VerboseLogging.Text = "Verbose Logging";
            this.BalloonHelp.SetToolTip(this.VerboseLogging, "Enables verbose logging mode for the flash programmer.\r\nDefault: disabled\r\n\r\nEnab" +
        "le this only if you are troublshooting firmware update issues.");
            this.VerboseLogging.UseVisualStyleBackColor = true;
            // 
            // FirmwareUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 561);
            this.Controls.Add(this.ConsoleGroup);
            this.Controls.Add(this.InputGroup);
            this.MinimumSize = new System.Drawing.Size(740, 600);
            this.Name = "FirmwareUpdate";
            this.Text = "Firmware Update Utility";
            this.Load += new System.EventHandler(this.FirmwareUpdate_Load);
            this.InputGroup.ResumeLayout(false);
            this.InputGroup.PerformLayout();
            this.ConsoleGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UpdateProcessError)).EndInit();
            this.ResumeLayout(false);

            }

        #endregion
        private ConsoleControl.ConsoleControl ConsoleOutput;
        private System.Windows.Forms.ComboBox FirmwareImageName;
        private System.Windows.Forms.GroupBox InputGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComPortName;
        private System.Windows.Forms.Button UpdateCommand;
        private System.Windows.Forms.GroupBox ConsoleGroup;
        private System.Windows.Forms.ErrorProvider UpdateProcessError;
        private System.Windows.Forms.ToolTip BalloonHelp;
        private System.Windows.Forms.CheckBox VerboseLogging;
        private System.Windows.Forms.CheckBox ShowAllComPorts;
        }
    }