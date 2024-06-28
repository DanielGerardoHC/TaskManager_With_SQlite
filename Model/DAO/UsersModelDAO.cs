
using AdministradorDeTareas.Interfaces;
using AdministradorDeTareas.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AdministradorDeTareas.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AdministradorDeTareas.Model.DAO
{
    public class UsersModelDAO : ITaskManagerServiceDAO<UsersModel> 
    {
        private readonly UsersModel currentUser = ViewModelBase.GetCurrentUser();
        public async Task<bool> Delete(int id) {
            throw new NotImplementedException();
        }
        public async Task<List<UsersModel>> GetAll() {
            throw new NotImplementedException();
        }
        public async Task<UsersModel> GetSpecificObject(int id) {
            try {
                using (var db = new TaskManagerDbContext())
                {
                    return await db.Users.FirstOrDefaultAsync(user => user.UserId == id);
                }
            }
            catch (Exception ex) {
                string errorMessage = $"Error: Operation could not be completed. Message: {ex.Message}";
                CustomMessageBox customMessageBox = new CustomMessageBox(errorMessage);
                customMessageBox.ShowDialog();
                return null;
            }
        }
        public async Task<List<UsersModel>> GetWhere(string userName)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Modify(UsersModel user)
        {
            try
            {
                using (var db = new TaskManagerDbContext())
                {
                    var userModified = db.Users.FirstOrDefault(u => u.UserId == currentUser.UserId && u.PasswordHash == user.PasswordHash);
                    if (userModified != null)
                    {
                        userModified.UserName = user.UserName;
                        userModified.FullName = user.FullName;
                        userModified.Email = user.Email;
                    }
                    else
                    {
                        CustomMessageBox.ShowCustomMessageBox("The password was incorrect");
                    }
                    return db.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                string message = $"Error: Operation could not be completed. Code: {ex.Message}";
                CustomMessageBox customMessageBox = new CustomMessageBox(message);
                customMessageBox.ShowDialog();
                return false;
            }
        }
        public async Task<bool> Add(UsersModel user) {
            try {
                using (var db = new TaskManagerDbContext())
                {
                    await db.Users.AddAsync(user);
                    return await db.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex) {
                CustomMessageBox customMessageBox = new CustomMessageBox($"Error: Operation could not be completed. Cod: {ex.Message}");
                customMessageBox.ShowDialog();
                return false;
            }
        }
        public async Task<UsersModel?> Auth(UsersModel user) {
            try {
                using (var db = new TaskManagerDbContext())
                {
                    var currentUser = await db.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.PasswordHash == user.PasswordHash);
                    return currentUser;
                }
            }
            catch (Exception ex) {
                CustomMessageBox customMessageBox = new CustomMessageBox($"Error: Operation could not be completed. Cod: {ex.Message}");
                customMessageBox.ShowDialog();
                return null;
            }
        }
        public async Task<bool> ChangePass(string oldPass, string newPass)
        {
            using (var db = new TaskManagerDbContext())
            {
                UsersModel? validUser = await db.Users.FirstOrDefaultAsync(u => u.UserId == currentUser.UserId && u.PasswordHash == oldPass);
                if (validUser != null)
                {
                    validUser.PasswordHash = newPass;
                }
                return db.SaveChanges() > 0;
            }
       }

        public async Task<bool> Exist(String UserName)
        {
            using (var db = new TaskManagerDbContext())
            {
                return await db.Users.AnyAsync(user => user.UserName == UserName);
            }
        }
    }
}
