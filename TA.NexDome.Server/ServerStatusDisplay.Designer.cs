namespace TA.NexDome.Server
{
    partial class ServerStatusDisplay
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
            this.label1 = new System.Windows.Forms.Label();
            this.registeredClientCount = new System.Windows.Forms.Label();
            this.OnlineClients = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusAnnunciatorPanel = new ASCOM.Controls.AnnunciatorPanel();
            this.RotationTitle = new ASCOM.Controls.Annunciator();
            this.CounterClockwiseAnnunciator = new ASCOM.Controls.Annunciator();
            this.AzimuthMotorAnnunciator = new ASCOM.Controls.Annunciator();
            this.ClockwiseAnnunciator = new ASCOM.Controls.Annunciator();
            this.AzimuthPositionAnnunciator = new ASCOM.Controls.Annunciator();
            this.AtHomeAnnunciator = new ASCOM.Controls.Annunciator();
            this.RainAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterTitle = new ASCOM.Controls.Annunciator();
            this.ShutterOpeningAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterMotorAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterClosingAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterPercentOpenAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterDispositionAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterLinkStateAnnunciator = new ASCOM.Controls.Annunciator();
            this.batteryVoltsAnnunciator = new ASCOM.Controls.Annunciator();
            this.SetupCommand = new System.Windows.Forms.Button();
            this.ShutterPositionBar = new System.Windows.Forms.ProgressBar();
            this.OpenButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.batteryVoltsBar = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.StopCommand = new System.Windows.Forms.Button();
            this.statusAnnunciatorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Registered clients:";
            // 
            // registeredClientCount
            // 
            this.registeredClientCount.AutoSize = true;
            this.registeredClientCount.Location = new System.Drawing.Point(313, 103);
            this.registeredClientCount.Name = "registeredClientCount";
            this.registeredClientCount.Size = new System.Drawing.Size(13, 13);
            this.registeredClientCount.TabIndex = 1;
            this.registeredClientCount.Text = "0";
            // 
            // OnlineClients
            // 
            this.OnlineClients.AutoSize = true;
            this.OnlineClients.Location = new System.Drawing.Point(411, 103);
            this.OnlineClients.Name = "OnlineClients";
            this.OnlineClients.Size = new System.Drawing.Size(13, 13);
            this.OnlineClients.TabIndex = 3;
            this.OnlineClients.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(365, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Online:";
            // 
            // statusAnnunciatorPanel
            // 
            this.statusAnnunciatorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusAnnunciatorPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.statusAnnunciatorPanel.Controls.Add(this.RotationTitle);
            this.statusAnnunciatorPanel.Controls.Add(this.CounterClockwiseAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.AzimuthMotorAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.ClockwiseAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.AzimuthPositionAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.AtHomeAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.RainAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.ShutterTitle);
            this.statusAnnunciatorPanel.Controls.Add(this.ShutterOpeningAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.ShutterMotorAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.ShutterClosingAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.ShutterPercentOpenAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.ShutterDispositionAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.ShutterLinkStateAnnunciator);
            this.statusAnnunciatorPanel.Controls.Add(this.batteryVoltsAnnunciator);
            this.statusAnnunciatorPanel.Location = new System.Drawing.Point(96, 12);
            this.statusAnnunciatorPanel.Name = "statusAnnunciatorPanel";
            this.statusAnnunciatorPanel.Size = new System.Drawing.Size(420, 37);
            this.statusAnnunciatorPanel.TabIndex = 5;
            // 
            // RotationTitle
            // 
            this.RotationTitle.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.RotationTitle.AutoSize = true;
            this.RotationTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.RotationTitle.Font = new System.Drawing.Font("Consolas", 10F);
            this.RotationTitle.ForeColor = System.Drawing.Color.White;
            this.RotationTitle.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.RotationTitle.Location = new System.Drawing.Point(3, 0);
            this.RotationTitle.Mute = false;
            this.RotationTitle.Name = "RotationTitle";
            this.RotationTitle.Size = new System.Drawing.Size(64, 17);
            this.RotationTitle.TabIndex = 1;
            this.RotationTitle.Text = "Rotator";
            // 
            // CounterClockwiseAnnunciator
            // 
            this.CounterClockwiseAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.CounterClockwiseAnnunciator.AutoSize = true;
            this.CounterClockwiseAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CounterClockwiseAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.CounterClockwiseAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.CounterClockwiseAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.CounterClockwiseAnnunciator.Location = new System.Drawing.Point(73, 0);
            this.CounterClockwiseAnnunciator.Mute = false;
            this.CounterClockwiseAnnunciator.Name = "CounterClockwiseAnnunciator";
            this.CounterClockwiseAnnunciator.Size = new System.Drawing.Size(16, 17);
            this.CounterClockwiseAnnunciator.TabIndex = 2;
            this.CounterClockwiseAnnunciator.Text = "◄";
            // 
            // AzimuthMotorAnnunciator
            // 
            this.AzimuthMotorAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.AzimuthMotorAnnunciator.AutoSize = true;
            this.AzimuthMotorAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.AzimuthMotorAnnunciator.Cadence = ASCOM.Controls.CadencePattern.BlinkAlarm;
            this.AzimuthMotorAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.AzimuthMotorAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.AzimuthMotorAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.AzimuthMotorAnnunciator.Location = new System.Drawing.Point(95, 0);
            this.AzimuthMotorAnnunciator.Mute = false;
            this.AzimuthMotorAnnunciator.Name = "AzimuthMotorAnnunciator";
            this.AzimuthMotorAnnunciator.Size = new System.Drawing.Size(48, 17);
            this.AzimuthMotorAnnunciator.TabIndex = 3;
            this.AzimuthMotorAnnunciator.Text = "Motor";
            // 
            // ClockwiseAnnunciator
            // 
            this.ClockwiseAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ClockwiseAnnunciator.AutoSize = true;
            this.ClockwiseAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ClockwiseAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ClockwiseAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ClockwiseAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ClockwiseAnnunciator.Location = new System.Drawing.Point(149, 0);
            this.ClockwiseAnnunciator.Mute = false;
            this.ClockwiseAnnunciator.Name = "ClockwiseAnnunciator";
            this.ClockwiseAnnunciator.Size = new System.Drawing.Size(16, 17);
            this.ClockwiseAnnunciator.TabIndex = 4;
            this.ClockwiseAnnunciator.Text = "►";
            // 
            // AzimuthPositionAnnunciator
            // 
            this.AzimuthPositionAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.AzimuthPositionAnnunciator.AutoSize = true;
            this.AzimuthPositionAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.AzimuthPositionAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.AzimuthPositionAnnunciator.ForeColor = System.Drawing.Color.PaleGoldenrod;
            this.AzimuthPositionAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.AzimuthPositionAnnunciator.Location = new System.Drawing.Point(171, 0);
            this.AzimuthPositionAnnunciator.Mute = false;
            this.AzimuthPositionAnnunciator.Name = "AzimuthPositionAnnunciator";
            this.AzimuthPositionAnnunciator.Size = new System.Drawing.Size(40, 17);
            this.AzimuthPositionAnnunciator.TabIndex = 5;
            this.AzimuthPositionAnnunciator.Tag = "{0:D3}°";
            this.AzimuthPositionAnnunciator.Text = "000°";
            // 
            // AtHomeAnnunciator
            // 
            this.AtHomeAnnunciator.ActiveColor = System.Drawing.Color.DarkSeaGreen;
            this.AtHomeAnnunciator.AutoSize = true;
            this.AtHomeAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.AtHomeAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.AtHomeAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.AtHomeAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.AtHomeAnnunciator.Location = new System.Drawing.Point(217, 0);
            this.AtHomeAnnunciator.Mute = false;
            this.AtHomeAnnunciator.Name = "AtHomeAnnunciator";
            this.AtHomeAnnunciator.Size = new System.Drawing.Size(40, 17);
            this.AtHomeAnnunciator.TabIndex = 12;
            this.AtHomeAnnunciator.Text = "Home";
            // 
            // RainAnnunciator
            // 
            this.RainAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.RainAnnunciator.AutoSize = true;
            this.RainAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.RainAnnunciator.Cadence = ASCOM.Controls.CadencePattern.BlinkFast;
            this.statusAnnunciatorPanel.SetFlowBreak(this.RainAnnunciator, true);
            this.RainAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.RainAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.RainAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.RainAnnunciator.Location = new System.Drawing.Point(263, 0);
            this.RainAnnunciator.Mute = false;
            this.RainAnnunciator.Name = "RainAnnunciator";
            this.RainAnnunciator.Size = new System.Drawing.Size(40, 17);
            this.RainAnnunciator.TabIndex = 19;
            this.RainAnnunciator.Text = "Rain";
            // 
            // ShutterTitle
            // 
            this.ShutterTitle.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterTitle.AutoSize = true;
            this.ShutterTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterTitle.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterTitle.ForeColor = System.Drawing.Color.White;
            this.ShutterTitle.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterTitle.Location = new System.Drawing.Point(3, 17);
            this.ShutterTitle.Mute = false;
            this.ShutterTitle.Name = "ShutterTitle";
            this.ShutterTitle.Size = new System.Drawing.Size(64, 17);
            this.ShutterTitle.TabIndex = 6;
            this.ShutterTitle.Text = "Shutter";
            // 
            // ShutterOpeningAnnunciator
            // 
            this.ShutterOpeningAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterOpeningAnnunciator.AutoSize = true;
            this.ShutterOpeningAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterOpeningAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterOpeningAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterOpeningAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterOpeningAnnunciator.Location = new System.Drawing.Point(73, 17);
            this.ShutterOpeningAnnunciator.Mute = false;
            this.ShutterOpeningAnnunciator.Name = "ShutterOpeningAnnunciator";
            this.ShutterOpeningAnnunciator.Size = new System.Drawing.Size(16, 17);
            this.ShutterOpeningAnnunciator.TabIndex = 7;
            this.ShutterOpeningAnnunciator.Text = "▲";
            // 
            // ShutterMotorAnnunciator
            // 
            this.ShutterMotorAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterMotorAnnunciator.AutoSize = true;
            this.ShutterMotorAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterMotorAnnunciator.Cadence = ASCOM.Controls.CadencePattern.BlinkAlarm;
            this.ShutterMotorAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterMotorAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterMotorAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterMotorAnnunciator.Location = new System.Drawing.Point(95, 17);
            this.ShutterMotorAnnunciator.Mute = false;
            this.ShutterMotorAnnunciator.Name = "ShutterMotorAnnunciator";
            this.ShutterMotorAnnunciator.Size = new System.Drawing.Size(48, 17);
            this.ShutterMotorAnnunciator.TabIndex = 8;
            this.ShutterMotorAnnunciator.Text = "Motor";
            // 
            // ShutterClosingAnnunciator
            // 
            this.ShutterClosingAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterClosingAnnunciator.AutoSize = true;
            this.ShutterClosingAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterClosingAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterClosingAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterClosingAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterClosingAnnunciator.Location = new System.Drawing.Point(149, 17);
            this.ShutterClosingAnnunciator.Mute = false;
            this.ShutterClosingAnnunciator.Name = "ShutterClosingAnnunciator";
            this.ShutterClosingAnnunciator.Size = new System.Drawing.Size(16, 17);
            this.ShutterClosingAnnunciator.TabIndex = 9;
            this.ShutterClosingAnnunciator.Text = "▼";
            // 
            // ShutterPercentOpenAnnunciator
            // 
            this.ShutterPercentOpenAnnunciator.ActiveColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ShutterPercentOpenAnnunciator.AutoSize = true;
            this.ShutterPercentOpenAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterPercentOpenAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterPercentOpenAnnunciator.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ShutterPercentOpenAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterPercentOpenAnnunciator.Location = new System.Drawing.Point(171, 17);
            this.ShutterPercentOpenAnnunciator.Mute = false;
            this.ShutterPercentOpenAnnunciator.Name = "ShutterPercentOpenAnnunciator";
            this.ShutterPercentOpenAnnunciator.Size = new System.Drawing.Size(32, 17);
            this.ShutterPercentOpenAnnunciator.TabIndex = 10;
            this.ShutterPercentOpenAnnunciator.Tag = "{0:D3}%";
            this.ShutterPercentOpenAnnunciator.Text = "000";
            // 
            // ShutterDispositionAnnunciator
            // 
            this.ShutterDispositionAnnunciator.ActiveColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ShutterDispositionAnnunciator.AutoSize = true;
            this.ShutterDispositionAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterDispositionAnnunciator.Cadence = ASCOM.Controls.CadencePattern.Wink;
            this.ShutterDispositionAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterDispositionAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterDispositionAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterDispositionAnnunciator.Location = new System.Drawing.Point(209, 17);
            this.ShutterDispositionAnnunciator.Mute = false;
            this.ShutterDispositionAnnunciator.Name = "ShutterDispositionAnnunciator";
            this.ShutterDispositionAnnunciator.Size = new System.Drawing.Size(40, 17);
            this.ShutterDispositionAnnunciator.TabIndex = 11;
            this.ShutterDispositionAnnunciator.Text = "Open";
            // 
            // ShutterLinkStateAnnunciator
            // 
            this.ShutterLinkStateAnnunciator.ActiveColor = System.Drawing.Color.DarkSeaGreen;
            this.ShutterLinkStateAnnunciator.AutoSize = true;
            this.ShutterLinkStateAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterLinkStateAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterLinkStateAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterLinkStateAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterLinkStateAnnunciator.Location = new System.Drawing.Point(255, 17);
            this.ShutterLinkStateAnnunciator.Mute = false;
            this.ShutterLinkStateAnnunciator.Name = "ShutterLinkStateAnnunciator";
            this.ShutterLinkStateAnnunciator.Size = new System.Drawing.Size(56, 17);
            this.ShutterLinkStateAnnunciator.TabIndex = 13;
            this.ShutterLinkStateAnnunciator.Text = "Closed";
            // 
            // batteryVoltsAnnunciator
            // 
            this.batteryVoltsAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.batteryVoltsAnnunciator.AutoSize = true;
            this.batteryVoltsAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.batteryVoltsAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.batteryVoltsAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.batteryVoltsAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.batteryVoltsAnnunciator.Location = new System.Drawing.Point(317, 17);
            this.batteryVoltsAnnunciator.Mute = false;
            this.batteryVoltsAnnunciator.Name = "batteryVoltsAnnunciator";
            this.batteryVoltsAnnunciator.Size = new System.Drawing.Size(64, 17);
            this.batteryVoltsAnnunciator.TabIndex = 18;
            this.batteryVoltsAnnunciator.Tag = "{0:F2}V";
            this.batteryVoltsAnnunciator.Text = "15.00 V";
            // 
            // SetupCommand
            // 
            this.SetupCommand.Location = new System.Drawing.Point(12, 99);
            this.SetupCommand.Name = "SetupCommand";
            this.SetupCommand.Size = new System.Drawing.Size(75, 23);
            this.SetupCommand.TabIndex = 8;
            this.SetupCommand.Text = "Setup...";
            this.SetupCommand.UseVisualStyleBackColor = true;
            // 
            // ShutterPositionBar
            // 
            this.ShutterPositionBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ShutterPositionBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterPositionBar.ForeColor = System.Drawing.Color.IndianRed;
            this.ShutterPositionBar.Location = new System.Drawing.Point(180, 58);
            this.ShutterPositionBar.MarqueeAnimationSpeed = 0;
            this.ShutterPositionBar.Name = "ShutterPositionBar";
            this.ShutterPositionBar.Size = new System.Drawing.Size(336, 10);
            this.ShutterPositionBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ShutterPositionBar.TabIndex = 11;
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(12, 12);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(75, 23);
            this.OpenButton.TabIndex = 12;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(12, 41);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 13;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Shutter Position";
            // 
            // batteryVoltsBar
            // 
            this.batteryVoltsBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.batteryVoltsBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.batteryVoltsBar.Location = new System.Drawing.Point(180, 75);
            this.batteryVoltsBar.MarqueeAnimationSpeed = 0;
            this.batteryVoltsBar.Maximum = 150;
            this.batteryVoltsBar.Minimum = 100;
            this.batteryVoltsBar.Name = "batteryVoltsBar";
            this.batteryVoltsBar.Size = new System.Drawing.Size(336, 10);
            this.batteryVoltsBar.Step = 1;
            this.batteryVoltsBar.TabIndex = 15;
            this.batteryVoltsBar.Value = 125;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(93, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Battery Volts";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(177, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "10V";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(496, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "15V";
            // 
            // StopCommand
            // 
            this.StopCommand.Location = new System.Drawing.Point(12, 70);
            this.StopCommand.Name = "StopCommand";
            this.StopCommand.Size = new System.Drawing.Size(75, 23);
            this.StopCommand.TabIndex = 19;
            this.StopCommand.Text = "Stop";
            this.StopCommand.UseVisualStyleBackColor = true;
            // 
            // ServerStatusDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(532, 129);
            this.Controls.Add(this.StopCommand);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.batteryVoltsBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OpenButton);
            this.Controls.Add(this.SetupCommand);
            this.Controls.Add(this.statusAnnunciatorPanel);
            this.Controls.Add(this.OnlineClients);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.registeredClientCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ShutterPositionBar);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::TA.NexDome.Server.Properties.Settings.Default, "MainFormLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Location = global::TA.NexDome.Server.Properties.Settings.Default.MainFormLocation;
            this.Name = "ServerStatusDisplay";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "NexDome ASCOM Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.LocationChanged += new System.EventHandler(this.frmMain_LocationChanged);
            this.VisibleChanged += new System.EventHandler(this.ServerStatusDisplay_VisibleChanged);
            this.statusAnnunciatorPanel.ResumeLayout(false);
            this.statusAnnunciatorPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label registeredClientCount;
        private System.Windows.Forms.Label OnlineClients;
        private System.Windows.Forms.Label label3;
        private ASCOM.Controls.AnnunciatorPanel statusAnnunciatorPanel;
        private ASCOM.Controls.Annunciator RotationTitle;
        private System.Windows.Forms.Button SetupCommand;
        private ASCOM.Controls.Annunciator CounterClockwiseAnnunciator;
        private ASCOM.Controls.Annunciator AzimuthMotorAnnunciator;
        private ASCOM.Controls.Annunciator ClockwiseAnnunciator;
        private ASCOM.Controls.Annunciator AzimuthPositionAnnunciator;
        private ASCOM.Controls.Annunciator ShutterTitle;
        private ASCOM.Controls.Annunciator ShutterOpeningAnnunciator;
        private ASCOM.Controls.Annunciator ShutterMotorAnnunciator;
        private ASCOM.Controls.Annunciator ShutterClosingAnnunciator;
        private ASCOM.Controls.Annunciator ShutterPercentOpenAnnunciator;
        private System.Windows.Forms.ProgressBar ShutterPositionBar;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button CloseButton;
        private ASCOM.Controls.Annunciator AtHomeAnnunciator;
        private ASCOM.Controls.Annunciator ShutterDispositionAnnunciator;
        private System.Windows.Forms.Label label2;
        private ASCOM.Controls.Annunciator ShutterLinkStateAnnunciator;
        private ASCOM.Controls.Annunciator batteryVoltsAnnunciator;
        private System.Windows.Forms.ProgressBar batteryVoltsBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button StopCommand;
        private ASCOM.Controls.Annunciator RainAnnunciator;
        }
}

