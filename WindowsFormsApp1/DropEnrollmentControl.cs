using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class DropEnrollmentControl : UserControl
    {
        private readonly DbConnection dbConnection = new DbConnection();

        public DropEnrollmentControl()
        {
            InitializeComponent();
        }

        private void DropEnrollmentControl_Load(object sender, EventArgs e)
        {
            LoadEnrollments();
        }

        private void btnDrop_Click(object sender, EventArgs e)
        {
            DropEnrollment();
        }

        private void LoadEnrollments()
        {
            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT e.enrollment_id,
                           s.student_no,
                           CONCAT(s.last_name, ', ', s.first_name) AS student_name,
                           c.course_code,
                           c.course_title,
                           e.term,
                           e.status
                    FROM enrollments e
                    INNER JOIN students s ON s.student_id = e.student_id
                    INNER JOIN courses c ON c.course_id = e.course_id
                    ORDER BY e.enrolled_at DESC";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvEnrollments.DataSource = dt;
            }

            dgvEnrollments.ReadOnly = true;
            dgvEnrollments.AllowUserToAddRows = false;
            dgvEnrollments.AllowUserToDeleteRows = false;
            dgvEnrollments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEnrollments.MultiSelect = false;
        }

        private void DropEnrollment()
        {
            if (!int.TryParse(txtEnrollmentId.Text.Trim(), out int enrollmentId))
            {
                MessageBox.Show("Please enter a valid enrollment ID.");
                return;
            }

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = @"
                        UPDATE enrollments
                        SET status = 'DROPPED'
                        WHERE enrollment_id = @enrollment_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@enrollment_id", enrollmentId);
                        int affected = cmd.ExecuteNonQuery();

                        if (affected == 0)
                        {
                            MessageBox.Show("Enrollment ID not found.");
                            return;
                        }
                    }

                    string clearGradeQuery = @"
                        UPDATE grades
                        SET grade = NULL,
                            remarks = NULL
                        WHERE enrollment_id = @enrollment_id";

                    using (MySqlCommand cmd = new MySqlCommand(clearGradeQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@enrollment_id", enrollmentId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Enrollment dropped successfully.");
                    LoadEnrollments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error dropping enrollment: " + ex.Message);
                }
            }
        }
    }
}
