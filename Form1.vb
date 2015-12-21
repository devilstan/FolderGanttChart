Imports System
Imports System.IO
Imports System.Diagnostics
Imports System.Collections
Imports System.Xml
Imports System.ComponentModel

Public Class Form1

    Dim rootDIR As String = "C:\Users\devilstan\Documents\測試基地\H188V030"

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Application.CommandLineArgs.Count > 0 Then
            Me.Text = My.Application.CommandLineArgs(0)
            rootDIR = Me.Text
        Else
            Me.Text = rootDIR
        End If

        Randomize()

        With GanttChart1
            .RowFont = New Font("Arial", 8.0, FontStyle.Regular, GraphicsUnit.Point)
            .FromDate = New Date(2015, 12, 12, 0, 0, 0)
            .ToDate = New Date(2015, 12, 31, 0, 0, 0)

            Dim xdoc As XmlDocument = New XmlDocument
            Dim xRoot As XmlNode
            Dim xNodeList As XmlNodeList = Nothing
            Dim xNodeTemp As XmlNode
            Dim xChildElement As XmlElement
            Dim xElement As XmlElement

            Dim lst As New List(Of BarInformation)
            Dim retry As Boolean = True
            While (retry)
                Try
                    '讀取 XML
                    xdoc.Load(rootDIR & "\XML_log.xml")
                    xRoot = CType(xdoc.DocumentElement, XmlNode)
                    '選擇 section
                    Dim rowindex As Integer = 0
                    For Each Dir As String In Directory.GetDirectories(rootDIR)
                        Dim dirarr As String()
                        dirarr = Dir.Split("\")

                        Try
                            xNodeTemp = xRoot.SelectSingleNode("folder[@name='" & dirarr(dirarr.Length - 1) & "']")
                            xNodeList = xNodeTemp.SelectNodes("log[@time!='']")
                        Catch ex As Exception
                            '如果xml找不到子目錄
                            '在 root[@name] 下寫入一個節點名稱為 dirarr(dirarr.Length - 1)(第 1 層)
                            xChildElement = xdoc.CreateElement("folder")
                            xChildElement.SetAttribute("name", dirarr(dirarr.Length - 1))
                            Dim theday As Date = "#" & Directory.GetCreationTime(Dir) & "#"
                            xChildElement.SetAttribute("create", theday.ToString("yyyy.MM.dd"))
                            xRoot.AppendChild(xChildElement)
                            xdoc.Save(rootDIR & "\XML_log.xml")
                            xNodeTemp = xRoot.SelectSingleNode("folder[@name='" & dirarr(dirarr.Length - 1) & "']")
                            xNodeList = xNodeTemp.SelectNodes("log[@time!='']")
                        Finally
                            If xNodeList.Count > 0 Then
                                lst.Add(New BarInformation(dirarr(dirarr.Length - 1), CType(xNodeList.Item(0), XmlElement).GetAttribute("time"), CType(xNodeList.Item(xNodeList.Count - 1), XmlElement).GetAttribute("time"), Color.FromArgb(245, 203, 92), Color.FromArgb(245, 203, 92), rowindex))
                            Else
                                Dim theday As Date = "#" & (CType(xNodeTemp, XmlElement).GetAttribute("create")) & "#"
                                lst.Add(New BarInformation(dirarr(dirarr.Length - 1), theday.ToString("yyyy.MM.dd"), theday.ToString("yyyy.MM.dd"), Color.FromArgb(245, 203, 92), Color.FromArgb(245, 203, 92), rowindex))
                            End If
                        End Try
                        rowindex = rowindex + 1
                    Next
                    retry = False
                Catch ex As Exception
                    MessageBox.Show(ex.Message & System.Environment.NewLine & ex.StackTrace)
                    '建立一個 XmlDocument 物件並加入 Declaration
                    xdoc = New XmlDocument
                    xdoc.AppendChild(xdoc.CreateXmlDeclaration("1.0", "UTF-8", "no"))
                    '建立根節點物件並加入 XmlDocument 中 (第 0 層)
                    xElement = xdoc.CreateElement("root")
                    '在 sections 寫入一個屬性
                    xElement.SetAttribute("name", "H123")
                    xdoc.AppendChild(xElement)
                    xdoc.Save(rootDIR & "\XML_log.xml")
                    retry = True
                End Try
            End While

            For Each bar As BarInformation In lst
                .AddChartBar(bar.RowText, bar, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index)
            Next

        End With

        If True Then
            With GanttChart2
                .FromDate = New Date(2007, 12, 24, 17, 0, 0)
                .ToDate = New Date(2007, 12, 24, 22, 0, 0)

                Dim lst As New List(Of BarInformation)
                Dim numberOfRowsToAdd As Integer = 5 'Rnd() * 100

                For i As Integer = 0 To numberOfRowsToAdd
                    Dim startHour As Integer = (Rnd() * 4) + 17
                    Dim endHour As Integer = (Rnd() * 3) + startHour
                    Dim startMinute As Integer = Rnd() * 59
                    Dim endMinute As Integer = Rnd() * 59

                    If startHour = endHour Then
                        If startHour = 17 Then
                            endHour += 1
                        Else
                            startHour -= 1
                        End If
                    End If

                    If endHour >= 22 Then
                        endHour = 22
                        endMinute = Rnd() * 20
                    End If


                    lst.Add(New BarInformation("Row " & i + 1, New Date(2007, 12, 24, startHour, startMinute, 0), New Date(2007, 12, 24, endHour, endMinute, 0), Color.Maroon, Color.Khaki, i))
                Next

                For Each bar As BarInformation In lst
                    .AddChartBar(bar.RowText, bar, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index)
                Next
            End With
        End If
        FileSystemWatcher1 = New System.IO.FileSystemWatcher()

        'this is the path we want to monitor
        FileSystemWatcher1.Path = rootDIR

        'Add a list of Filter we want to specify
        'make sure you use OR for each Filter as we need to
        'all of those 

        FileSystemWatcher1.NotifyFilter = IO.NotifyFilters.DirectoryName
        FileSystemWatcher1.NotifyFilter = FileSystemWatcher1.NotifyFilter Or IO.NotifyFilters.FileName
        FileSystemWatcher1.NotifyFilter = FileSystemWatcher1.NotifyFilter Or IO.NotifyFilters.Attributes

        ' add the handler to each event
        AddHandler FileSystemWatcher1.Changed, AddressOf logchange
        AddHandler FileSystemWatcher1.Created, AddressOf logchange
        AddHandler FileSystemWatcher1.Deleted, AddressOf logchange

        ' add the rename handler as the signature is different
        AddHandler FileSystemWatcher1.Renamed, AddressOf logrename

        'Set this property to true to start watching
        FileSystemWatcher1.EnableRaisingEvents = True
        FileSystemWatcher1.IncludeSubdirectories = True
    End Sub

    Private Sub GanttChart1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GanttChart1.MouseMove
        With GanttChart1
            Dim toolTipText As New List(Of String)

            If .MouseOverRowText.Length > 0 Then
                Dim val As BarInformation = CType(.MouseOverRowValue, BarInformation)
                toolTipText.Add("[b]Date:")
                toolTipText.Add("From ")
                toolTipText.Add(val.FromTime.ToLongDateString & " - " & val.FromTime.ToString("HH:mm"))
                toolTipText.Add("To ")
                toolTipText.Add(val.ToTime.ToLongDateString & " - " & val.ToTime.ToString("HH:mm"))
            Else
                toolTipText.Add("")
            End If

            .ToolTipTextTitle = .MouseOverRowText
            .ToolTipText = toolTipText

        End With
    End Sub

    Private Sub GanttChart2_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GanttChart2.MouseMove
        With GanttChart2
            Dim toolTipText As New List(Of String)

            If .MouseOverRowText.Length > 0 Then
                Dim val As BarInformation = CType(.MouseOverRowValue, BarInformation)
                toolTipText.Add("[b]Time:")
                toolTipText.Add("From " & val.FromTime.ToString("HH:mm"))
                toolTipText.Add("To " & val.ToTime.ToString("HH:mm"))
            Else
                toolTipText.Add("")
            End If

            .ToolTipTextTitle = .MouseOverRowText
            .ToolTipText = toolTipText

        End With
    End Sub

    Private Sub SaveImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveImageToolStripMenuItem.Click
        SaveImage(GanttChart1)
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        SaveImage(GanttChart2)
    End Sub

    Private Sub SaveImage(ByVal gantt As GanttChart)
        Dim filePath As String = InputBox("Where to save the file?", "Save image", "C:\Temp\GanttChartTester.jpg")
        If filePath.Length = 0 Then Exit Sub
        gantt.SaveImage(filePath)
        MsgBox("Picture saved", MsgBoxStyle.Information)
    End Sub

    Private Sub logchange(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        Dim temparr As String()
        Dim subfolder As String
        Dim sfile As String
        If e.Name.Contains("\") Then
            temparr = e.Name.Split("\")
            subfolder = temparr(0)
            sfile = temparr(1)
        Else
            subfolder = e.Name
            sfile = ""
        End If

        If e.Name.Contains("XML_log") Then
            'MsgBox(sfile.Contains("XML_log"))
        Else
            If e.ChangeType = IO.WatcherChangeTypes.Changed Then
                'MsgBox("change")
                writelogxml(subfolder, sfile)
                UpdateUI(GanttChart1)
            End If
            If e.ChangeType = IO.WatcherChangeTypes.Created Then
                writelogxml(subfolder, sfile)
                UpdateUI(GanttChart1)
            End If
            If e.ChangeType = IO.WatcherChangeTypes.Deleted Then
                'MsgBox("delete")
                writelogxml(subfolder, sfile)
                UpdateUI(GanttChart1)
            End If
        End If
    End Sub

    Public Sub logrename(ByVal source As Object, ByVal e As System.IO.RenamedEventArgs)
        MsgBox("rename")
        'txt_folderactivity.Text &= "File" & e.OldName &
        '              " has been renamed to " & e.Name & vbCrLf
    End Sub

    Private Delegate Sub UpdateUICallBack(ByVal GanttChart1 As Control)

    Private Sub UpdateUI(ByVal c As Control)
        If Me.InvokeRequired() Then
            Dim cb As New UpdateUICallBack(AddressOf UpdateUI)
            Me.Invoke(cb, c)
        Else

            With GanttChart1
                .RemoveBars()
                Dim lst As New List(Of BarInformation)
                Dim xdoc As XmlDocument = New XmlDocument
                Dim xRoot As XmlNode
                Dim xNodeList As XmlNodeList = Nothing
                Dim xNodeList2 As XmlNodeList = Nothing
                Dim xNodeTemp As XmlNode
                Dim xChildElement As XmlElement
                Try
                    '讀取 XML
                    xdoc.Load(rootDIR & "\XML_log.xml")
                    xRoot = CType(xdoc.DocumentElement, XmlNode)
                    '選擇 section
                    xNodeList = xRoot.SelectNodes("folder[@name!='']")
                    For intI As Integer = 0 To xNodeList.Count - 1
                        xNodeTemp = xNodeList.Item(intI)
                        xNodeList2 = xNodeTemp.SelectNodes("log[@time!='']")
                        If CType(xNodeTemp, XmlElement).GetAttribute("create") = "" Then
                            lst.Add(New BarInformation(CType(xNodeList.Item(intI), XmlElement).GetAttribute("name"), Date.Now.ToString("yyyy.MM.dd"), Date.Now.ToString("yyyy.MM.dd"), Color.Aqua, Color.Khaki, intI))
                        Else
                            lst.Add(New BarInformation(CType(xNodeList.Item(intI), XmlElement).GetAttribute("name"), CType(xNodeTemp, XmlElement).GetAttribute("create"), CType(xNodeTemp, XmlElement).GetAttribute("create"), Color.Aqua, Color.Khaki, intI))
                        End If

                        Select Case xNodeList2.Count
                            Case 0
                                'lst.Add(New BarInformation(CType(xNodeList(intI), XmlElement).GetAttribute("name"), CType(xNodeTemp, XmlElement).GetAttribute("create"), CType(xNodeTemp, XmlElement).GetAttribute("create"), Color.Aqua, Color.Khaki, intI))
                            Case 1
                                lst.Add(New BarInformation(CType(xNodeList(intI), XmlElement).GetAttribute("name"), CType(xNodeList2.Item(0), XmlElement).GetAttribute("time"), CType(xNodeList2.Item(0), XmlElement).GetAttribute("time"), Color.Aqua, Color.Khaki, intI))
                            Case Else
                                lst.Add(New BarInformation(CType(xNodeList(intI), XmlElement).GetAttribute("name"), CType(xNodeList2.Item(0), XmlElement).GetAttribute("time"), CType(xNodeList2.Item(xNodeList2.Count - 1), XmlElement).GetAttribute("time"), Color.Aqua, Color.Khaki, intI))
                        End Select
                    Next

                Catch ex As Exception
                    MessageBox.Show(ex.Message & System.Environment.NewLine & ex.StackTrace)
                End Try

                For Each bar As BarInformation In lst
                    .AddChartBar(bar.RowText, bar, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index)
                Next
                .Refresh()
            End With
        End If
    End Sub

    Private Sub writelogxml(folder As String, sfile As String)
        If File.Exists(rootDIR & "\XML_log.xml") Then
            Dim xdoc As XmlDocument = New XmlDocument
            Dim xRoot As XmlNode
            Dim xNodeTemp As XmlNode, xNodeTemp2 As XmlNodeList
            Dim xElement2 As XmlElement
            Dim xChildElement As XmlElement
            Try
                '讀取 XML
                xdoc.Load(rootDIR & "\XML_log.xml")
                xRoot = CType(xdoc.DocumentElement, XmlNode)
                '選擇 section
                xNodeTemp = xRoot.SelectSingleNode("folder[@name='" & folder & "']")
                If xNodeTemp Is Nothing Then
                    'root[@name]節點
                    'xNodeTemp = xRoot.SelectSingleNode("root[@name='H123']")
                    'xNodeTemp = xRoot.
                    xChildElement = xdoc.CreateElement("folder")
                    xChildElement.SetAttribute("name", folder)
                    xChildElement.SetAttribute("create", Date.Now.ToString("yyyy.MM.dd"))
                    xRoot.AppendChild(xChildElement)
                    If sfile <> "" Then
                        'log[@time]節點
                        xElement2 = xdoc.CreateElement("log")
                        xElement2.SetAttribute("time", Date.Now.ToString("yyyy.MM.dd"))
                        xChildElement.AppendChild(xElement2)

                    Else

                    End If
                    xdoc.Save(rootDIR & "\XML_log.xml")
                Else
                    xNodeTemp2 = xNodeTemp.SelectNodes("log[@time='" & Date.Now.ToString("yyyy.MM.dd") & "']")
                    If xNodeTemp2.Count > 0 Then
                    Else
                        xElement2 = xdoc.CreateElement("log")
                        xElement2.SetAttribute("time", Date.Now.ToString("yyyy.MM.dd"))
                        xNodeTemp.AppendChild(xElement2)
                        xdoc.Save(rootDIR & "\XML_log.xml")
                    End If
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message & System.Environment.NewLine & ex.StackTrace)
            End Try
        Else
            Dim xdoc As XmlDocument
            Dim xElement As XmlElement
            Dim xChildElement As XmlElement
            Dim xElement2 As XmlElement
            Try
                '建立一個 XmlDocument 物件並加入 Declaration
                xdoc = New XmlDocument
                xdoc.AppendChild(xdoc.CreateXmlDeclaration("1.0", "UTF-8", "no"))
                '建立根節點物件並加入 XmlDocument 中 (第 0 層)
                xElement = xdoc.CreateElement("root")
                '在 sections 寫入一個屬性
                xElement.SetAttribute("name", "H123")
                xdoc.AppendChild(xElement)
                '在 sections 下寫入一個節點名稱為 section(第 1 層)
                'xChildElement = xdoc.CreateElement("folder")
                'xChildElement.SetAttribute("name", folder)
                'xElement.AppendChild(xChildElement)
                '第 2 層節點
                'xElement2 = xdoc.CreateElement("log")
                'xElement2.SetAttribute("time", Date.Now.ToString("yyyy.MM.dd"))
                'xChildElement.AppendChild(xElement2)
                xdoc.Save(rootDIR & "\XML_log.xml")
            Catch ex As Exception
                MessageBox.Show(ex.Message & System.Environment.NewLine & ex.StackTrace)
            End Try

        End If
    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Minimized
            Me.Visible = False
            Me.NotifyIcon1.Visible = True
        End If
    End Sub

    Private Sub NotifyIcon1_Click(sender As Object, e As EventArgs) Handles NotifyIcon1.Click
        Me.Visible = True
        Me.WindowState = FormWindowState.Normal
        Me.NotifyIcon1.Visible = False
        Me.Show()
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.WindowState = FormWindowState.Minimized
        Me.Visible = False
        Me.NotifyIcon1.Visible = True
        e.Cancel = True
    End Sub
End Class
