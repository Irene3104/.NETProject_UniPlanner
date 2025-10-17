using System;
using System.Drawing;
using System.Windows.Forms;
using UniPlanner.Models;
using UniPlanner.Services;
using UniPlanner.Utils;

namespace UniPlanner.Forms
{
    /// <summary>
    /// Class schedule management form
    /// </summary>
    public partial class ScheduleForm : Form
    {
        private readonly ScheduleService _scheduleService;
        private ScheduleItem _selectedItem;

        public ScheduleForm()
        {
            InitializeComponent();
            _scheduleService = new ScheduleService();
            
            // Modern styling
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9.75F);
            
            InitializeControls();
            LoadSchedule();
        }

        /// <summary>
        /// Initialize form controls and data
        /// </summary>
        private void InitializeControls()
        {
            // Populate day of week combo
            cmbDayOfWeek.Items.Clear();
            cmbDayOfWeek.Items.AddRange(new object[] 
            { 
                "Sunday", "Monday", "Tuesday", "Wednesday", 
                "Thursday", "Friday", "Saturday" 
            });
            cmbDayOfWeek.SelectedIndex = 1; // Monday

            // Set default times
            txtStartTime.Text = "09:00";
            txtEndTime.Text = "10:00";
        }

        /// <summary>
        /// Load all schedules into DataGridView
        /// </summary>
        private void LoadSchedule()
        {
            try
            {
                dgvSchedule.Rows.Clear();
                var schedules = _scheduleService.GetAllOrdered();

                foreach (var schedule in schedules)
                {
                    dgvSchedule.Rows.Add(
                        schedule.Id,
                        schedule.SubjectName ?? "",
                        schedule.Subject,
                        schedule.GetDayName(),
                        schedule.StartTime,
                        schedule.EndTime,
                        schedule.Location,
                        schedule.Instructor
                    );
                }

                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading schedule: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Update summary statistics
        /// </summary>
        private void UpdateSummary()
        {
            var totalHours = _scheduleService.GetTotalWeeklyHours();
            var totalClasses = _scheduleService.GetAll().Count;
            
            lblSummary.Text = $"Total: {totalClasses} classes | Weekly Hours: {totalHours:F1}h";
        }

        /// <summary>
        /// Add new schedule entry
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var schedule = new ScheduleItem
                {
                    DayOfWeek = cmbDayOfWeek.SelectedIndex,
                    Subject = txtSubject.Text.Trim(),
                    SubjectName = txtSubjectName.Text.Trim(),
                    StartTime = txtStartTime.Text.Trim(),
                    EndTime = txtEndTime.Text.Trim(),
                    Location = txtLocation.Text.Trim(),
                    Instructor = txtInstructor.Text.Trim()
                };

                _scheduleService.Add(schedule);
                MessageBox.Show("Schedule added successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ClearInputs();
                LoadSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding schedule: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Update selected schedule entry
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedItem == null)
            {
                MessageBox.Show("Please select a schedule to update.", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                _selectedItem.DayOfWeek = cmbDayOfWeek.SelectedIndex;
                _selectedItem.Subject = txtSubject.Text.Trim();
                _selectedItem.SubjectName = txtSubjectName.Text.Trim();
                _selectedItem.StartTime = txtStartTime.Text.Trim();
                _selectedItem.EndTime = txtEndTime.Text.Trim();
                _selectedItem.Location = txtLocation.Text.Trim();
                _selectedItem.Instructor = txtInstructor.Text.Trim();

                _scheduleService.Update(_selectedItem);
                MessageBox.Show("Schedule updated successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ClearInputs();
                LoadSchedule();
                _selectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating schedule: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delete selected schedule entry
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSchedule.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a schedule to delete.", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this schedule?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dgvSchedule.SelectedRows[0].Cells[0].Value);
                    _scheduleService.Delete(id);
                    
                    MessageBox.Show("Schedule deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    ClearInputs();
                    LoadSchedule();
                    _selectedItem = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting schedule: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Handle row selection in DataGridView
        /// </summary>
        private void dgvSchedule_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSchedule.SelectedRows.Count > 0)
            {
                var row = dgvSchedule.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells[0].Value);
                
                _selectedItem = _scheduleService.GetById(id);
                
                if (_selectedItem != null)
                {
                    cmbDayOfWeek.SelectedIndex = _selectedItem.DayOfWeek;
                    txtSubject.Text = _selectedItem.Subject;
                    txtSubjectName.Text = _selectedItem.SubjectName ?? "";
                    txtStartTime.Text = _selectedItem.StartTime;
                    txtEndTime.Text = _selectedItem.EndTime;
                    txtLocation.Text = _selectedItem.Location ?? "";
                    txtInstructor.Text = _selectedItem.Instructor ?? "";
                }
            }
        }

        /// <summary>
        /// Validate form input
        /// </summary>
        private bool ValidateInput()
        {
            if (!ValidationHelper.IsValidString(txtSubject.Text, 1, 100))
            {
                MessageBox.Show("Please enter a valid subject code.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSubject.Focus();
                return false;
            }

            if (!ValidationHelper.IsValidTimeRange(txtStartTime.Text, txtEndTime.Text))
            {
                MessageBox.Show("Please enter valid time range (Start time must be before End time).", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStartTime.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clear all input fields
        /// </summary>
        private void ClearInputs()
        {
            cmbDayOfWeek.SelectedIndex = 1;
            txtSubject.Clear();
            txtSubjectName.Clear();
            txtStartTime.Text = "09:00";
            txtEndTime.Text = "10:00";
            txtLocation.Clear();
            txtInstructor.Clear();
            _selectedItem = null;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

