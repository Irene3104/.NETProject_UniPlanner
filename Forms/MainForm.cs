using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniPlanner.Models;
using UniPlanner.Services;
using UniPlanner.Utils;

namespace UniPlanner.Forms
{
    /// <summary>
    /// Main dashboard with weekly timetable view
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly ScheduleService _scheduleService;
        private readonly TaskService _taskService;
        private readonly TodoService _todoService;
        private readonly SubjectService _subjectService;

        public MainForm()
        {
            InitializeComponent();
            _scheduleService = new ScheduleService();
            _taskService = new TaskService();
            _todoService = new TodoService();
            _subjectService = new SubjectService();
            
            // Modern styling
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9.75F);
            
            LoadDashboard();
        }

        /// <summary>
        /// Load all dashboard data
        /// </summary>
        private void LoadDashboard()
        {
            LoadWeeklyTimetable();
            LoadTodaysTasks();
            LoadTodoPreview();
            UpdateWelcomeMessage();
        }

        /// <summary>
        /// Load weekly timetable into DataGridView
        /// </summary>
        private void LoadWeeklyTimetable()
        {
            try
            {
                dgvTimetable.Rows.Clear();

                var schedulesGrouped = _scheduleService.GetGroupedByDay();
                var subjectLookup = _subjectService.GetAll()
                    .GroupBy(s => s.Code)
                    .ToDictionary(g => g.Key, g => g.First());

                var scheduleWithTimes = schedulesGrouped.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value
                        .Select(s => new
                        {
                            Schedule = s,
                            Start = ParseTimeSpan(s.StartTime),
                            End = ParseTimeSpan(s.EndTime)
                        })
                        .Where(x => x.Start.HasValue && x.End.HasValue)
                        .ToList()
                );

                var timeSlots = new List<TimeSpan>();
                for (var time = TimeSpan.FromHours(8); time < TimeSpan.FromHours(18); time = time.Add(TimeSpan.FromHours(1)))
                {
                    timeSlots.Add(time);
                }

                foreach (var slot in timeSlots)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(dgvTimetable);
                    row.Cells[0].Value = slot.ToString(@"hh\:mm");

                    var hourEnd = slot.Add(TimeSpan.FromHours(1));

                    for (int day = 1; day <= 6; day++)
                    {
                        var cell = row.Cells[day];
                        cell.Value = string.Empty;
                        cell.Tag = null;

                        if (!scheduleWithTimes.ContainsKey(day))
                            continue;

                        var match = scheduleWithTimes[day]
                            .FirstOrDefault(x => x.Start < hourEnd && x.End > slot);

                        if (match == null)
                            continue;

                        var scheduleItem = match.Schedule;
                        var overlapStart = match.Start.Value > slot ? match.Start.Value : slot;
                        var overlapEnd = match.End.Value < hourEnd ? match.End.Value : hourEnd;

                        var startMinutes = (overlapStart - slot).TotalMinutes;
                        var endMinutes = (overlapEnd - slot).TotalMinutes;

                        SubjectItem subjectItem = null;
                        if (!string.IsNullOrWhiteSpace(scheduleItem.Subject) && subjectLookup.TryGetValue(scheduleItem.Subject, out var foundSubject))
                        {
                            subjectItem = foundSubject;
                        }

                        var subjectName = !string.IsNullOrWhiteSpace(scheduleItem.SubjectName)
                            ? scheduleItem.SubjectName
                            : subjectItem?.Name ?? scheduleItem.Subject;

                        var location = scheduleItem.Location ?? string.Empty;
                        var displayText = string.IsNullOrWhiteSpace(location)
                            ? subjectName
                            : $"{subjectName}\n{location}";

                        var showText = match.Start.Value >= slot && match.Start.Value < hourEnd;

                        cell.Tag = new ScheduleCellInfo
                        {
                            Schedule = scheduleItem,
                            Subject = subjectItem,
                            StartMinutes = startMinutes,
                            EndMinutes = endMinutes,
                            ShowText = showText,
                            DisplayText = displayText
                        };

                        if (showText)
                        {
                            cell.Value = displayText;
                        }
                    }

                    dgvTimetable.Rows.Add(row);
                }

                dgvTimetable.RowTemplate.Height = 80;
                dgvTimetable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTimetable.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvTimetable.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
                dgvTimetable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                dgvTimetable.CellClick -= DgvTimetable_CellClick;
                dgvTimetable.CellClick += DgvTimetable_CellClick;

                dgvTimetable.CellPainting -= DgvTimetable_CellPainting;
                dgvTimetable.CellPainting += DgvTimetable_CellPainting;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading timetable: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load today's assignments
        /// </summary>
        private void LoadTodaysTasks()
        {
            lstTodayTasks.Items.Clear();

            var today = DateTime.Today;
            var tasks = _taskService.GetAll()
                .Where(t => !t.IsCompleted && t.DueDate.Date == today)
                .OrderBy(t => t.Priority == "High" ? 1 : t.Priority == "Medium" ? 2 : 3)
                .ToList();

            if (tasks.Count == 0)
            {
                var item = new ListViewItem("No assignments due today");
                item.ForeColor = Color.Gray;
                lstTodayTasks.Items.Add(item);
            }
            else
            {
                foreach (var task in tasks)
                {
                    var item = new ListViewItem(task.Title);
                    item.SubItems.Add(task.Subject ?? "");
                    item.SubItems.Add(task.Priority);
                    
                    if (task.Priority == "High")
                        item.ForeColor = Color.FromArgb(231, 76, 60);
                    else if (task.Priority == "Medium")
                        item.ForeColor = Color.FromArgb(241, 196, 15);
                    
                    lstTodayTasks.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Load personal to-do preview
        /// </summary>
        private void LoadTodoPreview()
        {
            lstTodoPreview.Items.Clear();

            var todos = _todoService.GetActive().Take(5).ToList();

            if (todos.Count == 0)
            {
                lstTodoPreview.Items.Add("No active to-dos");
            }
            else
            {
                foreach (var todo in todos)
                {
                    lstTodoPreview.Items.Add($"☐ {todo.Title}");
                }
            }
        }

        /// <summary>
        /// Update welcome message with current date
        /// </summary>
        private void UpdateWelcomeMessage()
        {
            var today = DateTime.Today;
            var dayName = today.ToString("dddd");
            var dateStr = today.ToString("MMMM dd, yyyy");
            
            lblWelcome.Text = $"{dayName}, {dateStr}";
        }

        // Navigation button handlers
        private void btnSchedule_Click(object sender, EventArgs e)
        {
            using (var form = new ScheduleForm())
            {
                form.ShowDialog();
                LoadDashboard(); // Refresh
            }
        }

        private void btnAssignments_Click(object sender, EventArgs e)
        {
            using (var form = new TaskForm())
            {
                form.ShowDialog();
                LoadDashboard();
            }
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            using (var form = new TodoForm())
            {
                form.ShowDialog();
                LoadDashboard();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void btnSubjects_Click(object sender, EventArgs e)
        {
            using (var form = new SubjectForm())
            {
                form.ShowDialog();
                LoadDashboard(); // Refresh after changes
            }
        }

        /// <summary>
        /// Show detailed information when clicking on a timetable cell
        /// </summary>
        private void DgvTimetable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header and time column clicks
            if (e.RowIndex < 0 || e.ColumnIndex <= 0) return;

            var cell = dgvTimetable.Rows[e.RowIndex].Cells[e.ColumnIndex];
            
            // Check if cell has schedule data
            if (cell.Tag is Models.ScheduleItem scheduleItem)
            {
                // Get subject details
                var subject = _subjectService.GetByCode(scheduleItem.Subject);
                
                // Show custom popup form
                using (var detailForm = new ClassDetailForm(scheduleItem, subject))
                {
                    detailForm.ShowDialog(this);
                }
            }
        }

        private TimeSpan? ParseTimeSpan(string time)
        {
            if (TimeSpan.TryParse(time, out var value))
            {
                return value;
            }
            return null;
        }

        private Color GetSubjectColor(string colorHex)
        {
            if (string.IsNullOrWhiteSpace(colorHex))
            {
                return Color.LightBlue;
            }

            try
            {
                return ColorTranslator.FromHtml(colorHex);
            }
            catch
            {
                return Color.LightBlue;
            }
        }

        private Color GetContrastColor(Color background)
        {
            // Use perceived luminance to decide text color
            var luminance = (0.299 * background.R + 0.587 * background.G + 0.114 * background.B) / 255;
            return luminance > 0.5 ? Color.Black : Color.White;
        }

        private class ScheduleCellInfo
        {
            public Models.ScheduleItem Schedule { get; set; }
            public SubjectItem Subject { get; set; }
            public double StartMinutes { get; set; }
            public double EndMinutes { get; set; }
            public bool ShowText { get; set; }
            public string DisplayText { get; set; }
        }

        private void DgvTimetable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex <= 0)
            {
                return;
            }

            var cell = dgvTimetable.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (cell.Tag is ScheduleCellInfo info && info != null)
            {
                e.Handled = true;

                using (var gridBackground = new SolidBrush(dgvTimetable.BackgroundColor))
                {
                    e.Graphics.FillRectangle(gridBackground, e.CellBounds);
                }

                var color = GetSubjectColor(info.Subject?.Color);

                var width = e.CellBounds.Width - 4;
                var height = e.CellBounds.Height - 4;
                var x = e.CellBounds.X + 2;
                var y = e.CellBounds.Y + 2;

                var startRatio = Math.Max(0, info.StartMinutes / 60.0);
                var endRatio = Math.Min(1, info.EndMinutes / 60.0);

                var fillHeight = height * (endRatio - startRatio);
                var fillY = y + height * startRatio;

                if (fillHeight > 0)
                {
                    var fillRect = new RectangleF(x, (float)fillY, width, (float)fillHeight);
                    using (var fillBrush = new SolidBrush(color))
                    {
                        e.Graphics.FillRectangle(fillBrush, fillRect);
                    }
                }

                if (info.ShowText && !string.IsNullOrWhiteSpace(info.DisplayText))
                {
                    var textColor = GetContrastColor(color);
                    using (var textBrush = new SolidBrush(textColor))
                    {
                        var rect = new RectangleF(e.CellBounds.X + 4, e.CellBounds.Y + 4,
                            e.CellBounds.Width - 8, e.CellBounds.Height - 8);

                        var sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };

                        e.Graphics.DrawString(info.DisplayText, dgvTimetable.Font, textBrush, rect, sf);
                    }
                }

                bool hasPrev = e.RowIndex > 0 &&
                    dgvTimetable.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Tag is ScheduleCellInfo prevInfo &&
                    prevInfo.Schedule.Id == info.Schedule.Id;

                bool hasNext = e.RowIndex < dgvTimetable.Rows.Count - 1 &&
                    dgvTimetable.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Tag is ScheduleCellInfo nextInfo &&
                    nextInfo.Schedule.Id == info.Schedule.Id;

                var gridColor = dgvTimetable.GridColor;

                using (var gridPen = new Pen(gridColor))
                {
                    // Always draw left/right borders fully
                    e.Graphics.DrawLine(gridPen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom - 1);
                    e.Graphics.DrawLine(gridPen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

                    // Determine which top/bottom segments should be hidden
                    bool hideTop = hasPrev && info.StartMinutes <= 0;
                    bool hideBottom = hasNext && info.EndMinutes >= 60;

                    if (!hideTop || info.StartMinutes > 0)
                    {
                        if (hideTop)
                        {
                            using (var blendBrush = new SolidBrush(color))
                            {
                                var blendRect = new RectangleF(e.CellBounds.Left + 1, e.CellBounds.Top, e.CellBounds.Width - 2, 2);
                                e.Graphics.FillRectangle(blendBrush, blendRect);
                            }
                        }
                        else
                        {
                            e.Graphics.DrawLine(gridPen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Top);
                        }
                    }

                    if (!hideBottom || info.EndMinutes < 60)
                    {
                        if (hideBottom)
                        {
                            using (var blendBrush = new SolidBrush(color))
                            {
                                var blendRect = new RectangleF(e.CellBounds.Left + 1, e.CellBounds.Bottom - 2, e.CellBounds.Width - 2, 2);
                                e.Graphics.FillRectangle(blendBrush, blendRect);
                            }
                        }
                        else
                        {
                            e.Graphics.DrawLine(gridPen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                        }
                    }
                }
            }
        }
    }
}
