using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class UserRepository
    {
        private readonly UserDbContext context;

        public UserRepository(UserDbContext ctx)
        {
            context = ctx;
        }

        public async Task<User> GetUserByIdAsync(int value)
        {
            var user = await context
               .Users
               .SingleOrDefaultAsync(u => u.Id == value);

            return user;
        }

        public async Task<bool> CheckExistingUser(User user)
        {
            var dbUser = await context
              .Users
              .SingleOrDefaultAsync(u => u.Id == user.Id);

            if (dbUser != null)
                return true;

            return false;
        }

        public void DeleteUser(int id)
        {
            var dbUser = context.Users.FirstOrDefault(u => u.Id == id);
            if(dbUser != null)
            {
                context.Users.Remove(dbUser);
                context.SaveChanges();
            }
        }

        public async void CreateUser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
