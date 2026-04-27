using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PantawidPasada
{
    public partial class financialInfo : UserControl
    {

        private UserData userData;
        private financialInfo financialInfoControl;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
        int nLeftRect, int nTopRect,
        int nRightRect, int nBottomRect,
        int nWidthEllipse, int nHeightEllipse
        );



        public financialInfo()
        {
            
        }
        public financialInfo(UserData data)
        {
            InitializeComponent();
            setUpFinancialInfo();
            this.BackColor = Color.FromArgb(30, 58, 95);
            userData = data;
        }

        // =========================
        // 🔥 VALIDATION REMOVED
        // =========================
        public bool IsValid()
        {
            return true; // always allow Next
        }

        private void setUpFinancialInfo()
        {
            StyleTextBox(sourceOfIncome, "Other Source of Income");
            StyleTextBox(financialObligation, "Other Financial Obligation (loans, etc.)");

            // Monthly Income
            income.ForeColor = Color.Gray;
            income.SelectedIndex = 0;
            income.SelectedIndexChanged += (s, e) =>
            {
                income.ForeColor = income.SelectedIndex == 0 ? Color.Gray : Color.Black;
            };
            income.FlatStyle = FlatStyle.Flat;
            income.BackColor = Color.FromArgb(248, 250, 252);

            // Employment Type
            employmentType.ForeColor = Color.Gray;
            employmentType.SelectedIndex = 0;
            employmentType.SelectedIndexChanged += (s, e) =>
            {
                employmentType.ForeColor = employmentType.SelectedIndex == 0 ? Color.Gray : Color.Black;
            };
            employmentType.FlatStyle = FlatStyle.Flat;
            employmentType.BackColor = Color.FromArgb(248, 250, 252);

            // Apply rounded corners
            StyleControl(sourceOfIncome);
            StyleControl(financialObligation);
        }

        private void StyleTextBox(TextBox txt, string placeholder)
        {
            txt.BorderStyle = BorderStyle.None;
            txt.BackColor = Color.FromArgb(248, 250, 252);
            txt.ForeColor = Color.Black;
            txt.Font = new Font("Segoe UI", 20);
            txt.PlaceholderText = placeholder;
        }

        private void StyleControl(TextBox txt)
        {
            txt.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, txt.Width, txt.Height, 10, 10)
            );
        }

        public void FillData(UserData data)
        {
            data.Income = income.Text;
            data.EmploymentType = employmentType.Text;
            data.SourceOfIncome = sourceOfIncome.Text;
            data.FinancialObligation = financialObligation.Text;
        }
        private void financialInfo_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if(string.IsNullOrWhiteSpace(income.Text) || string.IsNullOrWhiteSpace(employmentType.Text) )
            {
                MessageBox.Show("Please fill up all required fields.", "Incomplete Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(string.IsNullOrWhiteSpace(sourceOfIncome.Text))
            {
                sourceOfIncome.Text = "N/A";
            }

            if (string.IsNullOrWhiteSpace(financialObligation.Text))
            {
                financialObligation.Text = "N/A";
            }

            financialInfoControl.FillData(userData);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(dataBaseDetails.connStr))
                {
                    conn.Open();

                    string query = @"UPDATE driverAccs 
                                     SET income = @income,
                                         employment_type = @employment,
                                         source_of_income = @source,
                                         finan_ob = @obligation
                                     WHERE usernameUser = @username";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@income", userData.Income);
                    cmd.Parameters.AddWithValue("@employment", userData.EmploymentType);
                    cmd.Parameters.AddWithValue("@source", userData.SourceOfIncome);
                    cmd.Parameters.AddWithValue("@obligation", userData.FinancialObligation);
                    cmd.Parameters.AddWithValue("@username", userData.username);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        
    
    }
    }
}