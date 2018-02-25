Imports System.IO
Imports System.Xml.Serialization

Public Class XControl

    Private _intestazione As String
    Private _delimitato As Boolean
    Private _delimitatore As String
    Private _delimitatoriFissi As New List(Of Integer)
    Private _trimWhiteSpace As Boolean
    Private _larghezzaFissa As Boolean

    Dim _titoloTraffico As String
    Dim _titoloTrafficoVoce As String
    Dim _titoloTrafficoSMS As String
    Dim _titoloTrafficoServizi As String
    Dim _titoloTrafficoDati As String

    Dim _titoloAnagrafica As String
    Dim _sottoTitoloTrafficoDati As String
    Dim _sottoTitoloTrafficoVoce As String



    Dim _titoloTrafficoVoceOpVirtuali As String

    Dim _campiVoce As New List(Of String)
    Dim _campiSMS As New List(Of String)
    Dim _campiDati As New List(Of String)
    Dim _campiAnagrafica As New List(Of String)
    Dim _tipo As New tipoComunicazione()



    Public Class tipoComunicazione

        Private _voce As New List(Of String)
        Private _sms As New List(Of String)
        Private _dati As New List(Of String)
        Private _altro As New List(Of String)
        Private _dettaglioDatiChiamante As New List(Of String)
        Private _dettaglioDatiChiamato As New List(Of String)

        Public Property Voce As List(Of String)
            Get
                Return _voce
            End Get
            Set(value As List(Of String))
                _voce = value
            End Set
        End Property

        Public Property Sms As List(Of String)
            Get
                Return _sms
            End Get
            Set(value As List(Of String))
                _sms = value
            End Set
        End Property

        Public Property Altro As List(Of String)
            Get
                Return _altro
            End Get
            Set(value As List(Of String))
                _altro = value
            End Set
        End Property

        Public Property Dati As List(Of String)
            Get
                Return _dati
            End Get
            Set(value As List(Of String))
                _dati = value
            End Set
        End Property

        Public Property DettaglioDatiChiamante As List(Of String)
            Get
                Return _dettaglioDatiChiamante
            End Get
            Set(value As List(Of String))
                _dettaglioDatiChiamante = value
            End Set
        End Property

        Public Property DettaglioDatiChiamato As List(Of String)
            Get
                Return _dettaglioDatiChiamato
            End Get
            Set(value As List(Of String))
                _dettaglioDatiChiamato = value
            End Set
        End Property
    End Class
    Public Property intestazione As String
        Get
            Return _intestazione
        End Get
        Set(value As String)
            _intestazione = value
        End Set
    End Property

    Public Property delimitato As Boolean
        Get
            Return _delimitato
        End Get
        Set(value As Boolean)
            _delimitato = value
        End Set
    End Property

    Public Property delimitatore As String
        Get
            Return _delimitatore
        End Get
        Set(value As String)
            _delimitatore = value
        End Set
    End Property

    Public Property trimWhiteSpace As Boolean
        Get
            Return _trimWhiteSpace
        End Get
        Set(value As Boolean)
            _trimWhiteSpace = value
        End Set
    End Property

    Public Property TitoloTrafficoSMS As String
        Get
            Return _titoloTrafficoSMS
        End Get
        Set(value As String)
            _titoloTrafficoSMS = value
        End Set
    End Property

    Public Property TitoloTrafficoServizi As String
        Get
            Return _titoloTrafficoServizi
        End Get
        Set(value As String)
            _titoloTrafficoServizi = value
        End Set
    End Property

    Public Property TitoloTrafficoVoce As String
        Get
            Return _titoloTrafficoVoce
        End Get
        Set(value As String)
            _titoloTrafficoVoce = value
        End Set
    End Property

    Public Property TitoloTrafficoVoceOpVirtuali As String
        Get
            Return _titoloTrafficoVoceOpVirtuali
        End Get
        Set(value As String)
            _titoloTrafficoVoceOpVirtuali = value
        End Set
    End Property

    Public Property CampiVoce As List(Of String)
        Get
            Return _campiVoce
        End Get
        Set(value As List(Of String))
            _campiVoce = value
        End Set
    End Property

    Public Property Tipo As tipoComunicazione
        Get
            Return _tipo
        End Get
        Set(value As tipoComunicazione)
            _tipo = value
        End Set
    End Property

    Public Property CampiSMS As List(Of String)
        Get
            Return _campiSMS
        End Get
        Set(value As List(Of String))
            _campiSMS = value
        End Set
    End Property

    Public Property CampiDati As List(Of String)
        Get
            Return _campiDati
        End Get
        Set(value As List(Of String))
            _campiDati = value
        End Set
    End Property

    Public Property SottoTitoloTrafficoDati As String
        Get
            Return _sottoTitoloTrafficoDati
        End Get
        Set(value As String)
            _sottoTitoloTrafficoDati = value
        End Set
    End Property

    Public Property SottoTitoloTrafficoVoce As String
        Get
            Return _sottoTitoloTrafficoVoce
        End Get
        Set(value As String)
            _sottoTitoloTrafficoVoce = value
        End Set
    End Property

    Public Property TitoloAnagrafica As String
        Get
            Return _titoloAnagrafica
        End Get
        Set(value As String)
            _titoloAnagrafica = value
        End Set
    End Property

    Public Property CampiAnagrafica As List(Of String)
        Get
            Return _campiAnagrafica
        End Get
        Set(value As List(Of String))
            _campiAnagrafica = value
        End Set
    End Property

    Public Property TitoloTrafficoDati As String
        Get
            Return _titoloTrafficoDati
        End Get
        Set(value As String)
            _titoloTrafficoDati = value
        End Set
    End Property

    Public Property delimitatoriFissi As List(Of Integer)
        Get
            Return _delimitatoriFissi
        End Get
        Set(value As List(Of Integer))
            _delimitatoriFissi = value
        End Set
    End Property

    Public Property LarghezzaFissa As Boolean
        Get
            Return _larghezzaFissa
        End Get
        Set(value As Boolean)
            _larghezzaFissa = value
        End Set
    End Property

    Public Property TitoloTraffico As String
        Get
            Return _titoloTraffico
        End Get
        Set(value As String)
            _titoloTraffico = value
        End Set
    End Property


    'load from file
    Public Function XCRead(filename As String) As XControl
        Dim fs As New FileStream(filename, FileMode.Open)
        ' Dim sr As StreamReader = New StreamReader(filename)
        Dim xmls As New XmlSerializer(GetType(XControl))
        Dim r As XControl = CType(xmls.Deserialize(fs), XControl)
        fs.Close()
        Return r
    End Function

    'save to file
    Public Sub XCSave(filename As String)
        Using sw As StreamWriter = New StreamWriter(filename)
            Dim xmls As New XmlSerializer(GetType(XControl))
            xmls.Serialize(sw, Me)
        End Using
    End Sub

End Class