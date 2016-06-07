
using System.Windows.Forms;


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
            System.DirectoryServices.DirectoryEntry Base;

            if (!strRootDSE.StartsWith("LDAP://", System.StringComparison.OrdinalIgnoreCase))
            {
                strRootDSE = "LDAP://" + strRootDSE;
            } // End if (!strRootDSE.StartsWith("LDAP://", StringComparison.OrdinalIgnoreCase))

            if (bIntegratedSecurity)
            {
                Base = new System.DirectoryServices.DirectoryEntry(strRootDSE);
            } // End if (bIntegratedSecurity)
            else
            {
                Base = new System.DirectoryServices.DirectoryEntry(strRootDSE, strUserName, strPassword);
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


                    foreach (System.DirectoryServices.DirectoryEntry rootIter in Base.Children)
                    {
                        TreeNode RootNode = childNode.Nodes.Add(rootIter.Name);
                        RootNode.Tag = rootIter;
                    } // Next rootIter
                }
                catch (System.Exception ex)
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



        //[System.Runtime.InteropServices.ComImport]
        //[System.Runtime.InteropServices.Guid("9068270b-0939-11d1-8be1-00c04fd8d503")]
        //[System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsDual)]
        //public interface IADsLargeInteger
        //{
        //    [System.Runtime.InteropServices.DispId(0x00000002)]
        //    uint HighPart { get; set; }

        //    [System.Runtime.InteropServices.DispId(0x00000003)]
        //    uint LowPart { get; set; }
        //}


        private void ctr_tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Fill the TreeView dynamic after Click
            if (e.Node.Nodes.Count == 0)
            {
                System.DirectoryServices.DirectoryEntry parent = (System.DirectoryServices.DirectoryEntry)e.Node.Tag;

                if (parent != null)
                {

                    if (parent.Children != null)
                    {

                        foreach (System.DirectoryServices.DirectoryEntry Iter in parent.Children)
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
                System.DirectoryServices.DirectoryEntry list = (System.DirectoryServices.DirectoryEntry)e.Node.Tag;
                if (list != null)
                {

                    ctr_list.Clear();

                    //Add some information to ListView ELement
                    ctr_list.Columns.Add("Attribute", 90, HorizontalAlignment.Left);
                    ctr_list.Columns.Add("Value", 350, HorizontalAlignment.Left);

                    foreach (object listIter in list.Properties.PropertyNames)
                    {
                        foreach (object Iter in list.Properties[listIter.ToString()])
                        {
                            string propertyName = listIter.ToString();
                            System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(propertyName, 0);
                            AddLdapObjectAsString(propertyName, Iter, item);
                            ctr_list.Items.AddRange(new ListViewItem[] { item });
                        } // Next Iter

                    } // Next listIter

                    ctr_list.ListViewItemSorter = this.m_ColumnSorter;
                    ctr_list.Sorting = SortOrder.Ascending;
                    ctr_list.Sort();

                } // End if (list != null)

            } // End Try
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            } // End Catch

        } // End Sub ctr_tree_AfterSelect


        public static void AddLdapObjectAsString(string propertyName, object Iter, System.Windows.Forms.ListViewItem item)
        {

            // lastLogon	        System.__ComObject
            // lastLogoff	        System.__ComObject
            // lastLogonTimestamp	System.__ComObject

            // accountExpires	System.__ComObject
            // badPasswordTime	System.__ComObject
            // pwdLastSet	    System.__ComObject
            // lockoutTime	    System.__ComObject
            // uSNCreated	    System.__ComObject
            // uSNChanged	    System.__ComObject


            // msExchMailboxGuid	            System.Byte[]
            // msExchVersion	                System.__ComObject
            // msExchMailboxSecurityDescriptor	System.__ComObject
            // nTSecurityDescriptor	            System.__ComObject

            if (System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "lastLogon")
                || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "lastLogoff")
                || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "lastLogonTimestamp")

                || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "pwdLastSet")
                || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "badPasswordTime")
                || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "lockoutTime")

                || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "uSNCreated")
                || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "uSNChanged")

                || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "accountExpires")

                )
            {
                // http://social.technet.microsoft.com/wiki/contents/articles/22461.understanding-the-ad-account-attributes-lastlogon-lastlogontimestamp-and-lastlogondate.aspx
                // http://stackoverflow.com/questions/1602036/how-to-list-all-computers-and-the-last-time-they-were-logged-onto-in-ad
                // http://stackoverflow.com/questions/33274162/the-namespace-of-iadslargeinteger
                // Active DS Type Library

                // System.Console.WriteLine(Iter);
                // System.Console.WriteLine(str);
                try
                {

                    // SecurityDescriptor sd = (SecurityDescriptor)ent.Properties["ntSecurityDescriptor"].Value;
                    // ActiveDs.SecurityDescriptor sd = (ActiveDs.SecurityDescriptor)Iter;

                    // sd.DiscretionaryAcl
                    // ActiveDs.AccessControlList acl = (ActiveDs.AccessControlList)sd.DiscretionaryAcl;


                    //foreach (ActiveDs.AccessControlEntry ace in (System.Collections.IEnumerable)acl)
                    //{
                    //    System.Console.WriteLine("Trustee: {0}", ace.Trustee);
                    //    System.Console.WriteLine("AccessMask: {0}", ace.AccessMask);
                    //    System.Console.WriteLine("Access Type: {0}", ace.AceType);
                    //}



                    // ActiveDs.IADsLargeInteger ISomeAdTime = (ActiveDs.IADsLargeInteger)Iter;
                    // long lngSomeAdTime = (long)ISomeAdTime.HighPart << 32 | (uint)ISomeAdTime.LowPart;

                    // IADsLargeInteger noActiveDsSomeTime = (IADsLargeInteger)Iter;
                    // System.Console.WriteLine(noActiveDsSomeTime);

                    long lngSomeAdTime = ConvertLargeIntegerToLong(Iter);

                    System.DateTime someAdTime = System.DateTime.MaxValue;

                    if (lngSomeAdTime == long.MaxValue || lngSomeAdTime <= 0 || System.DateTime.MaxValue.ToFileTime() <= lngSomeAdTime)
                    {
                        someAdTime = System.DateTime.MaxValue;
                    }
                    else
                    {
                        // someAdTime = System.DateTime.FromFileTime(lngSomeAdTime);
                        someAdTime = System.DateTime.FromFileTimeUtc(lngSomeAdTime).ToLocalTime();
                    }

                    item.SubItems.Add(someAdTime.ToString("dd.MM.yyyy HH:mm:ss"));
                }
                catch (System.Exception ex)
                {
                    item.SubItems.Add(ex.Message + System.Environment.NewLine + Iter.ToString());
                }
            }
            else if (System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "userCertificate")
                // || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "mSMQSignCertificates")
                // || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "mSMQDigest")
                )
            {
                System.Security.Cryptography.X509Certificates.X509Certificate cert = new System.Security.Cryptography.X509Certificates.X509Certificate((byte[])Iter);
                item.SubItems.Add(cert.ToString());
            }
            else if (System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "objectSid"))
            {
                System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier((byte[])Iter, 0);
                item.SubItems.Add(sid.ToString());
            }
            else if (
                System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "objectGUID")
                || System.StringComparer.OrdinalIgnoreCase.Equals(propertyName, "msExchMailboxGuid")
                )
            {
                System.Guid guid = new System.Guid((byte[])Iter);
                item.SubItems.Add(guid.ToString());
            }
            else
            {
                item.SubItems.Add(Iter.ToString());
            }

        } // End Sub AddLdapObjectAsString 


        private static long ConvertLargeIntegerToLong(object largeInteger)
        {
            System.Type t = largeInteger.GetType();

            int highPart = (int)t.InvokeMember("HighPart", System.Reflection.BindingFlags.GetProperty, null, largeInteger, null);
            int lowPart = (int)t.InvokeMember("LowPart", System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Public, null, largeInteger, null);

            return (long)highPart << 32 | (uint)lowPart;
        }


        private void frmPropertyBrowser_Load(object sender, System.EventArgs e)
        {
            frmConnect ConnectionData = new frmConnect(this);
            System.Windows.Forms.DialogResult dres = ConnectionData.ShowDialog();

            if(dres == System.Windows.Forms.DialogResult.OK)
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

                str += string.Format("{0}\t{1}" + System.Environment.NewLine, ThisItem.SubItems[0].Text, ThisItem.SubItems[1].Text);
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



        private static ListViewColumnSorter SetupColumnSorter()
        {
            ListViewColumnSorter lcs = new ListViewColumnSorter();
            lcs.Order = SortOrder.Ascending;

            return lcs;
        }

        ListViewColumnSorter m_ColumnSorter = SetupColumnSorter();


        private void ctr_list_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView lvSender = (ListView)sender;

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == this.m_ColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (this.m_ColumnSorter.Order == SortOrder.Ascending)
                {
                    this.m_ColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    this.m_ColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                this.m_ColumnSorter.SortColumn = e.Column;
                this.m_ColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lvSender.Sort();
        } // End Sub ctr_list_ColumnClick 


    } // End Class frmPropertyBrowser : Form


} // End Namespace PropertyBrowser
