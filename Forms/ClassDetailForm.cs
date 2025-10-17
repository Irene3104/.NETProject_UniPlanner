using System;
using System.Drawing;
using System.Windows.Forms;
using UniPlanner.Models;

namespace UniPlanner.Forms
{
    /// <summary>
    /// Custom popup form for displaying class details
    /// </summary>
    public partial class ClassDetailForm : Form
    {
        private readonly ScheduleItem _scheduleItem;
        private readonly SubjectItem _subjectItem;

        public ClassDetailForm(ScheduleItem scheduleItem, SubjectItem subjectItem)
        {
            _scheduleItem = scheduleItem;
            _subjectItem = subjectItem;
            
            InitializeComponent();
            ApplyModernStyle();
            LoadDetails();
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(450, 400);
        }

        private void LoadDetails()
        {
            // Set title
            this.Text = "Class Details";
            lblTitle.Text = $"📚 {(_subjectItem != null ? _subjectItem.Name : _scheduleItem.Subject)}";

            // Apply color
            if (_subjectItem != null && !string.IsNullOrEmpty(_subjectItem.Color))
            {
                try
                {
                    panelHeader.BackColor = ColorTranslator.FromHtml(_subjectItem.Color);
                }
                catch
                {
                    panelHeader.BackColor = Color.FromArgb(52, 152, 219);
                }
            }

            // Populate details - prioritize SubjectName from schedule, then Subject.Name from SubjectItem
            string subjectName = !string.IsNullOrEmpty(_scheduleItem.SubjectName) 
                ? _scheduleItem.SubjectName 
                : (_subjectItem != null ? _subjectItem.Name : _scheduleItem.Subject);
            
            string subjectCode = _subjectItem != null ? _subjectItem.Code : _scheduleItem.Subject;
            
            lblSubjectValue.Text = subjectName;
            lblCodeValue.Text = subjectCode;
            
            if (_subjectItem != null)
            {
                lblCreditsValue.Text = $"{_subjectItem.Credits} credits";
                lblCreditsLabel.Visible = true;
                lblCreditsValue.Visible = true;
            }
            else
            {
                lblCreditsLabel.Visible = false;
                lblCreditsValue.Visible = false;
            }

            lblInstructorValue.Text = _scheduleItem.Instructor ?? "N/A";
            lblLocationValue.Text = _scheduleItem.Location ?? "N/A";
            lblTimeValue.Text = $"{_scheduleItem.StartTime} - {_scheduleItem.EndTime}";
            lblDayValue.Text = _scheduleItem.GetDayName();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

