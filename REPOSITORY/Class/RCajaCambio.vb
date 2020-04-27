Imports DATA
Imports ENTITY
Imports REPOSITORY


Public Class RCajaCambio
    Inherits RBaseTSM
    Implements ICajaCambio

    Public Function Listar(IdCaja As Integer) As List(Of VCajaCambio) Implements ICajaCambio.Listar
        Try
            Using db = GetSchema()
                Dim listResult = (From a In db.TO0051
                                  Where a.omolnumi = IdCaja
                                  Select New VCajaCambio With {
                                      .Id = a.omnumi,
                                      .IdCaja = a.omolnumi,
                                      .TipoCambio = a.omTipoCam,
                                      .CorteBol = a.omCorteB,
                                      .CantidadBo = a.omCantB,
                                      .TotalBo = a.omTotalB,
                                      .CorteDolares = a.omCorteD,
                                      .CantidadDo = a.omCantD,
                                      .TotalD = a.omCantD
                                      }).ToList()
                Return listResult
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GuardarCajaCambio(listIdCaja As List(Of VCajaCambio), idCaja As Integer) As Boolean Implements ICajaCambio.GuardarCajaCambio
        Try
            Using db = GetSchema()
                Dim listaResu = db.TO0051.Where(Function(x) x.omolnumi = idCaja).FirstOrDefault()
                If Not listaResu Is Nothing Then
                    db.TO0051.RemoveRange(listaResu)
                End If
                For Each Lista As VCajaCambio In listIdCaja
                    Dim data = New TO0051 With
                    {
                        .omolnumi = idCaja,
                        .omestado = Lista.Estado,
                        .omTipoCam = Lista.TipoCambio,
                        .omCorteD = Lista.CorteDolares,
                        .omCantD = Lista.CantidadDo,
                        .omTotalD = Lista.TotalD,
                        .omCorteB = Lista.CorteBol,
                        .omCantB = Lista.CantidadBo,
                        .omTotalB = Lista.TotalBo
                    }
                    db.TO0051.Add(data)
                Next
                db.SaveChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function ListarCajaGeneral_Report(FechaDesde As Date, fechaHasta As Date) As List(Of VCajaGeneral) Implements ICajaCambio.ListarCajaGeneral_Report
        Try
            Using db = GetSchema()
                Dim listResult = (From a In db.VR_GO_CajaGeneral
                                  Where a.FechaCaja >= FechaDesde And a.FechaCaja <= fechaHasta
                                  Order By a.IdCaja
                                  Select New VCajaGeneral With {
                                      .IdCaja = a.IdCaja,
                                      .FechaCaja = a.FechaCaja,
                                      .Conciliacion = a.Conciliacion,
                                      .FechaPedido = a.FechaPedido,
                                      .Repartidor = a.Repartidor,
                                      .TotalConciliacion = a.TotalConciliacion,
                                      .TotalEfectivo = a.TotalEfectivo,
                                      .TotalCredito = a.TotalCredito,
                                      .TotalDeposito = a.TotalDeposito,
                                      .TotalGeneral = a.TotalGeneral,
                                      .Diferencia = a.Diferencia
                                      }).ToList()
                Return listResult
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
End Class
