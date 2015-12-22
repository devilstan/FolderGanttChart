Imports System.IO       'Get a reference to the IO class

Public Class AdvancedFileSystemWatcher
    Inherits FileSystemWatcher      'Inherit the base class

    'Create the public events the application can watch for
    Public Event NetworkPathUnavailable(ByVal Message As String)
    Public Event NetworkPathAvailable(ByVal Message As String)
    'Create the timer to use to scan the path
    Private WithEvents _myTimer As New System.Timers.Timer
    'create a boolean to record the network status
    Private _isNetworkUnavailable As Boolean = False

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        'use the timers default interval of 100 milliseconds and start the timer
        _myTimer.Start()
    End Sub


    Public Sub New(ByVal iNetworkWatchInterval As Int32)
        'check to see if the interval is a positive integer value and set it.
        If iNetworkWatchInterval > 0 Then
            _myTimer.Interval = iNetworkWatchInterval
        Else    'The user entered a
            _myTimer.Interval = 1000
        End If
        _myTimer.Start()

    End Sub

    Public Sub close()
        _myTimer.Stop()
        _myTimer.Dispose()

    End Sub

    ''' <summary>
    ''' In this sub we will try to determine if the folder exists If not there may be a network error.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub _myTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles _myTimer.Elapsed
        Dim myFolder As DirectoryInfo

        Try
            'create a new directoryinfo object so we can see if it exists
            myFolder = New DirectoryInfo(MyBase.Path.ToString)
            'Check the boolen to see if the path is available or not, this way we don't keep raising events. we only raise the event once
            If Not _isNetworkUnavailable Then
                'Ok the boolean says the path should be available lets check it
                If Not myFolder.Exists Then
                    'oops the path cannot be contacted or doesnt exist, set the boolean, and raise the unavailable event
                    _isNetworkUnavailable = True
                    RaiseEvent NetworkPathUnavailable("A network error has occured or the path does not exist anymore")
                End If
            Else    'Ok the boolean says the path should be unavailable, lets see if it is available again
                If myFolder.Exists Then
                    'alright the path is available again, set the boolean and raise the available event
                    _isNetworkUnavailable = False
                    RaiseEvent NetworkPathAvailable("The network or path has been restored")
                End If
            End If
        Catch ex As Exception
            Throw
        Finally
            myFolder = Nothing
        End Try

    End Sub

End Class
