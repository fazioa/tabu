Imports Microsoft.VisualBasic.FileIO
Imports tabu

Public Class Vodafone
    Dim _MyReader As TextFieldParser
    Dim iRigheDecodeVodafone As ULong = 0

    Function DecodeVodafone(pathNomeFile As String, ByRef _rigaTab As List(Of rigaTabulato), nomeFile As String, gestore As String)
        _MyReader = New FileIO.TextFieldParser(pathNomeFile)
        Dim currentRowFields As String()
        'imposta le specifiche per il gestore
        Dim specifica As New XControl
        specifica = specifica.XCRead(My.Application.Info.DirectoryPath & ".\specifiche\specificaVodafone.xml")

        _MyReader.TextFieldType = FileIO.FieldType.Delimited
        _MyReader.SetDelimiters("/", "\t")


        While Not _MyReader.EndOfData
            Try
                currentRowFields = _MyReader.ReadFields

                Select Case currentRowFields(0)
                    Case specifica.TitoloTraffico
                        '  SetImporter(_MyReader, specifica)
                        iRigheDecodeVodafone = DecodeVodafoneFoniaDatiSMS(specifica, _rigaTab, nomeFile, gestore)
                    Case specifica.TitoloTrafficoBTS
                        '  SetImporter(_MyReader, specifica)
                        iRigheDecodeVodafone = DecodeVodafoneFoniaDatiSMS_BTS(specifica, _rigaTab, nomeFile, gestore)
                End Select
            Catch ex As Exception
                MsgBox("Error - File " & nomeFile & " - Line nr." & _MyReader.LineNumber & " - " & ex.Message)
            End Try
        End While
        Return iRigheDecodeVodafone

    End Function

    Private Function DecodeVodafoneFoniaDatiSMS_BTS(_specifica As XControl, ByRef _rigaTab As List(Of rigaTabulato), _nomeFile As String, _gestore As String)
        Dim riga As rigaTabulato
        Dim bExit As Boolean = False
        Dim iRigheDecodeVodafoneFoniaDatiSMS As ULong = 0

        Dim sLine As String
        Dim sCella As String

        Dim currentRowFields As String()
        'scorre fino al sottotitolo
        _MyReader.SetDelimiters(":", "/")
        While Not _MyReader.EndOfData And Not bExit
            currentRowFields = _MyReader.ReadFields
            Select Case currentRowFields(0)
                Case _specifica.SottoTitoloTrafficoVoce
                    bExit = True
                Case "Cella"
                    sCella = currentRowFields(1)
            End Select
        End While

        bExit = False
        While Not _MyReader.EndOfData And Not bExit

            Try

                Dim i As Integer = 0
                riga = New rigaTabulato
                riga.Gestore = _gestore
                riga.NomeFile = System.IO.Path.GetFileName(_nomeFile)

                sLine = _MyReader.ReadLine() 'non usare il trim, altrimenti viene eliminato il primo campo, che generalmente è vuoto

                If sLine.StartsWith("Legenda:") Then
                    sLine = "" 'se la riga inizia con Legenda: allora abbiamo raggiunto la fine del contenuto
                    bExit = True
                End If

                If (sLine.Length > 0) Then
                    For Each campo In _specifica.CampiVoceBTS
                        'sDato è il dato estrapolato dal file di testo che dovrà essere inserito nella struttura riga
                        Dim sDato As String
                        If (_specifica.delimitatoriFissiBTS.Item(i + 1) < sLine.Length) Then
                            sDato = sLine.Substring(_specifica.delimitatoriFissiBTS.Item(i) - 1, _specifica.delimitatoriFissiBTS.Item(i + 1) - _specifica.delimitatoriFissiBTS.Item(i)).Trim
                        Else
                            sDato = sLine.Substring(_specifica.delimitatoriFissiBTS.Item(i) - 1, sLine.Length - _specifica.delimitatoriFissiBTS.Item(i)).Trim
                        End If

                        Select Case campo
                            Case "Chiamante"
                                riga.Chiamante = sDato
                            Case "Chiamato"
                                riga.Chiamato = sDato
                            Case "Chiamato"
                            Case "Origine / Smcs / Digitato"

                            Case "Data e Ora Inizio"
                                riga.DataOra = sDato
                                Dim convertedDate_inizio As Date = Convert.ToDateTime(riga.DataOra)
                                Dim convertedDate_fine As Date = Convert.ToDateTime(sLine.Substring(_specifica.delimitatoriFissiBTS.Item(i + 1) - 1, _specifica.delimitatoriFissiBTS.Item(i + 2) - _specifica.delimitatoriFissiBTS.Item(i + 1)).Trim) 'il campo successivo è data ora fine. Calcolo la durata
                                riga.Durata = (convertedDate_fine - convertedDate_inizio).Seconds
                                i = i + 1
                            Case "Tipo"
                                riga.Codice_tipo_chiamata = sDato
                                riga.Tipologia = GetTipoComunicazione(_specifica, sDato)
                                'Per il tabulato per BTS il valore della cella esiste già, quindi posso inserirlo
                                If (IsDatiChiamato(_specifica, riga.Codice_tipo_chiamata)) Then
                                    riga.DescrizioneCellaInizioFine_Chiamato = sCella
                                Else
                                    riga.DescrizioneCellaInizioFine_Chiamante = sCella
                                End If

                            Case "IMEI"
                                If (IsDatiChiamato(_specifica, riga.Codice_tipo_chiamata)) Then
                                    riga.Imei_chiamato = sDato
                                Else
                                    riga.Imei_chiamante = sDato
                                End If
                            Case "IMSI"
                                If (IsDatiChiamato(_specifica, riga.Codice_tipo_chiamata)) Then
                                    riga.Imsi_chiamato = sDato
                                Else
                                    riga.Imsi_chiamante = sDato
                                End If
                            Case "DescrizioneCellaInizioFine"
                                If (IsDatiChiamato(_specifica, riga.Codice_tipo_chiamata)) Then
                                    riga.DescrizioneCellaInizioFine_Chiamato = sCella
                                Else
                                    riga.DescrizioneCellaInizioFine_Chiamante = sCella
                                End If

                        End Select
                        i = i + 1

                    Next
                    _rigaTab.Add(riga)
                    iRigheDecodeVodafoneFoniaDatiSMS = iRigheDecodeVodafoneFoniaDatiSMS + 1
                Else
                    'se la lunghezza della lista campi è uno vuol dire che abbiamo raggiunto la fine del gruppo di righe
                    '  bExit = True
                End If
            Catch ex As MalformedLineException
                MsgBox("Error - File " & _nomeFile & " - Line nr." & _MyReader.LineNumber & " - " & ex.Message & " - " & _MyReader.ErrorLine)
            End Try
        End While
        Return iRigheDecodeVodafoneFoniaDatiSMS
    End Function

    Private Function DecodeVodafoneFoniaDatiSMS(_specifica As XControl, ByRef _rigaTab As List(Of rigaTabulato), _nomeFile As String, _gestore As String)
        Dim riga As rigaTabulato
        Dim bExit As Boolean = False
        Dim iRigheDecodeVodafoneFoniaDatiSMS As ULong = 0
        Dim sDato As String
        Dim sLine As String

        Dim currentRowFields As String()
        'scorre fino al sottotitolo
        _MyReader.SetDelimiters(":", "/")
        While Not _MyReader.EndOfData And Not bExit
            currentRowFields = _MyReader.ReadFields
            Select Case currentRowFields(0)
                Case _specifica.SottoTitoloTrafficoVoce
                    bExit = True
            End Select
        End While

        bExit = False
        While Not _MyReader.EndOfData And Not bExit

            Try

                Dim i As Integer = 0
                riga = New rigaTabulato
                riga.Gestore = _gestore
                riga.NomeFile = System.IO.Path.GetFileName(_nomeFile)

                sLine = _MyReader.ReadLine() 'non usare il trim, altrimenti viene eliminato il primo campo, che generalmente è vuoto

                If sLine.StartsWith("Legenda:") Then
                    sLine = "" 'se la riga inizia con Legenda: allora abbiamo raggiunto la fine del contenuto
                    bExit = True
                End If

                If (sLine.Length > 0) Then
                    For Each campo In _specifica.CampiVoce
                        'sDato è il dato estrapolato dal file di testo che dovrà essere inserito nella struttura riga
                        sDato = ""
                        If (_specifica.delimitatoriFissi.Item(i + 1) < sLine.Length) Then
                            sDato = sLine.Substring(_specifica.delimitatoriFissi.Item(i) - 1, _specifica.delimitatoriFissi.Item(i + 1) - _specifica.delimitatoriFissi.Item(i)).Trim
                        Else
                            'se abbiamo raggiunto l'ultimo delimitatore (9999) allora prendo la strima dal penultimo delimitatore fino alla fine
                            'faccio un ulteriore controllo perchè la stringa potrebbe avere lunghezza inferiore all'ultimo delimitatore (stringa mozzata o prob. malformata) 
                            If (_specifica.delimitatoriFissi.Item(i) < sLine.Length) Then
                                sDato = sLine.Substring(_specifica.delimitatoriFissi.Item(i) - 1, sLine.Length - _specifica.delimitatoriFissi.Item(i) + 1).Trim
                            End If
                        End If

                        Select Case campo
                            Case "Chiamante"
                                riga.Chiamante = sDato
                            Case "Chiamato"
                                riga.Chiamato = sDato
                            Case "Chiamato"
                            Case "Origine / Smcs / Digitato"

                            Case "Data e Ora Inizio"
                                riga.DataOra = sDato
                                Dim convertedDate_inizio As Date = Convert.ToDateTime(riga.DataOra)
                                Dim convertedDate_fine As Date = Convert.ToDateTime(sLine.Substring(_specifica.delimitatoriFissi.Item(i + 1) - 1, _specifica.delimitatoriFissi.Item(i + 2) - _specifica.delimitatoriFissi.Item(i + 1)).Trim) 'il campo successivo è data ora fine. Calcolo la durata
                                riga.Durata = (convertedDate_fine - convertedDate_inizio).Seconds
                                i = i + 1
                            Case "Tipo"
                                riga.Codice_tipo_chiamata = sDato
                                riga.Tipologia = GetTipoComunicazione(_specifica, sDato)
                            Case "IMEI"
                                If (IsDatiChiamato(_specifica, riga.Codice_tipo_chiamata)) Then
                                    riga.Imei_chiamato = sDato
                                Else
                                    riga.Imei_chiamante = sDato
                                End If
                            Case "IMSI"
                                If (IsDatiChiamato(_specifica, riga.Codice_tipo_chiamata)) Then
                                    riga.Imsi_chiamato = sDato
                                Else
                                    riga.Imsi_chiamante = sDato
                                End If
                            Case "DescrizioneCellaInizioFine"
                                If (IsDatiChiamato(_specifica, riga.Codice_tipo_chiamata)) Then
                                    riga.DescrizioneCellaInizioFine_Chiamato = sDato
                                Else
                                    riga.DescrizioneCellaInizioFine_Chiamante = sDato
                                End If

                        End Select
                        i = i + 1

                    Next
                    _rigaTab.Add(riga)
                    iRigheDecodeVodafoneFoniaDatiSMS = iRigheDecodeVodafoneFoniaDatiSMS + 1
                Else
                    'se la lunghezza della lista campi è uno vuol dire che abbiamo raggiunto la fine del gruppo di righe
                    '  bExit = True
                End If
            Catch ex As MalformedLineException
                MsgBox("Error - File " & _nomeFile & " - Line nr." & _MyReader.LineNumber & " - " & ex.Message & " - " & _MyReader.ErrorLine)
            End Try
        End While
        Return iRigheDecodeVodafoneFoniaDatiSMS
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
End Class
