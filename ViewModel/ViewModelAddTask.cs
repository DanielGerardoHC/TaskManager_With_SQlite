using AdministradorDeTareas.Model;
using AdministradorDeTareas.Model.DAO;
using AdministradorDeTareas.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
namespace AdministradorDeTareas.ViewModel
{
    public class ViewModelAddTask : ViewModelEditActions
    {
        public ViewModelAddTask() {
            // inicializacion de comandos
            CreateTask = new ViewModelCommand(ExecuteCreateTask);
        }
        #region Atributos
        private readonly TaskModelDAO _taskModelDao = new TaskModelDAO();
        private int? _prioritySelect;
        private string? _description;
        private string? _title;
        private DateTime? _dueDate;
        public delegate void TaskAddedEventHandler();
        public event TaskAddedEventHandler TaskAdded;
        public int? PrioritySelect
        {
            get { return _prioritySelect; }
            set
            {
                _prioritySelect = value;
                OnPropertyChanged(nameof(PrioritySelect));
            }
        }
        public string? Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public string? Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));

            }
        }
        public DateTime? DueDate
        {
            get { return _dueDate; }
            set
            {
                _dueDate = value;
                OnPropertyChanged(nameof(DueDate));
            }
        }
        public ICommand CreateTask { get; }
        #endregion Campos
        #region Metodos
        private async void AddTask() {
            try
            {
                if (IsValidTask())
                {
                    TaskModel newTask = new TaskModel();
                    newTask.StatusID = 1; //estado pendiente por default al agregar una nueva tarea
                    newTask.PriorityID = (int)(PrioritySelect + 1);
                    newTask.Title = Title;
                    newTask.Description = Description;
                    newTask.DueDate = DueDate;
                    newTask.UserID = (int)ViewModelBase.GetCurrentUser().UserId; //id del usuario que ha iniciado sesion
                    if (await _taskModelDao.Add(newTask))
                    {
                        //si el metodo post nos devuelve un true se llama al delegado
                        TaskAdded?.Invoke();
                        CloseActualWindow();
                    }
                }
            }
            catch(Exception ex) {
                CustomMessageBox.ShowCustomMessageBox($"Error: Operation could not be completed. Cod: {ex.Message}");
            }
        }
        private void ExecuteCreateTask(object obj) {
            AddTask();
        }
        private bool IsValidTask()
        {
            if(!string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Description) 
               && !string.IsNullOrWhiteSpace(DueDate.ToString())  && PrioritySelect != null)
            {
                return true;
            }
            else
            {
                CustomMessageBox.ShowCustomMessageBox("Please fill in all fields");
                return false;
            }       
        }
        private void CloseActualWindow() {
            //buscar la ventana actual
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            //cerrar la ventana si se encuentra y si la tarea se modifico con exito
            if (window != null) {
                window.Close();
            }
        }
        #endregion
    }
}
