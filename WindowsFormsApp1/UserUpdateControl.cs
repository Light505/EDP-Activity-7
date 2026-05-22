using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class UserUpdateControl : UserControl
    {
        private readonly DbConnection dbConnection = new DbConnection();

        public UserUpdateControl()
        {
            InitializeComponent();
        }

        private void UserUpdateControl_Load(object sender, EventArgs e)
        {
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUserId.Text.Trim(), out int userId))
            {
                MessageBox.Show("Please enter a valid User ID.");
                return;
            }

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT username, password, first_name, last_name, email
                    FROM users
                    WHERE user_id = @user_id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user_id", userId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtUsername.Text = reader.GetString("username");
                            txtPassword.Text = reader.GetString("password");
                            txtFirstName.Text = reader.GetString("first_name");
                            txtLastName.Text = reader.GetString("last_name");
                            txtEmail.Text = reader.GetString("email");
                        }
                        else
                        {
                            MessageBox.Show("User not found.");
                            ClearFields();
                        }
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUserId.Text.Trim(), out int userId))
            {
                MessageBox.Show("Please enter a valid User ID.");
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please fill out all fields.");
                return;
            }

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    UPDATE users
                    SET username = @username,
                        password = @password,
                        first_name = @first_name,
                        last_name = @last_name,
                        email = @email
                    WHERE user_id = @user_id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@first_name", firstName);
                    cmd.Parameters.AddWithValue("@last_name", lastName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@user_id", userId);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("User updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Update failed. User not found.");
                    }
                }
            }
        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
        }
    }
}