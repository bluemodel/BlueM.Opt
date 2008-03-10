Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Module Manager

    'Eigenschaften
    '#############

    Public List_Ziele() As Ziel                 'Liste der Zielfunktionen

    'Properties
    '##########

    'Gibt die Gesamtanzahl der Ziele zurück
    '**************************************
    Public ReadOnly Property AnzZiele() As Integer
        Get
            Return Manager.List_Ziele.Length
        End Get
    End Property

    'Gibt die Anzahl der OptimierungsZiele zurück
    '********************************************
    Public ReadOnly Property AnzPenalty() As Integer
        Get
            Dim n As Integer

            n = 0
            For Each ziel As Ziel In Manager.List_Ziele
                If (ziel.isOpt) Then n += 1
            Next

            Return n
        End Get
    End Property

    'Gibt die OptimierungsZiele zurück
    '*********************************
    Public ReadOnly Property List_OptZiele() As Ziel()
        Get
            Dim i As Integer
            Dim array() As Ziel

            ReDim array(Manager.AnzPenalty - 1)

            For i = 0 To Manager.AnzZiele - 1
                If (Manager.List_Ziele(i).isOpt) Then
                    array(i) = Manager.List_Ziele(i)
                End If
            Next

            Return array
        End Get
    End Property

    'Speichern der Zielliste in einer XML-Datei
    '******************************************
    Public Sub saveZIE(ByVal filename As String)

        ' Create an XmlRootAttribute, and set its properties.
        Dim xRoot As New XmlRootAttribute()
        xRoot.ElementName = "Zielliste"
        xRoot.Namespace = "http://130.83.196.154/BlueM"

        Dim serializer As New XmlSerializer(GetType(Common.Ziel()), xRoot)
        Dim writer As New StreamWriter(filename)
        serializer.Serialize(writer, Manager.List_Ziele)
        writer.Close()

    End Sub


End Module
