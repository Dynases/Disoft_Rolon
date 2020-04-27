Public Class VCajaGeneral
    Private _IdCaja As Integer
    Public Property IdCaja() As Integer
        Get
            Return _IdCaja
        End Get
        Set(ByVal value As Integer)
            _IdCaja = value
        End Set
    End Property

    Private _FechaCaja As DateTime
    Public Property FechaCaja() As DateTime
        Get
            Return _FechaCaja
        End Get
        Set(ByVal value As DateTime)
            _FechaCaja = value
        End Set
    End Property

    Private _Conciliacion As Integer
    Public Property Conciliacion() As Integer
        Get
            Return _Conciliacion
        End Get
        Set(ByVal value As Integer)
            _Conciliacion = value
        End Set
    End Property

    Private _FechaPedido As DateTime
    Public Property FechaPedido() As DateTime
        Get
            Return _FechaPedido
        End Get
        Set(ByVal value As DateTime)
            _FechaPedido = value
        End Set
    End Property

    Private _Repartidor As String
    Public Property Repartidor() As String
        Get
            Return _Repartidor
        End Get
        Set(ByVal value As String)
            _Repartidor = value
        End Set
    End Property

    Private _TotalConciliacion As Decimal
    Public Property TotalConciliacion() As Decimal
        Get
            Return _TotalConciliacion
        End Get
        Set(ByVal value As Decimal)
            _TotalConciliacion = value
        End Set
    End Property
    Private _TotalEfectivo As Decimal
    Public Property TotalEfectivo() As Decimal
        Get
            Return _TotalEfectivo
        End Get
        Set(ByVal value As Decimal)
            _TotalEfectivo = value
        End Set
    End Property

    Private _TotalCredito As Decimal
    Public Property TotalCredito() As Decimal
        Get
            Return _TotalCredito
        End Get
        Set(ByVal value As Decimal)
            _TotalCredito = value
        End Set
    End Property

    Private _TotalDeposito As Decimal
    Public Property TotalDeposito() As Decimal
        Get
            Return _TotalDeposito
        End Get
        Set(ByVal value As Decimal)
            _TotalDeposito = value
        End Set
    End Property

    Private _TotalGeneral As Decimal
    Public Property TotalGeneral() As Decimal
        Get
            Return _TotalGeneral
        End Get
        Set(ByVal value As Decimal)
            _TotalGeneral = value
        End Set
    End Property

    Private _Diferencia As Decimal
    Public Property Diferencia() As Decimal
        Get
            Return _Diferencia
        End Get
        Set(ByVal value As Decimal)
            _Diferencia = value
        End Set
    End Property

End Class
