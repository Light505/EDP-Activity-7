using System;
using System.Windows.Forms;

namespace InformationSystem
{
    public partial class TransactionManagementForm : Form
    {
        private readonly string currentUserRole;

        public TransactionManagementForm(string role)
        {
            InitializeComponent();
            currentUserRole = role;
        }

        private void btnEnroll_Click(object sender, EventArgs e) => ShowControl(new EnrollmentControl());
        private void btnDrop_Click(object sender, EventArgs e) => ShowControl(new DropEnrollmentControl());
        private void btnGrade_Click(object sender, EventArgs e) => ShowControl(new GradeSubmissionControl());

        private void btnReturn_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm(currentUserRole);
            dashboard.Show();
            this.Close();
        }

        private void ShowControl(UserControl control)
        {
            pnlContent.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(control);
        }
    }
}
