using System;
using System.Windows.Forms;

namespace InformationSystem
{
    public partial class DashboardForm : Form
    {
        private readonly string currentUserRole;

        public DashboardForm(string role)
        {
            InitializeComponent();
            currentUserRole = role;
            btnUserManagementForm.Visible = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportGeneratorForm report = new ReportGeneratorForm();
            report.Show();
        }

        private void btnUserManagementForm_Click(object sender, EventArgs e)
        {
            UserManagementForm userManagement = new UserManagementForm(currentUserRole);
            userManagement.Show();
            this.Close();
        }

        private void btnTransactionManagement_Click(object sender, EventArgs e)
        {
            TransactionManagementForm transactionManagementForm = new TransactionManagementForm(currentUserRole);
            transactionManagementForm.Show();
            this.Close();
        }
    }
}
