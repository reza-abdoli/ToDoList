using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Data.Dto;
using Data.Entity;
using Data.Migrations;
using Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Application.Service;

public class AdminService : IAdminService
{
    private readonly IRepository<ToDoList> _toDoListRepository;
    private readonly IRepository<User> _userRepository;
    public AdminService(IRepository<ToDoList> toDoListRepository
    , IRepository<User> userRepository)
    {
        _userRepository = userRepository;
        _toDoListRepository = toDoListRepository;
    }

    public async Task<List<ToDoListDto>> GetAllItems()
    {
        return await _toDoListRepository.GetQueryable()
        .Where(todolist => !todolist.IsDeleted)
        .Select(todolist => new ToDoListDto
        {
            Title = todolist.Title,
            Content = todolist.Content
        }) 
        .ToListAsync();
    }
}