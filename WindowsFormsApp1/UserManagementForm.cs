using System;
using System.Windows.Forms;

namespace InformationSystem
{
    public partial class UserManagementForm : Form
    {
        private readonly string currentUserRole;

        public UserManagementForm(string role)
        {
            InitializeComponent();
            currentUserRole = role;

            if (!IsAdmin())
            {
                MessageBox.Show("Only administrators can access user management.");
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnActivate.Enabled = false;
                btnDeactivate.Enabled = false;
                btnSearch.Enabled = false;
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm(currentUserRole);
            dashboard.Show();
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e) => ShowAdminControl(new UserAddControl());
        private void btnUpdate_Click(object sender, EventArgs e) => ShowAdminControl(new UserUpdateControl());
        private void btnActivate_Click(object sender, EventArgs e) => ShowAdminControl(new UserActivateControl());
        private void btnDeactivate_Click(object sender, EventArgs e) => ShowAdminControl(new UserDeactivateControl());
        private void btnSearch_Click(object sender, EventArgs e) => ShowAdminControl(new UserSearchControl());

        private void ShowAdminControl(UserControl control)
        {
            if (!IsAdmin())
            {
                MessageBox.Show("Only administrators can manage users.");
                return;
            }

            ShowControl(control);
        }

        private void ShowControl(UserControl control)
        {
            pnlContent.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(control);
        }

        private bool IsAdmin()
        {
            return string.Equals(currentUserRole, "Admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}
