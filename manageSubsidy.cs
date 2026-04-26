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
    public partial class manageSubsidy : UserControl
    {
        accessDriverInfo driverAccs = new accessDriverInfo();
        int totalSubsidy = 0;   

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
           int nLeftRect, int nTopRect,
           int nRightRect, int nBottomRect,
           int nWidthEllipse, int nHeightEllipse
       );

        private void StylePanel(Panel pnl, int radius = 20)
        {

            pnl.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, pnl.Width, pnl.Height, radius, radius)
            );
        }

        private void setUpManageSub()
        {
            panel1.BackColor = Color.FromArgb(255, 210, 90);
            label2.ForeColor = Color.FromArgb(30, 58, 95);
            giveSub.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, giveSub.Width, giveSub.Height, 20, 20));
            giveSub.BackColor = Color.FromArgb(244, 196, 48);
            panel2.BackColor = Color.FromArgb(30, 58, 95);
            panel3.BackColor = Color.FromArgb(30, 58, 95);
            label1.ForeColor = Color.FromArgb(255, 210, 90);
            label3.ForeColor = Color.FromArgb(255, 210, 90);
            label11.ForeColor = Color.FromArgb(255, 210, 90);
            label10.ForeColor = Color.FromArgb(255, 210, 90);
            label9.ForeColor = Color.FromArgb(255, 210, 90);
            
        }
        public manageSubsidy()
        {
            InitializeComponent();
            setUpManageSub();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            driverAccs.LoadApprovedDriversToDataGrid(dataGridView1);

            totalSubsidy = driverAccs.GetTotalApprovedDrivers();
            label3.Text = $"₱ {totalSubsidy * 5000}";
        }

        private void loadApprovedSubsidies(int adminID)
        {
            string connStr = dataBaseDetails.connStr;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM driverAccs WHERE driver_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", adminID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // PERSONAL
                            label10.Text = $"{reader["first_name"].ToString()} {reader["middle_name"].ToString()} {reader["last_name"].ToString()}";

                            label9.Text = reader["email"].ToString();
                        }
                    }
                }
            }
        }

        private void manageSubsidy_Load(object sender, EventArgs e)
        {
            StylePanel(panel2, 20);
            StylePanel(panel3, 20);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int driverId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["driver_id"].Value);
            loadApprovedSubsidies(driverId);
        }
    }
}
