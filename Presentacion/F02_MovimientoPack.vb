Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Controls
Imports Janus.Windows.GridEX
Imports Logica.AccesoLogica

Public Class F02_MovimientoPack

#Region "Atributos generales"

    Private stTitulo As String = "M O V I M I E N T O   D E   P A C K"
    Private inTipo As Byte = 1 '1=Equipos, 2=Productos

    Private stEst As String = "1"
    Private stAlm As String = "1"
    Private stIddc As String = "0"

    Public _nameButton As String
    Public _tab As SuperTabItem
    Public _modulo As SideNavItem
    Dim mostrarStock As Boolean

    Private boShow As Boolean = False
    Private boAdd As Boolean = False
    Private boModif As Boolean = False
    Private boDel As Boolean = False

#End Region

#Region "Propiedades generales"

    Public Property titulo
        Get
            Return stTitulo
        End Get
        Set(value)
            stTitulo = value
        End Set
    End Property

    Public Property tipo
        Get
            Return inTipo
        End Get
        Set(value)
            inTipo = value
        End Set
    End Property

#End Region

#Region "Variables globales locales"
    Dim FtTitulo As Font = New Font("Arial", gi_fuenteTamano + 1)
    Dim FtNormal As Font = New Font("Arial", gi_fuenteTamano)

    Dim DtBusqueda As DataTable
    Dim DtDetalle As DataTable
    Dim DtDetalle1 As DataTable
    Dim InDuracion As Byte = 5

    Dim BoNuevo As Boolean = False
    Dim BoModificar As Boolean = False
    Dim BoEliminar As Boolean = False
    Dim BoNavegar As Boolean = False

#End Region

#Region "Eventos generales"

    Private Sub My_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        P_prInicio()
    End Sub

    Private Sub MBtNuevo_Click(sender As Object, e As EventArgs) Handles MBtNuevo.Click
        P_prNuevoRegistro()
    End Sub

    Private Sub MBtModificar_Click(sender As Object, e As EventArgs) Handles MBtModificar.Click
        'P_prModificarRegistro()
    End Sub

    Private Sub MBtEliminar_Click(sender As Object, e As EventArgs) Handles MBtEliminar.Click
        'P_prEliminarRegistro()
    End Sub

    Private Sub MBtGrabar_Click(sender As Object, e As EventArgs) Handles MBtGrabar.Click
        P_prGrabarRegistro()
    End Sub

    Private Sub MBtSalir_Click(sender As Object, e As EventArgs) Handles MBtSalir.Click
        P_prCancelarRegistro()
    End Sub

    Private Sub MBtPrimero_Click(sender As Object, e As EventArgs) Handles MBtPrimero.Click
        If (dgjBusqueda.RowCount > 0) Then
            dgjBusqueda.MoveFirst()
        End If
    End Sub

    Private Sub MBtAnterior_Click(sender As Object, e As EventArgs) Handles MBtAnterior.Click
        If (dgjBusqueda.RowCount > 0) Then
            dgjBusqueda.MovePrevious()
        End If
    End Sub

    Private Sub MBtSiguiente_Click(sender As Object, e As EventArgs) Handles MBtSiguiente.Click
        If (dgjBusqueda.RowCount > 0) Then
            dgjBusqueda.MoveNext()
        End If
    End Sub

    Private Sub MBtUltimo_Click(sender As Object, e As EventArgs) Handles MBtUltimo.Click
        If (dgjBusqueda.RowCount > 0) Then
            dgjBusqueda.MoveLast()
        End If
    End Sub

    Private Sub DgjBusqueda_SelectionChanged(sender As Object, e As EventArgs) Handles dgjBusqueda.SelectionChanged
        If (dgjBusqueda.Row > -1 And (Not BoNuevo And Not BoModificar)) Then
            P_prLlenarDatos(dgjBusqueda.Row)
        End If
    End Sub

    Private Sub dgjBusqueda_DoubleClick(sender As Object, e As EventArgs) Handles dgjBusqueda.DoubleClick
        If (dgjBusqueda.Row > -1) Then
            MSuperTabControlPrincipal.SelectedTabIndex = 0
        End If
    End Sub

    Private Sub dgjBusqueda_KeyDown(sender As Object, e As KeyEventArgs) Handles dgjBusqueda.KeyDown
        If (e.KeyData = Keys.Enter) Then
            MSuperTabControlPrincipal.SelectedTabIndex = 0
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dgjDetalle_EditingCell(sender As Object, e As EditingCellEventArgs) Handles dgjDetalle.EditingCell
        'If (e.Column.Key.Equals("iccant")) Then
        '    e.Cancel = False
        'Else
        '    e.Cancel = True
        'End If

        e.Cancel = True
    End Sub

    Private Sub dgjDetalle_KeyDown(sender As Object, e As KeyEventArgs) Handles dgjDetalle.KeyDown


        'If (BoNuevo Or BoModificar) Then
        '    If (e.KeyData = Keys.Control + Keys.Enter) Then
        '        Dim dt As DataTable
        '        dt = L_fnObtenerTabla("a.canumi as numi, a.cadesc as [desc]",
        '                              "TC001 a",
        '                              "a.caserie=" + IIf(tipo = 1, "1", "0") + " and caest=1 order by a.canumi asc")

        '        Dim listEstCeldas As New List(Of Modelo.MCelda)
        '        listEstCeldas.Add(New Modelo.MCelda("numi,", True, "Código", 80))
        '        listEstCeldas.Add(New Modelo.MCelda("desc", True, "Descripción", 270))

        '        Dim ef = New Efecto
        '        ef.tipo = 3
        '        ef.dt = dt
        '        ef.SeleclCol = 2
        '        ef.listEstCeldas = listEstCeldas
        '        ef.alto = 50
        '        ef.ancho = 350
        '        ef.Context = "Seleccione Producto".ToUpper
        '        ef.ShowDialog()
        '        ef.StartPosition = FormStartPosition.CenterScreen
        '        Dim bandera As Boolean = False
        '        bandera = ef.band
        '        If (bandera = True) Then
        '            Dim Row As Janus.Windows.GridEX.GridEXRow = ef.Row

        '            'dgjDetalle.SetValue("cprod", Row.Cells("numi").Value)
        '            'dgjDetalle.SetValue("ncprod", Row.Cells("desc").Value)
        '            dgjDetalle.Col = dgjDetalle.RootTable.Columns("iccant").Index
        '            DtDetalle.Rows(dgjDetalle.Row).Item("iccprod") = Row.Cells("numi").Value
        '            DtDetalle.Rows(dgjDetalle.Row).Item("ncprod") = Row.Cells("desc").Value
        '        End If
        '    ElseIf (e.KeyData = Keys.Enter And dgjDetalle.Col = dgjDetalle.RootTable.Columns("iccant").Index) Then
        '        Dim filIndex As Integer = dgjDetalle.Row
        '        If (filIndex = dgjDetalle.RowCount - 1) Then
        '            P_prAddFilaDetalle()

        '            Dim dt As DataTable
        '            dt = L_fnObtenerTabla("a.canumi as numi, a.cadesc as [desc]",
        '                                  "TC001 a",
        '                                  "a.caserie=" + IIf(tipo = 1, "1", "0") + " and caest=1 order by a.canumi asc")

        '            Dim listEstCeldas As New List(Of Modelo.MCelda)
        '            listEstCeldas.Add(New Modelo.MCelda("numi,", True, "Código", 80))
        '            listEstCeldas.Add(New Modelo.MCelda("desc", True, "Descripción", 270))

        '            Dim ef = New Efecto
        '            ef.tipo = 3
        '            ef.dt = dt
        '            ef.SeleclCol = 2
        '            ef.listEstCeldas = listEstCeldas
        '            ef.alto = 50
        '            ef.ancho = 350
        '            ef.Context = "Seleccione Producto".ToUpper
        '            ef.ShowDialog()
        '            ef.StartPosition = FormStartPosition.CenterScreen
        '            Dim bandera As Boolean = False
        '            bandera = ef.band
        '            If (bandera = True) Then
        '                Dim Row As Janus.Windows.GridEX.GridEXRow = ef.Row

        '                dgjDetalle.Row = filIndex + 1
        '                dgjDetalle.Col = dgjDetalle.RootTable.Columns("iccant").Index

        '                'dgjDetalle.SetValue("cprod", Row.Cells("numi").Value)
        '                'dgjDetalle.SetValue("ncprod", Row.Cells("desc").Value)
        '                DtDetalle.Rows(dgjDetalle.Row).Item("iccprod") = Row.Cells("numi").Value
        '                DtDetalle.Rows(dgjDetalle.Row).Item("ncprod") = Row.Cells("desc").Value
        '            End If

        '        End If
        '    End If
        'End If
    End Sub

    Private Sub dgjDetalle_Enter(sender As Object, e As EventArgs) Handles dgjDetalle.Enter
        dgjDetalle.Row = 0
        dgjDetalle.Col = dgjDetalle.RootTable.Columns("ihcodpro").Index
    End Sub

    Private Sub dgjDetalle_CellEdited(sender As Object, e As ColumnActionEventArgs) Handles dgjDetalle.CellEdited
        'If (BoModificar And e.Column.Key.Equals("iccant")) Then
        '    If (dgjDetalle.GetValue("icid") <> 0) Then
        '        dgjDetalle.SetValue("estado", 2)
        '    End If
        'End If
    End Sub

    Private Sub QuitarProductoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitarProductoToolStripMenuItem.Click
        Try
            dgjDetalle.CurrentRow.EndEdit()
            dgjDetalle.CurrentRow.Delete()
            dgjDetalle.Refetch()
            dgjDetalle.Refresh()
        Catch ex As Exception
            'sms
            'MsgBox(ex)
        End Try
    End Sub

#End Region

#Region "Metodos y funciones indispensables"

    Private Sub P_prInicio()
        'Abrir conexión
        If (Not gb_ConexionAbierta) Then
            L_prAbrirConexion()
        End If

        'Validar requisitos del programa
        If (Not P_fnValidarRequisitos() = String.Empty) Then
            Return
        End If

        'Inicializar componentes
        P_prInicializarComponentes()

        'Habilitar/Deshabilitar compotentes del formulario
        P_prHDComponentes(False)


        'Armar todo las grillas
        BoNavegar = False
        P_prArmarGrillas()
        BoNavegar = True

        P_prActualizarPaginacion(0)
        P_prLlenarDatos(0)
    End Sub

    Private Function P_fnValidarRequisitos() As String
        Dim sms As String = ""

        If (Not sms = String.Empty) Then
            ToastNotification.Show(Me,
                                   "".ToUpper,
                                   My.Resources.WARNING,
                                   InDuracion * 1000,
                                   eToastGlowColor.Red,
                                   eToastPosition.TopCenter)
        End If

        Return sms
    End Function

    Private Sub P_prInicializarComponentes()
        'Form
        Me.Text = titulo
        Me.WindowState = FormWindowState.Maximized

        'Tab
        MSuperTabControlPrincipal.SelectedTabIndex = 0

        'Visible
        MBtImprimir.Visible = False
        QuitarProductoToolStripMenuItem.Visible = False

        'Enabled
        MBtGrabar.Enabled = False

        'ReadOnly
        tbCodigo.ReadOnly = True

        'Grid Busqueda
        dgjBusqueda.AllowEdit = Janus.Data.InheritableBoolean.False
        dgjDetalle.ContextMenuStrip = CmDetalle

        'Botones

        'Funciones
        P_prCambiarFuenteComponentes()
        P_prAsignarPermisos()

        'Usuario del sistema
        MTbUsuario.Text = gs_user
    End Sub

    Private Sub P_prCambiarFuenteComponentes()
        Dim xCtrl As Control
        For Each xCtrl In TableLayoutPanelPrincipal.Controls
            Try
                xCtrl.Font = FtNormal
            Catch ex As Exception
            End Try
        Next
        'For Each xCtrl In TableLayoutPanelPrincipal.Controls
        '    Try
        '        xCtrl.Font = FtNormal
        '    Catch ex As Exception
        '    End Try
        'Next
    End Sub

    Private Sub P_prAsignarPermisos()
        Dim dtRolUsu() As DataRow = L_prRolDetalleGeneral(gi_userRol).Select("yaprog='" + _nameButton + "'")

        If (dtRolUsu.Count = 1) Then
            boShow = dtRolUsu(0).Item("ycshow")
            boAdd = dtRolUsu(0).Item("ycadd")
            boModif = dtRolUsu(0).Item("ycmod")
            boDel = dtRolUsu(0).Item("ycdel")
        End If

        If boAdd = False Then
            MBtNuevo.Visible = False
        End If
        If boModif = False Then
            MBtModificar.Visible = False
        End If
        If boDel = False Then
            MBtEliminar.Visible = False
        End If

    End Sub

    Private Sub P_prHDComponentes(ByVal flat As Boolean)
        'TextBox
        tbObs.ReadOnly = Not flat
        tbCodPack.ReadOnly = Not flat
        tbCodPack.Enabled = flat
        tbProdPack.ReadOnly = Not flat
        tbCantP.IsInputReadOnly = Not flat
        tbPcosto.ReadOnly = Not flat
        tbCantNP.IsInputReadOnly = Not flat

        'DateTimer
        dtiFechaDoc.IsInputReadOnly = Not flat
        dtiFechaDoc.ButtonDropDown.Enabled = flat
        dtiFechaDesarm.IsInputReadOnly = Not flat


        'Switch Button

        'Radio Button

        'Grillas
        dgjDetalle.AllowEdit = IIf(flat, 1, 2)
    End Sub

    Private Sub P_prLimpiar()
        'TextBox
        tbCodigo.Clear()
        tbObs.Clear()
        tbCodPack.Clear()
        tbProdPack.Clear()
        tbCantP.Value = 0
        tbPcosto.Clear()
        tbPcosto.Enabled = False
        tbCantNP.Value = 0

        'DateTimer
        dtiFechaDoc.Value = Now.Date
        dtiFechaDesarm.Text = ""

        'Switch Button

        'Radio Button

        'Grillas
        P_prArmarGrillaDetalle("-1")

        MBtGrabar.Tooltip = "GRABAR"

        'Ocultar el GroupPanel Desarmado
        GroupPanelDesarmado.Visible = False

    End Sub

    Private Sub P_prArmarGrillas()
        P_prArmarGrillaBusqueda()
        P_prArmarGrillaDetalle("-1")
        P_prArmarGrillaDetalleDesarmado("-1")
    End Sub

    Private Sub P_prActualizarPaginacion(ByVal index As Integer)
        MLbPaginacion.Text = "Reg. " & index + 1 & " de " & dgjBusqueda.GetRows.Count
    End Sub

    Private Sub P_prMoverIndexActual()
        Dim index As Integer = CInt(MLbPaginacion.Text.Trim.Split(" ")(1).Trim)
        If (index < 0) Then
            index = 0
        End If
        If (index > dgjBusqueda.RowCount) Then
            index = dgjBusqueda.RowCount
        End If
        Try
            dgjBusqueda.MoveTo(index - 1)
            P_prLlenarDatos(dgjBusqueda.Row)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub P_prLlenarDatos(ByVal index As Integer)
        If (index <= dgjBusqueda.GetRows.Count - 1 And index >= 0) Then
            If (BoNavegar) Then
                With dgjBusqueda
                    Me.tbCodigo.Text = .GetValue("id").ToString
                    Me.dtiFechaDoc.Value = .GetValue("fdoc")
                    Me.tbObs.Text = .GetValue("obs").ToString
                    Me.tbCodPack.Text = .GetValue("codpack").ToString
                    Me.tbProdPack.Text = .GetValue("cadesc").ToString
                    Me.tbCantP.Value = .GetValue("cantP")
                    Me.tbPcosto.Text = .GetValue("pcosto").ToString
                    Me.tbCantNP.Value = .GetValue("cantNP")


                    stEst = .GetValue("est").ToString
                    stAlm = .GetValue("alm").ToString
                    'stIddc = .GetValue("iddc").ToString

                    'Aqui se coloca los datos de la grilla de los Equipos
                    P_prArmarGrillaDetalle(tbCodigo.Text)

                    If tbCantNP.Value = 0 Then
                        tbCantNP.Value = 0
                        GroupPanelDesarmado.Visible = False
                        dtiFechaDesarm.Text = ""
                        btnGrabarDesarm.Enabled = True
                    Else
                        GroupPanelDesarmado.Visible = True
                        tbCantNP.Value = .GetValue("cantNP")
                        dtiFechaDesarm.Value = .GetValue("fechaNP")
                        P_prArmarGrillaDetalleDesarmado(tbCodigo.Text)
                        btnGrabarDesarm.Enabled = False
                    End If

                    MLbFecha.Text = CType(.GetValue("fact").ToString, Date).ToString("dd/MM/yyyy")
                    MLbHora.Text = .GetValue("hact").ToString
                    MLbUsuario.Text = .GetValue("uact").ToString
                End With

                P_prActualizarPaginacion(dgjBusqueda.Row)

                If (Not boModif And boAdd) Then
                    If (Now.Date = Me.dtiFechaDoc.Value) Then
                        MBtModificar.Visible = True
                    Else
                        MBtModificar.Visible = False
                    End If
                End If
            End If
        Else
            If (dgjBusqueda.GetRows.Count > 0) Then
                P_prMoverIndexActual()
            End If
        End If
    End Sub

    Private Sub P_prNuevoRegistro()
        P_prLimpiar()
        P_prEstadoNueModEli(1)
        P_prHDComponentes(BoNuevo)
        'dtiFechaDoc.Select()
        tbCodPack.Select()
        P_prAddFilaDetalle()
    End Sub

    Private Sub P_prModificarRegistro()
        P_prEstadoNueModEli(2)
        P_prHDComponentes(BoModificar)
        dtiFechaDoc.Select()
        P_prAddFilaDetalle()
    End Sub

    Private Sub P_prEliminarRegistro()
        Dim numi As String = tbCodigo.Text 'Valor del código único
        Dim info As New TaskDialogInfo("¿esta seguro de eliminar el registro?".ToUpper,
                                       eTaskDialogIcon.Delete, "advertencia".ToUpper,
                                       "Esta a punto de eliminar el movimiento con".ToUpper _
                                       + vbCrLf + "código -> ".ToUpper _
                                       + numi + " " + vbCrLf + "Desea continuar?".ToUpper,
                                       eTaskDialogButton.Yes Or eTaskDialogButton.Cancel,
                                       eTaskDialogBackgroundColor.Blue)
        Dim result As eTaskDialogResult = TaskDialog.Show(info)
        If result = eTaskDialogResult.Yes Then
            Dim mensajeError As String = ""
            Dim resEliminar As Boolean = L_fnbValidarEliminacion(numi, "TI005", "ibid", mensajeError)
            If (resEliminar) Then
                Dim res As Boolean = L_fnMovimientoPackBorrar(numi) 'Medoto de lógica para eliminar
                If (res) Then
                    ToastNotification.Show(Me, "Codigo de movimiento: ".ToUpper + numi + " eliminado con Exito.".ToUpper,
                                           My.Resources.GRABACION_EXITOSA, InDuracion * 1000,
                                           eToastGlowColor.Green,
                                           eToastPosition.TopCenter)
                    BoNavegar = False
                    P_prArmarGrillaBusqueda()
                    BoNavegar = True
                    P_prMoverIndexActual()
                Else
                    ToastNotification.Show(Me, "No se pudo eliminar el movimiento con código: ".ToUpper + numi + ", intente nuevamente.".ToUpper,
                                           My.Resources.WARNING, InDuracion * 1000,
                                           eToastGlowColor.Red,
                                           eToastPosition.TopCenter)
                End If
            Else
                ToastNotification.Show(Me, mensajeError.ToUpper,
                                           My.Resources.I64x64_error, InDuracion * 1000,
                                           eToastGlowColor.Red,
                                           eToastPosition.TopCenter)
            End If
        End If
    End Sub

    Private Sub P_prGrabarRegistro()
        Dim id As String
        Dim fdoc As String
        Dim obs As String
        Dim codpack As String
        Dim cantP As Integer
        Dim pcosto As String
        Dim cantNP As Integer
        Dim est As String
        Dim alm As String

        dgjDetalle.Refetch()

        If (BoNuevo) Then
            If (P_fnValidarGrabacion()) Then

                id = ""
                fdoc = dtiFechaDoc.Value.ToString("yyyy/MM/dd")
                obs = tbObs.Text.Trim
                codpack = tbCodPack.Text
                cantP = tbCantP.Value
                pcosto = tbPcosto.Text
                cantNP = tbCantNP.Value
                est = "1"
                alm = stAlm

                'dtiFechaDoc.Select()


                Dim dt As New DataTable
                dt = CType(dgjDetalle.DataSource, DataTable).DefaultView.ToTable(False, "ihid", "ihigid", "ihcodpro", "cadesc", "ihcant", "ihtotcant", "ihpcosto", "stock", "estado")

                'Grabar
                Dim res As Boolean = L_fnMovimientoPackGrabar(id, fdoc, obs, codpack, cantP, pcosto, cantNP, est, alm, dt)

                If (res) Then
                    P_prLimpiar()
                    BoNavegar = False
                    P_prArmarGrillaBusqueda()
                    tbCodPack.Select()
                    BoNavegar = True

                    ToastNotification.Show(Me, "Codigo de movimiento Pack ".ToUpper + tbCodigo.Text + " Grabado con éxito.".ToUpper,
                                       My.Resources.GRABACION_EXITOSA,
                                       InDuracion * 1000,
                                       eToastGlowColor.Green,
                                       eToastPosition.TopCenter)
                Else
                    ToastNotification.Show(Me, "No se pudo grabar el codigo de movimiento Pack".ToUpper + tbCodigo.Text + ", intente nuevamente.".ToUpper,
                                       My.Resources.WARNING,
                                       InDuracion * 1000,
                                       eToastGlowColor.Red,
                                       eToastPosition.TopCenter)
                End If
            End If
        ElseIf (BoModificar) Then
            If (P_fnValidarGrabacion()) Then
                id = tbCodigo.Text.Trim
                fdoc = dtiFechaDoc.Value.ToString("yyyy/MM/dd")
                obs = tbObs.Text.Trim
                codpack = tbCodPack.Text
                cantP = tbCantP.Value
                pcosto = tbPcosto.Text
                cantNP = tbCantNP.Value
                est = "1"
                alm = stAlm


                dtiFechaDoc.Select()

                Dim dt As New DataTable
                dt = CType(dgjDetalle.DataSource, DataTable).DefaultView.ToTable(False, "ihid", "ihigid", "ihcodpro", "cadesc", "ihcant", "ihtotcant", "ihpcosto", "stock", "estado")

                'Grabar
                Dim res As Boolean = L_fnMovimientoPackModificar(id, fdoc, obs, codpack, cantP, pcosto, cantNP, est, alm, dt)

                If (res) Then

                    BoNavegar = False
                    P_prArmarGrillaBusqueda()
                    BoNavegar = True

                    P_prMoverIndexActual()

                    tbCodPack.Select()
                    'dtiFechaDoc.Select()
                    MBtSalir.PerformClick()

                    ToastNotification.Show(Me, "Codigo de cliente ".ToUpper + tbCodigo.Text + " Modificado con Exito.".ToUpper,
                                       My.Resources.GRABACION_EXITOSA,
                                       InDuracion * 1000,
                                       eToastGlowColor.Green,
                                       eToastPosition.TopCenter)
                Else
                    ToastNotification.Show(Me, "No se pudo modificar el codigo de cliente ".ToUpper + tbCodigo.Text + ", intente nuevamente.".ToUpper,
                                       My.Resources.WARNING,
                                       InDuracion * 1000,
                                       eToastGlowColor.Red,
                                       eToastPosition.TopCenter)
                End If
            End If
        End If
    End Sub

    Private Sub P_prCancelarRegistro()
        If (Not MBtGrabar.Enabled) Then
            Me.Close()
            _modulo.Select()
            _tab.Close()
        Else
            P_prLimpiar()
            P_prHDComponentes(False)
            P_prLlenarDatos(dgjBusqueda.Row)
        End If
        P_prEstadoNueModEli(4)
    End Sub

    Private Sub P_prEstadoNueModEli(value As Integer)
        BoNuevo = (value = 1)
        BoModificar = (value = 2)
        BoEliminar = (value = 3)

        MBtNuevo.Enabled = (value = 4)
        MBtModificar.Enabled = (value = 4)
        MBtEliminar.Enabled = (value = 4)
        MBtGrabar.Enabled = Not (value = 4)

        If (value = 4) Then
            MBtSalir.Tooltip = "SALIR"
            MBtSalir.Text = "SALIR"
        Else
            MBtSalir.Tooltip = "CANCELAR"
            MBtSalir.Text = "CANCELAR"
        End If

        MBtPrimero.Enabled = (value = 4)
        MBtAnterior.Enabled = (value = 4)
        MBtSiguiente.Enabled = (value = 4)
        MBtUltimo.Enabled = (value = 4)
        MSuperTabItemBusqueda.Visible = (value = 4)
        QuitarProductoToolStripMenuItem.Visible = Not (value = 4)

        MBtGrabar.Tooltip = IIf(value = 1, "GRABAR NUEVO REGISTRO", IIf(value = 2, "GRABAR MODIFICACIÓN DE REGISTRO", "GRABAR"))
        MRlAccion.Text = IIf(value = 1, "NUEVO", IIf(value = 2, "MODIFICAR", ""))

        'Compentes

        If (MSuperTabControlPrincipal.SelectedTabIndex = 1) Then
            MSuperTabControlPrincipal.SelectedTabIndex = 0
        End If

    End Sub

    Private Function P_fnObtenerID() As String
        Dim res As String = ""
        res = res + Now.Hour.ToString("00") + Now.Minute.ToString("00") + Now.Second.ToString("00") + "_" _
            + Now.Day.ToString("00") + Now.Month.ToString("00") + Now.Year.ToString("0000")
        Return res
    End Function

#End Region

#Region "Metodos y funciones generales"

    Private Sub P_prArmarGrillaBusqueda()
        DtBusqueda = New DataTable
        DtBusqueda = L_fnMovimientoPackGeneral()

        dgjBusqueda.BoundMode = Janus.Data.BoundMode.Bound
        dgjBusqueda.DataSource = DtBusqueda
        dgjBusqueda.RetrieveStructure()

        'dar formato a las columnas
        With dgjBusqueda.RootTable.Columns("id")
            .Caption = "Código"
            .Width = 80
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            '.CellStyle.BackColor = Color.AliceBlue
        End With

        With dgjBusqueda.RootTable.Columns("fdoc")
            .Caption = "Fecha"
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
            '.CellStyle.BackColor = Color.AliceBlue
        End With
        With dgjBusqueda.RootTable.Columns("obs")
            .Caption = "Observación"
            .Width = 200
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
            .Position = 2
            '.CellStyle.BackColor = Color.AliceBlue
        End With

        With dgjBusqueda.RootTable.Columns("codpack")
            .Caption = "Cod. Pack"
            .Width = 80
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
        End With
        With dgjBusqueda.RootTable.Columns("cadesc")
            .Caption = "Pack"
            .Width = 350
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
        End With
        With dgjBusqueda.RootTable.Columns("cantP")
            .Visible = False
        End With

        With dgjBusqueda.RootTable.Columns("pcosto")
            .Visible = False
        End With

        With dgjBusqueda.RootTable.Columns("cantNP")
            .Visible = False
        End With
        With dgjBusqueda.RootTable.Columns("fechaNP")
            .Visible = False
        End With
        With dgjBusqueda.RootTable.Columns("est")
            .Visible = False
        End With

        With dgjBusqueda.RootTable.Columns("alm")
            .Visible = False
        End With

        With dgjBusqueda.RootTable.Columns("fact")
            .Visible = False
        End With

        With dgjBusqueda.RootTable.Columns("hact")
            .Visible = False
        End With

        With dgjBusqueda.RootTable.Columns("uact")
            .Visible = False
        End With
        'Habilitar Filtradores
        With dgjBusqueda
            .GroupByBoxVisible = False
            '.FilterRowFormatStyle.BackColor = Color.Blue
            .DefaultFilterRowComparison = FilterConditionOperator.Contains
            .FilterMode = FilterMode.Automatic
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges
            'Diseño de la tabla
            .VisualStyle = VisualStyle.Office2007
            .SelectionMode = SelectionMode.MultipleSelection
            .AlternatingColors = True
            .RecordNavigator = True
        End With
    End Sub

    Private Sub P_prArmarGrillaDetalle(numi As String)
        DtDetalle = New DataTable
        DtDetalle = L_fnMovimientoPackDetalle(numi)

        dgjDetalle.BoundMode = Janus.Data.BoundMode.Bound
        dgjDetalle.DataSource = DtDetalle
        dgjDetalle.RetrieveStructure()

        'dar formato a las columnas
        With dgjDetalle.RootTable.Columns("ihid")
            .Visible = False
        End With
        With dgjDetalle.RootTable.Columns("ihigid")
            .Visible = False
        End With
        With dgjDetalle.RootTable.Columns("ihcodpro")
            .Caption = "Cód Prod."
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            '.CellStyle.BackColor = Color.AliceBlue
        End With

        With dgjDetalle.RootTable.Columns("cadesc")
            .Caption = "Descripción"
            .Width = 200
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
            '.CellStyle.BackColor = Color.AliceBlue
        End With

        With dgjDetalle.RootTable.Columns("ihcant")
            .Caption = "Cantidad"
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
        End With

        With dgjDetalle.RootTable.Columns("ihtotcant")
            .Caption = "Total Cant."
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
        End With
        With dgjDetalle.RootTable.Columns("ihpcosto")
            .Caption = "P. Costo Un."
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .FormatString = "0.00"
        End With

        With dgjDetalle.RootTable.Columns("stock")
            .Caption = "Stock"
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = False
        End With

        With dgjDetalle.RootTable.Columns("estado")
            .Visible = False
            '.CellStyle.BackColor = Color.AliceBlue
        End With

        'Habilitar Filtradores
        With dgjDetalle
            .GroupByBoxVisible = False
            .DefaultFilterRowComparison = FilterConditionOperator.Contains
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges

            'Diseño de la tabla
            .VisualStyle = VisualStyle.Office2007
            .SelectionMode = SelectionMode.MultipleSelection
            .AlternatingColors = True
            '.RecordNavigator = True
        End With
    End Sub

    Private Sub P_prArmarGrillaDetalleDesarmado(numi As String)
        DtDetalle1 = New DataTable
        DtDetalle1 = L_fnMovimientoPackDetalleTI0052(numi)

        dgjDesArmPack.BoundMode = Janus.Data.BoundMode.Bound
        dgjDesArmPack.DataSource = DtDetalle1
        dgjDesArmPack.RetrieveStructure()

        'dar formato a las columnas
        With dgjDesArmPack.RootTable.Columns("iiid")
            .Visible = False
        End With
        With dgjDesArmPack.RootTable.Columns("iiigid")
            .Visible = False
        End With
        With dgjDesArmPack.RootTable.Columns("iicodpro")
            .Caption = "Cód Prod."
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            '.CellStyle.BackColor = Color.AliceBlue
        End With

        With dgjDesArmPack.RootTable.Columns("cadesc")
            .Caption = "Descripción"
            .Width = 200
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near
            .Visible = True
            '.CellStyle.BackColor = Color.AliceBlue
        End With

        With dgjDesArmPack.RootTable.Columns("iicant")
            .Caption = "Cantidad"
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
        End With

        With dgjDesArmPack.RootTable.Columns("iitotcant")
            .Caption = "Total Cant."
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
        End With
        With dgjDesArmPack.RootTable.Columns("iipcosto")
            .Caption = "P. Costo Un."
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
            .FormatString = "0.00"
        End With

        With dgjDesArmPack.RootTable.Columns("stock")
            .Caption = "Stock"
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = False
        End With


        With dgjDesArmPack.RootTable.Columns("estado")
            .Visible = False
            '.CellStyle.BackColor = Color.AliceBlue
        End With

        'Habilitar Filtradores
        With dgjDesArmPack
            .GroupByBoxVisible = False
            .DefaultFilterRowComparison = FilterConditionOperator.Contains
            .FilterRowUpdateMode = FilterRowUpdateMode.WhenValueChanges

            'Diseño de la tabla
            .VisualStyle = VisualStyle.Office2007
            .SelectionMode = SelectionMode.MultipleSelection
            .AlternatingColors = True
            '.RecordNavigator = True
        End With
    End Sub

    Private Function P_fnValidarGrabacion() As Boolean
        Dim res As Boolean = True
        MEP.Clear()

        If (tbCodPack.Text = String.Empty) Then
            tbCodPack.BackColor = Color.Red
            MEP.SetError(tbCodPack, "ingrese Código del producto Pack!".ToUpper)
            res = False
        Else
            tbCodPack.BackColor = Color.White
            MEP.SetError(tbCodPack, "")
        End If

        If (tbCantP.Value <= 0) Then
            tbCantP.BackColor = Color.Red
            MEP.SetError(tbCantP, "La cantidad debe ser mayor a cero!".ToUpper)
            res = False
        Else
            tbCantP.BackColor = Color.White
            MEP.SetError(tbCantP, "")
        End If
        Return res
    End Function
    Private Function P_fnValidarGrabacionDesarmado() As Boolean
        Dim res As Boolean = True
        'MEP.Clear()

        If (Not IsNumeric(tbCantNP.Value) < 0) Then
            tbCantNP.BackColor = Color.Red
            MEP.SetError(tbCantNP, "La cantidad debe ser mayor a cero!".ToUpper)
            res = False
        Else
            tbCantNP.BackColor = Color.White
            MEP.SetError(tbCantNP, "")
        End If
        Return res
    End Function

    Private Sub P_prAddFilaDetalle()
        Dim fil As DataRow
        fil = DtDetalle.NewRow
        fil.Item("ihid") = 0
        fil.Item("ihigid") = 0
        fil.Item("ihcodpro") = 0
        fil.Item("cadesc") = "Nuevo"
        fil.Item("ihcant") = 0
        fil.Item("ihtotcant") = 0
        fil.Item("ihpcosto") = 0
        fil.Item("stock") = 0
        fil.Item("estado") = 0
        DtDetalle.Rows.Add(fil)
    End Sub
    Private Sub P_prAddFilaDetalleDesarmado()
        Dim fil As DataRow
        fil = DtDetalle1.NewRow
        fil.Item("iiid") = 0
        fil.Item("iiigid") = 0
        fil.Item("iicodpro") = 0
        fil.Item("cadesc") = "Nuevo"
        fil.Item("iicant") = 0
        fil.Item("iitotcant") = 0
        fil.Item("iipcosto") = 0
        fil.Item("stock") = 0
        fil.Item("estado") = 0
        DtDetalle1.Rows.Add(fil)
    End Sub


    Private Sub tbCodPack_KeyDown(sender As Object, e As KeyEventArgs) Handles tbCodPack.KeyDown
        If (e.KeyData = Keys.Control + Keys.Enter) Then
            Dim dt As DataTable
            dt = L_fnObtenerTabla("a.canumi as numi, a.cadesc as [desc]",
                                      "TC001 a",
                                      "a.caserie='False' and caest=1 and a.capack=1 order by a.canumi asc")

            Dim listEstCeldas As New List(Of Modelo.MCelda)
            listEstCeldas.Add(New Modelo.MCelda("numi,", True, "Código", 80))
            listEstCeldas.Add(New Modelo.MCelda("desc", True, "Descripción", 270))

            Dim ef = New Efecto
            ef.tipo = 3
            ef.dt = dt
            ef.SeleclCol = 2
            ef.listEstCeldas = listEstCeldas
            ef.alto = 450
            ef.ancho = 150
            ef.Context = "Seleccione Producto".ToUpper
            ef.Height = 361
            ef.ShowDialog()

            'ef.StartPosition = FormStartPosition.CenterParent
            Dim bandera As Boolean = False
            bandera = ef.band
            If (bandera = True) Then
                Dim Row As Janus.Windows.GridEX.GridEXRow = ef.Row

                tbCodPack.Text = Row.Cells("numi").Value
                tbProdPack.Text = Row.Cells("desc").Value
                _prCargarProductoPack(Row.Cells("numi").Value)
            End If
        End If
    End Sub
    Sub _prCargarProductoPack(numi As Integer)
        Dim dt As DataTable = L_fnDetallePack(numi)
        Dim pcostotot As Decimal = 0
        With dgjDetalle.RootTable.Columns("stock")
            .Caption = "Stock"
            .Width = 100
            .HeaderStyle.Font = FtTitulo
            .HeaderAlignment = Janus.Windows.GridEX.TextAlignment.Center
            .CellStyle.Font = FtNormal
            .CellStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far
            .Visible = True
        End With
        If (dt.Rows.Count > 0) Then
            CType(dgjDetalle.DataSource, DataTable).Rows.Clear()
            For i As Integer = 0 To dt.Rows.Count - 1

                P_prAddFilaDetalle()
                dgjDetalle.Row = dgjDetalle.RowCount - 1

                dgjDetalle.SetValue("ihcodpro", dt.Rows(i).Item("cbtccanumi1"))
                dgjDetalle.SetValue("cadesc", dt.Rows(i).Item("cadesc"))
                dgjDetalle.SetValue("ihcant", dt.Rows(i).Item("cbcant"))
                dgjDetalle.SetValue("ihpcosto", dt.Rows(i).Item("chprecio"))
                dgjDetalle.SetValue("stock", dt.Rows(i).Item("iacant"))
                pcostotot = pcostotot + (dgjDetalle.GetValue("ihcant") * dgjDetalle.GetValue("ihpcosto"))
            Next
            CType(dgjDetalle.DataSource, DataTable).AcceptChanges()
            'Dim pcostotot As Decimal = (dgjDetalle.GetValue("ihcant") * dgjDetalle.GetValue("ihpcosto"))
            tbPcosto.Text = pcostotot
            'tbPcosto.Text = dgjDetalle.GetTotal(dgjDetalle.RootTable.Columns("ihpcosto"), AggregateFunction.Sum)

            'dgjDetalle.Select()
            tbCantP.Select()

        End If
    End Sub

    Private Sub tbCantP_ValueChanged(sender As Object, e As EventArgs) Handles tbCantP.ValueChanged
        Dim dt As DataTable = CType(dgjDetalle.DataSource, DataTable)
        If (tbCantP.Focused) Then
            If (dt.Rows.Count > 0) Then
                For i As Integer = 0 To dt.Rows.Count - 1

                    dgjDetalle.Row = i
                    'dgjDetalle.SetValue("ihtotcant", dgjDetalle.GetValue("ihcant") * tbCantArm.Value)
                    dgjDetalle.SetValue("ihtotcant", dt.Rows(i).Item("ihcant") * tbCantP.Value)
                    If (dgjDetalle.GetValue("ihtotcant") > dt.Rows(i).Item("stock")) Then

                        Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
                        MessageBox.Show(Me, "La cantidad para armar el pack no debe ser mayor al del stock disponible, Producto: " + (dgjDetalle.GetValue("cadesc")) & vbCrLf &
                        "Stock=" + Str(dgjDetalle.GetValue("stock")).ToUpper, "AVISO!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        tbCantP.Value = 0

                    End If
                Next

            End If
            dgjDetalle.UpdateData()
        End If
    End Sub

    Private Sub btnDesarmarPack_Click(sender As Object, e As EventArgs) Handles btnDesarmarPack.Click
        Dim dt As DataTable
        If tbCantNP.Value > 0 Then
            Dim img As Bitmap = New Bitmap(My.Resources.Mensaje, 50, 50)
            ToastNotification.Show(Me, "Usted ya desarmó el Pack, no puede volver a desarmar.".ToUpper, img, 6000, eToastGlowColor.Red, eToastPosition.TopCenter)
        Else
            GroupPanelDesarmado.Visible = True
            tbCantNP.IsInputReadOnly = False
            tbCantNP.Select()
            dtiFechaDesarm.Visible = True
            dtiFechaDesarm.Value = Now.Date
            P_prArmarGrillaDetalleDesarmado(-1)
            dt = dgjDetalle.DataSource

            If (dt.Rows.Count > 0) Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    P_prAddFilaDetalleDesarmado()
                    dgjDesArmPack.Row = dgjDesArmPack.RowCount - 1

                    dgjDesArmPack.SetValue("iiigid", dt.Rows(i).Item("ihigid"))
                    dgjDesArmPack.SetValue("iicodpro", dt.Rows(i).Item("ihcodpro"))
                    dgjDesArmPack.SetValue("cadesc", dt.Rows(i).Item("cadesc"))
                    dgjDesArmPack.SetValue("iicant", dt.Rows(i).Item("ihcant"))
                    dgjDesArmPack.SetValue("iipcosto", dt.Rows(i).Item("ihpcosto"))
                    'dgjDesArmPack.SetValue("stock", dt.Rows(i).Item("stock"))
                Next
                CType(dgjDesArmPack.DataSource, DataTable).AcceptChanges()

            End If
        End If
    End Sub

    Private Sub tbCantNP_ValueChanged(sender As Object, e As EventArgs) Handles tbCantNP.ValueChanged
        Dim dt As DataTable = CType(dgjDesArmPack.DataSource, DataTable)
        Dim dt1 As DataTable = L_fnVerificarStockPack(tbCodPack.Text)
        If (tbCantNP.Focused) Then

            If (dt1.Rows.Count > 0) Then
                If (tbCantNP.Value > dt1.Rows(0).Item("iacant")) Then
                    Dim stock1 As Double = dt1.Rows(0).Item("iacant")
                    ToastNotification.Show(Me, "La cantidad no puede ser mayor al stock actual del pack. Stock Pack:".ToUpper + stock1.ToString,
                                           My.Resources.WARNING,
                                           InDuracion * 1100,
                                           eToastGlowColor.Red,
                                           eToastPosition.TopCenter)
                    tbCantNP.Value = 0
                Else
                    If (dt.Rows.Count > 0) Then
                        For i As Integer = 0 To dt.Rows.Count - 1
                            dgjDesArmPack.Row = i
                            dgjDesArmPack.SetValue("iitotcant", dt.Rows(i).Item("iicant") * tbCantNP.Value)
                        Next

                    End If
                    dgjDesArmPack.UpdateData()

                End If
            End If
        End If
    End Sub

    Private Sub btnGrabarDesarm_Click(sender As Object, e As EventArgs) Handles btnGrabarDesarm.Click

        If (P_fnValidarGrabacionDesarmado()) Then
            Dim dt As New DataTable
            dt = CType(dgjDesArmPack.DataSource, DataTable).DefaultView.ToTable(False, "iiid", "iiigid", "iicodpro", "cadesc", "iicant", "iitotcant", "iipcosto", "stock", "estado")

            'Grabar
            Dim res As Boolean = L_fnGrabarDesarmado(tbCodigo.Text, tbCodPack.Text, tbCantNP.Value, dtiFechaDesarm.Value, dt)

            If (res) Then
                P_prLimpiar()
                BoNavegar = False
                P_prArmarGrillaBusqueda()
                tbCodPack.Select()
                'BoNavegar = True

                ToastNotification.Show(Me, "Desarmado de Pack ".ToUpper + tbCodigo.Text + " Grabado con éxito.".ToUpper,
                                   My.Resources.GRABACION_EXITOSA,
                                   InDuracion * 1000,
                                   eToastGlowColor.Green,
                                   eToastPosition.TopCenter)
            Else
                ToastNotification.Show(Me, "No se pudo grabar el desarmado del Pack".ToUpper + tbCodigo.Text + ", intente nuevamente.".ToUpper,
                                   My.Resources.WARNING,
                                   InDuracion * 1000,
                                   eToastGlowColor.Red,
                                   eToastPosition.TopCenter)
            End If
        End If
    End Sub

    Private Sub dgjDesArmPack_EditingCell(sender As Object, e As EditingCellEventArgs) Handles dgjDesArmPack.EditingCell
        e.Cancel = True
    End Sub

#End Region

End Class