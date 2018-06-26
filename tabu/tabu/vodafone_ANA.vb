Imports Microsoft.VisualBasic.FileIO
Public Class Vodafone_anagrafica
    Dim _MyReader As TextFieldParser
    Dim currentRowFields As String()
    Sub DecodeVodafone(pathNomeFile As String, ByRef _rigaAnag As List(Of rigaAnagrafica), nomeFile As String, gestore As String)
        _MyReader = New FileIO.TextFieldParser(pathNomeFile)
        'imposta le specifiche per il gestore
        Dim specifica As New XControl
        specifica = specifica.XCRead(My.Application.Info.DirectoryPath & ".\specifiche\specificaVodafone.xml")

        _MyReader.TextFieldType = FileIO.FieldType.Delimited
        _MyReader.SetDelimiters(":")


        While Not _MyReader.EndOfData
            Try
                currentRowFields = _MyReader.ReadFields
                Select Case currentRowFields(0).Trim
                    Case specifica.TitoloAnagrafica
                        SetImporter(_MyReader, specifica)
                        DecodeVodafoneAnagrafica(specifica, _rigaAnag, nomeFile, gestore)
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


    Sub DecodeVodafoneAnagrafica(_specifica As XControl, ByRef _rigaAna As List(Of rigaAnagrafica), _nomeFile As String, _gestore As String)

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
                        Case "Numero Telefono"
                            riga.Utenza = currentRowFields(i)
                        Case "SN"
                            riga.Stato = currentRowFields(i)
                        Case "Data Attivazione SIM"
                            riga.DataAttivazione = currentRowFields(i)
                        Case "Data Disattivazione SIM"
                            riga.DataDisattivazione = currentRowFields(i)
                        Case "Dealer"
                            riga.DealerVendita = currentRowFields(i)
                        Case "C.F./P.IVA"
                            If (currentRowFields.Length > i) Then 'se CF è vuoto il parse non considera il campo, quindi currentRowFields potrebbe non avere il 17° campo 
                                riga.Codicefiscale = currentRowFields(i)
                            End If
                        Case "Domicilio Fattura Customer"
                            If (currentRowFields.Length > i) Then
                                riga.Indirizzo = currentRowFields(i)
                            End If
                        Case "Cognome"
                            If (currentRowFields.Length > (i + 1)) Then
                                riga.DatiAnagrafici = currentRowFields(i) + " " + currentRowFields(i + 1)
                                i = i + 1
                            Else
                                If (currentRowFields.Length > (i)) Then
                                    riga.DatiAnagrafici = currentRowFields(i)
                                End If
                            End If
                                Case "Residenza"
                            If (currentRowFields.Length > i) Then
                                riga.Indirizzo = currentRowFields(i)
                            End If
                        Case "Luogo/Data di Nascita"
                            If (currentRowFields.Length > i) Then
                                If (currentRowFields(i).Length > 0) Then
                                    Try
                                        Dim sDataNascita As String = currentRowFields(i).Substring(currentRowFields(i).Length - 11, 11)
                                        riga.DataNascita = sDataNascita
                                    Catch ex As Exception
                                    End Try

                                    Try
                                        Dim sLuogoNascita As String = currentRowFields(i).Substring(0, currentRowFields(i).Length - 10)
                                        riga.LuogoNascita = sLuogoNascita
                                    Catch ex As Exception
                                    End Try
                                End If
                            End If
                        Case "Numero Sim"
                            riga.IMSI = currentRowFields(i)
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
        If (_specifica.delimitatoAnagrafica) Then
            _MyReader.TextFieldType = FileIO.FieldType.Delimited
            If (_specifica.delimitatoreAnagrafica.Equals("tab")) Then
                _MyReader.SetDelimiters(vbTab) ' "|" per wind
            Else
                _MyReader.SetDelimiters(_specifica.delimitatoreAnagrafica) ' 
            End If
        Else
            _MyReader.TextFieldType = FileIO.FieldType.FixedWidth
        End If
        If (_specifica.trimWhiteSpace = True) Then
            _MyReader.TrimWhiteSpace() = True
        End If
    End Sub

End Class
