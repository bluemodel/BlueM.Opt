Imports System.Xml.Serialization

Public Class Settings_PES

    Public OptModus As EVO_MODUS                    'Single- oder Multi-Objective
    Public Strategie As EVO_STRATEGIE               'Typ der Evolutionsstrategie (+ oder ,)
    Public Startparameter As EVO_STARTPARAMETER     'Startparameter

    Public Schrittweite As Struct_Schrittweite

    Public Structure Struct_Schrittweite
        Public DnStart As Double                   'Startschrittweite
        Public DnEpsilon As Double                 'Minimale Schrittweite
        Public isDnVektor As Boolean               'Soll ein Schrittweitenvektor benutzt werden
        Public DnMutation As EVO_DnMutation        'Art der Mutation
        Public DnC As Double                       'Skalierung des learning Parameters
    End Structure

    Public n_Gen As Integer                    'Anzahl Generationen
    Public n_Eltern As Integer                 'Anzahl Eltern
    Public n_Nachf As Integer                  'Anzahl Kinder

    Public SekPop As Struct_SekPop

    Public Structure Struct_SekPop
        Public is_Interact As Boolean              'Mit Austausch zwischen Population und Sekundärer Population
        Public n_Interact As Integer               'Alle wieviel Generationen soll die aktuelle Population mit Mitgliedern der sekundären Population aufgefüllt werden
        Public is_Begrenzung As Boolean            'Soll die Anzahl Mitglieder in der SekPop begrenzt werden?
        Public n_MaxMembers As Integer             'Maximale Anzahl Mitglieder der Sekundärpopulation
    End Structure

    Public OptEltern As EVO_ELTERN             'Ermittlung der Individuum-Eltern
    Public n_RekombXY As Integer               'X/Y-Schema Rekombination
    Public is_DiversityTournament As Boolean   'Vor der eigentlichen Auswahl eines Elter wird zunächst nach der besseren Diversity geschaut

    Public Pop As Struct_Pop_Settings

    Public Structure Struct_Pop_Settings
        <XmlAttribute()> _
        Public is_POPUL As Boolean                  'Mit Populationen
        Public n_Runden As Integer                  'Anzahl Runden
        Public n_Popul As Integer                   'Anzahl Populationen
        Public n_PopEltern As Integer               'Anzahl Populationseltern
        Public PopEltern As EVO_POP_ELTERN          'Ermittlung der Populationseltern
        Public PopStrategie As EVO_STRATEGIE        'Typ der Evolutionsstrategie (+ oder ,) auf Populationsebene
        Public PopPenalty As EVO_POP_PENALTY        'Art der Beurteilung der Populationsgüte (Multiobjective)
    End Structure

    'Standardwerte setzen
    '********************
    Public Sub setStandard(ByVal modus As EVO_MODUS)

        Me.OptModus = modus

        Select Case Me.OptModus

            Case EVO_MODUS.Single_Objective

                Me.OptModus = EVO_MODUS.Single_Objective
                Me.Strategie = EVO_STRATEGIE.Plus_Strategie
                Me.Startparameter = EVO_STARTPARAMETER.Original

                Me.Schrittweite.DnMutation = EVO_DnMutation.Schwefel
                Me.Schrittweite.DnStart = 0.1
                Me.Schrittweite.DnEpsilon = 0.001
                Me.Schrittweite.isDnVektor = False
                Me.Schrittweite.DnC = 1.0

                Me.n_Gen = 100
                Me.n_Eltern = 3
                Me.n_Nachf = 10

                Me.SekPop.is_Interact = False
                Me.SekPop.n_Interact = 0
                Me.SekPop.is_Begrenzung = False
                Me.SekPop.n_MaxMembers = 0

                Me.OptEltern = EVO_ELTERN.XX_Mitteln_Diskret
                Me.n_RekombXY = 3
                Me.is_DiversityTournament = False

                Me.Pop.is_POPUL = False
                Me.Pop.n_Runden = 10
                Me.Pop.n_Popul = 3
                Me.Pop.n_PopEltern = 2
                Me.Pop.PopEltern = EVO_POP_ELTERN.Rekombination
                Me.Pop.PopStrategie = EVO_STRATEGIE.Plus_Strategie
                Me.Pop.PopPenalty = EVO_POP_PENALTY.Mittelwert


            Case EVO_MODUS.Multi_Objective

                Me.OptModus = EVO_MODUS.Multi_Objective
                Me.Strategie = EVO_STRATEGIE.Plus_Strategie
                Me.Startparameter = EVO_STARTPARAMETER.Original

                Me.Schrittweite.DnStart = 0.1
                Me.Schrittweite.isDnVektor = False
                Me.Schrittweite.isDnVektor = False
                Me.Schrittweite.DnC = 1.0

                Me.n_Gen = 100
                Me.n_Eltern = 15
                Me.n_Nachf = 50

                Me.SekPop.is_Interact = True
                Me.SekPop.n_Interact = 10
                Me.SekPop.is_Begrenzung = True
                Me.SekPop.n_MaxMembers = 50

                Me.OptEltern = EVO_ELTERN.XX_Mitteln_Diskret
                Me.n_RekombXY = 3
                Me.is_DiversityTournament = True

                Me.Pop.is_POPUL = False
                Me.Pop.n_Runden = 1
                Me.Pop.n_Popul = 1
                Me.Pop.n_PopEltern = 1
                Me.Pop.PopEltern = EVO_POP_ELTERN.Rekombination
                Me.Pop.PopStrategie = EVO_STRATEGIE.Plus_Strategie
                Me.Pop.PopPenalty = EVO_POP_PENALTY.Mittelwert

        End Select
    End Sub

End Class
