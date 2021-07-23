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
            this.ButtonEnable = new System.Windows.Forms.Button();
            this.ButtonDisable = new System.Windows.Forms.Button();
            this.TextBoxMessageLog = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.numericUpDownTimeout = new System.Windows.Forms.NumericUpDown();
            this.lbSecondsTimeout = new System.Windows.Forms.Label();
            this.numericUpDownBlockTime = new System.Windows.Forms.NumericUpDown();
            this.lbSecondsBlockTime = new System.Windows.Forms.Label();
            this.lbTimeout = new System.Windows.Forms.Label();
            this.ButtonClear = new System.Windows.Forms.Button();
            this.lbBlockTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlockTime)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonEnable
            // 
            this.ButtonEnable.Location = new System.Drawing.Point(24, 28);
            this.ButtonEnable.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.ButtonEnable.Name = "ButtonEnable";
            this.ButtonEnable.Size = new System.Drawing.Size(150, 53);
            this.ButtonEnable.TabIndex = 0;
            this.ButtonEnable.Text = "Enable";
            this.ButtonEnable.UseVisualStyleBackColor = true;
            this.ButtonEnable.Click += new System.EventHandler(this.ButtonEnable_Click);
            // 
            // ButtonDisable
            // 
            this.ButtonDisable.Location = new System.Drawing.Point(182, 23);
            this.ButtonDisable.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.ButtonDisable.Name = "ButtonDisable";
            this.ButtonDisable.Size = new System.Drawing.Size(150, 53);
            this.ButtonDisable.TabIndex = 1;
            this.ButtonDisable.Text = "Disable";
            this.ButtonDisable.UseVisualStyleBackColor = true;
            this.ButtonDisable.Click += new System.EventHandler(this.ButtonDisable_Click);
            // 
            // TextBoxMessageLog
            // 
            this.TextBoxMessageLog.BackColor = System.Drawing.SystemColors.InfoText;
            this.TextBoxMessageLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBoxMessageLog.Location = new System.Drawing.Point(0, 0);
            this.TextBoxMessageLog.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.TextBoxMessageLog.Name = "TextBoxMessageLog";
            this.TextBoxMessageLog.Size = new System.Drawing.Size(1986, 1263);
            this.TextBoxMessageLog.TabIndex = 2;
            this.TextBoxMessageLog.Text = "";
            this.TextBoxMessageLog.TextChanged += new System.EventHandler(this.TextBoxMessageLog_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.numericUpDownTimeout);
            this.splitContainer1.Panel1.Controls.Add(this.lbSecondsTimeout);
            this.splitContainer1.Panel1.Controls.Add(this.numericUpDownBlockTime);
            this.splitContainer1.Panel1.Controls.Add(this.lbSecondsBlockTime);
            this.splitContainer1.Panel1.Controls.Add(this.lbTimeout);
            this.splitContainer1.Panel1.Controls.Add(this.ButtonClear);
            this.splitContainer1.Panel1.Controls.Add(this.ButtonDisable);
            this.splitContainer1.Panel1.Controls.Add(this.lbBlockTime);
            this.splitContainer1.Panel1MinSize = 60;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.TextBoxMessageLog);
            this.splitContainer1.Size = new System.Drawing.Size(1990, 1336);
            this.splitContainer1.SplitterDistance = 60;
            this.splitContainer1.SplitterWidth = 9;
            this.splitContainer1.TabIndex = 3;
            // 
            // numericUpDownTimeout
            // 
            this.numericUpDownTimeout.Location = new System.Drawing.Point(1370, 30);
            this.numericUpDownTimeout.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.numericUpDownTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDownTimeout.Name = "numericUpDownTimeout";
            this.numericUpDownTimeout.Size = new System.Drawing.Size(100, 35);
            this.numericUpDownTimeout.TabIndex = 7;
            this.numericUpDownTimeout.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lbSecondsTimeout
            // 
            this.lbSecondsTimeout.AutoSize = true;
            this.lbSecondsTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbSecondsTimeout.Location = new System.Drawing.Point(1246, 72);
            this.lbSecondsTimeout.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbSecondsTimeout.Name = "lbSecondsTimeout";
            this.lbSecondsTimeout.Size = new System.Drawing.Size(104, 20);
            this.lbSecondsTimeout.TabIndex = 8;
            this.lbSecondsTimeout.Text = "(In Seconds)";
            // 
            // numericUpDownBlockTime
            // 
            this.numericUpDownBlockTime.Location = new System.Drawing.Point(980, 30);
            this.numericUpDownBlockTime.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.numericUpDownBlockTime.Name = "numericUpDownBlockTime";
            this.numericUpDownBlockTime.Size = new System.Drawing.Size(100, 35);
            this.numericUpDownBlockTime.TabIndex = 6;
            this.numericUpDownBlockTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lbSecondsBlockTime
            // 
            this.lbSecondsBlockTime.AutoSize = true;
            this.lbSecondsBlockTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbSecondsBlockTime.Location = new System.Drawing.Point(806, 72);
            this.lbSecondsBlockTime.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbSecondsBlockTime.Name = "lbSecondsBlockTime";
            this.lbSecondsBlockTime.Size = new System.Drawing.Size(104, 20);
            this.lbSecondsBlockTime.TabIndex = 5;
            this.lbSecondsBlockTime.Text = "(In Seconds)";
            // 
            // lbTimeout
            // 
            this.lbTimeout.AutoSize = true;
            this.lbTimeout.Location = new System.Drawing.Point(1268, 35);
            this.lbTimeout.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbTimeout.Name = "lbTimeout";
            this.lbTimeout.Size = new System.Drawing.Size(89, 30);
            this.lbTimeout.TabIndex = 4;
            this.lbTimeout.Text = "Timeout";
            // 
            // ButtonClear
            // 
            this.ButtonClear.Location = new System.Drawing.Point(1812, 23);
            this.ButtonClear.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.ButtonClear.Name = "ButtonClear";
            this.ButtonClear.Size = new System.Drawing.Size(150, 53);
            this.ButtonClear.TabIndex = 2;
            this.ButtonClear.Text = "Clear";
            this.ButtonClear.UseVisualStyleBackColor = true;
            this.ButtonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            // 
            // lbBlockTime
            // 
            this.lbBlockTime.AutoSize = true;
            this.lbBlockTime.Location = new System.Drawing.Point(766, 35);
            this.lbBlockTime.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbBlockTime.Name = "lbBlockTime";
            this.lbBlockTime.Size = new System.Drawing.Size(202, 30);
            this.lbBlockTime.TabIndex = 4;
            this.lbBlockTime.Text = "Delegate Block Time";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1990, 1336);
            this.Controls.Add(this.ButtonEnable);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AsyncTask";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlockTime)).EndInit();
            this.ResumeLayout(false);

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