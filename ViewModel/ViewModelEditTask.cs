 using AdministradorDeTareas.Model;
using AdministradorDeTareas.Model.DAO;
using AdministradorDeTareas.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdministradorDeTareas.ViewModel
{
    public class ViewModelEditTask : ViewModelEditActions
    {
        public ViewModelEditTask() {
            EditTaskCommand = new ViewModelCommand(ExecuteEditTask);
        }
        #region Atributos
        private TaskModelDAO taskModelDAO = new TaskModelDAO();
        public delegate void TaskEditedEventHandler();
        public event TaskEditedEventHandler TaskEdited;
        private string? _description;
        private string? _title;
        private DateTime? _dueDate;
        private TaskModel _selectedTask;
        public TaskModel SelectedTask {
            get { return _selectedTask; }
            set {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
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
        public ICommand EditTaskCommand { get; }        
        #endregion
        #region Metodos
        private async void ExecuteEditTask(object obj)
        {
            if (IsValidTask()) 
            {
                // llamamos al metodo que ejecutara la modificacion
                if (await taskModelDAO.Modify(ModifiedTask()))
                {
                    TaskEdited.Invoke();
                    closeActualWindow();
                }
            }
        }
        private bool IsValidTask()
        {
           if (!string.IsNullOrWhiteSpace(SelectedTask.Title)
                && !string.IsNullOrWhiteSpace(SelectedTask.Description) && !string.IsNullOrWhiteSpace(SelectedTask.DueDate.ToString())
                && SelectedTask.PriorityID != null && SelectedTask.StatusID != null)
           {
             return true;
           }
           else
           {
             CustomMessageBox.ShowCustomMessageBox("Please fill in all fields");
             return false;
           }
        }
        private TaskModel ModifiedTask()
        {
            return new TaskModel
            {
                TaskID = SelectedTask.TaskID,
                Title = SelectedTask.Title,
                Description = SelectedTask.Description,
                DueDate = SelectedTask.DueDate,
                UserID = SelectedTask.UserID,
                // ya que el index de combo box inicia en 0 entonces le aumentamos 1 para que
                // coincida con el Id identity de la base de datos
                PriorityID = SelectedTask.PriorityID + 1,
                StatusID = SelectedTask.StatusID + 1,
                Users = SelectedTask.Users,
                TaskStatus = SelectedTask.TaskStatus,
                Priority = SelectedTask.Priority
            };
        }
        private void closeActualWindow()
        {
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            // cerrar la ventana si se encuentra y si la tarea se modifico con exito
            if (window != null)
            {
                window.Close();
            }
        }
        #endregion
    }

}
