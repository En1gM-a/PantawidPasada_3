using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PantawidPasada
{
    public partial class manageAccAdmin : UserControl
    {

        accessAdminGovAccs accessAdminGovAccs = new accessAdminGovAccs();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        public manageAccAdmin()
        {
            InitializeComponent();
        }

        public manageAccAdmin(adminAcc acc)
        {
            InitializeComponent();
            setUpManageAcc();


            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            accessAdminGovAccs.LoadAdminsToDataGrid(dataGridView1);
            accessAdminGovAccs.LoadGovsToDataGrid(dataGridView2);

            if (acc.role != "Super Admin")
            {
                MakeGridFaded(dataGridView1);

            }


        }

        private void MakeGridFaded(DataGridView grid)
        {
            grid.Enabled = false;

            grid.DefaultCellStyle.ForeColor = Color.Gray;
            grid.DefaultCellStyle.SelectionForeColor = Color.Gray;
            grid.DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 245, 245);

            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);

            grid.EnableHeadersVisualStyles = false;
        }

        // =========================
        // ROLE RADIO BUTTON LOGIC
        // Agency options disabled when Admin is selected
        // =========================
        private void UpdateAgencyVisibility()
        {
            bool isGov = radioButton2.Checked;

            // Enable/disable agency radio buttons
            radioButton3.Enabled = isGov;
            radioButton4.Enabled = isGov;
            radioButton5.Enabled = isGov;
            radioButton6.Enabled = isGov;

            // Visually dim them when disabled
            Color activeColor = Color.FromArgb(30, 58, 95);
            Color dimColor = Color.Gray;

            radioButton3.ForeColor = isGov ? activeColor : dimColor;
            radioButton4.ForeColor = isGov ? activeColor : dimColor;
            radioButton5.ForeColor = isGov ? activeColor : dimColor;
            radioButton6.ForeColor = isGov ? activeColor : dimColor;

            // Clear agency selection if switching to Admin
            if (!isGov)
            {
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                radioButton5.Checked = false;
                radioButton6.Checked = false;
            }
        }

        private void radioButtonAdmin_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAgencyVisibility();
        }

        private void radioButtonGov_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAgencyVisibility();
        }

        private void setUpManageAcc()
        {
            panel1.BackColor = Color.FromArgb(255, 210, 90);
            label2.ForeColor = Color.FromArgb(30, 58, 95);
            saveButton.BackColor = Color.FromArgb(244, 196, 48);
            label21.ForeColor = Color.FromArgb(30, 58, 95);
            label20.ForeColor = Color.FromArgb(30, 58, 95);
            saveButton.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, saveButton.Width, saveButton.Height, 20, 20));
            updateStatus.BackColor = Color.FromArgb(244, 196, 48);
            updateStatus.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, updateStatus.Width, updateStatus.Height, 20, 20));

        }

        private void setStatus()
        {
            radioButton7.Enabled = true;
            radioButton8.Enabled = true;

            if (label18.Text == "Active")
                radioButton7.Enabled = false;
            else
                radioButton8.Enabled = false;
        }

        private void StylePanel(Panel pnl, int radius = 20)
        {

            pnl.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, pnl.Width, pnl.Height, radius, radius)
            );
        }

        private void manageAccAdmin_Load(object sender, EventArgs e)
        {
            StylePanel(panel3, 20);
            StylePanel(panel2, 20);
        }
        private void LoadAdminDetails(int adminID)
        {
            string connStr = dataBaseDetails.connStr;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM admins WHERE AdminID = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", adminID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // PERSONAL
                            label16.Text = $"{reader["FirstName"].ToString()} {reader["MiddleInitial"].ToString()} {reader["LastName"].ToString()}";
                            label17.Text = reader["RoleAdmin"].ToString();
                            label18.Text = reader["adminStatus"].ToString();


                        }
                    }
                }
            }

            setStatus();
        }
        private void LoadGovDetails(int govID)
        {
            string connStr = dataBaseDetails.connStr;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM govAccs WHERE GovID = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", govID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // PERSONAL
                            label16.Text = $"{reader["FirstName"].ToString()} {reader["MiddleInitial"].ToString()} {reader["LastName"].ToString()}";
                            label17.Text = reader["Agency"].ToString();
                            label18.Text = reader["govStatus"].ToString();


                        }
                    }
                }
            }

            setStatus();
        }
      

        private bool _loadingFromGrid1 = false;
        private bool _loadingFromGrid2 = false;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            _loadingFromGrid2 = true;
            dataGridView2.ClearSelection();
            _loadingFromGrid2 = false;

            _selectedSource = "admin";   // ADD
            _selectedID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["AdminID"].Value);
            LoadAdminDetails(_selectedID);
            
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            _loadingFromGrid1 = true;
            dataGridView1.ClearSelection();
            _loadingFromGrid1 = false;

            _selectedSource = "gov";     // ADD
            _selectedID = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["GovID"].Value);
            LoadGovDetails(_selectedID);
            
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // Check role selected
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Please select a role (Admin or Government Official).",
                    "Missing Role", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check required fields
            if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text) ||

                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please fill in all required fields (First Name, Last Name, Username, Password, Contact Nummber, Email).",
                    "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // If Gov, must select agency
            if (radioButton2.Checked)
            {
                if (!radioButton3.Checked && !radioButton4.Checked &&
                    !radioButton5.Checked && !radioButton6.Checked)
                {
                    MessageBox.Show("Please select an agency for the Government Official account.",
                        "Missing Agency", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Save based on role
            if (radioButton1.Checked)
                SaveAdminAccount();
            else
                SaveGovAccount();
        }

        private void SaveAdminAccount()
        {
            string connStr = dataBaseDetails.connStr;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO admins
                        (FirstName, LastName, MiddleInitial, RoleAdmin, UsernameAdmin, PasswordAdmin, adminStatus, contactNum, email)
                        VALUES
                        (@firstName, @lastName, @middleInit, @role, @username, @password, @status, @contact, @email)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@firstName", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@lastName", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@middleInit", string.IsNullOrWhiteSpace(textBox3.Text) ? "" : textBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@role", "Admin");
                    cmd.Parameters.AddWithValue("@username", label8.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", textBox4.Text);


                    cmd.Parameters.AddWithValue("@status", "Active");
                    cmd.Parameters.AddWithValue("@contact", textBox5.Text);
                    cmd.Parameters.AddWithValue("@email", textBox6.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(
                    $"Admin account for {textBox2.Text} {textBox1.Text} has been created successfully!",
                    "Account Created", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
                accessAdminGovAccs.LoadAdminsToDataGrid(dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving admin account: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================
        // SAVE GOV
        // =========================
        private void SaveGovAccount()
        {
            string connStr = dataBaseDetails.connStr;

            // Get selected agency
            string agency = "";
            if (radioButton3.Checked) agency = "LTFRB";
            else if (radioButton4.Checked) agency = "LTO";
            else if (radioButton5.Checked) agency = "DOTr";
            else if (radioButton6.Checked) agency = "DSWD";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO govAccs
                        (FirstName, LastName, MiddleInitial, Agency, Username, Password, govStatus, contactNum, email)
                        VALUES
                        (@firstName, @lastName, @middleInit, @agency, @username, @password, @status, @contact, @email)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@firstName", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@lastName", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@middleInit", string.IsNullOrWhiteSpace(textBox3.Text) ? "" : textBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@agency", agency);
                    cmd.Parameters.AddWithValue("@username", label8.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", textBox4.Text);
                    cmd.Parameters.AddWithValue("@status", "Active");
                    cmd.Parameters.AddWithValue("@contact", textBox5.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", textBox6.Text.Trim());


                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(
                    $"Government account for {textBox2.Text} {textBox1.Text} ({agency}) has been created successfully!",
                    "Account Created", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
                accessAdminGovAccs.LoadGovsToDataGrid(dataGridView2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving government account: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================
        // CLEAR FORM AFTER SAVE
        // =========================
        private void ClearForm()
        {
            textBox2.Clear();
            textBox1.Clear();
            textBox1.Clear();

            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            UpdateAgencyVisibility();
        }

        private void GenerateUsername()
        {
            string firstName = textBox2.Text.Trim().ToLower().Replace(" ", "");
            string lastName = textBox1.Text.Trim().ToLower().Replace(" ", "");

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                label8.Text = "user";
                return;
            }

            if (radioButton1.Checked)
                label8.Text = $"admin@{firstName}.{lastName}";
            else if (radioButton2.Checked)
                label8.Text = $"gov@{firstName}.{lastName}";
            else
                label8.Text = "user";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            GenerateUsername();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GenerateUsername();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAgencyVisibility();
            GenerateUsername();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAgencyVisibility();
            GenerateUsername();
        }

        private string _selectedSource = "";
        private int _selectedID = -1;

        private void updateStatus_Click(object sender, EventArgs e)
        {
            if (_selectedID == -1 || string.IsNullOrEmpty(_selectedSource))
            {
                MessageBox.Show("Please select an account from the table first.",
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!radioButton7.Checked && !radioButton8.Checked)
            {
                MessageBox.Show("Please select a status (Active or Deactivated).",
                    "No Status Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newStatus = radioButton7.Checked ? "Active" : "Deactivated";

            // Prevent setting the same status
            if (newStatus == label18.Text)
            {
                MessageBox.Show($"Account is already {newStatus}.",
                    "No Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string connStr = dataBaseDetails.connStr;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = _selectedSource == "admin"
                        ? "UPDATE admins SET adminStatus = @status WHERE AdminID = @id"
                        : "UPDATE govAccs SET govStatus = @status WHERE GovID = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@id", _selectedID);
                        cmd.ExecuteNonQuery();
                    }
                }

                label18.Text = newStatus;

                MessageBox.Show($"Account status updated to {newStatus} successfully!",
                    "Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh the correct grid
                if (_selectedSource == "admin")
                    accessAdminGovAccs.LoadAdminsToDataGrid(dataGridView1);
                else
                    accessAdminGovAccs.LoadGovsToDataGrid(dataGridView2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating status: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
