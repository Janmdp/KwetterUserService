using Data;
using Models;
using System;

namespace Logic
{
    public class UserLogic
    {
        private readonly UserRepository repo;

        public UserLogic(UserDbContext context)
        {
            repo = new UserRepository(context);
        }

        public User GetUser(string value)
        {
            User user = repo.GetUserByNameAsync(value).Result;
            return user;
        }
    }
}
