<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
    Me.ListBox1 = New System.Windows.Forms.ListBox()
    Me.ClearLogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
    Me.MenuStrip1.SuspendLayout()
    Me.SuspendLayout()
    '
    'MenuStrip1
    '
    Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
    Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ClearLogToolStripMenuItem})
    Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
    Me.MenuStrip1.Name = "MenuStrip1"
    Me.MenuStrip1.Size = New System.Drawing.Size(1130, 28)
    Me.MenuStrip1.TabIndex = 0
    Me.MenuStrip1.Text = "MenuStrip1"
    '
    'FileToolStripMenuItem
    '
    Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenLogToolStripMenuItem, Me.ToolStripSeparator1, Me.ExitToolStripMenuItem})
    Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
    Me.FileToolStripMenuItem.Size = New System.Drawing.Size(44, 24)
    Me.FileToolStripMenuItem.Text = "File"
    '
    'OpenLogToolStripMenuItem
    '
    Me.OpenLogToolStripMenuItem.Name = "OpenLogToolStripMenuItem"
    Me.OpenLogToolStripMenuItem.Size = New System.Drawing.Size(149, 26)
    Me.OpenLogToolStripMenuItem.Text = "Open Log"
    '
    'ToolStripSeparator1
    '
    Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
    Me.ToolStripSeparator1.Size = New System.Drawing.Size(146, 6)
    '
    'ExitToolStripMenuItem
    '
    Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
    Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(149, 26)
    Me.ExitToolStripMenuItem.Text = "Exit"
    '
    'ListBox1
    '
    Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.ListBox1.FormattingEnabled = True
    Me.ListBox1.ItemHeight = 16
    Me.ListBox1.Location = New System.Drawing.Point(0, 28)
    Me.ListBox1.Name = "ListBox1"
    Me.ListBox1.Size = New System.Drawing.Size(1130, 594)
    Me.ListBox1.TabIndex = 1
    '
    'ClearLogToolStripMenuItem
    '
    Me.ClearLogToolStripMenuItem.Name = "ClearLogToolStripMenuItem"
    Me.ClearLogToolStripMenuItem.Size = New System.Drawing.Size(84, 24)
    Me.ClearLogToolStripMenuItem.Text = "Clear Log"
    '
    'Form1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1130, 622)
    Me.Controls.Add(Me.ListBox1)
    Me.Controls.Add(Me.MenuStrip1)
    Me.MainMenuStrip = Me.MenuStrip1
    Me.Name = "Form1"
    Me.Text = "Form1"
    Me.MenuStrip1.ResumeLayout(False)
    Me.MenuStrip1.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
  Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents OpenLogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
  Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
  Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
  Friend WithEvents ClearLogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
