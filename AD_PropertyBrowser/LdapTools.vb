
Imports System.Collections.Generic
Imports System.Text
Imports System.DirectoryServices



Public Class LdapTools


    ' http://stackoverflow.com/questions/290548/validate-a-username-and-password-against-active-directory
    Public Shared Function IsAuthenticated(srvr As String, usr As String, pwd As String) As Boolean
        Dim authenticated As Boolean = False

        Try
            Dim entry As New DirectoryEntry(srvr, usr, pwd)
            Dim nativeObject As Object = entry.NativeObject


            authenticated = True
            'not authenticated; reason why is in cex
        Catch cex As DirectoryServicesCOMException
            'not authenticated due to some other exception [this is optional]
        Catch ex As Exception
        End Try

        Return authenticated
    End Function ' IsAuthenticated

    Public Shared Function GetDE(path As String, IntegratedSecurity As Boolean, username As String, password As String) As DirectoryEntry
        If IntegratedSecurity Then
            Return New DirectoryEntry(path)
        End If

        Return New DirectoryEntry(path, username, password)
    End Function


    Public Shared Function GetDE(path As String) As DirectoryEntry
        Return GetDE(path, True, Nothing, Nothing)
    End Function


    Public Shared Function GetGroups(userDn As String, recursive As Boolean) As String
        Dim groupMemberships As List(Of String) = Groups(Convert.ToString("LDAP://") & userDn, recursive)
        groupMemberships.Sort()


        Return String.Join(Environment.NewLine, groupMemberships.ToArray())
    End Function



    Public Shared Function Groups(userDn As String, recursive As Boolean) As List(Of String)
        Dim groupMemberships As New List(Of String)()
        Return AttributeValuesMultiString("memberOf", userDn, groupMemberships, recursive)
    End Function


    ' http://stackoverflow.com/questions/45437/determining-members-of-local-groups-via-c-sharp
    Public Shared Function AttributeValuesMultiString(attributeName As String, objectDn As String, valuesCollection As List(Of String), recursive As Boolean) As List(Of String)
        Using ent As New DirectoryEntry(objectDn)
            Dim ValueCollection As PropertyValueCollection = ent.Properties(attributeName)
            Dim en As System.Collections.IEnumerator = ValueCollection.GetEnumerator()

            While en.MoveNext()
                If en.Current IsNot Nothing Then
                    If Not valuesCollection.Contains(en.Current.ToString()) Then
                        valuesCollection.Add(en.Current.ToString())
                        If recursive Then
                            AttributeValuesMultiString(attributeName, "LDAP://" + en.Current.ToString(), valuesCollection, True)
                        End If
                    End If
                End If
            End While

            ' ent.Dispose();
            ent.Close()
        End Using
        ' End Using DirectoryEntry ent
        Return valuesCollection
    End Function


End Class
