
using AdministradorDeTareas.View;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdministradorDeTareas.Model 
{
    public class PriorityModel
    {
        public int?  PriorityID { get; set; }
        public string? PriorityStatus { get; set; }
        public virtual ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
