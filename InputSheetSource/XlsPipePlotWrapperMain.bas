Attribute VB_Name = "XlsPipePlotWrapperMain"
Option Explicit

Const XLSPIPEPLOT_PATH_RANGE = "D6"
Const OUTPUTFILE_RANGE = "B2"
Const NUMBER_OF_COLUMNS = 22

Sub GenerateScadFile()
' Action:
'
    Dim fn As String
    fn = Application.ThisWorkbook.Path & "\" & Range(OUTPUTFILE_RANGE)
    
    WriteToFile fn + ".txt"
    
    MakeOpenSCADFile fn
    
    
End Sub

Sub MakeOpenSCADFile(ByVal Filename As String)
' Action:
'
    Dim XlsPipePlotPath As String
    XlsPipePlotPath = Range(XLSPIPEPLOT_PATH_RANGE)

    Dim PID
    ChDir ThisWorkbook.Path
    PID = Shell("cmd /c cd && """ + XlsPipePlotPath + """ -input """ + Filename + ".txt"" -output """ + Filename, vbNormalFocus)
End Sub

Sub WriteToFile(ByVal Filename As String)
' Action: Writes current sheet to a comma separe
'
'
    Dim fso As Scripting.FileSystemObject
    Dim tsWrite As Scripting.TextStream
    
    Set fso = New Scripting.FileSystemObject
    
    Set tsWrite = fso.CreateTextFile(Filename, True)
    
    Dim lastRow As Long
    Dim outputRange As Range
    With ActiveSheet
        lastRow = .Cells(10000, 1).End(xlUp).Row
        Set outputRange = Range(.Cells(1, 1), .Cells(lastRow, 18))
    End With
    
    Dim i, j
    Dim resultString As String
    resultString = ""
    For i = 1 To outputRange.Rows.Count
        For j = 1 To NUMBER_OF_COLUMNS
            resultString = resultString & outputRange.Cells(i, j) & IIf(j < NUMBER_OF_COLUMNS, ";", vbNewLine)
        Next j
    Next i
    
    MsgBox resultString
    tsWrite.WriteLine resultString
End Sub


Sub AddPipe()
' Action: Adds one or more pipe segments at the rows of the selected cells
'
'
    Dim CurrRow As Integer, currRowCnt As Integer, Word1 As String
    Dim question
    
    If TypeName(Selection) <> "Range" Then
        MsgBox "Select one or more cells where you want to add new pipe segments", vbExclamation, "Insert pipe segment"
        Exit Sub
    End If
    
    CurrRow = Selection.Row
    currRowCnt = Selection.Rows.Count
    
    Word1 = Cells(CurrRow, 1)
    
    If Word1 = "Pipe" Then
        question = MsgBox("Insert " & CStr(currRowCnt) & " pipe segments BELOW row " + CStr(CurrRow) + " with the same properties as '" & _
                          Cells(CurrRow, 2) & "'?", vbYesNoCancel, "Insert pipe segment")
        If question <> vbYes Then
            Exit Sub
        End If
        Rows(CStr(CurrRow) & ":" & CStr(CurrRow + currRowCnt - 1)).Select
        Selection.Insert Shift:=xlUp, copyorigin:=xlFormatFromLeftOrAbove
        Rows(CurrRow + currRowCnt).Select
        Selection.Copy
        Rows(CStr(CurrRow) & ":" & CStr(CurrRow + currRowCnt - 1)).Select
        ActiveSheet.Paste
        Rows(CStr(CurrRow + 1) & ":" & CStr(CurrRow + currRowCnt - 1 + 1)).Select
    Else
        question = MsgBox("Insert " & CStr(currRowCnt) & " pipe segments ON row " + CStr(CurrRow) + " ?. ", vbYesNoCancel, "Insert pipe segments")
        If question <> vbYes Then
            Exit Sub
        End If
        Range(Cells(CurrRow, 1), Cells(CurrRow + currRowCnt - 1, 3)) = "Pipe"
        Range(Cells(CurrRow, 4), Cells(CurrRow + currRowCnt - 1, 4)).Formula = "=CONCATENATE(""PIPE_"",ROW())"
        Range(Cells(CurrRow, 6), Cells(CurrRow + currRowCnt - 1, 12)) = 0#   ' Length, Vertical angle, Azimuthal angle, Angle around axis, Dx, Dy, Dz
        Range(Cells(CurrRow, 15), Cells(CurrRow + currRowCnt - 1, 15)) = 1.5
        Range(Cells(CurrRow, 17), Cells(CurrRow + currRowCnt - 1, 20)) = "-"
    End If

End Sub


Sub AddComponent()
' Action: Adds one or more pipe segments at the rows of the selected cells
'
'
    Dim CurrRow As Integer, currRowCnt As Integer, Word1 As String
    Dim question
    
    If TypeName(Selection) <> "Range" Then
        MsgBox "Select one or more cells where you want to add new pipe segments", vbExclamation, "Insert pipe segment"
        Exit Sub
    End If
    
    CurrRow = Selection.Row
    currRowCnt = Selection.Rows.Count
    
    Word1 = Cells(CurrRow, 1)
    
    If Word1 = "Pipe" Then
        question = MsgBox("Insert " & CStr(currRowCnt) & " pipe segments BELOW row " + CStr(CurrRow) + " with the same properties as '" & _
                          Cells(CurrRow, 2) & "'?", vbYesNoCancel, "Insert pipe segment")
        If question <> vbYes Then
            Exit Sub
        End If
        Rows(CStr(CurrRow) & ":" & CStr(CurrRow + currRowCnt - 1)).Select
        Selection.Insert Shift:=xlUp, copyorigin:=xlFormatFromLeftOrAbove
        Rows(CurrRow + currRowCnt).Select
        Selection.Copy
        Rows(CStr(CurrRow) & ":" & CStr(CurrRow + currRowCnt - 1)).Select
        ActiveSheet.Paste
        Rows(CStr(CurrRow + 1) & ":" & CStr(CurrRow + currRowCnt - 1 + 1)).Select
    Else
        question = MsgBox("Insert " & CStr(currRowCnt) & " pipe segments ON row " + CStr(CurrRow) + " ?. ", vbYesNoCancel, "Insert pipe segments")
        If question <> vbYes Then
            Exit Sub
        End If
        Range(Cells(CurrRow, 1), Cells(CurrRow + currRowCnt - 1, 1)) = "Component"
        Range(Cells(CurrRow, 2), Cells(CurrRow + currRowCnt - 1, 2)) = "Connection"
        Range(Cells(CurrRow, 3), Cells(CurrRow + currRowCnt - 1, 3)) = "Connection"
        Range(Cells(CurrRow, 4), Cells(CurrRow + currRowCnt - 1, 4)).Formula = "=CONCATENATE(""COMP_"",ROW())"
        Range(Cells(CurrRow, 6), Cells(CurrRow + currRowCnt - 1, 12)) = 0#   ' Length, Vertical angle, Azimuthal angle, Angle around axis, Dx, Dy, Dz
        Range(Cells(CurrRow, 17), Cells(CurrRow + currRowCnt - 1, 17)).Formula = "=OFFSET($A$1,ROW()-3,3)"
        Range(Cells(CurrRow, 18), Cells(CurrRow + currRowCnt - 1, 18)) = 2
        Range(Cells(CurrRow, 19), Cells(CurrRow + currRowCnt - 1, 19)).Formula = "=OFFSET($A$1,ROW()+1,3)"
        Range(Cells(CurrRow, 20), Cells(CurrRow + currRowCnt - 1, 20)) = 1
    End If

End Sub

