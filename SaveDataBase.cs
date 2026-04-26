using MySql.Data.MySqlClient;
using PantawidPasada;
using System;
using System.Windows.Forms;

public class SaveDataBase
{
    string connStr = dataBaseDetails.connStr;

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
                 plate_number, lic_num, vehicle_type, subsidy_stats)
            VALUES
                (@fname, @lname, @mname, @address, @province,
                 @phone, @email, @user, @password,
                 @plate, @license, @vehicle, 'Not Requested')";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@fname", data.FirstName);
                cmd.Parameters.AddWithValue("@lname", data.LastName);
                cmd.Parameters.AddWithValue("@mname", data.MiddleName);
                cmd.Parameters.AddWithValue("@address", data.Address);
                cmd.Parameters.AddWithValue("@province", data.Province);
                cmd.Parameters.AddWithValue("@phone", data.Phone);
                cmd.Parameters.AddWithValue("@email", data.Email);
                cmd.Parameters.AddWithValue("@user", data.username);
                cmd.Parameters.AddWithValue("@password", data.Password);
                cmd.Parameters.AddWithValue("@plate", data.PlateNumber);
                cmd.Parameters.AddWithValue("@license", data.LicenseNumber);
                cmd.Parameters.AddWithValue("@vehicle", data.VehicleType);

                cmd.ExecuteNonQuery();
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