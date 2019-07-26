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
            this.ConsoleOutput = new ConsoleControl.ConsoleControl();
            this.FirmwareImageName = new System.Windows.Forms.ComboBox();
            this.InputGroup = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ComPortName = new System.Windows.Forms.ComboBox();
            this.UpdateCommand = new System.Windows.Forms.Button();
            this.ConsoleGroup = new System.Windows.Forms.GroupBox();
            this.UpdateProcessError = new System.Windows.Forms.ErrorProvider(this.components);
            this.InputGroup.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
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
            this.ConsoleOutput.Size = new System.Drawing.Size(694, 445);
            this.ConsoleOutput.TabIndex = 0;
            // 
            // FirmwareImageName
            // 
            this.FirmwareImageName.FormattingEnabled = true;
            this.FirmwareImageName.Location = new System.Drawing.Point(96, 3);
            this.FirmwareImageName.Name = "FirmwareImageName";
            this.FirmwareImageName.Size = new System.Drawing.Size(229, 21);
            this.FirmwareImageName.TabIndex = 1;
            this.FirmwareImageName.SelectedIndexChanged += new System.EventHandler(this.FirmwareImageName_SelectedIndexChanged);
            // 
            // InputGroup
            // 
            this.InputGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputGroup.Controls.Add(this.flowLayoutPanel1);
            this.InputGroup.Location = new System.Drawing.Point(13, 13);
            this.InputGroup.Name = "InputGroup";
            this.InputGroup.Size = new System.Drawing.Size(699, 54);
            this.InputGroup.TabIndex = 2;
            this.InputGroup.TabStop = false;
            this.InputGroup.Text = "Firmware Selections";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.FirmwareImageName);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.ComPortName);
            this.flowLayoutPanel1.Controls.Add(this.UpdateCommand);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(693, 35);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Firmware Image";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(331, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "COM Port";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ComPortName
            // 
            this.ComPortName.FormattingEnabled = true;
            this.ComPortName.Location = new System.Drawing.Point(396, 3);
            this.ComPortName.Name = "ComPortName";
            this.ComPortName.Size = new System.Drawing.Size(190, 21);
            this.ComPortName.TabIndex = 4;
            this.ComPortName.SelectedIndexChanged += new System.EventHandler(this.ComPortName_SelectedIndexChanged);
            // 
            // UpdateCommand
            // 
            this.UpdateProcessError.SetIconPadding(this.UpdateCommand, 8);
            this.UpdateCommand.Location = new System.Drawing.Point(592, 3);
            this.UpdateCommand.Name = "UpdateCommand";
            this.UpdateCommand.Size = new System.Drawing.Size(79, 23);
            this.UpdateCommand.TabIndex = 5;
            this.UpdateCommand.Text = "Update";
            this.UpdateCommand.UseVisualStyleBackColor = true;
            // 
            // ConsoleGroup
            // 
            this.ConsoleGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsoleGroup.Controls.Add(this.ConsoleOutput);
            this.ConsoleGroup.Location = new System.Drawing.Point(12, 85);
            this.ConsoleGroup.Name = "ConsoleGroup";
            this.ConsoleGroup.Size = new System.Drawing.Size(700, 464);
            this.ConsoleGroup.TabIndex = 3;
            this.ConsoleGroup.TabStop = false;
            this.ConsoleGroup.Text = "Flash Programmer Output";
            // 
            // UpdateProcessError
            // 
            this.UpdateProcessError.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.UpdateProcessError.ContainerControl = this;
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
            this.Text = "FirmwareUpdate";
            this.Load += new System.EventHandler(this.FirmwareUpdate_Load);
            this.InputGroup.ResumeLayout(false);
            this.InputGroup.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ConsoleGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UpdateProcessError)).EndInit();
            this.ResumeLayout(false);

            }

        #endregion
        private ConsoleControl.ConsoleControl ConsoleOutput;
        private System.Windows.Forms.ComboBox FirmwareImageName;
        private System.Windows.Forms.GroupBox InputGroup;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComPortName;
        private System.Windows.Forms.Button UpdateCommand;
        private System.Windows.Forms.GroupBox ConsoleGroup;
        private System.Windows.Forms.ErrorProvider UpdateProcessError;
        }
    }