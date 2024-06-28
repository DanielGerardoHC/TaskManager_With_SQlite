
using AdministradorDeTareas.View;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdministradorDeTareas.Model
{
    public class TaskModel
    {
         public int? TaskID { get; set; }
         public  string? Title {  get; set; }
         public string? Description { get; set; }
         public DateTime? DueDate { get; set; }
         public int? StatusID { get; set; }
         public int? UserID { get; set; }
         public int? PriorityID { get; set; }
         public virtual PriorityModel? Priority { get; set; }
         public virtual TaskStatusModel? TaskStatus { get; set; }
         public virtual UsersModel? Users { get; set; }
        
    }
}
