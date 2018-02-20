Public Class rigaAnagrafica
    Dim _nomeFile As String
    Dim _gestore As String
    Dim _utenza As String
    Dim _datiAnagrafici As String
    Dim _indirizzo As String
    Dim _societa As String
    Dim _dataNascita As String
    Dim _luogoNascita As String
    Dim _codicefiscale As String
    Dim _IMSI As String
    Dim _dealerVendita As String
    Dim _dealerAttivazione As String
    Dim _dataAttivazione As String
    Dim _dataDisattivazione As String
    Dim _stato As String

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


    Public Property Utenza As String
        Get
            Return _utenza
        End Get
        Set(value As String)
            _utenza = value
        End Set
    End Property

    Public Property DatiAnagrafici As String
        Get
            Return _datiAnagrafici
        End Get
        Set(value As String)
            _datiAnagrafici = value
        End Set
    End Property
    Public Property DataNascita As String
        Get
            Return _dataNascita
        End Get
        Set(value As String)
            _dataNascita = value
        End Set
    End Property
    Public Property LuogoNascita As String
        Get
            Return _luogoNascita
        End Get
        Set(value As String)
            _luogoNascita = value
        End Set
    End Property

    Public Property Codicefiscale As String
        Get
            Return _codicefiscale
        End Get
        Set(value As String)
            _codicefiscale = value
        End Set
    End Property
    Public Property Indirizzo As String
        Get
            Return _indirizzo
        End Get
        Set(value As String)
            _indirizzo = value
        End Set
    End Property

    Public Property Societa As String
        Get
            Return _societa
        End Get
        Set(value As String)
            _societa = value
        End Set
    End Property

    Public Property IMSI As String
        Get
            Return _IMSI
        End Get
        Set(value As String)
            _IMSI = value
        End Set
    End Property

    Public Property DealerVendita As String
        Get
            Return _dealerVendita
        End Get
        Set(value As String)
            _dealerVendita = value
        End Set
    End Property

    Public Property DealerAttivazione As String
        Get
            Return _dealerAttivazione
        End Get
        Set(value As String)
            _dealerAttivazione = value
        End Set
    End Property

    Public Property DataAttivazione As String
        Get
            Return _dataAttivazione
        End Get
        Set(value As String)
            _dataAttivazione = value
        End Set
    End Property

    Public Property DataDisattivazione As String
        Get
            Return _dataDisattivazione
        End Get
        Set(value As String)
            _dataDisattivazione = value
        End Set
    End Property
    Public Property Stato As String
        Get
            Return _stato
        End Get
        Set(value As String)
            _stato = value
        End Set
    End Property

End Class
