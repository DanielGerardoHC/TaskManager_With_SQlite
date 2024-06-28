
using AdministradorDeTareas.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdministradorDeTareas.Model
{
    public class UsersModel
    { 
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName {  get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public virtual ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
