using cwc.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cwc {
    public partial class Install : Form {

        public int nIsSet = 0;

        public Install() {
            InitializeComponent();
        }


        private void Install_Load(object sender, EventArgs e) {

            a.Text = PathHelper.GetExeDirectory();

            SystemPathModifier.fReadPath();
            fSetList();

        }

        private void cbPaths_SelectedIndexChanged(object sender, EventArgs e) {
             cbPaths.SelectedIndex = nIsSet;
        }

        private void btnOk_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnSet_Click(object sender, EventArgs e) {
            if(SystemPathModifier.AddPath(a.Text)) {
               fSetList();
            }
        }

        private void btnUnset_Click(object sender, EventArgs e) {
            if(SystemPathModifier.fRemoveAllCwc(a.Text)) {
               fSetList();
            }
        }

        public void fSetList() {

             cbPaths.Items.Clear();
             nIsSet = SystemPathModifier.fIsSet( a.Text);
            if(nIsSet == 0) {
               cbPaths.Items.Add("(Cwc Not Set)");
                btnUnset.Enabled = false;
                btnSet.Enabled = true;
                btnOk.Text = "Keep Cwc not set";
                lblTitle.Text = "Cwc is not in your environment, would you set it? (Recommended)";
          
            }else {
                btnUnset.Enabled = true;
                btnSet.Enabled = false;
                btnOk.Text = "Ok";
                lblTitle.Text = "Cwc is correctly configured in your environment";
            }

            cbPaths.Items.AddRange(SystemPathModifier.GetCurrentSystemPaths());

            cbPaths.SelectedIndex = nIsSet;
        }

        private void tbLocation_TextChanged(object sender, EventArgs e) {

        }

        private void cbAnnoy_CheckedChanged(object sender, EventArgs e) {

        }

		private void label3_Click(object sender, EventArgs e)
		{

		}

        private void btnAsscociate_Click(object sender, EventArgs e) {
            Program. fCheckForRegistringFiles(true);
        }
    }
}
