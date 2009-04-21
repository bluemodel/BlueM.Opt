''' <summary>
''' Klasse f�r die Definition von Objective Funktionen
''' </summary>
Public MustInherit Class ObjectiveFunction

    Public Enum ObjectiveType As Integer
        Series = 1
        Value = 2
        ValueFromSeries = 3
        IHA = 4
    End Enum

    ''' <summary>
    ''' Gibt an ob es sich um eine PrimaryObjective Function handelt
    ''' </summary>
    Public isPrimObjective As Boolean

    ''' <summary>
    ''' Bezeichnung
    ''' </summary>
    Public Bezeichnung As String

    ''' <summary>
    ''' Gruppe
    ''' </summary>
    Public Gruppe As String

    ''' <summary>
    ''' Richtung der ObjectiveFunction (d.h. zu maximieren oder zu minimieren)
    ''' </summary>
    Public Richtung As Constants.EVO_RICHTUNG

    ''' <summary>
    ''' Operator bzw Faktor
    ''' </summary>
    Public OpFact As Double

    ''' <summary>
    ''' Die Dateiendung der Ergebnisdatei, aus der das Simulationsergebnis ausgelesen werden soll
    ''' </summary>
    ''' <remarks>Erlaubte Werte: z.B. "WEL" oder "ASC"</remarks>
    Public Datei As String

    ''' <summary>
    ''' Die Simulationsgr��e, auf dessen Basis der Objectivewert berechnet werden soll
    ''' </summary>
    Public SimGr As String

    ''' <summary>
    ''' Name der Funktion, mit der der Objectivewert berechnet werden soll
    ''' </summary>
    ''' <remarks>Erlaubte Werte: "AbQuad", "Diff", "n�ber", "s�ber", "nUnter", "sUnter", "Volf", "IHA". Siehe auch Wiki</remarks>
    Public Funktion As String

    ''' <summary>
    ''' Gibt an, ob die Objective Function einen IstWert besitzt
    ''' </summary>
    Public hasIstWert As Boolean

    ''' <summary>
    ''' Objective Wert im Istzustand
    ''' </summary>
    Public IstWert As Double

    ''' <summary>
    ''' Gibt die Bezeichnung zur�ck
    ''' </summary>
    ''' <returns>Bezeichnung</returns>
    Public Overrides Function ToString() As String
        Return Me.Bezeichnung
    End Function

    ''' <summary>
    ''' Gibt an ob es ein GruppenLeader ist
    ''' </summary>
    Public ReadOnly Property isGroupLeader() As Boolean
        Get
            If Me.Bezeichnung = Me.Gruppe Then
                Return True
            End If
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Gibt an, ob es ein Gruppenmitglied ist
    ''' </summary>
    Public ReadOnly Property isGroupMember() As Boolean
        Get
            If Me.Gruppe = "" Then
                Return False
            End If
            Return True
        End Get
    End Property

    ''' <summary>
    ''' Returns the type of the ObjectiveFunction
    ''' </summary>
    Public ReadOnly Property GetObjType() As ObjectiveType
        Get
            If (TypeOf (Me) Is ObjectiveFunction_Series) Then
                Return ObjectiveType.Series
            ElseIf (TypeOf (Me) Is Objectivefunction_Value) Then
                Return ObjectiveType.Value
            ElseIf (TypeOf (Me) Is ObjectiveFunction_ValueFromSeries) Then
                Return ObjectiveType.ValueFromSeries
            ElseIf (TypeOf (Me) Is ObjectiveFunction_IHA) Then
                Return ObjectiveType.IHA
            Else
                Throw New Exception("Unable to determine type of ObjectiveFunction")
            End If
        End Get
    End Property

    ''' <summary>
    ''' Calculate the objective function value
    ''' </summary>
    ''' <param name="SimErgebnis">collection of simulation results</param>
    ''' <returns>objective function value</returns>
    Public MustOverride Function calculateObjective(ByVal SimErgebnis As Collection) As Double

    ''' <summary>
    ''' compare two values using a function
    ''' </summary>
    ''' <param name="SimWert">simulation value</param>
    ''' <param name="RefWert">reference value</param>
    ''' <param name="Funktion">comparison function</param>
    ''' <returns>function value</returns>
    ''' <remarks>BUG 218: Konstante und gleiche Zeitschrittweiten vorausgesetzt!</remarks>
    Protected Shared Function compareValues(ByVal SimWert As Double, ByVal RefWert As Double, ByVal Funktion As String)

        Dim QWert As Double

        'Fallunterscheidung Zielfunktion
        Select Case Funktion

            Case "AbQuad"
                'quadratische Abweichung
                '-----------------------
                QWert = (RefWert - SimWert) ^ 2

            Case "Diff"
                'absolute Abweichung
                '-------------------
                QWert = Math.Abs(RefWert - SimWert)

            Case Else
                Throw New Exception("Die Zielfunktion '" & Funktion & "' wird f�r Wertevergleiche nicht unterst�tzt!")

        End Select

        Return QWert

    End Function

    ''' <summary>
    ''' compare two series using a function
    ''' </summary>
    ''' <param name="SimReihe">simulation series</param>
    ''' <param name="RefReihe">reference series</param>
    ''' <param name="Funktion">comparison function</param>
    ''' <returns>function value</returns>
    ''' <remarks>BUG 218: Konstante und gleiche Zeitschrittweiten vorausgesetzt!</remarks>
    Protected Shared Function compareSeries(ByVal SimReihe As Wave.Zeitreihe, ByVal RefReihe As Wave.Zeitreihe, ByVal Funktion As String) As Double

        Dim QWert As Double
        Dim i As Integer

        'BUG 218: Kontrolle
        If (RefReihe.Length <> SimReihe.Length) Then
            Throw New Exception("Die Reihen '" & SimReihe.Title & "' und '" & RefReihe.Title & "' sind nicht kompatibel! (L�nge/Zeitschritt?) Siehe Bug 218")
        End If

        'Fallunterscheidung Zielfunktion
        Select Case Funktion

            Case "AbQuad"
                'Summe der Fehlerquadrate
                '------------------------
                QWert = 0
                For i = 0 To SimReihe.Length - 1
                    QWert += (RefReihe.YWerte(i) - SimReihe.YWerte(i)) ^ 2
                Next

            Case "Diff"
                'Summe der Fehler
                '----------------
                QWert = 0
                For i = 0 To SimReihe.Length - 1
                    QWert += Math.Abs(RefReihe.YWerte(i) - SimReihe.YWerte(i))
                Next

            Case "Volf"
                'Volumenfehler
                '-------------
                Dim VolSim As Double = 0
                Dim VolZiel As Double = 0
                For i = 0 To SimReihe.Length - 1
                    VolSim += SimReihe.YWerte(i)
                    VolZiel += RefReihe.YWerte(i)
                Next
                'Differenz bilden und auf ZielVolumen beziehen
                QWert = Math.Abs(VolZiel - VolSim) / VolZiel * 100

            Case "nUnter"
                'Relative Anzahl der Zeitschritte mit Unterschreitungen (in Prozent)
                '-------------------------------------------------------------------
                Dim nUnter As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < RefReihe.YWerte(i)) Then
                        nUnter += 1
                    End If
                Next
                QWert = nUnter / SimReihe.Length * 100

            Case "sUnter"
                'Summe der Unterschreitungen
                '---------------------------
                Dim sUnter As Double = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) < RefReihe.YWerte(i)) Then
                        sUnter += RefReihe.YWerte(i) - SimReihe.YWerte(i)
                    End If
                Next
                QWert = sUnter

            Case "n�ber"
                'Relative Anzahl der Zeitschritte mit �berschreitungen (in Prozent)
                '------------------------------------------------------------------
                Dim nUeber As Integer = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > RefReihe.YWerte(i)) Then
                        nUeber += 1
                    End If
                Next
                QWert = nUeber / SimReihe.Length * 100

            Case "s�ber"
                'Summe der �berschreitungen
                '--------------------------
                Dim sUeber As Double = 0
                For i = 0 To SimReihe.Length - 1
                    If (SimReihe.YWerte(i) > RefReihe.YWerte(i)) Then
                        sUeber += SimReihe.YWerte(i) - RefReihe.YWerte(i)
                    End If
                Next
                QWert = sUeber

            Case "NashSutt"
                'Nash Sutcliffe
                '--------------
                'Mittelwert bilden
                Dim Qobs_quer, zaehler, nenner As Double
                For i = 0 To SimReihe.Length - 1
                    Qobs_quer += RefReihe.YWerte(i)
                Next
                Qobs_quer = Qobs_quer / (SimReihe.Length)
                For i = 0 To SimReihe.Length - 1
                    zaehler += (RefReihe.YWerte(i) - SimReihe.YWerte(i)) * (RefReihe.YWerte(i) - SimReihe.YWerte(i))
                    nenner += (RefReihe.YWerte(i) - Qobs_quer) * (RefReihe.YWerte(i) - Qobs_quer)
                Next
                'abge�nderte Nash-Sutcliffe Formel: 0 als Zielwert (1- weggelassen)
                QWert = zaehler / nenner

            Case "Korr"
                'Korrelationskoeffizient (lineare Regression)
                'Es wird das Bestimmtheitsma� r^2 zur�ckgegeben [0-1]
                '----------------------------------------------------
                Dim kovar, var_x, var_y, avg_x, avg_y As Double
                'Mittelwerte
                avg_x = SimReihe.getWert("Average")
                avg_y = RefReihe.getWert("Average")
                'r^2 = sxy^2 / (sx^2 * sy^2)
                'Standardabweichung: var_x = sx^2 = 1 / (n-1) * SUMME[(x_i - x_avg)^2]
                'Kovarianz: kovar= sxy = 1 / (n-1) * SUMME[(x_i - x_avg) * (y_i - y_avg)]
                kovar = 0
                var_x = 0
                var_y = 0
                For i = 0 To SimReihe.Length - 1
                    kovar += (SimReihe.YWerte(i) - avg_x) * (RefReihe.YWerte(i) - avg_y)
                    var_x += (SimReihe.YWerte(i) - avg_x) ^ 2
                    var_y += (RefReihe.YWerte(i) - avg_y) ^ 2
                Next
                var_x = 1 / (SimReihe.Length - 1) * var_x
                var_y = 1 / (SimReihe.Length - 1) * var_y
                kovar = 1 / (SimReihe.Length - 1) * kovar
                'Bestimmtheitsma� = Korrelationskoeffizient^2
                QWert = kovar ^ 2 / (var_x * var_y)

            Case Else
                Throw New Exception("Die Zielfunktion '" & Funktion & "' wird f�r Reihenvergleiche nicht unterst�tzt!")

        End Select

        Return QWert

    End Function

End Class
