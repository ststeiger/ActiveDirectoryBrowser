
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
            }

            this.par = ParentForm;
        } // End Constructor 


        private void frmConnect_Load(object sender, EventArgs e)
        {

            try
            {
                System.DirectoryServices.DirectoryEntry AdRootDSE = new System.DirectoryServices.DirectoryEntry("LDAP://rootDSE");
                string rootdse = System.Convert.ToString(AdRootDSE.Properties["defaultNamingContext"].Value);

                if (!rootdse.StartsWith("LDAP://", StringComparison.OrdinalIgnoreCase) && !rootdse.StartsWith("LDAPS://", StringComparison.OrdinalIgnoreCase))
                {
                    rootdse = "LDAP://" + rootdse;
                }

                this.txtDomain.Text = rootdse;

                this.cbIntegratedSecurity.Checked = true;
            }
            catch (Exception ex)
            {
            }

        } // End Sub frmConnect_Load


        private void btnConnect_Click(object sender, EventArgs e)
        {
            par.strRootDSE = this.txtDomain.Text;
            par.bIntegratedSecurity = this.cbIntegratedSecurity.Checked;
            par.strUserName = this.txtUsername.Text;
            par.strPassword = this.txtPassword.Text;

            this.Close();
        } // End Sub btnConnect_Click


        private void cbIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            this.lblUser.Visible = !cbIntegratedSecurity.Checked;
            this.lblPassword.Visible = !cbIntegratedSecurity.Checked;

            this.txtUsername.Visible = !cbIntegratedSecurity.Checked;
            this.txtPassword.Visible = !cbIntegratedSecurity.Checked;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Form myfrm = new frmSearchUser(this.txtDomain.Text);
            myfrm.Show();
        } // End Sub cbIntegratedSecurity_CheckedChanged


    } // End Class frmConnect : Form


} // End Namespace PropertyBrowser
