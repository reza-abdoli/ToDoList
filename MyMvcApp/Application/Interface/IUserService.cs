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
        Task<bool> UserExists(int id);
    }
}