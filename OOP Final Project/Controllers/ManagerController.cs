using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OOP_Final_Project.Controllers;

[Authorize]
public class ManagerController : Controller
{
    [Authorize(Roles = "Manager")]
    public IActionResult Index()
    {
        return View();
    }

}
