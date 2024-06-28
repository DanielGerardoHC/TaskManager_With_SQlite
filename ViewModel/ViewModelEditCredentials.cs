using System.Windows.Input;
using AdministradorDeTareas.Model;
using AdministradorDeTareas.Model.DAO;
using System.Windows;
using AdministradorDeTareas.View;

namespace AdministradorDeTareas.ViewModel;

public class ViewModelEditCredentials : ViewModelUserAccount
{
    public ViewModelEditCredentials()
    {
        this.CurrentUser = new UsersModel
        {
            UserId = ViewModelBase.GetCurrentUser().UserId,
            UserName = ViewModelBase.GetCurrentUser().UserName,
            FullName = ViewModelBase.GetCurrentUser().FullName,
            Email = ViewModelBase.GetCurrentUser().Email,
            PasswordHash = ViewModelBase.GetCurrentUser().PasswordHash,
        };
        EditCredentialsCommand = new ViewModelCommand(ExecuteEditCredentials);
        CancelEditCommand = new ViewModelCommand(ExecuteCancelEdit);
    }
    #region Atributos
    private readonly UsersModelDAO _usersModelDao = new UsersModelDAO();
    public delegate void TaskDeletedEventHandler();
    public event TaskDeletedEventHandler AccountCredentialsEdited;
    public UsersModel? CurrentUser { get; set; }
    private string _password;
    public string Password
    {
        get { return _password;}
        set {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }
    public ICommand EditCredentialsCommand { get; }
    public ICommand CancelEditCommand { get; }
    #endregion
    #region Metodos
    private void ExecuteEditCredentials(object obj) {
        EditCredentials();
    }
    private void ExecuteCancelEdit(object obj)
    {
        this.CurrentUser = null;
        CloseActualWindow();
    }
    private async void EditCredentials() {
        if (await isValidCredentialsAsync())
        {
            if (await _usersModelDao.Modify(new UsersModel()
                {
                    UserId = CurrentUser.UserId,
                    UserName = CurrentUser.UserName,
                    FullName = CurrentUser.FullName,
                    PasswordHash = Password,
                    Email = CurrentUser.Email
                }))
            {
                ViewModelBase.SetCurrentUser(await _usersModelDao.GetSpecificObject((int)ViewModelBase.GetCurrentUser().UserId));
                AccountCredentialsEdited.Invoke();
                CloseActualWindow();
            }
            else
            {
                CustomMessageBox.ShowCustomMessageBox("Oh no! something went wrong");
            }
        }
    }
    private async Task<bool> isValidCredentialsAsync()
    {
        if (!string.IsNullOrWhiteSpace(CurrentUser.FullName)
            && !string.IsNullOrWhiteSpace(CurrentUser.UserName) 
            && !string.IsNullOrWhiteSpace(CurrentUser.Email) &&
            !string.IsNullOrWhiteSpace(Password))
        {
            // Verificar si el username no existe y tambien verificar si el username ingresado es igual al del usuario actual
            if (!await _usersModelDao.Exist(CurrentUser.UserName) ||
                this.CurrentUser.UserName == ViewModelBase.GetCurrentUser().UserName)
            {
                // verificar que la credencial sea la correcta
                if (CurrentUser.PasswordHash == this.Password)
                {
                    // retornar true
                    CurrentUser.PasswordHash = this.Password; 
                    return true;
                }
                CustomMessageBox.ShowCustomMessageBox("Password are incorrect");
                return false;
            }

            CustomMessageBox.ShowCustomMessageBox("A user with this UserName already exists");
            return false;
        }
        CustomMessageBox.ShowCustomMessageBox("Please fill in all registration fields");
        return false;
    }
    private void CloseActualWindow() {
        Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
        //cerrar la ventana si se encuentra y si la tarea se modifico con exito
        if (window != null) {
            window.Close();
        }
    }
    #endregion
}