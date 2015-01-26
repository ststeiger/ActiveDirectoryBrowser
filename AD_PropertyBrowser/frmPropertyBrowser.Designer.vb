<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPropertyBrowser
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPropertyBrowser))
        Me.ctr_list = New System.Windows.Forms.ListView()
        Me.ctr_tree = New System.Windows.Forms.TreeView()
        Me.SuspendLayout()
        '
        'ctr_list
        '
        Me.ctr_list.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctr_list.Location = New System.Drawing.Point(335, 0)
        Me.ctr_list.Name = "ctr_list"
        Me.ctr_list.Size = New System.Drawing.Size(602, 570)
        Me.ctr_list.TabIndex = 8
        Me.ctr_list.UseCompatibleStateImageBehavior = False
        Me.ctr_list.View = System.Windows.Forms.View.Details
        '
        'ctr_tree
        '
        Me.ctr_tree.Dock = System.Windows.Forms.DockStyle.Left
        Me.ctr_tree.Location = New System.Drawing.Point(0, 0)
        Me.ctr_tree.Name = "ctr_tree"
        Me.ctr_tree.Size = New System.Drawing.Size(335, 570)
        Me.ctr_tree.TabIndex = 7
        '
        'frmPropertyBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(937, 570)
        Me.Controls.Add(Me.ctr_list)
        Me.Controls.Add(Me.ctr_tree)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPropertyBrowser"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Property Browser"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents ctr_list As System.Windows.Forms.ListView
    Private WithEvents ctr_tree As System.Windows.Forms.TreeView
End Class
