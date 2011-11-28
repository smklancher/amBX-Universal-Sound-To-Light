Imports System.Drawing

Public Class ColorFreqency
    Public Property Color As Color

    Public Property LowHz As Integer
    Public Property HighHz As Integer

    Public Sub New(TheColor As Color, TheLowHz As Integer, TheHighHz As Integer)
        Color = TheColor
        LowHz = TheLowHz
        HighHz = TheHighHz

    End Sub


End Class
