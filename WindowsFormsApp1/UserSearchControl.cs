using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class UserSearchControl : UserControl
    {
        private readonly DbConnection dbConnection = new DbConnection();

        public UserSearchControl()
        {
            InitializeComponent();
        }

        private void UserSearchControl_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchUsers();
        }

        private void LoadUsers()
        {
            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT user_id, username, first_name, last_name, email, role, is_active
                    FROM users
                    ORDER BY user_id DESC";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvUsers.DataSource = dt;
            }

            dgvUsers.ReadOnly = true;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
        }

        private void SearchUsers()
        {
            string search = txtSearch.Text.Trim();

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT user_id, username, first_name, last_name, email, role, is_active
                    FROM users
                    WHERE username LIKE @search
                       OR first_name LIKE @search
                       OR last_name LIKE @search
                       OR email LIKE @search
                       OR CAST(user_id AS CHAR) LIKE @search
                    ORDER BY user_id DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvUsers.DataSource = dt;
            }
        }
    }
}