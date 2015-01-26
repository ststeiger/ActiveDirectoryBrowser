namespace PropertyBrowser
{
    partial class frmPropertyBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPropertyBrowser));
            this.ctr_list = new System.Windows.Forms.ListView();
            this.ctr_tree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // ctr_list
            // 
            this.ctr_list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctr_list.Location = new System.Drawing.Point(335, 0);
            this.ctr_list.Name = "ctr_list";
            this.ctr_list.Size = new System.Drawing.Size(602, 570);
            this.ctr_list.TabIndex = 6;
            this.ctr_list.UseCompatibleStateImageBehavior = false;
            this.ctr_list.View = System.Windows.Forms.View.Details;
            this.ctr_list.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctr_list_KeyDown);
            this.ctr_list.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ctr_list_MouseClick);
            // 
            // ctr_tree
            // 
            this.ctr_tree.Dock = System.Windows.Forms.DockStyle.Left;
            this.ctr_tree.Location = new System.Drawing.Point(0, 0);
            this.ctr_tree.Name = "ctr_tree";
            this.ctr_tree.Size = new System.Drawing.Size(335, 570);
            this.ctr_tree.TabIndex = 5;
            this.ctr_tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ctr_tree_AfterSelect);
            this.ctr_tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ctr_tree_NodeMouseClick);
            this.ctr_tree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ctr_tree_KeyDown);
            // 
            // frmPropertyBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 570);
            this.Controls.Add(this.ctr_list);
            this.Controls.Add(this.ctr_tree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPropertyBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Property Browser";
            this.Load += new System.EventHandler(this.frmPropertyBrowser_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ctr_list;
        private System.Windows.Forms.TreeView ctr_tree;
    }
}