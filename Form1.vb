Imports System
Imports System.IO
Imports System.Diagnostics
Imports System.Collections
Imports System.Xml
Imports System.ComponentModel
Imports System.Threading
Imports System.Security.Permissions
Imports System.Security.Principal

Public Class Form1
    Private WithEvents myfswFileWatcher As AdvancedFileSystemWatcher

    'Public rootDIR As String = "\\tyd095-pc\開發中\[Q]\H188V040"
    Public rootDIR As String = "C:\Users\devilstan\Documents"
    'Public rootDIR As String = "D:\workspace\myRepo\H188V040t" '"C:\Users\devilstan\Documents\測試基地\H188V030"
    'Public rootDIR As String = "C:\Users\devilstan\Documents\測試基地\H188V030"

    Dim folder_changed_now As String
    Dim filesinfo As Date()
    Dim update_delay As Integer
    Dim textbox_debug_clr As Boolean

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        update_delay = Timer1.Interval
        Timer1.Enabled = True
        Timer1.Stop()
        If My.Application.CommandLineArgs.Count > 0 Then
            Me.Text = My.Application.CommandLineArgs(0)
            rootDIR = Me.Text
        Else
            Me.Text = rootDIR
            If rootDIR = "" Then
                MsgBox("請使用 fire.bat 啟動甘甘")
                Exit Sub
            End If
        End If

        Randomize()

        Dim dirarr As String()
        dirarr = rootDIR.Split("\")
        NotifyIcon1.Text = dirarr(dirarr.Length - 1)

        With GanttChart1
            .RowFont = New Font("微軟正黑體", 8.0, FontStyle.Regular, GraphicsUnit.Point)
            '.FromDate = New Date(2015, 12, 12, 0, 0, 0)
            .ToDate = Date.Now 'New Date(2015, 12, 31, 0, 0, 0)

            Dim xdoc As XmlDocument = New XmlDocument
            Dim xRoot As XmlNode
            Dim xNodeList As XmlNodeList = Nothing
            Dim xNodeTemp As XmlNode = Nothing
            Dim xChildElement As XmlElement
            Dim xElement As XmlElement

            Dim lst As New List(Of BarInformation)
            Dim retry As Boolean = True
            While (retry)
                Try
                    '讀取 XML
                    xdoc.Load(rootDIR & "\XML_log.xml")
                    xRoot = CType(xdoc.DocumentElement, XmlNode)
                    .FromDate = CType(xdoc.DocumentElement, XmlElement).GetAttribute("create")
                    Dim rowindex As Integer = 0
                    '掃瞄子目錄
                    For Each Dir As String In Directory.GetDirectories(rootDIR)
                        'Dim dirarr As String()
                        dirarr = Dir.Split("\")
                        Try
                            '嘗試選擇子目錄
                            xNodeTemp = xRoot.SelectSingleNode("folder[@name='" & dirarr(dirarr.Length - 1) & "']")
                            '嘗試選擇子目錄裡的時間紀錄節點
                            xNodeList = xNodeTemp.SelectNodes("log[@time!='']")
                        Catch ex As Exception
                            '例外發生: 嘗試選擇子目錄裡的時間紀錄節點
                            '表示xml找不到子目錄紀錄， 需建立子目錄節點
                            Dim theday As Date = "#" & Directory.GetCreationTime(Dir) & "#"
                            xChildElement = xdoc.CreateElement("folder")
                            xChildElement.SetAttribute("name", dirarr(dirarr.Length - 1))
                            xChildElement.SetAttribute("create", theday.ToString("yyyy.MM.dd"))
                            xRoot.AppendChild(xChildElement)
                            '嘗試選擇子目錄 (再次)
                            xNodeTemp = xRoot.SelectSingleNode("folder[@name='" & dirarr(dirarr.Length - 1) & "']")
                            '嘗試選擇子目錄裡的時間紀錄節點 (再次)
                            xNodeList = xNodeTemp.SelectNodes("log[@time!='']")
                        Finally
                            'bar條繪製處理
                            '繪製子目錄起點
                            Dim theday As Date = "#" & CType(xNodeTemp, XmlElement).GetAttribute("create") & "#"
                            lst.Add(New BarInformation(dirarr(dirarr.Length - 1),
                                                           theday.ToString("yyyy.MM.dd"),
                                                           theday.AddMinutes(5.0F),
                                                           Color.FromArgb(239, 71, 111), Color.FromArgb(239, 71, 111), rowindex))
                            '繪製子目錄裡的時間記錄
                            theday = "#" & (CType(xNodeTemp, XmlElement).GetAttribute("create")) & "#"
                            Select Case xNodeList.Count
                                Case 0
                                Case 1
                                    lst.Add(New BarInformation(dirarr(dirarr.Length - 1),
                                                           CType(xNodeList.Item(0), XmlElement).GetAttribute("time"),
                                                           CType(xNodeList.Item(xNodeList.Count - 1), XmlElement).GetAttribute("time"),
                                                           Color.FromArgb(239, 71, 111), Color.FromArgb(239, 71, 111), rowindex))
                                Case Else
                                    lst.Add(New BarInformation(dirarr(dirarr.Length - 1),
                                                           CType(xNodeList.Item(0), XmlElement).GetAttribute("time"),
                                                           CType(xNodeList.Item(xNodeList.Count - 1), XmlElement).GetAttribute("time"),
                                                           Color.FromArgb(239, 71, 111), Color.FromArgb(239, 71, 111), rowindex))
                            End Select
                        End Try
                        rowindex = rowindex + 1
                    Next
                    xdoc.Save(rootDIR & "\XML_log.xml")
                    retry = False
                Catch ex As Exception
                    MessageBox.Show("初次監控執行，將在主目錄下建立記錄檔 XML_log.xml")
                    'Dim dirarr As String()
                    dirarr = rootDIR.Split("\")
                    Dim theday As Date = "#" & Directory.GetCreationTime(rootDIR) & "#"
                    xdoc = New XmlDocument
                    xdoc.AppendChild(xdoc.CreateXmlDeclaration("1.0", "UTF-8", "no"))
                    xElement = xdoc.CreateElement("root")
                    xElement.SetAttribute("name", dirarr(dirarr.Length - 1))
                    xElement.SetAttribute("create", theday.ToString("yyyy.MM.dd"))
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


                    lst.Add(New BarInformation("Row " & i + 1, New Date(2007, 12, 24, startHour, startMinute, 0),
                                               New Date(2007, 12, 24, endHour, endMinute, 0), Color.Maroon, Color.Khaki, i))
                Next

                For Each bar As BarInformation In lst
                    .AddChartBar(bar.RowText, bar, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index)
                Next
            End With
        End If
        FileSystemWatcher1 = New System.IO.FileSystemWatcher()  'local monitor
        myfswFileWatcher = New AdvancedFileSystemWatcher(5000)  'network monitor
        If True Then
            'this is the path we want to monitor
            myfswFileWatcher.Path = rootDIR

            'Add a list of Filter we want to specify
            'make sure you use OR for each Filter as we need to
            'all of those 
            myfswFileWatcher.NotifyFilter = (NotifyFilters.LastAccess Or
                                                NotifyFilters.LastWrite Or
                                                NotifyFilters.FileName Or
                                                NotifyFilters.DirectoryName)

            ' add the handler to each event
            AddHandler myfswFileWatcher.Changed, AddressOf logchange
            AddHandler myfswFileWatcher.Created, AddressOf logchange
            'AddHandler myfswFileWatcher.Deleted, AddressOf logchange

            ' add the rename handler as the signature is different
            AddHandler myfswFileWatcher.Renamed, AddressOf logrename

            'Start the filewatcher watching for files.
            myfswFileWatcher.EnableRaisingEvents = True
            System.Threading.Thread.Sleep(1000)
            myfswFileWatcher.IncludeSubdirectories = True
        Else
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

    Private Sub logchange(ByVal source As Object, ByVal e As System.IO.FileSystemEventArgs)
        If e.Name.ToUpper.Contains("XML_LOG") Or
           e.Name.ToUpper.Contains("~$") Or
           Path.GetExtension(e.FullPath).ToUpper.Contains("TMP") Then
            Exit Sub
        End If
        UpdateDebug(e)

        '資料夾或檔案(變動一個檔案可能引起多個連續事件)


        If e.Name.Contains("\") Then
            Me.folder_changed_now = e.Name.Split("\")(0)
        Else
            Me.folder_changed_now = ""
            Exit Sub
        End If

        If e.ChangeType = IO.WatcherChangeTypes.Changed Then
            'writelogxml(e.Name, filesinfo)
            'UpdateUI(GanttChart1)
        End If
        If e.ChangeType = IO.WatcherChangeTypes.Created Then
            'writelogxml(e.Name, filesinfo)
            'UpdateUI(GanttChart1)
        End If
        If e.ChangeType = IO.WatcherChangeTypes.Deleted Then
            'writelogxml(e.Name, filesinfo)
            'UpdateUI(GanttChart1)
        End If

    End Sub

    Public Sub logrename(ByVal source As Object, ByVal e As System.IO.RenamedEventArgs)
        'MsgBox("rename")
        'txt_folderactivity.Text &= "File" & e.OldName &
        '              " has been renamed to " & e.Name & vbCrLf
    End Sub

    Private Delegate Sub UpdateDebugCallBack(ByVal e As System.IO.FileSystemEventArgs)

    Private Sub UpdateDebug(ByVal e As System.IO.FileSystemEventArgs)
        If Me.InvokeRequired() Then
            Dim cb As New UpdateDebugCallBack(AddressOf UpdateDebug)
            Me.Invoke(cb, e)
        Else
            Dim wct As WatcherChangeTypes = e.ChangeType
            If textbox_debug_clr = True Then
                TextBox_debug.Text = ""
                textbox_debug_clr = False
            End If

            TextBox_debug.AppendText(wct.ToString() & ", " & e.Name & vbCrLf)
            Timer1.Stop()
            Timer1.Start()
            End If
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
                    Dim theday As Date
                    Try
                        '讀取 XML
                        xdoc.Load(rootDIR & "\XML_log.xml")
                        xRoot = CType(xdoc.DocumentElement, XmlNode)
                        '選擇子目錄節點
                        xNodeList = xRoot.SelectNodes("folder[@name!='']")
                        For intI As Integer = 0 To xNodeList.Count - 1
                            xNodeTemp = xNodeList.Item(intI)
                            xNodeList2 = xNodeTemp.SelectNodes("log[@time!='']")
                            If CType(xNodeTemp, XmlElement).GetAttribute("create") = "" Then
                                lst.Add(New BarInformation(CType(xNodeList.Item(intI), XmlElement).GetAttribute("name"),
                                                       Date.Now.ToString("yyyy.MM.dd HH"), Date.Now.ToString("yyyy.MM.dd HH"),
                                                       Color.FromArgb(239, 71, 111), Color.FromArgb(239, 71, 111), intI))
                            Else
                                theday = "#" & CType(xNodeTemp, XmlElement).GetAttribute("create") & "#"
                                lst.Add(New BarInformation(CType(xNodeList.Item(intI), XmlElement).GetAttribute("name"),
                                                       CType(xNodeTemp, XmlElement).GetAttribute("create"),
                                                       theday.AddMinutes(5.0F),
                                                       Color.FromArgb(239, 71, 111), Color.FromArgb(239, 71, 111), intI))
                            End If

                            Select Case xNodeList2.Count
                                Case 0
                                    'lst.Add(New BarInformation(CType(xNodeList(intI), XmlElement).GetAttribute("name"), CType(xNodeTemp, XmlElement).GetAttribute("create"), CType(xNodeTemp, XmlElement).GetAttribute("create"), Color.Aqua, Color.Khaki, intI))
                                Case 1
                                    lst.Add(New BarInformation(CType(xNodeList(intI), XmlElement).GetAttribute("name"),
                                                           CType(xNodeList2.Item(0), XmlElement).GetAttribute("time"),
                                                           CType(xNodeList2.Item(0), XmlElement).GetAttribute("time"),
                                                           Color.FromArgb(239, 71, 111), Color.FromArgb(239, 71, 111), intI))
                                Case Else
                                    lst.Add(New BarInformation(CType(xNodeList(intI), XmlElement).GetAttribute("name"),
                                                           CType(xNodeList2.Item(0), XmlElement).GetAttribute("time"),
                                                           CType(xNodeList2.Item(xNodeList2.Count - 1), XmlElement).GetAttribute("time"),
                                                           Color.FromArgb(239, 71, 111), Color.FromArgb(239, 71, 111), intI))
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

    'Private Sub writelogxml(folder As String, filesinfo As date())
    Private Sub writelogxml(folder As String, filesinfoo As IO.FileInfo())
        '設計規格:
        '當資料夾結構變動時，紀錄子目錄的變動時間，精度=小時
        '忽略檔案
        If filesinfoo(0).Name.ToUpper.Contains("XML_LOG") Or
           filesinfoo(0).Name.ToUpper.Contains("~$") Or
           filesinfoo(0).Name.ToUpper.Contains("~") Or
           Path.GetExtension(filesinfoo(0).FullName).ToUpper.Contains("TMP") Then
            Exit Sub
        End If

        'If FileInUse(rootDIR & "\XML_log.xml") Then
        'Exit Sub
        'End If

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
                '選擇節點folder[@name=子目錄名稱]
                xNodeTemp = xRoot.SelectSingleNode("folder[@name='" & folder & "']")
                If xNodeTemp Is Nothing Then
                    '如果找不到節點,則建立節點folder[@name=子目錄名稱]
                    xChildElement = xdoc.CreateElement("folder")
                    xChildElement.SetAttribute("name", folder)
                    xChildElement.SetAttribute("create", Date.Now.ToString("yyyy.MM.dd"))
                    xRoot.AppendChild(xChildElement)
                    If filesinfoo.Length > 0 Then
                        '建立節點log[@time]
                        xElement2 = xdoc.CreateElement("log")
                        xElement2.SetAttribute("time", filesinfoo(0).LastAccessTime.ToString("yyyy.MM.dd HH:mm:ss"))
                        xChildElement.AppendChild(xElement2)
                    End If
                Else
                    Dim date_tmp As Date
                    xNodeTemp2 = xNodeTemp.SelectNodes("log[@time!='']")
                    Select Case xNodeTemp2.Count
                        Case 0  '沒有紀錄，新增子目錄內的檔案列表中最近的存取時間
                            xElement2 = xdoc.CreateElement("log")
                            xElement2.SetAttribute("time", filesinfo(0).ToString("yyyy.MM.dd HH:mm:ss"))
                            xNodeTemp.AppendChild(xElement2)
                        Case 1  '存在一個紀錄，假如子目錄內的檔案列表中最近的存取時間>原本記錄，則新增子目錄內的檔案列表中最近的存取時間
                            date_tmp = "#" & CType(xNodeTemp2.Item(xNodeTemp2.Count - 1), XmlElement).GetAttribute("time") & "#"
                            If Date.Compare(filesinfo(0), date_tmp) > 0 Then
                                xElement2 = xdoc.CreateElement("log")
                                xElement2.SetAttribute("time", filesinfo(0).ToString("yyyy.MM.dd HH:mm:ss"))
                                xNodeTemp.AppendChild(xElement2)
                            End If
                        Case Else '存在多個紀錄，假如子目錄內的檔案列表中最近的存取時間與最後記錄的時差=0，複寫最後一個紀錄，否則新增一個紀錄
                            date_tmp = "#" & CType(xNodeTemp2.Item(xNodeTemp2.Count - 1), XmlElement).GetAttribute("time") & "#"
                            If date_tmp.ToString("yyyy.MM.dd HH") = filesinfo(0).ToString("yyyy.MM.dd HH") Then
                                xNodeTemp.RemoveChild(xNodeTemp2.Item(xNodeTemp2.Count - 1))
                            End If
                            xElement2 = xdoc.CreateElement("log")
                            xElement2.SetAttribute("time", filesinfo(0).ToString("yyyy.MM.dd HH:mm:ss"))
                            xNodeTemp.AppendChild(xElement2)
                    End Select
                End If
                If FileInUse(rootDIR & "\XML_log.xml") = False Then xdoc.Save(rootDIR & "\XML_log.xml")
            Catch ex As Exception
                MessageBox.Show(ex.Message & System.Environment.NewLine & ex.StackTrace)
            End Try
        Else
            Dim xdoc As XmlDocument
            Dim xElement As XmlElement
            Try
                Dim theday As Date = "#" & Directory.GetCreationTime(rootDIR) & "#"
                Dim dirarr As String()
                dirarr = rootDIR.Split("\")
                '建立一個 XmlDocument 物件並加入 Declaration
                xdoc = New XmlDocument
                xdoc.AppendChild(xdoc.CreateXmlDeclaration("1.0", "UTF-8", "no"))
                '建立根節點物件並加入 XmlDocument 中 (第 0 層)
                xElement = xdoc.CreateElement("root")
                '在 sections 寫入一個屬性
                xElement.SetAttribute("name", dirarr(dirarr.Length - 1))
                xElement.SetAttribute("create", theday.ToString("yyyy.MM.dd"))
                xdoc.AppendChild(xElement)
                If FileInUse(rootDIR & "\XML_log.xml") = False Then xdoc.Save(rootDIR & "\XML_log.xml")
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
        'Me.WindowState = FormWindowState.Minimized
        'Me.Visible = False
        Me.NotifyIcon1.Visible = False
        'e.Cancel = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Me.filesinfo Is Nothing And False Then
            Timer1.Stop()
            Timer1.Interval = update_delay
            textbox_debug_clr = True
            Exit Sub
        End If
        Dim myNOW As Date = Date.Now
        '讀取子目錄中所有檔案
        Dim di As New IO.DirectoryInfo(rootDIR & "\" & Me.folder_changed_now)
        Dim filesinfoo As IO.FileInfo()
        Dim myListo As New List(Of IO.FileInfo)()
        Dim recentList As String = ""
        Dim files() As String
        Dim myList As New List(Of Date)()
        Try
            'diar1 = di.GetFiles()
            files = IO.Directory.GetFiles(rootDIR & "\" & Me.folder_changed_now, "*.*", System.IO.SearchOption.AllDirectories)
            filesinfoo = di.GetFiles("*.*", System.IO.SearchOption.AllDirectories)
        Catch ex As Exception
            textbox_debug_clr = True
            Exit Sub
        End Try

        'list the names of all files in the specified directory
        For i As Integer = 0 To filesinfoo.Length - 1
            myListo.Add(filesinfoo(i))
        Next
        Array.Sort(Of IO.FileInfo)(filesinfoo, Function(p1, p2) p2.LastAccessTime.CompareTo(p1.LastAccessTime))
        For i As Integer = 0 To filesinfoo.Length - 1
            If DateTime.Compare(filesinfoo(i).LastAccessTime.AddSeconds(10), myNOW) > 0 Or
               DateTime.Compare(filesinfoo(i).LastWriteTime.AddSeconds(10), myNOW) > 0 Then
                recentList = recentList & filesinfoo(i).Name & vbCrLf
            End If
        Next

        For i As Integer = 0 To files.Length - 1
            myList.Add("#" & File.GetLastAccessTime(files(i)) & "#")
        Next
        '子目錄中所有檔案的最後存取時間排序, 0:最新, N:最舊
        Dim filesinfo As Date() = myList.ToArray()
        Array.Sort(Of Date)(filesinfo, Function(d1, d2) d2.CompareTo(d1))

        'Me.folder_changed_now = e.Name
        Me.filesinfo = filesinfo

        If folder_changed_now <> "" Then
            If FileInUse(rootDIR & "\XML_log.xml") = False Then
                writelogxml(folder_changed_now, filesinfoo)
                UpdateUI(GanttChart1)
            Else
                Dim thedelay As Integer = 300
                TextBox_debug.AppendText(thedelay & " 毫秒後重試一次" & vbCrLf)
                Timer1.Interval = thedelay
                Timer1.Start()
            End If

        Else

        End If
        'NotifyIcon1.Icon = SystemIcons.Information
        NotifyIcon1.BalloonTipTitle = "『" & folder_changed_now & "』有點動靜 - pid:" & Process.GetCurrentProcess.Id
        NotifyIcon1.BalloonTipText = recentList

        NotifyIcon1.Visible = True
        NotifyIcon1.ShowBalloonTip(2000)
        TextBox_debug.AppendText("更新通知" & vbCrLf)
        Timer1.Stop()
        textbox_debug_clr = True
    End Sub

    Private Function FileInUse(ByVal f As String) As Boolean
        Try
            Dim FS As IO.FileStream = IO.File.Open(f, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.None)
            FS.Close()
            FS.Dispose()
            FS = Nothing
        Catch
            TextBox_debug.AppendText("文件被其他程式鎖定" & vbCrLf)
            Return True
        End Try
        TextBox_debug.AppendText("文件可存取" & vbCrLf)
    End Function
End Class
