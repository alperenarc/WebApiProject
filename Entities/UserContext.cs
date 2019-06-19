using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
           : base(options)
        {
        }
        
        public DbSet<User> User { get; set; }
    }
}
