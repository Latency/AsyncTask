using System.Windows.Forms;
using Telerik.WinControls.UI;


namespace GUI.WinForms {
  sealed partial class RadForm1 : RadForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }


    // ReSharper disable once InconsistentNaming
    private const int CS_DROPSHADOW = 0x00020000;

    /// <summary>
    ///  Draw a shadow around form
    /// </summary>
    protected override CreateParams CreateParams
    {
      get
      {
        var cp = base.CreateParams;
        cp.ClassStyle |= CS_DROPSHADOW;
        return cp;
      }
    }


    #region Windows UIContext Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.radPanel_Bottom = new Telerik.WinControls.UI.RadPanel();
      this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
      this.statusBar = new Telerik.WinControls.UI.RadLabelElement();
      this.statusControl = new Telerik.WinControls.UI.RadLabelElement();
      ((System.ComponentModel.ISupportInitialize)(this.radPanel_Bottom)).BeginInit();
      this.radPanel_Bottom.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      this.SuspendLayout();
      // 
      // radPanel_Bottom
      // 
      this.radPanel_Bottom.Controls.Add(this.radStatusStrip1);
      this.radPanel_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.radPanel_Bottom.Location = new System.Drawing.Point(0, 631);
      this.radPanel_Bottom.Name = "radPanel_Bottom";
      this.radPanel_Bottom.Size = new System.Drawing.Size(1249, 28);
      this.radPanel_Bottom.TabIndex = 9;
      this.radPanel_Bottom.TabStop = false;
      // 
      // radStatusStrip1
      // 
      this.radStatusStrip1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.statusBar,
            this.statusControl});
      this.radStatusStrip1.Location = new System.Drawing.Point(0, 2);
      this.radStatusStrip1.Name = "radStatusStrip1";
      this.radStatusStrip1.Size = new System.Drawing.Size(1249, 26);
      this.radStatusStrip1.TabIndex = 0;
      this.radStatusStrip1.TabStop = false;
      this.radStatusStrip1.Text = "radStatusStrip1";
      // 
      // statusBar
      // 
      this.statusBar.AccessibleDescription = "statusBar";
      this.statusBar.AccessibleName = "statusBar";
      this.statusBar.Alignment = System.Drawing.ContentAlignment.TopLeft;
      this.statusBar.AutoSize = true;
      this.statusBar.BorderVisible = false;
      this.statusBar.FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentContent;
      this.statusBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      this.statusBar.Name = "statusBar";
      this.radStatusStrip1.SetSpring(this.statusBar, true);
      this.statusBar.Text = "";
      this.statusBar.TextWrap = true;
      // 
      // statusControl
      // 
      this.statusControl.AccessibleDescription = "statusControl";
      this.statusControl.AccessibleName = "statusControl";
      this.statusControl.Alignment = System.Drawing.ContentAlignment.TopRight;
      this.statusControl.AutoSize = false;
      this.statusControl.Bounds = new System.Drawing.Rectangle(0, 0, 130, 18);
      this.statusControl.Name = "statusControl";
      this.radStatusStrip1.SetSpring(this.statusControl, false);
      this.statusControl.Text = "";
      this.statusControl.TextWrap = true;

      // 
      // RadForm1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(1249, 659);
      this.Controls.Add(this.radPanel_Bottom);
      this.MinimumSize = new System.Drawing.Size(600, 480);
      this.Name = "RadForm1";
      // 
      // 
      // 
      this.RootElement.ApplyShapeToControl = true;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "ORM-Monitor";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RadForm1_FormClosed);
      this.Resize += new System.EventHandler(this.RadForm1_Resize);
      ((System.ComponentModel.ISupportInitialize)(this.radPanel_Bottom)).EndInit();
      this.radPanel_Bottom.ResumeLayout(false);
      this.radPanel_Bottom.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private RadPanel radPanel_Bottom;
    private RadStatusStrip radStatusStrip1;
    private RadLabelElement statusBar;
    private RadLabelElement statusControl;

    internal RadLabelElement StatusBar { get { return statusBar; } set { statusBar = value; } }

    internal RadLabelElement StatusControl { get { return statusControl; } set { statusControl = value; } }
  }
}