namespace AutoLiftingClashAnalysis
{
    partial class LiftingForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbSetsFolder = new System.Windows.Forms.ComboBox();
            this.lblFolder = new System.Windows.Forms.Label();
            this.lblHeight = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.lblStep = new System.Windows.Forms.Label();
            this.txtStep = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();

            //
            // lblFolder
            //
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(12, 15);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(130, 13);
            this.lblFolder.TabIndex = 0;
            this.lblFolder.Text = "Pasta de Selection Sets:";

            //
            // cmbSetsFolder
            //
            this.cmbSetsFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSetsFolder.FormattingEnabled = true;
            this.cmbSetsFolder.Location = new System.Drawing.Point(15, 31);
            this.cmbSetsFolder.Name = "cmbSetsFolder";
            this.cmbSetsFolder.Size = new System.Drawing.Size(257, 21);
            this.cmbSetsFolder.TabIndex = 1;

            //
            // lblHeight
            //
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(12, 65);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(130, 13);
            this.lblHeight.TabIndex = 2;
            this.lblHeight.Text = "Altura de Içamento (m):";

            //
            // txtHeight
            //
            this.txtHeight.Location = new System.Drawing.Point(15, 81);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(100, 20);
            this.txtHeight.TabIndex = 3;
            this.txtHeight.Text = "100";

            //
            // lblStep
            //
            this.lblStep.AutoSize = true;
            this.lblStep.Location = new System.Drawing.Point(150, 65);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(100, 13);
            this.lblStep.TabIndex = 4;
            this.lblStep.Text = "Passo Descida (m):";

            //
            // txtStep
            //
            this.txtStep.Location = new System.Drawing.Point(153, 81);
            this.txtStep.Name = "txtStep";
            this.txtStep.Size = new System.Drawing.Size(100, 20);
            this.txtStep.TabIndex = 5;
            this.txtStep.Text = "0.30";

            //
            // btnRun
            //
            this.btnRun.Location = new System.Drawing.Point(15, 120);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(257, 35);
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "Iniciar Simulação";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);

            //
            // progressBar
            //
            this.progressBar.Location = new System.Drawing.Point(15, 170);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(257, 23);
            this.progressBar.TabIndex = 7;

            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(15, 196);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 8;

            //
            // LiftingForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 231);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.txtStep);
            this.Controls.Add(this.lblStep);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.cmbSetsFolder);
            this.Controls.Add(this.lblFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LiftingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lifting Test Automation";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox cmbSetsFolder;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.TextBox txtStep;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
    }
}
