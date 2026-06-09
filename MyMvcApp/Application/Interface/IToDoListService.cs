using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Dto;

namespace Application.Interface;

public interface IToDoListService
{
        Task<string> CreateToDoList(ToDoListDto toDoListDto, int userId);
        Task<string> EditToDoList(ToDoListDto toDoListDto, int userId, int id);
        Task<string> Delete(int id, int userId);
        Task<List<ToDoListDto>> GetUserItems(int userId);
}