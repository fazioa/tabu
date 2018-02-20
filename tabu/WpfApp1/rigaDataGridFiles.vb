

Imports tabu

Public Class rigaFile
    Dim _pathNomeFile As String
    Dim _gestore As String

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

End Class
