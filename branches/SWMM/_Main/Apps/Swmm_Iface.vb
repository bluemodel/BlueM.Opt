Module swmm5_iface

    Declare Function swmm_run Lib "D:\Data\BMBF_Odysseus\SWMM\Develop\Evo\_Main\Apps\swmm5.dll" (ByVal F1 As String, ByVal F2 As String, ByVal F3 As String) As Integer
    Declare Function swmm_open Lib "D:\Data\BMBF_Odysseus\SWMM\Develop\Evo\_Main\Apps\swmm5.dll" (ByVal F1 As String, ByVal F2 As String, ByVal F3 As String) As Integer
    Declare Function swmm_start Lib "D:\Data\BMBF_Odysseus\SWMM\Develop\Evo\_Main\Apps\swmm5.dll" (ByVal saveFlag As Integer) As Integer
    Declare Function swmm_step Lib "D:\Data\BMBF_Odysseus\SWMM\Develop\Evo\_Main\Apps\swmm5.dll" (ByRef elapsedTime As Double) As Integer
    Declare Function swmm_end Lib "D:\Data\BMBF_Odysseus\SWMM\Develop\Evo\_Main\Apps\swmm5.dll" () As Integer
    Declare Function swmm_report Lib "D:\Data\BMBF_Odysseus\SWMM\Develop\Evo\_Main\Apps\swmm5.dll" () As Integer
    Declare Function swmm_getMassBalErr Lib "D:\Data\BMBF_Odysseus\SWMM\Develop\Evo\_Main\Apps\swmm5.dll" (ByRef runoffErr As Single, ByRef flowErr As Single, ByRef qualErr As Single) As Integer
    Declare Function swmm_close Lib "D:\Data\BMBF_Odysseus\SWMM\Develop\Evo\_Main\Apps\swmm5.dll" () As Integer
    Declare Function swmm_getVersion Lib "D:\Data\BMBF_Odysseus\SWMM\Develop\Evo\_Main\Apps\swmm5.dll" () As Long

    Private Structure STARTUPINFO
        Dim cb As Integer
        Dim lpReserved As String
        Dim lpDesktop As String
        Dim lpTitle As String
        Dim dwX As Integer
        Dim dwY As Integer
        Dim dwXSize As Integer
        Dim dwYSize As Integer
        Dim dwXCountChars As Integer
        Dim dwYCountChars As Integer
        Dim dwFillAttribute As Integer
        Dim dwFlags As Integer
        Dim wShowWindow As Short
        Dim cbReserved2 As Short
        Dim lpReserved2 As Integer
        Dim hStdInput As Integer
        Dim hStdOutput As Integer
        Dim hStdError As Integer
    End Structure

    Private Structure PROCESS_INFORMATION
        Dim hProcess As Integer
        Dim hThread As Integer
        Dim dwProcessID As Integer
        Dim dwThreadID As Integer
    End Structure

    Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Integer, ByVal dwMilliseconds As Integer) As Integer

    'UPGRADE_WARNING: Für die Struktur PROCESS_INFORMATION müssen Marshalling-Attribute möglicherweise als ein Argument in dieser Declare-Anweisung weitergegeben werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
    'UPGRADE_WARNING: Für die Struktur STARTUPINFO müssen Marshalling-Attribute möglicherweise als ein Argument in dieser Declare-Anweisung weitergegeben werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
    Private Declare Function CreateProcessA Lib "kernel32" (ByVal lpApplicationName As String, ByVal lpCommandLine As String, ByVal lpProcessAttributes As Integer, ByVal lpThreadAttributes As Integer, ByVal bInheritHandles As Integer, ByVal dwCreationFlags As Integer, ByVal lpEnvironment As Integer, ByVal lpCurrentDirectory As String, ByRef lpStartupInfo As STARTUPINFO, ByRef lpProcessInformation As PROCESS_INFORMATION) As Integer

    Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer

    Private Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Integer, ByRef lpExitCode As Integer) As Integer

    Private Const SUBCATCH As Short = 0
    Private Const NODE As Short = 1
    Private Const LINK As Short = 2
    Private Const SYS As Short = 3
    Private Const INFINITE As Short = -1
    Private Const SW_SHOWNORMAL As Short = 1
    Private Const SUBCATCHVARS As Short = 6 ' number of subcatch reporting variable
    Private Const NODEVARS As Short = 6 ' number of node reporting variables
    Private Const LINKVARS As Short = 5 ' number of link reporting variables
    Private Const SYSVARS As Short = 13 ' number of system reporting variables
    Private Const RECORDSIZE As Short = 4 ' number of bytes per file record

    Private Fout As Short ' file handle
    Private StartPos As Integer ' file position where results start
    Private BytesPerPeriod As Integer ' number of bytes used for storing
    ' results in file each reporting period

    Public Nperiods As Integer ' number of reporting periods
    Public FlowUnits As Integer ' flow units code
    Public Nsubcatch As Integer ' number of subcatchments
    Public Nnodes As Integer ' number of drainage system nodes
    Public Nlinks As Integer ' number of drainage system links
    Public Npolluts As Integer ' number of pollutants tracked
    Public StartDate As Double ' start date of simulation
    Public ReportStep As Integer ' reporting time step (seconds)


    Public Function RunSwmmExe(ByRef cmdLine As String) As Integer

        '------------------------------------------------------------------------------
        '  Input:   cmdLine = command line for running the console version of SWMM 5
        '  Output:  returns the exit code generated by running SWMM5.EXE
        '  Purpose: runs the command line version of SWMM 5.
        '------------------------------------------------------------------------------
        Dim pi As PROCESS_INFORMATION
        Dim si As STARTUPINFO
        Dim exitCode As Integer

        ' --- Initialize data structures
        si.cb = Len(si)
        si.wShowWindow = SW_SHOWNORMAL

        ' --- launch swmm5.exe
        exitCode = CreateProcessA(vbNullString, cmdLine, 0, 0, 0, 0, 0, vbNullString, si, pi)

        ' --- wait for program to end
        exitCode = WaitForSingleObject(pi.hProcess, INFINITE)

        ' --- retrieve the error code produced by the program
        'UPGRADE_WARNING: Die Standardeigenschaft des Objekts proc.hProcess konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Call GetExitCodeProcess(pi.hProcess, exitCode)

        ' --- release handles
        Call CloseHandle(pi.hThread)
        Call CloseHandle(pi.hProcess)
        RunSwmmExe = exitCode
    End Function


    Public Function RunSwmmDll(ByRef inpFile As String, ByRef rptFile As String, ByRef OutFile As String) As Integer
        '------------------------------------------------------------------------------
        '  Input:   inpFile = name of SWMM 5 input file
        '           rptFile = name of status report file
        '           outFile = name of binary output file
        '  Output:  returns a SWMM 5 error code or 0 if there are no errors
        '  Purpose: runs the dynamic link library version of SWMM 5.
        '------------------------------------------------------------------------------
        'UPGRADE_NOTE: err wurde aktualisiert auf err_Renamed. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim err_Renamed As Integer
        Dim elapsedTime As Double

        ' --- open a SWMM project
        err_Renamed = swmm_open(inpFile, rptFile, OutFile)
        If err_Renamed = 0 Then

            ' --- initialize all processing systems
            err_Renamed = swmm_start(1)
            If err_Renamed = 0 Then

                ' --- step through the simulation
                Do
                    ' --- allow Windows to process any pending events
                    System.Windows.Forms.Application.DoEvents()

                    ' --- extend the simulation by one routing time step
                    err_Renamed = swmm_step(elapsedTime)

                    '//////////////////////////////////////////
                    ' call a progress reporting function here,
                    ' using elapsedTime as an argument
                    '//////////////////////////////////////////

                Loop While elapsedTime > 0.0# And err_Renamed = 0
            End If

            ' --- close all processing systems
            swmm_end()
        End If

        ' --- close the project
        swmm_close()

        ' --- return the error code
        RunSwmmDll = err_Renamed
    End Function


    Function OpenSwmmOutFile(ByRef OutFile As String) As Integer
        '------------------------------------------------------------------------------
        '  Input:   outFile = name of binary output file
        '  Output:  returns 0 if successful, 1 if binary file invalid because
        '           SWMM 5 ran with errors, or 2 if the file cannot be opened
        '  Purpose: opens the binary output file created by a SWMM 5 run and
        '           retrieves the following simulation data that can be
        '           accessed by the application:
        '           Nperiods = number of reporting periods
        '           FlowUnits = flow units code
        '           Nsubcatch = number of subcatchments
        '           Nnodes = number of drainage system nodes
        '           Nlinks = number of drainage system links
        '           Npolluts = number of pollutants tracked
        '           StartDate = start date of simulation
        '           ReportStep = reporting time step (seconds)
        '------------------------------------------------------------------------------
        Dim magic1 As Integer
        Dim magic2 As Integer
        Dim errCode As Integer
        Dim version As Integer
        'UPGRADE_NOTE: err wurde aktualisiert auf err_Renamed. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim err_Renamed As Short

        ' --- open the output file
        On Error GoTo FINISH
        err_Renamed = 2
        Fout = FreeFile()
        FileOpen(Fout, OutFile, OpenMode.Binary, OpenAccess.Read)

        ' --- check that file contains at least 14 records
        If LOF(1) < 14 * RECORDSIZE Then
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts OpenOutFile konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            OpenSwmmOutFile = 1
            FileClose(1)
            Exit Function
        End If

        ' --- read parameters from end of file
        Seek(Fout, LOF(Fout) - 4 * RECORDSIZE + 1)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, StartPos)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, Nperiods)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, errCode)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, magic2)

        ' --- read magic number from beginning of file
        Seek(Fout, 1)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, magic1)

        ' --- perform error checks
        If magic1 <> magic2 Then
            err_Renamed = 1
        ElseIf errCode <> 0 Then
            err_Renamed = 1
        ElseIf Nperiods = 0 Then
            err_Renamed = 1
        Else
            err_Renamed = 0
        End If

        ' --- quit if errors found
        If err_Renamed > 0 Then
            FileClose((Fout))
            'UPGRADE_WARNING: Die Standardeigenschaft des Objekts OpenOutFile konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            OpenSwmmOutFile = err_Renamed
            Exit Function
        End If

        ' --- otherwise read additional parameters from start of file
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, version)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, FlowUnits)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, Nsubcatch)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, Nnodes)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, Nlinks)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, Npolluts)

        ' --- read data just before start of output results
        Seek(Fout, StartPos - 3 * RECORDSIZE + 1)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, StartDate)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, ReportStep)

        ' --- compute number of bytes stored per reporting period
        BytesPerPeriod = RECORDSIZE * 2
        BytesPerPeriod = BytesPerPeriod + RECORDSIZE * Nsubcatch * (SUBCATCHVARS + Npolluts)
        BytesPerPeriod = BytesPerPeriod + RECORDSIZE * Nnodes * (NODEVARS + Npolluts)
        BytesPerPeriod = BytesPerPeriod + RECORDSIZE * Nlinks * (LINKVARS + Npolluts)
        BytesPerPeriod = BytesPerPeriod + RECORDSIZE * SYSVARS

        ' --- return with file left open
        OpenSwmmOutFile = err_Renamed
        Exit Function

        ' --- error handler
FINISH:
        OpenSwmmOutFile = err_Renamed
        FileClose((Fout))
    End Function


    Function GetSwmmResult(ByVal iType As Integer, ByVal iIndex As Integer, ByVal vIndex As Integer, ByVal period As Integer, ByRef Value As Single) As Short
        '------------------------------------------------------------------------------
        '  Input:   iType = type of object whose value is being sought
        '                   (0 = subcatchment, 1 = node, 2 = link, 3 = system
        '           iIndex = index of item being sought (starting from 0)
        '           vIndex = index of variable being sought (see Interfacing Guide)
        '           period = reporting period index (starting from 1)
        '  Output:  value = value of variable being sought;
        '           function returns 1 if successful, 0 if not
        '  Purpose: finds the result of a specific variable for a given object
        '           at a specified time period.
        '------------------------------------------------------------------------------
        Dim offset As Integer
        Dim X As Single

        '// --- compute offset into output file
        Value = 0.0#
        GetSwmmResult = 0
        offset = StartPos + (period - 1) * BytesPerPeriod + 2 * RECORDSIZE + 1
        If iType = SUBCATCH Then
            offset = offset + RECORDSIZE * (iIndex * (SUBCATCHVARS + Npolluts) + vIndex)
        ElseIf iType = NODE Then
            offset = offset + RECORDSIZE * (Nsubcatch * (SUBCATCHVARS + Npolluts) + iIndex * (NODEVARS + Npolluts) + vIndex)
        ElseIf iType = LINK Then
            offset = offset + RECORDSIZE * (Nsubcatch * (SUBCATCHVARS + Npolluts) + Nnodes * (NODEVARS + Npolluts) + iIndex * (LINKVARS + Npolluts) + vIndex)
        ElseIf iType = SYS Then
            offset = offset + RECORDSIZE * (Nsubcatch * (SUBCATCHVARS + Npolluts) + Nnodes * (NODEVARS + Npolluts) + Nlinks * (LINKVARS + Npolluts) + vIndex)
        Else : Exit Function
        End If

        '// --- re-position the file and read result
        Seek(Fout, offset)
        'UPGRADE_WARNING: Get wurde zu FileGet aktualisiert und hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        FileGet(Fout, X)
        Value = X
        GetSwmmResult = 1
    End Function


    Public Sub CloseSwmmOutFile()
        '------------------------------------------------------------------------------
        '  Input:   none
        '  Output:  none
        '  Purpose: closes the binary output file.
        '------------------------------------------------------------------------------
        FileClose((Fout))
    End Sub
End Module