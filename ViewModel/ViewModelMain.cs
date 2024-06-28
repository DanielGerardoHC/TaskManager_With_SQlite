using AdministradorDeTareas.View;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace AdministradorDeTareas.ViewModel
{
    public class ViewModelMain : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        public ICommand ShowEditActionsCommand { get; }
        public ICommand ShowDashBoardCommand { get; }
        public ICommand ShowViewOptionsCommand { get; }
        public ICommand OpenGithubProfile {  get; }
        public ICommand LogOutCommand { get; }
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        
        public ViewModelMain()
        {
            //comands
            ShowViewOptionsCommand = new ViewModelCommand(ExecuteShowViewOptionsCommand);
            ShowEditActionsCommand = new ViewModelCommand(ExecuteShowEditActionsCommand);
            ShowDashBoardCommand = new ViewModelCommand(ExecuteShowDashBoardCommand);
            OpenGithubProfile = new ViewModelCommand(ExecuteOpenGithubProfile);
            LogOutCommand = new ViewModelCommand(ExecuteLogOutCommand);
            //Default 
            ExecuteShowDashBoardCommand(null);
        }

        private void ExecuteOpenGithubProfile(object obj)
        {
            string url = "https://github.com/DanielGerardoHC/TaskManager_With_Sqlite";
            // abre el enlace en el navegador predeterminado del sistema
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        private void ExecuteShowEditActionsCommand(object obj)
        {
            CurrentChildView = new ViewModelEditActions();
        }
        private void ExecuteShowViewOptionsCommand(object obj)
        {
            CurrentChildView = new ViewModelUserAccount();
        }
        private void ExecuteShowDashBoardCommand(object? obj)
        {
            CurrentChildView = new ViewModelDashBoard();
        }
        private void ExecuteLogOutCommand(object obj)
        {
            ViewModelBase.SetCurrentUser(null);
            ViewLogin viewLogin = new ViewLogin();
            viewLogin.Show();
            Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
            //cerrar la ventana si se encuentra y si la tarea se modifico con exito
            if (window != null)
            {
                window.Close();
            }
        }

    }
}
