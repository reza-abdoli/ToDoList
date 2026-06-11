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


[Authorize(Roles = "Admin")] // Only allow access to users with the "Admin" role
public class AdminController : Controller
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    public async Task<IActionResult> Items()
    {
        var items = await _adminService.GetAllItems();
        #region when testing with api-controller which works with JSON not View :)
                // if (!items.Any())
        //     return NotFound(new { message = "No items found." });     
            
        #endregion
        if (!items.Any())
            TempData["Error"] = "Nobody have ToDoList!";
        return View(items);
    }

}