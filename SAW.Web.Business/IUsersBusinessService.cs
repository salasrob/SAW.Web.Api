﻿using SAW.Web.Entities.Domain;
using SAW.Web.Entities.Requests;

namespace SAW.Web.Business
{
    public interface IUsersBusinessService
    {
        public Task<bool> Authenticate(string username, string password);
        Task<User> TwoFactorLoginAsync(string token);
        public Task LogOutAsync();
        public Task<int> CreateUser(UserAddRequest user);
        public Task<User> GetUserByUserName(string userName);
        public Task<User> GetUserById(int userId);
        public Task<List<User>> GetUsers();
    }
}
