﻿Imports Microsoft.VisualBasic.FileIO
Public Class TimVoce
    Dim _MyReader As TextFieldParser
    Dim currentRowFields As String()
    Dim iRighe As ULong = 0
    Function DecodeTim(pathNomeFile As String, ByRef _rigaTab As List(Of rigaTabulato), nomeFile As String, gestore As String)
        _MyReader = New FileIO.TextFieldParser(pathNomeFile)
        'imposta le specifiche per il gestore
        Dim specifica As New XControl
        specifica = specifica.XCRead(My.Application.Info.DirectoryPath & ".\specifiche\specificaTimVoce.xml")
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
                    Case specifica.TitoloTrafficoVoce
                        SetImporter(_MyReader, specifica)
                        iRighe = DecodeTimVoce(specifica, _rigaTab, nomeFile, gestore)
                End Select

            Catch ex As Microsoft.VisualBasic.
                       FileIO.MalformedLineException
                MsgBox("File " & nomeFile & " - Line " & _MyReader.LineNumber & " - " & ex.Message & "is not valid and will be skipped.")
            End Try
        End While
        Return iRighe
    End Function


    Function DecodeTimVoce(_specifica As XControl, ByRef _rigaTab As List(Of rigaTabulato), _nomeFile As String, _gestore As String)
        Dim riga As rigaTabulato
        Dim bExit As Boolean = False
        Dim iRigheDecodeTimVoce As ULong = 0

        Const NUMERO_CAMPO_TIPO_CHIAMATA As Integer = 21
        'salta una riga
        currentRowFields = _MyReader.ReadFields
        While Not _MyReader.EndOfData And Not bExit = True
            currentRowFields = read(_MyReader)
            If (currentRowFields.Length > 1) Then
                Dim i As Integer = 0
                riga = New rigaTabulato
                riga.Gestore = _gestore
                riga.NomeFile = System.IO.Path.GetFileName(_nomeFile)


                For Each campo In _specifica.CampiVoce

                    Select Case campo
                        Case "Telefono Chte"
                            riga.Chiamante = currentRowFields(i)
                        Case "Telefono Chto"
                            riga.Chiamato = currentRowFields(i)
                        Case "DATA"
                            Dim dataora As Date = Convert.ToDateTime(currentRowFields(i) + " " + Strings.Replace(currentRowFields(i + 1), ".", ":"))
                            riga.DataOra = dataora
                            i = i + 1
                        Case "Durata"
                            If (currentRowFields(i).Equals("")) Then
                                riga.Durata = 0
                            Else
                                riga.Durata = currentRowFields(i)
                            End If
                        Case "Imsi Chte"
                            riga.Imsi_chiamante = currentRowFields(i)
                        Case "Imsi Chto"
                            riga.Imsi_chiamato = currentRowFields(i)
                        Case "Imei Chte"
                            riga.Imei_chiamante = currentRowFields(i)
                        Case "Imei Chto"
                            riga.Imei_chiamato = currentRowFields(i)
                        Case "Cellid"
                            If (IsDatiChiamato(_specifica, currentRowFields(NUMERO_CAMPO_TIPO_CHIAMATA))) Then
                                riga.CellaChiamato_inizio = currentRowFields(i)
                            Else
                                riga.CellaChiamante_inizio = currentRowFields(i)
                            End If
                        Case "Cellid end"
                            If (IsDatiChiamato(_specifica, currentRowFields(NUMERO_CAMPO_TIPO_CHIAMATA))) Then
                                riga.CellaChiamato_fine = currentRowFields(i)
                            Else
                                riga.CellaChiamante_fine = currentRowFields(i)
                            End If
                        Case "CGI/ECGI/LocNumber"
                            If (IsDatiChiamato(_specifica, currentRowFields(NUMERO_CAMPO_TIPO_CHIAMATA))) Then
                                riga.DescrizioneCellaInizioFine_Chiamato = currentRowFields(i) & " - " & currentRowFields(i + 1)
                            Else
                                riga.DescrizioneCellaInizioFine_Chiamante = currentRowFields(i) & " - " & currentRowFields(i + 1)
                            End If
                        Case "Tipo Chta"
                            riga.Tipologia = GetTipoComunicazione(_specifica, currentRowFields(NUMERO_CAMPO_TIPO_CHIAMATA))
                            riga.Codice_tipo_chiamata = currentRowFields(i)
                    End Select
                    i = i + 1
                Next
                _rigaTab.Add(riga)
                iRigheDecodeTimVoce = iRigheDecodeTimVoce + 1
            Else
                'se la lunghezza della lista campi è uno vuol dire che abbiamo raggiunto la fine del gruppo di righe
                bExit = True
            End If
        End While
        Return iRigheDecodeTimVoce
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

