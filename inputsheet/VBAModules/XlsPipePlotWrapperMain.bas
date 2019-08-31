Attribute VB_Name = "XlsPipePlotWrapperMain"
Option Explicit

Const XLSPIPEPLOT_PATH_RANGE = "D6"
Const OUTPUTFILE_RANGE = "B2"
Const NUMBER_OF_COLUMNS = 22
Const EXTRA_ARG_RANGE = "D5"

Const DEBUG_MODE = True
Const VERBOSE_MODE = True

Sub testIt()
    
    ' Use late binding
    'Dim fso As Object
    'Set fso = CreateObject("Scripting.FileSystemObject")
    
    Dim fso As New FileSystemObject  ' Tools...References..."Microsoft scriptning runtime"
    
    ChDir ThisWorkbook.Path
    
    Dim fileScad As String
    fileScad = fso.GetAbsolutePathName(Range(OUTPUTFILE_RANGE).Value)
    
    Dim fileIntermediate As String
    fileIntermediate = fso.GetParentFolderName(fileScad) & "\" & fso.GetBaseName(fileScad) & ".txt"
    
    MsgBox fileScad & vbNewLine & fileIntermediate

End Sub


Sub GenerateScadFile()
' Action:
'
    Dim fso As New FileSystemObject  ' Tools...References..."Microsoft scriptning runtime"
    
    ChDir ThisWorkbook.Path
    
    Dim fileScad As String
    fileScad = fso.GetAbsolutePathName(Range(OUTPUTFILE_RANGE).Value)
    
    Dim fileIntermediate As String
    fileIntermediate = fso.GetParentFolderName(fileScad) & "\" & fso.GetBaseName(fileScad) & ".txt"

    WriteToFile fileIntermediate
    
    MakeOpenSCADFile fileIntermediate, fileScad ' Ge
End Sub

Sub MakeOpenSCADFile(ByVal fileIntermediate As String, ByVal fileScad As String)
' Action:
'
    Dim fso As New FileSystemObject  ' Tools...References..."Microsoft scriptning runtime"

    Dim XlsPipePlotPath As String
    XlsPipePlotPath = fso.GetAbsolutePathName(Range(XLSPIPEPLOT_PATH_RANGE).Value)

    Dim PID
    ChDir ThisWorkbook.Path
    
    Dim xtraArg As String
    xtraArg = Range(EXTRA_ARG_RANGE).Value
    
    Dim closeCommandWindow As String
    If xtraArg <> "" Then
        closeCommandWindow = "/k"
    Else
        closeCommandWindow = "/c"
    End If
    
    PID = Shell("cmd " & closeCommandWindow & " cd && """ & XlsPipePlotPath & """ -input """ & fileIntermediate & """ -output """ & fileScad & """ " & xtraArg, vbNormalFocus)
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
            resultString = resultString & CStr(outputRange.Cells(i, j)) & IIf(j < NUMBER_OF_COLUMNS, ";", vbNewLine)
        Next j
    Next i
    
    tsWrite.WriteLine resultString
    tsWrite.Close
End Sub

Sub ToggleAutoCalc(Optional TurnOn As Boolean = True)
' Action: Turns of automatic calculation
'
    If TurnOn = True Then
        Application.Calculation = xlCalculationAutomatic
        Application.ScreenUpdating = True
        Application.EnableEvents = True
    Else
        Application.Calculation = xlCalculationManual
        Application.ScreenUpdating = False
        Application.EnableEvents = False
    End If
End Sub

Sub AddLoopCheck()
' Action: Adds a loop check for each flowpath that calculates X, Y and Z
'
    Dim i As Integer, j As Integer
    Dim IsInsideFlowpath As Boolean
    Dim word1 As String
    Dim pipeLength As String, angleVert As String, angleAzi As String
    Dim dx As String, dy As String, dz As String
    Dim ifFormula As String
    Dim xOld As String, yOld As String, zOld As String
    Dim addressWord1 As String
    
    Dim lastRow As Long
    lastRow = ActiveSheet.UsedRange.Rows.Count
    
    ' Define range
    Dim inputRange As Range, xyzRange As Range
    With ActiveSheet
        Set inputRange = .Range(.Cells(20, 1), .Cells(lastRow, NUMBER_OF_COLUMNS))
        Set xyzRange = .Range(.Cells(20, NUMBER_OF_COLUMNS + 2), .Cells(lastRow, NUMBER_OF_COLUMNS + 5))
    End With
    
    ToggleAutoCalc TurnOn:=False
    
    For i = 1 To inputRange.Rows.Count
        word1 = LCase(inputRange(i, 1).Value)
        
        If word1 = "junction" Or word1 = "pipe" Or word1 = "volume" Or word1 = "component" Or IsInsideFlowpath = True Then
            IsInsideFlowpath = True
            addressWord1 = inputRange(i, 1).Address(RowAbsolute:=False, ColumnAbsolute:=False)
            pipeLength = inputRange(i, 6).Address(RowAbsolute:=False, ColumnAbsolute:=False)
            angleVert = inputRange(i, 7).Address(RowAbsolute:=False, ColumnAbsolute:=False)
            angleAzi = inputRange(i, 8).Address(RowAbsolute:=False, ColumnAbsolute:=False)
            dx = inputRange(i, 10).Address(RowAbsolute:=False, ColumnAbsolute:=False)
            dy = inputRange(i, 11).Address(RowAbsolute:=False, ColumnAbsolute:=False)
            dz = inputRange(i, 12).Address(RowAbsolute:=False, ColumnAbsolute:=False)
            
            xOld = xyzRange(i - 1, 1).Address(RowAbsolute:=False, ColumnAbsolute:=False)
            yOld = xyzRange(i - 1, 2).Address(RowAbsolute:=False, ColumnAbsolute:=False)
            zOld = xyzRange(i - 1, 3).Address(RowAbsolute:=False, ColumnAbsolute:=False)
            
            ifFormula = "=IF(OR(" & addressWord1 & "=""volume""," & addressWord1 & "=""pipe""," & addressWord1 & "=""junction""),"
            
            If word1 = "flowpath" Then
                xyzRange(i, 1).Value = 0#
                xyzRange(i, 2).Value = 0#
                xyzRange(i, 3).Value = 0#
            Else
                xyzRange(i, 1).Formula = ifFormula & xOld & "+" & pipeLength & "*cos(" & angleVert & "*pi()/180)*cos(" & angleAzi & "*pi()/180)+" & dx & "," & xOld & ")"
                xyzRange(i, 2).Formula = ifFormula & yOld & "+" & pipeLength & "*cos(" & angleVert & "*pi()/180)*sin(" & angleAzi & "*pi()/180)+" & dy & "," & yOld & ")"
                xyzRange(i, 3).Formula = ifFormula & zOld & "+" & pipeLength & "*sin(" & angleVert & "*pi()/180)+" & dz & "," & zOld & ")"
                
                If (word1 = "junction" Or word1 = "pipe" Or word1 = "volume" Or word1 = "component") = False Then IsInsideFlowpath = False
            End If
                
        Else
            IsInsideFlowpath = False
        End If
    Next i
    
    ToggleAutoCalc TurnOn:=True
    
    
    
End Sub



Sub AddPipe()
' Action: Adds one or more pipe segments at the rows of the selected cells
'
'
    Dim CurrRow As Integer, currRowCnt As Integer, word1 As String
    Dim question
    
    If TypeName(Selection) <> "Range" Then
        MsgBox "Select one or more cells where you want to add new pipe segments", vbExclamation, "Insert pipe segment"
        Exit Sub
    End If
    
    CurrRow = Selection.Row
    currRowCnt = Selection.Rows.Count
    
    word1 = Cells(CurrRow, 1)
    
    If word1 = "Pipe" Then
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
        'Range(Cells(CurrRow, 17), Cells(CurrRow + currRowCnt - 1, 20)) = "-"
    End If

End Sub

Sub AddVolume()
' Action: Adds one or more pipe segments at the rows of the selected cells
'
'
    Dim CurrRow As Integer, currRowCnt As Integer, word1 As String
    Dim question
    
    If TypeName(Selection) <> "Range" Then
        MsgBox "Select one or more cells where you want to add new pipe segments", vbExclamation, "Insert pipe segment"
        Exit Sub
    End If
    
    CurrRow = Selection.Row
    currRowCnt = Selection.Rows.Count
    
    word1 = Cells(CurrRow, 1)
    
    If word1 = "Pipe" Then
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
        Range(Cells(CurrRow, 1), Cells(CurrRow + currRowCnt - 1, 1)) = "Volume"
        Range(Cells(CurrRow, 2), Cells(CurrRow + currRowCnt - 1, 2)) = "Tank"
        Range(Cells(CurrRow, 3), Cells(CurrRow + currRowCnt - 1, 3)) = "TankX"
        Range(Cells(CurrRow, 4), Cells(CurrRow + currRowCnt - 1, 4)).Formula = "=CONCATENATE(""VOL_"",ROW())"
        Range(Cells(CurrRow, 6), Cells(CurrRow + currRowCnt - 1, 12)) = 0#   ' Length, Vertical angle, Azimuthal angle, Angle around axis, Dx, Dy, Dz
        'Range(Cells(CurrRow, 15), Cells(CurrRow + currRowCnt - 1, 15)) = 1.5
        'Range(Cells(CurrRow, 17), Cells(CurrRow + currRowCnt - 1, 20)) = "-"
    End If

End Sub


Sub AddJunction()
' Action: Adds one or more pipe segments at the rows of the selected cells
'
'
    Dim CurrRow As Integer, currRowCnt As Integer, word1 As String
    Dim question
    
    If TypeName(Selection) <> "Range" Then
        MsgBox "Select one or more cells where you want to add new component", vbExclamation, "Insert pipe segment"
        Exit Sub
    End If
    
    CurrRow = Selection.Row
    currRowCnt = Selection.Rows.Count
    
    word1 = Cells(CurrRow, 1)
    
    If word1 = "Pipe" Then
        question = MsgBox("Insert " & CStr(currRowCnt) & " component segments BELOW row " + CStr(CurrRow) + " with the same properties as '" & _
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
        Range(Cells(CurrRow, 1), Cells(CurrRow + currRowCnt - 1, 1)) = "Junction"
        Range(Cells(CurrRow, 2), Cells(CurrRow + currRowCnt - 1, 2)) = "Connection"
        Range(Cells(CurrRow, 3), Cells(CurrRow + currRowCnt - 1, 3)) = "Connection"
        Range(Cells(CurrRow, 4), Cells(CurrRow + currRowCnt - 1, 4)).Formula = "=CONCATENATE(""COMP_"",ROW())"
        Range(Cells(CurrRow, 6), Cells(CurrRow + currRowCnt - 1, 12)) = 0#   ' Length, Vertical angle, Azimuthal angle, Angle around axis, Dx, Dy, Dz
        Range(Cells(CurrRow, 17), Cells(CurrRow + currRowCnt - 1, 17)).Formula = "=OFFSET($A$1,ROW()-3,3)"
        Range(Cells(CurrRow, 18), Cells(CurrRow + currRowCnt - 1, 18)) = "B"
        Range(Cells(CurrRow, 19), Cells(CurrRow + currRowCnt - 1, 19)).Formula = "=OFFSET($A$1,ROW()+1,3)"
        Range(Cells(CurrRow, 20), Cells(CurrRow + currRowCnt - 1, 20)) = "A"
    End If

End Sub

