using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entity;

namespace Application.Interface
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}