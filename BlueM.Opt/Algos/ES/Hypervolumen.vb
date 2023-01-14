'BlueM.Opt
'Copyright (C) BlueM Dev Group
'Website: <https://www.bluemodel.org>
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program. If not, see <https://www.gnu.org/licenses/>.
'
Public Class Hypervolumen
    Private _Dimension As Integer 'Dimension des Zielfunktionsraumes
    Private _Referenzpunkt() As Double
    Private _Normalisiert As Boolean

    'Konstruktor
    Public Sub New()
        'Standardwerte setzen
        _Dimension = 0
        _Normalisiert = False
    End Sub
    'Eigenschaft Dimension des Problems
    Public Property Dimension() As Integer

        Get
            Return Me._Dimension
        End Get

        Set(ByVal Value As Integer)
            If Value > 0 Then
                Me._Dimension = Value
                ReDim Me._Referenzpunkt(Me._Dimension)
            Else
                Throw New Exception("Dimension des Zielfunktionsraumes muss immer größer 0 sein")
            End If
        End Set

    End Property
    'Eigenschaft Referenzpunkt zur Berechnung des Hypervolumes
    Public Property Referenzpunkt() As Double()

        Get
            Return Me._Referenzpunkt
        End Get
        Set(ByVal value() As Double)
            If Me._Dimension = 0 Then
                Throw New Exception("Es muss erst die Dimension des Problems übergeben werden, bevor ein Referenzpunkt definiert wird!")
            ElseIf value.GetUpperBound(0) <> Me.Dimension - 1 Then
                Throw New Exception("Der Referenzpunkt hat nicht die gleiche Dimension wie der Zielfunktionsraum!")
            Else
                value.CopyTo(Me._Referenzpunkt, 0)
            End If
        End Set

    End Property
    'Eigenschaft, ob Zielfunktionen Normalisiert werden sollen
    Public Property Normalisiert() As Boolean

        Get
            Return Me._Normalisiert
        End Get
        Set(ByVal value As Boolean)
            Me._Normalisiert = False
        End Set

    End Property
    'Methode - Berechnung des Hypervolumens
    Public Function GetHypervolume(ByVal AnzLoesungen As Integer, ByVal Loesungsvektor(,) As Double) As Double
        Dim i As Integer, j As Integer
        Dim Qvektor(,) As Double
        Dim Term As Double
        Dim HV As Double

        If AnzLoesungen < 1 Then
            Throw New Exception("Es muss mindestens eine Lösung übergeben werden!")
        End If
        If Loesungsvektor.GetUpperBound(1) <> Me._Dimension Then
            Throw New Exception("Der Lösungsvektor hat die falsche Dimension!")
        End If
        If Loesungsvektor.GetUpperBound(0) <> AnzLoesungen Then
            Throw New Exception("Der Lösungsvektor hat nicht die angebenen Anzahl an Lösungen!")
        End If

        ReDim Qvektor(AnzLoesungen, Me._Dimension - 1)

        If Me._Normalisiert Then
            For i = 0 To AnzLoesungen - 1
                For j = 0 To Me._Dimension - 1
                    Qvektor(i, j) = Loesungsvektor(i, j) / Me._Referenzpunkt(j)
                Next
            Next
        Else
            Loesungsvektor.CopyTo(Qvektor, 0)
        End If

        HV = 0.0
        Term = 1.0
        For j = 0 To Me._Dimension - 1
            Term *= Me._Referenzpunkt(j) - Qvektor(0, j)
        Next
        HV += Term
        For i = 1 To AnzLoesungen - 1
            Term = Me._Referenzpunkt(0) - Qvektor(i, 0)
            For j = 1 To Me._Dimension - 1
                Term *= Qvektor(i - 1, j) - Qvektor(i, j)
            Next
            HV += Term
        Next

        Return HV

    End Function

End Class
