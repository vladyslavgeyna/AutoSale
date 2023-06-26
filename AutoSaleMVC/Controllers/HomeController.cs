using System.Diagnostics;
using AutoSale.Domain.ViewModels;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoSaleMVC.Controllers;

public class HomeController : Controller
{
    private readonly ICarAdService _carAdService;
    private readonly IEmailService _emailService;

    public HomeController(ICarAdService carAdService, IEmailService emailService)
    {
        _carAdService = carAdService;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [Route("/Home/Error/{statusCode}")]
    public IActionResult Error(int statusCode)
    {
        if (statusCode == 404)
        {
            return View("NotFound");
        }
        else if (statusCode == 403)
        {
            return View("AccessDenied");
        }
        return View();
    }
    
    public IActionResult AccessDenied()
    {
        return View();
    }
    
    public new IActionResult NotFound()
    {
        return View();
    }
}