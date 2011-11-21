Imports Un4seen.Bass
Imports Un4seen.Bass.Utils
Imports System.Runtime.InteropServices

Public Class Sound
    Public Shared Source As Integer = -1
    Public Shared Property Interval As Integer
        Set(value As Integer)
            SampleTimer.Interval = value
        End Set
        Get
            Return SampleTimer.Interval
        End Get
    End Property


    Private Shared WithEvents SampleTimer As New Timer
    Private Shared recHandle As Integer

    Public Shared Sub Init()
        Interval = 100
    End Sub


    Public Shared Sub StartRecord()
        If Bass.BASS_RecordInit(Source) Then
            'create the paused recording channel
            recHandle = Bass.BASS_RecordStart(44100, 2, BASSFlag.BASS_RECORD_PAUSE, Nothing, IntPtr.Zero)

            ' start recording
            Bass.BASS_ChannelPlay(recHandle, False)

            'MessageBox.Show(String.Join(", ", Bass.BASS_RecordGetInputNames()))
            SampleTimer.Enabled = True
        End If
    End Sub

    Public Shared Sub StopRecord()
        SampleTimer.Enabled = False
        Bass.BASS_RecordFree()
    End Sub

    Private Shared Sub ResetRecord()
        'stop, clearing buffer
        Bass.BASS_ChannelStop(recHandle)

        'create the paused recording channel
        recHandle = Bass.BASS_RecordStart(44100, 2, BASSFlag.BASS_DEFAULT, Nothing, IntPtr.Zero)
    End Sub

    Private Shared Sub SampleTimer_Tick(sender As Object, e As System.EventArgs) Handles SampleTimer.Tick

        'volume levels
        Dim level As Integer
        level = Bass.BASS_ChannelGetLevel(recHandle)
        Log("Left: " & LowWord32(level) & ", " & "right: " & HighWord32(level))
        Log("Left: " & Utils.LevelToDB(LowWord32(level), 32768) & "dB, " & "right: " & Utils.LevelToDB(HighWord32(level), 32768) & "dB")

        'get channel info
        Dim info As BASS_CHANNELINFO
        info = Bass.BASS_ChannelGetInfo(recHandle)
        Log(info.ToString())

        ' length in bytes
        Dim len As Long = Bass.BASS_ChannelGetLength(recHandle)
        ' the time length
        Dim time As Double = Bass.BASS_ChannelBytes2Seconds(recHandle, len)
        Log(len & " bytes, " & time & " seconds")

        'position
        Dim pos As Long = Bass.BASS_ChannelGetPosition(recHandle)
        Log("Pos: " & pos)
        Log("Data Available: " & Bass.BASS_ChannelGetData(recHandle, New Byte(), BASSData.BASS_DATA_AVAILABLE))

        'peak frequency
        Dim energy As Single = 0
        Dim freqency As Integer = 0
        Dim vis As New Misc.Visuals()
        freqency = vis.DetectPeakFrequency(recHandle, energy)
        Log("Peak: " & freqency & "Hz, energy: " & energy)

        'ranges
        'Log("Vocal freqency amp: " & vis.DetectFrequency(recHandle, 120, 900, False))
        Bass.BASS_ChannelSetPosition(recHandle, 0)
        Log("Sub Base: " & vis.DetectFrequency(recHandle, 16, 60, False))

        Bass.BASS_ChannelSetPosition(recHandle, 0)

        Dim BassEnergy As Double = vis.DetectFrequency(recHandle, 60, 250, False)

        Dim HowRed As Integer = 0
        HowRed = Math.Min(BassEnergy * 255 * 2, 255)

        Light.SetColors(Color.FromArgb(HowRed, 0, 0), Color.FromArgb(HowRed, 0, 0))


        Log("Base: " & BassEnergy)



        Bass.BASS_ChannelSetPosition(recHandle, 0)


        Log("Midrange: " & vis.DetectFrequency(recHandle, 250, 2000, False))
        Bass.BASS_ChannelSetPosition(recHandle, 0)
        Log("High Mids 1: " & vis.DetectFrequency(recHandle, 2000, 4000, False))
        Bass.BASS_ChannelSetPosition(recHandle, 0)
        Log("High Mids 2: " & vis.DetectFrequency(recHandle, 4000, 6000, False))
        Bass.BASS_ChannelSetPosition(recHandle, 0)
        Log("High: " & vis.DetectFrequency(recHandle, 6000, 20000, False))

        Log("")

        ResetRecord()

    End Sub

    Private Shared Sub Log(msg As String)
        Form1.txtLog.Text = msg & vbNewLine & Form1.txtLog.Text
    End Sub
End Class
