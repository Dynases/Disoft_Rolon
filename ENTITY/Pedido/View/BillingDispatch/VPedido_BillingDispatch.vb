Public Class VPedido_BillingDispatch
    Inherits VPedido

    Private _estaFacturado As Boolean
    Public Property EstaFacturado() As Boolean
        Get
            Return _estaFacturado
        End Get
        Set(ByVal value As Boolean)
            _estaFacturado = value
        End Set
    End Property

    Private _nroFactura As String
    Public Property NroFactura() As String
        Get
            Return _nroFactura
        End Get
        Set(ByVal value As String)
            _nroFactura = value
        End Set
    End Property
    Private _idZona As Integer
    Public Property idZona() As Integer
        Get
            Return _idZona
        End Get
        Set(ByVal value As Integer)
            _idZona = value
        End Set
    End Property
    Private _nombreZona As String
    Public Property nombreZona() As String
        Get
            Return _nombreZona
        End Get
        Set(ByVal value As String)
            _nombreZona = value
        End Set
    End Property
End Class
