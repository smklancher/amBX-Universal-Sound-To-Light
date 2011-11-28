Imports Un4seen.Bass
Imports System.Runtime.InteropServices
Imports System.IO
Imports Un4seen.Bass.Misc

Public Class Form1

    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Mix.Deinit()
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'init light and sound mix
        Mix.Init(Me.Handle, LeftLightSim, RightLightSim)

        'populate the dropdown with recording sources
        Dim n As Integer = 0
        Dim info As New BASS_DEVICEINFO()
        While (Bass.BASS_RecordGetDeviceInfo(n, info))
            cmbSource.Items.Add(info.ToString)
            n += 1
        End While


    End Sub


   

    Private Sub cmdStart_Click(sender As System.Object, e As System.EventArgs) Handles cmdStart.Click

        Mix.StartMix()

    End Sub

    Private Sub cmdStop_Click(sender As Object, e As System.EventArgs) Handles cmdStop.Click
        Mix.StopMix()
    End Sub
End Class
