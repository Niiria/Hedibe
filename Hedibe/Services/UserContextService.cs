using Hedibe.Models.Account;
using System;

namespace Hedibe.Services
{
    public interface IUserContextService
    {
        void LoginUser(LoggedUser model);
        void LogoutUser();
        string GetLoggedUserRole();
        bool CheckLoggedUser();
        string GetUsername();
        string GetRole();
    }

    public class UserContextService : IUserContextService
    {
        private static LoggedUser LoggedUser = new();

        public string GetUsername()
        {
            if (LoggedUser.Username is null) return null;

            return LoggedUser.Username;
        }
        public string GetRole()
        {
            if (LoggedUser.Role.Name is null) return null;

            return LoggedUser.Role.Name;
        }
        public void LoginUser(LoggedUser model)
        {
            LoggedUser = model;
        }

        public void LogoutUser()
        {
            LoggedUser = new();
        }

        public bool CheckLoggedUser()
        {
            if (LoggedUser.Username is null) return false;

            return true;
        }

        public string GetLoggedUserRole()
        {
            if (LoggedUser.Role is not null)
                return LoggedUser.Role.Name;
            return "";
        }
    }
}
