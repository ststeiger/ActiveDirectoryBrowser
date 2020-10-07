Public Class frmSearchUser

    Protected m_RootDn As String


    Public Sub New()
        Me.New(Nothing)
    End Sub ' End Constructor frmSearchUser


    Public Sub New(RootDn As String)
        m_RootDn = RootDn
        InitializeComponent()
    End Sub ' End Constructor frmSearchUser



    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim strUserName As String = Me.txtUserName.Text
        Me.dgvDisplayData.DataSource = GetUserList(strUserName).DefaultView

        Me.dgvDisplayData.Sort(Me.dgvDisplayData.Columns(Me.dgvDisplayData.Columns(0).Name), System.ComponentModel.ListSortDirection.Ascending)
    End Sub


    ' http://social.technet.microsoft.com/wiki/contents/articles/5312.active-directory-characters-to-escape.aspx
    ' Active Directory
    ' The space character must be escaped only if it is the leading or trailing character in any component of a distinguished name.
    'Comma	,
    'Backslash character	\
    'Pound sign (hash sign)	#
    'Plus sign	+
    'Less than symbol	<
    'Greater than symbol	>
    'Semicolon	;
    'Double quote (quotation mark)	"
    'Equal sign	=

    ' The LDAP filter specification assigns special meaning to the following characters:
    ' * ( ) \ NUL

    ' Character	Hex Representation
    ' *	    \2A
    ' (	    \28
    ' )	    \29
    ' \	    \5C
    ' Nul	\00


    Private Function GetUserList(strUserName As String) As System.Data.DataTable
        Dim dt As System.Data.DataTable = New DataTable()

        dt.Columns.Add("sAMAccountName", GetType(String))
        dt.Columns.Add("DistinguishedName", GetType(String))
        dt.Columns.Add("cn", GetType(String))
        dt.Columns.Add("DisplayName", GetType(String))

        dt.Columns.Add("EmailAddress", GetType(String))
        dt.Columns.Add("DomainName", GetType(String))
        dt.Columns.Add("Department", GetType(String))
        dt.Columns.Add("title", GetType(String))
        dt.Columns.Add("company", GetType(String))
        dt.Columns.Add("memberof", GetType(String))


        'using (DirectoryEntry rootDSE = new DirectoryEntry("LDAP://DC=cor,DC=local", username, password))
        Using rootDSE As System.DirectoryServices.DirectoryEntry = LdapTools.GetDE(m_RootDn)

            Using search As New System.DirectoryServices.DirectorySearcher(rootDSE)
                search.PageSize = 1001
                ' To Pull up more than 100 records.
                'search.Filter = "(&(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))";//UserAccountControl will only Include Non-Disabled Users.

                Dim strUserCondition As String = ""
                If Not String.IsNullOrEmpty(strUserName) Then
                    ' strUserCondition = "(samAccountName=" & strUserName & ")"

                    strUserCondition = "(|(samAccountName=" + strUserName + ")"
                    strUserCondition += "(userPrincipalName=" + strUserName + ")"
                    strUserCondition += "(mail=" + strUserName + "))"
                End If

                'UserAccountControl will only Include Non-Disabled Users.
                'search.Filter = "(&(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2)(samAccountName=stefan.steiger))";


                'strFilter = "(&(objectCategory=person)(objectClass=user))"

                'search.Filter = String.Format("(&(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2){0})", strUserCondition)
                'search.Filter = String.Format("(&(objectClass=user){0})", strUserCondition)
                search.Filter = String.Format("(&{0})", strUserCondition)

                Using result As System.DirectoryServices.SearchResultCollection = search.FindAll()

                    For Each item As System.DirectoryServices.SearchResult In result
                        Dim sAMAccountName As String = Nothing
                        Dim DistinguishedName As String = Nothing
                        Dim cn As String = Nothing
                        Dim DisplayName As String = Nothing
                        Dim EmailAddress As String = Nothing
                        Dim DomainName As String = Nothing
                        Dim Department As String = Nothing
                        Dim title As String = Nothing
                        Dim company As String = Nothing
                        Dim memberof As String = Nothing


                        If item.Properties("sAMAccountName").Count > 0 Then
                            sAMAccountName = item.Properties("sAMAccountName")(0).ToString()
                        End If


                        If item.Properties("distinguishedName").Count > 0 Then
                            DistinguishedName = item.Properties("distinguishedName")(0).ToString()
                        End If


                        If item.Properties("cn").Count > 0 Then
                            cn = item.Properties("cn")(0).ToString()
                        End If

                        If item.Properties("DisplayName").Count > 0 Then
                            DisplayName = item.Properties("DisplayName")(0).ToString()
                        End If


                        If item.Properties("mail").Count > 0 Then
                            EmailAddress = item.Properties("mail")(0).ToString()
                        End If
                        If item.Properties("SamAccountName").Count > 0 Then
                            DomainName = item.Properties("SamAccountName")(0).ToString()
                        End If
                        If item.Properties("department").Count > 0 Then
                            Department = item.Properties("department")(0).ToString()
                        End If
                        If item.Properties("title").Count > 0 Then
                            title = item.Properties("title")(0).ToString()
                        End If
                        If item.Properties("company").Count > 0 Then
                            company = item.Properties("company")(0).ToString()
                        End If

                        If item.Properties("DistinguishedName").Count > 0 Then
                            DistinguishedName = item.Properties("DistinguishedName")(0).ToString()
                        End If

                        If item.Properties("memberof").Count > 0 Then
                            ' memberof = item.Properties["memberof"][0].ToString();
                            memberof = LdapTools.GetGroups(DistinguishedName, True)
                        End If


                        If item.Properties("AccountExpirationDate").Count > 0 Then
                            Dim aaa As String = item.Properties("AccountExpirationDate")(0).ToString()
                        End If


                        Dim dr As System.Data.DataRow = dt.NewRow()

                        dr("sAMAccountName") = sAMAccountName
                        dr("DistinguishedName") = DistinguishedName
                        dr("cn") = cn
                        dr("DisplayName") = DisplayName
                        dr("EmailAddress") = EmailAddress
                        dr("DomainName") = DomainName
                        dr("Department") = Department
                        dr("title") = title
                        dr("company") = company
                        dr("memberof") = memberof

                        dt.Rows.Add(dr)



                        DisplayName = String.Empty
                        EmailAddress = String.Empty
                        DomainName = String.Empty
                        Department = String.Empty
                        title = String.Empty
                        company = String.Empty

                        'rootDSE.Dispose();
                        memberof = String.Empty
                    Next item

                End Using ' SearchResultCollection result 

            End Using ' search 

        End Using ' rootDSE

        Return dt
    End Function ' GetUserList


    Private Function GetUserList() As System.Data.DataTable
        Return GetUserList(Nothing)
    End Function ' GetUserList


    Private Sub btnAuth_Click(sender As Object, e As EventArgs) Handles btnAuth.Click
        Dim usr As String = Me.txtUserName.Text
        Dim password As String = Me.txtPassword.Text

        MsgBox(LdapTools.IsAuthenticated(Me.m_RootDn, usr, password))
    End Sub ' btnAuth_Click 


    Public Sub MsgBox(obj As Object)
        System.Windows.Forms.MessageBox.Show(System.Convert.ToString(obj))
    End Sub ' MsgBox


End Class
