namespace InformationSystem
{
    partial class GradeSubmissionControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEnrollmentId;
        private System.Windows.Forms.Label lblGrade;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtEnrollmentId;
        private System.Windows.Forms.TextBox txtGrade;
        private System.Windows.Forms.ComboBox cmbRemarks;
        private System.Windows.Forms.Button btnSaveGrade;
        private System.Windows.Forms.DataGridView dgvGrades;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblEnrollmentId = new System.Windows.Forms.Label();
            this.lblGrade = new System.Windows.Forms.Label();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtEnrollmentId = new System.Windows.Forms.TextBox();
            this.txtGrade = new System.Windows.Forms.TextBox();
            this.cmbRemarks = new System.Windows.Forms.ComboBox();
            this.btnSaveGrade = new System.Windows.Forms.Button();
            this.dgvGrades = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(138, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Grade Submission";
            // 
            // lblEnrollmentId
            // 
            this.lblEnrollmentId.AutoSize = true;
            this.lblEnrollmentId.Location = new System.Drawing.Point(12, 50);
            this.lblEnrollmentId.Name = "lblEnrollmentId";
            this.lblEnrollmentId.Size = new System.Drawing.Size(74, 13);
            this.lblEnrollmentId.TabIndex = 1;
            this.lblEnrollmentId.Text = "Enrollment ID";
            // 
            // lblGrade
            // 
            this.lblGrade.AutoSize = true;
            this.lblGrade.Location = new System.Drawing.Point(12, 78);
            this.lblGrade.Name = "lblGrade";
            this.lblGrade.Size = new System.Drawing.Size(36, 13);
            this.lblGrade.TabIndex = 2;
            this.lblGrade.Text = "Grade";
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(12, 106);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(49, 13);
            this.lblRemarks.TabIndex = 3;
            this.lblRemarks.Text = "Remarks";
            // 
            // txtEnrollmentId
            // 
            this.txtEnrollmentId.Location = new System.Drawing.Point(92, 47);
            this.txtEnrollmentId.Name = "txtEnrollmentId";
            this.txtEnrollmentId.Size = new System.Drawing.Size(218, 20);
            this.txtEnrollmentId.TabIndex = 4;
            // 
            // txtGrade
            // 
            this.txtGrade.Location = new System.Drawing.Point(92, 75);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.Size = new System.Drawing.Size(218, 20);
            this.txtGrade.TabIndex = 5;
            // 
            // cmbRemarks
            // 
            this.cmbRemarks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRemarks.FormattingEnabled = true;
            this.cmbRemarks.Items.AddRange(new object[] {
            "PASSED",
            "FAILED",
            "INCOMPLETE",
            "WITHDRAWN"});
            this.cmbRemarks.Location = new System.Drawing.Point(92, 103);
            this.cmbRemarks.Name = "cmbRemarks";
            this.cmbRemarks.Size = new System.Drawing.Size(218, 21);
            this.cmbRemarks.TabIndex = 6;
            // 
            // btnSaveGrade
            // 
            this.btnSaveGrade.Location = new System.Drawing.Point(235, 135);
            this.btnSaveGrade.Name = "btnSaveGrade";
            this.btnSaveGrade.Size = new System.Drawing.Size(75, 23);
            this.btnSaveGrade.TabIndex = 7;
            this.btnSaveGrade.Text = "Save";
            this.btnSaveGrade.UseVisualStyleBackColor = true;
            this.btnSaveGrade.Click += new System.EventHandler(this.btnSaveGrade_Click);
            // 
            // dgvGrades
            // 
            this.dgvGrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrades.Location = new System.Drawing.Point(330, 35);
            this.dgvGrades.Name = "dgvGrades";
            this.dgvGrades.Size = new System.Drawing.Size(350, 170);
            this.dgvGrades.TabIndex = 8;
            // 
            // GradeSubmissionControl
            // 
            this.Controls.Add(this.dgvGrades);
            this.Controls.Add(this.btnSaveGrade);
            this.Controls.Add(this.cmbRemarks);
            this.Controls.Add(this.txtGrade);
            this.Controls.Add(this.txtEnrollmentId);
            this.Controls.Add(this.lblRemarks);
            this.Controls.Add(this.lblGrade);
            this.Controls.Add(this.lblEnrollmentId);
            this.Controls.Add(this.lblTitle);
            this.Name = "GradeSubmissionControl";
            this.Size = new System.Drawing.Size(700, 240);
            this.Load += new System.EventHandler(this.GradeSubmissionControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
