using AdministradorDeTareas.Model;
using LiveCharts;
using LiveCharts.Wpf;
using AdministradorDeTareas.Model.DAO;
using AdministradorDeTareas.View;

namespace AdministradorDeTareas.ViewModel
{
    public class ViewModelDashBoard : ViewModelBase
    {
        public ViewModelDashBoard() {
            PriorityTasksCollection = new SeriesCollection();
            StatusTasksCollection = new SeriesCollection();
            GetTasksFromApi();
        }
        #region Atributos
        private readonly TaskModelDAO _taskModelDao = new TaskModelDAO();
        private List<TaskModel>? _tasks;
        private List<TaskModel>? _pendingTasks;
        private List<TaskModel>? _highPriorityTasks;
        private List<TaskModel>? _lastTaskAdded;
        public List<TaskModel> TasksList
        {
            get
            {
                return _tasks;
            }
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(TasksList));
            }
        }
        public List<TaskModel> PendingTasks
        {
            get { return _pendingTasks; }
            set
            {
                _pendingTasks = value;
                OnPropertyChanged(nameof(PendingTasks));
            }
        }
        public List<TaskModel> HighPriorityTasks
        {
            get { return _highPriorityTasks; }
            set
            {
                _highPriorityTasks = value;
                OnPropertyChanged(nameof(HighPriorityTasks));
            }
        }
        public List<TaskModel> LastTaskAdded
        {
            get { return _lastTaskAdded; }
            set
            {
                _lastTaskAdded = value;
                OnPropertyChanged(nameof(LastTaskAdded));
            }
        }        
        public SeriesCollection PriorityTasksCollection { get; set; }
        public SeriesCollection StatusTasksCollection { get; set; }
        #endregion campos
        #region Metodos
        private async void GetTasksFromApi() {
            try { 
               // llamada al verbo get de la Api
               TasksList = await _taskModelDao.GetAll();
            
               // limpiar los piechart 
               if (TasksList != null)
               {
                   CreatePieCharts();
                   ShowTasksInfo();
               }
            }
            catch (Exception ex) {
                CustomMessageBox.ShowCustomMessageBox($"Error in dashboard. Operation could not be completed. message: {ex.Message}");
            }
        }
        private void CreatePieCharts()
        {
            // agrupar por estados de las tareas
            var tasksByStatus = TasksList.Where(x => x.TaskStatus != null).GroupBy(t => t.TaskStatus.StatusName)
                .Select(c => new { StatusName = c.Key, Count = c.Count() });
            
            // agrupar por prioridad de las tareas
            var tasksByPriority = TasksList.Where(x => x.Priority != null).GroupBy(t => t.Priority.PriorityStatus)
                              .Select(c => new { PriorityStatus = c.Key, Count = c.Count() });

            // cargar la agrupacion en la collection del piechart
            foreach (var group in tasksByStatus)
            {
                StatusTasksCollection.Add(new PieSeries
                {
                    Title = group.StatusName,
                    Values = new ChartValues<int> { group.Count }
                });
            }
            // cargar la agrupacion en la collection del piechart
            foreach (var group in tasksByPriority)
            {
                PriorityTasksCollection.Add(new PieSeries
                {
                    Title = group.PriorityStatus,
                    Values = new ChartValues<int> { group.Count }
                });
            }
        }
        private void ShowTasksInfo() {
            try {
                //filtrar las tareas que tengan un estado pendiente
                var pendingTasks = TasksList.Where(x => x.StatusID == 1).Reverse().ToList();
                
                //filtrar  las tareas que tengan prioridad  alta
                var highPriorityTasks = TasksList.Where(x => x.PriorityID == 3).Reverse().ToList();
                
                //ordenar las tareas por las ultimas agregadas 
                var lasTaskAdded = TasksList.ToList();
                lasTaskAdded.Reverse();
                
                //usar unicamente los primeros 3 registros
                HighPriorityTasks = highPriorityTasks.Take(3).ToList();
                LastTaskAdded = lasTaskAdded.Take(3).ToList();
                PendingTasks = pendingTasks.Take(3).ToList();
            }
            catch (Exception ex) {
                CustomMessageBox.ShowCustomMessageBox($"Error in dashboard. Operation could not be completed 'Load info tasks'. message: {ex.Message}");
            }
        }
        #endregion Metodos
    }
}
