using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Dto;
using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    // for *** CRUD *** operation
    public interface IRepository<IEntity> : IAsyncDisposable where IEntity : BaseEntity
    {
        // Task AddUser(User user);
        // Task AddToDoList(ToDoList toDoList);
        void AddEntity(IEntity entity);
        // if we had more tables(say x) we would write x Add method for them here...
        // but with generic types we could handle it once

        // IQueryable<User> GetQueryableUsers();
        // IQueryable<ToDoList> GetQueryableToDoLists();
        IQueryable<IEntity> GetQueryable(); 
        void DeleteEntity(IEntity entity);  
        void UpdateEntity(IEntity entity);  
        Task<IEntity?> GetEntityById(int id);

        public Task SaveChanges();
    }
}