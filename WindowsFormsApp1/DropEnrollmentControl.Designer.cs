namespace InformationSystem
{
    partial class DropEnrollmentControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEnrollmentId;
        private System.Windows.Forms.TextBox txtEnrollmentId;
        private System.Windows.Forms.Button btnDrop;
        private System.Windows.Forms.DataGridView dgvEnrollments;

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
            this.txtEnrollmentId = new System.Windows.Forms.TextBox();
            this.btnDrop = new System.Windows.Forms.Button();
            this.dgvEnrollments = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(135, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Drop Enrollment";
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
            // txtEnrollmentId
            // 
            this.txtEnrollmentId.Location = new System.Drawing.Point(92, 47);
            this.txtEnrollmentId.Name = "txtEnrollmentId";
            this.txtEnrollmentId.Size = new System.Drawing.Size(218, 20);
            this.txtEnrollmentId.TabIndex = 2;
            // 
            // btnDrop
            // 
            this.btnDrop.Location = new System.Drawing.Point(235, 75);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(75, 23);
            this.btnDrop.TabIndex = 3;
            this.btnDrop.Text = "Drop";
            this.btnDrop.UseVisualStyleBackColor = true;
            this.btnDrop.Click += new System.EventHandler(this.btnDrop_Click);
            // 
            // dgvEnrollments
            // 
            this.dgvEnrollments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnrollments.Location = new System.Drawing.Point(330, 35);
            this.dgvEnrollments.Name = "dgvEnrollments";
            this.dgvEnrollments.Size = new System.Drawing.Size(350, 170);
            this.dgvEnrollments.TabIndex = 4;
            // 
            // DropEnrollmentControl
            // 
            this.Controls.Add(this.dgvEnrollments);
            this.Controls.Add(this.btnDrop);
            this.Controls.Add(this.txtEnrollmentId);
            this.Controls.Add(this.lblEnrollmentId);
            this.Controls.Add(this.lblTitle);
            this.Name = "DropEnrollmentControl";
            this.Size = new System.Drawing.Size(700, 240);
            this.Load += new System.EventHandler(this.DropEnrollmentControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
