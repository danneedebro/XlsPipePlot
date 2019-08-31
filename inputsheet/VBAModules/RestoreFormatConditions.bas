Attribute VB_Name = "RestoreFormatConditions"
Option Explicit

' Define constants
Const NATIVE_OR_COMMAND = "ELLER"
Const NATIVE_AND_COMMAND = "OCH"
Const NATIVE_SEPARATOR = ";"


Public Type FCond
    Formula As String
    Column1 As Collection
    Column2 As Collection
    Color As Long
    BorderUp As Boolean
    BorderDown As Boolean
    BorderLeft As Boolean
    BorderRight As Boolean
    IsBold As Boolean
    IsItalic As Boolean
    NumberFormat As String
    FontColor As Long
End Type

Sub ResetFormat()
Attribute ResetFormat.VB_ProcData.VB_Invoke_Func = " \n14"
' Action: Resets the format conditions
'
'

'
    Dim i As Integer, j As Integer

    ' Define range
    Dim myRange As Range, subRange As Range
    Dim myRangeAddress As String
    Set myRange = Range(Cells(Selection.Rows(1).Row, 1), Cells(Selection.Rows(1).Row + Selection.Rows.Count, 22))
    Debug.Print myRange.Address
    
    ' Returns the address of the first word. Used in formulas
    Dim FirstWordAddress As String
    FirstWordAddress = myRange(1).Address(RowAbsolute:=False, ColumnAbsolute:=True)
    
    myRange.FormatConditions.Delete
    myRange.Interior.Pattern = xlNone
    
    Dim MyArr() As FCond
    ReDim MyArr(18)
    i = 0
    MyArr(i) = AddFConditions("", Array(1, 20), Array(16, 22), RGB(204, 255, 204), False, False, False, False)
    MyArr(i).Formula = GetFormula(FirstWordAddress, Array("Pipe"))
    i = i + 1
    MyArr(i) = AddFConditions("", Array(1, 20), Array(16, 22), RGB(255, 255, 153), BTop:=False, BDown:=False, BLeft:=False, BRight:=False)
    MyArr(i).Formula = GetFormula(FirstWordAddress, Array("Volume"))
    i = i + 1
    MyArr(i) = AddFConditions("", Array(1), Array(22), RGB(204, 255, 255), BTop:=False, BDown:=False, BLeft:=False, BRight:=False)
    MyArr(i).Formula = GetFormula(FirstWordAddress, Array("Junction"))
    i = i + 1
    MyArr(i) = AddFConditions("", Array(17), Array(20), RGB(192, 192, 192), BTop:=False, BDown:=False, BLeft:=False, BRight:=False)
    MyArr(i).Formula = GetFormula(FirstWordAddress, Array("Pipe", "Volume"))
    i = i + 1
    MyArr(i) = AddFConditions("", Array(10), Array(12), FontColor:=RGB(255, 0, 0), IsItalic:=True)
    Dim tmp As String
    tmp = GetFormula(FirstWordAddress, Array("Junction", "Pipe", "Volume"))
    tmp = "=" & NATIVE_AND_COMMAND & "(" & Right(tmp, Len(tmp) - 1) & NATIVE_SEPARATOR & myRange(1, 10).Address(RowAbsolute:=False, ColumnAbsolute:=False) & "<>0)"
    Debug.Print tmp
    MyArr(i).Formula = tmp
    
    i = i + 1
    MyArr(i) = AddFConditions("", Array(18), Array(18), FontColor:=RGB(255, 0, 0), IsItalic:=True)
    tmp = GetFormula(FirstWordAddress, Array("Junction"))
    tmp = "=" & NATIVE_AND_COMMAND & "(" & Right(tmp, Len(tmp) - 1) & NATIVE_SEPARATOR & "ÄRTAL(" & myRange(1, 18).Address(RowAbsolute:=False, ColumnAbsolute:=False) & "))"
    Debug.Print tmp
    MyArr(i).Formula = tmp
    
    i = i + 1
    MyArr(i) = AddFConditions("", Array(20), Array(20), FontColor:=RGB(255, 0, 0), IsItalic:=True)
    tmp = GetFormula(FirstWordAddress, Array("Junction"))
    tmp = "=" & NATIVE_AND_COMMAND & "(" & Right(tmp, Len(tmp) - 1) & NATIVE_SEPARATOR & "ÄRTAL(" & myRange(1, 20).Address(RowAbsolute:=False, ColumnAbsolute:=False) & "))"
    Debug.Print tmp
    MyArr(i).Formula = tmp
    
    ' Borders below
    i = i + 1
    MyArr(i) = AddFConditions("", Array(2, 6, 8, 11), Array(4, 6, 8, 11), 0, BTop:=True, BDown:=True, BLeft:=False, BRight:=False)
    MyArr(i).Formula = GetFormula(FirstWordAddress, Array("Junction", "Pipe", "Volume"))
    i = i + 1
    MyArr(i) = AddFConditions("", Array(1, 7, 10, 13, 15, 17, 19, 21), Array(1, 7, 10, 13, 15, 17, 19, 21), 0, BTop:=True, BDown:=True, BLeft:=True, BRight:=False)
    MyArr(i).Formula = GetFormula(FirstWordAddress, Array("Junction", "Pipe", "Volume"))
    i = i + 1
    MyArr(i) = AddFConditions("", Array(5, 9, 12, 14, 16, 18, 20, 22), Array(5, 9, 12, 14, 16, 18, 20, 22), 0, BTop:=True, BDown:=True, BLeft:=False, BRight:=True)
    MyArr(i).Formula = GetFormula(FirstWordAddress, Array("Junction", "Pipe", "Volume"))
    i = i + 1
    MyArr(i) = AddFConditions("", Array(10), Array(12), 0, FontColor:=RGB(128, 128, 128), NumberFormat:="0.000 \m")
    i = i + 1
    MyArr(i) = AddFConditions("", Array(6, 18, 20), Array(6, 18, 20), 0, NumberFormat:="0.000 \m")
    i = i + 1
    MyArr(i) = AddFConditions("", Array(7), Array(9), 0, NumberFormat:="0.00°")
    i = i + 1
    MyArr(i) = AddFConditions("", Array(13), Array(14), 0, NumberFormat:="0.00 \m\m")
    
    ReDim Preserve MyArr(i)
    
    ToggleAutoCalc False
    
    Dim subRangeAddress As String
    For i = LBound(MyArr) To UBound(MyArr)
        
    
        For j = 1 To MyArr(i).Column1.Count
            subRangeAddress = Range(myRange.Cells(1, MyArr(i).Column1(j)), myRange.Cells(myRange.Rows.Count, MyArr(i).Column2(j))).Address
            Set subRange = Range(subRangeAddress)
            Debug.Print subRange.Address
            
            If MyArr(i).Formula = "" Then
                subRange.Font.Bold = MyArr(i).IsBold
                subRange.NumberFormat = MyArr(i).NumberFormat
                subRange.Font.Color = MyArr(i).FontColor
                GoTo nextIteration
            End If
            
            With subRange
                .FormatConditions.Add Type:=xlExpression, Formula1:=MyArr(i).Formula
                
                With .FormatConditions(.FormatConditions.Count)
                    .SetFirstPriority
                    If MyArr(i).Color <> 0 Then .Interior.Color = MyArr(i).Color
                    .StopIfTrue = False
                    If MyArr(i).FontColor <> 0 Then .Font.Color = MyArr(i).FontColor
                    If MyArr(i).IsItalic = True Then .Font.Italic = True
                    
                    If MyArr(i).BorderDown = True Then
                        .Borders(xlBottom).LineStyle = xlContinuous
                        .Borders(xlBottom).TintAndShade = 0
                        .Borders(xlBottom).Weight = xlThin
                    Else
                        .Borders(xlBottom).LineStyle = xlNone
                    End If
                    
                    If MyArr(i).BorderUp = True Then
                        .Borders(xlTop).LineStyle = xlContinuous
                        .Borders(xlTop).TintAndShade = 0
                        .Borders(xlTop).Weight = xlThin
                    Else
                        .Borders(xlTop).LineStyle = xlNone
                    End If
                    
                    If MyArr(i).BorderLeft = True Then
                        .Borders(xlLeft).LineStyle = xlContinuous
                        .Borders(xlLeft).TintAndShade = 0
                        .Borders(xlLeft).Weight = xlThin
                    Else
                        .Borders(xlLeft).LineStyle = xlNone
                    End If
                    
                    If MyArr(i).BorderRight = True Then
                        .Borders(xlRight).LineStyle = xlContinuous
                        .Borders(xlRight).TintAndShade = 0
                        .Borders(xlRight).Weight = xlThin
                    Else
                        .Borders(xlRight).LineStyle = xlNone
                    End If
                End With
            End With
nextIteration:
        Next j

    Next i
    
    ToggleAutoCalc True
    
End Sub

Private Sub ToggleAutoCalc(Optional TurnOn As Boolean = True)
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


Function GetFormula(FirstWordAddress As String, FirstWords As Variant) As String
' Action: Returns a string

    Dim i As Integer
    Dim formulaString As String
    formulaString = ""

    
    For i = LBound(FirstWords) To UBound(FirstWords)
        formulaString = formulaString + FirstWordAddress + "=" + Chr(34) + FirstWords(i) + Chr(34)
        If i < UBound(FirstWords) Then
            formulaString = formulaString + NATIVE_SEPARATOR
        End If
    Next i
    
    If UBound(FirstWords) - LBound(FirstWords) = 0 Then
        formulaString = "=" + formulaString
    Else
        formulaString = "=" + NATIVE_OR_COMMAND + "(" + formulaString + ")"
    End If
    
    GetFormula = formulaString
End Function

Function AddFConditions(Formula As String, Col1 As Variant, Col2 As Variant, Optional Color As Long = 0, _
                        Optional IsBold As Boolean = False, Optional IsItalic As Boolean = False, _
                        Optional FontColor As Long = 0, Optional BTop As Boolean = False, _
                        Optional BDown As Boolean = False, Optional BLeft As Boolean = False, _
                        Optional BRight As Boolean = False, Optional NumberFormat As String = "General") As FCond
    Dim output As FCond
    Dim i As Integer
    output.Formula = Formula
    Set output.Column1 = New Collection
    Set output.Column2 = New Collection
    
    For i = LBound(Col1) To UBound(Col1)
        output.Column1.Add Col1(i)
    Next i
    For i = LBound(Col2) To UBound(Col2)
        output.Column2.Add Col2(i)
    Next i
    
    
    output.Color = Color
    output.BorderUp = BTop
    output.BorderDown = BDown
    output.BorderLeft = BLeft
    output.BorderRight = BRight
    output.NumberFormat = NumberFormat
    output.FontColor = FontColor
    output.IsBold = IsBold
    output.IsItalic = IsItalic
    AddFConditions = output
End Function


