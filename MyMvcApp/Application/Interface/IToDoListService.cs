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
        Task<ServiceResult> EditToDoList(ToDoListEditDto toDoListEditDto, int userId);
        Task<ServiceResult> Delete(int id, int userId);
        Task<List<ToDoListEditDto>> GetUserItems(int userId);
        Task<ToDoListEditDto?> GetById(int id, int userId);
}