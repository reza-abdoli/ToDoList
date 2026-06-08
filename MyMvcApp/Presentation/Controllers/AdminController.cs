using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")] // Only allow access to users with the "Admin" role
public class AdminController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetAllUsersItems()
    {
        var admin = await _userService.GetUserById(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value));
        if (admin == null || admin.Role != "Admin")
        {
            return Forbid(); // 403 Forbidden if the user is not an admin
        }
        var items = await _userService.GetAllUsersItems();
        if (items == null || items.Count == 0)
            return NotFound(new { message = "No items found for this user." });

        return Ok(items);
    }

}