using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interface;
using Data.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ToDoListController : ControllerBase
{
    private readonly IToDoListService _toDoListService;
    public ToDoListController(IToDoListService toDoListService)
    {
        _toDoListService = toDoListService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ToDoListDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _toDoListService.CreateToDoList(dto, userId);
        return Ok(new { message = result });
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ToDoListDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _toDoListService.EditToDoList(dto, userId, id); 
        if (result == "Updated successfully")
            return Ok(new { message = result });
        else
            return BadRequest(new { message = result });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _toDoListService.Delete(id, userId); 
        if (result == "Permission denied") return Forbid(); // 403
        else if(result == "todolist does not exists") return NotFound(new { message = result});
        return Ok( new { message = result});
    }
    [HttpGet("items")]
    public async Task<IActionResult> GetItems()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var items = await _toDoListService.GetUserItems(userId);
        if(!items.Any())
            return NotFound(new { message = "No items found for the user." });
        return Ok(items);
    }

}