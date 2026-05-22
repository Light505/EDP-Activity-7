using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class LoginForm : Form
    {
        private readonly DbConnection dbConnection = new DbConnection();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = @"
                        SELECT user_id, first_name, last_name, role, password
                        FROM users
                        WHERE username = @username
                          AND is_active = 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && PasswordHasher.VerifyPassword(password, reader.GetString("password")))
                            {
                                int userId = reader.GetInt32("user_id");
                                string firstName = reader.GetString("first_name");
                                string lastName = reader.GetString("last_name");
                                string role = reader.GetString("role");
                                string storedPassword = reader.GetString("password");

                                MessageBox.Show($"Welcome, {firstName} {lastName}!");

                                if (!PasswordHasher.IsHashed(storedPassword))
                                {
                                    reader.Close();
                                    UpgradePasswordHash(conn, userId, password);
                                }

                                DashboardForm dashboard = new DashboardForm(role);
                                dashboard.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Invalid username, password, or account inactive.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void UpgradePasswordHash(MySqlConnection conn, int userId, string password)
        {
            string query = "UPDATE users SET password = @password WHERE user_id = @user_id";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@password", PasswordHasher.HashPassword(password));
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnForgotPassword_Click(object sender, EventArgs e)
        {
            PasswordRecoveryForm recovery = new PasswordRecoveryForm();
            recovery.ShowDialog();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            LandingPageForm landingPageForm = new LandingPageForm();
            landingPageForm.Show();
            this.Close();
        }
    }
}
