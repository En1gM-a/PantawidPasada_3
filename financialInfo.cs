using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PantawidPasada
{
    public partial class financialInfo : UserControl
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
        int nLeftRect, int nTopRect,
        int nRightRect, int nBottomRect,
        int nWidthEllipse, int nHeightEllipse
        );

        public financialInfo()
        {
            InitializeComponent();
            setUpFinancialInfo();
        }

        // =========================
        // 🔥 VALIDATION REMOVED
        // =========================
        public bool IsValid()
        {
            return true; // always allow Next
        }

        private void setUpFinancialInfo()
        {
            StyleTextBox(sourceOfIncome, "Other Source of Income");
            StyleTextBox(financialObligation, "Other Financial Obligation (loans, etc.)");

            // Monthly Income
            income.ForeColor = Color.Gray;
            income.SelectedIndex = 0;
            income.SelectedIndexChanged += (s, e) =>
            {
                income.ForeColor = income.SelectedIndex == 0 ? Color.Gray : Color.Black;
            };
            income.FlatStyle = FlatStyle.Flat;
            income.BackColor = Color.FromArgb(248, 250, 252);

            // Employment Type
            employmentType.ForeColor = Color.Gray;
            employmentType.SelectedIndex = 0;
            employmentType.SelectedIndexChanged += (s, e) =>
            {
                employmentType.ForeColor = employmentType.SelectedIndex == 0 ? Color.Gray : Color.Black;
            };
            employmentType.FlatStyle = FlatStyle.Flat;
            employmentType.BackColor = Color.FromArgb(248, 250, 252);

            // Apply rounded corners
            StyleControl(sourceOfIncome);
            StyleControl(financialObligation);
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
            data.Income = income.Text;
            data.EmploymentType = employmentType.Text;
            data.SourceOfIncome = sourceOfIncome.Text;
            data.FinancialObligation = financialObligation.Text;
        }
        private void financialInfo_Load(object sender, EventArgs e)
        {

        }
    }
}