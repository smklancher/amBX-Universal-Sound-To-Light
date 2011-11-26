Imports UniversalSoundToLight.amBXLibrary

Public Class Light
    Private Shared LeftLight As amBX.Light
    Private Shared RightLight As amBX.Light

    Public Shared LeftLightSim As PictureBox
    Public Shared RightLightSim As PictureBox

    Public Shared SimOnly As Boolean = True


    Public Shared Sub Init(Optional LeftSim As PictureBox = Nothing, Optional RightSim As PictureBox = Nothing)
        'set pictureboxes to use for similated lights
        LeftLightSim = LeftSim
        RightLightSim = RightSim


        Try
            'connect to the drivers
            amBX.Connect(1, 0, Application.ProductName, Application.ProductVersion)

            
            'set up lights on each side
            LeftLight = amBX.Lights.Add("Left", Locations.NorthEast Or Locations.East Or Locations.SouthEast, amBXLibrary.Heights.AnyHeight)
            RightLight = amBX.Lights.Add("Right", Locations.NorthWest Or Locations.West Or Locations.SouthWest, amBXLibrary.Heights.AnyHeight)

            'the focus is for one light on each side, but should I add something for center, north, south?

            'if connect didn't error, then it isn't just a simulation
            SimOnly = False

        Catch ex As amBXExceptionamBXrtdllNotFound
            MsgBox("Ensure that ambxrt.dll is in the same folder as this application.")
        Catch ex As amBXExceptionNotInstalled
            MsgBox("amBX drivers not installed, lights will be simulation only.")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Shared Sub SetColors(LeftColor As System.Drawing.Color, RightColor As System.Drawing.Color)
        If SimOnly = False Then
            LeftLight.Color = LeftColor
            RightLight.Color = RightColor
        End If
        
        If LeftLightSim IsNot Nothing Then
            LeftLightSim.BackColor = LeftColor
            RightLightSim.BackColor = RightColor
        End If
    End Sub

    Public Shared Sub DeInit()
        'disconnect from drivers
        amBX.Disconnect()
    End Sub

End Class
