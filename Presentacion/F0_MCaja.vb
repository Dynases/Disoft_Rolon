Imports Logica.AccesoLogica
Imports Janus.Windows.GridEX
Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Controls
Imports System.IO
Imports LOGIC
Imports ENTITY

Public Class F0_MCaja

#Region "VARIABLES GLOBALES"
    Dim Numi_Chofer As Integer
    Dim Numi_Conciliacion As Integer
    Dim Bin As New MemoryStream
    Public _nameButton As String
    Public _tab As SuperTabItem
    Public _modulo As SideNavItem
    Private boShow As Boolean = False
    Private boAdd As Boolean = False
    Private boModif As Boolean = False
    Private boDel As Boolean = False
    Private InDuracion As Byte = 5
    Private ListaCambio As List(Of VCajaCambio) = New List(Of VCajaCambio)
    Private ListaDeposito As List(Of VCajaDeposito) = New List(Of VCajaDeposito)


#End Region


#Region "METODOS PRIVADOS"
    Private Sub _IniciarTodo()

        Try
            ''L_prAbrirConexion(gs_Ip, gs_UsuarioSql, gs_ClaveSql, gs_NombreBD)
            Me.WindowState = FormWindowState.Maximized
            _prCargarComboLibreria(cbbanco, 6, 1)
            _prInhabiliitar()
            _prCargarVenta()
            'Dim blah As New Bitmap(New Bitmap(My.Resources.compra), 20, 20)
            'Dim ico As Icon = Icon.FromHandle(blah.GetHicon())
            'Me.Icon = ico
            Me.Text = "CAJA"

            _prAsignarPermisos()

            If (GridEX1.RowCount > 0) Then
                _prMostrarRegistro(0)
            End If
            SuperTabControl1.SelectedTabIndex = 0
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub
    Private Sub _prCargarComboLibreria(mCombo As Janus.Windows.GridEX.EditControls.MultiColumnCombo, cod1 As String, cod2 As String)
        Try
            Dim dt As New DataTable
            dt = L_prCargarComboBanco()
            With mCombo
                .DropDownList.Columns.Clear()
                .DropDownList.Columns.Add("yccod3").Width = 70
                .DropDownList.Columns("yccod3").Caption = "COD"
                .DropDownList.Columns.Add("ycdes3").Width = 200
                .DropDownList.Columns("ycdes3").Caption = "DESCRIPCION"
                .ValueMember = "ycdes3"
                .DisplayMember = "ycdes3"
                .DataSource = dt
                .Refresh()
            End With
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub
    Private Function _prArmarListaCambio() As List(Of VCajaCambio)
        Try
            ListaCambio = New List(Of VCajaCambio)
            Dim Lista As VCajaCambio = Nothing
            _prLlenarListaCambio(Lista, 200, 100)
            ListaCambio.Add(Lista)

            Lista = Nothing
            _prLlenarListaCambio(Lista, 100, 50)
            ListaCambio.Add(Lista)

            Lista = Nothing
            _prLlenarListaCambio(Lista, 50, 20)
            ListaCambio.Add(Lista)

            Lista = Nothing
            _prLlenarListaCambio(Lista, 20, 10)
            ListaCambio.Add(Lista)

            Lista = Nothing
            _prLlenarListaCambio(Lista, 10, 5)
            ListaCambio.Add(Lista)

            Lista = Nothing
            _prLlenarListaCambio(Lista, 5, 1)
            ListaCambio.Add(Lista)

            Lista = Nothing
            _prLlenarListaCambio(Lista, 2, 0)
            ListaCambio.Add(Lista)

            Lista = Nothing
            _prLlenarListaCambio(Lista, 1, 0)
            ListaCambio.Add(Lista)

            Lista = Nothing
            _prLlenarListaCambio(Lista, 0.5, 0)
            ListaCambio.Add(Lista)

            Lista = Nothing
            _prLlenarListaCambio(Lista, 0.2, 0)
            ListaCambio.Add(Lista)

            Lista = Nothing
            _prLlenarListaCambio(Lista, 0.1, 0)
            ListaCambio.Add(Lista)
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try
    End Function

    Private Sub _prLlenarListaCambio(ByRef Lista As VCajaCambio, CorteBolivianos As Decimal, CordeDolares As Decimal)
        Lista = New VCajaCambio() With {.Id = 0, .IdCaja = 0, .TipoCambio = Tb_TipoCambio.Value,
                .CorteBol = CorteBolivianos, .CantidadBo = 0, .TotalBo = 0,
                .CorteDolares = CordeDolares, .CantidadDo = 0, .TotalD = 0}
    End Sub

    Public Sub _prCrearListaCambio(tipo As Integer, IdCaja As Integer)
        Try
            If tipo = 1 Then
                ListaCambio = New LCajaCambio().Listar(IdCaja)
            End If
            Dgv_Cortes.BoundMode = Janus.Data.BoundMode.Bound
            Dgv_Cortes.DataSource = ListaCambio
            Dgv_Cortes.RetrieveStructure()

            With Dgv_Cortes.RootTable.Columns("Id")
                .Visible = False

            End With

            With Dgv_Cortes.RootTable.Columns("IdCaja")
                .Visible = False
            End With
            With Dgv_Cortes.RootTable.Columns("Estado")
                .Visible = False
            End With
            With Dgv_Cortes.RootTable.Columns("TipoCambio")
                .Visible = False
            End With

            With Dgv_Cortes.RootTable.Columns("CorteBol")
                .Caption = "CORTE BS."
                .Width = 130
                .FormatString = "0.00"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .Visible = True
                .Position = 3
            End With
            With Dgv_Cortes.RootTable.Columns("CantidadBo")
                .Caption = "CANTIDAD BS."
                .Width = 130
                .FormatString = "0.00"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .Visible = True
                .Position = 4
            End With
            With Dgv_Cortes.RootTable.Columns("TotalBo")
                .Caption = "TOTAL BS."
                .Width = 170
                .FormatString = "0.00"
                .AggregateFunction = AggregateFunction.Sum
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .Visible = True
                .Position = 5
            End With
            With Dgv_Cortes.RootTable.Columns("CorteDolares")
                .Caption = "CORTE $us."
                .Width = 130
                .FormatString = "0"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .Visible = True
                .Position = 6
            End With

            With Dgv_Cortes.RootTable.Columns("CantidadDo")
                .Caption = "CANTIDAD $us"
                .Width = 130
                .FormatString = "0"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .Visible = True
                .Position = 7
            End With
            With Dgv_Cortes.RootTable.Columns("TotalD")
                .Caption = "TOTAL $us."
                .Width = 170
                .FormatString = "0.00"
                .AggregateFunction = AggregateFunction.Sum
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .Visible = True
                .Position = 8
            End With
            With Dgv_Cortes
                .GroupByBoxVisible = False
                'diseño de la grilla
                .TotalRow = InheritableBoolean.True
                .TotalRowFormatStyle.BackColor = Color.Gold
                .TotalRowPosition = TotalRowPosition.BottomFixed
                .VisualStyle = VisualStyle.Office2007

            End With
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub

    Private Sub _prArmarListaDeposito()
        Try
            cbbanco.SelectedIndex = 0
            Dim lista As VCajaDeposito = New VCajaDeposito() With {.Id = ListaDeposito.Count + 1, .IdCaja = 0, .Estado = 1, .Banco = cbbanco.Text, .Moneda = "BS", .Depos = "", .Fecha = DateTime.Today, .Monto = 0}
            ListaDeposito.Insert(ListaDeposito.Count, lista)
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub
    Public Sub _prCrearListaDeposito(tipo As Integer, IdCaja As Integer)
        If tipo = 1 Then
            ListaDeposito = New LCajaDeposito().Listar(IdCaja)
        End If
        _prArmarDeposito()
    End Sub
    Private Sub MostrarMensajeError(mensaje As String)
        ToastNotification.Show(Me,
                               mensaje.ToUpper,
                               My.Resources.WARNING,
                               5000,
                               eToastGlowColor.Red,
                               eToastPosition.TopCenter)

    End Sub
    Private Sub _prArmarDeposito()
        Try
            Dgv_Depositos.BoundMode = Janus.Data.BoundMode.Bound
            Dgv_Depositos.DataSource = ListaDeposito
            Dgv_Depositos.RetrieveStructure()
            Dgv_Depositos.AlternatingColors = True

            With Dgv_Depositos.RootTable.Columns("Id")
                .Visible = False

            End With
            With Dgv_Depositos.RootTable.Columns("IdCaja")
                .Visible = False
            End With
            With Dgv_Depositos.RootTable.Columns("Estado")
                .Visible = False
            End With
            With Dgv_Depositos.RootTable.Columns("Banco")
                .Caption = "BANCO"
                .EditType = EditType.MultiColumnDropDown
                .DropDown = cbbanco.DropDownList
                .Width = 250
                .Visible = True
                .Position = 3
            End With
            With Dgv_Depositos.RootTable.Columns("Moneda")
                .Caption = "MONEDA"
                .Width = 100
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = True
                .Position = 4
            End With
            With Dgv_Depositos.RootTable.Columns("Depos")
                .Caption = "NRO. DEPOSITO"
                .Width = 180
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .Visible = True
                .Position = 5
            End With
            With Dgv_Depositos.RootTable.Columns("Fecha")
                .Caption = "FECHA"
                .Width = 150
                .Visible = True
                .FormatString = "dd/MM/yyyy"

            End With
            With Dgv_Depositos.RootTable.Columns("Monto")
                .Caption = "MONTO"
                .Width = 150
                .AggregateFunction = AggregateFunction.Sum
                .FormatString = "0.00"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .Visible = True
                .Position = 7
            End With
            With Dgv_Depositos
                .GroupByBoxVisible = False
                'diseño de la grilla
                .TotalRow = InheritableBoolean.True
                .TotalRowFormatStyle.BackColor = Color.Gold
                .TotalRowPosition = TotalRowPosition.BottomFixed
                .VisualStyle = VisualStyle.Office2007
            End With
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub
    Public Sub _prCargarIcono(dt As DataTable)
        For i As Integer = 0 To dt.Rows.Count - 1 Step 1
            If (dt.Rows(i).Item("TOTAL") = dt.Rows(i).Item("TOTAL_ENTREGADO")) Then
                dt.Rows(i).Item("img") = _fnBytesArchivo(My.Resources.checked, 20, 20)
            Else
                dt.Rows(i).Item("img") = _fnBytesArchivo(My.Resources.cancel, 20, 20)
            End If
        Next
    End Sub

    Private Function _fnBytesArchivo(img As Bitmap, ancho As Integer, alto As Integer) As Object
        Bin = New MemoryStream()
        Dim img2 As New Bitmap(img, ancho, alto)
        img2.Save(Bin, System.Drawing.Imaging.ImageFormat.Png)
        Return Bin.ToArray()
    End Function
    Private Sub _prAsignarPermisos()
        Try
            Dim dtRolUsu() As DataRow = L_prRolDetalleGeneral(gi_userRol).Select("yaprog='" + _nameButton + "'")

            If (dtRolUsu.Count = 1) Then
                boShow = dtRolUsu(0).Item("ycshow")
                boAdd = dtRolUsu(0).Item("ycadd")
                boModif = dtRolUsu(0).Item("ycmod")
                boDel = dtRolUsu(0).Item("ycdel")
            End If

            If boAdd = False Then
                btnNuevo.Visible = False
            End If
            If boModif = False Then
                btnModificar.Visible = False
            End If
            If boDel = False Then
                btnEliminar.Visible = False
            End If
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub

    Private Sub _prInhabiliitar()
        Try
            TbCodigo.ReadOnly = True
            tbchofer.ReadOnly = True
            tbFecha.IsInputReadOnly = True

            ''''''''''
            btnModificar.Enabled = True
            btnGrabar.Enabled = False
            btnNuevo.Enabled = True
            btnEliminar.Enabled = True

            btBuscarChofer.Enabled = False

            GridEX1.Enabled = True

            Dgv_PedidoTotal.AllowEdit = InheritableBoolean.False

            tbdRecibido.IsInputReadOnly = True
            tbdSaldo.IsInputReadOnly = True
            Tb_TipoCambio.IsInputReadOnly = True
            Tb_TConciliacion.IsInputReadOnly = True
            Tb_TCredito.IsInputReadOnly = True
            Tb_TDeposito.IsInputReadOnly = True
            Tb_TDiferencia.IsInputReadOnly = True
            Tb_TGeneral.IsInputReadOnly = True
            Tb_TEfectivo.IsInputReadOnly = True
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub

    Private Sub _prhabilitar()
        TbCodigo.ReadOnly = True
        tbchofer.ReadOnly = False
        tbFecha.IsInputReadOnly = False
        ''  tbCliente.ReadOnly = False  por que solo podra seleccionar Cliente
        ''  tbVendedor.ReadOnly = False

        btnGrabar.Enabled = True

        btBuscarChofer.Enabled = True

        GridEX1.Enabled = False

        Dgv_PedidoTotal.AllowEdit = InheritableBoolean.True

        tbdRecibido.IsInputReadOnly = False

        Tb_TipoCambio.IsInputReadOnly = False
        'Tb_TConsiliacion.IsInputReadOnly = False
        'Tb_TCredito.IsInputReadOnly = False
        'Tb_TDeposito.IsInputReadOnly = False
        'Tb_TDiferencia.IsInputReadOnly = False
        'Tb_TGeneral.IsInputReadOnly = False
        'Tb_TEfectivo.IsInputReadOnly = False
    End Sub

    Public Sub _prFiltrar()
        'cargo el buscador
        Dim _Mpos As Integer
        _prCargarVenta()
        If GridEX1.RowCount > 0 Then
            _Mpos = 0
            GridEX1.Row = _Mpos
        Else
            _Limpiar()
            LblPaginacion.Text = "0/0"
        End If
    End Sub
    Private Sub _Limpiar()
        Try
            TbCodigo.Clear()
            tbchofer.Clear()
            tbFecha.Value = Now.Date
            _prCargarDetalleVenta(-1)
            Numi_Chofer = 0
            Numi_Conciliacion = 0
            lbconciliacion.Text = 0
            tbchofer.Focus()
            tbdRecibido.Value = 0
            tbdSaldo.Value = 0
            Tb_TConciliacion.Value = 0
            Tb_TCredito.Value = 0
            Tb_TDeposito.Value = 0
            Tb_TDiferencia.Value = 0
            Tb_TEfectivo.Value = 0
            Tb_TGeneral.Value = 0
            Tb_TipoCambio.Value = 6.96
            _LimpiarLista()
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub

    Public Sub _prMostrarRegistro(_N As Integer)
        Try
            'olnumi , olnumichof, chofer, olnumiconci, olfecha, olfact, olhact, oluact
            With GridEX1
                TbCodigo.Text = .GetValue("olnumi")
                tbFecha.Value = .GetValue("olfecha")
                Numi_Chofer = .GetValue("olnumichof")
                Numi_Conciliacion = .GetValue("olnumiconci")
                tbchofer.Text = .GetValue("chofer")
                lbconciliacion.Text = .GetValue("olnumiconci")
                lbFecha.Text = CType(.GetValue("olfact"), Date).ToString("dd/MM/yyyy")
                lbHora.Text = .GetValue("olhact").ToString
                lbUsuario.Text = .GetValue("oluact").ToString
                Tb_TCredito.Text = .GetValue("olCredito")
            End With

            _prCargarDetalleVenta(TbCodigo.Text)
            _prCrearListaCambio(1, Convert.ToInt32(TbCodigo.Text))
            _prCrearListaDeposito(1, Convert.ToInt32(TbCodigo.Text))
            Tb_TConciliacion.Value = GridEX1.GetValue("olmrec")
            Tb_TipoCambio.Value = GridEX1.GetValue("olTipoCambio")
            _prCalcular(Tb_TCredito.Value, 2)
            LblPaginacion.Text = Str(GridEX1.Row + 1) + "/" + GridEX1.RowCount.ToString
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try


    End Sub

    Private Function P_prArmarAyudaConciliacion() As Boolean
        Try
            Dim dt As DataTable

            dt = L_prListarConciliacion()
            'a.ibid ,a.ibfdoc ,a.ibconcep ,C.cbnumi as idchofer ,c.cbdesc as chofer

            Dim listEstCeldas As New List(Of Modelo.MCelda)
            listEstCeldas.Add(New Modelo.MCelda("ibid", True, "CONCILIACION", 120))
            listEstCeldas.Add(New Modelo.MCelda("ibfdoc", True, "Fecha", 220, "yyyy/MM/dd"))
            listEstCeldas.Add(New Modelo.MCelda("ibconcep", False))
            listEstCeldas.Add(New Modelo.MCelda("idchofer", False))
            listEstCeldas.Add(New Modelo.MCelda("chofer", True, "CHOFER".ToUpper, 300))


            Dim ef = New Efecto
            ef.tipo = 3
            ef.dt = dt
            ef.SeleclCol = 2
            ef.listEstCeldas = listEstCeldas
            ef.alto = 50
            ef.ancho = 350
            ef.Context = "Seleccione Conciliacion".ToUpper
            ef.ShowDialog()
            Dim bandera As Boolean = False
            bandera = ef.band
            If (bandera = True) Then

                'a.ibid ,a.ibfdoc ,a.ibconcep ,a.ibconcep ,c.cbdesc as chofer
                Dim Row As Janus.Windows.GridEX.GridEXRow = ef.Row

                lbconciliacion.Text = Row.Cells("ibid").Value
                Numi_Conciliacion = Row.Cells("ibid").Value
                tbchofer.Text = Row.Cells("chofer").Value
                Numi_Chofer = Row.Cells("idchofer").Value
            End If
            Return bandera
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Function

    Public Function _fnAccesible()
        Return tbFecha.IsInputReadOnly = False
    End Function
    Private Sub _prCargarDetalleVenta(_numi As String)
        Try
            Dim dt As New DataTable
            dt = L_prObtenerDetalleDeCaja(_numi)
            Dgv_PedidoTotal.DataSource = dt
            Dgv_PedidoTotal.RetrieveStructure()
            Dgv_PedidoTotal.AlternatingColors = True
            'oanumi , oafdoc, oaccli, cliente, oarepa, oaest, oaap, oapg, total,contado,credito, estado
            With Dgv_PedidoTotal.RootTable.Columns("oanumi")
                .Width = 100
                .Caption = "CODIGO"
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("oafdoc")
                .Width = 90
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("oaest")
                .Width = 90
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("oaap")
                .Width = 90
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("oapg")
                .Width = 90
                .Visible = False
            End With

            With Dgv_PedidoTotal.RootTable.Columns("oacnrofac")
                .Width = 150
                .Caption = "NRO. FACTURA"
                .Visible = True
            End With

            With Dgv_PedidoTotal.RootTable.Columns("cliente")
                .Caption = "CLIENTE"
                .Width = 400
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("total")
                .Caption = "TOTAL"
                .Width = 200
                .AggregateFunction = AggregateFunction.Sum
                .Visible = True
                .FormatString = "0.00"
            End With
            With Dgv_PedidoTotal.RootTable.Columns("contado")
                .Caption = "COBRADO"
                .Width = 200
                .AggregateFunction = AggregateFunction.Sum
                .Visible = True
                .FormatString = "0.00"
            End With
            With Dgv_PedidoTotal.RootTable.Columns("credito")
                .Caption = "CREDITO"
                .Width = 200
                .AggregateFunction = AggregateFunction.Sum
                '.Visible = (gi_vcre2 = 1)
                .Visible = True
                .FormatString = "0.00"
            End With
            With Dgv_PedidoTotal.RootTable.Columns("oarepa")
                .Width = 160
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("tcre")
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("estado")
                .Width = 50
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With

            With Dgv_PedidoTotal
                .GroupByBoxVisible = False
                'diseño de la grilla
                .TotalRow = InheritableBoolean.True
                .TotalRowFormatStyle.BackColor = Color.Gold
                .TotalRowPosition = TotalRowPosition.BottomFixed
                .VisualStyle = VisualStyle.Office2007

                Dim fc As GridEXFormatCondition = New GridEXFormatCondition(.RootTable.Columns("tcre"), ConditionOperator.Equal, "1")
                fc.FormatStyle.BackColor = Color.LightSalmon

                .RootTable.FormatConditions.Add(fc)
            End With
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try
    End Sub

    Public Sub cargarDetalleConciliacion()
        Try
            Dim dt As New DataTable

            'dt = L_prObtenerDetalleChofer(Numi_Chofer, tbFecha.Value.ToString("yyyy/MM/dd"))
            dt = L_prObtenerDetalleChofer(Numi_Chofer, tbFecha.Value.ToString("yyyy/MM/dd"), lbconciliacion.Text)
            Dgv_PedidoTotal.DataSource = dt
            Dgv_PedidoTotal.RetrieveStructure()
            Dgv_PedidoTotal.AlternatingColors = True
            'oanumi , oafdoc, oaccli, cliente, oarepa, oaest, oaap, oapg, total,contado,credito, estado
            With Dgv_PedidoTotal.RootTable.Columns("oanumi")
                .Width = 100
                .Caption = "CODIGO"
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("oafdoc")
                .Width = 90
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("oaest")
                .Width = 90
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("oaap")
                .Width = 90
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("oapg")
                .Width = 90
                .Visible = False
            End With


            With Dgv_PedidoTotal.RootTable.Columns("oacnrofac")
                .Width = 150
                .Caption = "Nro Factura"
                .Visible = True
            End With

            With Dgv_PedidoTotal.RootTable.Columns("cliente")
                .Caption = "cliente"
                .Width = 150
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("total")
                .Caption = "TOTAL"
                .Width = 200
                .Visible = True
                .FormatString = "0.00"
                .AggregateFunction = AggregateFunction.Sum
            End With
            With Dgv_PedidoTotal.RootTable.Columns("contado")
                .Caption = "COBRADO"
                .Width = 200
                .Visible = True
                .FormatString = "0.00"
                .AggregateFunction = AggregateFunction.Sum
            End With
            With Dgv_PedidoTotal.RootTable.Columns("credito")
                .Caption = "CREDITO"
                .Width = 200
                '.Visible = (gi_vcre2 = 1)
                .Visible = True
                .FormatString = "0.00"
                .AggregateFunction = AggregateFunction.Sum
            End With
            With Dgv_PedidoTotal.RootTable.Columns("oarepa")
                .Width = 160
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("tcre")
                .Visible = False
            End With
            With Dgv_PedidoTotal.RootTable.Columns("estado")
                .Width = 50
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With

            With Dgv_PedidoTotal
                .GroupByBoxVisible = False
                'diseño de la grilla
                .VisualStyle = VisualStyle.Office2007
                .TotalRow = InheritableBoolean.True
                .TotalRowFormatStyle.BackColor = Color.Gold
                .TotalRowPosition = TotalRowPosition.BottomFixed

                Dim fc As GridEXFormatCondition = New GridEXFormatCondition(.RootTable.Columns("tcre"), ConditionOperator.Equal, "1")
                fc.FormatStyle.BackColor = Color.LightSalmon

                .RootTable.FormatConditions.Add(fc)
            End With
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub

    Private Sub _prCargarVenta()
        Try
            Dim dt As New DataTable
            dt = L_prCajaGeneral()
            GridEX1.DataSource = dt
            GridEX1.RetrieveStructure()
            GridEX1.AlternatingColors = True

            'olnumi , olnumichof, chofer, olnumiconci, olfecha, olfact, olhact, oluact
            With GridEX1.RootTable.Columns("olnumi")
                .Width = 100
                .Caption = "CODIGO"
                .Visible = True
            End With

            With GridEX1.RootTable.Columns("olnumichof")
                .Width = 90
                .Visible = False
            End With

            With GridEX1.RootTable.Columns("chofer")
                .Width = 350
                .Visible = True
                .Caption = "CHOFER"
            End With

            With GridEX1.RootTable.Columns("olnumiconci")
                .Width = 150
                .Visible = True
                .Caption = "CONCILIACIÓN"
            End With

            With GridEX1.RootTable.Columns("olfecha")
                .Width = 120
                .Visible = True
                .Caption = "FECHA"
                .FormatString = "dd/MM/yyyy"
            End With

            With GridEX1.RootTable.Columns("olmrec")
                .Width = 130
                .Visible = True
                .Caption = "TOTAL CONCILIACIÓN"
                .FormatString = "0.00"
            End With

            With GridEX1.RootTable.Columns("olfact")
                .Width = 50
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With

            With GridEX1.RootTable.Columns("olhact")
                .Width = 50
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With

            With GridEX1.RootTable.Columns("oluact")
                .Width = 50
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
            With GridEX1.RootTable.Columns("olCredito")
                .Width = 50
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
            With GridEX1.RootTable.Columns("olTipoCambio")
                .Width = 50
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
            With GridEX1
                .DefaultFilterRowComparison = FilterConditionOperator.Contains
                .FilterMode = FilterMode.Automatic
                .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges
                .GroupByBoxVisible = False
                'diseño de la grilla
                .VisualStyle = VisualStyle.Office2007
            End With

            If (dt.Rows.Count <= 0) Then
                _prCargarDetalleVenta(-1)
            End If
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try
    End Sub

    Public Sub _fnObtenerFilaDetalle(ByRef pos As Integer, numi As Integer)
        For i As Integer = 0 To CType(Dgv_PedidoTotal.DataSource, DataTable).Rows.Count - 1 Step 1
            Dim _numi As Integer = CType(Dgv_PedidoTotal.DataSource, DataTable).Rows(i).Item("oanumi")
            If (_numi = numi) Then
                pos = i
                Return
            End If
        Next

    End Sub
    Private Sub _prSalir()
        If btnGrabar.Enabled = True Then
            _prInhabiliitar()
            If GridEX1.RowCount > 0 Then
                _prMostrarRegistro(0)
            End If
            btnSalir.Text = "SALIR"
        Else
            Me.Close()
            _modulo.Select()
            _tab.Close()
        End If
    End Sub
    Public Sub _PrimerRegistro()
        Dim _MPos As Integer
        If GridEX1.RowCount > 0 Then
            _MPos = 0
            ''   _prMostrarRegistro(_MPos)
            GridEX1.Row = _MPos
        End If
    End Sub

    Public Function _fnValidarTotal() As Boolean

        Dim dt As DataTable = CType(Dgv_Cortes.DataSource, DataTable)
        For i As Integer = 0 To dt.Rows.Count - 1 Step 1

            Dim totalSacado As Double = dt.Rows(i).Item("TOTAL")
            Dim TotalEntregado As Double = dt.Rows(i).Item("TOTAL_ENTREGADO")
            If (totalSacado <> TotalEntregado) Then


                Return False
            End If
        Next
        Return True
    End Function
    Public Function _ValidarCampos() As Boolean
        If (Numi_Chofer <= 0) Then
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "Por Favor Seleccione un Chofer con Ctrl+Enter".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            tbchofer.Focus()
            Return False

        End If
        Return True
    End Function
#End Region
    Private Sub tbchofer_KeyDown(sender As Object, e As KeyEventArgs) Handles tbchofer.KeyDown
        If (_fnAccesible()) Then
            If e.KeyData = Keys.Control + Keys.Enter Then
                P_BuscarChofer()
            End If
        End If
    End Sub

    Private Sub P_BuscarChofer()
        Try
            If P_prArmarAyudaConciliacion() Then
                cargarDetalleConciliacion()
                _LimpiarLista()
                'Colocar el total del contado en el tbdRecibido.Text
                tbdRecibido.Text = Dgv_PedidoTotal.GetTotal(Dgv_PedidoTotal.RootTable.Columns("contado"), AggregateFunction.Sum)
            End If
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try
    End Sub
    Private Sub _LimpiarLista()
        ListaCambio = New List(Of VCajaCambio)
        ListaDeposito = New List(Of VCajaDeposito)
        _prArmarListaCambio()
        _prArmarListaDeposito()
        _prCrearListaCambio(2, 0)
        _prCrearListaDeposito(2, 0)
    End Sub

    Private Sub F0_Caja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _IniciarTodo()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _Limpiar()
        _prhabilitar()

        btnNuevo.Enabled = False
        btnModificar.Enabled = False
        btnEliminar.Enabled = False
        btnGrabar.Enabled = True

        tbchofer.Select()
        btnSalir.Text = "CANCELAR"
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        _prSalir()
    End Sub
    Public Sub _GuardarNuevo()

        Try
            Dim numi As String = ""
            Dim res As Boolean = L_prCajaGrabar(numi, Numi_Chofer, Numi_Conciliacion, tbFecha.Value.ToString("yyyy/MM/dd"), Tb_TConciliacion.Value.ToString, CType(Dgv_PedidoTotal.DataSource, DataTable), Tb_TCredito.Value, Tb_TipoCambio.Value)
            If res Then
                Dim ListaCambios = New LCajaCambio().GuardarCajaCambio(ListaCambio, Convert.ToInt32(numi))
                Dim ListaDepositos = New LCajaDeposito().GuardarDepositoCambio(ListaDeposito, Convert.ToInt32(numi))
                Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)
                ToastNotification.Show(Me, "Código de Caja ".ToUpper + TbCodigo.Text + " Grabado con Exito.".ToUpper,
                                          img, 2000,
                                          eToastGlowColor.Green,
                                          eToastPosition.TopCenter
                                          )

                _prCargarVenta()
                _Limpiar()

            Else
                Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
                ToastNotification.Show(Me, "La Caja no pudo ser insertado".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            End If
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try
    End Sub
    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        If _ValidarCampos() = False Then
            Exit Sub
        End If
        If (TbCodigo.Text = String.Empty) Then
            _GuardarNuevo()
        Else
            If (TbCodigo.Text <> String.Empty) Then
                '_prGuardarModificado()
            End If
        End If
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        _prhabilitar()
        btnNuevo.Enabled = False
        btnModificar.Enabled = False
        btnEliminar.Enabled = False
        btnGrabar.Enabled = True


        tbchofer.Select()
        btnSalir.Text = "CANCELAR"
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Try
            If (GridEX1.RowCount > 0) Then
                Dim ef = New Efecto
                ef.tipo = 2
                ef.Context = "¿esta seguro de eliminar el registro?".ToUpper
                ef.Header = "mensaje principal".ToUpper
                ef.ShowDialog()
                Dim bandera As Boolean = False
                bandera = ef.band
                If (bandera = True) Then
                    Dim mensajeError As String = ""
                    Dim res As Boolean = L_fnCajaEliminar(TbCodigo.Text, CType(Dgv_PedidoTotal.DataSource, DataTable))
                    If res Then
                        Dim img As Bitmap = New Bitmap(My.Resources.checked, 50, 50)
                        ToastNotification.Show(Me, "Código de Caja ".ToUpper + TbCodigo.Text + " eliminado con Exito.".ToUpper,
                                                  img, 2000,
                                                  eToastGlowColor.Green,
                                                  eToastPosition.TopCenter)
                        _prFiltrar()
                    Else
                        Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
                        ToastNotification.Show(Me, mensajeError, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
                    End If
                End If
            End If
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try
    End Sub

    Private Sub GridEX1_SelectionChanged(sender As Object, e As EventArgs) Handles GridEX1.SelectionChanged
        If (GridEX1.RowCount >= 0 And GridEX1.Row >= 0) Then

            _prMostrarRegistro(GridEX1.Row)
        End If
    End Sub

    Private Sub btnSiguiente_Click(sender As Object, e As EventArgs) Handles btnSiguiente.Click
        Dim _pos As Integer = GridEX1.Row
        If _pos < GridEX1.RowCount - 1 Then
            _pos = GridEX1.Row + 1
            '' _prMostrarRegistro(_pos)
            GridEX1.Row = _pos
        End If
    End Sub

    Private Sub btnUltimo_Click(sender As Object, e As EventArgs) Handles btnUltimo.Click
        Dim _pos As Integer = GridEX1.Row
        If GridEX1.RowCount > 0 Then
            _pos = GridEX1.RowCount - 1
            ''  _prMostrarRegistro(_pos)
            GridEX1.Row = _pos
        End If
    End Sub

    Private Sub btnAnterior_Click(sender As Object, e As EventArgs) Handles btnAnterior.Click
        Dim _MPos As Integer = GridEX1.Row
        If _MPos > 0 And GridEX1.RowCount > 0 Then
            _MPos = _MPos - 1
            ''  _prMostrarRegistro(_MPos)
            GridEX1.Row = _MPos
        End If
    End Sub

    Private Sub btnPrimero_Click(sender As Object, e As EventArgs) Handles btnPrimero.Click
        _PrimerRegistro()
    End Sub

    Private Sub grtotalpedidos_KeyDown(sender As Object, e As KeyEventArgs) Handles Dgv_PedidoTotal.KeyDown
        If (e.KeyData = Keys.Control + Keys.Enter And Dgv_PedidoTotal.Row >= 0) Then
            P_prArmarDetallePedido(Dgv_PedidoTotal.GetValue("oanumi"))
        End If
    End Sub
    Private Sub P_prArmarDetallePedido(numi As Integer)
        Try
            Dim dt As DataTable
            dt = L_prVerDetallePedido(numi)
            Dim listEstCeldas As New List(Of Modelo.MCelda)
            listEstCeldas.Add(New Modelo.MCelda("obcprod", True, "CODIGO", 120))
            listEstCeldas.Add(New Modelo.MCelda("cadesc", True, "PRODUCTO", 250))
            listEstCeldas.Add(New Modelo.MCelda("obpbase", True, "PRECIO", 100))
            listEstCeldas.Add(New Modelo.MCelda("obpcant", True, "CANTIDAD", 100))
            listEstCeldas.Add(New Modelo.MCelda("obptot", True, "total".ToUpper, 100))
            Dim ef = New Efecto
            ef.tipo = 3
            ef.dt = dt
            ef.SeleclCol = 2
            ef.listEstCeldas = listEstCeldas
            ef.alto = 50
            ef.ancho = 350
            ef.Context = "detalle de pedido".ToUpper
            ef.ShowDialog()
            Dim bandera As Boolean = False
            bandera = ef.band
            If (bandera = True) Then
                ''a.ibid ,a.ibfdoc ,a.ibconcep ,a.ibconcep ,c.cbdesc as chofer
                'Dim Row As Janus.Windows.GridEX.GridEXRow = ef.Row
                'lbconciliacion.Text = Row.Cells("ibid").Value
                'Numi_Conciliacion = Row.Cells("ibid").Value
                'tbchofer.Text = Row.Cells("chofer").Value
                'Numi_Chofer = Row.Cells("idchofer").Value
            End If
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub


    Function F_GenerarTablaProductoReporte() As DataTable
        Dim dt As DataTable = CType(Dgv_Cortes.DataSource, DataTable)
        Dim TablaPrincipal As DataTable = New DataTable
        Try
            'Select Case'Hielo de 35 kg' AS producto, 20 AS totalpedidos, 10 AS movil, 10 AS pc, 20 AS totalentregado
            TablaPrincipal.Columns.Add("producto", Type.GetType("System.String"))
            TablaPrincipal.Columns.Add("totalpedidos", Type.GetType("System.Int32"))
            TablaPrincipal.Columns.Add("movil", Type.GetType("System.Int32"))
            TablaPrincipal.Columns.Add("pc", Type.GetType("System.Int32"))
            TablaPrincipal.Columns.Add("totalentregado", Type.GetType("System.Int32"))
            For i As Integer = 0 To dt.Rows.Count - 1 Step 1
                TablaPrincipal.Rows.Add()
                Dim n As Integer = TablaPrincipal.Rows.Count - 1
                TablaPrincipal.Rows(n).Item("producto") = CType(Dgv_Cortes.DataSource, DataTable).Rows(i).Item("cadesc")
                TablaPrincipal.Rows(n).Item("totalpedidos") = CType(Dgv_Cortes.DataSource, DataTable).Rows(i).Item("TOTAL")


                If (CType(Dgv_Cortes.DataSource, DataTable).Rows(i).Item("MOVIL").ToString.Equals("")) Then
                    TablaPrincipal.Rows(n).Item("movil") = 0
                Else
                    TablaPrincipal.Rows(n).Item("movil") = CInt(CType(Dgv_Cortes.DataSource, DataTable).Rows(i).Item("MOVIL"))
                End If


                If (CType(Dgv_Cortes.DataSource, DataTable).Rows(i).Item("PC").ToString.Equals("")) Then
                    TablaPrincipal.Rows(n).Item("pc") = 0
                Else
                    TablaPrincipal.Rows(n).Item("pc") = CInt(CType(Dgv_Cortes.DataSource, DataTable).Rows(i).Item("PC"))
                End If


                TablaPrincipal.Rows(n).Item("totalentregado") = CType(Dgv_Cortes.DataSource, DataTable).Rows(i).Item("TOTAL_ENTREGADO")

            Next

            Return TablaPrincipal

        Catch ex As Exception
            MostrarMensajeError(ex.Message)
            Return TablaPrincipal
        End Try

    End Function

    Private Sub P_GenerarReporte()
        Try
            Dim dtProducto As DataTable = F_GenerarTablaProductoReporte()
            Dim dtCliente As DataTable = CType(Dgv_PedidoTotal.DataSource, DataTable)


            If Not IsNothing(P_Global.Visualizador) Then
                P_Global.Visualizador.Close()
            End If

            P_Global.Visualizador = New Visualizador
            Dim objrep As New R_CajaGeneral
            objrep.Subreports.Item("R_CajaProducto.rpt").SetDataSource(dtProducto)
            objrep.Subreports.Item("R_CajaCliente.rpt").SetDataSource(dtCliente)
            objrep.SetDataSource(dtCliente)
            objrep.SetParameterValue("idcaja", TbCodigo.Text)
            objrep.SetParameterValue("chofer", tbchofer.Text)
            objrep.SetParameterValue("conciliacion", lbconciliacion.Text)
            objrep.SetParameterValue("usuario", L_Usuario)
            objrep.SetParameterValue("fecha", tbFecha.Text)

            P_Global.Visualizador.CRV1.ReportSource = objrep
            P_Global.Visualizador.Show()
            P_Global.Visualizador.BringToFront()
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub
    Private Sub P_GenerarReporteNuevo()
        Try
            Dim dtCortes As DataTable = L_prReporteObtenerCortes(TbCodigo.Text)
            Dim dtDepositos As DataTable = L_prReporteObtenerDepositos((TbCodigo.Text))
            Dim dtCliente As DataTable = CType(Dgv_PedidoTotal.DataSource, DataTable)

            If Not IsNothing(P_Global.Visualizador) Then
                P_Global.Visualizador.Close()
            End If

            P_Global.Visualizador = New Visualizador
            Dim objrep As New R_CierreCaja
            objrep.Subreports.Item("R_CajaCortes.rpt").SetDataSource(dtCortes)
            objrep.Subreports.Item("R_CajaDepositos.rpt").SetDataSource(dtDepositos)
            objrep.Subreports.Item("R_CajaDetalle.rpt").SetDataSource(dtCliente)
            objrep.SetDataSource(dtCliente)
            objrep.SetParameterValue("idcaja", TbCodigo.Text)
            objrep.SetParameterValue("chofer", tbchofer.Text)
            objrep.SetParameterValue("conciliacion", lbconciliacion.Text)
            objrep.SetParameterValue("usuario", L_Usuario)
            objrep.SetParameterValue("fecha", tbFecha.Text)
            objrep.SetParameterValue("tipocambio", Tb_TipoCambio.Text)
            'Totales
            objrep.SetParameterValue("TotalEfectivo", Tb_TEfectivo.Text)
            objrep.SetParameterValue("TotalDepositos", Tb_TDeposito.Text)
            objrep.SetParameterValue("TotalCreditos", Tb_TCredito.Text)
            objrep.SetParameterValue("TotalGeneral", Tb_TGeneral.Text)
            objrep.SetParameterValue("TotalConciliacion", Tb_TConciliacion.Text)
            objrep.SetParameterValue("Diferencia", Tb_TDiferencia.Text)

            P_Global.Visualizador.CRV1.ReportSource = objrep
            P_Global.Visualizador.Show()
            P_Global.Visualizador.BringToFront()
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If (TbCodigo.Text.Trim <> String.Empty) Then
            P_GenerarReporteNuevo()
        End If
    End Sub

    Private Sub grtotalpedidos_EditingCell(sender As Object, e As EditingCellEventArgs) Handles Dgv_PedidoTotal.EditingCell
        If (e.Column.Key = "credito") Then
            e.Cancel = False
        Else
            e.Cancel = True
        End If

    End Sub

    Private Sub grtotalpedidos_CellEdited(sender As Object, e As ColumnActionEventArgs) Handles Dgv_PedidoTotal.CellEdited
        Try
            If (e.Column.Key = "credito") Then
                If (Not IsNumeric(Dgv_PedidoTotal.GetValue("credito"))) Then
                    Dgv_PedidoTotal.SetValue("credito", 0)
                Else
                    If (Dgv_PedidoTotal.GetValue("credito") < 0) Then
                        Dgv_PedidoTotal.SetValue("credito", 0)
                    End If
                    If (Dgv_PedidoTotal.GetValue("credito") > Dgv_PedidoTotal.GetValue("contado")) Then
                        Dgv_PedidoTotal.SetValue("credito", Dgv_PedidoTotal.GetValue("contado"))
                    End If
                    Dgv_PedidoTotal.SetValue("contado", Dgv_PedidoTotal.GetValue("total") - Dgv_PedidoTotal.GetValue("credito"))
                End If
                Dgv_PedidoTotal.UpdateData()
                _prCalcular(0, 1)
            End If
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub

    Private Sub btBuscarChofer_Click(sender As Object, e As EventArgs) Handles btBuscarChofer.Click
        P_BuscarChofer()
    End Sub

    Private Sub tbdRecibido_ValueChanged(sender As Object, e As EventArgs) Handles tbdRecibido.ValueChanged
        tbdSaldo.Value = IIf(IsDBNull(CType(Dgv_PedidoTotal.DataSource, DataTable).Compute("sum(total)", "1=1")), 0, CType(Dgv_PedidoTotal.DataSource, DataTable).Compute("sum(total)", "1=1")) - tbdRecibido.Value
    End Sub


    Private Sub Dgv_Cortes_EditingCell(sender As Object, e As EditingCellEventArgs)

    End Sub

    Private Sub Dgv_Cortes_EditingCell_1(sender As Object, e As EditingCellEventArgs) Handles Dgv_Cortes.EditingCell

        If TbCodigo.ReadOnly = True Then
            If (e.Column.Index = Dgv_Cortes.RootTable.Columns("CantidadBo").Index Or
                e.Column.Index = Dgv_Cortes.RootTable.Columns("CantidadDo").Index) Then
                e.Cancel = False
            Else
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub Dgv_Cortes_CellEdited(sender As Object, e As ColumnActionEventArgs) Handles Dgv_Cortes.CellEdited
        Try
            If (e.Column.Key = "CantidadBo" Or e.Column.Key = "CantidadDo") Then
                Dim CorteBo, CantidadBo, CorteDo, CantidadDo, totalDo, tatalBo As Double
                CorteBo = Convert.ToDouble(Dgv_Cortes.CurrentRow.Cells("CorteBol").Value)
                CantidadBo = Convert.ToDouble(Dgv_Cortes.CurrentRow.Cells("CantidadBo").Value)
                tatalBo = CorteBo * CantidadBo
                Dgv_Cortes.CurrentRow.Cells("TotalBo").Value = tatalBo

                CorteDo = Convert.ToDouble(Dgv_Cortes.CurrentRow.Cells("CorteDolares").Value)
                CantidadDo = Convert.ToDouble(Dgv_Cortes.CurrentRow.Cells("CantidadDo").Value)
                totalDo = CorteDo * CantidadDo
                Dgv_Cortes.CurrentRow.Cells("TotalD").Value = totalDo
                Dgv_Cortes.UpdateData()

                _prCalcular(0, 1)
            End If
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub
    Private Sub Dgv_Depositos_CellEdited(sender As Object, e As ColumnActionEventArgs) Handles Dgv_Depositos.CellEdited
        If (e.Column.Key = "Monto") Then
            Dgv_Depositos.UpdateData()
            _prCalcular(0, 1)
        End If
    End Sub
    Private Sub _prCalcular(credito As Double, tipo As Integer)
        Try
            Dim totalCorteDol, totalCorteBol, TotalDeposito, totalConciliacion As Double
            totalCorteBol = Dgv_Cortes.GetTotal(Dgv_Cortes.RootTable.Columns("TotalBo"), AggregateFunction.Sum)
            If tipo = 1 Then
                credito = Dgv_PedidoTotal.GetTotal(Dgv_PedidoTotal.RootTable.Columns("credito"), AggregateFunction.Sum)
            Else
                credito = credito
            End If
            totalCorteDol = Dgv_Cortes.GetTotal(Dgv_Cortes.RootTable.Columns("TotalD"), AggregateFunction.Sum)
            TotalDeposito = Dgv_Depositos.GetTotal(Dgv_Depositos.RootTable.Columns("Monto"), AggregateFunction.Sum)
            totalConciliacion = Dgv_PedidoTotal.GetTotal(Dgv_PedidoTotal.RootTable.Columns("total"), AggregateFunction.Sum)
            Tb_TEfectivo.Value = totalCorteBol + (totalCorteDol * Tb_TipoCambio.Value)
            Tb_TDeposito.Value = TotalDeposito
            Tb_TCredito.Value = credito
            Tb_TGeneral.Value = Tb_TEfectivo.Value + Tb_TDeposito.Value + Tb_TCredito.Value
            Tb_TConciliacion.Value = totalConciliacion
            Tb_TDiferencia.Value = Tb_TGeneral.Value - Tb_TConciliacion.Value
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub

    Private Sub Dgv_Depositos_EditingCell(sender As Object, e As EditingCellEventArgs) Handles Dgv_Depositos.EditingCell
        Try
            If TbCodigo.ReadOnly = True Then
                If (e.Column.Index = Dgv_Depositos.RootTable.Columns("Banco").Index Or
                    e.Column.Index = Dgv_Depositos.RootTable.Columns("Moneda").Index Or
                    e.Column.Index = Dgv_Depositos.RootTable.Columns("Depos").Index Or
                    e.Column.Index = Dgv_Depositos.RootTable.Columns("Fecha").Index Or
                    e.Column.Index = Dgv_Depositos.RootTable.Columns("Monto").Index) Then
                    e.Cancel = False
                Else
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub

    Private Sub Dgv_Depositos_KeyDown(sender As Object, e As KeyEventArgs) Handles Dgv_Depositos.KeyDown
        Try
            If (_fnAccesible()) Then
                If e.KeyData = Keys.Enter Then
                    If (Dgv_Depositos.Col = Dgv_Depositos.RootTable.Columns("Banco").Index) Then
                        Dgv_Depositos.UpdateData()
                        ListaDeposito = Dgv_Depositos.DataSource
                        _prArmarListaDeposito()
                        _prArmarDeposito()
                    End If
                End If
                If e.KeyData = Keys.Escape Then
                    If Dgv_Depositos.RowCount > 1 Then
                        Dim Id = Dgv_Depositos.GetValue("Id")
                        Dgv_Depositos.UpdateData()
                        ListaDeposito = Dgv_Depositos.DataSource
                        Dim lista = ListaDeposito.Where(Function(x) x.Id = Id).FirstOrDefault()
                        ListaDeposito.Remove(lista)
                        '_prArmarListaDeposito()
                        Dgv_Depositos.DataSource = Nothing
                        _prArmarDeposito()
                    Else
                        Throw New Exception("Detalle de deposito no puede estar vacio")
                    End If
                End If
            End If
        Catch ex As Exception
            MostrarMensajeError(ex.Message)
        End Try

    End Sub

    Private Sub Tb_TipoCambio_ValueChanged(sender As Object, e As EventArgs) Handles Tb_TipoCambio.ValueChanged
        If (Tb_TipoCambio.Focused) Then
            Dgv_Depositos.UpdateData()
            Dgv_Depositos.UpdateData()
            Dgv_PedidoTotal.UpdateData()
            _prCalcular(0, 1)
        End If
    End Sub

    Private Sub GridEX1_KeyDown(sender As Object, e As KeyEventArgs) Handles GridEX1.KeyDown
        If (e.KeyData = Keys.Enter) Then
            SuperTabControl1.SelectedTabIndex = 0
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub GridEX1_DoubleClick(sender As Object, e As EventArgs) Handles GridEX1.DoubleClick
        If (GridEX1.Row > -1) Then
            SuperTabControl1.SelectedTabIndex = 0
        End If
    End Sub

    Private Sub SuperTabControl1_SelectedTabChanged(sender As Object, e As SuperTabStripSelectedTabChangedEventArgs) Handles SuperTabControl1.SelectedTabChanged

    End Sub
End Class