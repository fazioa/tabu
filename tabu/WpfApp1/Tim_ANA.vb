Imports Microsoft.VisualBasic.FileIO
Imports tabu

Public Class Tim_anagrafica
    Dim _MyReader As TextFieldParser
    Dim currentRowFields As String()
    Sub DecodeTim(pathNomeFile As String, ByRef _rigaAnag As List(Of rigaAnagrafica), nomeFile As String, gestore As String)
        _MyReader = New FileIO.TextFieldParser(pathNomeFile)
        'imposta le specifiche per il gestore
        Dim specifica As New XControl
        specifica = specifica.XCRead(My.Application.Info.DirectoryPath & ".\specifiche\specificaTimAnagrafica.xml")
        _MyReader.TextFieldType = FileIO.FieldType.Delimited
        _MyReader.SetDelimiters(":")


        While Not _MyReader.EndOfData
            Try
                currentRowFields = read(_MyReader)
            Catch ex As Exception
                MsgBox("Error - File " & nomeFile & " - Line nr." & _MyReader.LineNumber & " - " & ex.Message)
            End Try
            Try

                Select Case currentRowFields(0).Trim
                    Case specifica.TitoloAnagrafica
                        SetImporter(_MyReader, specifica)
                        DecodeTimAnagrafica(specifica, _rigaAnag, nomeFile, gestore)
                End Select

            Catch ex As Microsoft.VisualBasic.
                       FileIO.MalformedLineException
                MsgBox("File " & nomeFile & " - Line " & _MyReader.LineNumber & " - " & ex.Message & "is not valid and will be skipped.")
            End Try
        End While
    End Sub


    Sub DecodeTimAnagrafica(_specifica As XControl, ByRef _rigaAna As List(Of rigaAnagrafica), _nomeFile As String, _gestore As String)
        Dim riga As rigaAnagrafica
        Dim bExit As Boolean = False 'usata per l'uscita dal ciclo in caso di stringa di fine report

        Dim sLine As String

        While Not _MyReader.EndOfData And Not bExit
            Try
                riga = New rigaAnagrafica
                riga.Gestore = _gestore
                riga.NomeFile = System.IO.Path.GetFileName(_nomeFile)

                sLine = _MyReader.ReadFields(0) '_MyReader.ReadLine() - non usare il trim, altrimenti viene eliminato il primo campo, che generalmente è vuoto

                If (sLine.Equals(_specifica.FineReport)) Then
                    bExit = True
                Else
                    leggiRiga1(sLine, riga, _specifica)
                End If

                sLine = _MyReader.ReadFields(0)
                If (sLine.Equals(_specifica.FineReport) Or bExit) Then
                    bExit = True
                Else
                    leggiRiga2(sLine, riga, _specifica)
                End If

                sLine = _MyReader.ReadFields(0)
                If (sLine.Equals(_specifica.FineReport) Or bExit) Then
                    bExit = True
                Else
                    leggiRiga3(sLine, riga, _specifica)
                End If


                _rigaAna.Add(riga)

            Catch ex As MalformedLineException
                MsgBox("Error - File " & _nomeFile & " - Line nr." & _MyReader.LineNumber & " - " & ex.Message & " - " & _MyReader.ErrorLine)
            End Try
        End While
    End Sub

    Private Sub leggiRiga1(sLine As String, riga As rigaAnagrafica, _specifica As XControl)
        Dim i As Integer = 0
        Dim riga1 = _specifica.DelimitatoriFissiRiga1
        If (sLine.Length > 0) Then
            For Each campo In _specifica.CampiAnagraficaRiga1
                'sDato è il dato estrapolato dal file di testo che dovrà essere inserito nella struttura riga
                Dim sDato As String

                If (riga1.Count > i) Then
                    If (riga1.Item(i + 1) < sLine.Length) Then
                        sDato = sLine.Substring(riga1.Item(i) - 1, riga1.Item(i + 1) - riga1.Item(i)).Trim
                    Else
                        sDato = sLine.Substring(riga1.Item(i) - 1, sLine.Length - riga1.Item(i)).Trim
                    End If

                    Select Case campo
                        Case "MSISDN Master"
                            riga.Utenza = sDato
                        Case "SN"
                            riga.Stato = sDato
                        Case "Data Attivazione"
                            riga.DataAttivazione = sDato
                        Case "Data variazione/ultima operazione"
                            'valutare se inserire
                    End Select
                    i = i + 2
                End If
            Next
        End If

    End Sub

    Private Sub leggiRiga2(sLine As String, riga As rigaAnagrafica, _specifica As XControl)
        Dim i As Integer = 0
        Dim delitatori_riga = _specifica.DelimitatoriFissiRiga2

        If (sLine.Length > 0) Then

            For Each campo In _specifica.CampiAnagraficaRiga2
                'sDato è il dato estrapolato dal file di testo che dovrà essere inserito nella struttura riga
                Dim sDato As String
                If (delitatori_riga.Count > i) Then
                    If (delitatori_riga.Item(i + 1) < sLine.Length) Then
                        sDato = sLine.Substring(delitatori_riga.Item(i) - 1, delitatori_riga.Item(i + 1) - delitatori_riga.Item(i)).Trim
                    Else
                        sDato = sLine.Substring(delitatori_riga.Item(i) - 1, sLine.Length - delitatori_riga.Item(i)).Trim
                    End If

                    Select Case campo
                        Case "Cognome Nome/Rag.Sociale"
                            riga.DatiAnagrafici = sDato
                        Case "Codice Fiscale/P.IVA"
                            riga.Codicefiscale = sDato
                    End Select
                    i = i + 2
                End If
            Next
        End If
    End Sub
    Private Sub leggiRiga3(sLine As String, riga As rigaAnagrafica, _specifica As XControl)
        Dim i As Integer = 0
        Dim delitatori_riga = _specifica.DelimitatoriFissiRiga3
        If (sLine.Length > 0) Then
            For Each campo In _specifica.CampiAnagraficaRiga3
                'sDato è il dato estrapolato dal file di testo che dovrà essere inserito nella struttura riga
                Dim sDato As String
                If (delitatori_riga.Item(i + 1) < sLine.Length) Then
                    sDato = sLine.Substring(delitatori_riga.Item(i) - 1, delitatori_riga.Item(i + 1) - delitatori_riga.Item(i)).Trim
                Else
                    sDato = sLine.Substring(delitatori_riga.Item(i) - 1, sLine.Length - delitatori_riga.Item(i)).Trim
                End If

                Select Case campo
                    Case "Indirizzo"
                        riga.Indirizzo = sDato
                End Select
                i = i + 2
            Next
        End If
    End Sub

    Private Sub SetImporter(ByRef _MyReader As FileIO.TextFieldParser, ByRef _specifica As XControl)
        _MyReader.HasFieldsEnclosedInQuotes = False
        '_MyReader.CommentTokens = New String() {""""}
        If (_specifica.delimitato) Then
            _MyReader.TextFieldType = FileIO.FieldType.Delimited
            _MyReader.SetDelimiters(_specifica.delimitatore) ' "|" per wind
        Else
            _MyReader.TextFieldType = FileIO.FieldType.FixedWidth
        End If
        If (_specifica.trimWhiteSpace = True) Then
            _MyReader.TrimWhiteSpace() = True
        End If
    End Sub

    Private Function read(_myReader As TextFieldParser) As String()
        Return _myReader.ReadFields()
    End Function

End Class
