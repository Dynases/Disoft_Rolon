Imports DATA
Imports ENTITY
Imports REPOSITORY

Public Class RCajaDeposito
    Inherits RBaseTSM
    Implements ICajaDeposito

    Public Function Listar(IdCaja As Integer) As List(Of VCajaDeposito) Implements ICajaDeposito.Listar
        Try
            Using db = GetSchema()
                Dim listResult = (From a In db.TO0052
                                  Where a.onolnumi = IdCaja
                                  Select New VCajaDeposito With {
                                      .Id = a.onnumi,
                                      .IdCaja = a.onolnumi,
                                      .Estado = a.onestado,
                                      .Banco = a.onBanco,
                                      .Moneda = a.onMoneda,
                                      .Depos = a.onNDepos,
                                      .Fecha = a.onFecha,
                                      .Monto = a.onMonto
                                      }).ToList()
                Return listResult
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GuardarCajaDeposito(listCaja As List(Of VCajaDeposito), idCaja As Integer) As Boolean Implements ICajaDeposito.GuardarCajaDeposito
        Try
            Using db = GetSchema()
                Dim listaResu = db.TO0052.Where(Function(x) x.onolnumi = idCaja).FirstOrDefault()
                If Not listaResu Is Nothing Then
                    db.TO0052.RemoveRange(listaResu)
                End If
                For Each Lista As VCajaDeposito In listCaja
                    Dim data = New TO0052 With
                    {
                        .onolnumi = idCaja,
                        .onestado = Lista.Estado,
                        .onBanco = Lista.Banco,
                        .onMoneda = Lista.Moneda,
                        .onNDepos = Lista.Depos,
                        .onFecha = Lista.Fecha,
                        .onMonto = Lista.Monto
                    }
                    db.TO0052.Add(data)
                Next
                db.SaveChanges()
                Return True
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
End Class
