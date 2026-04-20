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
    public partial class fareMatrix : UserControl
    {

        fuelPriceData fuelPrice = new fuelPriceData();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        public fareMatrix()
        {
            InitializeComponent();
            setUpFareMatrix();
            _ = LoadFuelPricesAsync();
        }

        private void StylePanel(Panel pnl, int radius = 20)
        {
            
            pnl.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, pnl.Width, pnl.Height, radius, radius)
            );
        }

        private async Task LoadFuelPricesAsync()
        {
            try
            {
                // 1. Get all prices from the online source
                List<fuelPriceData> allPrices = await fuelPriceONLINE.GetPricesAsync();

                // 2. Sort by date descending
                allPrices.Sort((a, b) => b.Date.CompareTo(a.Date));

                // 3. Take the most recent and second most recent
                var mostRecent = allPrices.Count > 0 ? allPrices[0] : null;
                var secondMostRecent = allPrices.Count > 1 ? allPrices[1] : null;

                // 4. Update 2s safely on UI thread
                this.Invoke((MethodInvoker)(() =>
                {
                    double ringgitToPeso = 14.92; // 1 MYR = 14.92 PHP
                    if (mostRecent != null)
                    {
                        label10.Text = mostRecent.Date.ToString("MMMM dd ,yyyy dddd");


                        label26.Text = $"₱{(mostRecent.dieselPriceOnline * ringgitToPeso).ToString("N2")}";

                        label38.Text = $"₱{fuelPrice.fareCalculation(mostRecent.dieselPriceOnline * ringgitToPeso, 0.1).ToString("N2")}";
                        label39.Text = $"₱{fuelPrice.discountedFare(fuelPrice.fareCalculation(mostRecent.dieselPriceOnline * ringgitToPeso, 0.1), 20).ToString("N2")}";

                        label5.Text = $"₱{fuelPrice.fareCalculation(mostRecent.dieselPriceOnline * ringgitToPeso, 0.5).ToString("N2")}";
                        label11.Text = $"₱{fuelPrice.discountedFare(fuelPrice.fareCalculation(mostRecent.dieselPriceOnline * ringgitToPeso, 0.5), 20).ToString("N2")}";

                        label17.Text = $"₱{fuelPrice.fareCalculation(mostRecent.dieselPriceOnline * ringgitToPeso, 0.04).ToString("N2")}";
                        label24.Text = $"₱{fuelPrice.discountedFare(fuelPrice.fareCalculation(mostRecent.dieselPriceOnline * ringgitToPeso, 0.04), 20).ToString("N2")}";
                    }
                    


                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading fuel prices: " + ex.Message);
            }
        }

        private void setUpFareMatrix()
        {
            panel1.BackColor = Color.FromArgb(255, 210, 90);
            label1.ForeColor = Color.FromArgb(30, 58, 95);
            panel9.BackColor = Color.FromArgb(30, 58, 95);
            panel5.BackColor = Color.FromArgb(30, 58, 95);
            panel12.BackColor = Color.FromArgb(30, 58, 95);

            label15.ForeColor = Color.FromArgb(255, 210, 90);
            label8.ForeColor = Color.FromArgb(255, 210, 90);
            label7.ForeColor = Color.FromArgb(255, 210, 90);
            label16.ForeColor = Color.FromArgb(255,210, 255);
            label19.ForeColor = Color.FromArgb(255, 210, 90);
            label20.ForeColor = Color.FromArgb(255, 210, 90);
            label21.ForeColor = Color.FromArgb(255, 210, 90);
            label19.ForeColor = Color.FromArgb(255, 210, 90);
            label34.ForeColor = Color.FromArgb(255, 210, 90);
            label35.ForeColor = Color.FromArgb(255, 210, 90);
            label16.ForeColor = Color.FromArgb(255, 210, 90);
            
            panel6.BackColor = Color.FromArgb(255, 210, 90);
 
        }

        private void fareMatrix_Load(object sender, EventArgs e)
        {
            StylePanel(panel13, 20);
            StylePanel(panel4, 20);
            StylePanel(panel6, 20);
            StylePanel(panel2, 20);
            StylePanel(panel15, 20);
        }
    }
}
