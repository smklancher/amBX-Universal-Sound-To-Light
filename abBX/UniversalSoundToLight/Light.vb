Imports UniversalSoundToLight.amBXLibrary

Public Class Light
    Private Shared LeftLight As amBX.Light
    Private Shared RightLight As amBX.Light

    Public Shared LeftLightSim As PictureBox
    Public Shared RightLightSim As PictureBox

    Public Shared UsingRealLights As Boolean = False


    Public Shared Sub Init(Optional LeftSim As PictureBox = Nothing, Optional RightSim As PictureBox = Nothing)
        'set pictureboxes to use for similated lights
        LeftLightSim = LeftSim
        RightLightSim = RightSim

        'It's only a simulation unless there are no errors connecting to the driver
        UsingRealLights = False

        Try
            'connect to the drivers
            amBX.ConnectThreading(1, 0, Application.ProductName, Application.ProductVersion)

            
            'set up lights on each side
            LeftLight = amBX.Lights.Add("Left", Locations.NorthWest Or Locations.West Or Locations.SouthWest, amBXLibrary.Heights.AnyHeight)
            RightLight = amBX.Lights.Add("Right", Locations.NorthEast Or Locations.East Or Locations.SouthEast, amBXLibrary.Heights.AnyHeight)

            'Update the lights no matter how small or how often the change.  TODO: Make option for performance
            LeftLight.LightDelta = 0.02
            RightLight.LightDelta = 0.02
            LeftLight.LightUpdateIntervalMS = 100
            RightLight.LightUpdateIntervalMS = 100

            'the focus is for one light on each side, but should I add something for center, north, south?

            'if connect didn't error, then it isn't just a simulation
            UsingRealLights = True

        Catch ex As amBXExceptionamBXrtdllNotFound
            MsgBox("Ensure that ambxrt.dll is in the same folder as this application.")
        Catch ex As amBXExceptionNotInstalled
            MsgBox("amBX drivers not installed, lights will be simulation only.")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Shared Sub SetColors(LeftColor As Color, RightColor As Color)
        If UsingRealLights Then
            LeftLight.Color = LeftColor
            RightLight.Color = RightColor
        End If

        If LeftLightSim IsNot Nothing Then
            LeftLightSim.BackColor = LeftColor
        End If

        If RightLightSim IsNot Nothing Then
            RightLightSim.BackColor = RightColor
        End If
    End Sub


    Public Shared Function BlendColorAdd(c1 As Color, c2 As Color) As Color
        Return Color.FromArgb(Math.Min(c1.R + c2.R, 255), Math.Min(c1.G + c2.G, 255), Math.Min(c1.B + c2.B, 255))
    End Function

    Public Shared Function BlendColorAvg(c1 As Color, c2 As Color) As Color
        Return Color.FromArgb((c1.R + c2.R) / 2, (c1.G + c2.G) / 2, (c1.B + c2.B) / 2)
    End Function



    Public Shared Function ColorFromEnergy(Energy As Double, RedPercent As Double, GreenPercent As Double, BluePercent As Double) As Color
        Dim HowRed As Integer = 0
        HowRed = Math.Min(Energy * 255 * 2, 255) * RedPercent

        Dim HowGreen As Integer = 0
        HowGreen = Math.Min(Energy * 255 * 2, 255) * GreenPercent

        Dim HowBlue As Integer = 0
        HowBlue = Math.Min(Energy * 255 * 2, 255) * BluePercent

        Return Color.FromArgb(HowRed, HowGreen, HowBlue)
    End Function


    Public Shared Sub Update()
        If UsingRealLights Then
            Try
                amBX.Update(100)
            Catch ex As amBXExceptionUpdateTimeout
                Debug.Print("Update thread timed out.  Skipping update at " & Now.ToFileTime() & ": " & ex.Message)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub



    Public Shared Sub DeInit()
        'disconnect from drivers
        amBX.Disconnect()
    End Sub

End Class
