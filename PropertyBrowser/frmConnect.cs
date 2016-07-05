
using System.Windows.Forms;


namespace PropertyBrowser
{


    public partial class frmConnect : Form
    {

        private frmPropertyBrowser par;


        public frmConnect()
            : this(null)
        {}


        public frmConnect(frmPropertyBrowser ParentForm)
        {
            InitializeComponent();
            //Load += frmConnectionData_Load;

            if (ParentForm == null)
            {
                ParentForm = (frmPropertyBrowser)this.Parent;
                ParentForm = (frmPropertyBrowser)this.ParentForm;
            } // End if (ParentForm == null) 

            this.par = ParentForm;
        } // End Constructor 


        private void frmConnect_Load(object sender, System.EventArgs e)
        {

            try
            {
                System.DirectoryServices.DirectoryEntry AdRootDSE = new System.DirectoryServices.DirectoryEntry("LDAP://rootDSE");
                string rootdse = System.Convert.ToString(AdRootDSE.Properties["defaultNamingContext"].Value);

                if (!rootdse.StartsWith("LDAP://", System.StringComparison.OrdinalIgnoreCase) && !rootdse.StartsWith("LDAPS://", System.StringComparison.OrdinalIgnoreCase))
                {
                    rootdse = "LDAP://" + rootdse;
                }

                this.txtDomain.Text = rootdse;

                this.cbIntegratedSecurity.Checked = true;
            }
            catch (System.Exception ex)
            { }

        } // End Sub frmConnect_Load


        private void btnConnect_Click(object sender, System.EventArgs e)
        {
            par.strRootDSE = this.txtDomain.Text;
            par.bIntegratedSecurity = this.cbIntegratedSecurity.Checked;
            par.strUserName = this.txtUsername.Text;
            par.strPassword = this.txtPassword.Text;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        } // End Sub btnConnect_Click


        private void cbIntegratedSecurity_CheckedChanged(object sender, System.EventArgs e)
        {
            this.lblUser.Visible = !cbIntegratedSecurity.Checked;
            this.lblPassword.Visible = !cbIntegratedSecurity.Checked;

            this.txtUsername.Visible = !cbIntegratedSecurity.Checked;
            this.txtPassword.Visible = !cbIntegratedSecurity.Checked;
        } // End Sub cbIntegratedSecurity_CheckedChanged 


        static void test()
        {
            string domainAndUsername = string.Empty;
            string domain = string.Empty;
            string userName = string.Empty;
            string passWord = string.Empty;
            System.DirectoryServices.AuthenticationTypes at = System.DirectoryServices.AuthenticationTypes.Anonymous;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            domain = @"LDAP://w.x.y.z";
            domainAndUsername = @"LDAP://w.x.y.z/cn=Lawrence E."+ @" Smithmier\, Jr.,cn=Users,dc=corp,"+"dc=productiveedge,dc=com";
            userName = "Administrator";
            passWord = "xxxpasswordxxx";
            at = System.DirectoryServices.AuthenticationTypes.Secure;

            System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(domain, userName, passWord, at);

            System.DirectoryServices.DirectorySearcher mySearcher = new System.DirectoryServices.DirectorySearcher(entry);

            System.DirectoryServices.SearchResultCollection results;
            string filter = "maxPwdAge=*";
            mySearcher.Filter = filter;

            results = mySearcher.FindAll();
            long maxDays = 0;
            if (results.Count >= 1)
            {
                long maxPwdAge = (long)results[0].Properties["maxPwdAge"][0];
                maxDays = maxPwdAge / -864000000000;
            } // End if (results.Count >= 1) 

            System.DirectoryServices.DirectoryEntry entryUser = new System.DirectoryServices.DirectoryEntry(domainAndUsername, userName, passWord, at);
            mySearcher = new System.DirectoryServices.DirectorySearcher(entryUser);

            results = mySearcher.FindAll();
            long daysLeft=0;
            if (results.Count >= 1)
            {
                var lastChanged = results[0].Properties["pwdLastSet"][0];
                daysLeft = maxDays - System.DateTime.Today.Subtract(System.DateTime.FromFileTime((long)lastChanged)).Days;
            } // End if (results.Count >= 1) 
            
            System.Console.WriteLine("You must change your password within {0} days", daysLeft);
            System.Console.ReadLine();
        }


        public void FindLockedAccounts()
        {
            System.DirectoryServices.ActiveDirectory.Forest forest = System.DirectoryServices.ActiveDirectory.Forest.GetCurrentForest();

            System.DirectoryServices.ActiveDirectory.DirectoryContext context = null;
            foreach (System.DirectoryServices.ActiveDirectory.Domain thisDomain in forest.Domains)
            {
                string domainName = thisDomain.Name;
                System.Console.WriteLine(domainName);
                context = new System.DirectoryServices.ActiveDirectory.DirectoryContext(System.DirectoryServices.ActiveDirectory.DirectoryContextType.Domain, domainName);
            } // Next thisDomain 

            //get our current domain policy
            System.DirectoryServices.ActiveDirectory.Domain domain = System.DirectoryServices.ActiveDirectory.Domain.GetDomain(context);
            System.DirectoryServices.DirectoryEntry root = domain.GetDirectoryEntry();

            // System.DirectoryServices.DirectoryEntry AdRootDSE = new System.DirectoryServices.DirectoryEntry("LDAP://rootDSE");
            // string rootdse = System.Convert.ToString(AdRootDSE.Properties["defaultNamingContext"].Value);
            // System.DirectoryServices.DirectoryEntry root = new System.DirectoryServices.DirectoryEntry(rootdse);

            DomainPolicy policy = new DomainPolicy(root);   


            //default for when accounts stay locked indefinitely
            string qry = "(lockoutTime>=1)";

            // System.TimeSpan duration = new TimeSpan(0, 30, 0);
            System.TimeSpan duration = policy.LockoutDuration;

            if (duration != System.TimeSpan.MaxValue)
            {
                System.DateTime lockoutThreshold = System.DateTime.Now.Subtract(duration);
                qry = string.Format( "(lockoutTime>={0})", lockoutThreshold.ToFileTime() );
            } // End if (duration != System.TimeSpan.MaxValue) 

            System.DirectoryServices.DirectorySearcher ds = new System.DirectoryServices.DirectorySearcher(root, qry);

            using (System.DirectoryServices.SearchResultCollection src = ds.FindAll())
            {

                foreach (System.DirectoryServices.SearchResult sr in src)
                {
                    long ticks = (long)sr.Properties["lockoutTime"][0];
                    System.Console.WriteLine("{0} locked out at {1}", sr.Properties["name"][0], System.DateTime.FromFileTime(ticks));
                } // Next sr 

            } // End Using src 

        } // End Sub FindLockedAccounts


        private void btnSearchUser_Click(object sender, System.EventArgs e)
        {
            // FindLockedAccounts();
            System.Windows.Forms.Form myfrm = new frmSearchUser(this.txtDomain.Text);
            myfrm.Show();
            System.Console.WriteLine("Hello");
        } // End Sub cbIntegratedSecurity_CheckedChanged


    } // End Class frmConnect : Form


} // End Namespace PropertyBrowser
