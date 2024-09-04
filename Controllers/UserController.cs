using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Petdora.Models;
using Microsoft.AspNetCore.Identity;

namespace Petdora.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private DataContext _context;

    public UserController(ILogger<UserController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public ViewResult Index() => View();

    [HttpPost("users/register")]
    public IActionResult RegisterUser(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher<User> Hasher = new();
        newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
        _context.Add(newUser);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("UserId", newUser.UserId);

        return RedirectToAction("Dashboard", "Post");
    }

    [HttpPost("users/login")]
    public IActionResult LoginUser(LogUser loginAttempt)
    {
        if(!ModelState.IsValid)
        {
            return View("Index");
        }
        User? DbUser = _context.Users.FirstOrDefault(u => u.Email == loginAttempt.LogEmail);
        if (DbUser == null)
        {
            ModelState.AddModelError("LogPassword", "Invalid credentials.");
            return View("Index");
        }
        PasswordHasher<LogUser> Hasher = new();
        PasswordVerificationResult pwCompareResult = Hasher.VerifyHashedPassword(loginAttempt, DbUser.Password, loginAttempt.LogPassword);
        if (pwCompareResult == 0)
        {
            ModelState.AddModelError("LogPassword", "Invalid credentials");
            return View("Index");
        }
        HttpContext.Session.SetInt32("UserId", DbUser.UserId);

        return RedirectToAction("Dashboard", "Post");
    }

    [HttpGet("users/{userId}")]
    public IActionResult ViewUser (int userId)
    {
        User? ThisUser = _context.Users.FirstOrDefault(p => p.UserId == userId);
        if(ThisUser == null)
        {
            return RedirectToAction("Dashboard", "Post");
        }
        return View(ThisUser);
    }

    [HttpPost("users/logout")]
    public RedirectToActionResult Logout()
    {
        HttpContext.Session.Remove("UserId");
        return RedirectToAction("Index");
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}