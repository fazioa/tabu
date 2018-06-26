Imports Microsoft.VisualBasic.FileIO
Public Class H3G_ANA
    Dim _MyReader As TextFieldParser
    Dim currentRowFields As String()
    Sub DecodeWindTre(pathNomeFile As String, ByRef _rigaAnag As List(Of rigaAnagrafica), nomeFile As String, gestore As String)
        _MyReader = New FileIO.TextFieldParser(pathNomeFile)
        'imposta le specifiche per il gestore
        Dim specifica As New XControl
        specifica = specifica.XCRead(My.Application.Info.DirectoryPath & ".\specifiche\specificaWindTre.xml")
        SetImporter(_MyReader, specifica)


        currentRowFields = _MyReader.ReadFields
        While Not _MyReader.EndOfData
            Try
                Select Case currentRowFields(0).Trim
                    Case specifica.TitoloAnagrafica
                        DecodeWindTreAnagrafica(specifica, _rigaAnag, nomeFile, gestore)
                    Case Else
                        currentRowFields = _MyReader.ReadFields
                End Select

            Catch ex As Microsoft.VisualBasic.
                       FileIO.MalformedLineException
                MsgBox("Line " & _MyReader.LineNumber & " - " & ex.Message &
                "is not valid and will be skipped.")
            End Try
        End While


    End Sub


    Sub DecodeWindTreAnagrafica(_specifica As XControl, ByRef _rigaAna As List(Of rigaAnagrafica), _nomeFile As String, _gestore As String)

        Dim riga As rigaAnagrafica
        Dim bExit As Boolean = False

        'salta una riga
        currentRowFields = _MyReader.ReadFields
        While Not _MyReader.EndOfData And Not bExit = True
            currentRowFields = _MyReader.ReadFields
            If (currentRowFields.Length > 1) Then
                Dim i As Integer = 0
                riga = New rigaAnagrafica
                riga.Gestore = _gestore
                riga.NomeFile = System.IO.Path.GetFileName(_nomeFile)

                For Each campo In _specifica.CampiAnagrafica
                    Select Case campo
                        Case "Usim"
                            riga.IMSI = currentRowFields(i)
                        Case "Msisdn"
                            riga.Utenza = currentRowFields(i)
                        Case "Nome"
                            riga.DatiAnagrafici = currentRowFields(i + 1) + " " + currentRowFields(i) 'cognome e nome
                              i = i + 1
                        Case "Rag. Soc."
                            riga.Societa = currentRowFields(i)
                        Case "CF"
                            riga.Codicefiscale = currentRowFields(i)
                        Case "Data Nascita"
                            riga.DataNascita = currentRowFields(i)
                        Case "Citta Nascita"
                            riga.LuogoNascita = currentRowFields(i)
                        Case "Residenza"
                            riga.Indirizzo = currentRowFields(i)
                        Case "Stato Usim"
                            riga.Stato = currentRowFields(i)
                        Case "Data Attivazione"
                            riga.DataAttivazione = currentRowFields(i)
                        Case "Data Cessazione"
                            riga.DataDisattivazione = currentRowFields(i)
                        Case "Dealer"
                            riga.DealerAttivazione = currentRowFields(i)

                    End Select
                    i = i + 1
                Next
                _rigaAna.Add(riga)
            Else
                'se la lunghezza della lista campi è uno vuol dire che abbiamo raggiunto la fine del gruppo di righe
                bExit = True
            End If
        End While
    End Sub

    Private Sub SetImporter(ByRef _MyReader As FileIO.TextFieldParser, ByRef _specifica As XControl)
        _MyReader.HasFieldsEnclosedInQuotes = False
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

End Class
