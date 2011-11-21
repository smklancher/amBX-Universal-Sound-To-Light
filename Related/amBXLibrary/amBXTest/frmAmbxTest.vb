Imports amBXTest.amBXLibrary
Imports System.Runtime.InteropServices


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' amBXLibrary - http://www.vbfengshui.com
' Copyright (c) 2010 
' by Darin Higgins
' Portions of this code converted to VB.net from the original C amBX SDK files
' available at http://developer.ambx.com/
'
' Allows VB.net code to easily communicate with the amBX lighting and effects system
' by Phillips. You will need the amBX drivers installed. And it would be very beneficial 
' to have the amBX SDK installed. Both are readily available via www.ambx.com
'
' Original version available at www.vbfengshui.com
'
' This software is licensed under the Microsoft Reciprocal License (Ms-RL).
'
' Please see http://www.opensource.org/licenses/ms-rl.html for details
'
' If you wish to use this code in a commercial application, please let me hear about it!
' If you wish to modify it, improve it or correct bugs, please let me know as well.
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Public Class frmAmbxTest

    '---- track a few private ambx objects here
    Private LeftLight As amBX.Light
    Private RightLight As amBX.Light
    Private LeftFan As amBX.Fan
    Private Rumble As amBX.Rumble
    Private Movie As amBX.Movie



    Private Sub frmAmbxTest2_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        '---- disconnect the amBX drivers
        amBX.Disconnect()
    End Sub


    Private Sub frmAmbxTest_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            '---- connect to amBX
            amBX.Connect(1, 0, "amBXTest", "1.2.0")
            If Not amBX.IsConnected Then
                End
            End If

            '---- good to go, should be able to use the IamBX object now
            '     create some objects
            LeftLight = amBX.Lights.Add("Left", amBXLibrary.Locations.NorthEast, amBXLibrary.Heights.AnyHeight)
            RightLight = New amBX.Light(amBXLibrary.Locations.NorthWest, amBXLibrary.Heights.AnyHeight)
            
            LeftLight.Enabled = True
            amBX.Lights.Item("Left").Enabled = True
            RightLight.Enabled = True

            LeftFan = New amBX.Fan(amBXLibrary.Locations.Center, amBXLibrary.Heights.AnyHeight)

            Rumble = New amBX.Rumble(amBXLibrary.Locations.North, amBXLibrary.Heights.AnyHeight)

            'amBX.Update()

            '---- retrieve the ambx files in the current dir and populate the drop down boxes
            Dim dr = New System.IO.DirectoryInfo(".\")
            Dim Files = dr.GetFiles("*.amBX_bn")
            For Each file In Files
                cboEvents.Items.Add(file)
                cboMovies.Items.Add(file)
            Next

            '---- connect event handlers for the light color trackbars
            AddHandler trackBlueLeft.Scroll, AddressOf Me.HandleScroll
            AddHandler trackGreenLeft.Scroll, AddressOf Me.HandleScroll
            AddHandler trackRedLeft.Scroll, AddressOf Me.HandleScroll

        Catch ex As amBXExceptionamBXrtdllNotFound
            MsgBox("You'll need to get a copy of the ambxrt.dll file and place it in this test application's folder")
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub


    Private Sub HandleScroll(ByVal sender As Object, ByVal e As System.EventArgs)
        '---- as a trackbar changes, change the color of the lights
        LeftLight.Color = Color.FromArgb(trackRedLeft.Value, trackGreenLeft.Value, trackBlueLeft.Value)
        RightLight.Color = Color.FromArgb(trackRedLeft.Value, trackGreenLeft.Value, trackBlueLeft.Value)


    End Sub


    Private Sub trackFanLeft_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles trackFanLeft.Scroll
        '---- as the trackbar changes, change the fan intensity.
        LeftFan.Intensity = trackFanLeft.Value / 255
    End Sub


    Private Sub cboRumbleName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRumbleName.SelectedIndexChanged
        Rumble.SetRumble(cboRumbleName.SelectedItem.ToString, 1, 1)
    End Sub


    Private Sub btnRumbleStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRumbleStop.Click
        Rumble.Disable()
    End Sub


    Private Sub btnStopFan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopFan.Click
        If LeftFan.Enabled = amBXLibrary.amBX_EnabledStates.ENABLED Then
            LeftFan.Disable()
        Else
            LeftFan.Enable()
        End If
    End Sub


    Private Sub cboEvents_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboEvents.SelectedIndexChanged
        Dim f = DirectCast(cboEvents.SelectedItem, System.IO.FileInfo)

        Static EffectEvent As amBX.Event
        If EffectEvent IsNot Nothing Then EffectEvent.Dispose()
        EffectEvent = New amBX.Event(f.FullName)
        EffectEvent.Play()
    End Sub


    Private Sub cboMovies_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMovies.SelectedIndexChanged
        Dim f = DirectCast(cboMovies.SelectedItem, System.IO.FileInfo)

        If Movie IsNot Nothing Then Movie.Dispose()
        Movie = New amBX.Movie(f.FullName)
        btnPauseMovie.Text = "Pause"
        Movie.Play()
    End Sub


    Private Sub btnPauseMovie_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPauseMovie.Click
        If Movie IsNot Nothing Then
            btnPauseMovie.Text = If(Movie.Frozen, "Pause", "Play")
            Movie.Frozen = Not Movie.Frozen
        End If
    End Sub
End Class