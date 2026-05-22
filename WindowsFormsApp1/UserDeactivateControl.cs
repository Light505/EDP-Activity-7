using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class UserDeactivateControl : UserControl
    {
        private readonly DbConnection dbConnection = new DbConnection();

        public UserDeactivateControl()
        {
            InitializeComponent();
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a username.");
                return;
            }

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = "UPDATE users SET is_active = 0 WHERE username = @username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("User deactivated.");
                        txtUsername.Clear();
                    }
                    else
                    {
                        MessageBox.Show("User not found.");
                    }
                }
            }
        }
    }
}