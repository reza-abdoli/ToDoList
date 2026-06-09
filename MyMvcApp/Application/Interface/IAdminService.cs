using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Dto;
using Data.Entity;

namespace Application.Interface
{
    public interface IAdminService
    {
        public Task<List<ToDoListDto>> GetAllItems();
    }
}