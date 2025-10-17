using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniPlanner.Models;
using UniPlanner.Services;
using UniPlanner.Utils;

namespace UniPlanner.Forms
{
    /// <summary>
    /// Task and assignment management form
    /// </summary>
    public partial class TaskForm : Form
    {
        private readonly TaskService _taskService;
        private readonly SubjectService _subjectService;
        private TaskItem _selectedItem;

        public TaskForm()
        {
            InitializeComponent();
            _taskService = new TaskService();
            _subjectService = new SubjectService();
            
            // Modern styling
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9.75F);
            
            InitializeControls();
            LoadTasks();
        }

        /// <summary>
        /// Initialize form controls
        /// </summary>
        private void InitializeControls()
        {
            // Populate priority combo
            cmbPriority.Items.Clear();
            cmbPriority.Items.AddRange(new object[] { "High", "Medium", "Low" });
            cmbPriority.SelectedIndex = 1; // Medium

            // Populate subject combo
            LoadSubjects();

            // Populate filter combo
            cmbFilter.Items.Clear();
            cmbFilter.Items.AddRange(new object[] 
            { 
                "All Tasks", "Upcoming", "Overdue", "Completed", 
                "High Priority", "Medium Priority", "Low Priority" 
            });
            cmbFilter.SelectedIndex = 0; // All Tasks

            // Set default date
            dtpDueDate.Value = DateTime.Today.AddDays(1);
        }

        /// <summary>
        /// Load subjects into combo box
        /// </summary>
        private void LoadSubjects()
        {
            cmbSubject.Items.Clear();
            cmbSubject.Items.Add("-- No Subject --");
            
            var subjects = _subjectService.GetAll();
            foreach (var subject in subjects)
            {
                cmbSubject.Items.Add(subject);
            }
            
            // Don't set DisplayMember - use ToString() override instead
            cmbSubject.SelectedIndex = 0;
        }

        /// <summary>
        /// Load tasks into ListView with current filter
        /// </summary>
        private void LoadTasks()
        {
            try
            {
                lstTasks.Items.Clear();
                var tasks = GetFilteredTasks();

                var subjects = _subjectService.GetAll()
                    .GroupBy(s => s.Code)
                    .ToDictionary(g => g.Key, g => g.First());

                var sydneyTimeZone = GetSydneyTimeZone();
                var todaySydney = ConvertToSydney(DateTime.Now, sydneyTimeZone).Date;

                foreach (var task in tasks)
                {
                    var dueSydney = ConvertToSydney(task.DueDate, sydneyTimeZone);
                    var dueDisplay = FormatDueDisplay(dueSydney, todaySydney);
                    var dueDateDisplay = dueSydney.ToString("dd/MM/yyyy");

                    string subjectDisplay = string.Empty;
                    if (!string.IsNullOrWhiteSpace(task.Subject))
                    {
                        if (subjects.TryGetValue(task.Subject, out var subject))
                        {
                            subjectDisplay = subject.Name;
                        }
                        else
                        {
                            subjectDisplay = task.Subject;
                        }
                    }

                    var item = new ListViewItem(new[]
                    {
                        task.Id.ToString(),
                        task.Title,
                        dueDisplay,
                        dueDateDisplay,
                        task.Priority,
                        subjectDisplay,
                        task.IsCompleted ? "✓" : ""
                    });

                    if (task.IsCompleted)
                        item.ForeColor = Color.Green;
                    else if (IsOverdue(task.DueDate, sydneyTimeZone))
                        item.ForeColor = Color.Red;
                    else if (task.Priority == "High")
                        item.ForeColor = Color.DarkOrange;

                    lstTasks.Items.Add(item);
                }

                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Get filtered task list based on filter selection
        /// </summary>
        private System.Collections.Generic.IReadOnlyList<TaskItem> GetFilteredTasks()
        {
            switch (cmbFilter.SelectedIndex)
            {
                case 1: // Upcoming
                    return _taskService.GetUpcomingOrdered();
                case 2: // Overdue
                    return _taskService.GetOverdue();
                case 3: // Completed
                    return _taskService.GetCompleted();
                case 4: // High Priority
                    return _taskService.GetByPriority("High");
                case 5: // Medium Priority
                    return _taskService.GetByPriority("Medium");
                case 6: // Low Priority
                    return _taskService.GetByPriority("Low");
                default: // All Tasks
                    return _taskService.GetAll();
            }
        }

        /// <summary>
        /// Update summary statistics
        /// </summary>
        private void UpdateSummary()
        {
            var all = _taskService.GetAll();
            var sydneyTz = GetSydneyTimeZone();

            var completed = all.Count(t => t.IsCompleted);
            var overdue = all.Count(t => IsOverdue(t.DueDate, sydneyTz));

            lblSummary.Text = $"Total: {all.Count} | Completed: {completed} | Overdue: {overdue}";
        }

        /// <summary>
        /// Add new task
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                // Get selected subject
                string subjectText = "";
                if (cmbSubject.SelectedIndex > 0 && cmbSubject.SelectedItem is SubjectItem selectedSubject)
                {
                    subjectText = selectedSubject.Code;
                }

                var task = new TaskItem
                {
                    Title = txtTitle.Text.Trim(),
                    DueDate = dtpDueDate.Value.Date,
                    Priority = cmbPriority.SelectedItem.ToString(),
                    Subject = subjectText,
                    Description = txtDescription.Text.Trim(),
                    IsCompleted = false
                };

                _taskService.Add(task);
                MessageBox.Show("Task added successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ClearInputs();
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding task: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Update selected task
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedItem == null)
            {
                MessageBox.Show("Please select a task to update.", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                // Get selected subject
                string subjectText = "";
                if (cmbSubject.SelectedIndex > 0 && cmbSubject.SelectedItem is SubjectItem selectedSubject)
                {
                    subjectText = selectedSubject.Code;
                }

                _selectedItem.Title = txtTitle.Text.Trim();
                _selectedItem.DueDate = dtpDueDate.Value.Date;
                _selectedItem.Priority = cmbPriority.SelectedItem.ToString();
                _selectedItem.Subject = subjectText;
                _selectedItem.Description = txtDescription.Text.Trim();

                _taskService.Update(_selectedItem);
                MessageBox.Show("Task updated successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ClearInputs();
                LoadTasks();
                _selectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating task: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delete selected task
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a task to delete.", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this task?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(lstTasks.SelectedItems[0].SubItems[0].Text);
                    _taskService.Delete(id);
                    
                    MessageBox.Show("Task deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    ClearInputs();
                    LoadTasks();
                    _selectedItem = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting task: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Mark selected task as complete
        /// </summary>
        private void btnMarkComplete_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a task to mark as complete.", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int id = Convert.ToInt32(lstTasks.SelectedItems[0].SubItems[0].Text);
                _taskService.MarkComplete(id);
                
                MessageBox.Show("Task marked as complete!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error marking task: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handle item selection in ListView
        /// </summary>
        private void lstTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTasks.SelectedItems.Count > 0)
            {
                var item = lstTasks.SelectedItems[0];
                int id = Convert.ToInt32(item.SubItems[0].Text);
                
                _selectedItem = _taskService.GetById(id);
                
                if (_selectedItem != null)
                {
                    txtTitle.Text = _selectedItem.Title;
                    dtpDueDate.Value = _selectedItem.DueDate;
                    cmbPriority.SelectedItem = _selectedItem.Priority;
                    txtDescription.Text = _selectedItem.Description ?? "";
                    
                    // Set subject combo
                    if (!string.IsNullOrEmpty(_selectedItem.Subject))
                    {
                        var subject = _subjectService.GetByCode(_selectedItem.Subject);
                        if (subject != null)
                        {
                            cmbSubject.SelectedItem = subject;
                        }
                        else
                        {
                            cmbSubject.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        cmbSubject.SelectedIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Filter changed event
        /// </summary>
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTasks();
        }

        /// <summary>
        /// Validate form input
        /// </summary>
        private bool ValidateInput()
        {
            if (!ValidationHelper.IsValidString(txtTitle.Text, 1, 200))
            {
                MessageBox.Show("Please enter a valid task title (1-200 characters).", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return false;
            }

            if (cmbPriority.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a priority level.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPriority.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clear all input fields
        /// </summary>
        private void ClearInputs()
        {
            txtTitle.Clear();
            cmbSubject.SelectedIndex = 0;
            txtDescription.Clear();
            cmbPriority.SelectedIndex = 1;
            dtpDueDate.Value = DateTime.Today.AddDays(1);
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

        private TimeZoneInfo GetSydneyTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney");
            }
            catch (InvalidTimeZoneException)
            {
                return TimeZoneInfo.Local;
            }
        }

        private DateTime ConvertToSydney(DateTime date, TimeZoneInfo tz)
        {
            var source = date;
            if (source.Kind == DateTimeKind.Unspecified)
            {
                source = DateTime.SpecifyKind(source, DateTimeKind.Local);
            }
            return TimeZoneInfo.ConvertTime(source, tz);
        }

        private string FormatDueDisplay(DateTime dueSydney, DateTime todaySydney)
        {
            var days = (dueSydney.Date - todaySydney).Days;
            string relative;

            if (days == 0) relative = "Today";
            else if (days == 1) relative = "Tomorrow";
            else if (days == -1) relative = "Yesterday";
            else if (days > 1 && days <= 7) relative = $"In {days} days";
            else if (days < -1 && days >= -7) relative = $"{Math.Abs(days)} days ago";
            else relative = dueSydney.ToString("dd/MM/yyyy");

            if (relative == dueSydney.ToString("dd/MM/yyyy"))
            {
                return relative;
            }

            return $"{dueSydney:dd/MM/yyyy} ({relative})";
        }

        private bool IsOverdue(DateTime dueDate, TimeZoneInfo tz)
        {
            var dueSydney = ConvertToSydney(dueDate, tz).Date;
            var todaySydney = ConvertToSydney(DateTime.Now, tz).Date;
            return dueSydney < todaySydney;
        }
    }
}

