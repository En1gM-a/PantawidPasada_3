using MySql.Data.MySqlClient;
using PantawidPasada;
using System;
using System.Windows.Forms;

public class SaveDataBase
{
    string connStr = "server=localhost;user id=root;password=karlbensi12345;database=pantawid_pasada;";

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(connStr);
    }

    protected void SaveToDatabase(UserData data)
    {


        using (MySqlConnection conn = new MySqlConnection(connStr))
        {
            try
            {
                conn.Open();

                string query = @"INSERT INTO driverAccs
            (first_name, last_name, middle_name, address, province,
             phone_num, email, usernameUser, passwordUser,
             income, employment_type, source_of_income, finan_ob,
             plate_number, lic_num, vehicle_type, subsidy_stats)
             
             VALUES
            (@fname, @lname, @mname, @address, @province,
             @phone, @email, @user, @password,
             @income, @employment, @source, @obligation,
             @plate, @license, @vehicle, @subStat)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                // 🔥 PARAMETERS (VERY IMPORTANT)
                cmd.Parameters.AddWithValue("@fname", data.FirstName);
                cmd.Parameters.AddWithValue("@lname", data.LastName);
                cmd.Parameters.AddWithValue("@mname", data.MiddleName);
                cmd.Parameters.AddWithValue("@address", data.Address);
                cmd.Parameters.AddWithValue("@province", data.Province);

                cmd.Parameters.AddWithValue("@phone", data.Phone);
                cmd.Parameters.AddWithValue("@email", data.Email);
                cmd.Parameters.AddWithValue("@user", data.username);
                cmd.Parameters.AddWithValue("@password", data.Password);

                cmd.Parameters.AddWithValue("@income", data.Income);
                cmd.Parameters.AddWithValue("@employment", data.EmploymentType);
                cmd.Parameters.AddWithValue("@source", data.SourceOfIncome);
                cmd.Parameters.AddWithValue("@obligation", data.FinancialObligation);

                cmd.Parameters.AddWithValue("@plate", data.PlateNumber);
                cmd.Parameters.AddWithValue("@license", data.LicenseNumber);
                cmd.Parameters.AddWithValue("@vehicle", data.VehicleType);
                cmd.Parameters.AddWithValue("@subStat", "Pending");

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data saved successfully! ✅");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }

        public void SaveToDB(UserData data)
        {
            SaveToDatabase(data);
        }

}