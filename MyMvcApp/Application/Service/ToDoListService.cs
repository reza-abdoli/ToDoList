using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Data.Dto;
using Data.Entity;
using Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Application.Service
{
    public class ToDoListService : IToDoListService
    {
        #region ctor
        private readonly IRepository<ToDoList> _repository;
        public ToDoListService(IRepository<ToDoList> repository)
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
        
        #region TodoLists
        public async Task<string> CreateToDoList(ToDoListDto toDoListDto, int userId)
        {
            var todoList = new ToDoList
            {
                Title = toDoListDto.Title,
                Content = toDoListDto.Content,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = userId
            };
            _repository.AddEntity(todoList);
            await _repository.SaveChanges();
            return "created successfully";
        }

        public async Task<string> Delete(int id, int userId)
        {
            var tdl = await _repository.GetEntityById(id); //
            if (tdl == null || tdl.IsDeleted)
                return "todolist does not exists";

            if (tdl.UserId != userId)
                return "Permission denied";

            tdl.UpdatedAt = DateTime.UtcNow;
            tdl.IsDeleted = true;
            _repository.DeleteEntity(tdl);
            await _repository.SaveChanges();
            return "item deleted successfully";
        }


        public async Task<string> EditToDoList(ToDoListDto toDoListDto, int userId, int id)
        {

            // should I check if user is null? I think it is not necessary because if user is null, it means the token is invalid, and the request won't even reach this point because of the [Authorize] attribute in controller
            #region answer
            /*
            . Authorize تضمین نمی‌کند که user در دیتابیس وجود دارد

                این کامنتت کاملاً درست نیست:

                // if user is null, token is invalid

                فرض کن:

                کاربر Login کرده و JWT گرفته.
                بعداً رکورد کاربر از دیتابیس حذف شده.
                هنوز JWT معتبر است.

                در این حالت:

                [Authorize]

                موفق می‌شود، اما:

                var user = await GetUserByName(userName);

                ممکن است null برگرداند.

edit: if user has been deleted its toDoLists has been deleted as well because Cascade Delete
                                 ef core does that by itself so that no problem will occur at
                                 delete/edit method(returns not found because todolist not
                                 exists.)
                                 and at creation method at line 
                                 UserId = userId
                                 will have error => FK constraint failed - UserId=5 does not exist in User table 
                                 چون UserId یه Foreign Key هست و دیتابیس اجازه نمیده رکوردی با FK نامعتبر اضافه بشه. ✅

                                 


            */
            #endregion
            
            var todoList = await _repository.GetEntityById(id);//
            if (todoList == null)
                return "todolist not found.";

            if (todoList.UserId != userId)
                return "Access denied.";

            todoList.Title = toDoListDto.Title;
            todoList.Content = toDoListDto.Content;
            todoList.UpdatedAt = DateTime.UtcNow;
            _repository.UpdateEntity(todoList);
            await _repository.SaveChanges();
            return "Updated successfully";
        }

        public async Task<List<ToDoListDto>> GetUserItems(int userId)
        {
            var items = await _repository.GetQueryable().Where(item => item.UserId == userId && !item.IsDeleted)
                .Select(item => new ToDoListDto
                {
                    Title = item.Title,
                    Content = item.Content
                }).ToListAsync();
            return items;
        }
        #endregion

    }
}