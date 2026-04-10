using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
    public static class UserStore
    {
        public static List<User> Users = new List<User>
        {
            new User { Id = 1, Name = "Dhyan", Email = "dhyan@email.com" },
            new User { Id = 2, Name = "Test User", Email = "test@email.com" }
        };
    }
}