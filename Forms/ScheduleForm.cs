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
        private readonly SubjectService _subjectService;
        private ScheduleItem _selectedItem;
        private bool _suppressSelectionChanged;

        public ScheduleForm()
        {
            InitializeComponent();
            _scheduleService = new ScheduleService();
            _subjectService = new SubjectService();
            
            // Modern styling
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9.75F);
            
            InitializeControls();
            LoadSubjects();
            LoadSchedule();
            ClearInputs(); // Clear input fields on form load
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
        /// Load subjects into dropdown
        /// </summary>
        private void LoadSubjects()
        {
            try
            {
                cmbSubject.Items.Clear();
                var subjects = _subjectService.GetAll();
                
                if (subjects.Count == 0)
                {
                    cmbSubject.Items.Add("No subjects available - Please add subjects first");
                    cmbSubject.Enabled = false;
                    btnAdd.Enabled = false;
                    btnUpdate.Enabled = false;
                    
                    MessageBox.Show("No subjects found! Please add subjects first before creating schedules.\n\nClick 'Manage Subjects' to add subjects.", 
                        "No Subjects Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cmbSubject.Items.Add("-- Select Subject --");
                    foreach (var subject in subjects)
                    {
                        cmbSubject.Items.Add(subject);
                    }
                    cmbSubject.SelectedIndex = 0;
                    cmbSubject.Enabled = true;
                    btnAdd.Enabled = true;
                    btnUpdate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load all schedules into DataGridView
        /// </summary>
        private void LoadSchedule()
        {
            try
            {
                _suppressSelectionChanged = true;

                dgvSchedule.Rows.Clear();
                var schedules = _scheduleService.GetAllOrdered();

                foreach (var schedule in schedules)
                {
                    dgvSchedule.Rows.Add(
                        schedule.Id,
                        schedule.SubjectName ?? "",
                        schedule.SubjectCode,
                        schedule.GetDayName(),
                        schedule.StartTime,
                        schedule.EndTime,
                        schedule.Location,
                        schedule.Instructor
                    );
                }

                // Clear any selection to prevent auto-filling input fields
                dgvSchedule.ClearSelection();
                _selectedItem = null;
                _suppressSelectionChanged = false;

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
                var selectedSubject = cmbSubject.SelectedItem as SubjectItem;
                if (selectedSubject == null)
                {
                    MessageBox.Show("Please select a subject.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var schedule = new ScheduleItem
                {
                    DayOfWeek = cmbDayOfWeek.SelectedIndex,
                    SubjectCode = selectedSubject.Code,
                    SubjectName = selectedSubject.Name,
                    StartTime = txtStartTime.Text.Trim(),
                    EndTime = txtEndTime.Text.Trim(),
                    Location = txtLocation.Text.Trim(),
                    Instructor = txtInstructor.Text.Trim()
                };

                // Check for time conflicts
                var conflictingSchedule = _scheduleService.GetConflictingSchedule(schedule);
                if (conflictingSchedule != null)
                {
                    string conflictMessage = $"Schedule conflict detected!\n\n" +
                        $"The time slot conflicts with an existing class:\n\n" +
                        $"Subject: {conflictingSchedule.SubjectName ?? conflictingSchedule.SubjectCode}\n" +
                        $"Day: {conflictingSchedule.GetDayName()}\n" +
                        $"Time: {conflictingSchedule.StartTime} - {conflictingSchedule.EndTime}\n" +
                        $"Location: {conflictingSchedule.Location}\n\n" +
                        $"Please choose a different time slot.";
                    
                    MessageBox.Show(conflictMessage, "Schedule Conflict", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
                var selectedSubject = cmbSubject.SelectedItem as SubjectItem;
                if (selectedSubject == null)
                {
                    MessageBox.Show("Please select a subject.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _selectedItem.DayOfWeek = cmbDayOfWeek.SelectedIndex;
                _selectedItem.SubjectCode = selectedSubject.Code;
                _selectedItem.SubjectName = selectedSubject.Name;
                _selectedItem.StartTime = txtStartTime.Text.Trim();
                _selectedItem.EndTime = txtEndTime.Text.Trim();
                _selectedItem.Location = txtLocation.Text.Trim();
                _selectedItem.Instructor = txtInstructor.Text.Trim();

                // Check for time conflicts (excluding current item)
                var conflictingSchedule = _scheduleService.GetConflictingSchedule(_selectedItem, _selectedItem.Id);
                if (conflictingSchedule != null)
                {
                    string conflictMessage = $"Schedule conflict detected!\n\n" +
                        $"The time slot conflicts with an existing class:\n\n" +
                        $"Subject: {conflictingSchedule.SubjectName ?? conflictingSchedule.SubjectCode}\n" +
                        $"Day: {conflictingSchedule.GetDayName()}\n" +
                        $"Time: {conflictingSchedule.StartTime} - {conflictingSchedule.EndTime}\n" +
                        $"Location: {conflictingSchedule.Location}\n\n" +
                        $"Please choose a different time slot.";
                    
                    MessageBox.Show(conflictMessage, "Schedule Conflict", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
            if (_suppressSelectionChanged)
            {
                return;
            }

            if (dgvSchedule.SelectedRows.Count > 0)
            {
                var row = dgvSchedule.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells[0].Value);
                
                _selectedItem = _scheduleService.GetById(id);
                
                if (_selectedItem != null)
                {
                    cmbDayOfWeek.SelectedIndex = _selectedItem.DayOfWeek;
                    
                    // Find and select the subject in dropdown
                    for (int i = 0; i < cmbSubject.Items.Count; i++)
                    {
                        if (cmbSubject.Items[i] is SubjectItem subject && subject.Code == _selectedItem.SubjectCode)
                        {
                            cmbSubject.SelectedIndex = i;
                            break;
                        }
                    }
                    
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
            if (cmbSubject.SelectedItem == null || !(cmbSubject.SelectedItem is SubjectItem))
            {
                MessageBox.Show("Please select a subject.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSubject.Focus();
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
            cmbSubject.SelectedIndex = 0;
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

