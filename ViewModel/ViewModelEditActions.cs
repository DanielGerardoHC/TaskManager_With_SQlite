using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AdministradorDeTareas.Model;
using AdministradorDeTareas.View;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ServiceModel.Channels;
using System.Windows;
using System.Text.Json.Nodes;
using AdministradorDeTareas.Model.DAO;
using static System.Net.WebRequestMethods;

namespace AdministradorDeTareas.ViewModel
{
    public class ViewModelEditActions : ViewModelBase
    {
        public ViewModelEditActions() {
            // inicializamos todos los comandos que usaremos
            ShowAddTask = new ViewModelCommand(ExecuteShowAddTask);
            SearchTask = new ViewModelCommand(ExecuteSearchTask);
            GetTasks = new ViewModelCommand(ExecuteGetTasks);
            ShowViewDeleteTask = new ViewModelCommand(ExecuteShowViewDeleteTask);
            ShowEditTask = new ViewModelCommand(ExecuteShowEditTask);
            // llenamos el listbox llamando al verbo get
            GetAllTasks();
        }
        #region Atributos
        private static TaskModelDAO _taskModelDAO = new TaskModelDAO();
        private List<TaskModel> _tasks;
        private string _txtSearch;
        private TaskModel _selectedTask { get; set; }
        public ICommand ShowEditTask { get;  }
        public ICommand ShowAddTask { get; }
        public ICommand SearchTask { get; }
        public ICommand GetTasks { get; }  
        public ICommand ShowViewDeleteTask {  get; }
        public List<TaskModel> TasksList
        {
            // si o si debe cada propiedad debe tener su get y set
            // junto con el metodo OnpropetyChanged
            get { return _tasks; }
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(TasksList));
            }
        }
        public string TxtSearch
        {
            get { return _txtSearch; }
            set
            {
                _txtSearch = value;
                OnPropertyChanged(nameof(TxtSearch));
            }
        }
        public TaskModel SelectedTask
        {
            get
            {    return _selectedTask; }
            set 
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }

        }
        #endregion
        #region Metodos
        private void ExecuteSearchTask(object obj)
        {
            GetTaskWhere();
        }
        private void RechargeTaskList()
        {
            GetAllTasks();
        }
        private void ExecuteShowAddTask(object obj)
        {
            // Creamos una instancia de la vista viewAddTask
            ViewAddTask viewAddTask = new ViewAddTask();
            ViewModelAddTask viewModelAddTask = viewAddTask.DataContext as ViewModelAddTask;
            if (viewModelAddTask != null)
            {
                // enviamos el metodo RechargeTaskList al delegado del viewModel de
                // la vista viewAddTask
                viewModelAddTask.TaskAdded += RechargeTaskList;
                viewAddTask.ShowDialog();
            }
        }
        private void ExecuteGetTasks(object obj)
        {
            GetAllTasks();
        }
        private void ExecuteShowEditTask(object obj)
        {
            if (SelectedTask != null)
            {
                ViewEditTask viewEditTask = new ViewEditTask(new TaskModel
                {
                    TaskID = _selectedTask.TaskID,
                    Title = _selectedTask.Title,
                    Description = _selectedTask.Description,
                    DueDate = _selectedTask.DueDate,
                    UserID = _selectedTask.UserID,
                    PriorityID = _selectedTask.PriorityID - 1,
                    StatusID = _selectedTask.StatusID - 1,
                    Users = _selectedTask.Users,
                    TaskStatus = _selectedTask.TaskStatus,
                    Priority = SelectedTask.Priority
                });
                ViewModelEditTask viewModelEditTask = viewEditTask.DataContext as ViewModelEditTask;
                if (viewModelEditTask != null)
                {
                    viewModelEditTask.TaskEdited += RechargeTaskList;
                    viewEditTask.ShowDialog();
                }
            }
            else
            {
                CustomMessageBox.ShowCustomMessageBox("Please select a task");
            }
            
        }
        public void ExecuteShowViewDeleteTask(object obj)
        {
            if (SelectedTask != null)
            {
                ViewDeleteTask viewDeleteTask = new ViewDeleteTask(SelectedTask);
                // Asignacion del viewModel como contexto de datos para la vista ViewDeleteTask
                ViewModelDeleteTask viewModelDeleteTask = viewDeleteTask.DataContext as ViewModelDeleteTask;
                if (viewModelDeleteTask != null)
                {
                    // asignacion del metodo RechargeTaskList al delegado de la VistaModelo
                    viewModelDeleteTask.TaskDeleted += RechargeTaskList;
                    viewDeleteTask.ShowDialog();
                }
            }
            else
            {
                CustomMessageBox.ShowCustomMessageBox("Please select a task");
            }
        }
        public async void GetTaskWhere() {
            if (!string.IsNullOrWhiteSpace(TxtSearch)){
                TasksList = await _taskModelDAO.GetWhere(TxtSearch);
            }
        }
        public async void GetAllTasks()
        {
            TasksList = await _taskModelDAO.GetAll();
        }
        #endregion
    }
}