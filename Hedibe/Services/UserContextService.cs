using Hedibe.Models.Account;
using System;

namespace Hedibe.Services
{
    public interface IUserContextService
    {
        void LoginUser(LoggedUserDto model);
        void LogoutUser();
        bool IsUserLoggedIn();
        string GetUsername();
        string GetRole();
        public int? GetUserId();
        bool GrantAccessToRoles(params string[] access_roles);
    }

    public class UserContextService : IUserContextService
    {
        private static LoggedUserDto LoggedUser = new();

        public int? GetUserId()
        {
            if (LoggedUser.Id is null) return null;

            return LoggedUser.Id;
        }
        public string GetUsername()
        {
            if (LoggedUser.Username is null) return null;

            return LoggedUser.Username;
        }
        public string GetRole()
        {
            if (LoggedUser.Role is null) return null;

            return LoggedUser.Role.Name;
        }
        public void LoginUser(LoggedUserDto model)
        {
            LoggedUser = model;
        }

        public void LogoutUser()
        {
            LoggedUser = new();
        }

        public bool IsUserLoggedIn()
        {
            if (LoggedUser.Username is null) return false;

            return true;
        }

        public bool GrantAccessToRoles(string[] access_roles)
        {
            if (IsUserLoggedIn())
            {
                foreach (var access_role in access_roles)
                {
                    if (access_role == LoggedUser.Role.Name)
                        return true;
                }
            }
            return false;
        }

    }
}
