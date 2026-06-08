using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<ToDoList> ToDoLists {  get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }
        
    }
}