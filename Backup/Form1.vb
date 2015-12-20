Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Randomize()

        With GanttChart1
            .FromDate = New Date(2007, 12, 12, 0, 0, 0)
            .ToDate = New Date(2007, 12, 24, 0, 0, 0)

            Dim lst As New List(Of BarInformation)

            lst.Add(New BarInformation("Row 1", New Date(2007, 12, 12), New Date(2007, 12, 16), Color.Aqua, Color.Khaki, 0))
            lst.Add(New BarInformation("Row 2", New Date(2007, 12, 13), New Date(2007, 12, 20), Color.AliceBlue, Color.Khaki, 1))
            lst.Add(New BarInformation("Row 3", New Date(2007, 12, 14), New Date(2007, 12, 24), Color.Violet, Color.Khaki, 2))
            lst.Add(New BarInformation("Row 2", New Date(2007, 12, 21), New Date(2007, 12, 22, 12, 0, 0), Color.Yellow, Color.Khaki, 1))
            lst.Add(New BarInformation("Row 1", New Date(2007, 12, 17), New Date(2007, 12, 24), Color.LawnGreen, Color.Khaki, 0))

            For Each bar As BarInformation In lst
                .AddChartBar(bar.RowText, bar, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index)
            Next
        End With

        With GanttChart2
            .FromDate = New Date(2007, 12, 24, 17, 0, 0)
            .ToDate = New Date(2007, 12, 24, 22, 0, 0)

            Dim lst As New List(Of BarInformation)
            Dim numberOfRowsToAdd As Integer = Rnd() * 100

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

End Class