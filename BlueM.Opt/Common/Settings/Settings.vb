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
Imports System.Xml.Serialization
''' <summary>
''' Klasse Settings
''' zum Speichern aller EVO-Einstellungen aus dem Form
''' </summary>
Public Class Settings

    'Klasse für generelle Settings
    '-----------------------------
    Public Class Settings_General
        Private mUseMultithreading As Boolean
        Private mNThreads As Integer
        Private mMultithreadingAllowed As Boolean
        Private mDrawOnlyCurrentGeneration As Boolean

        ''' <summary>
        ''' Multithreading erlauben/verbieten und gleichzeitig ein-/ausschalten
        ''' </summary>
        Public Property MultithreadingAllowed() As Boolean
            Get
                Return Me.mMultithreadingAllowed
            End Get
            Set(ByVal allow As Boolean)
                Me.mMultithreadingAllowed = allow
                If (allow) Then
                    Me.mMultithreadingAllowed = True
                    Me.UseMultithreading = True
                Else
                    Me.mMultithreadingAllowed = False
                    Me.UseMultithreading = False
                End If
            End Set
        End Property

        ''' <summary>
        ''' Multithreading verwenden
        ''' </summary>
        Public Property UseMultithreading() As Boolean
            Get
                Return mUseMultithreading
            End Get
            Set(ByVal value As Boolean)
                mUseMultithreading = value
            End Set
        End Property

        ''' <summary>
        ''' Number of threads to use for multithreading
        ''' </summary>
        Public Property NThreads() As Integer
            Get
                Return mNThreads
            End Get
            Set(ByVal value As Integer)
                mNThreads = value
            End Set
        End Property

        ''' <summary>
        ''' Nur die aktuelle Generation im Hauptdiagramm anzeigen
        ''' </summary>
        Public Property DrawOnlyCurrentGeneration() As Boolean
            Get
                Return mDrawOnlyCurrentGeneration
            End Get
            Set(ByVal value As Boolean)
                mDrawOnlyCurrentGeneration = value
            End Set
        End Property

        Public Sub setStandard()
            Me.MultithreadingAllowed = True
            Me.UseMultithreading = True
            Me.NThreads = Environment.ProcessorCount
            Me.DrawOnlyCurrentGeneration = False
        End Sub
    End Class

    Public General As Settings_General
    Public PES As Settings_PES
    Public HookeJeeves As Settings_HookeJeeves
    Public MetaEvo As Settings_MetaEvo
    Public DDS As Settings_DDS
    Public SensiPlot As Settings_Sensiplot
    Public TSP As Settings_TSP

    Public Sub New()
        Me.General = New Settings_General()
        Me.General.setStandard()
    End Sub

    ''' <summary>
    ''' Saves the current settings to an XML file
    ''' </summary>
    ''' <param name="path">path to file</param>
    Public Sub Save(ByVal path As String)

        Dim writer As IO.StreamWriter
        Dim serializer As XmlSerializer

        'Streamwriter öffnen
        writer = New IO.StreamWriter(path)

        serializer = New XmlSerializer(GetType(Settings), New XmlRootAttribute("Settings"))
        serializer.Serialize(writer, Me)

        writer.Close()

    End Sub

    ''' <summary>
    ''' Loads settings from an XML file
    ''' </summary>
    ''' <param name="path">path to file</param>
    ''' <returns></returns>
    Public Shared Function Load(path As String) As Settings

        Dim serializer As New XmlSerializer(GetType(Common.Settings))
        Dim settings As Settings

        AddHandler serializer.UnknownElement, AddressOf serializerUnknownElement
        AddHandler serializer.UnknownAttribute, AddressOf serializerUnknownAttribute

        'Filestream öffnen
        Dim fs As New IO.FileStream(path, IO.FileMode.Open)

        'Deserialisieren
        'TODO: XmlDeserializationEvents ms-help://MS.VSCC.v90/MS.MSDNQTR.v90.en/fxref_system.xml/html/e0657840-5678-bf57-6e7a-1bd93b2b27d1.htm
        settings = CType(serializer.Deserialize(fs), Common.Settings)

        fs.Close()

        Return settings

    End Function

    'Fehlerbehandlung Serialisierung
    '*******************************
    Private Shared Sub serializerUnknownElement(ByVal sender As Object, ByVal e As XmlElementEventArgs)
        Throw New Exception($"The element '{e.Element.Name}' is unknown!")
    End Sub

    Private Shared Sub serializerUnknownAttribute(ByVal sender As Object, ByVal e As XmlAttributeEventArgs)
        Throw New Exception($"The attribute '{e.Attr.Name}' is unknown!")
    End Sub

End Class
