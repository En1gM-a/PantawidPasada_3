namespace PantawidPasada
{
    partial class vehicleInfo
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DriverLicense = new TextBox();
            vehicleType = new ComboBox();
            PlateNo = new TextBox();
            label4 = new Label();
            SuspendLayout();
            // 
            // DriverLicense
            // 
            DriverLicense.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            DriverLicense.Location = new Point(16, 269);
            DriverLicense.Multiline = true;
            DriverLicense.Name = "DriverLicense";
            DriverLicense.Size = new Size(402, 38);
            DriverLicense.TabIndex = 41;
            // 
            // vehicleType
            // 
            vehicleType.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            vehicleType.FormattingEnabled = true;
            vehicleType.Items.AddRange(new object[] { "Select Vehicle Type", "Modern Jeepney", "Traditional Jeepney", "UV Express / Van", "Bus", "Tricycle", "Motorsiklo / Habal-Habal", "Taxi / Grab / Transport Network Vehicle", "Other" });
            vehicleType.Location = new Point(16, 81);
            vehicleType.Name = "vehicleType";
            vehicleType.Size = new Size(402, 38);
            vehicleType.TabIndex = 40;
            // 
            // PlateNo
            // 
            PlateNo.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            PlateNo.Location = new Point(16, 170);
            PlateNo.Multiline = true;
            PlateNo.Name = "PlateNo";
            PlateNo.Size = new Size(402, 38);
            PlateNo.TabIndex = 39;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(16, 9);
            label4.Name = "label4";
            label4.Size = new Size(344, 47);
            label4.TabIndex = 37;
            label4.Text = "Vehicle Information";
            // 
            // vehicleInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(DriverLicense);
            Controls.Add(vehicleType);
            Controls.Add(PlateNo);
            Controls.Add(label4);
            Name = "vehicleInfo";
            Size = new Size(437, 393);
            Load += vehicleInfo_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox DriverLicense;
        private ComboBox vehicleType;
        private TextBox PlateNo;
        private Label label4;
    }
}
