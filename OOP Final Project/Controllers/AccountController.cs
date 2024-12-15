using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OOP_Final_Project.Data;
using OOP_Final_Project.Models;
using OOP_Final_Project.Models.Identity;
using OOP_Final_Project.Models.ViewModels;

namespace OOP_Final_Project.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;
    private readonly ApplicationDbContext _context;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _logger = logger;
    }

    // GET: Register
    [HttpGet]
    public IActionResult Register()
    {
        // Load available roles (AccountTypes) into the view
        ViewData["AccountTypes"] = _context.AccountTypes;
        return View();
    }

    // POST: Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Create new user with provided details
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                AccountTypeId = model.AccountTypeId, // Assign selected role (AccountType)
                AccountType = _context.AccountTypes.FirstOrDefault(at => at.Id == model.AccountTypeId) ?? new AccountType { Name = "Default" } // Set AccountType
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Automatically sign in the user after registration
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // Reload roles if registration fails
        ViewData["AccountTypes"] = _context.AccountTypes;
        return View(model);
    }


    // GET: Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Log the attempt to login
            _logger.LogInformation("Attempting login for user: {UserName}", model.UserName);

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Redirect based on role
                    var accountTypeId = user.AccountTypeId;

                    if (accountTypeId == 1) // Manager
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (accountTypeId == 2) // Doctor
                    {
                        return RedirectToAction("Index", "Doctor");
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "User not found.");
        }

        return View(model);
    }

    // POST: Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


}
