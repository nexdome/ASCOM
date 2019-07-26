namespace TA.NexDome.Server
{
    partial class SetupDialogForm
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
            System.Windows.Forms.ToolTip toolTip1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.ShutterOpenCloseTimeSeconds = new System.Windows.Forms.NumericUpDown();
            this.FullRotationTimeSeconds = new System.Windows.Forms.NumericUpDown();
            this.PresetHD6 = new System.Windows.Forms.Button();
            this.PresetHD10 = new System.Windows.Forms.Button();
            this.PresetHD15 = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.HomeAzimuthUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.ParkAzimuth = new System.Windows.Forms.NumericUpDown();
            this.RotatorMaximumSpeedTrackBar = new System.Windows.Forms.TrackBar();
            this.RotatorRampTimeTrackBar = new System.Windows.Forms.TrackBar();
            this.ShutterAccelerationRampTimeTrackBar = new System.Windows.Forms.TrackBar();
            this.ShutterMaximumSpeedTrackBar = new System.Windows.Forms.TrackBar();
            this.communicationSettingsControl1 = new TA.NexDome.Server.CommunicationSettingsControl();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.AboutBox = new System.Windows.Forms.Button();
            this.ConnectionErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.CommunicationsGroup = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.RotatorParametersGroup = new System.Windows.Forms.GroupBox();
            this.RotatorRampTimeCurrentValue = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.RotatorSpeedCurrentValue = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ShutterParametersGroup = new System.Windows.Forms.GroupBox();
            this.ShutterRampTimeCurrentValue = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.ShutterMaximumSpeedCurrentValue = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.settingsWarningLabel = new System.Windows.Forms.Label();
            this.FirmwareUpdateCommand = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ShutterOpenCloseTimeSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullRotationTimeSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HomeAzimuthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParkAzimuth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotatorMaximumSpeedTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotatorRampTimeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShutterAccelerationRampTimeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShutterMaximumSpeedTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).BeginInit();
            this.CommunicationsGroup.SuspendLayout();
            this.RotatorParametersGroup.SuspendLayout();
            this.ShutterParametersGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 30000;
            toolTip1.InitialDelay = 200;
            toolTip1.ReshowDelay = 100;
            toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            toolTip1.ToolTipTitle = "Settings Help";
            // 
            // ShutterOpenCloseTimeSeconds
            // 
            this.ShutterOpenCloseTimeSeconds.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::TA.NexDome.Server.Properties.Settings.Default, "ShutterOpenCloseTimeSeconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ShutterOpenCloseTimeSeconds.Location = new System.Drawing.Point(175, 45);
            this.ShutterOpenCloseTimeSeconds.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.ShutterOpenCloseTimeSeconds.Name = "ShutterOpenCloseTimeSeconds";
            this.ShutterOpenCloseTimeSeconds.Size = new System.Drawing.Size(80, 20);
            this.ShutterOpenCloseTimeSeconds.TabIndex = 1;
            this.ShutterOpenCloseTimeSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            toolTip1.SetToolTip(this.ShutterOpenCloseTimeSeconds, "THe maximum time allowed for the shutter to fully open or close.");
            this.ShutterOpenCloseTimeSeconds.Value = global::TA.NexDome.Server.Properties.Settings.Default.ShutterOpenCloseTimeSeconds;
            // 
            // FullRotationTimeSeconds
            // 
            this.FullRotationTimeSeconds.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::TA.NexDome.Server.Properties.Settings.Default, "FullRotationTimeSeconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FullRotationTimeSeconds.Location = new System.Drawing.Point(175, 19);
            this.FullRotationTimeSeconds.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.FullRotationTimeSeconds.Name = "FullRotationTimeSeconds";
            this.FullRotationTimeSeconds.Size = new System.Drawing.Size(80, 20);
            this.FullRotationTimeSeconds.TabIndex = 1;
            this.FullRotationTimeSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            toolTip1.SetToolTip(this.FullRotationTimeSeconds, "The maximum time that the dome should be allowed to rotate without stopping.");
            this.FullRotationTimeSeconds.Value = global::TA.NexDome.Server.Properties.Settings.Default.FullRotationTimeSeconds;
            // 
            // PresetHD6
            // 
            this.PresetHD6.Location = new System.Drawing.Point(61, 71);
            this.PresetHD6.Name = "PresetHD6";
            this.PresetHD6.Size = new System.Drawing.Size(66, 23);
            this.PresetHD6.TabIndex = 2;
            this.PresetHD6.Text = "2m Dome";
            toolTip1.SetToolTip(this.PresetHD6, "Load default timeouts for a 2m/6ft dome");
            this.PresetHD6.UseVisualStyleBackColor = true;
            this.PresetHD6.Click += new System.EventHandler(this.PresetHD6_Click);
            // 
            // PresetHD10
            // 
            this.PresetHD10.Location = new System.Drawing.Point(133, 71);
            this.PresetHD10.Name = "PresetHD10";
            this.PresetHD10.Size = new System.Drawing.Size(66, 23);
            this.PresetHD10.TabIndex = 2;
            this.PresetHD10.Text = "3m Dome";
            toolTip1.SetToolTip(this.PresetHD10, "Load default timeouts for a 3m/10ft dome");
            this.PresetHD10.UseVisualStyleBackColor = true;
            this.PresetHD10.Click += new System.EventHandler(this.PresetHD10_Click);
            // 
            // PresetHD15
            // 
            this.PresetHD15.Location = new System.Drawing.Point(205, 71);
            this.PresetHD15.Name = "PresetHD15";
            this.PresetHD15.Size = new System.Drawing.Size(66, 23);
            this.PresetHD15.TabIndex = 2;
            this.PresetHD15.Text = "5m Dome";
            toolTip1.SetToolTip(this.PresetHD15, "Load default timeouts for a 5m/15ft dome");
            this.PresetHD15.UseVisualStyleBackColor = true;
            this.PresetHD15.Click += new System.EventHandler(this.PresetHD15_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::TA.NexDome.Server.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(402, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(69, 82);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Tag = global::TA.NexDome.Server.Properties.Settings.Default.AscomLogoWebDestination;
            toolTip1.SetToolTip(this.picASCOM, "Click to visit the ASCOM Standards web site");
            this.picASCOM.Click += new System.EventHandler(this.BrowseToWebPage);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToWebPage);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PresetHD15);
            this.groupBox1.Controls.Add(this.PresetHD10);
            this.groupBox1.Controls.Add(this.PresetHD6);
            this.groupBox1.Controls.Add(this.FullRotationTimeSeconds);
            this.groupBox1.Controls.Add(this.ShutterOpenCloseTimeSeconds);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 265);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 111);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Safety Timeouts";
            toolTip1.SetToolTip(this.groupBox1, "Dome safety timeouts");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Presets";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Shutter open/close (seconds)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Full rotation (seconds)";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::TA.NexDome.Server.Properties.Resources.TiGra_Astronomy_Icon_256x256;
            this.pictureBox1.Location = new System.Drawing.Point(402, 97);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(69, 82);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = global::TA.NexDome.Server.Properties.Settings.Default.TigraLogoWebDestination;
            toolTip1.SetToolTip(this.pictureBox1, "Visit the Tigra Astronomy web site");
            this.pictureBox1.Click += new System.EventHandler(this.BrowseToWebPage);
            this.pictureBox1.DoubleClick += new System.EventHandler(this.BrowseToWebPage);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Image = global::TA.NexDome.Server.Properties.Resources.NexDome;
            this.pictureBox2.Location = new System.Drawing.Point(12, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(370, 167);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Tag = global::TA.NexDome.Server.Properties.Settings.Default.NexDomeWebDestination;
            toolTip1.SetToolTip(this.pictureBox2, "Click to visit the NexDome web site");
            this.pictureBox2.Click += new System.EventHandler(this.BrowseToWebPage);
            this.pictureBox2.DoubleClick += new System.EventHandler(this.BrowseToWebPage);
            // 
            // HomeAzimuthUpDown
            // 
            this.HomeAzimuthUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::TA.NexDome.Server.Properties.Settings.Default, "HomeSensorAzimuth", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.HomeAzimuthUpDown.Location = new System.Drawing.Point(88, 18);
            this.HomeAzimuthUpDown.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.HomeAzimuthUpDown.Name = "HomeAzimuthUpDown";
            this.HomeAzimuthUpDown.Size = new System.Drawing.Size(53, 20);
            this.HomeAzimuthUpDown.TabIndex = 17;
            toolTip1.SetToolTip(this.HomeAzimuthUpDown, "Home Azimuth is defined as the distance\r\n(in degrees) starting from True North, t" +
        "hat\r\nthe dome must be rotated clockwise to\r\nreach the home sensor.");
            this.HomeAzimuthUpDown.Value = global::TA.NexDome.Server.Properties.Settings.Default.HomeSensorAzimuth;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(230, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Park Azimuth";
            toolTip1.SetToolTip(this.label5, "The Park Azimuth should be set so that\r\nit brings your shutter charging contacts\r" +
        "\ninto alignment with the charger.");
            this.label5.Click += new System.EventHandler(this.Label5_Click);
            // 
            // ParkAzimuth
            // 
            this.ParkAzimuth.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::TA.NexDome.Server.Properties.Settings.Default, "ParkAzimuth", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ParkAzimuth.Location = new System.Drawing.Point(311, 18);
            this.ParkAzimuth.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.ParkAzimuth.Name = "ParkAzimuth";
            this.ParkAzimuth.Size = new System.Drawing.Size(53, 20);
            this.ParkAzimuth.TabIndex = 19;
            toolTip1.SetToolTip(this.ParkAzimuth, "Home Azimuth is defined as the distance\r\n(in degrees) starting from True North, t" +
        "hat\r\nthe dome must be rotated clockwise to\r\nreach the home sensor.");
            this.ParkAzimuth.Value = global::TA.NexDome.Server.Properties.Settings.Default.ParkAzimuth;
            this.ParkAzimuth.ValueChanged += new System.EventHandler(this.NumericUpDown1_ValueChanged);
            // 
            // RotatorMaximumSpeedTrackBar
            // 
            this.RotatorMaximumSpeedTrackBar.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::TA.NexDome.Server.Properties.Settings.Default, "RotatorMaximumSpeed", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RotatorMaximumSpeedTrackBar.LargeChange = 50;
            this.RotatorMaximumSpeedTrackBar.Location = new System.Drawing.Point(10, 52);
            this.RotatorMaximumSpeedTrackBar.Maximum = 1000;
            this.RotatorMaximumSpeedTrackBar.Minimum = 200;
            this.RotatorMaximumSpeedTrackBar.Name = "RotatorMaximumSpeedTrackBar";
            this.RotatorMaximumSpeedTrackBar.Size = new System.Drawing.Size(289, 45);
            this.RotatorMaximumSpeedTrackBar.SmallChange = 10;
            this.RotatorMaximumSpeedTrackBar.TabIndex = 23;
            this.RotatorMaximumSpeedTrackBar.TickFrequency = 100;
            this.RotatorMaximumSpeedTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            toolTip1.SetToolTip(this.RotatorMaximumSpeedTrackBar, "Motor maximum speed, in whole steps per second.\r\nDefault=600");
            this.RotatorMaximumSpeedTrackBar.Value = global::TA.NexDome.Server.Properties.Settings.Default.RotatorMaximumSpeed;
            this.RotatorMaximumSpeedTrackBar.Scroll += new System.EventHandler(this.RotatorMaximumSpeedTrackBar_Scroll);
            // 
            // RotatorRampTimeTrackBar
            // 
            this.RotatorRampTimeTrackBar.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::TA.NexDome.Server.Properties.Settings.Default, "RotatorRampTimeMilliseconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.RotatorRampTimeTrackBar.LargeChange = 500;
            this.RotatorRampTimeTrackBar.Location = new System.Drawing.Point(10, 103);
            this.RotatorRampTimeTrackBar.Maximum = 5000;
            this.RotatorRampTimeTrackBar.Minimum = 1000;
            this.RotatorRampTimeTrackBar.Name = "RotatorRampTimeTrackBar";
            this.RotatorRampTimeTrackBar.Size = new System.Drawing.Size(289, 45);
            this.RotatorRampTimeTrackBar.SmallChange = 100;
            this.RotatorRampTimeTrackBar.TabIndex = 28;
            this.RotatorRampTimeTrackBar.TickFrequency = 500;
            this.RotatorRampTimeTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            toolTip1.SetToolTip(this.RotatorRampTimeTrackBar, "Acceleration ramp time in milliseconds\r\n1000 ms = 1 sec\r\nDefault = 1500 ms (1.5s)" +
        "");
            this.RotatorRampTimeTrackBar.Value = global::TA.NexDome.Server.Properties.Settings.Default.RotatorRampTimeMilliseconds;
            this.RotatorRampTimeTrackBar.Scroll += new System.EventHandler(this.RotatorRampTimeTrackBar_Scroll);
            // 
            // ShutterAccelerationRampTimeTrackBar
            // 
            this.ShutterAccelerationRampTimeTrackBar.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::TA.NexDome.Server.Properties.Settings.Default, "ShutterAccelerationRampTimeMilliseconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ShutterAccelerationRampTimeTrackBar.LargeChange = 500;
            this.ShutterAccelerationRampTimeTrackBar.Location = new System.Drawing.Point(6, 70);
            this.ShutterAccelerationRampTimeTrackBar.Maximum = 5000;
            this.ShutterAccelerationRampTimeTrackBar.Minimum = 1000;
            this.ShutterAccelerationRampTimeTrackBar.Name = "ShutterAccelerationRampTimeTrackBar";
            this.ShutterAccelerationRampTimeTrackBar.Size = new System.Drawing.Size(289, 45);
            this.ShutterAccelerationRampTimeTrackBar.SmallChange = 100;
            this.ShutterAccelerationRampTimeTrackBar.TabIndex = 38;
            this.ShutterAccelerationRampTimeTrackBar.TickFrequency = 500;
            this.ShutterAccelerationRampTimeTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            toolTip1.SetToolTip(this.ShutterAccelerationRampTimeTrackBar, "Acceleration ramp time in milliseconds\r\n1000 ms = 1 sec\r\nDefault = 1500 ms (1.5s)" +
        "");
            this.ShutterAccelerationRampTimeTrackBar.Value = global::TA.NexDome.Server.Properties.Settings.Default.ShutterAccelerationRampTimeMilliseconds;
            this.ShutterAccelerationRampTimeTrackBar.Scroll += new System.EventHandler(this.ShutterAccelerationRampTimeTrackBar_Scroll);
            // 
            // ShutterMaximumSpeedTrackBar
            // 
            this.ShutterMaximumSpeedTrackBar.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::TA.NexDome.Server.Properties.Settings.Default, "ShutterMaximumSpeed", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ShutterMaximumSpeedTrackBar.LargeChange = 50;
            this.ShutterMaximumSpeedTrackBar.Location = new System.Drawing.Point(6, 19);
            this.ShutterMaximumSpeedTrackBar.Maximum = 1000;
            this.ShutterMaximumSpeedTrackBar.Minimum = 200;
            this.ShutterMaximumSpeedTrackBar.Name = "ShutterMaximumSpeedTrackBar";
            this.ShutterMaximumSpeedTrackBar.Size = new System.Drawing.Size(289, 45);
            this.ShutterMaximumSpeedTrackBar.SmallChange = 10;
            this.ShutterMaximumSpeedTrackBar.TabIndex = 33;
            this.ShutterMaximumSpeedTrackBar.TickFrequency = 100;
            this.ShutterMaximumSpeedTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            toolTip1.SetToolTip(this.ShutterMaximumSpeedTrackBar, "Motor maximum speed, in whole steps per second.\r\nDefault=600");
            this.ShutterMaximumSpeedTrackBar.Value = global::TA.NexDome.Server.Properties.Settings.Default.ShutterMaximumSpeed;
            this.ShutterMaximumSpeedTrackBar.Scroll += new System.EventHandler(this.ShutterMaximumSpeedTrackBar_Scroll);
            // 
            // communicationSettingsControl1
            // 
            this.communicationSettingsControl1.AutoSize = true;
            this.communicationSettingsControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.communicationSettingsControl1.Location = new System.Drawing.Point(6, 19);
            this.communicationSettingsControl1.Name = "communicationSettingsControl1";
            this.communicationSettingsControl1.Size = new System.Drawing.Size(354, 36);
            this.communicationSettingsControl1.TabIndex = 7;
            toolTip1.SetToolTip(this.communicationSettingsControl1, resources.GetString("communicationSettingsControl1.ToolTip"));
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(412, 308);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(412, 338);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // AboutBox
            // 
            this.AboutBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AboutBox.Location = new System.Drawing.Point(412, 236);
            this.AboutBox.Name = "AboutBox";
            this.AboutBox.Size = new System.Drawing.Size(59, 23);
            this.AboutBox.TabIndex = 8;
            this.AboutBox.Text = "About...";
            this.AboutBox.UseVisualStyleBackColor = true;
            this.AboutBox.Click += new System.EventHandler(this.AboutBox_Click);
            // 
            // ConnectionErrorProvider
            // 
            this.ConnectionErrorProvider.BlinkRate = 1000;
            this.ConnectionErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.ConnectionErrorProvider.ContainerControl = this;
            // 
            // CommunicationsGroup
            // 
            this.CommunicationsGroup.Controls.Add(this.communicationSettingsControl1);
            this.CommunicationsGroup.Location = new System.Drawing.Point(12, 185);
            this.CommunicationsGroup.Name = "CommunicationsGroup";
            this.CommunicationsGroup.Size = new System.Drawing.Size(370, 74);
            this.CommunicationsGroup.TabIndex = 9;
            this.CommunicationsGroup.TabStop = false;
            this.CommunicationsGroup.Text = "Communications";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Home Azimuth";
            // 
            // RotatorParametersGroup
            // 
            this.RotatorParametersGroup.Controls.Add(this.RotatorRampTimeCurrentValue);
            this.RotatorParametersGroup.Controls.Add(this.label10);
            this.RotatorParametersGroup.Controls.Add(this.label11);
            this.RotatorParametersGroup.Controls.Add(this.label12);
            this.RotatorParametersGroup.Controls.Add(this.RotatorRampTimeTrackBar);
            this.RotatorParametersGroup.Controls.Add(this.RotatorSpeedCurrentValue);
            this.RotatorParametersGroup.Controls.Add(this.label8);
            this.RotatorParametersGroup.Controls.Add(this.label7);
            this.RotatorParametersGroup.Controls.Add(this.label6);
            this.RotatorParametersGroup.Controls.Add(this.RotatorMaximumSpeedTrackBar);
            this.RotatorParametersGroup.Controls.Add(this.label5);
            this.RotatorParametersGroup.Controls.Add(this.ParkAzimuth);
            this.RotatorParametersGroup.Controls.Add(this.label4);
            this.RotatorParametersGroup.Controls.Add(this.HomeAzimuthUpDown);
            this.RotatorParametersGroup.Location = new System.Drawing.Point(12, 382);
            this.RotatorParametersGroup.Name = "RotatorParametersGroup";
            this.RotatorParametersGroup.Size = new System.Drawing.Size(370, 163);
            this.RotatorParametersGroup.TabIndex = 14;
            this.RotatorParametersGroup.TabStop = false;
            this.RotatorParametersGroup.Text = "Rotator Parameters";
            // 
            // RotatorRampTimeCurrentValue
            // 
            this.RotatorRampTimeCurrentValue.AutoSize = true;
            this.RotatorRampTimeCurrentValue.Location = new System.Drawing.Point(329, 116);
            this.RotatorRampTimeCurrentValue.Name = "RotatorRampTimeCurrentValue";
            this.RotatorRampTimeCurrentValue.Size = new System.Drawing.Size(31, 13);
            this.RotatorRampTimeCurrentValue.TabIndex = 32;
            this.RotatorRampTimeCurrentValue.Text = "1000";
            this.RotatorRampTimeCurrentValue.Click += new System.EventHandler(this.Label9_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(268, 135);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "5000";
            this.label10.Click += new System.EventHandler(this.Label10_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 135);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "1000";
            this.label11.Click += new System.EventHandler(this.Label11_Click);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(10, 135);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(289, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "Acceleration Ramp Time (ms)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label12.Click += new System.EventHandler(this.Label12_Click);
            // 
            // RotatorSpeedCurrentValue
            // 
            this.RotatorSpeedCurrentValue.AutoSize = true;
            this.RotatorSpeedCurrentValue.Location = new System.Drawing.Point(329, 65);
            this.RotatorSpeedCurrentValue.Name = "RotatorSpeedCurrentValue";
            this.RotatorSpeedCurrentValue.Size = new System.Drawing.Size(31, 13);
            this.RotatorSpeedCurrentValue.TabIndex = 27;
            this.RotatorSpeedCurrentValue.Text = "1000";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(268, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "1000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "200";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(289, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Rotator Maximum Speed (steps/sec)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ShutterParametersGroup
            // 
            this.ShutterParametersGroup.Controls.Add(this.ShutterRampTimeCurrentValue);
            this.ShutterParametersGroup.Controls.Add(this.label13);
            this.ShutterParametersGroup.Controls.Add(this.label14);
            this.ShutterParametersGroup.Controls.Add(this.label15);
            this.ShutterParametersGroup.Controls.Add(this.ShutterAccelerationRampTimeTrackBar);
            this.ShutterParametersGroup.Controls.Add(this.ShutterMaximumSpeedCurrentValue);
            this.ShutterParametersGroup.Controls.Add(this.label17);
            this.ShutterParametersGroup.Controls.Add(this.label18);
            this.ShutterParametersGroup.Controls.Add(this.label19);
            this.ShutterParametersGroup.Controls.Add(this.ShutterMaximumSpeedTrackBar);
            this.ShutterParametersGroup.Location = new System.Drawing.Point(12, 552);
            this.ShutterParametersGroup.Name = "ShutterParametersGroup";
            this.ShutterParametersGroup.Size = new System.Drawing.Size(370, 132);
            this.ShutterParametersGroup.TabIndex = 15;
            this.ShutterParametersGroup.TabStop = false;
            this.ShutterParametersGroup.Text = "Shutter Parameters";
            // 
            // ShutterRampTimeCurrentValue
            // 
            this.ShutterRampTimeCurrentValue.AutoSize = true;
            this.ShutterRampTimeCurrentValue.Location = new System.Drawing.Point(325, 83);
            this.ShutterRampTimeCurrentValue.Name = "ShutterRampTimeCurrentValue";
            this.ShutterRampTimeCurrentValue.Size = new System.Drawing.Size(31, 13);
            this.ShutterRampTimeCurrentValue.TabIndex = 42;
            this.ShutterRampTimeCurrentValue.Text = "1000";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(264, 102);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 13);
            this.label13.TabIndex = 41;
            this.label13.Text = "5000";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 102);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(31, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "1000";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(6, 102);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(289, 13);
            this.label15.TabIndex = 39;
            this.label15.Text = "Acceleration Ramp Time (ms)";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ShutterMaximumSpeedCurrentValue
            // 
            this.ShutterMaximumSpeedCurrentValue.AutoSize = true;
            this.ShutterMaximumSpeedCurrentValue.Location = new System.Drawing.Point(325, 32);
            this.ShutterMaximumSpeedCurrentValue.Name = "ShutterMaximumSpeedCurrentValue";
            this.ShutterMaximumSpeedCurrentValue.Size = new System.Drawing.Size(31, 13);
            this.ShutterMaximumSpeedCurrentValue.TabIndex = 37;
            this.ShutterMaximumSpeedCurrentValue.Text = "1000";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(264, 51);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(31, 13);
            this.label17.TabIndex = 36;
            this.label17.Text = "1000";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 51);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(25, 13);
            this.label18.TabIndex = 35;
            this.label18.Text = "200";
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(6, 51);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(289, 13);
            this.label19.TabIndex = 34;
            this.label19.Text = "Rotator Maximum Speed (steps/sec)";
            this.label19.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // settingsWarningLabel
            // 
            this.settingsWarningLabel.AutoSize = true;
            this.settingsWarningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsWarningLabel.ForeColor = System.Drawing.Color.IndianRed;
            this.settingsWarningLabel.Location = new System.Drawing.Point(13, 691);
            this.settingsWarningLabel.Name = "settingsWarningLabel";
            this.settingsWarningLabel.Size = new System.Drawing.Size(382, 24);
            this.settingsWarningLabel.TabIndex = 16;
            this.settingsWarningLabel.Text = "Settings take effect after all clients disconnect";
            // 
            // FirmwareUpdateCommand
            // 
            this.FirmwareUpdateCommand.Location = new System.Drawing.Point(402, 392);
            this.FirmwareUpdateCommand.Name = "FirmwareUpdateCommand";
            this.FirmwareUpdateCommand.Size = new System.Drawing.Size(75, 23);
            this.FirmwareUpdateCommand.TabIndex = 17;
            this.FirmwareUpdateCommand.Text = "Update";
            this.FirmwareUpdateCommand.UseVisualStyleBackColor = true;
            this.FirmwareUpdateCommand.Click += new System.EventHandler(this.FirmwareUpdateCommand_Click);
            // 
            // SetupDialogForm
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(483, 734);
            this.Controls.Add(this.FirmwareUpdateCommand);
            this.Controls.Add(this.settingsWarningLabel);
            this.Controls.Add(this.ShutterParametersGroup);
            this.Controls.Add(this.RotatorParametersGroup);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CommunicationsGroup);
            this.Controls.Add(this.AboutBox);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::TA.NexDome.Server.Properties.Settings.Default, "SetupDialogLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = global::TA.NexDome.Server.Properties.Settings.Default.SetupDialogLocation;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(395, 213);
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure NexDome ASCOM Server";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ShutterOpenCloseTimeSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FullRotationTimeSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HomeAzimuthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ParkAzimuth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotatorMaximumSpeedTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotatorRampTimeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShutterAccelerationRampTimeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShutterMaximumSpeedTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConnectionErrorProvider)).EndInit();
            this.CommunicationsGroup.ResumeLayout(false);
            this.CommunicationsGroup.PerformLayout();
            this.RotatorParametersGroup.ResumeLayout(false);
            this.RotatorParametersGroup.PerformLayout();
            this.ShutterParametersGroup.ResumeLayout(false);
            this.ShutterParametersGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private CommunicationSettingsControl communicationSettingsControl1;
        private System.Windows.Forms.Button AboutBox;
        private System.Windows.Forms.ErrorProvider ConnectionErrorProvider;
        private System.Windows.Forms.GroupBox CommunicationsGroup;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown ShutterOpenCloseTimeSeconds;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button PresetHD15;
        private System.Windows.Forms.Button PresetHD10;
        private System.Windows.Forms.Button PresetHD6;
        private System.Windows.Forms.NumericUpDown FullRotationTimeSeconds;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox RotatorParametersGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown ParkAzimuth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown HomeAzimuthUpDown;
        private System.Windows.Forms.TrackBar RotatorMaximumSpeedTrackBar;
        private System.Windows.Forms.Label RotatorSpeedCurrentValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label RotatorRampTimeCurrentValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TrackBar RotatorRampTimeTrackBar;
        private System.Windows.Forms.GroupBox ShutterParametersGroup;
        private System.Windows.Forms.Label ShutterRampTimeCurrentValue;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TrackBar ShutterAccelerationRampTimeTrackBar;
        private System.Windows.Forms.Label ShutterMaximumSpeedCurrentValue;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TrackBar ShutterMaximumSpeedTrackBar;
        private System.Windows.Forms.Label settingsWarningLabel;
        private System.Windows.Forms.Button FirmwareUpdateCommand;
        }
}