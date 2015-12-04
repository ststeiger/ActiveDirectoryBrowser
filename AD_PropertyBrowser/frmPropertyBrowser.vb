
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
                    For Each Iter As [Object] In list.Properties(listIter.ToString())
                        Dim item As New System.Windows.Forms.ListViewItem(listIter.ToString(), 0)
                        item.SubItems.Add(Iter.ToString())

                        ctr_list.Items.AddRange(New ListViewItem() {item})
                    Next Iter
                Next listIter
            End If ' list IsNot Nothing 
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub ' ctr_tree_AfterSelect


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


End Class ' frmPropertyBrowser
