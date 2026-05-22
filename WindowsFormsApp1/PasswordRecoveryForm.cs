using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class PasswordRecoveryForm : Form
    {
        private readonly DbConnection dbConnection = new DbConnection();

        public PasswordRecoveryForm()
        {
            InitializeComponent();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
        }

        private void PasswordRecoveryForm_Load(object sender, EventArgs e)
        {
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill out all fields.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = @"
                        UPDATE users 
                        SET password = @password 
                        WHERE username = @username
                          AND email = @email
                          AND is_active = 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", PasswordHasher.HashPassword(newPassword));

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Password updated successfully.");
                            txtUsername.Clear();
                            txtEmail.Clear();
                            txtNewPassword.Clear();
                            txtConfirmPassword.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Username/email not found or account inactive.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
