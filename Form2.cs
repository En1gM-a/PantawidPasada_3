using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PantawidPasada
{
    public partial class Form2 : Form
    {
        int currentStep = 0;
        UserControl[] steps;

        private personalInfo personalInfo;
        private contact contact;
        private vehicleInfo vehicleInfo;
        private summaryDetails summary;

        private UserData userData = new UserData();
        private SaveDataBase saveDataBase = new SaveDataBase();

        public Form2()
        {
            InitializeComponent();

            personalInfo = new personalInfo();
            personalInfo.Dock = DockStyle.Fill;
            panel1.Controls.Add(personalInfo);

            contact = new contact();
            contact.Dock = DockStyle.Fill;
            contact.Visible = false;
            panel1.Controls.Add(contact);

            vehicleInfo = new vehicleInfo();
            vehicleInfo.Dock = DockStyle.Fill;
            vehicleInfo.Visible = false;
            panel1.Controls.Add(vehicleInfo);

            summary = new summaryDetails();
            summary.Dock = DockStyle.Fill;
            summary.Visible = false;
            panel1.Controls.Add(summary);

            steps = new UserControl[]
            {
                personalInfo,
                contact,
                vehicleInfo,
                summary
            };

            progressBar1.Minimum = 0;
            progressBar1.Maximum = steps.Length - 1;
            progressBar1.Value = 0;
            progressBar1.Style = ProgressBarStyle.Continuous;

            this.BackColor = Color.FromArgb(30, 58, 95);
            ShowStep(currentStep);
        }

        private void ShowStep(int index)
        {
            foreach (var uc in steps)
                uc.Visible = false;

            steps[index].Visible = true;
            steps[index].BringToFront();

            prev.Enabled = index != 0;
            next.Text = (index == steps.Length - 1) ? "Finish" : "Next";

            next.Enabled = true;
            next.BackColor = Color.FromArgb(244, 196, 48);

            progressBar1.Value = index;
        }

        private void next_Click(object sender, EventArgs e)
        {
            if (steps[currentStep] == personalInfo)
                personalInfo.FillData(userData);

            else if (steps[currentStep] == contact)
                contact.FillData(userData);

            else if (steps[currentStep] == vehicleInfo)
                vehicleInfo.FillData(userData);

            if (currentStep < steps.Length - 1)
            {
                currentStep++;

                if (steps[currentStep] == summary)
                    summary.LoadData(userData);

                ShowStep(currentStep);
            }
            else
            {
                saveDataBase.SaveToDB(userData);
                MessageBox.Show(
                    "Sign up successful! Please log in to continue.",
                    "Account Created",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                this.Hide();
                Form1 form1 = new Form1();
                form1.ShowDialog();
                this.Close();
            }
        }

        private void prev_Click(object sender, EventArgs e)
        {
            if (currentStep > 0)
            {
                currentStep--;
                ShowStep(currentStep);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }
    }
}