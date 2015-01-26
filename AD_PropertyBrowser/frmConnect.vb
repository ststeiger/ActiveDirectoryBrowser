
Public Class frmConnect

    Private par As frmPropertyBrowser


    Public Sub New()
        Me.New(Nothing)
    End Sub


    Public Sub New(ParentForm As frmPropertyBrowser)
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        If ParentForm Is Nothing Then
            ParentForm = DirectCast(Me.Parent, frmPropertyBrowser)
            ParentForm = DirectCast(Me.ParentForm, frmPropertyBrowser)
        End If

        Me.par = ParentForm

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub ' Constructor


    Private Sub frmConnect_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            Dim AdRootDSE As System.DirectoryServices.DirectoryEntry = New System.DirectoryServices.DirectoryEntry("LDAP://rootDSE")
            Dim rootdse As String = System.Convert.ToString(AdRootDSE.Properties("defaultNamingContext").Value)

            If Not rootdse.StartsWith("LDAP://", StringComparison.OrdinalIgnoreCase) Then
                rootdse = "LDAP://" + rootdse
            End If

            Me.txtDomain.Text = rootdse

            Me.cbIntegratedSecurity.Checked = True
        Catch ex As Exception

        End Try
    End Sub ' frmConnect_Load


    Private Sub btnConnect_Click(sender As System.Object, e As System.EventArgs) Handles btnConnect.Click
        par.strRootDSE = Me.txtDomain.Text
        par.bIntegratedSecurity = Me.cbIntegratedSecurity.Checked
        par.strUserName = Me.txtUsername.Text
        par.strPassword = Me.txtPassword.Text

        Me.Close()
    End Sub ' btnConnect_Click


    Private Sub cbIntegratedSecurity_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbIntegratedSecurity.CheckedChanged
        Me.lblUsername.Visible = Not cbIntegratedSecurity.Checked
        Me.lblPassword.Visible = Not cbIntegratedSecurity.Checked

        Me.txtUsername.Visible = Not cbIntegratedSecurity.Checked
        Me.txtPassword.Visible = Not cbIntegratedSecurity.Checked
    End Sub ' cbIntegratedSecurity_CheckedChanged


    Private Sub btnSearchUser_Click(sender As Object, e As EventArgs) Handles btnSearchUser.Click
        Dim myfrm As System.Windows.Forms.Form = New frmSearchUser(Me.txtDomain.Text)
        myfrm.Show()
    End Sub


End Class ' frmConnect
