Imports System
Imports System.IO
Imports System.Diagnostics
Imports System.Collections
Imports System.Xml

Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Randomize()

        With GanttChart1
            .RowFont = New Font("Arial", 8.0, FontStyle.Regular, GraphicsUnit.Point)
            .FromDate = New Date(2015, 12, 12, 0, 0, 0)
            .ToDate = New Date(2015, 12, 31, 0, 0, 0)

            Dim xdoc As XmlDocument = New XmlDocument
            Dim xRoot As XmlNode
            Dim xNodeList As XmlNodeList
            Dim xNodeTemp As XmlNode
            Dim lst As New List(Of BarInformation)
            Try
                '讀取 XML
                xdoc.Load("XML_log.xml")
                xRoot = CType(xdoc.DocumentElement, XmlNode)
                '選擇 section
                Dim rowindex As Integer = 0
                For Each Dir As String In Directory.GetDirectories("C:\Users\devilstan\Downloads\BT\0816ftn026")
                    Dim dirarr As String()
                    dirarr = Dir.Split("\")
                    xNodeTemp = xRoot.SelectSingleNode("folder[@name='" & dirarr(dirarr.Length - 1) & "']")
                    xNodeList = xNodeTemp.SelectNodes("log[@time!='']")
                    If xNodeList.Count > 0 Then lst.Add(New BarInformation(Dir, CType(xNodeList.Item(0), XmlElement).GetAttribute("time"), CType(xNodeList.Item(xNodeList.Count - 1), XmlElement).GetAttribute("time"), Color.Aqua, Color.Khaki, rowindex))
                    rowindex = rowindex + 1
                Next

            Catch ex As Exception
                MessageBox.Show(ex.Message & System.Environment.NewLine & ex.StackTrace)
            End Try

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FileSystemWatcher1 = New System.IO.FileSystemWatcher()

        'this is the path we want to monitor
        FileSystemWatcher1.Path = "C:\Users\devilstan\Downloads\BT\0816ftn026"

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

        Button1.Enabled = False
        Button2.Enabled = True

        'End of code for btn_start_click
    End Sub

    Private Sub logchange(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        Dim temparr As String()
        Dim subfolder As String
        Dim sfile As String
        temparr = e.Name.Split("\")
        subfolder = temparr(0)
        sfile = temparr(1)

        If e.ChangeType = IO.WatcherChangeTypes.Changed Then
            'MsgBox("change")
            writelogxml(subfolder)
            UpdateUI(GanttChart1)
        End If
        If e.ChangeType = IO.WatcherChangeTypes.Created Then
            writelogxml(subfolder)
            UpdateUI(GanttChart1)
        End If
        If e.ChangeType = IO.WatcherChangeTypes.Deleted Then
            'MsgBox("delete")
            writelogxml(subfolder)
            UpdateUI(GanttChart1)
        End If
    End Sub

    Public Sub logrename(ByVal source As Object, ByVal e As System.IO.RenamedEventArgs)
        MsgBox("rename")
        'txt_folderactivity.Text &= "File" & e.OldName &
        '              " has been renamed to " & e.Name & vbCrLf
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Stop watching the folder
        FileSystemWatcher1.EnableRaisingEvents = False
        Button1.Enabled = True
        Button2.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim objReader As New StreamReader("test.txt")
        Dim sLine As String = ""
        Dim arrText As New ArrayList()
        Do
            sLine = objReader.ReadLine()
            If Not sLine Is Nothing Then
                arrText.Add(sLine)
            End If
        Loop Until sLine Is Nothing
        objReader.Close()
        UpdateUI(GanttChart1)
    End Sub

    Private Delegate Sub UpdateUICallBack(ByVal GanttChart1 As Control)

    Private Sub UpdateUI(ByVal c As Control)
        If Me.InvokeRequired() Then
            Dim cb As New UpdateUICallBack(AddressOf UpdateUI)
            Me.Invoke(cb, c)
        Else
            Dim xdoc As XmlDocument = New XmlDocument
            Dim xRoot As XmlNode
            Dim xNodeList As XmlNodeList
            Dim xNodeTemp As XmlNode
            Try
                '讀取 XML
                xdoc.Load("XML_log.xml")
                xRoot = CType(xdoc.DocumentElement, XmlNode)
                '選擇 section
                xNodeTemp = xRoot.SelectSingleNode("folder[@name='" & "test" & "']")
                xNodeList = xNodeTemp.SelectNodes("log[@time!='']")
            Catch ex As Exception
                MessageBox.Show(ex.Message & System.Environment.NewLine & ex.StackTrace)
            End Try

            With GanttChart1
                .RemoveBars()
                Dim lst As New List(Of BarInformation)
                lst.Add(New BarInformation("程式碼維護", CType(xNodeList.Item(0), XmlElement).GetAttribute("time"), CType(xNodeList.Item(xNodeList.Count - 1), XmlElement).GetAttribute("time"), Color.Aqua, Color.Khaki, 0))
                lst.Add(New BarInformation("制御仕樣書", New Date(2015, 12, 13), New Date(2015, 12, 20), Color.AliceBlue, Color.Khaki, 1))
                lst.Add(New BarInformation("測試仕樣書", New Date(2015, 12, 14), New Date(2015, 12, 24), Color.Violet, Color.Khaki, 2))
                lst.Add(New BarInformation("檢核", New Date(2015, 12, 21), New Date(2015, 12, 22, 12, 0, 0), Color.Yellow, Color.Khaki, 3))
                lst.Add(New BarInformation("會議記錄", New Date(2015, 12, 17), New Date(2015, 12, 24), Color.LawnGreen, Color.Khaki, 4))
                For Each bar As BarInformation In lst
                    .AddChartBar(bar.RowText, bar, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index)
                Next
                .Refresh()
            End With


        End If
    End Sub

    Private Sub writelogxml(folder As String)
        If File.Exists("XML_log.xml") Then
            Dim xdoc As XmlDocument = New XmlDocument
            Dim xRoot As XmlNode
            Dim xNodeTemp As XmlNode, xNodeTemp2 As XmlNodeList
            Dim xElement2 As XmlElement
            Dim xChildElement As XmlElement
            Try
                '讀取 XML
                xdoc.Load("XML_log.xml")
                xRoot = CType(xdoc.DocumentElement, XmlNode)
                '選擇 section
                xNodeTemp = xRoot.SelectSingleNode("folder[@name='" & folder & "']")
                If xNodeTemp Is Nothing Then
                    'root[@name]節點
                    'xNodeTemp = xRoot.SelectSingleNode("root[@name='H123']")
                    'xNodeTemp = xRoot.
                    xChildElement = xdoc.CreateElement("folder")
                    xChildElement.SetAttribute("name", folder)
                    xRoot.AppendChild(xChildElement)
                    'log[@time]節點
                    xElement2 = xdoc.CreateElement("log")
                    xElement2.SetAttribute("time", Date.Now.ToString("yyyy.MM.dd"))
                    xChildElement.AppendChild(xElement2)
                    xdoc.Save("XML_log.xml")
                Else
                    xNodeTemp2 = xNodeTemp.SelectNodes("log[@time='" & Date.Now.ToString("yyyy.MM.dd") & "']")
                    If xNodeTemp2.Count > 0 Then
                    Else
                        xElement2 = xdoc.CreateElement("log")
                        xElement2.SetAttribute("time", Date.Now.ToString("yyyy.MM.dd"))
                        xNodeTemp.AppendChild(xElement2)
                        xdoc.Save("XML_log.xml")
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
                xChildElement = xdoc.CreateElement("folder")
                xChildElement.SetAttribute("name", folder)
                xElement.AppendChild(xChildElement)
                '第 2 層節點
                xElement2 = xdoc.CreateElement("log")
                xElement2.SetAttribute("time", Date.Now.ToString("yyyy.MM.dd"))
                xChildElement.AppendChild(xElement2)
                xdoc.Save("XML_log.xml")
            Catch ex As Exception
                MessageBox.Show(ex.Message & System.Environment.NewLine & ex.StackTrace)
            End Try

        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        writelogxml("test")
    End Sub
End Class