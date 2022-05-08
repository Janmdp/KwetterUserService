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

        public async Task<User> GetUserByNameAsync(string value)
        {
            var user = await context
               .Users
               .SingleOrDefaultAsync(u => u.Username == value);

            return user;
        }
    }
}
