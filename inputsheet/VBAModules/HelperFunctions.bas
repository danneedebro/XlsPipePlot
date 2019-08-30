Attribute VB_Name = "HelperFunctions"
Option Explicit

Const PI = 3.1415926535

Function GetAngle_DyDz(ByVal dy As Double, ByVal dz As Double) As Double
' Action: Returns the angle
    GetAngle_DyDz = Atn(dy / dz) * 180 / PI
End Function

Function GetAngle_DyHyp(ByVal dy As Double, ByVal Hypotenuse As Double) As Double
' Action: Returns the angle
    ' TODO: fix
    GetAngle_DyHyp = 1
End Function

Function GetAngle_Slope(ByVal SlopePercent As Double) As Double
' Action: Returns the angle
    GetAngle_Slope = Atn(SlopePercent / 100) * 180 / PI
End Function
