namespace AdministradorDeTareas.Model;

public class ChangePasswordModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}