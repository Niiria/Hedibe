using Hedibe.Models.Account;
using System;

namespace Hedibe.Services
{
    public interface IUserContextService
    {
        LoggedUser GetLoggedUser();
        void LoginUser(LoggedUser model);
        void LogoutUser();
        string GetLoggedUserRole();
    }

    public class UserContextService : IUserContextService
    {
        private static LoggedUser LoggedUser = new();

        public LoggedUser GetLoggedUser()
        {
            if (LoggedUser.Username is null) return null;

            return LoggedUser;
        }
        public void LoginUser(LoggedUser model)
        {
            LoggedUser = model;
        }

        public void LogoutUser()
        {
            LoggedUser = new();
        }

        public string GetLoggedUserRole()
        {
            if (LoggedUser.Role is not null)
                return LoggedUser.Role.Name;
            return "";
        }
    }
}
