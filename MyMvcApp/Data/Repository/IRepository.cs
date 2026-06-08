using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Dto;
using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public interface IRepository : IAsyncDisposable
    {
        Task AddUser(User user);
        // Task<User?> GetUser(string name);
        Task AddToDoList(ToDoList toDoList);

        IQueryable<User> GetQueryableUsers();
        IQueryable<ToDoList> GetQueryableToDoLists();

        void DeleteToDoList(ToDoList toDoList);  
        void UpdateToDoList(ToDoList toDoList);  
        public Task SaveChanges();
    }
}