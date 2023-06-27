using System.Diagnostics;
using AutoSale.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AutoSaleMVC.Controllers;

public class HomeController : Controller
{

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "CarAd");
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