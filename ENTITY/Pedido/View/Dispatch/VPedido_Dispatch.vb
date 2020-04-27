Public Class VPedido_Dispatch
    Inherits VPedido
    Private _idZona As Integer
    Public Property idZona() As Integer
        Get
            Return _idZona
        End Get
        Set(ByVal value As Integer)
            _idZona = value
        End Set
    End Property
End Class
