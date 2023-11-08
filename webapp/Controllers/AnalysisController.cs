using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using school_project.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace school_project.Controllers;

public class AnalysisController: Controller
{
    [HttpGet]
    
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult DataAnalysis()
    {
        return View();
    }



}