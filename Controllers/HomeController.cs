using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pokedraft.Models;

namespace Pokedraft.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Draft()
    {
        return View();
    }

    public IActionResult Itens()
    {
        return View();
    }

    public IActionResult Batalha()
    {
        return View();
    }

    public IActionResult Resultado()
    {
        return View();
    }

    public IActionResult AboutUs()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
