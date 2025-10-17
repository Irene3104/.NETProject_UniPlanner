using System.Drawing;
using System.Windows.Forms;

namespace UniPlanner.Forms
{
    partial class MainForm
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
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.grpTimetable = new System.Windows.Forms.GroupBox();
            this.dgvTimetable = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMonday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTuesday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWednesday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThursday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFriday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSaturday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.grpTodayTasks = new System.Windows.Forms.GroupBox();
            this.lstTodayTasks = new System.Windows.Forms.ListView();
            this.colTaskTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTaskSubject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTaskPriority = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpTodoPreview = new System.Windows.Forms.GroupBox();
            this.lstTodoPreview = new System.Windows.Forms.ListBox();
            this.panelNav = new System.Windows.Forms.Panel();
            this.btnSchedule = new System.Windows.Forms.Button();
            this.btnAssignments = new System.Windows.Forms.Button();
            this.btnTodos = new System.Windows.Forms.Button();
            this.btnSubjects = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.grpTimetable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimetable)).BeginInit();
            this.panelSidebar.SuspendLayout();
            this.grpTodayTasks.SuspendLayout();
            this.grpTodoPreview.SuspendLayout();
            this.panelNav.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panelHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHeader.Controls.Add(this.lblAppTitle);
            this.panelHeader.Controls.Add(this.lblWelcome);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1400, 80);
            this.panelHeader.TabIndex = 0;
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblAppTitle.Location = new System.Drawing.Point(20, 15);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(359, 37);
            this.lblAppTitle.TabIndex = 0;
            this.lblAppTitle.Text = "📚 Smart Student Organizer";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblWelcome.Location = new System.Drawing.Point(25, 52);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(100, 20);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Today's Date";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.grpTimetable);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 80);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(15);
            this.panelMain.Size = new System.Drawing.Size(1050, 670);
            this.panelMain.TabIndex = 1;
            // 
            // grpTimetable
            // 
            this.grpTimetable.BackColor = System.Drawing.Color.White;
            this.grpTimetable.Controls.Add(this.dgvTimetable);
            this.grpTimetable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTimetable.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.grpTimetable.Location = new System.Drawing.Point(15, 15);
            this.grpTimetable.Name = "grpTimetable";
            this.grpTimetable.Padding = new System.Windows.Forms.Padding(10);
            this.grpTimetable.Size = new System.Drawing.Size(1020, 640);
            this.grpTimetable.TabIndex = 0;
            this.grpTimetable.TabStop = false;
            this.grpTimetable.Text = "📅 Weekly Timetable";
            // 
            // dgvTimetable
            // 
            this.dgvTimetable.AllowUserToAddRows = false;
            this.dgvTimetable.AllowUserToDeleteRows = false;
            this.dgvTimetable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTimetable.BackgroundColor = System.Drawing.Color.White;
            this.dgvTimetable.ColumnHeadersHeight = 40;
            this.dgvTimetable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTimetable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime,
            this.colMonday,
            this.colTuesday,
            this.colWednesday,
            this.colThursday,
            this.colFriday,
            this.colSaturday});
            this.dgvTimetable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTimetable.Location = new System.Drawing.Point(10, 29);
            this.dgvTimetable.Name = "dgvTimetable";
            this.dgvTimetable.ReadOnly = true;
            this.dgvTimetable.RowHeadersVisible = false;
            this.dgvTimetable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTimetable.Size = new System.Drawing.Size(1000, 601);
            this.dgvTimetable.TabIndex = 0;
            // 
            // colTime
            // 
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            // 
            // colMonday
            // 
            this.colMonday.HeaderText = "Monday";
            this.colMonday.Name = "colMonday";
            this.colMonday.ReadOnly = true;
            // 
            // colTuesday
            // 
            this.colTuesday.HeaderText = "Tuesday";
            this.colTuesday.Name = "colTuesday";
            this.colTuesday.ReadOnly = true;
            // 
            // colWednesday
            // 
            this.colWednesday.HeaderText = "Wednesday";
            this.colWednesday.Name = "colWednesday";
            this.colWednesday.ReadOnly = true;
            // 
            // colThursday
            // 
            this.colThursday.HeaderText = "Thursday";
            this.colThursday.Name = "colThursday";
            this.colThursday.ReadOnly = true;
            // 
            // colFriday
            // 
            this.colFriday.HeaderText = "Friday";
            this.colFriday.Name = "colFriday";
            this.colFriday.ReadOnly = true;
            // 
            // colSaturday
            // 
            this.colSaturday.HeaderText = "Saturday";
            this.colSaturday.Name = "colSaturday";
            this.colSaturday.ReadOnly = true;
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelSidebar.Controls.Add(this.grpTodoPreview);
            this.panelSidebar.Controls.Add(this.grpTodayTasks);
            this.panelSidebar.Controls.Add(this.panelNav);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSidebar.Location = new System.Drawing.Point(1050, 80);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Padding = new System.Windows.Forms.Padding(10);
            this.panelSidebar.Size = new System.Drawing.Size(350, 670);
            this.panelSidebar.TabIndex = 2;
            // 
            // grpTodayTasks
            // 
            this.grpTodayTasks.BackColor = System.Drawing.Color.White;
            this.grpTodayTasks.Controls.Add(this.lstTodayTasks);
            this.grpTodayTasks.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTodayTasks.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.grpTodayTasks.Location = new System.Drawing.Point(10, 280);
            this.grpTodayTasks.Name = "grpTodayTasks";
            this.grpTodayTasks.Size = new System.Drawing.Size(330, 180);
            this.grpTodayTasks.TabIndex = 1;
            this.grpTodayTasks.TabStop = false;
            this.grpTodayTasks.Text = "📝 Today's Assignments";
            // 
            // lstTodayTasks
            // 
            this.lstTodayTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTaskTitle,
            this.colTaskSubject,
            this.colTaskPriority});
            this.lstTodayTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTodayTasks.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lstTodayTasks.FullRowSelect = true;
            this.lstTodayTasks.GridLines = true;
            this.lstTodayTasks.HideSelection = false;
            this.lstTodayTasks.Location = new System.Drawing.Point(3, 21);
            this.lstTodayTasks.Name = "lstTodayTasks";
            this.lstTodayTasks.Size = new System.Drawing.Size(324, 176);
            this.lstTodayTasks.TabIndex = 0;
            this.lstTodayTasks.UseCompatibleStateImageBehavior = false;
            this.lstTodayTasks.View = System.Windows.Forms.View.Details;
            // 
            // colTaskTitle
            // 
            this.colTaskTitle.Text = "Task";
            this.colTaskTitle.Width = 150;
            // 
            // colTaskSubject
            // 
            this.colTaskSubject.Text = "Subject";
            this.colTaskSubject.Width = 80;
            // 
            // colTaskPriority
            // 
            this.colTaskPriority.Text = "Priority";
            this.colTaskPriority.Width = 70;
            // 
            // grpTodoPreview
            // 
            this.grpTodoPreview.BackColor = System.Drawing.Color.White;
            this.grpTodoPreview.Controls.Add(this.lstTodoPreview);
            this.grpTodoPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTodoPreview.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.grpTodoPreview.Location = new System.Drawing.Point(10, 460);
            this.grpTodoPreview.Name = "grpTodoPreview";
            this.grpTodoPreview.Size = new System.Drawing.Size(330, 200);
            this.grpTodoPreview.TabIndex = 2;
            this.grpTodoPreview.TabStop = false;
            this.grpTodoPreview.Text = "✓ Personal To-Do";
            // 
            // lstTodoPreview
            // 
            this.lstTodoPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTodoPreview.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lstTodoPreview.FormattingEnabled = true;
            this.lstTodoPreview.ItemHeight = 17;
            this.lstTodoPreview.Location = new System.Drawing.Point(3, 21);
            this.lstTodoPreview.Name = "lstTodoPreview";
            this.lstTodoPreview.Size = new System.Drawing.Size(324, 206);
            this.lstTodoPreview.TabIndex = 0;
            // 
            // panelNav
            // 
            this.panelNav.BackColor = System.Drawing.Color.White;
            this.panelNav.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelNav.Controls.Add(this.btnSchedule);
            this.panelNav.Controls.Add(this.btnAssignments);
            this.panelNav.Controls.Add(this.btnTodos);
            this.panelNav.Controls.Add(this.btnSubjects);
            this.panelNav.Controls.Add(this.btnRefresh);
            this.panelNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNav.Location = new System.Drawing.Point(10, 10);
            this.panelNav.Name = "panelNav";
            this.panelNav.Size = new System.Drawing.Size(330, 270);
            this.panelNav.TabIndex = 0;
            // 
            // btnSchedule
            // 
            this.btnSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSchedule.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnSchedule.ForeColor = System.Drawing.Color.White;
            this.btnSchedule.Location = new System.Drawing.Point(15, 15);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(300, 45);
            this.btnSchedule.TabIndex = 0;
            this.btnSchedule.Text = "📅 Manage Class Schedule";
            this.btnSchedule.UseVisualStyleBackColor = false;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            // 
            // btnAssignments
            // 
            this.btnAssignments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnAssignments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAssignments.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAssignments.ForeColor = System.Drawing.Color.White;
            this.btnAssignments.Location = new System.Drawing.Point(15, 70);
            this.btnAssignments.Name = "btnAssignments";
            this.btnAssignments.Size = new System.Drawing.Size(300, 45);
            this.btnAssignments.TabIndex = 1;
            this.btnAssignments.Text = "📝 Manage Assignments";
            this.btnAssignments.UseVisualStyleBackColor = false;
            this.btnAssignments.Click += new System.EventHandler(this.btnAssignments_Click);
            // 
            // btnTodos
            // 
            this.btnTodos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnTodos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTodos.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnTodos.ForeColor = System.Drawing.Color.White;
            this.btnTodos.Location = new System.Drawing.Point(15, 115);
            this.btnTodos.Name = "btnTodos";
            this.btnTodos.Size = new System.Drawing.Size(300, 45);
            this.btnTodos.TabIndex = 2;
            this.btnTodos.Text = "✓ Manage Personal To-Do";
            this.btnTodos.UseVisualStyleBackColor = false;
            this.btnTodos.Click += new System.EventHandler(this.btnTodos_Click);
            // 
            // btnSubjects
            // 
            this.btnSubjects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnSubjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubjects.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnSubjects.ForeColor = System.Drawing.Color.White;
            this.btnSubjects.Location = new System.Drawing.Point(15, 165);
            this.btnSubjects.Name = "btnSubjects";
            this.btnSubjects.Size = new System.Drawing.Size(300, 45);
            this.btnSubjects.TabIndex = 3;
            this.btnSubjects.Text = "📚 Manage Subjects";
            this.btnSubjects.UseVisualStyleBackColor = false;
            this.btnSubjects.Click += new System.EventHandler(this.btnSubjects_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRefresh.Location = new System.Drawing.Point(15, 220);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(300, 35);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "🔄 Refresh Dashboard";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1400, 750);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smart Student Organizer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.grpTimetable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTimetable)).EndInit();
            this.panelSidebar.ResumeLayout(false);
            this.grpTodayTasks.ResumeLayout(false);
            this.grpTodoPreview.ResumeLayout(false);
            this.panelNav.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.GroupBox grpTimetable;
        private System.Windows.Forms.DataGridView dgvTimetable;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.GroupBox grpTodayTasks;
        private System.Windows.Forms.ListView lstTodayTasks;
        private System.Windows.Forms.ColumnHeader colTaskTitle;
        private System.Windows.Forms.ColumnHeader colTaskSubject;
        private System.Windows.Forms.ColumnHeader colTaskPriority;
        private System.Windows.Forms.GroupBox grpTodoPreview;
        private System.Windows.Forms.ListBox lstTodoPreview;
        private System.Windows.Forms.Panel panelNav;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.Button btnAssignments;
        private System.Windows.Forms.Button btnTodos;
        private System.Windows.Forms.Button btnSubjects;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMonday;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTuesday;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWednesday;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThursday;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFriday;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSaturday;
    }
}
