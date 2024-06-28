using System.Windows.Input;
using AdministradorDeTareas.Model;
using AdministradorDeTareas.Model.DAO;
using System.Windows;

namespace AdministradorDeTareas.ViewModel;

public class ViewModelChangePassword : ViewModelBase
{
    public ViewModelChangePassword() {
        ChangePasswordCommand = new ViewModelCommand(ExecuteChangePasswordCommand);
        CancelChangePassword = new ViewModelCommand(ExecuteCancelChangePassword);
    }
    #region Atributos
    private readonly UsersModelDAO _usersModelDao = new UsersModelDAO();
    private string? _OldPassword;
    private string? _NewPassword;
    public string OldPassword
    {
        get { return _OldPassword; }
        set
        {
            _OldPassword = value;
            OnPropertyChanged(nameof(OldPassword));
        }
    }
    public string NewPassword
    {
        get { return _NewPassword; }
        set
        {
            _NewPassword = value;
            OnPropertyChanged(nameof(NewPassword));
        }
    }
    public ICommand ChangePasswordCommand { get; }
    public ICommand CancelChangePassword { get; }
    #endregion
    #region Metodos
    private void ExecuteChangePasswordCommand(object obj) {
        ChangePassword();
    }
    private void ExecuteCancelChangePassword(object obj) {
         CloseWindow();
    }
    private async void ChangePassword() {
        if (OldPassword != "" && NewPassword.Length >= 4)
        {
            if (await _usersModelDao.ChangePass(OldPassword, NewPassword))
            {
                ViewModelBase.SetCurrentUser(await _usersModelDao.GetSpecificObject((int)ViewModelBase.GetCurrentUser().UserId));
                CloseWindow();
            }
        }
    }
    public void CloseWindow() {
        Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);
        //cerrar la ventana si se encuentra y si la tarea se modifico con exito
        if (window != null) {
            window.Close();
        } 
    }
    #endregion
}