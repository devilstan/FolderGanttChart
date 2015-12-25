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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ContextMenuGanttChart1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveImageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuGanttChart2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FileSystemWatcher1 = New System.IO.FileSystemWatcher()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.TextBox_debug = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GanttChart1 = New gangan.GanttChart()
        Me.GanttChart2 = New gangan.GanttChart()
        Me.ContextMenuGanttChart1.SuspendLayout()
        Me.ContextMenuGanttChart2.SuspendLayout()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuGanttChart1
        '
        Me.ContextMenuGanttChart1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveImageToolStripMenuItem})
        Me.ContextMenuGanttChart1.Name = "ContextMenuGanttChart1"
        Me.ContextMenuGanttChart1.Size = New System.Drawing.Size(143, 26)
        '
        'SaveImageToolStripMenuItem
        '
        Me.SaveImageToolStripMenuItem.Name = "SaveImageToolStripMenuItem"
        Me.SaveImageToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.SaveImageToolStripMenuItem.Text = "Save image"
        '
        'ContextMenuGanttChart2
        '
        Me.ContextMenuGanttChart2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1})
        Me.ContextMenuGanttChart2.Name = "ContextMenuGanttChart1"
        Me.ContextMenuGanttChart2.Size = New System.Drawing.Size(143, 26)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(142, 22)
        Me.ToolStripMenuItem1.Text = "Save image"
        '
        'FileSystemWatcher1
        '
        Me.FileSystemWatcher1.EnableRaisingEvents = True
        Me.FileSystemWatcher1.SynchronizingObject = Me
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'TextBox_debug
        '
        Me.TextBox_debug.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_debug.Location = New System.Drawing.Point(661, 22)
        Me.TextBox_debug.Multiline = True
        Me.TextBox_debug.Name = "TextBox_debug"
        Me.TextBox_debug.ReadOnly = True
        Me.TextBox_debug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox_debug.Size = New System.Drawing.Size(335, 498)
        Me.TextBox_debug.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.TextBox_debug)
        Me.GroupBox1.Controls.Add(Me.GanttChart1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 7)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(996, 520)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 500
        '
        'GanttChart1
        '
        Me.GanttChart1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GanttChart1.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.GanttChart1.ContextMenuStrip = Me.ContextMenuGanttChart1
        Me.GanttChart1.Cursor = System.Windows.Forms.Cursors.Default
        Me.GanttChart1.DateFont = New System.Drawing.Font("微軟正黑體", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GanttChart1.Font = New System.Drawing.Font("微軟正黑體", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.GanttChart1.FromDate = New Date(CType(0, Long))
        Me.GanttChart1.Location = New System.Drawing.Point(0, 23)
        Me.GanttChart1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GanttChart1.Name = "GanttChart1"
        Me.GanttChart1.RowFont = New System.Drawing.Font("微軟正黑體", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.GanttChart1.Size = New System.Drawing.Size(655, 497)
        Me.GanttChart1.TabIndex = 0
        Me.GanttChart1.Text = "GanttChart1"
        Me.GanttChart1.TimeFont = New System.Drawing.Font("微軟正黑體", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GanttChart1.ToDate = New Date(CType(0, Long))
        Me.GanttChart1.ToolTipText = CType(resources.GetObject("GanttChart1.ToolTipText"), System.Collections.Generic.List(Of String))
        Me.GanttChart1.ToolTipTextTitle = ""
        '
        'GanttChart2
        '
        Me.GanttChart2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GanttChart2.BackColor = System.Drawing.Color.White
        Me.GanttChart2.ContextMenuStrip = Me.ContextMenuGanttChart2
        Me.GanttChart2.DateFont = New System.Drawing.Font("Verdana", 8.0!)
        Me.GanttChart2.FromDate = New Date(CType(0, Long))
        Me.GanttChart2.Location = New System.Drawing.Point(12, 534)
        Me.GanttChart2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GanttChart2.Name = "GanttChart2"
        Me.GanttChart2.RowFont = New System.Drawing.Font("Verdana", 8.0!)
        Me.GanttChart2.Size = New System.Drawing.Size(996, 34)
        Me.GanttChart2.TabIndex = 1
        Me.GanttChart2.Text = "GanttChart2"
        Me.GanttChart2.TimeFont = New System.Drawing.Font("Verdana", 8.0!)
        Me.GanttChart2.ToDate = New Date(CType(0, Long))
        Me.GanttChart2.ToolTipText = CType(resources.GetObject("GanttChart2.ToolTipText"), System.Collections.Generic.List(Of String))
        Me.GanttChart2.ToolTipTextTitle = ""
        Me.GanttChart2.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1024, 581)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GanttChart2)
        Me.Font = New System.Drawing.Font("微軟正黑體", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(347, 333)
        Me.Name = "Form1"
        Me.Text = "Gantt Chart Tester"
        Me.ContextMenuGanttChart1.ResumeLayout(False)
        Me.ContextMenuGanttChart2.ResumeLayout(False)
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GanttChart1 As gangan.GanttChart
    Friend WithEvents GanttChart2 As gangan.GanttChart
    Friend WithEvents ContextMenuGanttChart2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuGanttChart1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveImageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileSystemWatcher1 As IO.FileSystemWatcher
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents TextBox_debug As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Timer1 As Timer
End Class
