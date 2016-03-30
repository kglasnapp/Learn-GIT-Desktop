Option Explicit On
Option Strict On

Imports System
Imports System.Text
Imports System.IO
Imports System.Data
Imports Microsoft.VisualBasic

Public Class frmMain
  Dim ignores() As String = {}
  Dim dt As DataTable
  Dim ignoreDct As New Dictionary(Of String, Integer)

  Private Sub frmMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
    SaveFormParms(Me)
    SaveSetting(AppName, Me.Name, "Horizontal-Split", CStr(SplitContainer1.SplitterDistance))
    log("Robot Log View Ended Normally")
  End Sub

  Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
    log("Start of Robot Log Display")
    If (IO.File.Exists("ignores.txt")) Then
      ignores = IO.File.ReadAllLines("ignores.txt")
    End If
    dt = InitDT("actions", "Index,i|WallTime,d|Time,s,10|Type,s,10|Data,s,20|Misc Data,s,20|File,s,20")
    RestoreFormParms(Me)
    Try
      SplitContainer1.SplitterDistance = CInt(GetSetting(AppName, Me.Name, "Horizontal-Split", "250"))
    Catch
      SplitContainer1.SplitterDistance = Me.Height \ 2
    End Try
    Me.Text = "Robot Log File View"
  End Sub

  Private Sub OpenLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenLogToolStripMenuItem.Click
    Dim ar() As String = OpenMultiFileDialog("logs", "logs (*.dsevents)|*.dsevents|All Files(*.*)|*.*")
    Dim cnt As Integer = 0
    dt.Rows.Clear()
    For Each fn As String In ar
      cnt += DisplayLog(fn, cnt)
      My.Application.DoEvents()
    Next
    dgv.DataSource = dt
    dgv.Columns.Item("Type").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    dgv.Columns.Item("Time").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    dgv.Columns.Item("Misc Data").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    dgv.Columns.Item("Data").Width = 400
    dgv.Columns.Item("File").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    dgv.Columns("WallTime").DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss"
    dgv.Columns.Item("WallTime").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    lblCount.Text = "Record Count:" + cnt.ToString
  End Sub

  Function DisplayLog(fn As String, lineOffset As Integer) As Integer
    log("Display File:" & fn)
    Dim file As String = IO.Path.GetFileNameWithoutExtension(fn)
    Dim p() As String = file.Replace(" ", "_").Split("_"c)
    Dim d As New Date(CInt(p(0)), CInt(p(1)), CInt(p(2)), CInt(p(3)), CInt(p(4)), CInt(p(5)))
    Dim path As String = IO.Path.GetDirectoryName(fn)
    Dim logSummary As New Text.StringBuilder
    Dim posSummary As New Text.StringBuilder
    posSummary.AppendLine("Time,EncoderX,EncoderY,Distance,Speed")
    Dim actionSummary As New Text.StringBuilder
    actionSummary.AppendLine("Date,Elasped Time,Type,Data,Misc Data")
    Dim ar As Byte() = IO.File.ReadAllBytes(fn)
    Dim textForLine As String
    Dim zeroCount = 0
    logSummary.AppendLine("File:" & fn)
    Dim lastTime = "0:0"
    Dim millis As Double = 0
    Dim lastMillis As Double = 0
    Dim lastDistance As Double = 0
    Dim cnt As Integer = 0
    ignoreDct.Clear()
    Dim ignores_p0 As String = "count,Code,flags,TagVersion"
    Dim ignores_p1 As String = "Thread.java:745,Robot Drive... Output not updated often enough.|"
    ignores_p1 += "Error at java.lang.Thread.run(Thread.java:745): Robot Drive... Output not updated often enough.|"
    ignores_p1 += "java.lang.Thread.run(Thread.java:745)|"
    ignores_p1 += "onTarget:falseTimeoutfalse|"
    ignores_p1 += "After execute|"
    ignores_p1 += "bloblength0|"
    ignores_p1 += "Warning at org.usfirst.frc3932.Robot.disabledPeriodic(Robot.java:156): Joystick Button 8 on port 1 not available, check if controller is plugged in|"
    ignores_p1 += "Joystick Button 8 on port 1 not available, check if controller is plugged in <location> org.usfirst.frc3932.Robot.disabledPeriodic(Robot.java:156)|"
    ignores_p1 += "onTarget:falseTimeout/Elapsed Time = false0.0"
    ignores_p1 += "WARNING: Joystick Button 8 on port 1 not available, check if controller is plugged in"
    ignores_p1 += ""
    Dim secondOffset As Integer = 0

    For i As Integer = 20 To ar.Length - 1
      If (ar(i) = 0) Then zeroCount += 1
      If (ar(i) > 128 And zeroCount > 3) Then
        Dim hexOfLine As String = ArtoHex(ar, i, 16)
        Dim lenOfText As Integer = ArtoNs(ar, i + 14)
        Dim lineTime As Integer = ArtoNs(ar, i + 2)
        If (secondOffset = 0) Then secondOffset = lineTime
        Try
          textForLine = BytetoStr(ar, i + 16, lenOfText)
        Catch ex As Exception
          textForLine = BytetoStr(ar, i + 17, ar.Length - i - 18)
        End Try
        If (textForLine.Length > 0) Then
          For Each parm As String In getParms(textForLine)
            Dim parms() As String = parm.Split("|"c)
            If parms(0) = "time" Then
              lastTime = parms(1)
              millis = convTime(lastTime)
            Else
              If (parms(1) <> "") Then
                If (Not ignores_p0.Contains(parms(0))) Then
                  If (Not ignores_p1.Contains(parms(1))) Then
                    cnt += 1
                    actionSummary.AppendLine(d.AddSeconds(lineTime - secondOffset).ToString + "," + millis.ToString & "," & parms(0) & ",""" & parms(1) + """")
                    dt.Rows.Add(lineOffset + cnt, d.AddSeconds(lineTime - secondOffset), millis, parms(0), parms(1), "", file)
                  Else
                    addToIgnores(parms(0) & "-" & parms(1))
                  End If
                Else
                  'addToIgnores(parms(0))
                End If
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
    For Each key As String In ignoreDct.Keys
      dt.Rows.Add(0, d, 0, "Ignored", key, ignoreDct(key), file)
    Next
    dt.Rows.Add(0, d, 0, "Count", cnt.ToString, "", file)
    If Not IO.Directory.Exists(path & DirSep & "summary") Then MkDir(path & DirSep & "summary")
    If Not IO.Directory.Exists(path & DirSep & "speeds") Then MkDir(path & DirSep & "speeds")
    If Not IO.Directory.Exists(path & DirSep & "action") Then MkDir(path & DirSep & "action")
    Try
      IO.File.WriteAllText(path & DirSep & "summary" & DirSep & file & ".txt", logSummary.ToString)
      IO.File.WriteAllText(path & DirSep & "speeds" & DirSep & file & ".csv", posSummary.ToString)

      IO.File.WriteAllText(path & DirSep & "action" & DirSep & file & ".csv", actionSummary.ToString)
    Catch ex As Exception
      log("Unable to write file " + ex.Message)
    End Try
    Return cnt
  End Function

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

  Sub addToIgnores(s As String)
    If ignoreDct.ContainsKey(s) Then
      ignoreDct(s) += 1
    Else
      ignoreDct.Add(s, 0)
    End If
  End Sub

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

  Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
    Me.Close()
  End Sub

  Private Sub SummariesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SummariesToolStripMenuItem.Click
    Dim ar() As String = OpenMultiFileDialog("sumnmaries", "text (*.txt)|*.txt|All Files(*.*)|*.*")
    For Each fn As String In ar
      Dim viewer As String = GetViewer() + " """ + fn + """"
      Try
        log("View Summaries:" + viewer)
        ShellFancy(viewer.Trim, AppWinStyle.NormalFocus, False)
      Catch
        MsgBox("Unable to Start viewer for " & fn, MsgBoxStyle.Exclamation)
      End Try
    Next
  End Sub

  Private Sub SpeedFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpeedFilesToolStripMenuItem.Click
    Dim ar() As String = OpenMultiFileDialog("speeds", "csv (*.csv)|*.csv|All Files(*.*)|*.*")
    For Each fn As String In ar
      Dim viewer As String = GetExcel() + " """ + fn + """"
      Try
        log("View Speeds:" + viewer)
        ShellFancy(viewer.Trim, AppWinStyle.NormalFocus, False)
      Catch
        MsgBox("Unable to Start viewer for " & fn, MsgBoxStyle.Exclamation)
      End Try
    Next
  End Sub

  Private Sub ActionFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActionFilesToolStripMenuItem.Click
    Dim ar() As String = OpenMultiFileDialog("speeds", "csv (*.csv)|*.csv|All Files(*.*)|*.*")
    For Each fn As String In ar
      Dim viewer As String = GetExcel() + " """ + fn + """"
      Try
        log("View Speeds:" + viewer)
        ShellFancy(viewer.Trim, AppWinStyle.NormalFocus, False)
      Catch
        MsgBox("Unable to Start viewer for " & fn, MsgBoxStyle.Exclamation)
      End Try
    Next
  End Sub
End Class
