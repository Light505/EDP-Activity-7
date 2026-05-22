using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InformationSystem
{
    public partial class EnrollmentControl : UserControl
    {
        private readonly DbConnection dbConnection = new DbConnection();

        public EnrollmentControl()
        {
            InitializeComponent();
        }

        private void EnrollmentControl_Load(object sender, EventArgs e)
        {
            LoadStudents();
            LoadCourses();
            LoadEnrollments();
        }

        private void btnEnroll_Click(object sender, EventArgs e)
        {
            EnrollStudent();
        }

        private void LoadStudents()
        {
            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT student_id, CONCAT(student_no, ' - ', last_name, ', ', first_name) AS student_name
                    FROM students
                    ORDER BY student_no";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbStudent.DataSource = dt;
                cmbStudent.DisplayMember = "student_name";
                cmbStudent.ValueMember = "student_id";
            }
        }

        private void LoadCourses()
        {
            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT course_id, CONCAT(course_code, ' - ', course_title) AS course_name
                    FROM courses
                    ORDER BY course_code";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbCourse.DataSource = dt;
                cmbCourse.DisplayMember = "course_name";
                cmbCourse.ValueMember = "course_id";
            }
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

        private void EnrollStudent()
        {
            if (cmbStudent.SelectedValue == null || cmbCourse.SelectedValue == null)
            {
                MessageBox.Show("Please select a student and course.");
                return;
            }

            string term = txtTerm.Text.Trim();

            if (string.IsNullOrWhiteSpace(term))
            {
                MessageBox.Show("Please enter a term.");
                return;
            }

            int studentId = Convert.ToInt32(cmbStudent.SelectedValue);
            int courseId = Convert.ToInt32(cmbCourse.SelectedValue);

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                try
                {
                    conn.Open();

                    string existingStatus = null;
                    int existingEnrollmentId = 0;

                    string existingQuery = @"
                        SELECT enrollment_id, status
                        FROM enrollments
                        WHERE student_id = @student_id
                          AND course_id = @course_id
                          AND term = @term
                        LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(existingQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@student_id", studentId);
                        cmd.Parameters.AddWithValue("@course_id", courseId);
                        cmd.Parameters.AddWithValue("@term", term);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                existingEnrollmentId = Convert.ToInt32(reader["enrollment_id"]);
                                existingStatus = reader["status"].ToString();
                            }
                        }
                    }

                    if (string.Equals(existingStatus, "ENROLLED", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Student is already enrolled in this course for the selected term.");
                        return;
                    }

                    if (string.Equals(existingStatus, "DROPPED", StringComparison.OrdinalIgnoreCase))
                    {
                        string reactivateQuery = @"
                            UPDATE enrollments
                            SET status = 'ENROLLED',
                                enrolled_at = CURRENT_TIMESTAMP
                            WHERE enrollment_id = @enrollment_id";

                        using (MySqlCommand cmd = new MySqlCommand(reactivateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@enrollment_id", existingEnrollmentId);
                            cmd.ExecuteNonQuery();
                        }

                        string resetGradeQuery = @"
                            INSERT INTO grades(enrollment_id, grade, remarks)
                            VALUES (@enrollment_id, NULL, NULL)
                            ON DUPLICATE KEY UPDATE
                                grade = NULL,
                                remarks = NULL";

                        using (MySqlCommand cmd = new MySqlCommand(resetGradeQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@enrollment_id", existingEnrollmentId);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Dropped enrollment restored successfully.");
                        LoadEnrollments();
                        return;
                    }

                    using (MySqlCommand cmd = new MySqlCommand("sp_enroll_student", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_student_id", studentId);
                        cmd.Parameters.AddWithValue("@p_course_id", courseId);
                        cmd.Parameters.AddWithValue("@p_term", term);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Student enrolled successfully.");
                    LoadEnrollments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error enrolling student: " + ex.Message);
                }
            }
        }
    }
}
