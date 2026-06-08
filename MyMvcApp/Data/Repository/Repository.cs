using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.Dto;
using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<User> _dbSetUser;
        private readonly DbSet<ToDoList> _dbSetToDoList;
        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSetUser = _appDbContext.Set<User>();
            _dbSetToDoList = _appDbContext.Set<ToDoList>();
        }

        public async Task AddUser(User user)
        {
            await _dbSetUser.AddAsync(user);
        }

        public async ValueTask DisposeAsync()
        {
            if (_appDbContext != null)
            {
                await _appDbContext.DisposeAsync();
            }
        }

        public IQueryable<User> GetQueryableUsers()
        {
            return _dbSetUser.AsQueryable();
        }
        public IQueryable<ToDoList> GetQueryableToDoLists()
        {
            return _dbSetToDoList.AsQueryable();
        }

        // public async Task<User?> GetUser(string name)
        // {
        //     return await _dbSetUser.FirstOrDefaultAsync(u => u.Name == name);
        // }

        public async Task SaveChanges()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task AddToDoList(ToDoList toDoList)
        {
            await _dbSetToDoList.AddAsync(toDoList);
        }

        public void DeleteToDoList(ToDoList toDoList)
        {
            _dbSetToDoList.Update(toDoList);
        }

        public void UpdateToDoList(ToDoList toDoList)
        {
            _dbSetToDoList.Update(toDoList);
        }
    }
}