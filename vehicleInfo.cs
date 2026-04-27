using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PantawidPasada
{
    public partial class vehicleInfo : UserControl
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        public vehicleInfo()
        {
            InitializeComponent();
            setUpVehicleInfo();
        }

        // =========================
        // 🔥 VALIDATION REMOVED
        // =========================
        public bool IsValid()
        {
            return true; // always allow Next
        }

        private void setUpVehicleInfo()
        {
            StyleTextBox(PlateNo, "Plate Number");
            StyleTextBox(DriverLicense, "Driver's License Number");

            // ComboBox setup
            vehicleType.ForeColor = Color.Gray;
            vehicleType.SelectedIndex = 0;

            vehicleType.SelectedIndexChanged += (s, e) =>
            {
                vehicleType.ForeColor = vehicleType.SelectedIndex == 0
                    ? Color.Gray
                    : Color.Black;
            };

            vehicleType.FlatStyle = FlatStyle.Flat;
            vehicleType.BackColor = Color.FromArgb(248, 250, 252);

            // Rounded corners
            StyleControl(PlateNo);
            StyleControl(DriverLicense);
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
            data.PlateNumber = PlateNo.Text;
            data.LicenseNumber = DriverLicense.Text;
            data.VehicleType = vehicleType.Text;
        }

        public bool ISfilled()
        {
            if(string.IsNullOrWhiteSpace(vehicleType.Text) || string.IsNullOrWhiteSpace(DriverLicense.Text) || string.IsNullOrWhiteSpace(PlateNo.Text))
            {
                return false;
            }
            return true;
        }

        private void vehicleInfo_Load(object sender, EventArgs e)
        {

        }
    }
}