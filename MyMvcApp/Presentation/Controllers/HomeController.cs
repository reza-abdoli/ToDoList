using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {

        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Items", "ToDoList");
        }
        return RedirectToAction("Login", "User");
        
    }
}