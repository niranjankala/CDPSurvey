using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDPReporting.QuestionsManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCreateUpdateQuestions_Click(object sender, EventArgs e)
        {
            try
            {
                //string filepath = String.Empty;               
                if (string.IsNullOrEmpty(txtSpreadSheetPath.Text))
                {
                    MessageBox.Show("Please select CDP questions details spreadsheet file");
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                DataSet cdpQuestionDataSet = LoadDataSetFromExcel(txtSpreadSheetPath.Text);
                string connString = GetSqlConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    for (int i = 0; i < cdpQuestionDataSet.Tables.Count; i++)
                    {
                        if (cdpQuestionDataSet.Tables[i].TableName != "Questions$") continue;
                        foreach (DataRow row in cdpQuestionDataSet.Tables[i].Rows)
                        {
                            using (SqlCommand cmd = new SqlCommand("USP_CreateQuestion", connection))
                            {

                                cmd.CommandType = CommandType.StoredProcedure;

                                string qId = Convert.ToString(row["QId"]);

                                cmd.Parameters.AddWithValue("@QId", string.IsNullOrEmpty(qId) ? Guid.NewGuid() : new Guid(qId));
                                cmd.Parameters.AddWithValue("@QuestionId", Convert.ToString(row["QuestionId"]));
                                cmd.Parameters.AddWithValue("@Title", Convert.ToString(row["Title"]));
                                cmd.Parameters.AddWithValue("@QuestionText", Convert.ToString(row["QuestionText"]));
                                cmd.Parameters.AddWithValue("@Type", Convert.ToString(row["Type"]));
                                cmd.Parameters.AddWithValue("@QuestionOrder", Convert.ToInt32(row["QuestionOrder"]));
                                cmd.Parameters.AddWithValue("@QuestionGroupId", new Guid(Convert.ToString(row["QuestionGroupId"])));
                                cmd.Parameters.AddWithValue("@ParentQuestionId",  DBNull.Value);
                                cmd.Parameters.AddWithValue("@IsQuestionSubItem", Convert.ToString(row["IsQuestionSubItem"]) == "Yes" ? true : false);
                                cmd.Parameters.AddWithValue("@EligibleForPrePopulation", Convert.ToString(row["EligibleForPrePopulation"]) == "Yes" ? true : false);
                                cmd.Parameters.AddWithValue("@ApplicableStatus", Convert.ToString(row["ApplicableStatus"]) == "Yes"? true:false);
                                cmd.Parameters.AddWithValue("@OptionList", Convert.ToString(row["OptionsList"]));
                                cmd.Parameters.AddWithValue("@OptionOtherText", Convert.ToString(row["OptionOthersText"]));

                                object result = cmd.ExecuteScalar();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }

        }

        private void btnBrowseQuestionsExcelFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {

                openFileDialog1.Title = "Select the spreadsheet to generate XML";
                openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                openFileDialog1.FileName = "CDPQuestions.xlsx";

                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.

                if (result == DialogResult.OK)
                {
                    txtSpreadSheetPath.Text = openFileDialog1.FileName;

                    Properties.Settings.Default.SpreadsheetPath = txtSpreadSheetPath.Text;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(Properties.Settings.Default.SpreadsheetPath))
                txtSpreadSheetPath.Text = Properties.Settings.Default.SpreadsheetPath;
        }


        private DataSet LoadDataSetFromExcel(string excelFilePath)
        {
            DataSet myDataset = new DataSet();

            if (!string.IsNullOrWhiteSpace(excelFilePath))
            {
                DataTable dt = new DataTable();
                string connectionString = string.Format(@"provider=Microsoft.Ace.OLEDB.12.0;data source={0};Extended Properties=Excel 12.0;", excelFilePath);
                OleDbConnection objConn = new OleDbConnection(connectionString);
                objConn.Open();
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);


                //Read From EXCEL and Create DataSet

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    string tablename = "";
                    DataRow dr;
                    dr = dt.Rows[i];
                    tablename = dr["TABLE_NAME"].ToString().Trim();
                    if (tablename != null)
                    {
                        DataTable librariesDt = new DataTable();
                        librariesDt.TableName = tablename;
                        if (!librariesDt.TableName.ToString().Contains("_xlnm#_FilterDatabase"))
                        {
                            myDataset.Tables.Add(librariesDt);
                            OleDbDataAdapter objAdp = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", tablename), objConn);
                            objAdp.Fill(myDataset.Tables[tablename]);
                            objAdp.Dispose();
                        }
                    }
                }
            }
            return myDataset;
        }
        string GetSqlConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DBConnctionString"].ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
            {

            }
            return connectionString;
        }

    }
}
