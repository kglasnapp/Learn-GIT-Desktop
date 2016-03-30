Option Explicit On
Option Strict On
Option Compare Text

Module modStrByte

  Private b_crlf() As Byte = StrToByte(vbCrLf)
  Public DirSep As Char = IO.Path.DirectorySeparatorChar
  Public AppPath As String = My.Application.Info.DirectoryPath & DirSep
  Public AppName As String = IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath)

  Sub log(s As String)
    frmMain.ListBox1.Items.Add(s)
    If (frmMain.ListBox1.Items.Count > 20000) Then frmMain.ListBox1.Items.Clear()
  End Sub

    Function StrToByte(ByVal s As String) As Byte()
        Try
            Dim ar(s.Length - 1) As Byte
            Dim c() As Char = s.ToCharArray
            For i As Integer = 0 To s.Length - 1
                ar(i) = CByte(Asc(c(i)))
            Next
            Return ar
        Catch ex As Exception
      log("StrToByte exception")
            Dim ar() As Byte = {}
            Return ar
        End Try
    End Function

    Function BytetoLine(ByVal ar As Byte(), ByVal Start As Integer) As String
        Dim len As Integer = FindStr(ar, Start, ar.Length - Start, b_crlf) - Start
        Return (BytetoStr(ar, Start, len))
    End Function

    Function BytetoStr(ByVal ar As Byte(), ByVal Start As Integer, ByVal Length As Integer) As String
        If ar.Length < Start + Length Then
            Try
                Dim i As Integer = Min(Length, ar.Length - Start)
                Dim s As String = System.Text.Encoding.ASCII.GetString(ar, Start, i)
                Return s
            Catch ex As Exception
        log("BytetoStr exception")
                Return ""
            End Try
        End If
        Return System.Text.Encoding.ASCII.GetString(ar, Start, Length)
    End Function

    Function BytetoStrEx(ByVal ar As Byte(), ByVal Start As Integer, ByVal Length As Integer) As String
        If (Start + Length) < ar.Length Then
            Return System.Text.Encoding.ASCII.GetString(ar, Start, Length)
        Else
            Return ""
        End If
    End Function

    Function BytetoStr(ByVal ar As Byte()) As String
        Return System.Text.Encoding.ASCII.GetString(ar)
    End Function

    Function BytetoNullStr(ByVal ar() As Byte, ByVal start As Integer, ByVal len As Integer) As String
        Dim i As Integer = Array.IndexOf(ar, CByte(0), start, len)
        If i < 0 Then Return ""
        Return System.Text.Encoding.ASCII.GetString(ar, start, i - start)
    End Function

    Function ArInstr(ByVal ar() As Byte, ByVal Start As Integer, ByVal Len As Integer, ByVal String2 As String) As Integer
        Dim i As Integer, j As Integer
        Try
            Dim s() As Byte = StrToByte(String2)
            For i = Start To Start + Len - 1
                For j = 0 To s.Length - 1
                    If ar(i + j) <> s(j) Then Exit For
                Next
                If j = s.Length Then Return i
            Next
            Return -1
        Catch e As Exception
      log("Error -- ArInstr got an error start=" & Start & " len=" & Len & " ar.length=" & ar.Length)
            Return -1
        End Try
    End Function

    Function ArInstr(ByVal ar() As Byte, ByVal Start As Integer, ByVal Len As Integer, ByVal String2() As Byte) As Integer
        Dim i As Integer, j As Integer
        Try
            For i = Start To Start + Len - 1
                For j = 0 To String2.Length - 1 ' Can make faser with indexof
                    If ar(i + j) <> String2(j) Then Exit For
                Next
                If j = String2.Length Then Return i
            Next
            Return -1
        Catch e As Exception
      log("Error -- ArInstr - byte() - got an error start=" & Start & " len=" & Len & " ar.length=" & ar.Length)
            Return -1
        End Try
    End Function

    Function ArMatch(ByVal ar() As Byte, ByVal Start As Integer, ByVal StringtoFind() As Byte) As Boolean
        Try
            Dim i As Integer
            For i = 0 To StringtoFind.Length - 1
                If ar(Start + i) <> StringtoFind(i) Then Exit For
            Next
            If i = StringtoFind.Length Then Return True Else Return False
        Catch
      log("Warning -- ArMatch had an error ar=" & ar.Length & " start=" & Start & " string=" & BytetoStr(StringtoFind))
      Return False
        End Try
    End Function

    Function ArCount(ByVal ar() As Byte, ByVal Start As Integer, ByVal Length As Integer, ByVal c As Byte) As Integer
        Dim cnt As Integer = 0
        For i As Integer = Start To Start + Length - 1
            If ar(i) = c Then
                cnt += 1
            End If
        Next
        Return cnt
    End Function

    Function FindByte(ByVal Ar() As Byte, ByVal Start As Integer, ByVal Len As Integer, ByVal [Byte] As Byte) As Integer
        Return Array.IndexOf(Ar, [Byte], Start, Len)
    End Function

    Function FindStr(ByVal Ar() As Byte, ByVal Start As Integer, ByVal Length As Integer, ByVal Str() As Byte) As Integer
        Dim i As Integer, LastStart As Integer
        Try
            If Str.Length = 0 Then Return Start
            If Ar.Length < Start + Length Then
                Log("Warning -- File Corupted Trying to Search Beyond the end of the file")
                Return -1
            End If
            Do
                LastStart = Start
                Start = Array.IndexOf(Ar, Str(0), Start, Length)
                If Start < 0 Then Return -1
                Length = Length - (Start - LastStart)
                For i = 1 To Str.Length - 1
                    If i + Start >= Ar.Length Then Return -1
                    If Str(i) <> Ar(Start + i) Then Exit For
                Next
                If i = Str.Length Then Return Start
                If Ar.Length < Start + Length Then Return -1
                Start += 1
                Length -= 1
            Loop
        Catch e As Exception
      log("FindStr got an err ar.len=" & Ar.Length & " start=" & Start & " length=" & Length & " str=" & BytetoStr(Str))
            Return -1
        End Try
    End Function

    Function FindStrIgnoreCase(ByVal Ar() As Byte, ByVal Start As Integer, ByVal Length As Integer, _
     ByVal Str() As Byte, ByVal LogErr As Boolean) As Integer
        Dim i As Integer = Start, j As Integer = 0, LastStart As Integer = Start
        Try
            Do
                i = LastStart
                j = 0
                If (i - Start + Str.Length) >= Length Then
                    If LogErr Then Log("FindStrIgnoreCase --> Could not Find Str=" & BytetoStr(Str) & " in msg=" & BytetoStr(Ar, Start, Length))
                    Return -1
                End If
                Do
                    Dim b As Byte = Ar(i)
                    If b >= Asc("A"c) And b <= Asc("Z"c) Then b += CByte(32)
                    If b <> Str(j) Then Exit Do
                    i += 1 : j += 1
                    If j >= Str.Length Then Exit Do
                Loop
                If j = Str.Length Then Return LastStart
                LastStart += 1
            Loop
            Return -1
        Catch e As Exception
      log("FindStrIgnoreCase --> Got an error Start=" & Start & " length=" & Length)
            Return -1
        End Try
    End Function

    Function Find2(ByVal ar() As Byte, ByVal Start As Integer, ByRef Length As Integer, ByVal b1 As Byte, ByVal b2 As Byte) As Integer
        Dim i As Integer, j As Integer
        Try
            i = Array.IndexOf(ar, b1, Start, Length)
            If i < 0 Then Return -1
            j = Array.IndexOf(ar, b2, i + 1, Length)
            Return j
        Catch
            Return -1
        End Try
    End Function

    Function BytetoLong(ByVal ar() As Byte, ByVal Start As Integer, ByVal Len As Integer) As Long
        Dim s As String = BytetoStr(ar, Start, Len)
        Try
            Return CLng(Val(s))
        Catch
            Log("Error -- Unable to convert <<" & s & ">> to a long")
            Return 0
        End Try
    End Function

    Function BytetoInt(ByVal ar() As Byte, ByVal Start As Integer, ByVal Len As Integer) As Integer
        Dim Res As Integer = 0
        For i As Integer = Start To Start + Len - 1
            If ar(i) = 32 Then Return Res
            Res = Res * 10 + ar(i) - 48
        Next
        Return Res
    End Function

    Function BytetoInt(ByVal ar() As Byte, ByVal Start As Integer) As Integer
        Dim Res As Integer = 0
        For i As Integer = Start To Start + 10
            If ar(i) = 32 Then Continue For
            If ar(i) = 10 Or ar(i) = 13 Then Return Res
            Res = Res * 10 + ar(i) - 48
        Next
        Return Res
    End Function

    Function BytetouInt(ByVal ar() As Byte, ByVal Start As Integer, ByVal Len As Integer) As UInteger
        Dim Res As UInteger = 0
        For i As Integer = Start To Start + Len - 1
            If ar(i) = 32 Then Continue For
            Res = CUInt(Res * 10 + ar(i) - 48)
        Next
        Return Res
    End Function

    Function ArtoHex4(ByVal ar() As Byte, ByVal Start As Integer) As Integer
        Dim res As Integer = 0
        For i As Integer = Start To Start + 3
            res = res * 16 + HextoInt(ar(i))
        Next
        Return res
    End Function

    Function HextoInt(ByVal b As Byte) As Integer
        If b >= CByte(Asc("0")) And b <= CByte(Asc("9")) Then Return b - CByte(Asc("0"))
        If b >= CByte(Asc("A")) And b <= CByte(Asc("F")) Then Return b - CByte(Asc("A")) + 10
        If b >= CByte(Asc("a")) And b <= CByte(Asc("f")) Then Return b - CByte(Asc("a")) + 10
        Return 0
    End Function

    Function toOctal(ByVal i As Integer) As String
        Return "\" & (i >> 6) & ((i >> 3) And 7) & (i And 7)
    End Function

    Function toUtf(ByVal i As Integer) As Integer
        Return 256 * (16 * 12 + (i >> 6)) + 128 + (i And 63)
    End Function

  Function Min(ByVal a As Integer, ByVal b As Integer) As Integer
    If a < b Then Return a Else Return b
  End Function

  Function Min(ByVal a As Long, ByVal b As Long) As Long
    If a < b Then Return a Else Return b
  End Function

  Function Min(ByVal a As Date, ByVal b As Date) As Date
    If a < b Then Return a Else Return b
  End Function

  Function Min(ByVal a As Double, ByVal b As Double) As Double
    If a < b Then Return a Else Return b
  End Function

  Function ArtoHl(ByVal ar() As Byte, ByVal Start As Integer) As Long
    Dim i As Integer = ar(Start + 3) * 256 + ar(Start + 2)
    Dim j As Integer = ar(Start + 1) * 256 + ar(Start + 0)
    Dim l As Long = i * 256L * 256L + j
    Return l ' Needed to do this as a bug in VS 2005 
  End Function

  Function ArtoNs(ByVal ar() As Byte, ByVal Start As Integer) As Integer
    Return ar(Start) * 256 + ar(Start + 1)
  End Function

  Function ArtoHs(ByVal ar() As Byte, ByVal Start As Integer) As Integer
    Return ar(Start + 1) * 256 + ar(Start)
  End Function

  Function ArtoHI(ByVal ar() As Byte, ByVal Start As Integer) As Integer
    Dim i As Integer = ar(Start + 3) * 256 + ar(Start + 2)
    Dim j As Integer = ar(Start + 1) * 256 + ar(Start + 0)
    Dim l As Long = i * 256L * 256L + j
    Dim d As Long = 256L * 256 * 256 * 128 - 1
    If l > d Then Return CInt(l - d)
    Return CInt(l)
  End Function

  Function ArtoHUI(ByVal ar() As Byte, ByVal Start As Integer) As UInteger
    Dim i As Integer = ar(Start + 3) * 256 + ar(Start + 2)
    Dim j As Integer = ar(Start + 1) * 256 + ar(Start + 0)
    Dim l As Long = i * 256L * 256L + j
    Return CUInt(l)
  End Function

  Function ArtoHex(ByVal ar() As Byte, ByVal Start As Integer, ByVal Len As Integer) As String
    Dim s As New System.Text.StringBuilder(128)
    For i As Integer = 0 To Len - 1
      s.Append(ar(i + Start).ToString("x2") & " ")
    Next
    Return s.ToString
  End Function

  Public Function InitDT(ByVal Name As String, ByVal Control As String) As DataTable
    Dim dt As New DataTable
    dt.TableName = Name
    For Each s As String In Control.Split("|"c)
      Dim p() As String = s.Split(","c)
      If Not dt.Columns.Contains(p(0)) Then
        With dt.Columns.Add(p(0))
          If p(1) = "A" Then .DataType = GetType(System.Int32)
          If p(1) = "I" Then .DataType = GetType(System.UInt32)
          If p(1) = "L" Then .DataType = GetType(System.Int64)
          If p(1) = "D" Or p(1) = "T" Then .DataType = GetType(System.DateTime)
          If p(1) = "F" Then .DataType = GetType(System.Double)
          If p(1) = "S" Then .DataType = GetType(System.String)
        End With
      End If
    Next
    Return dt
  End Function

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
      If Not IO.Directory.Exists(.InitialDirectory) Then .InitialDirectory = AppPath
      .Filter = Filter
      .FilterIndex = 1
      .Multiselect = True
      .FileName = ""
      If .ShowDialog() = Windows.Forms.DialogResult.OK Then
        If .FileNames.Length = 0 Then Return Split("")
        Dim ar(.FileNames.Length - 1) As String
        Array.Copy(.FileNames, ar, .FileNames.Length)
        Array.Sort(ar)
        SaveSetting(AppName, "Directory", Key, IO.Path.GetDirectoryName(.FileNames(0)))
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
  '
  ' Generic Routines to maniuplate items on forms
  ''
  Sub SaveFormParms(ByVal frm As Form, Optional ByVal Name As String = "")
    If Name.Equals("") Then Name = frm.Name
    If frm.WindowState = FormWindowState.Normal Then
      SaveSetting(AppName, Name, "Width", CStr(frm.Width))
      SaveSetting(AppName, Name, "Height", CStr(frm.Height))
      SaveSetting(AppName, Name, "Top", CStr(frm.Top))
      SaveSetting(AppName, Name, "Left", CStr(frm.Left))
    End If
  End Sub

  Sub RestoreFormParms(ByVal frm As Form, Optional ByVal Name As String = "")
    If Name.Equals("") Then Name = frm.Name
    With frm
      Try
        .Width = CInt(GetSetting(AppName, Name, "Width", "500"))
        .Height = CInt(GetSetting(AppName, Name, "Height", "500"))
        .Top = CInt(GetSetting(AppName, Name, "Top", "0"))
        .Left = CInt(GetSetting(AppName, Name, "Left", "0"))
      Catch e As Exception
        .Width = 500
        .Height = 500
        .Top = 0
        .Left = 0
      End Try
    End With
  End Sub

  Function Version() As String
    With My.Application.Info.Version
      Return .Major & "." & .Minor & "." & .Build & "." & .Revision
    End With
  End Function


  Function GetExcel() As String
    Dim ar() As String = {"c:\Program Files\Microsoft Office\Office14\EXCEL.EXE",
        "C:\Program Files\Microsoft Office\OFFICE11\excel.exe", "c:\Program Files (x86)\Notepad++\notepad++.exe", "C:\WINDOWS\system32\Notepad.exe"}
    For Each t As String In ar
      If IO.File.Exists(t) Then Return t
    Next
    Return ""
  End Function

  Function GetViewer() As String
    Dim ar() As String = {"c:\Program Files (x86)\Notepad++\notepad++.exe", "C:\WINDOWS\system32\Notepad.exe"}
    For Each t As String In ar
      If IO.File.Exists(t) Then Return t
    Next
    Return ""
  End Function

  Public Function ShellFancy(ByVal Command As String, Optional ByVal Style As AppWinStyle = AppWinStyle.MinimizedFocus, _
        Optional ByVal Wait As Boolean = False, Optional ByVal Timeout As Integer = -1) As String
    Try
      Dim ar() As String = CommandSplit(Command)
      log("Run Command=" & ar(0) & " args=" & ar(1))
      Dim Proc As System.Diagnostics.Process = New System.Diagnostics.Process()
      Proc.EnableRaisingEvents = False
      Proc.StartInfo.FileName = ar(0)
      Proc.StartInfo.Arguments = ar(1)
      Proc.Start()
      If Wait Then Proc.WaitForExit()
      Return ""
    Catch ex As Exception
      Shell(Command, AppWinStyle.MaximizedFocus)
      Return ""
    End Try
  End Function

  Private Function CommandSplit(ByVal cmd As String) As String()
    Dim ar(1) As String
    Dim i As Integer = cmd.IndexOf(""" ") + 1
    If i <= 0 Then i = cmd.IndexOf(" "c)
    ar(0) = cmd.Substring(0, i).Trim
    ar(1) = cmd.Substring(i + 1).Trim
    Return ar
  End Function
End Module
