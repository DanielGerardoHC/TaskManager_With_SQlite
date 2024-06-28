using AdministradorDeTareas.Model;
using AdministradorDeTareas.Model.DAO;
using AdministradorDeTareas.View;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualBasic.ApplicationServices;

namespace AdministradorDeTareas.ViewModel
{
    public class ViewModelLogIn : ViewModelBase
    {
        public ViewModelLogIn()
        {
            LoginCommand = new ViewModelCommand(ExecuteLogin);
            RegisterCommand = new ViewModelCommand(ExecuteRegister);
            EnsureCreatedDataBase();
        }
        #region Atributos
        UsersModelDAO UserDAO = new UsersModelDAO();
        private string? _password;
        private string? _username;
        public string? Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string? Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        #endregion
        #region  Metodos
        public void ExecuteLogin(object obj)
        {
           Login();
        }
        public void ExecuteRegister(object obj)
        { 
            ViewRegister viewRegister = new ViewRegister();
            viewRegister.Show();
            CloseActualWindow();
        }
        public async void EnsureCreatedDataBase() {
            // metodo para revisar que exista la base de datos Sqlite
                using (var db = new TaskManagerDbContext()) { 
                    if (await db.Database.EnsureCreatedAsync()) {
                        List<PriorityModel> prioritiesList = new List<PriorityModel> {
                            new PriorityModel { PriorityStatus = "Low" },
                            new PriorityModel { PriorityStatus = "Medium" },
                            new PriorityModel { PriorityStatus = "High" }
                        };
                        await db.Priorities.AddRangeAsync(prioritiesList);
                        List<TaskStatusModel> taskStatusList = new List<TaskStatusModel> {
                            new TaskStatusModel { StatusName = "Pending" },
                            new TaskStatusModel { StatusName = "In Progress" },
                            new TaskStatusModel { StatusName = "Completed" }
                        };
                        await db.TaskStatuses.AddRangeAsync(taskStatusList);
                        await db.SaveChangesAsync();
                    }
                }
        }
        public async void Login() {
            if (Username != null && Password != null) 
            {
                UsersModel authUser = new UsersModel
                {
                   UserId = 0,
                   PasswordHash = this.Password,
                   UserName = this.Username, 
                   Email = "null",
                   FullName = "null"
                };
                
                UsersModel currentUser = await UserDAO.Auth(authUser);
                if (currentUser != null) {
                    ViewModelBase.SetCurrentUser(currentUser);
                    ViewMainWindow Main = new ViewMainWindow();
                    Main.Show();
                    CloseActualWindow();
                }
                else
                {
                    CustomMessageBox.ShowCustomMessageBox("The username or password are incorrect");
                }
            }
            else {
                CustomMessageBox.ShowCustomMessageBox("Please enter valid credentials");
            }
        }
        private void CloseActualWindow() {
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            // cerrar la ventana si se encuentra y si la tarea se modifico con exito
            if (window != null) {
                window.Close();
            }
        }
        #endregion
    }
}
