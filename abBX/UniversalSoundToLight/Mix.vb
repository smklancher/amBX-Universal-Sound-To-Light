Public Class Mix

    Public Shared Property Interval As Integer
        Set(value As Integer)
            SampleTimer.Interval = value
        End Set
        Get
            Return SampleTimer.Interval
        End Get
    End Property


    Private Shared WithEvents SampleTimer As New Timer

    Public Shared Sub StartMix()
        Sound.StartRecord()
        SampleTimer.Enabled = True
    End Sub

    Public Shared Sub StopMix()
        SampleTimer.Enabled = False
        Sound.StopRecord()
    End Sub

    Public Shared Sub Init(MainWindow As System.IntPtr, Optional LeftSim As PictureBox = Nothing, Optional RightSim As PictureBox = Nothing)
        'init light drivers
        Light.Init(LeftSim, RightSim)

        'init sound sample interval, etc
        Sound.Init(MainWindow)

        Interval = 250
    End Sub


    Public Shared Sub Deinit()
        Try
            Sound.StopRecord()
            Light.DeInit()
        Catch ex As Exception
            Debug.Print("Problem cleaning up: " & ex.Message)
        End Try
    End Sub


    Private Shared Sub SampleTimer_Tick(sender As Object, e As System.EventArgs) Handles SampleTimer.Tick



        'sounds stuff
        

        Sound.Sound_Tick()






    End Sub

    Private Shared Sub Log(msg As String)
        Form1.txtLog.Text = msg & vbNewLine & Form1.txtLog.Text
    End Sub
End Class
