Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Controls
Imports ENTITY
Imports Janus.Windows.GridEX
Imports LOGIC
Imports UTILITIES

Public Class frmCajaGeneral
#Region "Privado, metodos y funciones"
    Public _nameButton As String
    Public _tab As SuperTabItem
    Public _modulo As SideNavItem

    Dim listResult As List(Of VCajaGeneral) = New List(Of VCajaGeneral)
    Private Sub Init()
        Try
            ConfigForm()
            Tb_FechaDesde.Value = DateTime.Today
            TB_FechaHasta.Value = DateTime.Today
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try
    End Sub

    Private Sub ConfigForm()
        Try
            Me.Text = "CAJA REPORTE GENERAL"
            Me.WindowState = FormWindowState.Maximized
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub CargarListaCaja()
        Try
            listResult = New LCajaCambio().ListarCajaGeneral_Report(Tb_FechaDesde.Value, TB_FechaHasta.Value)
            ArmarLista()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub ArmarLista()
        Dgv_Caja.BoundMode = Janus.Data.BoundMode.Bound
        Dgv_Caja.DataSource = listResult
        Dgv_Caja.RetrieveStructure()


        With Dgv_Caja.RootTable.Columns("IdCaja")
            .Caption = "Código"
            .Width = 80
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .Visible = False
            .Position = 0
        End With

        With Dgv_Caja.RootTable.Columns("FechaCaja")
            .Caption = "FechaCaja"
            .Width = 100
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .Visible = True
            .Position = 1
        End With

        With Dgv_Caja.RootTable.Columns("Conciliacion")
            .Caption = "Nro. Conciliación"
            .Width = 100
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .Position = 2
        End With
        With Dgv_Caja.RootTable.Columns("FechaPedido")
            .Caption = "FechaPedido"
            .Width = 80
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .Visible = False
            .Position = 3
        End With

        With Dgv_Caja.RootTable.Columns("Repartidor")
            .Caption = "Repartidor"
            .Width = 270
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
            .Position = 4
        End With
        With Dgv_Caja.RootTable.Columns("TotalConciliacion")
            .Caption = "Total Conciliación"
            .Width = 150
            .AggregateFunction = AggregateFunction.Sum
            .FormatString = "0.00"
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .Position = 5
        End With
        With Dgv_Caja.RootTable.Columns("TotalEfectivo")
            .Caption = "Total Efectivo"
            .Width = 130
            .AggregateFunction = AggregateFunction.Sum
            .FormatString = "0.00"
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .Position = 6
        End With
        With Dgv_Caja.RootTable.Columns("TotalCredito")
            .Caption = "Total Crédito"
            .Width = 130
            .FormatString = "0.00"
            .AggregateFunction = AggregateFunction.Sum
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .Position = 7
        End With
        With Dgv_Caja.RootTable.Columns("TotalDeposito")
            .Caption = "Total Depósito"
            .Width = 130
            .FormatString = "0.00"
            .AggregateFunction = AggregateFunction.Sum
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .Position = 8
        End With
        With Dgv_Caja.RootTable.Columns("TotalGeneral")
            .Caption = "Total General"
            .Width = 150
            .FormatString = "0.00"
            .AggregateFunction = AggregateFunction.Sum
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .Position = 9
        End With
        With Dgv_Caja.RootTable.Columns("Diferencia")
            .Caption = "Diferencia"
            .Width = 140
            .AggregateFunction = AggregateFunction.Sum
            .FormatString = "0.00"
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .Position = 10
        End With
        With Dgv_Caja
            .GroupByBoxVisible = False
            .DefaultFilterRowComparison = FilterConditionOperator.Contains
            .FilterMode = FilterMode.Automatic
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges
            .VisualStyle = VisualStyle.Office2007
            .SelectionMode = SelectionMode.MultipleSelection
            .AlternatingColors = True
            .AllowEdit = InheritableBoolean.False
            .AllowColumnDrag = False
            .AutomaticSort = False
            .TotalRow = InheritableBoolean.True
            .TotalRowFormatStyle.FontBold = TriState.True
            .TotalRowFormatStyle.BackColor = Color.Gold
            .TotalRowPosition = TotalRowPosition.BottomScrollable
            '.ColumnHeaders = InheritableBoolean.False
        End With
    End Sub

    Private Sub MostrarMensajeError(mensaje As String)
        ToastNotification.Show(Me,
                               mensaje.ToUpper,
                               My.Resources.WARNING,
                               ENMensaje.MEDIANO,
                               eToastGlowColor.Red,
                               eToastPosition.TopCenter)
    End Sub


#End Region
#Region "Eventos"
    Private Sub frmCajaGeneral_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Init()
    End Sub

    Private Sub Tb_FechaDesde_ValueChanged(sender As Object, e As EventArgs) Handles Tb_FechaDesde.ValueChanged
    End Sub

    Private Sub TB_FechaHasta_ValueChanged(sender As Object, e As EventArgs) Handles TB_FechaHasta.ValueChanged

    End Sub

    Private Sub btGenerar_Click(sender As Object, e As EventArgs) Handles btGenerar.Click
        listResult = New LCajaCambio().ListarCajaGeneral_Report(Tb_FechaDesde.Value, TB_FechaHasta.Value)
        ArmarLista()
    End Sub

    Private Sub bt_Imprimir_Click(sender As Object, e As EventArgs) Handles bt_Imprimir.Click
        Try
            Dim Lista As List(Of VCajaGeneral) = New LCajaCambio().ListarCajaGeneral_Report(Tb_FechaDesde.Value, TB_FechaHasta.Value)
            If (Lista.Count = 0) Then
                Throw New Exception("No registros para generar el reporte.")
            End If

            If Not IsNothing(P_Global.Visualizador) Then
                P_Global.Visualizador.Close()
            End If

            P_Global.Visualizador = New Visualizador
            Dim objrep As New R_CajaGeneralConciliacion

            objrep.SetDataSource(Lista)
            objrep.SetParameterValue("FechaDesde", Tb_FechaDesde.Value)
            objrep.SetParameterValue("FechaHasta", TB_FechaHasta.Value)

            P_Global.Visualizador.CRV1.ReportSource = objrep
            P_Global.Visualizador.Show()
            P_Global.Visualizador.BringToFront()
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try
    End Sub
#End Region
End Class