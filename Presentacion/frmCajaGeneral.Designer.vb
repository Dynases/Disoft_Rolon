<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCajaGeneral
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PanelSuperior = New System.Windows.Forms.Panel()
        Me.btGenerar = New DevComponents.DotNetBar.ButtonX()
        Me.LabelX2 = New DevComponents.DotNetBar.LabelX()
        Me.TB_FechaHasta = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.LabelX1 = New DevComponents.DotNetBar.LabelX()
        Me.Tb_FechaDesde = New DevComponents.Editors.DateTimeAdv.DateTimeInput()
        Me.bt_Imprimir = New DevComponents.DotNetBar.ButtonX()
        Me.Dgv_Caja = New Janus.Windows.GridEX.GridEX()
        Me.PanelSuperior.SuspendLayout()
        CType(Me.TB_FechaHasta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tb_FechaDesde, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Dgv_Caja, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelSuperior
        '
        Me.PanelSuperior.Controls.Add(Me.btGenerar)
        Me.PanelSuperior.Controls.Add(Me.LabelX2)
        Me.PanelSuperior.Controls.Add(Me.TB_FechaHasta)
        Me.PanelSuperior.Controls.Add(Me.LabelX1)
        Me.PanelSuperior.Controls.Add(Me.Tb_FechaDesde)
        Me.PanelSuperior.Controls.Add(Me.bt_Imprimir)
        Me.PanelSuperior.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelSuperior.Location = New System.Drawing.Point(0, 0)
        Me.PanelSuperior.Name = "PanelSuperior"
        Me.PanelSuperior.Size = New System.Drawing.Size(922, 60)
        Me.PanelSuperior.TabIndex = 1
        '
        'btGenerar
        '
        Me.btGenerar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btGenerar.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btGenerar.Dock = System.Windows.Forms.DockStyle.Right
        Me.btGenerar.Image = Global.Presentacion.My.Resources.Resources.REPORTE2
        Me.btGenerar.ImageFixedSize = New System.Drawing.Size(30, 30)
        Me.btGenerar.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btGenerar.Location = New System.Drawing.Point(772, 0)
        Me.btGenerar.Name = "btGenerar"
        Me.btGenerar.Size = New System.Drawing.Size(75, 60)
        Me.btGenerar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btGenerar.TabIndex = 236
        Me.btGenerar.Text = "Generar"
        '
        'LabelX2
        '
        '
        '
        '
        Me.LabelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX2.Location = New System.Drawing.Point(219, 21)
        Me.LabelX2.Name = "LabelX2"
        Me.LabelX2.Size = New System.Drawing.Size(64, 23)
        Me.LabelX2.TabIndex = 235
        Me.LabelX2.Text = "Fecha hasta:"
        '
        'TB_FechaHasta
        '
        '
        '
        '
        Me.TB_FechaHasta.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.TB_FechaHasta.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.TB_FechaHasta.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.TB_FechaHasta.ButtonDropDown.Visible = True
        Me.TB_FechaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TB_FechaHasta.IsPopupCalendarOpen = False
        Me.TB_FechaHasta.Location = New System.Drawing.Point(291, 20)
        '
        '
        '
        '
        '
        '
        Me.TB_FechaHasta.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.TB_FechaHasta.MonthCalendar.CalendarDimensions = New System.Drawing.Size(1, 1)
        Me.TB_FechaHasta.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.TB_FechaHasta.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.TB_FechaHasta.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.TB_FechaHasta.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.TB_FechaHasta.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.TB_FechaHasta.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.TB_FechaHasta.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.TB_FechaHasta.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.TB_FechaHasta.MonthCalendar.DisplayMonth = New Date(2017, 2, 1, 0, 0, 0, 0)
        Me.TB_FechaHasta.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        '
        '
        '
        Me.TB_FechaHasta.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.TB_FechaHasta.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.TB_FechaHasta.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.TB_FechaHasta.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.TB_FechaHasta.MonthCalendar.TodayButtonVisible = True
        Me.TB_FechaHasta.Name = "TB_FechaHasta"
        Me.TB_FechaHasta.Size = New System.Drawing.Size(120, 22)
        Me.TB_FechaHasta.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.TB_FechaHasta.TabIndex = 234
        '
        'LabelX1
        '
        '
        '
        '
        Me.LabelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LabelX1.Location = New System.Drawing.Point(13, 21)
        Me.LabelX1.Name = "LabelX1"
        Me.LabelX1.Size = New System.Drawing.Size(74, 23)
        Me.LabelX1.TabIndex = 233
        Me.LabelX1.Text = "Fecha Desde:"
        '
        'Tb_FechaDesde
        '
        '
        '
        '
        Me.Tb_FechaDesde.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.Tb_FechaDesde.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Tb_FechaDesde.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown
        Me.Tb_FechaDesde.ButtonDropDown.Visible = True
        Me.Tb_FechaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tb_FechaDesde.IsPopupCalendarOpen = False
        Me.Tb_FechaDesde.Location = New System.Drawing.Point(90, 20)
        '
        '
        '
        '
        '
        '
        Me.Tb_FechaDesde.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Tb_FechaDesde.MonthCalendar.CalendarDimensions = New System.Drawing.Size(1, 1)
        Me.Tb_FechaDesde.MonthCalendar.ClearButtonVisible = True
        '
        '
        '
        Me.Tb_FechaDesde.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
        Me.Tb_FechaDesde.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90
        Me.Tb_FechaDesde.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.Tb_FechaDesde.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.Tb_FechaDesde.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder
        Me.Tb_FechaDesde.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1
        Me.Tb_FechaDesde.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Tb_FechaDesde.MonthCalendar.DisplayMonth = New Date(2017, 2, 1, 0, 0, 0, 0)
        Me.Tb_FechaDesde.MonthCalendar.FirstDayOfWeek = System.DayOfWeek.Monday
        '
        '
        '
        Me.Tb_FechaDesde.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.Tb_FechaDesde.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90
        Me.Tb_FechaDesde.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.Tb_FechaDesde.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Tb_FechaDesde.MonthCalendar.TodayButtonVisible = True
        Me.Tb_FechaDesde.Name = "Tb_FechaDesde"
        Me.Tb_FechaDesde.Size = New System.Drawing.Size(120, 22)
        Me.Tb_FechaDesde.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.Tb_FechaDesde.TabIndex = 232
        '
        'bt_Imprimir
        '
        Me.bt_Imprimir.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.bt_Imprimir.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.bt_Imprimir.Dock = System.Windows.Forms.DockStyle.Right
        Me.bt_Imprimir.Image = Global.Presentacion.My.Resources.Resources.PEDI_ENTRAGA_PEDIDOS
        Me.bt_Imprimir.ImageFixedSize = New System.Drawing.Size(30, 30)
        Me.bt_Imprimir.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.bt_Imprimir.Location = New System.Drawing.Point(847, 0)
        Me.bt_Imprimir.Name = "bt_Imprimir"
        Me.bt_Imprimir.Size = New System.Drawing.Size(75, 60)
        Me.bt_Imprimir.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.bt_Imprimir.TabIndex = 5
        Me.bt_Imprimir.Text = "Imprimir"
        '
        'Dgv_Caja
        '
        Me.Dgv_Caja.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Dgv_Caja.Location = New System.Drawing.Point(0, 60)
        Me.Dgv_Caja.Name = "Dgv_Caja"
        Me.Dgv_Caja.Size = New System.Drawing.Size(922, 433)
        Me.Dgv_Caja.TabIndex = 3
        '
        'frmCajaGeneral
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(922, 493)
        Me.Controls.Add(Me.Dgv_Caja)
        Me.Controls.Add(Me.PanelSuperior)
        Me.Name = "frmCajaGeneral"
        Me.Text = "frmCajaGeneral"
        Me.PanelSuperior.ResumeLayout(False)
        CType(Me.TB_FechaHasta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tb_FechaDesde, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Dgv_Caja, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelSuperior As Panel
    Friend WithEvents LabelX1 As DevComponents.DotNetBar.LabelX
    Friend WithEvents Tb_FechaDesde As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents bt_Imprimir As DevComponents.DotNetBar.ButtonX
    Friend WithEvents Dgv_Caja As Janus.Windows.GridEX.GridEX
    Friend WithEvents LabelX2 As DevComponents.DotNetBar.LabelX
    Friend WithEvents TB_FechaHasta As DevComponents.Editors.DateTimeAdv.DateTimeInput
    Friend WithEvents btGenerar As DevComponents.DotNetBar.ButtonX
End Class
