
Imports Microsoft.VisualBasic.FileIO

Class MainWindow
    Dim itemsDataset As New List(Of rigaFile)
    Dim righeTabulato As New List(Of rigaTabulato)
    Dim righeAnagrafica As New List(Of rigaAnagrafica)
    Private Sub gridFileList_Loaded(sender As Object, e As RoutedEventArgs) Handles gridFileList.Loaded
        gridFileList.ItemsSource = itemsDataset
        grid_dettaglio_tabulato.ItemsSource = righeTabulato
        grid_anagrafica.ItemsSource = righeAnagrafica
    End Sub

    Private Sub Button_esamina_Click(sender As Object, e As RoutedEventArgs) Handles Button_esamina.Click
        esamina()

        gridFileList.Items.Refresh()
    End Sub

    Private Sub esamina()
        'esamina i file inseriti nel datagrid e scrive in tabella il gestore di appartenenza

        For Each row In itemsDataset

            Dim currentRow As String()
            Dim MyReader As New FileIO.TextFieldParser(row.pathNomeFile)

            For i = 1 To 2
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters("|")

                currentRow = MyReader.ReadFields

                Select Case currentRow(0).Trim
                    Case "Telecom Italia S.p.A.                         CONSULTAZIONE TRAFFICO RADIOMOBILE"
                        row.Gestore = constants.telecomTraffico
                    Case "Telecom Italia S.P.A. traffico telematico radiomobile"
                        row.Gestore = constants.telecomTrafficoTelematico
                    Case "Ricerca Traffico Storico"
                        row.Gestore = constants.vodafoneTrafficoTelematico
                    Case "Wind Tre S.p.A. con Socio Unico - Ufficio LDS"
                        'va avanti di una riga
                        currentRow = MyReader.ReadFields
                        If (currentRow(0).Equals("##Report Anagrafica Massiva")) Then
                            row.Gestore = constants.windTreAnagrafica
                        Else
                            row.Gestore = constants.windTreTraffico
                        End If
                    Case "### DATI RICHIESTA ###"
                        row.Gestore = constants.windTraffico
                End Select
            Next
        Next
    End Sub

    Private Sub Button_importa_Click(sender As Object, e As RoutedEventArgs) Handles Button_importa.Click
        esamina()
        righeTabulato.Clear()

        ' a questo punto il dataset e relativo datagrid sono già riempiti.

        'per ogni file, in base al gestore, viene chiamato il relativo importatore
        For Each rowInDataset In itemsDataset


            Select Case rowInDataset.Gestore
                Case constants.telecomTraffico
                Case constants.telecomTrafficoTelematico

                Case constants.vodafoneTrafficoTelematico

                Case constants.windTraffico
                    Dim wind As New Wind()
                    wind.DecodeWind(rowInDataset.pathNomeFile, righeTabulato, righeAnagrafica, rowInDataset.pathNomeFile, rowInDataset.Gestore)

                Case constants.windTreTraffico
                    Dim tre_traffico As New H3G_TRAFF()
                    tre_traffico.DecodeWindTre(rowInDataset.pathNomeFile, righeTabulato, rowInDataset.pathNomeFile, rowInDataset.Gestore)
                Case constants.windTreAnagrafica
                    Dim tre_anagrafica As New H3G_ANA()
                    tre_anagrafica.DecodeWindTre(rowInDataset.pathNomeFile, righeAnagrafica, rowInDataset.pathNomeFile, rowInDataset.Gestore)
            End Select
        Next


        gridFileList.Items.Refresh()
        grid_dettaglio_tabulato.Items.Refresh()
        grid_anagrafica.Items.Refresh()
    End Sub


    Private Sub Button_file_Click_1(sender As Object, e As RoutedEventArgs) Handles Button_file.Click

        Dim openFileDialog1 As New Microsoft.Win32.OpenFileDialog()
        openFileDialog1.Multiselect = True
        If (openFileDialog1.ShowDialog() = True) Then
            Dim selectedFileList As String()
            'inserire file selezionati in un array
            selectedFileList = openFileDialog1.FileNames
            Dim bFlag As Boolean = False

            For Each sFileName In selectedFileList


                For Each riga In itemsDataset
                    If (System.IO.Path.GetFileName(riga.pathNomeFile).Equals(System.IO.Path.GetFileName(sFileName))) Then
                        bFlag = True
                    End If
                Next

                If (Not bFlag) Then
                    itemsDataset.Add(New rigaFile() With {
                         .pathNomeFile = sFileName,
                        .Gestore = "--"
                   })
                End If
                bFlag = False
            Next
            gridFileList.Items.Refresh()

        End If
    End Sub

    Private Sub Button_esporta_Click(sender As Object, e As RoutedEventArgs) Handles Button_esporta.Click

        ExportToExcelAndCsv(grid_dettaglio_tabulato, "tabulato.csv")
        ExportToExcelAndCsv(grid_anagrafica, "anagrafica.csv")

    End Sub

    Private Sub ExportToExcelAndCsv(dgDisplay As DataGrid, sNomeFile As String)

        dgDisplay.SelectAllCells()
        dgDisplay.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader
        ApplicationCommands.Copy.Execute(Nothing, dgDisplay)
        Dim result As String = Clipboard.GetData(DataFormats.CommaSeparatedValue)
        dgDisplay.UnselectAllCells()

        Dim file1 As System.IO.StreamWriter = New System.IO.StreamWriter(sNomeFile)
        file1.WriteLine(result)
        'file1.WriteLine(result.Replace(",", ", "))
        file1.Close()



        'Process.Start("C:\Users\fazio\Documents\test.csv")


        MessageBox.Show(" Exporting DataGrid data to Excel file created.xls")
    End Sub
End Class

