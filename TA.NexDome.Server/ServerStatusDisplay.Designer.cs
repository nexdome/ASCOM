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
            this.annunciatorPanel1 = new ASCOM.Controls.AnnunciatorPanel();
            this.RotationTitle = new ASCOM.Controls.Annunciator();
            this.CounterClockwiseAnnunciator = new ASCOM.Controls.Annunciator();
            this.AzimuthMotorAnnunciator = new ASCOM.Controls.Annunciator();
            this.ClockwiseAnnunciator = new ASCOM.Controls.Annunciator();
            this.AzimuthPositionAnnunciator = new ASCOM.Controls.Annunciator();
            this.AtHomeAnnunciator = new ASCOM.Controls.Annunciator();
            this.UserPin1Annunciator = new ASCOM.Controls.Annunciator();
            this.UserPin2Annunciator = new ASCOM.Controls.Annunciator();
            this.UserPin3Annunciator = new ASCOM.Controls.Annunciator();
            this.UserPin4Annunciator = new ASCOM.Controls.Annunciator();
            this.ShutterTitle = new ASCOM.Controls.Annunciator();
            this.ShutterOpeningAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterMotorAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterClosingAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterCurrentAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterOpenAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterClosedAnnunciator = new ASCOM.Controls.Annunciator();
            this.ShutterIndeterminateAnnunciator = new ASCOM.Controls.Annunciator();
            this.SetupCommand = new System.Windows.Forms.Button();
            this.ShutterCurrentBar = new System.Windows.Forms.ProgressBar();
            this.OpenButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.annunciatorPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Registered clients:";
            // 
            // registeredClientCount
            // 
            this.registeredClientCount.AutoSize = true;
            this.registeredClientCount.Location = new System.Drawing.Point(193, 75);
            this.registeredClientCount.Name = "registeredClientCount";
            this.registeredClientCount.Size = new System.Drawing.Size(13, 13);
            this.registeredClientCount.TabIndex = 1;
            this.registeredClientCount.Text = "0";
            // 
            // OnlineClients
            // 
            this.OnlineClients.AutoSize = true;
            this.OnlineClients.Location = new System.Drawing.Point(291, 75);
            this.OnlineClients.Name = "OnlineClients";
            this.OnlineClients.Size = new System.Drawing.Size(13, 13);
            this.OnlineClients.TabIndex = 3;
            this.OnlineClients.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(245, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Online:";
            // 
            // annunciatorPanel1
            // 
            this.annunciatorPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annunciatorPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.annunciatorPanel1.Controls.Add(this.RotationTitle);
            this.annunciatorPanel1.Controls.Add(this.CounterClockwiseAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.AzimuthMotorAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.ClockwiseAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.AzimuthPositionAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.AtHomeAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.UserPin1Annunciator);
            this.annunciatorPanel1.Controls.Add(this.UserPin2Annunciator);
            this.annunciatorPanel1.Controls.Add(this.UserPin3Annunciator);
            this.annunciatorPanel1.Controls.Add(this.UserPin4Annunciator);
            this.annunciatorPanel1.Controls.Add(this.ShutterTitle);
            this.annunciatorPanel1.Controls.Add(this.ShutterOpeningAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.ShutterMotorAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.ShutterClosingAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.ShutterCurrentAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.ShutterOpenAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.ShutterClosedAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.ShutterIndeterminateAnnunciator);
            this.annunciatorPanel1.Location = new System.Drawing.Point(96, 12);
            this.annunciatorPanel1.Name = "annunciatorPanel1";
            this.annunciatorPanel1.Size = new System.Drawing.Size(387, 37);
            this.annunciatorPanel1.TabIndex = 5;
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
            this.RotationTitle.Text = "Azimuth";
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
            // UserPin1Annunciator
            // 
            this.UserPin1Annunciator.ActiveColor = System.Drawing.Color.DarkSeaGreen;
            this.UserPin1Annunciator.AutoSize = true;
            this.UserPin1Annunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.UserPin1Annunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.UserPin1Annunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.UserPin1Annunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.UserPin1Annunciator.Location = new System.Drawing.Point(263, 0);
            this.UserPin1Annunciator.Mute = false;
            this.UserPin1Annunciator.Name = "UserPin1Annunciator";
            this.UserPin1Annunciator.Size = new System.Drawing.Size(24, 17);
            this.UserPin1Annunciator.TabIndex = 16;
            this.UserPin1Annunciator.Text = " 1";
            // 
            // UserPin2Annunciator
            // 
            this.UserPin2Annunciator.ActiveColor = System.Drawing.Color.DarkSeaGreen;
            this.UserPin2Annunciator.AutoSize = true;
            this.UserPin2Annunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.UserPin2Annunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.UserPin2Annunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.UserPin2Annunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.UserPin2Annunciator.Location = new System.Drawing.Point(293, 0);
            this.UserPin2Annunciator.Mute = false;
            this.UserPin2Annunciator.Name = "UserPin2Annunciator";
            this.UserPin2Annunciator.Size = new System.Drawing.Size(24, 17);
            this.UserPin2Annunciator.TabIndex = 17;
            this.UserPin2Annunciator.Text = " 2";
            // 
            // UserPin3Annunciator
            // 
            this.UserPin3Annunciator.ActiveColor = System.Drawing.Color.DarkSeaGreen;
            this.UserPin3Annunciator.AutoSize = true;
            this.UserPin3Annunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.UserPin3Annunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.UserPin3Annunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.UserPin3Annunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.UserPin3Annunciator.Location = new System.Drawing.Point(323, 0);
            this.UserPin3Annunciator.Mute = false;
            this.UserPin3Annunciator.Name = "UserPin3Annunciator";
            this.UserPin3Annunciator.Size = new System.Drawing.Size(24, 17);
            this.UserPin3Annunciator.TabIndex = 17;
            this.UserPin3Annunciator.Text = " 3";
            // 
            // UserPin4Annunciator
            // 
            this.UserPin4Annunciator.ActiveColor = System.Drawing.Color.DarkSeaGreen;
            this.UserPin4Annunciator.AutoSize = true;
            this.UserPin4Annunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.UserPin4Annunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.UserPin4Annunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.UserPin4Annunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.UserPin4Annunciator.Location = new System.Drawing.Point(353, 0);
            this.UserPin4Annunciator.Mute = false;
            this.UserPin4Annunciator.Name = "UserPin4Annunciator";
            this.UserPin4Annunciator.Size = new System.Drawing.Size(24, 17);
            this.UserPin4Annunciator.TabIndex = 17;
            this.UserPin4Annunciator.Text = " 4";
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
            // ShutterCurrentAnnunciator
            // 
            this.ShutterCurrentAnnunciator.ActiveColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ShutterCurrentAnnunciator.AutoSize = true;
            this.ShutterCurrentAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterCurrentAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterCurrentAnnunciator.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ShutterCurrentAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterCurrentAnnunciator.Location = new System.Drawing.Point(171, 17);
            this.ShutterCurrentAnnunciator.Mute = false;
            this.ShutterCurrentAnnunciator.Name = "ShutterCurrentAnnunciator";
            this.ShutterCurrentAnnunciator.Size = new System.Drawing.Size(32, 17);
            this.ShutterCurrentAnnunciator.TabIndex = 10;
            this.ShutterCurrentAnnunciator.Tag = "{0:D3}";
            this.ShutterCurrentAnnunciator.Text = "000";
            // 
            // ShutterOpenAnnunciator
            // 
            this.ShutterOpenAnnunciator.ActiveColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ShutterOpenAnnunciator.AutoSize = true;
            this.ShutterOpenAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterOpenAnnunciator.Cadence = ASCOM.Controls.CadencePattern.Wink;
            this.ShutterOpenAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterOpenAnnunciator.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ShutterOpenAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterOpenAnnunciator.Location = new System.Drawing.Point(209, 17);
            this.ShutterOpenAnnunciator.Mute = false;
            this.ShutterOpenAnnunciator.Name = "ShutterOpenAnnunciator";
            this.ShutterOpenAnnunciator.Size = new System.Drawing.Size(40, 17);
            this.ShutterOpenAnnunciator.TabIndex = 11;
            this.ShutterOpenAnnunciator.Text = "Open";
            // 
            // ShutterClosedAnnunciator
            // 
            this.ShutterClosedAnnunciator.ActiveColor = System.Drawing.Color.DarkSeaGreen;
            this.ShutterClosedAnnunciator.AutoSize = true;
            this.ShutterClosedAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterClosedAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterClosedAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterClosedAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterClosedAnnunciator.Location = new System.Drawing.Point(255, 17);
            this.ShutterClosedAnnunciator.Mute = false;
            this.ShutterClosedAnnunciator.Name = "ShutterClosedAnnunciator";
            this.ShutterClosedAnnunciator.Size = new System.Drawing.Size(56, 17);
            this.ShutterClosedAnnunciator.TabIndex = 13;
            this.ShutterClosedAnnunciator.Text = "Closed";
            // 
            // ShutterIndeterminateAnnunciator
            // 
            this.ShutterIndeterminateAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterIndeterminateAnnunciator.AutoSize = true;
            this.ShutterIndeterminateAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterIndeterminateAnnunciator.Cadence = ASCOM.Controls.CadencePattern.BlinkAlarm;
            this.ShutterIndeterminateAnnunciator.Font = new System.Drawing.Font("Consolas", 10F);
            this.ShutterIndeterminateAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterIndeterminateAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ShutterIndeterminateAnnunciator.Location = new System.Drawing.Point(317, 17);
            this.ShutterIndeterminateAnnunciator.Mute = false;
            this.ShutterIndeterminateAnnunciator.Name = "ShutterIndeterminateAnnunciator";
            this.ShutterIndeterminateAnnunciator.Size = new System.Drawing.Size(64, 17);
            this.ShutterIndeterminateAnnunciator.TabIndex = 15;
            this.ShutterIndeterminateAnnunciator.Text = "Unknown";
            // 
            // SetupCommand
            // 
            this.SetupCommand.Location = new System.Drawing.Point(12, 70);
            this.SetupCommand.Name = "SetupCommand";
            this.SetupCommand.Size = new System.Drawing.Size(75, 23);
            this.SetupCommand.TabIndex = 8;
            this.SetupCommand.Text = "Setup...";
            this.SetupCommand.UseVisualStyleBackColor = true;
            // 
            // ShutterCurrentBar
            // 
            this.ShutterCurrentBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ShutterCurrentBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ShutterCurrentBar.Location = new System.Drawing.Point(210, 58);
            this.ShutterCurrentBar.Maximum = 20;
            this.ShutterCurrentBar.Name = "ShutterCurrentBar";
            this.ShutterCurrentBar.Size = new System.Drawing.Size(273, 10);
            this.ShutterCurrentBar.TabIndex = 11;
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
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Shutter Motor Current";
            // 
            // ServerStatusDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(499, 105);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OpenButton);
            this.Controls.Add(this.SetupCommand);
            this.Controls.Add(this.annunciatorPanel1);
            this.Controls.Add(this.OnlineClients);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.registeredClientCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ShutterCurrentBar);
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
            this.annunciatorPanel1.ResumeLayout(false);
            this.annunciatorPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label registeredClientCount;
        private System.Windows.Forms.Label OnlineClients;
        private System.Windows.Forms.Label label3;
        private ASCOM.Controls.AnnunciatorPanel annunciatorPanel1;
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
        private ASCOM.Controls.Annunciator ShutterCurrentAnnunciator;
        private System.Windows.Forms.ProgressBar ShutterCurrentBar;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button CloseButton;
        private ASCOM.Controls.Annunciator AtHomeAnnunciator;
        private ASCOM.Controls.Annunciator UserPin1Annunciator;
        private ASCOM.Controls.Annunciator UserPin2Annunciator;
        private ASCOM.Controls.Annunciator UserPin3Annunciator;
        private ASCOM.Controls.Annunciator UserPin4Annunciator;
        private ASCOM.Controls.Annunciator ShutterOpenAnnunciator;
        private ASCOM.Controls.Annunciator ShutterClosedAnnunciator;
        private ASCOM.Controls.Annunciator ShutterIndeterminateAnnunciator;
        private System.Windows.Forms.Label label2;
    }
}

