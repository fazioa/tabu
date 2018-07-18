Imports Microsoft.VisualBasic.FileIO
Public Class H3G_TRAFF
    Dim _MyReader As TextFieldParser
    Dim currentRowFields As String()
    Sub DecodeWindTre(pathNomeFile As String, ByRef _rigaTab As List(Of rigaTabulato), nomeFile As String, gestore As String)
        _MyReader = New FileIO.TextFieldParser(pathNomeFile)
        'imposta le specifiche per il gestore
        Dim specifica As New XControl
        specifica = specifica.XCRead(My.Application.Info.DirectoryPath & ".\specifiche\specificaWindTre.xml")
        SetImporter(_MyReader, specifica)


        currentRowFields = _MyReader.ReadFields
        While Not _MyReader.EndOfData
            Try
                Select Case currentRowFields(0).Trim
                    Case specifica.TitoloTrafficoVoce
                        DecodeWindTreVoce(specifica, _rigaTab, nomeFile, gestore)
                    Case specifica.TitoloTrafficoSMS
                        DecodeWindTreSMS(specifica, _rigaTab, nomeFile, gestore)
                    Case specifica.TitoloTrafficoDati
                        DecodeWindTreDATI(specifica, _rigaTab, nomeFile, gestore)

                    Case specifica.TitoloTrafficoServizi
                        DecodeWindTreDati(specifica, _rigaTab, nomeFile, gestore)
                    Case specifica.TitoloTrafficoVoceOpVirtuali
                        DecodeWindTreVoce(specifica, _rigaTab, nomeFile, gestore)
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

    Sub DecodeWindTreSMS(_specifica As XControl, _rigaTab As List(Of rigaTabulato), _nomeFile As String, _gestore As String)
        'Dim currentRowFields As String()
        Dim riga As rigaTabulato
        Dim bExit As Boolean = False

        'salta una riga
        currentRowFields = _MyReader.ReadFields
        While Not _MyReader.EndOfData And Not bExit = True
            Try
                currentRowFields = _MyReader.ReadFields
            Catch ex As Exception
                MsgBox("Error - File " & _nomeFile & " - Line nr." & _MyReader.LineNumber & " - " & ex.Message)
            End Try

            If (currentRowFields.Length > 1) Then
                Dim i As Integer = 0
                riga = New rigaTabulato
                riga.Gestore = _gestore
                riga.NomeFile = System.IO.Path.GetFileName(_nomeFile)


                For Each campo In _specifica.CampiSMS
                    Select Case campo
                        Case "Mittente"
                            riga.Chiamante = currentRowFields(i)
                        Case "Destinatario"
                            riga.Chiamato = currentRowFields(i)
                        Case "DataOra"
                            riga.DataOra = currentRowFields(i)
                        Case "Tipo"
                            riga.Tipologia = GetTipoComunicazione(_specifica, currentRowFields(i))
                            riga.Codice_tipo_chiamata = currentRowFields(i)
                        Case "Stato"
                            riga.CodiceStatoSMS = currentRowFields(i)
                        Case "Imsi"
                            riga.Imsi_chiamante = currentRowFields(i)

                        Case "Imei"
                            riga.Imei_chiamante = currentRowFields(i)
                        Case "CellaMittente"
                            riga.CellaChiamante_fine = currentRowFields(i)

                        Case "CellaDestinatario"
                            riga.CellaChiamato_inizio = currentRowFields(i)
                        Case "Rete Mittente"
                            riga.Rete = currentRowFields(i)
                        Case "Rete Destinatario"
                            riga.ReteDestinatario = currentRowFields(i)
                    End Select
                    i = i + 1
                Next
                _rigaTab.Add(riga)
            Else
                'se la lunghezza della lista campi è uno vuol dire che abbiamo raggiunto la fine del gruppo di righe
                bExit = True
            End If
        End While
    End Sub

    Sub DecodeWindTreVoce(_specifica As XControl, ByRef _rigaTab As List(Of rigaTabulato), _nomeFile As String, _gestore As String)

        Dim riga As rigaTabulato
        Dim bExit As Boolean = False

        'salta una riga
        currentRowFields = _MyReader.ReadFields
        While Not _MyReader.EndOfData And Not bExit = True
            currentRowFields = _MyReader.ReadFields
            If (currentRowFields.Length > 1) Then
                Dim i As Integer = 0
                riga = New rigaTabulato
                riga.Gestore = _gestore
                riga.NomeFile = System.IO.Path.GetFileName(_nomeFile)


                For Each campo In _specifica.CampiVoce

                    Select Case campo
                        Case "Chiamante"
                            riga.Chiamante = currentRowFields(i)
                        Case "Chiamato"
                            riga.Chiamato = currentRowFields(i)
                        Case "DataOra"
                            riga.DataOra = currentRowFields(i)
                        Case "Durata"
                            riga.Durata = currentRowFields(i)
                        Case "Tipo"
                            riga.Tipologia = GetTipoComunicazione(_specifica, currentRowFields(i))
                            riga.Codice_tipo_chiamata = currentRowFields(i)
                        Case "Rete"
                            riga.Rete = currentRowFields(i)
                        Case "Imsi"
                            If (IsDatiChiamato(_specifica, riga.Codice_tipo_chiamata)) Then
                                riga.Imsi_chiamato = currentRowFields(i)
                            Else
                                riga.Imsi_chiamante = currentRowFields(i)

                            End If
                        Case "Imei"
                            If (IsDatiChiamato(_specifica, riga.Codice_tipo_chiamata)) Then
                                riga.Imei_chiamato = currentRowFields(i)
                            Else
                                riga.Imei_chiamante = currentRowFields(i)
                            End If
                        Case "DescrizioneCellaInizioFine"
                            If (IsDatiChiamato(_specifica, riga.Codice_tipo_chiamata)) Then
                                riga.DescrizioneCellaInizioFine_Chiamato = currentRowFields(i)
                            Else
                                riga.DescrizioneCellaInizioFine_Chiamante = currentRowFields(i)
                            End If
                    End Select
                    i = i + 1
                Next
                _rigaTab.Add(riga)
            Else
                'se la lunghezza della lista campi è uno vuol dire che abbiamo raggiunto la fine del gruppo di righe
                bExit = True
            End If
        End While
    End Sub

    Sub DecodeWindTreDati(_specifica As XControl, ByRef _rigaTab As List(Of rigaTabulato), _nomeFile As String, _gestore As String)

        Dim riga As rigaTabulato
        Dim bExit As Boolean = False

        'salta una riga
        currentRowFields = _MyReader.ReadFields
        While Not _MyReader.EndOfData And Not bExit = True
            currentRowFields = _MyReader.ReadFields
            If (currentRowFields.Length > 1) Then
                Dim i As Integer = 0
                riga = New rigaTabulato
                riga.Gestore = _gestore
                riga.NomeFile = System.IO.Path.GetFileName(_nomeFile)

                For Each campo In _specifica.CampiDati

                    Select Case campo
                        Case "Chiamante"
                            riga.Chiamante = currentRowFields(i)
                        Case "Apn"
                            riga.Chiamato = currentRowFields(i)
                        Case "Data"
                            riga.DataOra = currentRowFields(i)
                        Case "Durata Sec"
                            riga.Durata = currentRowFields(i)
                        Case "Tipo"
                            riga.Tipologia = GetTipoComunicazione(_specifica, currentRowFields(i))
                            riga.Codice_tipo_chiamata = currentRowFields(i)
                        Case "Imsi"
                            riga.Imsi_chiamante = currentRowFields(i)
                        Case "Imei"
                            riga.Imei_chiamante = currentRowFields(i)
                        Case "DescrizioneCellaInizioFine"
                            riga.DescrizioneCellaInizioFine_Chiamante = currentRowFields(i)
                        Case "Rete"
                            riga.Rete = currentRowFields(i)
                    End Select
                    i = i + 1
                Next
                _rigaTab.Add(riga)
            Else
                'se la lunghezza della lista campi è uno vuol dire che abbiamo raggiunto la fine del gruppo di righe
                bExit = True
            End If
        End While
    End Sub


    Private Function IsDatiChiamato(_specifica As XControl, tipo As String) As Boolean
        Dim _dettagliChiamatoListaSigle As List(Of String) = _specifica.Tipo.DettaglioDatiChiamato
        Dim sVal As String
        For Each sVal In _dettagliChiamatoListaSigle

            If (tipo.Contains(sVal)) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function GetTipoComunicazione(_specifica As XControl, sValore As String) As String
        Dim _voceListaSigle As List(Of String) = _specifica.Tipo.Voce
        If (_voceListaSigle.Contains(sValore)) Then
            Return "Voce"
        Else
            Dim _smsListaSigle As List(Of String) = _specifica.Tipo.Sms
            If (_smsListaSigle.Contains(sValore)) Then
                Return "SMS"
            Else
                Dim _datiListaSigle As List(Of String) = _specifica.Tipo.Dati
                If (_datiListaSigle.Contains(sValore)) Then
                    Return "Dati"
                Else
                    Dim _altroListaSigle As List(Of String) = _specifica.Tipo.Altro
                    If (_altroListaSigle.Contains(sValore)) Then
                        Return "Altro"
                    End If
                End If
            End If
        End If
        Return "non definita"
    End Function

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
