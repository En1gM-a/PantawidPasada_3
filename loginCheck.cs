using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace PantawidPasada
{
    public class loginCheck
    {
        private string connStr = dataBaseDetails.connStr;
        UserData userData = new UserData();
        adminAcc adminData = new adminAcc();
        govData dataGov = new govData();
        HashPassword hash = new HashPassword();
        public string LoginError { get; private set; } = "";


        // Returns true if login successful, false otherwise

        protected bool CheckLoginUser(string? username, string? password, UserData data)
        {
            try
            {
                string hashedPassword = hash.HashPass(password);

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = "SELECT * FROM driverAccs WHERE usernameUser=@username AND passwordUser=@password";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            // 🔥 FILL THE PASSED OBJECT
                            data.FirstName = reader["first_name"].ToString();
                            data.LastName = reader["last_name"].ToString();
                            data.MiddleName = reader["middle_name"].ToString();
                            data.Address = reader["address"].ToString();
                            data.Province = reader["province"].ToString();
                            data.Phone = reader["phone_num"].ToString();
                            data.Email = reader["email"].ToString();
                            data.username = reader["usernameUser"].ToString();
                            data.Password = reader["passwordUser"].ToString();
                            data.Income = reader["income"].ToString();
                            data.EmploymentType = reader["employment_type"].ToString();
                            data.SourceOfIncome = reader["source_of_income"].ToString();
                            data.FinancialObligation = reader["finan_ob"].ToString();
                            data.PlateNumber = reader["plate_number"].ToString();
                            data.LicenseNumber = reader["lic_num"].ToString();
                            data.VehicleType = reader["vehicle_type"].ToString();
                            data.subsidyStatus = reader["subsidy_stats"].ToString();
                            data.createDay = reader["created_at"].ToString();
                            data.reason = reader["reason"].ToString();

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking login: " + ex.Message);
                return false;
            }
        }

        protected bool CheckLoginAdmin(string? username, string? password, adminAcc data)
        {
            try
            {
                string hashedPassword = hash.HashPass(password);

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = "SELECT * FROM admins WHERE UsernameAdmin=@username AND PasswordAdmin=@password";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            // 🔥 FILL THE PASSED OBJECT
                            data.FirstName = reader["FirstName"].ToString();
                            data.LastName = reader["LastName"].ToString();
                            data.MiddleInit = reader["MiddleInitial"].ToString();
                            data.role = reader["RoleAdmin"].ToString();
                            data.username = reader["UsernameAdmin"].ToString();
                            data.email = reader["email"].ToString();
                            data.phoneNum = reader["contactNum"].ToString();
                            data.createDay = reader["CreatedAt"].ToString();
                            data.status = reader["adminStatus"].ToString();


                            if (reader["adminStatus"].ToString() == "Deactivated")
                            {
                                LoginError = "deactivated";
                                return false;
                            }


                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking login: " + ex.Message);
                return false;
            }
        }

        protected bool CheckLoginGov(string? username, string? password, govData data)
        {
            try
            {
                string hashedPassword = hash.HashPass(password);

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = "SELECT * FROM govAccs WHERE Username=@username AND Password=@password";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            // 🔥 FILL THE PASSED OBJECT
                            data.firstName = reader["FirstName"].ToString();
                            data.lastName = reader["LastName"].ToString();
                            data.middleInit = reader["MiddleInitial"].ToString();
                            data.agency = reader["Agency"].ToString();
                            data.username = reader["Username"].ToString();
                            data.govStats = reader["govStatus"].ToString();
                            data.contactNum = reader["contactNum"].ToString();
                            data.email = reader["email"].ToString();
                            data.createDay = reader["CreatedAt"].ToString();

                            if (reader["govStatus"].ToString() == "Deactivated")
                            {
                                LoginError = "deactivated";
                                return false;
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking login: " + ex.Message);
                return false;
            }
        }

        protected bool CheckLoginFuelEditor(string? username, string? password, fuelEditorData data)
        {
            try
            {

                string hashedPassword = hash.HashPass(password);
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                SELECT * 
                FROM fuelEditors 
                WHERE username = @username 
                AND password = @password";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            // 🔥 Fill object
                            data.editor_id = Convert.ToInt32(reader["editor_id"]);
                            data.name = reader["name"].ToString();
                            data.username = reader["username"].ToString();
                            data.status = reader["status"].ToString();

                            // 🔥 Check status
                            if (data.status == "Deactivated")
                            {
                                LoginError = "deactivated";
                                return false;
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking fuel editor login: " + ex.Message);
                return false;
            }
        }

        public bool loginUser(string? user, string? pass, UserData data)
        {
            return CheckLoginUser(user, pass, data);
        }
        public bool loginAdmin(string? user, string? pass, adminAcc data)
        {
            return CheckLoginAdmin(user, pass, data);
        }

        public bool loginGov(string? user, string? pass, govData data)
        {
            return CheckLoginGov(user, pass, data);
        }

        public bool loginFuelEditor(string? user, string? pass, fuelEditorData data)
        {
            return CheckLoginFuelEditor(user, pass, data);
        }
    }
}
