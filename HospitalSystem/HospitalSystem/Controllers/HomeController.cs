using Microsoft.AspNetCore.Mvc;

namespace HospitalSystem.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
