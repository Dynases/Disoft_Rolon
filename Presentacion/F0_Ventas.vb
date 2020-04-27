Imports Logica.AccesoLogica
Imports Janus.Windows.GridEX
Imports DevComponents.DotNetBar
Imports System.IO
Imports DevComponents.DotNetBar.SuperGrid
Imports GMap.NET.MapProviders
Imports GMap.NET
Imports GMap.NET.WindowsForms.Markers
Imports GMap.NET.WindowsForms
Imports GMap.NET.WindowsForms.ToolTips
Imports System.Drawing
Imports DevComponents.DotNetBar.Controls
Imports System.Threading
Imports System.Drawing.Text
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Drawing.Printing
Imports CrystalDecisions.Shared
Imports Facturacion

Public Class F0_Ventas

#Region "Variables Globales"
    Dim _CodCliente As Integer = 0
    Dim _CodEmpleado As Integer = 0
    Dim OcultarFact As Integer = 0
    Dim _codeBar As Integer = 1
    Dim _dias As Integer = 0
    Public _nameButton As String
    Public _tab As SuperTabItem
    Public _modulo As SideNavItem
    Dim FilaSelectLote As DataRow = Nothing
    Dim Table_Producto As DataTable
    Dim G_Lote As Boolean = False '1=igual a mostrar las columnas de lote y fecha de Vencimiento
    Dim prod As String
    Dim grup1 As String = " "
    Dim grup2 As String = " "

#End Region

#Region "Metodos Privados"
    Private Sub _IniciarTodo()
        MSuperTabControl.SelectedTabIndex = 0
        Me.WindowState = FormWindowState.Maximized

        lbTipoMoneda.Visible = False
        swMoneda.Visible = False

        _prCargarVenta()
        _prInhabiliitar()
        grVentas.Focus()
        Me.Text = "VENTAS"
        Dim blah As New Bitmap(New Bitmap(My.Resources.VENT_PAGOS), 20, 20)
        Dim ico As Icon = Icon.FromHandle(blah.GetHicon())
        Me.Icon = ico
        _prAsignarPermisos()
        _prValidadFactura()


        'Ocultar botones
        btnNuevo.Visible = False
        btnModificar.Visible = False
        btnGrabar.Visible = False
        btnEliminar.Visible = False
        LabelX17.Visible = False
        cbSucursal.Visible = False
        LabelX3.Visible = False
        tbObservacion.Visible = False
        lbIce.Visible = False
        tbIce.Visible = False
        LabelX13.Visible = False
        tbSubTotal.Visible = False
        LabelX9.Visible = False
        tbMdesc.Visible = False
        LabelX8.Visible = False
        tbPdesc.Visible = False

        'Ocultar paneles de Facturación
        'GroupPanelFactura2.Visible = False
        'GroupPanelFactura.Visible = False
    End Sub
    Public Sub _prCargarNameLabel()
        Dim dt As DataTable = L_fnNameLabel()
        If (dt.Rows.Count > 0) Then
            _codeBar = 1 'dt.Rows(0).Item("codeBar")
        End If
    End Sub
    Sub _prValidadFactura()
        'If (OcultarFact = 1) Then
        '    GroupPanelFactura2.Visible = False
        '    GroupPanelFactura.Visible = False
        'Else
        '    GroupPanelFactura2.Visible = True
        '    GroupPanelFactura.Visible = True
        'End If

    End Sub

    Private Sub _prCargarComboLibreriaSucursal(mCombo As Janus.Windows.GridEX.EditControls.MultiColumnCombo)
        'Dim dt As New DataTable
        'dt = L_fnListarSucursales()
        'With mCombo
        '    .DropDownList.Columns.Clear()
        '    .DropDownList.Columns.Add("aanumi").Width = 60
        '    .DropDownList.Columns("aanumi").Caption = "COD"
        '    .DropDownList.Columns.Add("aabdes").Width = 500
        '    .DropDownList.Columns("aabdes").Caption = "SUCURSAL"
        '    .ValueMember = "aanumi"
        '    .DisplayMember = "aabdes"
        '    .DataSource = dt
        '    .Refresh()
        'End With
    End Sub
    Private Sub _prAsignarPermisos()
        Dim dtRolUsu() As DataRow = L_prRolDetalleGeneral(gi_userRol).Select("yaprog='" + _nameButton + "'")
        Dim show, add, modif, del As Boolean

        If (dtRolUsu.Count = 1) Then
            show = dtRolUsu(0).Item("ycshow")
            add = dtRolUsu(0).Item("ycadd")
            modif = dtRolUsu(0).Item("ycmod")
            del = dtRolUsu(0).Item("ycdel")
        End If

        If add = False Then
            btnNuevo.Visible = False
        End If
        If modif = False Then
            btnModificar.Visible = False
        End If
        If del = False Then
            btnEliminar.Visible = False
        End If
    End Sub
    Private Sub _prInhabiliitar()

        tbCodigo.ReadOnly = True
        tbCliente.ReadOnly = True
        tbVendedor.ReadOnly = True
        tbObservacion.ReadOnly = True
        tbFechaVenta.IsInputReadOnly = True
        tbFechaVenc.IsInputReadOnly = True
        swMoneda.IsReadOnly = True
        swTipoVenta.IsReadOnly = True
        Sb1Estado.IsReadOnly = True
        swEmision.IsReadOnly = True

        'Datos facturacion
        tbNroAutoriz.ReadOnly = True
        tbNroFactura.ReadOnly = True
        tbCodigoControl.ReadOnly = True
        dtiFechaFactura.IsInputReadOnly = True
        dtiFechaFactura.ButtonDropDown.Enabled = False

        btnModificar.Enabled = True
        btnGrabar.Enabled = False
        btnNuevo.Enabled = True
        btnEliminar.Enabled = True

        tbSubTotal.IsInputReadOnly = True
        tbIce.IsInputReadOnly = True
        tbtotal.IsInputReadOnly = True

        grVentas.Enabled = True
        PanelNavegacion.Enabled = True
        grdetalle.RootTable.Columns("img").Visible = False
        If (GPanelProductos.Visible = True) Then
            _DesHabilitarProductos()
        End If

        TbNit.ReadOnly = True
        TbNombre1.ReadOnly = True
        TbNombre2.ReadOnly = True
        cbSucursal.ReadOnly = True
        FilaSelectLote = Nothing
    End Sub
    Private Sub _prhabilitar()

        grVentas.Enabled = False
        tbCodigo.ReadOnly = False
        ''  tbCliente.ReadOnly = False  por que solo podra seleccionar Cliente
        ''  tbVendedor.ReadOnly = False
        tbObservacion.ReadOnly = False
        tbFechaVenta.IsInputReadOnly = False
        tbFechaVenc.IsInputReadOnly = False
        swMoneda.IsReadOnly = False
        swTipoVenta.IsReadOnly = False
        btnGrabar.Enabled = True

        TbNit.ReadOnly = False
        TbNombre1.ReadOnly = False
        TbNombre2.ReadOnly = False

        'Datos facturacion
        tbNroAutoriz.ReadOnly = False
        tbNroFactura.ReadOnly = False
        tbCodigoControl.ReadOnly = False
        dtiFechaFactura.IsInputReadOnly = False

        If (tbCodigo.Text.Length > 0) Then
            cbSucursal.ReadOnly = True
        Else
            cbSucursal.ReadOnly = False

        End If
    End Sub
    Public Sub _prFiltrar()
        'cargo el buscador
        Dim _Mpos As Integer
        _prCargarVenta()
        If grVentas.RowCount > 0 Then
            _Mpos = 0
            grVentas.Row = _Mpos
        Else
            _Limpiar()
            LblPaginacion.Text = "0/0"
        End If
    End Sub
    Private Sub _Limpiar()
        tbCodigo.Clear()
        tbCliente.Clear()
        tbVendedor.Clear()
        tbObservacion.Clear()
        swMoneda.Value = True
        swTipoVenta.Value = True
        _CodCliente = 0
        _CodEmpleado = 0
        tbFechaVenta.Value = Now.Date
        tbFechaVenc.Visible = False
        lbCredito.Visible = False
        _prCargarDetalleVenta(-1)
        MSuperTabControl.SelectedTabIndex = 0
        tbSubTotal.Value = 0
        tbPdesc.Value = 0
        tbMdesc.Value = 0
        tbIce.Value = 0
        tbtotal.Value = 0
        With grdetalle.RootTable.Columns("img")
            .Width = 80
            .Caption = "Eliminar"
            .CellStyle.ImageHorizontalAlignment = ImageHorizontalAlignment.Center
            .Visible = True
        End With
        _prAddDetalleVenta()
        If (GPanelProductos.Visible = True) Then
            GPanelProductos.Visible = False
            PanelTotal.Visible = True
            PanelInferior.Visible = True
        End If
        tbCliente.Focus()

        TbNit.Clear()
        TbNombre1.Clear()
        TbNombre2.Clear()

        tbNroAutoriz.Clear()
        tbNroFactura.Clear()
        tbCodigoControl.Clear()
        dtiFechaFactura.Value = Now.Date
        If (CType(cbSucursal.DataSource, DataTable).Rows.Count > 0) Then
            cbSucursal.SelectedIndex = 0
        End If
        FilaSelectLote = Nothing

        tbCliente.Focus()
        Table_Producto = Nothing
    End Sub
    Public Sub _prMostrarRegistro(_N As Integer)
        '' grVentas.Row = _N
        '     a.tanumi ,a.taalm ,a.tafdoc ,a.taven ,vendedor .yddesc as vendedor ,a.tatven ,a.tafvcr ,a.taclpr,
        'cliente.yddesc as cliente ,a.tamon ,IIF(tamon=1,'Boliviano','Dolar') as moneda,a.taest ,a.taobs ,
        'a.tadesc ,a.tafact ,a.tahact ,a.tauact,(Sum(b.tbptot)-a.tadesc ) as total,taproforma

        With grVentas
            cbSucursal.Value = .GetValue("taalm")
            tbCodigo.Text = .GetValue("tanumi")
            tbFechaVenta.Value = .GetValue("tafdoc")
            _CodEmpleado = .GetValue("oaccbnumi")
            tbVendedor.Text = .GetValue("repartidor")
            swTipoVenta.Value = .GetValue("tatven")
            _CodCliente = .GetValue("taclpr")
            tbCliente.Text = .GetValue("cliente")
            swMoneda.Value = .GetValue("tamon")


            tbFechaVenc.Value = .GetValue("tafvcr")


            'If (gb_FacturaEmite) Then
            Dim dt As DataTable = L_fnObtenerTabla("fvanitcli, fvadescli1, fvadescli2, fvaautoriz, fvanfac, fvaccont, fvafec,fvaest", "TFV001", "fvanumi=" + tbCodigo.Text.Trim + " and fvaest=1")
            If (dt.Rows.Count = 1) Then
                TbNit.Text = dt.Rows(0).Item("fvanitcli").ToString
                TbNombre1.Text = dt.Rows(0).Item("fvadescli1").ToString
                TbNombre2.Text = dt.Rows(0).Item("fvadescli2").ToString

                tbNroAutoriz.Text = dt.Rows(0).Item("fvaautoriz").ToString
                tbNroFactura.Text = dt.Rows(0).Item("fvanfac").ToString
                tbCodigoControl.Text = dt.Rows(0).Item("fvaccont").ToString
                dtiFechaFactura.Value = dt.Rows(0).Item("fvafec")
                Sb1Estado.Value = dt.Rows(0).Item("fvaest")
            Else
                TbNit.Clear()
                TbNombre1.Clear()
                TbNombre2.Clear()

                tbNroAutoriz.Clear()
                tbNroFactura.Clear()
                tbCodigoControl.Clear()
                dtiFechaFactura.Value = "2000/01/01"
            End If
            'End If

            lbFecha.Text = CType(.GetValue("tafact"), Date).ToString("dd/MM/yyyy")
            lbHora.Text = .GetValue("tahact").ToString
            lbUsuario.Text = .GetValue("tauact").ToString

        End With

        _prCargarDetalleVenta(tbCodigo.Text)
        'tbMdesc.Value = grVentas.GetValue("tadesc")
        'tbIce.Value = grVentas.GetValue("taice")
        _prCalcularPrecioTotal()
        LblPaginacion.Text = Str(grVentas.Row + 1) + "/" + grVentas.RowCount.ToString

    End Sub

    Private Sub _prCargarDetalleVenta(_numi As String)
        Dim dt As New DataTable
        dt = L_fnDetalleVenta(_numi)
        grdetalle.DataSource = dt
        grdetalle.RetrieveStructure()
        grdetalle.AlternatingColors = True
        '      a.tbnumi ,a.tbtv1numi ,a.tbty5prod ,b.yfcdprod1 as producto,a.tbest ,a.tbcmin ,a.tbumin ,Umin .ycdes3 as unidad,a.tbpbas ,a.tbptot,a.tbdesc ,a.tbobs ,
        'a.tbfact ,a.tbhact ,a.tbuact

        With grdetalle.RootTable.Columns("tbnumi")
            .Width = 100
            .Caption = "CODIGO"
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("tbtv1numi")
            .Width = 90
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("tbty5prod")
            .Caption = "COD.PROD"
            .Width = 90
            .Visible = True
        End With
        With grdetalle.RootTable.Columns("producto")
            .Caption = "PRODUCTOS"
            .Width = 250
            .Visible = True
        End With
        With grdetalle.RootTable.Columns("tbest")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("tbcmin")
            .Width = 160
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .FormatString = "0.00"
            .Caption = "Cantidad".ToUpper
        End With
        With grdetalle.RootTable.Columns("tbumin")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("tbpbas")
            .Width = 120
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .FormatString = "0.00"
            .Caption = "Precio U.".ToUpper
        End With
        With grdetalle.RootTable.Columns("tbptot")
            .Width = 100
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .FormatString = "0.00"
            .Caption = "Sub Total".ToUpper
        End With
        With grdetalle.RootTable.Columns("tbporc")
            .Width = 100
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = False
            .FormatString = "0.00"
            .Caption = "P.Desc(%)".ToUpper
        End With
        With grdetalle.RootTable.Columns("tbdesc")
            .Width = 100
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .FormatString = "0.00"
            .Caption = "Descuento".ToUpper
        End With
        With grdetalle.RootTable.Columns("tbtotdesc")
            .Width = 100
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .FormatString = "0.00"
            .Caption = "Total".ToUpper
        End With
        With grdetalle.RootTable.Columns("tbobs")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("tbpcos")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("tbptot2")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("tbfact")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("tbhact")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("tbuact")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("estado")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grdetalle.RootTable.Columns("img")
            .Width = 80
            .Caption = "Eliminar".ToUpper
            .CellStyle.ImageHorizontalAlignment = ImageHorizontalAlignment.Center
            .Visible = False
        End With
        With grdetalle
            .GroupByBoxVisible = False
            'diseño de la grilla
            .VisualStyle = VisualStyle.Office2007
        End With
    End Sub

    Private Sub _prCargarVenta()
        Dim dt As New DataTable
        dt = L_fnGeneralVenta()
        grVentas.DataSource = dt
        grVentas.RetrieveStructure()
        grVentas.AlternatingColors = True

        With grVentas.RootTable.Columns("tanumi")
            .Width = 100
            .Caption = "CÓDIGO"
            .Visible = True
        End With
        With grVentas.RootTable.Columns("oacnrofac")
            .Width = 120
            .Caption = "NRO. FACTURA"
            .Visible = True
        End With
        With grVentas.RootTable.Columns("taalm")
            .Width = 90
            .Visible = False
        End With
        With grVentas.RootTable.Columns("tafdoc")
            .Width = 90
            .Visible = True
            .Caption = "FECHA"
        End With
        With grVentas.RootTable.Columns("oaccbnumi")
            .Width = 160
            .Visible = False
        End With
        With grVentas.RootTable.Columns("repartidor")
            .Width = 250
            .Visible = True
            .Caption = "Repartidor".ToUpper
        End With
        With grVentas.RootTable.Columns("tatven")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grVentas.RootTable.Columns("tafvcr")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grVentas.RootTable.Columns("taclpr")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grVentas.RootTable.Columns("cliente")
            .Width = 250
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
            .Caption = "CLIENTE"
        End With
        With grVentas.RootTable.Columns("tamon")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grVentas.RootTable.Columns("moneda")
            .Width = 150
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
            .Caption = "MONEDA"
        End With
        With grVentas.RootTable.Columns("taest")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With

        With grVentas.RootTable.Columns("tafact")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grVentas.RootTable.Columns("tahact")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grVentas.RootTable.Columns("tauact")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grVentas.RootTable.Columns("total")
            .Width = 150
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .Caption = "TOTAL"
            .FormatString = "0.00"
        End With
        With grVentas
            .DefaultFilterRowComparison = FilterConditionOperator.Contains
            .FilterMode = FilterMode.Automatic
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges
            .GroupByBoxVisible = False
            'diseño de la grilla

        End With

        If (dt.Rows.Count <= 0) Then
            _prCargarDetalleVenta(-1)
        End If
    End Sub


    Public Sub actualizarSaldoSinLote(ByRef dt As DataTable)
        'b.yfcdprod1 ,a.iclot ,a.icfven  ,a.iccven 

        '      a.tbnumi ,a.tbtv1numi ,a.tbty5prod ,b.yfcdprod1 as producto,a.tbest ,a.tbcmin ,a.tbumin ,Umin .ycdes3 as unidad,a.tbpbas ,a.tbptot ,a.tbobs ,
        'a.tbpcos,a.tblote ,a.tbfechaVenc , a.tbptot2, a.tbfact ,a.tbhact ,a.tbuact,1 as estado,Cast(null as Image) as img,
        'Cast (0 as decimal (18,2)) as stock
        Dim _detalle As DataTable = CType(grdetalle.DataSource, DataTable)

        For i As Integer = 0 To dt.Rows.Count - 1 Step 1
            Dim sum As Integer = 0
            Dim codProducto As Integer = dt.Rows(i).Item("yfnumi")
            For j As Integer = 0 To grdetalle.RowCount - 1 Step 1
                grdetalle.Row = j
                Dim estado As Integer = grdetalle.GetValue("estado")
                If (estado = 0) Then
                    If (codProducto = grdetalle.GetValue("tbty5prod")) Then
                        sum = sum + grdetalle.GetValue("tbcmin")
                    End If
                End If
            Next
            dt.Rows(i).Item("stock") = dt.Rows(i).Item("stock") - sum
        Next

    End Sub

    Private Sub _prCargarProductos(_cliente As String)
        If (cbSucursal.SelectedIndex < 0) Then
            Return
        End If
        Dim dtname As DataTable = L_fnNameLabel()
        Dim dt As New DataTable

        If (G_Lote = True) Then
            dt = L_fnListarProductos(cbSucursal.Value, _cliente)  ''1=Almacen
            'Table_Producto = dt.Copy
        Else
            dt = L_fnListarProductosSinLote(cbSucursal.Value, _cliente, CType(grdetalle.DataSource, DataTable))  ''1=Almacen
            'Table_Producto = dt.Copy
        End If



        ''  actualizarSaldoSinLote(dt)
        grProductos.DataSource = dt
        grProductos.RetrieveStructure()
        grProductos.AlternatingColors = True

        '      a.yfnumi ,a.yfcprod ,a.yfcdprod1,a.yfcdprod2 ,a.yfgr1,gr1.ycdes3 as grupo1,a.yfgr2
        ',gr2.ycdes3 as grupo2 ,a.yfgr3,gr3.ycdes3 as grupo3,a.yfgr4 ,gr4 .ycdes3 as grupo4,a.yfumin ,Umin .ycdes3 as UnidMin
        ' ,b.yhprecio 

        With grProductos.RootTable.Columns("yfnumi")
            .Width = 100
            .Caption = "CODIGO"
            .Visible = False

        End With
        With grProductos.RootTable.Columns("yfcprod")
            .Width = 60
            .Caption = "CÓDIGO"
            .Visible = True
        End With

        With grProductos.RootTable.Columns("yfcdprod1")
            .Width = 250
            .Visible = True
            .Caption = "DESCRIPCIÓN"
        End With
        With grProductos.RootTable.Columns("yfcdprod2")
            .Width = 150
            .Visible = False
            .Caption = "Descripcion Corta"
        End With


        With grProductos.RootTable.Columns("yfgr1")
            .Width = 160
            .Visible = False
        End With
        If (dtname.Rows.Count > 0) Then

            With grProductos.RootTable.Columns("grupo1")
                .Width = 120
                .Caption = dtname.Rows(0).Item("Grupo 1").ToString
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = True
            End With
            With grProductos.RootTable.Columns("grupo2")
                .Width = 120
                .Caption = dtname.Rows(0).Item("Grupo 2").ToString
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = True
            End With

            With grProductos.RootTable.Columns("grupo3")
                .Width = 120
                .Caption = dtname.Rows(0).Item("Grupo 3").ToString
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
            With grProductos.RootTable.Columns("grupo4")
                .Width = 120
                .Caption = dtname.Rows(0).Item("Grupo 4").ToString
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
        Else
            With grProductos.RootTable.Columns("grupo1")
                .Width = 120
                .Caption = "Grupo 1"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = True
            End With
            With grProductos.RootTable.Columns("grupo2")
                .Width = 120
                .Caption = "Grupo 2"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = True
            End With
            With grProductos.RootTable.Columns("grupo3")
                .Width = 120
                .Caption = "Grupo 3"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
            With grProductos.RootTable.Columns("grupo4")
                .Width = 120
                .Caption = "Grupo 4"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
        End If


        With grProductos.RootTable.Columns("yfgr2")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With

        With grProductos.RootTable.Columns("yfgr3")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With

        With grProductos.RootTable.Columns("yfgr4")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With


        With grProductos.RootTable.Columns("yfumin")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grProductos.RootTable.Columns("UnidMin")
            .Width = 120
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
            .Caption = "Unidad Min."
        End With
        With grProductos.RootTable.Columns("yhprecio")
            .Width = 90
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
            .Caption = "PRECIO"
            .FormatString = "0.00"
        End With
        With grProductos.RootTable.Columns("pcos")
            .Width = 120
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
            .Caption = "Precio Costo"
            .FormatString = "0.00"
        End With
        With grProductos.RootTable.Columns("stock")
            .Width = 90
            .FormatString = "0.00"
            .Visible = True
            .Caption = "STOCK"
        End With

        With grProductos
            .DefaultFilterRowComparison = FilterConditionOperator.Contains
            .FilterMode = FilterMode.Automatic
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges
            .GroupByBoxVisible = False
            'diseño de la grilla
            .VisualStyle = VisualStyle.Office2007
        End With
        _prAplicarCondiccionJanusSinLote()
    End Sub
    Private Sub _prCargarProductosSeleccion(_cliente As String)
        If (cbSucursal.SelectedIndex < 0) Then
            Return
        End If
        Dim dtname As DataTable = L_fnNameLabel()
        Dim dt As New DataTable

        If (G_Lote = True) Then
            If grup2 = " " Then
                dt = L_fnListarProductosFiltradoCompuesto(cbSucursal.Value, _cliente, grup1)  ''1=Almacen
                grup1 = " "
            ElseIf grup1 = " " Then
                dt = L_fnListarProductosFiltradoAccion(cbSucursal.Value, _cliente, grup2)  ''1=Almacen
                grup2 = " "
            End If
            'dt = L_fnListarProductosFiltrado(cbSucursal.Value, _cliente, grup1)  ''1=Almacen
            'Table_Producto = dt.Copy
        Else
            dt = L_fnListarProductosSinLote(cbSucursal.Value, _cliente, CType(grdetalle.DataSource, DataTable))  ''1=Almacen
            'Table_Producto = dt.Copy
        End If



        ''  actualizarSaldoSinLote(dt)
        grProductos.DataSource = dt
        grProductos.RetrieveStructure()
        grProductos.AlternatingColors = True

        '      a.yfnumi ,a.yfcprod ,a.yfcdprod1,a.yfcdprod2 ,a.yfgr1,gr1.ycdes3 as grupo1,a.yfgr2
        ',gr2.ycdes3 as grupo2 ,a.yfgr3,gr3.ycdes3 as grupo3,a.yfgr4 ,gr4 .ycdes3 as grupo4,a.yfumin ,Umin .ycdes3 as UnidMin
        ' ,b.yhprecio 

        With grProductos.RootTable.Columns("yfnumi")
            .Width = 100
            .Caption = "CODIGO"
            .Visible = False

        End With
        With grProductos.RootTable.Columns("yfcprod")
            .Width = 60
            .Caption = "CÓDIGO"
            .Visible = True
        End With

        With grProductos.RootTable.Columns("yfcdprod1")
            .Width = 250
            .Visible = True
            .Caption = "DESCRIPCIÓN"
        End With
        With grProductos.RootTable.Columns("yfcdprod2")
            .Width = 150
            .Visible = False
            .Caption = "Descripcion Corta"
        End With


        With grProductos.RootTable.Columns("yfgr1")
            .Width = 160
            .Visible = False
        End With
        If (dtname.Rows.Count > 0) Then

            With grProductos.RootTable.Columns("grupo1")
                .Width = 120
                .Caption = dtname.Rows(0).Item("Grupo 1").ToString
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = True
            End With
            With grProductos.RootTable.Columns("grupo2")
                .Width = 120
                .Caption = dtname.Rows(0).Item("Grupo 2").ToString
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = True
            End With

            With grProductos.RootTable.Columns("grupo3")
                .Width = 120
                .Caption = dtname.Rows(0).Item("Grupo 3").ToString
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
            With grProductos.RootTable.Columns("grupo4")
                .Width = 120
                .Caption = dtname.Rows(0).Item("Grupo 4").ToString
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
        Else
            With grProductos.RootTable.Columns("grupo1")
                .Width = 120
                .Caption = "Grupo 1"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = True
            End With
            With grProductos.RootTable.Columns("grupo2")
                .Width = 120
                .Caption = "Grupo 2"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = True
            End With
            With grProductos.RootTable.Columns("grupo3")
                .Width = 120
                .Caption = "Grupo 3"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
            With grProductos.RootTable.Columns("grupo4")
                .Width = 120
                .Caption = "Grupo 4"
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
                .Visible = False
            End With
        End If


        With grProductos.RootTable.Columns("yfgr2")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With

        With grProductos.RootTable.Columns("yfgr3")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With

        With grProductos.RootTable.Columns("yfgr4")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With


        With grProductos.RootTable.Columns("yfumin")
            .Width = 50
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
        End With
        With grProductos.RootTable.Columns("UnidMin")
            .Width = 120
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
            .Caption = "Unidad Min."
        End With
        With grProductos.RootTable.Columns("yhprecio")
            .Width = 90
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
            .Caption = "PRECIO"
            .FormatString = "0.00"
        End With
        With grProductos.RootTable.Columns("pcos")
            .Width = 120
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = False
            .Caption = "Precio Costo"
            .FormatString = "0.00"
        End With
        With grProductos.RootTable.Columns("stock")
            .Width = 90
            .FormatString = "0.00"
            .Visible = True
            .Caption = "STOCK"
        End With

        With grProductos
            .DefaultFilterRowComparison = FilterConditionOperator.Contains
            .FilterMode = FilterMode.Automatic
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges
            .GroupByBoxVisible = False
            'diseño de la grilla
            .VisualStyle = VisualStyle.Office2007
        End With
        _prAplicarCondiccionJanusSinLote()
    End Sub
    Public Sub _prAplicarCondiccionJanusSinLote()
        Dim fc As GridEXFormatCondition
        fc = New GridEXFormatCondition(grProductos.RootTable.Columns("stock"), ConditionOperator.Between, -9998 And 0)
        'fc.FormatStyle.FontBold = TriState.True
        fc.FormatStyle.ForeColor = Color.Red    'Color.Tan
        grProductos.RootTable.FormatConditions.Add(fc)
        Dim fr As GridEXFormatCondition
        fr = New GridEXFormatCondition(grProductos.RootTable.Columns("stock"), ConditionOperator.Equal, -9999)
        fr.FormatStyle.ForeColor = Color.BlueViolet
        grProductos.RootTable.FormatConditions.Add(fr)
    End Sub


    Public Sub actualizarSaldo(ByRef dt As DataTable, CodProducto As Integer)
        'b.yfcdprod1 ,a.iclot ,a.icfven  ,a.iccven 

        '      a.tbnumi ,a.tbtv1numi ,a.tbty5prod ,b.yfcdprod1 as producto,a.tbest ,a.tbcmin ,a.tbumin ,Umin .ycdes3 as unidad,a.tbpbas ,a.tbptot ,a.tbobs ,
        'a.tbpcos,a.tblote ,a.tbfechaVenc , a.tbptot2, a.tbfact ,a.tbhact ,a.tbuact,1 as estado,Cast(null as Image) as img,
        'Cast (0 as decimal (18,2)) as stock
        Dim _detalle As DataTable = CType(grdetalle.DataSource, DataTable)

        For i As Integer = 0 To dt.Rows.Count - 1 Step 1
            Dim lote As String = dt.Rows(i).Item("iclot")
            Dim FechaVenc As Date = dt.Rows(i).Item("icfven")
            Dim sum As Integer = 0
            For j As Integer = 0 To _detalle.Rows.Count - 1
                Dim estado As Integer = _detalle.Rows(j).Item("estado")
                If (estado = 0) Then
                    If (lote = _detalle.Rows(j).Item("tblote") And
                        FechaVenc = _detalle.Rows(j).Item("tbfechaVenc") And CodProducto = _detalle.Rows(j).Item("tbty5prod")) Then
                        sum = sum + _detalle.Rows(j).Item("tbcmin")
                    End If
                End If
            Next
            dt.Rows(i).Item("iccven") = dt.Rows(i).Item("iccven") - sum
        Next

    End Sub

    Private Sub _prCargarLotesDeProductos(CodProducto As Integer, nameProducto As String)

    End Sub
    Public Sub _prAplicarCondiccionJanusLote()
        Dim fc As GridEXFormatCondition
        fc = New GridEXFormatCondition(grProductos.RootTable.Columns("iccven"), ConditionOperator.Equal, 0)
        fc.FormatStyle.BackColor = Color.Gold
        fc.FormatStyle.FontBold = TriState.True
        fc.FormatStyle.ForeColor = Color.White
        grProductos.RootTable.FormatConditions.Add(fc)

        Dim fc2 As GridEXFormatCondition
        fc2 = New GridEXFormatCondition(grProductos.RootTable.Columns("icfven"), ConditionOperator.LessThanOrEqualTo, Now.Date)
        fc2.FormatStyle.BackColor = Color.Red
        fc2.FormatStyle.FontBold = TriState.True
        fc2.FormatStyle.ForeColor = Color.White
        grProductos.RootTable.FormatConditions.Add(fc2)
    End Sub
    Private Sub _prAddDetalleVenta()
        '   a.tbnumi ,a.tbtv1numi ,a.tbty5prod ,b.yfcdprod1 as producto,a.tbest ,a.tbcmin ,a.tbumin ,Umin .ycdes3 as unidad,a.tbpbas ,a.tbptot ,a.tbobs ,
        'a.tbpcos,a.tblote ,a.tbfechaVenc , a.tbptot2, a.tbfact ,a.tbhact ,a.tbuact,1 as estado,Cast(null as Image) as img
        Dim Bin As New MemoryStream
        Dim img As New Bitmap(My.Resources.delete, 28, 28)
        img.Save(Bin, Imaging.ImageFormat.Png)
        CType(grdetalle.DataSource, DataTable).Rows.Add(_fnSiguienteNumi() + 1, 0, 0, "", 0, "", 0, 0, 0, "", 0, 0, 0, 0, 0, "", 0, "20170101", CDate("2017/01/01"), 0, Now.Date, "", "", 0, Bin.GetBuffer, 0)
    End Sub

    Public Function _fnSiguienteNumi()
        Dim dt As DataTable = CType(grdetalle.DataSource, DataTable)
        Dim rows() As DataRow = dt.Select("tbnumi=MAX(tbnumi)")
        If (rows.Count > 0) Then
            Return rows(rows.Count - 1).Item("tbnumi")
        End If
        Return 1
    End Function
    Public Function _fnAccesible()
        Return tbFechaVenta.IsInputReadOnly = False
    End Function
    Private Sub _HabilitarProductos()
        GPanelProductos.Height = 530
        GPanelProductos.Visible = True
        'PanelTotal.Visible = False
        'PanelInferior.Visible = False
        _prCargarProductos(Str(_CodCliente))
        grProductos.Focus()
        grProductos.MoveTo(grProductos.FilterRow)
        grProductos.Col = 2
    End Sub
    Private Sub _HabilitarProductosSeleccion()
        GPanelProductos.Visible = True
        PanelTotal.Visible = False
        PanelInferior.Visible = False
        'prod = grProductos.GetValue("yfcdprod1")
        'grup1 = grProductos.GetValue("grupo1")
        'grup2 = grProductos.GetValue("grupo2")
        _prCargarProductosSeleccion(_CodCliente)
        grProductos.Focus()
        grProductos.MoveTo(grProductos.FilterRow)
        grProductos.Col = 2
    End Sub
    Private Sub _HabilitarFocoDetalle(fila As Integer)
        _prCargarProductos(Str(_CodCliente))
        grdetalle.Focus()
        grdetalle.Row = fila
        grdetalle.Col = 2
    End Sub
    Private Sub _DesHabilitarProductos()
        GPanelProductos.Visible = False
        PanelTotal.Visible = True
        PanelInferior.Visible = True

        grdetalle.Select()
        grdetalle.Col = 5
        grdetalle.Row = grdetalle.RowCount - 1

    End Sub
    Public Sub _fnObtenerFilaDetalle(ByRef pos As Integer, numi As Integer)
        For i As Integer = 0 To CType(grdetalle.DataSource, DataTable).Rows.Count - 1 Step 1
            Dim _numi As Integer = CType(grdetalle.DataSource, DataTable).Rows(i).Item("tbnumi")
            If (_numi = numi) Then
                pos = i
                Return
            End If
        Next

    End Sub

    Public Sub _fnObtenerFilaDetalleProducto(ByRef pos As Integer, numi As Integer)
        For i As Integer = 0 To CType(grProductos.DataSource, DataTable).Rows.Count - 1 Step 1
            Dim _numi As Integer = CType(grProductos.DataSource, DataTable).Rows(i).Item("yfnumi")
            If (_numi = numi) Then
                pos = i
                Return
            End If
        Next

    End Sub

    Public Function _fnExisteProducto(idprod As Integer) As Boolean
        For i As Integer = 0 To CType(grdetalle.DataSource, DataTable).Rows.Count - 1 Step 1
            Dim _idprod As Integer = CType(grdetalle.DataSource, DataTable).Rows(i).Item("tbty5prod")
            Dim estado As Integer = CType(grdetalle.DataSource, DataTable).Rows(i).Item("estado")
            If (_idprod = idprod And estado >= 0) Then

                Return True
            End If
        Next
        Return False
    End Function

    Public Function _fnExisteProductoConLote(idprod As Integer, lote As String, fechaVenci As Date) As Boolean
        For i As Integer = 0 To CType(grdetalle.DataSource, DataTable).Rows.Count - 1 Step 1
            Dim _idprod As Integer = CType(grdetalle.DataSource, DataTable).Rows(i).Item("tbty5prod")
            Dim estado As Integer = CType(grdetalle.DataSource, DataTable).Rows(i).Item("estado")
            '          a.tbnumi ,a.tbtv1numi ,a.tbty5prod ,b.yfcdprod1 as producto,a.tbest ,a.tbcmin ,a.tbumin ,Umin .ycdes3 as unidad,a.tbpbas ,a.tbptot ,a.tbobs ,
            'a.tbpcos,a.tblote ,a.tbfechaVenc , a.tbptot2, a.tbfact ,a.tbhact ,a.tbuact,1 as estado,Cast(null as Image) as img,
            'Cast (0 as decimal (18,2)) as stock
            Dim _LoteDetalle As String = CType(grdetalle.DataSource, DataTable).Rows(i).Item("tblote")
            Dim _FechaVencDetalle As Date = CType(grdetalle.DataSource, DataTable).Rows(i).Item("tbfechaVenc")
            If (_idprod = idprod And estado >= 0 And lote = _LoteDetalle And fechaVenci = _FechaVencDetalle) Then

                Return True
            End If
        Next
        Return False
    End Function
    Public Sub P_PonerTotal(rowIndex As Integer)
        If (rowIndex < grdetalle.RowCount) Then

            Dim lin As Integer = grdetalle.GetValue("tbnumi")
            Dim pos As Integer = -1
            _fnObtenerFilaDetalle(pos, lin)
            Dim cant As Double = grdetalle.GetValue("tbcmin")
            Dim uni As Double = grdetalle.GetValue("tbpbas")
            Dim cos As Double = grdetalle.GetValue("tbpcos")
            Dim MontoDesc As Double = grdetalle.GetValue("tbdesc")
            Dim dt As DataTable = CType(grdetalle.DataSource, DataTable)
            If (pos >= 0) Then
                Dim TotalUnitario As Double = cant * uni
                Dim TotalCosto As Double = cant * cos
                'grDetalle.SetValue("lcmdes", montodesc)

                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot") = TotalUnitario
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbtotdesc") = TotalUnitario - MontoDesc
                grdetalle.SetValue("tbptot", TotalUnitario)
                grdetalle.SetValue("tbtotdesc", TotalUnitario - MontoDesc)

                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot2") = TotalCosto
                grdetalle.SetValue("tbptot2", TotalCosto)

                Dim estado As Integer = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("estado")
                If (estado = 1) Then
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("estado") = 2
                End If
            End If
            _prCalcularPrecioTotal()
        End If



    End Sub
    Public Sub _prCalcularPrecioTotal()
        Dim montodesc As Double = tbMdesc.Value
        Dim pordesc As Double = ((montodesc * 100) / grdetalle.GetTotal(grdetalle.RootTable.Columns("tbtotdesc"), AggregateFunction.Sum))
        tbPdesc.Value = pordesc
        tbSubTotal.Value = grdetalle.GetTotal(grdetalle.RootTable.Columns("tbtotdesc"), AggregateFunction.Sum)
        'tbIce.Value = grdetalle.GetTotal(grdetalle.RootTable.Columns("tbptot2"), AggregateFunction.Sum) * (gi_ICE / 100)
        If (gb_FacturaIncluirICE = True) Then
            tbtotal.Value = grdetalle.GetTotal(grdetalle.RootTable.Columns("tbtotdesc"), AggregateFunction.Sum) - montodesc + tbIce.Value
        Else
            tbtotal.Value = grdetalle.GetTotal(grdetalle.RootTable.Columns("tbtotdesc"), AggregateFunction.Sum) - montodesc
        End If

    End Sub
    Public Sub _prEliminarFila()
        If (grdetalle.Row >= 0) Then
            If (grdetalle.RowCount >= 2) Then
                Dim estado As Integer = grdetalle.GetValue("estado")
                Dim pos As Integer = -1
                Dim lin As Integer = grdetalle.GetValue("tbnumi")
                _fnObtenerFilaDetalle(pos, lin)
                If (estado = 0) Then
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("estado") = -2

                End If
                If (estado = 1) Then
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("estado") = -1
                End If
                grdetalle.RootTable.ApplyFilter(New Janus.Windows.GridEX.GridEXFilterCondition(grdetalle.RootTable.Columns("estado"), Janus.Windows.GridEX.ConditionOperator.GreaterThanOrEqualTo, 0))
                _prCalcularPrecioTotal()
                grdetalle.Select()
                grdetalle.Col = 5
                grdetalle.Row = grdetalle.RowCount - 1
            End If
        End If
        grdetalle.Refetch()
        grdetalle.Refresh()

    End Sub
    Public Function _ValidarCampos() As Boolean
        If (_CodCliente <= 0) Then
            Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
            ToastNotification.Show(Me, "Por Favor Seleccione un Cliente con Ctrl+Enter".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            tbCliente.Focus()
            Return False

        End If
        If (_CodEmpleado <= 0) Then
            Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
            ToastNotification.Show(Me, "Por Favor Seleccione un Vendedor con Ctrl+Enter".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            tbVendedor.Focus()
            Return False
        End If
        If (cbSucursal.SelectedIndex < 0) Then
            Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
            ToastNotification.Show(Me, "Por Favor Seleccione una Sucursal".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            cbSucursal.Focus()
            Return False
        End If
        If (grdetalle.RowCount = 1) Then
            grdetalle.Row = grdetalle.RowCount - 1
            If (grdetalle.GetValue("tbty5prod") = 0) Then
                Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
                ToastNotification.Show(Me, "Por Favor Seleccione  un detalle de producto".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
                Return False
            End If

        End If
        Return True
    End Function

    Public Sub _GuardarNuevo()

    End Sub
    Public Sub _prImiprimirNotaVenta(numi As String)
        Dim ef = New Efecto

        ef.tipo = 2
        ef.Context = "MENSAJE PRINCIPAL".ToUpper
        ef.Header = "¿desea imprimir la nota de venta?".ToUpper
        ef.ShowDialog()
        Dim bandera As Boolean = False
        bandera = ef.band
        If (bandera = True) Then
            P_GenerarReporte(numi)
        End If
    End Sub
    Public Sub _prImiprimirFacturaPreimpresa(numi As String)
        Dim ef = New Efecto

        ef.tipo = 2
        ef.Context = "MENSAJE PRINCIPAL".ToUpper
        ef.Header = "¿desea imprimir la factura Preimpresa?".ToUpper
        ef.ShowDialog()
        Dim bandera As Boolean = False
        bandera = ef.band
        If (bandera = True) Then
            P_GenerarReporteFactura(numi)
        End If
    End Sub
    Private Sub _prGuardarModificado()

    End Sub
    Private Sub _prSalir()
        If btnGrabar.Enabled = True Then
            _prInhabiliitar()
            If grVentas.RowCount > 0 Then
                _prMostrarRegistro(0)
            End If
        Else
            _tab.Close()
            _modulo.Select()
        End If
    End Sub
    Public Sub _prCargarIconELiminar()
        For i As Integer = 0 To CType(grdetalle.DataSource, DataTable).Rows.Count - 1 Step 1
            Dim Bin As New MemoryStream
            Dim img As New Bitmap(My.Resources.delete, 28, 28)
            img.Save(Bin, Imaging.ImageFormat.Png)
            CType(grdetalle.DataSource, DataTable).Rows(i).Item("img") = Bin.GetBuffer
            grdetalle.RootTable.Columns("img").Visible = True
        Next

    End Sub
    Public Sub _PrimerRegistro()
        Dim _MPos As Integer
        If grVentas.RowCount > 0 Then
            _MPos = 0
            ''   _prMostrarRegistro(_MPos)
            grVentas.Row = _MPos
        End If
    End Sub
    Public Sub InsertarProductosSinLote()
        Dim pos As Integer = -1
        grdetalle.Row = grdetalle.RowCount - 1
        _fnObtenerFilaDetalle(pos, grdetalle.GetValue("tbnumi"))
        Dim existe As Boolean = _fnExisteProducto(grProductos.GetValue("yfnumi"))
        If ((pos >= 0) And (Not existe)) Then
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbty5prod") = grProductos.GetValue("yfnumi")
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("codigo") = grProductos.GetValue("yfcprod")
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("yfcbarra") = grProductos.GetValue("yfcbarra")
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("producto") = grProductos.GetValue("yfcdprod1")
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbumin") = grProductos.GetValue("yfumin")
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("unidad") = grProductos.GetValue("UnidMin")
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpbas") = grProductos.GetValue("yhprecio")
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot") = grProductos.GetValue("yhprecio")
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbtotdesc") = grProductos.GetValue("yhprecio")
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbcmin") = 1
            If (gb_FacturaIncluirICE) Then
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpcos") = grProductos.GetValue("pcos")
            Else
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpcos") = 0
            End If
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot2") = grProductos.GetValue("pcos")

            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("stock") = grProductos.GetValue("stock")
            _prCalcularPrecioTotal()
            _DesHabilitarProductos()
        Else
            If (existe) Then
                Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
                ToastNotification.Show(Me, "El producto ya existe en el detalle".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            End If
        End If
    End Sub
    Public Sub InsertarProductosConLote()
        Dim pos As Integer = -1
        grdetalle.Row = grdetalle.RowCount - 1
        _fnObtenerFilaDetalleProducto(pos, grProductos.GetValue("yfnumi"))
        Dim posProducto As Integer = grProductos.Row
        FilaSelectLote = CType(grProductos.DataSource, DataTable).Rows(pos)


        If (grProductos.GetValue("stock") > 0) Then
            _prCargarLotesDeProductos(grProductos.GetValue("yfnumi"), grProductos.GetValue("yfcdprod1"))
        Else
            Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
            ToastNotification.Show(Me, "El Producto: ".ToUpper + grProductos.GetValue("yfcdprod1") + " NO CUENTA CON STOCK DISPONIBLE", img, 5000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            FilaSelectLote = Nothing
        End If

    End Sub

    Private Function P_fnGrabarFacturarTFV001(numi As String) As Boolean
        Dim a As Double = CDbl(tbtotal.Value + tbMdesc.Value)
        'Dim b As Double = CDbl(IIf(IsDBNull(tbIce.Value), 0, tbIce.Value)) 'Ya esta calculado el 55% del ICE
        Dim b As Double = CDbl("0")
        Dim c As Double = CDbl("0")
        Dim d As Double = CDbl("0")
        Dim e As Double = a - b - c - d
        Dim f As Double = CDbl(tbMdesc.Value)
        Dim g As Double = e - f
        Dim h As Double = g * (gi_IVA / 100)

        Dim res As Boolean = False
        'Grabado de Cabesera Factura
        L_Grabar_Factura(numi,
                        dtiFechaFactura.Value.ToString("yyyy/MM/dd"),
                        IIf(Val(tbNroFactura.Text) = 0, "0", tbNroFactura.Text),
                        IIf(Val(tbNroAutoriz.Text) = 0, "0", tbNroAutoriz.Text),
                        "1",
                        TbNit.Text.Trim,
                        "0",
                        TbNombre1.Text,
                        "",
                        CStr(Format(a, "####0.00")),
                        CStr(Format(b, "####0.00")),
                        CStr(Format(c, "####0.00")),
                        CStr(Format(d, "####0.00")),
                        CStr(Format(e, "####0.00")),
                        CStr(Format(f, "####0.00")),
                        CStr(Format(g, "####0.00")),
                        CStr(Format(h, "####0.00")),
                        "",
                        Now.Date.ToString("yyyy/MM/dd"),
                        "''",
                        cbSucursal.Value,
                        numi)

        'Grabado de Detalle de Factura
        grProductos.Update()

        'Dim s As String = ""
        For Each fil As GridEXRow In grdetalle.GetRows
            If (Not fil.Cells("tbcmin").Value.ToString.Trim.Equals("") And
                Not fil.Cells("tbty5prod").Value.ToString.Trim.Equals("0")) Then
                's = fil.Cells("codP").Value
                's = fil.Cells("des").Value
                's = fil.Cells("can").Value
                's = fil.Cells("imp").Value
                L_Grabar_Factura_Detalle(numi.ToString,
                                        fil.Cells("tbty5prod").Value.ToString.Trim,
                                        fil.Cells("producto").Value.ToString.Trim,
                                        fil.Cells("tbcmin").Value.ToString.Trim,
                                        fil.Cells("tbpbas").Value.ToString.Trim,
                                        numi)
                res = True
            End If
        Next
        Return res
    End Function

    Private Function P_fnValidarFactura() As Boolean
        Return True
    End Function

    Public Function P_fnImageToByteArray(ByVal imageIn As Image) As Byte()
        Dim ms As New System.IO.MemoryStream()
        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
        Return ms.ToArray()
    End Function

    Private Function P_fnValidarFacturaVigente() As Boolean
        Dim est As String = L_fnObtenerDatoTabla("TFV001", "fvaest", "fvanumi=" + tbCodigo.Text.Trim)
        Return (est.Equals("True") Or est = String.Empty)
    End Function

    Private Sub P_GenerarReporte(numi As String)

    End Sub
    Private Sub P_prImprimirFacturar(numi As String, impFactura As Boolean, grabarPDF As Boolean)
        Dim _Fecha, _FechaAl As Date
        Dim _Ds, _Ds1, _Ds2, _Ds3 As New DataSet
        Dim _Autorizacion, _Nit, _Fechainv, _Total, _Key, _Cod_Control, _Hora,
            _Literal, _TotalDecimal, _TotalDecimal2 As String
        Dim I, _NumFac, _numidosif, _TotalCC As Integer
        Dim ice, _Desc, _TotalLi As Decimal
        Dim _VistaPrevia As Integer = 0

        _Desc = CDbl(0)
        If Not IsNothing(P_Global.Visualizador) Then
            P_Global.Visualizador.Close()
        End If

        _Fecha = Now.Date.ToString("dd/MM/yyyy")
        _Hora = Now.Hour.ToString + ":" + Now.Minute.ToString
        _Ds1 = L_Dosificacion("1", "1", _Fecha)

        _Ds = L_Reporte_Factura(numi, numi)
        _Autorizacion = _Ds1.Tables(0).Rows(0).Item("yeautoriz").ToString
        _NumFac = CInt(_Ds1.Tables(0).Rows(0).Item("yenunf")) + 1
        _Nit = _Ds.Tables(0).Rows(0).Item("fvanitcli").ToString
        _Fechainv = Microsoft.VisualBasic.Right(_Fecha.ToShortDateString, 4) +
                    Microsoft.VisualBasic.Right(Microsoft.VisualBasic.Left(_Fecha.ToShortDateString, 5), 2) +
                    Microsoft.VisualBasic.Left(_Fecha.ToShortDateString, 2)
        _Total = _Ds.Tables(0).Rows(0).Item("fvatotal").ToString
        ice = _Ds.Tables(0).Rows(0).Item("fvaimpsi")
        _numidosif = _Ds1.Tables(0).Rows(0).Item("yenumi").ToString
        _Key = _Ds1.Tables(0).Rows(0).Item("yekey")
        _FechaAl = _Ds1.Tables(0).Rows(0).Item("yefal")

        Dim maxNFac As Integer = L_fnObtenerMaxIdTabla("TFV001", "fvanfac", "fvaautoriz = " + _Autorizacion)
        _NumFac = maxNFac + 1

        _TotalCC = Math.Round(CDbl(_Total), MidpointRounding.AwayFromZero)
        _Cod_Control = ControlCode.generateControlCode(_Autorizacion, _NumFac, _Nit, _Fechainv, CStr(_TotalCC), _Key)

        'Literal 
        _TotalLi = _Ds.Tables(0).Rows(0).Item("fvastot") - _Ds.Tables(0).Rows(0).Item("fvadesc")
        _TotalDecimal = _TotalLi - Math.Truncate(_TotalLi)
        _TotalDecimal2 = CDbl(_TotalDecimal) * 100

        'Dim li As String = Facturacion.ConvertirLiteral.A_fnConvertirLiteral(CDbl(_Total) - CDbl(_TotalDecimal)) + " con " + IIf(_TotalDecimal2.Equals("0"), "00", _TotalDecimal2) + "/100 Bolivianos"
        _Literal = Facturacion.ConvertirLiteral.A_fnConvertirLiteral(CDbl(_TotalLi) - CDbl(_TotalDecimal)) + " con " + IIf(_TotalDecimal2.Equals("0"), "00", _TotalDecimal2) + "/100 Bolivianos"
        _Ds2 = L_Reporte_Factura_Cia("1")
        QrFactura.Text = _Ds2.Tables(0).Rows(0).Item("scnit").ToString + "|" + Str(_NumFac).Trim + "|" + _Autorizacion + "|" + _Fecha + "|" + _Total + "|" + _TotalLi.ToString + "|" + _Cod_Control + "|" + _Nit + "|" + ice.ToString + "|0|0|" + Str(_Desc).Trim

        'L_Modificar_Factura("fvanumi = " + CStr(numi),
        '                    "",
        '                    CStr(_NumFac),
        '                    CStr(_Autorizacion),
        '                    "",
        '                    "",
        '                    "",
        '                    "",
        '                    "",
        '                    "",
        '                    "",
        '                    "",
        '                    "",
        '                    "",
        '                    "",
        '                    "",
        '                    "",
        '                    _Cod_Control,
        '                    _FechaAl.ToString("yyyy/MM/dd"),
        '                    "",
        '                    "",
        '                    CStr(numi))


        'updateTO001C(numi, Str(_NumFac))
        _Ds = L_Reporte_Factura(numi, numi)

        _Ds3 = L_ObtenerRutaImpresora("1") ' Datos de Impresion de Facturación

        For I = 0 To _Ds.Tables(0).Rows.Count - 1
            _Ds.Tables(0).Rows(I).Item("fvaimgqr") = P_fnImageToByteArray(QrFactura.Image)
        Next
        P_Global.Visualizador = New Visualizador
        Dim objrep As New R_FacturaPreImpresa1
        Dim dia, mes, ano As Integer
        Dim Fecliteral, mesl As String
        Fecliteral = _Ds.Tables(0).Rows(0).Item("fvafec").ToString
        dia = Microsoft.VisualBasic.Left(Fecliteral, 2)
        mes = Microsoft.VisualBasic.Mid(Fecliteral, 4, 2)
        ano = Microsoft.VisualBasic.Mid(Fecliteral, 7, 4)
        If mes = 1 Then
            mesl = "Enero"
        End If
        If mes = 2 Then
            mesl = "Febrero"
        End If
        If mes = 3 Then
            mesl = "Marzo"
        End If
        If mes = 4 Then
            mesl = "Abril"
        End If
        If mes = 5 Then
            mesl = "Mayo"
        End If
        If mes = 6 Then
            mesl = "Junio"
        End If
        If mes = 7 Then
            mesl = "Julio"
        End If
        If mes = 8 Then
            mesl = "Agosto"
        End If
        If mes = 9 Then
            mesl = "Septiembre"
        End If
        If mes = 10 Then
            mesl = "Octubre"
        End If
        If mes = 11 Then
            mesl = "Noviembre"
        End If
        If mes = 12 Then
            mesl = "Diciembre"
        End If

        Fecliteral = "Cochabamba, " + dia.ToString + " de " + mesl + " del " + ano.ToString
        objrep.SetDataSource(_Ds.Tables(0))
        objrep.SetParameterValue("Literal", _Literal)
        objrep.SetParameterValue("Fechali", Fecliteral)
        objrep.SetParameterValue("Ley", _Ds1.Tables(0).Rows(0).Item("yenota").ToString())
        'objrep.PrintOptions.PrinterName = "L4150 Series(Red) (Copiar 1)"
        objrep.PrintOptions.PrinterName = _Ds3.Tables(0).Rows(0).Item("cbrut").ToString
        objrep.PrintToPrinter(1, False, 1, 1)

    End Sub
    Private Sub ponerDescripcionProducto(ByRef dt As DataTable)

    End Sub

    Private Sub P_GenerarReporteFactura(numi As String)

    End Sub

    Sub _prCargarProductoDeLaProforma(numiProforma As Integer)

    End Sub
    Public Sub _prPedirLotesProducto(ByRef lote As String, ByRef FechaVenc As Date, ByRef iccven As Double, CodProducto As Integer, nameProducto As String, cant As Integer)

    End Sub

#End Region

#Region "Eventos Formulario"
    Private Sub F0_Ventas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _IniciarTodo()
    End Sub
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _Limpiar()
        _prhabilitar()

        btnNuevo.Enabled = False
        btnModificar.Enabled = False
        btnEliminar.Enabled = False
        btnGrabar.Enabled = True
        PanelNavegacion.Enabled = False

        'btnNuevo.Enabled = False
        'btnModificar.Enabled = False
        'btnEliminar.Enabled = False
        'GPanelProductos.Visible = False
        '_prhabilitar()

        '_Limpiar()
    End Sub
    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        _prSalir()

    End Sub



    Private Sub tbCliente_KeyDown(sender As Object, e As KeyEventArgs) Handles tbCliente.KeyDown
        'If (_fnAccesible()) Then
        '    If e.KeyData = Keys.Control + Keys.Enter Then

        '        Dim dt As DataTable

        '        dt = L_fnListarClientes()
        '        '              a.ydnumi, a.ydcod, a.yddesc, a.yddctnum, a.yddirec
        '        ',a.ydtelf1 ,a.ydfnac 

        '        Dim listEstCeldas As New List(Of Modelo.Celda)
        '        listEstCeldas.Add(New Modelo.Celda("ydnumi,", True, "ID", 50))
        '        listEstCeldas.Add(New Modelo.Celda("ydcod", False, "ID", 50))
        '        listEstCeldas.Add(New Modelo.Celda("ydrazonsocial", True, "RAZON SOCIAL", 180))
        '        listEstCeldas.Add(New Modelo.Celda("yddesc", True, "NOMBRE", 280))
        '        listEstCeldas.Add(New Modelo.Celda("yddctnum", True, "N. Documento".ToUpper, 150))
        '        listEstCeldas.Add(New Modelo.Celda("yddirec", True, "DIRECCION", 220))
        '        listEstCeldas.Add(New Modelo.Celda("ydtelf1", True, "Telefono".ToUpper, 200))
        '        listEstCeldas.Add(New Modelo.Celda("ydfnac", True, "F.Nacimiento".ToUpper, 150, "MM/dd,YYYY"))
        '        listEstCeldas.Add(New Modelo.Celda("ydnumivend,", False, "ID", 50))
        '        listEstCeldas.Add(New Modelo.Celda("vendedor,", False, "ID", 50))
        '        listEstCeldas.Add(New Modelo.Celda("yddias", False, "CRED", 50))
        '        Dim ef = New Efecto
        '        ef.tipo = 3
        '        ef.dt = dt
        '        ef.SeleclCol = 2
        '        ef.listEstCeldas = listEstCeldas
        '        ef.alto = 50
        '        ef.ancho = 350
        '        ef.Context = "Seleccione Cliente".ToUpper
        '        ef.ShowDialog()
        '        Dim bandera As Boolean = False
        '        bandera = ef.band
        '        If (bandera = True) Then
        '            Dim Row As Janus.Windows.GridEX.GridEXRow = ef.Row

        '            _CodCliente = Row.Cells("ydnumi").Value
        '            tbCliente.Text = Row.Cells("ydrazonsocial").Value
        '            _dias = Row.Cells("yddias").Value

        '            Dim numiVendedor As Integer = IIf(IsDBNull(Row.Cells("ydnumivend").Value), 0, Row.Cells("ydnumivend").Value)
        '            If (numiVendedor > 0) Then
        '                tbVendedor.Text = Row.Cells("vendedor").Value
        '                _CodEmpleado = Row.Cells("ydnumivend").Value

        '                grdetalle.Select()
        '                Table_Producto = Nothing
        '            Else
        '                tbVendedor.Clear()
        '                _CodEmpleado = 0
        '                tbVendedor.Focus()
        '                Table_Producto = Nothing

        '            End If
        '        End If

        '    End If

        'End If
    End Sub
    Private Sub tbVendedor_KeyDown(sender As Object, e As KeyEventArgs) Handles tbVendedor.KeyDown
        'If (_fnAccesible()) Then
        '    If e.KeyData = Keys.Control + Keys.Enter Then

        '        Dim dt As DataTable

        '        dt = L_fnListarEmpleado()
        '        '              a.ydnumi, a.ydcod, a.yddesc, a.yddctnum, a.yddirec
        '        ',a.ydtelf1 ,a.ydfnac 

        '        Dim listEstCeldas As New List(Of Modelo.Celda)
        '        listEstCeldas.Add(New Modelo.Celda("ydnumi,", True, "ID", 50))
        '        listEstCeldas.Add(New Modelo.Celda("ydcod", False, "ID", 50))
        '        listEstCeldas.Add(New Modelo.Celda("yddesc", True, "NOMBRE", 280))
        '        listEstCeldas.Add(New Modelo.Celda("yddctnum", True, "N. Documento".ToUpper, 150))
        '        listEstCeldas.Add(New Modelo.Celda("yddirec", True, "DIRECCION", 220))
        '        listEstCeldas.Add(New Modelo.Celda("ydtelf1", True, "Telefono".ToUpper, 200))
        '        listEstCeldas.Add(New Modelo.Celda("ydfnac", True, "F.Nacimiento".ToUpper, 150, "MM/dd,YYYY"))
        '        Dim ef = New Efecto
        '        ef.tipo = 3
        '        ef.dt = dt
        '        ef.SeleclCol = 1
        '        ef.listEstCeldas = listEstCeldas
        '        ef.alto = 50
        '        ef.ancho = 350
        '        ef.Context = "Seleccione Vendedor".ToUpper
        '        ef.ShowDialog()
        '        Dim bandera As Boolean = False
        '        bandera = ef.band
        '        If (bandera = True) Then
        '            Dim Row As Janus.Windows.GridEX.GridEXRow = ef.Row
        '            If (IsNothing(Row)) Then
        '                tbVendedor.Focus()
        '                Return

        '            End If
        '            _CodEmpleado = Row.Cells("ydnumi").Value
        '            tbVendedor.Text = Row.Cells("yddesc").Value
        '            grdetalle.Select()

        '        End If

        '    End If

        'End If
    End Sub
    Private Sub swTipoVenta_ValueChanged(sender As Object, e As EventArgs) Handles swTipoVenta.ValueChanged
        If (swTipoVenta.Value = False) Then
            lbCredito.Visible = True
            tbFechaVenc.Visible = True
            tbFechaVenc.Value = DateAdd(DateInterval.Day, _dias, Now.Date)
        Else
            lbCredito.Visible = False
            tbFechaVenc.Visible = False
        End If
    End Sub

    Private Sub grdetalle_EditingCell(sender As Object, e As EditingCellEventArgs) Handles grdetalle.EditingCell
        If (_fnAccesible()) Then
            'Habilitar solo las columnas de Precio, %, Monto y Observación
            'If (e.Column.Index = grdetalle.RootTable.Columns("yfcbarra").Index Or
            If (e.Column.Index = grdetalle.RootTable.Columns("yfcbarra").Index Or
                e.Column.Index = grdetalle.RootTable.Columns("tbcmin").Index Or
                e.Column.Index = grdetalle.RootTable.Columns("tbporc").Index Or
                e.Column.Index = grdetalle.RootTable.Columns("tbpbas").Index Or
                e.Column.Index = grdetalle.RootTable.Columns("tbdesc").Index) Then
                e.Cancel = False
            Else
                e.Cancel = True
            End If
        Else
            e.Cancel = True
        End If

    End Sub

    Private Sub grdetalle_Enter(sender As Object, e As EventArgs) Handles grdetalle.Enter

        If (_fnAccesible()) Then
            If (_CodCliente <= 0) Then
                ToastNotification.Show(Me, "           Antes de Continuar Por favor Seleccione un Cliente!!             ", My.Resources.WARNING, 4000, eToastGlowColor.Red, eToastPosition.TopCenter)
                tbCliente.Focus()

                Return
            End If
            If (_CodEmpleado <= 0) Then


                ToastNotification.Show(Me, "           Antes de Continuar Por favor Seleccione un Vendedor!!             ", My.Resources.WARNING, 4000, eToastGlowColor.Red, eToastPosition.TopCenter)
                tbVendedor.Focus()
                Return

            End If

            grdetalle.Select()
            If _codeBar = 1 Then
                'If gb_CodigoBarra Then
                '    grdetalle.Col = 3
                '    grdetalle.Row = 0
                'Else
                '    grdetalle.Col = 5
                '    grdetalle.Row = 0
                'End If
            End If
        End If


    End Sub
    Private Sub grdetalle_KeyDown(sender As Object, e As KeyEventArgs) Handles grdetalle.KeyDown
        If (Not _fnAccesible()) Then
            Return
        End If
        If (e.KeyData = Keys.Enter) Then
            Dim f, c As Integer
            c = grdetalle.Col
            f = grdetalle.Row

            If (grdetalle.Col = grdetalle.RootTable.Columns("tbcmin").Index) Then
                If (grdetalle.GetValue("producto") <> String.Empty) Then
                    _prAddDetalleVenta()
                    _HabilitarProductos()
                Else
                    ToastNotification.Show(Me, "Seleccione un Producto Por Favor", My.Resources.WARNING, 3000, eToastGlowColor.Red, eToastPosition.TopCenter)
                End If

            End If
            If (grdetalle.Col = grdetalle.RootTable.Columns("producto").Index) Then
                If (grdetalle.GetValue("producto") <> String.Empty) Then
                    _prAddDetalleVenta()
                    _HabilitarProductos()
                Else
                    ToastNotification.Show(Me, "Seleccione un Producto Por Favor", My.Resources.WARNING, 3000, eToastGlowColor.Red, eToastPosition.TopCenter)
                End If

            End If
            If (grdetalle.Col = grdetalle.RootTable.Columns("yfcbarra").Index) Then
                If (grdetalle.GetValue("yfcbarra").ToString().Trim() <> String.Empty) Then
                    cargarProductos()
                    If (grdetalle.Row = grdetalle.RowCount - 1) Then
                        If (existeProducto(grdetalle.GetValue("yfcbarra").ToString)) Then
                            If (Not verificarExistenciaUnica(grdetalle.GetValue("yfcbarra").ToString)) Then
                                ponerProducto(grdetalle.GetValue("yfcbarra").ToString)
                                _prAddDetalleVenta()
                            Else
                                sumarCantidad(grdetalle.GetValue("yfcbarra").ToString)
                            End If
                        Else
                            grdetalle.DataChanged = False
                            ToastNotification.Show(Me, "El código de barra del producto no existe", My.Resources.WARNING, 3000, eToastGlowColor.Red, eToastPosition.TopCenter)
                        End If
                    Else
                        grdetalle.DataChanged = False
                        grdetalle.Row = grdetalle.RowCount - 1
                        grdetalle.Col = grdetalle.RootTable.Columns("yfcbarra").Index
                        ToastNotification.Show(Me, "El cursor debe situarse en la ultima fila", My.Resources.WARNING, 3000, eToastGlowColor.Red, eToastPosition.TopCenter)
                    End If
                Else
                    ToastNotification.Show(Me, "El código de barra no puede quedar vacio", My.Resources.WARNING, 3000, eToastGlowColor.Red, eToastPosition.TopCenter)
                End If

            End If
            'opcion para cargar la grilla con el codigo de barra
            'If (grdetalle.Col = grdetalle.RootTable.Columns("yfcbarra").Index) Then

            '    If (grdetalle.GetValue("yfcbarra") <> String.Empty) Then
            '        _buscarRegistro(grdetalle.GetValue("yfcbarra"))


            '        '_prAddDetalleVenta()
            '        '_HabilitarProductos()
            '        ' MsgBox("hola de la grilla" + grdetalle.GetValue("yfcbarra") + t.Container.ToString)
            '        'ojo
            '    Else
            '        ToastNotification.Show(Me, "Seleccione un Producto Por Favor", My.Resources.WARNING, 3000, eToastGlowColor.Red, eToastPosition.TopCenter)
            '    End If

            'End If
salirIf:
        End If

        If (e.KeyData = Keys.Control + Keys.Enter And grdetalle.Row >= 0 And
            grdetalle.Col = grdetalle.RootTable.Columns("producto").Index) Then
            Dim indexfil As Integer = grdetalle.Row
            Dim indexcol As Integer = grdetalle.Col
            _HabilitarProductos()

        End If
        If (e.KeyData = Keys.Escape And grdetalle.Row >= 0) Then

            _prEliminarFila()


        End If


    End Sub

    Private Sub cargarProductos()
        Dim dt As DataTable
        If (G_Lote = True) Then
            dt = L_fnListarProductos(cbSucursal.Value, Str(_CodCliente))  ''1=Almacen
            Table_Producto = dt.Copy
        Else
            dt = L_fnListarProductosSinLote(cbSucursal.Value, Str(_CodCliente), CType(grdetalle.DataSource, DataTable).Clone)  ''1=Almacen
            Table_Producto = dt.Copy
        End If
    End Sub

    Private Function existeProducto(codigo As String) As Boolean
        Return (Table_Producto.Select("yfcbarra='" + codigo.Trim() + "'", "").Count > 0)
    End Function

    Private Function verificarExistenciaUnica(codigo As String) As Boolean
        Dim cont As Integer = 0
        For Each fila As GridEXRow In grdetalle.GetRows()
            If (fila.Cells("yfcbarra").Value.ToString.Trim = codigo.Trim) Then
                cont += 1
            End If
        Next
        Return (cont >= 1)
    End Function

    Private Sub ponerProducto(codigo As String)
        grdetalle.DataChanged = True
        CType(grdetalle.DataSource, DataTable).AcceptChanges()
        Dim fila As DataRow() = Table_Producto.Select("yfcbarra='" + codigo.Trim + "'", "")
        If (fila.Count > 0) Then
            Dim pos As Integer = -1
            _fnObtenerFilaDetalle(pos, grdetalle.GetValue("tbnumi"))
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbty5prod") = fila(0).ItemArray(0)
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("codigo") = fila(0).ItemArray(1)
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("yfcbarra") = fila(0).ItemArray(2)
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("producto") = fila(0).ItemArray(3)
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbumin") = fila(0).ItemArray(13)
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("unidad") = fila(0).ItemArray(14)
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpbas") = fila(0).ItemArray(15)
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot") = fila(0).ItemArray(15)
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbtotdesc") = fila(0).ItemArray(15)
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbcmin") = 1
            If (gb_FacturaIncluirICE) Then
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpcos") = fila(0).ItemArray(17)
            Else
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpcos") = 0
            End If
            CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot2") = fila(0).ItemArray(17)

            'CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tblote") = grProductos.GetValue("iclot")
            'CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbfechaVenc") = grProductos.GetValue("icfven")
            'CType(grdetalle.DataSource, DataTable).Rows(pos).Item("stock") = grProductos.GetValue("iccven")
            _prCalcularPrecioTotal()
        End If
    End Sub

    Private Sub sumarCantidad(codigo As String)
        Dim fila As DataRow() = CType(grdetalle.DataSource, DataTable).Select("yfcbarra='" + codigo.Trim + "'", "")
        If (fila.Count > 0) Then
            Dim pos1 As Integer = -1
            _fnObtenerFilaDetalle(pos1, fila(0).Item("tbnumi"))

            Dim cant As Integer = grdetalle.GetRow(pos1).Cells("tbcmin").Value + 1
            Dim stock As Integer = grdetalle.GetRow(pos1).Cells("stock").Value
            'If (cant > stock) Then
            Dim lin As Integer = grdetalle.GetRow(pos1).Cells("tbnumi").Value
            Dim pos2 As Integer = -1
            _fnObtenerFilaDetalle(pos2, lin)
            CType(grdetalle.DataSource, DataTable).Rows(pos2).Item("tbcmin") = cant
            CType(grdetalle.DataSource, DataTable).Rows(pos2).Item("tbptot") = CType(grdetalle.DataSource, DataTable).Rows(pos2).Item("tbpbas") * cant
            CType(grdetalle.DataSource, DataTable).Rows(pos2).Item("tbptot2") = grdetalle.GetRow(pos1).Cells("tbpcos").Value * cant
            CType(grdetalle.DataSource, DataTable).Rows(pos2).Item("tbtotdesc") = CType(grdetalle.DataSource, DataTable).Rows(pos2).Item("tbpbas") * cant
            Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
            'ToastNotification.Show(Me, "La cantidad de la venta no debe ser mayor al del stock" & vbCrLf &
            '        "Stock=" + Str(stock).ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            grdetalle.SetValue("yfcbarra", "")
            grdetalle.SetValue("tbcmin", 0)
            grdetalle.SetValue("tbptot", 0)
            grdetalle.SetValue("tbptot2", 0)
            grdetalle.DataChanged = True
            'grdetalle.Refetch()
            grdetalle.Refresh()
            '_prCalcularPrecioTotal()
            'Else
            '    If (cant = stock) Then
            '        'grdetalle.SelectedFormatStyle.ForeColor = Color.Blue
            '        'grdetalle.CurrentRow.Cells.Item(e.Column).FormatStyle = New GridEXFormatStyle
            '        'grdetalle.CurrentRow.Cells(e.Column).FormatStyle.BackColor = Color.Pink
            '        'grdetalle.CurrentRow.Cells.Item(e.Column).FormatStyle.BackColor = Color.DodgerBlue
            '        'grdetalle.CurrentRow.Cells.Item(e.Column).FormatStyle.ForeColor = Color.White
            '        'grdetalle.CurrentRow.Cells.Item(e.Column).FormatStyle.FontBold = TriState.True
            '    End If
            'End If

            _prCalcularPrecioTotal()
        End If
    End Sub

    Private Sub _buscarRegistro(cbarra As String)
        Dim _t As DataTable
        _t = L_fnListarProductosC(cbarra)
        If _t.Rows.Count > 0 Then
            CType(grdetalle.DataSource, DataTable).Rows(0).Item("producto") = _t.Rows(0).Item("yfcdprod1")
            CType(grdetalle.DataSource, DataTable).Rows(0).Item("tbcmin") = 1
            CType(grdetalle.DataSource, DataTable).Rows(0).Item("unidad") = _t.Rows(0).Item("uni")

        Else
            MsgBox("Codigo de Producto No Exite")
        End If
        'CType(grdetalle.DataSource, DataTable).Rows(0).Item("tbpbas") =
        'CType(grdetalle.DataSource, DataTable).Rows(0).Item("tbumin") = 1
        'CType(grdetalle.DataSource, DataTable).Rows(0).Item("tbptot2") = grdetalle.GetValue("tbpcos") * 1
        'ojo 'Dim pos, lin As Integer
        'pos = grdetalle.Row
        'lin = grdetalle.Col

        'CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbcmin") = 1
        'CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot") = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpbas")
        'CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot2") = grdetalle.GetValue("tbpcos") * 1


    End Sub
    Private Sub grProductos_KeyDown(sender As Object, e As KeyEventArgs) Handles grProductos.KeyDown
        If (Not _fnAccesible()) Then
            Return
        End If
        If (e.KeyData = Keys.Enter) Then
            Dim f, c As Integer
            c = grProductos.Col
            f = grProductos.Row
            If (f >= 0) Then

                If (IsNothing(FilaSelectLote)) Then
                    ''''''''''''''''''''''''
                    If (G_Lote = True) Then
                        InsertarProductosConLote()
                    Else
                        InsertarProductosSinLote()
                    End If
                    '''''''''''''''
                Else

                    '_fnExisteProductoConLote()
                    Dim pos As Integer = -1
                    grdetalle.Row = grdetalle.RowCount - 1
                    _fnObtenerFilaDetalle(pos, grdetalle.GetValue("tbnumi"))
                    Dim numiProd = FilaSelectLote.Item("yfnumi")
                    Dim lote As String = grProductos.GetValue("iclot")
                    Dim FechaVenc As Date = grProductos.GetValue("icfven")
                    If (Not _fnExisteProductoConLote(numiProd, lote, FechaVenc)) Then
                        'b.yfcdprod1, a.iclot, a.icfven, a.iccven
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbty5prod") = FilaSelectLote.Item("yfnumi")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("codigo") = FilaSelectLote.Item("yfcprod")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("yfcbarra") = FilaSelectLote.Item("yfcbarra")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("producto") = FilaSelectLote.Item("yfcdprod1")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbumin") = FilaSelectLote.Item("yfumin")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("unidad") = FilaSelectLote.Item("UnidMin")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpbas") = FilaSelectLote.Item("yhprecio")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot") = FilaSelectLote.Item("yhprecio")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbtotdesc") = FilaSelectLote.Item("yhprecio")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbcmin") = 1
                        'If (gb_FacturaIncluirICE) Then
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpcos") = FilaSelectLote.Item("pcos")
                        'Else
                        '    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpcos") = 0
                        'End If
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot2") = FilaSelectLote.Item("pcos")

                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tblote") = grProductos.GetValue("iclot")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbfechaVenc") = grProductos.GetValue("icfven")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("stock") = grProductos.GetValue("iccven")
                        _prCalcularPrecioTotal()
                        _DesHabilitarProductos()
                        FilaSelectLote = Nothing
                    Else
                        Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
                        ToastNotification.Show(Me, "El producto con el lote ya existe modifique su cantidad".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
                    End If



                End If

            End If
        End If
        If e.KeyData = Keys.Escape Then
            _DesHabilitarProductos()
            FilaSelectLote = Nothing
        End If
        '*** Filtrar por Compuesto ***'
        If (e.KeyData = Keys.Control + Keys.C) Then
            Dim indexfil As Integer = grdetalle.Row
            Dim indexcol As Integer = grdetalle.Col
            grup1 = grProductos.GetValue("grupo1")
            _HabilitarProductosSeleccion()
        End If
        '*** Filtrar por Acción ***'
        If (e.KeyData = Keys.Control + Keys.A) Then
            Dim indexfil As Integer = grdetalle.Row
            Dim indexcol As Integer = grdetalle.Col
            grup2 = grProductos.GetValue("grupo2")
            'Dim aux = Split(grup2, " ")
            'Dim aux1 = aux(0)
            _HabilitarProductosSeleccion()

        End If

    End Sub
    Private Sub grdetalle_CellValueChanged(sender As Object, e As ColumnActionEventArgs) Handles grdetalle.CellValueChanged
        If (e.Column.Index = grdetalle.RootTable.Columns("tbcmin").Index) Or (e.Column.Index = grdetalle.RootTable.Columns("tbpbas").Index) Then
            If (Not IsNumeric(grdetalle.GetValue("tbcmin")) Or grdetalle.GetValue("tbcmin").ToString = String.Empty) Then

                'grDetalle.GetRow(rowIndex).Cells("cant").Value = 1
                '  grDetalle.CurrentRow.Cells.Item("cant").Value = 1
                Dim lin As Integer = grdetalle.GetValue("tbnumi")
                Dim pos As Integer = -1
                _fnObtenerFilaDetalle(pos, lin)
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbcmin") = 1
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot") = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpbas")

                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbporc") = 0
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbdesc") = 0
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbtotdesc") = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpbas")
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot2") = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpcos")
                'grdetalle.SetValue("tbcmin", 1)
                'grdetalle.SetValue("tbptot", grdetalle.GetValue("tbpbas"))
            Else
                If (grdetalle.GetValue("tbcmin") > 0) Then
                    Dim rowIndex As Integer = grdetalle.Row
                    Dim porcdesc As Double = grdetalle.GetValue("tbporc")
                    Dim montodesc As Double = ((grdetalle.GetValue("tbpbas") * grdetalle.GetValue("tbcmin")) * (porcdesc / 100))
                    Dim lin As Integer = grdetalle.GetValue("tbnumi")
                    Dim pos As Integer = -1
                    _fnObtenerFilaDetalle(pos, lin)
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbdesc") = montodesc
                    grdetalle.SetValue("tbdesc", montodesc)
                    P_PonerTotal(rowIndex)

                Else
                    Dim lin As Integer = grdetalle.GetValue("tbnumi")
                    Dim pos As Integer = -1
                    _fnObtenerFilaDetalle(pos, lin)
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbcmin") = 1
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot") = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpbas")
                    _prCalcularPrecioTotal()
                    'grdetalle.SetValue("tbcmin", 1)
                    'grdetalle.SetValue("tbptot", grdetalle.GetValue("tbpbas"))

                End If
            End If
        End If
        '''''''''''''''''''''PORCENTAJE DE DESCUENTO '''''''''''''''''''''
        If (e.Column.Index = grdetalle.RootTable.Columns("tbporc").Index) Then
            If (Not IsNumeric(grdetalle.GetValue("tbporc")) Or grdetalle.GetValue("tbporc").ToString = String.Empty) Then

                'grDetalle.GetRow(rowIndex).Cells("cant").Value = 1
                '  grDetalle.CurrentRow.Cells.Item("cant").Value = 1
                Dim lin As Integer = grdetalle.GetValue("tbnumi")
                Dim pos As Integer = -1
                _fnObtenerFilaDetalle(pos, lin)
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbporc") = 0
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbdesc") = 0
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbtotdesc") = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot")
                'grdetalle.SetValue("tbcmin", 1)
                'grdetalle.SetValue("tbptot", grdetalle.GetValue("tbpbas"))
            Else
                If (grdetalle.GetValue("tbporc") > 0 And grdetalle.GetValue("tbporc") <= 100) Then

                    Dim porcdesc As Double = grdetalle.GetValue("tbporc")
                    Dim montodesc As Double = (grdetalle.GetValue("tbptot") * (porcdesc / 100))
                    Dim lin As Integer = grdetalle.GetValue("tbnumi")
                    Dim pos As Integer = -1
                    _fnObtenerFilaDetalle(pos, lin)
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbdesc") = montodesc
                    grdetalle.SetValue("tbdesc", montodesc)

                    Dim rowIndex As Integer = grdetalle.Row
                    P_PonerTotal(rowIndex)

                Else
                    Dim lin As Integer = grdetalle.GetValue("tbnumi")
                    Dim pos As Integer = -1
                    _fnObtenerFilaDetalle(pos, lin)
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbporc") = 0
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbdesc") = 0
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbtotdesc") = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot")
                    grdetalle.SetValue("tbporc", 0)
                    grdetalle.SetValue("tbdesc", 0)
                    grdetalle.SetValue("tbtotdesc", grdetalle.GetValue("tbptot"))
                    _prCalcularPrecioTotal()
                    'grdetalle.SetValue("tbcmin", 1)
                    'grdetalle.SetValue("tbptot", grdetalle.GetValue("tbpbas"))

                End If
            End If
        End If


        '''''''''''''''''''''MONTO DE DESCUENTO '''''''''''''''''''''
        If (e.Column.Index = grdetalle.RootTable.Columns("tbdesc").Index) Then
            If (Not IsNumeric(grdetalle.GetValue("tbdesc")) Or grdetalle.GetValue("tbdesc").ToString = String.Empty) Then

                'grDetalle.GetRow(rowIndex).Cells("cant").Value = 1
                '  grDetalle.CurrentRow.Cells.Item("cant").Value = 1
                Dim lin As Integer = grdetalle.GetValue("tbnumi")
                Dim pos As Integer = -1
                _fnObtenerFilaDetalle(pos, lin)
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbporc") = 0
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbdesc") = 0
                CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbtotdesc") = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot")
                'grdetalle.SetValue("tbcmin", 1)
                'grdetalle.SetValue("tbptot", grdetalle.GetValue("tbpbas"))
            Else
                If (grdetalle.GetValue("tbdesc") > 0 And grdetalle.GetValue("tbdesc") <= grdetalle.GetValue("tbptot")) Then

                    Dim montodesc As Double = grdetalle.GetValue("tbdesc")
                    Dim pordesc As Double = ((montodesc * 100) / grdetalle.GetValue("tbptot"))

                    Dim lin As Integer = grdetalle.GetValue("tbnumi")
                    Dim pos As Integer = -1
                    _fnObtenerFilaDetalle(pos, lin)
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbdesc") = montodesc
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbporc") = pordesc

                    grdetalle.SetValue("tbporc", pordesc)
                    Dim rowIndex As Integer = grdetalle.Row
                    P_PonerTotal(rowIndex)

                Else
                    Dim lin As Integer = grdetalle.GetValue("tbnumi")
                    Dim pos As Integer = -1
                    _fnObtenerFilaDetalle(pos, lin)
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbporc") = 0
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbdesc") = 0
                    CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbtotdesc") = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot")
                    grdetalle.SetValue("tbporc", 0)
                    grdetalle.SetValue("tbdesc", 0)
                    grdetalle.SetValue("tbtotdesc", grdetalle.GetValue("tbptot"))
                    _prCalcularPrecioTotal()
                    'grdetalle.SetValue("tbcmin", 1)
                    'grdetalle.SetValue("tbptot", grdetalle.GetValue("tbpbas"))

                End If
            End If
        End If

    End Sub
    Private Sub tbPdesc_ValueChanged(sender As Object, e As EventArgs) Handles tbPdesc.ValueChanged
        If (tbPdesc.Focused) Then
            If (Not tbPdesc.Text = String.Empty And Not tbtotal.Text = String.Empty) Then
                If (tbPdesc.Value = 0 Or tbPdesc.Value > 100) Then
                    tbPdesc.Value = 0
                    tbMdesc.Value = 0

                    _prCalcularPrecioTotal()

                Else

                    Dim porcdesc As Double = tbPdesc.Value
                    Dim montodesc As Double = (grdetalle.GetTotal(grdetalle.RootTable.Columns("tbtotdesc"), AggregateFunction.Sum) * (porcdesc / 100))
                    tbMdesc.Value = montodesc

                    tbIce.Value = grdetalle.GetTotal(grdetalle.RootTable.Columns("tbptot2"), AggregateFunction.Sum) * (gi_ICE / 100)

                    If (gb_FacturaIncluirICE = True) Then
                        tbtotal.Value = grdetalle.GetTotal(grdetalle.RootTable.Columns("tbtotdesc"), AggregateFunction.Sum) - montodesc + tbIce.Value
                    Else
                        tbtotal.Value = grdetalle.GetTotal(grdetalle.RootTable.Columns("tbtotdesc"), AggregateFunction.Sum) - montodesc
                    End If
                End If


            End If
            If (tbPdesc.Text = String.Empty) Then
                tbMdesc.Value = 0

            End If
        End If
    End Sub

    Private Sub tbMdesc_ValueChanged(sender As Object, e As EventArgs) Handles tbMdesc.ValueChanged
        If (tbMdesc.Focused) Then

            Dim total As Double = tbtotal.Value
            If (Not tbMdesc.Text = String.Empty And Not tbMdesc.Text = String.Empty) Then
                If (tbMdesc.Value = 0 Or tbMdesc.Value > total) Then
                    tbMdesc.Value = 0
                    tbPdesc.Value = 0
                    _prCalcularPrecioTotal()
                Else
                    Dim montodesc As Double = tbMdesc.Value
                    Dim pordesc As Double = ((montodesc * 100) / grdetalle.GetTotal(grdetalle.RootTable.Columns("tbtotdesc"), AggregateFunction.Sum))
                    tbPdesc.Value = pordesc
                    tbIce.Value = grdetalle.GetTotal(grdetalle.RootTable.Columns("tbptot2"), AggregateFunction.Sum) * (gi_ICE / 100)
                    If (gb_FacturaIncluirICE = True) Then
                        tbtotal.Value = grdetalle.GetTotal(grdetalle.RootTable.Columns("tbtotdesc"), AggregateFunction.Sum) - montodesc + tbIce.Value
                    Else
                        tbtotal.Value = grdetalle.GetTotal(grdetalle.RootTable.Columns("tbtotdesc"), AggregateFunction.Sum) - montodesc
                    End If

                End If

            End If

            If (tbMdesc.Text = String.Empty) Then
                tbMdesc.Value = 0

            End If
        End If

    End Sub

    Private Sub grdetalle_CellEdited(sender As Object, e As ColumnActionEventArgs) Handles grdetalle.CellEdited
        If (e.Column.Index = grdetalle.RootTable.Columns("tbcmin").Index) Then
            If (Not IsNumeric(grdetalle.GetValue("tbcmin")) Or grdetalle.GetValue("tbcmin").ToString = String.Empty) Then

                grdetalle.SetValue("tbcmin", 1)
                grdetalle.SetValue("tbptot", grdetalle.GetValue("tbpbas"))
                grdetalle.SetValue("tbporc", 0)
                grdetalle.SetValue("tbdesc", 0)
                grdetalle.SetValue("tbtotdesc", grdetalle.GetValue("tbpbas"))


            Else
                If (grdetalle.GetValue("tbcmin") > 0) Then

                    Dim cant As Integer = grdetalle.GetValue("tbcmin")
                    Dim stock As Integer = grdetalle.GetValue("stock")
                    If (cant > stock) And stock <> -9999 Then
                        Dim lin As Integer = grdetalle.GetValue("tbnumi")
                        Dim pos As Integer = -1
                        _fnObtenerFilaDetalle(pos, lin)
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbcmin") = 1
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot") = CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbpbas")
                        CType(grdetalle.DataSource, DataTable).Rows(pos).Item("tbptot2") = grdetalle.GetValue("tbpcos") * 1
                        Dim img As Bitmap = New Bitmap(My.Resources.mensaje, 50, 50)
                        ToastNotification.Show(Me, "La cantidad de la venta no debe ser mayor al del stock" & vbCrLf &
                        "Stock=" + Str(stock).ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
                        grdetalle.SetValue("tbcmin", 1)
                        grdetalle.SetValue("tbptot", grdetalle.GetValue("tbpbas"))
                        grdetalle.SetValue("tbptot2", grdetalle.GetValue("tbpcos") * 1)

                        _prCalcularPrecioTotal()
                    Else
                        If (cant = stock) Then


                            'grdetalle.SelectedFormatStyle.ForeColor = Color.Blue
                            'grdetalle.CurrentRow.Cells.Item(e.Column).FormatStyle = New GridEXFormatStyle
                            'grdetalle.CurrentRow.Cells(e.Column).FormatStyle.BackColor = Color.Pink
                            'grdetalle.CurrentRow.Cells.Item(e.Column).FormatStyle.BackColor = Color.DodgerBlue
                            'grdetalle.CurrentRow.Cells.Item(e.Column).FormatStyle.ForeColor = Color.White
                            'grdetalle.CurrentRow.Cells.Item(e.Column).FormatStyle.FontBold = TriState.True
                        End If
                    End If

                Else

                    grdetalle.SetValue("tbcmin", 1)
                    grdetalle.SetValue("tbptot", grdetalle.GetValue("tbpbas"))
                    grdetalle.SetValue("tbporc", 0)
                    grdetalle.SetValue("tbdesc", 0)
                    grdetalle.SetValue("tbtotdesc", grdetalle.GetValue("tbpbas"))

                End If
            End If
        End If
    End Sub
    Private Sub grdetalle_MouseClick(sender As Object, e As MouseEventArgs) Handles grdetalle.MouseClick
        If (Not _fnAccesible()) Then
            Return
        End If
        If (grdetalle.RowCount >= 2) Then
            If (grdetalle.CurrentColumn.Index = grdetalle.RootTable.Columns("img").Index) Then
                _prEliminarFila()
            End If
        End If


    End Sub
    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click

    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click

    End Sub
    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click

    End Sub

    Private Sub grVentas_SelectionChanged(sender As Object, e As EventArgs) Handles grVentas.SelectionChanged
        If (grVentas.RowCount >= 0 And grVentas.Row >= 0) Then
            _prMostrarRegistro(grVentas.Row)
        End If
    End Sub

    Private Sub btnSiguiente_Click(sender As Object, e As EventArgs) Handles btnSiguiente.Click
        Dim _pos As Integer = grVentas.Row
        If _pos < grVentas.RowCount - 1 And _pos >= 0 Then
            _pos = grVentas.Row + 1
            '' _prMostrarRegistro(_pos)
            grVentas.Row = _pos
        End If
    End Sub

    Private Sub btnUltimo_Click(sender As Object, e As EventArgs) Handles btnUltimo.Click
        Dim _pos As Integer = grVentas.Row
        If grVentas.RowCount > 0 Then
            _pos = grVentas.RowCount - 1
            ''  _prMostrarRegistro(_pos)
            grVentas.Row = _pos
        End If
    End Sub

    Private Sub btnAnterior_Click(sender As Object, e As EventArgs) Handles btnAnterior.Click
        Dim _MPos As Integer = grVentas.Row
        If _MPos > 0 And grVentas.RowCount > 0 Then
            _MPos = _MPos - 1
            ''  _prMostrarRegistro(_MPos)
            grVentas.Row = _MPos
        End If
    End Sub

    Private Sub btnPrimero_Click(sender As Object, e As EventArgs) Handles btnPrimero.Click
        _PrimerRegistro()
    End Sub
    Private Sub grVentas_KeyDown(sender As Object, e As KeyEventArgs) Handles grVentas.KeyDown
        If e.KeyData = Keys.Enter Then
            MSuperTabControl.SelectedTabIndex = 0
            grdetalle.Focus()
        End If
    End Sub

    Private Sub TbNit_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TbNit.Validating
        Dim nom1, nom2 As String
        nom1 = ""
        nom2 = ""
        If (TbNit.Text.Trim = String.Empty) Then
            TbNit.Text = "0"
        End If
        L_Validar_Nit(TbNit.Text.Trim, nom1, nom2)
        TbNombre1.Text = nom1
        TbNombre2.Text = nom2
    End Sub
    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If (Not _fnAccesible()) Then
            'P_GenerarReporte(tbCodigo.Text)
            P_prImprimirFacturar(tbCodigo.Text, True, True)
        End If
    End Sub

    Private Sub TbNit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TbNit.KeyPress
        g_prValidarTextBox(1, e)
    End Sub


    Private Sub swTipoVenta_Leave(sender As Object, e As EventArgs) Handles swTipoVenta.Leave
        grdetalle.Select()
    End Sub

    Private Sub cbSucursal_ValueChanged(sender As Object, e As EventArgs) Handles cbSucursal.ValueChanged
        Table_Producto = Nothing
    End Sub

    Private Sub btnAnularFact_Click(sender As Object, e As EventArgs) Handles btnAnularFact.Click
        If (MessageBox.Show("Esta seguro de ANULAR la Factura " + tbNroFactura.Text + "?", "PREGUNTA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes) Then
            'L_Modificar_Factura("fvanumi = " + tbCodigo.Text + " and fvanfac = " + tbNroFactura.Text + " and fvaautoriz = " + tbNroAutoriz.Text, "", "", "", "0")
            L_ActualizaTFV001(tbCodigo.Text, tbNroFactura.Text, tbNroAutoriz.Text, "0")
            L_ActualizaTFV0011(tbCodigo.Text)
            updateTO001C(tbCodigo.Text, "0")
            Dim mensajeError As String = ""
            Dim res As Boolean = L_fnEliminarVenta(tbCodigo.Text, mensajeError)

            _prCargarVenta()

            If res Then
                ToastNotification.Show(Me, "La Factura " + tbNroFactura.Text + ", Se ANULADO correctamente",
                                   My.Resources.OK, 3000,
                                   eToastGlowColor.Blue, eToastPosition.BottomLeft)
            Else

                Dim img As Bitmap = New Bitmap(My.Resources.cancel, 50, 50)
                ToastNotification.Show(Me, "No se pudo realizar la anulación ", img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            End If

        End If
    End Sub


#End Region
End Class