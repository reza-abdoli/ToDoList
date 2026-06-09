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
        private readonly IRepository<User> _repository;
        public UserService(IRepository<User> repository)
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

        #region signup-login
        public async Task<User?> Login(SignupLoginDto signupLoginDto)
        {
            var user = await _repository.GetQueryable().SingleOrDefaultAsync(u => u.Name == signupLoginDto.Name);

            if (user == null || !BCrypt.Net.BCrypt.Verify(signupLoginDto.Password, user.Password))
                return null;

            return user;
        }

        public async Task<string> Signup(SignupLoginDto signupLoginDto)
        {
            if (await _repository.GetQueryable().AnyAsync(u => u.Name == signupLoginDto.Name)) //
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
                _repository.AddEntity(user);
                await _repository.SaveChanges();
                return "User created successfully.";
            }
        }

        public async Task<bool> UserExists(int id)
        {
            return await _repository.GetEntityById(id) != null;
        }

        #endregion


        // public async Task<List<ToDoListDto>> GetAllUsersItems()
        // {
        //     var items = await _repository.GetQueryableToDoLists().Where(item => !item.IsDeleted)
        //         .Select(item => new ToDoListDto
        //         {
        //             Title = item.Title,
        //             Content = item.Content
        //         }).ToListAsync();
        //     return items;
        // }

    }
}