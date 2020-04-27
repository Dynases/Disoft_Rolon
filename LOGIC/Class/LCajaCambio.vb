Imports ENTITY
Imports REPOSITORY
Public Class LCajaCambio
    Friend iCajaCambio As ICajaCambio

    Public Sub New()
        Me.iCajaCambio = New RCajaCambio()
    End Sub
    Public Function Listar(IdCaja As Integer) As List(Of VCajaCambio)
        Try
            Return iCajaCambio.Listar(IdCaja)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GuardarCajaCambio(listaCajaCambio As List(Of VCajaCambio), IdCaja As Integer) As Boolean
        Try
            Return iCajaCambio.GuardarCajaCambio(listaCajaCambio, IdCaja)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function ListarCajaGeneral_Report(FechaDesde As DateTime, fechaHasta As DateTime) As List(Of VCajaGeneral)
        Try
            Return iCajaCambio.ListarCajaGeneral_Report(FechaDesde, fechaHasta)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function


End Class
