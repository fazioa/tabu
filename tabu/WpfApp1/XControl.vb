Imports System.IO
Imports System.Xml.Serialization

Public Class XControl

    Private _delimitato As Boolean
    Private _delimitatore As String
    Private _delimitatoAnagrafica As Boolean
    Private _delimitatoreAnagrafica As String
    Private _delimitatoriFissi As New List(Of Integer)

    Private _delimitatoriFissiRiga1 As New List(Of Integer)
    Private _delimitatoriFissiRiga2 As New List(Of Integer)
    Private _delimitatoriFissiRiga3 As New List(Of Integer)

    Private _trimWhiteSpace As Boolean
    Private _larghezzaFissa As Boolean

    Private _titoloTraffico As String
    Private _titoloTrafficoVoce As String
    Private _titoloTrafficoSMS As String
    Private _titoloTrafficoServizi As String
    Private _titoloTrafficoDati As String

    Private _titoloAnagrafica As String
    Private _sottoTitoloTrafficoDati As String
    Private _sottoTitoloTrafficoVoce As String



    Private _titoloTrafficoVoceOpVirtuali As String

    Private _campiVoce As New List(Of String)
    Private _campiSMS As New List(Of String)
    Private _campiDati As New List(Of String)
    Private _campiAnagrafica As New List(Of String)

    Private _campiAnagraficaRiga1 As New List(Of String)
    Private _campiAnagraficaRiga2 As New List(Of String)
    Private _campiAnagraficaRiga3 As New List(Of String)

    Private _fineReport As String

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

    Public Property delimitatoAnagrafica As Boolean
        Get
            Return _delimitatoAnagrafica
        End Get
        Set(value As Boolean)
            _delimitatoAnagrafica = value
        End Set
    End Property

    Public Property delimitatoreAnagrafica As String
        Get
            Return _delimitatoreAnagrafica
        End Get
        Set(value As String)
            _delimitatoreAnagrafica = value
        End Set
    End Property

    Public Property DelimitatoriFissiRiga1 As List(Of Integer)
        Get
            Return _delimitatoriFissiRiga1
        End Get
        Set(value As List(Of Integer))
            _delimitatoriFissiRiga1 = value
        End Set
    End Property

    Public Property DelimitatoriFissiRiga2 As List(Of Integer)
        Get
            Return _delimitatoriFissiRiga2
        End Get
        Set(value As List(Of Integer))
            _delimitatoriFissiRiga2 = value
        End Set
    End Property

    Public Property DelimitatoriFissiRiga3 As List(Of Integer)
        Get
            Return _delimitatoriFissiRiga3
        End Get
        Set(value As List(Of Integer))
            _delimitatoriFissiRiga3 = value
        End Set
    End Property

    Public Property CampiAnagraficaRiga1 As List(Of String)
        Get
            Return _campiAnagraficaRiga1
        End Get
        Set(value As List(Of String))
            _campiAnagraficaRiga1 = value
        End Set
    End Property

    Public Property CampiAnagraficaRiga2 As List(Of String)
        Get
            Return _campiAnagraficaRiga2
        End Get
        Set(value As List(Of String))
            _campiAnagraficaRiga2 = value
        End Set
    End Property

    Public Property CampiAnagraficaRiga3 As List(Of String)
        Get
            Return _campiAnagraficaRiga3
        End Get
        Set(value As List(Of String))
            _campiAnagraficaRiga3 = value
        End Set
    End Property

    Public Property FineReport As String
        Get
            Return _fineReport
        End Get
        Set(value As String)
            _fineReport = value
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