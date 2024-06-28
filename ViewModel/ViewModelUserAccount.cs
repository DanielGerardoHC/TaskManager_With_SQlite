using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AdministradorDeTareas.Model;
using AdministradorDeTareas.View;
using Microsoft.Xaml.Behaviors.Core;

namespace AdministradorDeTareas.ViewModel
{
    public class ViewModelUserAccount : ViewModelBase
    {
        public ViewModelUserAccount() {
            ShowEditCredentialsCommand = new ViewModelCommand(ExecuteShowEditCredentials);
            ShowChangePasswordCommand = new ViewModelCommand(ExecuteShowChangePassword);
            GetAccoutCredentials();
        }
        #region Atributos
        private UsersModel _CurrentUser;
        public UsersModel CurrentUser
        {
            get { return _CurrentUser; }
            set
            {
                _CurrentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        public ICommand ShowEditCredentialsCommand { get; }
        public ICommand ShowChangePasswordCommand { get; }
        #endregion
        #region Metodos
        private void ExecuteShowChangePassword(object obj) {
            ViewChangePassword viewChangePassword = new ViewChangePassword();
            viewChangePassword.ShowDialog();
        }
        private void ExecuteShowEditCredentials(object obj) {
            try {
                ViewEditUserCredentials viewEditUserCredentials = new ViewEditUserCredentials();
                ViewModelEditCredentials viewModelEditCredentials = viewEditUserCredentials.DataContext as ViewModelEditCredentials;
                if (viewModelEditCredentials != null) {
                    // le asignamos al delegado el metodo GetAccountCredentials
                    viewModelEditCredentials.AccountCredentialsEdited += GetAccoutCredentials;
                    viewEditUserCredentials.ShowDialog();
                }
            }
            catch (Exception ex) {
                CustomMessageBox.ShowCustomMessageBox("Error: Could not display the view");
            }
        }
        public void GetAccoutCredentials() {
            CurrentUser = ViewModelBase.GetCurrentUser();
        }
        #endregion
    }

}
