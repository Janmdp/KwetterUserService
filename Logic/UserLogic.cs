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

        public void CreateUser(User user)
        {
           repo.CreateUser(user);

        }

        public bool CheckExistingUser(User user)
        {
            return repo.CheckExistingUser(user).Result;
        }

        public void DeleteUser(int id)
        {
            repo.DeleteUser(id);
        }
    }
}
