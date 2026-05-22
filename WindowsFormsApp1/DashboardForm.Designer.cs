namespace InformationSystem
{
    partial class DashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnUserManagementForm = new System.Windows.Forms.Button();
            this.btnTransactionManagement = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(33, 30);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(113, 22);
            this.btnAbout.TabIndex = 1;
            this.btnAbout.Text = "About Program";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(650, 30);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(113, 22);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnReports
            // 
            this.btnReports.Location = new System.Drawing.Point(650, 398);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(113, 22);
            this.btnReports.TabIndex = 3;
            this.btnReports.Text = "Generate Reports";
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(325, 170);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(126, 25);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Dashboard";
            // 
            // btnUserManagementForm
            // 
            this.btnUserManagementForm.Location = new System.Drawing.Point(33, 398);
            this.btnUserManagementForm.Name = "btnUserManagementForm";
            this.btnUserManagementForm.Size = new System.Drawing.Size(113, 22);
            this.btnUserManagementForm.TabIndex = 5;
            this.btnUserManagementForm.Text = "User Management";
            this.btnUserManagementForm.UseVisualStyleBackColor = true;
            this.btnUserManagementForm.Click += new System.EventHandler(this.btnUserManagementForm_Click);
            // 
            // btnTransactionManagement
            // 
            this.btnTransactionManagement.Location = new System.Drawing.Point(321, 398);
            this.btnTransactionManagement.Name = "btnTransactionManagement";
            this.btnTransactionManagement.Size = new System.Drawing.Size(144, 22);
            this.btnTransactionManagement.TabIndex = 6;
            this.btnTransactionManagement.Text = "Transaction Management";
            this.btnTransactionManagement.UseVisualStyleBackColor = true;
            this.btnTransactionManagement.Click += new System.EventHandler(this.btnTransactionManagement_Click);
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnTransactionManagement);
            this.Controls.Add(this.btnUserManagementForm);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnReports);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnAbout);
            this.Name = "DashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DashboardForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnUserManagementForm;
        private System.Windows.Forms.Button btnTransactionManagement;
    }
}