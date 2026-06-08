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
    private readonly IUserService _userService;
    public ToDoListController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ToDoListDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Implementation for creating a to-do list item
        var userName = User.FindFirst(ClaimTypes.Name)!.Value;
        // var user = await _userService.GetUserByName(userName);
        var result = await _userService.CreateToDoList(dto, userName);
        return Ok(new { message = result });
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ToDoListDto dto)
    {
        // Implementation for editing a to-do list item
        var userName = User.FindFirst(ClaimTypes.Name)!.Value;
        var result = await _userService.EditToDoList(dto, userName, id); // we could check it through userId but it is not necessary because Name is unique and it is enough to identify the user
        if (result == "Updated successfully")
            return Ok(new { message = result });

        else
            return BadRequest(new { message = result });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _userService.Delete(id, userId); 
        // if (result == "item deleted successfully") 
        if (result == "Permission denied") return Forbid(); // 403
        else if(result == "todolist does not exists") return NotFound(new { message = result});
        return Ok( new { message = result});
    }
    [HttpGet("items")]
    public async Task<IActionResult> GetItems()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var items = await _userService.GetUserItems(userId);
        if(!items.Any())
            return NotFound(new { message = "No items found for the user." });
        return Ok(items);
    }

}