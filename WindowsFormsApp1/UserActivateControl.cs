using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class UserActivateControl : UserControl
    {
        private readonly DbConnection dbConnection = new DbConnection();

        public UserActivateControl()
        {
            InitializeComponent();
        }

        private void btnActivate_Click(object sender, EventArgs e)
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

                string query = "UPDATE users SET is_active = 1 WHERE username = @username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("User activated.");
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
