namespace modbus_coil_test
{
    partial class Form1
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
            txtIpAddress = new TextBox();
            btnConnect = new Button();
            btnStartAuto = new Button();
            btnManual960 = new Button();
            lblSensor6 = new Label();
            lblSensor7 = new Label();
            txtInterval = new TextBox();
            btnManual961 = new Button();
            txtCycleMin = new TextBox();
            btnStartCycle = new Button();
            lblCycleStatus = new Label();
            SuspendLayout();
            // 
            // txtIpAddress
            // 
            txtIpAddress.Location = new Point(85, 34);
            txtIpAddress.Name = "txtIpAddress";
            txtIpAddress.Size = new Size(180, 23);
            txtIpAddress.TabIndex = 0;
            txtIpAddress.TextChanged += textBox1_TextChanged;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(295, 32);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(75, 27);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "button1";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += button1_Click;
            // 
            // btnStartAuto
            // 
            btnStartAuto.Location = new Point(329, 168);
            btnStartAuto.Name = "btnStartAuto";
            btnStartAuto.Size = new Size(143, 32);
            btnStartAuto.TabIndex = 2;
            btnStartAuto.Text = "켜질 시간/mm";
            btnStartAuto.UseVisualStyleBackColor = true;
            btnStartAuto.Click += btnStartAuto_Click;
            // 
            // btnManual960
            // 
            btnManual960.Location = new Point(103, 283);
            btnManual960.Name = "btnManual960";
            btnManual960.Size = new Size(143, 32);
            btnManual960.TabIndex = 3;
            btnManual960.Text = "960";
            btnManual960.UseVisualStyleBackColor = true;
            btnManual960.Click += btnManual960_Click_1;
            // 
            // lblSensor6
            // 
            lblSensor6.AutoSize = true;
            lblSensor6.Location = new Point(103, 100);
            lblSensor6.Name = "lblSensor6";
            lblSensor6.Size = new Size(39, 15);
            lblSensor6.TabIndex = 4;
            lblSensor6.Text = "label1";
            // 
            // lblSensor7
            // 
            lblSensor7.AutoSize = true;
            lblSensor7.Location = new Point(237, 100);
            lblSensor7.Name = "lblSensor7";
            lblSensor7.Size = new Size(39, 15);
            lblSensor7.TabIndex = 5;
            lblSensor7.Text = "label2";
            // 
            // txtInterval
            // 
            txtInterval.Location = new Point(96, 177);
            txtInterval.Name = "txtInterval";
            txtInterval.Size = new Size(180, 23);
            txtInterval.TabIndex = 6;
            // 
            // btnManual961
            // 
            btnManual961.BackColor = Color.BlanchedAlmond;
            btnManual961.Location = new Point(329, 283);
            btnManual961.Name = "btnManual961";
            btnManual961.Size = new Size(143, 32);
            btnManual961.TabIndex = 7;
            btnManual961.Text = "961";
            btnManual961.UseVisualStyleBackColor = false;
            btnManual961.Click += btnManual961_Click;
            // 
            // txtCycleMin
            // 
            txtCycleMin.Location = new Point(97, 212);
            txtCycleMin.Name = "txtCycleMin";
            txtCycleMin.Size = new Size(179, 23);
            txtCycleMin.TabIndex = 8;
            // 
            // btnStartCycle
            // 
            btnStartCycle.Location = new Point(329, 209);
            btnStartCycle.Name = "btnStartCycle";
            btnStartCycle.Size = new Size(143, 32);
            btnStartCycle.TabIndex = 9;
            btnStartCycle.Text = "순환 타이머/분";
            btnStartCycle.UseVisualStyleBackColor = true;
            btnStartCycle.Click += btnStartCycle_Click;
            // 
            // lblCycleStatus
            // 
            lblCycleStatus.AutoSize = true;
            lblCycleStatus.Location = new Point(499, 220);
            lblCycleStatus.Name = "lblCycleStatus";
            lblCycleStatus.Size = new Size(39, 15);
            lblCycleStatus.TabIndex = 10;
            lblCycleStatus.Text = "label2";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblCycleStatus);
            Controls.Add(btnStartCycle);
            Controls.Add(txtCycleMin);
            Controls.Add(btnManual961);
            Controls.Add(txtInterval);
            Controls.Add(lblSensor7);
            Controls.Add(lblSensor6);
            Controls.Add(btnManual960);
            Controls.Add(btnStartAuto);
            Controls.Add(btnConnect);
            Controls.Add(txtIpAddress);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIpAddress;
        private Button btnConnect;
        private Button btnStartAuto;
        private Button btnManual960;
        private Label lblSensor6;
        private Label lblSensor7;
        private TextBox txtInterval;
        private Button btnManual961;
        private TextBox txtCycleMin;
        private Button btnStartCycle;
        private Label lblCycleStatus;
    }
}
