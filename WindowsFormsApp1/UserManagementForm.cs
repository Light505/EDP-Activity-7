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
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm(currentUserRole);
            dashboard.Show();
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e) => ShowControl(new UserAddControl());
        private void btnUpdate_Click(object sender, EventArgs e) => ShowControl(new UserUpdateControl());
        private void btnActivate_Click(object sender, EventArgs e) => ShowControl(new UserActivateControl());
        private void btnDeactivate_Click(object sender, EventArgs e) => ShowControl(new UserDeactivateControl());
        private void btnSearch_Click(object sender, EventArgs e) => ShowControl(new UserSearchControl());

        private void ShowControl(UserControl control)
        {
            pnlContent.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(control);
        }
    }
}
