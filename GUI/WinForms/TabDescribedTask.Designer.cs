using System.Windows.Forms;


namespace GUI.WinForms {
  sealed partial class TabDescribedTask : UserControl {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabDescribedTask));
      this.groupBox9 = new System.Windows.Forms.GroupBox();
      this.StartButton = new Telerik.WinControls.UI.RadButton();
      this.StopButton = new Telerik.WinControls.UI.RadButton();
      this.ClearButton = new Telerik.WinControls.UI.RadButton();
      this.checkBoxIncomplete = new System.Windows.Forms.CheckBox();
      this.checkBoxHighPriority = new System.Windows.Forms.CheckBox();
      this.textBoxFilter = new System.Windows.Forms.TextBox();
      this.imageListTasks = new System.Windows.Forms.ImageList(this.components);
      this.olvTasks = new BrightIdeasSoftware.ObjectListView();
      this.olvColumnTask = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
      this.olvColumnPriority = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
      this.olvColumnProgress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
      this.olvColumnStatus = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
      this.olvColumnAction = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
      this.olvColumnDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
      this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
      this.radCollapsiblePanel1 = new Telerik.WinControls.UI.RadCollapsiblePanel();
      this.imageListButton = new System.Windows.Forms.ImageList(this.components);
      this.groupBox9.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.StartButton)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.StopButton)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ClearButton)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.olvTasks)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).BeginInit();
      this.radCollapsiblePanel1.PanelContainer.SuspendLayout();
      this.radCollapsiblePanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox9
      // 
      this.groupBox9.AutoSize = true;
      this.groupBox9.Controls.Add(this.StartButton);
      this.groupBox9.Controls.Add(this.StopButton);
      this.groupBox9.Controls.Add(this.ClearButton);
      this.groupBox9.Controls.Add(this.checkBoxIncomplete);
      this.groupBox9.Controls.Add(this.checkBoxHighPriority);
      this.groupBox9.Controls.Add(this.textBoxFilter);
      this.groupBox9.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox9.Location = new System.Drawing.Point(5, 5);
      this.groupBox9.Name = "groupBox9";
      this.groupBox9.Size = new System.Drawing.Size(137, 569);
      this.groupBox9.TabIndex = 40;
      this.groupBox9.TabStop = false;
      this.groupBox9.Text = "Filter";
      // 
      // StartButton
      // 
      this.StartButton.Location = new System.Drawing.Point(7, 110);
      this.StartButton.MaximumSize = new System.Drawing.Size(110, 24);
      this.StartButton.MinimumSize = new System.Drawing.Size(110, 24);
      this.StartButton.Name = "StartButton";
      // 
      // 
      // 
      this.StartButton.RootElement.MaxSize = new System.Drawing.Size(110, 24);
      this.StartButton.RootElement.MinSize = new System.Drawing.Size(110, 24);
      this.StartButton.Size = new System.Drawing.Size(110, 24);
      this.StartButton.TabIndex = 4;
      this.StartButton.Text = "Start New TaskName";
      this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
      // 
      // StopButton
      // 
      this.StopButton.Location = new System.Drawing.Point(7, 157);
      this.StopButton.MaximumSize = new System.Drawing.Size(110, 24);
      this.StopButton.MinimumSize = new System.Drawing.Size(110, 24);
      this.StopButton.Name = "StopButton";
      // 
      // 
      // 
      this.StopButton.RootElement.MaxSize = new System.Drawing.Size(110, 24);
      this.StopButton.RootElement.MinSize = new System.Drawing.Size(110, 24);
      this.StopButton.Size = new System.Drawing.Size(110, 24);
      this.StopButton.TabIndex = 5;
      this.StopButton.Text = "Stop All Tasks";
      this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
      // 
      // ClearButton
      // 
      this.ClearButton.Location = new System.Drawing.Point(7, 204);
      this.ClearButton.MaximumSize = new System.Drawing.Size(110, 24);
      this.ClearButton.MinimumSize = new System.Drawing.Size(110, 24);
      this.ClearButton.Name = "ClearButton";
      // 
      // 
      // 
      this.ClearButton.RootElement.MaxSize = new System.Drawing.Size(110, 24);
      this.ClearButton.RootElement.MinSize = new System.Drawing.Size(110, 24);
      this.ClearButton.Size = new System.Drawing.Size(110, 24);
      this.ClearButton.TabIndex = 6;
      this.ClearButton.Text = "Clear All Tasks";
      this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
      // 
      // checkBoxIncomplete
      // 
      this.checkBoxIncomplete.AutoSize = true;
      this.checkBoxIncomplete.Location = new System.Drawing.Point(7, 76);
      this.checkBoxIncomplete.Name = "checkBoxIncomplete";
      this.checkBoxIncomplete.Size = new System.Drawing.Size(110, 17);
      this.checkBoxIncomplete.TabIndex = 3;
      this.checkBoxIncomplete.Text = "Incomplete Only";
      this.checkBoxIncomplete.UseVisualStyleBackColor = true;
      this.checkBoxIncomplete.CheckedChanged += new System.EventHandler(this.checkBoxIncomplete_CheckedChanged);
      // 
      // checkBoxHighPriority
      // 
      this.checkBoxHighPriority.AutoSize = true;
      this.checkBoxHighPriority.Location = new System.Drawing.Point(7, 52);
      this.checkBoxHighPriority.Name = "checkBoxHighPriority";
      this.checkBoxHighPriority.Size = new System.Drawing.Size(117, 17);
      this.checkBoxHighPriority.TabIndex = 2;
      this.checkBoxHighPriority.Text = "&High Priority Only";
      this.checkBoxHighPriority.UseVisualStyleBackColor = true;
      this.checkBoxHighPriority.CheckedChanged += new System.EventHandler(this.checkBoxHighPriority_CheckedChanged);
      // 
      // textBoxFilter
      // 
      this.textBoxFilter.Location = new System.Drawing.Point(7, 20);
      this.textBoxFilter.Name = "textBoxFilter";
      this.textBoxFilter.Size = new System.Drawing.Size(100, 20);
      this.textBoxFilter.TabIndex = 1;
      this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
      // 
      // imageListTasks
      // 
      this.imageListTasks.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTasks.ImageStream")));
      this.imageListTasks.TransparentColor = System.Drawing.Color.Fuchsia;
      this.imageListTasks.Images.SetKeyName(0, "download");
      this.imageListTasks.Images.SetKeyName(1, "backandforth");
      this.imageListTasks.Images.SetKeyName(2, "faq");
      this.imageListTasks.Images.SetKeyName(3, "windows");
      this.imageListTasks.Images.SetKeyName(4, "filter");
      this.imageListTasks.Images.SetKeyName(5, "printer");
      this.imageListTasks.Images.SetKeyName(6, "electronics");
      this.imageListTasks.Images.SetKeyName(7, "film");
      // 
      // olvTasks
      // 
      this.olvTasks.AllColumns.Add(this.olvColumnTask);
      this.olvTasks.AllColumns.Add(this.olvColumnPriority);
      this.olvTasks.AllColumns.Add(this.olvColumnProgress);
      this.olvTasks.AllColumns.Add(this.olvColumnStatus);
      this.olvTasks.AllColumns.Add(this.olvColumnAction);
      this.olvTasks.AllColumns.Add(this.olvColumnDate);
      this.olvTasks.AllowColumnReorder = true;
      this.olvTasks.AllowDrop = true;
      this.olvTasks.CellEditUseWholeCell = false;
      this.olvTasks.CheckBoxes = true;
      this.olvTasks.CheckedAspectName = "";
      this.olvTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnTask,
            this.olvColumnPriority,
            this.olvColumnProgress,
            this.olvColumnStatus,
            this.olvColumnAction,
            this.olvColumnDate});
      this.olvTasks.Cursor = System.Windows.Forms.Cursors.Default;
      this.olvTasks.EmptyListMsg = "No tasks match the filter";
      this.olvTasks.FullRowSelect = true;
      this.olvTasks.HeaderWordWrap = true;
      this.olvTasks.HideSelection = false;
      this.olvTasks.IncludeColumnHeadersInCopy = true;
      this.olvTasks.Location = new System.Drawing.Point(175, 0);
      this.olvTasks.Name = "olvTasks";
      this.olvTasks.OverlayImage.InsetX = 0;
      this.olvTasks.OverlayImage.InsetY = 0;
      this.olvTasks.OverlayImage.Transparency = 255;
      this.olvTasks.OverlayText.Alignment = System.Drawing.ContentAlignment.BottomLeft;
      this.olvTasks.OverlayText.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
      this.olvTasks.OverlayText.BorderWidth = 2F;
      this.olvTasks.OverlayText.Rotation = -20;
      this.olvTasks.OverlayText.Text = "";
      this.olvTasks.RowHeight = 54;
      this.olvTasks.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
      this.olvTasks.SelectedBackColor = System.Drawing.Color.Silver;
      this.olvTasks.SelectedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
      this.olvTasks.ShowCommandMenuOnRightClick = true;
      this.olvTasks.ShowGroups = false;
      this.olvTasks.ShowHeaderInAllViews = false;
      this.olvTasks.ShowItemToolTips = true;
      this.olvTasks.Size = new System.Drawing.Size(1066, 578);
      this.olvTasks.SmallImageList = this.imageListSmall;
      this.olvTasks.SortGroupItemsByPrimaryColumn = false;
      this.olvTasks.TabIndex = 36;
      this.olvTasks.TabStop = false;
      this.olvTasks.TriStateCheckBoxes = true;
      this.olvTasks.UseCellFormatEvents = true;
      this.olvTasks.UseCompatibleStateImageBehavior = false;
      this.olvTasks.UseFilterIndicator = true;
      this.olvTasks.UseFiltering = true;
      this.olvTasks.UseHotItem = true;
      this.olvTasks.UseTranslucentHotItem = true;
      this.olvTasks.UseTranslucentSelection = true;
      this.olvTasks.View = System.Windows.Forms.View.Details;
      // 
      // olvColumnTask
      // 
      this.olvColumnTask.AspectName = "TaskName";
      this.olvColumnTask.CellPadding = new System.Drawing.Rectangle(4, 2, 4, 2);
      this.olvColumnTask.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.olvColumnTask.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnTask.ImageAspectName = "ImageName";
      this.olvColumnTask.MinimumWidth = 40;
      this.olvColumnTask.Text = "TaskName";
      this.olvColumnTask.ToolTipText = "";
      this.olvColumnTask.Width = 400;
      // 
      // olvColumnPriority
      // 
      this.olvColumnPriority.AspectName = "Priority";
      this.olvColumnPriority.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.olvColumnPriority.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnPriority.Text = "Priority";
      this.olvColumnPriority.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnPriority.Width = 110;
      // 
      // olvColumnProgress
      // 
      this.olvColumnProgress.AspectName = "Progress";
      this.olvColumnProgress.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.olvColumnProgress.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnProgress.Text = "Progress";
      this.olvColumnProgress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnProgress.Width = 150;
      // 
      // olvColumnStatus
      // 
      this.olvColumnStatus.AspectName = "Status";
      this.olvColumnStatus.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.olvColumnStatus.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnStatus.IsTileViewColumn = true;
      this.olvColumnStatus.MinimumWidth = 30;
      this.olvColumnStatus.Text = "Status";
      this.olvColumnStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnStatus.ToolTipText = "";
      this.olvColumnStatus.Width = 140;
      // 
      // olvColumnAction
      // 
      this.olvColumnAction.AspectName = "Action";
      this.olvColumnAction.ButtonSize = new System.Drawing.Size(80, 26);
      this.olvColumnAction.ButtonSizing = BrightIdeasSoftware.OLVColumn.ButtonSizingMode.FixedBounds;
      this.olvColumnAction.CellEditUseWholeCell = true;
      this.olvColumnAction.EnableButtonWhenItemIsDisabled = true;
      this.olvColumnAction.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.olvColumnAction.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnAction.ImageAspectName = "";
      this.olvColumnAction.IsButton = true;
      this.olvColumnAction.Text = "Action";
      this.olvColumnAction.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnAction.Width = 90;
      // 
      // olvColumnDate
      // 
      this.olvColumnDate.AspectName = "Date";
      this.olvColumnDate.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.olvColumnDate.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnDate.ImageAspectName = "Date";
      this.olvColumnDate.Text = "Date";
      this.olvColumnDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.olvColumnDate.Width = 160;
      // 
      // imageListSmall
      // 
      this.imageListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmall.ImageStream")));
      this.imageListSmall.TransparentColor = System.Drawing.Color.Fuchsia;
      this.imageListSmall.Images.SetKeyName(0, "Add");
      this.imageListSmall.Images.SetKeyName(1, "Cancel");
      this.imageListSmall.Images.SetKeyName(2, "Heart");
      this.imageListSmall.Images.SetKeyName(3, "Tick");
      this.imageListSmall.Images.SetKeyName(4, "Coin");
      this.imageListSmall.Images.SetKeyName(5, "Lamp");
      // 
      // radCollapsiblePanel1
      // 
      this.radCollapsiblePanel1.BackColor = System.Drawing.Color.Transparent;
      this.radCollapsiblePanel1.Dock = System.Windows.Forms.DockStyle.Left;
      this.radCollapsiblePanel1.ExpandDirection = Telerik.WinControls.UI.RadDirection.Right;
      this.radCollapsiblePanel1.HorizontalHeaderAlignment = Telerik.WinControls.UI.RadHorizontalAlignment.Stretch;
      this.radCollapsiblePanel1.Location = new System.Drawing.Point(0, 0);
      this.radCollapsiblePanel1.Name = "radCollapsiblePanel1";
      this.radCollapsiblePanel1.OwnerBoundsCache = new System.Drawing.Rectangle(0, 0, 175, 581);
      // 
      // radCollapsiblePanel1.PanelContainer
      // 
      this.radCollapsiblePanel1.PanelContainer.AutoScroll = false;
      this.radCollapsiblePanel1.PanelContainer.Controls.Add(this.groupBox9);
      this.radCollapsiblePanel1.PanelContainer.Padding = new System.Windows.Forms.Padding(5);
      this.radCollapsiblePanel1.PanelContainer.Size = new System.Drawing.Size(147, 579);
      this.radCollapsiblePanel1.Size = new System.Drawing.Size(175, 581);
      this.radCollapsiblePanel1.TabIndex = 41;
      this.radCollapsiblePanel1.TabStop = false;
      this.radCollapsiblePanel1.Text = "radCollapsiblePanel1";
      this.radCollapsiblePanel1.VerticalHeaderAlignment = Telerik.WinControls.UI.RadVerticalAlignment.Stretch;
      this.radCollapsiblePanel1.Expanded += new System.EventHandler(this.radCollapsiblePanel1_Expanded);
      this.radCollapsiblePanel1.Collapsing += new System.ComponentModel.CancelEventHandler(this.radCollapsiblePanel1_Collapsing);
      // 
      // imageListButton
      // 
      this.imageListButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListButton.ImageStream")));
      this.imageListButton.TransparentColor = System.Drawing.Color.Fuchsia;
      this.imageListButton.Images.SetKeyName(0, "Hover");
      this.imageListButton.Images.SetKeyName(1, "Normal");
      this.imageListButton.Images.SetKeyName(2, "Pressed");
      // 
      // TabDescribedTask
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.Controls.Add(this.radCollapsiblePanel1);
      this.Controls.Add(this.olvTasks);
      this.Name = "TabDescribedTask";
      this.Size = new System.Drawing.Size(1244, 581);
      this.groupBox9.ResumeLayout(false);
      this.groupBox9.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.StartButton)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.StopButton)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ClearButton)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.olvTasks)).EndInit();
      this.radCollapsiblePanel1.PanelContainer.ResumeLayout(false);
      this.radCollapsiblePanel1.PanelContainer.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.radCollapsiblePanel1)).EndInit();
      this.radCollapsiblePanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private GroupBox                                   groupBox9;
    private TextBox                                    textBoxFilter;
    public  BrightIdeasSoftware.ObjectListView         olvTasks;
    private ImageList                                  imageListTasks,
                                                       imageListButton,
                                                       imageListSmall;
    private CheckBox                                   checkBoxIncomplete,
                                                       checkBoxHighPriority;
    private BrightIdeasSoftware.OLVColumn              olvColumnPriority,
                                                       olvColumnTask,
                                                       olvColumnStatus,
                                                       olvColumnProgress,
                                                       olvColumnAction,
                                                       olvColumnDate;
    private Telerik.WinControls.UI.RadCollapsiblePanel radCollapsiblePanel1;
    private Telerik.WinControls.UI.RadButton StartButton;
    private Telerik.WinControls.UI.RadButton StopButton;
    private Telerik.WinControls.UI.RadButton ClearButton;
  }
}
