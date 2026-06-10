using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Enum;
using Data.Dto;

namespace Application.Interface;

public interface IToDoListService
{
        Task<ServiceResult> CreateToDoList(ToDoListDto toDoListDto, int userId);
        Task<ServiceResult> EditToDoList(ToDoListDto toDoListDto, int userId, int id);
        Task<ServiceResult> Delete(int id, int userId);
        Task<List<ToDoListDto>> GetUserItems(int userId);
}