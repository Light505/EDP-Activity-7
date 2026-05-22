using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class GradeSubmissionControl : UserControl
    {
        private readonly DbConnection dbConnection = new DbConnection();

        public GradeSubmissionControl()
        {
            InitializeComponent();
        }

        private void GradeSubmissionControl_Load(object sender, EventArgs e)
        {
            if (cmbRemarks.Items.Count > 0)
            {
                cmbRemarks.SelectedIndex = 0;
            }

            LoadGrades();
        }

        private void btnSaveGrade_Click(object sender, EventArgs e)
        {
            SaveGrade();
        }

        private void LoadGrades()
        {
            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT g.grade_id,
                           e.enrollment_id,
                           s.student_no,
                           CONCAT(s.last_name, ', ', s.first_name) AS student_name,
                           c.course_code,
                           c.course_title,
                           e.term,
                           e.status,
                           g.grade,
                           g.remarks
                    FROM grades g
                    INNER JOIN enrollments e ON e.enrollment_id = g.enrollment_id
                    INNER JOIN students s ON s.student_id = e.student_id
                    INNER JOIN courses c ON c.course_id = e.course_id
                    WHERE e.status = 'ENROLLED'
                    ORDER BY e.enrolled_at DESC";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvGrades.DataSource = dt;
            }

            dgvGrades.ReadOnly = true;
            dgvGrades.AllowUserToAddRows = false;
            dgvGrades.AllowUserToDeleteRows = false;
            dgvGrades.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGrades.MultiSelect = false;
        }

        private void SaveGrade()
        {
            if (!int.TryParse(txtEnrollmentId.Text.Trim(), out int enrollmentId))
            {
                MessageBox.Show("Please enter a valid enrollment ID.");
                return;
            }

            if (!decimal.TryParse(txtGrade.Text.Trim(), out decimal grade))
            {
                MessageBox.Show("Please enter a valid grade.");
                return;
            }

            if (grade < 0 || grade > 100)
            {
                MessageBox.Show("Grade must be between 0 and 100.");
                return;
            }

            if (cmbRemarks.SelectedItem == null)
            {
                MessageBox.Show("Please select remarks.");
                return;
            }

            string remarks = cmbRemarks.SelectedItem.ToString();

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();

                    string statusQuery = @"
                        SELECT status
                        FROM enrollments
                        WHERE enrollment_id = @enrollment_id";

                    string enrollmentStatus = null;

                    using (MySqlCommand cmd = new MySqlCommand(statusQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@enrollment_id", enrollmentId);
                        object result = cmd.ExecuteScalar();
                        enrollmentStatus = result?.ToString();
                    }

                    if (enrollmentStatus == null)
                    {
                        MessageBox.Show("Enrollment ID not found.");
                        return;
                    }

                    if (!string.Equals(enrollmentStatus, "ENROLLED", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Dropped enrollments cannot be graded.");
                        return;
                    }

                    string query = @"
                        UPDATE grades
                        SET grade = @grade,
                            remarks = @remarks
                        WHERE enrollment_id = @enrollment_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@grade", grade);
                        cmd.Parameters.AddWithValue("@remarks", remarks);
                        cmd.Parameters.AddWithValue("@enrollment_id", enrollmentId);

                        int affected = cmd.ExecuteNonQuery();

                        if (affected == 0)
                        {
                            MessageBox.Show("Enrollment ID not found.");
                            return;
                        }
                    }

                    MessageBox.Show("Grade saved successfully.");
                    LoadGrades();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving grade: " + ex.Message);
                }
            }
        }
    }
}
