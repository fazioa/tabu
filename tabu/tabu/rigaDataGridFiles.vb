

Imports tabu

Public Class rigaFile
    Dim _pathNomeFile As String
    Dim _gestore As String
    Dim _righe_importate As Integer

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

        Public Property pathNomeFile As String
            Get
                Return _pathNomeFile
            End Get
            Set(value As String)
                _pathNomeFile = value
            End Set
        End Property

    Public Property Righe_Importate As Integer
        Get
            Return _righe_importate
        End Get
        Set(value As Integer)
            _righe_importate = value
        End Set
    End Property
End Class
