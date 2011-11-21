<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAmbxTest
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.trackRedLeft = New System.Windows.Forms.TrackBar
        Me.trackGreenLeft = New System.Windows.Forms.TrackBar
        Me.trackBlueLeft = New System.Windows.Forms.TrackBar
        Me.trackFanLeft = New System.Windows.Forms.TrackBar
        Me.cboRumbleName = New System.Windows.Forms.ComboBox
        Me.btnRumbleStop = New System.Windows.Forms.Button
        Me.btnStopFan = New System.Windows.Forms.Button
        Me.cboEvents = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.cboMovies = New System.Windows.Forms.ComboBox
        Me.btnPauseMovie = New System.Windows.Forms.Button
        CType(Me.trackRedLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trackGreenLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trackBlueLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trackFanLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'trackRedLeft
        '
        Me.trackRedLeft.Location = New System.Drawing.Point(127, 12)
        Me.trackRedLeft.Maximum = 255
        Me.trackRedLeft.Name = "trackRedLeft"
        Me.trackRedLeft.Size = New System.Drawing.Size(155, 45)
        Me.trackRedLeft.TabIndex = 0
        '
        'trackGreenLeft
        '
        Me.trackGreenLeft.Location = New System.Drawing.Point(127, 81)
        Me.trackGreenLeft.Maximum = 255
        Me.trackGreenLeft.Name = "trackGreenLeft"
        Me.trackGreenLeft.Size = New System.Drawing.Size(155, 45)
        Me.trackGreenLeft.TabIndex = 1
        '
        'trackBlueLeft
        '
        Me.trackBlueLeft.Location = New System.Drawing.Point(127, 46)
        Me.trackBlueLeft.Maximum = 255
        Me.trackBlueLeft.Name = "trackBlueLeft"
        Me.trackBlueLeft.Size = New System.Drawing.Size(155, 45)
        Me.trackBlueLeft.TabIndex = 2
        '
        'trackFanLeft
        '
        Me.trackFanLeft.Location = New System.Drawing.Point(127, 171)
        Me.trackFanLeft.Maximum = 255
        Me.trackFanLeft.Name = "trackFanLeft"
        Me.trackFanLeft.Size = New System.Drawing.Size(155, 45)
        Me.trackFanLeft.TabIndex = 3
        '
        'cboRumbleName
        '
        Me.cboRumbleName.FormattingEnabled = True
        Me.cboRumbleName.Items.AddRange(New Object() {"amBX_boing", "amBX_crash", "amBX_engine", "amBX_explosion", "amBX_hit", "amBX_quake", "amBX_rattle", "amBX_road", "amBX_shot", "amBX_thud", "amBX_thunder"})
        Me.cboRumbleName.Location = New System.Drawing.Point(127, 222)
        Me.cboRumbleName.Name = "cboRumbleName"
        Me.cboRumbleName.Size = New System.Drawing.Size(155, 21)
        Me.cboRumbleName.TabIndex = 4
        '
        'btnRumbleStop
        '
        Me.btnRumbleStop.Location = New System.Drawing.Point(300, 220)
        Me.btnRumbleStop.Name = "btnRumbleStop"
        Me.btnRumbleStop.Size = New System.Drawing.Size(75, 23)
        Me.btnRumbleStop.TabIndex = 5
        Me.btnRumbleStop.Text = "Stop"
        Me.btnRumbleStop.UseVisualStyleBackColor = True
        '
        'btnStopFan
        '
        Me.btnStopFan.Location = New System.Drawing.Point(300, 171)
        Me.btnStopFan.Name = "btnStopFan"
        Me.btnStopFan.Size = New System.Drawing.Size(75, 23)
        Me.btnStopFan.TabIndex = 6
        Me.btnStopFan.Text = "Stop"
        Me.btnStopFan.UseVisualStyleBackColor = True
        '
        'cboEvents
        '
        Me.cboEvents.FormattingEnabled = True
        Me.cboEvents.Location = New System.Drawing.Point(127, 263)
        Me.cboEvents.Name = "cboEvents"
        Me.cboEvents.Size = New System.Drawing.Size(155, 21)
        Me.cboEvents.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Light Color"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(76, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(15, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "R"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(76, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(14, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "B"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(76, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(15, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "G"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 171)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Fan Speed"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 225)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(98, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Rumbler Waveform"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 266)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(54, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Effect File"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 293)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(55, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Movie File"
        '
        'cboMovies
        '
        Me.cboMovies.FormattingEnabled = True
        Me.cboMovies.Location = New System.Drawing.Point(127, 290)
        Me.cboMovies.Name = "cboMovies"
        Me.cboMovies.Size = New System.Drawing.Size(155, 21)
        Me.cboMovies.TabIndex = 15
        '
        'btnPauseMovie
        '
        Me.btnPauseMovie.Location = New System.Drawing.Point(300, 290)
        Me.btnPauseMovie.Name = "btnPauseMovie"
        Me.btnPauseMovie.Size = New System.Drawing.Size(75, 23)
        Me.btnPauseMovie.TabIndex = 17
        Me.btnPauseMovie.Text = "Pause"
        Me.btnPauseMovie.UseVisualStyleBackColor = True
        '
        'frmAmbxTest2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(501, 343)
        Me.Controls.Add(Me.btnPauseMovie)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cboMovies)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboEvents)
        Me.Controls.Add(Me.btnStopFan)
        Me.Controls.Add(Me.btnRumbleStop)
        Me.Controls.Add(Me.cboRumbleName)
        Me.Controls.Add(Me.trackFanLeft)
        Me.Controls.Add(Me.trackGreenLeft)
        Me.Controls.Add(Me.trackBlueLeft)
        Me.Controls.Add(Me.trackRedLeft)
        Me.Name = "frmAmbxTest2"
        Me.Text = "frmAmbxTest"
        CType(Me.trackRedLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trackGreenLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trackBlueLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trackFanLeft, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents trackRedLeft As System.Windows.Forms.TrackBar
    Friend WithEvents trackGreenLeft As System.Windows.Forms.TrackBar
    Friend WithEvents trackBlueLeft As System.Windows.Forms.TrackBar
    Friend WithEvents trackFanLeft As System.Windows.Forms.TrackBar
    Friend WithEvents cboRumbleName As System.Windows.Forms.ComboBox
    Friend WithEvents btnRumbleStop As System.Windows.Forms.Button
    Friend WithEvents btnStopFan As System.Windows.Forms.Button
    Friend WithEvents cboEvents As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cboMovies As System.Windows.Forms.ComboBox
    Friend WithEvents btnPauseMovie As System.Windows.Forms.Button
End Class
