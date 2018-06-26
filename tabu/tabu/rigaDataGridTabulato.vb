Public Class rigaTabulato
    Dim _nomeFile As String
    Dim _gestore As String
    Dim _tipologia As String
    Dim _codice_tipo_chiamata As String
    Dim _codiceStatoSMS As String
    Dim _chiamante As String
    Dim _chiamato As String
    Dim _dataOra As Date
    Dim _durata As ULong

    Dim _imsi_chiamante As String
    Dim _imei_chiamante As String
    Dim _imsi_chiamato As String
    Dim _imei_chiamato As String

    Dim _rete As String
    Dim _reteDestinatario As String

    Dim _cellaChiamante_inizio As String
    Dim _cellaChiamante_fine As String
    Dim _cellaChiamato_inizio As String
    Dim _cellaChiamato_fine As String

    Dim _descrizioneCellaInizioFine_Chiamante As String
    Dim _descrizioneCellaInizioFine_Chiamato As String


    Public Sub New()


    End Sub

    Public Property Gestore As String
        Get
            Return _gestore
        End Get
        Set(value As String)
            _gestore = value
        End Set
    End Property

    Public Property NomeFile As String
        Get
            Return _nomeFile
        End Get
        Set(value As String)
            _nomeFile = value
        End Set
    End Property
    Public Property Tipologia As String
        Get
            Return _tipologia
        End Get
        Set(value As String)
            _tipologia = value
        End Set
    End Property
    Public Property Codice_tipo_chiamata As String
        Get
            Return _codice_tipo_chiamata
        End Get
        Set(value As String)
            _codice_tipo_chiamata = value
        End Set
    End Property

    Public Property CodiceStatoSMS As String
        Get
            Return _codiceStatoSMS
        End Get
        Set(value As String)
            _codiceStatoSMS = value
        End Set
    End Property
    Public Property Chiamante As String
        Get
            Return _chiamante
        End Get
        Set(value As String)
            _chiamante = value
        End Set
    End Property

    Public Property Chiamato As String
        Get
            Return _chiamato
        End Get
        Set(value As String)
            _chiamato = value
        End Set
    End Property

    Public Property DataOra As Date
        Get
            Return _dataOra
        End Get
        Set(value As Date)
            _dataOra = value
        End Set
    End Property

    Public Property Durata As ULong
        Get
            Return _durata
        End Get
        Set(value As ULong)
            _durata = value
        End Set
    End Property



    Public Property Imsi_chiamante As String
        Get
            Return _imsi_chiamante
        End Get
        Set(value As String)
            _imsi_chiamante = value
        End Set
    End Property

    Public Property Imei_chiamante As String
        Get
            Return _imei_chiamante
        End Get
        Set(value As String)
            _imei_chiamante = value
        End Set
    End Property

    Public Property Imsi_chiamato As String
        Get
            Return _imsi_chiamato
        End Get
        Set(value As String)
            _imsi_chiamato = value
        End Set
    End Property

    Public Property Imei_chiamato As String
        Get
            Return _imei_chiamato
        End Get
        Set(value As String)
            _imei_chiamato = value
        End Set
    End Property



    Public Property CellaChiamante_inizio As String
        Get
            Return _cellaChiamante_inizio
        End Get
        Set(value As String)
            _cellaChiamante_inizio = value
        End Set
    End Property
    Public Property CellaChiamante_fine As String
        Get
            Return _cellaChiamante_fine
        End Get
        Set(value As String)
            _cellaChiamante_fine = value
        End Set
    End Property

    Public Property DescrizioneCellaInizioFine_Chiamante As String
        Get
            Return _descrizioneCellaInizioFine_Chiamante
        End Get
        Set(value As String)
            _descrizioneCellaInizioFine_Chiamante = value
        End Set
    End Property

    Public Property CellaChiamato_inizio As String
        Get
            Return _cellaChiamato_inizio
        End Get
        Set(value As String)
            _cellaChiamato_inizio = value
        End Set
    End Property

    Public Property CellaChiamato_fine As String
        Get
            Return _cellaChiamato_fine
        End Get
        Set(value As String)
            _cellaChiamato_fine = value
        End Set
    End Property
    Public Property DescrizioneCellaInizioFine_Chiamato As String
        Get
            Return _descrizioneCellaInizioFine_Chiamato
        End Get
        Set(value As String)
            _descrizioneCellaInizioFine_Chiamato = value
        End Set
    End Property

    Public Property Rete As String
        Get
            Return _rete
        End Get
        Set(value As String)
            _rete = value
        End Set
    End Property
    Public Property ReteDestinatario As String
        Get
            Return _reteDestinatario
        End Get
        Set(value As String)
            _reteDestinatario = value
        End Set
    End Property

End Class
