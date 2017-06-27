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
                        foreach (DataRow row in cdpQuestionDataSet.Tables[i].Rows)
                        {
                            using (SqlCommand cmd = new SqlCommand("CreateSimergyUser", connection))
                            {
                                string passwrod = Convert.ToString(row["Password"]);
                                if (string.IsNullOrWhiteSpace(passwrod))
                                    passwrod = "Simergy1";

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@emailId", emailId);
                                cmd.Parameters.AddWithValue("@password", passwrod);
                                cmd.Parameters.AddWithValue("@firstName", Convert.ToString(row["FirstName"]));
                                cmd.Parameters.AddWithValue("@lastName", Convert.ToString(row["LastName"]));
                                cmd.Parameters.AddWithValue("@company", Convert.ToString(row["Company"]));
                                cmd.Parameters.AddWithValue("@addressLine1", Convert.ToString(row["AddressLine1"]));
                                cmd.Parameters.AddWithValue("@addressLine2", Convert.ToString(row["AddressLine2"]));
                                cmd.Parameters.AddWithValue("@city", Convert.ToString(row["City"]));
                                cmd.Parameters.AddWithValue("@state", Convert.ToString(row["State"]));
                                cmd.Parameters.AddWithValue("@zipCode", Convert.ToString(row["PinCode"]));
                                cmd.Parameters.AddWithValue("@country", Convert.ToString(row["Country"]));
                                cmd.Parameters.AddWithValue("@contactNumber", Convert.ToString(row["ContactNumber"]));
                                cmd.Parameters.AddWithValue("@fax", Convert.ToString(row["Fax"]));
                                cmd.Parameters.AddWithValue("@startDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@endDate", SqlDbType.DateTime).Value = DBNull.Value;

                                bool isUserAlreadyExists = false;
                                using (SqlCommand cmdGetPersonId = new SqlCommand(
                                    string.Format("SELECT p.PersonID from Person p 			INNER JOIN PersonToContact p2c on p.PersonID = p2c.PersonID			INNER JOIN Contact c  on c.ContactID = p2c.ContactID			WHERE c.Description = '{0}'", emailId)
                                    , connection))
                                {
                                    object personId = cmdGetPersonId.ExecuteScalar();
                                    if (personId != null && ((int)personId) > 0)
                                    {
                                        isUserAlreadyExists = true;
                                    }
                                }

                                object result = cmd.ExecuteScalar();
                                if (!isUserAlreadyExists)
                                    logBuilder.AppendLine(string.Format("{0}", emailId));
                                else
                                {
                                    logExistingUserBuilder.AppendLine(string.Format("{0}", emailId));
                                }
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
