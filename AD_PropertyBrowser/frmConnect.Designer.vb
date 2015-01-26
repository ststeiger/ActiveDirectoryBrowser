<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConnect
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
        Me.cbIntegratedSecurity = New System.Windows.Forms.CheckBox()
        Me.txtDomain = New System.Windows.Forms.TextBox()
        Me.lblDomain = New System.Windows.Forms.Label()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cbIntegratedSecurity
        '
        Me.cbIntegratedSecurity.AutoSize = True
        Me.cbIntegratedSecurity.Location = New System.Drawing.Point(107, 37)
        Me.cbIntegratedSecurity.Name = "cbIntegratedSecurity"
        Me.cbIntegratedSecurity.Size = New System.Drawing.Size(123, 17)
        Me.cbIntegratedSecurity.TabIndex = 22
        Me.cbIntegratedSecurity.Text = "Integrierte Sicherheit"
        Me.cbIntegratedSecurity.UseVisualStyleBackColor = True
        '
        'txtDomain
        '
        Me.txtDomain.Location = New System.Drawing.Point(107, 11)
        Me.txtDomain.Name = "txtDomain"
        Me.txtDomain.Size = New System.Drawing.Size(253, 20)
        Me.txtDomain.TabIndex = 21
        Me.txtDomain.Text = "LDAP://DC=cor,DC=local"
        '
        'lblDomain
        '
        Me.lblDomain.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblDomain.Location = New System.Drawing.Point(13, 11)
        Me.lblDomain.Name = "lblDomain"
        Me.lblDomain.Size = New System.Drawing.Size(88, 16)
        Me.lblDomain.TabIndex = 20
        Me.lblDomain.Text = "Domäne:"
        Me.lblDomain.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnConnect
        '
        Me.btnConnect.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnConnect.Location = New System.Drawing.Point(285, 112)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnConnect.TabIndex = 19
        Me.btnConnect.Text = "&Verbinden..."
        '
        'txtPassword
        '
        Me.txtPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.txtPassword.Location = New System.Drawing.Point(107, 86)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(253, 20)
        Me.txtPassword.TabIndex = 17
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(107, 60)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(253, 20)
        Me.txtUsername.TabIndex = 16
        Me.txtUsername.Text = "stefan.steiger"
        '
        'lblPassword
        '
        Me.lblPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblPassword.Location = New System.Drawing.Point(11, 87)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(88, 16)
        Me.lblPassword.TabIndex = 18
        Me.lblPassword.Text = "Passwort:"
        Me.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblUsername
        '
        Me.lblUsername.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblUsername.Location = New System.Drawing.Point(11, 61)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(88, 16)
        Me.lblUsername.TabIndex = 15
        Me.lblUsername.Text = "Benutzername:"
        Me.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmConnect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(371, 147)
        Me.Controls.Add(Me.cbIntegratedSecurity)
        Me.Controls.Add(Me.txtDomain)
        Me.Controls.Add(Me.lblDomain)
        Me.Controls.Add(Me.btnConnect)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.lblUsername)
        Me.Name = "frmConnect"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Verbinden"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbIntegratedSecurity As System.Windows.Forms.CheckBox
    Private WithEvents txtDomain As System.Windows.Forms.TextBox
    Private WithEvents lblDomain As System.Windows.Forms.Label
    Private WithEvents btnConnect As System.Windows.Forms.Button
    Private WithEvents txtPassword As System.Windows.Forms.TextBox
    Private WithEvents txtUsername As System.Windows.Forms.TextBox
    Private WithEvents lblPassword As System.Windows.Forms.Label
    Private WithEvents lblUsername As System.Windows.Forms.Label
End Class
