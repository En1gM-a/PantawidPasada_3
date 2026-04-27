using System;
using System.Collections.Generic;
using System.Text;

namespace PantawidPasada
{
    public class UserData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string? birthday { get; set; }

        // Contact
        public string Phone { get; set; }
        public string Email { get; set; }
        public string username { get; set; }
        public string Password { get; set; }

        // Financial
        public string Income { get; set; }
        public string EmploymentType { get; set; }
        public string SourceOfIncome { get; set; }
        public string FinancialObligation { get; set; }

        // Vehicle
        public string PlateNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string VehicleType { get; set; }
        public string subsidyStatus { get; set; }
        public string createDay { get; set; }

        public string? reason { get; set; }
    }
}
