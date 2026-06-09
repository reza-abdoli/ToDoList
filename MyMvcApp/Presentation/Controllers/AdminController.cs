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
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetAllUsersItems()
    {
        var items = await _adminService.GetAllItems();
        if (!items.Any())
            return NotFound(new { message = "No items found." });
        return Ok(items);
    }

}