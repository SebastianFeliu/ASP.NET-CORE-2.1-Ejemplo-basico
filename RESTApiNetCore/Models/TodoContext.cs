using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTApiNetCore.Models
{
    public class TodoContext : DbContext
    {
      public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
         {
         }

        public virtual DbSet<TodoItem> TodoItem { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}

