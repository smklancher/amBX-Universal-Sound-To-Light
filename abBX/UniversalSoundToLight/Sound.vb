Imports Un4seen.Bass
Imports Un4seen.Bass.Utils
Imports System.Runtime.InteropServices

Public Class Sound
    Public Shared Source As Integer = -1
    Private Shared recHandle As Integer

    Public Shared Sub Init(MainWindow As System.IntPtr)
        'register BASS.NET to suppress splash screen
        Un4seen.Bass.BassNet.Registration("smknight@gmail.com", "2X28221818152222")

        'init default output
        Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, MainWindow)

    End Sub


    Public Shared Sub StartRecord()
        If Bass.BASS_RecordInit(Source) Then
            'create the paused recording channel
            recHandle = Bass.BASS_RecordStart(44100, 2, BASSFlag.BASS_RECORD_PAUSE, Nothing, IntPtr.Zero)

            ' start recording
            Bass.BASS_ChannelPlay(recHandle, False)

            'MessageBox.Show(String.Join(", ", Bass.BASS_RecordGetInputNames()))
        End If
    End Sub

    Public Shared Sub StopRecord()
        Bass.BASS_RecordFree()
    End Sub

    Private Shared Sub ResetRecord()
        'stop, clearing buffer
        Bass.BASS_ChannelStop(recHandle)

        'create the paused recording channel
        recHandle = Bass.BASS_RecordStart(44100, 2, BASSFlag.BASS_DEFAULT, Nothing, IntPtr.Zero)
    End Sub

    Public Shared Sub Sound_Tick()

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

        Dim BassEnergy As Double = Sound.FreqencyEnergy(60, 250)


        Dim BassLight As Color = Light.ColorFromEnergy(BassEnergy, 0, 0, 1)
        'Light.SetColors(BassLight, BassLight)
        'Light.Update()

        Log("Base: " & BassEnergy)



        Bass.BASS_ChannelSetPosition(recHandle, 0)



        Dim MidEnergy As Double = Sound.FreqencyEnergy(250, 2000)


        Dim MidLight As Color = Light.ColorFromEnergy(MidEnergy, 0, 1, 0)
        'Light.SetColors(BassLight, BassLight)
        'Light.Update()

        Log("Midrange: " & MidEnergy)



        Bass.BASS_ChannelSetPosition(recHandle, 0)


        Log("Midrange: " & vis.DetectFrequency(recHandle, 250, 2000, False))
        Bass.BASS_ChannelSetPosition(recHandle, 0)
        Log("High Mids 1: " & vis.DetectFrequency(recHandle, 2000, 4000, False))
        Bass.BASS_ChannelSetPosition(recHandle, 0)
        Log("High Mids 2: " & vis.DetectFrequency(recHandle, 4000, 6000, False))
        Bass.BASS_ChannelSetPosition(recHandle, 0)
        Log("High: " & vis.DetectFrequency(recHandle, 6000, 20000, False))

        Log("")


        Light.SetColors(Light.BlendColorAdd(BassLight, MidLight), Light.BlendColorAvg(BassLight, MidLight))
        Light.Update()



        ResetRecord()

    End Sub


    ''' <summary>
    ''' Returns the energy(?) (0.0-1.0) of the bounded freqencies of sound in Hz.
    ''' </summary>
    ''' <param name="Low">The low.</param>
    ''' <param name="High">The high.</param><returns>energy(?) (0.0-1.0)</returns>
    Public Shared Function FreqencyEnergy(Low As Integer, High As Integer) As Double
        Dim vis As New Misc.Visuals()
        Return vis.DetectFrequency(recHandle, Low, High, False)
    End Function


    Private Shared Sub Log(msg As String)
        Form1.txtLog.Text = msg & vbNewLine & Form1.txtLog.Text
    End Sub
End Class
