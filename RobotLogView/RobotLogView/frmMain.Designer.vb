<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
    Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.OpenLogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
    Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.ClearLogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
    Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.TextBox1 = New System.Windows.Forms.TextBox()
    Me.dgv = New System.Windows.Forms.DataGridView()
    Me.ListBox1 = New System.Windows.Forms.ListBox()
    Me.lblCount = New System.Windows.Forms.Label()
    Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.SummariesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.ActionFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.SpeedFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.MenuStrip1.SuspendLayout()
    CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SplitContainer1.Panel1.SuspendLayout()
    Me.SplitContainer1.Panel2.SuspendLayout()
    Me.SplitContainer1.SuspendLayout()
    CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SplitContainer2.Panel1.SuspendLayout()
    Me.SplitContainer2.Panel2.SuspendLayout()
    Me.SplitContainer2.SuspendLayout()
    CType(Me.dgv, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'MenuStrip1
    '
    Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
    Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ClearLogToolStripMenuItem, Me.ViewToolStripMenuItem})
    Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
    Me.MenuStrip1.Name = "MenuStrip1"
    Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 2, 0, 2)
    Me.MenuStrip1.Size = New System.Drawing.Size(848, 24)
    Me.MenuStrip1.TabIndex = 0
    Me.MenuStrip1.Text = "MenuStrip1"
    '
    'FileToolStripMenuItem
    '
    Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenLogToolStripMenuItem, Me.ToolStripSeparator1, Me.ExitToolStripMenuItem})
    Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
    Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
    Me.FileToolStripMenuItem.Text = "File"
    '
    'OpenLogToolStripMenuItem
    '
    Me.OpenLogToolStripMenuItem.Name = "OpenLogToolStripMenuItem"
    Me.OpenLogToolStripMenuItem.Size = New System.Drawing.Size(126, 22)
    Me.OpenLogToolStripMenuItem.Text = "Open Log"
    '
    'ToolStripSeparator1
    '
    Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
    Me.ToolStripSeparator1.Size = New System.Drawing.Size(123, 6)
    '
    'ExitToolStripMenuItem
    '
    Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
    Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(126, 22)
    Me.ExitToolStripMenuItem.Text = "Exit"
    '
    'ClearLogToolStripMenuItem
    '
    Me.ClearLogToolStripMenuItem.Name = "ClearLogToolStripMenuItem"
    Me.ClearLogToolStripMenuItem.Size = New System.Drawing.Size(69, 20)
    Me.ClearLogToolStripMenuItem.Text = "Clear Log"
    '
    'SplitContainer1
    '
    Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
    Me.SplitContainer1.Name = "SplitContainer1"
    Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
    '
    'SplitContainer1.Panel1
    '
    Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
    '
    'SplitContainer1.Panel2
    '
    Me.SplitContainer1.Panel2.Controls.Add(Me.ListBox1)
    Me.SplitContainer1.Size = New System.Drawing.Size(848, 481)
    Me.SplitContainer1.SplitterDistance = 211
    Me.SplitContainer1.TabIndex = 1
    '
    'SplitContainer2
    '
    Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
    Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
    Me.SplitContainer2.Name = "SplitContainer2"
    Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
    '
    'SplitContainer2.Panel1
    '
    Me.SplitContainer2.Panel1.Controls.Add(Me.lblCount)
    Me.SplitContainer2.Panel1.Controls.Add(Me.Label1)
    Me.SplitContainer2.Panel1.Controls.Add(Me.TextBox1)
    '
    'SplitContainer2.Panel2
    '
    Me.SplitContainer2.Panel2.Controls.Add(Me.dgv)
    Me.SplitContainer2.Size = New System.Drawing.Size(848, 211)
    Me.SplitContainer2.SplitterDistance = 25
    Me.SplitContainer2.TabIndex = 2
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(20, 6)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(44, 13)
    Me.Label1.TabIndex = 1
    Me.Label1.Text = "Search:"
    '
    'TextBox1
    '
    Me.TextBox1.Location = New System.Drawing.Point(70, 3)
    Me.TextBox1.Name = "TextBox1"
    Me.TextBox1.Size = New System.Drawing.Size(212, 20)
    Me.TextBox1.TabIndex = 0
    '
    'dgv
    '
    Me.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
    Me.dgv.Dock = System.Windows.Forms.DockStyle.Fill
    Me.dgv.Location = New System.Drawing.Point(0, 0)
    Me.dgv.Name = "dgv"
    Me.dgv.Size = New System.Drawing.Size(848, 182)
    Me.dgv.TabIndex = 0
    '
    'ListBox1
    '
    Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.ListBox1.FormattingEnabled = True
    Me.ListBox1.Location = New System.Drawing.Point(0, 0)
    Me.ListBox1.Name = "ListBox1"
    Me.ListBox1.Size = New System.Drawing.Size(848, 266)
    Me.ListBox1.TabIndex = 0
    '
    'lblCount
    '
    Me.lblCount.AutoSize = True
    Me.lblCount.Location = New System.Drawing.Point(359, 6)
    Me.lblCount.Name = "lblCount"
    Me.lblCount.Size = New System.Drawing.Size(38, 13)
    Me.lblCount.TabIndex = 2
    Me.lblCount.Text = "Count:"
    '
    'ViewToolStripMenuItem
    '
    Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SummariesToolStripMenuItem, Me.ActionFilesToolStripMenuItem, Me.SpeedFilesToolStripMenuItem})
    Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
    Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
    Me.ViewToolStripMenuItem.Text = "View"
    '
    'SummariesToolStripMenuItem
    '
    Me.SummariesToolStripMenuItem.Name = "SummariesToolStripMenuItem"
    Me.SummariesToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
    Me.SummariesToolStripMenuItem.Text = "Summaries Files"
    '
    'ActionFilesToolStripMenuItem
    '
    Me.ActionFilesToolStripMenuItem.Name = "ActionFilesToolStripMenuItem"
    Me.ActionFilesToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
    Me.ActionFilesToolStripMenuItem.Text = "Action Files"
    '
    'SpeedFilesToolStripMenuItem
    '
    Me.SpeedFilesToolStripMenuItem.Name = "SpeedFilesToolStripMenuItem"
    Me.SpeedFilesToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
    Me.SpeedFilesToolStripMenuItem.Text = "Speed Files"
    '
    'frmMain
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(848, 505)
    Me.Controls.Add(Me.SplitContainer1)
    Me.Controls.Add(Me.MenuStrip1)
    Me.MainMenuStrip = Me.MenuStrip1
    Me.Margin = New System.Windows.Forms.Padding(2)
    Me.Name = "frmMain"
    Me.Text = "Form1"
    Me.MenuStrip1.ResumeLayout(False)
    Me.MenuStrip1.PerformLayout()
    Me.SplitContainer1.Panel1.ResumeLayout(False)
    Me.SplitContainer1.Panel2.ResumeLayout(False)
    CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.SplitContainer1.ResumeLayout(False)
    Me.SplitContainer2.Panel1.ResumeLayout(False)
    Me.SplitContainer2.Panel1.PerformLayout()
    Me.SplitContainer2.Panel2.ResumeLayout(False)
    CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
    Me.SplitContainer2.ResumeLayout(False)
    CType(Me.dgv, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
  Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents OpenLogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
  Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents ClearLogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
  Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
  Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
  Friend WithEvents dgv As System.Windows.Forms.DataGridView
  Friend WithEvents lblCount As System.Windows.Forms.Label
  Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents SummariesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents ActionFilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents SpeedFilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
