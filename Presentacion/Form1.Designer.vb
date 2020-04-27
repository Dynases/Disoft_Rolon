<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim cbgrupo4_DesignTimeLayout As Janus.Windows.GridEX.GridEXLayout = New Janus.Windows.GridEX.GridEXLayout()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.btgrupo2 = New DevComponents.DotNetBar.ButtonX()
        Me.btgrupo4 = New DevComponents.DotNetBar.ButtonX()
        Me.lbgrupo4 = New DevComponents.DotNetBar.LabelX()
        Me.cbgrupo4 = New Janus.Windows.GridEX.EditControls.MultiColumnCombo()
        Me.CircularProgress1 = New DevComponents.DotNetBar.Controls.CircularProgress()
        Me.KnobControl1 = New DevComponents.Instrumentation.KnobControl()
        Me.ProgressSteps1 = New DevComponents.DotNetBar.ProgressSteps()
        Me.StepItem1 = New DevComponents.DotNetBar.StepItem()
        Me.StepItem2 = New DevComponents.DotNetBar.StepItem()
        Me.StepItem3 = New DevComponents.DotNetBar.StepItem()
        Me.ButtonX1 = New DevComponents.DotNetBar.ButtonX()
        Me.btnGrabar = New DevComponents.DotNetBar.ButtonX()
        Me.ButtonX2 = New DevComponents.DotNetBar.ButtonX()
        Me.ColorPickerDropDown1 = New DevComponents.DotNetBar.ColorPickerDropDown()
        Me.ColorPickerDropDown2 = New DevComponents.DotNetBar.ColorPickerDropDown()
        Me.PanelToolBar2 = New System.Windows.Forms.Panel()
        Me.btnImprimir = New DevComponents.DotNetBar.ButtonX()
        CType(Me.cbgrupo4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelToolBar2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btgrupo2
        '
        Me.btgrupo2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btgrupo2.BackColor = System.Drawing.Color.Transparent
        Me.btgrupo2.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat
        Me.btgrupo2.Image = Global.Presentacion.My.Resources.Resources._14
        Me.btgrupo2.ImageFixedSize = New System.Drawing.Size(25, 23)
        Me.btgrupo2.Location = New System.Drawing.Point(386, 214)
        Me.btgrupo2.Name = "btgrupo2"
        Me.btgrupo2.Size = New System.Drawing.Size(28, 23)
        Me.btgrupo2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btgrupo2.TabIndex = 211
        Me.btgrupo2.Visible = False
        '
        'btgrupo4
        '
        Me.btgrupo4.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btgrupo4.BackColor = System.Drawing.Color.Transparent
        Me.btgrupo4.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat
        Me.btgrupo4.ImageFixedSize = New System.Drawing.Size(25, 23)
        Me.btgrupo4.Location = New System.Drawing.Point(525, 137)
        Me.btgrupo4.Name = "btgrupo4"
        Me.btgrupo4.Size = New System.Drawing.Size(28, 23)
        Me.btgrupo4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btgrupo4.TabIndex = 215
        Me.btgrupo4.Visible = False
        '
        'lbgrupo4
        '
        Me.lbgrupo4.BackColor = System.Drawing.Color.Transparent
        '
        '
        '
        Me.lbgrupo4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lbgrupo4.Font = New System.Drawing.Font("Georgia", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbgrupo4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.lbgrupo4.Location = New System.Drawing.Point(249, 134)
        Me.lbgrupo4.Name = "lbgrupo4"
        Me.lbgrupo4.SingleLineColor = System.Drawing.SystemColors.Control
        Me.lbgrupo4.Size = New System.Drawing.Size(116, 23)
        Me.lbgrupo4.TabIndex = 214
        Me.lbgrupo4.Text = "Grupo 4:"
        '
        'cbgrupo4
        '
        cbgrupo4_DesignTimeLayout.LayoutString = resources.GetString("cbgrupo4_DesignTimeLayout.LayoutString")
        Me.cbgrupo4.DesignTimeLayout = cbgrupo4_DesignTimeLayout
        Me.cbgrupo4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbgrupo4.Location = New System.Drawing.Point(375, 135)
        Me.cbgrupo4.MaxLength = 40
        Me.cbgrupo4.Name = "cbgrupo4"
        Me.cbgrupo4.Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme.Custom
        Me.cbgrupo4.Office2007CustomColor = System.Drawing.Color.DodgerBlue
        Me.cbgrupo4.SelectedIndex = -1
        Me.cbgrupo4.SelectedItem = Nothing
        Me.cbgrupo4.Size = New System.Drawing.Size(144, 22)
        Me.cbgrupo4.TabIndex = 213
        Me.cbgrupo4.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        '
        'CircularProgress1
        '
        '
        '
        '
        Me.CircularProgress1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.CircularProgress1.Location = New System.Drawing.Point(409, 291)
        Me.CircularProgress1.Name = "CircularProgress1"
        Me.CircularProgress1.Size = New System.Drawing.Size(75, 23)
        Me.CircularProgress1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP
        Me.CircularProgress1.TabIndex = 216
        '
        'KnobControl1
        '
        Me.KnobControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.KnobControl1.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.KnobControl1.Location = New System.Drawing.Point(599, 316)
        Me.KnobControl1.MaxValue = New Decimal(New Integer() {100, 0, 0, 0})
        Me.KnobControl1.MinValue = New Decimal(New Integer() {0, 0, 0, 0})
        Me.KnobControl1.Name = "KnobControl1"
        Me.KnobControl1.Size = New System.Drawing.Size(75, 60)
        Me.KnobControl1.TabIndex = 217
        Me.KnobControl1.Text = "KnobControl1"
        '
        'ProgressSteps1
        '
        '
        '
        '
        Me.ProgressSteps1.BackgroundStyle.Class = "ProgressSteps"
        Me.ProgressSteps1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ProgressSteps1.ContainerControlProcessDialogKey = True
        Me.ProgressSteps1.Items.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.StepItem1, Me.StepItem2, Me.StepItem3})
        Me.ProgressSteps1.Location = New System.Drawing.Point(525, 102)
        Me.ProgressSteps1.Name = "ProgressSteps1"
        Me.ProgressSteps1.Size = New System.Drawing.Size(75, 23)
        Me.ProgressSteps1.TabIndex = 218
        '
        'StepItem1
        '
        Me.StepItem1.Name = "StepItem1"
        Me.StepItem1.SymbolSize = 13.0!
        Me.StepItem1.Text = "Step 1"
        '
        'StepItem2
        '
        Me.StepItem2.Name = "StepItem2"
        Me.StepItem2.SymbolSize = 13.0!
        Me.StepItem2.Text = "Step 2"
        '
        'StepItem3
        '
        Me.StepItem3.Name = "StepItem3"
        Me.StepItem3.SymbolSize = 13.0!
        Me.StepItem3.Text = "Step 3"
        '
        'ButtonX1
        '
        Me.ButtonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ButtonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.ButtonX1.Location = New System.Drawing.Point(524, 175)
        Me.ButtonX1.Name = "ButtonX1"
        Me.ButtonX1.Size = New System.Drawing.Size(141, 92)
        Me.ButtonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.ButtonX1.TabIndex = 219
        Me.ButtonX1.Text = "ButtonX1"
        '
        'btnGrabar
        '
        Me.btnGrabar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnGrabar.ColorTable = DevComponents.DotNetBar.eButtonColor.Orange
        Me.btnGrabar.Dock = System.Windows.Forms.DockStyle.Left
        Me.btnGrabar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGrabar.ImageFixedSize = New System.Drawing.Size(48, 48)
        Me.btnGrabar.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnGrabar.Location = New System.Drawing.Point(0, 0)
        Me.btnGrabar.Name = "btnGrabar"
        Me.btnGrabar.Size = New System.Drawing.Size(72, 450)
        Me.btnGrabar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnGrabar.TabIndex = 220
        Me.btnGrabar.Text = "GRABAR"
        Me.btnGrabar.TextColor = System.Drawing.Color.White
        '
        'ButtonX2
        '
        Me.ButtonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ButtonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.Orange
        Me.ButtonX2.Dock = System.Windows.Forms.DockStyle.Left
        Me.ButtonX2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonX2.ImageFixedSize = New System.Drawing.Size(48, 48)
        Me.ButtonX2.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.ButtonX2.Location = New System.Drawing.Point(72, 0)
        Me.ButtonX2.Name = "ButtonX2"
        Me.ButtonX2.Size = New System.Drawing.Size(147, 450)
        Me.ButtonX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.ButtonX2.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.ColorPickerDropDown1, Me.ColorPickerDropDown2})
        Me.ButtonX2.TabIndex = 221
        Me.ButtonX2.Text = "GRABAR"
        Me.ButtonX2.TextColor = System.Drawing.Color.White
        '
        'ColorPickerDropDown1
        '
        Me.ColorPickerDropDown1.GlobalItem = False
        Me.ColorPickerDropDown1.Name = "ColorPickerDropDown1"
        Me.ColorPickerDropDown1.Text = "ColorPickerDropDown1"
        '
        'ColorPickerDropDown2
        '
        Me.ColorPickerDropDown2.GlobalItem = False
        Me.ColorPickerDropDown2.Name = "ColorPickerDropDown2"
        Me.ColorPickerDropDown2.Text = "ColorPickerDropDown2"
        '
        'PanelToolBar2
        '
        Me.PanelToolBar2.Controls.Add(Me.btnImprimir)
        Me.PanelToolBar2.Location = New System.Drawing.Point(430, 31)
        Me.PanelToolBar2.Name = "PanelToolBar2"
        Me.PanelToolBar2.Size = New System.Drawing.Size(80, 72)
        Me.PanelToolBar2.TabIndex = 222
        '
        'btnImprimir
        '
        Me.btnImprimir.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnImprimir.ColorTable = DevComponents.DotNetBar.eButtonColor.Orange
        Me.btnImprimir.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnImprimir.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImprimir.ImageFixedSize = New System.Drawing.Size(48, 48)
        Me.btnImprimir.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btnImprimir.Location = New System.Drawing.Point(0, 0)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(80, 72)
        Me.btnImprimir.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnImprimir.TabIndex = 11
        Me.btnImprimir.Text = "IMPRIMIR"
        Me.btnImprimir.TextColor = System.Drawing.Color.White
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.PanelToolBar2)
        Me.Controls.Add(Me.ButtonX2)
        Me.Controls.Add(Me.btnGrabar)
        Me.Controls.Add(Me.ButtonX1)
        Me.Controls.Add(Me.ProgressSteps1)
        Me.Controls.Add(Me.KnobControl1)
        Me.Controls.Add(Me.CircularProgress1)
        Me.Controls.Add(Me.btgrupo4)
        Me.Controls.Add(Me.lbgrupo4)
        Me.Controls.Add(Me.cbgrupo4)
        Me.Controls.Add(Me.btgrupo2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.cbgrupo4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelToolBar2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btgrupo2 As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btgrupo4 As DevComponents.DotNetBar.ButtonX
    Friend WithEvents lbgrupo4 As DevComponents.DotNetBar.LabelX
    Friend WithEvents cbgrupo4 As Janus.Windows.GridEX.EditControls.MultiColumnCombo
    Friend WithEvents CircularProgress1 As DevComponents.DotNetBar.Controls.CircularProgress
    Friend WithEvents KnobControl1 As DevComponents.Instrumentation.KnobControl
    Friend WithEvents ProgressSteps1 As DevComponents.DotNetBar.ProgressSteps
    Friend WithEvents StepItem1 As DevComponents.DotNetBar.StepItem
    Friend WithEvents StepItem2 As DevComponents.DotNetBar.StepItem
    Friend WithEvents StepItem3 As DevComponents.DotNetBar.StepItem
    Friend WithEvents ButtonX1 As DevComponents.DotNetBar.ButtonX
    Protected WithEvents btnGrabar As DevComponents.DotNetBar.ButtonX
    Protected WithEvents ButtonX2 As DevComponents.DotNetBar.ButtonX
    Friend WithEvents ColorPickerDropDown1 As DevComponents.DotNetBar.ColorPickerDropDown
    Friend WithEvents ColorPickerDropDown2 As DevComponents.DotNetBar.ColorPickerDropDown
    Protected WithEvents PanelToolBar2 As Panel
    Protected WithEvents btnImprimir As DevComponents.DotNetBar.ButtonX
End Class
