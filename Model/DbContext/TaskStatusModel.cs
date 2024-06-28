using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministradorDeTareas.Model
{
    public class TaskStatusModel
    {
        public int? StatusID { get; set; }
        public string? StatusName { get; set; }
        public virtual ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
