using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PantawidPasada
{
    public class accessDriverInfo
    {
        private void LoadDrivers(DataGridView datagrid)
        {
            string connStr = "server=localhost;user=root;password=karlbensi12345;database=pantawid_pasada;";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
                SELECT 
                    driver_id,
                    last_name AS 'Last Name',
                    first_name AS 'First Name',
                    IFNULL(LEFT(middle_name, 1), '') AS 'M.I',
                    subsidy_stats AS 'Subsidy Status'
                FROM driverAccs
                WHERE subsidy_stats IN ('Pending', 'Under Review', 'On Hold')";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                datagrid.DataSource = dt;
                if (datagrid.Columns.Contains("driver_id"))
                {
                    datagrid.Columns["driver_id"].Visible = false;
                }
            }
        }

        public void LoadDriversToDataGrid(DataGridView datagrid)
        {
            LoadDrivers(datagrid);
        }
    }
}
