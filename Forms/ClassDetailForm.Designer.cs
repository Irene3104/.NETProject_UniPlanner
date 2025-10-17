using System.Drawing;
using System.Windows.Forms;

namespace UniPlanner.Forms
{
    partial class ClassDetailForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.lblSubjectLabel = new System.Windows.Forms.Label();
            this.lblSubjectValue = new System.Windows.Forms.Label();
            this.lblCodeLabel = new System.Windows.Forms.Label();
            this.lblCodeValue = new System.Windows.Forms.Label();
            this.lblCreditsLabel = new System.Windows.Forms.Label();
            this.lblCreditsValue = new System.Windows.Forms.Label();
            this.lblInstructorLabel = new System.Windows.Forms.Label();
            this.lblInstructorValue = new System.Windows.Forms.Label();
            this.lblLocationLabel = new System.Windows.Forms.Label();
            this.lblLocationValue = new System.Windows.Forms.Label();
            this.lblTimeLabel = new System.Windows.Forms.Label();
            this.lblTimeValue = new System.Windows.Forms.Label();
            this.lblDayLabel = new System.Windows.Forms.Label();
            this.lblDayValue = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelHeader.Size = new System.Drawing.Size(450, 80);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(410, 50);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Class Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.lblSubjectLabel);
            this.panelContent.Controls.Add(this.lblSubjectValue);
            this.panelContent.Controls.Add(this.lblCodeLabel);
            this.panelContent.Controls.Add(this.lblCodeValue);
            this.panelContent.Controls.Add(this.lblCreditsLabel);
            this.panelContent.Controls.Add(this.lblCreditsValue);
            this.panelContent.Controls.Add(this.lblInstructorLabel);
            this.panelContent.Controls.Add(this.lblInstructorValue);
            this.panelContent.Controls.Add(this.lblLocationLabel);
            this.panelContent.Controls.Add(this.lblLocationValue);
            this.panelContent.Controls.Add(this.lblTimeLabel);
            this.panelContent.Controls.Add(this.lblTimeValue);
            this.panelContent.Controls.Add(this.lblDayLabel);
            this.panelContent.Controls.Add(this.lblDayValue);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 80);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.panelContent.Size = new System.Drawing.Size(450, 280);
            this.panelContent.TabIndex = 1;
            // 
            // lblSubjectLabel
            // 
            this.lblSubjectLabel.AutoSize = true;
            this.lblSubjectLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSubjectLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblSubjectLabel.Location = new System.Drawing.Point(30, 25);
            this.lblSubjectLabel.Name = "lblSubjectLabel";
            this.lblSubjectLabel.Size = new System.Drawing.Size(60, 19);
            this.lblSubjectLabel.TabIndex = 0;
            this.lblSubjectLabel.Text = "Subject:";
            // 
            // lblSubjectValue
            // 
            this.lblSubjectValue.AutoSize = true;
            this.lblSubjectValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubjectValue.Location = new System.Drawing.Point(150, 25);
            this.lblSubjectValue.Name = "lblSubjectValue";
            this.lblSubjectValue.Size = new System.Drawing.Size(100, 19);
            this.lblSubjectValue.TabIndex = 1;
            this.lblSubjectValue.Text = "Subject Name";
            // 
            // lblCodeLabel
            // 
            this.lblCodeLabel.AutoSize = true;
            this.lblCodeLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCodeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblCodeLabel.Location = new System.Drawing.Point(30, 55);
            this.lblCodeLabel.Name = "lblCodeLabel";
            this.lblCodeLabel.Size = new System.Drawing.Size(99, 19);
            this.lblCodeLabel.TabIndex = 2;
            this.lblCodeLabel.Text = "Subject Code:";
            // 
            // lblCodeValue
            // 
            this.lblCodeValue.AutoSize = true;
            this.lblCodeValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCodeValue.Location = new System.Drawing.Point(150, 55);
            this.lblCodeValue.Name = "lblCodeValue";
            this.lblCodeValue.Size = new System.Drawing.Size(75, 19);
            this.lblCodeValue.TabIndex = 3;
            this.lblCodeValue.Text = "COMP101";
            // 
            // lblCreditsLabel
            // 
            this.lblCreditsLabel.AutoSize = true;
            this.lblCreditsLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCreditsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblCreditsLabel.Location = new System.Drawing.Point(30, 85);
            this.lblCreditsLabel.Name = "lblCreditsLabel";
            this.lblCreditsLabel.Size = new System.Drawing.Size(60, 19);
            this.lblCreditsLabel.TabIndex = 4;
            this.lblCreditsLabel.Text = "Credits:";
            // 
            // lblCreditsValue
            // 
            this.lblCreditsValue.AutoSize = true;
            this.lblCreditsValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCreditsValue.Location = new System.Drawing.Point(150, 85);
            this.lblCreditsValue.Name = "lblCreditsValue";
            this.lblCreditsValue.Size = new System.Drawing.Size(62, 19);
            this.lblCreditsValue.TabIndex = 5;
            this.lblCreditsValue.Text = "3 credits";
            // 
            // lblInstructorLabel
            // 
            this.lblInstructorLabel.AutoSize = true;
            this.lblInstructorLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblInstructorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblInstructorLabel.Location = new System.Drawing.Point(30, 115);
            this.lblInstructorLabel.Name = "lblInstructorLabel";
            this.lblInstructorLabel.Size = new System.Drawing.Size(77, 19);
            this.lblInstructorLabel.TabIndex = 6;
            this.lblInstructorLabel.Text = "Instructor:";
            // 
            // lblInstructorValue
            // 
            this.lblInstructorValue.AutoSize = true;
            this.lblInstructorValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblInstructorValue.Location = new System.Drawing.Point(150, 115);
            this.lblInstructorValue.Name = "lblInstructorValue";
            this.lblInstructorValue.Size = new System.Drawing.Size(50, 19);
            this.lblInstructorValue.TabIndex = 7;
            this.lblInstructorValue.Text = "Dr. Lee";
            // 
            // lblLocationLabel
            // 
            this.lblLocationLabel.AutoSize = true;
            this.lblLocationLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLocationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblLocationLabel.Location = new System.Drawing.Point(30, 145);
            this.lblLocationLabel.Name = "lblLocationLabel";
            this.lblLocationLabel.Size = new System.Drawing.Size(69, 19);
            this.lblLocationLabel.TabIndex = 8;
            this.lblLocationLabel.Text = "Location:";
            // 
            // lblLocationValue
            // 
            this.lblLocationValue.AutoSize = true;
            this.lblLocationValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLocationValue.Location = new System.Drawing.Point(150, 145);
            this.lblLocationValue.Name = "lblLocationValue";
            this.lblLocationValue.Size = new System.Drawing.Size(100, 19);
            this.lblLocationValue.TabIndex = 9;
            this.lblLocationValue.Text = "Building5 04.201";
            // 
            // lblTimeLabel
            // 
            this.lblTimeLabel.AutoSize = true;
            this.lblTimeLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTimeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblTimeLabel.Location = new System.Drawing.Point(30, 175);
            this.lblTimeLabel.Name = "lblTimeLabel";
            this.lblTimeLabel.Size = new System.Drawing.Size(44, 19);
            this.lblTimeLabel.TabIndex = 10;
            this.lblTimeLabel.Text = "Time:";
            // 
            // lblTimeValue
            // 
            this.lblTimeValue.AutoSize = true;
            this.lblTimeValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTimeValue.Location = new System.Drawing.Point(150, 175);
            this.lblTimeValue.Name = "lblTimeValue";
            this.lblTimeValue.Size = new System.Drawing.Size(100, 19);
            this.lblTimeValue.TabIndex = 11;
            this.lblTimeValue.Text = "09:00 - 10:00";
            // 
            // lblDayLabel
            // 
            this.lblDayLabel.AutoSize = true;
            this.lblDayLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDayLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblDayLabel.Location = new System.Drawing.Point(30, 205);
            this.lblDayLabel.Name = "lblDayLabel";
            this.lblDayLabel.Size = new System.Drawing.Size(37, 19);
            this.lblDayLabel.TabIndex = 12;
            this.lblDayLabel.Text = "Day:";
            // 
            // lblDayValue
            // 
            this.lblDayValue.AutoSize = true;
            this.lblDayValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDayValue.Location = new System.Drawing.Point(150, 205);
            this.lblDayValue.Name = "lblDayValue";
            this.lblDayValue.Size = new System.Drawing.Size(60, 19);
            this.lblDayValue.TabIndex = 13;
            this.lblDayValue.Text = "Monday";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(0, 360);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(450, 40);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ClassDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 400);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClassDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Class Details";
            this.panelHeader.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblSubjectLabel;
        private System.Windows.Forms.Label lblSubjectValue;
        private System.Windows.Forms.Label lblCodeLabel;
        private System.Windows.Forms.Label lblCodeValue;
        private System.Windows.Forms.Label lblCreditsLabel;
        private System.Windows.Forms.Label lblCreditsValue;
        private System.Windows.Forms.Label lblInstructorLabel;
        private System.Windows.Forms.Label lblInstructorValue;
        private System.Windows.Forms.Label lblLocationLabel;
        private System.Windows.Forms.Label lblLocationValue;
        private System.Windows.Forms.Label lblTimeLabel;
        private System.Windows.Forms.Label lblTimeValue;
        private System.Windows.Forms.Label lblDayLabel;
        private System.Windows.Forms.Label lblDayValue;
        private System.Windows.Forms.Button btnClose;
    }
}

