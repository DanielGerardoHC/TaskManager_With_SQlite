using AdministradorDeTareas.Model;
using AdministradorDeTareas.Model.DAO;
using AdministradorDeTareas.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdministradorDeTareas.ViewModel
{
    public class ViewModelRegister : ViewModelBase
    {
        readonly UsersModelDAO usersModelDAO = new UsersModelDAO();
        readonly TaskModelDAO taskModelDAO = new TaskModelDAO();
        private string? _userName;
        private string? _password;
        private string? _email;
        private string? _name;
        public string? userName {
            get { return _userName; }
            set 
            { 
                  this._userName = value;
                  OnPropertyChanged(nameof(userName));
            } 
        }
        public string? password { 
            get { return _password; } 
            set {
                this._password = value;
                OnPropertyChanged(nameof(password));
            } 
        }
        public string? email { 
            get { return _email; } 
            set 
            { 
                this._email = value;
                OnPropertyChanged(nameof(email));
            } 
        }
        public string? name { 
            get { return _name; } 
            set 
            { 
                this._name = value;
                OnPropertyChanged(nameof(name));
            } 
        }
        public ViewModelRegister() 
        {
            ReturnLoginPageCommand = new ViewModelCommand(ExecuteReturnLoginPage);       
            ShowAlertCommand = new ViewModelCommand(ExecuteShowAlert);
            RegisterCommand = new ViewModelCommand(ExecuteRegister);
        }
        public ICommand ReturnLoginPageCommand { get; }
        public ICommand ShowAlertCommand { get; }
        public ICommand RegisterCommand { get; }

        public void ExecuteReturnLoginPage(object obj)
        {
            ReturnLoginPage();
        }
        public void ExecuteShowAlert(object obj)
        {
            CustomMessageBox.ShowCustomMessageBox("This username you will use to log in!");
        }
        public void ExecuteRegister(object obj)
        {
            Register();
        }
        public void ReturnLoginPage()
        {
            ViewLogin loginView = new ViewLogin();
            loginView.Show();
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            // cerrar la ventana si se encuentra
            if (window != null)
            {
                window.Close();
            }
        }
        private async void Register() {
            try {
                if (await isValidCredentialsAsync()) {
                    UsersModel newUser = new UsersModel();
                    newUser.UserName = this.userName;
                    newUser.FullName = this.name;
                    newUser.Email = this.email;
                    newUser.PasswordHash = this.password;
                    if (await usersModelDAO.Add(newUser)) {
                        // regresamos al usuario a la vista del log in
                        if (await addDefaultTasksAsync(newUser))
                        {
                            ViewLogin loginView = new ViewLogin();
                            loginView.Show();
                            closeActualWindow();
                        }
                    }
                }
            }
            catch (Exception ex) {
                CustomMessageBox.ShowCustomMessageBox("Something went wrong on the registration msg: "+ex.Message);
            }
        }
        private async Task<bool> isValidCredentialsAsync()
        {
            if (!string.IsNullOrWhiteSpace(password)
                && !string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(email)
                && !string.IsNullOrWhiteSpace(name))
            {
                if (!await usersModelDAO.Exist(this.userName))
                {
                    if (this.password.Length > 4)
                    {
                        return true;
                    }

                    CustomMessageBox.ShowCustomMessageBox("Password must be at least 4 characters long");
                    return false;
                }

                CustomMessageBox.ShowCustomMessageBox("A user with this UserName already exists");
                return false;
            }
            CustomMessageBox.ShowCustomMessageBox("Please fill in all registration fields");
            return false;
        }
        private void closeActualWindow()
        {
            Window? window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            // cerrar la ventana si se encuentra
            if (window != null)
            {
                window.Close();
            }
        }
        private async Task<bool> addDefaultTasksAsync(UsersModel newUser)
        {
            try
            {
                // obtener el registro de la bd del usuario que se acaba de registrar
                UsersModel currentUser = await usersModelDAO.Auth(newUser);

                // obtener el id del usuario
                int? userId = currentUser.UserId;
                TaskModel highPriorityExample = new TaskModel {
                    Title = "Example High Priority Task",
                    Description = "Here is an example of a high-priority task and pending",
                    DueDate = DateTime.Now,
                    PriorityID = 3,
                    StatusID = 1,
                    UserID = userId,
                };
                TaskModel LowPriorityExample = new TaskModel
                {
                    Title = "Example Low Priority Task",
                    Description = "Here is an example of a Low-priority task and in progress",
                    DueDate = DateTime.Now,
                    PriorityID = 1,
                    StatusID = 2,
                    UserID = userId
                };
                TaskModel MediumPriorityExample = new TaskModel
                {
                    Title = "Example Medium Priority Task",
                    Description = "Here is an example of a Medium-priority task and Completed",
                    DueDate = DateTime.Now,
                    PriorityID = 2,
                    StatusID = 3,
                    UserID = userId
                };
                taskModelDAO.Add(highPriorityExample);
                taskModelDAO.Add(MediumPriorityExample);
                taskModelDAO.Add(LowPriorityExample);
                return true;
            }
            catch(Exception ex)
            {
                CustomMessageBox.ShowCustomMessageBox("Failed to load default tasks");
                return false;
            }
        }
    }
}
