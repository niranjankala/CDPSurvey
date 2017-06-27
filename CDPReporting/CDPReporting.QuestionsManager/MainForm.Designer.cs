namespace CDPReporting.QuestionsManager
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseQuestionsExcelFile = new System.Windows.Forms.Button();
            this.txtSpreadSheetPath = new System.Windows.Forms.TextBox();
            this.btnCreateUpdateQuestions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Spreadsheet: CDPQuestions.xlsx";
            // 
            // btnBrowseQuestionsExcelFile
            // 
            this.btnBrowseQuestionsExcelFile.Location = new System.Drawing.Point(551, 28);
            this.btnBrowseQuestionsExcelFile.Name = "btnBrowseQuestionsExcelFile";
            this.btnBrowseQuestionsExcelFile.Size = new System.Drawing.Size(79, 20);
            this.btnBrowseQuestionsExcelFile.TabIndex = 8;
            this.btnBrowseQuestionsExcelFile.Text = "Browse";
            this.btnBrowseQuestionsExcelFile.UseVisualStyleBackColor = true;
            this.btnBrowseQuestionsExcelFile.Click += new System.EventHandler(this.btnBrowseQuestionsExcelFile_Click);
            // 
            // txtSpreadSheetPath
            // 
            this.txtSpreadSheetPath.Location = new System.Drawing.Point(12, 28);
            this.txtSpreadSheetPath.Name = "txtSpreadSheetPath";
            this.txtSpreadSheetPath.Size = new System.Drawing.Size(533, 20);
            this.txtSpreadSheetPath.TabIndex = 7;
            // 
            // btnCreateUpdateQuestions
            // 
            this.btnCreateUpdateQuestions.Location = new System.Drawing.Point(219, 82);
            this.btnCreateUpdateQuestions.Name = "btnCreateUpdateQuestions";
            this.btnCreateUpdateQuestions.Size = new System.Drawing.Size(172, 34);
            this.btnCreateUpdateQuestions.TabIndex = 8;
            this.btnCreateUpdateQuestions.Text = "Create/Update Questions";
            this.btnCreateUpdateQuestions.UseVisualStyleBackColor = true;
            this.btnCreateUpdateQuestions.Click += new System.EventHandler(this.btnCreateUpdateQuestions_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 150);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCreateUpdateQuestions);
            this.Controls.Add(this.btnBrowseQuestionsExcelFile);
            this.Controls.Add(this.txtSpreadSheetPath);
            this.Name = "MainForm";
            this.Text = "Create/Update Questions ";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseQuestionsExcelFile;
        private System.Windows.Forms.TextBox txtSpreadSheetPath;
        private System.Windows.Forms.Button btnCreateUpdateQuestions;
    }
}

