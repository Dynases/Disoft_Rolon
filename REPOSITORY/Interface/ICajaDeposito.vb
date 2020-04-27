Imports ENTITY

Public Interface ICajaDeposito
    Function Listar(IdCaja As Integer) As List(Of VCajaDeposito)
    Function GuardarCajaDeposito(listCaja As List(Of VCajaDeposito), idCaja As Integer) As Boolean
End Interface
