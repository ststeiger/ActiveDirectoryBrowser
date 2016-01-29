
using System.Windows.Forms;


namespace PropertyBrowser
{


    public partial class frmSearchUser : Form
    {

        protected string m_RootDn; 


        public frmSearchUser() : this(null)
        { } // End Constructor frmSearchUser


        public frmSearchUser(string RootDn)
        {
            m_RootDn = RootDn;
            InitializeComponent();
        } // End Constructor frmSearchUser



        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            string strUserName = this.txtUserName.Text;
            this.dgvDisplayData.DataSource = GetUserList(strUserName).DefaultView;

            this.dgvDisplayData.Sort(this.dgvDisplayData.Columns[this.dgvDisplayData.Columns[0].Name], System.ComponentModel.ListSortDirection.Ascending);
        } // End Sub btnSearch_Click 


        private System.Data.DataTable GetUserList()
        {
            return GetUserList(null);
        } // End Function GetUserList


        private System.Data.DataTable GetUserList(string strUserName)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("sAMAccountName", typeof(string));
            dt.Columns.Add("DistinguishedName", typeof(string));
            dt.Columns.Add("cn", typeof(string));
            dt.Columns.Add("DisplayName", typeof(string));

            dt.Columns.Add("EmailAddress", typeof(string));
            dt.Columns.Add("DomainName", typeof(string));
            dt.Columns.Add("Department", typeof(string));
            dt.Columns.Add("title", typeof(string));
            dt.Columns.Add("company", typeof(string));
            dt.Columns.Add("memberof", typeof(string));


            //using (System.DirectoryServices.DirectoryEntry rootDSE = new System.DirectoryServices.DirectoryEntry("LDAP://DC=cor,DC=local", username, password))
            using (System.DirectoryServices.DirectoryEntry rootDSE = LdapTools.GetDE(m_RootDn))
            {

                using (System.DirectoryServices.DirectorySearcher search = new System.DirectoryServices.DirectorySearcher(rootDSE))
                {
                    search.PageSize = 1001;// To Pull up more than 100 records.

                    //search.Filter = "(&(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))";//UserAccountControl will only Include Non-Disabled Users.

                    string strUserCondition = "";
                    if(!string.IsNullOrEmpty(strUserName))
                        strUserCondition = "(samAccountName=" + strUserName + ")";

                    //UserAccountControl will only Include Non-Disabled Users.
                    //search.Filter = "(&(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2)(samAccountName=stefan.steiger))";
                    
                    search.Filter = string.Format("(&(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2){0})", strUserCondition);

                    using (System.DirectoryServices.SearchResultCollection result = search.FindAll())
                    {

                        foreach (System.DirectoryServices.SearchResult item in result)
                        {
                            string sAMAccountName = null;
                            string DistinguishedName = null;
                            string cn = null;
                            string DisplayName = null;
                            string EmailAddress = null;
                            string DomainName = null;
                            string Department = null;
                            string title = null;
                            string company = null;
                            string memberof = null;


                            if (item.Properties["sAMAccountName"].Count > 0)
                            {
                                sAMAccountName = item.Properties["sAMAccountName"][0].ToString();
                            }

                            if (item.Properties["distinguishedName"].Count > 0)
                            {
                                DistinguishedName = item.Properties["distinguishedName"][0].ToString();
                            }

                            if (item.Properties["cn"].Count > 0)
                            {
                                cn = item.Properties["cn"][0].ToString();
                            }

                            if (item.Properties["DisplayName"].Count > 0)
                            {
                                DisplayName = item.Properties["DisplayName"][0].ToString();
                            }

                            if (item.Properties["mail"].Count > 0)
                            {
                                EmailAddress = item.Properties["mail"][0].ToString();
                            }

                            if (item.Properties["SamAccountName"].Count > 0)
                            {
                                DomainName = item.Properties["SamAccountName"][0].ToString();
                            }

                            if (item.Properties["department"].Count > 0)
                            {
                                Department = item.Properties["department"][0].ToString();
                            }

                            if (item.Properties["title"].Count > 0)
                            {
                                title = item.Properties["title"][0].ToString();
                            }

                            if (item.Properties["company"].Count > 0)
                            {
                                company = item.Properties["company"][0].ToString();
                            }

                            if (item.Properties["DistinguishedName"].Count > 0)
                            {
                                DistinguishedName = item.Properties["DistinguishedName"][0].ToString();
                            }

                            if (item.Properties["memberof"].Count > 0)
                            {
                                // memberof = item.Properties["memberof"][0].ToString();
                                memberof = LdapTools.GetGroups(DistinguishedName, true);
                            }


                            if (item.Properties["AccountExpirationDate"].Count > 0)
                            {
                                string aaa = item.Properties["AccountExpirationDate"][0].ToString();
                            }


                            System.Data.DataRow dr = dt.NewRow();

                            dr["sAMAccountName"] = sAMAccountName;
                            dr["DistinguishedName"] = DistinguishedName;
                            dr["cn"] = cn;
                            dr["DisplayName"] = DisplayName;
                            dr["EmailAddress"] = EmailAddress;
                            dr["DomainName"] = DomainName;
                            dr["Department"] = Department;
                            dr["title"] = title;
                            dr["company"] = company;
                            dr["memberof"] = memberof;

                            dt.Rows.Add(dr);



                            DisplayName = string.Empty;
                            EmailAddress = string.Empty;
                            DomainName = string.Empty;
                            Department = string.Empty;
                            title = string.Empty;
                            company = string.Empty;
                            memberof = string.Empty;

                            //rootDSE.Dispose();
                        } // Next SearchResult item 

                    } // End Using SearchResultCollection result 

                } // End Using search 

            } // End Using rootDSE

            return dt;
        } // End Function GetUserList



        private void btnAuth_Click(object sender, System.EventArgs e)
        {
            string usr = this.txtUserName.Text;
            string password = this.txtPassword.Text;

            MsgBox(LdapTools.IsAuthenticated(this.m_RootDn, usr, password));
        } // End Sub btnAuth_Click 


        public void MsgBox(object obj)
        {
            System.Windows.Forms.MessageBox.Show(System.Convert.ToString(obj));
        } // End Sub MsgBox


    } // End Class frmSearchUser : Form


} // End Namespace PropertyBrowser
