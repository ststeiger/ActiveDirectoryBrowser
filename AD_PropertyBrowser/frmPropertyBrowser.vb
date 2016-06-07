
Imports System.DirectoryServices


Public Class frmPropertyBrowser


    Public bIntegratedSecurity As Boolean
    Public strRootDSE As String
    Public strUserName As String
    Public strPassword As String


    Public Sub Connect()
        'strRootDSE = "LDAP://DC=cor,DC=local" 
        'strUserName = "stefan.steiger" 
        'strPassword = "Test"


        If Not strRootDSE.StartsWith("LDAP://", StringComparison.OrdinalIgnoreCase) AndAlso Not strRootDSE.StartsWith("LDAPS://", StringComparison.OrdinalIgnoreCase) Then
            strRootDSE = "LDAP://" + strRootDSE
        End If


        Dim Base As DirectoryEntry

        If bIntegratedSecurity Then
            Base = New DirectoryEntry(strRootDSE)
        Else
            Base = New DirectoryEntry(strRootDSE, strUserName, strPassword)
        End If


        'Read the root:
        If Base IsNot Nothing Then

            ctr_tree.Nodes.Clear()
            ctr_tree.BeginUpdate()

            Dim childNode As TreeNode = Nothing

            Try
                childNode = ctr_tree.Nodes.Add(Base.Name)
                childNode.Tag = Base

                For Each rootIter As DirectoryEntry In Base.Children
                    Dim RootNode As TreeNode = childNode.Nodes.Add(rootIter.Name)
                    RootNode.Tag = rootIter
                Next rootIter

            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                childNode.Expand()

                ctr_tree.EndUpdate()
            End Try

        Else
            MsgBox("Base is nothing.")
        End If ' (Base != null)

    End Sub ' Connect


    Private Sub ctr_tree_AfterSelect(sender As System.Object, e As System.Windows.Forms.TreeViewEventArgs) Handles ctr_tree.AfterSelect
        'Fill the TreeView dynamic after Click
        If e.Node.Nodes.Count = 0 Then
            Dim parent As DirectoryEntry = DirectCast(e.Node.Tag, DirectoryEntry)

            If parent IsNot Nothing Then

                If parent.Children IsNot Nothing Then

                    For Each Iter As DirectoryEntry In parent.Children
                        Dim childNode As TreeNode = e.Node.Nodes.Add(Iter.Name)
                        childNode.Tag = Iter
                    Next Iter

                End If ' parent.Children IsNot Nothing

            End If ' parent IsNot Nothing

        End If ' .Node.Nodes.Count = 0 

        'Fill the ListView Element
        Try
            Dim list As DirectoryEntry = DirectCast(e.Node.Tag, DirectoryEntry)

            If list IsNot Nothing Then
                ctr_list.Clear()

                'Add some information to ListView ELement
                ctr_list.Columns.Add("Attribute", 90, HorizontalAlignment.Left)
                ctr_list.Columns.Add("Value", 350, HorizontalAlignment.Left)

                For Each listIter As Object In list.Properties.PropertyNames
                    For Each Iter As Object In list.Properties(listIter.ToString())
                        Dim propertyName As String = listIter.ToString()
                        Dim item As New System.Windows.Forms.ListViewItem(listIter.ToString(), 0)
                        AddLdapObjectAsString(propertyName, Iter, item)
                        ctr_list.Items.AddRange(New ListViewItem() {item})
                    Next Iter
                Next listIter


                ctr_list.ListViewItemSorter = Me.m_ColumnSorter
                ctr_list.Sorting = SortOrder.Ascending
                ctr_list.Sort()
            End If ' list IsNot Nothing 
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub ' ctr_tree_AfterSelect



    Public Shared Sub AddLdapObjectAsString(propertyName As String, Iter As Object, item As System.Windows.Forms.ListViewItem)

        ' accountExpires	System.__ComObject
        ' badPasswordTime	System.__ComObject
        ' pwdLastSet	    System.__ComObject
        ' lockoutTime	    System.__ComObject
        ' uSNCreated	    System.__ComObject
        ' uSNChanged	    System.__ComObject

        ' lastLogon	        System.__ComObject
        ' lastLogoff	        System.__ComObject
        ' lastLogonTimestamp	System.__ComObject

        ' forceLogoff System.__ComObject
        ' creationTime System.__ComObject
        ' maxPwdAge System.__ComObject
        ' minPwdAge System.__ComObject

        ' wellKnownObjects System.__ComObject
        ' otherWellKnownObjects System.__ComObject

        ' modifiedCount System.__ComObject
        ' modifiedCountAtLastProm System.__ComObject

        ' nTSecurityDescriptor System.__ComObject
        ' msExchMailboxSecurityDescriptor System.__ComObject



        'If System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "forceLogoff") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "creationTime") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "maxPwdAge") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "minPwdAge") Then
        '    Try
        '        Dim lngSomeAdTime As Long = ConvertLargeIntegerToLong(Iter)


        '        item.SubItems.Add(lngSomeAdTime.ToString())
        '    Catch ex As System.Exception
        '        item.SubItems.Add(ex.Message + System.Environment.NewLine + Iter.ToString())
        '    End Try

        If System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "lastLogon") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "lastLogoff") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "lastLogonTimestamp") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "pwdLastSet") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "badPasswordTime") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "lockoutTime") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "uSNCreated") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "uSNChanged") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "accountExpires") Then


            ' http://social.technet.microsoft.com/wiki/contents/articles/22461.understanding-the-ad-account-attributes-lastlogon-lastlogontimestamp-and-lastlogondate.aspx
            ' http://stackoverflow.com/questions/1602036/how-to-list-all-computers-and-the-last-time-they-were-logged-onto-in-ad
            ' http://stackoverflow.com/questions/33274162/the-namespace-of-iadslargeinteger
            ' Active DS Type Library

            ' System.Console.WriteLine(Iter);
            ' System.Console.WriteLine(str);
            Try

                ' SecurityDescriptor sd = (SecurityDescriptor)ent.Properties["ntSecurityDescriptor"].Value;
                ' ActiveDs.SecurityDescriptor sd = (ActiveDs.SecurityDescriptor)Iter;

                ' sd.DiscretionaryAcl
                ' ActiveDs.AccessControlList acl = (ActiveDs.AccessControlList)sd.DiscretionaryAcl;


                'foreach (ActiveDs.AccessControlEntry ace in (System.Collections.IEnumerable)acl)
                '{
                '    System.Console.WriteLine("Trustee: {0}", ace.Trustee);
                '    System.Console.WriteLine("AccessMask: {0}", ace.AccessMask);
                '    System.Console.WriteLine("Access Type: {0}", ace.AceType);
                '}



                ' ActiveDs.IADsLargeInteger ISomeAdTime = (ActiveDs.IADsLargeInteger)Iter;
                ' long lngSomeAdTime = (long)ISomeAdTime.HighPart << 32 | (uint)ISomeAdTime.LowPart;

                ' IADsLargeInteger noActiveDsSomeTime = (IADsLargeInteger)Iter;
                ' System.Console.WriteLine(noActiveDsSomeTime);

                Dim lngSomeAdTime As Long = ConvertLargeIntegerToLong(Iter)

                Dim someAdTime As System.DateTime = System.DateTime.MaxValue

                If lngSomeAdTime = Long.MaxValue OrElse lngSomeAdTime <= 0 OrElse System.DateTime.MaxValue.ToFileTime() <= lngSomeAdTime Then
                    someAdTime = System.DateTime.MaxValue
                Else
                    ' someAdTime = System.DateTime.FromFileTime(lngSomeAdTime);
                    someAdTime = System.DateTime.FromFileTimeUtc(lngSomeAdTime).ToLocalTime()
                End If

                item.SubItems.Add(someAdTime.ToString("dd.MM.yyyy HH:mm:ss"))
            Catch ex As System.Exception
                item.SubItems.Add(ex.Message + System.Environment.NewLine + Iter.ToString())
            End Try
        ElseIf System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "msExchRecipientTypeDetails") Then
            Try
                Dim lngSomeVersion As Long = ConvertLargeIntegerToLong(Iter)
                Dim strVersion As String = lngSomeVersion.ToString()
                ' http://memphistech.net/?p=457
                ' https://blogs.technet.microsoft.com/johnbai/2013/09/11/o365-exchange-and-ad-how-msexchrecipientdisplaytype-and-msexchangerecipienttypedetails-relate-to-your-on-premises/

                Select Case lngSomeVersion

                    Case 1
                        strVersion = "User Mailbox"
                    Case 2
                        strVersion = "Linked Mailbox"
                    Case 4
                        strVersion = "Shared Mailbox"
                    Case 8
                        strVersion = "Legacy Mailbox"
                    Case 16
                        strVersion = "Room Mailbox"
                    Case 32
                        strVersion = "Equipment Mailbox"
                    Case 64
                        strVersion = "Mail Contact"
                    Case 128
                        strVersion = "Mail User"
                    Case 256
                        strVersion = "Mail-Enabled Universal Distribution Group"
                    Case 512
                        strVersion = "Mail-Enabled Non-Universal Distribution Group"
                    Case 1024
                        strVersion = "Mail-Enabled Universal Security Group"
                    Case 2048
                        strVersion = "Dynamic Distribution Group"
                    Case 4096
                        strVersion = "Public Folder"
                    Case 8192
                        strVersion = "System Attendant Mailbox"
                    Case 16384
                        strVersion = "System Mailbox"
                    Case 32768
                        strVersion = "Cross-Forest Mail Contact"
                    Case 65536
                        strVersion = "User"
                    Case 131072
                        strVersion = "Contact"
                    Case 262144
                        strVersion = "Universal Distribution Group"
                    Case 524288
                        strVersion = "Universal Security Group"
                    Case 1048576
                        strVersion = "Non-Universal Group"
                    Case 2097152
                        strVersion = "Disabled User"
                    Case 4194304
                        strVersion = "Microsoft Exchange"
                    Case 8388608
                        strVersion = "Arbitration Mailbox"
                    Case 16777216
                        strVersion = "Mailbox Plan"
                    Case 33554432
                        strVersion = "Linked User"
                    Case 268435456
                        strVersion = "Room List"
                    Case 536870912
                        strVersion = "Discovery Mailbox"
                    Case 1073741824
                        strVersion = "Role Group"
                    Case 2147483648
                        strVersion = "Remote Mailbox"
                    Case 137438953472
                        strVersion = "Team Mailbox"
                    Case Else
                        strVersion = lngSomeVersion.ToString()
                End Select

                item.SubItems.Add(strVersion)
            Catch ex As System.Exception
                item.SubItems.Add(ex.Message + System.Environment.NewLine + Iter.ToString())
            End Try

        ElseIf System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "msExchRemoteRecipientType") Then
            Try
                Dim lngSomeVersion As System.Int64 = System.Convert.ToInt64(Iter)
                Dim strVersion As String = lngSomeVersion.ToString()
                ' http://memphistech.net/?p=457
                ' https://blogs.technet.microsoft.com/johnbai/2013/09/11/o365-exchange-and-ad-how-msexchrecipientdisplaytype-and-msexchangerecipienttypedetails-relate-to-your-on-premises/

                Select Case lngSomeVersion
                    Case 1
                        strVersion = "ProvisionedMailbox (Cloud MBX)"
                    Case 2
                        strVersion = "ProvisionedArchive (Cloud Archive)"
                    Case 3
                        strVersion = "ProvisionedMailbox, ProvisionedArchive" ' (mailbox provisioned in Cloud & Archive provisioned in Cloud)* either via EMC or new-remotemailbox cmd
                    Case 4
                        strVersion = "Migrated mailbox from on-prem"
                    Case 6
                        strVersion = "Migrated mailbox from on-prem, ProvisionedArchive in EXO" ' (mailbox migrated from on-prem & archive provisioned in Cloud)
                    Case 16
                        strVersion = "DeprovisionArchive"
                    Case 20
                        strVersion = "DeprovisionArchive, Migrated"
                    Case 32
                        strVersion = "RoomMailbox"
                    Case 36
                        strVersion = "Migrated, RoomMailbox"
                    Case 64
                        strVersion = "EquipmentMailbox"
                    Case 68
                        strVersion = "Migrated, EquipmentMailbox"
                    Case 96
                        strVersion = "SharedMailbox"
                    Case 100
                        strVersion = "Migrated, Shared Mailbox in EXO"
                    Case Else
                        strVersion = lngSomeVersion.ToString()
                End Select

                item.SubItems.Add(strVersion)
            Catch ex As System.Exception
                item.SubItems.Add(ex.Message + System.Environment.NewLine + Iter.ToString())
            End Try

        ElseIf System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "msExchRecipientDisplayType") Then
            Try
                Dim lngSomeVersion As System.Int64 = System.Convert.ToInt64(Iter)
                Dim strVersion As String = lngSomeVersion.ToString()
                ' http://memphistech.net/?p=457

                Select Case lngSomeVersion
                    Case 0
                        strVersion = "MailboxUser"
                    Case 1
                        strVersion = "DistrbutionGroup"
                    Case 2
                        strVersion = "PublicFolder"
                    Case 3
                        strVersion = "DynamicDistributionGroup"
                    Case 4
                        strVersion = "Organization"
                    Case 5
                        strVersion = "PrivateDistributionList"
                    Case 6
                        strVersion = "RemoteMailUser"
                    Case 7
                        strVersion = "ConferenceRoomMailbox"
                    Case 8
                        strVersion = "EquipmentMailbox"
                    Case 1073741824
                        strVersion = "ACLableMailboxUser"
                    Case 1043741833
                        strVersion = "SecurityDistributionGroup"
                    Case -2147483642
                        strVersion = "SyncedMailboxUser"
                    Case -2147483391
                        strVersion = "SyncedUDGasUDG"
                    Case -2147483386
                        strVersion = "SyncedUDGasContact"
                    Case -2147483130
                        strVersion = "SyncedPublicFolder"
                    Case -2147482874
                        strVersion = "SyncedDynamicDistributionGroup"
                    Case -2147482106
                        strVersion = "SyncedRemoteMailUser"
                    Case -2147481850
                        strVersion = "SyncedConferenceRoomMailbox"
                    Case -2147481594
                        strVersion = "SyncedEquipmentMailbox"
                    Case -2147481343
                        strVersion = "SyncedUSGasUDG"
                    Case -2147481338
                        strVersion = "SyncedUSGasContact"
                    Case -1073741818
                        strVersion = "ACLableSyncedMailboxUser"
                    Case -1073740282
                        strVersion = "ACLableSyncedRemoteMailUser"
                    Case -1073739514
                        strVersion = "ACLableSyncedUSGasContact"
                    Case -1073739511
                        strVersion = "SyncedUSGasUSG"
                    Case Else
                        strVersion = lngSomeVersion.ToString()
                End Select

                item.SubItems.Add(strVersion)
            Catch ex As System.Exception
                item.SubItems.Add(ex.Message + System.Environment.NewLine + Iter.ToString())
            End Try
        ElseIf System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "msExchVersion") Then
            Try
                Dim lngSomeVersion As Long = ConvertLargeIntegerToLong(Iter)
                Dim strVersion As String = ""

                ' http://blogs.metcorpconsulting.com/tech/?p=1313
                If lngSomeVersion < 4535486012416 Then
                    strVersion = "Exchange 2003 and earlier (" + lngSomeVersion.ToString() + ")"
                ElseIf lngSomeVersion = 4535486012416 Then
                    strVersion = "Exchange 2007 (4535486012416)"
                ElseIf lngSomeVersion = 44220983382016 Then
                    strVersion = "Exchange 2010 (44220983382016)"
                Else
                    strVersion = lngSomeVersion.ToString()
                End If

                item.SubItems.Add(strVersion)
            Catch ex As System.Exception
                item.SubItems.Add(ex.Message + System.Environment.NewLine + Iter.ToString())
            End Try
        ElseIf System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "userCertificate") Then
            ' || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "mSMQSignCertificates")
            ' || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "mSMQDigest")
            Dim cert As New System.Security.Cryptography.X509Certificates.X509Certificate(DirectCast(Iter, Byte()))
            item.SubItems.Add(cert.ToString())
        ElseIf System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "objectSid") Then
            Dim sid As New System.Security.Principal.SecurityIdentifier(DirectCast(Iter, Byte()), 0)
            item.SubItems.Add(sid.ToString())
        ElseIf System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "objectGUID") OrElse System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "msExchMailboxGuid") Then
            Dim guid As New System.Guid(DirectCast(Iter, Byte()))
            item.SubItems.Add(guid.ToString())
        ElseIf Iter IsNot Nothing AndAlso Object.ReferenceEquals(Iter.GetType(), GetType(Byte())) Then
            Dim ba As Byte() = DirectCast(Iter, Byte())
            item.SubItems.Add("0x" + System.BitConverter.ToString(ba).Replace("-", ""))
        Else
            item.SubItems.Add(Iter.ToString())
        End If

    End Sub ' AddLdapObjectAsString 


    Private Shared Function ConvertLargeIntegerToLong(largeInteger As Object) As Long
        Dim t As System.Type = largeInteger.GetType()

        Dim highPart As Integer = CInt(t.InvokeMember("HighPart", System.Reflection.BindingFlags.GetProperty, Nothing, largeInteger, Nothing))
        Dim lowPart As Integer = CInt(t.InvokeMember("LowPart", System.Reflection.BindingFlags.GetProperty Or System.Reflection.BindingFlags.[Public], Nothing, largeInteger, Nothing))

        Return CLng(highPart) << 32 Or CUInt(lowPart)
    End Function ' ConvertLargeIntegerToLong



    Private Sub frmPropertyBrowser_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim ConnectionData As New frmConnect(Me)
        Dim dres As System.Windows.Forms.DialogResult = ConnectionData.ShowDialog()

        If dres = Windows.Forms.DialogResult.OK Then
            Connect()
        End If

    End Sub ' frmPropertyBrowser_Load


    Private Sub ctr_tree_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles ctr_tree.NodeMouseClick
        If e.Button = MouseButtons.Right Then
            Dim strNodeText As String = e.Node.Text
            'menuStrip1.Show();
            System.Windows.Forms.Clipboard.SetText(strNodeText)
        End If
    End Sub ' ctr_tree_NodeMouseClick 


    Private Sub ctr_tree_KeyDown(sender As Object, e As KeyEventArgs) Handles ctr_tree.KeyDown
        If (e.Control AndAlso (e.KeyCode = Keys.C)) OrElse (e.Control AndAlso (e.KeyCode = Keys.X)) Then
            Dim tv As TreeView = DirectCast(sender, TreeView)

            Dim strNodeText As String = tv.SelectedNode.Text
            System.Windows.Forms.Clipboard.SetText(strNodeText)
            'MsgBox(strNodeText);

            e.Handled = True
        End If
    End Sub ' ctr_tree_KeyDown 


    Private Sub CopyListViewToClipboard(lv As ListView)
        Dim str As String = ""

        For Each ThisItem As ListViewItem In lv.SelectedItems
            ' string strName = ThisItem.Text;
            ' Console.WriteLine(strName);

            ' for (int i = 0; i < ThisItem.SubItems.Count; ++i) Console.WriteLine(ThisItem.SubItems[i].Text);
            ' foreach (ListViewItem.ListViewSubItem ThisSubItem in ThisItem.SubItems) Console.WriteLine(ThisSubItem.Text);

            ' string attr = ThisItem.SubItems["Attribute"].Text;
            ' string val = ThisItem.SubItems["Value"].Text;

            str += String.Format("{0}" & vbTab & "{1}" + Environment.NewLine, ThisItem.SubItems(0).Text, ThisItem.SubItems(1).Text)
        Next
        ' Next ThisItem
        System.Windows.Forms.Clipboard.SetText(str)
    End Sub ' CopyListViewToClipboard 


    Private Sub ctr_list_MouseClick(sender As Object, e As MouseEventArgs) Handles ctr_list.MouseClick
        If e.Button = MouseButtons.Right Then
            Dim lv As ListView = DirectCast(sender, ListView)
            CopyListViewToClipboard(lv)
        End If
    End Sub ' ctr_list_MouseClick


    Private Sub ctr_list_KeyDown(sender As Object, e As KeyEventArgs) Handles ctr_list.KeyDown
        If (e.Control AndAlso (e.KeyCode = Keys.C)) OrElse (e.Control AndAlso (e.KeyCode = Keys.X)) Then
            Dim lv As ListView = DirectCast(sender, ListView)
            CopyListViewToClipboard(lv)

            e.Handled = True
        End If
    End Sub ' ctr_list_KeyDown 



    Private Shared Function SetupColumnSorter() As ListViewColumnSorter
        Dim lcs As New ListViewColumnSorter()
        lcs.Order = SortOrder.Ascending

        Return lcs
    End Function


    Private m_ColumnSorter As ListViewColumnSorter = SetupColumnSorter()


    Private Sub ctr_list_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles ctr_list.ColumnClick
        Dim lvSender As ListView = DirectCast(sender, ListView)

        ' Determine if clicked column is already the column that is being sorted.
        If e.Column = Me.m_ColumnSorter.SortColumn Then
            ' Reverse the current sort direction for this column.
            If Me.m_ColumnSorter.Order = SortOrder.Ascending Then
                Me.m_ColumnSorter.Order = SortOrder.Descending
            Else
                Me.m_ColumnSorter.Order = SortOrder.Ascending
            End If
        Else
            ' Set the column number that is to be sorted; default to ascending.
            Me.m_ColumnSorter.SortColumn = e.Column
            Me.m_ColumnSorter.Order = SortOrder.Ascending
        End If

        ' Perform the sort with these new sort options.
        lvSender.Sort()
    End Sub ' ctr_list_ColumnClick 


End Class ' frmPropertyBrowser
