Imports Microsoft.VisualBasic.FileIO
Public Class Wind
    Dim _MyReader As TextFieldParser
    Dim currentRowFields As String()
    Dim iRigheDecodeWind As ULong = 0

    Function DecodeWind(pathNomeFile As String, ByRef _rigaTab As List(Of rigaTabulato), ByRef _rigaAnag As List(Of rigaAnagrafica), nomeFile As String, gestore As String)
        _MyReader = New FileIO.TextFieldParser(pathNomeFile)
        'imposta le specifiche per il gestore
        Dim specifica As New XControl
        specifica = specifica.XCRead(My.Application.Info.DirectoryPath & ".\specifiche\specificaWind.xml")
        SetImporter(_MyReader, specifica)

        While Not _MyReader.EndOfData
            Try
                currentRowFields = read(_MyReader)
            Catch ex As Exception
                MsgBox("Error - File " & nomeFile & " - Line nr." & _MyReader.LineNumber & " - " & ex.Message)
            End Try
            Try

                Select Case currentRowFields(0).Trim
                    Case specifica.TitoloTrafficoVoce
                        currentRowFields = read(_MyReader)
                        Select Case currentRowFields(0).Trim
                            Case specifica.SottoTitoloTrafficoVoce
                                iRigheDecodeWind = DecodeWindVoce(specifica, _rigaTab, nomeFile, gestore)
                            Case specifica.SottoTitoloTrafficoDati
                                iRigheDecodeWind = DecodeWindDati(specifica, _rigaTab, nomeFile, gestore)
                        End Select
                End Select
                'in uscita è possibile agganciare l'anagrafica con un secondo Select
                Select Case currentRowFields(0).Trim
                    Case specifica.TitoloAnagrafica
                        DecodeWindAnagrafica(specifica, _rigaAnag, nomeFile, gestore)
                End Select

            Catch ex As Microsoft.VisualBasic.
                       FileIO.MalformedLineException
                MsgBox("File " & nomeFile & " - Line " & _MyReader.LineNumber & " - " & ex.Message & "is not valid and will be skipped.")
            End Try
        End While
        Return iRigheDecodeWind
    End Function

    Sub DecodeWindAnagrafica(_specifica As XControl, ByRef _rigaAnag As List(Of rigaAnagrafica), _nomeFile As String, _gestore As String)
        Dim rigaAnagrafica As rigaAnagrafica
        Dim bExit As Boolean = False
        'salta una riga
        currentRowFields = read(_MyReader)
        While Not _MyReader.EndOfData And Not bExit = True
            currentRowFields = read(_MyReader)
            If (currentRowFields.Length > 1) Then
                Dim i As Integer = 0
                rigaAnagrafica = New rigaAnagrafica
                rigaAnagrafica.Gestore = _gestore
                rigaAnagrafica.NomeFile = System.IO.Path.GetFileName(_nomeFile)

                For Each campo In _specifica.CampiAnagrafica
                    Select Case campo
                        Case "UTENZA"
                            rigaAnagrafica.Utenza = currentRowFields(i)
                        Case "DATI_ANAGRAFICI"
                            rigaAnagrafica.DatiAnagrafici = currentRowFields(i)
                        Case "DATA DI NASCITA"
                            rigaAnagrafica.DataNascita = currentRowFields(i)
                        Case "LUOGO DI NASCITA"
                            rigaAnagrafica.LuogoNascita = currentRowFields(i)
                        Case "C.F."
                            rigaAnagrafica.Codicefiscale = currentRowFields(i)
                        Case "INDIRIZZO"
                            rigaAnagrafica.Indirizzo = currentRowFields(i)
                        Case "SOCIETA"
                            rigaAnagrafica.Societa = currentRowFields(i)
                        Case "IMSI"
                            rigaAnagrafica.IMSI = currentRowFields(i)
                        Case "DEALER_ATT"
                            rigaAnagrafica.DealerAttivazione = currentRowFields(i)
                        Case "DEALER_VEND"
                            rigaAnagrafica.DealerVendita = currentRowFields(i)
                        Case "DATA ATT"
                            rigaAnagrafica.DataAttivazione = currentRowFields(i)
                        Case "DATA DISATT"
                            rigaAnagrafica.DataDisattivazione = currentRowFields(i)
                        Case "STATO"
                            rigaAnagrafica.Stato = currentRowFields(i)
                    End Select
                    i = i + 1
                Next
                _rigaAnag.Add(rigaAnagrafica)
            Else
                'se la lunghezza della lista campi è uno vuol dire che abbiamo raggiunto la fine del gruppo di righe
                bExit = True
            End If
        End While
    End Sub



    Function DecodeWindVoce(_specifica As XControl, ByRef _rigaTab As List(Of rigaTabulato), _nomeFile As String, _gestore As String)
        Dim riga As rigaTabulato
        Dim bExit As Boolean = False
        Dim iRigheDecodeVoce As ULong = 0

        Const NUMERO_CAMPO_TIPO_CHIAMATA As Integer = 20
        'salta una riga
        ' currentRowFields = _MyReader.ReadFields
        While Not _MyReader.EndOfData And Not bExit = True
            currentRowFields = read(_MyReader)
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
                            Dim dataora As Date = Convert.ToDateTime(currentRowFields(i) + " " + Strings.Replace(currentRowFields(i + 1), ".", ":"))
                            riga.DataOra = dataora
                            i = i + 1
                        Case "Durata"
                            If (currentRowFields(i).Equals("")) Then
                                riga.Durata = 0
                            Else
                                riga.Durata = currentRowFields(i)
                            End If
                        Case "Tipo"
                            riga.Tipologia = GetTipoComunicazione(_specifica, currentRowFields(i))
                            riga.Codice_tipo_chiamata = currentRowFields(i)
                        Case "Imsi_chiamante"
                            riga.Imsi_chiamante = currentRowFields(i)
                        Case "Imsi_chiamato"
                            riga.Imsi_chiamato = currentRowFields(i)
                        Case "Imei_chiamante"
                            riga.Imei_chiamante = currentRowFields(i)
                        Case "Imei_chiamato"
                            riga.Imei_chiamato = currentRowFields(i)
                        Case "CellaInizio"
                            If (IsDatiChiamato(_specifica, currentRowFields(NUMERO_CAMPO_TIPO_CHIAMATA))) Then
                                riga.CellaChiamato_inizio = currentRowFields(i)
                            Else
                                riga.CellaChiamante_inizio = currentRowFields(i)
                            End If
                        Case "CellaFine"
                            If (IsDatiChiamato(_specifica, currentRowFields(NUMERO_CAMPO_TIPO_CHIAMATA))) Then
                                riga.CellaChiamato_fine = currentRowFields(i)
                            Else
                                riga.CellaChiamante_fine = currentRowFields(i)
                            End If
                        Case "DescrizioneCellaInizio"
                            If (IsDatiChiamato(_specifica, currentRowFields(NUMERO_CAMPO_TIPO_CHIAMATA))) Then
                                riga.DescrizioneCellaInizioFine_Chiamato = currentRowFields(i) & " - " & currentRowFields(i + 1)
                            Else
                                riga.DescrizioneCellaInizioFine_Chiamante = currentRowFields(i) & " - " & currentRowFields(i + 1)
                            End If
                            i = i + 1
                        Case "DescrizioneCellaFine"
                            If ((Not currentRowFields(i).Equals("")) Or (Not currentRowFields(i + 1).Equals(""))) Then
                                If (IsDatiChiamato(_specifica, currentRowFields(NUMERO_CAMPO_TIPO_CHIAMATA))) Then
                                    riga.DescrizioneCellaInizioFine_Chiamato = riga.DescrizioneCellaInizioFine_Chiamato & " - " & currentRowFields(i) & " - " & currentRowFields(i + 1)
                                Else
                                    riga.DescrizioneCellaInizioFine_Chiamante = riga.DescrizioneCellaInizioFine_Chiamante & " - " & currentRowFields(i) & " - " & currentRowFields(i + 1)
                                End If
                            End If
                            i = i + 1
                        Case "TipoTraffico"
                            riga.Tipologia = GetTipoComunicazione(_specifica, currentRowFields(i))
                            riga.Codice_tipo_chiamata = currentRowFields(i)
                    End Select
                    i = i + 1
                Next
                _rigaTab.Add(riga)
                iRigheDecodeVoce = iRigheDecodeVoce + 1
            Else
                'se la lunghezza della lista campi è uno vuol dire che abbiamo raggiunto la fine del gruppo di righe
                bExit = True
            End If
        End While
        Return iRigheDecodeVoce
    End Function

    Function DecodeWindDati(_specifica As XControl, ByRef _rigaTab As List(Of rigaTabulato), _nomeFile As String, _gestore As String)
        Dim riga As rigaTabulato
        Dim bExit As Boolean = False
        Dim iRigheDecodeDati As ULong = 0

        'salta una riga
        ' currentRowFields = _MyReader.ReadFields
        While Not _MyReader.EndOfData And Not bExit = True
            currentRowFields = read(_MyReader)
            If (currentRowFields.Length > 1) Then
                Dim i As Integer = 0
                riga = New rigaTabulato
                riga.Gestore = _gestore
                riga.NomeFile = System.IO.Path.GetFileName(_nomeFile)


                For Each campo In _specifica.CampiDati

                    Select Case campo
                        Case "Chiamante"
                            riga.Chiamante = currentRowFields(i)
                        Case "Chiamato"
                            riga.Chiamato = currentRowFields(i)
                        Case "DataOra"
                            Dim dd As String = Strings.Left(currentRowFields(i), 2)
                            Dim month As String = Strings.Mid(currentRowFields(i), 4, 2)
                            Dim yyyy As String = Strings.Right(currentRowFields(i), 4)

                            Dim hh As String = Strings.Left(currentRowFields(i + 1), 2)
                            Dim mm As String = Strings.Mid(currentRowFields(i + 1), 3, 2)
                            Dim ss As String = Strings.Right(currentRowFields(i + 1), 2)

                            Dim dataora As Date = New Date(yyyy, month, dd, hh, mm, ss)
                            riga.DataOra = dataora
                            i = i + 1
                        Case "Durata"
                            If (currentRowFields(i).Equals("")) Then
                                riga.Durata = 0
                            Else
                                riga.Durata = currentRowFields(i)
                            End If
                        Case "Tipo"
                            riga.Tipologia = GetTipoComunicazione(_specifica, currentRowFields(i))
                            riga.Codice_tipo_chiamata = currentRowFields(i)
                        Case "Imsi_chiamante"
                            riga.Imsi_chiamante = currentRowFields(i)
                        Case "Imei_chiamante"
                            riga.Imei_chiamante = currentRowFields(i)
                        Case "CellaInizio"
                            riga.CellaChiamante_inizio = currentRowFields(i)
                        Case "CellaFine"
                            riga.CellaChiamante_fine = currentRowFields(i)
                        Case "DescrizioneCellaInizio"
                            riga.DescrizioneCellaInizioFine_Chiamante = currentRowFields(i) & " - " & currentRowFields(i + 1)
                            i = i + 1
                        Case "DescrizioneCellaFine"
                            riga.DescrizioneCellaInizioFine_Chiamante = riga.DescrizioneCellaInizioFine_Chiamante & " - " & currentRowFields(i) & " - " & currentRowFields(i + 1)
                            i = i + 1
                        Case "TipoTraffico"
                            riga.Tipologia = GetTipoComunicazione(_specifica, currentRowFields(i))
                            riga.Codice_tipo_chiamata = currentRowFields(i)
                    End Select
                    i = i + 1
                Next
                _rigaTab.Add(riga)
                iRigheDecodeDati = iRigheDecodeDati + 1
            Else
                'se la lunghezza della lista campi è uno vuol dire che abbiamo raggiunto la fine del gruppo di righe
                bExit = True
            End If
        End While
        Return iRigheDecodeDati
    End Function

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

