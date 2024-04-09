namespace Console
{
    partial class Form1
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            ButtonEnable = new Button();
            ButtonDisable = new Button();
            TextBoxMessageLog = new RichTextBox();
            splitContainer1 = new SplitContainer();
            numericUpDownTimeout = new NumericUpDown();
            lbSecondsTimeout = new Label();
            lbSecondsBlockTime = new Label();
            numericUpDownBlockTime = new NumericUpDown();
            lbBlockTime = new Label();
            lbTimeout = new Label();
            ButtonClear = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownBlockTime).BeginInit();
            SuspendLayout();
            // 
            // ButtonEnable
            // 
            ButtonEnable.Location = new Point(17, 12);
            ButtonEnable.Margin = new Padding(5, 6, 5, 6);
            ButtonEnable.Name = "ButtonEnable";
            ButtonEnable.Size = new Size(125, 44);
            ButtonEnable.TabIndex = 0;
            ButtonEnable.Text = "Enable";
            ButtonEnable.UseVisualStyleBackColor = true;
            ButtonEnable.Click += ButtonEnable_Click;
            // 
            // ButtonDisable
            // 
            ButtonDisable.Location = new Point(152, 12);
            ButtonDisable.Margin = new Padding(5, 6, 5, 6);
            ButtonDisable.Name = "ButtonDisable";
            ButtonDisable.Size = new Size(125, 44);
            ButtonDisable.TabIndex = 1;
            ButtonDisable.Text = "Disable";
            ButtonDisable.UseVisualStyleBackColor = true;
            ButtonDisable.Click += ButtonDisable_Click;
            // 
            // TextBoxMessageLog
            // 
            TextBoxMessageLog.BackColor = SystemColors.InfoText;
            TextBoxMessageLog.Dock = DockStyle.Fill;
            TextBoxMessageLog.Location = new Point(0, 0);
            TextBoxMessageLog.Margin = new Padding(5, 6, 5, 6);
            TextBoxMessageLog.Name = "TextBoxMessageLog";
            TextBoxMessageLog.Size = new Size(1654, 1011);
            TextBoxMessageLog.TabIndex = 2;
            TextBoxMessageLog.Text = "";
            TextBoxMessageLog.TextChanged += TextBoxMessageLog_TextChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.Fixed3D;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(5, 6, 5, 6);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.AutoScroll = true;
            splitContainer1.Panel1.Controls.Add(numericUpDownTimeout);
            splitContainer1.Panel1.Controls.Add(lbSecondsTimeout);
            splitContainer1.Panel1.Controls.Add(ButtonEnable);
            splitContainer1.Panel1.Controls.Add(lbSecondsBlockTime);
            splitContainer1.Panel1.Controls.Add(ButtonDisable);
            splitContainer1.Panel1.Controls.Add(numericUpDownBlockTime);
            splitContainer1.Panel1.Controls.Add(lbBlockTime);
            splitContainer1.Panel1.Controls.Add(lbTimeout);
            splitContainer1.Panel1.Controls.Add(ButtonClear);
            splitContainer1.Panel1MinSize = 90;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(TextBoxMessageLog);
            splitContainer1.Size = new Size(1658, 1113);
            splitContainer1.SplitterDistance = 90;
            splitContainer1.SplitterWidth = 8;
            splitContainer1.TabIndex = 3;
            // 
            // numericUpDownTimeout
            // 
            numericUpDownTimeout.Location = new Point(1025, 18);
            numericUpDownTimeout.Margin = new Padding(5, 6, 5, 6);
            numericUpDownTimeout.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            numericUpDownTimeout.Name = "numericUpDownTimeout";
            numericUpDownTimeout.Size = new Size(83, 31);
            numericUpDownTimeout.TabIndex = 3;
            numericUpDownTimeout.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // lbSecondsTimeout
            // 
            lbSecondsTimeout.AutoSize = true;
            lbSecondsTimeout.Font = new Font("Microsoft Sans Serif", 7F);
            lbSecondsTimeout.Location = new Point(1023, 59);
            lbSecondsTimeout.Margin = new Padding(5, 0, 5, 0);
            lbSecondsTimeout.Name = "lbSecondsTimeout";
            lbSecondsTimeout.Size = new Size(88, 17);
            lbSecondsTimeout.TabIndex = 6;
            lbSecondsTimeout.Text = "(In Seconds)";
            // 
            // lbSecondsBlockTime
            // 
            lbSecondsBlockTime.AutoSize = true;
            lbSecondsBlockTime.Font = new Font("Microsoft Sans Serif", 7F);
            lbSecondsBlockTime.Location = new Point(657, 59);
            lbSecondsBlockTime.Margin = new Padding(5, 0, 5, 0);
            lbSecondsBlockTime.Name = "lbSecondsBlockTime";
            lbSecondsBlockTime.Size = new Size(88, 17);
            lbSecondsBlockTime.TabIndex = 5;
            lbSecondsBlockTime.Text = "(In Seconds)";
            // 
            // numericUpDownBlockTime
            // 
            numericUpDownBlockTime.Location = new Point(659, 18);
            numericUpDownBlockTime.Margin = new Padding(5, 6, 5, 6);
            numericUpDownBlockTime.Name = "numericUpDownBlockTime";
            numericUpDownBlockTime.Size = new Size(83, 31);
            numericUpDownBlockTime.TabIndex = 2;
            numericUpDownBlockTime.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // lbBlockTime
            // 
            lbBlockTime.AutoSize = true;
            lbBlockTime.Location = new Point(480, 22);
            lbBlockTime.Margin = new Padding(5, 0, 5, 0);
            lbBlockTime.Name = "lbBlockTime";
            lbBlockTime.Size = new Size(172, 25);
            lbBlockTime.TabIndex = 4;
            lbBlockTime.Text = "Delegate Block Time";
            // 
            // lbTimeout
            // 
            lbTimeout.AutoSize = true;
            lbTimeout.Location = new Point(940, 22);
            lbTimeout.Margin = new Padding(5, 0, 5, 0);
            lbTimeout.Name = "lbTimeout";
            lbTimeout.Size = new Size(77, 25);
            lbTimeout.TabIndex = 4;
            lbTimeout.Text = "Timeout";
            // 
            // ButtonClear
            // 
            ButtonClear.Location = new Point(1503, 13);
            ButtonClear.Margin = new Padding(5, 6, 5, 6);
            ButtonClear.Name = "ButtonClear";
            ButtonClear.Size = new Size(125, 44);
            ButtonClear.TabIndex = 4;
            ButtonClear.Text = "Clear";
            ButtonClear.UseVisualStyleBackColor = true;
            ButtonClear.Click += ButtonClear_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1658, 1113);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(5, 6, 5, 6);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AsyncTask";
            Load += Form1_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDownTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownBlockTime).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button ButtonEnable;
        private System.Windows.Forms.Button ButtonDisable;
        private System.Windows.Forms.RichTextBox TextBoxMessageLog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button ButtonClear;
        private System.Windows.Forms.Label lbSecondsBlockTime;
        private System.Windows.Forms.Label lbTimeout;
        private System.Windows.Forms.NumericUpDown numericUpDownBlockTime;
        private System.Windows.Forms.Label lbSecondsTimeout;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeout;
        private System.Windows.Forms.Label lbBlockTime;
    }
}