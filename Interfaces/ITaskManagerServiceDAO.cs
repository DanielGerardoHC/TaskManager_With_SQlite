using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdministradorDeTareas.Interfaces
{
    interface ITaskManagerServiceDAO<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetWhere(string Title);
        Task<bool> Delete(int idobj);
        Task<bool> Add(T obj);
        Task<T> GetSpecificObject(int id);
        Task<bool> Modify(T obj);

    }
}
