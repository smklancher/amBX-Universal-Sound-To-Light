Imports Un4seen.Bass
Imports System.Runtime.InteropServices
Imports System.IO
Imports Un4seen.Bass.Misc

Public Class Form1

    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Sound.StopRecord()
            Light.DeInit()
        Catch ex As Exception
            Debug.Print("Problem cleaning up: " & ex.Message)
        End Try
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'register BASS.NET to suppress splash screen
        Un4seen.Bass.BassNet.Registration("smknight@gmail.com", "2X28221818152222")

        'init default output
        Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, Me.Handle)

        'init sound sample interval, etc
        Sound.Init()

        'populate the dropdown with recording sources
        Dim n As Integer = 0
        Dim info As New BASS_DEVICEINFO()
        While (Bass.BASS_RecordGetDeviceInfo(n, info))
            cmbSource.Items.Add(info.ToString)
            n += 1
        End While



    End Sub


    Private _myRecProc As RECORDPROC ' make it global, so that the GC can not remove it
    Private _byteswritten As Integer = 0
    Private _recbuffer() As Byte ' local recording buffer
    Private fs As FileStream
    Private _waveWriter As WaveWriter = Nothing ' make it global, so that the GC can not remove it



    Private Function MyRecording(handle As Integer, buffer As IntPtr, length As Integer, user As IntPtr) As Boolean
        Dim cont As Boolean = True
        If length > 0 AndAlso buffer <> IntPtr.Zero Then
            ' increase the rec buffer as needed
            If _recbuffer Is Nothing OrElse _recbuffer.Length < length Then
                _recbuffer = New Byte(length) {}
            End If
            ' copy from managed to unmanaged memory
            Marshal.Copy(buffer, _recbuffer, 0, length)
            _byteswritten += length
            ' write to file
            'fs.Write(_recbuffer, 0, length)
            _waveWriter.Write(buffer, length)


            ' stop recording after a certain amout (just to demo)
            If _byteswritten > 800000 Then
                cont = False ' stop recording
                'fs.Close()
                _waveWriter.Close()
            End If
        End If
        Return cont
    End Function

    Private Sub cmdStart_Click(sender As System.Object, e As System.EventArgs) Handles cmdStart.Click
        Light.Init(LeftLightSim, RightLightSim)
        Sound.StartRecord()

    End Sub

    Private Sub cmdStop_Click(sender As Object, e As System.EventArgs) Handles cmdStop.Click
        Sound.StopRecord()
        Light.DeInit()
    End Sub
End Class
