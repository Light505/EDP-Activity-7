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
                        SELECT user_id, first_name, last_name, role
                        FROM users
                        WHERE username = @username
                          AND password = @password
                          AND is_active = 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string firstName = reader.GetString("first_name");
                                string lastName = reader.GetString("last_name");
                                string role = reader.GetString("role");

                                MessageBox.Show($"Welcome, {firstName} {lastName}!");

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