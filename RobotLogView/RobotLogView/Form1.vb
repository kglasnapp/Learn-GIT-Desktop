Option Explicit On
Option Strict On

Imports System
Imports System.Text
Imports System.IO
Imports System.Data
Imports Microsoft.VisualBasic

Public Class Form1
  Public DirSep As Char = IO.Path.DirectorySeparatorChar
  Public AppPath As String = My.Application.Info.DirectoryPath & DirSep
  Public AppName As String = IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath)

  Private Sub OpenLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenLogToolStripMenuItem.Click
    Dim ar() As String = OpenMultiFileDialog("logs", "logs (*.dsevents)|*.dsevents|All Files(*.*)|*.*")
    For Each fn As String In ar
      DisplayLog(fn)
    Next
  End Sub

  ' Routine to display a string as a number of lines
  Function Dump16(ByVal s As String) As String
    Dim res As String = vbCrLf
    If s.Length = 0 Then Return "No Data"
    For i As Integer = 1 To s.Length Step 16
      If i + 16 < s.Length Then
        res &= Microsoft.VisualBasic.Right("0000" & Hex(i - 1), 4) & ": " & Dump(Mid(s, i, 16)) & vbCrLf
      Else
        res &= Microsoft.VisualBasic.Right("0000" & Hex(i - 1), 4) & ": " & Dump(Mid(s, i), True) & vbCrLf
      End If
    Next
    Return res
  End Function

  ' Routine to convert a String to Hex characters
  Function Dump(ByVal s As String, Optional ByVal PAD As Boolean = False) As String
    Dim i, c As Integer
    Dim u As String = ""
    Dump = ""
    For i = 1 To Len(s)
      Dump = Dump & Microsoft.VisualBasic.Right("0" & Hex(Asc(Mid(s, i, 1))), 2) & " "
      c = (c + Asc(Mid(s, i, 1))) And 255
      If Mid(s, i, 1) < " " Or Mid(s, i, 1) > Chr(&H7FS) Then
        u = u & "."
      Else
        u = u & Mid(s, i, 1)
      End If
    Next i
    If PAD And Dump.Length < 48 Then Dump &= Space(16 * 3 - Dump.Length)
    Dump = Dump & u
  End Function

  ' Routine to convert a String to Hex characters
  Function DumpAr(ByVal ar() As Byte, ByVal Start As Integer, ByVal Length As Integer, Optional ByVal IncludeText As Boolean = True) As String
    Dim i As Integer
    Dim u As String = ""
    Dim cnt As Integer = 0
    DumpAr = ""
    Try
      For i = Start To Start + Length - 1
        DumpAr = DumpAr & Microsoft.VisualBasic.Right("0" & Hex(ar(i)), 2) & " "
        If ar(i) < Asc(" ") Or ar(i) >= 128 Then
          u = u & "."
        Else
          u = u & Chr(ar(i))
        End If
        If (cnt Mod 16) = 15 Then DumpAr &= " " & u & vbCrLf : u = ""
        cnt += 1
      Next i
      If IncludeText Then DumpAr = DumpAr & u
    Catch e As Exception
      log("DumpAR -> Unable to display entire ar res=" & DumpAr)
      Return DumpAr
    End Try
  End Function

  Function DumpStr(ByVal s As String) As String
    Dim i, c As Integer
    Dim u As String = ""
    DumpStr = ""
    For i = 1 To Len(s)
      c = (c + Asc(Mid(s, i, 1))) And 255
      If Mid(s, i, 1) < " " Or Mid(s, i, 1) > Chr(&H7FS) Then
        u = u & "."
      Else
        u = u & Mid(s, i, 1)
      End If
    Next i
    DumpStr = DumpStr & u
  End Function

  Function Hex2(ByVal i As Integer) As String
    Return Microsoft.VisualBasic.Right("00" & Hex(i), 2)
  End Function

  Function Hex4(ByVal i As Integer) As String
    Return Microsoft.VisualBasic.Right("0000" & Hex(i), 4)
  End Function

  Function HextoByte(ByVal s As String) As Byte
    Dim l As Long = HextoLong(s)
    If l < 256 Then Return CByte(l)
    Return 0
  End Function

  Function HextoLong(ByVal s As String) As Long
    Dim Result As Long = 0
    For Each c As Char In s.ToCharArray
      If c >= "0" And c <= "9" Then Result = Result * 16 + Val(c) : Continue For
      If c >= "A" And c <= "F" Then Result = Result * 16 + Asc(c) - Asc("A") + 10 : Continue For
      If c >= "a" And c <= "f" Then Result = Result * 16 + Asc(c) - Asc("a") + 10 : Continue For
    Next
    Return Result
  End Function

  Function GetSubstr(ByVal s As String, ByVal StartStr As String) As String
    Dim i As Integer = InStr(s, StartStr)
    If i = 0 Then Return ""
    i += StartStr.Length
    Return Mid(s, i)
  End Function

  Function GetSubstr(ByVal s As String, ByVal StartStr As String, ByVal EndStr As String) As String
    Dim i As Integer = InStr(s, StartStr)
    If i = 0 Then Return ""
    i += StartStr.Length
    If EndStr.Equals("") Then Return Mid(s, i)
    Dim j As Integer = InStr(i, s, EndStr)
    If j = 0 Then Return ""
    Return Mid(s, i, j - i)
  End Function

  Function isText(ByVal ar() As Byte, ByVal Start As Integer, ByVal Length As Integer) As Boolean
    If ar.Length <= Start + Length Then Return False
    For i As Integer = Start To Start + Length - 1
      If ar(i) < Asc(" ") Or ar(i) > Asc("z") Then Return False
    Next
    Return True
  End Function

  Function SplitCSV(ByVal s As String) As String()
    Dim i As Integer = InStr(s, """")
    If i <= 0 Then Return s.Split(","c)
    Dim u As String = ""
    Dim quote As Boolean = False
    Dim result As New List(Of String)
    For Each c As Char In s.Split
      Select Case c
        Case """"c
          quote = Not quote
        Case ","c
          If quote Then Continue For
          result.Add(u)
          u = ""
        Case Else
          u &= c
      End Select
    Next
    Return result.ToArray
  End Function

  Function OpenMultiFileDialog(ByVal Key As String, Optional ByVal Filter As String = "log files (*.txt)|*.txt|All files (*.*)|*.*") As String()
    With New OpenFileDialog()
      If Key.Equals("") Then
        .InitialDirectory = AppPath
        Key = "APPPath"
      Else
        .InitialDirectory = GetSetting(AppName, "Directory", Key, AppPath)
      End If
      .Title = "Open files for " & Replace(Key, "SEL", "") & " initial Directory --> " & .InitialDirectory
      If Not Directory.Exists(.InitialDirectory) Then .InitialDirectory = AppPath
      .Filter = Filter
      .FilterIndex = 1
      .Multiselect = True
      .FileName = ""
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        If .FileNames.Length = 0 Then Return Split("")
        Dim ar(.FileNames.Length - 1) As String
        Array.Copy(.FileNames, ar, .FileNames.Length)
        Array.Sort(ar)
        SaveSetting(AppName, "Directory", Key, Path.GetDirectoryName(.FileNames(0)))
        Return ar
      Else
        Return InitArray()
      End If
    End With
  End Function

  Function InitArray(ByVal ParamArray Values() As String) As String()
    Dim V As New List(Of String)
    For Each s As String In Values
      V.Add(s)
    Next
    Return V.ToArray
  End Function

  Sub log(s As String)
    ListBox1.Items.Add(s)
  End Sub

  Dim ignores() As String = {}

  Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
    log("Start of Robot Log Display")
    If (IO.File.Exists("ignores.txt")) Then
      ignores = IO.File.ReadAllLines("ignores.txt")
    End If
  End Sub

  Sub DisplayLog(fn As String)
    log("Display File:" & fn)
    Dim file As String = IO.Path.GetFileNameWithoutExtension(fn)
    Dim path As String = IO.Path.GetDirectoryName(fn)
    Dim logSummary As New Text.StringBuilder
    Dim posSummary As New Text.StringBuilder
    Dim actionSummary As New Text.StringBuilder
    Dim ar As Byte() = IO.File.ReadAllBytes(fn)
    Dim textForLine As String
    Dim zeroCount = 0
    logSummary.AppendLine("File:" & fn)
    Dim lastTime = "0:0"
    Dim millis As Double = 0
    Dim lastMillis As Double = 0
    Dim lastDistance As Double = 0
    For i As Integer = 20 To ar.Length - 1
      If (ar(i) = 0) Then zeroCount += 1
      If (ar(i) > 128 And zeroCount > 3) Then
        Dim hexOfLine As String = ArtoHex(ar, i, 16)
        Dim lenOfText As Integer = ArtoNs(ar, i + 14)
        Try
          textForLine = BytetoStr(ar, i + 16, lenOfText)
        Catch ex As Exception
          textForLine = BytetoStr(ar, i + 17, ar.Length - i - 18)
        End Try
        For Each ig As String In ignores
          textForLine = textForLine.Replace(ig, "")
        Next
        textForLine = textForLine.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Trim
        Dim idx = 0
        If (textForLine.Length > 0) Then
          For Each parm As String In getParms(textForLine)
            Dim parms() As String = parm.Split("|"c)
            If parms(0) = "time" Then
              lastTime = parms(1)
              millis = convTime(lastTime)
            Else
              Dim ignores_p0 As String = "count,Code,flags"
              Dim ignores_p1 As String = "Thread.java:745,Robot Drive... Output not updated often enough."
              ignores_p1 += "Error at java.lang.Thread.run(Thread.java:745): Robot Drive... Output not updated often enough."
              ignores_p1 += "java.lang.Thread.run(Thread.java:745)"
              ignores_p1 += "onTarget:falseTimeoutfalse"
              If (Not ignores_p0.Contains(parms(0))) And (Not ignores_p1.Contains(parms(1))) Then
                actionSummary.AppendLine(millis & "," & parms(0) & "," & parms(1))
              End If
              End If
              If parms(0) = "message" Then
                Dim mItems = parms(1).Split(" "c)
                If (mItems.Length = 2) Then
                  If IsNumeric(mItems(0)) And IsNumeric(mItems(1)) Then
                    Dim distance As Double = Math.Round((CDbl(mItems(0)) + Math.Abs(CDbl(mItems(1)))) / (1409 * 2), 2)
                    Dim speed As Double = Math.Round((distance - lastDistance) / (millis - lastMillis + 0.0000001), 2)
                    lastDistance = distance
                    lastMillis = millis
                    posSummary.AppendLine(millis & "," & mItems(0) & "," & mItems(1) & "," & distance & "," & speed)
                  'log(lastTime & "," & mItems(0) & "," & mItems(1))
                  End If
                End If
              End If
          Next
          logSummary.AppendLine(ArtoHex(ar, i, 8) + textForLine)
          'log(hexOfLine & " len:" & lenOfText & " " & textForLine)
        End If
        zeroCount = 0
        i += 16
      End If
    Next
    If Not IO.Directory.Exists(path & DirSep & "summary") Then MkDir(path & DirSep & "summary")
    If Not IO.Directory.Exists(path & DirSep & "speeds") Then MkDir(path & DirSep & "speeds")
    IO.File.WriteAllText(path & DirSep & "summary" & DirSep & file & ".txt", logSummary.ToString)
    IO.File.WriteAllText(path & DirSep & "speeds" & DirSep & file & ".csv", posSummary.ToString)
    If Not IO.Directory.Exists(path & DirSep & "action") Then MkDir(path & DirSep & "action")
    IO.File.WriteAllText(path & DirSep & "action" & DirSep & file & ".txt", actionSummary.ToString)
  End Sub

  Function getParm(parm As String, s As String) As String()
    Dim res As New List(Of String)
    Dim p = "<" & parm & ">"
    Dim idx = s.IndexOf(p)
    Do While idx >= 0
      idx += p.Length
      Dim e As Integer = s.IndexOf("<", idx + 1)
      If e < 0 Then e = s.Length - 1
      res.Add(s.Substring(idx, e - idx).Trim)
      idx = s.IndexOf(p, e)
    Loop
    Return res.ToArray
  End Function

  Function getParm(parm As String, s As String, ByRef idx As Integer) As String
    Dim p As String = "<" & parm & ">"
    idx = s.IndexOf(p, idx)
    If (idx < 0) Then Return ""
    idx += p.Length + 1
    Dim e As Integer = s.IndexOf("<", idx + 1)
    If e < 0 Then e = s.Length - 1
    Dim res = s.Substring(idx, e - idx).Trim
    idx = e
    Return res
  End Function

  Function getParms(s As String) As String()
    Dim idx = s.IndexOf("<")
    Dim res As New List(Of String)
    Do While (idx >= 0)
      Dim e = s.IndexOf(">", idx)
      Dim v = s.Substring(idx + 1, e - idx - 1)
      idx = s.IndexOf("<", e + 1)
      If idx < 0 Then
        res.Add(v & "|" & s.Substring(e + 1).Trim)
        Return res.ToArray
      End If
      Dim p = s.Substring(e + 1, idx - e - 1).Trim
      res.Add(v & "|" & p)
    Loop
    Return res.ToArray
  End Function

  Function convTime(s As String) As Double
    Dim p() As String = s.Split(":"c)
    If (p.Length = 1 And IsNumeric(p(0))) Then Return CDbl(p(0))
    If (p.Length <> 2) Then Return 0
    If (Not IsNumeric(p(0))) Then Return 0
    If (Not IsNumeric(p(1))) Then Return 0
    Return CDbl(p(0)) * 60 + CDbl(p(1))
  End Function

  Private Sub ClearLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearLogToolStripMenuItem.Click
    ListBox1.Items.Clear()
  End Sub
End Class
