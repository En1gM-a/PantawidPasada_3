using MySql.Data.MySqlClient;
using System;

namespace PantawidPasada
{
    public class DatabaseSeeder
    {
        public void SeedGovAccounts()
        {
            string connStr = dataBaseDetails.connStr;
            HashPassword hasher = new HashPassword();
            Random rand = new Random();

            string[] firstNames = {
                "Juan","Maria","Jose","Ana","Luis","Mark","Carlo","Daniel","Francis","Kevin",
                "Angela","Rose","Carla","Stephanie","Liza","Hazel","Irene","Karen","Beverly","Donna",
                "Arnold","Gilbert","Hector","Alvin","Dennis","Rodel","Cesar","Rogelio","Michael","Patrick"
            };

            string[] lastNames = {
                "Santos","Reyes","Cruz","Garcia","Bautista","Flores","Mendoza","Ramos","Torres","Rivera",
                "Lopez","Gonzales","Aquino","Castro","Navarro","Villanueva","Herrera","Vega","Salazar","Perez"
            };

            string[] agencies = {
                "DOTr","DOH","DPWH","DSWD"
            };

            string[] statuses = {
                "Active","Deactivated"
            };

            string hashedPass = hasher.HashPass("wowowin");

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                for (int i = 0; i < 50; i++)
                {
                    string first = firstNames[rand.Next(firstNames.Length)];
                    string last = lastNames[rand.Next(lastNames.Length)];
                    string middle = ((char)('A' + rand.Next(0, 26))).ToString();

                    string unique = Guid.NewGuid().ToString("N").Substring(0, 5);

                    string username = $"gov_{first.ToLower()}{last.ToLower()}{unique}";
                    string email = $"{username}@gov.ph";
                    string contact = "09" + rand.Next(100000000, 999999999);

                    string query = @"
                    INSERT INTO govAccs
                    (FirstName, LastName, MiddleInitial, Agency,
                     Username, Password, govStatus, contactNum, email)
                    VALUES
                    (@first, @last, @middle, @agency,
                     @username, @password, @status, @contact, @email)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@first", first);
                    cmd.Parameters.AddWithValue("@last", last);
                    cmd.Parameters.AddWithValue("@middle", middle);
                    cmd.Parameters.AddWithValue("@agency", agencies[rand.Next(agencies.Length)]);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPass);
                    cmd.Parameters.AddWithValue("@status", statuses[rand.Next(statuses.Length)]);
                    cmd.Parameters.AddWithValue("@contact", contact);
                    cmd.Parameters.AddWithValue("@email", email);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}