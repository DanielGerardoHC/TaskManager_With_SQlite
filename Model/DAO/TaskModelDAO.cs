
using AdministradorDeTareas.Interfaces;
using AdministradorDeTareas.View;
using AdministradorDeTareas.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.EntityFrameworkCore;

namespace AdministradorDeTareas.Model.DAO
{
    public class TaskModelDAO : ITaskManagerServiceDAO<TaskModel>
    {
        private readonly UsersModel currentUser = ViewModelBase.GetCurrentUser();
        public async Task<List<TaskModel>> GetAll() {
            try {
                using (var db = new TaskManagerDbContext())
                {
                      var allTasks = await (from task in db.Tasks
                            join priority in db.Priorities on task.PriorityID equals priority.PriorityID
                            join status in db.TaskStatuses on task.StatusID equals status.StatusID
                            join user in db.Users on task.UserID equals user.UserId
                            where (task.UserID == currentUser.UserId)
                            select new TaskModel()
                            {
                                TaskID = task.TaskID,
                                Title = task.Title,
                                Description = task.Description,
                                DueDate = task.DueDate,
                                PriorityID = task.PriorityID,
                                StatusID = task.StatusID,
                                UserID = task.UserID,
                                TaskStatus = status,
                                Priority = priority,
                                Users = user 
                            }).ToListAsync();
                      return allTasks;
                }
            }
            catch (Exception ex) {
                CustomMessageBox.ShowCustomMessageBox($"Error: Operation could not be completed. Message: {ex.Message}");
                return null;
            }
        }
        public async Task<List<TaskModel>> GetWhere(string title) {
            try {
                using (var db = new TaskManagerDbContext())
                {
                    var allTasks = await (from task in db.Tasks
                        join priority in db.Priorities on task.PriorityID equals priority.PriorityID
                        join status in db.TaskStatuses on task.StatusID equals status.StatusID
                        join user in db.Users on task.UserID equals user.UserId
                        where (task.UserID == currentUser.UserId && task.Title == title)
                        select new TaskModel()
                        {
                            TaskID = task.TaskID,
                            Title = task.Title,
                            Description = task.Description,
                            DueDate = task.DueDate,
                            PriorityID = task.PriorityID,
                            StatusID = task.StatusID,
                            UserID = task.UserID,
                            TaskStatus = status,
                            Priority = priority,
                            Users = user 
                        }).ToListAsync();
                    return allTasks;
                }
            }
            catch (Exception ex) {
                string Message = $"Error: Operation could not be completed 'GetTasks'. Cod: {ex.Message}";
                CustomMessageBox.ShowCustomMessageBox(Message);
                return null;
            }
        }
        public async Task<bool> Delete(int id) {
            try {
                using (var db = new TaskManagerDbContext())
                {
                    var taskToDelete = db.Tasks.FirstOrDefault(task => task.TaskID == id);
                    if (taskToDelete != null)
                     db.Tasks.Remove(taskToDelete);
                    
                    return await db.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex) {
                string Message = $"Task not deleted. Error Code: {ex.Message}";
                CustomMessageBox.ShowCustomMessageBox(Message);
                return false;
            }
        }
        public async Task<bool> Add(TaskModel task) {
            try {
                using (var db = new TaskManagerDbContext())
                {
                    db.Tasks.Add(task);
                    return await db.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex) {
                CustomMessageBox.ShowCustomMessageBox($"Error: Operation could not be completed. 'AddTasks' CodServer: {ex.Message}");
                return false;
            }
        }
        public async Task<TaskModel> GetSpecificObject(int id) {
            try {
                using (var db = new TaskManagerDbContext())
                {
                    return await db.Tasks.FirstOrDefaultAsync(task => task.TaskID == id);
                }
            }
            catch (Exception ex) {
                CustomMessageBox.ShowCustomMessageBox($"Error: Operation could not be completed. Message: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> Modify(TaskModel taskModify) {
            try {
                using (var db = new TaskManagerDbContext())
                {
                    db.Entry(taskModify).State = EntityState.Modified; 
                    return await db.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex) {
                CustomMessageBox.ShowCustomMessageBox($"Error: Operation could not be completed. Cod: {ex.Message}");
                return false;
            }
        }
    }
}
