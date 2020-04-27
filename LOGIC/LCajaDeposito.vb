Imports ENTITY
Imports REPOSITORY

Public Class LCajaDeposito
    Friend iCajaDeposito As ICajaDeposito

    Public Sub New()
        Me.iCajaDeposito = New RCajaDeposito()
    End Sub
    Public Function Listar(IdCaja As Integer) As List(Of VCajaDeposito)
        Try
            Return iCajaDeposito.Listar(IdCaja)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GuardarDepositoCambio(listaCajaDeposito As List(Of VCajaDeposito), IdCaja As Integer) As Boolean
        Try
            Return iCajaDeposito.GuardarCajaDeposito(listaCajaDeposito, IdCaja)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
End Class
