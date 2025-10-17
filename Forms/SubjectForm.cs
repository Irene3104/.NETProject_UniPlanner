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
    /// Subject management form
    /// </summary>
    public partial class SubjectForm : Form
    {
        private readonly SubjectService _subjectService;
        private SubjectItem _selectedSubject;

        public SubjectForm()
        {
            InitializeComponent();
            _subjectService = new SubjectService();
            
            // Modern styling
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9.75F);
            
            InitializeColorPicker();
            LoadSubjects();
        }

        /// <summary>
        /// Initialize color picker with preset colors
        /// </summary>
        private void InitializeColorPicker()
        {
            cmbColor.Items.Clear();
            cmbColor.Items.AddRange(new object[]
            {
                "#FF6B6B",  // Red
                "#4ECDC4",  // Teal
                "#45B7D1",  // Blue
                "#FFA07A",  // Orange
                "#98D8C8",  // Mint
                "#F7DC6F",  // Yellow
                "#BB8FCE",  // Purple
                "#85C1E2",  // Light Blue
                "#F8B739",  // Gold
                "#52B788"   // Green
            });
            cmbColor.SelectedIndex = 0;
        }

        /// <summary>
        /// Load all subjects into ListView
        /// </summary>
        private void LoadSubjects()
        {
            try
            {
                lstSubjects.Items.Clear();
                var subjects = _subjectService.GetAll();

                foreach (var subject in subjects)
                {
                    var item = new ListViewItem(subject.Code);
                    item.SubItems.Add(subject.Name);
                    item.SubItems.Add(subject.Instructor ?? "");
                    item.SubItems.Add(subject.Credits.ToString());
                    item.Tag = subject;
                    
                    // Apply color to item
                    try
                    {
                        item.BackColor = ColorTranslator.FromHtml(subject.Color);
                        item.ForeColor = Color.White;
                    }
                    catch
                    {
                        item.BackColor = Color.LightBlue;
                    }

                    lstSubjects.Items.Add(item);
                }

                lblSummary.Text = $"Total Subjects: {subjects.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Add new subject
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var subject = new SubjectItem
                {
                    Code = txtCode.Text.Trim().ToUpper(),
                    Name = txtName.Text.Trim(),
                    Instructor = txtInstructor.Text.Trim(),
                    Credits = (int)nudCredits.Value,
                    Color = cmbColor.SelectedItem.ToString()
                };

                _subjectService.Add(subject);
                MessageBox.Show("Subject added successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ClearInputs();
                LoadSubjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding subject: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Update selected subject
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedSubject == null)
            {
                MessageBox.Show("Please select a subject to update.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                _selectedSubject.Code = txtCode.Text.Trim().ToUpper();
                _selectedSubject.Name = txtName.Text.Trim();
                _selectedSubject.Instructor = txtInstructor.Text.Trim();
                _selectedSubject.Credits = (int)nudCredits.Value;
                _selectedSubject.Color = cmbColor.SelectedItem.ToString();

                _subjectService.Update(_selectedSubject);
                MessageBox.Show("Subject updated successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ClearInputs();
                LoadSubjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating subject: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delete selected subject
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedSubject == null)
            {
                MessageBox.Show("Please select a subject to delete.", "No Selection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete '{_selectedSubject.Code} - {_selectedSubject.Name}'?", 
                "Confirm Delete", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _subjectService.Delete(_selectedSubject.Id);
                    MessageBox.Show("Subject deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    ClearInputs();
                    LoadSubjects();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting subject: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Handle subject selection
        /// </summary>
        private void lstSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSubjects.SelectedItems.Count > 0)
            {
                var item = lstSubjects.SelectedItems[0];
                _selectedSubject = item.Tag as SubjectItem;

                if (_selectedSubject != null)
                {
                    txtCode.Text = _selectedSubject.Code;
                    txtName.Text = _selectedSubject.Name ?? string.Empty;
                    txtInstructor.Text = _selectedSubject.Instructor ?? "";

                    var credits = _selectedSubject.Credits;
                    if (credits < nudCredits.Minimum)
                    {
                        credits = (int)nudCredits.Minimum;
                    }
                    else if (credits > nudCredits.Maximum)
                    {
                        credits = (int)nudCredits.Maximum;
                    }
                    nudCredits.Value = credits;
                    
                    // Set color combo
                    if (!string.IsNullOrWhiteSpace(_selectedSubject.Color) && cmbColor.Items.Contains(_selectedSubject.Color))
                    {
                        cmbColor.SelectedItem = _selectedSubject.Color;
                    }
                    else
                    {
                        cmbColor.SelectedIndex = 0;
                        _selectedSubject.Color = cmbColor.SelectedItem.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Validate form input
        /// </summary>
        private bool ValidateInput()
        {
            if (!ValidationHelper.IsValidString(txtCode.Text, 1, 20))
            {
                MessageBox.Show("Please enter a valid subject code (1-20 characters).", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return false;
            }

            if (!ValidationHelper.IsValidString(txtName.Text, 1, 100))
            {
                MessageBox.Show("Please enter a valid subject name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clear all input fields
        /// </summary>
        private void ClearInputs()
        {
            txtCode.Clear();
            txtName.Clear();
            txtInstructor.Clear();
            nudCredits.Value = 3;
            cmbColor.SelectedIndex = 0;
            _selectedSubject = null;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Update color preview when selection changes
        /// </summary>
        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbColor.SelectedItem != null)
            {
                try
                {
                    var color = ColorTranslator.FromHtml(cmbColor.SelectedItem.ToString());
                    panelColorPreview.BackColor = color;
                }
                catch
                {
                    panelColorPreview.BackColor = Color.LightBlue;
                }
            }
        }
    }
}

