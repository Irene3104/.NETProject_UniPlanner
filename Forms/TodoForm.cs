using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniPlanner.Models;
using UniPlanner.Services;

namespace UniPlanner.Forms
{
    /// <summary>
    /// Personal to-do list management form
    /// </summary>
    public partial class TodoForm : Form
    {
        private readonly TodoService _todoService;

        public TodoForm()
        {
            InitializeComponent();
            _todoService = new TodoService();
            
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9.75F);
            
            LoadTodos();
        }

        private void LoadTodos()
        {
            lstTodos.Items.Clear();
            
            var todos = _todoService.GetAll();
            foreach (var todo in todos)
            {
                // Add the TodoItem object itself
                int index = lstTodos.Items.Add(todo);
                lstTodos.SetItemChecked(index, todo.IsCompleted);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewTodo.Text))
                return;

            var todo = new TodoItem(txtNewTodo.Text.Trim());
            _todoService.Add(todo);
            
            txtNewTodo.Clear();
            LoadTodos();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstTodos.SelectedItem == null)
                return;

            var todo = lstTodos.SelectedItem as TodoItem;
            if (todo != null)
            {
                _todoService.Delete(todo.Id);
                LoadTodos();
            }
        }

        private void lstTodos_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var todo = lstTodos.Items[e.Index] as TodoItem;
            if (todo != null)
            {
                _todoService.ToggleComplete(todo.Id);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

