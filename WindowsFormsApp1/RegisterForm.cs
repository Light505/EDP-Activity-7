using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class RegisterForm : Form
    {
        private readonly DbConnection dbConnection = new DbConnection();

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            RegisterUser();
        }

        private void RegisterUser()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (!ValidateInput(username, password, confirmPassword, firstName, lastName, email))
            {
                return;
            }

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                if (UsernameOrEmailExists(conn, username, email))
                {
                    MessageBox.Show("Username or email already exists.");
                    return;
                }

                string insertQuery = @"
                    INSERT INTO users (username, password, first_name, last_name, email, is_active)
                    VALUES (@username, @password, @first_name, @last_name, @email, 1)";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@first_name", firstName);
                    cmd.Parameters.AddWithValue("@last_name", lastName);
                    cmd.Parameters.AddWithValue("@email", email);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Registration successful.");

            ClearForm();

            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private bool ValidateInput(string username, string password, string confirmPassword, string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please fill out all fields.");
                return false;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return false;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format.");
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool UsernameOrEmailExists(MySqlConnection conn, string username, string email)
        {
            string checkQuery = "SELECT COUNT(*) FROM users WHERE username=@username OR email=@email";

            using (MySqlCommand cmd = new MySqlCommand(checkQuery, conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@email", email);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            LandingPageForm landingPageForm = new LandingPageForm();
            landingPageForm.Show();
            this.Close();
        }
    }
}
