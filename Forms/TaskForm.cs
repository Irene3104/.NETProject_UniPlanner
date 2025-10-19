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

            // Set default date to Sydney today
            dtpDueDate.Value = TimeZoneHelper.GetSydneyNow().Date;
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

                var todaySydney = TimeZoneHelper.GetSydneyNow().Date;

                foreach (var task in tasks)
                {
                    var dueSydney = ConvertToSydney(task.DueDate, TimeZoneHelper.GetSydneyTimeZone());
                    var dueDisplay = FormatDueDisplay(dueSydney, todaySydney);
                    var dueDateDisplay = dueSydney.ToString("dd-MM-yyyy");

                    string subjectDisplay = string.Empty;
                    if (!string.IsNullOrWhiteSpace(task.SubjectCode))
                    {
                        if (subjects.TryGetValue(task.SubjectCode, out var subject))
                        {
                            subjectDisplay = subject.Name;
                        }
                        else
                        {
                            subjectDisplay = task.SubjectCode;
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
                        task.IsCompleted ? "✓" : "",
                        task.Description ?? ""
                    });

                    if (task.IsCompleted)
                        item.ForeColor = Color.Green;
                    else if (IsOverdue(task.DueDate, TimeZoneInfo.Utc))
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
                string subjectName = "";
                if (cmbSubject.SelectedIndex > 0 && cmbSubject.SelectedItem is SubjectItem selectedSubject)
                {
                    subjectText = selectedSubject.Code;
                    subjectName = selectedSubject.Name;
                }

                var task = new TaskItem
                {
                    Title = txtTitle.Text.Trim(),
                    DueDate = dtpDueDate.Value.Date,
                    Priority = cmbPriority.SelectedItem.ToString(),
                    SubjectCode = subjectText,
                    SubjectName = subjectName,
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
                string subjectName = "";
                if (cmbSubject.SelectedIndex > 0 && cmbSubject.SelectedItem is SubjectItem selectedSubject)
                {
                    subjectText = selectedSubject.Code;
                    subjectName = selectedSubject.Name;
                }

                _selectedItem.Title = txtTitle.Text.Trim();
                _selectedItem.DueDate = dtpDueDate.Value.Date;
                _selectedItem.Priority = cmbPriority.SelectedItem.ToString();
                _selectedItem.SubjectCode = subjectText;
                _selectedItem.SubjectName = subjectName;
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
                MessageBox.Show("Please select a task to mark/unmark.", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int id = Convert.ToInt32(lstTasks.SelectedItems[0].SubItems[0].Text);
                
                // Toggle completion status
                _taskService.ToggleComplete(id);
                
                // Get updated task to check new status
                var updatedTask = _taskService.GetById(id);
                string statusMessage = updatedTask.IsCompleted 
                    ? "Task marked as complete!" 
                    : "Task marked as incomplete!";
                
                MessageBox.Show(statusMessage, "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                LoadTasks();
                
                // Update button text based on new status
                UpdateMarkCompleteButton(updatedTask.IsCompleted);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating task status: {ex.Message}", "Error", 
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
                    if (!string.IsNullOrEmpty(_selectedItem.SubjectCode))
                    {
                        // Find the subject in the combo box by code
                        bool found = false;
                        for (int i = 1; i < cmbSubject.Items.Count; i++) // Skip index 0 (-- No Subject --)
                        {
                            if (cmbSubject.Items[i] is SubjectItem subject && subject.Code == _selectedItem.SubjectCode)
                            {
                                cmbSubject.SelectedIndex = i;
                                found = true;
                                break;
                            }
                        }
                        
                        if (!found)
                        {
                            cmbSubject.SelectedIndex = 0; // -- No Subject --
                        }
                    }
                    else
                    {
                        cmbSubject.SelectedIndex = 0; // -- No Subject --
                    }
                    
                    // Update Mark Complete button text based on task status
                    UpdateMarkCompleteButton(_selectedItem.IsCompleted);
                }
            }
            else
            {
                // Reset button text when nothing is selected
                btnMarkComplete.Text = "✓ Mark Complete";
            }
        }

        /// <summary>
        /// Update Mark Complete button text based on completion status
        /// </summary>
        private void UpdateMarkCompleteButton(bool isCompleted)
        {
            if (isCompleted)
            {
                btnMarkComplete.Text = "↩ Unmark Complete";
                btnMarkComplete.BackColor = Color.FromArgb(243, 156, 18); // Orange color
            }
            else
            {
                btnMarkComplete.Text = "✓ Mark Complete";
                btnMarkComplete.BackColor = Color.FromArgb(142, 68, 173); // Purple color
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
            
            // Reset Mark Complete button to default state
            btnMarkComplete.Text = "✓ Mark Complete";
            btnMarkComplete.BackColor = Color.FromArgb(142, 68, 173); // Purple color
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
            return TimeZoneHelper.GetSydneyTimeZone();
        }

        private DateTime ConvertToSydney(DateTime date, TimeZoneInfo tz)
        {
            return TimeZoneHelper.ConvertToSydney(date, tz);
        }

        private string FormatDueDisplay(DateTime dueSydney, DateTime todaySydney)
        {
            var days = (dueSydney.Date - todaySydney).Days;
            string relative;

            if (days == 0) relative = "today";
            else if (days == 1) relative = "Tomorrow";
            else if (days == -1) relative = "Yesterday";
            else if (days > 1 && days <= 7) relative = $"In {days} days";
            else if (days < -1 && days >= -7) relative = $"{Math.Abs(days)} days ago";
            else relative = dueSydney.ToString("dd-MM-yyyy");

            if (relative == dueSydney.ToString("dd-MM-yyyy"))
            {
                return relative;
            }

            return $"{dueSydney:dd-MM-yyyy} ({relative})";
        }

        private bool IsOverdue(DateTime dueDate, TimeZoneInfo tz)
        {
            return TimeZoneHelper.IsOverdue(dueDate, tz);
        }
    }
}

