using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Dto;
using Data.Entity;

namespace Application.Interface
{
    public interface IUserService : IAsyncDisposable
    {
        Task<string> Signup(SignupLoginDto signupLoginDto);
        Task<User?> Login(SignupLoginDto signupLoginDto);
        Task<string> CreateToDoList(ToDoListDto toDoListDto, string userName);
        Task<string> EditToDoList(ToDoListDto toDoListDto, string userName, int id);
        Task<string> Delete(int id, int userId);
        Task<User?> GetUserByName(string name);
        Task<List<ToDoListDto>> GetUserItems(int userId);

        Task<List<ToDoListDto>> GetAllUsersItems(); // for admin to get all items of a user by user id
        Task<User?> GetUserById(int userId); // to check if the user is admin or not
    }
}