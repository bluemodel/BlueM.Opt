Imports System.Xml.Serialization

Public Class PES_Settings

    '*******************************************************************************
    '*******************************************************************************
    '**** Klasse PES_Settings                                                   ****
    '**** zum Speichern aller PES-Einstellungen aus dem Form                    ****
    '****                                                                       ****
    '**** Fachgebiet Ingenieurhydrologie und Wasserbewirtschaftung              ****
    '**** TU Darmstadt                                                          ****
    '*******************************************************************************
    '*******************************************************************************

    Public is_MO_Pareto As Boolean             'Multi-Objective mit Pareto Front
    Public ty_EvoTyp As EVO_STRATEGIE          'Typ der Evolutionsstrategie (+ oder ,)
    Public ty_StartPar As EVO_STARTPARAMETER   'Startparameter
    Public DnStart As Double                   'Startschrittweite
    Public is_DnVektor As Boolean              'Soll ein Schrittweitenvektor benutzt werden

    Public n_Gen As Integer                    'Anzahl Generationen
    Public n_Eltern As Integer                 'Anzahl Eltern
    Public n_Nachf As Integer                  'Anzahl Kinder

    Public is_Interact As Boolean              'Mit Austausch zwischen Population und Sekundärer Population
    Public n_Interact As Integer               'Alle wieviel Generationen soll die aktuelle Population mit Mitgliedern der sekundären Population aufgefüllt werden

    Public n_MemberSekPop As Integer           'Maximale Anzahl Mitglieder der Sekundärpopulation

    Public ty_OptEltern As EVO_ELTERN          'Ermittlung der Individuum-Eltern
    Public n_RekombXY As Integer               'X/Y-Schema Rekombination

    Public Pop As Struct_Pop_Settings
    Public Structure Struct_Pop_Settings
        <XmlAttribute()> _
        Public is_POPUL As Boolean                 'Mit Populationen
        Public n_Runden As Integer                 'Anzahl Runden
        Public n_Popul As Integer                  'Anzahl Populationen
        Public n_PopEltern As Integer              'Anzahl Populationseltern
        Public ty_OptPopEltern As EVO_POP_ELTERN   'Ermittlung der Populationseltern
        Public ty_PopEvoTyp As EVO_STRATEGIE       'Typ der Evolutionsstrategie (+ oder ,) auf Populationsebene
        Public ty_PopPenalty As EVO_POP_PENALTY    'Art der Beurteilung der Populationsgüte (Multiobjective)
    End Structure

End Class
