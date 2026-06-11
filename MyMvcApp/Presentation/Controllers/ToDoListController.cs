using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Enum;
using Application.Interface;
using Data.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;


[Authorize]
public class ToDoListController : Controller
{
    private readonly IToDoListService _toDoListService;
    public ToDoListController(IToDoListService toDoListService)
    {
        _toDoListService = toDoListService;
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(ToDoListDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _toDoListService.CreateToDoList(dto, userId);
        return RedirectToAction("Items");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var item = await _toDoListService.GetById(id, userId);
        if (item == null)
        {
            TempData["Error"] = "Item not found";
            return RedirectToAction("Items"); // برگرده، نه View(null)
        }
        return View(item);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(ToDoListEditDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _toDoListService.EditToDoList(dto, userId);
        #region why to use TempData[]
        // return result switch
        // {
        //     ServiceResult.Success => RedirectToAction("Items"),
        //     ServiceResult.NotFound => View(),
        //     ServiceResult.PermissionDenied => Forbid(),
        //     _ => BadRequest()
        // };
        #endregion

        switch (result)
        {
            case ServiceResult.Success:
                TempData["Success"] = "Edited Successfully";
                break;
            case ServiceResult.NotFound:
                TempData["Error"] = "ToDoList does not exist";
                break;
            case ServiceResult.PermissionDenied:
                TempData["Error"] = "You don't have permission to edit this item";
                break;
            default:
                TempData["Error"] = "Editing was not successful";
                break;
        }
        return RedirectToAction("Items");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _toDoListService.Delete(id, userId);

        #region why to use TempData[]
        // return result switch
        // {
        //     ServiceResult.Success => RedirectToAction("Items"),
        //     ServiceResult.NotFound => NotFound(new { message = "todolist does not exist" }),
        //     ServiceResult.PermissionDenied => Forbid(),
        //     _ => BadRequest()
        // };

        #endregion
        switch (result)
        {
            case ServiceResult.Success:
                TempData["Success"] = "Deleted Successfully";
                break;
            case ServiceResult.NotFound:
                TempData["Error"] = "ToDoList does not exist";
                break;
            case ServiceResult.PermissionDenied:
                TempData["Error"] = "You don't have permission to delete this item";
                break;
            default:
                TempData["Error"] = "Deleting was not successful";
                break;
        }
        return RedirectToAction("Items");
    }


    [HttpGet]
    public async Task<IActionResult> Items()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var items = await _toDoListService.GetUserItems(userId);
        return View(items);
    }

}