using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Enum;
using Data.Dto;
using Data.Entity;

namespace Application.Interface
{
    public interface IUserService : IAsyncDisposable
    {
        Task<ServiceResult> Signup(SignupLoginDto signupLoginDto);
        Task<User?> Login(SignupLoginDto signupLoginDto);
        Task<bool> UserExists(int id);
    }
}