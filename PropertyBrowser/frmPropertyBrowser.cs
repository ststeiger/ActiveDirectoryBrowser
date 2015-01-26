
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using System.DirectoryServices;


namespace PropertyBrowser
{


    public partial class frmPropertyBrowser : Form
    {
        public bool bIntegratedSecurity;
        public string strRootDSE;
        public string strUserName;
        public string strPassword;


        public frmPropertyBrowser()
        {
            InitializeComponent();
        } // End Constructor


        public void Connect()
        {
            DirectoryEntry Base;

            if (!strRootDSE.StartsWith("LDAP://", StringComparison.OrdinalIgnoreCase))
            {
                strRootDSE = "LDAP://" + strRootDSE;
            } // End if (!strRootDSE.StartsWith("LDAP://", StringComparison.OrdinalIgnoreCase))

            if (bIntegratedSecurity)
            {
                Base = new DirectoryEntry(strRootDSE);
            } // End if (bIntegratedSecurity)
            else
            {
                Base = new DirectoryEntry(strRootDSE, strUserName, strPassword);
            } // End else of if (bIntegratedSecurity)


            //Read the root:
            if (Base != null)
            {

                ctr_tree.Nodes.Clear();
                ctr_tree.BeginUpdate();

                TreeNode childNode = null;

                try
                {
                    childNode = ctr_tree.Nodes.Add(Base.Name);
                    childNode.Tag = Base;


                    foreach (DirectoryEntry rootIter in Base.Children)
                    {
                        TreeNode RootNode = childNode.Nodes.Add(rootIter.Name);
                        RootNode.Tag = rootIter;
                    } // Next rootIter
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    childNode.Expand();
                    ctr_tree.EndUpdate();
                } // End Finally

            } // End if (Base != null)

        } // End Sub Connect


        private void ctr_tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Fill the TreeView dynamic after Click
            if (e.Node.Nodes.Count == 0)
            {
                DirectoryEntry parent = (DirectoryEntry)e.Node.Tag;

                if (parent != null)
                {

                    if (parent.Children != null)
                    {

                        foreach (DirectoryEntry Iter in parent.Children)
                        {
                            TreeNode childNode = e.Node.Nodes.Add(Iter.Name);
                            childNode.Tag = Iter;
                        } // Next Iter

                    } // End if (parent.Children != null)

                } // End if (parent != null)

            } // End if (e.Node.Nodes.Count == 0)


            //Fill the ListView Element
            try
            {
                DirectoryEntry list = (DirectoryEntry)e.Node.Tag;
                if (list != null)
                {

                    ctr_list.Clear();

                    //Add some information to ListView ELement
                    ctr_list.Columns.Add("Attribute", 90, HorizontalAlignment.Left);
                    ctr_list.Columns.Add("Value", 350, HorizontalAlignment.Left);

                    foreach (object listIter in list.Properties.PropertyNames)
                    {
                        foreach (Object Iter in list.Properties[listIter.ToString()])
                        {
                            System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(listIter.ToString(), 0);
                            item.SubItems.Add(Iter.ToString());
                            ctr_list.Items.AddRange(new ListViewItem[] { item });
                        } // Next Iter

                    } // Next listIter

                } // End if (list != null)

            } // End Try
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            } // End Catch

        } // End Sub ctr_tree_AfterSelect


        private void frmPropertyBrowser_Load(object sender, EventArgs e)
        {
            frmConnect ConnectionData = new frmConnect(this);
            ConnectionData.ShowDialog();
            Connect();
        }


        private void ctr_tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string strNodeText = e.Node.Text;
                System.Windows.Forms.Clipboard.SetText(strNodeText);
                //menuStrip1.Show();
            }
        } // End Sub ctr_tree_NodeMouseClick 


        private void ctr_tree_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && (e.KeyCode == Keys.C)) || (e.Control && (e.KeyCode == Keys.X)))
            {
                TreeView tv = (TreeView)sender;

                string strNodeText = tv.SelectedNode.Text;
                System.Windows.Forms.Clipboard.SetText(strNodeText);
                //MsgBox(strNodeText);

                e.Handled = true;
            }
        } // End Sub frmPropertyBrowser_Load


        private void CopyListViewToClipboard(ListView lv)
        {
            string str = "";

            foreach (ListViewItem ThisItem in lv.SelectedItems)
            {
                // string strName = ThisItem.Text;
                // Console.WriteLine(strName);

                // for (int i = 0; i < ThisItem.SubItems.Count; ++i) Console.WriteLine(ThisItem.SubItems[i].Text);
                // foreach (ListViewItem.ListViewSubItem ThisSubItem in ThisItem.SubItems) Console.WriteLine(ThisSubItem.Text);

                // string attr = ThisItem.SubItems["Attribute"].Text;
                // string val = ThisItem.SubItems["Value"].Text;

                str += string.Format("{0}\t{1}" + Environment.NewLine, ThisItem.SubItems[0].Text, ThisItem.SubItems[1].Text);
            } // Next ThisItem

            System.Windows.Forms.Clipboard.SetText(str);
        } // End Sub CopyListViewToClipboard 


        private void ctr_list_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListView lv = (ListView)sender;
                CopyListViewToClipboard(lv);
            }
        } // End Sub ctr_list_MouseClick


        private void ctr_list_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && (e.KeyCode == Keys.C)) || (e.Control && (e.KeyCode == Keys.X)))
            {
                ListView lv = (ListView)sender;
                CopyListViewToClipboard(lv);

                e.Handled = true;
            }
        } // End Sub ctr_list_KeyDown 


        public static void MsgBox(object obj)
        {
            string str = System.Convert.ToString(obj);
            System.Windows.Forms.MessageBox.Show(str);
        }


    } // End Class frmPropertyBrowser : Form


} // End Namespace PropertyBrowser
