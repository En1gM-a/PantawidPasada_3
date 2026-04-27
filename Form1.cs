using System;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace PantawidPasada
{
    public partial class Form1 : Form
    {

      

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int EM_SETMARGINS = 0xD3;
        private const int EC_LEFTMARGIN = 0x1;
        private const int EC_RIGHTMARGIN = 0x2;

        private void SetTextBoxLeftPadding(TextBox txt, int leftPadding)
        {
            // lParam = (right << 16) | left
            int lParam = (0 << 16) | leftPadding;
            SendMessage(txt.Handle, EM_SETMARGINS, EC_LEFTMARGIN | EC_RIGHTMARGIN, lParam);
        }

        public static class TextBoxHelper
        {
            // Windows API to set textbox margins
            [DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

            private const int EM_SETMARGINS = 0xD3;
            private const int EC_LEFTMARGIN = 0x1;
            private const int EC_RIGHTMARGIN = 0x2;

            /// <summary>
            /// Applies rounded style + inner padding to any TextBox.
            /// </summary>
            public static void StyleTextBox(
                TextBox txt,
                string placeholder = "",
                int fontSize = 11,
                int leftMargin = 20,
                int rightMargin = 12,
                int cornerRadius = 10,
                Color? backColor = null,
                Color? foreColor = null)
            {
                txt.BorderStyle = BorderStyle.None;
                txt.BackColor = backColor ?? Color.FromArgb(248, 250, 252);
                txt.ForeColor = foreColor ?? Color.Black;
                txt.Font = new Font("Segoe UI", fontSize);

                if (!string.IsNullOrEmpty(placeholder))
                    txt.PlaceholderText = placeholder;

                // Apply rounded clip region
                txt.Region = Region.FromHrgn(
                    CreateRoundRectRgn(0, 0, txt.Width, txt.Height, cornerRadius, cornerRadius)
                );

                // Push cursor and text inward so it doesn't get clipped
                SendMessage(txt.Handle, EM_SETMARGINS, EC_LEFTMARGIN | EC_RIGHTMARGIN,
                    MakeLParam(leftMargin, rightMargin));
            }

            // Helper to pack two 16-bit values into one 32-bit int
            private static int MakeLParam(int low, int high)
                => (high << 16) | (low & 0xFFFF);

            [DllImport("gdi32.dll")]
            private static extern IntPtr CreateRoundRectRgn(
                int nLeftRect, int nTopRect,
                int nRightRect, int nBottomRect,
                int nWidthEllipse, int nHeightEllipse);
        }

        public Form1()
        {
            InitializeComponent();
            panel1.BackColor = Color.FromArgb(30, 58, 95);
            label3.ForeColor = Color.FromArgb(30, 58, 95);

            this.Shown += (s, e) =>
            {
                this.ActiveControl = null;
            };
            SetupLoginUI();
        }

        private void SetupLoginUI()
        {



            // USERNAME TextBox
            TextBoxHelper.StyleTextBox(usernameLogin, placeholder: "Username");
            usernameLogin.BorderStyle = BorderStyle.None;
            usernameLogin.BackColor = Color.FromArgb(248, 250, 252);
            usernameLogin.ForeColor = Color.Black;
            usernameLogin.Font = new Font("Segoe UI", 20);
            
        
            usernameLogin.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, usernameLogin.Width, usernameLogin.Height, 10, 10));


            // PASSWORD TextBox
            TextBoxHelper.StyleTextBox(passwordLogin, placeholder: "Password");
            passwordLogin.BorderStyle = BorderStyle.None;
            passwordLogin.BackColor = Color.FromArgb(248, 250, 252);
            passwordLogin.ForeColor = Color.Black;
            passwordLogin.Font = new Font("Segoe UI", 20);
            
            passwordLogin.UseSystemPasswordChar = true;
            passwordLogin.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, passwordLogin.Width, passwordLogin.Height, 10, 10));


            // LOGIN Button

            login.FlatStyle = FlatStyle.Flat;
            login.FlatAppearance.BorderSize = 0;
            login.BackColor = Color.FromArgb(244, 196, 48); // yellow
            login.ForeColor = Color.Black;
            login.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            login.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, login.Width, login.Height, 10, 10));

            // Hover effect for LOGIN button
            login.MouseEnter += Login_HoverEnter;
            login.MouseLeave += Login_HoverLeave;

            signUp.MouseEnter += signUp_HoverEnter;
            signUp.MouseLeave += signUp_HoverLeave;
        }

        private bool isPasswordHidden = true;

        private void Form1_Load(object sender, EventArgs e)
        {
            passwordLogin.UseSystemPasswordChar = true;
            pictureBox6.Image = Properties.Resources.eye_closed;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

            if (isPasswordHidden)
            {
                passwordLogin.UseSystemPasswordChar = false;
                pictureBox6.Image = Properties.Resources.eye_opened;
                isPasswordHidden = false;
            }
            else
            {
                passwordLogin.UseSystemPasswordChar = true;
                pictureBox6.Image = Properties.Resources.eye_closed;
                isPasswordHidden = true;
            }
        }
        private void signUp_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();
            this.Close();
            
        }

        private void signUp_HoverEnter(object sender, EventArgs e)
        {
            signUp.ForeColor = Color.FromArgb(220, 170, 30); // darker yellow
        }

        private void signUp_HoverLeave(object sender, EventArgs e)
        {
            signUp.ForeColor = Color.Black; // original yellow
        }

        private void Login_HoverEnter(object sender, EventArgs e)
        {
            login.BackColor = Color.FromArgb(220, 170, 30); // darker yellow
        }

        private void Login_HoverLeave(object sender, EventArgs e)
        {
            login.BackColor = Color.FromArgb(244, 196, 48); // original yellow
        }

        
        private void login_Click(object sender, EventArgs e)
        {
            loginCheck auth = new loginCheck();

            string username = usernameLogin.Text;
            string password = passwordLogin.Text;

            UserData currentUser = new UserData();
            adminAcc currentAdmin = new adminAcc();
            govData currentGov = new govData();
            fuelEditorData currentFuel = new fuelEditorData();

            bool isLoggedIn;
            
            if(username.Contains("admin@") || username.Contains("superadmin"))
            {
                isLoggedIn = auth.loginAdmin(username, password, currentAdmin);
            }
            else if(username.Contains("gov@"))
            {
                isLoggedIn = auth.loginGov(username, password, currentGov);
            }
            else if(username.Contains("editor"))
            {
                isLoggedIn = auth.loginFuelEditor(username, password, currentFuel);
            }
            else
            {
                isLoggedIn = auth.loginUser(username, password, currentUser);
            }
            bool isAdmin = usernameLogin.Text.Contains("admin@")||usernameLogin.Text.Contains("superadmin");
            bool isFuel = usernameLogin.Text.Contains("editor");

            bool isGov = usernameLogin.Text.Contains("gov@");

            if (isLoggedIn)
            {
                
                this.Hide();
                if (isAdmin)
                {
                    adminPanel adminPanel = new adminPanel(currentAdmin);
                    adminPanel.ShowDialog();
                    this.Close();
                }
                else if (isGov)
                {
                    governmentPanel governmentPanel = new governmentPanel(currentGov);
                    governmentPanel.ShowDialog();
                    this.Close();

                }
                else if(isFuel)
                {
                    homeFuelEditor fuelPanel = new homeFuelEditor(currentFuel);
                    fuelPanel.ShowDialog();
                    this.Close();
                }
                else
                {
                   
                    Form3 form3 = new Form3(currentUser);
                    form3.ShowDialog();
                    this.Close();
                }
                // Open next form or panel
            }
            else
            {
                if (auth.LoginError == "deactivated")
                {
                    MessageBox.Show(
                        "This account has been deactivated. Please contact support to reactivate your account.",
                        "Account Deactivated",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "The username or password you entered is incorrect.\nPlease try again.",
                        "Login Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

        }

        private void login_MouseEnter(object sender, EventArgs e)
        {

        }

        private void login_MouseLeave(object sender, EventArgs e)
        {

        }

        private void signUp_MouseEnter(object sender, EventArgs e)
        {

        }

        private void signUp_MouseLeave(object sender, EventArgs e)
        {

        }
    }
}
