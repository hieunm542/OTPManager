using System;
using System.Collections.Generic;
using System.Linq;
using JwtAuthDemo.Infrastructure;
using JwtAuthDemo.Models;
using Microsoft.Extensions.Logging;

namespace JwtAuthDemo.Services
{
    public interface IUserService
    {
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
        Guid GetUserID(string userName);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private DBContext _dbContext;
        // inject your database here for user validation
        public UserService(ILogger<UserService> logger, DBContext dBContext)
        {
            _logger = logger;
            _dbContext = dBContext;
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            User user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
                return false;
            else
            {
                if (user.Password != password)
                    return false;
            }

            return true;
        }
        public Guid GetUserID(string userName)
        {
            User user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return Guid.Empty;
            }
            return user.ID;
        }
        public string GetUserRole(string userName)
        {
            User user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return string.Empty;
            }
            if (user.Role == "admin")
                return UserRoles.Admin;
            else
                return UserRoles.BasicUser;
        }
    }

    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }
}
