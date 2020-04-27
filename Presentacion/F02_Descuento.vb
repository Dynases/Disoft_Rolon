Imports Logica.AccesoLogica
Imports Janus.Windows.GridEX
Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Controls


Public Class F02_Descuento

#Region "Variables Globales"

    Dim _Pos As Integer
    Dim _Nuevo As Boolean
    Dim _Dsencabezado As DataSet

    Dim _BindingSource As BindingSource
    Dim _Modificar As Boolean

    Dim modif As Boolean = True

    Public _nameButton As String
    Public _tab As SuperTabItem
    Public _modulo As SideNavItem

    Dim dtPreciosDesc As New DataTable

#End Region

#Region "Metodos"
    Private Sub _PIniciarTodo()
        If (Not gb_ConexionAbierta) Then
            L_prAbrirConexion()
        End If
        P_prArmarComboCategoria()
        _PCargarGridCategoriasPrecios(-1)
        _Filtrar()

        MBtNuevo.Visible = False
        MBtModificar.Visible = False
        MBtEliminar.Visible = False

        Me.Text = " P R E C I O S  -  D E S C U E N T O S "
        Me.WindowState = FormWindowState.Maximized

        btNuevoP.Image = My.Resources.ADICIONAR

        MBtPrimero.PerformClick()

        'activar los permisos del rol
        _PAsignarPermisos()

        'Asignar el menustrip quitar al grid JGr_Descuentos
        JGr_Descuentos.ContextMenuStrip = CmDetalle
    End Sub

    Private Sub _PAsignarPermisos()
        Dim dtRolUsu() As DataRow = L_prRolDetalleGeneral(gi_userRol).Select("yaprog='" + _nameButton + "'")

        Dim show As Boolean = False
        Dim add As Boolean = False
        Dim modif As Boolean = False
        Dim del As Boolean = False

        If (dtRolUsu.Count = 1) Then
            show = dtRolUsu(0).Item("ycshow")
            add = dtRolUsu(0).Item("ycadd")
            modif = dtRolUsu(0).Item("ycmod")
            del = dtRolUsu(0).Item("ycdel")
        End If

        If add = False Then
            'MBtNuevo.Visible = False
        End If
        If modif = False Then
            'MBtModificar.Visible = False
        End If
        If del = False Then
            'MBtEliminar.Visible = False
        End If
    End Sub
    Private Sub _PGrabarRegistro()
        'borrar toda la tabla por la categoria
        Dim i, j, codProd, numi, catPrecio As Integer
        Dim precio As Double
        Dim dtCatPrecios As DataTable
        dtCatPrecios = L_CategoriaPrecioGeneral()
        'terminar edicion de la grilla
        JCb_CatProducto.Focus()

        For i = 0 To JGr_Detalle.RowCount - 1
            JGr_Detalle.Row = i
            If (JGr_Detalle.CurrentRow.Cells("huboCambio").Value) Then
                codProd = JGr_Detalle.CurrentRow.Cells(0).Value
                L_PrecioProd_BorrarPorCodProd(codProd)
                For j = 0 To dtCatPrecios.Rows.Count - 1
                    catPrecio = dtCatPrecios.Rows(j).Item("cinumi")
                    If IsNumeric(JGr_Detalle.CurrentRow.Cells(j + 3).Value) = True Then
                        precio = JGr_Detalle.CurrentRow.Cells(j + 3).Value
                    Else
                        precio = 0
                    End If
                    L_PrecioProd_Grabar(numi, codProd, catPrecio, precio)
                Next
            End If
        Next

        ToastNotification.Show(Me, "Precios de " + JCb_CatProducto.Text + " Grabado con Exito.", My.Resources.GRABACION_EXITOSA, 5000, eToastGlowColor.Green, eToastPosition.BottomRight)
        _PCargarDetalle(JCb_CatProducto.Value)
    End Sub

    Private Sub _PSalirRegistro()
        Me.Close()
        _modulo.Select()
        _tab.Close()
    End Sub
    Private Sub _PAgregarFilasDetalle(cant As Integer)
        Dim newRow As GridEXRow
        Dim i As Integer
        For i = 1 To cant
            newRow = JGr_Detalle.AddItem()
        Next

    End Sub

    Private Sub P_prArmarComboCategoria()
        Dim Dt As DataTable
        Dt = L_fnObtenerCategoria()
        ''   Dt = L_fnObtenerLibreria("5", IIf(TipoForm = 1, "cenum>0", "cenum<0"))
        g_prArmarCombo(JCb_CatProducto, Dt, 60, 200, "Código", "Categoría")
    End Sub
    Private Sub _PCargarComboLibreria(ByVal cb As Janus.Windows.GridEX.EditControls.MultiColumnCombo, ByVal concep As Integer)
        Dim _Ds As New DataSet
        _Ds = L_General_LibreriaDetalle(-1, concep)

        With cb
            .DropDownList.Columns.Clear()

            .DropDownList.Columns.Add(_Ds.Tables(0).Columns("cenum").ToString).Width = 50
            .DropDownList.Columns(0).Caption = "Código"
            .DropDownList.Columns.Add(_Ds.Tables(0).Columns("cedesc").ToString).Width = 250
            .DropDownList.Columns(1).Caption = "Descripcion"

            .ValueMember = _Ds.Tables(0).Columns("cenum").ToString
            .DisplayMember = _Ds.Tables(0).Columns("cedesc").ToString
            .DataSource = _Ds.Tables(0)
            .Refresh()
        End With

    End Sub

    Private Sub _PCargarDetalle(idTipoProd As String)
        Dim dtProd, dtCatPrecios As New DataTable
        dtProd = L_Productos_GeneralFiltrado3(-1, "cacat= " + idTipoProd + "AND caest=1 AND caserie=0 AND cecon=103 AND cenum=cagr3")
        'dtCatPrecios = L_General_LibreriaDetalle(-1, 8).Tables(0)
        dtCatPrecios = L_CategoriaPrecioGeneral()

        'Cargar categoria de precios
        Dim i, j As Integer
        j = 2
        For i = 0 To dtCatPrecios.Rows.Count() - 1
            'aumentar columna al datatable de productos
            dtProd.Columns.Add(dtCatPrecios.Rows(i).Item("cicod"), Type.GetType("System.Double"))
        Next
        dtProd.Columns.Add("huboCambio", Type.GetType("System.Boolean"))
        dtProd.Columns.Add("listPrecio", Type.GetType("System.String"))


        'cargar datos de la tabla
        Dim dtPreciosProd As DataTable
        For i = 0 To dtProd.Rows.Count - 1
            Dim where As String = "chcprod='" + dtProd.Rows(i).Item("canumi").ToString + "'"
            dtPreciosProd = L_PrecioProd_GeneralConCatPrecio(-1, where).Tables(0)
            Dim listPrecio = String.Empty
            Dim k As Integer = 0
            For j = 0 To dtPreciosProd.Rows.Count - 1
                Dim catPrecio As String = dtPreciosProd.Rows(j).Item("cicod")
                Dim precio As Double = dtPreciosProd.Rows(j).Item("chprecio")
                dtProd.Rows(i).Item(catPrecio) = precio
                listPrecio += precio.ToString + "|"
            Next
            dtProd.Rows(i).Item("huboCambio") = False
            dtProd.Rows(i).Item("listPrecio") = listPrecio.Trim("|")
        Next

        JGr_Detalle.BoundMode = BoundMode.Bound
        JGr_Detalle.DataSource = dtProd
        JGr_Detalle.RetrieveStructure()

        'dar formato a las columnas
        With JGr_Detalle.RootTable.Columns(0)
            .Caption = "Cod. Producto"
            .Width = 80
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.FontSize = gi_fuenteTamano
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .CellStyle.BackColor = Color.AliceBlue
        End With

        With JGr_Detalle.RootTable.Columns(1)
            .Caption = "Codigo Flex"
            .Width = 80
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.FontSize = gi_fuenteTamano
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .CellStyle.BackColor = Color.AliceBlue
        End With

        ''JGr_Detalle.RootTable.Columns.Add()
        With JGr_Detalle.RootTable.Columns(2)
            .Caption = "Descripcion"
            .Width = 350
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.FontSize = gi_fuenteTamano
            .CellStyle.BackColor = Color.AliceBlue
        End With

        For i = 3 To JGr_Detalle.RootTable.Columns.Count - 1
            With JGr_Detalle.RootTable.Columns(i)
                .Width = 65
                .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
                .CellStyle.FontSize = gi_fuenteTamano
                .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
                .FormatString = "0.00"
                .Visible = False
            End With
        Next

        With JGr_Detalle.RootTable.Columns("huboCambio")
            .Visible = False
        End With

        With JGr_Detalle.RootTable.Columns("listPrecio")
            .Visible = False
        End With
        With JGr_Detalle.RootTable.Columns(3)
            .Caption = "Familia"
            .Width = 200
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .TextAlignment = TextAlignment.Near
            .CellStyle.FontSize = gi_fuenteTamano
            .CellStyle.BackColor = Color.AliceBlue
            .Visible = True
        End With

        'Habilitar Filtradores
        With JGr_Detalle
            .DefaultFilterRowComparison = FilterConditionOperator.Contains
            .FilterMode = FilterMode.Automatic
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges
            .GroupByBoxVisible = False
            'diseño de la grilla
            .VisualStyle = VisualStyle.Office2007
        End With


    End Sub

    Private Sub _PCargarGridCategoriasPrecios(cod As String)

        dtPreciosDesc = L_fnMostrarDescuentosPrecios(cod)

        JGr_Descuentos.DataSource = dtPreciosDesc
        JGr_Descuentos.RetrieveStructure()

        With JGr_Descuentos.RootTable.Columns("danumi")
            .Visible = False
        End With
        With JGr_Descuentos.RootTable.Columns("dacanumi")
            .Caption = "Cod Prod."
            .Width = 50
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.FontSize = gi_fuenteTamano
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .CellStyle.BackColor = Color.AliceBlue
            .Visible = False
        End With
        With JGr_Descuentos.RootTable.Columns("dacant1")
            .Caption = "Desde"
            .Width = 100
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.FontSize = gi_fuenteTamano
            .CellStyle.BackColor = Color.AliceBlue
        End With
        With JGr_Descuentos.RootTable.Columns("dacant2")
            .Caption = "Hasta"
            .Width = 100
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.FontSize = gi_fuenteTamano
            .CellStyle.BackColor = Color.AliceBlue
        End With
        With JGr_Descuentos.RootTable.Columns("dapreciou")
            .Caption = "Precio Un."
            .Width = 120
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.FontSize = gi_fuenteTamano
            .CellStyle.BackColor = Color.AliceBlue
            .FormatString = "0.0000"
        End With
        With JGr_Descuentos.RootTable.Columns("dafinicio")
            .Caption = "Fecha Inicio"
            .Width = 180
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.FontSize = gi_fuenteTamano
            .CellStyle.BackColor = Color.AliceBlue
            .Visible = False
        End With
        With JGr_Descuentos.RootTable.Columns("daffin")
            .Caption = "Fecha Fin"
            .Width = 180
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.FontSize = gi_fuenteTamano
            .CellStyle.BackColor = Color.AliceBlue
            .Visible = False
        End With
        With JGr_Descuentos.RootTable.Columns("estado")
            .Visible = False
        End With
        'Habilitar Filtradores
        With JGr_Descuentos
            .DefaultFilterRowComparison = FilterConditionOperator.Contains
            '.FilterMode = FilterMode.Automatic
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges
            .GroupByBoxVisible = False
            'diseño de la grilla
            .VisualStyle = VisualStyle.Office2007
            '.AllowEdit = InheritableBoolean.False
        End With


    End Sub


    Private Sub _Filtrar()
        _Dsencabezado = New DataSet
        _Dsencabezado = L_General_LibreriaDetalle(-1, 5)
        '_First = False
        If _Dsencabezado.Tables(0).Rows.Count <> 0 Then
            _Pos = 0
            _MostrarRegistro(_Pos)
            If _Dsencabezado.Tables(0).Rows.Count > 0 Then
                MBtPrimero.Visible = True
                MBtAnterior.Visible = True
                MBtSiguiente.Visible = True
                MBtUltimo.Visible = True
            End If
        End If

    End Sub
    Private Sub _MostrarRegistro(_N As Integer)
        JCb_CatProducto.SelectedIndex = _N
    End Sub
    Private Sub _PrimerRegistro()
        _Pos = 0
        _MostrarRegistro(_Pos)
        MLbPaginacion.Text = Str(_Pos + 1) + "/" + _Dsencabezado.Tables(0).Rows.Count.ToString
    End Sub
    Private Sub _AnteriorRegistro()
        If _Pos > 0 Then
            _Pos = _Pos - 1
            _MostrarRegistro(_Pos)
            MLbPaginacion.Text = Str(_Pos + 1) + "/" + _Dsencabezado.Tables(0).Rows.Count.ToString
        End If
    End Sub
    Private Sub _SiguienteRegistro()
        If _Pos < _Dsencabezado.Tables(0).Rows.Count - 1 Then
            _Pos = _Pos + 1
            _MostrarRegistro(_Pos)
            MLbPaginacion.Text = Str(_Pos + 1) + "/" + _Dsencabezado.Tables(0).Rows.Count.ToString
        End If
    End Sub
    Private Sub _UltimoRegistro()
        _Pos = _Dsencabezado.Tables(0).Rows.Count - 1
        _MostrarRegistro(_Pos)
        MLbPaginacion.Text = Str(_Pos + 1) + "/" + _Dsencabezado.Tables(0).Rows.Count.ToString
    End Sub
#End Region

    Private Sub P_PrecioProductos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _PIniciarTodo()
    End Sub

    Private Sub JCb_CatProducto_ValueChanged(sender As Object, e As EventArgs) Handles JCb_CatProducto.ValueChanged
        If JCb_CatProducto.SelectedIndex >= 0 Then
            _PCargarDetalle(JCb_CatProducto.Value)
        End If
    End Sub

    Private Sub JGr_Detalle_KeyPress(sender As Object, e As KeyPressEventArgs) Handles JGr_Detalle.KeyPress
        'If Char.IsDigit(e.KeyChar) = False Then
        '    e.Handled = True
        'End If

    End Sub

    Private Sub MBtPrimero_Click(sender As Object, e As EventArgs) Handles MBtPrimero.Click
        _PrimerRegistro()
    End Sub

    Private Sub MBtAnterior_Click(sender As Object, e As EventArgs) Handles MBtAnterior.Click
        _AnteriorRegistro()
    End Sub

    Private Sub MBtSiguiente_Click(sender As Object, e As EventArgs) Handles MBtSiguiente.Click
        _SiguienteRegistro()
    End Sub

    Private Sub MBtUltimo_Click(sender As Object, e As EventArgs) Handles MBtUltimo.Click
        _UltimoRegistro()
    End Sub

    Private Sub JGr_Detalle_EditingCell(sender As Object, e As EditingCellEventArgs) Handles JGr_Detalle.EditingCell
        If e.Column.Index = 0 Or e.Column.Index = 1 Or e.Column.Index = 2 Or e.Column.Index = 3 Then
            e.Cancel = True
        End If

        If modif = False Then
            e.Cancel = True
        End If
    End Sub

    Private Sub MBtSalir_Click(sender As Object, e As EventArgs) Handles MBtSalir.Click
        _PSalirRegistro()
    End Sub

    Private Sub MBtGrabar_Click(sender As Object, e As EventArgs) Handles MBtGrabar.Click
        '_PGrabarRegistro()
    End Sub

    Private Sub MBtImprimir_Click(sender As Object, e As EventArgs) Handles MBtImprimir.Click
        Try
            Dim myppd As New PrintPreviewDialog
            GridEXPrintDocumentListaPrecios.PageHeaderCenter = "TIPO DE PRODUCTO : " + JCb_CatProducto.Text
            GridEXPrintDocumentListaPrecios.HeaderDistance = 75
            myppd.Document = GridEXPrintDocumentListaPrecios
            myppd.WindowState = FormWindowState.Maximized
            myppd.ShowDialog()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub JGr_Detalle_CellEdited(sender As Object, e As ColumnActionEventArgs) Handles JGr_Detalle.CellEdited
        Try
            If (Not IsNumeric(JGr_Detalle.CurrentRow.Cells(e.Column.Index).Value)) Then
                JGr_Detalle.CurrentRow.Cells(e.Column.Index).Value = 0
            End If

            Dim listPrecio = JGr_Detalle.CurrentRow.Cells("listPrecio").Value.ToString.Split("|")
            Dim valorActual = Convert.ToDecimal(JGr_Detalle.CurrentRow.Cells(e.Column.Index).Value)
            Dim valorInicial As Decimal = 0
            Try
                valorInicial = Convert.ToDecimal(listPrecio(e.Column.Index - 3))
            Catch
            End Try

            If (valorActual <> valorInicial) Then
                JGr_Detalle.CurrentRow.Cells("huboCambio").Value = True
            Else
                JGr_Detalle.CurrentRow.Cells("huboCambio").Value = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub JGr_Detalle_Click(sender As Object, e As EventArgs) Handles JGr_Detalle.Click
        If JGr_Detalle.CurrentRow.Selected <> JGr_Detalle.FilterRow.Selected Then
            If JGr_Detalle.Focused Then
                If JGr_Detalle.GetValue("canumi") > 0 Then
                    tbCodPro.Text = JGr_Detalle.CurrentRow.Cells("canumi").Value.ToString
                    lbProducto.Text = JGr_Detalle.CurrentRow.Cells("cadesc").Value.ToString

                    _PCargarGridCategoriasPrecios(tbCodPro.Text)
                    _MostrarFechas(JGr_Descuentos.Row)
                    _Inhabilitar()



                    'P_prAddFilaDetallePrecio()
                    'CType(JGr_Descuentos.DataSource, DataTable).Rows(0).Item("dacant1") = JGr_Detalle.CurrentRow.Cells("canumi").Value.ToString
                End If
            End If
        End If
    End Sub
    Public Sub _MostrarFechas(_N As Integer)
        With JGr_Descuentos
            dtiFechaIni.Value = .GetValue("dafinicio")
            dtiFechaFin.Value = .GetValue("daffin")
        End With
    End Sub
    Private Sub _Inhabilitar()
        JGr_Descuentos.Enabled = False
        dtiFechaIni.IsInputReadOnly = True
        dtiFechaIni.ButtonDropDown.Enabled = False
        dtiFechaFin.IsInputReadOnly = True
        dtiFechaFin.ButtonDropDown.Enabled = False

        btGrabarP.Enabled = False
    End Sub
    Private Sub _Habilitar()
        JGr_Descuentos.Enabled = True
        dtiFechaIni.IsInputReadOnly = False
        dtiFechaIni.ButtonDropDown.Enabled = True
        dtiFechaFin.IsInputReadOnly = False
        dtiFechaFin.ButtonDropDown.Enabled = True

        btGrabarP.Enabled = True
    End Sub

    Private Sub btNuevoP_Click(sender As Object, e As EventArgs) Handles btNuevoP.Click
        If JGr_Descuentos.RowCount > 0 Then
            L_fnActualizarEstadoPrecios(tbCodPro.Text)
            _PCargarGridCategoriasPrecios(tbCodPro.Text)
        End If
        dtiFechaIni.Value = Now.Date
        dtiFechaFin.Value = Now.Date
        P_prAddFilaDetallePrecio(-1)
        _Habilitar()
    End Sub

    Private Sub JGr_Descuentos_EditingCell(sender As Object, e As EditingCellEventArgs) Handles JGr_Descuentos.EditingCell
        If (e.Column.Key.Equals("dacant1") Or e.Column.Key.Equals("dacant2") Or e.Column.Key.Equals("dapreciou")) Then
            e.Cancel = False
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub P_prAddFilaDetallePrecio(cant2 As Integer)
        Dim fil As DataRow
        fil = dtPreciosDesc.NewRow
        fil.Item("danumi") = 0
        fil.Item("dacanumi") = 0
        fil.Item("dacant1") = cant2 + 1
        fil.Item("dacant2") = 0
        fil.Item("dapreciou") = 0.00
        fil.Item("dafinicio") = Now.Date
        fil.Item("daffin") = Now.Date
        fil.Item("estado") = 0

        dtPreciosDesc.Rows.Add(fil)


        JGr_Descuentos.Col = 3
        JGr_Descuentos.Select()
    End Sub

    Private Sub JGr_Descuentos_KeyDown(sender As Object, e As KeyEventArgs) Handles JGr_Descuentos.KeyDown
        If (e.KeyData = Keys.Enter And JGr_Descuentos.Col = JGr_Descuentos.RootTable.Columns("dapreciou").Index) Then
            Dim filIndex As Integer = JGr_Descuentos.Row
            If (filIndex = JGr_Descuentos.RowCount - 1) Then
                Dim cant2 = JGr_Descuentos.CurrentRow.Cells(3).Value
                P_prAddFilaDetallePrecio(cant2)
            End If
        End If
    End Sub

    Private Sub btGrabarP_Click(sender As Object, e As EventArgs) Handles btGrabarP.Click
        Dim numi As String = ""
        If (_ValidarCampos()) Then
            'Grabar
            Dim res As Boolean = L_fnGrabarPreciosDescuentos(numi, tbCodPro.Text, dtiFechaIni.Value, dtiFechaFin.Value, CType(JGr_Descuentos.DataSource, DataTable))

            If (res) Then
                ToastNotification.Show(Me, "Descuento de Producto Grabado con éxito.".ToUpper,
                                   My.Resources.GRABACION_EXITOSA,
                                   3000,
                                   eToastGlowColor.Green,
                                   eToastPosition.TopCenter)
                btGrabarP.Enabled = False
            Else
                ToastNotification.Show(Me, "No se pudo grabar los descuentos.".ToUpper,
                                   My.Resources.WARNING,
                                   3000,
                                   eToastGlowColor.Red,
                                   eToastPosition.TopCenter)
            End If
        End If
    End Sub
    Public Function _ValidarCampos() As Boolean
        If (tbCodPro.Text <= 0) Then
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "Por Favor debe dar click a un producto".ToUpper, img, 2000, eToastGlowColor.Red, eToastPosition.BottomCenter)
            Return False
        End If

        For Each aux In JGr_Descuentos.GetRows
            If (aux.Cells(2)).Value.ToString = String.Empty Or (aux.Cells(3)).Value.ToString = String.Empty Or (aux.Cells(4)).Value.ToString = String.Empty Then
                Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
                ToastNotification.Show(Me, "Los campos no pueden estar vacios por favor coloque datos mayores a 0".ToUpper, img, 4000, eToastGlowColor.Red, eToastPosition.BottomCenter)
                Return False
            Else
                If (aux.Cells(2)).Value <= 0 Or (aux.Cells(3)).Value <= 0 Or (aux.Cells(4)).Value <= 0 Then
                    Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
                    ToastNotification.Show(Me, "Las campos no pueden ser 0 por favor coloque datos mayores a 0".ToUpper, img, 4000, eToastGlowColor.Red, eToastPosition.BottomCenter)
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    Private Sub QuitarProductoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitarProductoToolStripMenuItem.Click
        Try
            JGr_Descuentos.CurrentRow.EndEdit()
            JGr_Descuentos.CurrentRow.Delete()
            JGr_Descuentos.Refetch()
            JGr_Descuentos.Refresh()
            JGr_Descuentos.UpdateData()
        Catch ex As Exception
            'sms
            'MsgBox(ex)
        End Try
    End Sub

End Class