using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Data.Dto;
using Data.Entity;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Application.Service
{
    public class UserService : IUserService
    {
        #region ctor
        private readonly IRepository _repository;
        public UserService(IRepository repository)
        {
            _repository = repository;
        }
        public async ValueTask DisposeAsync()
        {
            if (_repository != null)
            {
                await _repository.DisposeAsync();
            }
        }
        #endregion

        #region TodoLists
        public async Task<string> CreateToDoList(ToDoListDto toDoListDto, string userName)
        {
            // User user = await _repository.GetQueryableUsers().SingleOrDefaultAsync(u => u.Id == userId); //
            var user = await this.GetUserByName(userName);
            if (user == null) // ← حتماً چک کن
                return "User not found.";
            var todoList = new ToDoList
            {
                Title = toDoListDto.Title,
                Content = toDoListDto.Content,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                // must set up but idk how!!
                UserId = user.Id,
                User = user
            };
            await _repository.AddToDoList(todoList);
            await _repository.SaveChanges();
            return "created successfully";
        }

        public async Task<string> Delete(int id, int userId)
        {
            var tdl = await _repository.GetQueryableToDoLists().SingleOrDefaultAsync(t => t.Id == id); //
            if (tdl == null || tdl.IsDeleted)
                return "todolist does not exists";

            if (tdl.UserId != userId)
                return "Permission denied";

            tdl.UpdatedAt = DateTime.UtcNow;
            tdl.IsDeleted = true;
            _repository.DeleteToDoList(tdl);
            await _repository.SaveChanges();
            return "item deleted successfully";
        }


        public async Task<string> EditToDoList(ToDoListDto toDoListDto, string userName, int id)
        {
            var user = await this.GetUserByName(userName);
            if (user == null)
                return "User not found.";
            // should I check if user is null? I think it is not necessary because if user is null, it means the token is invalid, and the request won't even reach this point because of the [Authorize] attribute in controller
            #region answer
            /*
            . Authorize تضمین نمی‌کند که user در دیتابیس وجود دارد

این کامنتت کاملاً درست نیست:

// if user is null, token is invalid

فرض کن:

کاربر Login کرده و JWT گرفته.
بعداً رکورد کاربر از دیتابیس حذف شده.
هنوز JWT معتبر است.

در این حالت:

[Authorize]

موفق می‌شود، اما:

var user = await GetUserByName(userName);

ممکن است null برگرداند.
            */
            #endregion
            var todoList = await _repository.GetQueryableToDoLists().SingleOrDefaultAsync(t => t.Id == id); //
            if (todoList == null)
                return "todolist not found.";

            if (todoList.UserId != user.Id)
                return "Access denied.";

            todoList.Title = toDoListDto.Title;
            todoList.Content = toDoListDto.Content;
            todoList.UpdatedAt = DateTime.UtcNow;
            _repository.UpdateToDoList(todoList);
            await _repository.SaveChanges();
            return "Updated successfully";
        }

        public async Task<List<ToDoListDto>> GetUserItems(int userId)
        {
            var items = await _repository.GetQueryableToDoLists().Where(item => item.UserId == userId && !item.IsDeleted)
                .Select(item => new ToDoListDto
                {
                    Title = item.Title,
                    Content = item.Content
                }).ToListAsync();
            return items;
        }
        #endregion

        #region signup-login
        public async Task<User?> Login(SignupLoginDto signupLoginDto)
        {
            var user = await _repository.GetQueryableUsers()
                .SingleOrDefaultAsync(u => u.Name == signupLoginDto.Name);

            if (user == null || !BCrypt.Net.BCrypt.Verify(signupLoginDto.Password, user.Password))
                return null;

            return user;
        }

        public async Task<string> Signup(SignupLoginDto signupLoginDto)
        {
            if (await _repository.GetQueryableUsers().AnyAsync(u => u.Name == signupLoginDto.Name)) //
            {
                return "User already exists.";
            }
            else
            {
                var user = new User
                {
                    Name = signupLoginDto.Name,
                    Password = BCrypt.Net.BCrypt.HashPassword(signupLoginDto.Password),
                    CreatedAt = DateTime.Now,
                    Role = "User" // default role is "User"
                };
                await _repository.AddUser(user);
                await _repository.SaveChanges();
                return "User created successfully.";
            }
        }

        #endregion

        public async Task<User?> GetUserByName(string name)
        {
            return await _repository.GetQueryableUsers().SingleOrDefaultAsync(u => u.Name == name);
        }

        public async Task<List<ToDoListDto>> GetAllUsersItems()
        {
            var items = await _repository.GetQueryableToDoLists().Where(item => !item.IsDeleted)
                .Select(item => new ToDoListDto
                {
                    Title = item.Title,
                    Content = item.Content
                }).ToListAsync();
            return items;
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _repository.GetQueryableUsers().SingleOrDefaultAsync(u => u.Id == userId);
        }
    }
}