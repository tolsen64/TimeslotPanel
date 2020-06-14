Module modGlobals

    Public Function MilTo12(milTime As String) As String
        Dim newTime As String = ""
        Dim t As String() = milTime.Split(":"c)
        If t(0) = 0 Then
            newTime = "12:" & t(1) & " AM"
        ElseIf t(0) < 12 Then
            newTime = Join(t, ":") & " AM"
        ElseIf t(0) = 12 Then
            newTime = Join(t, ":") & " PM"
        Else
            newTime = t(0) - 12 & ":" & t(1) & " PM"
        End If
        Return newTime
    End Function


    '<Runtime.CompilerServices.Extension>
    'Public Function GetInvertedColor(b As Brush) As Brush
    '    Dim scb As SolidColorBrush = DirectCast(b, SolidColorBrush)
    '    Dim c As Color = GetInvertedColor(scb.Color)
    '    Return New SolidColorBrush(c)
    'End Function

    '<Runtime.CompilerServices.Extension>
    'Public Function ToMediaColor(c As System.Drawing.Color) As Color
    '    Return Color.FromArgb(c.A, c.R, c.G, c.B)
    'End Function

    '<Runtime.CompilerServices.Extension>
    'Public Function ToDrawingColor(c As Color) As System.Drawing.Color
    '    Return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B)
    'End Function

    '<Runtime.CompilerServices.Extension>
    'Public Function GetInvertedColor(c As Color) As Color
    '    Return GetInvertedColor(c.A, c.R, c.G, c.B)
    'End Function

    'Private Function GetInvertedColor(A As Integer, R As Integer, G As Integer, B As Integer) As Color
    '    Dim inv As Color = Color.FromArgb(A, 255 - R, 255 - G, 255 - B)
    '    Dim diff As Integer = 0
    '    diff += Math.Abs(R - inv.R)
    '    diff += Math.Abs(G - inv.G)
    '    diff += Math.Abs(B - inv.B)
    '    Dim factor As Double = diff / (255 * 3)
    '    inv = Color.FromArgb(A, inv.R * factor, inv.G * factor, inv.B * factor)
    '    Return inv
    'End Function

#Region "ColorExtensions"

    'Dim randomizer As New Random()

    '<System.Runtime.CompilerServices.Extension>
    'Public Function GetContrast(source As Brush, preserveOpacity As Boolean) As Brush
    '    Dim scb As SolidColorBrush = DirectCast(source, SolidColorBrush)
    '    Dim c As Color = GetContrast(scb.Color, preserveOpacity)
    '    Return New SolidColorBrush(c)
    'End Function

    '<System.Runtime.CompilerServices.Extension>
    'Public Function GetContrast(source As Color, preserveOpacity As Boolean) As Color
    '    Dim inputColor As Color = source
    '    'if RGB values are close to each other by a diff less than 10%, then if RGB values are lighter side, decrease the blue by 50% (eventually it will increase in conversion below), if RBB values are on darker side, decrease yellow by about 50% (it will increase in conversion)
    '    Dim avgColorValue As Byte = CByte((source.R + source.G + source.B) / 3)
    '    Dim diff_r As Integer = Math.Abs(source.R - avgColorValue)
    '    Dim diff_g As Integer = Math.Abs(source.G - avgColorValue)
    '    Dim diff_b As Integer = Math.Abs(source.B - avgColorValue)
    '    If diff_r < 20 AndAlso diff_g < 20 AndAlso diff_b < 20 Then
    '        'The color is a shade of gray
    '        If avgColorValue < 123 Then
    '            'color is dark
    '            inputColor.B = 220
    '            inputColor.G = 230
    '            inputColor.R = 50
    '        Else
    '            inputColor.R = 255
    '            inputColor.G = 255
    '            inputColor.B = 50
    '        End If
    '    End If
    '    Dim sourceAlphaValue As Byte = source.A
    '    If Not preserveOpacity Then
    '        'We don't want contrast color to be more than 50% transparent ever.
    '        sourceAlphaValue = Math.Max(source.A, CByte(127))
    '    End If
    '    Dim rgb As New RGB() With {
    '            .R = inputColor.R,
    '            .G = inputColor.G,
    '            .B = inputColor.B
    '        }
    '    Dim hsb As HSB = ConvertToHSB(rgb)
    '    hsb.H = If(hsb.H < 180, hsb.H + 180, hsb.H - 180)
    '    'hsb.B = isColorDark ? 240 : 50; //Added to create dark on light, and light on dark
    '    rgb = ConvertToRGB(hsb)
    '    Return New Color() With {
    '            .A = sourceAlphaValue,
    '            .R = rgb.R,
    '            .G = CByte(Math.Truncate(rgb.G)),
    '            .B = CByte(Math.Truncate(rgb.B))
    '        }
    'End Function

    'Friend Function ConvertToRGB(hsb As HSB) As RGB
    '    ' By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
    '    Dim chroma As Double = hsb.S * hsb.B
    '    Dim hue2 As Double = hsb.H / 60
    '    Dim x As Double = chroma * (1 - Math.Abs(hue2 Mod 2 - 1))
    '    Dim r1 As Double = 0.0
    '    Dim g1 As Double = 0.0
    '    Dim b1 As Double = 0.0
    '    If hue2 >= 0 AndAlso hue2 < 1 Then
    '        r1 = chroma
    '        g1 = x
    '    ElseIf hue2 >= 1 AndAlso hue2 < 2 Then
    '        r1 = x
    '        g1 = chroma
    '    ElseIf hue2 >= 2 AndAlso hue2 < 3 Then
    '        g1 = chroma
    '        b1 = x
    '    ElseIf hue2 >= 3 AndAlso hue2 < 4 Then
    '        g1 = x
    '        b1 = chroma
    '    ElseIf hue2 >= 4 AndAlso hue2 < 5 Then
    '        r1 = x
    '        b1 = chroma
    '    ElseIf hue2 >= 5 AndAlso hue2 <= 6 Then
    '        r1 = chroma
    '        b1 = x
    '    End If
    '    Dim m As Double = hsb.B - chroma
    '    Return New RGB() With {
    '            .R = r1 + m,
    '            .G = g1 + m,
    '            .B = b1 + m
    '        }
    'End Function

    'Friend Function ConvertToHSB(rgb As RGB) As HSB
    '    ' By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
    '    Dim r As Double = rgb.R
    '    Dim g As Double = rgb.G
    '    Dim b As Double = rgb.B

    '    Dim max__1 As Double = Max(r, g, b)
    '    Dim min__2 As Double = Min(r, g, b)
    '    Dim chroma As Double = max__1 - min__2
    '    Dim hue2 As Double = 0.0
    '    If chroma <> 0 Then
    '        If max__1 = r Then
    '            hue2 = (g - b) / chroma
    '        ElseIf max__1 = g Then
    '            hue2 = (b - r) / chroma + 2
    '        Else
    '            hue2 = (r - g) / chroma + 4
    '        End If
    '    End If
    '    Dim hue As Double = hue2 * 60
    '    If hue < 0 Then
    '        hue += 360
    '    End If
    '    Dim brightness As Double = max__1
    '    Dim saturation As Double = 0
    '    If chroma <> 0 Then
    '        saturation = chroma / brightness
    '    End If
    '    Return New HSB() With {
    '            .H = hue,
    '            .S = saturation,
    '            .B = brightness
    '        }
    'End Function

    'Private Function Max(d1 As Double, d2 As Double, d3 As Double) As Double
    '    If d1 > d2 Then
    '        Return Math.Max(d1, d3)
    '    End If
    '    Return Math.Max(d2, d3)
    'End Function

    'Private Function Min(d1 As Double, d2 As Double, d3 As Double) As Double
    '    If d1 < d2 Then
    '        Return Math.Min(d1, d3)
    '    End If
    '    Return Math.Min(d2, d3)
    'End Function

    'Friend Structure RGB
    '    Friend R As Double
    '    Friend G As Double
    '    Friend B As Double
    'End Structure

    'Friend Structure HSB
    '    Friend H As Double
    '    Friend S As Double
    '    Friend B As Double
    'End Structure

#End Region

End Module
